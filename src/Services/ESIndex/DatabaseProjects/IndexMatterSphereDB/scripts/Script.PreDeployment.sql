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
:r .\PreScripts\ServiceBroker.sql
:r .\PreScripts\SetUpChangeTracking.sql
:r .\PreScripts\RelationshipTables.sql
:r .\PreScripts\DropStoredProcedures.sql

Print 'Finished Pre-Deployment'