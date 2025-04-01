db = connect("mongodb://127.0.0.1:27017/dotnet_template_simple_mongodb_api");

// Products indexes
db.products.createIndex({ "code": 1 }, { unique: true});

