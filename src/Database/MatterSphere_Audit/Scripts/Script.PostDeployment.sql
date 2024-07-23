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

:r .\PostDeployment\1_Audit_ColumnNameMappingData.sql
GO
:r .\PostDeployment\3_Disable_tgrLicSych.sql
GO
:r .\PostDeployment\4_SetUpAuditing.sql
GO
:r .\PostDeployment\5_Roles.sql
GO

Print 'Finished Post Deployment Scripts'




