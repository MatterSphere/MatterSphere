CREATE TABLE [dbo].[eEmailMessageConfig](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[emailDocInternalChgSubjectMsg] [nvarchar](50) NULL,
	[emailDocInternalChgBodyMsg] [nvarchar](max) NULL,
	[emailDocExternalChgSubjectMsg] [nvarchar](50) NULL,
	[emailDocExternalChgBodyMsg] [nvarchar](max) NULL,
	[emailForgotPWReqSubjectMsg] [nvarchar](50) NULL,
	[emailForgotPWReqBodyMsg] [nvarchar](max) NULL,
	[emailSuccessPWSubjectMsg] [nvarchar](50) NULL,
	[emailSuccessPWBodyMsg] [nvarchar](max) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_eEmailMessageConfig_rowguid]  DEFAULT (newid())
	CONSTRAINT [PK_eEmailMessageConfig] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]