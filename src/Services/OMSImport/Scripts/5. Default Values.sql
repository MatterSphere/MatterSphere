-----------------------------------------------------------------
-- Run on newly created OMSImport Database
-----------------------------------------------------------------

-- Insert default values into OMSImport db

INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'DEPT' , 'DEFDEPT' , 'Import' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'FILETYPE' , 'IMPORT' , 'Import' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'FUNDTYPE' , 'NOCHG' , 'No Charge' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'SOURCE' , 'IMPORT' , 'Import' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'UKNADDRESS' , '-1' , 'Address unknown' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'USER' , '-200' , 'Import' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'USERCULTURE' , 'en-gb' , 'English - (Great Britain)' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'USERISOCODE' , 'GBP' , '(Great British Pounds)' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'CONTTYPE' , 'CONTACT' , 'Contact' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'CLTYPE' , 'IMPORT' , 'Import' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'DOCWALLET' , 'GENERAL' , 'General' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'DOCTYPE' , 'LETTERHEAD', 'Letterhead' )
GO
INSERT [OMSImport].[dbo].[Defaults] ( [defType] , [defCode] , [defDesc] )
VALUES ( 'USERBRANCH' , '1' , 'User Default Branch' )
GO