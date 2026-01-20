-- =============================================
-- EmoCheck Database - Security Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Security tables - Users, Roles, Permissions
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: Role (Roles del sistema)
-- =============================================
CREATE TABLE [security].[Role](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [security].[Role] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [security].[Role] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[Role] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

-- =============================================
-- Table: User (Usuarios del sistema)
-- =============================================
CREATE TABLE [security].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NULL,
	[SiteID] [int] NULL,
	[AreaID] [int] NULL,
	[JobTypeID] [int] NULL,
	[StateID] [int] NOT NULL,
	[ApplicationID] [int] NULL,
	[Username] [varchar](100) NULL,
	[Email] [varchar](150) NOT NULL,
	[PasswordHash] [varchar](500) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NULL,
	[DocumentType] [varchar](50) NULL, -- CC, CE, Pasaporte
	[DocumentNumber] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[Gender] [varchar](20) NULL, -- Male, Female, Other, PreferNotToSay
	[BirthDate] [date] NULL,
	[ProfileImage] [varchar](500) NULL,
	[IsEmailConfirmed] [bit] NOT NULL,
	[EmailConfirmedAt] [datetime] NULL,
	[LastLogin] [datetime] NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[LockedUntil] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [security].[User] ADD DEFAULT (0) FOR [IsEmailConfirmed]
GO

ALTER TABLE [security].[User] ADD DEFAULT (0) FOR [FailedLoginAttempts]
GO

ALTER TABLE [security].[User] ADD DEFAULT (0) FOR [IsLocked]
GO

ALTER TABLE [security].[User] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [security].[User] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[User] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_Company] FOREIGN KEY([CompanyID])
REFERENCES [configuration].[Company] ([CompanyID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_Company]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_Site] FOREIGN KEY([SiteID])
REFERENCES [configuration].[Site] ([SiteID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_Site]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_Area] FOREIGN KEY([AreaID])
REFERENCES [configuration].[Area] ([AreaID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_Area]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_JobType] FOREIGN KEY([JobTypeID])
REFERENCES [configuration].[JobType] ([JobTypeID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_JobType]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_State]
GO

ALTER TABLE [security].[User] WITH CHECK ADD CONSTRAINT [FK_User_Application] FOREIGN KEY([ApplicationID])
REFERENCES [configuration].[Application] ([ApplicationID])
GO

ALTER TABLE [security].[User] CHECK CONSTRAINT [FK_User_Application]
GO

-- =============================================
-- Table: UserRole (Relación Usuarios-Roles)
-- =============================================
CREATE TABLE [security].[UserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[AssignedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [security].[UserRole] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [security].[UserRole] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[UserRole] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [security].[UserRole] WITH CHECK ADD CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [security].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO

ALTER TABLE [security].[UserRole] WITH CHECK ADD CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleID])
REFERENCES [security].[Role] ([RoleID])
GO

ALTER TABLE [security].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO

ALTER TABLE [security].[UserRole] WITH CHECK ADD CONSTRAINT [FK_UserRole_AssignedBy] FOREIGN KEY([AssignedBy])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [security].[UserRole] CHECK CONSTRAINT [FK_UserRole_AssignedBy]
GO

-- =============================================
-- Table: RefreshToken (Tokens de refresco JWT)
-- =============================================
CREATE TABLE [security].[RefreshToken](
	[RefreshTokenID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Token] [varchar](500) NOT NULL,
	[ExpiresAt] [datetime] NOT NULL,
	[IsRevoked] [bit] NOT NULL,
	[RevokedAt] [datetime] NULL,
	[ReplacedByToken] [varchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedByIP] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RefreshTokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [security].[RefreshToken] ADD DEFAULT (0) FOR [IsRevoked]
GO

ALTER TABLE [security].[RefreshToken] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[RefreshToken] WITH CHECK ADD CONSTRAINT [FK_RefreshToken_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [security].[RefreshToken] CHECK CONSTRAINT [FK_RefreshToken_User]
GO

-- =============================================
-- Table: PasswordResetToken (Tokens de recuperación de contraseña)
-- =============================================
CREATE TABLE [security].[PasswordResetToken](
	[PasswordResetTokenID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Token] [varchar](500) NOT NULL,
	[ExpiresAt] [datetime] NOT NULL,
	[IsUsed] [bit] NOT NULL,
	[UsedAt] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[CreatedByIP] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PasswordResetTokenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [security].[PasswordResetToken] ADD DEFAULT (0) FOR [IsUsed]
GO

ALTER TABLE [security].[PasswordResetToken] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[PasswordResetToken] WITH CHECK ADD CONSTRAINT [FK_PasswordResetToken_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [security].[PasswordResetToken] CHECK CONSTRAINT [FK_PasswordResetToken_User]
GO

-- =============================================
-- Table: InformedConsent (Consentimientos Informados)
-- =============================================
CREATE TABLE [security].[InformedConsent](
	[InformedConsentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Version] [varchar](20) NOT NULL, -- Version del consentimiento (1.0, 1.1, etc.)
	[ConsentText] [nvarchar](max) NOT NULL,
	[IsAccepted] [bit] NOT NULL,
	[AcceptedAt] [datetime] NULL,
	[IPAddress] [varchar](50) NULL,
	[UserAgent] [varchar](500) NULL,
	[PDFPath] [varchar](500) NULL, -- Ruta al PDF firmado
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[InformedConsentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [security].[InformedConsent] ADD DEFAULT (0) FOR [IsAccepted]
GO

ALTER TABLE [security].[InformedConsent] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [security].[InformedConsent] WITH CHECK ADD CONSTRAINT [FK_InformedConsent_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [security].[InformedConsent] CHECK CONSTRAINT [FK_InformedConsent_User]
GO

PRINT 'Security tables created successfully!'
GO
