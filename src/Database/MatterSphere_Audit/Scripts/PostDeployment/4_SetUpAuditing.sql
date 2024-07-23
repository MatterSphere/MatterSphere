-- ADD THESE ROWS TO A SCRIPT WHICH WILL BE RUN AT PACKAGE INSTALL

DECLARE @AUDIT_DB_NAME SYSNAME
DECLARE @SCHEMANAME SYSNAME
SET @AUDIT_DB_NAME = DB_NAME () + '_Audit'

select @SCHEMANAME=OBJECT_SCHEMA_NAME(object_id) from sys.objects where type = 'U' and name = 'dbClient'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = @SCHEMANAME,      @TABLE_NAME  = 'dbClient'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',       @TABLE_NAME  = 'dbClientContacts'

select @SCHEMANAME=OBJECT_SCHEMA_NAME(object_id) from sys.objects where type = 'U' and name = 'dbContact'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = @SCHEMANAME,      @TABLE_NAME  = 'dbContact'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',      @TABLE_NAME  = 'dbContactIndividual'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',       @TABLE_NAME  = 'dbContactAddresses'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',       @TABLE_NAME  = 'dbAddress'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',       @TABLE_NAME  = 'dbContactNumbers'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = 'dbo',       @TABLE_NAME  = 'dbContactEmails'

select @SCHEMANAME=OBJECT_SCHEMA_NAME(object_id) from sys.objects where type = 'U' and name = 'dbFile'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = @SCHEMANAME,      @TABLE_NAME  = 'dbFile'

select @SCHEMANAME=OBJECT_SCHEMA_NAME(object_id) from sys.objects where type = 'U' and name = 'dbAssociates'
exec [audit].[CreateAudit] @DB_NAME = @AUDIT_DB_NAME,      @SCHEMA_NAME = @SCHEMANAME,      @TABLE_NAME  = 'dbAssociates'


