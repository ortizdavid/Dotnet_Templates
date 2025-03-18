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

