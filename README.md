# 🎯 EmoCheck API - Backend# 🎯 EmoCheck API



> Plataforma web de evaluación y monitoreo de salud mental y bienestar emocional en entornos laborales.## 🎯 ¿Qué es EmoCheck?



[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)EmoCheck es una **plataforma web de evaluación y monitoreo de salud mental y bienestar emocional en el trabajo**. Permite a las empresas cumplir con normativas de salud ocupacional (SVE Psicosocial) mientras cuidan la salud mental de sus trabajadores de forma continua, confidencial y basada en evidencia científica.

[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/sql-server)

[![Angular](https://img.shields.io/badge/Angular-21-DD0031?logo=angular)](https://angular.io/)---

[![License](https://img.shields.io/badge/License-Proprietary-red.svg)]()

---

### **Tipos de Usuarios:**

## 📋 Tabla de Contenidos

### 1. **Trabajadores/Empleados**

- [¿Qué es EmoCheck?](#-qué-es-emocheck)- Acceden para realizar autoevaluaciones

- [Usuarios del Sistema](#-usuarios-del-sistema)- Ven sus propios resultados

- [Módulos de Evaluación](#-módulos-de-evaluación)- Reciben recomendaciones personalizadas

- [Flujo de Usuario](#-flujo-de-usuario)- Acceden a recursos de bienestar

- [Panel de Administración](#-panel-de-administración)

- [Arquitectura Técnica](#-arquitectura-técnica)### 2. **Líderes HSE / Psicólogos Ocupacionales**

- [Base de Datos](#-base-de-datos)- Ven reportes agregados (no individuales, por confidencialidad)

- [Seguridad y Cumplimiento](#-seguridad-y-cumplimiento)- Gestionan alertas críticas

- [Integraciones](#-integraciones)- Dan seguimiento a casos de riesgo

- [Stack Tecnológico](#-stack-tecnológico)- Generan reportes por área/sede

- [Instalación](#-instalación)

- [Documentación](#-documentación)### 3. **Administradores del Sistema**

- Configuran la plataforma

---- Gestionan usuarios y empresas

- Acceden a todos los módulos

## 🎯 ¿Qué es EmoCheck?- Configuran integraciones



EmoCheck es una **plataforma web de evaluación y monitoreo de salud mental y bienestar emocional en el trabajo**. Permite a las empresas cumplir con normativas de salud ocupacional (SVE Psicosocial) mientras cuidan la salud mental de sus trabajadores de forma continua, confidencial y basada en evidencia científica.---



### 🌟 Características Principales## 📊 Módulos de Evaluación



✅ **Evaluaciones Científicas**: Instrumentos validados (GAD-7, PHQ-9, ISI)  ### **1. Salud Mental**

✅ **Semaforización Automática**: Verde/Amarillo/Rojo  Tamizaje de condiciones psicológicas comunes utilizando instrumentos científicos validados:

✅ **Alertas Inteligentes**: Notificaciones automáticas para casos críticos

✅ **Recomendaciones Personalizadas**: Recursos adaptados al resultado  - **Ansiedad (GAD-7)**: Generalized Anxiety Disorder-7

✅ **Dashboards Ejecutivos**: Visualización de tendencias y comparativos  - **Depresión (PHQ-9)**: Patient Health Questionnaire-9

✅ **Cumplimiento Legal**: Ley 1581/2012 y Resolución 2404/2019  - **Insomnio (ISI)**: Insomnia Severity Index

✅ **Confidencialidad Total**: Cifrado y anonimización de datos sensibles  - **Estrés Percibido**: Escala de percepción de estrés



---**Resultado**: Puntaje + semaforización (verde/amarillo/rojo) + recomendaciones personalizadas



## 👥 Usuarios del Sistema### **2. Fatiga Laboral**

Evaluación rápida de:

### 1. 👔 **Trabajadores/Empleados**- Nivel de energía cognitiva

- Realizan autoevaluaciones- Agotamiento emocional

- Ven sus resultados personales- Capacidad de concentración

- Reciben recomendaciones

- Acceden a recursos de bienestar**Objetivo**: Detectar burnout en etapas tempranas



### 2. 👨‍⚕️ **Líderes HSE / Psicólogos**### **3. Clima Organizacional**

- Ven reportes agregados (no individuales)Percepción del trabajador sobre:

- Gestionan alertas críticas- Entorno laboral

- Dan seguimiento a casos- Liderazgo

- Generan reportes por área/sede- Propósito y motivación

- Relaciones interpersonales

### 3. 👨‍💼 **Administradores**

- Configuran la plataforma### **4. Riesgo Psicosocial**

- Gestionan usuarios y empresasBasado en la **Batería del Ministerio del Trabajo** (Colombia):

- Acceden a todos los módulos- Factores intralaborales

- Configuran integraciones- Factores extralaborales

- Estrés laboral

---

**Cumplimiento legal**: Resolución 2404 de 2019

## 📊 Módulos de Evaluación

---

### 1. 🧠 **Salud Mental**

Tamizaje de condiciones psicológicas comunes:## 🔄 Flujo de Usuario

- **Ansiedad (GAD-7)**: Generalized Anxiety Disorder-7

- **Depresión (PHQ-9)**: Patient Health Questionnaire-9### **Paso 1: Bienvenida y Registro**

- **Insomnio (ISI)**: Insomnia Severity Index```

- **Estrés Percibido**: Escala validada científicamenteUsuario ingresa → Se registra → Acepta Consentimiento Informado Digital

```

**Resultado**: Puntaje + semaforización + recomendaciones- El consentimiento explica: qué se hará con sus datos, confidencialidad, propósito

- Se guarda digitalmente con trazabilidad (fecha, hora, IP)

### 2. ⚡ **Fatiga Laboral**

Evaluación de:### **Paso 2: Completar Perfil**

- Nivel de energía cognitiva```

- Agotamiento emocionalDatos del usuario:

- Capacidad de concentración- Nombre completo

- Documento de identidad

**Objetivo**: Detectar burnout temprano- Área

- Sede

### 3. 🏢 **Clima Organizacional**- Tipo de cargo

Percepción sobre:- Correo corporativo

- Entorno laboral```

- Liderazgo

- Propósito y motivación### **Paso 3: Realizar Evaluación**

- Relaciones interpersonales```

Selecciona módulo → Responde cuestionario → Sistema calcula resultado automáticamente

### 4. ⚠️ **Riesgo Psicosocial**```

Basado en **Batería Ministerio del Trabajo** (Colombia):- Las preguntas están estandarizadas (instrumentos validados científicamente)

- Factores intralaborales- Sistema asigna puntuación automática según algoritmos establecidos

- Factores extralaborales

- Estrés laboral### **Paso 4: Ver Resultados**

```

**Cumplimiento**: Resolución 2404 de 2019Resultado semaforizado:

🟢 Verde: Bienestar adecuado

---🟡 Amarillo: Atención preventiva

🔴 Rojo: Requiere intervención

## 🔄 Flujo de Usuario```



### Paso 1️⃣: Bienvenida y Registro### **Paso 5: Recomendaciones Personalizadas**

```Según el resultado, el sistema entrega:

Usuario ingresa → Se registra → Acepta Consentimiento Informado- Recursos de mindfulness

```- Pausas activas

- Consentimiento digital con trazabilidad completa- Ejercicios de respiración

- Explica uso de datos y confidencialidad- Recomendación de consulta psicológica (si aplica)



### Paso 2️⃣: Completar Perfil### **Paso 6: Centro de Recursos de Bienestar**

```Acceso permanente a:

Datos requeridos:- Calibración emocional

- Nombre completo- Mindfulness

- Documento de identidad- Neuropausas

- Área, Sede, Tipo de cargo- Apoyo profesional (solicitud de cita con psicólogo)

- Correo corporativo

```---



### Paso 3️⃣: Realizar Evaluación## 🔐 Panel de Administración (Backend)

```

Selecciona módulo → Responde cuestionario → Cálculo automático### **1. Gestión de Usuarios**

```- Crear/editar/eliminar usuarios

- Preguntas estandarizadas- Asignar a empresa, área, sede

- Puntuación automática- Ver estado: activo/inactivo

- Gestionar roles (trabajador, líder, admin)

### Paso 4️⃣: Ver Resultados

```### **2. Monitoreo de Resultados**

🟢 Verde: Bienestar adecuado

🟡 Amarillo: Atención preventiva**Tablero Visual con:**

🔴 Rojo: Requiere intervención- Indicadores globales por módulo

```- Filtros: fecha, área, sede, nivel de riesgo

- Gráficos de tendencias (ej: ¿aumentó la ansiedad este mes?)

### Paso 5️⃣: Recomendaciones- Comparativos entre áreas

- Recursos de mindfulness

- Pausas activas**Ejemplo de vista:**

- Ejercicios de respiración```

- Consulta psicológica (si aplica)Área: Producción (50 trabajadores)

-------------------------------------

### Paso 6️⃣: Centro de RecursosSalud Mental:

Acceso permanente a:  🟢 35 (70%)

- Calibración emocional  🟡 10 (20%)

- Mindfulness  🔴 5 (10%)  ⚠️ ALERTA

- Neuropausas```

- Apoyo profesional

### **3. Alertas Críticas**

---Cuando un trabajador sale en **rojo**:

- Se genera alerta automática

## 🔐 Panel de Administración- Notificación al psicólogo/HSE asignado

- Registro del seguimiento (?se contact��? ?qu�� acci��n se tom��?)

### 1. Gestión de Usuarios

- CRUD completo de usuarios**Importante**: El sistema NO muestra el nombre completo, usa ID o iniciales para proteger confidencialidad.

- Asignación por empresa/área/sede

- Gestión de roles y permisos### **4. Gesti��n de Consentimientos**

- Repositorio de todos los consentimientos firmados

### 2. Monitoreo de Resultados- Descarga de PDF por usuario

- Trazabilidad: qui��n acept��, cu��ndo, desde d��nde

**Dashboard con:**

- Indicadores globales por módulo### **5. Reportes Autom��ticos**

- Filtros: fecha, área, sede, riesgoGeneraci��n de informes con indicadores SVE Psicosocial:

- Gráficos de tendencias- N�� casos activos

- Comparativos entre áreas- N�� casos cerrados

- % de prevalencia/incidencia

**Ejemplo:**- % de participaci��n

```- Exportaci��n: Excel, PDF

Área: Producción (50 trabajadores)- Integraci��n con Power BI/Tableau

-------------------------------------

Salud Mental:---

  🟢 35 (70%)

  🟡 10 (20%)## ??? Arquitectura T��cnica

  🔴 5 (10%)  ⚠️ ALERTA

```### **Frontend: Angular 21 (Standalone)**

- Una sola aplicaci��n web responsive

### 3. Alertas Críticas- Dise?o modular (4 m��dulos de evaluaci��n)

Cuando un trabajador sale en rojo:- UX amigable con mensajes de acompa?amiento emocional

- ✅ Alerta automática- Semaforizaci��n visual clara

- ✅ Notificación a psicólogo/HSE- Gr��ficos interactivos para dashboards

- ✅ Registro de seguimiento

- ✅ Protección de confidencialidad (ID/iniciales)**Caracter��sticas:**

- Componentes standalone (sin NgModules)

### 4. Gestión de Consentimientos- Routing modular

- Repositorio digital- Estado global con Signals

- Descarga de PDFs- Guards para protecci��n de rutas

- Trazabilidad completa- Interceptors para autenticaci��n



### 5. Reportes Automáticos### **Backend: C# / .NET 8 (Arquitectura Hexagonal)**

Indicadores SVE Psicosocial:

- N° casos activos/cerrados**?Por qu�� hexagonal?**

- % prevalencia/incidenciaSeparaci��n clara de responsabilidades en capas:

- % participación

- Exportación: Excel, PDF```

- Integración Power BI/Tableau?? Domain (Dominio)

   - Entidades: Usuario, Evaluacion, Resultado

---   - Interfaces: IUsuarioRepository, IEvaluacionService

   - L��gica de negocio pura

## 🏗️ Arquitectura Técnica

?? Application (Aplicaci��n)

### Frontend: Angular 21 (Standalone)   - Casos de uso: CrearEvaluacion, CalcularResultado, GenerarReporte

- Aplicación web responsive   - DTOs: UsuarioDto, ResultadoDto

- Diseño modular (4 módulos)   - Servicios de aplicaci��n

- UX amigable con mensajes empáticos

- Semaforización visual?? Infrastructure (Infraestructura)

- Gráficos interactivos   - Repositorios: UsuarioRepository (SQL Server)

   - Servicios externos: EmailService, PowerBIService

**Características:**   - Autenticaci��n: JwtTokenService

- Componentes standalone   - Configuraciones

- Routing modular```

- Estado global con Signals

- Guards de protección**Seguridad:**

- Interceptors para auth- JWT + Refresh Tokens

- Roles y permisos (Claims-based)

### Backend: C# / .NET 8 (Hexagonal)- Encriptaci��n de datos sensibles

- HTTPS obligatorio

**Arquitectura en capas:**- Rate limiting

- CORS configurado

```

📁 Domain (Dominio)### **Base de Datos: SQL Server**

   - Entidades: Usuario, Evaluacion, Resultado

   - Interfaces: IUsuarioRepository, IEvaluacionService**Modelo de Datos - Tablas Principales:**

   - Lógica de negocio pura# ??? EmoCheck Database - SQL Server



📁 Application (Aplicación)## ?? Descripci��n General

   - Casos de uso: CrearEvaluacion, CalcularResultado

   - DTOs: UsuarioDto, ResultadoDtoBase de datos SQL Server para el proyecto **EmoCheck**, dise?ada con arquitectura normalizada, esquemas organizados y siguiendo las mejores pr��cticas de seguridad y auditor��a.

   - Servicios de aplicación

   ---

📁 Infrastructure (Infraestructura)

   - Repositorios: UsuarioRepository## ??? Arquitectura de Esquemas

   - Servicios externos: EmailService, PowerBIService

   - Autenticación: JwtTokenServiceLa base de datos est�� organizada en **6 esquemas principales**:

```

### 1. **dbo** (Default Schema)

**APIs REST:**Tablas maestras del sistema:

```http- `State` - Estados generales del sistema

POST   /api/auth/login- `Country` - Pa��ses

POST   /api/auth/refresh-token- `City` - Ciudades

POST   /api/usuarios/registro

GET    /api/evaluaciones/{usuarioId}### 2. **configuration** (Configuraci��n)

POST   /api/evaluaciones/salud-mentalConfiguraci��n organizacional:

GET    /api/resultados/{evaluacionId}- `Company` - Empresas/Organizaciones

GET    /api/dashboard/indicadores- `Site` - Sedes

POST   /api/dashboard/reportes/exportar- `Area` - ��reas/Departamentos

```- `JobType` - Tipos de cargo

- `Application` - Aplicaciones del sistema

**Seguridad:**

- ✅ JWT + Refresh Tokens### 3. **security** (Seguridad)

- ✅ Roles y permisos (Claims)Usuarios y autenticaci��n:

- ✅ Cifrado de datos sensibles- `User` - Usuarios del sistema

- ✅ HTTPS obligatorio- `Role` - Roles

- ✅ Rate limiting- `UserRole` - Relaci��n usuarios-roles

- ✅ CORS configurado- `RefreshToken` - Tokens JWT

- `PasswordResetToken` - Tokens de recuperaci��n

---- `InformedConsent` - Consentimientos informados



## 🗄️ Base de Datos### 4. **assessment** (Evaluaciones)

M��dulos de evaluaci��n:

### SQL Server 2022- `AssessmentModule` - M��dulos de evaluaci��n

- `Question` - Preguntas

**Esquemas organizados:**- `QuestionOption` - Opciones de respuesta

- `Evaluation` - Evaluaciones realizadas

1. **dbo**: Tablas maestras (State, Country, City)- `EvaluationResponse` - Respuestas

2. **configuration**: Empresas, Sedes, Áreas, Tipos de cargo

3. **security**: Usuarios, Roles, Tokens, Consentimientos### 5. **results** (Resultados)

4. **assessment**: Módulos, Preguntas, EvaluacionesResultados y alertas:

5. **results**: Resultados, Recomendaciones, Alertas- `EvaluationResult` - Resultados de evaluaciones

6. **resources**: Recursos de bienestar, Apoyo profesional- `DimensionScore` - Puntajes por dimensi��n

7. **audit**: Logs de auditoría, Sistema, Emails- `Recommendation` - Recomendaciones personalizadas

- `RecommendationType` - Tipos de recomendaci��n

**Total: 35+ tablas**- `Alert` - Alertas cr��ticas

- `CaseTracking` - Seguimiento de casos

### Características:

- ✅ Normalización (3FN)### 6. **resources** (Recursos)

- ✅ Foreign Keys para integridadRecursos de bienestar:

- ✅ Índices optimizados- `ResourceCategory` - Categor��as de recursos

- ✅ Campos CreatedAt/UpdatedAt- `WellnessResource` - Recursos de bienestar

- ✅ Soft delete (IsActive)- `UserResourceAccess` - Acceso a recursos

- ✅ Multi-tenant- `ProfessionalSupport` - Apoyo profesional

- `SupportRequest` - Solicitudes de apoyo

📄 **Ver documentación completa**: [Database/README.md](Database/README.md)

📊 **Ver diagrama ER**: [Database/ER_DIAGRAM.md](Database/ER_DIAGRAM.md)### 7. **audit** (Auditor��a)

Trazabilidad y logs:

---- `AuditLog` - Registro de auditor��a

- `SystemLog` - Logs del sistema

## 🔒 Seguridad y Cumplimiento- `EmailLog` - Emails enviados

- `DataExport` - Exportaciones de datos

### Ley 1581 de 2012 (Colombia)

✅ Consentimiento informado explícito  ---

✅ Finalidad clara del tratamiento

✅ Derecho de acceso/rectificación/eliminación  ## ?? Total de Tablas: 35+

✅ Cifrado y almacenamiento seguro

✅ Trazabilidad de aceptaciones  ---



### Confidencialidad Médica**Seguridad en DB:**

- Datos ultra sensibles- Cifrado AES-256 para campos sensibles (PasswordHash, Documento)

- Solo el usuario ve sus resultados- Anonimizaci��n en reportes (solo ID)

- Administradores: datos agregados/anonimizados- Backups autom��ticos diarios

- Cifrado AES-256- Logs de auditor��a (qui��n accedi�� a qu�� y cu��ndo)

- Logs de auditoría completos- ��ndices optimizados para consultas frecuentes

- Procedimientos almacenados para reportes complejos

### Trazabilidad

Registro de cada acción:---

```json

{## ?? Seguridad y Cumplimiento Legal

  "userId": 12345,

  "action": "Completed Mental Health Assessment",### **Ley 1581 de 2012 (Protecci��n de Datos Personales - Colombia)**

  "timestamp": "2026-01-20T10:30:45Z",? Consentimiento informado expl��cito

  "ip": "192.168.1.100",? Finalidad clara del tratamiento de datos

  "result": "Yellow"? Derecho de acceso, rectificaci��n y eliminaci��n

}? Cifrado y almacenamiento seguro

```? Trazabilidad de aceptaciones



---### **Confidencialidad M��dica**

- Los datos de salud son **ultra sensibles**

## 🔗 Integraciones- Solo el usuario ve sus resultados individuales

- Administradores ven datos **agregados** o **anonimizados**

### 1. Power BI / Tableau- En alertas cr��ticas: se usa ID, no nombre completo

- Dashboards avanzados- Separaci��n de datos personales y datos de salud

- Conexión directa a SQL Server

- Actualización en tiempo real### **Trazabilidad Total**

Cada acci��n se registra:

### 2. APIs de ARL```json

- Envío automático de reportes{

- Cumplimiento SST  "id": "12345",

- Formato XML/JSON  "usuario": "user@empresa.com",

  "accion": "Complet�� evaluaci��n de Salud Mental",

### 3. HR Tech / HRIS  "fecha": "2026-01-20T10:30:45Z",

- Importación masiva de usuarios  "ip": "192.168.1.100",

- Sincronización organizacional  "detalles": {

    "evaluacionId": "EVA-001",

### 4. Notificaciones    "modulo": "Salud Mental",

- Email (SMTP)    "resultado": "Amarillo"

- SMS (Twilio)  }

- Push notifications}

```

### 5. Exportación

- PDF, Excel, CSV, XML### **Medidas de Seguridad Implementadas**

- ? HTTPS obligatorio (TLS 1.3)

---- ? Autenticaci��n multifactor (opcional)

- ? Tokens con expiraci��n corta

## 📦 Stack Tecnológico- ? Refresh tokens almacenados de forma segura

- ? Validaci��n de inputs (prevenci��n XSS, SQL Injection)

### Backend- ? Rate limiting (prevenci��n DDoS)

- .NET 8 (C#)- ? Logs de accesos sospechosos

- Entity Framework Core 8- ? Aislamiento de datos por empresa

- ASP.NET Core Web API

- FluentValidation---

- AutoMapper

- MediatR (CQRS)## ?? Integraciones

- Serilog

- xUnit### **1. Power BI / Tableau**

- Dashboard avanzado con visualizaciones

### Frontend- Conexi��n directa a vistas de SQL Server

- Angular 21- Actualizaci��n en tiempo real

- TypeScript 5.x- Filtros interactivos por empresa/��rea/sede

- RxJS

- Chart.js### **2. APIs de ARL (Administradoras de Riesgos Laborales)**

- TailwindCSS / Angular Material- Env��o autom��tico de reportes agregados

- Cumplimiento de normativas SST

### Base de Datos- Formato XML o JSON seg��n requerimientos

- SQL Server 2022

- Redis (caché)### **3. HR Tech / HRIS (Human Resources Information System)**

- Importaci��n masiva de usuarios

### DevOps- Sincronizaci��n de cambios organizacionales

- Git / GitHub- Actualizaci��n autom��tica de ��reas/sedes

- Docker / Docker Compose

- CI/CD (GitHub Actions)### **4. Sistema de Notificaciones**

- Swagger / OpenAPI- Email (SMTP configurado)

- SMS (Twilio/similar)

---- Push notifications (para versi��n PWA futura)



## 🚀 Instalación### **5. Exportaci��n de Datos**

- PDF (reportes individuales y agregados)

### Prerrequisitos- Excel (tablas din��micas)

- .NET 8 SDK- CSV (para an��lisis externos)

- SQL Server 2019+- XML (para auditor��as y ARL)

- Node.js 20+

- Angular CLI 21---



### 1. Clonar el repositorio## ?? Resumen Ejecutivo

```bash

git clone https://github.com/CRISTIANROJAS1995/emocheck-api.git### **?Qu�� hace EmoCheck?**

cd emocheck-api

```1. ? **Eval��a** la salud mental y bienestar de trabajadores con cuestionarios cient��ficos

2. ? **Detecta** casos de riesgo con semaforizaci��n autom��tica

### 2. Crear base de datos3. ? **Alerta** a los responsables de HSE/psicolog��a cuando hay casos cr��ticos

```powershell4. ? **Recomienda** recursos personalizados seg��n el resultado

# Crear DB5. ? **Reporta** indicadores agregados para toma de decisiones

sqlcmd -S localhost -Q "CREATE DATABASE [EmoCheckDB]"6. ? **Cumple** con normativas legales de protecci��n de datos y salud ocupacional

7. ? **Integra** con herramientas externas (BI, ARL, HRIS)

# Ejecutar scripts

cd Database### **Beneficios Clave**

sqlcmd -S localhost -d EmoCheckDB -i 01_CREATE_SCHEMAS.sql

sqlcmd -S localhost -d EmoCheckDB -i 02_CREATE_MASTER_TABLES.sql**Para la Empresa:**

# ... ejecutar todos los scripts en orden- Cumplimiento legal (Resoluci��n 2404/2019)

```- Reducci��n de ausentismo

- Mejora del clima laboral

### 3. Configurar Backend- Datos para toma de decisiones

```bash- ROI medible

cd src/EmoCheck.API

dotnet restore**Para el Trabajador:**

dotnet build- Autoconocimiento de su salud mental

```- Acceso a recursos de bienestar

- Confidencialidad garantizada

**Configurar `appsettings.json`:**- Apoyo profesional cuando lo necesita

```json- Prevenci��n de condiciones graves

{

  "ConnectionStrings": {**Para el ��rea de HSE/RRHH:**

    "DefaultConnection": "Server=localhost;Database=EmoCheckDB;Trusted_Connection=true;"- Visibilidad de tendencias

  },- Alertas tempranas

  "JwtSettings": {- Reportes automatizados

    "SecretKey": "your-secret-key-here",- Seguimiento de casos

    "Issuer": "EmoCheckAPI",- Evidencia para auditor��as

    "Audience": "EmoCheckWeb",

    "ExpirationMinutes": 60---

  }

}## ?? Stack Tecnol��gico

```

### **Frontend**

### 4. Ejecutar Backend- Angular 21 (Standalone Components)

```bash- TypeScript 5.x

dotnet run- RxJS para programaci��n reactiva

```- Chart.js / ApexCharts para gr��ficos

- TailwindCSS / Angular Material para UI

API disponible en: `https://localhost:7001`- PWA capabilities (opcional)



### 5. Configurar Frontend### **Backend**

```bash- .NET 8 (C#)

cd ../EmoCheck.Web- Entity Framework Core 8

npm install- ASP.NET Core Web API

ng serve- FluentValidation

```- AutoMapper

- MediatR (CQRS pattern)

App disponible en: `http://localhost:4200`- Serilog (logging estructurado)

- xUnit (testing)

---

### **Base de Datos**

## 📚 Documentación- SQL Server 2022 / Azure SQL

- Redis (cach�� de sesiones)

- 📄 [Documentación del Proyecto](DOCUMENTACION_PROYECTO.md)

- 🗄️ [Documentación de Base de Datos](Database/README.md)### **DevOps**

- 📊 [Diagrama Entidad-Relación](Database/ER_DIAGRAM.md)- Git / GitHub

- 🔧 [Guía de API](docs/API_GUIDE.md) *(próximamente)*- Docker / Docker Compose

- 🎨 [Guía de UX/UI](docs/UX_GUIDE.md) *(próximamente)*- CI/CD (GitHub Actions / Azure DevOps)

- SonarQube (an��lisis de c��digo)

---- Swagger / OpenAPI (documentaci��n)



## 📞 Contacto### **Infraestructura (asumida por cliente)**

- Azure App Service / AWS EC2

**Repositorio**: https://github.com/CRISTIANROJAS1995/emocheck-api  - Azure SQL Database / AWS RDS

**Rama principal**: main  - Azure Blob Storage / AWS S3

**Fecha de inicio**: Enero 20, 2026  - Azure Application Insights / CloudWatch

- CDN para assets est��ticos

---- SSL/TLS certificates


## 📄 Licencia

Este proyecto es confidencial y propiedad del proyecto EmoCheck.
Todos los derechos reservados © 2026

---

**Última actualización**: 2026-01-20
**Versión**: 1.0.0
**Autor**: Cristian Rojas
