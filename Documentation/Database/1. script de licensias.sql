USE [db_aa9e81_veriffica];
GO

-- ============================================
-- TABLA: CompanyLicense
-- ============================================
IF OBJECT_ID('[dbo].[CompanyLicense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyLicense];
GO

CREATE TABLE [dbo].[CompanyLicense](
    [CompanyLicenseID] INT IDENTITY(1,1) NOT NULL,
    [CompanyID] INT NOT NULL,
    [TotalLicenses] INT NOT NULL,
    [UsedLicenses] INT NOT NULL CONSTRAINT DF_CompanyLicense_UsedLicenses DEFAULT (0),
    [CreatedAt] DATETIME NOT NULL CONSTRAINT DF_CompanyLicense_CreatedAt DEFAULT (GETDATE()),
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [ModifiedAt] DATETIME NULL,
    [ModifiedBy] NVARCHAR(100) NULL,
    CONSTRAINT PK_CompanyLicense PRIMARY KEY CLUSTERED ([CompanyLicenseID] ASC)
);
GO

ALTER TABLE [dbo].[CompanyLicense] WITH CHECK 
ADD CONSTRAINT [FK_CompanyLicense_Company] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company]([CompanyID]);
GO

-- ============================================
-- TABLA: CompanyLicenseLog
-- ============================================
IF OBJECT_ID('[dbo].[CompanyLicenseLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyLicenseLog];
GO

CREATE TABLE [dbo].[CompanyLicenseLog](
    [CompanyLicenseLogID] INT IDENTITY(1,1) NOT NULL,
    [CompanyLicenseID] INT NOT NULL,
    [RelatedExamResultID] INT NULL,
    [ActionType] NVARCHAR(50) NOT NULL, -- Ej: 'ASSIGN', 'USE', 'RETURN'
    [Quantity] INT NOT NULL,
    [CreatedAt] DATETIME NOT NULL CONSTRAINT DF_CompanyLicenseLog_CreatedAt DEFAULT (GETDATE()),
    [CreatedBy] NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_CompanyLicenseLog PRIMARY KEY CLUSTERED ([CompanyLicenseLogID] ASC),
    CONSTRAINT FK_CompanyLicenseLog_CompanyLicense FOREIGN KEY ([CompanyLicenseID])
        REFERENCES [dbo].[CompanyLicense]([CompanyLicenseID])
);
GO
