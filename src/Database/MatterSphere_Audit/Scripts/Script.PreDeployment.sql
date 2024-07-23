/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\PreDeployment\1_DataTypeCorrection_text_to_nvarchar.sql
GO
:r .\PreDeployment\2_CreateAuditDatabase.sql
GO
:r .\PreDeployment\3_CreateAuditSchemaOnAuditDB.sql
GO
:r .\PreDeployment\4_CreateMSAUDITORlogin.sql
GO
:r .\PreDeployment\5_CreateIMPERSONATEPerms.sql
GO
:r .\PreDeployment\6_CreateIMPERSONATEPermsTrigger.sql
GO
:r .\PreDeployment\7_Columns.sql
GO

Print 'Finished Pre-Deployment Scripts'