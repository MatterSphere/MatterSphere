using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Script;
using FWBS.OMS.Teams;


namespace FWBS.OMS
{

    using DocumentManagement.Storage;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.Security;
    using FWBS.OMS.Security.Permissions;

    /// <summary>
    /// An object that describes a a storage item / document file template that can be used
    /// to create a storage item.
    /// </summary>
    [Security.SecurableType("PRECEDENT")]
	public class Precedent : PasswordProtectedBase, IDisposable, IEnquiryCompatible, IStorageItem, Security.ISecurable, IStorageItemVersionable, IStorageItemDuplication, IStorageItemLockable
	{
		#region Events
		
		/// <summary>
		/// An event that gets raised when the script has be changed within the object.
		/// </summary>
		public event EventHandler ScriptChanged = null;

		/// <summary>
		/// An event that gets raised when a value within the precedent has changed.
		/// </summary>
		public event EventHandler Changed = null;

		/// <summary>
		/// An event that gets raised when a client and file has been associated with precedent.
		/// </summary>
		public event PrecedentLinkCancelHandler Validating = null;

		/// <summary>
		/// An event that gets raised when just before a document has been saved based on this 
		/// current precedent object.
		/// </summary>
		public event DocumentSavingHandler DocumentSaving = null;

		/// <summary>
		/// An event that gets raised when a document has been saved based on this current precedent.
		/// </summary>
		public event DocumentSavedHandler DocumentSaved = null;

		/// <summary>
		/// An event that gets raised when the precedent has been loaded.
		/// </summary>
		public event EventHandler Load = null;

		/// <summary>
		/// An event that gets raised when the precedent is about to go through a first stage merge.
		/// </summary>
		public event PrecedentLinkCancelHandler Merging = null;

		/// <summary>
		/// An event that gets raised when the precedent is about to go through a second stage merge.
		/// </summary>
		public event PrecedentSecondStageMergingEventHandler SecondStageMerging = null;

		/// <summary>
		/// An event that gets raised when the physical document has been saved
		/// </summary>
		public event PhysicalDocumentSavedEventHandler PhysicalDocumentSaved = null;

		#endregion
	   
		#region Event Methods

		/// <summary>
		/// Calls the script changed event.
		/// </summary>
		protected virtual void OnScriptChanged() 
		{
			if (ScriptChanged != null)
				ScriptChanged(this, EventArgs.Empty);
		}	


		/// <summary>
		/// Calls the data changed event.
		/// </summary>
		protected internal virtual void OnChanged() 
		{
			if (Changed != null)
				Changed(this, EventArgs.Empty);
		}	

		/// <summary>
		/// Calls the validating event.
		/// </summary>
		protected internal virtual void OnValidating(PrecedentLinkCancelEventArgs e) 
		{
			if (Validating != null)
				Validating(this, e);
		}

		/// <summary>
		/// Calls the precedent load event.
		/// </summary>
		protected internal virtual void OnLoad()
		{
			if (Load != null)
				Load(this, EventArgs.Empty);
		}

		/// <summary>
		/// Calls the document saving event.
		/// </summary>
		protected internal virtual void OnDocumentSaving(DocumentSavingEventArgs e)
		{
			if (DocumentSaving != null)
				DocumentSaving(this, e);
		}

		/// <summary>
		/// Calls the document saved event.
		/// </summary>
		protected internal virtual void OnDocumentSaved(DocumentSavedEventArgs e)
		{
			if (DocumentSaved != null)
				DocumentSaved(this, e);
		}


		/// <summary>
		/// Calls the precedent merging event.
		/// </summary>
		protected internal virtual void OnMerging(PrecedentLinkCancelEventArgs e)
		{
			if (ExecuteWizard(e.PrecLink) == false)
			{
				e.Cancel = true;
				return;
			}

			if (Merging != null)
				Merging(this, e);
		}


		/// <summary>
		/// Calls the precedent second stage before merge event.
		/// </summary>
		protected internal virtual void OnSecondStageMerging(PrecedentSecondStageMergingEventArgs e)
		{
			if (SecondStageMerging != null)
				SecondStageMerging(this, e);
		}

		/// <summary>
		/// Calls the document saved event.
		/// </summary>
		protected internal virtual void OnPhysicalDocumentSaved(PhysicalDocumentSavedEventArgs e)
		{
			if (PhysicalDocumentSaved != null)
				PhysicalDocumentSaved(this, e);
		}

		#endregion

		#region Fields
		/// <summary>
		/// Code Lookup to Specifiy the Extended Data to use for the Auto Draw Form
		/// </summary>
		private CodeLookupDisplayReadOnly _extcode = null;
		/// <summary>
		/// Code Lookup Outbound Activity Code
		/// </summary>
		private CodeLookupDisplayReadOnly _activity = null;
		/// <summary>
		/// Code Lookup Inbound Activity Code
		/// </summary>
		private CodeLookupDisplayReadOnly _iwdactivity = null;
		/// <summary>
		/// Code Lookup Libaray
		/// </summary>
		private CodeLookupDisplay _library = null;
		/// <summary>
		/// Code Lookup Cataegory
		/// </summary>
		private CodeLookupDisplay _category = null;
		/// <summary>
		/// Code Lookup Precedent Type
		/// </summary>
		private CodeLookupDisplay _doctype = null;
		/// <summary>
		/// Code Lookup Sub Category
		/// </summary>
		private CodeLookupDisplay _subcategory = null;
        /// <summary>
        /// Code Lookup Sub Category
        /// </summary>
        private CodeLookupDisplay _minorcategory = null;
        /// <summary>
        /// Code Lookup Contact Type
        /// </summary>
        private CodeLookupDisplay _contacttype = null;
		/// <summary>
		/// Code Lookup Associate Type
		/// </summary>
		private CodeLookupDisplay _assoctype = null;
		/// <summary>
		/// Auto wizard object type.
		/// </summary>
		private CodeLookupDisplayReadOnly _autowiztype = null;
		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataSet _precedent = null;
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		private const string Sql = "select * from dbprecedents";
		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		private const string Table = "PRECEDENT";
		/// <summary>
		/// If the Precedent is Exculsive to Branch
		/// </summary>
		private Branch _branch = null;
		/// <summary>
		/// Holds a reference to the script object for performing Precedent Events.
		/// </summary>
		private Script.ScriptGen _script = null;
		/// <summary>
		/// Holds a collection of multi precent items.
		/// </summary>
		private Precedent.MultiPrecedentCollection _multiprec = null;
        private PrecedentsTeamsAccessCollection _teamsaccess = null;

        /// <summary>
        /// Before Change Fields
        /// </summary>
        private string before_title;
		private string before_type;
		private string before_library;
		private string before_category;
		private string before_subcategory;
        private string before_minorcategory;


        private PrecType type = null;

        /// <summary>
		/// The main part of system postfix string.
        /// min system postfix string: ' (DEL:1)'.
        /// max system postfix string: ' (DEL:500)'.
		/// </summary>
		private const string ArchiveDelPostfixPart = "DEL:";

		#endregion

		#region Methods

		/// <summary>
		/// Executes tha attached Precedent wizard.
		/// </summary>
		private bool ExecuteWizard(PrecedentLink link)
		{
			//Only run a wizard if a wizard has been selected for the precedent.
			if (AutoWizard != String.Empty)
			{
				Enquiry enq = null;
				IEnquiryCompatible obj = null;

				//Bind the wizard to a specific object for a business mapped object.  a blank property
				//will allow for a custom bound / unbound form.
				switch (AutoWizardType.Code)
				{
					case "CONTACT":
						obj = link.Associate.Contact;
						break;
					case "CLIENT":
						obj = link.Associate.OMSFile.Client;
						break;
					case "FILE":
						obj = link.Associate.OMSFile;
						break;
					case "ASSOCIATE":
						obj = link.Associate;
						break;
					default:
						obj = null;
						break;

				}

				//Depending on the bound wizard type, created the enquiry form and
				//pass the prec links jobs parameters to it.
				if (obj == null)
					enq = Enquiry.GetEnquiry(AutoWizard,null, EnquiryMode.Add, false, link.Params);
				else
					enq = Enquiry.GetEnquiry(AutoWizard,null, obj, false, link.Params);

				//Pass the enquiry form / wizard to the calling UI.
				ShowEnquiryEventArgs enqe = new ShowEnquiryEventArgs(enq);
				Session.CurrentSession.OnShowWizard(enqe);
				
				if (enqe.Cancel || enqe.ReturnObject == null)
					return false;
				else
				{
					//If the return value is a data table (a bound / unbound form then
					//add each column and value to the precedent link parameters collection
					//so that it could be merged.
					if (enqe.ReturnObject is DataTable)
					{
						DataTable dt = (DataTable)enqe.ReturnObject;
						foreach (DataColumn col in dt.Columns)
						{
							link.Params.Add(col.ColumnName.ToUpper(), dt.Rows[0][col]);
						}
					}
					link.ApplyParams();
					return true;
				}
			}
			if (Convert.ToString(GetXmlProperty("AutoExtendedData",String.Empty)) != String.Empty)
			{
				OmsObject obj = new OmsObject(Convert.ToString(GetXmlProperty("AutoExtendedData",String.Empty)));
				object objc = null;
				switch (obj.TypeCompatible.ToUpper())
				{
					case "FWBS.OMS.CONTACT":
						objc = link.Associate.Contact;
						break;
					case "FWBS.OMS.CLIENT":
						objc = link.Associate.OMSFile.Client;
						break;
					case "FWBS.OMS.OMSFILE":
						objc = link.Associate.OMSFile;
						break;
					case "FWBS.OMS.ASSOCIATE":
						objc = link.Associate;
						break;
					case "FWBS.OMS.USER":
						objc = Session.CurrentSession.CurrentUser;
						break;
					case "FWBS.OMS.FEEEARNER":
						objc = Session.CurrentSession.CurrentFeeEarner;
						break;
					default:
						objc = null;
						break;
				}

				if (objc != null)
				{
					if (objc is IExtendedDataCompatible)
					{
						ExtendedData ext = ((IExtendedDataCompatible)objc).ExtendedData[obj.Windows];
						//Pass the enquiry form / wizard to the calling UI.
						ShowExtendedDataEventArgs enqe = new ShowExtendedDataEventArgs(ext.Code,(FWBS.OMS.Interfaces.IEnquiryCompatible)objc);
						Session.CurrentSession.OnShowExtendedData(enqe);
						if (enqe.Cancel)
							return false;
						else
							return true;
					}
					else
						return false;
				}
				else
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets the time recording activity code based on the document direction given.
		/// </summary>
		/// <param name="docDirection">The direction of the document.</param>
		/// <returns>The time recording activity code.</returns>
		public string GetTimeActivityCode(DocumentDirection docDirection)
		{
			if (docDirection == DocumentDirection.In)
			{
				if (TimeRecordingActivityInwardCode == "")
					return TimeRecordingActivityCode;
				else
					return TimeRecordingActivityInwardCode;
			}
			else
				return TimeRecordingActivityCode;
		}

		/// <summary>
		/// Create a New Script object for the Precedent
		/// </summary>
		public void NewScript()
		{
			_script = null;
		}

		/// <summary>
		/// Descriptive for the precedent object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Title + " - " + this.Description;
		}

		#endregion

		#region Constructors
		
		/// <summary>
		/// Creates a new storage item. 		
		/// </summary>
		internal Precedent()
		{
			isnew_storeitem = true;
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Add a new record.
			Global.CreateBlankRecord(ref dt , true);

			if (_precedent == null) _precedent = new DataSet("PRECEDENTINFO");
			_precedent.Tables.Add(dt);


			//Set the created by and created date of the item.
			this.SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
			this.SetExtraInfo("Created", DateTime.Now);
			//Set the default values.
			TextOnly = false;
			Reminder = false;
			ReminderDayCount = 0;
			ReminderReviewDayCount = -1;
			TimeRecordingUnits = 0;
			AllowSecondStageMerge = false;
			MilestoneChangeAutomatic = false;
			IsMultiPrecedent = false;
			SetExtraInfo("precdirection", false);
			SetExtraInfo("precdeleted", true);

			BuildXML();

            this.OnExtLoaded();
		}

		[EnquiryUsage(true)]
		internal Precedent (long id) : this(id,false)
		{

		}

		/// <summary>
		/// Constructs an existing precedent by passing the underlying data source to the constructor.
		/// </summary>
		/// <param name="ds">The underlying data source.</param>
		/// <param name="title">The title of the precedent trying to be accessed.</param>
		private Precedent (DataSet ds, string title)
		{
			_precedent = ds;

			if ((_precedent== null) || (_precedent.Tables[Table].Rows.Count == 0)) 
				throw new OMSException2("PRECNOTFOUND", "Specified precedent '%1%' does not appear to exist within the database.", null, false, title);


			//Only check passwords when the basic security type is being used.
			if (!Session.CurrentSession.AdvancedSecurityEnabled)
			{
				//Ask for the password on open.  Perhaps extend the precedent object so that the
				//password can be applied to different levels of security, like viewing, changing
				//opeing, deleting etc...
				if (IsPasswordValid() == false)
				{
					CancelEventArgs args = new CancelEventArgs();
					Session.CurrentSession.OnPasswordRequest(this, args);
					if (args.Cancel)
					{
						Dispose();
						throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
					}
				}
			}

			BuildXML();

			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentPrecedents.Add(ID.ToString(), this);

			LoadScript(false);

            this.OnExtLoaded();
		}


		/// <summary>
		/// Initaites this class and fetches a single store item from the database.
		/// </summary>
		/// <param name="id">Storage item identifier used to fetch a unqie single item.</param>
        /// <param name="designMode"></param>
		internal protected Precedent (long id, bool designMode)
		{
			Fetch(id, null);
			BuildXML();

			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentPrecedents.Add(ID.ToString(), this);

			LoadScript(designMode);

            this.OnExtLoaded();
		}

		private void LoadScript(bool designMode)
		{
			if (HasScript)
			{
				Script.Load(designMode);
				PrecedentScriptType precscr =  _script.Scriptlet as PrecedentScriptType;
				if (precscr != null)
				{
					precscr.SetPrecedentObject(this);
				}
				if (designMode==false)
					OnLoad();
			}
		}

		/// <summary>
		/// Creates a new precedent based on the parameters passed.
		/// </summary>
		/// <param name="title">The title of the precedent.</param>
		/// <param name="type">The type of precedent.</param>
		/// <param name="description">The description of the precedent.</param>
		/// <param name="extension">The file extension to use to override the original.</param>
		/// <param name="storageLocation">The storage location identifer 0 = File System, 1 = BLOB.</param>
		public Precedent(string title, string type, string description, string extension, short storageLocation) : this()
		{
			isnew_storeitem = true;

			//Set defaults.
			this.type = PrecType.GetPrecType(type);
			SetExtraInfo("prectitle", title);
			SetExtraInfo("prectype", this.type.Code);
			SetExtraInfo("precdesc", description);
			SetExtraInfo("brid", DBNull.Value);
			SetExtraInfo("precpubname", Session.CurrentSession.CompanyName);
			SetExtraInfo("precProgType", this.type.DefaultApplication);

			//Set storage location.
			if (storageLocation < 0)
			{
				if (Session.CurrentSession.DefaultPrecStorageProvider < 0)
					storageLocation = this.type.DefaultStorageProvider;
				else
					storageLocation = Session.CurrentSession.DefaultPrecStorageProvider;
			}
			CurrentStorageProviderID = storageLocation;

			//Set extension.
			if (extension == null || extension == "") extension = this.type.DefaultPrecExtension;
			SetExtraInfo("precextension", extension);
			
			//Get the identifier of the directory used to store the document.
			short dirid;
			Session.CurrentSession.GetSystemDirectory(SystemDirectories.OMPrecedents, out dirid);
			SetExtraInfo("precdirid", dirid);
	


		}
		
		/// <summary>
		/// Constructs an existing precedent.
		/// </summary>
		/// <param name="id">The precedent id to retrieve.</param>
        /// <param name="merge"></param>
		private void Fetch (long id, DataRow merge)
		{
			//Make sure that the parameters list is cleared after use.	
			IDataParameter[] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("PRECID", System.Data.SqlDbType.BigInt, 0, id);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			DataSet data = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprPrecedentRecord", new string[1] {Table}, paramlist);

			if ((data == null) || (data.Tables[Table].Rows.Count == 0)) 
				throw new OMSException2("PRECNOTFOUND", "Specified precedent '%1%' does not appear to exist within the database.", null, false, id.ToString());

			if (merge != null)
				Global.Merge(data.Tables[Table].Rows[0], merge);

			_precedent = data;

			timestamp = DateTime.UtcNow;

			//Ask for the password on open.  Perhaps extend the precedent object so that the
			//password can be applied to different levels of security, like viewing, changing
			//opeing, deleting etc...
			if (IsPasswordValid() == false)
			{
				CancelEventArgs args = new CancelEventArgs();
				Session.CurrentSession.OnPasswordRequest(this, args);
				if (args.Cancel)
				{
					Dispose();
					throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
				}
			}

			before_title = this.Title;
			before_type = this.PrecedentType;
			before_library = this.Library;
			before_category = this.Category;
			before_subcategory = this.SubCategory;
            before_minorcategory = this.MinorCategory;

            _isdirty = false;


			//Refresh the security
			SecurityManager.CurrentManager.Refresh(this);

		}

		#endregion

		#region XML Settings Methods

		private XmlProperties xmlprops;

		private void BuildXML()
		{
			if (xmlprops == null)
				xmlprops = new XmlProperties(this, "precXML");

		}

		public object GetXmlProperty(string name, object defaultValue)
		{
			BuildXML();
			return xmlprops.GetProperty(name, defaultValue);
		}

		public void SetXmlProperty(string name, object val)
		{
			BuildXML();
			if (xmlprops.SetProperty(name, val))
				_isdirty = true;
		}



        #endregion

        #region Properties

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <remarks></remarks>
        [Browsable(false)]
        public ObjectState State
        {
            get
            {
                try
                {
                    switch (_precedent.Tables[Table].Rows[0].RowState)
                    {
                        case DataRowState.Added:
                            return ObjectState.Added;
                        case DataRowState.Modified:
                            return ObjectState.Modified;
                        case DataRowState.Deleted:
                            return ObjectState.Deleted;
                        case DataRowState.Unchanged:
                            return ObjectState.Unchanged;
                        default:
                            return ObjectState.Unitialised;
                    }
                }
                catch
                {
                    return ObjectState.Unitialised;
                }
            }
        }

        [Browsable(false)]
        public PrecType CurrentPrecedentType
		{
			get
			{
				if (type == null || !type.Code.ToUpperInvariant().Equals(this.PrecedentType.ToLowerInvariant()))
					type = PrecType.GetPrecType(PrecedentType);
				return type;
			}
		}

		[DefaultValue("")]
		[LocCategory("(Details)")]
		[Lookup("DocVersioning")]
		[Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
		[Design.DataList("DSDOCVERSIONING", UseNull = true, DisplayMember = "cddesc", ValueMember = "cdcode")]
		[TypeConverter(typeof(Design.DataListConverter))]
		public string DocumentVersioning
		{
			get
			{

				//O = Overwrite (Standard)
				//N = Creates a brand new document (Save As);
				//V = Automatically creates new version.
				//M = Creates a new root/major version.

				string val = Convert.ToString(GetXmlProperty("DocVersioning", String.Empty));
				switch (val)
				{
					case "O":
						return val;
					case "N":
						return val;
					case "V":
						return val;
					case "M":
						return val;
					default:
						return String.Empty;
				}
			}
			set
			{
				switch (value)
				{
					case "O":
						break;
					case "N":
						break;
					case "V":
						break;
					case "M":
						break;
					default:
						value = String.Empty;
						break;
				}

				SetXmlProperty("DocVersioning", value);
			}
		}


		[LocCategory("Properties")]
		[Lookup("Wallet")]
		[Browsable(false)]
		public virtual string Wallet
		{
			get
			{
				return Convert.ToString(GetXmlProperty("Wallet", ""));
			}
			set
			{
				SetXmlProperty("Wallet", value);
			}
		}
		
		
		[LocCategory("SCRIPT")]
		[Parameter(CodeLookupDisplaySettings.omsObjects)]
		[CodeLookupSelectorTitle("EXTENDEDDATAS","Extended Datas")]
		[Lookup("AutoExtended")]
		public virtual CodeLookupDisplayReadOnly AutoExtendedData
		{
			get
			{
				if (_extcode == null)
				{
					_extcode = new CodeLookupDisplayReadOnly("EXTENDEDDATA");
					_extcode.Code = Convert.ToString(GetXmlProperty("AutoExtendedData",""));
				}
				return _extcode;
			}
			set
			{
				_extcode = value;
				SetXmlProperty("AutoExtendedData", value.Code);
			}
		}

		
		[LocCategory("SCRIPT")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.EnquiryFormEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public string AutoWizard
		{
			get
			{
				return Convert.ToString(GetXmlProperty("AutoWizard", String.Empty));
			}
			set
			{
				if (value == null) value = String.Empty;
				SetXmlProperty("AutoWizard", value);
			}
		}

		[LocCategory("SCRIPT")]
		public CodeLookupDisplayReadOnly AutoWizardType
		{
			get
			{
				if (_autowiztype == null) 
				{
					_autowiztype = new CodeLookupDisplayReadOnly("PRECAUTWIZTYPE");
					_autowiztype.Code = Convert.ToString(GetXmlProperty("AutoWizardType", String.Empty));
				}
				return _autowiztype;
			}
			set
			{
				if (_autowiztype == null) _autowiztype = new CodeLookupDisplayReadOnly("PRECAUTWIZTYPE");
				_autowiztype = value;
				if (value == null)
					SetXmlProperty("AutoWizardType", "");
				else
					SetXmlProperty("AutoWizardType", value.Code);
			}
		}

		/// <summary>
		/// The Get and Set for The Admin Kit of the Contact Type
		/// </summary>
		[LocCategory("Contact")]
		[Lookup("1ContactType")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("CONTTYPE","Contact Type")]
		public CodeLookupDisplay ContactTypeCodeLookup
		{
			get
			{
				if (_contacttype == null) 
				{
					_contacttype = new CodeLookupDisplay("CONTTYPE");
					_contacttype.Code = this.ContactType;
				}
				return _contacttype;
			}
			set
			{
				if (_contacttype == null) _contacttype = new CodeLookupDisplay("CONTTYPE");
				_contacttype = value;
				ContactType = _contacttype.Code;
			}
		}

		/// <summary>
		/// The Get and Set for The Admin Kit of the Accoicate Type
		/// </summary>
		[LocCategory("Contact")]
		[Lookup("2ASSOCAS")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("ASSTYPE","Associate Types")]
		[Parameter(CodeLookupDisplaySettings.PrecedentAssoc)]
		public CodeLookupDisplay AssocTypeCodeLookup
		{
			get
			{
				if (_assoctype == null) 
				{
					_assoctype = new CodeLookupDisplay("SUBASSOC");
					_assoctype.Code = this.AssocType;
				}
				return _assoctype;
			}
			set
			{
				if (_assoctype == null) _assoctype = new CodeLookupDisplay("SUBASSOC");
				_assoctype = value;
				AssocType = _assoctype.Code;
			}
		}
		
		/// <summary>
		/// The Get and Set for The Admin Kit of the Contact Type
		/// </summary>
		[LocCategory("PRECEDENT")]
		[Lookup("PRECLIBRARY")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("PRECLIBRARY","Precedent Library")]
		public CodeLookupDisplay LibraryCodeLookup
		{
			get
			{
				if (_library == null) 
				{
					_library = new CodeLookupDisplay("PRECLIBRARY");
					_library.Code = this.Library;
				}
				return _library;
			}
			set
			{
				if (_library == null) _library = new CodeLookupDisplay("PRECLIBRARY");
				_library = value;
				Library = _library.Code;
			}
		}

		
		/// <summary>
		/// The Get and Set for The Admin Kit of the Out Bound Activity Code
		/// </summary>
		[LocCategory("TimeRec")]
		[Lookup("TimeRecCode")]
		[CodeLookupSelectorTitle("TIMEACT","Time Recording Activities")]
		public CodeLookupDisplayReadOnly TimeRecordingActivityCodeCodeLookup
		{
			get
			{
				if (_activity == null) 
				{
					_activity = new CodeLookupDisplayReadOnly("TIMEACTCODE");
					_activity.Code = TimeRecordingActivityCode;
				}
				return _activity;
			}
			set
			{
				if (_activity == null) _activity = new CodeLookupDisplayReadOnly("TIMEACTCODE");
				_activity = value;
				TimeRecordingActivityCode = _activity.Code;
			}
		}

		/// <summary>
		/// The Get and Set for The Admin Kit of the In Bound Activity Code
		/// </summary>
		[LocCategory("TimeRec")]
		[Lookup("TimeRecInwardCo")]
		[CodeLookupSelectorTitle("TIMEACT","Time Recording Activities")]
		public CodeLookupDisplayReadOnly TimeRecordingActivityInwardCodeCodeLookup
		{
			get
			{
				if (_iwdactivity == null)
				{
					_iwdactivity = new CodeLookupDisplayReadOnly("TIMEACTCODE");
					_iwdactivity.Code = this.TimeRecordingActivityInwardCode;
				}
				return _iwdactivity;
			}
			set
			{
				if (_iwdactivity == null) _iwdactivity = new CodeLookupDisplayReadOnly("TIMEACTCODE");
				_iwdactivity = value;
				TimeRecordingActivityInwardCode = _iwdactivity.Code;
			}
		}

		/// <summary>
		/// The Get and Set for The Admin Kit of the Category Code
		/// </summary>
		[LocCategory("PRECEDENT")]
		[Lookup("PRECCAT")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("PRECCAT","Precedent Categories")]
		public CodeLookupDisplay CategoryCodeLookup
		{
			get
			{
				if (_category == null) 
				{
					_category = new CodeLookupDisplay("PRECCAT");
					_category.Code = this.Category;
				}
				return _category;
			}
			set
			{
				if (_category == null) _category = new CodeLookupDisplay("PRECCAT");
				_category = value;
				Category = _category.Code;
			}
		}

		/// <summary>
		/// The Get and Set for The Admin Kit of the Precedent Type
		/// </summary>
		[LocCategory("PRECEDENT")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[Lookup("PrecType")]
		[CodeLookupSelectorTitle("PRECTYPE","Precedent Types")]
		public CodeLookupDisplay PrecedentTypeCodeLookup
		{
			get
			{
				if (_doctype == null) 
				{
					_doctype = new CodeLookupDisplay("DOCTYPE");
					_doctype.Code = this.PrecedentType;
				}

				return _doctype;
			}
			set
			{
				if (_doctype == null) _doctype = new CodeLookupDisplay("DOCTYPE");
				_doctype = value;
				PrecedentType = _doctype.Code;
			}
		}
		
		/// <summary>
		/// The Get and Set for The Admin Kit of the Sub Precedent Type
		/// </summary>
		[LocCategory("PRECEDENT")]
		[Lookup("PRECSUBCAT")]
		[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
		[CodeLookupSelectorTitle("PRECSUBCAT","Precedent Sub Categories")]
		public CodeLookupDisplay SubCategoryCodeLookup
		{
			get
			{
				if (_subcategory == null) 
				{
					_subcategory = new CodeLookupDisplay("PRECSUBCAT");
					_subcategory.Code = this.SubCategory;
				}
				return _subcategory;
			}
			set
			{
				if (_subcategory == null) _subcategory = new CodeLookupDisplay("PRECSUBCAT");
				_subcategory = value;
				SubCategory = _subcategory.Code;
			}
		}

        /// <summary>
        /// The Get and Set for The Admin Kit of the Minor Precedent Type
        /// </summary>
        [LocCategory("PRECEDENT")]
        [Lookup("PRECMINORCAT")]
        [CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
        [CodeLookupSelectorTitle("PRECMINORCAT", "Precedent Minor Categories")]
        public CodeLookupDisplay MinorCategoryCodeLookup
        {
            get
            {
                if (_minorcategory == null)
                {
                    _minorcategory = new CodeLookupDisplay("PRECMINORCAT");
                    _minorcategory.Code = this.MinorCategory;
                }
                return _minorcategory;
            }
            set
            {
                if (_minorcategory == null) _minorcategory = new CodeLookupDisplay("PRECMINORCAT");
                _minorcategory = value;
                MinorCategory = _minorcategory.Code;
            }
        }

        /// <summary>
        /// Gets the multi precedent collection.
        /// </summary>
        [EnquiryUsage(false)]
		[LocCategory("MULTIPREC")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.MultiPrecedentEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
		public Precedent.MultiPrecedentCollection MultiPrecedents
		{
			get
			{
				if (_multiprec == null)
				{
					if (IsNew)
						_multiprec = new Precedent.MultiPrecedentCollection(this);
					else
						_multiprec = new Precedent.MultiPrecedentCollection(this, _precedent.Tables[1]);
				}
				return _multiprec;
			}
		}

        /// <summary>
        /// Gets the teams access relationship collection.
        /// </summary>
        [EnquiryUsage(false)]
        [LocCategory("SECURITY")]
        [CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
        [CodeLookupSelectorTitle("TEAMSACCESS", "Team Access")]
        [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.PrecedentsTeamsAccessEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
        public PrecedentsTeamsAccessCollection TeamsAccess
        {
            get
            {
                if (_teamsaccess == null)
                {
                    _teamsaccess = new PrecedentsTeamsAccessCollection(this);
                }
                return _teamsaccess;
            }
        }

        [LocCategory("(Details)")]
		[Lookup("PRECBRANCH")]
		[System.ComponentModel.Browsable(false)]
		public Branch Branch
		{
			get
			{
				if (_branch == null && GetExtraInfo("brID") != DBNull.Value)
					_branch = new Branch(FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("brID"),-1));
				return _branch;
			}
			set
			{
				_branch = value;
				if (_branch != null)
					SetExtraInfo("brID",_branch.ID);
				else
					SetExtraInfo("brID",DBNull.Value);
			}
		}

		private bool _isnew = false;
		/// <summary>
		/// Gets a value indicating whether the precedent object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (_precedent.Tables[Table].Rows[0].RowState == DataRowState.Added || _isnew);
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the unique identifier of the precedent.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(Details)")]
		[Lookup("PRECID")]
		public virtual long ID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("precid"));
			}
		}

		/// <summary>
		/// Gets and Set the Milestone Code Plan
		/// </summary>
		[Browsable(false)]
		public virtual object MilestonePlan
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecMSCode"));
			}
			set
			{
				SetExtraInfo("PrecMSCode",value);
			}
		}
		
		/// <summary>
		/// Gets the precedent description title.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(Details)")]
		[Lookup("PRECTITLE")]
		public virtual string Title
		{
			get
			{
				return Convert.ToString(GetExtraInfo("prectitle"));
			}
			set
			{
				SetExtraInfo("prectitle",value);
			}
		}

		/// <summary>
		/// Gets the precedent addressee same as Contact Type for best match.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string ContactType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precaddressee"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("precaddressee",DBNull.Value);
				else
					SetExtraInfo("precaddressee",value);
			}
		}

		/// <summary>
		/// Gets the precedent associate type for best match.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string AssocType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precassoctype"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("precassoctype",DBNull.Value);
				else
					SetExtraInfo("precassoctype",value);
			}
		}

		/// <summary>
		/// Gets the precedent description.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(Details)")]
		public virtual string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precdesc"));
			}
			set
			{
				SetExtraInfo("precdesc",value);
			}
		}


		/// <summary>
		/// Gets or Sets the language that the precedent is written in.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string Language
		{
			get
			{
				return Convert.ToString(GetExtraInfo("preclanguage"));
			}
			set
			{
				if (value == null || value.Trim() == "")
					SetExtraInfo("preclanguage",DBNull.Value);
				else
					SetExtraInfo("preclanguage",value);
			}
		}

		/// <summary>
		/// Gets or Sets the precedent Preview Text.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string PrecedentPreview
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precpreview"));
			}
			set
			{
				SetExtraInfo("precpreview",value);
			}
		}


		/// <summary>
		/// Gets the main category of the precedent library object.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("MULTIPREC")]
		[Lookup("MULTIPREC")]
		public bool IsMultiPrecedent
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetExtraInfo("precmultiprec"), false);
			}
			set
			{
				SetExtraInfo("precmultiprec",value);
				if (MultiPrecedents.Count == 0 && value)
				{
					AskEventArgs e = new AskEventArgs("ADDPRECTOMULTI", "Would you like to add this parent precedent to the multi precedent setup?", "", AskResult.Yes);
					Session.CurrentSession.OnAsk(this, e);
					if (e.Result == AskResult.Yes)
						MultiPrecedents.Add(MultiPrecedents.New(this));
				}
			}
		}

		/// <summary>
		/// Gets the precedent type category.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public string PrecedentType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("prectype"));
			}
			set
			{
				SetExtraInfo("prectype",value);
                var precExtension = PrecType.GetPrecType(value).DefaultPrecExtension;
                SetExtraInfo("precextension", precExtension);
			}
		}

		/// <summary>
		/// Get the Library Catagory of the Precednet Library Object
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string Library
		{
			get
			{
				return Convert.ToString(GetExtraInfo("preclibrary"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("preclibrary",DBNull.Value);
				else
					SetExtraInfo("preclibrary",value);
			}
		}

		/// <summary>
		/// Gets the main category of the precedent library object.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string Category
		{
			get
			{
				return Convert.ToString(GetExtraInfo("preccategory"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("preccategory",DBNull.Value);
				else
					SetExtraInfo("preccategory",value);
			}
		}

		/// <summary>
		/// Gets the sub category filter of the precedent library object.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string SubCategory
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precsubcategory"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("precsubcategory",DBNull.Value);
				else
					SetExtraInfo("precsubcategory",value);
			}		
		}

        /// <summary>
        /// Gets the minor category filter of the precedent library object.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(false)]
        public virtual string MinorCategory
        {
            get
            {
                return Convert.ToString(GetExtraInfo("precminorcategory"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("precminorcategory", DBNull.Value);
                else
                    SetExtraInfo("precminorcategory", value);
            }
        }


        /// <summary>
        /// Gets a value that specifies whether the precedent is a text only document.
        /// This may be used for inserting standard text / disclaimers into other documents
        /// and precedents.
        /// </summary>
        [EnquiryUsage(true)]
		[LocCategory("(Details)")]
		public virtual bool TextOnly
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("prectext"));
			}
			set
			{
				SetExtraInfo("prectext",value);
			}	
		}

		/// <summary>
		/// Gets a value specifying that the precedent uses second stage merge or not.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(Details)")]
		[Lookup("ALLOWSSM")]
		public bool AllowSecondStageMerge
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("precinternalprocess"));
			}
			set
			{
				SetExtraInfo("precinternalprocess", value);
			}
		}

		/// <summary>
		/// Gets the precedents publishers company name.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Properties")]
		public virtual string PublishersName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precpubname"));
			}
			set
			{
				SetExtraInfo("precpubname",value);
			}
		}

		/// <summary>
		/// Gets a flag value indicating whether there is a reminder prompt for the current precedent.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Reminder")]
		public virtual bool Reminder
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("precreminder"));
			}
			set
			{
				SetExtraInfo("precreminder",value);
			}	
		}


		/// <summary>
		/// Gets the number of days that the reminder is used to prompt the user after the document is saved.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Reminder")]
		[Lookup("REMDAYCOUNT")]
		[RefreshProperties(RefreshProperties.All)]
		public virtual short ReminderDayCount
		{
			get
			{
				return Convert.ToInt16(GetExtraInfo("precnoofdays"));
			}
			set
			{
				SetExtraInfo("precnoofdays",value);
			}	
		}

		/// <summary>
		/// Gets a the textual comment that gets placed in the task list when the reminder is saved for the document created.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Reminder")]
		[Lookup("REMSUBJECT")]
		[RefreshProperties(RefreshProperties.All)]
		public virtual string ReminderSubject
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecRemComment"));
			}
			set
			{
				SetExtraInfo("PrecRemComment",value);
			}	
		}

	
		/// <summary>
		/// Gets the number of days that the reminder is used to prompt the user after the document is saved.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Reminder")]
		[Lookup("REMRVWDAYCOUNT")]
		[RefreshProperties(RefreshProperties.All)]
		public virtual short ReminderReviewDayCount
		{
			get
			{
				return Convert.ToInt16(GetExtraInfo("precreviewnoofdays"));
			}
			set
			{
				SetExtraInfo("precreviewnoofdays",value);
			}	
		}

		/// <summary>
		/// The number of units used for a time recording entry when a document is saved using the current precedent as a template.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TimeRec")]
		[Lookup("TimRecUnits")]
		public virtual int TimeRecordingUnits
		{
			get
			{
				return Convert.ToInt32(GetExtraInfo("prectimerecunits"));
			}
			set
			{
				SetExtraInfo("prectimerecunits",value);
			}
		}

		/// <summary>
		/// Gets the description of the time recording item for when a document based on the precedent is saved.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("TimeRec")]
		[Lookup("TimeRecDes")]
		public virtual string TimeRecordingDescription
		{
			get
			{
				return Convert.ToString(GetExtraInfo("prectimerecdesc"));
			}
			set
			{
				SetExtraInfo("prectimerecdesc",value);
			}
		}

		/// <summary>
		/// Code from the time recording activities determining the type of time recording entry.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string TimeRecordingActivityCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("prectimereccode"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("prectimereccode",DBNull.Value);
				else
					SetExtraInfo("prectimereccode",value);
			}
		}

		
		/// <summary>
		/// Code from the time recording activities determining the type of time recording entry.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string TimeRecordingActivityInwardCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("prectimerecincode"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("prectimerecincode",DBNull.Value);
				else
					SetExtraInfo("prectimerecincode",value);
			}
		}

		/// <summary>
		/// Gets or Sets auto SMS message text.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string SMSMessage
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precSMSMessage"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("precSMSMessage",DBNull.Value);
				else
					SetExtraInfo("precSMSMessage",value);
			}
		}

		/// <summary>
		/// Gets the modification dates and users.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(Details)")]
		public virtual ModificationData TrackingStamp
		{
			get
			{
				return new ModificationData(
					Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("Created"), DBNull.Value),
					Common.ConvertDef.ToInt32(GetExtraInfo("CreatedBy"), 0),
					Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("Updated"), DBNull.Value),
					Common.ConvertDef.ToInt32(GetExtraInfo("UpdatedBy"), 0));
			}
		}

		/// <summary>
		/// Gets the Precedent Program Type.
		/// </summary>
		[LocCategory("(Details)")]
		[Lookup("Application")]
		public virtual Apps.RegisteredApplication PrecProgType
		{
			get
			{
				return Apps.ApplicationManager.CurrentManager.GetRegisteredApplication(Convert.ToInt16(GetExtraInfo("precprogtype")));

			}
			set
			{
				SetExtraInfo("precprogtype",value.ID);
			}
		}

		[EnquiryUsage(true)]
		internal string ProgramType
		{
			get
			{
				return PrecProgType.Name;
			}
		}

		/// <summary>
		/// Gets a string value that indicates whether the precedent has an attached script
		/// </summary>
		[Browsable(false)]
		public virtual bool HasScript
		{
			get
			{
				try
				{
					if (Convert.ToString(GetExtraInfo("PrecScript")) == "")
						return false;
					else
						return true;
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the script name.
		/// </summary>
		[LocCategory("Script")]
		public virtual string ScriptName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecScript"));
			}
			set
			{
				SetExtraInfo("PrecScript",value);
				this.Script.Code=value;
				OnScriptChanged();
			}
		}

		/// <summary>
		/// Gets the enquir script type of the of the current enquiry form.
		/// </summary>
		[Browsable(false)]
		public ScriptGen Script
		{
			get
			{
				if (_script == null)
				{
					if (HasScript && ScriptGen.Exists(Convert.ToString(GetExtraInfo("PrecScript"))))
					{
						_script = ScriptGen.GetScript(Convert.ToString(GetExtraInfo("PrecScript")));
					}
					else
					{
						_script = new ScriptGen(Convert.ToString(GetExtraInfo("PrecScript")),"PRECEDENT");
					}
				}
				return _script;
			}
		}


		/// <summary>
		/// Gets the live directory path.
		/// </summary>
		[EnquiryUsage(true)]
		internal string LiveDirectory
		{
			get
			{
				if (GetExtraInfo("precdirid") == DBNull.Value)
					return String.Empty;
				else
					return Session.CurrentSession.GetDirectory((short)GetExtraInfo("precdirid")).FullName;
			}
		}


		/// <summary>
		/// Gets or Sets the live directory id of the precedent.
		/// </summary>
		internal short DirectoryID
		{
			get
			{
				try
				{
					return Convert.ToInt16(GetExtraInfo("precdirid"));
				}
				catch
				{
					short dirid;
					Session.CurrentSession.GetSystemDirectory(SystemDirectories.OMPrecedents, out dirid);
					return dirid;
				}
			}
			set
			{
				SetExtraInfo("precdirid", value);
			}
		}

		/// <summary>
		/// Gets the default incoming / outgoing direction of the storage item when the item was received and saved.
		/// </summary>
		[LocCategory("(Details)")]
		[Lookup("DEFAULTDIRECTIO")]
		public DocumentDirection DefaultDirection
		{
			get
			{
				try
				{
					bool ret = Convert.ToBoolean(GetExtraInfo("precdirection"));
					if (ret)
						return DocumentDirection.In;
					else
						return DocumentDirection.Out;
				}
				catch
				{
					return DocumentDirection.Out;
				}
			}
		}


		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual bool MilestoneConfirmMoveStage
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("PrecMoveMS"));

			}
			set
			{
				SetExtraInfo("PrecMoveMS",value);
			}
		}

		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual string MilestoneChangePrompt
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecMSChangePrompt"));

			}
			set
			{
				SetExtraInfo("PrecMSChangePrompt",value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("DATA")]
		[Lookup("CLNOTES")]
		[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
		[TextOnly(true)]
		public virtual string MilestoneNote
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecMSNote"));

			}
			set
			{
				SetExtraInfo("PrecMSNote",value);
			}
		}

		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual bool MilestoneChangeAutomatic
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("PrecMSAuto"));

			}
			set
			{
				SetExtraInfo("PrecMSAuto",value);
			}
		}

		[EnquiryUsage(true)]
		[Browsable(false)]
		public virtual int MilestoneStage
		{
			get
			{
				return Convert.ToInt32(GetExtraInfo("PrecMS"));
			}
			set
			{
				SetExtraInfo("PrecMS",value);
			}
		}

		[Browsable(false)]
		internal string PrecedentPath
		{
			get
			{
				return Convert.ToString(GetExtraInfo("PrecPath"));
			}
			set
			{
				SetExtraInfo("PrecPath",value);
			}
		}

		
		/// <summary>
		/// Gets the precedent ID to be displayed.
		/// </summary>
		[Browsable(false)]
		public string DisplayID
		{
			get
			{
				return ID.ToString();
			}
		}
		
		#endregion

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public virtual void SetExtraInfo (string fieldName, object val)
		{
            this.SetExtraInfo(_precedent.Tables[Table].Rows[0], fieldName, val);
			OnChanged();
		}
		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{ 
			object val = _precedent.Tables[Table].Rows[0][fieldName];
			//UTCFIX: DM - 30/11/06 - return local time
			if (val is DateTime)
				return ((DateTime)val).ToLocalTime();
			else
				return val;
		}

		/// <summary>
		/// Returns the specified fields data.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public virtual Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _precedent.Tables[Table].Columns[fieldName].DataType;
			}
			catch 
			{
				throw new Exception("Error Getting Extra Info Field " + fieldName + " Probably Not Initialized");
			}
		}
		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public virtual DataSet GetDataset()
		{
			return _precedent;
		}
		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public virtual DataTable GetDataTable()
		{
			return _precedent.Tables[Table].Copy();
		}

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public void Update()
		{
			CheckPermissions();

            ObjectState state = State;
            if (this.OnExtCreatingUpdatingOrDeleting(state))
                return;

			_isnew = IsNew;
			if (_isnew)
				_isdirty = true;

			if (Session.CurrentSession.EnablePrecedentVersioning)
			{
				//Update the Precedent Version headers.
				if (_isnew == false)
				{
					foreach (PrecedentVersion ver in versions.Values)
					{
						ver.InternalUpdate();
					}
				}
			}

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (IsDirty)
			{
				//
				// Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				//
				UpdateDataRow();

				// 
				// Update FileType Startup Documents.
				//
				if (before_category != "" || before_library != "" || before_subcategory != "" || before_minorcategory != "" || before_title != "" || before_type != "")
				{
					if (before_category != this.Category || before_library != this.Library
					|| before_subcategory != this.SubCategory || before_minorcategory != this.MinorCategory || before_title != this.Title || before_type != this.PrecedentType)
					{
						string _hashcode = before_library + "/" + before_type + "/" + before_category + "/" + before_subcategory + "/" + before_minorcategory + "/" + before_title;
						_hashcode = _hashcode.ToUpper();

						DataTable dt = FileType.GetTypes(typeof(FileType), true);
						foreach (DataRow dr in dt.Rows)
						{
							FileType ft = FileType.GetFileType(Convert.ToString(dr["typeCode"]));
							for (int i = ft.StartupDocuments.Count - 1; i >= 0; i--)
							{
								FileType.Job j = ft.StartupDocuments[i];
								if (j.Hashcode == _hashcode)
								{
									ft.StartupDocuments.RemoveAt(i);
									ft.StartupDocuments.Insert(i, new FileType.Job(ft, this));
									ft.Update();
								}
							}
						}
					}
				}

				before_title = this.Title;
				before_type = this.PrecedentType;
				before_library = this.Library;
				before_category = this.Category;
				before_subcategory = this.SubCategory;
                before_minorcategory = this.MinorCategory;
            }


			//Update any multi precedent logic.
			if (_multiprec != null)
				_multiprec.Update();

            if (_teamsaccess != null)
                _teamsaccess.Update();

            if (_isnew)
			{
				Session.CurrentSession.CurrentPrecedents.Add(ID.ToString(), this);
			}

			_isnew = false;
			_isdirty = false;

            this.OnExtCreatedUpdatedDeleted(state);
		}

		private void UpdateDataRow()
		{
			var row = _precedent.Tables[Table].Rows[0];

			if (_precedent.Tables[Table].PrimaryKey == null || _precedent.Tables[Table].PrimaryKey.Length == 0)
				_precedent.Tables[Table].PrimaryKey = new DataColumn[1] { _precedent.Tables[Table].Columns["precid"] };

			SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Updated", DateTime.Now);

			if (xmlprops != null) xmlprops.Update();

			Session.CurrentSession.Connection.Update(row, "dbPrecedents");
		}


		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Gets the changes of the current object and and refreshes the object
		/// then reapplies the changes to avoid any concurrency issues.  This is in 
		/// theory forcing any changes made to the object.
		/// </summary>
		/// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
		public void Refresh(bool applyChanges)
		{
			if (IsNew)
				return;

            if (this.OnExtRefreshing())
                return;

			DataTable changes = _precedent.Tables[Table].GetChanges();

			xmlprops = null;
			_multiprec = null;


			if (changes != null && applyChanges && changes.Rows.Count > 0)
				Fetch(this.ID, changes.Rows[0]);
			else
				Fetch(this.ID, null);

            this.OnExtRefreshed();
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			xmlprops = null;
			_precedent.RejectChanges();
			_isdirty = false;
		}

		private bool _isdirty = false;
		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return (_isdirty || _precedent.GetChanges() != null);
			}
		}

		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_precedent != null)
				{
					_precedent.Dispose();
					_precedent = null;
				}
				
			}

			//Free unmanaged objects here.
		}

		
		#endregion

		#region Static Methods
		/// <summary>
		/// Fetches a precedent item based on the unique identifier given.
		/// </summary>
		/// <param name="id">Unique identifier of the item within the data store.</param>
		/// <returns>A precedent item.</returns>
		public static Precedent GetPrecedent(long id)
		{
			Session.CurrentSession.CheckLoggedIn();
			Precedent pr = Session.CurrentSession.CurrentPrecedents[id.ToString()] as Precedent;
			
			if (pr == null)
			{
				pr = new Precedent(id);
			}		
			return pr;
		}

		/// <summary>
		/// Fetches a precedent item based on the title.
		/// </summary>
		/// <param name="title">Title name.</param>
        /// <param name="assoc"></param>
		/// <returns>A precedent item.</returns>
		public static Precedent GetPrecedent(string title, Associate assoc)
		{
			Session.CurrentSession.CheckLoggedIn();
			string lang = Session.CurrentSession.DefaultCulture;
			if (assoc != null) lang = assoc.OMSFile.PreferedLanguage;

			PromptEventArgs e = new PromptEventArgs(PromptType.Search,FWBS.OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PrecedentFilter), new object[2]{title, lang}, "");
			Session.CurrentSession.OnPrompt(Session.OMS, e);
			if (e.Result != null)
			{
				long id = Common.ConvertDef.ToInt64(e.Result, -1);
				return Precedent.GetPrecedent(id);
			}
			else
				return null;
		}


        /// <summary>
        /// Gets a precedent object that tries and finds the best match precedent with the criteria given.
        /// </summary>
        /// <param name="title">The precedent title.</param>
        /// <param name="type">The precedent / document type.</param>
        /// <param name="library">The primary precedent library grouping.</param>
        /// <param name="category">A main category code.</param>
        /// <param name="subcategory">A fursther sub category code.</param>
        /// <param name="minorcategory">A fursther minor category code.</param>
        /// <param name="language">Prefered language version of the precedent to match.</param>
        public static Precedent GetPrecedent (string title, string type, string library, string category, string subcategory, string language, string minorcategory)
		{
			Session.CurrentSession.CheckLoggedIn();

			//Make sure that the parameters list is cleared after use.	
			IDataParameter[] paramlist = new IDataParameter[8];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("TITLE", System.Data.SqlDbType.NVarChar, 50, title);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("TYPE", System.Data.SqlDbType.NVarChar, 15, type);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("LIBRARY", System.Data.SqlDbType.NVarChar, 15, library);
			paramlist[3] = Session.CurrentSession.Connection.AddParameter("CATEGORY", System.Data.SqlDbType.NVarChar, 15, category);
			paramlist[4] = Session.CurrentSession.Connection.AddParameter("SUBCATEGORY", System.Data.SqlDbType.NVarChar, 15, subcategory);
            paramlist[5] = Session.CurrentSession.Connection.AddParameter("MINORCATEGORY", System.Data.SqlDbType.NVarChar, 15, minorcategory);
            paramlist[6] = Session.CurrentSession.Connection.AddParameter("LANGUAGE", System.Data.SqlDbType.NVarChar, 10, language);
			paramlist[7] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			DataSet _precedent = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprPrecedentMAtch", new string[1] {Table}, paramlist);

			if (_precedent != null && _precedent.Tables[Table].Rows.Count > 0)
			{
				long id = Convert.ToInt64( _precedent.Tables[Table].Rows[0]["precid"]);
				Precedent pr = Session.CurrentSession.CurrentPrecedents[id.ToString()] as Precedent;
				if (pr == null)
				{
					pr = new Precedent(_precedent, title);
				}		
				return pr;
			}
			else
				return new Precedent(_precedent, title);
		}

		public static Precedent GetPrecedent (string title, string type, string library, string category, string subcategory, string minorcategory)
		{
			return GetPrecedent(title, type, library, category, subcategory,  System.Threading.Thread.CurrentThread.CurrentCulture.Name, minorcategory);
		}

        [Obsolete("Use another static methods with 'minorcategory' parameter.")]
        public static Precedent GetPrecedent(string title, string type, string library, string category, string subcategory)
        {
            return GetPrecedent(title, type, library, category, subcategory, System.Threading.Thread.CurrentThread.CurrentCulture.Name, "");
        }

        [Obsolete("Use another static methods with 'minorcategory' parameter.")]
        public static Precedent GetPrecedent(string title, string type, string library, string category, string subcategory, CultureInfo culture)
        {
            return GetPrecedent(title, type, library, category, subcategory, culture.Name, "");
        }

        /// <summary>
        /// Gets a precedent object based on the OMSApplication passed to the method.
        /// </summary>
        /// <param name="app">The oms application passed.</param>
        /// <returns>A precedent object associated.</returns>
        public static Precedent GetDefaultPrecedent(IOMSApp app)
		{
			Guid guid = app.GetType().GUID;
			Apps.RegisteredApplication regapp = Apps.ApplicationManager.CurrentManager.GetRegisteredApplication(guid);
			return regapp.BlankPrecedent;		
		}

		/// <summary>
		/// This static method will look from at the inobj to look through the configsetting xml file for 
		/// a precedent ID will be then used to create a Precedent Object
		/// </summary>
		/// <param name="type">The precedent type / document type code.</param>
		/// <param name="inObj">The object to interigate for the default precedent to use.</param>
		/// <returns>Th matched precedent object.</returns>
		public static Precedent GetDefaultPrecedent(string type,object inObj)
		{
			Precedent prec = null;
			string title = "";
			string lib = "";
			string cat = "";
			string subcat = "";
            string minorcat = "";
			string lang = Session.CurrentSession.DefaultCulture;

			//Check to see if the associate is overriding the default precedent.
			if (inObj is FWBS.OMS.Associate)
			{ 	try
				{
					//Check file.
					Associate assoc = (FWBS.OMS.Associate)inObj;
					prec = assoc.OMSFile.GetDefaultPrecedent(type);
					lang = assoc.OMSFile.PreferedLanguage;
					if (prec == null)
					{// Check Client.
						prec = assoc.OMSFile.Client.GetDefaultPrecedent(type);
					}
				}
				finally
				{
				}
			}
			
			//Check to see if the file is overriding the default precedent.
			if (inObj is FWBS.OMS.OMSFile)
			{ 
				try
				{
					OMSFile file = (FWBS.OMS.OMSFile)inObj; 
					prec = file.GetDefaultPrecedent(type);
					lang = file.PreferedLanguage;
					if (prec == null)
					{// Check Client.
						prec = file.Client.GetDefaultPrecedent(type);
					}
				}
				finally
				{
				}
			}

			//Check to see if the client is overriding the default precedent.
			if (inObj is FWBS.OMS.Client)
			{ 
				try
				{
					Client cl = (FWBS.OMS.Client)inObj;
					prec = cl.GetDefaultPrecedent(type);
					lang = cl.PreferedLanguage;
				}
				finally
				{
				}
			}

			//Check the default system reg info.
			if (prec == null)
			{ 
				try
				{
					string xpath = "defaultTemplates/template[@type = '" + System.Xml.XmlConvert.EncodeName(type) + "']";
					string ret = Session.CurrentSession.GetSessionConfigSetting(xpath, "");
					title = Session.CurrentSession.GetSessionConfigSetting(xpath + "/@title", "");
					lib = Session.CurrentSession.GetSessionConfigSetting(xpath + "/@library", "");
					cat = Session.CurrentSession.GetSessionConfigSetting(xpath + "/@category", "");
					subcat = Session.CurrentSession.GetSessionConfigSetting(xpath + "/@subcategory", "");
                    minorcat = Session.CurrentSession.GetSessionConfigSetting(xpath + "/@minorcategory", "");

                    try
					{
						if (ret.Trim() == String.Empty) 
							prec = Precedent.GetPrecedent(title, type, lib, cat, subcat, lang, minorcat);
						else
						{
							long id = long.Parse(ret);
							prec = Precedent.GetPrecedent(id);
						}
					}
					catch
					{
						try
						{
							prec = Precedent.GetPrecedent(title, type, lib, cat, subcat, lang, minorcat);
						}
						catch
						{
						}
					}
					

					//If no settings are found within the session settings then use a built in default.
					if (prec == null) 
						prec = Precedent.GetPrecedent("DEFAULT", type, "", "", "", lang, minorcat); 
					
				}
				catch (Exception ex)
				{
					throw new OMSException2("ERNODEFTEMPLATE", "Error In Precedent Creation no Default Template Defined for %1%",ex, false, type);
				}
			}

			return prec;

		}

		/// <summary>
		/// Retrieves all of the all Precedents from the database
		/// </summary>
		public static DataTable GetAllPrecedents()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			string SQL = @"SELECT PrecTitle,PrecID,PrecDesc,PrecCategory,PrecSubCategory,PrecMinorCategory,PrecAddressee,PrecType,PrecScript,dbo.GetCodeLookupDesc('PRECCAT', preccategory, @UI) as PrecCategoryDesc ,dbo.GetCodeLookupDesc('PRECSUBCAT', precsubcategory, @UI) as PrecSubCategoryDesc ,dbo.GetCodeLookupDesc('PRECMINORCAT', precminorcategory, @UI) as PrecMinorCategoryDesc,dbo.GetCodeLookupDesc('CONTTYPE', precaddressee, @UI) as PrecAddresseeDesc from DBPrecedents";
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(SQL, "PRECEDENTS", paramlist);
			return dt;
		}

		public static DataTable GetAssocPrecedents(FWBS.OMS.Associate assocobj,string acttype)
		{

			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[10];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("USRID", System.Data.SqlDbType.Int, 15, Session.CurrentSession.CurrentUser.ID.ToString());
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("CAT", System.Data.SqlDbType.NVarChar, 15, assocobj.OMSFile.PrecedentCategory);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("SUBCAT", System.Data.SqlDbType.NVarChar, 15, assocobj.OMSFile.PrecedentSubCategory);
            paramlist[3] = Session.CurrentSession.Connection.AddParameter("MINORCAT", System.Data.SqlDbType.NVarChar, 15, assocobj.OMSFile.PrecedentMinorCategory);
            paramlist[4] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 15, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			paramlist[5] = Session.CurrentSession.Connection.AddParameter("TYPE", System.Data.SqlDbType.NVarChar, 15, acttype);
			if (assocobj.IsClient)
				paramlist[6] = Session.CurrentSession.Connection.AddParameter("ADDRESSEE", System.Data.SqlDbType.NVarChar, 50, assocobj.AssocType);
			else
				paramlist[6] = Session.CurrentSession.Connection.AddParameter("ADDRESSEE", System.Data.SqlDbType.NVarChar, 50, assocobj.Contact.ContactTypeCode);
			paramlist[7] = Session.CurrentSession.Connection.AddParameter("LIBRARY", System.Data.SqlDbType.NVarChar, 15, assocobj.OMSFile.PrecedentLibrary);
			paramlist[8] = Session.CurrentSession.Connection.AddParameter("LANGUAGE", System.Data.SqlDbType.NVarChar, 15, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			if (assocobj.OMSFile.MilestonePlan != null)
				paramlist[9] = Session.CurrentSession.Connection.AddParameter("MS_STAGE", System.Data.SqlDbType.Int, 15, assocobj.OMSFile.MilestonePlan.NextStage);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("schSearchPrecedent", "PRECASSOC", paramlist);
			return dt;
			
		}



        #endregion

        #region Delete Routines



        /// <summary>
        /// The while() loop is used to create a unique Precedent title 
        /// This is useful when updated versions of Precedents e.g. Oyez forms are being installed
        /// and the older versions need to be archived prior to the update.
        /// 
        /// Limiting n to 500 is a crude effort to reduce the risk of an endless loop occurring.
        /// </summary>
        /// <param name="precID"></param>


        public static bool ArchivePrecedent(long precID)
        {
            bool err = true;
            int duplicateIndex = 1;
            const int maxTitleLength = 50;
            const int archiveMaxDuplicateTitleCount = 500;
            while (err)
            {
                try
                {
                    IDataParameter[] pars = new IDataParameter[1];
                    pars[0] = Session.CurrentSession.Connection.AddParameter("id", SqlDbType.BigInt, 0, precID);
                    var postfix = $" ({ ArchiveDelPostfixPart }{ duplicateIndex })";
                    var truncIndex = maxTitleLength - postfix.Length;
                    Session.CurrentSession.Connection.ExecuteSQL($"UPDATE dbPrecedents SET PRECLIBRARY = 'ARCHIVE', PrecTitle =  (SELECT LEFT(PrecTitle, { truncIndex })) + '{ postfix }' WHERE PrecID = @ID", pars);
                    return true;
                }
                catch
                {
                    if (duplicateIndex < archiveMaxDuplicateTitleCount)
                    {
                        err = true;
                        duplicateIndex++;
                    }
                    else
                        return false;
                }
            }
            return false;
        }


        public static bool RestorePrecedent(long precID)
        {
            try
            {
                IDataParameter[] pars = new IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.AddParameter("id", SqlDbType.BigInt, 0, precID);
                Session.CurrentSession.Connection.ExecuteSQL($"UPDATE dbPrecedents SET PrecLibrary = NULL, PrecTitle = (SELECT CASE WHEN CHARINDEX(' ({ ArchiveDelPostfixPart }', PrecTitle) = 0 "
                                                             + $"THEN PrecTitle ELSE LEFT(PrecTitle, CHARINDEX(' ({ ArchiveDelPostfixPart }', PrecTitle) - 1) END) WHERE PrecID = @ID", pars);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool IsPartOfMultiPrecedent(long precID)
        {
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("precID", SqlDbType.BigInt, 0, precID);
            
            //check if Precedent being archived is part of a multi-Precedent
            string sql = @"Select * from dbPrecedentMulti where multiMasterID = @precID or multiChildID = @precID";
            System.Data.DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "MultiPrecedent", pars);
            return (dt != null && dt.Rows.Count > 0);
        }


		public void Restore()
		{
			if (_precedent.Tables[Table].Columns.Contains("precDeleted"))
			{
				if (Convert.ToBoolean(GetExtraInfo("precdeleted")) == true)
				{
					SetExtraInfo("precDeleted", false);
					SetExtraInfo("precRetain", DBNull.Value);

					UpdateDataRow();
				}
			}
		}

		public void Delete()
		{
			Delete(true, System.DateTime.Now.AddDays(Session.CurrentSession.DeletionRetentionPeriod), false);
		}

		/// <summary>
		/// Deletes current instance of the object.
		/// </summary>
		public void Delete(bool deleteFile, DateTime retainUntil, bool silent)
		{
			try
			{
				if (deleteFile)
				{
					DocumentManagement.Storage.StorageProvider provider = ThisStorageItem.GetStorageProvider();
					provider.Purge(this, false);
				}
			}
			catch{}

			//Backward compatibility.
			if (_precedent.Tables[Table].Columns.Contains("precDeleted"))
			{
				SetExtraInfo("precDeleted", true);
				SetExtraInfo("precRetain", retainUntil);
			}
			else
				_precedent.Tables[Table].Rows[0].Delete();
			
			UpdateDataRow();
			
		}

		#endregion


		#region IEnquiryCompatible Implementation

		/// <summary>
		/// An event that gets raised when a property changes within the object.
		/// </summary>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

		/// <summary>
		/// Raises the property changed event with the specified event arguments.
		/// </summary>
		/// <param name="e">Property Changed Event Arguments.</param>
		protected void OnPropertyChanged (FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current printer object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.PrecedentEdit), param);
		}

		/// <summary>
		/// Edits the current printer object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}

		#endregion

		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		[Browsable(false)]
		public object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region PasswordProtectedBase

		/// <summary>
		/// Gets the password of the storage item.
		/// </summary>
		protected override string Password
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precpassword"));
			}
			set
			{
				SetExtraInfo("precpassword", value);
			}
		}

		/// <summary>
		/// Gets the password Hint of the storage item.
		/// </summary>
		[Browsable(false)]
		public override string PasswordHint
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precpasswordhint"));
			}
			set
			{
				SetExtraInfo("precpasswordhint", value);
			}
		}

		/// <summary>
		/// Returns the string represenation of the password request screen.
		/// </summary>
		/// <returns>A string.</returns>
		public override string ToPasswordString()
		{
			return ToString();
		}

		#endregion

		#region IStorageItem Members

		System.Drawing.Icon IStorageItem.GetIcon()
		{
			return Common.IconReader.GetFileIcon(String.Format("test.{0}", ThisStorageItem.Extension), Common.IconReader.IconSize.Small, false);
		}

		bool DocumentManagement.Storage.IStorageItem.Supports(DocumentManagement.Storage.StorageFeature feature)
		{
			PrecType type = (PrecType)ThisStorageItem.GetItemType();

			if (type.StorageFeaturesSupported == 0)
			{
				switch (feature)
				{
					case StorageFeature.Versioning:
						return (Session.CurrentSession.EnablePrecedentVersioning);
					case DocumentManagement.Storage.StorageFeature.Retrieving:
						return true;
					case DocumentManagement.Storage.StorageFeature.Storing:
						return true;
                    case StorageFeature.Locking:
                        return true;
					default:
						return false;
				}
			}
			else
				return type.Supports(feature);

		}

		/// <summary>
		/// Gets the extension of the item when saved to a file.
		/// </summary>
		string DocumentManagement.Storage.IStorageItem.Extension
		{
			get
			{
				string ext = Convert.ToString(GetExtraInfo("precextension")).Replace(".", "");
				if (String.IsNullOrEmpty(ext))
				{
					PrecType dt = PrecType.GetPrecType(this.PrecedentType);
					ext = dt.DefaultPrecExtension;
					SetExtraInfo("precextension", ext);
				}
				return ext;
			}
		}

		/// <summary>
		/// Gets the current storage location type id.
		/// </summary>
		internal short CurrentStorageProviderID
		{
			get
			{
				return Common.ConvertDef.ToInt16(GetExtraInfo("preclocation"), -1);
			}
			set
			{
				if (CurrentStorageProviderID != value)
				{
					SetExtraInfo("preclocation", value);
				}
			}
		}

		string DocumentManagement.Storage.IStorageItem.Pointer
		{
			get
			{
				return this.ID.ToString();
			}
		}

		string DocumentManagement.Storage.IStorageItem.DisplayID
		{
			get
			{
				return this.ID.ToString();
			}
		}

		string DocumentManagement.Storage.IStorageItem.Name
		{
			get
			{
				return Title;
			}
		}

		bool DocumentManagement.Storage.IStorageItem.Accepted
		{
			get
			{
				return !Convert.ToBoolean(GetExtraInfo("precdeleted"));
			}
			set
			{
				SetExtraInfo("precdeleted", !value);
			}
		}

		string DocumentManagement.Storage.IStorageItem.Token
		{
			get
			{
				return Convert.ToString(GetExtraInfo("precpath"));
			}
			set
			{
				if (value == null) value = string.Empty;
				SetExtraInfo("precpath", value);
			}
		}

		string IStorageItem.Preview
		{
			get
			{
				return PrecedentPreview;
			}
			set
			{
				PrecedentPreview = value;
			}
		}


		void DocumentManagement.Storage.IStorageItem.ChangeStorage(DocumentManagement.Storage.StorageProvider provider, bool transfer)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			DocumentManagement.Storage.StorageProvider newProvider = DocumentManagement.Storage.StorageManager.CurrentManager.GetStorageProvider(provider.Id);
			if (newProvider != null)
			{
				if (newProvider.Id != CurrentStorageProviderID)
				{
					if (IsNew == false && transfer)
					{
						DocumentManagement.Storage.StorageProvider prov = GetStorageProvider();
						FetchResults si = prov.Fetch(this, true);
						newProvider.Store(this, si.LocalFile, null, true);
					}

					CurrentStorageProviderID = provider.Id;
				}
			}
		}

		public DocumentManagement.Storage.StorageProvider GetStorageProvider()
		{
			return DocumentManagement.Storage.StorageManager.CurrentManager.GetStorageProvider(CurrentStorageProviderID);
		}

		DocumentManagement.Storage.IStorageItemType DocumentManagement.Storage.IStorageItem.GetItemType()
		{
			return CurrentPrecedentType;
		}

		System.IO.FileInfo DocumentManagement.Storage.IStorageItem.GetIdealLocalFile()
		{
			System.IO.DirectoryInfo dir = StorageManager.CurrentManager.LocalDocuments.LocalPrecedentDirectory;
			string fp = System.IO.Path.Combine(dir.FullName, String.Format("{0}.{1}", FWBS.Common.FilePath.ExtractInvalidChars(ThisStorageItem.Pointer), FWBS.Common.FilePath.ExtractInvalidChars(ThisStorageItem.Extension)));
			return new System.IO.FileInfo(fp);
		}

		private DocumentManagement.Storage.IStorageItem ThisStorageItem
		{
			get
			{
				return (DocumentManagement.Storage.IStorageItem)this;
			}
		}

		private bool isnew_storeitem = false;
		bool IStorageItem.IsNew
		{
			get
			{
				return isnew_storeitem;
			}
		}

		bool IStorageItem.IsConflicting
		{
			get
			{
				return false;
			}
		}

		IStorageItem IStorageItem.GetConflict()
		{
			return null;
		}

		bool IStorageItem.IsDirty
		{
			get
			{
				return this.IsDirty;
			}
		}

		void IStorageItem.Update()
		{
			Update();
			isnew_storeitem = false;
		}


		StorageSettingsCollection settings = null;
		StorageSettingsCollection IStorageItem.GetSettings()
		{
			return settings;
		}

		void IStorageItem.ApplySettings(StorageSettingsCollection settings)
		{
			this.settings = settings;
		}
		void IStorageItem.ClearSettings()
		{
			if (settings != null)
				settings.Clear();
			settings = null;
		}


		void IStorageItem.AddActivity(string type, string code)
		{
		}

		void IStorageItem.AddActivity(string type, string code, string data)
		{
		}


		System.Data.DataTable IStorageItem.GetActivities()
		{
			return null;
		}


		#endregion

		#region ISecurable Members

		string FWBS.OMS.Security.ISecurable.SecurityId
		{
			get { return ID.ToString(); }
		}

		FWBS.OMS.Security.SecurityOptions FWBS.OMS.Security.ISecurable.SecurityOptions
		{
			get
			{
				return (FWBS.OMS.Security.SecurityOptions)0;
			}
			set
			{
				throw new NotSupportedException("Security Flags are not valid on a Precedent");
			}
		}

		private DateTime timestamp;
		[Browsable(false)]
		public DateTime TimeStamp
		{
			get
			{
				return timestamp;
			}
		}

		private void CheckPermissions()
		{
			bool isnew = IsNew;
			bool isdirty = IsDirty;
			bool isdeleting = (_precedent.Tables[Table].Rows[0].RowState == DataRowState.Deleted || Convert.ToBoolean(_precedent.Tables[Table].Rows[0]["precdeleted"]) == true);

			if (isnew)
				new SystemPermission(StandardPermissionType.CreatePrecedent).Check();
			else if (isdirty)
			{
				new SystemPermission(StandardPermissionType.UpdatePrecedent).Check();
			}
		}

		#endregion

		#region Multi Precedent Routines

		/// <summary>
		/// Adds to the sessions job list the multi precedents based on the current precedent.
		/// </summary>
		/// <param name="file">The file to use the multi precedent for.</param>
		public void GenerateJobList(OMSFile file)
		{
			GenerateJobList(file, null);
		}

		/// <summary>
		/// Adds to the sessions job list the multi precedents based on the current precedent.
		/// </summary>
		/// <param name="assoc">The associate the multi precedent is for</param>
		public void GenerateJobList(Associate assoc)
		{
			GenerateJobList(assoc.OMSFile, assoc);
		}

		private void GenerateJobList(OMSFile file, Associate assoc)
		{
			if (IsMultiPrecedent)
			{
				PrecedentJobList joblist = Session.CurrentSession.CurrentPrecedentJobList;
				foreach(Precedent.MultiPrecedent multi in MultiPrecedents)
				{
					Precedent prec = null;


					if (multi.Precedent == null)
						prec = Precedent.GetDefaultPrecedent(multi.Type, file);
					else
						prec = multi.Precedent;

					//Make sure that the precedent being used is the same one.
					bool diff = false;
					if (
							prec.Title != multi.Title ||
							prec.Library != multi.Library ||
							prec.Category != multi.Category ||
							prec.SubCategory != multi.SubCategory ||
                            prec.MinorCategory != multi.MinorCategory
                        )
					{
						diff = true;
					}

					PrecedentJob job = new PrecedentJob(prec);

					if (diff)
					{
						job.ErrorMessage = Session.CurrentSession.Resources.GetResource("RESDIFFPREC", "The %PRECEDENT% that was specified in the multi %PRECEDENT% setup appears to be different.", "", true, null).Text;
					}

					var bestfitassoc = file.GetBestFitAssociate(multi.ContactType, multi.AssocType, assoc);

					if (bestfitassoc == null)
					{
						continue;
					}
					else
					{
						job.Associate = bestfitassoc;
					}

					job.SaveMode = multi.SaveMode;
					job.PrintMode = multi.PrintMode;
					job.AsNewTemplate = multi.AsNewTemplate;
					joblist.Add(job);
				}
			}
		}

		#endregion

		#region Multi Precedent

		
		/// <summary>
		/// A simple precedent object which links one precedent to another so that each multi precedent
		/// can be used when the parent precedent is being used.
		/// </summary>
		public class MultiPrecedent : LookupTypeDescriptor
		{
			#region Fields

			private DataRow _info;
			private Precedent _parent = null;
			/// <summary>
			/// Code Lookup Contact Type
			/// </summary>
			private CodeLookupDisplay _contacttype = null;
			/// <summary>
			/// Code Lookup Associate Type
			/// </summary>
			private CodeLookupDisplay _assoctype = null;

			private PrecPrintMode _printmode = PrecPrintMode.None;
			private PrecSaveMode _savemode = PrecSaveMode.None;



			#endregion

			#region Constructors

			internal MultiPrecedent(Precedent parent, DataRow info)
			{
				_info = info;
				_parent = parent;
				_savemode = (PrecSaveMode)Common.ConvertDef.ToEnum(GetExtraInfo("multisavemode"), PrecSaveMode.None);
				_printmode = (PrecPrintMode)Common.ConvertDef.ToEnum(GetExtraInfo("multiprintmode"), PrecPrintMode.None);
			}

			internal MultiPrecedent(Precedent parent, DataRow info, Precedent precedent)
			{
				_info = info;
				_parent = parent;
				SetExtraInfo("multimasterid", _parent.ID);
				SetExtraInfo("multichildid", precedent.ID);
				Title = precedent.Title;
				Type = precedent.PrecedentType;
				Library = precedent.Library;
				Category = precedent.Category;
				SubCategory = precedent.SubCategory;
                MinorCategory = precedent.MinorCategory;
                ContactType = precedent.ContactType;
				AssocType = precedent.AssocType;
				AsNewTemplate = true;
				PrintMode = PrecPrintMode.None;
				SaveMode = PrecSaveMode.None;
			}

			#endregion

			#region Methods

			internal object GetExtraInfo(string fieldName)
			{
				object val = _info[fieldName];

				//UTCFIX: DM - 30/11/06 - return local time
				if (val is DateTime)
					return ((DateTime)val).ToLocalTime();
				else
					return val;
			}

			internal void SetExtraInfo(string fieldName, object value)
			{
				//UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
				if (value is DateTime)
				{
					DateTime dteval = (DateTime)value;
					if (dteval.Kind == DateTimeKind.Unspecified)
						value = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
				}

				if (_info[fieldName] != value)
				{
					_info[fieldName] = value;
					_parent.OnChanged();
				}
			}

			public override string ToString()
			{
				return this.Precedent.Description;
			}


			#endregion

			#region Properties
			
			internal DataRow Info
			{
				get
				{
					return _info;
				}
			}

			[LocCategory("(DETAILS)")]
			public Precedent Precedent
			{
				get
				{
					if (GetExtraInfo("multichildid") == DBNull.Value)
						return null;
					else
						return Precedent.GetPrecedent(Common.ConvertDef.ToInt64(GetExtraInfo("multichildid"),-1));
				}
			}

			internal string Type
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiprectype"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multiprectype", DBNull.Value);
					else
						SetExtraInfo("multiprectype", value);
				}
			}

			internal string Title
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiprectitle"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multiprectitle", DBNull.Value);
					else
						SetExtraInfo("multiprectitle", value);
				}
			}

			internal string Library
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multipreclibrary"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multipreclibrary", DBNull.Value);
					else
						SetExtraInfo("multipreclibrary", value);
				}
			}

			internal string Category
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multipreccategory"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multipreccategory", DBNull.Value);
					else
						SetExtraInfo("multipreccategory", value);
				}
			}

			internal string SubCategory
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiprecsubcategory"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multiprecsubcategory", DBNull.Value);
					else
						SetExtraInfo("multiprecsubcategory", value);
				}
			}

            internal string MinorCategory
            {
                get
                {
                    return Convert.ToString(GetExtraInfo("multiprecminorcategory"));
                }
                set
                {
                    if (value == null || value == "")
                        SetExtraInfo("multiprecminorcategory", DBNull.Value);
                    else
                        SetExtraInfo("multiprecminorcategory", value);
                }
            }

            [LocCategory("Contact")]
			[Lookup("1ContactType")]
			[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
			[CodeLookupSelectorTitle("CONTTYPE","Contact Types")]
			public CodeLookupDisplay ContactTypeCodeLookup
			{
				get
				{
					if (_contacttype == null) 
					{
						_contacttype = new CodeLookupDisplay("CONTTYPE");
						_contacttype.Code = this.ContactType;
					}
					return _contacttype;
				}
				set
				{
					if (_contacttype == null) _contacttype = new CodeLookupDisplay("CONTTYPE");
					_contacttype = value;
					ContactType = _contacttype.Code;
				}
			}


			/// <summary>
			/// The Get and Set for The Admin Kit of the Accoicate Type
			/// </summary>
			[LocCategory("Contact")]
			[Lookup("2ASSOCAS")]
			[CodeLookupUIAttribute(CodeLookupUIAttributes.ChangeCode)]
			[CodeLookupSelectorTitle("ASSTYPE","Associate Types")]
			[Parameter(CodeLookupDisplaySettings.PrecedentAssoc)]
			public CodeLookupDisplay AssocTypeCodeLookup
			{
				get
				{
					if (_assoctype == null) 
					{
						_assoctype = new CodeLookupDisplay("SUBASSOC");
						_assoctype.Code = this.AssocType;
					}
					return _assoctype;
				}
				set
				{
					if (_assoctype == null) _assoctype = new CodeLookupDisplay("SUBASSOC");
					_assoctype = value;
					AssocType = _assoctype.Code;
				}
			}


			[Browsable(false)]
			public string ContactType
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiContactType"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multicontacttype", DBNull.Value);
					else
						SetExtraInfo("multicontacttype", value);
				}	
			}

			
			[Browsable(false)]
			public string AssocType
			{
				get
				{
					return Convert.ToString(GetExtraInfo("multiassoctype"));
				}
				set
				{
					if (value == null || value == "")
						SetExtraInfo("multiassoctype", DBNull.Value);
					else
						SetExtraInfo("multiassoctype", value);
				}	
			}

			[LocCategory("DATA")]
			public PrecSaveMode SaveMode
			{
				get
				{
					return _savemode;
				}
				set
				{
					_savemode = value;
					SetExtraInfo("multisavemode", value);
				}
			}

			[LocCategory("DATA")]		
			[System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.FlagsEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
			public PrecPrintMode PrintMode
			{
				get
				{
					return _printmode;
				}
				set
				{
					_printmode = value;
					SetExtraInfo("multiprintmode", value);
				}
			}

			[LocCategory("DATA")]
			public bool AsNewTemplate
			{
				get
				{
					return Common.ConvertDef.ToBoolean(GetExtraInfo("multinewtemplate"), true);
				}
				set
				{
					SetExtraInfo("multinewtemplate", value);
				}
			}

			#endregion
		}


		#endregion

		#region MultiPrecedent Collection

		public class MultiPrecedentCollection : Crownwood.Magic.Collections.CollectionWithEvents
		{
			private DataTable _info;
			private Precedent _parent;

			private const string Table = "MULTIPREC";
			private const string Sql = "select * from dbprecedentmulti";

			private MultiPrecedentCollection(){}

			internal MultiPrecedentCollection(Precedent parent, DataTable info)
			{
				_info = info.Copy();
				_parent = parent;
				BuildCollection();
			}

			internal MultiPrecedentCollection(Precedent parent)
			{
				_parent = parent;

				IDataParameter [] pars = new IDataParameter[1];
				pars[0] = Session.CurrentSession.Connection.AddParameter("prec", _parent.ID);
                _info = Session.CurrentSession.Connection.ExecuteSQLTable(@"select * from dbprecedentmulti pm
                                                                            inner join dbPrecedents p
                                                                            on p.precID = pm.multiChildID
                                                                            where multimasterid = @prec
                                                                            and (p.PrecLibrary is null or p.PrecLibrary <> 'ARCHIVE')"
                                                                            , "MULTIPREC", pars);
                BuildCollection();
			}

			private void BuildCollection()
			{
				foreach (DataRow row in _info.Rows)
				{
					Add(new MultiPrecedent(_parent, row));
				}

				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (_info.PrimaryKey == null || _info.PrimaryKey.Length == 0)
					_info.PrimaryKey = new DataColumn[1]{_info.Columns["multiID"]};

			}

			public MultiPrecedent New(Precedent precedent)
			{
				DataRow row = _info.NewRow();
				//UTCFIX: DM- 30/11/06 - This will make sure the job id is unique.
				row["multiid"] = System.DateTime.UtcNow.Ticks;
				return new MultiPrecedent(_parent, row, precedent);
			}

			public MultiPrecedent Add(MultiPrecedent value)
			{
				// Use base class to process actual collection operation
				base.List.Add(value as object);

				return value;
			}
			
			public void Remove(MultiPrecedent value)
			{
				// Use base class to process actual collection operation
				base.List.Remove(value as object);
			}

			public void Insert(int index, MultiPrecedent value)
			{
				// Use base class to process actual collection operation
				base.List.Insert(index, value as object);
			}

			public bool Contains(MultiPrecedent value)
			{
				// Use base class to process actual collection operation
				return base.List.Contains(value as object);
			}

			public MultiPrecedent this[int index]
			{
				// Use base class to process actual collection operation
				get { return (base.List[index] as MultiPrecedent); }
			}


			internal void Update()
			{
				bool added = false;
				foreach (MultiPrecedent prec in this)
				{
					if (prec.Info.RowState == DataRowState.Detached)
					{
						added = true;
						_info.Rows.Add(prec.Info);
					}
				}

				foreach(DataRow row in _info.Rows)
				{
					bool exists = false;
					for(int idx = 0; idx < this.Count; idx++)
					{
						MultiPrecedent prec = this[idx];
						if (prec.Info == row)
						{
							prec.SetExtraInfo("multimasterid", _parent.ID);
							prec.SetExtraInfo("multiorder", idx);
							exists = true;
							break;
						}
					}
					if (exists == false) row.Delete();
				}

				if (_info.GetChanges() != null)
				{
					Session.CurrentSession.Connection.Update(_info, Sql + " where multimasterid = " + _parent.ID.ToString());
					if (added)
						_info.AcceptChanges();
				}
			}
		}

		#endregion


		#region IStorageItemVersionable

		private event EventHandler<NewVersionEventArgs> newVersion;
		public event EventHandler<NewVersionEventArgs> NewVersion
		{
			add
			{
				newVersion += value;
			}
			remove
			{
				newVersion -= value;
			}

		}

		void IStorageItemVersionable.OnNewVersion(NewVersionEventArgs e)
		{
			if (newVersion != null)
			{
				newVersion(this, e);
			}
		}

		private DateTime lastFetchedVersions = DateTime.Now;
		private DataTable versionstable = null;
		private System.Collections.Generic.Dictionary<Guid, PrecedentVersion> versions = new System.Collections.Generic.Dictionary<Guid, PrecedentVersion>();

		internal void AddVersion(PrecedentVersion version)
		{
			lock (versions)
			{
				if (!versions.ContainsKey(version.Id))
				{
					versions.Add(version.Id, version);
				}
				lastFetchedVersions = DateTime.MinValue.ToLocalTime();
			}
		}

		public DataTable GetVersionsTable(bool force)
		{
			//UTCFIX: DM - 30/11/06 - Make last fetched versions stamp local.
			if (DateTime.Now.Subtract(lastFetchedVersions.ToLocalTime()).Minutes > 0 || versionstable == null || force)
			{
				IDataParameter[] pars = new IDataParameter[1];
				pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ID);
				versionstable = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbPrecedentVersion where precid = @precid and verdeleted = 0 order by verdepth, vernumber", "VERSIONS", pars);
				lastFetchedVersions = DateTime.Now;
			}

			return versionstable.Copy();
		}

		public IStorageItemVersion[] GetVersions()
		{
			DataTable dt = GetVersionsTable(false);

			using (var vw = new DataView(dt))
			{
				vw.Sort = "verdepth asc, vernumber asc";

				lock (versions)
				{
					System.Collections.Generic.List<PrecedentVersion> list = new System.Collections.Generic.List<PrecedentVersion>();

					foreach (DataRowView r in vw)
					{
						Guid id = (Guid)r["verid"];
						PrecedentVersion dv;
						if (versions.ContainsKey(id))
						{
							dv = versions[id];
							if (dv.IsDirty == false)
							{
								dv.Refresh(this, r.Row);
							}
							versions.Remove(id);
						}
						else
						{
							dv = new DocumentManagement.PrecedentVersion(this, r.Row);
						}
						list.Add(dv);
					}

					if (versions.Count == 0 && list.Count == 0 && IsNew == false)
					{
						PrecedentVersion latest = (DocumentManagement.PrecedentVersion)ThisVersionableStorageItem.CreateVersion();
						latest.Token = ThisStorageItem.Token;
						//Default version needs accepting, version already exists from main header record, but only if the precedent is not new
						if (!IsNew)
							latest.Accepted = true;
						latest.Preview = ThisStorageItem.Preview;
						latest.Checksum = ThisDuplicationStorageItem.Checksum;
						ThisVersionableStorageItem.SetLatestVersion(latest);
						latest.InternalUpdate();
						list.Add(latest);
					}
					else
					{
						foreach (Guid id in versions.Keys)
						{
							PrecedentVersion ver = versions[id];
							list.Add(ver); //Now adding to list even if not new because duplicate precedent versions were being created 
						}
					}

					versions.Clear();


					foreach (PrecedentVersion ver in list)
					{
						versions.Add(ver.Id, ver);
					}

					return list.ToArray();
				}
			}
		}

		public IStorageItemVersion GetVersion(Guid id)
		{
			DataTable dt = GetVersionsTable(false);
			DataView vw = new DataView(dt);
			vw.RowFilter = String.Format("verid = '{0}'", id);
			if (vw.Count > 0)
				return new DocumentManagement.PrecedentVersion(this, vw[0].Row);
			else
				return null;
		}

		public IStorageItemVersion GetVersion(string label)
		{
			if (String.IsNullOrEmpty(label))
				return null;

			DataTable dt = GetVersionsTable(false);
			DataView vw = new DataView(dt);
			vw.RowFilter = String.Format("verlabel = '{0}'", label);
			if (vw.Count > 0)
				return new DocumentManagement.PrecedentVersion(this, vw[0].Row);
			else
				return null;
		}


		private DocumentManagement.PrecedentVersion latest = null;

		public IStorageItemVersion GetLatestVersion()
		{
			DocumentManagement.Storage.IStorageItemVersion[] versions = ThisVersionableStorageItem.GetVersions();

			if (versions.Length == 0)
			{
				latest = (DocumentManagement.PrecedentVersion)ThisVersionableStorageItem.CreateVersion();
				latest.Token = ThisStorageItem.Token;
				//Default version needs accepting, version already exists from main header record, but only if the document is not new
				if (!IsNew)
					latest.Accepted = true;
				latest.Preview = ThisStorageItem.Preview;
				latest.Checksum = ThisDuplicationStorageItem.Checksum;
				latest.InternalUpdate();
			}
			else
			{
				object id = GetExtraInfo("precCurrentVersion");
				if (Convert.IsDBNull(id))
				{
					latest = null;
				}
				else if (latest == null)
				{
					Guid currentId = (Guid)id;

					foreach (PrecedentVersion ver in versions)
					{
						if (ver.Id == currentId)
						{
							latest = ver;
							break;
						}
					}
				}
				else
				{
					Guid currentId = (Guid)id;

					if (latest.Id != currentId)
					{
						foreach (PrecedentVersion ver in versions)
						{
							if (ver.Id == currentId)
							{
								latest = ver;
								break;
							}
						}

					}
				}

				if (latest == null)
					latest = (PrecedentVersion)versions[versions.Length - 1];
			}

			if (!this.versions.ContainsKey(latest.Id))
			{
				lock (this.versions)
				{
					this.versions.Add(latest.Id, latest);
				}
			}
			return latest;
		}


		void IStorageItemVersionable.DeleteVersion(IStorageItemVersion version)
		{
			if (version == null)
				throw new ArgumentNullException("version");
			if (version.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
				throw new ArgumentException("The parent precedent must be the same as the current precedent");
			if (version.IsLatestVersion)
				throw new StorageException("ERRCANTDELLAT", "Cannot delete version '%1%', it is set as the latest version.", null, version.Label);

			lock (version)
			{
				PrecedentVersion docver = version as PrecedentVersion;
				if (docver != null)
				{
					if (docver.GetVersions().Length > 0)
						throw new StorageException("ERRDELHASCHILD", "Cannot delete version, sub versions exist under the version '%1%'.", null, version.Label);
					docver.Delete();
					docver.Update();
				}

				if (versions.ContainsKey(version.Id))
					versions.Remove(version.Id);
				lastFetchedVersions = DateTime.MinValue.ToLocalTime();
			}
		}

		IStorageItemVersion IStorageItemVersionable.CreateVersion()
		{
			//Making the assumption that the unaccepted precedent version is the precedent being saved
			if (versions.Count == 1)
			{
				var latest = GetLatestVersion();
				if (!latest.Accepted)
					return latest;
			}

			return new PrecedentVersion(this);
		}

		IStorageItemVersion IStorageItemVersionable.CreateVersion(IStorageItemVersion original)
		{
			if (original == null)
				throw new ArgumentNullException("original");
			if (original.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
				throw new ArgumentException("The parent precedent must be the same as the current precedent");

			//Making the assumption that the unaccepted precedent version is the precedent being saved
			if (!original.Accepted)
				return original;

			return new PrecedentVersion((PrecedentVersion)original, false);
		}

		IStorageItemVersion IStorageItemVersionable.CreateSubVersion(IStorageItemVersion original)
		{
			if (original == null)
				throw new ArgumentNullException("original");
			if (original.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
				throw new ArgumentException("The parent precedent must be the same as the current precedent");

			//Making the assumption that the unaccepted precedent version is the precedent being saved
			if (!original.Accepted)
				return original;

			return new PrecedentVersion((PrecedentVersion)original, true);
		}


		void IStorageItemVersionable.SetLatestVersion(IStorageItemVersion current)
		{
			if (current == null)
				throw new ArgumentNullException("current");
			if (current.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
				throw new ArgumentException("The parent precedent must be the same as the current precedent");

			lock (current)
			{
				latest = (PrecedentVersion)current;
				latest.InternalUpdate();
				SetExtraInfo("precCurrentVersion", latest.Id);
				ThisStorageItem.Token = latest.Token;
				ThisStorageItem.Preview = latest.Preview;
				ThisDuplicationStorageItem.Checksum = latest.Checksum;
			}
		}

		PrecedentVersion workingVersion;
		void IStorageItemVersionable.SetWorkingVersion(IStorageItemVersion version)
		{
			if (version == null)
				throw new ArgumentNullException("version");
			if (version.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
				throw new ArgumentException("The parent precedent must be the same as the current working precedent");

			workingVersion = (PrecedentVersion)version;
		}

		IStorageItemVersion IStorageItemVersionable.GetWorkingVersion()
		{
			if (workingVersion == null)
				workingVersion = (PrecedentVersion)ThisVersionableStorageItem.GetLatestVersion();
			return workingVersion;
		}

		#endregion
		

		#region ThisStorageItems

		private DocumentManagement.Storage.IStorageItemDuplication ThisDuplicationStorageItem
		{
			get
			{
				return (DocumentManagement.Storage.IStorageItemDuplication)this;
			}
		}

		private DocumentManagement.Storage.IStorageItemVersionable ThisVersionableStorageItem
		{
			get
			{
				return (DocumentManagement.Storage.IStorageItemVersionable)this;
			}
		}


		private DocumentManagement.Storage.IStorageItemLockable ThisLockableStorageItem
		{
			get
			{
				return (DocumentManagement.Storage.IStorageItemLockable)this;
			}
		}

		#endregion

		#region IStorageItemDuplication

		#region Duplication Checksum Logic

		/// <summary>
		/// Gets the checksum of the active precedent.
		/// </summary>
		internal static string GenerateChecksum(string text)
		{

			if (text == null || text.Trim() == String.Empty)
				return "";

			System.Text.UnicodeEncoding enc = new System.Text.UnicodeEncoding();
			byte[] input = enc.GetBytes(text);

			System.Security.Cryptography.SHA1CryptoServiceProvider sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			byte[] hash = sha1.ComputeHash(input);
			System.Text.StringBuilder buff = new System.Text.StringBuilder();
			foreach (byte hashByte in hash)
			{
				buff.Append(String.Format("{0:X1}", hashByte));
			}

			return buff.ToString();
		}

		void IStorageItemDuplication.GenerateChecksum(string value)
		{
			if (String.IsNullOrEmpty(value))
				ThisDuplicationStorageItem.Checksum = null;
			else
				ThisDuplicationStorageItem.Checksum = OMSDocument.GenerateChecksum(value);
		}
		
		/// <summary>
		/// A flag that allows the duplication of a precedent.
		/// </summary>
		private bool _allowduplicateprec = false;

		/// <summary>
		/// A flag that allows the duplication of the precedent.
		/// </summary>
		[EnquiryUsage(true)]
		[Browsable(false)]
		public bool AllowDuplication
		{
			get
			{
				return _allowduplicateprec;
			}
			set
			{
				_allowduplicateprec = value;
			}
		}

		public IStorageItemDuplication CheckForDuplicate()
		{
			OMSDocument duplicate;
			if (Exists(true, out duplicate))
				return duplicate;
			else
				return null;
		}


		/// <summary>
		/// Gets or Sets the duplicate checksum identity.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		string IStorageItemDuplication.Checksum
		{
			get
			{
				string checksum = Convert.ToString(GetExtraInfo("precchecksum"));
				if (String.IsNullOrEmpty(checksum))
				{
					if (latest != null)
						ThisDuplicationStorageItem.Checksum = ((IStorageItemDuplication)latest).Checksum;
				}

				return Convert.ToString(GetExtraInfo("precchecksum"));
			}
			set
			{
				if (String.IsNullOrEmpty(value))
					SetExtraInfo("precchecksum", DBNull.Value);
				else
					SetExtraInfo("precchecksum", value);
			}
		}


		private bool Exists()
		{
			OMSDocument duplicate;
			return Exists(false, out duplicate);
		}

		private bool Exists(bool getdoc, out OMSDocument duplicate)
		{            
			duplicate = null;
			return false;
		}


		#endregion

		#endregion

		#region "IStorageLockable"

		[Browsable(false)]
		public DateTime? CheckedOutTime
		{
		    get
		    {
		        CheckOutStatusRefresh();
		        if (GetExtraInfo("precCheckedOut") == DBNull.Value)
		            return null;
		        return Convert.ToDateTime(GetExtraInfo("precCheckedOut"));
		    }
		}

		[Browsable(false)]
		public int CheckOutDuration
		{
		    get
		    {
		        CheckOutStatusRefresh();
		        DateTime? dte = ThisLockableStorageItem.CheckedOutTime;
		        return dte == null ? 0 : DateTime.Now.Subtract(dte.Value.ToLocalTime()).Minutes;
		    }
		}

		[Browsable(false)]
		public bool IsCheckedOut
		{
		    get
		    {
		        return !IsNew && ThisLockableStorageItem.CheckedOutTime != null;
		    }   
		}

		[Browsable(false)]
		public bool IsCheckedOutByCurrentUser
		{
		    get
		    {
		        if (ThisLockableStorageItem.IsCheckedOut)
		        {
		            User checkedoutby = ThisLockableStorageItem.CheckedOutBy;
		            return (checkedoutby.ID == Session.CurrentSession.CurrentUser.ID);
		        }

		        return false;
            }
		}

		[Browsable(false)]
		public bool IsCheckedOutByAnother
		{
		    get
		    {
		        if (!ThisLockableStorageItem.IsCheckedOut) return false;
		        User checkedOutBy = ThisLockableStorageItem.CheckedOutBy;
		        return (checkedOutBy.ID != Session.CurrentSession.CurrentUser.ID);
		    }
		}

	    private DateTime lastCheckoutCheck = DateTime.Now;
	    private User checkedoutby = null;

	    private void CheckOutStatusRefresh()
	    {
	        // Make last checked out stamp local.
	        if (DateTime.Now.Subtract(lastCheckoutCheck.ToLocalTime()).Minutes > 0)
	        {
	            IDataParameter[] pars = new IDataParameter[1];
	            pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ID);


	            DataTable tbl = Session.CurrentSession.Connection.ExecuteSQLTable("select preccheckedout, preccheckedoutby from dbPrecedents where precID = @precid", "PRECEDENTS", pars);
	            if (tbl.Rows.Count > 0)
	            {
	                var dt = tbl.Rows[0]["preccheckedout"] as DateTime?;
	                if (dt.HasValue && dt.Value.Kind == DateTimeKind.Unspecified)
	                {
	                    dt = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);
	                    if (!dt.HasValue)
	                        SetExtraInfo("precCheckedOut", DBNull.Value);
	                    else
	                        SetExtraInfo("precCheckedOut", dt.Value);

	                    SetExtraInfo("precCheckedOutBy", tbl.Rows[0]["preccheckedoutby"]);
	                }
	            }
	            lastCheckoutCheck = DateTime.Now;
	        }
	    }

        [Browsable(false)]
		public bool CanCheckOut
		{
            get
            {
                return !IsNew && !ThisLockableStorageItem.IsCheckedOut;
            }
		}

		[Browsable(false)]
		public bool CanCheckIn
		{
		    get
		    {
		        return !IsNew && !ThisStorageItem.IsNew && ThisLockableStorageItem.IsCheckedOutByCurrentUser;
		    }
		}

		[Browsable(false)]
		public bool CanUndo
		{
		    get
		    {
		        return !IsNew && !ThisStorageItem.IsNew && ThisLockableStorageItem.IsCheckedOutByCurrentUser;
		    }
		}

		[Browsable(false)]
		public User CheckedOutBy
		{
		    get
		    {
		        CheckOutStatusRefresh();
		        if (GetExtraInfo("precCheckedOutBy") != DBNull.Value)
		        {
		            if (checkedoutby == null || checkedoutby.ID != Convert.ToInt32(GetExtraInfo("precCheckedOutBy")))
		                checkedoutby = User.GetUser(Convert.ToInt32(GetExtraInfo("precCheckedOutBy")));
		        }
		        else
		        {
		            checkedoutby = null;
		        }
		        return checkedoutby;
            }
		}

		[Browsable(false)]
		public string CheckedOutMachine
		{
		    get
		    {
		        if (!ThisLockableStorageItem.IsCheckedOut) return string.Empty;
		        string name = Convert.ToString(GetExtraInfo("precCheckedOutLocation"));
		        string[] pars = name.Split(new string[] { "]:[" }, StringSplitOptions.None);
		        return pars.Length > 0 ? pars[0].Trim('[', ']') : "?";
		    }
		}

		[Browsable(false)]
		public string CheckedOutLocation
		{
		    get
		    {
		        if (!ThisLockableStorageItem.IsCheckedOut) return string.Empty;
		        string name = Convert.ToString(GetExtraInfo("docCheckedOutLocation"));
		        string[] pars = name.Split(new string[] { "]:[" }, StringSplitOptions.None);
		        return pars.Length > 1 ? pars[1].Trim('[', ']') : "?";
		    }
		}

		[Browsable(false)]
		public void CheckOut(FileInfo localFile)
		{
            // CM 120215 - WI 5738  - If Precedent has read only access, prevent it from being checked out
            if (!SecurityManager.CurrentManager.IsGranted(new PrecedentPermission(this, StandardPermissionType.Update)))
            {
                System.Diagnostics.Debug.WriteLine("Read Only Access on Precedent - Checkout disabled", "IStorageItemLockable.CheckOut");
                return;
            }

            if (IsNew == false)
            {
                User user = ThisLockableStorageItem.CheckedOutBy;

                if (!ThisLockableStorageItem.IsCheckedOut)
                {
                    DateTime now = DateTime.Now;

                    IDataParameter[] pars = new IDataParameter[4];
                    pars[0] = Session.CurrentSession.Connection.AddParameter("checkedout", now);
                    pars[1] = Session.CurrentSession.Connection.AddParameter("by", Session.CurrentSession.CurrentUser.ID);
                    pars[2] = Session.CurrentSession.Connection.AddParameter("precid", ID);


                    string location = string.Empty;

                    if (localFile == null || !File.Exists(localFile.FullName))
                        location = string.Format("[{0}]:[?]", Common.Functions.GetMachineName());
                    else
                        location = string.Format("[{0}]:[{1}]", Common.Functions.GetMachineName(), localFile.FullName);

                    pars[3] = Session.CurrentSession.Connection.AddParameter("location", location);

                    Session.CurrentSession.Connection.ExecuteSQL("update dbprecedents set preccheckedout = @checkedout, preccheckedoutby = @by, preccheckedoutlocation = @location where precid = @precid", pars);

                    SetExtraInfo("precCheckedOut", now);
                    SetExtraInfo("precCheckedOutBy", Session.CurrentSession.CurrentUser.ID);
                    SetExtraInfo("precCheckedOutLocation", location);

                    StorageManager.CurrentManager.LocalDocuments.Set(this, localFile, false);

                    ThisStorageItem.AddActivity("CHECKEDOUT", null);
                }
                else
                {
                    if (ThisLockableStorageItem.IsCheckedOutByAnother)
                        throw new StorageItemCheckedOutException(user.FullName, ThisLockableStorageItem.CheckedOutTime.Value);
                }
            }
        }

		[Browsable(false)]
		public void CheckIn()
		{
		    if (IsNew || ThisStorageItem.IsNew) return;
		    User user = ThisLockableStorageItem.CheckedOutBy;

		    if (ThisLockableStorageItem.IsCheckedOutByCurrentUser)
		    {
		        IDataParameter[] pars = new IDataParameter[1];
		        pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ID);
		        Session.CurrentSession.Connection.ExecuteSQL("update dbprecedents set preccheckedout = null, preccheckedoutby = null, preccheckedoutlocation = null where precid = @precid", pars);

		        SetExtraInfo("precCheckedOut", DBNull.Value);
		        SetExtraInfo("precCheckedOutBy", DBNull.Value);
		        SetExtraInfo("precCheckedOutLocation", DBNull.Value);

		        StorageManager.CurrentManager.LocalDocuments.Set(this, null, false);

		        ThisStorageItem.AddActivity("CHECKEDIN", null);
		    }
		    else
		    {
		        if (ThisLockableStorageItem.IsCheckedOutByAnother)
		            throw new StorageItemCheckedOutException(user.FullName, ThisLockableStorageItem.CheckedOutTime.Value);
		    }
		}

		[Browsable(false)]
		public void UndoCheckOut()
		{
		    if (IsNew || ThisStorageItem.IsNew) return;
		    User user = ThisLockableStorageItem.CheckedOutBy;

		    if (ThisLockableStorageItem.CanUndo)
		    {
		        IDataParameter[] pars = new IDataParameter[1];
		        pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ID);
		        Session.CurrentSession.Connection.ExecuteSQL("update dbprecedents set preccheckedout = null, preccheckedoutby = null, preccheckedoutlocation = null where precid = @precid", pars);

		        SetExtraInfo("precCheckedOut", DBNull.Value);
		        SetExtraInfo("precCheckedOutBy", DBNull.Value);
		        SetExtraInfo("precCheckedOutLocation", DBNull.Value);

		        StorageManager.CurrentManager.LocalDocuments.Set(this, null, false);

		        ThisStorageItem.AddActivity("UNDO", null);
		    }
		    else
		    {
		        if (ThisLockableStorageItem.IsCheckedOutByAnother)
		            throw new StorageItemCheckedOutException(user.FullName, ThisLockableStorageItem.CheckedOutTime.Value);
		    }
		}

        [Browsable(false)]
        public void UpdateCheckedOutLocation(FileInfo localFile)
	    {
	        IDataParameter[] pars = new IDataParameter[2];
	        pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ID);
	        string location = string.Empty;
	        if (localFile == null || !File.Exists(localFile.FullName))
	            location = string.Format("[{0}]:[?]", Common.Functions.GetMachineName());
	        else
	            location = string.Format("[{0}]:[{1}]", Common.Functions.GetMachineName(), localFile.FullName);
	        pars[1] = Session.CurrentSession.Connection.AddParameter("location", location);
	        Session.CurrentSession.Connection.ExecuteSQL("update dbprecedents set preccheckedoutlocation = @location where precid = @precid", pars);
	        SetExtraInfo("precCheckedOutLocation", location);
        }
	    #endregion
	}



	/// <summary>
	/// A collection of job list items.
	/// </summary>
	public class PrecedentJobList : System.Collections.IEnumerable
	{
		#region Events

		/// <summary>
		/// An event that gets fired when a single job is removed.
		/// </summary>
		public event EventHandler Removed = null;
		/// <summary>
		/// An event that gets fired when a job is added to the list.
		/// </summary>
		public event EventHandler Added = null;
		/// <summary>
		/// An event that gets fired when a job order is changed.
		/// </summary>
		public event EventHandler Moved = null;
		/// <summary>
		/// An event that gets fired when the jobs list gets cleared.
		/// </summary>
		public event EventHandler Cleared = null;

		#endregion

		#region Fields

		/// <summary>
		/// The array list that holds the actual data.
		/// </summary>
		private System.Collections.ArrayList _data = new System.Collections.ArrayList();

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _jobs = null;

		/// <summary>
		/// Flags whether the list has been persisted to the database.
		/// </summary>
		private bool _persisted = false;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbJobs";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		internal const string Table = "JOBS";

		/// <summary>
		/// The owner of the job list.
		/// </summary>
		private User _owner = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PrecedentJobList()
		{
			Fetch(Session.CurrentSession.CurrentUser);
		}

		/// <summary>
		/// Constructs a job list based on a specified user object.
		/// </summary>
		public PrecedentJobList(User user)
		{
			Fetch(user);
		}

		/// <summary>
		/// Constructs a job list based on a specified user id.
		/// </summary>
		public PrecedentJobList(int usrid)
		{
			User usr = null;
			try
			{
				usr = User.GetUser(usrid);
			}
			catch
			{
				usr = Session.CurrentSession.CurrentUser;
			}
			Fetch(usr);
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Gets a job using a prefered order ordinal value.
		/// </summary>
		public PrecedentJob this[int index]
		{
			get
			{
				return (PrecedentJob)_data[index];
			}
		}

		#endregion

		#region Event Methods

		/// <summary>
		/// Raised the job removed event.
		/// </summary>
		protected void OnRemoved()
		{
			if (Removed != null)
				Removed(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raised the job added event.
		/// </summary>
		protected void OnAdded()
		{
			if (Added != null)
				Added(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raised the job moved event.
		/// </summary>
		protected void OnMoved()
		{
			if (Moved != null)
				Moved(this, EventArgs.Empty);
		}

		/// <summary>
		/// Raised the job cleared event.
		/// </summary>
		protected void OnCleared()
		{
			if (Cleared != null)
				Cleared(this, EventArgs.Empty);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Refreshes the job list from the list held in the database.
		/// </summary>
		public void Refresh()
		{
			Fetch(_owner);
		}

		/// <summary>
		/// Adds a job to the end of the list.
		/// </summary>
		/// <param name="job">Job to add.</param>
		/// <returns>True, if the item was successfully added.</returns>
		public int Add(PrecedentJob job)
		{
			job.Validate();
			try
			{
				_jobs.Rows.Add(job.JobRow);
			}
			catch (System.Data.ConstraintException)
			{
				//DMB added 14/6/2011 to test at site 1 machine for 1 user 
				// so if falls into error mode try additional sleeping
				System.Threading.Thread.Sleep(10);
				job.SetExtraInfo("jobID", DateTime.UtcNow.Ticks);
				_jobs.Rows.Add(job.JobRow);
			}
			int ret = _data.Add(job);
			
			job._parent = this;
			if (ret > -1)
				OnAdded();

			return ret;
		}

		/// <summary>
		/// Inserts a job into a specific order number.
		/// </summary>
		/// <param name="index">Order index.</param>
		/// <param name="job">The job to insert.</param>
		public void Insert(int index, PrecedentJob job)
		{
			job.Validate();
			bool added = false;
			if (_data.Contains(job))
				_data.Remove(job);
			else
				added = true;
			_data.Insert(index, job);
			job._parent = this;
			if (added) OnAdded();
			else OnMoved();
		}

        /// <summary>
        /// Removes an associate at the specific prefered order.
        /// </summary>
        /// <param name="index">The ordinal value to remove.</param>
        public void RemoveAt(int index)
		{
			PrecedentJob job = _data[index] as PrecedentJob;
			if (job != null) job._parent = null;
			_data.RemoveAt(index);
			OnRemoved();
		}

        /// <summary>
        /// Removes an associate at the specific prefered order.
        /// </summary>
        /// <param name="job">The job object to remove.</param>
        public void Remove(PrecedentJob job)
		{
			_data.Remove(job);
			job._parent = null;
			if (job.Completed)
				job.JobRow["jobstatus"] = JobStatus.Completed;
			else
				job.JobRow["jobstatus"] = JobStatus.Cancelled;
			OnRemoved();
		}

		/// <summary>
		/// Checks to see if a specified job already exists within the
		/// list.
		/// </summary>
		/// <param name="job">The job to check for.</param>
		/// <returns>True, if the job already exists.</returns>
		public bool Contains (PrecedentJob job)
		{
			return _data.Contains(job);
		}

		/// <summary>
		/// Clears the whole associate list.
		/// </summary>
		public void Clear()
		{
			foreach (PrecedentJob job in _data)
				job._parent = null;
			_data.Clear();
			OnCleared();
		}

		/// <summary>
		/// Persists the list to the database.
		/// </summary>
		public void Save()
		{
			_persisted = true;
			Update();
		}

		/// <summary>
		/// Persists the job list to the database if the jobs were originally from there.
		/// </summary>
		public void Update()
		{
			if (_persisted)
			{
				int ctr = 0;
				foreach (PrecedentJob job in this)
				{
					if (job.Completed) 
						job.JobRow["jobstatus"] = JobStatus.Completed;
					else
					{
						if (Common.ConvertDef.ToInt16(job.JobRow["jobstatus"], 0) == 0)
						{
							job.JobRow["usrid"] = _owner.ID;
							job.JobRow["joborder"] = ctr;
							ctr++;
						}
					}
				}
				if (_jobs.GetChanges() != null)
				{
					Session.CurrentSession.Connection.Update(_jobs, Sql +  " where usrid = " + Convert.ToString(_owner.ID) + " and jobstatus = 0");
					_jobs.AcceptChanges();
				}
				
			}

			DataView vw = new DataView(_jobs);
			vw.RowFilter = "jobstatus > 0";
			for(int idx = vw.Count -1; idx >= 0; idx--)
				vw.Delete(idx);
			_jobs.AcceptChanges();
			if (_jobs.Rows.Count == 0) 
				_persisted = false;
		}

		/// <summary>
		/// Fetches a persisted job list from the database.
		/// </summary>
		/// <param name="user">The user whose job list to fetch.</param>
		private void Fetch(User user)
		{
			try
			{
				Session.CurrentSession.Connection.Connect(true);

				if (user == null) 
					_owner = Session.CurrentSession.CurrentUser;
				else
					_owner = user;

				_data.Clear();
				IDataParameter[] pars = new  IDataParameter[1];
				pars[0] = Session.CurrentSession.Connection.AddParameter("usrid", SqlDbType.Int, 0, _owner.ID);
				_jobs = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where usrid = @usrid and jobstatus = 0 order by joborder asc", Table, pars);
				if (_jobs != null)
				{
					_jobs.Columns["usrid"].DefaultValue = _owner.ID;
					_jobs.Columns["joborder"].DefaultValue = 0;
					_jobs.Columns["jobprintmode"].DefaultValue = PrecPrintMode.None;
					_jobs.Columns["jobsavemode"].DefaultValue = PrecSaveMode.None;
					_jobs.Columns["jobnewtemplate"].DefaultValue = true;
					_jobs.Columns["jobhaserror"].DefaultValue = false;
					_jobs.Columns["jobstatus"].DefaultValue = JobStatus.Live;

					if (_jobs.Rows.Count > 0)
					{
						_persisted = true;
						foreach (DataRow row in _jobs.Rows)
						{
							PrecedentJob job = new PrecedentJob(row, this);
							job.Validated = true;
							_data.Add(job);
						}
					}

					if (_jobs.PrimaryKey == null || _jobs.PrimaryKey.Length == 0)
						_jobs.PrimaryKey = new DataColumn[1]{_jobs.Columns["jobid"]};
				}
			}
			finally
			{
				Session.CurrentSession.Connection.Disconnect(true);
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Holds the job schema.
		/// </summary>
		internal DataTable JobSchema
		{
			get
			{
				return _jobs;
			}
		}



		/// <summary>
		/// Gets the live job count of the job list.
		/// </summary>
		public int LiveCount
		{
			get
			{
				int toreturn = 0;
				try
				{
					foreach (PrecedentJob pj in _data)
					{
						if (pj.Completed == false)
							toreturn++;
					}
				}
				catch
				{
				}			
				return toreturn;;
			}
		}

		public int Count
		{
			get
			{
				return _data.Count;
			}
		}

		/// <summary>
		/// Gets the user of the current job list.
		/// </summary>
		public User Owner
		{
			get
			{
				return _owner;
			}
		}

		#endregion

		#region IEnumerable Implementation

		public System.Collections.IEnumerator GetEnumerator() 
		{ 
			return _data.GetEnumerator(); 
		} 

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets the available persisted job list owners other that the current user from the database.
		/// </summary>
		/// <returns>A data table of users.</returns>
		public static DataTable GetAvailableJobListOwners()
		{
			Session.CurrentSession.CheckLoggedIn();
			string sql = "select null as usrid, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as usrfullname union select dbuser.usrid, dbuser.usrfullname from dbuser inner join dbjobs on dbuser.usrid = dbjobs.usrid where dbuser.usrid <> @CU and dbjobs.jobstatus = 0 group by dbuser.usrid, dbuser.usrfullname";
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			pars[1] = Session.CurrentSession.Connection.AddParameter("CU", Session.CurrentSession.CurrentUser.ID);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "USERS", pars);
			return dt;
		}

		#endregion
	}

	public class PrecedentLink
	{
		#region Fields

		/// <summary>
		/// The current precedent being used.
		/// </summary>
		protected Precedent _prec = null;

		
		/// <summary>
		/// The associate object to the precedent.
		/// </summary>
		private Associate _assoc = null;


		/// <summary>
		/// The field parser that gets used with the associate and client information.
		/// </summary>
		private FieldParser _parser = null;

		/// <summary>
		/// Extra information to pass to the job list object.
		/// </summary>
		private Common.KeyValueCollection _params = null;

		/// <summary>
		/// A flag that determines whether the precedent can be used.
		/// </summary>
		private bool _validated = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor to create the Precedent link with a pre-client.
		/// </summary>
		public PrecedentLink(Precedent prec)
		{
			_prec = prec;
			if (_prec== null)
				throw new OMSException(HelpIndexes.PrecedentNotFound,"");
		}

		/// <summary>
		/// Constructor to create the Precedent link with an existing associate.
		/// </summary>
		public PrecedentLink(Precedent prec, Associate assoc) : this (prec)
		{
			_prec = prec;
			Associate = assoc;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal PrecedentLink(){}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the precedent that is being used..
		/// </summary>
		public virtual Precedent Precedent
		{
			get
			{
				if (_validated)
					return _prec;
				else
				{
					throw new OMSException2("ERRPRECNOTVALID", "The precedent has not passed validation.");
				}
			}

		}

		/// <summary>
		/// Gets or Sets the Associate to work with for this job.
		/// </summary>
		public virtual Associate Associate
		{
			get
			{
				return _assoc;
			}
			set
			{
				if (_assoc != value)
				{
					_assoc = value;

					if (_parser != null)
					{
						Validate();
						_parser.ChangeObject(this);
					}

					_validated = false;
					Validate();
				}
				if (_assoc == null) _validated = false;
			}
		}


		/// <summary>
		/// Gets a field parser object associated with the current job.
		/// </summary>
		public FieldParser Parser
		{
			get
			{
				if (_parser == null)
				{
					_parser = new FieldParser(_assoc);

					if (Session.CurrentSession.CurrentUser.WorksForMatterHandler && _assoc != null)
						_parser.CurrentFeeEarner = _assoc.OMSFile.PrincipleFeeEarner;

					_parser.ChangeParameters(_params);
				}
				return _parser;
			}
		}

		/// <summary>
		/// Gets extra parameter information.
		/// </summary>
		public Common.KeyValueCollection Params
		{
			get
			{
				if (_params == null)
					_params = new Common.KeyValueCollection();
				return _params;
			}
		}

		/// <summary>
		/// Applies the job parameters with to field parsing parameters.
		/// </summary>
		public void ApplyParams()
		{
			Parser.ChangeParameters(_params);
		}

		/// <summary>
		/// Gets or Sets the validated flag.
		/// </summary>
		internal bool Validated
		{
			get
			{
				return _validated;
			}
			set
			{
				_validated = value;
			}
		}

	
		#endregion

		#region Methods

		/// <summary>
		/// Fetches the precedent object from its location.
		/// </summary>
		/// <returns></returns>
		public DocumentManagement.Storage.FetchResults Merge()
		{
			PrecedentLinkCancelEventArgs e = new PrecedentLinkCancelEventArgs(this, false);
			Precedent.OnMerging(e);

			if (e.Cancel)
			{
				return null;
			}
			else
			{
				DocumentManagement.Storage.StorageProvider provider = ((DocumentManagement.Storage.IStorageItem)Precedent).GetStorageProvider();
				return provider.Fetch(Precedent, true);
			}
		}

		/// <summary>
		/// Validates the precedent and link information.
		/// </summary>
		public void Validate()
		{
			if (_validated == false)
			{
				if (_prec != null)
				{
					PrecedentLinkCancelEventArgs e = new PrecedentLinkCancelEventArgs(this, false);
					_prec.OnValidating(e);
					if (e.Cancel)
						_validated = false;
					else
						_validated = true;
				}
			}
		}

		/// <summary>
		/// Performs a second stage merge on the precedent.
		/// </summary>
		public System.IO.FileInfo SecondStageMerge()
		{
			//Only if a null precedent has been passed or the precedent is text only do the second stage
			//merge.
			PrecedentSecondStageMergingEventArgs e = new PrecedentSecondStageMergingEventArgs(this, false);
			Precedent.OnSecondStageMerging(e);
			if (e.Cancel) return null;
			if (Precedent.TextOnly == true && Precedent.AllowSecondStageMerge == true)
				return _parser.BuildMergeFile(e.MergeData);
			else
				return null;
		}

		

		public Associate GetBestFitAssociate(Associate inassoc)
		{
			
			return inassoc.OMSFile.GetBestFitAssociate(_prec.ContactType, _prec.AssocType);

		}

		/// <summary>
		/// Get Best Fit Associate will try and match the precedent to the available#
		/// precedent
		/// </summary>
		/// <param name="infile">File Object to list through Associates</param>
		/// <returns>Associate Object</returns>
		public Associate GetBestFitAssociate(OMSFile infile)
		{
			return infile.GetBestFitAssociate(_prec.ContactType, _prec.AssocType);

		}

		#endregion
	}

	/// <summary>
	/// Precedent Job takes on the precedent object and then exposes the automation
	/// facilities that are relative to the job such as QuickSave. 
	/// </summary>
	public class PrecedentJob : PrecedentLink, IExtraInfo
	{
		#region Fields


		/// <summary>
		/// A flag thatspecifies whether the job has been completed or not.
		/// </summary>
		private bool _completed = false;

		/// <summary>
		/// An underlying data row that holds the job information for the persisting the information.
		/// </summary>
		private DataRow _job = null;

		/// <summary>
		/// A reference to the owning precedent job list.
		/// </summary>
		internal PrecedentJobList _parent = null;



		#endregion
		
		#region Constructors

		/// <summary>
		/// Constructor to create the Precedent Job, the precedent must be sent
		/// </summary>
		public PrecedentJob(Precedent prec) : base (prec)
		{
			_job = Session.CurrentSession.CurrentPrecedentJobList.JobSchema.NewRow();
			//UTCFIX: DM- 30/11/06 - This will make sure the job id is unique.
			System.Threading.Thread.Sleep(1);
			SetExtraInfo("jobid", DateTime.UtcNow.Ticks);
			SetExtraInfo("precid", _prec.ID);
			FeeEarner = Session.CurrentSession.CurrentFeeEarner;
			BuildXML();
		}

		/// <summary>
		/// Copies an existing precedent job into a new one.
		/// </summary>
		/// <param name="job"></param>
		internal PrecedentJob(PrecedentJob job) : this(job._prec)
		{
			
			object [] vals = new object [job.JobRow.Table.Columns.Count];
			job.JobRow.ItemArray.CopyTo(vals, 0);
			_job.ItemArray = vals;
			Associate = job.Associate;
			Parser.CurrentFeeEarner = FeeEarner;
			BuildXML();
		}

		/// <summary>
		/// Constructor to create the precedent job with an underlying persisted data source.
		/// </summary>
		/// <param name="job">The data row</param>
        /// <param name="list"></param>
		internal PrecedentJob (DataRow job, PrecedentJobList list) 
		{
			_job = job;
			_parent = list;
			_prec = Precedent.GetPrecedent(Convert.ToInt64(job["precid"]));
			Associate = Associate.GetAssociate(Convert.ToInt64(job["associd"]));
			SetExtraInfo("precid", _prec.ID);
			Parser.CurrentFeeEarner = FeeEarner;
			BuildXML();
		}

		#endregion
				
		#region Properties

		/// <summary>
		/// Gets the job identifier.
		/// </summary>
		public long ID 
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("jobid"));
			}
		}
		
		/// <summary>
		/// Gets or Sets the printer to be used as an override.
		/// </summary>
		public Printer Printer
		{
			get
			{
				int id = Common.ConvertDef.ToInt32(GetXmlProperty("printer", -1), -1);
				if (id == -1)
					return null;
				else
					return Printer.GetPrinter(id);
				
			}
			set
			{
				if (value == null)
					SetXmlProperty("printer", "");
				else
					SetXmlProperty("printer", value.ID);
			}
		}

		/// <summary>
		/// Gets or Sets the associate of the job.
		/// </summary>
		public override Associate Associate
		{
			get
			{
				return base.Associate;
			}
			set
			{
				base.Associate = value;
				if (value != null)
				{
					SetExtraInfo("associd", value.ID);
					if (Session.CurrentSession.CurrentUser.WorksForMatterHandler)
						FeeEarner = value.OMSFile.PrincipleFeeEarner;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the fee earner that the job is being done by.
		/// </summary>
		public FeeEarner FeeEarner
		{
			get
			{
				if (GetExtraInfo("feeusrid") == DBNull.Value)
					return Session.CurrentSession.CurrentFeeEarner;
				else
					return FeeEarner.GetFeeEarner(Common.ConvertDef.ToInt32(GetExtraInfo("feeusrid"), -1));
			}
			set
			{
				if (value == null)
					SetExtraInfo("feeusrid", DBNull.Value);
				else
					SetExtraInfo("feeusrid", value.ID);

				Parser.CurrentFeeEarner = FeeEarner;
			}
		}

		/// <summary>
		/// Gets or Sets the save settings on merge of  that gets created 
		/// from the precedent used.
		/// </summary>
		public FWBS.OMS.PrecSaveMode SaveMode
		{
			get
			{
				return (PrecSaveMode)Common.ConvertDef.ToEnum(GetExtraInfo("jobsavemode"), PrecSaveMode.None);
			}
			set
			{
				SetExtraInfo("jobsavemode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the print settings on merge of the document that gets created 
		/// from the precedent used.
		/// </summary>
		public FWBS.OMS.PrecPrintMode PrintMode
		{
			get
			{
				return (PrecPrintMode)Common.ConvertDef.ToEnum(GetExtraInfo("jobprintmode"), PrecPrintMode.None);
			}
			set
			{
				SetExtraInfo("jobprintmode", value);
			}
		}



		/// <summary>
		/// Gets or Sets a flag that specifies whether this Job should be treated as a new Job 
		/// and not imported into existing documents.
		/// </summary>
		public bool AsNewTemplate
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetExtraInfo("jobnewtemplate"), true);
			}
			set
			{
				SetExtraInfo("jobnewtemplate", value);
			}
		}

		/// <summary>
		/// Gets or Sets whether this Job has been completed.
		/// </summary>
		public bool Completed
		{
			get
			{
				return _completed;
			}
			set
			{
				if (value)
				{
					HasError = false;
					ErrorMessage = "";
				}
				_completed = value;
			}
		}

		/// <summary>
		/// Gets or Sets whether this Job has had an error.
		/// </summary>
		public bool HasError
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetExtraInfo("jobhaserror"), false);
			}
			set
			{
				if (value) Completed = false;
				SetExtraInfo("jobhaserror", value);
			}
		}

		/// <summary>
		/// Gets or Sets the error message.
		/// </summary>
		public string ErrorMessage
		{
			get
			{
				return Convert.ToString(GetExtraInfo("joberror"));
			}
			set
			{
				if (value != null && value != "") 
				{
					HasError = true;
					Completed = false;
				}
				if (value == null || value == "")
					SetExtraInfo("joberror", DBNull.Value);
				else
					SetExtraInfo("joberror", value);
			}
		}



		
		/// <summary>
		/// Gets the internal job row.
		/// </summary>
		internal DataRow JobRow
		{
			get
			{
				return _job;
			}
		}

		/// <summary>
		/// Gets a flag that indicates whether the job belongs to another user other than the
		/// currently logged in user.
		/// </summary>
		public bool IsRemote
		{
			get
			{
				int usr = Convert.ToInt32(GetExtraInfo("usrid"));
				if (usr == Session.CurrentSession.CurrentUser.ID)
					return false;
				else
					return true;
			}
		}


		#endregion

		#region Methods

		public override string ToString()
		{
			string txt = this.ID.ToString();
			txt += Environment.NewLine;
			txt += Precedent.Title;
			txt += Precedent.Description;
			txt += Environment.NewLine;
			txt += this.FeeEarner.FullName;
			txt += Environment.NewLine;
			txt += Associate.OMSFile.ToString();
			txt += Environment.NewLine;
			txt += this.Associate.Contact.Name;
			return txt;
		}

		/// <summary>
		/// Adopts the job from a remote job list item and copies it into the current users
		/// precedent job list.
		/// </summary>
		public bool Adopt()
		{
			if (IsRemote)
			{
				PrecedentJobList originallist = _parent;

				Session.CurrentSession.CurrentPrecedentJobList.Add(new PrecedentJob(this));

				if (originallist != null)
				{
					originallist.Remove(this);
					JobRow["jobstatus"] = JobStatus.Adopted;
					originallist.Update();
				}
				return true;
			}
			else
				return false;

		}

		/// <summary>
		/// Gets the value form athe underlying data source.
		/// </summary>
		/// <param name="fieldName">The field name to get.</param>
		/// <returns>The value.</returns>
		public object GetExtraInfo(string fieldName)
		{
			object val = _job[fieldName];

			//UTCFIX: DM - 30/11/06 - return local time
			if (val is DateTime)
				return ((DateTime)val).ToLocalTime();
			else
				return val;
		}

		/// <summary>
		/// Sets the underlying data source to specific value.
		/// </summary>
		/// <param name="fieldName">The field name to set.</param>
		/// <param name="val">The value to set the field to.</param>
		public void SetExtraInfo(string fieldName, object val)
		{
			//UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
			if (val is DateTime)
			{
				DateTime dteval = (DateTime)val;
				if (dteval.Kind == DateTimeKind.Unspecified)
					val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
			}

			_job[fieldName] = val;
		}

		#endregion

		#region XML Settings Methods

		private XmlProperties xmlprops;


		/// <summary>
		/// Builds a schema of a default type.
		/// </summary>
		private void BuildXML()
		{
			if (xmlprops == null)
				xmlprops = new XmlProperties(this, "jobXML");
		}

		private object GetXmlProperty(string name, object defaultValue)
		{
			BuildXML();
			return xmlprops.GetProperty(name, defaultValue);
		}

		private void SetXmlProperty(string name, object val)
		{
			BuildXML();
			xmlprops.SetProperty(name, val);
			xmlprops.Update();
		}

	

		#endregion
				
		#region IExtraInfo Members

	 
		Type IExtraInfo.GetExtraInfoType(string fieldName)
		{
			throw new NotImplementedException();
		}

		DataSet IExtraInfo.GetDataset()
		{
			throw new NotImplementedException();
		}

		DataTable IExtraInfo.GetDataTable()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
	

	#region Document Saving Event Handling

	public delegate void DocumentSavingHandler (object sender, DocumentSavingEventArgs e);

	public class DocumentSavingEventArgs : CancelEventArgs
	{
		private OMSDocument _doc;

		public DocumentSavingEventArgs (OMSDocument document, bool cancel)
		{
			_doc = document;
			base.Cancel = cancel;
		}

		public OMSDocument Document
		{
			get
			{
				return _doc;
			}
		}
	}

	public delegate void DocumentSavedHandler (object sender, DocumentSavedEventArgs e);

	public class DocumentSavedEventArgs : CancelEventArgs
	{
		private OMSDocument _doc;

		public DocumentSavedEventArgs (OMSDocument document)
		{
			_doc = document;
		}

		public OMSDocument Document
		{
			get
			{
				return _doc;
			}
		}
	}

	public delegate void PhysicalDocumentSavedEventHandler(object sender, PhysicalDocumentSavedEventArgs e);

	/// <summary>
	/// Precedent Validating event arguments.
	/// </summary>
	public class PhysicalDocumentSavedEventArgs : CancelEventArgs
	{
		private OMSDocument _doc;

		public PhysicalDocumentSavedEventArgs(OMSDocument document)
		{
			_doc = document;
		}

		public OMSDocument Document
		{
			get
			{
				return _doc;
			}
		}
	}

	#endregion

	#region Precedent Link & Second Stage Merging Event Handling
	
	public delegate void PrecedentLinkCancelHandler(object sender, PrecedentLinkCancelEventArgs e);

	/// <summary>
	/// Precedent Validating event arguments.
	/// </summary>
	public class PrecedentLinkCancelEventArgs : CancelEventArgs
	{
		private PrecedentLink _preclink;
	
		public PrecedentLinkCancelEventArgs (PrecedentLink preclink, bool cancel)
		{
			_preclink = preclink;
			base.Cancel = cancel;
		}

		public PrecedentLink PrecLink
		{
			get
			{
				return _preclink;
			}
		}

	}
	
	public delegate void PrecedentSecondStageMergingEventHandler(object sender, PrecedentSecondStageMergingEventArgs e);
	
	/// <summary>
	/// Precedent Validating event arguments.
	/// </summary>
	public class PrecedentSecondStageMergingEventArgs : PrecedentLinkCancelEventArgs
	{
		private DataTable _dt = new DataTable();
	
		public PrecedentSecondStageMergingEventArgs (PrecedentLink preclink, bool cancel) : base (preclink, cancel)
		{
		}

		public DataTable MergeData
		{
			get
			{
				return _dt;
			}
		}

	}

	#endregion
	
}

