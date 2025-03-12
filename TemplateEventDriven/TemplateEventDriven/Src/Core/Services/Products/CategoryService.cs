using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Events;
using TemplateEventDriven.Core.Models.Messaging;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Repositories.Products;
using TemplateEventDriven.Core.Services.Events;

namespace TemplateEventDriven.Core.Services.Products;

public class CategoryService
{
    private readonly CategoryRepository _repository;
    private readonly EventService<CategoryEvent> _eventService;
    private readonly IHttpContextAccessor _contextAccessor;

    public CategoryService(CategoryRepository repository, EventService<CategoryEvent> eventService, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _eventService = eventService;
        _contextAccessor = contextAccessor;
    }

    public async Task CreateCategory(CategoryRequest request)
    {
        if (request is null)
        {
            throw new BadRequestException("The category request cannot be null. Please provide valid input.");
        }
        if (await _repository.ExistsRecord("CategoryName", request.CategoryName))
        {
            throw new ConflictException($"Category '{request.CategoryName}' already exists");
        }
        var category = new Category
        {
            CategoryName = request.CategoryName,
            Description = request.Description
        };
        await _repository.CreateAsync(category);

        var categoryCreated = new
        {
            UniqueId = category.UniqueId,
            CategoryName = request.CategoryName,
            Description = request.Description,
            CreatedAt = category.CreatedAt
        };

        await _eventService.PublishCreatedEvent(
            category.CategoryId,
            Exchanges.CategoryExchange, 
            RoutingKeys.Category.Created, 
            EventActions.Category.Create,
            categoryCreated
        );
    }

    public async Task UpdateCategory(CategoryRequest request, Guid uniqueId)
    {
        if (request is null)
        {
            throw new BadRequestException("The category request cannot be null. Please provide valid input.");
        }
        var category = await _repository.GetByUniqueIdAsync(uniqueId);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }

        var categoryBefore = new
        {
            UniqueId = category.UniqueId,
            CategoryName = request.CategoryName,
            Description = request.Description,
            UpdatedAt = category.UpdatedAt
        };

        category.CategoryName = request.CategoryName;
        category.Description = request.Description;
        category.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(category);

        var categoryAfter = new
        {
            UniqueId = category.UniqueId,
            CategoryName = request.CategoryName,
            Description = request.Description,
            Uptadedt = category.UpdatedAt
        };

        await _eventService.PublishUpdatedEvent(
            category.CategoryId,
            Exchanges.CategoryExchange, 
            RoutingKeys.Category.Updated,
            EventActions.Category.Update,
            categoryBefore,
            categoryAfter
        );
    }

    public async Task<Pagination<Category>> GetAllCategories(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var categories = await _repository.GetAllAsync(param.PageSize, param.PageIndex);
        var pagination = new Pagination<Category>(categories, count, param.PageIndex, param.PageSize, _contextAccessor);  
        return pagination;
    }

    public async Task<Category> GetCategoryByUniqueId(Guid uniqueId)
    {
        var category = await _repository.GetByUniqueIdAsync(uniqueId);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }
        return category;
    }

    public async Task DeleteCategory(Guid uniqueId)
    {
        var category = await _repository.GetByUniqueIdAsync(uniqueId);
        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }

        var categoryDeleted = new
        {
            UniqueId = category.UniqueId,
            CategoryName = category.CategoryName,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };

        await _repository.DeleteAsync(category);

        await _eventService.PublishDeletedEvent(
            category.CategoryId,
            Exchanges.CategoryExchange, 
            RoutingKeys.Category.Deleted,
            EventActions.Category.Delete,
            categoryDeleted
        );
    }

    public async Task ImportCategoriesCSV(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new BadRequestException("No file selected.");
        }
        if (Path.GetExtension(formFile.FileName).ToLower() != ".csv")
        {
            throw new BadRequestException("Invalid file format. Please upload a CSV file.");
        }
        var categories = await ParseCSV(formFile);
        await _repository.CreateBatchAsync(categories);

        var categoryImported = new
        {
            TotalRecords = categories.Count(),
            Items = categories
        };

        await _eventService.PublishImportedEvent(
            Exchanges.CategoryExchange,
            RoutingKeys.Category.Imported,
            EventActions.Category.ImportCsv,
            categoryImported
        );
    }

    private async Task<IEnumerable<Category>> ParseCSV(IFormFile formFile)
    {
        var categories = new List<Category>();
        using (StreamReader reader = new StreamReader(formFile.OpenReadStream()))
        {
            // Skip the header line
            await reader.ReadLineAsync();
            string? line;
            int lineNumber = 0;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                var data = line.Split(',');
                var categoryName = data[0].Trim();
                var description = data[1].Trim();
                // verify number of fields
                if (data.Length != 2)
                {
                    throw new BadRequestException("Invalid CSV format. Each line must contain CategoryName, Description.");
                }
                if (await _repository.ExistsRecord("CategoryName", categoryName))
                {
                    throw new BadRequestException($"Error on Line: {lineNumber}. Category '{categoryName}' already exist");
                }
                categories.Add(new Category
                {
                    CategoryName = categoryName,
                    Description =  description,
                });
            }
        }
        return categories;
    }

}