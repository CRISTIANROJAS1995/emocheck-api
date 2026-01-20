-- =======================================================================
-- Script: Agregar columna AssignedLicenses a CityLicense
-- Fecha: 2025-01-21
-- Descripción: Agrega la columna AssignedLicenses a la tabla CityLicense
--              para mantener consistencia con BusinessGroupLicense,
--              CompanyLicense y AreaLicense
-- =======================================================================

USE [db_aa9e81_veriffica]; -- Cambiar por el nombre de tu base de datos
GO

-- Verificar si la columna ya existe
IF NOT EXISTS (
    SELECT *
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'CityLicense'
    AND COLUMN_NAME = 'AssignedLicenses'
)
BEGIN
    PRINT 'Agregando columna AssignedLicenses a CityLicense...';

    -- Agregar la columna con valor por defecto 0
    ALTER TABLE CityLicense
    ADD AssignedLicenses INT NOT NULL DEFAULT 0;

    PRINT 'Columna AssignedLicenses agregada exitosamente.';

    -- Actualizar el cálculo de AvailableLicenses si es necesario
    -- Nota: AvailableLicenses es una columna calculada en Entity Framework,
    -- por lo que no se modifica directamente en la base de datos

    PRINT 'Script completado exitosamente.';
END
ELSE
BEGIN
    PRINT 'La columna AssignedLicenses ya existe en CityLicense.';
END
GO

-- Verificar el resultado
SELECT
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'CityLicense'
ORDER BY ORDINAL_POSITION;
GO

-- Mostrar datos actuales de CityLicense (si existen)
IF EXISTS (SELECT 1 FROM CityLicense)
BEGIN
    SELECT
        CityLicenseID,
        CityID,
        AreaLicenseID,
        AllocatedLicenses,
        AssignedLicenses,
        UsedLicenses,
        (AllocatedLicenses - AssignedLicenses - UsedLicenses) AS AvailableLicenses,
        CreatedBy,
        CreatedAt,
        ModifiedBy,
        ModifiedAt
    FROM CityLicense;
END
ELSE
BEGIN
    PRINT 'No hay registros en CityLicense.';
END
GO

PRINT '=======================================================================';
PRINT 'IMPORTANTE: Ahora CityLicense tiene la misma estructura que las demás:';
PRINT '- AllocatedLicenses: Licencias asignadas desde el área';
PRINT '- AssignedLicenses: Licencias asignadas a niveles inferiores (futuro)';
PRINT '- UsedLicenses: Licencias consumidas';
PRINT '- AvailableLicenses: AllocatedLicenses - AssignedLicenses - UsedLicenses';
PRINT '=======================================================================';
GO
