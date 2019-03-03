USE [Voting]
GO
/****** Object:  Table [dbo].[Deputies]    Script Date: 2/2/2019 9:57:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deputies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MunicipalityId] [int] NOT NULL,
	[Firstname] [nvarchar](max) NOT NULL,
	[Lastname] [nvarchar](max) NOT NULL,
	[TitlePre] [nvarchar](max) NULL,
	[TitlePost] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Deputies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Email]    Script Date: 2/2/2019 9:57:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Email](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Receiver] [nvarchar](max) NOT NULL,
	[Body] [nvarchar](max) NULL,
	[IsSent] [bit] NOT NULL,
	[ErrorCount] [int] NOT NULL,
	[ErrorMessage] [nvarchar](max) NULL,
 CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Municipalities]    Script Date: 2/2/2019 9:57:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Municipalities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Municipality] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parties]    Script Date: 2/2/2019 9:57:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parties](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MunicipalityId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Parties] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessions]    Script Date: 2/2/2019 9:57:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Chairman] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[MunicipalityId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shortcuts]    Script Date: 2/2/2019 9:57:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shortcuts](
	[Hash] [nvarchar](50) NOT NULL,
	[TopicId] [int] NOT NULL,
 CONSTRAINT [PK_Shortcuts] PRIMARY KEY CLUSTERED 
(
	[Hash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Topics]    Script Date: 2/2/2019 9:57:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SessionId] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[Order] [int] NOT NULL,
	[Time] [datetime] NULL,
	[Total] [int] NULL,
	[IsProcedural] [bit] NOT NULL,
	[IsSecret] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Topics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votings]    Script Date: 2/2/2019 9:57:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TopicId] [int] NOT NULL,
	[DeputyId] [int] NOT NULL,
	[PartyId] [int] NULL,
	[Vote] [int] NOT NULL,
 CONSTRAINT [PK_Votings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Deputies] ADD  CONSTRAINT [DF_Deputies_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Email] ADD  CONSTRAINT [DF_Table_1_IsSend]  DEFAULT ((0)) FOR [IsSent]
GO
ALTER TABLE [dbo].[Email] ADD  CONSTRAINT [DF_Email_ErrorCount]  DEFAULT ((0)) FOR [ErrorCount]
GO
ALTER TABLE [dbo].[Municipalities] ADD  CONSTRAINT [DF_Municipalities_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Parties] ADD  CONSTRAINT [DF_Parties_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Sessions] ADD  CONSTRAINT [DF_Sessions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [DF_Topics_IsProcedural]  DEFAULT ((0)) FOR [IsProcedural]
GO
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [DF_Topics_IsSecret]  DEFAULT ((0)) FOR [IsSecret]
GO
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [DF_Topics_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Deputies]  WITH CHECK ADD  CONSTRAINT [FK_Deputies_Municipalities] FOREIGN KEY([MunicipalityId])
REFERENCES [dbo].[Municipalities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Deputies] CHECK CONSTRAINT [FK_Deputies_Municipalities]
GO
ALTER TABLE [dbo].[Parties]  WITH CHECK ADD  CONSTRAINT [FK_Parties_Municipalities] FOREIGN KEY([MunicipalityId])
REFERENCES [dbo].[Municipalities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Parties] CHECK CONSTRAINT [FK_Parties_Municipalities]
GO
ALTER TABLE [dbo].[Sessions]  WITH CHECK ADD  CONSTRAINT [FK_Sessions_Municipalities] FOREIGN KEY([MunicipalityId])
REFERENCES [dbo].[Municipalities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sessions] CHECK CONSTRAINT [FK_Sessions_Municipalities]
GO
ALTER TABLE [dbo].[Shortcuts]  WITH CHECK ADD  CONSTRAINT [FK_Shortcuts_Topics] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Topics] ([Id])
GO
ALTER TABLE [dbo].[Shortcuts] CHECK CONSTRAINT [FK_Shortcuts_Topics]
GO
ALTER TABLE [dbo].[Topics]  WITH CHECK ADD  CONSTRAINT [FK_Topics_Sessions] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Sessions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Topics] CHECK CONSTRAINT [FK_Topics_Sessions]
GO
ALTER TABLE [dbo].[Votings]  WITH CHECK ADD  CONSTRAINT [FK_Votings_Deputies] FOREIGN KEY([DeputyId])
REFERENCES [dbo].[Deputies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Votings] CHECK CONSTRAINT [FK_Votings_Deputies]
GO
ALTER TABLE [dbo].[Votings]  WITH CHECK ADD  CONSTRAINT [FK_Votings_Parties] FOREIGN KEY([PartyId])
REFERENCES [dbo].[Parties] ([Id])
GO
ALTER TABLE [dbo].[Votings] CHECK CONSTRAINT [FK_Votings_Parties]
GO
ALTER TABLE [dbo].[Votings]  WITH CHECK ADD  CONSTRAINT [FK_Votings_Topics] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Topics] ([Id])
GO
ALTER TABLE [dbo].[Votings] CHECK CONSTRAINT [FK_Votings_Topics]
GO
