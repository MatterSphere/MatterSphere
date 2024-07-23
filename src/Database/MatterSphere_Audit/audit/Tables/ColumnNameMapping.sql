CREATE TABLE [Audit].[ColumnNameMapping](
	[TableName] [nvarchar](200) NOT NULL,
	[ColumnName] [nvarchar](200) NOT NULL,
	[FriendlyName] [nvarchar](256) NOT NULL,
	[Exclude] [bit] NOT NULL CONSTRAINT [DF_ColumnNameMapping_Exclude]  DEFAULT (0),
	[LinkField] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ColumnNameMapping] PRIMARY KEY CLUSTERED 
(
	[TableName] ASC,
	[ColumnName] ASC
)
)