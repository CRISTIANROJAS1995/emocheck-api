-- =============================================
-- EmoCheck Database - Schema Creation
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Create schemas for database organization
-- =============================================

USE [EmoCheckDB]
GO

-- Create schemas
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'security')
BEGIN
    EXEC('CREATE SCHEMA [security]')
    PRINT 'Schema [security] created successfully'
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'configuration')
BEGIN
    EXEC('CREATE SCHEMA [configuration]')
    PRINT 'Schema [configuration] created successfully'
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'assessment')
BEGIN
    EXEC('CREATE SCHEMA [assessment]')
    PRINT 'Schema [assessment] created successfully'
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'results')
BEGIN
    EXEC('CREATE SCHEMA [results]')
    PRINT 'Schema [results] created successfully'
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'resources')
BEGIN
    EXEC('CREATE SCHEMA [resources]')
    PRINT 'Schema [resources] created successfully'
END
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'audit')
BEGIN
    EXEC('CREATE SCHEMA [audit]')
    PRINT 'Schema [audit] created successfully'
END
GO

PRINT 'All schemas created successfully!'
GO
