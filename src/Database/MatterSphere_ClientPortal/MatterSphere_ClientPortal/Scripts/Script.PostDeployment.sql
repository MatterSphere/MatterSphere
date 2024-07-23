/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\Post\eEmailMessageConfigDefault.sql
:r .\Post\dbDocumentMCPv2.sql
:r .\Post\dbUser_MCPv2.sql
:r .\Post\ptl2FA.sql
:r .\Post\dbUser_FK_Trigger.sql
:r .\Post\RolePermissionsRebuild.sql
