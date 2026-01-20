-- =============================================
-- EmoCheck Database - Assessment Tables
-- Version: 1.0
-- Date: 2026-01-20
-- Description: Assessment module tables - Modules, Questions, Evaluations
-- =============================================

USE [EmoCheckDB]
GO

-- =============================================
-- Table: AssessmentModule (Módulos de evaluación)
-- =============================================
CREATE TABLE [assessment].[AssessmentModule](
	[AssessmentModuleID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](500) NULL,
	[InstrumentType] [varchar](100) NULL, -- GAD-7, PHQ-9, ISI, Custom
	[Category] [varchar](100) NULL, -- MentalHealth, WorkFatigue, OrganizationalClimate, PsychosocialRisk
	[MaxScore] [int] NULL,
	[MinScore] [int] NULL,
	[Icon] [varchar](255) NULL,
	[Color] [varchar](50) NULL,
	[EstimatedMinutes] [int] NULL,
	[DisplayOrder] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AssessmentModuleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [assessment].[AssessmentModule] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [assessment].[AssessmentModule] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [assessment].[AssessmentModule] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [assessment].[AssessmentModule] WITH CHECK ADD CONSTRAINT [FK_AssessmentModule_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [assessment].[AssessmentModule] CHECK CONSTRAINT [FK_AssessmentModule_State]
GO

-- =============================================
-- Table: Question (Preguntas de los cuestionarios)
-- =============================================
CREATE TABLE [assessment].[Question](
	[QuestionID] [int] IDENTITY(1,1) NOT NULL,
	[AssessmentModuleID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[QuestionText] [nvarchar](1000) NOT NULL,
	[QuestionType] [varchar](50) NOT NULL, -- SingleChoice, MultipleChoice, Scale, Text
	[DisplayOrder] [int] NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [assessment].[Question] ADD DEFAULT (1) FOR [IsRequired]
GO

ALTER TABLE [assessment].[Question] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [assessment].[Question] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [assessment].[Question] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [assessment].[Question] WITH CHECK ADD CONSTRAINT [FK_Question_AssessmentModule] FOREIGN KEY([AssessmentModuleID])
REFERENCES [assessment].[AssessmentModule] ([AssessmentModuleID])
GO

ALTER TABLE [assessment].[Question] CHECK CONSTRAINT [FK_Question_AssessmentModule]
GO

ALTER TABLE [assessment].[Question] WITH CHECK ADD CONSTRAINT [FK_Question_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [assessment].[Question] CHECK CONSTRAINT [FK_Question_State]
GO

-- =============================================
-- Table: QuestionOption (Opciones de respuesta)
-- =============================================
CREATE TABLE [assessment].[QuestionOption](
	[QuestionOptionID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[OptionText] [nvarchar](500) NOT NULL,
	[OptionValue] [int] NOT NULL, -- Valor para cálculo de puntaje
	[DisplayOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[QuestionOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [assessment].[QuestionOption] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [assessment].[QuestionOption] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [assessment].[QuestionOption] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [assessment].[QuestionOption] WITH CHECK ADD CONSTRAINT [FK_QuestionOption_Question] FOREIGN KEY([QuestionID])
REFERENCES [assessment].[Question] ([QuestionID])
GO

ALTER TABLE [assessment].[QuestionOption] CHECK CONSTRAINT [FK_QuestionOption_Question]
GO

ALTER TABLE [assessment].[QuestionOption] WITH CHECK ADD CONSTRAINT [FK_QuestionOption_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [assessment].[QuestionOption] CHECK CONSTRAINT [FK_QuestionOption_State]
GO

-- =============================================
-- Table: Evaluation (Evaluaciones realizadas)
-- =============================================
CREATE TABLE [assessment].[Evaluation](
	[EvaluationID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[AssessmentModuleID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[EvaluationCode] [varchar](100) NULL, -- Código único de la evaluación
	[StartedAt] [datetime] NOT NULL,
	[CompletedAt] [datetime] NULL,
	[IsCompleted] [bit] NOT NULL,
	[CompletionPercentage] [decimal](5,2) NULL,
	[IPAddress] [varchar](50) NULL,
	[UserAgent] [varchar](500) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [assessment].[Evaluation] ADD DEFAULT (0) FOR [IsCompleted]
GO

ALTER TABLE [assessment].[Evaluation] ADD DEFAULT (0) FOR [CompletionPercentage]
GO

ALTER TABLE [assessment].[Evaluation] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [assessment].[Evaluation] ADD DEFAULT (GETDATE()) FOR [UpdatedAt]
GO

ALTER TABLE [assessment].[Evaluation] WITH CHECK ADD CONSTRAINT [FK_Evaluation_User] FOREIGN KEY([UserID])
REFERENCES [security].[User] ([UserID])
GO

ALTER TABLE [assessment].[Evaluation] CHECK CONSTRAINT [FK_Evaluation_User]
GO

ALTER TABLE [assessment].[Evaluation] WITH CHECK ADD CONSTRAINT [FK_Evaluation_AssessmentModule] FOREIGN KEY([AssessmentModuleID])
REFERENCES [assessment].[AssessmentModule] ([AssessmentModuleID])
GO

ALTER TABLE [assessment].[Evaluation] CHECK CONSTRAINT [FK_Evaluation_AssessmentModule]
GO

ALTER TABLE [assessment].[Evaluation] WITH CHECK ADD CONSTRAINT [FK_Evaluation_State] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([StateID])
GO

ALTER TABLE [assessment].[Evaluation] CHECK CONSTRAINT [FK_Evaluation_State]
GO

-- =============================================
-- Table: EvaluationResponse (Respuestas del usuario)
-- =============================================
CREATE TABLE [assessment].[EvaluationResponse](
	[EvaluationResponseID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[QuestionID] [int] NOT NULL,
	[QuestionOptionID] [int] NULL, -- NULL si es respuesta de texto
	[ResponseValue] [int] NULL, -- Valor de la opción seleccionada
	[ResponseText] [nvarchar](2000) NULL, -- Para preguntas abiertas
	[AnsweredAt] [datetime] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationResponseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [assessment].[EvaluationResponse] ADD DEFAULT (GETDATE()) FOR [CreatedAt]
GO

ALTER TABLE [assessment].[EvaluationResponse] WITH CHECK ADD CONSTRAINT [FK_EvaluationResponse_Evaluation] FOREIGN KEY([EvaluationID])
REFERENCES [assessment].[Evaluation] ([EvaluationID])
GO

ALTER TABLE [assessment].[EvaluationResponse] CHECK CONSTRAINT [FK_EvaluationResponse_Evaluation]
GO

ALTER TABLE [assessment].[EvaluationResponse] WITH CHECK ADD CONSTRAINT [FK_EvaluationResponse_Question] FOREIGN KEY([QuestionID])
REFERENCES [assessment].[Question] ([QuestionID])
GO

ALTER TABLE [assessment].[EvaluationResponse] CHECK CONSTRAINT [FK_EvaluationResponse_Question]
GO

ALTER TABLE [assessment].[EvaluationResponse] WITH CHECK ADD CONSTRAINT [FK_EvaluationResponse_QuestionOption] FOREIGN KEY([QuestionOptionID])
REFERENCES [assessment].[QuestionOption] ([QuestionOptionID])
GO

ALTER TABLE [assessment].[EvaluationResponse] CHECK CONSTRAINT [FK_EvaluationResponse_QuestionOption]
GO

PRINT 'Assessment tables created successfully!'
GO
