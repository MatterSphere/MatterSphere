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

:r .\PreDeployment\dbTokens.sql
:r .\PreDeployment\Add_Column_docFolderGuid.sql
:r .\PreDeployment\dbMSConfig_OMS2k-update_to_30_stages.sql
:r .\PreDeployment\dbMSData_OMS2k-update_to_30_stages.sql
:r .\PreDeployment\vwdbfile.sql
:r .\PreDeployment\vwdbDocument.sql
:r .\PreDeployment\24377_Add_Column_Indexes.sql
:r .\PreDeployment\vwdbClient.sql
:r .\PreDeployment\vwdbDocumentPreview.sql
:r .\PreDeployment\vwdbAssociates.sql
:r .\PreDeployment\vwdbContact.sql
:r .\PreDeployment\28760_Precedent_MinorCategory.sql
:r .\PreDeployment\32360_AddNewColumnForDashboards.sql
:r .\PreDeployment\34800_MatterType_Add_MinorCategory.sql
:r .\PreDeployment\22027_Expand_contRegCoName_dbContactCompany.sql
:r .\PreDeployment\31864_Expand_usrFavObjParam3_dbUserFavourites.sql
:r .\PreDeployment\38302_Expand_dbAssociates_assocHeading.sql
:r .\PreDeployment\34161_CommandCentre_KeyDates.sql
:r .\PreDeployment\36179_uXML.sql
:r .\PreDeployment\38026_uAddressLine.sql
:r .\PreDeployment\38026_uPostcode.sql
:r .\PreDeployment\42468_Bundler_Add_Columns.sql
:r .\PreDeployment\43016_AdvancedSecurity_InactiveGroup.sql
:r .\PreDeployment\45697_External_Access_Denies_Views.sql
:r .\PreDeployment\43542_Add_Column_to_track_deleted_entities.sql
:r .\PreDeployment\45385_dbFileManagementApplication.sql
Print 'Finished Pre-Deployment Scripts'