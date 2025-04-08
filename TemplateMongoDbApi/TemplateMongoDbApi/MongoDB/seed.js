db = connect("mongodb://127.0.0.1/dotnet_template_mongodb_api");

const roleSuperAdminId = ObjectId("605c72ef1532070a12c618c9");
const roleAdminId = ObjectId("615c72ef1532670a12d618c3");

// Seed Roles
db.roles.insertMany([
    { 
        "_id": roleSuperAdminId, 
        "code": "role_super_admin", 
        "role_name": "Super Administrator",
        "created_at": new Date() 
    },
    { 
        "_id": roleAdminId, 
        "code": "role_super_admin", 
        "role_name": "Administrator",
        "created_at": new Date() 
    }
]);

// Seed Users
db.users.insertMany([
    { 
        "role": { 
            "_id": roleSuperAdminId, 
            "code": "role_super_admin", 
            "role_name": "Super Administrator",
            "created_at": new Date() 
        },
        "user_name": "admin01",
        "email": "admin01@gmail.com",
        "password": "$2a$11$SkXy0zV.0RV6ZSvZlblIBeqSRsBQSNGz3tWTEva24wJi/Tcav5CtS", // hashed password
        "created_at": new Date(),
        "updated_at": new Date(),
        "is_active": true
    },
    { 
        "role": { 
            "_id": roleAdminId, 
            "code": "role_admin", 
            "role_name": "Administrator",
            "created_at": new Date() 
        },
        "user_name": "admin02",
        "email": "admin02@gmail.com",
        "password": "$2a$11$7fCv6ZbRgSTIx1/3r.zRSebTqf.z4ZnGBnD6DPKr6PpVUpRJ8C2l6", // hashed password
        "created_at": new Date(),
        "updated_at": new Date(),
        "is_active": true
    }
]);