-- =============================================
-- EmoCheck Database - Master Tables (dbo schema)
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Core master tables - State, Country, etc.
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: State (Estados generales del sistema)
-- =============================================
CREATE TABLE [dbo].[State](
	[StateID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[State] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [dbo].[State] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[State] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

-- =============================================
-- Table: Country (Países)
-- =============================================
CREATE TABLE [dbo].[Country](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](10) NULL,
	[PhoneCode] [varchar](10) NULL,
	[Currency] [varchar](10) NULL,
	[TimeZone] [varchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Country] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [dbo].[Country] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[Country] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

-- =============================================
-- Table: City (Ciudades)
-- =============================================
CREATE TABLE [dbo].[City](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](10) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[City] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [dbo].[City] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [dbo].[City] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [dbo].[City] WITH CHECK ADD CONSTRAINT [FK_City_Country] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([CountryID])
GO

ALTER TABLE [dbo].[City] CHECK CONSTRAINT [FK_City_Country]
GO

PRINT 'Master tables (dbo) created successfully!'
GO
