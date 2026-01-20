# DOCUMENTACIÓN TÉCNICA - VERIFFICA BACKEND

**Fecha:** Octubre 2025  
**Versión:** 1.0  
**Framework:** .NET 8.0  
**Base de Datos:** SQL Server (sin migrations)

---

## 1. DESCRIPCIÓN GENERAL

Veriffica es un sistema backend para la gestión y administración de exámenes/evaluaciones de candidatos. El sistema se integra con un servicio externo llamado **VerifEye** para la aplicación y calificación de pruebas.

### Propósito
- Gestionar empresas, usuarios y candidatos
- Administrar catálogos de exámenes
- Asignar exámenes a candidatos (subjects)
- Sincronizar resultados desde VerifEye
- Generar reportes en PDF de resultados
- Controlar licencias de exámenes por empresa

---

## 2. ARQUITECTURA

El proyecto sigue **Arquitectura Hexagonal** (Ports and Adapters) con 4 capas claramente separadas:

```
┌─────────────────────────────────────────────┐
│              API (Capa Web)                  │
│  - Controllers                               │
│  - Program.cs (Startup)                      │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│          APPLICATION (Casos de Uso)          │
│  - Services (Lógica de aplicación)           │
│  - DTOs (Objetos de transferencia)           │
│  - Mappers (Conversión entidad ↔ DTO)        │
│  - Facades (Operaciones complejas)           │
└─────────────────────────────────────────────┘
                    ↓
┌─────────────────────────────────────────────┐
│           DOMAIN (Núcleo del Negocio)        │
│  - Entities (Modelos de dominio)             │
│  - Interfaces (Contratos)                    │
│  - Enums                                     │
│  - Exceptions                                │
└─────────────────────────────────────────────┘
                    ↑
┌─────────────────────────────────────────────┐
│        INFRASTRUCTURE (Adaptadores)          │
│  - Repositories (Persistencia)               │
│  - Data (DbContext)                          │
│  - Services (Integraciones externas)         │
└─────────────────────────────────────────────┘
```

---

## 3. ESTRUCTURA DE CARPETAS

```
Veriffica/
├── API/
│   ├── Controllers/          # Endpoints REST
│   ├── Templates/            # Plantillas HTML para PDFs
│   ├── Program.cs            # Configuración de la aplicación
│   └── appsettings.json      # Configuración (DB, JWT, VerifEye)
│
├── Application/
│   ├── Services/             # Lógica de aplicación
│   ├── DTOs/                 # Objetos de transferencia de datos
│   ├── Mappers/              # Conversión manual Entidad ↔ DTO
│   └── Facades/              # Operaciones complejas multi-servicio
│
├── Domain/
│   ├── Entities/             # Modelos de dominio
│   ├── Interfaces/           # Contratos de servicios y repositorios
│   ├── Enums/                # Enumeraciones (RoleEnum, StateEnum)
│   ├── Exceptions/           # Excepciones personalizadas
│   └── DTOs/VerifEye/        # DTOs para integración externa
│
└── Infrastructure/
    ├── Data/                 # ApplicationDbContext
    ├── Repositories/         # Implementación de repositorios
    └── Services/             # VerifEyeService, ExamReportService
```

---

## 4. MODELO DE DATOS

### 4.1 Entidades Principales

#### **User** (Usuario del Sistema)
- `UserID` (PK)
- `Email`, `Password`, `FullName`, `Phone`, `Address`
- `StateID` (FK → State)
- `ProfileImage`
- Relaciones M:N con `Role`, `Area`, `City`

#### **Company** (Empresa Cliente)
- `CompanyID` (PK)
- `Name`, `Nit`, `Description`, `Address`, `Phone`
- `BusinessGroupID` (FK → BusinessGroup)
- `CountryID` (FK → Country)
- `StateID` (FK → State)
- `IsVisible` (bool)

#### **Subject** (Candidato/Evaluado)
- `SubjectID` (PK)
- `Identifier` (Identificador único interno)
- `IdentifierExternal` (ID en VerifEye)
- `Name`, `LastName`, `Email`, `Phone`
- `CompanyID` (FK → Company)

#### **Exam** (Catálogo de Exámenes)
- `ExamID` (PK)
- `ExamTypeID` (FK → ExamType)
- `ExternalTemplateId` (Template ID en VerifEye)
- `ExternalExamName`, `ExternalLocale`, `ExternalCustomerId`
- `Description`, `Company`

#### **ExamSubject** (Asignación Examen-Candidato)
- `ExamSubjectID` (PK)
- `ExamID` (FK → Exam)
- `SubjectID` (FK → Subject)
- `ExternalExamId` (ID del examen instanciado en VerifEye)
- `ExternalExamUrl` (Link para que el candidato tome el examen)
- `ExternalExamStatus`, `ExternalExamStep`

#### **ExamResult** (Resultados de Examen)
- `ExamResultID` (PK)
- `ExamSubjectID` (FK → ExamSubject)
- `ExternalExamScore1`, `ExternalExamScore2`, `ExternalExamScore3`, `ExternalExamScore4`
- `ExternalExamResult1`, `ExternalExamResult2`, `ExternalExamResult3`
- `ExternalExamQuestions` (JSON de preguntas)
- `ExternalExamQueued`, `ExternalExamScored`
- `ResultExamId` (ID único del resultado)

#### **BusinessGroupLicense** (Licencias de Grupo Empresarial) ⭐ NUEVO
- `BusinessGroupLicenseID` (PK)
- `BusinessGroupID` (FK → BusinessGroup)
- `PurchasedLicenses` (Licencias compradas)
- `AssignedLicenses` (Licencias asignadas a empresas)
- `UsedLicenses` (Licencias realmente consumidas)
- `OverdraftLicenses` (Sobregiro permitido)
- `AvailableLicenses` (Calculado: Purchased - Assigned)

#### **CompanyLicense** (Licencias de Empresa) ⚡ ACTUALIZADO
- `CompanyLicenseID` (PK)
- `CompanyID` (FK → Company)
- `BusinessGroupLicenseID` (FK → BusinessGroupLicense)
- `AllocatedLicenses` (Licencias recibidas del grupo)
- `AssignedLicenses` (Licencias asignadas a áreas)
- `UsedLicenses` (Licencias consumidas)
- `AvailableLicenses` (Calculado: Allocated - Assigned)

#### **AreaLicense** (Licencias de Área) ⭐ NUEVO
- `AreaLicenseID` (PK)
- `AreaID` (FK → Area)
- `CompanyLicenseID` (FK → CompanyLicense)
- `AllocatedLicenses` (Licencias recibidas de la empresa)
- `AssignedLicenses` (Licencias asignadas a ciudades)
- `UsedLicenses` (Licencias consumidas)
- `AvailableLicenses` (Calculado: Allocated - Assigned)

#### **CityLicense** (Licencias de Ciudad - Opcional) ⭐ NUEVO
- `CityLicenseID` (PK)
- `CityID` (FK → City)
- `AreaLicenseID` (FK → AreaLicense)
- `AllocatedLicenses` (Licencias recibidas del área)
- `UsedLicenses` (Licencias consumidas)
- `AvailableLicenses` (Calculado: Allocated - Used)

#### **License** (Tracking Individual de Licencias) ⭐ NUEVO
- `LicenseID` (PK)
- `LicenseCode` (Código único: ej. LIC-BG001-2025-000001)
- `BusinessGroupLicenseID`, `CompanyLicenseID`, `AreaLicenseID`, `CityLicenseID`
- `StatusID` (1=Assigned, 2=Used, 3=Expired)
- `ExamSubjectID` (FK → ExamSubject cuando se asigna)
- `ExamResultID` (FK → ExamResult cuando se usa)
- `AssignedDate`, `UsedDate`

#### **LicenseLog** (Log Unificado de Operaciones) ⭐ NUEVO
- `LicenseLogID` (PK)
- `LicenseLevel` ('BusinessGroup', 'Company', 'Area', 'City')
- `EntityID` (ID de la entidad en ese nivel)
- `ActionType` ('purchase', 'assign', 'use', 'overdraft', 'release')
- `Quantity`, `BalanceBefore`, `BalanceAfter`
- `LicenseID`, `RelatedExamResultID`

#### **LicenseConfiguration** (Configuración Global) ⭐ NUEVO
- `LicenseConfigurationID` (PK)
- `ConfigKey` (ej: 'MaxOverdraftLicenses')
- `ConfigValue` (ej: 10)
- `Description`

#### **CompanyLicenseLog** (Histórico de Licencias - DEPRECADO)
- `CompanyLicenseLogID` (PK, auto-increment)
- `CompanyLicenseID` (FK → CompanyLicense)
- ⚠️ **NOTA:** Este log se mantiene por compatibilidad pero el nuevo sistema usa `LicenseLog`

### 4.2 Entidades de Catálogo

- **Area** (Áreas de trabajo)
- **BusinessGroup** (Grupos empresariales)
- **City** (Ciudades)
- **Country** (Países)
- **State** (Estados: Activo/Inactivo/Pendiente/Completado)
- **Role** (Roles de usuario)
- **Permission** (Permisos del sistema)
- **ExamType** (Tipos de examen)

### 4.3 Tablas de Relación Many-to-Many

- `Rl_UserRole` → User ↔ Role
- `Rl_UserArea` → User ↔ Area
- `Rl_UserCity` → User ↔ City
- `Rl_RolePermission` → Role ↔ Permission

### 4.4 Diagrama de Relaciones Principales

```
Company (1) ─────→ (N) Subject
   │                      │
   │                      │
   └→ (1:N) CompanyLicense │
                           │
Exam (1) ←─────────────────┘
   │
   │
   ↓
ExamSubject (N:1)
   │
   │
   ↓
ExamResult (N:1)
```

### 4.5 Diagrama de Jerarquía de Licencias (NUEVO SISTEMA) ⭐

```
┌─────────────────────────────────────┐
│      BusinessGroup (Compra)         │
│  PurchasedLicenses: 100             │
│  AssignedLicenses: 100              │
│  OverdraftLicenses: 5               │
└─────────────────┬───────────────────┘
                  │
        ┌─────────┴──────────┐
        ↓                    ↓
  ┌──────────┐         ┌──────────┐
  │ Company A│         │ Company B│
  │  Alloc:60│         │  Alloc:40│
  │  Assign:50         │  Assign:40
  │  Used:45 │         │  Used:38 │
  └────┬─────┘         └────┬─────┘
       │                    │
   ┌───┴───┐            ┌───┴───┐
   ↓       ↓            ↓       ↓
 Area1   Area2        Area3   Area4
Alloc:30 Alloc:20   Alloc:25 Alloc:15
Assign:25 Assign:0  Assign:20 Assign:0
Used:20  Used:15    Used:18  Used:0
   │       │            │       │
   ↓       ↓            ↓       ↓
City1-5  (Sin)      City6-8   (Sin)
         ciudades            ciudades

Reglas:
✅ BusinessGroup solo puede asignar lo que compró
✅ Company solo puede asignar lo que recibió
✅ Area solo puede asignar lo que recibió
✅ City solo puede usar lo que recibió
✅ Sobregiro: Permitido hasta MaxOverdraftLicenses (configurable)
✅ Al crear ExamResult → License.StatusID cambia de 1 (Assigned) a 2 (Used)
```

---

## 5. TECNOLOGÍAS Y DEPENDENCIAS

### Frameworks y Librerías
- **.NET 8.0** - Framework principal
- **Entity Framework Core** - ORM (Code First, sin migrations)
- **SQL Server** - Base de datos
- **JWT Bearer** - Autenticación y autorización
- **Swashbuckle (Swagger)** - Documentación de API
- **DinkToPdf** - Generación de PDFs
- **HttpClient** - Comunicación con VerifEye API

### Paquetes NuGet Principales
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
<PackageReference Include="Swashbuckle.AspNetCore" />
<PackageReference Include="DinkToPdf" />
```

---

## 6. CONFIGURACIÓN

### 6.1 Connection String (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "data source=SQL5111.site4now.net;initial catalog=db_aa9e81_veriffica;User Id=db_aa9e81_veriffica_admin;Password=Cambiar123.;MultipleActiveResultSets=True;App=EntityFramework;TrustServerCertificate=True"
  }
}
```

### 6.2 JWT Configuration
```json
{
  "JWT": {
    "Key": "w9Xm3LpF7RzNq2YuKcBtVgJ1oHAxEsT6MdQeLiPbWvTkJoFgUrYhZnXcOsMaEy",
    "Issuer": "https://veriffica.com/home/main",
    "Audience": "verifica.info"
  }
}
```

### 6.3 VerifEye API Configuration
```json
{
  "VerifEye": {
    "AuthBase64": "NkMyQjFCQzA4NjZENEQxM0FCMzZDQTQ0QTYxREEyNTE6NTgwQTY3OTM2QTExNDE1NkE4MThDM0Y1NTAyMTMyNzA="
  }
}
```

---

## 7. SEGURIDAD Y AUTENTICACIÓN

### 7.1 Sistema de Roles
El sistema implementa 4 roles principales:

```csharp
public enum RoleEnum
{
    Basic = 1,         // Usuario básico
    Admin = 2,         // Administrador total
    Investigator = 3,  // Investigador/Evaluador
    Reports = 4        // Acceso a reportes
}
```

### 7.2 Autorización en Controllers
Los endpoints están protegidos con el atributo `[Authorize]`:

```csharp
[Authorize(Roles = "admin")]           // Solo admin
[Authorize(Roles = "admin,investigator")] // Admin o investigator
```

### 7.3 Generación de JWT
El token JWT se genera en `LoginController` con:
- Claims: UserID, Email, FullName, Roles
- Expiración configurable
- Firma con clave simétrica (HMAC SHA256)

---

## 8. INYECCIÓN DE DEPENDENCIAS

En `Program.cs` se configuran todos los servicios:

### 8.1 DbContext
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

### 8.2 Repositorios (Scoped)
```csharp
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IExamSubjectRepository, ExamSubjectRepository>();
builder.Services.AddScoped<IExamResultRepository, ExamResultRepository>();
builder.Services.AddScoped<ICompanyLicenseRepository, CompanyLicenseRepository>();
// ... más repositorios
```

### 8.3 Servicios de Aplicación (Scoped)
```csharp
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IExamSubjectService, ExamSubjectService>();
builder.Services.AddScoped<IResultService, ResultService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
```

### 8.4 Servicios Externos (Scoped)
```csharp
builder.Services.AddScoped<IVerifEyeService, VerifEyeService>();
builder.Services.AddScoped<IExamReportService, ExamReportService>();
```

### 8.5 Facades
```csharp
builder.Services.AddScoped<UserHierarchyFacade>();
```

### 8.6 PDF Converter (Singleton)
```csharp
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
```

---

## 9. API ENDPOINTS

### 9.1 LoginController
```
POST   /api/Login/login              # Autenticación de usuario
```

### 9.2 UserController
```
GET    /api/User/users               # Listar usuarios [admin]
GET    /api/User/user/{id}           # Obtener usuario por ID [admin]
POST   /api/User/user                # Crear usuario [admin]
PUT    /api/User/user/{id}           # Actualizar usuario [admin]
DELETE /api/User/user/{id}           # Eliminar usuario [admin]
PUT    /api/User/change-password     # Cambiar contraseña [todos]
```

### 9.3 CompanyController
```
GET    /api/Company/companies        # Listar empresas [admin,investigator]
GET    /api/Company/company/{id}     # Obtener empresa [admin,investigator]
POST   /api/Company/company          # Crear empresa [admin]
PUT    /api/Company/company/{id}     # Actualizar empresa [admin]
DELETE /api/Company/company/{id}     # Eliminar empresa [admin]
POST   /api/Company/assign-licenses  # Asignar licencias [admin]
```

### 9.4 SubjectController
```
GET    /api/Subject/subjects         # Listar candidatos [admin,investigator]
GET    /api/Subject/subject/{id}     # Obtener candidato [admin,investigator]
POST   /api/Subject/subject          # Crear candidato [admin,investigator]
PUT    /api/Subject/subject/{id}     # Actualizar candidato [admin,investigator]
DELETE /api/Subject/subject/{id}     # Eliminar candidato [admin]
POST   /api/Subject/migration        # Migrar candidatos desde VerifEye [admin]
```

### 9.5 CatalogController
```
GET    /api/Catalog/exams            # Listar catálogo de exámenes [admin,investigator]
GET    /api/Catalog/exam-types       # Listar tipos de examen [admin,investigator]
GET    /api/Catalog/areas            # Listar áreas [admin,investigator]
GET    /api/Catalog/cities           # Listar ciudades [admin,investigator]
GET    /api/Catalog/countries        # Listar países [admin,investigator]
GET    /api/Catalog/business-groups  # Listar grupos empresariales [admin,investigator]
```

### 9.6 QueueController
```
POST   /api/Queue/assign-exam        # Asignar examen a candidato [admin,investigator]
GET    /api/Queue/exams              # Listar exámenes en cola [admin,investigator,reports]
```

### 9.7 ResultController
```
POST   /api/Result/sync-results      # Sincronizar resultados desde VerifEye [admin,investigator]
GET    /api/Result/results           # Listar resultados [admin,investigator,reports]
```

### 9.8 ReportsController
```
GET    /api/Reports/pdf/{examResultId}  # Generar PDF de resultado [admin,investigator,reports]
```

---

## 10. SERVICIOS PRINCIPALES

### 10.1 UserService
**Responsabilidades:**
- CRUD de usuarios
- Gestión de roles de usuario
- Cambio de contraseña
- Validaciones de negocio

**Métodos clave:**
- `GetAllUsersAsync()` - Lista usuarios con roles
- `CreateUserAsync(dto)` - Crea usuario y asigna roles
- `ChangePasswordAsync(userId, newPassword)` - Cambia contraseña con hash

### 10.2 CompanyService
**Responsabilidades:**
- CRUD de empresas
- Gestión de licencias
- Asignación de licencias

**Métodos clave:**
- `GetAllCompaniesAsync()` - Lista empresas con relaciones
- `AssignLicensesAsync(dto)` - Asigna licencias y registra en log
- Validación de visibilidad (`IsVisible`)

### 10.3 SubjectService
**Responsabilidades:**
- CRUD de candidatos (subjects)
- Migración desde VerifEye
- Sincronización de IDs externos

**Métodos clave:**
- `CreateSubjectAsync(dto)` - Crea candidato y lo registra en VerifEye
- `MigrationOfConverusAsync()` - Importa candidatos desde VerifEye
- Generación de `Identifier` único

### 10.4 ExamService
**Responsabilidades:**
- CRUD de exámenes
- Listado de plantillas disponibles en VerifEye

**Métodos clave:**
- `GetListAvailableTestsAsync()` - Obtiene templates de VerifEye
- `CreateExamAsync(dto)` - Registra examen con IDs externos

### 10.5 ExamSubjectService (QueueService)
**Responsabilidades:**
- Asignación de exámenes a candidatos
- Generación de links de examen
- Control de licencias disponibles

**Métodos clave:**
- `AssignExamToSubjectAsync(dto)` - Asigna examen, consume licencia
- Crea registro en VerifEye (`PrepareTestAsync`)
- Valida disponibilidad de licencias

### 10.6 ResultService
**Responsabilidades:**
- Sincronización de resultados desde VerifEye
- Generación de reportes PDF

**Métodos clave:**
- `SyncResultsAsync()` - Trae resultados de VerifEye y los guarda
- `GenerateExamReportPdfAsync(examResultId)` - Genera PDF con DinkToPdf
- Parseo de JSON de preguntas y respuestas

### 10.7 CatalogService
**Responsabilidades:**
- Proporcionar datos de catálogos
- Listado de áreas, ciudades, países, tipos de examen

**Métodos clave:**
- `GetExamsAsync()` - Lista exámenes
- `GetExamTypesAsync()` - Lista tipos de examen
- Métodos de consulta para otros catálogos

---

## 11. INTEGRACIÓN CON VERIFEYE

### 11.1 VerifEyeService
Servicio que encapsula todas las llamadas HTTP al API de VerifEye.

**Endpoints Consumidos:**
- `POST /examinees` - Crear candidato
- `GET /examinees` - Listar candidatos
- `GET /examinees/{id}` - Obtener candidato por ID
- `GET /test-templates` - Listar plantillas de examen
- `POST /test-templates/{id}/exams` - Preparar examen
- `GET /exams` - Listar exámenes en cola
- `GET /exams/{id}` - Obtener examen por ID
- `DELETE /exams/{id}` - Eliminar examen en cola
- `GET /exam-results` - Listar resultados
- `GET /exam-results/{id}` - Obtener resultado completo

**Autenticación:**
- Header: `Authorization: Basic {AuthBase64}`
- Token Base64 configurado en `appsettings.json`

### 11.2 Flujo de Asignación de Examen
1. Usuario asigna examen a candidato en Veriffica
2. Sistema valida licencias disponibles
3. Se llama a `VerifEyeService.PrepareTestAsync()`
4. VerifEye retorna URL de examen y ID de examen
5. Se guarda `ExamSubject` con IDs externos
6. Se consume una licencia de la empresa
7. Candidato recibe link para realizar examen

### 11.3 Flujo de Sincronización de Resultados
1. Usuario ejecuta "Sync Results"
2. Se llama a `VerifEyeService.ListExamResultsAsync()`
3. Por cada resultado nuevo:
   - Se obtiene detalle completo con `GetExamRepositoryByIdAsync()`
   - Se busca el `ExamSubject` correspondiente
   - Se crea registro `ExamResult` con todos los scores
4. Resultados quedan disponibles para reportes

---

## 12. GENERACIÓN DE REPORTES PDF

### 12.1 Tecnología
- **DinkToPdf** - Wrapper de wkhtmltopdf (OJO solo funcionan estilos CSS tradicionales, nada de flex, todo con tablas etc...)
- Plantilla HTML en `API/Templates/report-template.html`
- Librería nativa `libwkhtmltox.dll`

### 12.2 Proceso de Generación
1. Se obtiene `ExamResult` con relaciones
2. Se parsean JSON de preguntas y respuestas
3. Se carga plantilla HTML
4. Se reemplazan placeholders con datos reales:
   - Información del candidato
   - Scores y resultados
   - Preguntas y respuestas
   - Imágenes (fotos del examen)
5. Se convierte HTML a PDF con DinkToPdf
6. Se retorna como `FileContentResult`

### 12.3 Estructura del Reporte
- Header con logo y datos de empresa
- Información del candidato
- Resumen de scores (Score1, Score2, Score3, Score4)
- Listado de preguntas con respuestas
- Fotos capturadas durante el examen
- Footer con fecha y firma

---

## 13. PATRONES DE DISEÑO APLICADOS

### 13.1 Repository Pattern
Abstracción de la capa de datos mediante interfaces:
```csharp
public interface IGenericRepository<T>
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

Cada entidad tiene su repositorio específico heredando de `GenericRepository<T>`.

### 13.2 Service Layer Pattern
Lógica de negocio encapsulada en servicios:
- Servicios implementan interfaces del dominio
- Controllers solo invocan servicios
- Validaciones en capa de servicio

### 13.3 DTO Pattern
Objetos de transferencia separados de entidades:
- `CreateUserDto`, `UpdateUserDto`, `UserDto`
- Mappers manuales (sin AutoMapper)
- Control total sobre qué datos exponer

### 13.4 Facade Pattern
`UserHierarchyFacade` para operaciones complejas:
- Coordina múltiples servicios
- Simplifica operaciones multi-paso
- Reduce acoplamiento en controllers

### 13.5 Dependency Injection
Todo el sistema usa DI:
- Inversión de control completa
- Testeable y desacoplado
- Configuración en `Program.cs`

---

## 14. MAPPERS

Los mappers son clases estáticas que convierten entidades a DTOs y viceversa.

### 14.1 Ejemplos

**UserMapper:**
```csharp
public static UserDto ToUserDto(User user)
{
    return new UserDto
    {
        UserID = user.UserID,
        Email = user.Email,
        FullName = user.FullName,
        Roles = user.UserRoles.Select(ur => new RoleDto { ... }).ToList()
    };
}
```

**SubjectMapper:**
```csharp
public static SubjectDto ToSubjectDto(Subject subject)
{
    return new SubjectDto
    {
        SubjectID = subject.SubjectID,
        Identifier = subject.Identifier,
        Name = subject.Name,
        CompanyName = subject.Company.Name
    };
}
```

### 14.2 Ventajas
- Control total sobre conversiones
- No requiere configuración adicional
- Fácil debugging
- Performance óptimo

---

## 15. VALIDACIONES

### 15.1 Validaciones en Entidades
Los constructores de entidades realizan validaciones básicas:
```csharp
public User(int stateId, string email, string password, string fullName)
{
    if (string.IsNullOrEmpty(email))
        throw new ArgumentException("Email no puede ser vacío");
    
    // ... asignaciones
}
```

### 15.2 Validaciones en Servicios
Los servicios validan reglas de negocio:
- Disponibilidad de licencias
- Existencia de relaciones (Company, Subject)
- Permisos de usuario
- Estados válidos

### 15.3 Excepciones Personalizadas
El proyecto tiene excepciones del dominio:
- Permiten manejo específico de errores
- Mensajes claros para el cliente
- Separación de errores de negocio vs técnicos

---

## 16. MANEJO DE ESTADOS

### 16.1 StateEnum
```csharp
public enum StateEnum
{
    Active = 1,      // Activo
    Inative = 2,     // Inactivo
    Pending = 3,     // Pendiente
    Completed = 4    // Completado
}
```

### 16.2 Uso
- Usuarios: Active/Inactive
- Empresas: Active/Inactive
- Exámenes: Pending/Completed
- Resultados: Pending/Completed

---

## 17. BASE DE DATOS

### 17.1 Estrategia
**NO SE USAN MIGRATIONS**  
La base de datos ya existe y fue creada con scripts SQL.

### 17.2 Configuración en DbContext
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configuración de llaves compuestas para tablas M:N
    modelBuilder.Entity<Rl_UserRole>()
        .HasKey(ur => new { ur.UserID, ur.RoleID });
    
    // Configuración de relaciones
    modelBuilder.Entity<Rl_UserRole>()
        .HasOne(ur => ur.User)
        .WithMany(u => u.UserRoles)
        .HasForeignKey(ur => ur.UserID);
}
```

### 17.3 DbSets
```csharp
public DbSet<User> User { get; set; }
public DbSet<Company> Company { get; set; }
public DbSet<Subject> Subject { get; set; }
public DbSet<Exam> Exam { get; set; }
public DbSet<ExamSubject> ExamSubject { get; set; }
public DbSet<ExamResult> ExamResult { get; set; }
public DbSet<CompanyLicense> CompanyLicense { get; set; }
public DbSet<CompanyLicenseLog> CompanyLicenseLog { get; set; }
// ... más entidades
```

### 17.4 Scripts de Base de Datos
Disponibles en:
- `Documentation/Database/Script Initial.sql`
- `Documentation/Database/Veriffica.bak`
- `db_aa9e81_veriffica_9_7_2025.bak`

---

## 18. CORS Y SEGURIDAD

### 18.1 Configuración CORS
```csharp
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
```
**⚠️ NOTA:** En producción se debe restringir `WithOrigins()` a dominios específicos.

### 18.2 HTTPS
```csharp
app.UseHttpsRedirection();
```

### 18.3 Autenticación/Autorización
```csharp
app.UseAuthentication();  // Primero autenticación
app.UseAuthorization();   // Luego autorización
```

---

## 19. SWAGGER

### 19.1 Configuración
```csharp
builder.Services.AddSwaggerGen();
app.UseSwagger();
app.UseSwaggerUI();
```

### 19.2 Acceso
URL: `https://localhost:{port}/swagger`

### 19.3 Funcionalidad
- Documentación interactiva de todos los endpoints
- Prueba de APIs desde el navegador
- Esquemas de DTOs
- Información de autorización

---

## 20. FLUJOS DE NEGOCIO PRINCIPALES

### 20.1 Creación de Usuario
1. Admin crea usuario con `POST /api/User/user`
2. Se valida email único
3. Se hashea la contraseña
4. Se asignan roles mediante `Rl_UserRole`
5. Se retorna el usuario creado

### 20.2 Creación de Empresa
1. Admin crea empresa con `POST /api/Company/company`
2. Se valida NIT único
3. Se asocia a BusinessGroup y Country
4. Se establece estado (Active)
5. Se retorna la empresa creada

### 20.3 Creación de Candidato
1. Investigator crea candidato con `POST /api/Subject/subject`
2. Se genera `Identifier` único
3. Se llama a VerifEye para crear examinee
4. Se guarda `IdentifierExternal` (ID de VerifEye)
5. Candidato queda listo para asignar exámenes

### 20.4 Asignación de Examen
1. Investigator asigna examen con `POST /api/Queue/assign-exam`
2. Se valida que la empresa tenga licencias disponibles
3. Se llama a VerifEye `PrepareTestAsync()`
4. VerifEye retorna:
   - `ExternalExamId`
   - `ExternalExamUrl` (link para el candidato)
5. Se crea registro `ExamSubject`
6. Se consume una licencia (`UsedLicenses++`)
7. Se retorna el link del examen

### 20.5 Sincronización de Resultados
1. Usuario ejecuta `POST /api/Result/sync-results`
2. Se obtienen todos los exam-results de VerifEye
3. Por cada resultado:
   - Se busca el `ExamSubject` por `ExternalExamId`
   - Si no existe `ExamResult`, se crea
   - Se guardan scores, respuestas, fotos, etc.
4. Se retorna cantidad de resultados sincronizados

### 20.6 Generación de Reporte
1. Usuario solicita `GET /api/Reports/pdf/{examResultId}`
2. Se obtiene `ExamResult` con todas las relaciones
3. Se parsean JSON de preguntas y respuestas
4. Se carga plantilla HTML
5. Se reemplazan datos dinámicos
6. Se convierte a PDF con DinkToPdf
7. Se retorna archivo PDF para descarga

---

## 21. BUENAS PRÁCTICAS IMPLEMENTADAS

### 21.1 Arquitectura
✅ Separación de responsabilidades (Hexagonal)  
✅ Inversión de dependencias (DIP)  
✅ Inyección de dependencias  
✅ Interfaces para contratos  

### 21.2 Código
✅ Constructores privados en entidades  
✅ Validaciones en constructores  
✅ Nombres descriptivos (C#)  
✅ Async/Await en toda la aplicación  
✅ Using statements para recursos  

### 21.3 Seguridad
✅ JWT para autenticación  
✅ Autorización por roles  
✅ Hashing de contraseñas  
✅ Validación de permisos en servicios  

### 21.4 Base de Datos
✅ Transacciones implícitas con EF Core  
✅ Relaciones configuradas en DbContext  
✅ Indices en columnas clave (en SQL)  
✅ Constraints de integridad referencial  

### 21.5 API
✅ RESTful endpoints  
✅ Códigos de estado HTTP apropiados  
✅ Documentación con Swagger  
✅ DTOs para entrada/salida  

---

## 22. LIMITACIONES Y CONSIDERACIONES

### 22.1 Sin Migrations
- Los cambios en el modelo de datos deben hacerse manualmente en SQL
- Requiere sincronización entre código y base de datos
- No hay historial de cambios de esquema

### 22.2 CORS Abierto
- Configuración actual permite cualquier origen (`*`)
- En producción debe restringirse a dominios específicos

### 22.3 Mappers Manuales
- Cada nuevo DTO requiere mapper manual
- Más código que AutoMapper
- Mayor control pero más mantenimiento

### 22.4 Gestión de Licencias
- Las licencias se consumen al asignar examen
- No hay proceso de liberación de licencias
- Si un examen no se completa, la licencia queda consumida

### 22.5 Dependencia Externa
- Alta dependencia de VerifEye API
- Si VerifEye falla, varias funcionalidades quedan inoperables
- No hay caché de datos de VerifEye

---

## 23. SISTEMA DE LICENCIAS JERÁRQUICO ⭐ ACTUALIZACIÓN 2025

### 23.1 Descripción del Nuevo Sistema

El sistema de licencias fue rediseñado para implementar una **jerarquía secuencial** que permite un control granular de licencias desde el nivel más alto (BusinessGroup) hasta el nivel más bajo (City, opcional).

### 23.2 Jerarquía de 4 Niveles

```
Nivel 1: BusinessGroup (Compra licencias)
   ↓ Asigna
Nivel 2: Company (Recibe y distribuye)
   ↓ Asigna
Nivel 3: Area (Recibe y distribuye)
   ↓ Asigna
Nivel 4: City (Recibe y consume) [OPCIONAL]
```

### 23.3 Reglas de Negocio

1. **Compra**: Solo BusinessGroup puede comprar licencias
2. **Asignación Descendente**: Cada nivel solo puede asignar lo que tiene disponible
3. **No Sobrepaso**: No se pueden asignar más licencias de las disponibles
4. **Tracking Individual**: Cada licencia tiene un código único y se rastrea individualmente
5. **Estados de Licencia**:
   - `Assigned (1)`: Licencia asignada a un examen pero no usado aún
   - `Used (2)`: Licencia consumida (cuando se crea ExamResult)
   - `Expired (3)`: Licencia expirada (futuro)
6. **Sobregiro Controlado**: BusinessGroup puede usar licencias negativas hasta un límite configurable
7. **Descuento Automático**: Al comprar nuevas licencias, se descuenta automáticamente el sobregiro

### 23.4 Sobregiro (Overdraft)

**Concepto**: Permite usar licencias cuando el saldo es cero, generando saldo negativo.

**Configuración**:
- Variable global: `MaxOverdraftLicenses` (default: 10)
- Se configura en tabla `LicenseConfiguration`

**Ejemplo**:
```
BusinessGroup compró: 100 licencias
Usó: 100 licencias
MaxOverdraft: 10

Usuario asigna 5 exámenes más:
- OverdraftLicenses = 5
- RealBalance = 100 - 100 - 5 = -5 (negativo)

Usuario compra 50 licencias nuevas:
- Sistema descuenta automáticamente: 50 - 5 = 45
- PurchasedLicenses = 150
- OverdraftLicenses = 0
- Disponibles = 45
```

### 23.5 Flujo de Asignación de Examen

```
1. Usuario asigna examen a candidato
2. Sistema busca licencia disponible en Area/City
3. Encuentra License con StatusID=1 (Assigned) y ExamSubjectID=NULL
4. Asigna: License.ExamSubjectID = ExamSubjectID
5. Candidato recibe link y realiza examen
6. Sistema sincroniza resultado y crea ExamResult
7. [TRIGGER AUTOMÁTICO EN SQL]
8. Busca License por ExamSubjectID
9. Actualiza License.StatusID = 2 (Used)
10. Actualiza License.ExamResultID = ExamResultID
11. Incrementa UsedLicenses en toda la jerarquía:
    - BusinessGroupLicense.UsedLicenses++
    - CompanyLicense.UsedLicenses++
    - AreaLicense.UsedLicenses++
    - CityLicense.UsedLicenses++ (si aplica)
12. Registra en LicenseLog con ActionType='use'
```

### 23.6 Validaciones Automáticas

El sistema valida en cada operación:

**Al Asignar a Company**:
```csharp
if (BusinessGroup.AvailableLicenses < quantity)
    throw new InvalidOperationException("Licencias insuficientes en BusinessGroup");
```

**Al Asignar a Area**:
```csharp
if (Company.AvailableLicenses < quantity)
    throw new InvalidOperationException("Licencias insuficientes en Company");
```

**Al Asignar a City**:
```csharp
if (Area.AvailableLicenses < quantity)
    throw new InvalidOperationException("Licencias insuficientes en Area");
```

**Al Consumir Licencia**:
```csharp
if (Area/City.AvailableLicenses <= 0 && !CanUseOverdraft)
    throw new InvalidOperationException("Sin licencias disponibles");
```

### 23.7 Log de Auditoría

Todas las operaciones quedan registradas en `LicenseLog` con:
- Nivel jerárquico (BusinessGroup, Company, Area, City)
- Tipo de acción (purchase, assign, use, overdraft, release)
- Cantidad de licencias
- Balance antes y después
- Usuario que ejecutó la acción
- Fecha y hora

### 23.8 Consultas Útiles

**Estado de BusinessGroup**:
```sql
SELECT * FROM vw_BusinessGroupLicenseStatus WHERE BusinessGroupID = 1;
```

**Estado de Company**:
```sql
SELECT * FROM vw_CompanyLicenseStatus WHERE CompanyID = 1;
```

**Estado de Area**:
```sql
SELECT * FROM vw_AreaLicenseStatus WHERE AreaID = 1;
```

**Historial de Operaciones**:
```sql
SELECT * FROM LicenseLog 
WHERE LicenseLevel = 'BusinessGroup' AND EntityID = 1
ORDER BY CreatedAt DESC;
```

**Licencias Disponibles por Area**:
```sql
SELECT COUNT(*) FROM License 
WHERE AreaLicenseID = 1 
  AND StatusID = 1 
  AND ExamSubjectID IS NULL;
```

### 23.9 Archivos Clave del Sistema

**Entidades**:
- `Domain/Entities/BusinessGroupLicense.cs`
- `Domain/Entities/CompanyLicense.cs` (actualizada)
- `Domain/Entities/AreaLicense.cs`
- `Domain/Entities/CityLicense.cs`
- `Domain/Entities/License.cs`
- `Domain/Entities/LicenseLog.cs`
- `Domain/Entities/LicenseConfiguration.cs`

**Enum**:
- `Domain/Enums/LicenseStatusEnum.cs`

**Servicio Principal**:
- `Domain/Interfaces/Services/ILicenseManagementService.cs`
- `Application/Services/LicenseManagementService.cs`

**Script SQL**:
- `Documentation/Database/Script_Nueva_Jerarquia_Licencias.sql`

**Guía de Implementación**:
- `GUIA_IMPLEMENTACION_LICENCIAS.md`

### 23.10 Ventajas del Nuevo Sistema

✅ **Control Granular**: Cada nivel controla sus propias licencias  
✅ **Auditoría Completa**: Todo queda registrado en logs  
✅ **Validaciones Automáticas**: Imposible asignar más de lo disponible  
✅ **Sobregiro Configurable**: Flexibilidad para casos excepcionales  
✅ **Tracking Individual**: Cada licencia es rastreable  
✅ **Estados Claros**: Assigned vs Used  
✅ **Trigger Automático**: Marcado de usado sin intervención manual  
✅ **Escalable**: Fácil agregar más niveles si se necesita  

---

## 24. TROUBLESHOOTING

### 23.1 Error de Conexión a Base de Datos
**Problema:** `Cannot connect to SQL Server`  
**Solución:**
- Verificar connection string en `appsettings.json`
- Verificar acceso de red al servidor SQL
- Verificar credenciales de usuario

### 23.2 Error 401 Unauthorized
**Problema:** Token JWT inválido o expirado  
**Solución:**
- Verificar que el token se envíe en header: `Authorization: Bearer {token}`
- Renovar token haciendo login nuevamente
- Verificar configuración JWT en `appsettings.json`

### 23.3 Error 403 Forbidden
**Problema:** Usuario sin permisos para el endpoint  
**Solución:**
- Verificar roles del usuario en base de datos
- Verificar atributo `[Authorize(Roles = "...")]` en controller
- Asignar rol apropiado al usuario

### 23.4 Error al Generar PDF
**Problema:** `libwkhtmltox.dll not found`  
**Solución:**
- Verificar que `libwkhtmltox.dll` esté en la carpeta de salida
- Verificar configuración en `.csproj`: `<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>`

### 23.5 Error al Comunicarse con VerifEye
**Problema:** `401 Unauthorized` o `Connection timeout`  
**Solución:**
- Verificar `AuthBase64` en `appsettings.json`
- Verificar conectividad con VerifEye API
- Verificar que credenciales sean válidas

---

## 24. DEPLOYMENT

### 24.1 Requisitos del Servidor
- Windows Server 2016+ o Linux con .NET 8 Runtime
- IIS 10+ o Nginx/Apache con proxy reverso
- SQL Server 2017+ (local o remoto)
- .NET 8 Runtime instalado
- `libwkhtmltox.dll` en carpeta de aplicación (Windows)

### 24.2 Pasos de Deployment
1. Publicar proyecto:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. Copiar archivos a servidor

3. Configurar `appsettings.json` con datos de producción:
   - Connection String de producción
   - JWT Key segura (diferente a desarrollo)
   - VerifEye AuthBase64
   - CORS origins específicos

4. Configurar IIS:
   - Crear Application Pool (.NET CLR Version: No Managed Code)
   - Crear sitio web apuntando a carpeta publish
   - Asignar permisos al Application Pool

5. Verificar que `libwkhtmltox.dll` esté presente

6. Iniciar aplicación

### 24.3 Variables de Entorno
Alternativamente, se pueden usar variables de entorno:
```bash
ConnectionStrings__DefaultConnection="..."
JWT__Key="..."
VerifEye__AuthBase64="..."
```

---

## 25. TESTING

### 25.1 Pruebas Recomendadas
- **Unit Tests**: Servicios de aplicación
- **Integration Tests**: Repositorios con base de datos de prueba
- **API Tests**: Controllers con Postman o similar

### 25.2 Herramientas Sugeridas
- xUnit o NUnit para unit tests
- Moq para mocking de dependencias
- FluentAssertions para assertions legibles
- Postman para pruebas de API

---