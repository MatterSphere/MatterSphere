USE [master]
GO
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '$(UserName)')
	CREATE LOGIN [$(UserName)] FROM WINDOWS WITH DEFAULT_DATABASE=[master]
GO

USE [$(DatabaseName)]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '$(UserName)')
	CREATE USER [$(UserName)] FOR LOGIN [$(UserName)]
--CHECK IF ROLE ALREADY EXISTS
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'OMSServiceRole' AND type = 'R')
BEGIN
	CREATE ROLE [OMSServiceRole]
END

--NOW GRANT ACCESS
GRANT EXECUTE ON dbo.SetUserContext TO [OMSServiceRole]
GRANT VIEW DEFINITION ON SCHEMA :: dbo TO [OMSServiceRole]
GRANT VIEW DEFINITION ON SCHEMA :: config TO [OMSServiceRole]
GRANT VIEW DEFINITION ON SCHEMA :: [external] TO [OMSServiceRole]
GRANT CREATE PROCEDURE TO [OMSServiceRole]
GRANT ALTER ON SCHEMA :: [temp_external] TO [OMSServiceRole]
GRANT ALTER ON SCHEMA :: [temp_dbo] TO [OMSServiceRole]
GRANT EXECUTE ON SCHEMA :: [temp_external] TO [OMSServiceRole]
GRANT EXECUTE ON SCHEMA :: [temp_dbo] TO [OMSServiceRole]
GO

--ADD USER TO ROLE
ALTER ROLE [db_datareader] ADD MEMBER [$(UserName)]
ALTER ROLE [db_datawriter] ADD MEMBER [$(UserName)]
ALTER ROLE [OMSAdminRole] ADD MEMBER [$(UserName)]
ALTER ROLE [OMSRole] ADD MEMBER [$(UserName)]
ALTER ROLE [OMSServiceRole] ADD MEMBER [$(UserName)]


IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'OMSServiceSchema')
BEGIN
	DECLARE @sql NVARCHAR(MAX) =  N'CREATE SCHEMA OMSServiceSchema AUTHORIZATION [$(UserName)]'
	EXEC sp_executesql @sql
END

ALTER USER [$(UserName)] WITH DEFAULT_SCHEMA = OMSServiceSchema
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'sql_dependency_subscriber' AND type = 'R')
BEGIN
	EXEC sp_addrole 'sql_dependency_subscriber' 
END

GRANT CREATE QUEUE to OMSServiceRole 
GRANT CREATE SERVICE to OMSServiceRole 
GRANT REFERENCES on CONTRACT::[http://schemas.microsoft.com/SQL/Notifications/PostQueryNotification] to OMSServiceRole  
GRANT SUBSCRIBE QUERY NOTIFICATIONS TO OMSServiceRole  
GRANT RECEIVE ON QueryNotificationErrorsQueue TO OMSServiceRole 
EXEC sp_addrolemember 'sql_dependency_subscriber', 'OMSServiceRole'
GO
