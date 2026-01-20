-- =============================================
-- Script: Crear tabla LicenseConfiguration
-- Descripción: Tabla de configuración para el sistema de sobregiro de licencias
-- Fecha: 2025-10-21
-- =============================================

USE [db_aa9e81_veriffica]
GO

-- Crear tabla LicenseConfiguration
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LicenseConfigurations]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LicenseConfigurations](
        [LicenseConfigurationID] [int] IDENTITY(1,1) NOT NULL,
        [MaxOverdraftLicenses] [int] NOT NULL DEFAULT 0,
        [IsOverdraftEnabled] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime2](7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_LicenseConfigurations] PRIMARY KEY CLUSTERED ([LicenseConfigurationID] ASC)
    ) ON [PRIMARY]

    PRINT 'Tabla LicenseConfigurations creada exitosamente'
END
ELSE
BEGIN
    PRINT 'La tabla LicenseConfigurations ya existe'
END
GO

-- Insertar configuración inicial (si no existe ninguna)
IF NOT EXISTS (SELECT 1 FROM [dbo].[LicenseConfigurations])
BEGIN
    INSERT INTO [dbo].[LicenseConfigurations]
        ([MaxOverdraftLicenses], [IsOverdraftEnabled], [CreatedDate], [ModifiedDate])
    VALUES
        (100, 1, GETDATE(), GETDATE())

    PRINT 'Configuración inicial insertada: MaxOverdraftLicenses=100, IsOverdraftEnabled=true'
END
ELSE
BEGIN
    PRINT 'Ya existe una configuración en LicenseConfigurations'
END
GO

-- Verificar creación
SELECT
    LicenseConfigurationID,
    MaxOverdraftLicenses,
    IsOverdraftEnabled,
    CreatedDate,
    ModifiedDate
FROM [dbo].[LicenseConfigurations]
GO

PRINT 'Script completado exitosamente'
GO
