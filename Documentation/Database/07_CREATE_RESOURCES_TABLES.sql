-- =============================================
-- EmoCheck Database - Resources Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Wellness resources tables
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: ResourceCategory (Categorías de recursos)
-- =============================================
CREATE TABLE [resources].[ResourceCategory](
	[ResourceCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[Icon] [varchar](255) NULL,
	[Color] [varchar](50) NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ResourceCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [resources].[ResourceCategory] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [resources].[ResourceCategory] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [resources].[ResourceCategory] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [resources].[ResourceCategory] WITH CHECK ADD CONSTRAINT [FK_ResourceCategory_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [resources].[ResourceCategory] CHECK CONSTRAINT [FK_ResourceCategory_State]
GO

-- =============================================
-- Table: WellnessResource (Recursos de bienestar)
-- =============================================
CREATE TABLE [resources].[WellnessResource](
	[WellnessResourceID] [int] IDENTITY(1,1) NOT NULL,
	[ResourceCategoryID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[ResourceType] [varchar](50) NOT NULL, -- Video, Audio, Article, Exercise, PDF
	[ContentURL] [varchar](500) NULL,
	[ThumbnailURL] [varchar](500) NULL,
	[Duration] [int] NULL, -- En minutos
	[Tags] [varchar](500) NULL, -- Separados por comas
	[ViewCount] [int] NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[IsFeatured] [bit] NOT NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WellnessResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (0) FOR [ViewCount]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (1) FOR [IsPublic]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (0) FOR [IsFeatured]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [resources].[WellnessResource] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [resources].[WellnessResource] WITH CHECK ADD CONSTRAINT [FK_WellnessResource_ResourceCategory] FOREIGN KEY([ResourceCategoryID])
REFERENCES [resources].[ResourceCategory] ([ResourceCategoryID])
GO

ALTER TABLE [resources].[WellnessResource] CHECK CONSTRAINT [FK_WellnessResource_ResourceCategory]
GO

ALTER TABLE [resources].[WellnessResource] WITH CHECK ADD CONSTRAINT [FK_WellnessResource_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [resources].[WellnessResource] CHECK CONSTRAINT [FK_WellnessResource_State]
GO

-- =============================================
-- Table: UserResourceAccess (Acceso de usuarios a recursos)
-- =============================================
CREATE TABLE [resources].[UserResourceAccess](
	[UserResourceAccessID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[WellnessResourceID] [int] NOT NULL,
	[AccessCount] [int] NOT NULL,
	[FirstAccessedAt] [datetime] NOT NULL,
	[LastAccessedAt] [datetime] NOT NULL,
	[CompletionPercentage] [decimal](5,2) NULL,
	[IsCompleted] [bit] NOT NULL,
	[CompletedAt] [datetime] NULL,
	[Rating] [int] NULL, -- 1-5 estrellas
	[Feedback] [nvarchar](1000) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserResourceAccessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [resources].[UserResourceAccess] ADD DEFAULT (0) FOR [AccessCount]
GO

ALTER TABLE [resources].[UserResourceAccess] ADD DEFAULT (0) FOR [IsCompleted]
GO

ALTER TABLE [resources].[UserResourceAccess] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [resources].[UserResourceAccess] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [resources].[UserResourceAccess] WITH CHECK ADD CONSTRAINT [FK_UserResourceAccess_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [resources].[UserResourceAccess] CHECK CONSTRAINT [FK_UserResourceAccess_User]
GO

ALTER TABLE [resources].[UserResourceAccess] WITH CHECK ADD CONSTRAINT [FK_UserResourceAccess_WellnessResource] FOREIGN KEY([WellnessResourceID])
REFERENCES [resources].[WellnessResource] ([WellnessResourceID])
GO

ALTER TABLE [resources].[UserResourceAccess] CHECK CONSTRAINT [FK_UserResourceAccess_WellnessResource]
GO

-- =============================================
-- Table: ProfessionalSupport (Apoyo profesional)
-- =============================================
CREATE TABLE [resources].[ProfessionalSupport](
	[ProfessionalSupportID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NULL,
	[StateID] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Specialty] [varchar](100) NULL, -- Psicología clínica, Psicología organizacional
	[LicenseNumber] [varchar](100) NULL,
	[Email] [varchar](150) NULL,
	[Phone] [varchar](50) NULL,
	[AvailableSchedule] [varchar](500) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProfessionalSupportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [resources].[ProfessionalSupport] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [resources].[ProfessionalSupport] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [resources].[ProfessionalSupport] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [resources].[ProfessionalSupport] WITH CHECK ADD CONSTRAINT [FK_ProfessionalSupport_Company] FOREIGN KEY([CompanyID])
REFERENCES [configuration].[Company] ([CompanyID])
GO

ALTER TABLE [resources].[ProfessionalSupport] CHECK CONSTRAINT [FK_ProfessionalSupport_Company]
GO

ALTER TABLE [resources].[ProfessionalSupport] WITH CHECK ADD CONSTRAINT [FK_ProfessionalSupport_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [resources].[ProfessionalSupport] CHECK CONSTRAINT [FK_ProfessionalSupport_State]
GO

-- =============================================
-- Table: SupportRequest (Solicitudes de apoyo)
-- =============================================
CREATE TABLE [resources].[SupportRequest](
	[SupportRequestID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ProfessionalSupportID] [int] NULL,
	[StateID] [int] NOT NULL,
	[RequestType] [varchar](100) NOT NULL, -- Consultation, Emergency, FollowUp
	[Priority] [varchar](50) NOT NULL, -- Urgent, High, Normal
	[PreferredDate] [datetime] NULL,
	[PreferredTime] [varchar](50) NULL,
	[Reason] [nvarchar](1000) NULL,
	[Status] [varchar](50) NOT NULL, -- Pending, Scheduled, Completed, Cancelled
	[ScheduledDate] [datetime] NULL,
	[CompletedDate] [datetime] NULL,
	[Notes] [nvarchar](2000) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SupportRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [resources].[SupportRequest] ADD DEFAULT ('Pending') FOR [Status]
GO

ALTER TABLE [resources].[SupportRequest] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [resources].[SupportRequest] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [resources].[SupportRequest] WITH CHECK ADD CONSTRAINT [FK_SupportRequest_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [resources].[SupportRequest] CHECK CONSTRAINT [FK_SupportRequest_User]
GO

ALTER TABLE [resources].[SupportRequest] WITH CHECK ADD CONSTRAINT [FK_SupportRequest_ProfessionalSupport] FOREIGN KEY([ProfessionalSupportID])
REFERENCES [resources].[ProfessionalSupport] ([ProfessionalSupportID])
GO

ALTER TABLE [resources].[SupportRequest] CHECK CONSTRAINT [FK_SupportRequest_ProfessionalSupport]
GO

ALTER TABLE [resources].[SupportRequest] WITH CHECK ADD CONSTRAINT [FK_SupportRequest_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [resources].[SupportRequest] CHECK CONSTRAINT [FK_SupportRequest_State]
GO

PRINT 'Resources tables created successfully!'
GO
