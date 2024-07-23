CREATE TABLE [dbo].[fdRaPIdFileHospitals](
	[rpdHospID] [bigint] IDENTITY(1,1) NOT NULL,
	[fileID] [bigint] NULL,
	[rpdHospType] [nvarchar](15) NULL,
	[assocID] [bigint] NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdFileHospitals_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdFileHospitals] PRIMARY KEY CLUSTERED 
(
	[rpdHospID] ASC
) 
) 