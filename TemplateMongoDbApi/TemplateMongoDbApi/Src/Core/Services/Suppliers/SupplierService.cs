using TemplateMongoDbApi.Common.Exceptions;
using TemplateMongoDbApi.Common.Helpers;
using TemplateMongoDbApi.Core.DTOs.Suppliers;
using TemplateMongoDbApi.Core.Mappers.Suppliers;
using TemplateMongoDbApi.Core.Models.Products;
using TemplateMongoDbApi.Core.Models.Suppliers;
using TemplateMongoDbApi.Core.Repositories;
using TemplateMongoDbApi.Core.Repositories.Suppliers;

namespace TemplateMongoDbApi.Core.Services.Suppliers;

public class SupplierService
{
    private readonly SupplierRepository _repository;
    private readonly ProductRepository _productRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public SupplierService(SupplierRepository repository, ProductRepository productRepository, IHttpContextAccessor contextAccessor)
    {
        _repository = repository;
        _productRepository = productRepository;
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
    }

    public async Task UpdateSupplier(SupplierRequest request, string supplierId)
    {
        if (request is null)
        {
            throw new BadRequestException("The supplier request cannot be null. Please provide Name, Identification, Contacts and Address");
        }
        var supplier = await _repository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{supplierId}' not found.");
        }
        supplier.SupplierName = request.SupplierName;
        supplier.IdentificationNumber = request.IdentificationNumber;
        supplier.PrimaryPhone = request.PrimaryPhone;
        supplier.SecondaryPhone = request.SecondaryPhone;
        supplier.Email = request.Email;
        supplier.Address = request.Address;
        supplier.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(supplier, supplierId);
    }

    public async Task<Pagination<SupplierResponse>> GetAllSuppliers(PaginationParam param)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var suppliers = await _repository.GetAllAsync(param.PageSize, param.PageIndex);
        var supplierResponses = SupplierMapper.ToResponseList(suppliers);
        var pagination = new Pagination<SupplierResponse>(supplierResponses, count, param.PageIndex, param.PageSize, _contextAccessor);
        return pagination;
    }

    public async Task<SupplierResponse> GetSupplierById(string supplierId)
    {
        var supplier = await _repository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{supplierId}' not found.");
        }
        var supplierResponse = SupplierMapper.ToResponse(supplier);
        return supplierResponse;
    }

    public async Task DeleteSupplier(string supplierId)
    {
        var supplier = await _repository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{supplierId}' not found.");
        }
        await _repository.DeleteAsync(supplierId);
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

    public async Task<IEnumerable<Product>> GetSupplierProducts(string supplierId)
    {
        var supplier = await _repository.GetByIdAsync(supplierId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{supplierId}' not found.");
        }
        var products = await _productRepository.GetAllBySupplierAsync(supplierId);
        return products;
    }
}