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


	:r .\dbVersion.sql
	:r .\AllInternalGroup.sql
	:r .\DefaultPolicy.sql
	:r .\ValenciaScriptChanges\dbCaptainsLogType_LOGIN_LOGOFF.sql
	:r .\ValenciaScriptChanges\dbUser_SecurityID.sql
	:r .\ValenciaScriptChanges\UserType_Remote.sql
	:r .\V7Updates\AdminCodeLookups.sql
	:r .\V7Updates\DBContactCompany.sql
	:r .\V7Updates\DBEnquiry_enqFields.sql
	:r .\V7Updates\eEmailMessageConfigDefault.sql
	:r .\V5ScriptChanges\DBApi_Data.sql
	:r .\V5ScriptChanges\DBScriptType_Data.sql
	:r .\SilverstoneScriptChanges\Populate_dbBizDays_en_gb.sql
	:r .\Security\dbCodeLookups.sql
	:r .\Security\ObjectPolicyConfig_Update.sql
	:r .\NewInstallScripts\dbRegInfo_FullInstall.sql
	:r .\NewInstallScripts\Default_AdminMenu.sql
	:r .\NewInstallScripts\Default_API.sql
	:r .\NewInstallScripts\Default_AssociateType.sql
	:r .\NewInstallScripts\Default_ClientType.sql
	:r .\NewInstallScripts\Default_CodeLookup.sql
	:r .\NewInstallScripts\Default_CommandBar.sql
	:r .\NewInstallScripts\Default_CommandCentreType.sql
	:r .\NewInstallScripts\Default_ContactType.sql
	:r .\NewInstallScripts\Default_Country.sql
	:r .\NewInstallScripts\Default_Currency.sql
	:r .\NewInstallScripts\Default_Department.sql
	:r .\NewInstallScripts\Default_EnquiryDataList.sql
	:r .\NewInstallScripts\Default_Enums.sql
	:r .\NewInstallScripts\Default_FeeEarnerType.sql
	:r .\NewInstallScripts\Default_FileType.sql
	:r .\NewInstallScripts\Default_Language.sql
	:r .\NewInstallScripts\Default_Printer.sql
	:r .\NewInstallScripts\Default_ScriptType.sql
	:r .\NewInstallScripts\Default_SecurityRole.sql
	:r .\NewInstallScripts\Default_UserType.sql
	:r .\AdvSecurity\UpdateUserGroupContact.sql
	:r .\AdvSecurity\UpdateUserGroupDocument.sql
	:r .\AdvSecurity\UpdateUserGroupFile.sql
	:r .\NewInstallScripts\Default_Address.sql
	:r .\NewInstallScripts\Default_CommandBarControl.sql
	:r .\NewInstallScripts\Default_FundType.sql
	:r .\NewInstallScripts\Default_Branch.sql
	:r .\NewInstallScripts\Default_EnquiryDataList.sql
	:r .\NewInstallScripts\Default_Seeds.sql
	:r .\NewInstallScripts\Default_Users.sql
	:r .\ValenciaScriptChanges\dbUser_AccessType_Populate.sql
	:r .\ValenciaScriptChanges\dbUser_SecurityID_Populate.sql
	:r .\V5ScriptChanges\DBAPIFWBSUPdate.sql
	:r .\Security\SecurityPermissions.sql
	:r .\SQLAgentJobs\ClientDeleteJob.sql
	:r .\SQLAgentJobs\ContactMergeDeleteJob.sql
	:r .\SQLAgentJobs\MatterMergeDeleteJob.sql
	:r .\SilverstoneScriptChanges\dbAssociateType_typeCode_SOURCE.sql
	:r .\SilverstoneScriptChanges\dbCodeLookup_SUBASSOC_SOURCE.sql
	:r .\SilverstoneScriptChanges\dbCommandBarControl_MAIN_FILEALL.sql
	:r .\SilverstoneScriptChanges\dbCommandBarControl_Update_to_ctrlLevel.sql
	:r .\Security\RunPermissions.sql
	:r .\NewInstallScripts\OMSPermissions.sql
	:r .\dbEnquiryControls\ctrlIDs.sql
	:r .\AdvSecurity\UpdateUserGroup_Inheritance.sql
	:r .\V7Updates\VersionCodeLookup_AK.sql
	:r .\V8Updates\Delete_Tickford_Caption_From_CodeLookup.sql
	:r .\V8Updates\RolePermissionsRebuild.sql
	:r .\V8Updates\dbObjectLocking_Amend_rowguid_column_to_UniqueIdentifier.sql
	:r .\V8Updates\Change_CodeLookups_For_3e_MatterSphere.sql
	:r .\V8Updates\Object_Type_Configuration_Menu_Update.sql
	:r .\V8Updates\RemoveWalletFilterCodeLookup.sql
	--:r .\AdvSecurity\DirectoryServicesUpdate.sql	
	:r .\AdvancedSecurityInheritedClientAccessDefault.sql
	:r .\27625_COMPATIBILITY_LEVEL.sql	
	:r .\NewInstallScripts\Default_Dashboard.sql
print 'Script Finishing'