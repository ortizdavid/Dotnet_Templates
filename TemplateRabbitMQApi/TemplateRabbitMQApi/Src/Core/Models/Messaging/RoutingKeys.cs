namespace TemplateRabbitMQApi.Core.Models.Messaging;

public static class RoutingKeys
{
    // User Keys
    public static class User
    {
        public const string Created = "user.created";
        public const string Updated = "user.updated";
        public const string Deleted = "user.deleted";
    }

    // Product Keys
    public static class Product
    {
        public const string Created = "product.created";
        public const string Imported = "product.imported";
        public const string Updated = "product.updated";
        public const string Deleted = "product.deleted";
        public const string ImageCreated = "product.image.created";
    }
   
    // Category Keys
    public static class Category
    {
        public const string Created = "category.created";
        public const string Imported = "category.imported";
        public const string Updated = "category.updated";
        public const string Deleted = "category.deleted";
    }

    // Supplier Keys
    public static class Supplier
    {
        public const string Created = "supplier.created";
        public const string Imported = "supplier.imported";
        public const string Updated = "supplier.updated";
        public const string Deleted = "supplier.deleted";
    }
}