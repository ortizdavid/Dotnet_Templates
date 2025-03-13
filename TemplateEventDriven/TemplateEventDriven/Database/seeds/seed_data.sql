-- Roles
INSERT INTO Roles(Code, RoleName, CreatedAt, UpdatedAt, UniqueId) 
VALUES 
    ('role_super_admin', 'Super Administrator', GETDATE(), GETDATE(), NEWID()),
    ('role_admin', 'Administrator', GETDATE(), GETDATE(), NEWID());
GO

-- Users
INSERT INTO Users (RoleId, UserName, Email, Password, CreatedAt, UpdatedAt, UniqueId, IsActive) 
VALUES 
    (1, 'admin01', 'admin01@gmail.com', '$2a$11$SkXy0zV.0RV6ZSvZlblIBeqSRsBQSNGz3tWTEva24wJi/Tcav5CtS', GETDATE(), GETDATE(), NEWID(), 1),
    (2, 'admin02', 'admin02@gmail.com', '$2a$11$7fCv6ZbRgSTIx1/3r.zRSebTqf.z4ZnGBnD6DPKr6PpVUpRJ8C2l6', GETDATE(), GETDATE(), NEWID(), 1);
GO


-- EventTypes
INSERT INTO EventTypes (Name, Description) VALUES
('Created', 'When a new record is created'),
('Updated', 'When a record is updated'),
('Deleted', 'When a record is deleted'),
('Imported', 'When data is imported (CSV, JSON, Excel)'),
('Processed', 'When an imported file is processed'),
('FailedProcessing', 'When a general process fails due to business rules or errors'),
('FailedImport', 'When an import fails due to validation errors'),
('Exported', 'When data is exported'),
('Published', 'When an event/message is sent to another service'),
('Consumed', 'When an event/message is processed by a consumer');
GO
