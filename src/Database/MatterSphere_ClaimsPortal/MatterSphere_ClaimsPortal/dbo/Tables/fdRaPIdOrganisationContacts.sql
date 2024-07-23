CREATE TABLE [dbo].[fdRaPIdOrganisationContacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[rpdOrgID] [nvarchar](50) NULL,
	[contID] [bigint] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdContacts_rowguid]  DEFAULT (newid()),
	rpdOrgName NVARCHAR(100) NULL,
	rpdOrgPath NVARCHAR(50) NULL,
 CONSTRAINT [PK_fdRaPIdContacts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)
)