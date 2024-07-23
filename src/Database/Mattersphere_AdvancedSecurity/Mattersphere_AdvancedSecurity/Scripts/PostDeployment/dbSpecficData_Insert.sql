IF NOT EXISTS ( SELECT spLookup FROM dbo.dbSpecificData WHERE spLookup = 'SECLEVEL' AND brID = 1 )
BEGIN
	INSERT dbo.dbSpecificData ( spLookup , brID , spData )
	VALUES ( 'SECLEVEL' , 1 , 1920 )
END
GO