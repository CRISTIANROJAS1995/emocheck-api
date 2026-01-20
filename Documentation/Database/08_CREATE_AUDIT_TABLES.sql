-- =============================================
-- EmoCheck Database - Audit Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Audit and logging tables
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: AuditLog (Registro de auditoría)
-- =============================================
CREATE TABLE [audit].[AuditLog](
	[AuditLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Action] [varchar](100) NOT NULL,
	[Entity] [varchar](100) NULL, -- User, Evaluation, Alert, etc.
	[EntityID] [int] NULL,
	[OldValues] [nvarchar](max) NULL, -- JSON con valores anteriores
	[NewValues] [nvarchar](max) NULL, -- JSON con valores nuevos
	[IPAddress] [varchar](50) NULL,
	[UserAgent] [varchar](500) NULL,
	[Timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuditLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [audit].[AuditLog] ADD DEFAULT (GETDATE()) FOR [Timestamp]
GO

ALTER TABLE [audit].[AuditLog] WITH CHECK ADD CONSTRAINT [FK_AuditLog_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [audit].[AuditLog] CHECK CONSTRAINT [FK_AuditLog_User]
GO

-- Índice para búsquedas por usuario
CREATE NONCLUSTERED INDEX [IX_AuditLog_UserID] ON [audit].[AuditLog]
(
	[UserID] ASC
)
INCLUDE ([Action], [Entity], [Timestamp])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- Índice para búsquedas por fecha
CREATE NONCLUSTERED INDEX [IX_AuditLog_Timestamp] ON [audit].[AuditLog]
(
	[Timestamp] DESC
)
INCLUDE ([UserID], [Action], [Entity])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- =============================================
-- Table: SystemLog (Logs del sistema)
-- =============================================
CREATE TABLE [audit].[SystemLog](
	[SystemLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [varchar](20) NOT NULL, -- Information, Warning, Error, Critical
	[Source] [varchar](200) NULL, -- API endpoint, Service name
	[Message] [nvarchar](2000) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[StackTrace] [nvarchar](max) NULL,
	[AdditionalData] [nvarchar](max) NULL, -- JSON con datos adicionales
	[Timestamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SystemLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [audit].[SystemLog] ADD DEFAULT (GETDATE()) FOR [Timestamp]
GO

-- Índice para búsquedas por nivel y fecha
CREATE NONCLUSTERED INDEX [IX_SystemLog_Level_Timestamp] ON [audit].[SystemLog]
(
	[Level] ASC,
	[Timestamp] DESC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

-- =============================================
-- Table: EmailLog (Registro de emails enviados)
-- =============================================
CREATE TABLE [audit].[EmailLog](
	[EmailLogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ToEmail] [varchar](150) NOT NULL,
	[Subject] [varchar](500) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[EmailType] [varchar](100) NULL, -- Welcome, Alert, Reminder, PasswordReset
	[Status] [varchar](50) NOT NULL, -- Sent, Failed, Pending
	[ErrorMessage] [nvarchar](1000) NULL,
	[SentAt] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmailLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [audit].[EmailLog] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [audit].[EmailLog] WITH CHECK ADD CONSTRAINT [FK_EmailLog_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [audit].[EmailLog] CHECK CONSTRAINT [FK_EmailLog_User]
GO

-- =============================================
-- Table: DataExport (Registro de exportaciones)
-- =============================================
CREATE TABLE [audit].[DataExport](
	[DataExportID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CompanyID] [int] NULL,
	[ExportType] [varchar](100) NOT NULL, -- Report, Dashboard, RawData
	[Format] [varchar](50) NOT NULL, -- PDF, Excel, CSV
	[FileSize] [bigint] NULL, -- En bytes
	[FilePath] [varchar](500) NULL,
	[Filters] [nvarchar](max) NULL, -- JSON con filtros aplicados
	[RecordCount] [int] NULL,
	[Status] [varchar](50) NOT NULL, -- Processing, Completed, Failed
	[ErrorMessage] [nvarchar](1000) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CompletedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[DataExportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [audit].[DataExport] ADD DEFAULT ('Processing') FOR [Status]
GO

ALTER TABLE [audit].[DataExport] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [audit].[DataExport] WITH CHECK ADD CONSTRAINT [FK_DataExport_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [audit].[DataExport] CHECK CONSTRAINT [FK_DataExport_User]
GO

ALTER TABLE [audit].[DataExport] WITH CHECK ADD CONSTRAINT [FK_DataExport_Company] FOREIGN KEY([CompanyID])
REFERENCES [configuration].[Company] ([CompanyID])
GO

ALTER TABLE [audit].[DataExport] CHECK CONSTRAINT [FK_DataExport_Company]
GO

PRINT 'Audit tables created successfully!'
GO
