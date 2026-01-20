-- =============================================
-- EmoCheck Database - Initial Data (Seeds)
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Insert initial data for the system
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Insert States
-- =============================================
SET IDENTITY_INSERT [dbo].[State] ON
GO

INSERT INTO [dbo].[State] ([StateID], [Name], [Description], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Active', 'Active state', 1, GETDATE(), GETDATE()),
(2, 'Inactive', 'Inactive state', 1, GETDATE(), GETDATE()),
(3, 'Pending', 'Pending state', 1, GETDATE(), GETDATE()),
(4, 'Completed', 'Completed state', 1, GETDATE(), GETDATE()),
(5, 'Cancelled', 'Cancelled state', 1, GETDATE(), GETDATE()),
(6, 'InProgress', 'In Progress state', 1, GETDATE(), GETDATE()),
(7, 'Archived', 'Archived state', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [dbo].[State] OFF
GO

-- =============================================
-- Insert Countries
-- =============================================
SET IDENTITY_INSERT [dbo].[Country] ON
GO

INSERT INTO [dbo].[Country] ([CountryID], [Name], [Code], [PhoneCode], [Currency], [TimeZone], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Colombia', 'CO', '+57', 'COP', 'America/Bogota', 1, GETDATE(), GETDATE()),
(2, 'México', 'MX', '+52', 'MXN', 'America/Mexico_City', 1, GETDATE(), GETDATE()),
(3, 'Estados Unidos', 'US', '+1', 'USD', 'America/New_York', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [dbo].[Country] OFF
GO

-- =============================================
-- Insert Cities (Colombia)
-- =============================================
SET IDENTITY_INSERT [dbo].[City] ON
GO

INSERT INTO [dbo].[City] ([CityID], [CountryID], [Name], [Code], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 1, 'Bogotá', 'BOG', 1, GETDATE(), GETDATE()),
(2, 1, 'Medellín', 'MDE', 1, GETDATE(), GETDATE()),
(3, 1, 'Cali', 'CLO', 1, GETDATE(), GETDATE()),
(4, 1, 'Barranquilla', 'BAQ', 1, GETDATE(), GETDATE()),
(5, 1, 'Cartagena', 'CTG', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [dbo].[City] OFF
GO

-- =============================================
-- Insert Application
-- =============================================
SET IDENTITY_INSERT [configuration].[Application] ON
GO

INSERT INTO [configuration].[Application] ([ApplicationID], [Name], [Code], [Description], [Version], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'EmoCheck Web', 'EMOCHECK_WEB', 'EmoCheck Web Application', '1.0.0', 1, GETDATE(), GETDATE()),
(2, 'EmoCheck Admin', 'EMOCHECK_ADMIN', 'EmoCheck Admin Panel', '1.0.0', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [configuration].[Application] OFF
GO

-- =============================================
-- Insert Job Types
-- =============================================
SET IDENTITY_INSERT [configuration].[JobType] ON
GO

INSERT INTO [configuration].[JobType] ([JobTypeID], [Name], [Description], [Level], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Operativo', 'Cargo operativo', 'Operativo', 1, GETDATE(), GETDATE()),
(2, 'Administrativo', 'Cargo administrativo', 'Administrativo', 1, GETDATE(), GETDATE()),
(3, 'Coordinador', 'Cargo de coordinación', 'Táctico', 1, GETDATE(), GETDATE()),
(4, 'Jefe', 'Cargo de jefatura', 'Táctico', 1, GETDATE(), GETDATE()),
(5, 'Gerente', 'Cargo gerencial', 'Estratégico', 1, GETDATE(), GETDATE()),
(6, 'Director', 'Cargo de dirección', 'Estratégico', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [configuration].[JobType] OFF
GO

-- =============================================
-- Insert Roles
-- =============================================
SET IDENTITY_INSERT [security].[Role] ON
GO

INSERT INTO [security].[Role] ([RoleID], [Name], [Code], [Description], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Employee', 'EMPLOYEE', 'Regular employee user', 1, GETDATE(), GETDATE()),
(2, 'HSE Leader', 'HSE_LEADER', 'Health, Safety and Environment leader', 1, GETDATE(), GETDATE()),
(3, 'Psychologist', 'PSYCHOLOGIST', 'Occupational psychologist', 1, GETDATE(), GETDATE()),
(4, 'Company Admin', 'COMPANY_ADMIN', 'Company administrator', 1, GETDATE(), GETDATE()),
(5, 'System Admin', 'SYSTEM_ADMIN', 'System administrator', 1, GETDATE(), GETDATE()),
(6, 'ARL Admin', 'ARL_ADMIN', 'ARL administrator', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [security].[Role] OFF
GO

-- =============================================
-- Insert Assessment Modules
-- =============================================
SET IDENTITY_INSERT [assessment].[AssessmentModule] ON
GO

INSERT INTO [assessment].[AssessmentModule] 
([AssessmentModuleID], [StateID], [Name], [Code], [Description], [InstrumentType], [Category], [MaxScore], [MinScore], [Icon], [Color], [EstimatedMinutes], [DisplayOrder], [IsActive], [CreatedAt], [UpdatedAt]) 
VALUES
(1, 1, 'Salud Mental', 'MENTAL_HEALTH', 'Tamizaje de ansiedad, depresión, insomnio y estrés percibido', 'GAD-7, PHQ-9, ISI', 'MentalHealth', 100, 0, 'brain-icon', '#4A90E2', 10, 1, 1, GETDATE(), GETDATE()),
(2, 1, 'Fatiga Laboral', 'WORK_FATIGUE', 'Evaluación rápida de energía cognitiva y emocional', 'Custom', 'WorkFatigue', 100, 0, 'battery-icon', '#7ED321', 5, 2, 1, GETDATE(), GETDATE()),
(3, 1, 'Clima Organizacional', 'ORG_CLIMATE', 'Percepción del entorno, liderazgo y propósito', 'Custom', 'OrganizationalClimate', 100, 0, 'team-icon', '#50E3C2', 8, 3, 1, GETDATE(), GETDATE()),
(4, 1, 'Riesgo Psicosocial', 'PSYCHOSOCIAL_RISK', 'Batería Ministerio del Trabajo Colombia', 'Min Trabajo', 'PsychosocialRisk', 100, 0, 'alert-icon', '#F5A623', 15, 4, 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [assessment].[AssessmentModule] OFF
GO

-- =============================================
-- Insert Recommendation Types
-- =============================================
SET IDENTITY_INSERT [results].[RecommendationType] ON
GO

INSERT INTO [results].[RecommendationType] ([RecommendationTypeID], [Name], [Code], [Description], [Icon], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 'Mindfulness', 'MINDFULNESS', 'Ejercicios de meditación y respiración consciente', 'lotus-icon', 1, GETDATE(), GETDATE()),
(2, 'Pausa Activa', 'ACTIVE_BREAK', 'Pausas activas y ejercicios físicos', 'stretch-icon', 1, GETDATE(), GETDATE()),
(3, 'Consulta Psicológica', 'PSYCH_CONSULT', 'Recomendación de consulta con psicólogo', 'psychologist-icon', 1, GETDATE(), GETDATE()),
(4, 'Neuropausa', 'NEUROPAUSE', 'Descansos para recuperación cognitiva', 'brain-rest-icon', 1, GETDATE(), GETDATE()),
(5, 'Ejercicio Físico', 'EXERCISE', 'Rutinas de ejercicio físico', 'fitness-icon', 1, GETDATE(), GETDATE()),
(6, 'Nutrición', 'NUTRITION', 'Recomendaciones nutricionales', 'food-icon', 1, GETDATE(), GETDATE()),
(7, 'Sueño', 'SLEEP', 'Higiene del sueño y descanso', 'sleep-icon', 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [results].[RecommendationType] OFF
GO

-- =============================================
-- Insert Resource Categories
-- =============================================
SET IDENTITY_INSERT [resources].[ResourceCategory] ON
GO

INSERT INTO [resources].[ResourceCategory] ([ResourceCategoryID], [StateID], [Name], [Code], [Description], [Icon], [Color], [DisplayOrder], [IsActive], [CreatedAt], [UpdatedAt]) VALUES
(1, 1, 'Calibración Emocional', 'EMOTIONAL_CAL', 'Herramientas para calibrar emociones', 'heart-icon', '#E74C3C', 1, 1, GETDATE(), GETDATE()),
(2, 1, 'Mindfulness', 'MINDFULNESS', 'Ejercicios de meditación y atención plena', 'meditation-icon', '#9B59B6', 2, 1, GETDATE(), GETDATE()),
(3, 1, 'Neuropausas', 'NEUROPAUSE', 'Pausas para recuperación cognitiva', 'brain-icon', '#3498DB', 3, 1, GETDATE(), GETDATE()),
(4, 1, 'Apoyo Profesional', 'PROFESSIONAL', 'Contacto con profesionales de salud mental', 'support-icon', '#2ECC71', 4, 1, GETDATE(), GETDATE())
GO

SET IDENTITY_INSERT [resources].[ResourceCategory] OFF
GO

PRINT 'Initial data inserted successfully!'
PRINT '================================================'
PRINT 'Database EmoCheck created and configured!'
PRINT '================================================'
PRINT 'Schemas created: 6'
PRINT 'Tables created: 35+'
PRINT 'Initial records inserted'
PRINT '================================================'
GO
