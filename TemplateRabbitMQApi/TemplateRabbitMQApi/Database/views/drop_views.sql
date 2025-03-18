IF OBJECT_ID('ViewUserData', 'V') IS NOT NULL
    DROP VIEW  ViewUserData;
GO

IF OBJECT_ID('ViewProductData', 'V') IS NOT NULL
    DROP VIEW ViewProductData;
GO

IF OBJECT_ID('ViewProductReportData', 'V') IS NOT NULL
    DROP VIEW ViewProductReportData;
GO

IF OBJECT_ID('ViewSupplierReportData', 'V') IS NOT NULL
    DROP VIEW ViewSupplierReportData;
GO

IF OBJECT_ID('ViewCategoryReportData', 'V') IS NOT NULL
    DROP VIEW ViewCategoryReportData;
GO

IF OBJECT_ID('ViewUserActiveInactives', 'V') IS NOT NULL
    DROP VIEW ViewUserActiveInactives;
GO

IF OBJECT_ID('ViewUserPercentageActiveInactives', 'V') IS NOT NULL
    DROP VIEW ViewUserPercentageActiveInactives;
GO

IF OBJECT_ID('ViewProductTotalPriceBySuppliers', 'V') IS NOT NULL
    DROP VIEW ViewProductTotalPriceBySuppliers;
GO

IF OBJECT_ID('ViewSupplierTopSuppliers', 'V') IS NOT NULL
    DROP VIEW ViewSupplierTopSuppliers;
GO

IF OBJECT_ID('ViewCategoryTopCategories', 'V') IS NOT NULL
    DROP VIEW ViewCategoryTopCategories;
GO