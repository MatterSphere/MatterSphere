CREATE TABLE [dbo].[fdRaPIdOrganisation](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[rpdOrgInsurerName] [nvarchar](100) NULL,
	[rpdOrgID] [nvarchar](50) NULL,
	[rpdOrgName] [nvarchar](100) NULL,
	[rpdOrgPath] [nvarchar](50) NULL,
	[rpdOrgHouseName] [nvarchar](50) NULL,
	[rpdOrgHouseNumber] [nvarchar](50) NULL,
	[rpdOrgStreet1] [nvarchar](50) NULL,
	[rpdOrgStreet2] [nvarchar](50) NULL,
	[rpdOrgCity] [nvarchar](50) NULL,
	[rpdOrgCounty] [nvarchar](50) NULL,
	[rpdOrgDistrict] [nvarchar](50) NULL,
	[rpdOrgPostCode] [nvarchar](50) NULL,
	[rpdOrgCountry] [nvarchar](50) NULL,
	[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdOrganisation_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_fdRaPIdOrganisation_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)