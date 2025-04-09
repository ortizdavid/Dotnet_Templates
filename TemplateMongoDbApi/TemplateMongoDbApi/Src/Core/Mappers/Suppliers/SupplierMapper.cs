using TemplateMongoDbApi.Core.DTOs.Suppliers;
using TemplateMongoDbApi.Core.Models.Suppliers;

namespace TemplateMongoDbApi.Core.Mappers.Suppliers;

public static class SupplierMapper
{
    public static SupplierResponse ToResponse(Supplier? suppler)
    {
        return new SupplierResponse
        {
            SupplierId = suppler?.SupplierId.ToString(),
            SupplierName = suppler?.SupplierName,
            IdentificationNumber = suppler?.IdentificationNumber,
            Email = suppler?.Email,
            PrimaryPhone = suppler?.PrimaryPhone,
            SecondaryPhone = suppler?.SecondaryPhone,
            Address = suppler?.Address,
            CreatedAt = suppler?.CreatedAt,
            UpdatedAt = suppler?.UpdatedAt
        };
    }

    public static List<SupplierResponse> ToResponseList(IEnumerable<Supplier> suppliers)
    {
        return suppliers.Select(s => new SupplierResponse
        {
            SupplierId = s.SupplierId.ToString(),
            SupplierName = s.SupplierName,
            IdentificationNumber = s.IdentificationNumber,
            Email = s.Email,
            PrimaryPhone = s.PrimaryPhone,
            SecondaryPhone = s.SecondaryPhone,
            Address = s.Address,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }
}