-----------------------------------------------------------------
-- Replace parameters %1 and %2 with the destination folder paths
-----------------------------------------------------------------

CREATE DATABASE [OMSImport]
ON (NAME = N'OMSImport_Data', 
	FILENAME = N'%1\OMSImport_Data.MDF' , 
	SIZE = 30, FILEGROWTH = 10%) 
LOG ON (NAME = N'OMSImport_Log', 
	FILENAME = N'%2\OMSImport_Log.LDF' , 
	SIZE = 40, FILEGROWTH = 10%)
