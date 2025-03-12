namespace TemplateEventDriven.Core.Models.Events;

public static class EventActions
{
    public static class User
    {
        public const string Create = "Create User";
        public const string ChangePassword = "Change User Password";
        public const string UploadImage = "Upload User Image";
        public const string Activate = "Activate User";
        public const string Deactivate = "Deactivate User";
        public const string Delete = "Delete User";
    }

    public static class Product
    {
        public const string Create = "Create Product";
        public const string Update = "Update Product";
        public const string Delete = "Delete Product";
        public const string ImportCsv = "Import Product CSV";
        public const string UploadImages = "Upload Product Images";
    }

    public static class Category
    {
        public const string Create = "Create Category";
        public const string Update = "Update Category";
        public const string Delete = "Delete Category";
        public const string ImportCsv = "Import Category CSV";
    }

    public static class Supplier
    {
        public const string Create = "Create Supplier";
        public const string Update = "Update Supplier";
        public const string Delete = "Delete Supplier";
        public const string ImportCsv = "Import Supplier CSV";
    }
}