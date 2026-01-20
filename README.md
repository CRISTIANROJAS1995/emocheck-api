## ?? ?Qu谷 es EmoCheck?

EmoCheck es una **plataforma web de evaluaci車n y monitoreo de salud mental y bienestar emocional en el trabajo**. Permite a las empresas cumplir con normativas de salud ocupacional (SVE Psicosocial) mientras cuidan la salud mental de sus trabajadores de forma continua, confidencial y basada en evidencia cient赤fica.

---

## ?? Usuarios del Sistema

### **3 Tipos de Usuarios:**

### 1. **Trabajadores/Empleados**
- Acceden para realizar autoevaluaciones
- Ven sus propios resultados
- Reciben recomendaciones personalizadas
- Acceden a recursos de bienestar

### 2. **L赤deres HSE / Psic車logos Ocupacionales**
- Ven reportes agregados (no individuales, por confidencialidad)
- Gestionan alertas cr赤ticas
- Dan seguimiento a casos de riesgo
- Generan reportes por 芍rea/sede

### 3. **Administradores del Sistema**
- Configuran la plataforma
- Gestionan usuarios y empresas
- Acceden a todos los m車dulos
- Configuran integraciones

---

## ?? M車dulos de Evaluaci車n

### **1. Salud Mental**
Tamizaje de condiciones psicol車gicas comunes utilizando instrumentos cient赤ficos validados:

- **Ansiedad (GAD-7)**: Generalized Anxiety Disorder-7
- **Depresi車n (PHQ-9)**: Patient Health Questionnaire-9
- **Insomnio (ISI)**: Insomnia Severity Index
- **Estr谷s Percibido**: Escala de percepci車n de estr谷s

**Resultado**: Puntaje + semaforizaci車n (verde/amarillo/rojo) + recomendaciones personalizadas

### **2. Fatiga Laboral**
Evaluaci車n r芍pida de:
- Nivel de energ赤a cognitiva
- Agotamiento emocional
- Capacidad de concentraci車n

**Objetivo**: Detectar burnout en etapas tempranas

### **3. Clima Organizacional**
Percepci車n del trabajador sobre:
- Entorno laboral
- Liderazgo
- Prop車sito y motivaci車n
- Relaciones interpersonales

### **4. Riesgo Psicosocial**
Basado en la **Bater赤a del Ministerio del Trabajo** (Colombia):
- Factores intralaborales
- Factores extralaborales
- Estr谷s laboral

**Cumplimiento legal**: Resoluci車n 2404 de 2019

---

## ?? Flujo de Usuario

### **Paso 1: Bienvenida y Registro**
```
Usuario ingresa ↙ Se registra ↙ Acepta Consentimiento Informado Digital
```
- El consentimiento explica: qu谷 se har芍 con sus datos, confidencialidad, prop車sito
- Se guarda digitalmente con trazabilidad (fecha, hora, IP)

### **Paso 2: Completar Perfil**
```
Datos del usuario:
- Nombre completo
- Documento de identidad
- 芍rea
- Sede
- Tipo de cargo
- Correo corporativo
```

### **Paso 3: Realizar Evaluaci車n**
```
Selecciona m車dulo ↙ Responde cuestionario ↙ Sistema calcula resultado autom芍ticamente
```
- Las preguntas est芍n estandarizadas (instrumentos validados cient赤ficamente)
- Sistema asigna puntuaci車n autom芍tica seg迆n algoritmos establecidos

### **Paso 4: Ver Resultados**
```
Resultado semaforizado:
?? Verde: Bienestar adecuado
?? Amarillo: Atenci車n preventiva
?? Rojo: Requiere intervenci車n
```

### **Paso 5: Recomendaciones Personalizadas**
Seg迆n el resultado, el sistema entrega:
- Recursos de mindfulness
- Pausas activas
- Ejercicios de respiraci車n
- Recomendaci車n de consulta psicol車gica (si aplica)

### **Paso 6: Centro de Recursos de Bienestar**
Acceso permanente a:
- Calibraci車n emocional
- Mindfulness
- Neuropausas
- Apoyo profesional (solicitud de cita con psic車logo)

---

## ?? Panel de Administraci車n (Backend)

### **1. Gesti車n de Usuarios**
- Crear/editar/eliminar usuarios
- Asignar a empresa, 芍rea, sede
- Ver estado: activo/inactivo
- Gestionar roles (trabajador, l赤der, admin)

### **2. Monitoreo de Resultados**

**Tablero Visual con:**
- Indicadores globales por m車dulo
- Filtros: fecha, 芍rea, sede, nivel de riesgo
- Gr芍ficos de tendencias (ej: ?aument車 la ansiedad este mes?)
- Comparativos entre 芍reas

**Ejemplo de vista:**
```
芍rea: Producci車n (50 trabajadores)
-------------------------------------
Salud Mental:
  ?? 35 (70%)
  ?? 10 (20%)
  ?? 5 (10%)  ?? ALERTA
```

### **3. Alertas Cr赤ticas**
Cuando un trabajador sale en **rojo**:
- Se genera alerta autom芍tica
- Notificaci車n al psic車logo/HSE asignado
- Registro del seguimiento (?se contact車? ?qu谷 acci車n se tom車?)

**Importante**: El sistema NO muestra el nombre completo, usa ID o iniciales para proteger confidencialidad.

### **4. Gesti車n de Consentimientos**
- Repositorio de todos los consentimientos firmados
- Descarga de PDF por usuario
- Trazabilidad: qui谷n acept車, cu芍ndo, desde d車nde

### **5. Reportes Autom芍ticos**
Generaci車n de informes con indicadores SVE Psicosocial:
- N～ casos activos
- N～ casos cerrados
- % de prevalencia/incidencia
- % de participaci車n
- Exportaci車n: Excel, PDF
- Integraci車n con Power BI/Tableau

---

## ??? Arquitectura T谷cnica

### **Frontend: Angular 21 (Standalone)**
- Una sola aplicaci車n web responsive
- Dise?o modular (4 m車dulos de evaluaci車n)
- UX amigable con mensajes de acompa?amiento emocional
- Semaforizaci車n visual clara
- Gr芍ficos interactivos para dashboards

**Caracter赤sticas:**
- Componentes standalone (sin NgModules)
- Routing modular
- Estado global con Signals
- Guards para protecci車n de rutas
- Interceptors para autenticaci車n

### **Backend: C# / .NET 8 (Arquitectura Hexagonal)**

**?Por qu谷 hexagonal?**  
Separaci車n clara de responsabilidades en capas:

```
?? Domain (Dominio)
   - Entidades: Usuario, Evaluacion, Resultado
   - Interfaces: IUsuarioRepository, IEvaluacionService
   - L車gica de negocio pura
   
?? Application (Aplicaci車n)
   - Casos de uso: CrearEvaluacion, CalcularResultado, GenerarReporte
   - DTOs: UsuarioDto, ResultadoDto
   - Servicios de aplicaci車n
   
?? Infrastructure (Infraestructura)
   - Repositorios: UsuarioRepository (SQL Server)
   - Servicios externos: EmailService, PowerBIService
   - Autenticaci車n: JwtTokenService
   - Configuraciones
```

**Seguridad:**
- JWT + Refresh Tokens
- Roles y permisos (Claims-based)
- Encriptaci車n de datos sensibles
- HTTPS obligatorio
- Rate limiting
- CORS configurado

### **Base de Datos: SQL Server**

**Modelo de Datos - Tablas Principales:**
# ??? EmoCheck Database - SQL Server

## ?? Descripci車n General

Base de datos SQL Server para el proyecto **EmoCheck**, dise?ada con arquitectura normalizada, esquemas organizados y siguiendo las mejores pr芍cticas de seguridad y auditor赤a.

---

## ??? Arquitectura de Esquemas

La base de datos est芍 organizada en **6 esquemas principales**:

### 1. **dbo** (Default Schema)
Tablas maestras del sistema:
- `State` - Estados generales del sistema
- `Country` - Pa赤ses
- `City` - Ciudades

### 2. **configuration** (Configuraci車n)
Configuraci車n organizacional:
- `Company` - Empresas/Organizaciones
- `Site` - Sedes
- `Area` - 芍reas/Departamentos
- `JobType` - Tipos de cargo
- `Application` - Aplicaciones del sistema

### 3. **security** (Seguridad)
Usuarios y autenticaci車n:
- `User` - Usuarios del sistema
- `Role` - Roles
- `UserRole` - Relaci車n usuarios-roles
- `RefreshToken` - Tokens JWT
- `PasswordResetToken` - Tokens de recuperaci車n
- `InformedConsent` - Consentimientos informados

### 4. **assessment** (Evaluaciones)
M車dulos de evaluaci車n:
- `AssessmentModule` - M車dulos de evaluaci車n
- `Question` - Preguntas
- `QuestionOption` - Opciones de respuesta
- `Evaluation` - Evaluaciones realizadas
- `EvaluationResponse` - Respuestas

### 5. **results** (Resultados)
Resultados y alertas:
- `EvaluationResult` - Resultados de evaluaciones
- `DimensionScore` - Puntajes por dimensi車n
- `Recommendation` - Recomendaciones personalizadas
- `RecommendationType` - Tipos de recomendaci車n
- `Alert` - Alertas cr赤ticas
- `CaseTracking` - Seguimiento de casos

### 6. **resources** (Recursos)
Recursos de bienestar:
- `ResourceCategory` - Categor赤as de recursos
- `WellnessResource` - Recursos de bienestar
- `UserResourceAccess` - Acceso a recursos
- `ProfessionalSupport` - Apoyo profesional
- `SupportRequest` - Solicitudes de apoyo

### 7. **audit** (Auditor赤a)
Trazabilidad y logs:
- `AuditLog` - Registro de auditor赤a
- `SystemLog` - Logs del sistema
- `EmailLog` - Emails enviados
- `DataExport` - Exportaciones de datos

---

## ?? Total de Tablas: 35+

---

**Seguridad en DB:**
- Cifrado AES-256 para campos sensibles (PasswordHash, Documento)
- Anonimizaci車n en reportes (solo ID)
- Backups autom芍ticos diarios
- Logs de auditor赤a (qui谷n accedi車 a qu谷 y cu芍ndo)
- 赤ndices optimizados para consultas frecuentes
- Procedimientos almacenados para reportes complejos

---

## ?? Seguridad y Cumplimiento Legal

### **Ley 1581 de 2012 (Protecci車n de Datos Personales - Colombia)**
? Consentimiento informado expl赤cito  
? Finalidad clara del tratamiento de datos  
? Derecho de acceso, rectificaci車n y eliminaci車n  
? Cifrado y almacenamiento seguro  
? Trazabilidad de aceptaciones  

### **Confidencialidad M谷dica**
- Los datos de salud son **ultra sensibles**
- Solo el usuario ve sus resultados individuales
- Administradores ven datos **agregados** o **anonimizados**
- En alertas cr赤ticas: se usa ID, no nombre completo
- Separaci車n de datos personales y datos de salud

### **Trazabilidad Total**
Cada acci車n se registra:
```json
{
  "id": "12345",
  "usuario": "user@empresa.com",
  "accion": "Complet車 evaluaci車n de Salud Mental",
  "fecha": "2026-01-20T10:30:45Z",
  "ip": "192.168.1.100",
  "detalles": {
    "evaluacionId": "EVA-001",
    "modulo": "Salud Mental",
    "resultado": "Amarillo"
  }
}
```

### **Medidas de Seguridad Implementadas**
- ? HTTPS obligatorio (TLS 1.3)
- ? Autenticaci車n multifactor (opcional)
- ? Tokens con expiraci車n corta
- ? Refresh tokens almacenados de forma segura
- ? Validaci車n de inputs (prevenci車n XSS, SQL Injection)
- ? Rate limiting (prevenci車n DDoS)
- ? Logs de accesos sospechosos
- ? Aislamiento de datos por empresa

---

## ?? Integraciones

### **1. Power BI / Tableau**
- Dashboard avanzado con visualizaciones
- Conexi車n directa a vistas de SQL Server
- Actualizaci車n en tiempo real
- Filtros interactivos por empresa/芍rea/sede

### **2. APIs de ARL (Administradoras de Riesgos Laborales)**
- Env赤o autom芍tico de reportes agregados
- Cumplimiento de normativas SST
- Formato XML o JSON seg迆n requerimientos

### **3. HR Tech / HRIS (Human Resources Information System)**
- Importaci車n masiva de usuarios
- Sincronizaci車n de cambios organizacionales
- Actualizaci車n autom芍tica de 芍reas/sedes

### **4. Sistema de Notificaciones**
- Email (SMTP configurado)
- SMS (Twilio/similar)
- Push notifications (para versi車n PWA futura)

### **5. Exportaci車n de Datos**
- PDF (reportes individuales y agregados)
- Excel (tablas din芍micas)
- CSV (para an芍lisis externos)
- XML (para auditor赤as y ARL)

---

## ?? Resumen Ejecutivo

### **?Qu谷 hace EmoCheck?**

1. ? **Eval迆a** la salud mental y bienestar de trabajadores con cuestionarios cient赤ficos
2. ? **Detecta** casos de riesgo con semaforizaci車n autom芍tica
3. ? **Alerta** a los responsables de HSE/psicolog赤a cuando hay casos cr赤ticos
4. ? **Recomienda** recursos personalizados seg迆n el resultado
5. ? **Reporta** indicadores agregados para toma de decisiones
6. ? **Cumple** con normativas legales de protecci車n de datos y salud ocupacional
7. ? **Integra** con herramientas externas (BI, ARL, HRIS)

### **Beneficios Clave**

**Para la Empresa:**
- Cumplimiento legal (Resoluci車n 2404/2019)
- Reducci車n de ausentismo
- Mejora del clima laboral
- Datos para toma de decisiones
- ROI medible

**Para el Trabajador:**
- Autoconocimiento de su salud mental
- Acceso a recursos de bienestar
- Confidencialidad garantizada
- Apoyo profesional cuando lo necesita
- Prevenci車n de condiciones graves

**Para el 芍rea de HSE/RRHH:**
- Visibilidad de tendencias
- Alertas tempranas
- Reportes automatizados
- Seguimiento de casos
- Evidencia para auditor赤as

---

## ?? Stack Tecnol車gico

### **Frontend**
- Angular 21 (Standalone Components)
- TypeScript 5.x
- RxJS para programaci車n reactiva
- Chart.js / ApexCharts para gr芍ficos
- TailwindCSS / Angular Material para UI
- PWA capabilities (opcional)

### **Backend**
- .NET 8 (C#)
- Entity Framework Core 8
- ASP.NET Core Web API
- FluentValidation
- AutoMapper
- MediatR (CQRS pattern)
- Serilog (logging estructurado)
- xUnit (testing)

### **Base de Datos**
- SQL Server 2022 / Azure SQL
- Redis (cach谷 de sesiones)

### **DevOps**
- Git / GitHub
- Docker / Docker Compose
- CI/CD (GitHub Actions / Azure DevOps)
- SonarQube (an芍lisis de c車digo)
- Swagger / OpenAPI (documentaci車n)

### **Infraestructura (asumida por cliente)**
- Azure App Service / AWS EC2
- Azure SQL Database / AWS RDS
- Azure Blob Storage / AWS S3
- Azure Application Insights / CloudWatch
- CDN para assets est芍ticos
- SSL/TLS certificates
