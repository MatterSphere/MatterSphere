CREATE TABLE [Audit].[Configuration](
	[acID] [bigint] IDENTITY(1,1) NOT NULL,
	[acTableName] [nvarchar](max) NOT NULL,
	[acTableFriendlyName] [nvarchar](256) NULL,
	[acAuditTableName] [nvarchar](max) NOT NULL,
	[acDelete] [bit] NOT NULL CONSTRAINT [DF_Configuration_acDelete]  DEFAULT (1),
	[acUpdate] [bit] NOT NULL CONSTRAINT [DF_Configuration_acUpdate]  DEFAULT (1),
	[acInsert] [bit] NOT NULL CONSTRAINT [DF_Configuration_acInsert]  DEFAULT (1),
	[acRetentionPeriod] [int] NULL CONSTRAINT [DF_Configuration_acRetentionPeriod]  DEFAULT (0),
	[acGroup] [nvarchar](500) NULL,
	[acSourceTableTriggerID] [bigint] NULL,
	[acActive] [bit] NOT NULL CONSTRAINT [DF_Configuration_acActive]  DEFAULT (1),
	[Created] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[Updated] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[rowGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_Configuration_rowGuid]  DEFAULT (newid()),
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[acID] ASC
)
)
