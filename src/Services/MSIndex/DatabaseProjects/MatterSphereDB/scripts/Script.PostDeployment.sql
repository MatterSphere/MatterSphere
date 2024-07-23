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
:r .\Post\InitialData.sql
:r .\Post\CodeLookups.sql
:r .\Post\IndexingJobCreation.sql
:r .\Post\RolePermissionsRebuild.sql
Print 'Finished Post Deployment Scripts'