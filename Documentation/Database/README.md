# 🗄️ EmoCheck Database - SQL Server

## 📋 Descripción General

Base de datos SQL Server para el proyecto **EmoCheck**, diseñada con arquitectura normalizada, esquemas organizados y siguiendo las mejores prácticas de seguridad y auditoría.

---

## 🏗️ Arquitectura de Esquemas

La base de datos está organizada en **6 esquemas principales**:

### 1. **dbo** (Default Schema)
Tablas maestras del sistema:
- `State` - Estados generales del sistema
- `Country` - Países
- `City` - Ciudades

### 2. **configuration** (Configuración)
Configuración organizacional:
- `Company` - Empresas/Organizaciones
- `Site` - Sedes
- `Area` - Áreas/Departamentos
- `JobType` - Tipos de cargo
- `Application` - Aplicaciones del sistema

### 3. **security** (Seguridad)
Usuarios y autenticación:
- `User` - Usuarios del sistema
- `Role` - Roles
- `UserRole` - Relación usuarios-roles
- `RefreshToken` - Tokens JWT
- `PasswordResetToken` - Tokens de recuperación
- `InformedConsent` - Consentimientos informados

### 4. **assessment** (Evaluaciones)
Módulos de evaluación:
- `AssessmentModule` - Módulos de evaluación
- `Question` - Preguntas
- `QuestionOption` - Opciones de respuesta
- `Evaluation` - Evaluaciones realizadas
- `EvaluationResponse` - Respuestas

### 5. **results** (Resultados)
Resultados y alertas:
- `EvaluationResult` - Resultados de evaluaciones
- `DimensionScore` - Puntajes por dimensión
- `Recommendation` - Recomendaciones personalizadas
- `RecommendationType` - Tipos de recomendación
- `Alert` - Alertas críticas
- `CaseTracking` - Seguimiento de casos

### 6. **resources** (Recursos)
Recursos de bienestar:
- `ResourceCategory` - Categorías de recursos
- `WellnessResource` - Recursos de bienestar
- `UserResourceAccess` - Acceso a recursos
- `ProfessionalSupport` - Apoyo profesional
- `SupportRequest` - Solicitudes de apoyo

### 7. **audit** (Auditoría)
Trazabilidad y logs:
- `AuditLog` - Registro de auditoría
- `SystemLog` - Logs del sistema
- `EmailLog` - Emails enviados
- `DataExport` - Exportaciones de datos

---

## 📊 Total de Tablas: 35+

---

## 🚀 Instalación y Ejecución

### **Requisitos**
- SQL Server 2019 o superior
- SQL Server Management Studio (SSMS) o Azure Data Studio

### **Orden de Ejecución de Scripts**

Ejecutar los scripts en el siguiente orden:

```sql
-- 1. Crear la base de datos (manualmente)
CREATE DATABASE [EmoCheckDB]
GO

-- 2. Ejecutar scripts en orden
01_CREATE_SCHEMAS.sql          -- Crear esquemas
02_CREATE_MASTER_TABLES.sql    -- Tablas maestras (dbo)
03_CREATE_CONFIGURATION_TABLES.sql  -- Tablas de configuración
04_CREATE_SECURITY_TABLES.sql  -- Tablas de seguridad
05_CREATE_ASSESSMENT_TABLES.sql -- Tablas de evaluaciones
06_CREATE_RESULTS_TABLES.sql   -- Tablas de resultados
07_CREATE_RESOURCES_TABLES.sql -- Tablas de recursos
08_CREATE_AUDIT_TABLES.sql     -- Tablas de auditoría
09_INSERT_INITIAL_DATA.sql     -- Datos iniciales
```

### **Ejecución Rápida (PowerShell)**

```powershell
# Variables
$Server = "localhost"
$Database = "EmoCheckDB"
$ScriptPath = "C:\Repositorios\emocheck-api\Database"

# Crear base de datos
sqlcmd -S $Server -Q "CREATE DATABASE [$Database]"

# Ejecutar scripts
Get-ChildItem "$ScriptPath\*.sql" | Sort-Object Name | ForEach-Object {
    Write-Host "Executing: $($_.Name)" -ForegroundColor Green
    sqlcmd -S $Server -d $Database -i $_.FullName
}

Write-Host "Database created successfully!" -ForegroundColor Cyan
```

---

## 🔑 Características Principales

### ✅ **Normalización**
- Base de datos normalizada (3FN)
- Relaciones bien definidas con Foreign Keys
- Integridad referencial garantizada

### ✅ **Seguridad**
- Consentimientos informados digitales
- Cifrado de datos sensibles (PasswordHash)
- Anonimización en reportes
- Trazabilidad completa

### ✅ **Auditoría**
- Registro de todas las acciones (`AuditLog`)
- Logs del sistema (`SystemLog`)
- Historial de emails enviados
- Tracking de exportaciones

### ✅ **Escalabilidad**
- Índices optimizados para consultas frecuentes
- Campos `CreatedAt` y `UpdatedAt` en todas las tablas
- Soft delete con campo `IsActive`
- Soporte para múltiples empresas (multi-tenant)

### ✅ **Flexibilidad**
- Módulos de evaluación configurables
- Tipos de recomendaciones personalizables
- Recursos de bienestar extensibles
- Roles y permisos granulares

---

## 📐 Convenciones de Nomenclatura

### **Tablas**
- Singular: `User`, `Company`, `Evaluation`
- PascalCase
- Nombres descriptivos en inglés

### **Columnas**
- PascalCase: `UserID`, `FirstName`, `CreatedAt`
- Primary Keys: `[TableName]ID`
- Foreign Keys: `[ReferencedTable]ID`

### **Constraints**
- Primary Keys: `PK_TableName`
- Foreign Keys: `FK_TableName_ReferencedTable`
- Unique: `UQ_TableName_ColumnName`
- Defaults: `DF_TableName_ColumnName`

### **Índices**
- `IX_TableName_ColumnName`

---

## 🔐 Datos Sensibles

### **Campos Encriptados**
- `User.PasswordHash` - Hashed con BCrypt
- `User.DocumentNumber` - Encriptado AES-256

### **Campos Confidenciales**
- Datos médicos/psicológicos en `EvaluationResponse`
- Resultados individuales en `EvaluationResult`
- Consentimientos en `InformedConsent`

### **Acceso Controlado**
- Solo el usuario ve sus propios resultados
- Administradores ven datos **agregados** o **anonimizados**
- Logs completos de acceso a datos sensibles

---

## 📈 Indicadores Calculados

La base de datos soporta el cálculo de:

- **Prevalencia**: % de trabajadores con condición actual
- **Incidencia**: % de casos nuevos en período
- **Participación**: % de evaluaciones completadas
- **Distribución de riesgo**: Por nivel (verde/amarillo/rojo)
- **Tendencias**: Evolución en el tiempo
- **Comparativos**: Por área, sede, cargo

---

## 🔄 Relaciones Principales

```
Company (1) -----> (N) Site
Company (1) -----> (N) Area
Company (1) -----> (N) User

User (1) -----> (N) Evaluation
User (1) -----> (N) Alert
User (N) <-----> (N) Role (through UserRole)

Evaluation (1) -----> (N) EvaluationResponse
Evaluation (1) -----> (1) EvaluationResult

EvaluationResult (1) -----> (N) Recommendation
EvaluationResult (1) -----> (N) DimensionScore
EvaluationResult (1) -----> (N) Alert

Alert (1) -----> (1) CaseTracking
```

---

## 📝 Scripts de Mantenimiento

### **Backup Diario**
```sql
-- Backup completo
BACKUP DATABASE [EmoCheckDB]
TO DISK = 'C:\Backups\EmoCheckDB_Full.bak'
WITH FORMAT, COMPRESSION, STATS = 10
GO
```

### **Limpieza de Logs Antiguos**
```sql
-- Eliminar logs de auditoría mayores a 2 años
DELETE FROM [audit].[AuditLog]
WHERE [Timestamp] < DATEADD(YEAR, -2, GETDATE())
GO

-- Eliminar logs del sistema mayores a 6 meses
DELETE FROM [audit].[SystemLog]
WHERE [Timestamp] < DATEADD(MONTH, -6, GETDATE())
AND [Level] IN ('Information', 'Warning')
GO
```

### **Estadísticas y Mantenimiento**
```sql
-- Actualizar estadísticas
EXEC sp_updatestats
GO

-- Rebuild índices fragmentados
ALTER INDEX ALL ON [security].[User] REBUILD
ALTER INDEX ALL ON [assessment].[Evaluation] REBUILD
ALTER INDEX ALL ON [results].[EvaluationResult] REBUILD
GO
```

---

## 🎯 Datos Iniciales Incluidos

Al ejecutar `09_INSERT_INITIAL_DATA.sql` se insertan:

- ✅ 7 Estados del sistema
- ✅ 3 Países (Colombia, México, USA)
- ✅ 5 Ciudades de Colombia
- ✅ 2 Aplicaciones
- ✅ 6 Tipos de cargo
- ✅ 6 Roles de usuario
- ✅ 4 Módulos de evaluación
- ✅ 7 Tipos de recomendación
- ✅ 4 Categorías de recursos

---

## 📞 Notas Técnicas

### **Colaciones**
Se recomienda usar `Latin1_General_CI_AS` para compatibilidad con español.

### **Tamaños de Texto**
- Nombres: `varchar(200)`
- Emails: `varchar(150)`
- Descripciones cortas: `varchar(500)`
- Descripciones largas: `nvarchar(2000)`
- Contenido completo: `nvarchar(max)`

### **Fechas**
- Todas las fechas en `datetime`
- Usar `GETDATE()` para defaults
- Timezone: Depende del servidor (usar UTC preferiblemente)

### **Decimales**
- Puntajes: `decimal(10,2)`
- Porcentajes: `decimal(5,2)`

---

## 📚 Recursos Adicionales

- [Documentación del Proyecto](../DOCUMENTACION_PROYECTO.md)
- [Modelo Entidad-Relación](./ER_DIAGRAM.md) *(próximamente)*
- [Diccionario de Datos](./DATA_DICTIONARY.md) *(próximamente)*

---

**Última actualización**: 2026-01-20  
**Versión de Base de Datos**: 1.0  
**Autor**: GitHub Copilot (AI Assistant)
