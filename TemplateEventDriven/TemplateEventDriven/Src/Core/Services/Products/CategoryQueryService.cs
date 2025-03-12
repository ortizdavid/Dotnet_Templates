using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Repositories.Products;

namespace TemplateEventDriven.Core.Services;

public class CategoryQueryService
{
    private readonly CategoryRepository _repository;
    private readonly IHttpContextAccessor _contextAccessor;

    public CategoryQueryService(CategoryRepository repository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _contextAccessor = contextAccessor;
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
}