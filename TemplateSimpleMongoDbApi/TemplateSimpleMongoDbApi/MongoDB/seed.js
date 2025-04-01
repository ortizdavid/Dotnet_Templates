db = connect("mongodb://127.0.0.1:27017/dotnet_template_simple_mongodb_api");

db.products.insertMany([
    { "name": "Product 01", "price": 10.99, "code": "P-001" },
    { "name": "Product 02", "price": 20.99, "code": "P-002" },
    { "name": "Product 03", "price": 30.99, "code": "P-003" },
    { "name": "Product 04", "price": 40.99, "code": "P-004" },
    { "name": "Product 05", "price": 50.99, "code": "P-005" },
    { "name": "Product 06", "price": 60.99, "code": "P-006" },
    { "name": "Product 07", "price": 70.99, "code": "P-007" },
    { "name": "Product 08", "price": 80.99, "code": "P-008" },
    { "name": "Product 09", "price": 90.99, "code": "P-009" },
    { "name": "Product 10", "price": 100.99, "code": "P-010" }
]);

print("Inserted 10 rows");