CREATE TABLE [dbo].[fdRaPIdHospitalDetails](
		[rpdHospID] [bigint] IDENTITY(1,1) NOT NULL,
		[rpdMedHospitalType] [nvarchar](25) NULL,
		[rpdMedHospitalName] [nvarchar](75) NULL,
		[rpdMedHospitalPostCode] [nvarchar](15) NULL,
		[rpdMedHospitalAddLine1] [nvarchar](50) NULL,
		[rpdMedHospitalAddLine2] [nvarchar](50) NULL,
		[rpdMedHospitalAddLine3] [nvarchar](50) NULL,
		[rpdMedHospitalAddLine4] [nvarchar](50) NULL,
		[rowguid] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_fdRaPIdHospitalDetails_rowguid]  DEFAULT (newid()),
	CONSTRAINT [PK_fdRaPIdHospitalDetails] PRIMARY KEY CLUSTERED 
(
	[rpdHospID] ASC
	)
	)