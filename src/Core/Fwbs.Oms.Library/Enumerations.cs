using System;

namespace FWBS.OMS
{
    [Flags()]
    public enum ClientTables
    {
        None = 0,
        Files = 1,
        DefaultContacts = 2,
        ContactAddresses = 4,
        ContactNumbers = 8,
        ContactEmails = 16,
        Contacts = 32,
        TimeStats = 64,
        TimeRecords = 128
    }
    
    /// <summary>
	/// Contact General Type
	/// </summary>
	public enum OMSTypeContactGeneralType
	{
		Individual,
		Company
	}


	/// <summary>
	/// Nav Buttons Styles used in the OMSTypes
	/// </summary>
	public enum NavButtonStyle 
	{
		DarkGrey = 0,
		Grey = 2,
		Blue = 4,
		Cyan = 6
	}

	/// <summary>
	/// Document open modes.
	/// </summary>
	public enum DocOpenMode
	{
		View,
		Edit,
		Print
	}

	/// <summary>
	/// Activity Styles
	/// </summary>
	public enum ActivityStyles
	{
		FixedRateLegalAid,
		LegalAid,
		NotLegalAid
	}

	/// <summary>
	/// Time Record Time Statuses
	/// </summary>
	public enum TimeStatuses
	{
		Unbilled,
		PartBilled,
		FullyBilled,
		WrittenOff
	}
	
	/// <summary>
	/// Flags the appending of notes to be at the beginning or end.
	/// </summary>
	public enum NoteAppendingLocation
	{
		Beginning,
		End
	}

	/// <summary>
	/// Precedent Save Mode.
	/// </summary>
	[Flags()]
	public enum PrecSaveMode
	{
		None = 0,
		Quick = 1,
		Save = 3
	}

	/// <summary>
	/// The Remote Account Settings for Interactive Law
	/// </summary>
	[Flags()]
	public enum RemoteAccSettings
	{
		None = 0,
		Contact = 1,
		Enquiry = 2,
		Milestone = 4,
		Documents = 8,
		SMS = 16
	}

	/// <summary>
	/// Precedent Print Mode.
	/// </summary>
	[Flags()]
	public enum PrecPrintMode
	{
		None = 0,
		Print = 1,
		Fax = 2,
		Email = 4,
		Dialog = 9

	}

	/// <summary>
	/// A list of valid default system search lists
	/// </summary>
	public enum SystemSearchLists
	{
		None,
		AssociateFilter, //SYSASSOCFILTER
		Associates, //SYSASSOCLIST
		Appointments, //APPOINTMENTS
		Contact, //SYSCONTACTLIST
		File, //SYSFILELIST
		TimeRecording, //SYSTIMEREC
		FieldCommon, //SYSFLDCOMMON
		FieldAssociates, //SYSFLDASSOC
		FieldAppointments, //SYSFLDAPP
		FieldExtendedData, //SYSFLDEXTLIST
		DocumentTimeRecording, //SYSDOCTIMEREC
		Precedent, //SYSPRECLIST
        PrecedentFavourites, //SCHSYSPRECFAV
        PrecedentFilter, //SYSPRECFILTER
		RemoteAssociates, //ASSOCREMOTE
		Help, //SYSHELP
		SearchListPicker, //SCHPICKER
		ReportPicker, // RPTPICKER
		DocumentAttachments, //DOCUMENTATT
		Address, //Address
		SearchContacts, //CONTACTS
		PackageCodeLookups, //PDCDLOOKUPS
		PackageEnquiry, //PDENQUIRY
		PackageExtendedData, //PDEXTEND
		PackageObjects, //PDOBJECTS
        PackagePrecedents, //PDPRECLIST
		PackageReports, //PDREPORTS
		PackageScripts, //PDSCRIPTS
		PackageSearchLists, //PDSEARCHLIST
		PackageSqlScripts, //PDSQLSCRIPTS
		PackageDataLists,
		PackageDataPackages,        
		OyezAliases, //OYEZALIAS
		LaserAliases, //LASERALIAS
		AddressPostcode,
		DocumentDuplicates,	//DOCUMENTSDUP
        DocumentDuplicatesExternal,	
        ContactInfoUsageCheck,
		FileAssociateCopy, //SCHFILASSOCCOPY
        ReportServerPicker, // SCHRSRPTPICKER
        DocumentLog, // SCHDOCLOG
        AssociatesShort,
        WorkflowPicker,
        PackageWorkflows,
    }

	/// <summary>
	/// A list of valid default sytem search list groups
	/// </summary>
	public enum SystemSearchListGroups
	{
		None,
		Client, //CLIENT
		ClientConflict, //CLIENTCONFLICT
		ContactAssociate, //CONTACTASS
		ContactConflict, //CONTACT
		SearchManagerContact, //CONTACTSM
		SearchManagerClient, //CLSM
		SearchManagerFile, //CLFILESM
		Package, //PACKAGE
		Address, //ADDRESS
		User, //USER
		FeeEarner, //FEEEARNER
		Document, //DOCUMENT
		ClientDocument, //CLIDOCUMENT
		ClientFile, //CLIENT&FILE
		FindCodeLookup, // FINDCDLK
		DocumentClient,
        DocumentContact,
        DocumentAll,
		DocumentLast,
        DocumentCheckedOut,
        DocumentLocal,
		SelectMilestone,
		AddressService,
        DocumentLatestUpdate,
        DocumentLatestOpened,
        FMSelectMilestone
    }

	
	/// <summary>
	/// A list of valid default system forms.
	/// </summary>
    public enum SystemForms
    {
        UserSettings,
        UserWizard,
        UserImportWizard,
        UserAdmin,
        FeeEarnerWizard,
        FeeEarnerAdmin,
        AddressWizard,
        AddressEdit,
        ClientWizard,
        ClientContactEdit,
        AssociateWizard,
        AssociateEdit,
        ContactWizard,
        FileWizard,
        ContactGroupWizard,
        PrecedentEdit,
        PrecedentWizard,
        PrecedentSearch,
        FaxTransmission,
        SaveDocumentWizard,
        TimeRecordEdit,
        SMS,
        SMSEdit,
        DocReceipt,
        SavePrecedentWizard,
        DateWizard,
        TaskWizard,
        TaskEdit,
        TaskItem,
        AppointmentWizard,
        AppointmentEdit,
        AppointmentItem,
        Email,
        TelephoneNumber,
        FileReview,
        FaxTransmissionNoAssociate,
        FinanceLogEdit,
        FinanceLogWizard,
        None,
        RegisterOMSObject,
        ManualTimeWizard,
        AddApplication,
        SecurityCheck,
        FileFunding,
        PreClientType,
        RemoteAccountWizard,
        ClientTypePicker,
        OriginalClientType,
        ContactTypePicker,
        AssociateTypePicker,
        FileTypePicker,
        MenuFolder,
        MenuItem,
        DocumentEdit,
        EmailEdit,
        TelephoneNumberEdit,
        LicenseUpload,
        LicenseDownload,
        Milestones,
        SystemEdit,
        BranchEdit,
        UFNInformation,
        FilePhaseEdit,
        FilePhaseWizard,
        AddDistributedAssembly,
        AddDistributedModule,
        ConsoleItem
    }

	/// <summary>
	/// Status acknowledgement for Process Job Statuses.
	/// </summary>
	public enum ProcessJobStatus
	{
		Finished,
		Error,
		PauseJobs
	}

	internal enum JobStatus
	{
		Live = 0,
		Completed = 1,
		Cancelled = 2,
		Adopted = 3
	}

	/// <summary>
	/// OMS Object Types
	/// </summary>
	public enum OMSObjectTypes
	{
		Addin,
		Enquiry,
		List,
		ListGroup,
		ExtData
	}

	/// <summary>
	/// System directory location list.
	/// </summary>
	public enum SystemDirectories
	{
		OMDocuments,
		OMPrecedents,
		OMArchive
	}

	/// <summary>
	/// Incoming or outgoing storage / document direction.
	/// </summary>
	public enum DocumentDirection
	{
		Out = 0,
		In = 1
	}
	/// <summary>
	/// Different types of xml schema definition file return types.
	/// </summary>
	public enum XSDTypes
	{
		DataSet,
		XmlDocument
	}

	/// <summary>
	/// Valid User Interface API consumers.
	/// </summary>
	public enum UIClientType
	{
		Windows = 0,
		Web = 1,
		PDA = 2
	}

	/// <summary>
	/// Extended data compatible modes (Adding or Editing).
	/// </summary>
	[Flags()]
	public enum ExtendedDataMode
	{
		/// <summary>
		/// The extended data item can be viewed.
		/// </summary>
		View = 1,
		/// <summary>
		/// The extended data item can be added.
		/// </summary>
		Add = 2,
		/// <summary>
		/// The extended data item can be edited.
		/// </summary>
		Edit = 4,
		/// <summary>
		/// The extended data item can be deleted.
		/// </summary>
		Delete = 8

	}


}

namespace FWBS.OMS.SourceEngine
{
	/// <summary>
	/// Source types used for fetching data or running commands.
	/// </summary>
	[Flags()]
	public enum SourceType
	{
		Class = 1,
		Object = 2,
		Dynamic = 4,
		Linked = 8,
		OMS = 16,
		Instance = 32
	}
}

namespace FWBS.OMS
{

	public enum HelpIndexes
	{
		InvalidOMSUser = 1,
		InvalidOMSPassword = 2,
		InactiveOMSUser = 3,
		NoCompanyRegistration = 8,
		NotLoggedIn = 9,
		TerminalNotRegistered = 10,
		SoftwareInactive = 11,
		AllSystemLicensesUsed = 12,
		DemoVersionExpiring = 13,
		DemoVersionExpired = 14,
		LicenseNotRegistered = 15,
		LicenseInfoCorrupt = 16,
		OldPasswordDiffers = 17,
		UserNotAFeeEarner = 18,
		UserDoesNotWorkForAnyone = 19,
		InvalidBranchSet = 20,
		AddressNotAssigned = 21,
		EnquiryDoesNotExist = 23,
		EnquiryDuplicateKey = 24,
		DefaultPrinterNotSet = 25,
		EnquiryHeaderCorrupt = 27,
		EnquiryNoQuestions = 28,
		EnquiryNoDataSection = 29,
		EnquiryInvalidField = 30,
		EnquiryNoUnderlyingData = 31,
		EnquiryNullBusinessObject = 32,
		EnquiryInvalidBusinessObjectType = 33,
		EnquiryPropertyInvokeError = 34,
		EnquiryConstructorInvokeError = 35,
		EnquiryRequiredField = 36,
		EnquiryDoesNotSupportMode = 37,
		EnquiryDoesNotSupportBinding = 38,
		EnquiryMethodInvokeError = 39,
		EnquiryCannotSaveEnquiry = 40,
		EnquiryOnlyInDesignMode = 41,
		CodeLookupDoesNotExist = 42,
		ExtendedDataDoesNotExist = 43,
		SourceTypeNotSupported = 44,
		CommandBarDoesNotExist = 45,
		CannotConnectToSession = 46,
		AssemblyNotOMSClient = 47,
		SearchListDoesNotExist = 48,
		EnquiryDoesNotSupportModeAndBinding = 49,
		ExtendedDataInvalidConnection = 50,
		ExtendedDataMode = 51,
		SourceMethodInvokeError = 52,
		SourceConstructorInvokeError = 53,
		SearchInvalidReturnField = 54,
		SearchNoItemSelected = 55,
		SearchNotCompatibleResultset = 56,
		ClientTypeDoesNotExist = 57,
		ContactTypeDoesNotExist = 58,
		FileTypeDoesNotExist = 59,
		EnquiryEngineVersionDated = 60,
		SourceTypeDoesNotSupportSchema = 61,
		ScriptPathNeeded = 62,
		ScriptNotFound = 63,
		PrecedentNotFound = 64,
		DocumentNotFound = 65,
		DirectoryNotSetup = 66,
		InvalidDocumentPassword = 67,
		ItemNotInStorage = 68,
		StorageLocationProviderItemNotInstalled = 69,
		ProviderDoesNotSupportType = 70,
		InvalidStoreItemType = 71,
		StoreItemSaveError = 72,
		SearchNoColumnsHaveBeenSet = 73,
		SearchNoCode = 74,
		SearchDataBuilderIncomplete = 75,
		EnquiryNoCode = 76,
		SourceNoSourceSet = 77,
		SourceTypeInvalid = 78,
		ContactNotFound = 79,
		ApplicationNotFound =80,
		OMSFileNotFound = 81,
		PasswordRequestCancelled = 82,
		ApplicationNotCreateable = 83,
		ClientNotFound = 84,
		ClientNotInitialised = 85,
		OMSFileNotInitialised = 86,
		ContactNotInitialised = 87,
		AddressNotAssignedToContact = 88,
		CommandBarRunCommandNotSet = 89,
		NoActiveDocument = 90,
		NotLicensedForModule = 91,
		AllLicensesAllocated = 92,
		DataListNoCodeSet = 93,
		ExtendedDataCodeAlreadyExists = 94,
		UnabletoSetDocVariable = 95,
		ExtendedDataNoCode = 96,
		OMSTypeNoCode = 97,
		OMSTypeCodeDoesNotExist = 98,
		OMSCodeCannotBeChangeWhenSet = 99,
		UnableToGetAssociate = 100,
		OMSDescriptionNotSet = 101,
		FeeEarnerNotResponsible = 102,
		UnableToGetActiveType = 103,
		PrecedentFileNotFound = 104,
		CurrentClientMustExist = 105,
		LimitedPreClient	= 106,
		FundTypeDoesNotExist = 107,
		SearchNoReturnFields = 108,
		AccessCiriticalData = 109,
		TimeActivityNotSet = 110,
		DocumentFileNotFound = 111,
		DataListCodeDoesNotExist = 112,
		AvailableWhenNew = 113,
		SearchMissingButton = 114,
		EnquiryUpdateCancelled = 115,
		ConfirmationPasswordDiffers = 116,
		ServiceUser = 117
	}
}

namespace FWBS.OMS.EnquiryEngine
{
	/// <summary>
	/// Enquiry mode (Adding or Editing).
	/// </summary>
	[Flags()]
	public enum EnquiryMode
	{
		None = 0,
		/// <summary>
		/// Tells the enquiry engine object to add a new entity.
		/// </summary>
		Add = 1,
		/// <summary>
		/// Tells the enquiry engine object to edit a new entity.
		/// </summary>
		Edit = 2,
		/// <summary>
		/// Creates an enquiry object with just search capabilities.
		/// </summary>
		Search = 4
	}



	/// <summary>
	/// Enquiry binding type that tells the enquiry engine where and how
	/// it is going to update and get the data that it is given.
	/// </summary>
	public enum EnquiryBinding
	{
		/// <summary>
		/// No database manipulation used at all.  This can be used for rendered
		/// forms that will be manipulated in code.
		/// </summary>
		Unbound = 1,
		/// <summary>
		/// Bind straight to the database using a SQL select statement to receive data and DML statements to update or insert.
		/// </summary>
		Bound = 2,
		/// <summary>
		/// Creates a IEnquiryCompatible object to manipulate the data store.  The object itself
		/// will control how it gets the data and how it updates that data.
		/// </summary>
		BusinessMapping = 3,
	}


	/// <summary>
	/// Enquiry code lookup types.
	/// </summary>
	public enum EnquiryCodeLookupType
	{
		/// <summary>
		/// Enquiry header type.
		/// </summary>
		EnqHeader,
		/// <summary>
		/// Welcome wizard type.
		/// </summary>
		EnqWelcome,
		/// <summary>
		/// Descriptive wizard page header type.
		/// </summary>
		EnqPage,
		/// <summary>
		/// Brief wizard / tab page header Type.
		/// </summary>
		EnqTab,
		/// <summary>
		/// Question / caption text type.
		/// </summary>
		EnqQuestion,
		/// <summary>
		/// Data list explanation /name type.
		/// </summary>
		EnqDataList,
		/// <summary>
		/// Command description types.
		/// </summary>
		EnqCommand,
		/// <summary>
		/// The control name.
		/// </summary>
		EnqControl,
		/// <summary>
		/// The control grouping type.
		/// </summary>
		EnqCtrlGroup
	}
}


namespace FWBS.OMS.FileManagement
{

    public enum ActionsOrderType
    {
        FileActions1st,
        TaskActions1st
    }

    public enum ActionsToDisplay
    {
        FileAndTask,
        FileOnly,
        TaskOnly,
        None
    }
}

