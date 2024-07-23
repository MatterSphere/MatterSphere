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

:r .\PostDeployment\Delete_Tickford_Caption_From_CodeLookup.sql
GO
:r .\PostDeployment\dbObjectLocking_Amend_rowguid_column_to_UniqueIdentifier.sql
GO
:r .\PostDeployment\Change_CodeLookups_For_3e_MatterSphere.sql
GO
:r .\PostDeployment\Object_Type_Configuration_Menu_Update.sql
GO
:r .\PostDeployment\RemoveWalletFilterCodeLookup.sql
GO
:r .\PostDeployment\FileAccessDenies.sql
GO
:r .\PostDeployment\SetObjectNotSetPermissions.sql
GO
:r .\PostDeployment\CreatefwbsGrantStoredProcedures.sql
GO
:r .\PostDeployment\UpdatesprAssociateType.sql
GO
:r .\PostDeployment\UpdatesprClientType.sql
GO
:r .\PostDeployment\UpdatesprCommandCentreType.sql
GO
:r .\PostDeployment\UpdatesprContactType.sql
GO
:r .\PostDeployment\UpdatesprDocumentType.sql
GO
:r .\PostDeployment\UpdatesprFeeEarnerType.sql
GO
:r .\PostDeployment\UpdatesprFileType.sql
GO
:r .\PostDeployment\UpdatesprSearchListBuilder.sql
GO
:r .\PostDeployment\UpdatesprUserType.sql
GO
:r .\PostDeployment\DropADAssemblies.sql
GO
:r .\PostDeployment\AssemblyMatterCentreCLR.sql
GO
:r .\PostDeployment\AdvancedSecurityInheritedClientAccessDefault.sql
GO
:r .\PostDeployment\24377_Change_SP_Function.sql
GO
:r .\PostDeployment\WO27924_Changes.sql
GO
:r .\PostDeployment\dbUser_Add_usrPowerUserProfileID_Column.sql
GO
:r .\PostDeployment\28486_RecreateClusteredIndex.sql
GO
:r .\PostDeployment\28594_Precedent_object_locking.sql
GO
:r .\PostDeployment\28225_Prompts_that_are_not_a_code_lookup.sql
GO
:r .\PostDeployment\24357_GetCodeLookupDesc_vs_GetCodeLookupDescription.sql
GO
:r .\PostDeployment\31863_AddMemosCodeLookup.sql
GO
:r .\PostDeployment\31939_Toolbar_Update_specific_buttons_codelookups.sql
GO
:r .\PostDeployment\31944_ReplacePostRoomComponent.sql
GO
:r .\PostDeployment\Hardcode_removal.sql
GO
:r .\PostDeployment\32050_new_brand_ident_update_search_codelookup.sql
GO
:r .\PostDeployment\32681_DataGridView_SearchControl.sql
GO
:r .\PostDeployment\32816_dbEnquiryControl_InsertUpdate_ucRichBrowser.sql
GO
:r .\PostDeployment\32454_dbEnquiryControl_InsertUpdate_omsbrowser.sql
GO
:r .\PostDeployment\32318_Create_Matter_Folders_Tree_Enquiry_Form_Control.sql
GO
:r .\PostDeployment\35540_dbCommandCentreType_RemoveDictations.sql
GO
:r .\PostDeployment\32692_dbCommandCentreType_UpdateIcons.sql
GO
:r .\PostDeployment\32170_PrecedentAccessManagement.sql
GO
:r .\PostDeployment\32167_PrecedentManagerFavorites.sql
GO
:r .\PostDeployment\28760_Precedent_MinorCategory.sql
GO
:r .\PostDeployment\32682_FilterCodeLookup.sql
GO
:r .\PostDeployment\32120_Precedent_field_improvements.sql
GO
:r .\PostDeployment\33329_TemplateMatterTypesChanged.sql
GO
:r .\PostDeployment\32116_Precedent_Common_Field_Improvements.sql
GO
:r .\PostDeployment\33331_DocumentsTab_Refactoring.sql
GO
:r .\PostDeployment\34013_Add_AutoSuggest_Text_Box_Control.sql
GO
:r .\PostDeployment\34016_Create_Upload_Control.sql
GO
:r .\PostDeployment\34016_AddPictureBoxControlCodeLookups.sql
GO
:r .\PostDeployment\34014_Add_ucSearchConflicts_Control.sql
GO
:r .\PostDeployment\34413_General_Client_Details.sql
GO
:r .\PostDeployment\34427_Add_Placeholder_for_Basic_Inquiry_Controls.sql
GO
:r .\PostDeployment\34158_Appointments.sql
GO
:r .\PostDeployment\34158_HideButtons_Attribute_Added.sql
GO
:r .\PostDeployment\34855-DashboardCodeLookups.sql
GO
:r .\PostDeployment\34162_Tasks.sql
GO
:r .\PostDeployment\34161_CommandCentre_KeyDates.sql
GO
:r .\PostDeployment\33222_ContactType_AdditionalInformation.sql
GO
:r .\PostDeployment\34159_CommandCentre_Associates.sql
GO
:r .\PostDeployment\34163_CommandCentre_Undertakings.sql
GO
:r .\PostDeployment\35674_ItemAddingError.sql
GO
:r .\PostDeployment\36014_NewIndex_for_SCHCLIARCHLIST.sql
GO
:r .\PostDeployment\35736_ContactType_removal.sql
GO
:r .\PostDeployment\36089_NewIndexForSearchListSorting.sql
GO
:r .\PostDeployment\36038_States_for_address.sql
GO
:r .\PostDeployment\36179_Objects_With_uXML.sql
GO
:r .\PostDeployment\36131_Archiving_errors_install.sql
GO
:r .\PostDeployment\34240_CommandCentre_ContactManager.sql
GO
:r .\PostDeployment\31919_Trigger_tgrFileNumberGenerator.sql
GO
:r .\PostDeployment\32193_Trigger_generating_existing_number_of_Client.sql
GO
:r .\PostDeployment\36450_Changing_of_data_grid_columns.sql
GO
:r .\PostDeployment\35300_Legal_aid_issues.sql
GO
:r .\PostDeployment\36357_CommandCentre_TaskAssignment.sql
GO
:r .\PostDeployment\36043_Undertakings.sql  
GO
:r .\PostDeployment\36490_Configurable_Navigation_Panel.sql  
GO
:r .\PostDeployment\37523_Populate_dbFileFolder.sql
GO
:r .\PostDeployment\37178_OMS_code_lookups_fix.sql
GO
:r .\PostDeployment\36133_Change_Delete_Triggers.sql
GO
:r .\PostDeployment\38023_38026_Modify_Columns_3E.sql
GO
:r .\PostDeployment\39422_Dashboard_Refactoring.sql
GO
:r .\PostDeployment\40152_VirtualDrive_Performance.sql
GO
:r .\PostDeployment\39627_DocumentDialog_Timeout.sql
GO
:r .\PostDeployment\DocumentArchiving_Indexes.sql
GO
:r .\PostDeployment\43500_DocuSign_AddConstraints.sql
GO
--Add or update Jobs
:r .\PostDeployment\SQLAgentJobs\ClientDeleteJob.sql
GO
:r .\PostDeployment\SQLAgentJobs\ContactMergeDeleteJob.sql
GO
:r .\PostDeployment\SQLAgentJobs\MatterMergeDeleteJob.sql
GO
:r .\PostDeployment\SQLAgentJobs\OMSPermissions.sql
GO
--RolePermissionsRebuild and dbVersion must be the last two scripts!!!
:r .\PostDeployment\RolePermissionsRebuild.sql
GO
:r .\PostDeployment\dbVersion.sql
GO
Print 'Finished Post Deployment Scripts'
