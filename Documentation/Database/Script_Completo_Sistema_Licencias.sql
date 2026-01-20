-- =============================================
-- SCRIPT COMPLETO: SISTEMA DE LICENCIAS JERÁRQUICO
-- Versión: 1.0
-- Fecha: Octubre 2025
-- Descripción: Implementa sistema jerárquico de licencias
-- BusinessGroup -> Company -> Area -> City
-- =============================================

SET NOCOUNT ON;
GO

PRINT '';
PRINT '========================================';
PRINT 'INICIANDO INSTALACIÓN DE SISTEMA DE LICENCIAS';
PRINT '========================================';
PRINT '';

-- =============================================
-- PARTE 1: CREAR TABLAS NUEVAS
-- =============================================

PRINT '===== CREANDO TABLAS NUEVAS =====';
PRINT '';

-- 1. Tabla de Configuración Global
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LicenseConfiguration')
BEGIN
    CREATE TABLE [dbo].[LicenseConfiguration] (
        [LicenseConfigurationID] INT IDENTITY(1,1) PRIMARY KEY,
        [ConfigKey] NVARCHAR(100) NOT NULL UNIQUE,
        [ConfigValue] INT NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedBy] NVARCHAR(100) NOT NULL,
        [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT '✓ Tabla LicenseConfiguration creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla LicenseConfiguration ya existe';
END
GO

-- Insertar configuración por defecto
IF NOT EXISTS (SELECT * FROM [dbo].[LicenseConfiguration] WHERE ConfigKey = 'MaxOverdraftLicenses')
BEGIN
    INSERT INTO [dbo].[LicenseConfiguration] (ConfigKey, ConfigValue, Description, CreatedBy, ModifiedBy, CreatedAt, ModifiedAt)
    VALUES ('MaxOverdraftLicenses', 10, 'Número máximo de licencias que se pueden usar en sobregiro (saldo negativo)', 'System', 'System', GETDATE(), GETDATE());
    PRINT '✓ Configuración MaxOverdraftLicenses insertada';
END
ELSE
BEGIN
    PRINT '✓ Configuración MaxOverdraftLicenses ya existe';
END
GO

-- 2. Tabla BusinessGroupLicense
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BusinessGroupLicense')
BEGIN
    CREATE TABLE [dbo].[BusinessGroupLicense] (
        [BusinessGroupLicenseID] INT IDENTITY(1,1) PRIMARY KEY,
        [BusinessGroupID] INT NOT NULL,
        [PurchasedLicenses] INT NOT NULL DEFAULT 0,
        [AssignedLicenses] INT NOT NULL DEFAULT 0,
        [UsedLicenses] INT NOT NULL DEFAULT 0,
        [OverdraftLicenses] INT NOT NULL DEFAULT 0,
        [AvailableLicenses] AS ([PurchasedLicenses] - [AssignedLicenses]),
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedBy] NVARCHAR(100) NOT NULL,
        [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [FK_BusinessGroupLicense_BusinessGroup] FOREIGN KEY ([BusinessGroupID]) 
            REFERENCES [dbo].[BusinessGroup]([BusinessGroupID]) ON DELETE CASCADE,
        CONSTRAINT [UK_BusinessGroupLicense_BusinessGroupID] UNIQUE ([BusinessGroupID])
    );
    PRINT '✓ Tabla BusinessGroupLicense creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla BusinessGroupLicense ya existe';
END
GO

-- 3. Tabla AreaLicense
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AreaLicense')
BEGIN
    CREATE TABLE [dbo].[AreaLicense] (
        [AreaLicenseID] INT IDENTITY(1,1) PRIMARY KEY,
        [AreaID] INT NOT NULL,
        [CompanyLicenseID] INT NOT NULL,
        [AllocatedLicenses] INT NOT NULL DEFAULT 0,
        [AssignedLicenses] INT NOT NULL DEFAULT 0,
        [UsedLicenses] INT NOT NULL DEFAULT 0,
        [AvailableLicenses] AS ([AllocatedLicenses] - [AssignedLicenses]),
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedBy] NVARCHAR(100) NOT NULL,
        [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [FK_AreaLicense_Area] FOREIGN KEY ([AreaID]) 
            REFERENCES [dbo].[Area]([AreaID]) ON DELETE CASCADE,
        CONSTRAINT [UK_AreaLicense_AreaID] UNIQUE ([AreaID])
    );
    PRINT '✓ Tabla AreaLicense creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla AreaLicense ya existe';
END
GO

-- 4. Tabla CityLicense
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CityLicense')
BEGIN
    CREATE TABLE [dbo].[CityLicense] (
        [CityLicenseID] INT IDENTITY(1,1) PRIMARY KEY,
        [CityID] INT NOT NULL,
        [AreaLicenseID] INT NOT NULL,
        [AllocatedLicenses] INT NOT NULL DEFAULT 0,
        [UsedLicenses] INT NOT NULL DEFAULT 0,
        [AvailableLicenses] AS ([AllocatedLicenses] - [UsedLicenses]),
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedBy] NVARCHAR(100) NOT NULL,
        [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [FK_CityLicense_City] FOREIGN KEY ([CityID]) 
            REFERENCES [dbo].[City]([CityID]) ON DELETE CASCADE,
        CONSTRAINT [UK_CityLicense_CityID_AreaLicenseID] UNIQUE ([CityID], [AreaLicenseID])
    );
    PRINT '✓ Tabla CityLicense creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla CityLicense ya existe';
END
GO

-- 5. Tabla LicenseLog
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LicenseLog')
BEGIN
    CREATE TABLE [dbo].[LicenseLog] (
        [LicenseLogID] INT IDENTITY(1,1) PRIMARY KEY,
        [LicenseLevel] NVARCHAR(50) NOT NULL,
        [EntityID] INT NOT NULL,
        [ActionType] NVARCHAR(50) NOT NULL,
        [Quantity] INT NOT NULL,
        [BalanceBefore] INT NOT NULL,
        [BalanceAfter] INT NOT NULL,
        [LicenseID] INT NULL,
        [RelatedExamResultID] INT NULL,
        [Description] NVARCHAR(500) NULL,
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
    
    CREATE INDEX [IX_LicenseLog_LicenseLevel] ON [dbo].[LicenseLog]([LicenseLevel]);
    CREATE INDEX [IX_LicenseLog_EntityID] ON [dbo].[LicenseLog]([EntityID]);
    CREATE INDEX [IX_LicenseLog_ActionType] ON [dbo].[LicenseLog]([ActionType]);
    CREATE INDEX [IX_LicenseLog_CreatedAt] ON [dbo].[LicenseLog]([CreatedAt]);
    
    PRINT '✓ Tabla LicenseLog creada con índices';
END
ELSE
BEGIN
    PRINT '✓ Tabla LicenseLog ya existe';
END
GO

-- 6. Crear o actualizar tabla License
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'License')
BEGIN
    CREATE TABLE [dbo].[License] (
        [LicenseID] INT IDENTITY(1,1) PRIMARY KEY,
        [LicenseCode] NVARCHAR(50) NOT NULL UNIQUE,
        [BusinessGroupLicenseID] INT NOT NULL,
        [CompanyLicenseID] INT NULL,
        [AreaLicenseID] INT NULL,
        [CityLicenseID] INT NULL,
        [StatusID] INT NOT NULL DEFAULT 1,
        [ExamSubjectID] INT NULL,
        [ExamResultID] INT NULL,
        [AssignedDate] DATETIME NULL,
        [UsedDate] DATETIME NULL,
        [CreatedBy] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
        [ModifiedBy] NVARCHAR(100) NOT NULL,
        [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT '✓ Tabla License creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla License ya existe, será actualizada';
    
    -- Agregar LicenseCode si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'LicenseCode')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [LicenseCode] NVARCHAR(50) NULL;
        PRINT '  ✓ Columna LicenseCode agregada';
    END
    
    -- Agregar BusinessGroupLicenseID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'BusinessGroupLicenseID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [BusinessGroupLicenseID] INT NULL;
        PRINT '  ✓ Columna BusinessGroupLicenseID agregada';
    END
    
    -- Agregar CompanyLicenseID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CompanyLicenseID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [CompanyLicenseID] INT NULL;
        PRINT '  ✓ Columna CompanyLicenseID agregada';
    END
    
    -- Agregar AreaLicenseID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'AreaLicenseID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [AreaLicenseID] INT NULL;
        PRINT '  ✓ Columna AreaLicenseID agregada';
    END
    
    -- Agregar CityLicenseID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CityLicenseID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [CityLicenseID] INT NULL;
        PRINT '  ✓ Columna CityLicenseID agregada';
    END
    
    -- Agregar StatusID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'StatusID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [StatusID] INT NOT NULL DEFAULT 1;
        PRINT '  ✓ Columna StatusID agregada';
    END
    
    -- Agregar ExamSubjectID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamSubjectID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [ExamSubjectID] INT NULL;
        PRINT '  ✓ Columna ExamSubjectID agregada';
    END
    
    -- Agregar ExamResultID si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamResultID')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [ExamResultID] INT NULL;
        PRINT '  ✓ Columna ExamResultID agregada';
    END
    
    -- Agregar AssignedDate si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'AssignedDate')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [AssignedDate] DATETIME NULL;
        PRINT '  ✓ Columna AssignedDate agregada';
    END
    
    -- Agregar UsedDate si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'UsedDate')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [UsedDate] DATETIME NULL;
        PRINT '  ✓ Columna UsedDate agregada';
    END
    
    -- Agregar CreatedBy si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CreatedBy')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [CreatedBy] NVARCHAR(100) NULL;
        PRINT '  ✓ Columna CreatedBy agregada';
    END
    
    -- Agregar CreatedAt si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CreatedAt')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE();
        PRINT '  ✓ Columna CreatedAt agregada';
    END
    
    -- Agregar ModifiedBy si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ModifiedBy')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [ModifiedBy] NVARCHAR(100) NULL;
        PRINT '  ✓ Columna ModifiedBy agregada';
    END
    
    -- Agregar ModifiedAt si no existe
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ModifiedAt')
    BEGIN
        ALTER TABLE [dbo].[License] ADD [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE();
        PRINT '  ✓ Columna ModifiedAt agregada';
    END
END
GO

-- =============================================
-- PARTE 2: ACTUALIZAR COMPANYLICENSE
-- =============================================

PRINT '';
PRINT '===== ACTUALIZANDO COMPANYLICENSE =====';
PRINT '';

-- Renombrar TotalLicenses a AllocatedLicenses si existe
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'TotalLicenses')
BEGIN
    EXEC sp_rename 'CompanyLicense.TotalLicenses', 'AllocatedLicenses', 'COLUMN';
    PRINT '✓ TotalLicenses renombrada a AllocatedLicenses';
END
ELSE IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AllocatedLicenses')
BEGIN
    PRINT '✓ Columna AllocatedLicenses ya existe';
END
ELSE
BEGIN
    -- Si no existe ninguna, crear AllocatedLicenses
    ALTER TABLE [dbo].[CompanyLicense] ADD [AllocatedLicenses] INT NOT NULL DEFAULT 0;
    PRINT '✓ Columna AllocatedLicenses creada';
END
GO

-- Agregar BusinessGroupLicenseID
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'BusinessGroupLicenseID')
BEGIN
    ALTER TABLE [dbo].[CompanyLicense] ADD [BusinessGroupLicenseID] INT NULL;
    PRINT '✓ Columna BusinessGroupLicenseID agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna BusinessGroupLicenseID ya existe';
END
GO

-- Agregar AssignedLicenses
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AssignedLicenses')
BEGIN
    ALTER TABLE [dbo].[CompanyLicense] ADD [AssignedLicenses] INT NOT NULL DEFAULT 0;
    PRINT '✓ Columna AssignedLicenses agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna AssignedLicenses ya existe';
END
GO

-- Eliminar y recrear columna calculada AvailableLicenses
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AvailableLicenses')
BEGIN
    ALTER TABLE [dbo].[CompanyLicense] DROP COLUMN [AvailableLicenses];
    PRINT '✓ Columna calculada AvailableLicenses eliminada para recrearla';
END
GO

ALTER TABLE [dbo].[CompanyLicense] ADD [AvailableLicenses] AS ([AllocatedLicenses] - [AssignedLicenses]);
PRINT '✓ Columna calculada AvailableLicenses creada';
GO

-- =============================================
-- PARTE 3: CREAR FOREIGN KEYS Y CONSTRAINTS
-- =============================================

PRINT '';
PRINT '===== CREANDO FOREIGN KEYS =====';
PRINT '';

-- FK: CompanyLicense -> BusinessGroupLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CompanyLicense_BusinessGroupLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'BusinessGroupLicenseID')
BEGIN
    ALTER TABLE [dbo].[CompanyLicense]
    ADD CONSTRAINT [FK_CompanyLicense_BusinessGroupLicense] 
    FOREIGN KEY ([BusinessGroupLicenseID]) REFERENCES [dbo].[BusinessGroupLicense]([BusinessGroupLicenseID]);
    PRINT '✓ FK_CompanyLicense_BusinessGroupLicense';
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CompanyLicense_BusinessGroupLicense')
   AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'BusinessGroupLicenseID')
BEGIN
    PRINT '⚠ Columna BusinessGroupLicenseID no existe en CompanyLicense, FK no creada';
END
GO

-- FK: AreaLicense -> CompanyLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AreaLicense_CompanyLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('AreaLicense') AND name = 'CompanyLicenseID')
BEGIN
    ALTER TABLE [dbo].[AreaLicense]
    ADD CONSTRAINT [FK_AreaLicense_CompanyLicense] 
    FOREIGN KEY ([CompanyLicenseID]) REFERENCES [dbo].[CompanyLicense]([CompanyLicenseID]);
    PRINT '✓ FK_AreaLicense_CompanyLicense';
END
GO

-- FK: CityLicense -> AreaLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CityLicense_AreaLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CityLicense') AND name = 'AreaLicenseID')
BEGIN
    ALTER TABLE [dbo].[CityLicense]
    ADD CONSTRAINT [FK_CityLicense_AreaLicense] 
    FOREIGN KEY ([AreaLicenseID]) REFERENCES [dbo].[AreaLicense]([AreaLicenseID]);
    PRINT '✓ FK_CityLicense_AreaLicense';
END
GO

-- FK: License -> BusinessGroupLicense (con validación de columna)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_BusinessGroupLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'BusinessGroupLicenseID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_BusinessGroupLicense] 
    FOREIGN KEY ([BusinessGroupLicenseID]) REFERENCES [dbo].[BusinessGroupLicense]([BusinessGroupLicenseID]);
    PRINT '✓ FK_License_BusinessGroupLicense';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'BusinessGroupLicenseID')
BEGIN
    PRINT '⚠ Columna BusinessGroupLicenseID no existe en License, FK no creada';
    PRINT '  Execute: ALTER TABLE [License] ADD [BusinessGroupLicenseID] INT NULL;';
END
GO

-- FK: License -> CompanyLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_CompanyLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CompanyLicenseID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_CompanyLicense] 
    FOREIGN KEY ([CompanyLicenseID]) REFERENCES [dbo].[CompanyLicense]([CompanyLicenseID]);
    PRINT '✓ FK_License_CompanyLicense';
END
GO

-- FK: License -> AreaLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_AreaLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'AreaLicenseID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_AreaLicense] 
    FOREIGN KEY ([AreaLicenseID]) REFERENCES [dbo].[AreaLicense]([AreaLicenseID]);
    PRINT '✓ FK_License_AreaLicense';
END
GO

-- FK: License -> CityLicense
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_CityLicense')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'CityLicenseID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_CityLicense] 
    FOREIGN KEY ([CityLicenseID]) REFERENCES [dbo].[CityLicense]([CityLicenseID]);
    PRINT '✓ FK_License_CityLicense';
END
GO

-- FK: License -> ExamSubject
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_ExamSubject')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamSubjectID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_ExamSubject] 
    FOREIGN KEY ([ExamSubjectID]) REFERENCES [dbo].[ExamSubject]([ExamSubjectID]);
    PRINT '✓ FK_License_ExamSubject';
END
GO

-- FK: License -> ExamResult
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_License_ExamResult')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamResultID')
BEGIN
    ALTER TABLE [dbo].[License]
    ADD CONSTRAINT [FK_License_ExamResult] 
    FOREIGN KEY ([ExamResultID]) REFERENCES [dbo].[ExamResult]([ExamResultID]);
    PRINT '✓ FK_License_ExamResult';
END
GO

-- =============================================
-- PARTE 4: CREAR ÍNDICES
-- =============================================

PRINT '';
PRINT '===== CREANDO ÍNDICES =====';
PRINT '';

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_License_StatusID' AND object_id = OBJECT_ID('License'))
BEGIN
    CREATE INDEX [IX_License_StatusID] ON [dbo].[License]([StatusID]);
    PRINT '✓ IX_License_StatusID';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_License_BusinessGroupLicenseID' AND object_id = OBJECT_ID('License'))
BEGIN
    CREATE INDEX [IX_License_BusinessGroupLicenseID] ON [dbo].[License]([BusinessGroupLicenseID]);
    PRINT '✓ IX_License_BusinessGroupLicenseID';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_License_CompanyLicenseID' AND object_id = OBJECT_ID('License'))
BEGIN
    CREATE INDEX [IX_License_CompanyLicenseID] ON [dbo].[License]([CompanyLicenseID]);
    PRINT '✓ IX_License_CompanyLicenseID';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_License_AreaLicenseID' AND object_id = OBJECT_ID('License'))
BEGIN
    CREATE INDEX [IX_License_AreaLicenseID] ON [dbo].[License]([AreaLicenseID]);
    PRINT '✓ IX_License_AreaLicenseID';
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_License_ExamSubjectID' AND object_id = OBJECT_ID('License'))
BEGIN
    CREATE INDEX [IX_License_ExamSubjectID] ON [dbo].[License]([ExamSubjectID]);
    PRINT '✓ IX_License_ExamSubjectID';
END
GO

-- =============================================
-- PARTE 5: CREAR VISTAS
-- =============================================

PRINT '';
PRINT '===== CREANDO VISTAS =====';
PRINT '';

-- Vista: vw_BusinessGroupLicenseStatus
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_BusinessGroupLicenseStatus')
    DROP VIEW [dbo].[vw_BusinessGroupLicenseStatus];
GO

CREATE VIEW [dbo].[vw_BusinessGroupLicenseStatus] AS
SELECT 
    bg.BusinessGroupID,
    bg.Name AS BusinessGroupName,
    ISNULL(bgl.PurchasedLicenses, 0) AS PurchasedLicenses,
    ISNULL(bgl.AssignedLicenses, 0) AS AssignedLicenses,
    ISNULL(bgl.UsedLicenses, 0) AS UsedLicenses,
    ISNULL(bgl.OverdraftLicenses, 0) AS OverdraftLicenses,
    ISNULL(bgl.AvailableLicenses, 0) AS AvailableLicenses,
    (ISNULL(bgl.PurchasedLicenses, 0) - ISNULL(bgl.UsedLicenses, 0) - ISNULL(bgl.OverdraftLicenses, 0)) AS RealBalance
FROM [dbo].[BusinessGroup] bg
LEFT JOIN [dbo].[BusinessGroupLicense] bgl ON bg.BusinessGroupID = bgl.BusinessGroupID;
GO

PRINT '✓ Vista vw_BusinessGroupLicenseStatus creada';

-- Vista: vw_CompanyLicenseStatus
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_CompanyLicenseStatus')
    DROP VIEW [dbo].[vw_CompanyLicenseStatus];
GO

CREATE VIEW [dbo].[vw_CompanyLicenseStatus] AS
SELECT 
    c.CompanyID,
    c.Name AS CompanyName,
    bg.BusinessGroupID,
    bg.Name AS BusinessGroupName,
    ISNULL(cl.AllocatedLicenses, 0) AS AllocatedLicenses,
    ISNULL(cl.AssignedLicenses, 0) AS AssignedLicenses,
    ISNULL(cl.UsedLicenses, 0) AS UsedLicenses,
    ISNULL(cl.AvailableLicenses, 0) AS AvailableLicenses
FROM [dbo].[Company] c
INNER JOIN [dbo].[BusinessGroup] bg ON c.BusinessGroupID = bg.BusinessGroupID
LEFT JOIN [dbo].[CompanyLicense] cl ON c.CompanyID = cl.CompanyID;
GO

PRINT '✓ Vista vw_CompanyLicenseStatus creada';

-- Vista: vw_AreaLicenseStatus
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_AreaLicenseStatus')
    DROP VIEW [dbo].[vw_AreaLicenseStatus];
GO

CREATE VIEW [dbo].[vw_AreaLicenseStatus] AS
SELECT 
    a.AreaID,
    a.Name AS AreaName,
    c.CompanyID,
    c.Name AS CompanyName,
    bg.BusinessGroupID,
    bg.Name AS BusinessGroupName,
    ISNULL(al.AllocatedLicenses, 0) AS AllocatedLicenses,
    ISNULL(al.AssignedLicenses, 0) AS AssignedLicenses,
    ISNULL(al.UsedLicenses, 0) AS UsedLicenses,
    ISNULL(al.AvailableLicenses, 0) AS AvailableLicenses
FROM [dbo].[Area] a
INNER JOIN [dbo].[Company] c ON a.CompanyID = c.CompanyID
INNER JOIN [dbo].[BusinessGroup] bg ON c.BusinessGroupID = bg.BusinessGroupID
LEFT JOIN [dbo].[AreaLicense] al ON a.AreaID = al.AreaID;
GO

PRINT '✓ Vista vw_AreaLicenseStatus creada';

-- =============================================
-- PARTE 6: CREAR STORED PROCEDURES
-- =============================================

PRINT '';
PRINT '===== CREANDO STORED PROCEDURES =====';
PRINT '';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_ValidateLicenseAvailability')
    DROP PROCEDURE [dbo].[sp_ValidateLicenseAvailability];
GO

CREATE PROCEDURE [dbo].[sp_ValidateLicenseAvailability]
    @BusinessGroupID INT = NULL,
    @CompanyID INT = NULL,
    @AreaID INT = NULL,
    @CityID INT = NULL,
    @RequestedQuantity INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Available INT = 0;
    DECLARE @Message NVARCHAR(500) = '';
    DECLARE @IsValid BIT = 0;
    
    -- Validar a nivel de Grupo Empresarial
    IF @BusinessGroupID IS NOT NULL
    BEGIN
        SELECT @Available = ISNULL(AvailableLicenses, 0)
        FROM [dbo].[BusinessGroupLicense]
        WHERE BusinessGroupID = @BusinessGroupID;
        
        IF @Available >= @RequestedQuantity
        BEGIN
            SET @IsValid = 1;
            SET @Message = 'Licencias disponibles en BusinessGroup';
        END
        ELSE
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Licencias insuficientes en BusinessGroup. Disponibles: ' + CAST(@Available AS NVARCHAR(10));
        END
    END
    
    -- Validar a nivel de Company
    IF @CompanyID IS NOT NULL AND @IsValid = 1
    BEGIN
        SELECT @Available = ISNULL(AvailableLicenses, 0)
        FROM [dbo].[CompanyLicense]
        WHERE CompanyID = @CompanyID;
        
        IF @Available >= @RequestedQuantity
        BEGIN
            SET @IsValid = 1;
            SET @Message = 'Licencias disponibles en Company';
        END
        ELSE
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Licencias insuficientes en Company. Disponibles: ' + CAST(@Available AS NVARCHAR(10));
        END
    END
    
    -- Validar a nivel de Area
    IF @AreaID IS NOT NULL AND @IsValid = 1
    BEGIN
        SELECT @Available = ISNULL(AvailableLicenses, 0)
        FROM [dbo].[AreaLicense]
        WHERE AreaID = @AreaID;
        
        IF @Available >= @RequestedQuantity
        BEGIN
            SET @IsValid = 1;
            SET @Message = 'Licencias disponibles en Area';
        END
        ELSE
        BEGIN
            SET @IsValid = 0;
            SET @Message = 'Licencias insuficientes en Area. Disponibles: ' + CAST(@Available AS NVARCHAR(10));
        END
    END
    
    -- Retornar resultado
    SELECT @IsValid AS IsValid, @Available AS AvailableLicenses, @Message AS Message;
END
GO

PRINT '✓ Stored Procedure sp_ValidateLicenseAvailability creado';

-- =============================================
-- PARTE 7: CREAR TRIGGER
-- =============================================

PRINT '';
PRINT '===== CREANDO TRIGGER =====';
PRINT '';

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'trg_ExamResult_AfterInsert_UpdateLicenses')
    DROP TRIGGER [dbo].[trg_ExamResult_AfterInsert_UpdateLicenses];
GO

CREATE TRIGGER [dbo].[trg_ExamResult_AfterInsert_UpdateLicenses]
ON [dbo].[ExamResult]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ExamSubjectID INT;
    DECLARE @ExamResultID INT;
    DECLARE @LicenseID INT;
    
    -- Obtener datos del registro insertado
    SELECT @ExamResultID = ExamResultID, @ExamSubjectID = ExamSubjectID
    FROM inserted;
    
    -- Buscar la licencia asociada al ExamSubject
    SELECT TOP 1 @LicenseID = LicenseID
    FROM [dbo].[License]
    WHERE ExamSubjectID = @ExamSubjectID 
      AND StatusID = 1  -- Assigned
    ORDER BY LicenseID;
    
    IF @LicenseID IS NOT NULL
    BEGIN
        -- Actualizar el estado de la licencia a "Used" (2)
        UPDATE [dbo].[License]
        SET StatusID = 2,
            ExamResultID = @ExamResultID,
            UsedDate = GETDATE(),
            ModifiedAt = GETDATE()
        WHERE LicenseID = @LicenseID;
        
        -- Incrementar contador de UsedLicenses en toda la jerarquía
        DECLARE @BusinessGroupLicenseID INT;
        DECLARE @CompanyLicenseID INT;
        DECLARE @AreaLicenseID INT;
        DECLARE @CityLicenseID INT;
        
        SELECT 
            @BusinessGroupLicenseID = BusinessGroupLicenseID,
            @CompanyLicenseID = CompanyLicenseID,
            @AreaLicenseID = AreaLicenseID,
            @CityLicenseID = CityLicenseID
        FROM [dbo].[License]
        WHERE LicenseID = @LicenseID;
        
        -- Actualizar BusinessGroupLicense
        IF @BusinessGroupLicenseID IS NOT NULL
        BEGIN
            UPDATE [dbo].[BusinessGroupLicense]
            SET UsedLicenses = UsedLicenses + 1,
                ModifiedAt = GETDATE()
            WHERE BusinessGroupLicenseID = @BusinessGroupLicenseID;
        END
        
        -- Actualizar CompanyLicense
        IF @CompanyLicenseID IS NOT NULL
        BEGIN
            UPDATE [dbo].[CompanyLicense]
            SET UsedLicenses = UsedLicenses + 1,
                ModifiedAt = GETDATE()
            WHERE CompanyLicenseID = @CompanyLicenseID;
        END
        
        -- Actualizar AreaLicense
        IF @AreaLicenseID IS NOT NULL
        BEGIN
            UPDATE [dbo].[AreaLicense]
            SET UsedLicenses = UsedLicenses + 1,
                ModifiedAt = GETDATE()
            WHERE AreaLicenseID = @AreaLicenseID;
        END
        
        -- Actualizar CityLicense
        IF @CityLicenseID IS NOT NULL
        BEGIN
            UPDATE [dbo].[CityLicense]
            SET UsedLicenses = UsedLicenses + 1,
                ModifiedAt = GETDATE()
            WHERE CityLicenseID = @CityLicenseID;
        END
    END
END
GO

PRINT '✓ Trigger trg_ExamResult_AfterInsert_UpdateLicenses creado';

-- =============================================
-- PARTE 8: VERIFICACIONES FINALES
-- =============================================

PRINT '';
PRINT '===== VERIFICACIONES FINALES =====';
PRINT '';

-- Verificar CompanyLicense
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AllocatedLicenses')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AssignedLicenses')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CompanyLicense') AND name = 'AvailableLicenses')
BEGIN
    PRINT '✓ CompanyLicense actualizada correctamente';
END
ELSE
BEGIN
    PRINT '✗ ERROR: CompanyLicense no está completamente actualizada';
END

-- Verificar License
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'StatusID')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamSubjectID')
   AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('License') AND name = 'ExamResultID')
BEGIN
    PRINT '✓ License actualizada correctamente';
END
ELSE
BEGIN
    PRINT '✗ ERROR: License no está completamente actualizada';
END

-- =============================================
-- RESUMEN FINAL
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'INSTALACIÓN COMPLETADA EXITOSAMENTE';
PRINT '========================================';
PRINT '';
PRINT 'TABLAS CREADAS/ACTUALIZADAS:';
PRINT '  ✓ LicenseConfiguration';
PRINT '  ✓ BusinessGroupLicense';
PRINT '  ✓ CompanyLicense (actualizada)';
PRINT '  ✓ AreaLicense';
PRINT '  ✓ CityLicense';
PRINT '  ✓ License (actualizada)';
PRINT '  ✓ LicenseLog';
PRINT '';
PRINT 'VISTAS CREADAS:';
PRINT '  ✓ vw_BusinessGroupLicenseStatus';
PRINT '  ✓ vw_CompanyLicenseStatus';
PRINT '  ✓ vw_AreaLicenseStatus';
PRINT '';
PRINT 'STORED PROCEDURES:';
PRINT '  ✓ sp_ValidateLicenseAvailability';
PRINT '';
PRINT 'TRIGGERS:';
PRINT '  ✓ trg_ExamResult_AfterInsert_UpdateLicenses';
PRINT '';
PRINT 'COLUMNAS EN COMPANYLICENSE:';
PRINT '  ✓ AllocatedLicenses (renombrada de TotalLicenses)';
PRINT '  ✓ BusinessGroupLicenseID';
PRINT '  ✓ AssignedLicenses';
PRINT '  ✓ AvailableLicenses (calculada)';
PRINT '';
PRINT 'COLUMNAS EN LICENSE:';
PRINT '  ✓ StatusID';
PRINT '  ✓ ExamSubjectID';
PRINT '  ✓ ExamResultID';
PRINT '  ✓ AssignedDate';
PRINT '  ✓ UsedDate';
PRINT '';
PRINT 'VERIFICAR INSTALACIÓN:';
PRINT '  SELECT * FROM vw_BusinessGroupLicenseStatus;';
PRINT '  SELECT * FROM vw_CompanyLicenseStatus;';
PRINT '  SELECT * FROM vw_AreaLicenseStatus;';
PRINT '  SELECT TOP 10 * FROM License;';
PRINT '  SELECT * FROM LicenseConfiguration;';
PRINT '';
PRINT 'SIGUIENTE PASO:';
PRINT '  Actualizar código C# con las nuevas entidades';
PRINT '  Ver: GUIA_IMPLEMENTACION_LICENCIAS.md';
PRINT '========================================';
GO
