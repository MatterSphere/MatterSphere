﻿DECLARE @Length INT = (SELECT CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'dbUserFavourites' AND COLUMN_NAME = 'usrFavObjParam3')
IF @Length < 1000
	ALTER TABLE dbo.dbUserFavourites ALTER COLUMN usrFavObjParam3 NVARCHAR (1000)
GO