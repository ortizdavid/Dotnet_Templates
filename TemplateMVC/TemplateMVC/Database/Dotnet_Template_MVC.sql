-- SWITCH TO THE DATABASE
USE Dotnet_Template_MVC;
GO

-- Roles TABLE
IF OBJECT_ID('Roles', 'U') IS NOT NULL
    DROP TABLE Roles;
GO
CREATE TABLE Roles (
    RoleId INT IDENTITY PRIMARY KEY,
    RoleName NVARCHAR(150) UNIQUE NOT NULL,
    Code NVARCHAR(30) UNIQUE NOT NULL,
    UniqueId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO
-- INSERT
INSERT INTO Roles(Code, RoleName) VALUES('role_super_admin', 'Super Administrator');
INSERT INTO Roles(Code, RoleName) VALUES('role_admin', 'Administrator');
GO


-- USERS TABLE
IF OBJECT_ID('Users', 'U') IS NOT NULL
    DROP TABLE Users;
GO
CREATE TABLE Users (
    UserId INT IDENTITY PRIMARY KEY,
    RoleId INT NOT NULL,
    UserName NVARCHAR(150) UNIQUE NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    Password NVARCHAR(250) NOT NULL,
    Image NVARCHAR(100),
    IsActive BIT NOT NULL DEFAULT 1,
    RecoveryToken NVARCHAR(200) DEFAULT CAST(NEWID() AS NVARCHAR(200)),
    UniqueId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Role FOREIGN KEY(RoleId) REFERENCES Roles(RoleId)
);
GO
-- INSERT
INSERT INTO Users(RoleId, UserName, Email, Password) VALUES(1, 'admin01', 'admin01@gmail.com', '$2a$11$SkXy0zV.0RV6ZSvZlblIBeqSRsBQSNGz3tWTEva24wJi/Tcav5CtS');
GO

-- CATEGORIES TABLE
IF OBJECT_ID('Categories', 'U') IS NOT NULL
    DROP TABLE Categories;
GO
CREATE TABLE Categories (
    CategoryId INT IDENTITY PRIMARY KEY,
    CategoryName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(150),
    UniqueId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- SUPPLIERS TABLE
IF OBJECT_ID('Suppliers', 'U') IS NOT NULL
    DROP TABLE Suppliers;
GO
CREATE TABLE Suppliers (
    SupplierId INT IDENTITY PRIMARY KEY,
    SupplierName NVARCHAR(100) NOT NULL,
    IdentificationNumber NVARCHAR(30) UNIQUE,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    PrimaryPhone NVARCHAR(20) UNIQUE NOT NULL,
    SecondaryPhone NVARCHAR(20),
    Address NVARCHAR(150),
    UniqueId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- PRODUCTS TABLE
IF OBJECT_ID('Products', 'U') IS NOT NULL
    DROP TABLE Products;
GO
CREATE TABLE Products (
    ProductId INT IDENTITY PRIMARY KEY,
    CategoryId INT NOT NULL,
    SupplierId INT NOT NULL,
    Code NVARCHAR(20) UNIQUE NOT NULL,
    ProductName NVARCHAR(150) NOT NULL,
    UnitPrice DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    Description NVARCHAR(100),
    UniqueId UNIQUEIDENTIFIER DEFAULT NEWID(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Categories FOREIGN KEY(CategoryId) REFERENCES Categories(CategoryId),
    CONSTRAINT FK_Suppliers FOREIGN KEY(SupplierId) REFERENCES Suppliers(SupplierId)
);
GO

-- IMAGES TABLE
IF OBJECT_ID('ProductImages', 'U') IS NOT NULL
    DROP TABLE ProductImages;
GO
CREATE TABLE ProductImages (
    ImageId INT IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL,
    FileName NVARCHAR(150),
    UploadDir NVARCHAR(150),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Products FOREIGN KEY(ProductId) REFERENCES Products(ProductId)
);
GO

-- VIEWS

-- ViewUserData
IF OBJECT_ID('ViewUserData', 'V') IS NOT NULL
    DROP VIEW  ViewUserData;
GO
CREATE VIEW ViewUserData AS
SELECT    
    Us.UserId,
    Us.UniqueId,
    Us.UserName,
    Us.Email,
    Us.Password,
    Us.Image,
    Us.IsActive,
    Us.RecoveryToken,
    Us.CreatedAt,
    Us.UpdatedAt,
    Ro.RoleId, 
    Ro.RoleName,
    Ro.Code AS RoleCode
FROM Users Us
LEFT JOIN Roles Ro ON Ro.RoleId = Us.RoleId;
GO

-- ViewProductData
IF OBJECT_ID('ViewProductData', 'V') IS NOT NULL
    DROP VIEW ViewProductData;
GO
CREATE VIEW ViewProductData AS
SELECT 
    Pr.ProductId, 
    Pr.UniqueId,
    Pr.ProductName, 
    Pr.Code,
    Pr.UnitPrice, 
    Pr.Description, 
    Pr.CreatedAt, 
    Pr.UpdatedAt,
    Ca.CategoryId,
    Ca.CategoryName,
    Su.SupplierId,
    Su.SupplierName
FROM Products Pr
LEFT JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId
LEFT JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId;
GO

-- Reports
-- ViewProductReportData
IF OBJECT_ID('ViewProductReportData', 'V') IS NOT NULL
    DROP VIEW ViewProductReportData;
GO
CREATE VIEW ViewProductReportData AS
SELECT 
    Pr.ProductId, 
    Pr.ProductName, 
    Pr.Code, 
    Pr.UnitPrice, 
    Pr.Description,
    Pr.CreatedAt,
    Ca.CategoryId,
    Ca.CategoryName,
    Su.SupplierId,
    Su.SupplierName
FROM Products Pr
JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId;
GO

-- ViewSupplierReportData
IF OBJECT_ID('ViewSupplierReportData', 'V') IS NOT NULL
    DROP VIEW ViewSupplierReportData;
GO
CREATE VIEW ViewSupplierReportData AS
SELECT
    Su.SupplierId,
    Su.SupplierName,
    Su.IdentificationNumber,
    Su.Email,
    Su.PrimaryPhone,
    Su.SecondaryPhone,
    Su.Address,
    Su.CreatedAt
FROM Suppliers Su;
GO

-- ViewCategoryReportData
IF OBJECT_ID('ViewCategoryReportData', 'V') IS NOT NULL
    DROP VIEW ViewCategoryReportData;
GO
CREATE VIEW ViewCategoryReportData AS
SELECT
    Ca.CategoryId,
    Ca.CategoryName,
    Ca.Description,
    Ca.CreatedAt
FROM Categories Ca;
GO


-- Statistics
-- ViewUserActiveInactives
IF OBJECT_ID('ViewUserActiveInactives', 'V') IS NOT NULL
    DROP VIEW ViewUserActiveInactives;
GO
CREATE VIEW ViewUserActiveInactives AS
SELECT 
    SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS ActiveUsers,
    SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS InactiveUsers
FROM Users;
GO

-- ViewUserPercentageActiveInactives
IF OBJECT_ID('ViewUserPercentageActiveInactives', 'V') IS NOT NULL
    DROP VIEW ViewUserPercentageActiveInactives;
GO
CREATE VIEW ViewUserPercentageActiveInactives AS
SELECT 
    (SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS ActivePercentage,
    (SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS InactivePercentage
FROM Users;
GO


-- ViewProductTotalPriceBySuppliers
IF OBJECT_ID('ViewProductTotalPriceBySuppliers', 'V') IS NOT NULL
    DROP VIEW ViewProductTotalPriceBySuppliers;
GO
CREATE VIEW ViewProductTotalPriceBySuppliers AS
SELECT 
    Su.SupplierName, 
    SUM(Pr.UnitPrice) AS TotalPrice
FROM Products Pr
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId
GROUP BY Su.SupplierName;
GO

-- ViewSupplierTopSuppliers
IF OBJECT_ID('ViewSupplierTopSuppliers', 'V') IS NOT NULL
    DROP VIEW ViewSupplierTopSuppliers;
GO
CREATE VIEW ViewSupplierTopSuppliers AS
SELECT 
    Su.SupplierName, 
    COUNT(Pr.ProductId) AS ProductCount
FROM Products Pr
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId
GROUP BY Su.SupplierName;
GO

-- ViewCategoryTopCategories
IF OBJECT_ID('ViewCategoryTopCategories', 'V') IS NOT NULL
    DROP VIEW ViewCategoryTopCategories;
GO
CREATE VIEW ViewCategoryTopCategories AS
SELECT 
    Ca.CategoryName, 
    COUNT(Pr.ProductId) AS ProductCount
FROM Products Pr
JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId
GROUP BY Ca.CategoryName;
GO

