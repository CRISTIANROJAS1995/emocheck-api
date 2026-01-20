-- =============================================
-- EmoCheck Database - Results Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Results, Recommendations, Alerts tables
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: EvaluationResult (Resultados de evaluaciones)
-- =============================================
CREATE TABLE [results].[EvaluationResult](
	[EvaluationResultID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[TotalScore] [decimal](10,2) NOT NULL,
	[PercentageScore] [decimal](5,2) NULL,
	[RiskLevel] [varchar](50) NOT NULL, -- Green, Yellow, Red
	[RiskLevelCode] [int] NOT NULL, -- 1=Green, 2=Yellow, 3=Red
	[Interpretation] [nvarchar](2000) NULL,
	[CalculatedAt] [datetime] NOT NULL,
	[NextEvaluationDate] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [results].[EvaluationResult] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[EvaluationResult] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [results].[EvaluationResult] WITH CHECK ADD CONSTRAINT [FK_EvaluationResult_Evaluation] FOREIGN KEY([EvaluationID])
REFERENCES [assessment].[Evaluation] ([EvaluationID])
GO

ALTER TABLE [results].[EvaluationResult] CHECK CONSTRAINT [FK_EvaluationResult_Evaluation]
GO

ALTER TABLE [results].[EvaluationResult] WITH CHECK ADD CONSTRAINT [FK_EvaluationResult_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [results].[EvaluationResult] CHECK CONSTRAINT [FK_EvaluationResult_State]
GO

-- =============================================
-- Table: DimensionScore (Puntajes por dimensión)
-- =============================================
CREATE TABLE [results].[DimensionScore](
	[DimensionScoreID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationResultID] [int] NOT NULL,
	[DimensionName] [varchar](200) NOT NULL, -- Ansiedad, Depresión, Insomnio, etc.
	[DimensionCode] [varchar](50) NOT NULL,
	[Score] [decimal](10,2) NOT NULL,
	[MaxScore] [decimal](10,2) NULL,
	[PercentageScore] [decimal](5,2) NULL,
	[RiskLevel] [varchar](50) NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DimensionScoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [results].[DimensionScore] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[DimensionScore] WITH CHECK ADD CONSTRAINT [FK_DimensionScore_EvaluationResult] FOREIGN KEY([EvaluationResultID])
REFERENCES [results].[EvaluationResult] ([EvaluationResultID])
GO

ALTER TABLE [results].[DimensionScore] CHECK CONSTRAINT [FK_DimensionScore_EvaluationResult]
GO

-- =============================================
-- Table: RecommendationType (Tipos de recomendación)
-- =============================================
CREATE TABLE [results].[RecommendationType](
	[RecommendationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[Icon] [varchar](255) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecommendationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [results].[RecommendationType] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [results].[RecommendationType] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[RecommendationType] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

-- =============================================
-- Table: Recommendation (Recomendaciones personalizadas)
-- =============================================
CREATE TABLE [results].[Recommendation](
	[RecommendationID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationResultID] [int] NOT NULL,
	[RecommendationTypeID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](2000) NOT NULL,
	[Priority] [int] NOT NULL, -- 1=High, 2=Medium, 3=Low
	[ResourceURL] [varchar](500) NULL,
	[IsViewed] [bit] NOT NULL,
	[ViewedAt] [datetime] NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RecommendationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [results].[Recommendation] ADD DEFAULT (0) FOR [IsViewed]
GO

ALTER TABLE [results].[Recommendation] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[Recommendation] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [results].[Recommendation] WITH CHECK ADD CONSTRAINT [FK_Recommendation_EvaluationResult] FOREIGN KEY([EvaluationResultID])
REFERENCES [results].[EvaluationResult] ([EvaluationResultID])
GO

ALTER TABLE [results].[Recommendation] CHECK CONSTRAINT [FK_Recommendation_EvaluationResult]
GO

ALTER TABLE [results].[Recommendation] WITH CHECK ADD CONSTRAINT [FK_Recommendation_RecommendationType] FOREIGN KEY([RecommendationTypeID])
REFERENCES [results].[RecommendationType] ([RecommendationTypeID])
GO

ALTER TABLE [results].[Recommendation] CHECK CONSTRAINT [FK_Recommendation_RecommendationType]
GO

ALTER TABLE [results].[Recommendation] WITH CHECK ADD CONSTRAINT [FK_Recommendation_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [results].[Recommendation] CHECK CONSTRAINT [FK_Recommendation_State]
GO

-- =============================================
-- Table: Alert (Alertas críticas)
-- =============================================
CREATE TABLE [results].[Alert](
	[AlertID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[EvaluationResultID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[AlertLevel] [varchar](50) NOT NULL, -- Critical, High, Medium
	[AlertLevelCode] [int] NOT NULL, -- 1=Critical, 2=High, 3=Medium
	[Title] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[IsAcknowledged] [bit] NOT NULL,
	[AcknowledgedBy] [int] NULL,
	[AcknowledgedAt] [datetime] NULL,
	[IsResolved] [bit] NOT NULL,
	[ResolvedBy] [int] NULL,
	[ResolvedAt] [datetime] NULL,
	[ResolutionNotes] [nvarchar](2000) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AlertID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [results].[Alert] ADD DEFAULT (0) FOR [IsAcknowledged]
GO

ALTER TABLE [results].[Alert] ADD DEFAULT (0) FOR [IsResolved]
GO

ALTER TABLE [results].[Alert] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[Alert] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [results].[Alert] WITH CHECK ADD CONSTRAINT [FK_Alert_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [results].[Alert] CHECK CONSTRAINT [FK_Alert_User]
GO

ALTER TABLE [results].[Alert] WITH CHECK ADD CONSTRAINT [FK_Alert_EvaluationResult] FOREIGN KEY([EvaluationResultID])
REFERENCES [results].[EvaluationResult] ([EvaluationResultID])
GO

ALTER TABLE [results].[Alert] CHECK CONSTRAINT [FK_Alert_EvaluationResult]
GO

ALTER TABLE [results].[Alert] WITH CHECK ADD CONSTRAINT [FK_Alert_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [results].[Alert] CHECK CONSTRAINT [FK_Alert_State]
GO

ALTER TABLE [results].[Alert] WITH CHECK ADD CONSTRAINT [FK_Alert_AcknowledgedBy] FOREIGN KEY([AcknowledgedBy])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [results].[Alert] CHECK CONSTRAINT [FK_Alert_AcknowledgedBy]
GO

ALTER TABLE [results].[Alert] WITH CHECK ADD CONSTRAINT [FK_Alert_ResolvedBy] FOREIGN KEY([ResolvedBy])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [results].[Alert] CHECK CONSTRAINT [FK_Alert_ResolvedBy]
GO

-- =============================================
-- Table: CaseTracking (Seguimiento de casos)
-- =============================================
CREATE TABLE [results].[CaseTracking](
	[CaseTrackingID] [int] IDENTITY(1,1) NOT NULL,
	[AlertID] [int] NOT NULL,
	[AssignedTo] [int] NULL, -- Psicólogo/HSE asignado
	[StateID] [int] NOT NULL,
	[CaseNumber] [varchar](50) NULL,
	[Priority] [varchar](50) NULL, -- Urgent, High, Normal
	[Status] [varchar](50) NOT NULL, -- Open, InProgress, Closed
	[ContactAttempts] [int] NOT NULL,
	[LastContactDate] [datetime] NULL,
	[NextFollowUpDate] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
	[ClosedAt] [datetime] NULL,
	[ClosureReason] [nvarchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CaseTrackingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [results].[CaseTracking] ADD DEFAULT (0) FOR [ContactAttempts]
GO

ALTER TABLE [results].[CaseTracking] ADD DEFAULT ('Open') FOR [Status]
GO

ALTER TABLE [results].[CaseTracking] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [results].[CaseTracking] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [results].[CaseTracking] WITH CHECK ADD CONSTRAINT [FK_CaseTracking_Alert] FOREIGN KEY([AlertID])
REFERENCES [results].[Alert] ([AlertID])
GO

ALTER TABLE [results].[CaseTracking] CHECK CONSTRAINT [FK_CaseTracking_Alert]
GO

ALTER TABLE [results].[CaseTracking] WITH CHECK ADD CONSTRAINT [FK_CaseTracking_AssignedTo] FOREIGN KEY([AssignedTo])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [results].[CaseTracking] CHECK CONSTRAINT [FK_CaseTracking_AssignedTo]
GO

ALTER TABLE [results].[CaseTracking] WITH CHECK ADD CONSTRAINT [FK_CaseTracking_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [results].[CaseTracking] CHECK CONSTRAINT [FK_CaseTracking_State]
GO

PRINT 'Results tables created successfully!'
GO
