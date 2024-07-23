using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;




namespace FWBS.OMS
{
    using DocumentManagement;
    using DocumentManagement.Storage;
    using FWBS.OMS.Security;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.StatusManagement;
    using FWBS.OMS.StatusManagement.Activities;


    /// <summary>
    /// A class that describes a document against a client, file or associate.
    /// </summary>
    [Security.SecurableType("DOCUMENT")]
	public class OMSDocument : PasswordProtectedBase, IEnquiryCompatible, IDisposable, IAlert, IStorageItem, IStorageItemVersionable, IStorageItemLockable, IStorageItemDuplication, Security.ISecurable, IOMSType, IExtendedDataCompatible
    {

        private bool milestonePermissionsErrorMessageDisplayed;
		
        #region Fields

        private ExtendedDataList _extData = null;

		/// <summary>
		/// The Reminder Note
		/// </summary>
		private string _remindernote = "";
		/// <summary>
		/// The Reminder Subject
		/// </summary>
		private string _remindersubject = "";
		/// <summary>
		/// The Reminder Days
		/// </summary>
		private int _reminderdays = 0;
		/// <summary>
		/// The Enable Reminder
		/// </summary>
		private bool _reminder = false;
		/// <summary>
		/// The Reminder Due Date
		/// </summary>
		private Common.DateTimeNULL _reminderdue;
		/// <summary>
		/// The Fee Earner to assign the the task to
		/// </summary>
		private int _reminderfeeearner = Session.CurrentSession.CurrentFeeEarner.ID;
		/// <summary>
		/// Append this note to the Client Viewable Note Area of the File
		/// </summary>
		private string _clientnotes = "";
		/// <summary>
		/// Append this note to the File Notepad not Viewable Note Area of the File
		/// </summary>
		private string _filenotes = "";
		/// <summary>
		/// The new file review date, if any.
		/// </summary>
		private Common.DateTimeNULL _newfilereviewdate = DBNull.Value;

		/// <summary>
		/// Time Records
		/// </summary>
		private TimeCollection _timerecords;

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _doc = null;
        /// <summary>
        /// Email data.
        /// </summary>
        private EmailDocument email = null;
		/// <summary>
		/// Time Data Source
		/// </summary>
		private DataTable _time = null;
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		private const string Sql = "select * from config.dbdocument";
		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		private const string Table = "DOCUMENT";
		
		/// <summary>
		/// Editing Time stored in mem
		/// </summary>
		private int _editingtime = 0;

		/// <summary>
		/// An array of sub documents to save.
		/// </summary>
		private SubDocument [] _subDocuments = null;

		/// <summary>
		/// A reference to an auto generated sms message.
		/// </summary>
		private SMS _sms = null;

		/// <summary>
		/// Holds the number of spelling mistakes in the current document.
		/// </summary>
		private int _spellingErrors = 0;


        private bool _DocMaxRevisionCount=false; 

		/// <summary>
		/// Holds the number of grammer mistakes in the current document.
		/// </summary>
		private int  _grammarErrors = 0;


		/// <summary>
		/// A reference to the OMS file that holds the duplicated document.
		/// </summary>
		private OMSFile _duplicatedFileRef = null;

		/// <summary>
		/// A flag that allows the duplication of a document.
		/// </summary>
		private bool _allowduplicatedoc = false;
		
		/// <summary>
		/// Preview of the document.
		/// </summary>
		private string _preview = null;

		/// <summary>
		/// A memory variable so that once saved the document will not automatically close.
		/// </summary>
		private bool _continueAfterSave = false;

        /// <summary>
        /// Temporary alerts.  
        /// </summary>
        private Dictionary<string, Alert> _alerts = new Dictionary<string, Alert>();


		#endregion

		#region Constructors
		
		/// <summary>
		/// Sets up a derived object as a new storage item object. 		
		/// </summary>
		internal OMSDocument()
		{
		    isnew_storeitem = true;

		    _doc = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
		    _doc.Columns["docid"].AutoIncrement = true;

		    //Add a new record.
		    Global.CreateBlankRecord(ref _doc, true);

		    Init();
        }

        private void Init()
        {
            //Set the created by and created date of the item.
            this.SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
            this.SetExtraInfo("Created", DateTime.Now);
            this.SetExtraInfo("docAuthored", DateTime.Now);

            //Storage provider must set this to false when file is saved successfully.
            this.SetExtraInfo("docDeleted", true);

            this.OnExtLoaded();
        }

        [EnquiryUsage(true)]
        internal OMSDocument(long id) : this(id, Session.CurrentSession.DuplicateDocumentIDs)
        {
        }

        [EnquiryUsage(true)]
        internal OMSDocument(string extid) : this(extid, Session.CurrentSession.DuplicateDocumentIDs)
        {
        }

		/// <summary>
		/// Creates and instance of an existing document from the database.
		/// </summary>
		/// <param name="id">Storage item identifier used to fetch a unqie single item.</param>
        /// <param name="duplicateCheck"></param>
		internal OMSDocument (long id, bool duplicateCheck)
		{
			Fetch(id.ToString(), duplicateCheck, false, null);

			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentDocuments.Add(ID.ToString(), this);

            this.OnExtLoaded();
        }

        internal OMSDocument (string extid, bool duplicateCheck)
		{
			Fetch(extid, duplicateCheck, true, null);

			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentDocuments.Add(ID.ToString(), this);

            this.OnExtLoaded();
        }


        public OMSDocument(Associate assoc, string description, Precedent basePrecedent, Precedent lastPrecedent, int editTime, DocumentDirection direction, string extension, short storageLocation, SubDocument[] subDocuments, Guid FolderGuid)
            : this(assoc, description, basePrecedent, lastPrecedent, editTime, direction, extension, storageLocation, subDocuments)
        {
            if (FolderGuid != Guid.Empty)
            {
                SetExtraInfo("docFolderGUID", (Convert.ToString(FolderGuid)));
            }
        }


		/// <summary>
		/// Creates a new document object using a client.
		/// </summary>
		/// <param name="assoc">The associate object that is being assigned to the newly created document.</param>
		/// <param name="description">The friendly description of the document.</param>
		/// <param name="basePrecedent">The original precedent directly related to the document.</param>
		/// <param name="lastPrecedent">The precedent directly related to the document.</param>
		/// <param name="direction">The direction of the document, in or out.</param>
		/// <param name="extension">The file extension to use to override the original.</param>
		/// <param name="storageLocation">The storage location identifer 0 = File System, 1 = BLOB.</param>
		/// <param name="editTime">The default editing time of the document.</param>
		/// <param name="subDocuments">Any sub documents that may be used.</param>
        public OMSDocument(Associate assoc, string description, Precedent basePrecedent, Precedent lastPrecedent, int editTime, DocumentDirection direction, string extension, short storageLocation, SubDocument[] subDocuments)
            : this()
		{
			Session.CurrentSession.CheckLoggedIn();

            StorageManager.CurrentManager.ValidateFileExtension(extension);

            isnew_storeitem = true;

			//Check to see if a precedent is attached.
			if (basePrecedent == null)
				throw new OMSException2("PRECREQ4DOC", "There must be a base precedent for the document when a new document is created.", "");

			//Set the client file associate information.
			ChangeAssociate(assoc);
			
			//Build a default document description.
			if (string.IsNullOrEmpty(description))
                description = GenerateDocumentDescription(assoc, description, basePrecedent, lastPrecedent, direction);

			//Set defaults.
			DocType type = DocType.GetDocType(basePrecedent.PrecedentType);
			if (description.Length > 150)
				SetExtraInfo("docdesc", description.Substring(0,150));
			else
				SetExtraInfo("docdesc", description);
			SetExtraInfo("docdirection", (direction == DocumentDirection.In ? true : false));
			SetExtraInfo("doctype", type.Code);
			SetExtraInfo("docfilename", String.Empty);
			SetExtraInfo("docbrid", Session.CurrentSession.CurrentBranch.ID);
			SetExtraInfo("docAppID", type.DefaultApplication);

            //***No folderGuid passed in - so we want to generate
            //Set based on the matter, template etc. if we don't have a folderguid
            //this.FolderGUID = 



            if (Session.CurrentSession.CurrentUser.WorksForMatterHandler)
                //Author
                Author = OMSFile.PrincipleFeeEarner;
            else
                Author = Session.CurrentSession.CurrentUser.WorksFor;



			ReminderDays = Session.CurrentSession.TaskReminder;
			
			//Find out the number of days from now the next file review date will be.
			short filereviewdaycount = 0;

			//Set the base precedent id if known.
			if (basePrecedent != null)
			{
				SetExtraInfo("docbaseprecid",basePrecedent.ID);
				ReminderSubject = basePrecedent.ReminderSubject;
				if (basePrecedent.ReminderDayCount > 0)
					ReminderDays = basePrecedent.ReminderDayCount;
				HasReminder = basePrecedent.Reminder;
				filereviewdaycount = basePrecedent.ReminderReviewDayCount;

				//If the base precedent has an SMS message associated and the client is licensed for
				//Then setup an SMS object.
				if (basePrecedent.SMSMessage != String.Empty)
					SMSText = basePrecedent.SMSMessage;
			}

			//Set the last precedent id if known.
			if (lastPrecedent != null)
			{
				SetExtraInfo("docprecid", lastPrecedent.ID);
				ReminderSubject = lastPrecedent.ReminderSubject;
				if (lastPrecedent.ReminderDayCount > 0)
					ReminderDays = lastPrecedent.ReminderDayCount;
				HasReminder = lastPrecedent.Reminder;
				filereviewdaycount = lastPrecedent.ReminderReviewDayCount;
				
				//If the last precedent used has an SMS message associated and the client is licensed for
				//Then setup an SMS object.
				if (lastPrecedent.SMSMessage != String.Empty)
					SMSText = lastPrecedent.SMSMessage;
			}

			//Set the new file review date if applicable.
            if (filereviewdaycount >= 0)
            {
                //UTCFIX: DM - 30/11/06 - Make review date must be local.
                NewFileReviewDate = DateTime.Today.AddDays(filereviewdaycount);
            }
            else
                NewFileReviewDate = DBNull.Value;

			
			_editingtime = editTime;

			//Set the storage location provider if not already specified.
			if (storageLocation < 0) 
			{
				OMSFile file = this.OMSFile;
				if (file.DefaultStorageProvider < 0)
				{
					if (file.CurrentFileType.DefaultStorageProvider < 0)
					{
						if (file.Client.DefaultStorageProvider < 0)
						{
							if (file.Client.CurrentClientType.DefaultStorageProvider < 0)
							{
								if (Session.CurrentSession.DefaultDocStorageProvider < 0)
									storageLocation = type.DefaultStorageProvider;
								else
									storageLocation = Session.CurrentSession.DefaultDocStorageProvider;
							}
							else
								storageLocation = file.Client.CurrentClientType.DefaultStorageProvider;
						}
						else
							storageLocation = file.Client.DefaultStorageProvider;
					}
					else
						storageLocation = file.CurrentFileType.DefaultStorageProvider;
				}
				else
					storageLocation = file.DefaultStorageProvider;
 
				
			}
			CurrentStorageProviderID = storageLocation;

			//Set the default extension if not already specified.
			if (string.IsNullOrEmpty(extension)) 
                extension = type.DefaultDocExtension;
            SetExtraInfo("docextension", extension);

			//Get the identifier of the directory used to store the document.
			short dirid;
			Session.CurrentSession.GetSystemDirectory(SystemDirectories.OMDocuments, out dirid);
			SetExtraInfo("docdirid", dirid);

            var tr = new TimeRecord(this, true);

			//Add a default time recording entry.
			this.TimeRecords.Add(tr);
			
			//Add an SMS time recording entry if an SMS is going to be sent.
            Activities acc = tr.Activities;
            if (_sms != null && acc.Exists("SMS"))
			{
				TimeRecord smstr = new TimeRecord(this,true);
				smstr.ActivityCode = "SMS";
				smstr.TimeDescription = CodeLookup.GetLookup("TIMEACTCODE", "SMS", "SMS Message");
				this.TimeRecords.Add(smstr);
			}

			//Pass the current document as the parent to any sub documents.
			_subDocuments = subDocuments;
			foreach (SubDocument d in SubDocuments)
			{
				d.Parent = this;
			}

			//Set the default client notes to the precedent milestone notes.
			if (Session.CurrentSession.IsPackageInstalled("MILESTONES"))
			{				
				Precedent milestone_prec = (lastPrecedent == null ? basePrecedent: lastPrecedent);
				if (milestone_prec != null)
					AppendClientNotes = milestone_prec.MilestoneNote;
			}

		}

        private static string GenerateDocumentDescription(Associate assoc, string description, Precedent basePrecedent, Precedent lastPrecedent, DocumentDirection direction)
        {
            if (lastPrecedent != null)
                description = lastPrecedent.Description;
            else if (basePrecedent != null)
            {
                description = basePrecedent.Description;
            }

            if (string.IsNullOrEmpty(description))
                description = FWBS.OMS.CodeLookup.GetLookup("DOCTYPE", "BLANK");

            if (direction == DocumentDirection.In)
                description += " " + FWBS.OMS.Session.CurrentSession.Resources.GetResource("DIRECTIONIN", "From", "").Text;
            else
                description += " " + FWBS.OMS.Session.CurrentSession.Resources.GetResource("DIRECTIONOUT", "To", "").Text;

            if (assoc != null)
                description += " " + assoc.Contact.Name;
            return description;
        }

		public OMSDocument (Associate assoc, string description, Precedent basePrecedent, Precedent lastPrecedent, int editTime, DocumentDirection direction) : this(assoc, description, basePrecedent, lastPrecedent, editTime, direction, "", -1, null)
		{

        }

		public OMSDocument (Associate assoc, string description, Precedent basePrecedent, Precedent lastPrecedent, int editTime, DocumentDirection direction, string extension, short storageLocation) : this(assoc, description, basePrecedent, lastPrecedent, editTime, direction, extension, storageLocation, null)
		{

        }

		/// <summary>
		/// Constructs an existing document.
		/// </summary>
		/// <param name="id">The precedent id to retrieve.</param>
        /// <param name="duplicateCheck"></param>
        /// <param name="external"></param>
        /// <param name="merge"></param>
		private void Fetch (string id, bool duplicateCheck, bool external, DataRow merge)
		{
            DataTable data = null;

            latest = null;

			IDataParameter[] paramlist = new IDataParameter[3];
			
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("DUPLICATECHECK", System.Data.SqlDbType.Bit,0,duplicateCheck);

            bool externalidcheck = (Session.CurrentSession.SupportsExternalDocumentIds && external);
            
            if (externalidcheck)
            {
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("DOCID", System.Data.SqlDbType.NVarChar, 50, id);
                data = Session.CurrentSession.Connection.ExecuteProcedureTable("sprDocumentRecordExternal", Table, paramlist);
            }
            else
            {
                long val;
                if (long.TryParse(id, out val))
                {
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("DOCID", System.Data.SqlDbType.BigInt, 0, id);
                    data = Session.CurrentSession.Connection.ExecuteProcedureTable("sprDocumentRecord", Table, paramlist);
                }
                else
                    throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("ERRNOEXTDOCIDSP", "Searching by external document numbers is not supported.", "").Text);
            }



            if ((data == null) || (data.Rows.Count == 0)) 
				throw new OMSException2("DOCNOTFOUND", "Document '%1%' does not appear to exist.", null, false, id.ToString());

            timestamp = DateTime.UtcNow;

			//If there is more than one document with the same identifier then raise it to the UI level.
			//This will only happen with conversions from OMS2k.
            if (data.Rows.Count > 1)
            {
                long newid = -1;


                string searchcode;

                if (externalidcheck)
                    searchcode = Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.DocumentDuplicatesExternal);
                else
                    searchcode = Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.DocumentDuplicates);

                PromptEventArgs e = new PromptEventArgs(PromptType.Search, searchcode, new object[1] { id }, Session.CurrentSession.Resources.GetMessage("DUPDOCID", "There is more than one document with the same document identifier of '%1%'", "", false, id.ToString()).Text);
                Session.CurrentSession.OnPrompt(this, e);
                newid = Common.ConvertDef.ToInt64(e.Result, -1);
                if (e.Result == null || newid == -1)
                    throw new OMSException2("DUPDOCCANCELLED", "A conflict of document references cannot be resolved for document id '%1%'.", null, false, id.ToString());

                foreach (DataRow row in data.Rows)
                {
                    if (Convert.ToInt64(row["docid"]) != newid)
                        row.Delete();
                }


                data.AcceptChanges();

                if (data.Rows.Count == 0)
                    throw new OMSException2("DOCNOTFOUND", "Document '%1%' does not appear to exist.", null, false, id.ToString());

                if (merge != null)
                    Global.Merge(data.Rows[0], merge);


            }

            _doc = data;

             //Only check passwords when the basic security type is being used.
            if (!Session.CurrentSession.AdvancedSecurityEnabled)
            {
                //Ask for the password on open.  Perhaps extend the document object so that the
                //password can be applied to different levels of security, like viewing, changing
                //opeing, deleting etc...
                OMSFile.ValidatePassword();
                if (IsPasswordValid() == false)
                {
                    System.ComponentModel.CancelEventArgs args = new System.ComponentModel.CancelEventArgs();
                    Session.CurrentSession.OnPasswordRequest(this, args);
                    if (args.Cancel)
                    {
                        Dispose();
                        throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
                    }
                }
            }

            //Refresh the security
            SecurityManager.CurrentManager.Refresh(this);
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <remarks></remarks>
        public ObjectState State
        {
            get
            {
                try
                {
                    switch (_doc.Rows[0].RowState)
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


        public DocType CurrentDocumentType
        {
            get
            {
                return (DocType)GetOMSType();
            }
        }







		/// <summary>
		/// Gets a value indicating whether the document object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
        private bool isnew = false;
		public bool IsNew
		{
			get
			{
				try
				{
					return (_doc.Rows[0].RowState == DataRowState.Added || isnew);
				}
				catch
				{
					return false;
				}
			}
		}


		/// <summary>
		/// This will append a note to the bottom of the Client Viewable Notes in the OMSFile
		/// </summary>
		public string AppendClientNotes
		{
			get
			{
				return _clientnotes;
			}
			set
			{
				_clientnotes = value;
			}
		}

		[EnquiryUsage(true)]
		public string AppendClientViewableNotepad
		{
			get
			{
				return _clientnotes;
			}
			set
			{
				_clientnotes = value;
			}
		}

		[EnquiryUsage(true)]
		public string AppendFileNotViewableNotepad
		{
			get
			{
				return _filenotes;
			}
			set
			{
				_filenotes = value;
			}
		}


		/// <summary>
		/// Gets the unique identifier of the document.
		/// </summary>
		public long ID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("docid"));
			}
		}

        /// <summary>
        /// Gets the document ID to be displayed.
        /// </summary>
        public string DisplayID
        {
            get
            {
                string displayID = "";

                if (Session.CurrentSession.SupportsExternalDocumentIds)
                    displayID = ExternalId;

                if (string.IsNullOrEmpty(displayID))
                    displayID = ID.ToString();

                return displayID;
            }
        }


		/// <summary>
		/// Gets the stored description of the document.
		/// </summary>
		[EnquiryUsage(true)]
		public string AlternateDescription
		{
			get
			{
				return Convert.ToString(GetExtraInfo("docStyledesc"));
			}
			set
			{
				SetExtraInfo("docStyledesc",value);
				
			}
		}


		/// <summary>
		/// Gets or Sets the stored description of the document.
		/// </summary>
		[EnquiryUsage(true)]
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("docdesc"));
			}
			set
			{
                string oldval = Description;
                if (value != oldval)
                {
				    SetExtraInfo("docdesc",value);
                    OnPropertyChanged(new PropertyChangedEventArgs("Description", oldval, value));
                }
			}
		}

		/// <summary>
		/// Gets an associate address that owns this storage item object.
		/// </summary>
		[EnquiryUsage(true)]
		public string Addressee
		{
			get
			{
				return this.Associate.Addressee;
			}
		}

		/// <summary>
		/// Gets an associate Contact Name that owns this storage item object.
		/// </summary>
		[EnquiryUsage(true)]
		public string ContactName
		{
			get
			{
				return this.Associate.Contact.Name;
			}
		}


		
		/// <summary>
		/// Gets an associate object that owns this storage item object.
		/// </summary>
		public Associate Associate
		{
			get
			{
				return Associate.GetAssociate(Convert.ToInt64(GetExtraInfo("associd")));
			}
		}

		/// <summary>
		/// Gets a branch object that the document was created at.
		/// </summary>
		public Branch Branch
		{
			get
			{
				return new Branch(Convert.ToInt32(GetExtraInfo("docbrid")));
			}
		}

        /// <summary>
        /// The User who Authored the Document.
        /// </summary>
        public User Author
        {
            get
            {
                object author = GetExtraInfo("docAuthoredBy");
                if (author == DBNull.Value || author == null)
                    return null;
                else
                    return User.GetUser((int)author);

            }
            set
            {
                if (value == null)
                    SetExtraInfo("docAuthoredBy", DBNull.Value);
                else
                    SetExtraInfo("docAuthoredBy", value.ID);
            }
        }

		/// <summary>
		/// Gets the incoming / outgoing direction of the storage item when the item was received and saved.
		/// </summary>
        [EnquiryUsage(true)]
		public DocumentDirection Direction
		{
			get
			{
				try
				{
					bool ret = Convert.ToBoolean(GetExtraInfo("docdirection"));
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
        public bool IsOutbound
        {
            get
            {
                return (Direction == DocumentDirection.Out);
            }
        }

        [EnquiryUsage(true)]
        public bool IsInbound
        {
            get
            {
                return (Direction == DocumentDirection.In);
            }
        }

        public long? PhaseId
        {
            get
            {
                object id = GetExtraInfo("phid");
                if (id == DBNull.Value || id == null)
                    return null;
                else
                    return Convert.ToInt64(id);
            }
        }

        private string phasedesc;
        [EnquiryUsage(true)]
        public string Phase
        {
            get
            {
                long? id = PhaseId;


                if (id.HasValue)
                {
                    if (phasedesc == null)
                    {
                        IDataParameter[] pars = new IDataParameter[1];
                        pars[0] = Session.CurrentSession.Connection.CreateParameter("id", id.Value);
                        phasedesc = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar("select phdesc from dbfilephase where phid = @id", pars));
                    }
                    return phasedesc;
                }
                else
                    return String.Empty;
            }
        }

		/// <summary>
		/// Gets the storage type of the item.
		/// </summary>
		[EnquiryUsage(true)]
		public string DocumentType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("doctype"));
			}
		}

        /// <summary>
        /// Gets the external docid.
        /// </summary>
        [EnquiryUsage(true)]
        public string ExternalId
        {
            get
            {
                if (_doc.Columns.Contains("docidext"))
                    return Convert.ToString(GetExtraInfo("docidext"));
                else
                    return String.Empty;
            }
            set
            {
                if (!ExternalId.Equals(value ?? String.Empty))
                {
                    if (_doc.Columns.Contains("docidext"))
                    {
                        if (string.IsNullOrEmpty(value))
                            SetExtraInfo("docidext", DBNull.Value);
                        else
                            SetExtraInfo("docidext", value);
                    }
                }
            }
        }

		/// <summary>
		/// Gets the precedent that the current document is based on.
		/// </summary>
		public Precedent BasePrecedent
		{
			get
			{
				if (GetExtraInfo("docbaseprecid") == DBNull.Value)
				{
					return null;
				}
				else
					return Precedent.GetPrecedent(Convert.ToInt64(GetExtraInfo("docbaseprecid")));

			}
		}

		/// <summary>
		/// Gets the last precedent used within the document.
		/// </summary>
		public Precedent LastPrecedent
		{
			get
			{
				if (GetExtraInfo("docprecid") == DBNull.Value)
				{
					return null;
				}
				else
					return Precedent.GetPrecedent(Convert.ToInt64(GetExtraInfo("docprecid")));
			
			}
		}

		/// <summary>
		/// Gets a boolean value specifying whether the item is archived.
		/// </summary>
		public bool IsArchived
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("docarchived"));
			}
		}


		[EnquiryUsage(true)]
		public int EditingTime
		{
			get
			{
				return _editingtime;
			}
			set
			{
				_editingtime = value;
			}
		}

		/// <summary>
		/// Gets or Sets the received date of the document.  This is different to the created by.
		/// Created by is the date that the document was first created in the database, whilst the
		/// Received date can be changed to the date the document was actually received.
		/// </summary>
		[EnquiryUsage(true)]
		public Common.DateTimeNULL AuthoredDate
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("docAuthored"), DBNull.Value);
			}
			set
			{
				
				SetExtraInfo("docAuthored", value.ToObject());
			}
		}

		/// <summary>
		/// Gets the modification dates and users.
		/// </summary>
		[EnquiryUsage(true)]
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
		/// Gets the client identifier associated to the document.
		/// </summary>
		internal long ClientID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("clid"));
			}
		}

		/// <summary>
		/// Gets the file identifier associated to the document.
		/// </summary>
		public long OMSFileID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("fileid"));
			}
		}

		/// <summary>
		/// Gets the associate identifier associated to the document.
		/// </summary>
		internal long AssocID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("associd"));
			}
		}

		/// <summary>
		/// Gets the branch identifier associated to the document.
		/// </summary>
		internal long BranchID
		{
			get
			{
				try
				{
					return Convert.ToInt32(GetExtraInfo("docbrid"));
				}
				catch
				{
					return -1;
				}
			}

		}

		/// <summary>
		/// Gets the live directory path.
		/// </summary>
		internal string LiveDirectory
		{
			get
			{
                if (GetExtraInfo("docdirid") == DBNull.Value)
                    return String.Empty;
                else
				    return Session.CurrentSession.GetDirectory((short)GetExtraInfo("docdirid")).FullName;
			}
		}

		/// <summary>
		/// Gets the archive directory.
		/// </summary>
		internal string ArchiveDirectory
		{
			get
			{
                if (GetExtraInfo("docdirid") == DBNull.Value)
                    return String.Empty;
                else
                    return Session.CurrentSession.GetDirectory((short)GetExtraInfo("docdirid")).FullName;
			}
		}

		/// <summary>
		/// Gets or Sets the live directory id.
		/// </summary>
		internal short DirectoryID
		{
			get
			{
				try
				{
					return Convert.ToInt16(GetExtraInfo("docdirid"));
				}
				catch
				{
					short dirid;
					Session.CurrentSession.GetSystemDirectory(SystemDirectories.OMDocuments, out dirid);
					return dirid;
				}
			}
			set
			{
				SetExtraInfo("docdirid", value);
			}
		}

		/// <summary>
		/// Gets or Sets the archive directory id.
		/// </summary>
		internal short ArchiveDirectoryID
		{
			get
			{
				try
				{
					return Convert.ToInt16(GetExtraInfo("docarchivedirid"));
				}
				catch
				{
					short dirid;
					Session.CurrentSession.GetSystemDirectory(SystemDirectories.OMArchive, out dirid);
					return dirid;
				}
			}
			set
			{
				SetExtraInfo("docarchivedirid", value);
			}
		}

		/// <summary>
		/// Gets the Document Program Type.
		/// </summary>
		public Apps.RegisteredApplication DocProgType
		{
			get
			{
				return Apps.ApplicationManager.CurrentManager.GetRegisteredApplication(Convert.ToInt16(GetExtraInfo("docappid")));
			}
			set
			{
				SetExtraInfo("docappid",value.ID);
			}
		}

		#region Reminder Properties

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
		public bool HasReminder
		{
			get
			{
				return _reminder;
			}
			set
			{
				_reminder = value;
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
        public string ReminderSubject
		{
			get
			{
				return _remindersubject;
			}
			set
			{
				_remindersubject = value;
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
        public int ReminderFeeEarner
		{
			get
			{
				return _reminderfeeearner;
			}
			set
			{
				_reminderfeeearner = value;
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
        public string ReminderNote
		{
			get
			{
				return _remindernote;
			}
			set
			{
				_remindernote = value;
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
        public int ReminderDays
		{
			get
			{
				return _reminderdays;
			}
			set
			{
                int oldval = ReminderDays;

                if (oldval != value)
                {
                    //UTCFIX: DM - 30/11/06 - Make Reminder date must be local.
                    _reminderdays = value;

                    Common.DateTimeNULL olddue = ReminderDue;
                    _reminderdue = DateTime.Today.AddDays(_reminderdays);

                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDue", olddue, this.ReminderDue));
                }
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("Tasks")]
        public Common.DateTimeNULL ReminderDue
		{
			get
			{
				return _reminderdue;
			}
			set
			{
                Common.DateTimeNULL olddue = ReminderDue;
                int olddays = ReminderDays;

                //UTCFIX: DM - 30/11/06 - DateTimeNull should handle the date comparisons.
                if (!olddue.Equals(value))
                {
                    _reminderdue = value;
                    if (_reminderdue.IsNull)
                    {
                        _reminderdue = DateTime.Today;
                    }

                    //UTCFIX: DM - 30/11/06 - Make the following dates local kinds for the arithmatic.
                    DateTime today = DateTime.Today;
                    DateTime due = _reminderdue.ToLocalTime();
                    TimeSpan diff = due.Subtract(today);
                    _reminderdays = Convert.ToInt32(diff.TotalDays);
                                        
                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDays", olddays, this.ReminderDays));
                    OnPropertyChanged(new PropertyChangedEventArgs("ReminderDue", olddue, this.ReminderDue));
                }
			}
		}

		#endregion

		#region SMS Properties

		/// <summary>
		/// Gets the SMS message that is attached to the document for the document save.
		/// </summary>
		[EnquiryUsage(false)]
		[LocCategory("SMS")]
		[Obsolete("This property has been deprecated in V10.1")]
		public SMS SMS
		{
			get
			{
				if (_sms == null)
				{
                    _sms = new SMS(Associate);
                    _sms.Number = Associate.DefaultMobile;
					_sms.Cancel = true;
				}
				return _sms;
			}
		}

        [EnquiryUsage(true)]
        [LocCategory("SMS")]
        [Obsolete("This property has been deprecated in V10.1")]
        internal bool SMSSend
        {
            get
            {
                return (!(SMS.Cancel));
            }
            set
            {
                SMS.Cancel = !value;
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("SMS")]
        [Obsolete("This property has been deprecated in V10.1")]
        internal string SMSText
        {
            get
            {
                return SMS.Text;
            }
            set
            {
                string oldval = SMS.Text;
                if (oldval != value)
                {
                    SMS.Text = value;
                    SMS.Parse();
                    OnPropertyChanged(new PropertyChangedEventArgs("SMSText", oldval, SMS.Text));
                    if (String.IsNullOrEmpty(SMS.Text) == false && this.SMSSend == false)
                    {
                        OnPropertyChanged(new PropertyChangedEventArgs("SMSSend", false, true));
                        this.SMSSend = true;
                    }
                }
            }
        }

		[EnquiryUsage(true)]
		[LocCategory("SMS")]
		[Obsolete("This property has been deprecated in V10.1")]
		internal string SMSNumber
		{
			get
			{
				return SMS.Number;
			}
			set
			{
                string oldval = SMS.Number;
                if (value != oldval)
                {
                    SMS.Number = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("SMSNumber", oldval, SMS.Number));
                }
			}
		}

		#endregion

		[EnquiryUsage(true)]
		internal Common.DateTimeNULL NewFileReviewDate
		{
			get
			{
				return _newfilereviewdate;
			}
			set
			{
				_newfilereviewdate = value;
			}
		}

		/// <summary>
		/// Gets a File object that is associated to the Document.
		/// </summary>
		public OMSFile OMSFile
		{
			get
			{
				return OMSFile.GetFile((long)GetExtraInfo("fileid"));
			}
		}

		/// <summary>
		/// Gets the document Preview Text.
		/// </summary>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
		public string Preview
		{
			get
			{

                if (_doc.Columns.Contains("docPreview"))
                {
                    string prev = Convert.ToString(GetExtraInfo("docPreview"));
                    if (String.IsNullOrEmpty(prev))
                    {
                        if (latest != null)
                            this.Preview = latest.Preview;
                    }

                    return Convert.ToString(GetExtraInfo("docPreview"));
                }
                else
                {
                    if (_preview == null)
                    {
                        // DMB: only go to the database if document previews are enabled
                        if (IsNew == false && Session.CurrentSession.DocumentPreviewEnabled)
                        {
                            IDataParameter[] pars = new IDataParameter[1];
                            pars[0] = Session.CurrentSession.Connection.CreateParameter("docid", ID);
                            _preview = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar("select docpreview from dbdocumentpreview where docid = @docid", pars));
                        }
                    }

                    if (String.IsNullOrEmpty(_preview))
                    {
                        if (latest != null)
                            this.Preview = latest.Preview;
                    }

                    return _preview;
                }


			}
			set
			{
				if (Session.CurrentSession.DocumentPreviewEnabled)
				{
					if (_doc.Columns.Contains("docPreview"))
					{
						if (value == null || value == "")
							SetExtraInfo("docPreview", DBNull.Value);
						else			
							SetExtraInfo("docPreview", value);
					}
					else
					{
						if (value == null)
							value = "";

                        if (_preview != value)
                        {
                            _preview = value;
                            isdirty = true;
                        }
					}
				}
			}
		}


        /// <summary>
        /// Gets the quick multi-line description display of client details.  
        /// This is built up by the client type configuration file.
        /// </summary>
        [EnquiryUsage(true)]
        public string DocumentDetail
        {
            get
            {
                try
                {
                    string text = CurrentDocumentType.DocumentDetailsConfig;
                    if (text == String.Empty) text = FWBS.OMS.Global.GetResString("DefaultDocumentDetails", true);
                    return CurrentDocumentType.ParseDynamicProperty(this, text);

                }
                catch
                {
                    return "N/A";
                }

            }
        }

		/// <summary>
		/// Gets or Sets the document waller property.
		/// </summary>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
        [Obsolete("Wallets have been depricated. Use the FolderGUID property to save documents to folders.", false)]
		public string Wallet
		{
			get
			{
                if (Convert.ToString(GetExtraInfo("docwallet")) != "")
                    return Convert.ToString(GetExtraInfo("docwallet"));
                else
                {
                    if (this.BasePrecedent != null && Convert.ToString(this.BasePrecedent.Wallet) != "")
                    {
                        SetExtraInfo("docwallet", Convert.ToString(this.BasePrecedent.Wallet));
                        return Convert.ToString(this.BasePrecedent.Wallet);
                    }
                    else
                    {
                        SetExtraInfo("docwallet", "GENERAL");
                        return "GENERAL";
                    }
                }
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("docwallet","GENERAL");
				else			
					SetExtraInfo("docwallet", value);
			}
		}

        [EnquiryUsage(true)]
        [System.ComponentModel.Browsable(false)]
        public Guid FolderGUID
        {
            get
            {
                string guid = Convert.ToString(GetExtraInfo("docFolderGUID"));
                if(string.IsNullOrWhiteSpace(guid))
                    return Guid.Empty;
                else
                    return (Guid)GetExtraInfo("docFolderGUID");
            }
            set
            {
                if (value != Guid.Empty)
                {
                    SetExtraInfo("docFolderGUID", value);
                }
                else
                {
                    SetExtraInfo("docFolderGUID", DBNull.Value);
                }
            }
        }


		/// <summary>
		/// Gets an array of sub documents that could potentially be saved.
		/// </summary>
		public SubDocument[] SubDocuments
		{
			get
			{
				if (_subDocuments == null)
					_subDocuments= new SubDocument[0];
				return _subDocuments;
			}
		}
		
		/// <summary>
		/// Gets an in memory only variable of the number of spelling mistakes the current document has.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public int SpellingMistakes
		{
			get
			{
				return _spellingErrors;
			}
			set
			{
				_spellingErrors =  value;
			}
		}


        [System.ComponentModel.Browsable(false)]
        public bool ReachDocMaxRevisionCount
        {
            get
            {
                return _DocMaxRevisionCount;
            }
            set
            {
                _DocMaxRevisionCount = value;
            }
        }

		/// <summary>
		/// Gets an in memory only variable of the number of grammer mistakes the current document has.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public int GrammarMistakes
		{
			get
			{
				return _grammarErrors;
			}
			set
			{
				_grammarErrors =  value;
			}
		}

		/// <summary>
		/// Gets an in memory only flag which determines whether a document stays open when saved.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
        [EnquiryUsage(true)]
		public bool ContinueAfterSave
		{
			get
			{
				return _continueAfterSave;
			}
			set
			{
				_continueAfterSave =  value;
			}
		}

        private bool allowContinueAfterSave = true;

        [System.ComponentModel.Browsable(false)]
        [EnquiryUsage(false)]
        public bool AllowContinueAfterSave
        {
            get
            {
                return allowContinueAfterSave;
            }
            set
            {
                allowContinueAfterSave = value;
            }
        }


		#endregion

		#region Time Recording
		/// <summary>
		/// Gets the Time Recording collection in object form.
		/// </summary>
		public TimeCollection TimeRecords
		{
			get
			{
				if (_timerecords == null)
				{
					DownloadTimeRecords();
					_timerecords = new TimeCollection(ref _time,this);
				}
				return _timerecords;
			}
		}

		/// <summary>
		/// Gets the Time Records table for the file when it is needed.
		/// </summary>
		private void DownloadTimeRecords()
		{
			if (_time == null)
			{
				_time = Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeRecords","TIME", true, new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID),Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name)});
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
			try
			{
                switch (fieldName)
                {
                    case "phid":
                        phasedesc = null;
                        break;
                }

				if (_doc.Columns.Contains(fieldName))
				{

                    if ((_doc.Columns[fieldName].DataType == typeof(string)) && (Convert.ToString(val).Length > _doc.Columns[fieldName].MaxLength))
					{
						if (_doc.Columns[fieldName].MaxLength != -1)
							val = Convert.ToString(val).Substring(0,_doc.Columns[fieldName].MaxLength);
					}

                    this.SetExtraInfo(_doc.Rows[0], fieldName, val);
                }
				else
					throw new Exception("FieldName : " + fieldName + " Doesn't Exist in Document Table!");

			}
			catch (Exception ex)
			{
				throw new Exception("Error Working with Fieldname : " + fieldName,ex);
			}
		}
		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{
            //UTCFIX: DM - 30/11/06 - return local time
			object val = _doc.Rows[0][fieldName];
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
				return _doc.Columns[fieldName].DataType;
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
			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}
		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public virtual DataTable GetDataTable()
		{
			return _doc.Copy();
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

			//Run the linked precedents saving method before saving the document.
			Precedent bp = BasePrecedent;
			if (bp != null)
			{
				DocumentSavingEventArgs e = new DocumentSavingEventArgs(this, false);
				bp.OnDocumentSaving(e);
				if (e.Cancel) return;
			}
			Precedent lp = LastPrecedent;
			if (lp != null)
			{
				DocumentSavingEventArgs e = new DocumentSavingEventArgs(this, false);
				lp.OnDocumentSaving(e);
				if (e.Cancel) return;
			}

            InternalUpdate();

			//Raise the Document Saved event on the linked precedents.
			if (bp != null)
			{
				DocumentSavedEventArgs e = new DocumentSavedEventArgs(this);
				bp.OnDocumentSaved(e);
			}
			if (lp != null)
			{
				DocumentSavedEventArgs e = new DocumentSavedEventArgs(this);
				lp.OnDocumentSaved(e);
			}

            this.OnExtCreatedUpdatedDeleted(state);
		}


        public void PhysicalDocumentSaved()
        {
            CheckPermissions();

            //Run the linked precedents saving method before saving the document.
            Precedent bp = BasePrecedent;
            if (bp != null)
            {
                PhysicalDocumentSavedEventArgs e = new PhysicalDocumentSavedEventArgs(this);
                bp.OnPhysicalDocumentSaved(e);
                if (e.Cancel) return;
            }
            Precedent lp = LastPrecedent;
            if (lp != null)
            {
                PhysicalDocumentSavedEventArgs e = new PhysicalDocumentSavedEventArgs(this);
                lp.OnPhysicalDocumentSaved(e);
                if (e.Cancel) return;
            }
        }

        internal void InternalUpdate()
        {

            CheckPermissions();

            
            //Validates each of the sub documents.
            foreach (SubDocument d in SubDocuments)
            {
                d.Validate();
            }


            DataRow row = _doc.Rows[0];

            isnew = IsNew;
            if (isnew)
                isdirty = true;

            //Update the document version headers.
            if (isnew == false)
            {
                foreach (DocumentVersion ver in versions.Values)
                {
                    ver.InternalUpdate();
                }
            }


            //Check if there are any changes made before setting the updated
            //and updated by properties then update.
            if (IsDirty)
            {
                // DMB Move here to only dup check if dirty
                // Dont allow a duplicate document to be saved unless allowed to.
                if (Exists())
                {
                    if ((Session.CurrentSession.DuplicateDocumentAction == "NODUPLICATION" || _allowduplicatedoc == false) && DuplicatedFileRef != null)
                        throw new StorageItemDuplicatedException(this, Session.CurrentSession.Resources.GetMessage("DUPLICATEDOC", "Duplicated within %FILE% '%1%'", "", DuplicatedFileRef.ToString()).Text);
                }

                //Set the phase identifier from the owning file if in the table and is new.
                try
                {
                    if (isnew && _doc.Columns.Contains("phID"))
                        SetExtraInfo("phID", OMSFile.GetExtraInfo("phID"));
                }
                catch { }



                SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
                SetExtraInfo("Updated", DateTime.Now);

                //NOTE: Backward compatability for BLOB storage versioning
                byte [] data = null;
                if (!isnew)
                {
                      if (CurrentStorageProviderID == 1)
                    {
                        string originaltoken = Convert.ToString(row["docfilename", DataRowVersion.Original]);
                        string currenttoken = Convert.ToString(row["docfilename", DataRowVersion.Current]);
                        if (currenttoken != originaltoken)
                        {
                            IDataParameter[] pars = new IDataParameter[1];
                            pars[0] = Session.CurrentSession.Connection.CreateParameter("token", currenttoken);
                            data = (byte[])Session.CurrentSession.Connection.ExecuteProcedureScalar("sprFetchStorageItem", pars);
                        }

                    }
                }

                if (isnew && _doc.Rows[0].RowState == DataRowState.Modified)
                {
                    _doc.Rows[0].AcceptChanges();
                    _doc.Rows[0].SetAdded();
                }

                UpdateDataRow();

                //NOTE: Backward compatability for BLOB storage versioning
                if (data != null)
                {
                    IDataParameter[] pars = new IDataParameter[2];
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("docid", ID);
                    pars[1] = Session.CurrentSession.Connection.CreateParameter("document", SqlDbType.Image, 0, data);
                    Session.CurrentSession.Connection.ExecuteProcedure("sprSaveDocumentBLOB", pars);
                }

            }

            if (email != null && email.IsDirty)
            {
                email.SetExtraInfo("DOCID", this.ID);
                email.Update();
            }

            if (isnew)
            {
                Session.CurrentSession.CurrentDocuments.Add(ID.ToString(), this);
                Security.SecurityManager.CurrentManager.ApplyDefaultSettings(this.OMSFile, this);
            }


            if (isdirty)
            {
                if (_preview != null && Session.CurrentSession.DocumentPreviewEnabled)
                {
                    IDataParameter[] pars = new IDataParameter[2];
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("docid", ID);
                    pars[1] = Session.CurrentSession.Connection.CreateParameter("text", _preview);
                    Session.CurrentSession.Connection.ExecuteProcedure("sprUpdateDocumentPreview", pars);
                }
            }


            if ((Session.CurrentSession.IsPackageInstalled("TIMERECORDING")) && (Session.CurrentSession.IsLicensedFor("TIMEREC")))
            {
                //Update the time records for the document.
                if (_timerecords != null && !_timerecords.SkipTime)
                {
                    for (int i = 0; i < _timerecords.Count; i++)
                    {
                        TimeRecord tim = _timerecords[i];
                        tim.DocumentID = Convert.ToInt64(GetExtraInfo("docid"));
                        _timerecords[i] = tim;
                    }
                    _timerecords.Update();
                    _timerecords.Clear();
                }
            }


            if (!String.IsNullOrWhiteSpace(this.AppendClientNotes) || !String.IsNullOrWhiteSpace(this.AppendFileNotViewableNotepad))
            {
                if (!String.IsNullOrWhiteSpace(this.AppendClientNotes))
                    this.OMSFile.AppendExternalNoteText(NoteAppendingLocation.End, this.AppendClientNotes);

                if (!String.IsNullOrWhiteSpace(this.AppendFileNotViewableNotepad))
                    this.OMSFile.AppendNoteText(NoteAppendingLocation.End, this.AppendFileNotViewableNotepad);

                AppendClientNotes = "";
                AppendFileNotViewableNotepad = "";
                this.OMSFile.Update();
            }

            if (Session.CurrentSession.IsPackageInstalled("MILESTONES"))
            {
                Precedent milestone_prec = (LastPrecedent == null ? BasePrecedent : LastPrecedent);

                if (milestone_prec.MilestoneChangeAutomatic && this.OMSFile.MilestonePlan != null)
                {

                    try
                    {
                        if (this.OMSFile.MilestonePlan.MSPlan != "" && Convert.ToBoolean(this.OMSFile.MilestonePlan.GetType().GetProperty("MSStage" + milestone_prec.MilestoneStage.ToString() + "Checked").GetValue(this.OMSFile.MilestonePlan, null)) == false)
                        {
                            if (milestone_prec.MilestoneConfirmMoveStage)
                            {
                                //1925795742 
                                FWBS.OMS.AskEventArgs ask;
                                if (milestone_prec.MilestoneChangePrompt != "")
                                    ask = new AskEventArgs(milestone_prec.MilestoneChangePrompt, "PRECMS", FWBS.OMS.AskResult.No, milestone_prec.MilestonePlan.ToString(), milestone_prec.MilestoneStage.ToString());
                                else
                                    ask = new AskEventArgs("1925795742", "PRECMS", FWBS.OMS.AskResult.No, milestone_prec.MilestonePlan.ToString(), milestone_prec.MilestoneStage.ToString());
                                Session.CurrentSession.OnAsk(this, ask);
                                if (ask.Result == AskResult.Yes)
                                {
                                    this.OMSFile.MilestonePlan.GetType().GetProperty("MSStage" + milestone_prec.MilestoneStage.ToString() + "Checked").SetValue(this.OMSFile.MilestonePlan, true, null);
                                    this.OMSFile.MilestonePlan.Update();
                                }
                            }
                            else
                            {
                                this.OMSFile.MilestonePlan.GetType().GetProperty("MSStage" + milestone_prec.MilestoneStage.ToString() + "Checked").SetValue(this.OMSFile.MilestonePlan, true, null);
                                this.OMSFile.MilestonePlan.Update();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Session.CurrentSession.OnWarning(this, new MessageEventArgs(ex));
                    }
                }

            }

            //If the document reminder has been ticked then add
            //a document task to the files task list.
            if (HasReminder)
            {
                Tasks t = OMSFile.Tasks;
                t.Add(FWBS.OMS.FeeEarner.GetFeeEarner(_reminderfeeearner), this, "DOCUMENT", _remindersubject, _reminderdue, _remindernote);
                t.Update();

                HasReminder = false;
            }

            //If the document is a new document and a there is a file review date on the 
            //parent file then adjust the file review date.
            if (isnew && OMSFile.HasReviewDate)
            {
                OMSFile file = OMSFile;
                Common.DateTimeNULL review = file.ReviewDate;

                //UTCFIX: DM - 30/11/06 - DateTimeNull should handle the date comparisons.
                if (NewFileReviewDate != DBNull.Value && NewFileReviewDate > DateTime.Today) // MNW Fix to stop later than date && NewFileReviewDate < review)
                {
                    file.CreateNextReviewDate(NewFileReviewDate, "");
                    file.Update();
                }
            }

            //Updates each of the sub documents.
            foreach (SubDocument d in SubDocuments)
            {
                d.Update();
            }


            //Send the associated SMS if onw is available.
            if (_sms != null)
            {
                try
                {
                    if (_sms.Cancel == false)
                        _sms.Send();
                    _sms.Cancel = true;
                }
                catch (Exception e)
                {
                    _sms.Cancel = true;
                    OMSFile.AddEvent("SMSDOCERROR", e.Message, "DOCID - " + ID.ToString());
                    OMSFile.Update();
                }
            }

            //Updates any changes to the Milestone Plan after a Document Save
            if (this.OMSFile.MilestonePlan != null && this.OMSFile.MilestonePlan.IsDirty)
            {
                //If there is permission to update the milestone plan then do it
                bool canUpdateFile = false;
                try
                {
                    new FilePermission(this.OMSFile, StandardPermissionType.Update).Check();
                    new SystemPermission(StandardPermissionType.UpdateFile).Check();
                    canUpdateFile = true;
                }
                catch
                {
                    //do nothing as the only 
                }
                if (canUpdateFile)
                    this.OMSFile.MilestonePlan.Update();
                else
                {
                    if (!milestonePermissionsErrorMessageDisplayed)
                    {
                        System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("WARNDOCPERMMSTN", "You do not have permission to update this %FILE% therefore any changes made to the Milestone plan will not be applied.", "").Text, Session.CurrentSession.Resources.GetMessage("WARNDOCMSTNMSG", "Milestone Warning", "").Text );
                        milestonePermissionsErrorMessageDisplayed = true;
                    }
                }
            }

            //Update all the extended data objects, if any.
            if (_extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in _extData)
                {
                    ext.UpdateExtendedData();
                }
            }

            isdirty = false;
            isnew = false;
        }

        private void UpdateDataRow()
        {
            var row = _doc.Rows[0];

            //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
            if (_doc.PrimaryKey == null || _doc.PrimaryKey.Length == 0)
                _doc.PrimaryKey = new DataColumn[1] { _doc.Columns["docid"] };


            //DMB improve update performance
            Session.CurrentSession.Connection.Update(row, "config.dbDocument");
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

            this.OnExtRefreshing();

			DataTable changes = _doc.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID.ToString(), false, false, changes.Rows[0]);
            else
                Fetch(this.ID.ToString(), false, false, null);

            GetVersionsTable(true);


            //Refresh all the extended data sources, if any.
            if (_extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in _extData)
                    ext.RefreshExtendedData(applyChanges);
            }

            this.OnExtRefreshed();
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
		    RejectChanges();

		    if (IsNew)
            {
                Clear();
            }
		}

        private void RejectChanges()
        {
            _doc.RejectChanges();

            if (_extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in _extData)
                {
                    ext.CancelExtendedData();
                }
            }

            lastCheckoutCheck = DateTime.Today;
        }

        public void CancelOnFailedSave()
        {            
            RejectChanges();

            // assumption that if there is no stored file name it means this is an absolutely new record
            if (string.IsNullOrEmpty(_doc.Rows[0]["docFileName"].ToString()))
            {
                Clear();
                isnew = true;
                isdirty = true;
            }
        }

        /// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
        private bool isdirty = false;
		public bool IsDirty
		{
			get
			{
				return (isdirty || _doc.GetChanges() != null);
			}
		}

        /// <summary>
        /// Override so that the entity can be hidden from external viewing in MatterSphere
        /// Normal security will still be adhered to this is only an override to hide the entity
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsExternallyVisible
        {
            get
            {
                return SecurityOptions.HasFlag(SecurityOptions.IsExternallyVisible);
            }
            set
            {
                if (value)
                    SecurityOptions |= SecurityOptions.IsExternallyVisible;
                else
                    SecurityOptions &= ~SecurityOptions.IsExternallyVisible;
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
		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
                if (_extData != null)
                {
                    _extData.Dispose();
                    _extData = null;
                }

				if (_doc != null)
				{
					_doc.Dispose();
					_doc = null;
				}

				if (_time != null)
				{
					_time.Dispose();
					_time = null;
				}

				if (_timerecords != null)
				{
					_timerecords = null;
				}

                if (activities != null)
                {
                    activities.Dispose();
                    activities = null;
                }

                if (versionstable != null)
                {
                    versionstable.Dispose();
                    versionstable = null;
                }

				
			}

			//Free unmanaged objects here.
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
		protected void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current client object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.SaveDocumentWizard), param);
		}

		/// <summary>
		/// Edits the current client object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return  Enquiry.GetEnquiry(customForm, Parent, this, param);
		}

		
		#endregion
		
		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		public object Parent
		{
			get
			{
				return OMSFile;
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Fetches a storage item based on the unique identifier given.
		/// </summary>
		/// <param name="id">Unique identifier of the item within the data store.</param>
		/// <returns>A store item.</returns>
		public static OMSDocument GetDocument(long id)
		{
            Session.CurrentSession.CheckLoggedIn();
            OMSDocument doc = Session.CurrentSession.CurrentDocuments[id.ToString()] as OMSDocument;
            if (doc == null)
            {
                doc = new OMSDocument(id, false);
            }
            return doc;
		}

        /// <summary>
        /// Discards previously cached data and fetches a storage item based on the unique identifier given.
        /// </summary>
        /// <param name="id">Unique identifier of the item within the data store.</param>
        /// <returns>A store item.</returns>
        public static OMSDocument GetUncachedDocument(long id)
        {
            Session.CurrentSession.CurrentDocuments.Remove(id.ToString());
            return GetDocument(id);
        }

        public static OMSDocument GetDocument(string extid)
        {
            return GetDocument(extid, false);
        }

		public static OMSDocument GetDocument(long id, bool duplicateCheck)
		{
			Session.CurrentSession.CheckLoggedIn();
            if (!duplicateCheck)
                return GetDocument(id);
            else
			    return new OMSDocument(id, true);
		}

        public static OMSDocument GetDocument(string extid, bool duplicateCheck)
        {
            Session.CurrentSession.CheckLoggedIn();
            return new OMSDocument(extid, duplicateCheck);
        }

		/// <summary>
		/// Deletes an oms document from the database.
		/// </summary>
		/// <param name="doc">The document to delete.</param>
		public static void Delete(OMSDocument doc)
		{
			doc.Delete();
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
				return Convert.ToString(GetExtraInfo("docpassword"));
			}
			set
			{
				SetExtraInfo("docpassword", value);
			}
		}

		/// <summary>
		/// Gets the password Hint of the storage item.
		/// </summary>
		public override string PasswordHint
		{
			get
			{
				return Convert.ToString(GetExtraInfo("docpasswordhint"));
			}
			set
			{
				SetExtraInfo("docpasswordhint", value);
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

		/// <summary>
		/// The string representation of the document object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Description;
		}

		#endregion

        #region IExtendedDataCompatible Implementation
        /// <summary>
        /// Gets the extended data list indexer which will expose
        /// each of the extended data objects on the particular object.
        /// </summary>
        public  ExtendedDataList ExtendedData
        {
            get
            {
                if (_extData == null)
                {
                    //Use the contact type configuration to initialise the extended data objects.
                    DocType t = CurrentDocumentType;
                    string[] codes = new string[t.ExtData.Count];
                    int ctr = 0;
                    foreach (OMSType.ExtendedData ext in t.ExtData)
                    {
                        codes.SetValue(ext.Code, ctr);
                        ctr++;
                    }
                    if (codes.Length > 0)
                        _extData = new ExtendedDataList(this, codes);
                    else
                        _extData = new ExtendedDataList();
                }
                return _extData;
            }
        }

        #endregion


		#region Private
		private int DocumentTaskCount
		{
			get
			{
				DataTable count = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT COUNT(*) FROM dbTasks WHERE docID = @docid" ,"TASKCOUNT", false, new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID),Session.CurrentSession.Connection.AddParameter("@docid", System.Data.SqlDbType.BigInt, 10, this.ID)});
				return Convert.ToInt32(count.Rows[0][0]);
			}
		}
		
		private int DocumentTimeCount
		{
			get
			{
				DataTable count = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT COUNT(*) FROM DBTIMELEDGER WHERE docid = @docid" ,"TIMECOUNT", false, new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID),Session.CurrentSession.Connection.AddParameter("@docid", System.Data.SqlDbType.BigInt, 10, this.ID)});
				return Convert.ToInt32(count.Rows[0][0]);
			}
		}

		private void UnlinkDocumentTime()
		{
			Session.CurrentSession.Connection.ExecuteSQLScalar("UPDATE DBTIMELEDGER SET docid = null WHERE docid = @docid" , new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID),Session.CurrentSession.Connection.AddParameter("@docid", System.Data.SqlDbType.BigInt, 10, this.ID)});
		}

		private void UnlinkDocumentTask()
		{
			Session.CurrentSession.Connection.ExecuteSQLScalar("UPDATE DBTASKS SET docid = null WHERE docid = @docid" , new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID),Session.CurrentSession.Connection.AddParameter("@docid", System.Data.SqlDbType.BigInt, 10, this.ID)});
		}




		#endregion

        #region Methods

        public OMSDocument Clone()
        {
            OMSDocument clone = new OMSDocument();

            foreach (DataColumn col in this._doc.Columns)
            {
                if (clone._doc.Columns.Contains(col.ColumnName) 
                    && clone._doc.Columns[col.ColumnName].ReadOnly == false)
                    clone._doc.Rows[0][col.ColumnName] = this._doc.Rows[0][col];
            }

            clone.ExternalId = null;
            clone.SetExtraInfo("docfilename", "");

            clone.Init();

            EmailDocument ed = new EmailDocument();
            if (ed.Exists(this.ID))
            {
                email = new EmailDocument(this);
                clone.email = (EmailDocument)email.Clone();
            }
            return clone;
        }

        /// <summary>
		/// Changes the associate source of the document.
		/// </summary>
		/// <param name="assoc">The associate object to assign.</param>
		public void ChangeAssociate(Associate assoc)
		{
			if (assoc != null)
			{
				SetExtraInfo("clid", assoc.OMSFile.ClientID);
				SetExtraInfo("fileid", assoc.OMSFileID);
				SetExtraInfo("associd", assoc.ID);
			}
		}

		public void Restore()
		{

			if (_doc.Columns.Contains("docDeleted"))
			{
				try
				{
					if (GetExtraInfo("docDeleted") == System.DBNull.Value)
					{
						SetExtraInfo("docDeleted",false);
					}
				}
				catch
				{
					SetExtraInfo("docDeleted",false);
				}

				if (Convert.ToBoolean(GetExtraInfo("docdeleted")) == true)
				{
					SetExtraInfo("docDeleted", false);
					SetExtraInfo("docRetain", DBNull.Value);

                    if (!IsNew)
                        UpdateDataRow();
				}
			}
		}

        private void Clear()
        {
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);

            DeleteDocument(pars);
        }

        private void DeleteDocument(IDataParameter[] pars)
        {
            Session.CurrentSession.Connection.ExecuteProcedure("DeleteDocWithEmptyPathById", pars);
        }

        public void  Delete()
		{
			Delete(false, System.DateTime.Now.AddDays(Session.CurrentSession.DeletionRetentionPeriod), false);
		}

		/// <summary>
		/// Deletes current instance of the object.
		/// </summary>
		public void Delete(bool deleteFile, DateTime retainUntil, bool silent)
		{
            new DocumentPermission(this, StandardPermissionType.Delete).Check();
            new SystemPermission(StandardPermissionType.DeleteDocument).Check();
            
            int tasks = this.DocumentTaskCount;
			int times = this.DocumentTimeCount;
			if (tasks > 0 && times > 0)
			{
				AskEventArgs askargs = new AskEventArgs("DOCHASTASKTIME","You are about to delete a document from the system, this will unlink the (%1%) tasks and (%2%) time entries from this document, and will permanently delete this document are you sure ?","",AskResult.Yes,Convert.ToString(tasks),Convert.ToString(times));
				if (silent == false) Session.CurrentSession.OnAsk(this,askargs);
				if (askargs.Result == FWBS.OMS.AskResult.Yes)
				{
					UnlinkDocumentTask();
					UnlinkDocumentTime();
				}
				else
					return;
			}
			else if (tasks > 0)
			{
				AskEventArgs askargs = new AskEventArgs("DOCHASTASK","You are about to delete a document from the system, this will unlink the (%1%) tasks from this document and will permanently delete this document are you sure ?","",AskResult.Yes,Convert.ToString(tasks));
				if (silent == false) Session.CurrentSession.OnAsk(this,askargs);
				if (askargs.Result == FWBS.OMS.AskResult.Yes)
					UnlinkDocumentTask();
				else
					return;
			}
			else if (times > 0)
			{
				AskEventArgs askargs = new AskEventArgs("DOCHASTIME","You are about to delete a document from the system, this will unlink the (%1%) time recording from this document and will permanently delete this document are you sure ?","",AskResult.Yes,Convert.ToString(times));
				if (silent == false) Session.CurrentSession.OnAsk(this,askargs);
				if (askargs.Result == FWBS.OMS.AskResult.Yes)
					UnlinkDocumentTime();
				else
					return;
			}

			try
			{
                if (deleteFile)
                {
                    DocumentManagement.Storage.StorageProvider provider = ThisStorageItem.GetStorageProvider();
                    provider.Purge(this, false);
                }
			}
			catch{}

			OMSFile.AddEvent("DOCDELETED", String.Format("Document Deleted - ID: {0}", ID.ToString()), String.Format("ID: {0} - Desc: {1}", ID.ToString(), Description));

			//Backward compatibility.
			if (_doc.Columns.Contains("docDeleted"))
			{
				SetExtraInfo("docDeleted", true);
				SetExtraInfo("docRetain", retainUntil);
			}
			else
            {
				_doc.Rows[0].Delete();
            }
            UpdateDataRow();
            try
            {
                OMSFile.Update();
            }
            catch {}
        }


        private bool SpellingAndGrammarCheckRequired()
        {
            if (Session.CurrentSession.CurrentUser.SpellingAndGrammarCheckRequired == Common.TriState.True)
            {
                return true;
            }

            if (Session.CurrentSession.CurrentUser.SpellingAndGrammarCheckRequired == Common.TriState.False)
            {
                return false;
            }

            if (Session.CurrentSession.SpellingAndGrammarCheckRequired == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

		#endregion

		#region Retention Logic

		public RetentionPolicy GetRetentionPolicy()
		{
			string policy = OMSFile.CurrentFileType.DocumentRetentionPolicy;
			int period = OMSFile.CurrentFileType.DocumentRetentionPeriod;

			if (policy == String.Empty || period < 0)
			{
				policy = Session.CurrentSession.DocumentRetentionPolicy;
				period = Session.CurrentSession.DocumentRetentionPeriod;

				if (period < 0 || policy == String.Empty)
				{
					policy = String.Empty;
					period = -1;
				}
			}
			
			return new RetentionPolicy(policy, period);
		}


		#endregion

		#region IAlert

        public void AddAlert(Alert alert)
        {
            string key = alert.Message.ToUpperInvariant();
            if (_alerts.ContainsKey(key))
                _alerts.Remove(key);

            _alerts.Add(key, alert);
        }

        public void ClearAlerts()
        {
            foreach (Alert alert in _alerts.Values)
            {
                alert.ChangeStatus(Alert.AlertStatus.Off);
            }
        }

        



		/// <summary>
		/// Gets a list of alerts for the object.
		/// </summary>
		public Alert[] Alerts
		{
			get
			{
				List<Alert> arr = new List<Alert>();

				if (SMS.Cancel == false && OMS.Session.CurrentSession.IsLicensedFor("SMS") == false)
				{
					arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("SMSLICALERT", "The attached %PRECEDENT% is configured to send a SMS.  However you are not licensed to send it", "",true).Text,  FWBS.OMS.Alert.AlertStatus.Green));
				}

                if (!SpellingAndGrammarCheckRequired())
                {
                    arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("SPELLCHKOFF", "Spelling and grammar checks are not enabled.\nThis can be changed in the User Settings if required.", "", false, "").Text, FWBS.OMS.Alert.AlertStatus.Amber));
                }

				if (SpellingMistakes > 0)
				{
					arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("SPELLINGERRORS", "'%1%' spelling mistake(s) detected", "",false, SpellingMistakes.ToString()).Text, FWBS.OMS.Alert.AlertStatus.Amber));
				}

				if (GrammarMistakes > 0)
				{
					arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("GRAMMERERRORS", "'%1%' grammar mistake(s) detected", "",false, GrammarMistakes.ToString()).Text, FWBS.OMS.Alert.AlertStatus.Amber));
				}

                if (ReachDocMaxRevisionCount == true)
                {
                    arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("DOCVERCHKEXED", "This document has reached the maximum revision version.", "", false, "").Text, FWBS.OMS.Alert.AlertStatus.Amber)); 
                }

				if (Exists())
				{
					arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("DOCCHECKSUMDUP", "This document may already exist under %FILE% '%1%'.", "", true, DuplicatedFileRef.ToString()).Text, FWBS.OMS.Alert.AlertStatus.Red));
				}
               

                if (workingVersion != null)
                {
                    if (IsNew == false)
                    {
                        if (workingVersion.IsLatestVersion == false)
                        {
                            arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("DOCVERNOTLATEST", "The current working document version '%1%' is not the latest version.", "", true, workingVersion.Label).Text, FWBS.OMS.Alert.AlertStatus.Amber));
                        }
                    }
                }

                IStorageItemLockable lockable = this.GetStorageProvider().GetLockableItem(this);
                if (lockable != null)
                {
                    User checkedOutBy = lockable.CheckedOutBy;
                    if (checkedOutBy != null)
                    {
                        if (checkedOutBy.ID == Session.CurrentSession.CurrentUser.ID)
                            arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("DOCCHECKOUTBYME", "The document is checked out by you at '%1%'.", "", true, lockable.CheckedOutTime.Value.ToString()).Text, FWBS.OMS.Alert.AlertStatus.Green));
                        else
                        {
                            arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("DOCCHECKEDOUT", "The document is checked out by '%1%' at '%2%'.", "", true, checkedOutBy.FullName, lockable.CheckedOutTime.Value.ToString()).Text, FWBS.OMS.Alert.AlertStatus.Red));
                        }
                    }
                }

                if (IsConflict(false))
                {
                    arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("WARNDOCCONFLICT", "The document has already been changed by another user.", "").Text, FWBS.OMS.Alert.AlertStatus.Red));
                }

                arr.AddRange(_alerts.Values);
                arr.Sort();
                return arr.ToArray();
			}
		}

		#endregion

        #region IStorageItemLockable

        [System.ComponentModel.Browsable(false)]
        public string CheckedOutMachine 
        {
            get
            {
                if (ThisLockableStorageItem.IsCheckedOut)
                {
                    string name = Convert.ToString(GetExtraInfo("docCheckedOutLocation"));
                    string[] pars = name.Split(new string[] { "]:[" }, StringSplitOptions.None);
                    if (pars.Length > 0)
                    {
                        return pars[0].Trim('[', ']');
                    }
                    else
                        return "?";
                }
                else
                    return String.Empty;
            }
        }

        [System.ComponentModel.Browsable(false)]
        public string CheckedOutLocation 
        {
            get
            {
                if (ThisLockableStorageItem.IsCheckedOut)
                {
                    string name = Convert.ToString(GetExtraInfo("docCheckedOutLocation"));
                    string[] pars = name.Split(new string[] { "]:[" }, StringSplitOptions.None);
                    if (pars.Length > 1)
                    {
                        return pars[1].Trim('[', ']');
                    }
                    else
                        return "?";

                }
                return String.Empty;
            }
        }

        [System.ComponentModel.Browsable(false)]
        public DateTime? CheckedOutTime
        {
            get
            {
                CheckOutStatusRefresh();
                if (GetExtraInfo("docCheckedOut") == DBNull.Value)
                    return null;
                else
                    return Convert.ToDateTime(GetExtraInfo("docCheckedOut"));
            }
        }


        [System.ComponentModel.Browsable(false)]
        public int CheckOutDuration
        {
            get
            {
                CheckOutStatusRefresh();
                DateTime? dte = ThisLockableStorageItem.CheckedOutTime;
                if (dte == null)
                    return 0;
                else
                {
                    //UTCFIX: DM - 30/11/06 - Make dte.Value local.
                    return DateTime.Now.Subtract(dte.Value.ToLocalTime()).Minutes;
                }
            }
        }

        [System.ComponentModel.Browsable(false)]
        public User CheckedOutBy
        {
            get
            {
                CheckOutStatusRefresh();
                if (GetExtraInfo("docCheckedOutBy") == DBNull.Value)
                {
                    if (checkedoutby != null)
                        checkedoutby = null;
                }
                else
                {
                    if (checkedoutby == null || checkedoutby.ID != Convert.ToInt32(GetExtraInfo("docCheckedOutBy")))
                        checkedoutby = User.GetUser(Convert.ToInt32(GetExtraInfo("docCheckedOutBy")));
                }
                return checkedoutby;
            }
        }

        private DateTime lastCheckoutCheck = DateTime.Now;
        private User checkedoutby = null;

        private void CheckOutStatusRefresh()
        {
            //UTCFIX: DM - 30/11/06 - Make last checked out stamp local.
            if (DateTime.Now.Subtract(lastCheckoutCheck.ToLocalTime()).Minutes > 0)
            {
                IDataParameter[] pars = new IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);

                DataTable tbl = Session.CurrentSession.Connection.ExecuteSQLTable("select docCheckedOut, docCheckedOutBy, docCheckedOutLocation from dbDocument where docid = @docid", "DOCS", pars);
                if (tbl.Rows.Count > 0)
                {
                    var dt = tbl.Rows[0]["docCheckedOut"] as DateTime?;
                    if (dt.HasValue && dt.Value.Kind == DateTimeKind.Unspecified)
                        dt = DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc);

                    SetExtraInfo("docCheckedOut", dt.HasValue ? (object)dt.Value : DBNull.Value);
                    SetExtraInfo("docCheckedOutBy", tbl.Rows[0]["docCheckedOutBy"]);
                    SetExtraInfo("docCheckedOutLocation", tbl.Rows[0]["docCheckedOutLocation"]);
                }
                lastCheckoutCheck = DateTime.Now;
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool CanCheckOut
        {
            get
            {
                if (IsNew == false)
                {
                    User user = ThisLockableStorageItem.CheckedOutBy;

                    if (!ThisLockableStorageItem.IsCheckedOut)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        void IStorageItemLockable.CheckOut(System.IO.FileInfo localFile)
        {
            // CM 120215 - WI 5738  - If Document has read only access, prevent it from being checked out
            if (!FWBS.OMS.Security.SecurityManager.CurrentManager.IsGranted(new DocumentPermission(this, StandardPermissionType.Update)))
            {
                System.Diagnostics.Debug.WriteLine("Read Only Access on Document - Checkout disabled", "IStorageItemLockable.CheckOut");
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
                    pars[2] = Session.CurrentSession.Connection.AddParameter("docid", ID);


                    string location = String.Empty;

                    if (localFile == null || !System.IO.File.Exists(localFile.FullName))
                        location = String.Format("[{0}]:[?]", Common.Functions.GetMachineName());
                    else
                        location = String.Format("[{0}]:[{1}]", Common.Functions.GetMachineName(), localFile.FullName);

                    pars[3] = Session.CurrentSession.Connection.AddParameter("location", location);

                    Session.CurrentSession.Connection.ExecuteSQL("update dbdocument set doccheckedout = @checkedout, doccheckedoutby = @by, doccheckedoutlocation = @location where docid = @docid", pars);

                    SetExtraInfo("docCheckedOut", now);
                    SetExtraInfo("docCheckedOutBy", Session.CurrentSession.CurrentUser.ID);
                    SetExtraInfo("docCheckedOutLocation", location);

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

        void IStorageItemLockable.UpdateCheckedOutLocation(System.IO.FileInfo localFile)
        {
            IDataParameter[] pars = new IDataParameter[2];
            pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);
            string location = String.Empty;
            if (localFile == null || !System.IO.File.Exists(localFile.FullName))
                location = String.Format("[{0}]:[?]", Common.Functions.GetMachineName());
            else
                location = String.Format("[{0}]:[{1}]", Common.Functions.GetMachineName(), localFile.FullName);
            pars[1] = Session.CurrentSession.Connection.AddParameter("location", location);
            Session.CurrentSession.Connection.ExecuteSQL("update dbdocument set doccheckedoutlocation = @location where docid = @docid", pars);
            SetExtraInfo("docCheckedOutLocation", location);
        }

        [System.ComponentModel.Browsable(false)]
        public bool CanUndo
        {
            get
            {
                if (IsNew == false && ThisStorageItem.IsNew == false)
                {
                    return ThisLockableStorageItem.IsCheckedOutByCurrentUser;
                }

                return false;
            }
        }

        void IStorageItemLockable.UndoCheckOut()
        {
            if (IsNew == false && ThisStorageItem.IsNew == false)
            {
                User user = ThisLockableStorageItem.CheckedOutBy;

                if (ThisLockableStorageItem.CanUndo)
                {
                    IDataParameter[] pars = new IDataParameter[1];
                    pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);
                    Session.CurrentSession.Connection.ExecuteSQL("update dbdocument set doccheckedout = null, doccheckedoutby = null, doccheckedoutlocation = null where docid = @docid", pars);

                    SetExtraInfo("docCheckedOut", DBNull.Value);
                    SetExtraInfo("docCheckedOutBy", DBNull.Value);
                    SetExtraInfo("docCheckedOutLocation", DBNull.Value);

                    StorageManager.CurrentManager.LocalDocuments.Set(this, null, false);

                    ThisStorageItem.AddActivity("UNDO", null);
                }
                else
                {
                    if (ThisLockableStorageItem.IsCheckedOutByAnother)
                        throw new StorageItemCheckedOutException(user.FullName, ThisLockableStorageItem.CheckedOutTime.Value);
                }
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool CanCheckIn
        {
            get
            {
                if (IsNew == false && ThisStorageItem.IsNew == false)
                {
                    return ThisLockableStorageItem.IsCheckedOutByCurrentUser;
                }

                return false;
            }
        }

        void IStorageItemLockable.CheckIn()
        {
            if (IsNew == false && ThisStorageItem.IsNew == false)
            {
                User user = ThisLockableStorageItem.CheckedOutBy;

                if (ThisLockableStorageItem.IsCheckedOutByCurrentUser)
                {
                    IDataParameter[] pars = new IDataParameter[1];
                    pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);
                    Session.CurrentSession.Connection.ExecuteSQL("update dbdocument set doccheckedout = null, doccheckedoutby = null, doccheckedoutlocation = null where docid = @docid", pars);

                    SetExtraInfo("docCheckedOut", DBNull.Value);
                    SetExtraInfo("docCheckedOutBy", DBNull.Value);
                    SetExtraInfo("docCheckedOutLocation", DBNull.Value);

                    StorageManager.CurrentManager.LocalDocuments.Set(this, null, false);

                    ThisStorageItem.AddActivity("CHECKEDIN", null);
                }
                else
                {
                    if (ThisLockableStorageItem.IsCheckedOutByAnother)
                        throw new StorageItemCheckedOutException(user.FullName, ThisLockableStorageItem.CheckedOutTime.Value);
                }
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsCheckedOut
        {
            get
            {
                if (IsNew)
                    return false;

                return (ThisLockableStorageItem.CheckedOutTime != null);
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsCheckedOutByCurrentUser
        {
            get
            {
                if (ThisLockableStorageItem.IsCheckedOut)
                {
                    User checkedoutby = ThisLockableStorageItem.CheckedOutBy;
                    return (ThisLockableStorageItem.CheckedOutBy.ID == Session.CurrentSession.CurrentUser.ID);
                }

                return false;
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsCheckedOutByAnother
        {
            get
            {
                if (ThisLockableStorageItem.IsCheckedOut)
                {
                    User checkedoutby = ThisLockableStorageItem.CheckedOutBy;
                    return (ThisLockableStorageItem.CheckedOutBy.ID != Session.CurrentSession.CurrentUser.ID);
                }

                return false;
            }
        }

        #endregion

        #region IStorageItemVersionable

        private event EventHandler<NewVersionEventArgs> newVersion;
        public event EventHandler<NewVersionEventArgs> NewVersion
        {
            add
            {
                newVersion+=value;
            }
            remove
            {
                newVersion-=value;
            }

        }

        void IStorageItemVersionable.OnNewVersion(NewVersionEventArgs e)
        {
            if (newVersion != null)
            {
                newVersion(this, e);
            }
        }

        private DateTime lastFetchedVersions  = DateTime.Now;
        private DataTable versionstable = null;
        private System.Collections.Generic.Dictionary<Guid, DocumentVersion> versions = new System.Collections.Generic.Dictionary<Guid,DocumentVersion>();

        internal void AddVersion(DocumentVersion version)
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
                pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);
                versionstable = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbdocumentversion where docid = @docid and verdeleted = 0 order by verdepth, vernumber", "VERSIONS", pars);
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
                    System.Collections.Generic.List<DocumentVersion> list = new System.Collections.Generic.List<DocumentVersion>();

                    foreach (DataRowView r in vw)
                    {
                        Guid id = (Guid)r["verid"];
                        DocumentVersion dv;
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
                            dv = new DocumentManagement.DocumentVersion(this, r.Row);
                        }
                        list.Add(dv);
                    }

                    if (versions.Count == 0 && list.Count == 0 && IsNew == false)
                    {
                        DocumentVersion latest = (DocumentManagement.DocumentVersion)ThisVersionableStorageItem.CreateVersion();
                        latest.Token = ThisStorageItem.Token;
                        //Default version needs accepting, version already exists frommain header record.
                        //but only if the document is not new
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
                            DocumentVersion ver = versions[id];
                            list.Add(ver); //DM-07-12-2010 - Now adding to list even if not new because duplicate document versions were being created 
                        }
                    }

                    versions.Clear();
       

                    foreach (DocumentVersion ver in list)
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
                return new DocumentManagement.DocumentVersion(this, vw[0].Row);
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
                return new DocumentManagement.DocumentVersion(this, vw[0].Row);
            else
                return null;
        }


        private DocumentManagement.DocumentVersion latest = null;

        public IStorageItemVersion GetLatestVersion()
        {
            DocumentManagement.Storage.IStorageItemVersion[] versions = ThisVersionableStorageItem.GetVersions();

            if (versions.Length == 0)
            {
                latest = (DocumentManagement.DocumentVersion)ThisVersionableStorageItem.CreateVersion();
                latest.Token = ThisStorageItem.Token;
                //Default version needs accepting, version already exists frommain header record.
                //but only if the document is not new
                if (!IsNew)
                    latest.Accepted = true;
                latest.Preview = ThisStorageItem.Preview;
                latest.Checksum = ThisDuplicationStorageItem.Checksum;
                latest.InternalUpdate();
            }
            else
            {
                object id = GetExtraInfo("docCurrentVersion");
                if (Convert.IsDBNull(id))
                {
                    latest = null;
                }
                else if (latest == null)
                {
                    Guid currentId = (Guid)id;

                    foreach (DocumentVersion ver in versions)
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
                        foreach (DocumentVersion ver in versions)
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
                    latest = (DocumentVersion)versions[versions.Length - 1];


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
                throw new ArgumentException("The parent document must be the same as the current document");
            if (version.IsLatestVersion)
                throw new StorageException("ERRCANTDELLAT", "Cannot delete version '%1%', it is set as the latest version.", null, version.Label);

            lock (version)
            {
                DocumentVersion docver = version as DocumentVersion;
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
            //DM 07-12-2010  - Making the assumption that the unaccepted document version is the document being saved
            if (versions.Count == 1)
            {
                var latest = GetLatestVersion();
                if (!latest.Accepted)
                    return latest;
            }

            return new DocumentVersion(this);
        }

        IStorageItemVersion IStorageItemVersionable.CreateVersion(IStorageItemVersion original)
        {
            if (original == null)
                throw new ArgumentNullException("original");
            if (original.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
                throw new ArgumentException("The parent document must be the same as the current document");

            //DM 07-12-2010  - Making the assumption that the unaccepted document version is the document being saved
            if (!original.Accepted)
                return original;

            return new DocumentVersion((DocumentVersion)original, false);
        }

        IStorageItemVersion IStorageItemVersionable.CreateSubVersion(IStorageItemVersion original)
        {
            if (original == null)
                throw new ArgumentNullException("original");
            if (original.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
                throw new ArgumentException("The parent document must be the same as the current document");

            //DM 07-12-2010  - Making the assumption that the unaccepted document version is the document being saved
            if (!original.Accepted)
                return original;

            return new DocumentVersion((DocumentVersion)original, true);
        }

        
        void IStorageItemVersionable.SetLatestVersion(IStorageItemVersion current)
        {
            if (current == null)
                throw new ArgumentNullException("current");
            if (current.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
                throw new ArgumentException("The parent document must be the same as the current document");

            lock (current)
            {
                latest = (DocumentVersion)current;
                latest.InternalUpdate();
                SetExtraInfo("docCurrentVersion", latest.Id);
                SetExtraInfo("docextension", latest.Extension);
                ThisStorageItem.Token = latest.Token;
                ThisStorageItem.Preview = latest.Preview;
                ThisDuplicationStorageItem.Checksum = latest.Checksum;
            }
        }

        DocumentVersion workingVersion;
        void IStorageItemVersionable.SetWorkingVersion(IStorageItemVersion version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            if (version.BaseStorageItem.Pointer != ThisStorageItem.Pointer)
                throw new ArgumentException("The parent document must be the same as the current working document");

            workingVersion = (DocumentVersion)version;
        }

        IStorageItemVersion IStorageItemVersionable.GetWorkingVersion()
        {
            if (workingVersion == null)
                workingVersion = (DocumentVersion)ThisVersionableStorageItem.GetLatestVersion();
            return workingVersion;
        }

        #endregion

        #region IStorageItem Members

        private DateTime? GetLastOpened(bool currentUser)
        {
            DataView vw = new DataView(ThisStorageItem.GetActivities());
            if (currentUser)
                vw.RowFilter = String.Format("(logtype = 'OPENED' or logtype = 'SAVED') and usrid = {0}", Session.CurrentSession.CurrentUser.ID);
            else
                vw.RowFilter = String.Format("(logtype = 'OPENED' or logtype = 'SAVED') and usrid <> {0}", Session.CurrentSession.CurrentUser.ID);
            vw.Sort = "logtime desc";

            if (vw.Count > 0)
                return Convert.ToDateTime(vw[0]["logtime"]);
            else
                return null;
        }

        private DateTime? GetLastSaved(bool currentUser, out Guid? versionId)
        {
            DataView vw = new DataView(ThisStorageItem.GetActivities());
            if (currentUser)
                vw.RowFilter = String.Format("logtype = 'SAVED' and usrid = {0}", Session.CurrentSession.CurrentUser.ID);
            else
                vw.RowFilter = String.Format("logtype = 'SAVED' and usrid <> {0}", Session.CurrentSession.CurrentUser.ID);
            vw.Sort = "logtime desc";

            if (vw.Count > 0)
            {
                if (Convert.IsDBNull(vw[0]["verid"]))
                    versionId = null;
                else
                    versionId = (Guid)vw[0]["verid"];

                return Convert.ToDateTime(vw[0]["logtime"]);
            }
            else
            {
                versionId = null;
                return null;
            }
        }

        public bool IsConflicting
        {
            get
            {
                return IsConflict(true);
            }
        }

        public System.Drawing.Icon GetIcon()
        {
           return Common.IconReader.GetFileIcon(String.Format("test.{0}", ThisStorageItem.Extension), Common.IconReader.IconSize.Small, false);   
        }

        private bool IsConflict(bool refresh)
        {
            if (refresh)
                lastActivityCheck = DateTime.MinValue.ToLocalTime();

            Guid? versionId;
            DateTime? opened = GetLastOpened(true);
            DateTime? saved = GetLastSaved(false, out versionId);

            if (opened == null)
                return false;
            if (saved == null)
                return false;

            if (versions.Count == 0)
                ThisVersionableStorageItem.GetVersions();

            //version id could be null if versions are not being stored in the document log.
            //Certain document types no longer have the versioning option switched on.
            if (versionId.HasValue)
            {
                if (!versions.ContainsKey(versionId.Value))
                    return false;
            }

            return (opened.Value < saved.Value);
        }


        public IStorageItem GetConflict()
        {
            if (IsConflict(false))
            {
                Guid? versionId;
                GetLastSaved(false, out versionId);
                if (versionId == null)
                    return this;
                else
                {
                    if (versions.Count == 0)
                        ThisVersionableStorageItem.GetVersions();

                    lock (this.versions)
                    {
                        if (versions.ContainsKey(versionId.Value))
                            return versions[versionId.Value];
                        else
                            return null;
                    }
                }
            }
            else
                return null;
        }

        private bool isnew_storeitem = false;
        bool IStorageItem.IsNew        
        {
            get
            {
                return isnew_storeitem;
            }
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
            InternalUpdate();
            isnew_storeitem = false;
        }

        StorageSettingsCollection settings = null;
        public StorageSettingsCollection GetSettings()
        {
            return settings;
        }

        public void ApplySettings(StorageSettingsCollection settings)
        {
            this.settings = settings;
        }


        public void ClearSettings()
        {
            if (settings != null)
                settings.Clear();
            settings = null;
        }

        public bool Supports(DocumentManagement.Storage.StorageFeature feature)
        {
            DocType type = CurrentDocumentType;

            StorageFeature features = type.StorageFeaturesSupported;

            if (StorageManager.CurrentManager.IsFeatureImplemented(feature))
            {
                if (features == 0)
                    return true;
                else
                    return type.Supports(feature);
            }
            else
                return false;
  
        }

        /// <summary>
        /// Gets the extension of the item when saved to a file.
        /// </summary>
        string IStorageItem.Extension
        {
            get
            {
                string ext = Convert.ToString(GetExtraInfo("docextension")).Replace(".", "");
                if (String.IsNullOrEmpty(ext))
                {
                    ext = CurrentDocumentType.DefaultDocExtension;
                    SetExtraInfo("docextension", ext);
                }
                return ext;
            }
        }

        string IStorageItem.Pointer
        {
            get
            {
                return ID.ToString();
            }
        }

        string IStorageItem.Name
        {
            get 
            {
                return Description;
            }
        }

        public bool Accepted
        {
            get
            {
                return !Common.ConvertDef.ToBoolean(GetExtraInfo("docdeleted"), true);
            }
            set
            {
                SetExtraInfo("docdeleted", !value);
            }
        }

  

        string IStorageItem.Token
        {
            get
            {
                string token = Convert.ToString(GetExtraInfo("docfilename"));

                if (String.IsNullOrEmpty(token))
                {
                    if (latest != null)
                        ThisStorageItem.Token = latest.Token;
                }

                return Convert.ToString(GetExtraInfo("docfilename"));
            }
            set
            {
                if (value == null) value = string.Empty;
                SetExtraInfo("docfilename", value);

            }
        }

        void IStorageItem.ChangeStorage(DocumentManagement.Storage.StorageProvider provider, bool transfer)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            DocumentManagement.Storage.StorageProvider newProvider = StorageManager.CurrentManager.GetStorageProvider(provider.Id);
            if (newProvider != null)
            {
                if (newProvider.Id != CurrentStorageProviderID)
                {
                    if (IsNew == false && transfer)
                    {
                        DocumentManagement.Storage.StorageProvider prov = GetStorageProvider();
                        FetchResults si = prov.Fetch(this, true);
                        newProvider.Store(this, si.LocalFile, si.Tag, true);
                    }

                    CurrentStorageProviderID = provider.Id;
                }
            }
        }

        public DocumentManagement.Storage.StorageProvider GetStorageProvider()
        {
            return StorageManager.CurrentManager.GetStorageProvider(CurrentStorageProviderID);
        }

        IStorageItemType IStorageItem.GetItemType()
        {
            return CurrentDocumentType;
        }

        public System.IO.FileInfo GetIdealLocalFile()
        {
            System.IO.DirectoryInfo dir = StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory;
            string fp = System.IO.Path.Combine(dir.FullName, String.Format("{0}.{1}", FWBS.Common.FilePath.ExtractInvalidChars(ThisStorageItem.Pointer), FWBS.Common.FilePath.ExtractInvalidChars(ThisStorageItem.Extension)));
            return new System.IO.FileInfo(fp);
        }

      
        /// <summary>
        /// Gets the current storage location type id.
        /// </summary>
        internal short CurrentStorageProviderID
        {
            get
            {
                return Common.ConvertDef.ToInt16(GetExtraInfo("doclocation"), -1);
            }
            set
            {
                if (CurrentStorageProviderID != value)
                {
                    SetExtraInfo("doclocation", value);
                }
            }
        }

        private DocumentManagement.Storage.IStorageItem ThisStorageItem
        {
            get
            {
                return (DocumentManagement.Storage.IStorageItem)this;
            }
        }

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

        private DateTime lastActivityCheck = DateTime.MinValue.ToLocalTime();
        private DataTable activities= null;

        public void AddActivity(string action, string subaction)
        {
            AddActivity(action, subaction, null);
        }
        public void AddActivity(string action, string subaction, string data)
        {
            if (String.IsNullOrEmpty(action))
                throw new ArgumentException("subaction");

            if (IsNew == false)
            {
                List<IDataParameter> pars = new List<IDataParameter>();

                pars.Add(Session.CurrentSession.Connection.AddParameter("docid", ID));
                pars.Add(Session.CurrentSession.Connection.AddParameter("action", SqlDbType.NVarChar, 15, action));
                if (String.IsNullOrEmpty(subaction))
                    pars.Add(Session.CurrentSession.Connection.AddParameter("subaction", SqlDbType.NVarChar, 15, DBNull.Value));
                else
                    pars.Add(Session.CurrentSession.Connection.AddParameter("subaction", SqlDbType.NVarChar, 15, subaction));
                pars.Add(Session.CurrentSession.Connection.AddParameter("usrid", Session.CurrentSession.CurrentUser.ID));


                if (String.IsNullOrEmpty(data))
                    pars.Add(Session.CurrentSession.Connection.AddParameter("data", DBNull.Value));
                else
                    pars.Add(Session.CurrentSession.Connection.AddParameter("data", SqlDbType.NVarChar, 100, data));

                if (Session.CurrentSession.IsProcedureInstalled("sprCreateDocumentAction"))
                    activities = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCreateDocumentAction", "ACTIVITIES", pars.ToArray());
                else
                    activities = Session.CurrentSession.Connection.ExecuteSQLTable("insert into dbdocumentlog(docid, logtype, logcode, usrid) values(@docid, @action, @subaction, @usrid);select * from dbdocumentlog where docid = @docid", "ACTIVITIES", pars.ToArray());

                lastActivityCheck = DateTime.Now;
            }
        }

        public System.Data.DataTable GetActivities()
        {
            if (activities == null || DateTime.Now.Subtract(lastActivityCheck).Minutes > 0)
            {
                IDataParameter[] pars = new IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.AddParameter("docid", ID);

                activities = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbdocumentlog where docid = @docid", "ACTIVITIES", pars);
                
                lastActivityCheck = DateTime.Now;
            }

            return activities.Copy();
        }


        #endregion

        #region IStorageItemDuplication

        #region Duplication Checksum Logic

        /// <summary>
        /// Gets the checksum of the active document.
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

        /// <summary>
        /// Gets the file that the document is duplicated on.
        /// </summary>
        private OMSFile DuplicatedFileRef
        {
            get
            {
                return _duplicatedFileRef;
            }
        }


        void IStorageItemDuplication.GenerateChecksum(string value)
        {
            if (String.IsNullOrEmpty(value))
                ThisDuplicationStorageItem.Checksum = null;
            else
                ThisDuplicationStorageItem.Checksum = OMSDocument.GenerateChecksum(value);
        }

        /// <summary>
        /// A flag that allows the duplication of the document.
        /// </summary>
        [EnquiryUsage(true)]
        public bool AllowDuplication
        {
            get
            {
                return _allowduplicatedoc;
            }
            set
            {
                _allowduplicatedoc = value;
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
        /// Gets or Sets the duplicate checsum identity.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        string IStorageItemDuplication.Checksum
        {
            get
            {
                string checksum = Convert.ToString(GetExtraInfo("docchecksum"));
                if (String.IsNullOrEmpty(checksum))
                {
                    if (latest != null)
                        ThisDuplicationStorageItem.Checksum = ((IStorageItemDuplication)latest).Checksum;
                }

                return Convert.ToString(GetExtraInfo("docchecksum"));
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    SetExtraInfo("docchecksum", DBNull.Value);
                else
                    SetExtraInfo("docchecksum", value);
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
            long fileid = -1;
            long docid = -1;

            if (ThisDuplicationStorageItem.Checksum == String.Empty)
                return false;

            if (IsNew)
            {
                string sql = "select top 1 fileid, docid from config.dbdocument where docchecksum = @checksum and docdeleted = 0";
                IDataParameter[] pars = new IDataParameter[2];
                // MNW FIX from nvarchar to varchar
                pars[0] = Session.CurrentSession.Connection.AddParameter("checksum", System.Data.SqlDbType.VarChar, 50, ThisDuplicationStorageItem.Checksum);

                switch (Session.CurrentSession.DuplicateDocumentCheckerLevel)
                {
                    case "S":
                        sql = "select top 1 fileid, docid from config.dbdocument where docchecksum = @checksum and docdeleted = 0";
                        break;
                    case "O":
                        return false;
                    case "C":
                        sql = "select top 1 fileid, docid from config.dbdocument where docchecksum = @checksum and clid = @id and docdeleted = 0";
                        pars[1] = Session.CurrentSession.Connection.CreateParameter("id", ClientID);
                        break;
                    case "F":
                        sql = "select top 1 fileid, docid from config.dbdocument where docchecksum = @checksum and fileid = @id and docdeleted = 0";
                        pars[1] = Session.CurrentSession.Connection.CreateParameter("id", OMSFileID);
                        break;
                    default:
                        goto case "S";
                }

                if (_duplicatedFileRef == null || getdoc)
                {
                    DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "DUPLICATES", pars);
                    if (dt.Rows.Count > 0)
                    {
                        fileid = Common.ConvertDef.ToInt64(dt.Rows[0]["fileid"], -1);
                        docid = Common.ConvertDef.ToInt64(dt.Rows[0]["docid"], -1);
                        if (getdoc)
                        {
                            duplicate = OMSDocument.GetDocument(docid);
                        }
                    }
                    try
                    {
                        if (fileid != -1) _duplicatedFileRef = OMSFile.GetFile(fileid);
                    }
                    catch { }
                }

                if (_duplicatedFileRef != null)
                    return true;
            }

            return false;
        }


        #endregion

        #endregion

        #region ISecurable Members

        string FWBS.OMS.Security.ISecurable.SecurityId
        {
            get { return ID.ToString(); }
        }

        private FWBS.OMS.Security.SecurityOptions SecurityOptions
        {
            get
            {
                object val = GetExtraInfo("SecurityOptions");
                return (FWBS.OMS.Security.SecurityOptions)FWBS.Common.ConvertDef.ToInt64(val, 0);
            }
            set
            {
                SetExtraInfo("SecurityOptions", (long)value);
            }
        }

		FWBS.OMS.Security.SecurityOptions FWBS.OMS.Security.ISecurable.SecurityOptions
		{
			get
			{
                return SecurityOptions;
			}
			set
			{
                if (value != SecurityOptions)
                    SecurityOptions = value;
			}
		}

        private DateTime timestamp;
        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
        }

        private void CheckPermissions()
        {
            bool isnew = IsNew || ThisStorageItem.IsNew;
            bool isdirty = IsDirty;

            if (isnew)
            {
                new FilePermission(OMSFile, StandardPermissionType.SaveDocument).Check();
            }
            else if (isdirty)
            {
                new DocumentPermission(this, StandardPermissionType.Update).Check();
            }

            new SystemPermission(StandardPermissionType.SaveDocument).Check();
            new FileActivity(OMSFile, FileStatusActivityType.DocumentModification).Check();
        }

        #endregion 

        #region IOMSType Members

        public virtual OMSType GetOMSType()
        {
            return FWBS.OMS.DocType.GetDocType(this.DocumentType);
        }

        public object LinkValue
        {
            get
            {
                return ID;
            }
        }

        private string _defaultTab;
        public string DefaultTab
        {
            get
            {
                return _defaultTab;
            }
            set
            {
                _defaultTab = value;
            }
        }

        #endregion



        #region IOMSType Members


        public void SetCurrentSessions()
        {
        }

        #endregion
    }

	
}

