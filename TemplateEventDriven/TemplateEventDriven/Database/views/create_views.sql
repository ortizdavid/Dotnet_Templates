-- ViewUserData
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
    Rt.RefreshId,
    Rt.Token AS RefreshToken,
    Rt.ExpiryDate AS RefreshTokenExpiryDate,
    Ro.RoleId, 
    Ro.RoleName,
    Ro.Code AS RoleCode
FROM Users Us
LEFT JOIN (
    -- Get the latest refresh token per user
    SELECT 
        UserId, 
        MAX(RefreshId) AS LatestRefreshId
    FROM UserRefreshTokens
    GROUP BY UserId
) AS LatestRt ON LatestRt.UserId = Us.UserId
LEFT JOIN UserRefreshTokens Rt ON Rt.RefreshId = LatestRt.LatestRefreshId
LEFT JOIN Roles Ro ON Ro.RoleId = Us.RoleId;
GO

-- ViewProductData
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
JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId;
GO

-- Reports
-- ViewProductReportData
CREATE VIEW ViewProductReportData AS
SELECT 
    Pr.ProductId, 
    Pr.ProductName, 
    Pr.Code, 
    Pr.UnitPrice, 
    Ca.CategoryName,
    Pr.Description,
    Pr.CreatedAt
FROM Products Pr
JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId;
GO

-- ViewSupplierReportData
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
CREATE VIEW ViewUserActiveInactives AS
SELECT 
    SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) AS ActiveUsers,
    SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) AS InactiveUsers
FROM Users;
GO

-- ViewUserPercentageActiveInactives
CREATE VIEW ViewUserPercentageActiveInactives AS
SELECT 
    (SUM(CASE WHEN IsActive = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS ActivePercentage,
    (SUM(CASE WHEN IsActive = 0 THEN 1 ELSE 0 END) * 100.0 / COUNT(*)) AS InactivePercentage
FROM Users;
GO

-- ViewProductTotalPriceBySuppliers
CREATE VIEW ViewProductTotalPriceBySuppliers AS
SELECT 
    Su.SupplierName, 
    SUM(Pr.UnitPrice) AS TotalPrice
FROM Products Pr
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId
GROUP BY Su.SupplierName;
GO

-- ViewSupplierTopSuppliers
CREATE VIEW ViewSupplierTopSuppliers AS
SELECT 
    Su.SupplierName, 
    COUNT(Pr.ProductId) AS ProductCount
FROM Products Pr
JOIN Suppliers Su ON Su.SupplierId = Pr.SupplierId
GROUP BY Su.SupplierName;
GO

-- ViewCategoryTopCategories
CREATE VIEW ViewCategoryTopCategories AS
SELECT 
    Ca.CategoryName, 
    COUNT(Pr.ProductId) AS ProductCount
FROM Products Pr
JOIN Categories Ca ON Ca.CategoryId = Pr.CategoryId
GROUP BY Ca.CategoryName;
GO

