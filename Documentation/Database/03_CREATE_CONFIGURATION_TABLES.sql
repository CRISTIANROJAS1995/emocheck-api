-- =============================================
-- EmoCheck Database - Configuration Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Configuration tables - Companies, Areas, Sites
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: Company (Empresas/Organizaciones)
-- =============================================
CREATE TABLE [configuration].[Company](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[CountryID] [int] NULL,
	[Name] [varchar](200) NOT NULL,
	[BusinessName] [varchar](200) NULL,
	[TaxID] [varchar](50) NULL, -- NIT en Colombia
	[Logo] [varchar](500) NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
	[Address] [varchar](255) NULL,
	[Website] [varchar](255) NULL,
	[Industry] [varchar](100) NULL,
	[EmployeeCount] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [configuration].[Company] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [configuration].[Company] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [configuration].[Company] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [configuration].[Company] WITH CHECK ADD CONSTRAINT [FK_Company_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [configuration].[Company] CHECK CONSTRAINT [FK_Company_State]
GO

ALTER TABLE [configuration].[Company] WITH CHECK ADD CONSTRAINT [FK_Company_Country] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([CountryID])
GO

ALTER TABLE [configuration].[Company] CHECK CONSTRAINT [FK_Company_Country]
GO

-- =============================================
-- Table: Site (Sedes de la empresa)
-- =============================================
CREATE TABLE [configuration].[Site](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[CityID] [int] NULL,
	[StateID] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Code] [varchar](50) NULL,
	[Address] [varchar](255) NULL,
	[Phone] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [configuration].[Site] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [configuration].[Site] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [configuration].[Site] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [configuration].[Site] WITH CHECK ADD CONSTRAINT [FK_Site_Company] FOREIGN KEY([CompanyID])
REFERENCES [configuration].[Company] ([CompanyID])
GO

ALTER TABLE [configuration].[Site] CHECK CONSTRAINT [FK_Site_Company]
GO

ALTER TABLE [configuration].[Site] WITH CHECK ADD CONSTRAINT [FK_Site_City] FOREIGN KEY([CityID])
REFERENCES [dbo].[City] ([CityID])
GO

ALTER TABLE [configuration].[Site] CHECK CONSTRAINT [FK_Site_City]
GO

ALTER TABLE [configuration].[Site] WITH CHECK ADD CONSTRAINT [FK_Site_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [configuration].[Site] CHECK CONSTRAINT [FK_Site_State]
GO

-- =============================================
-- Table: Area (Áreas/Departamentos)
-- =============================================
CREATE TABLE [configuration].[Area](
	[AreaID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Code] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[ManagerName] [varchar](200) NULL,
	[ManagerEmail] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AreaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [configuration].[Area] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [configuration].[Area] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [configuration].[Area] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [configuration].[Area] WITH CHECK ADD CONSTRAINT [FK_Area_Company] FOREIGN KEY([CompanyID])
REFERENCES [configuration].[Company] ([CompanyID])
GO

ALTER TABLE [configuration].[Area] CHECK CONSTRAINT [FK_Area_Company]
GO

ALTER TABLE [configuration].[Area] WITH CHECK ADD CONSTRAINT [FK_Area_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [configuration].[Area] CHECK CONSTRAINT [FK_Area_State]
GO

-- =============================================
-- Table: JobType (Tipos de cargo)
-- =============================================
CREATE TABLE [configuration].[JobType](
	[JobTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](255) NULL,
	[Level] [varchar](50) NULL, -- Operativo, Administrativo, Directivo, etc.
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[JobTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [configuration].[JobType] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [configuration].[JobType] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [configuration].[JobType] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

-- =============================================
-- Table: Application (Aplicaciones del sistema)
-- =============================================
CREATE TABLE [configuration].[Application](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
	[Version] [varchar](20) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [configuration].[Application] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [configuration].[Application] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [configuration].[Application] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

PRINT 'Configuration tables created successfully!'
GO
