CREATE TABLE [dbo].[fdRaPIdInsData](
	[contID] [bigint] NOT NULL,
	[rpdInsOrgID] [nvarchar](50) NULL,
	[rpdInsType] [uCodeLookup] NULL,
	[rpdInsOrgPath] [nvarchar](200) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdIndData_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdIndData] PRIMARY KEY CLUSTERED 
(
	[contID] ASC
)
)