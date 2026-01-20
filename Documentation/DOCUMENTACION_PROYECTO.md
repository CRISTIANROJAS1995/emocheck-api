# 📋 Documentación del Proyecto EmoCheck

**Fecha:** Enero 20, 2026  
**Versión:** 1.0  
**Repositorio:** emocheck-api  
**Owner:** CRISTIANROJAS1995

---

## 🎯 ¿Qué es EmoCheck?

EmoCheck es una **plataforma web de evaluación y monitoreo de salud mental y bienestar emocional en el trabajo**. Permite a las empresas cumplir con normativas de salud ocupacional (SVE Psicosocial) mientras cuidan la salud mental de sus trabajadores de forma continua, confidencial y basada en evidencia científica.

---

## 👥 Usuarios del Sistema

### **3 Tipos de Usuarios:**

### 1. **Trabajadores/Empleados**
- Acceden para realizar autoevaluaciones
- Ven sus propios resultados
- Reciben recomendaciones personalizadas
- Acceden a recursos de bienestar

### 2. **Líderes HSE / Psicólogos Ocupacionales**
- Ven reportes agregados (no individuales, por confidencialidad)
- Gestionan alertas críticas
- Dan seguimiento a casos de riesgo
- Generan reportes por área/sede

### 3. **Administradores del Sistema**
- Configuran la plataforma
- Gestionan usuarios y empresas
- Acceden a todos los módulos
- Configuran integraciones

---

## 📊 Módulos de Evaluación

### **1. Salud Mental**
Tamizaje de condiciones psicológicas comunes utilizando instrumentos científicos validados:

- **Ansiedad (GAD-7)**: Generalized Anxiety Disorder-7
- **Depresión (PHQ-9)**: Patient Health Questionnaire-9
- **Insomnio (ISI)**: Insomnia Severity Index
- **Estrés Percibido**: Escala de percepción de estrés

**Resultado**: Puntaje + semaforización (verde/amarillo/rojo) + recomendaciones personalizadas

### **2. Fatiga Laboral**
Evaluación rápida de:
- Nivel de energía cognitiva
- Agotamiento emocional
- Capacidad de concentración

**Objetivo**: Detectar burnout en etapas tempranas

### **3. Clima Organizacional**
Percepción del trabajador sobre:
- Entorno laboral
- Liderazgo
- Propósito y motivación
- Relaciones interpersonales

### **4. Riesgo Psicosocial**
Basado en la **Batería del Ministerio del Trabajo** (Colombia):
- Factores intralaborales
- Factores extralaborales
- Estrés laboral

**Cumplimiento legal**: Resolución 2404 de 2019

---

## 🔄 Flujo de Usuario

### **Paso 1: Bienvenida y Registro**
```
Usuario ingresa → Se registra → Acepta Consentimiento Informado Digital
```
- El consentimiento explica: qué se hará con sus datos, confidencialidad, propósito
- Se guarda digitalmente con trazabilidad (fecha, hora, IP)

### **Paso 2: Completar Perfil**
```
Datos del usuario:
- Nombre completo
- Documento de identidad
- Área
- Sede
- Tipo de cargo
- Correo corporativo
```

### **Paso 3: Realizar Evaluación**
```
Selecciona módulo → Responde cuestionario → Sistema calcula resultado automáticamente
```
- Las preguntas están estandarizadas (instrumentos validados científicamente)
- Sistema asigna puntuación automática según algoritmos establecidos

### **Paso 4: Ver Resultados**
```
Resultado semaforizado:
🟢 Verde: Bienestar adecuado
🟡 Amarillo: Atención preventiva
🔴 Rojo: Requiere intervención
```

### **Paso 5: Recomendaciones Personalizadas**
Según el resultado, el sistema entrega:
- Recursos de mindfulness
- Pausas activas
- Ejercicios de respiración
- Recomendación de consulta psicológica (si aplica)

### **Paso 6: Centro de Recursos de Bienestar**
Acceso permanente a:
- Calibración emocional
- Mindfulness
- Neuropausas
- Apoyo profesional (solicitud de cita con psicólogo)

---

## 🔐 Panel de Administración (Backend)

### **1. Gestión de Usuarios**
- Crear/editar/eliminar usuarios
- Asignar a empresa, área, sede
- Ver estado: activo/inactivo
- Gestionar roles (trabajador, líder, admin)

### **2. Monitoreo de Resultados**

**Tablero Visual con:**
- Indicadores globales por módulo
- Filtros: fecha, área, sede, nivel de riesgo
- Gráficos de tendencias (ej: ¿aumentó la ansiedad este mes?)
- Comparativos entre áreas

**Ejemplo de vista:**
```
Área: Producción (50 trabajadores)
-------------------------------------
Salud Mental:
  🟢 35 (70%)
  🟡 10 (20%)
  🔴 5 (10%)  ⚠️ ALERTA
```

### **3. Alertas Críticas**
Cuando un trabajador sale en **rojo**:
- Se genera alerta automática
- Notificación al psicólogo/HSE asignado
- Registro del seguimiento (¿se contactó? ¿qué acción se tomó?)

**Importante**: El sistema NO muestra el nombre completo, usa ID o iniciales para proteger confidencialidad.

### **4. Gestión de Consentimientos**
- Repositorio de todos los consentimientos firmados
- Descarga de PDF por usuario
- Trazabilidad: quién aceptó, cuándo, desde dónde

### **5. Reportes Automáticos**
Generación de informes con indicadores SVE Psicosocial:
- N° casos activos
- N° casos cerrados
- % de prevalencia/incidencia
- % de participación
- Exportación: Excel, PDF
- Integración con Power BI/Tableau

---

## 🏗️ Arquitectura Técnica

### **Frontend: Angular 21 (Standalone)**
- Una sola aplicación web responsive
- Diseño modular (4 módulos de evaluación)
- UX amigable con mensajes de acompañamiento emocional
- Semaforización visual clara
- Gráficos interactivos para dashboards

**Características:**
- Componentes standalone (sin NgModules)
- Routing modular
- Estado global con Signals
- Guards para protección de rutas
- Interceptors para autenticación

### **Backend: C# / .NET 8 (Arquitectura Hexagonal)**

**¿Por qué hexagonal?**  
Separación clara de responsabilidades en capas:

```
📁 Domain (Dominio)
   - Entidades: Usuario, Evaluacion, Resultado
   - Interfaces: IUsuarioRepository, IEvaluacionService
   - Lógica de negocio pura
   
📁 Application (Aplicación)
   - Casos de uso: CrearEvaluacion, CalcularResultado, GenerarReporte
   - DTOs: UsuarioDto, ResultadoDto
   - Servicios de aplicación
   
📁 Infrastructure (Infraestructura)
   - Repositorios: UsuarioRepository (SQL Server)
   - Servicios externos: EmailService, PowerBIService
   - Autenticación: JwtTokenService
   - Configuraciones
```

**APIs REST Principales:**
```http
# Autenticación
POST /api/auth/login
POST /api/auth/refresh-token
POST /api/auth/logout

# Usuarios
POST /api/usuarios/registro
GET /api/usuarios/{id}
PUT /api/usuarios/{id}
DELETE /api/usuarios/{id}

# Consentimientos
POST /api/consentimientos
GET /api/consentimientos/{usuarioId}

# Evaluaciones
POST /api/evaluaciones/salud-mental
POST /api/evaluaciones/fatiga-laboral
POST /api/evaluaciones/clima-organizacional
POST /api/evaluaciones/riesgo-psicosocial
GET /api/evaluaciones/{usuarioId}

# Resultados
GET /api/resultados/{usuarioId}
GET /api/resultados/{evaluacionId}/detalle

# Dashboard Administrativo
GET /api/dashboard/indicadores
GET /api/dashboard/alertas
GET /api/dashboard/reportes
POST /api/dashboard/reportes/exportar

# Gestión Administrativa
GET /api/admin/usuarios
GET /api/admin/empresas
GET /api/admin/areas
GET /api/admin/sedes
```

**Seguridad:**
- JWT + Refresh Tokens
- Roles y permisos (Claims-based)
- Encriptación de datos sensibles
- HTTPS obligatorio
- Rate limiting
- CORS configurado

### **Base de Datos: SQL Server**

**Modelo de Datos - Tablas Principales:**

```sql
-- Gestión de Organizaciones
Empresas (Id, Nombre, NIT, RazonSocial, Activa, FechaCreacion)
Areas (Id, Nombre, EmpresaId, Descripcion)
Sedes (Id, Nombre, Ciudad, Direccion, EmpresaId)

-- Gestión de Usuarios
Usuarios (
    Id, 
    NombreCompleto, 
    Documento, 
    Email, 
    PasswordHash,
    AreaId, 
    SedeId, 
    RolId, 
    TipoCargoId,
    Activo,
    FechaRegistro
)

Roles (Id, Nombre, Descripcion)
TiposCargo (Id, Nombre)

-- Consentimiento Informado
Consentimientos (
    Id, 
    UsuarioId, 
    Aceptado, 
    FechaHora, 
    IP,
    UserAgent,
    DocumentoPDF
)

-- Evaluaciones
Modulos (Id, Nombre, Descripcion, TipoInstrumento, PuntajeMax)
Preguntas (Id, ModuloId, TextoPregunta, Orden, TipoRespuesta)
OpcionesRespuesta (Id, PreguntaId, TextoOpcion, Valor, Orden)

Evaluaciones (
    Id, 
    UsuarioId, 
    ModuloId, 
    FechaInicio, 
    FechaFin, 
    Estado, 
    Completada
)

Respuestas (
    Id, 
    EvaluacionId, 
    PreguntaId, 
    OpcionRespuestaId,
    ValorRespuesta,
    FechaRespuesta
)

-- Resultados y Alertas
Resultados (
    Id, 
    EvaluacionId, 
    PuntajeTotal, 
    Nivel,  -- Verde/Amarillo/Rojo
    Fecha,
    Observaciones
)

Recomendaciones (
    Id, 
    ResultadoId, 
    Texto, 
    Tipo,  -- Mindfulness, PausaActiva, ConsultaPsicologica
    Prioridad
)

Alertas (
    Id, 
    UsuarioId, 
    ResultadoId,
    Nivel,  -- Crítico, Alto, Medio
    Atendida, 
    ResponsableId,
    FechaCreacion,
    FechaAtencion,
    Observaciones
)

-- Auditoría y Trazabilidad
AuditoriaLogs (
    Id, 
    UsuarioId, 
    Accion, 
    Entidad,
    EntidadId,
    FechaHora, 
    IP,
    Detalles
)
```

**Seguridad en DB:**
- Cifrado AES-256 para campos sensibles (PasswordHash, Documento)
- Anonimización en reportes (solo ID)
- Backups automáticos diarios
- Logs de auditoría (quién accedió a qué y cuándo)
- Índices optimizados para consultas frecuentes
- Procedimientos almacenados para reportes complejos

---

## 🔒 Seguridad y Cumplimiento Legal

### **Ley 1581 de 2012 (Protección de Datos Personales - Colombia)**
✅ Consentimiento informado explícito  
✅ Finalidad clara del tratamiento de datos  
✅ Derecho de acceso, rectificación y eliminación  
✅ Cifrado y almacenamiento seguro  
✅ Trazabilidad de aceptaciones  

### **Confidencialidad Médica**
- Los datos de salud son **ultra sensibles**
- Solo el usuario ve sus resultados individuales
- Administradores ven datos **agregados** o **anonimizados**
- En alertas críticas: se usa ID, no nombre completo
- Separación de datos personales y datos de salud

### **Trazabilidad Total**
Cada acción se registra:
```json
{
  "id": "12345",
  "usuario": "user@empresa.com",
  "accion": "Completó evaluación de Salud Mental",
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
- ✅ HTTPS obligatorio (TLS 1.3)
- ✅ Autenticación multifactor (opcional)
- ✅ Tokens con expiración corta
- ✅ Refresh tokens almacenados de forma segura
- ✅ Validación de inputs (prevención XSS, SQL Injection)
- ✅ Rate limiting (prevención DDoS)
- ✅ Logs de accesos sospechosos
- ✅ Aislamiento de datos por empresa

---

## 🔗 Integraciones

### **1. Power BI / Tableau**
- Dashboard avanzado con visualizaciones
- Conexión directa a vistas de SQL Server
- Actualización en tiempo real
- Filtros interactivos por empresa/área/sede

### **2. APIs de ARL (Administradoras de Riesgos Laborales)**
- Envío automático de reportes agregados
- Cumplimiento de normativas SST
- Formato XML o JSON según requerimientos

### **3. HR Tech / HRIS (Human Resources Information System)**
- Importación masiva de usuarios
- Sincronización de cambios organizacionales
- Actualización automática de áreas/sedes

### **4. Sistema de Notificaciones**
- Email (SMTP configurado)
- SMS (Twilio/similar)
- Push notifications (para versión PWA futura)

### **5. Exportación de Datos**
- PDF (reportes individuales y agregados)
- Excel (tablas dinámicas)
- CSV (para análisis externos)
- XML (para auditorías y ARL)

---

## 🎨 Experiencia de Usuario (UX)

### **Mensajes de Acompañamiento Emocional**
El sistema usa lenguaje empático y cercano:

- ✨ **Bienvenida**: "Hola María, personas sanas, organizaciones fuertes"
- 🧘 **Reflexión**: "Tu bienestar también es parte de la productividad"
- 💚 **Alerta**: "Tu cuerpo está en alerta. Vamos a bajarle el ritmo juntos"
- 🎯 **Motivación**: "Recuerda: conocerte es el primer paso para cuidarte"

### **Elementos Clave de UX:**

#### **Pausas Guiadas**
- Ejercicios de respiración de 2 minutos
- Animaciones visuales relajantes
- Audio opcional (voz guía)

#### **Resultados Explicados**
- Lenguaje sencillo (sin tecnicismos)
- Gráficos visuales claros
- Explicación del puntaje
- Comparación con evaluaciones anteriores

#### **Recomendaciones Accionables**
- No solo teoría, sino pasos concretos
- Videos cortos (2-3 minutos)
- Recursos descargables
- Enlaces a herramientas externas

#### **Acceso Inmediato a Ayuda**
- Botón de "Necesito hablar con alguien"
- Chat o formulario de contacto
- Líneas de emergencia visibles

### **Semaforización Visual**

```
🟢 VERDE - Bienestar Adecuado
   "¡Excelente! Continúa con tus hábitos saludables"
   
🟡 AMARILLO - Atención Preventiva
   "Algunos aspectos necesitan atención. Te recomendamos..."
   
🔴 ROJO - Requiere Intervención
   "Tu bienestar necesita apoyo profesional. Contáctanos ahora"
```

---

## 📈 Indicadores SVE Psicosocial

El sistema calcula automáticamente:

### **1. Prevalencia**
Porcentaje de trabajadores con condición actual
```
Prevalencia = (Casos actuales / Total trabajadores) × 100
```

### **2. Incidencia**
Porcentaje de casos nuevos en un período
```
Incidencia = (Casos nuevos en período / Total trabajadores) × 100
```

### **3. Participación**
Porcentaje de trabajadores que completaron evaluación
```
Participación = (Evaluaciones completadas / Total trabajadores) × 100
```

### **4. Efectividad de Intervenciones**
Comparación antes/después de implementar acciones
```
Mejora = ((Puntaje después - Puntaje antes) / Puntaje antes) × 100
```

### **5. Distribución por Nivel de Riesgo**
Porcentajes por área/sede:
```
Área X:
- Verde: 70%
- Amarillo: 20%
- Rojo: 10%
```

---

## 🚀 Resumen Ejecutivo

### **¿Qué hace EmoCheck?**

1. ✅ **Evalúa** la salud mental y bienestar de trabajadores con cuestionarios científicos
2. ✅ **Detecta** casos de riesgo con semaforización automática
3. ✅ **Alerta** a los responsables de HSE/psicología cuando hay casos críticos
4. ✅ **Recomienda** recursos personalizados según el resultado
5. ✅ **Reporta** indicadores agregados para toma de decisiones
6. ✅ **Cumple** con normativas legales de protección de datos y salud ocupacional
7. ✅ **Integra** con herramientas externas (BI, ARL, HRIS)

### **Beneficios Clave**

**Para la Empresa:**
- Cumplimiento legal (Resolución 2404/2019)
- Reducción de ausentismo
- Mejora del clima laboral
- Datos para toma de decisiones
- ROI medible

**Para el Trabajador:**
- Autoconocimiento de su salud mental
- Acceso a recursos de bienestar
- Confidencialidad garantizada
- Apoyo profesional cuando lo necesita
- Prevención de condiciones graves

**Para el Área de HSE/RRHH:**
- Visibilidad de tendencias
- Alertas tempranas
- Reportes automatizados
- Seguimiento de casos
- Evidencia para auditorías

---

## 📦 Stack Tecnológico

### **Frontend**
- Angular 21 (Standalone Components)
- TypeScript 5.x
- RxJS para programación reactiva
- Chart.js / ApexCharts para gráficos
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
- Redis (caché de sesiones)

### **DevOps**
- Git / GitHub
- Docker / Docker Compose
- CI/CD (GitHub Actions / Azure DevOps)
- SonarQube (análisis de código)
- Swagger / OpenAPI (documentación)

### **Infraestructura (asumida por cliente)**
- Azure App Service / AWS EC2
- Azure SQL Database / AWS RDS
- Azure Blob Storage / AWS S3
- Azure Application Insights / CloudWatch
- CDN para assets estáticos
- SSL/TLS certificates

---

## 📝 Próximos Pasos

### **Fase 1: Planificación (2 semanas)**
- [ ] Definir requerimientos funcionales detallados
- [ ] Diseñar modelo de datos completo
- [ ] Crear wireframes de pantallas clave
- [ ] Definir arquitectura de integración

### **Fase 2: Desarrollo Backend (6 semanas)**
- [ ] Setup proyecto .NET 8 con arquitectura hexagonal
- [ ] Implementar autenticación y autorización
- [ ] Desarrollar APIs de módulo de Salud Mental
- [ ] Implementar sistema de alertas
- [ ] Crear reportes y dashboards API

### **Fase 3: Desarrollo Frontend (6 semanas)**
- [ ] Setup proyecto Angular 21
- [ ] Implementar flujo de registro y consentimiento
- [ ] Desarrollar módulo de evaluaciones
- [ ] Crear dashboard administrativo
- [ ] Implementar centro de recursos

### **Fase 4: Integración y Testing (3 semanas)**
- [ ] Integración frontend-backend
- [ ] Testing E2E
- [ ] Testing de seguridad
- [ ] Optimización de performance
- [ ] Documentación técnica

### **Fase 5: Despliegue (1 semana)**
- [ ] Setup de infraestructura
- [ ] Configuración de CI/CD
- [ ] Migración de datos (si aplica)
- [ ] Capacitación a administradores
- [ ] Go-live

---

## 📞 Contacto Técnico

**Repositorio:** https://github.com/CRISTIANROJAS1995/emocheck-api  
**Rama principal:** main  
**Fecha de inicio:** Enero 20, 2026  

---

## 📄 Licencia y Confidencialidad

Este documento es confidencial y propiedad del proyecto EmoCheck.  
Todos los derechos reservados © 2026

---

**Última actualización:** 2026-01-20  
**Versión del documento:** 1.0  
**Autor:** GitHub Copilot (AI Assistant)
