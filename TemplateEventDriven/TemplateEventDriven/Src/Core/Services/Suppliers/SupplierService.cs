using TemplateEventDriven.Common.Exceptions;
using TemplateEventDriven.Common.Helpers;
using TemplateEventDriven.Core.Models.Events;
using TemplateEventDriven.Core.Models.Messaging;
using TemplateEventDriven.Core.Models.Products;
using TemplateEventDriven.Core.Models.Suppliers;
using TemplateEventDriven.Core.Repositories;
using TemplateEventDriven.Core.Repositories.Suppliers;
using TemplateEventDriven.Core.Services.Events;

namespace TemplateEventDriven.Core.Services.Suppliers;

public class SupplierService
{
    private readonly SupplierRepository _repository;
    private readonly ProductRepository _productRepository;
    private readonly EventService<SupplierEvent> _eventService;
    private readonly IHttpContextAccessor _contextAccessor;

    public SupplierService(SupplierRepository repository, ProductRepository productRepository, 
        EventService<SupplierEvent> eventService, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _productRepository = productRepository;
        _eventService = eventService;
        _contextAccessor = contextAccessor;
    }

    public async Task CreateSupplier(SupplierRequest request)
    {
        if (request is null)
        {
            throw new BadRequestException("The supplier request cannot be null. Please provide Name, Identification, Contacts and Address");
        }
        if (await _repository.ExistsRecord("IdentificationNumber", request.IdentificationNumber))
        {
            throw new ConflictException($"Supplier with identification number '{request.IdentificationNumber}' already exists");
        }
        if (await _repository.ExistsRecord("PrimaryPhone", request.PrimaryPhone))
        {
            throw new ConflictException($"Supplier with primary phone '{request.PrimaryPhone}' already exists");
        }
        if (await _repository.ExistsRecord("SecondaryPhone", request.SecondaryPhone))
        {
            throw new ConflictException($"Supplier with secondary phone '{request.SecondaryPhone}' already exists");
        }
        if (await _repository.ExistsRecord("Email", request.Email))
        {
            throw new ConflictException($"Supplier with email '{request.Email}' already exists");
        }
        var supplier = new Supplier()
        {
            SupplierName = request.SupplierName,
            IdentificationNumber = request.IdentificationNumber,
            PrimaryPhone = request.PrimaryPhone,
            SecondaryPhone = request.SecondaryPhone,
            Email = request.Email,
            Address = request.Address
        };
        await _repository.CreateAsync(supplier);

        var newSupplier = new
        {
            SupplierName = request.SupplierName,
            IdentificationNumber = request.IdentificationNumber,
            PrimaryPhone = request.PrimaryPhone,
            SecondaryPhone = request.SecondaryPhone,
            Email = request.Email,
            Address = request.Address,
            CreatedAt = supplier.CreatedAt
        };

        await _eventService.PublishCreatedEvent(~
            supplier.SupplierId,
            Exchanges.SupplierExchange, 
            RoutingKeys.Supplier.Created,
            EventActions.Supplier.Create,
            newSupplier
        );
    }

    public async Task UpdateSupplier(SupplierRequest request, Guid uniqueId)
    {
        if (request is null)
        {
            throw new BadRequestException("The supplier request cannot be null. Please provide Name, Identification, Contacts and Address");
        }
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }

        var supplierAfter = new
        {
            SupplierName = request.SupplierName,
            IdentificationNumber = request.IdentificationNumber,
            PrimaryPhone = request.PrimaryPhone,
            SecondaryPhone = request.SecondaryPhone,
            Email = request.Email,
            Address = request.Address,
            UpdatedAt = supplier.UpdatedAt
        };

        supplier.SupplierName = request.SupplierName;
        supplier.IdentificationNumber = request.IdentificationNumber;
        supplier.PrimaryPhone = request.PrimaryPhone;
        supplier.SecondaryPhone = request.SecondaryPhone;
        supplier.Email = request.Email;
        supplier.Address = request.Address;
        supplier.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(supplier);

        var supplierBefore = new
        {
            SupplierName = supplier.SupplierName,
            IdentificationNumber = supplier.IdentificationNumber,
            PrimaryPhone = supplier.PrimaryPhone,
            SecondaryPhone = supplier.SecondaryPhone,
            Email = supplier.Email,
            Address = supplier.Address,
            UpdatedAt = supplier.UpdatedAt
        };

        await _eventService.PublishUpdatedEvent(
            supplier.SupplierId,
            Exchanges.SupplierExchange, 
            RoutingKeys.Supplier.Updated,
            EventActions.Supplier.Update,
            supplierBefore,
            supplierAfter
        );
    }

    public async Task<Pagination<Supplier>> GetAllSuppliers(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var suppliers = await _repository.GetAllAsync(param.PageSize, param.PageIndex);
        var pagination = new Pagination<Supplier>(suppliers, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<Supplier> GetSupplierByUniqueId(Guid uniqueId)
    {
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        return supplier;
    }

    public async Task DeleteSupplier(Guid uniqueId)
    {
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        var supplierDeleted = new
        {
            SupplierName = supplier.SupplierName,
            IdentificationNumber = supplier.IdentificationNumber,
            PrimaryPhone = supplier.PrimaryPhone,
            SecondaryPhone = supplier.SecondaryPhone,
            Email = supplier.Email,
            Address = supplier.Address,
            CreatedAt = supplier.CreatedAt
        };

        await _repository.DeleteAsync(supplier);

        await _eventService.PublishDeletedEvent(
            supplier.SupplierId,
            Exchanges.SupplierExchange, 
            RoutingKeys.Supplier.Deleted,
            EventActions.Supplier.Delete,
            supplierDeleted
        );
    }

    public async Task ImportSuppliersCSV(IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new BadRequestException("No file selected.");
        }
        if (Path.GetExtension(formFile.FileName).ToLower() != ".csv")
        {
            throw new BadRequestException("Invalid file format. Please upload a CSV file.");
        }
        var suppliers = await ParseCSV(formFile);
        await _repository.CreateBatchAsync(suppliers);

        var supplierImported = suppliers;

        await _eventService.PublishFaliedImportEvent(
            Exchanges.SupplierExchange, 
            RoutingKeys.Supplier.Imported,
            EventActions.Supplier.ImportCsv,
            supplierImported
        );
    }

    private async Task<IEnumerable<Supplier>> ParseCSV(IFormFile file)
    {
        var suppliers = new List<Supplier>();
        using (StreamReader reader = new StreamReader(file.OpenReadStream()))
        {
            // Skip the header line
            await reader.ReadLineAsync();
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var data = line.Split(',');
                var supplierName = data[0];
                var identification = data[1];
                var email = data[2];
                var primaryPhone = data[3];
                var secondaryPhone = data[4];
                var address = data[5];
                // verify number of fields
                if (data.Length != 6)
                {
                    throw new BadRequestException("Invalid CSV format. Each line must contain SupplierName,IdentificationNumber,Email,PrimaryPhone,SecondaryPhone,Address.");
                }
                //verify if exists
                if (await _repository.ExistsRecord("IdentificationNumber", identification))
                {
                    throw new BadRequestException($"Supplier Identification Number '{identification}' already exists.");
                }
                if (await _repository.ExistsRecord("PrimaryPhone", primaryPhone))
                {
                    throw new BadRequestException($"Supplier Primary Phone '{primaryPhone}' already exists.");
                }
                if (await _repository.ExistsRecord("SecondaryPhone", secondaryPhone))
                {
                    throw new BadRequestException($"Supplier Secondary Phone '{secondaryPhone}' already exists.");
                }
                if (await _repository.ExistsRecord("Email", email))
                {
                    throw new BadRequestException($"Supplier Email '{email}' already exists.");
                }
                var supplier = new Supplier
                {
                    SupplierName = supplierName,
                    IdentificationNumber = identification,
                    Email = email,
                    PrimaryPhone = primaryPhone,
                    SecondaryPhone = secondaryPhone,
                    Address = address
                };
                suppliers.Add(supplier);
            }
        }
        return suppliers;
    }

    public async Task<IEnumerable<Product>> GetSupplierProducts(Guid uniqueId)
    {
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        var products = await _productRepository.GetAllBySupplierAsync(supplier.SupplierId);
        return products;
    }
}