/*
╔══════════════════════════════════════════════════════════════════════════════╗
║  SCRIPT: ELIMINAR TABLAS NO UTILIZADAS DEL SISTEMA DE LICENCIAS             ║
║  Versión: 1.0                                                                 ║
║  Fecha: Enero 2025                                                            ║
║  Autor: Sistema Veriffica                                                     ║
║                                                                               ║
║  PROPOSITO:                                                                   ║
║  Este script elimina las tablas que NO se utilizan en el modelo simplificado ║
║  de gestión de licencias basado únicamente en CONTADORES.                    ║
║                                                                               ║
║  TABLAS A ELIMINAR:                                                           ║
║  1. License - Tracking individual de licencias (NO SE USA)                   ║
║  2. CompanyLicenseLog - Log específico de company (REEMPLAZADO por LicenseLog)║
║  3. LicenseConfiguration - Configuración de límites (NO SE USA)              ║
║                                                                               ║
║  MODELO FINAL:                                                                ║
║  Solo se usan tablas de CONTADORES:                                           ║
║  - BusinessGroupLicense (PurchasedLicenses, AssignedLicenses, UsedLicenses)  ║
║  - CompanyLicense (AllocatedLicenses, AssignedLicenses, UsedLicenses)        ║
║  - AreaLicense (AllocatedLicenses, AssignedLicenses, UsedLicenses)           ║
║  - CityLicense (AllocatedLicenses, AssignedLicenses, UsedLicenses)           ║
║  - LicenseLog (Auditoría unificada de TODAS las operaciones)                 ║
║                                                                               ║
║  ADVERTENCIA:                                                                 ║
║  - Este script ELIMINA DATOS de forma PERMANENTE                             ║
║  - Se recomienda hacer un BACKUP completo de la base de datos antes          ║
║  - Verificar que las tablas NO tengan dependencias críticas                  ║
║  - Ejecutar primero en ambiente de DESARROLLO/PRUEBAS                        ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

USE [db_aa9e81_veriffica];
GO

PRINT '═══════════════════════════════════════════════════════════';
PRINT 'INICIO: Limpieza de tablas no utilizadas';
PRINT 'Fecha: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '═══════════════════════════════════════════════════════════';
GO

-- ============================================================================
-- PASO 1: Verificar existencia de tablas
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 1: Verificando existencia de tablas               │';
PRINT '└─────────────────────────────────────────────────────────┘';

IF OBJECT_ID('dbo.License', 'U') IS NOT NULL
    PRINT '✓ Tabla License existe - Será eliminada';
ELSE
    PRINT '✗ Tabla License NO existe';

IF OBJECT_ID('dbo.CompanyLicenseLog', 'U') IS NOT NULL
    PRINT '✓ Tabla CompanyLicenseLog existe - Será eliminada';
ELSE
    PRINT '✗ Tabla CompanyLicenseLog NO existe';

IF OBJECT_ID('dbo.LicenseConfiguration', 'U') IS NOT NULL
    PRINT '✓ Tabla LicenseConfiguration existe - Será eliminada';
ELSE
    PRINT '✗ Tabla LicenseConfiguration NO existe';

GO

-- ============================================================================
-- PASO 2: Verificar dependencias (Foreign Keys)
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 2: Verificando Foreign Keys                       │';
PRINT '└─────────────────────────────────────────────────────────┘';

SELECT
    OBJECT_NAME(fk.parent_object_id) AS 'Tabla con FK',
    fk.name AS 'Nombre FK',
    OBJECT_NAME(fk.referenced_object_id) AS 'Tabla Referenciada'
FROM sys.foreign_keys fk
WHERE OBJECT_NAME(fk.referenced_object_id) IN ('License', 'CompanyLicenseLog', 'LicenseConfiguration')
   OR OBJECT_NAME(fk.parent_object_id) IN ('License', 'CompanyLicenseLog', 'LicenseConfiguration');

GO

-- ============================================================================
-- PASO 3: Eliminar Foreign Keys de la tabla License
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 3: Eliminando Foreign Keys                        │';
PRINT '└─────────────────────────────────────────────────────────┘';

-- Eliminar FKs que REFERENCIAN a License (desde otras tablas)
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql = @sql + 'ALTER TABLE ' + OBJECT_NAME(fk.parent_object_id)
    + ' DROP CONSTRAINT ' + fk.name + ';' + CHAR(13)
FROM sys.foreign_keys fk
WHERE OBJECT_NAME(fk.referenced_object_id) IN ('License', 'CompanyLicenseLog', 'LicenseConfiguration');

IF LEN(@sql) > 0
BEGIN
    PRINT 'Eliminando Foreign Keys que referencian a tablas no usadas...';
    EXEC sp_executesql @sql;
    PRINT '✓ Foreign Keys eliminadas exitosamente';
END
ELSE
    PRINT '✓ No hay Foreign Keys que eliminar';

GO

-- Eliminar FKs que License tiene hacia otras tablas
SET @sql = '';

SELECT @sql = @sql + 'ALTER TABLE ' + OBJECT_NAME(fk.parent_object_id)
    + ' DROP CONSTRAINT ' + fk.name + ';' + CHAR(13)
FROM sys.foreign_keys fk
WHERE OBJECT_NAME(fk.parent_object_id) IN ('License', 'CompanyLicenseLog', 'LicenseConfiguration');

IF LEN(@sql) > 0
BEGIN
    PRINT 'Eliminando Foreign Keys de tablas no usadas...';
    EXEC sp_executesql @sql;
    PRINT '✓ Foreign Keys de tablas no usadas eliminadas';
END
ELSE
    PRINT '✓ No hay Foreign Keys de tablas no usadas';

GO

-- ============================================================================
-- PASO 4: Verificar cantidad de registros (para backup)
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 4: Contando registros antes de eliminar           │';
PRINT '└─────────────────────────────────────────────────────────┘';

DECLARE @LicenseCount INT = 0;
DECLARE @CompanyLicenseLogCount INT = 0;
DECLARE @LicenseConfigCount INT = 0;

IF OBJECT_ID('dbo.License', 'U') IS NOT NULL
BEGIN
    SELECT @LicenseCount = COUNT(*) FROM License;
    PRINT '  License: ' + CAST(@LicenseCount AS VARCHAR) + ' registros';
END

IF OBJECT_ID('dbo.CompanyLicenseLog', 'U') IS NOT NULL
BEGIN
    SELECT @CompanyLicenseLogCount = COUNT(*) FROM CompanyLicenseLog;
    PRINT '  CompanyLicenseLog: ' + CAST(@CompanyLicenseLogCount AS VARCHAR) + ' registros';
END

IF OBJECT_ID('dbo.LicenseConfiguration', 'U') IS NOT NULL
BEGIN
    SELECT @LicenseConfigCount = COUNT(*) FROM LicenseConfiguration;
    PRINT '  LicenseConfiguration: ' + CAST(@LicenseConfigCount AS VARCHAR) + ' registros';
END

GO

-- ============================================================================
-- PASO 5: ELIMINAR TABLA License
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 5: Eliminando tabla License                       │';
PRINT '└─────────────────────────────────────────────────────────┘';

IF OBJECT_ID('dbo.License', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.License;
    PRINT '✓ Tabla License ELIMINADA exitosamente';
END
ELSE
    PRINT '⚠ Tabla License no existe - Nada que eliminar';

GO

-- ============================================================================
-- PASO 6: ELIMINAR TABLA CompanyLicenseLog
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 6: Eliminando tabla CompanyLicenseLog             │';
PRINT '└─────────────────────────────────────────────────────────┘';

IF OBJECT_ID('dbo.CompanyLicenseLog', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.CompanyLicenseLog;
    PRINT '✓ Tabla CompanyLicenseLog ELIMINADA exitosamente';
END
ELSE
    PRINT '⚠ Tabla CompanyLicenseLog no existe - Nada que eliminar';

GO

-- ============================================================================
-- PASO 7: ELIMINAR TABLA LicenseConfiguration
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 7: Eliminando tabla LicenseConfiguration          │';
PRINT '└─────────────────────────────────────────────────────────┘';

IF OBJECT_ID('dbo.LicenseConfiguration', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.LicenseConfiguration;
    PRINT '✓ Tabla LicenseConfiguration ELIMINADA exitosamente';
END
ELSE
    PRINT '⚠ Tabla LicenseConfiguration no existe - Nada que eliminar';

GO

-- ============================================================================
-- PASO 8: Verificar estructura final
-- ============================================================================
PRINT '';
PRINT '┌─────────────────────────────────────────────────────────┐';
PRINT '│ PASO 8: Verificando estructura final                   │';
PRINT '└─────────────────────────────────────────────────────────┘';

PRINT '';
PRINT 'TABLAS DE LICENCIAS RESTANTES (Modelo Simplificado):';
PRINT '-----------------------------------------------------';

SELECT
    TABLE_NAME,
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = t.TABLE_NAME) AS 'Columnas'
FROM INFORMATION_SCHEMA.TABLES t
WHERE TABLE_NAME LIKE '%License%'
ORDER BY TABLE_NAME;

GO

-- ============================================================================
-- RESUMEN FINAL
-- ============================================================================
PRINT '';
PRINT '═══════════════════════════════════════════════════════════';
PRINT 'LIMPIEZA COMPLETADA EXITOSAMENTE';
PRINT '═══════════════════════════════════════════════════════════';
PRINT '';
PRINT '✓ Tablas eliminadas:';
PRINT '   1. License (tracking individual - NO SE USA)';
PRINT '   2. CompanyLicenseLog (reemplazado por LicenseLog)';
PRINT '   3. LicenseConfiguration (configuración - NO SE USA)';
PRINT '';
PRINT '✓ Modelo final simplificado:';
PRINT '   - BusinessGroupLicense (contadores)';
PRINT '   - CompanyLicense (contadores)';
PRINT '   - AreaLicense (contadores)';
PRINT '   - CityLicense (contadores)';
PRINT '   - LicenseLog (auditoría unificada)';
PRINT '';
PRINT 'Fecha finalización: ' + CONVERT(VARCHAR, GETDATE(), 120);
PRINT '═══════════════════════════════════════════════════════════';
GO

/*
╔══════════════════════════════════════════════════════════════════════════════╗
║  NOTAS IMPORTANTES POST-EJECUCIÓN                                            ║
╚══════════════════════════════════════════════════════════════════════════════╝

1. CÓDIGO ACTUALIZADO:
   - ✓ LicenseManagementService.cs - Código de License eliminado
   - ✓ Program.cs - Repositorios no usados eliminados
   - ✓ ApplicationDbContext.cs - DbSet<License> eliminado
   - ✓ Entidades License, CompanyLicenseLog, LicenseConfiguration eliminadas
   - ✓ Interfaces y repositorios eliminados

2. FÓRMULA DE DISPONIBILIDAD (TODOS LOS NIVELES):
   AvailableLicenses = AllocatedLicenses - AssignedLicenses - UsedLicenses

3. FLUJO SIMPLIFICADO:
   BusinessGroup (COMPRA)
   ↓ Asigna
   Company (RECIBE)
   ↓ Asigna
   Area (RECIBE)
   ↓ Asigna (opcional)
   City (RECIBE)
   ↓ Consume
   ExamSubject → ExamResult (incrementa UsedLicenses en jerarquía)

4. AUDITORÍA:
   - Tabla LicenseLog registra TODAS las operaciones
   - Niveles: BusinessGroup, Company, Area, City
   - Tipos: Purchase, Assign, Use, Release
   - Incluye BalanceBefore y BalanceAfter

5. VALIDACIONES AUTOMÁTICAS:
   - No se puede asignar más de lo disponible
   - Contadores automáticos en cada nivel
   - Trazabilidad completa en LicenseLog

6. PRÓXIMOS PASOS:
   - ✓ Ejecutar este script en DESARROLLO
   - ✓ Probar funcionalidad de compra/asignación
   - ✓ Verificar sincronización de resultados
   - ✓ Validar auditoría en LicenseLog
   - Ejecutar en PRODUCCIÓN (con backup previo)

7. DOCUMENTACIÓN:
   - Actualizar GUIA_TECNICA_LICENCIAS.md
   - Eliminar referencias a códigos de licencia (LIC-BG1-000001)
   - Actualizar diagramas de flujo
   - Documentar modelo simplificado

*/
