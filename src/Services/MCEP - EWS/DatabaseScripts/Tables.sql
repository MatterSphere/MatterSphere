/****** Object:  Schema [MCEP]    Script Date: 24/05/2013 10:41:22 ******/
CREATE SCHEMA [MCEP]
GO

/****** Object:  Table [MCEP].[User]    Script Date: 24/05/2013 08:23:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [MCEP].[User](
	[UserID] [int] NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[RootFolderName] [nvarchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NULL,
	[LastRan] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[RootFolderID] [nvarchar](max) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [MCEP].[User] ADD  CONSTRAINT [DF_User_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [MCEP].[User] ADD  CONSTRAINT [DF_User_Active]  DEFAULT ((1)) FOR [Active]
GO

/****** Object:  Table [MCEP].[Queue]    Script Date: 24/05/2013 08:23:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [MCEP].[Queue](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserEmail] [nvarchar](50) NOT NULL,
	[FolderID] [nvarchar](max) NOT NULL,
	[MessageID] [nvarchar](max) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[FileID] [bigint] NOT NULL,
	[AssocID] [bigint] NOT NULL,
	[DocID] [bigint] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[Processed] [bit] NOT NULL,
	[Result] [nvarchar](100) NULL,
	[ErrorMessage] [nvarchar](max) NULL,
 CONSTRAINT [PK_Queue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [MCEP].[Queue] ADD  CONSTRAINT [DF_Queue_AssocID]  DEFAULT ((0)) FOR [AssocID]
GO

ALTER TABLE [MCEP].[Queue] ADD  CONSTRAINT [DF_Queue_DocID]  DEFAULT ((0)) FOR [DocID]
GO


