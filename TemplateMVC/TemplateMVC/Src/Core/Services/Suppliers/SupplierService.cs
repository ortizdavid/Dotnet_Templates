using Microsoft.AspNetCore.Http;
using TemplateMVC.Common.Exceptions;
using TemplateMVC.Helpers;
using TemplateMVC.Core.Models.Products;
using TemplateMVC.Core.Models.Suppliers;
using TemplateMVC.Core.Repositories;
using TemplateMVC.Core.Repositories.Suppliers;
using TemplateMVC.Common.Helpers;

namespace TemplateMVC.Core.Services.Suppliers;

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

    public async Task CreateSupplier(CreateSupplierViewModel viewModel)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("The supplier viewModel cannot be null. Please provide Name, Identification, Contacts and Address");
        }
        if (await _repository.ExistsRecord("IdentificationNumber", viewModel.IdentificationNumber))
        {
            throw new ConflictException($"Supplier with identification number '{viewModel.IdentificationNumber}' already exists");
        }
        if (await _repository.ExistsRecord("PrimaryPhone", viewModel.PrimaryPhone))
        {
            throw new ConflictException($"Supplier with primary phone '{viewModel.PrimaryPhone}' already exists");
        }
        if (await _repository.ExistsRecord("SecondaryPhone", viewModel.SecondaryPhone))
        {
            throw new ConflictException($"Supplier with secondary phone '{viewModel.SecondaryPhone}' already exists");
        }
        if (await _repository.ExistsRecord("Email", viewModel.Email))
        {
            throw new ConflictException($"Supplier with email '{viewModel.Email}' already exists");
        }
        var supplier = new Supplier()
        {
            SupplierName = viewModel.SupplierName,
            IdentificationNumber = viewModel.IdentificationNumber,
            PrimaryPhone = viewModel.PrimaryPhone,
            SecondaryPhone = viewModel.SecondaryPhone,
            Email = viewModel.Email,
            Address = viewModel.Address
        };
        await _repository.CreateAsync(supplier);
    }

    public async Task UpdateSupplier(UpdateSupplierViewModel viewModel, Guid uniqueId)
    {
        if (viewModel is null)
        {
            throw new BadRequestException("The supplier viewModel cannot be null. Please provide Name, Identification, Contacts and Address");
        }
        var supplier = await _repository.GetByUniqueIdAsync(uniqueId);
        if (supplier is null)
        {
            throw new NotFoundException($"Supplier with ID '{uniqueId}' not found.");
        }
        if (await _repository.ExistsRecordExcluded(viewModel.IdentificationNumber, viewModel.Email, viewModel.PrimaryPhone, viewModel.SecondaryPhone, uniqueId))
        {
            throw new ConflictException("Identification or Email or Phones is already in use"); 
        }
        supplier.SupplierName = viewModel.SupplierName;
        supplier.IdentificationNumber = viewModel.IdentificationNumber;
        supplier.PrimaryPhone = viewModel.PrimaryPhone;
        supplier.SecondaryPhone = viewModel.SecondaryPhone;
        supplier.Email = viewModel.Email;
        supplier.Address = viewModel.Address;
        supplier.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(supplier);
    }

    public async Task<Pagination<Supplier>> GetAllSuppliers(PaginationParam param, SearchFilter filter)
    {
        if (param is null)
        {
            throw new BadRequestException("Please provide 'PageIndex' and 'PageSize'");
        }
        var count = await _repository.CountAsync();
        var suppliers = await _repository.GetAllStortedAsync(param.PageSize, param.PageIndex, filter.SearchString, filter.SortOrder);
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
        await _repository.DeleteAsync(supplier);
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