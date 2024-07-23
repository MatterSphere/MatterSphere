
/****** Object:  Table [dbo].[dbPowerUserProfiles]    Script Date: 29/07/2019 10:23:06 ******/

CREATE TABLE [dbo].[dbPowerUserProfiles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[PowerRoles] [ntext] NULL,
	[PowerMenuItem] [ntext] NULL,
	[BranchID] [int] NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PowerUserProfiles_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_PowerUserProfiles] UNIQUE NONCLUSTERED 
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[dbPowerUserProfiles] ADD  CONSTRAINT [DF_PowerUserProfiles_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO


