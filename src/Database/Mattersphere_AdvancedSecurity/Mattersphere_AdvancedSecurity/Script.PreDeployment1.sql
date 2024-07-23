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
:r .\Scripts\PreDeployment\Transfer_dbAssociates.sql
GO
:r .\Scripts\PreDeployment\Transfer_dbClient.sql
GO
:r .\Scripts\PreDeployment\Transfer_dbContact.sql
GO
:r .\Scripts\PreDeployment\Transfer_dbDocument.sql
GO
:r .\Scripts\PreDeployment\Transfer_dbDocumentPreview.sql
GO
:r .\Scripts\PreDeployment\Transfer_dbFile.sql
GO