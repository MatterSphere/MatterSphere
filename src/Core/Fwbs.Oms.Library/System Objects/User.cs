using System;
using System.Data;
using System.Linq;
using System.Threading;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    using System.Collections.Generic;
    using FWBS.OMS.Data;


    /// <summary>
    /// 1000 Holds a user definition.  This user object can be used with the enquiry engine.
    /// </summary>
    public class User : LookupTypeDescriptor, IOMSType, IDisposable, IExtendedDataCompatible
	{
	
		#region Constants

		public const string ROLE_PARTNER = "PARTNER";
		public const string ROLE_ADMIN = "ADMIN";

		#endregion

		#region Fields
		/// <summary>
		/// Sets the Default Tab when called from the WinUI Layer
		/// </summary>
		private string _defaultTab = null;

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _user = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbUser";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "USER";


		/// <summary>
		/// Holds a reference to user overloaded session script object.
		/// </summary>
		private Script.ScriptGen _script = null;

	
		/// <summary>
		/// Holds the unique identifier of the user.  This should never be changed as it is the unique id.
		/// </summary>
		private int _id = 0;

	
		/// <summary>
		/// Override Table Is Dirty
		/// </summary>
		private bool _isdirty = false;

		/// <summary>
		/// Holds the different extended data sources for the contact.
		/// </summary>
		private ExtendedDataList _extData = null;

		/// <summary>
		/// Skips the setting of the Updated and UpdatedBy fields when on the next Update on the User object
		/// </summary>
		private bool _doNotSetUpdatedFieldsOnNextUpdate;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new user with a specified user type.
		/// </summary>
		/// <param name="type">The user type.</param>
		public User (UserType type) : this()
		{
			if (type == null)
				throw new ArgumentNullException("type");

			this.SetExtraInfo("usrtype", type.Code);
		}

		/// <summary>
		/// Creates a new user object.  This routine is used by the enquiry engine
		/// to create new user object.
		/// </summary>
		internal User()
		{
			_user = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Add a new record.
			Global.CreateBlankRecord(ref _user, true);

			//Set the created by and created date of the item.
			this.SetExtraInfo("usrType", "STANDARD");
			this.SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
			this.SetExtraInfo("Created", DateTime.Now);


			BuildXML();

            this.OnExtLoaded();
		}

    
        /// <summary>
        /// Initialised an existing user object with the specified identifier.
        /// </summary>
        /// <param name="id">User Identifier.</param>
        [EnquiryUsage(true)]
		internal User (int id)
		{
			Fetch(id, null);

			BuildXML();

			if (this.GetType() == typeof(User))
				Session.CurrentSession.CurrentUsers.Add(ID.ToString(), this);

            this.OnExtLoaded();
		}

		internal User(DataTable data, string userName)
		{
			//Only used for loggin in.
			if (data == null)
				throw new ArgumentNullException("data");

			this._user = data.Copy();

			//PJ - DM was accessing the Initials instance property when there was no data from a user logging
			//in which did not exist.
			if ((_user == null) || (_user.Rows.Count == 0))
				throw new Security.InvalidOMSUserException(userName);

 
			BuildXML();

			if (this.GetType() == typeof(User))
				Session.CurrentSession.CurrentUsers.Add(ID.ToString(), this);

            this.OnExtLoaded();
		}

		/// <summary>
		///	Creates a new user based on the login type and userName that is to be used.
		/// This contructor is used by the Session login method to create the currently logged in user.
		/// The login type tells the user object what unique identifer to use.
		/// </summary>
		/// <param name="loginType">Login type SQLm OMS or NT, whatever is required.</param>
		/// <param name="userName">User name to compare.</param>
		[Obsolete("This constructor will no longer be needed from Davenport onwards")]
		internal User (string loginType, string userName)
		{
			string sqlwhere = "";
			
			if (userName == null || userName.Trim() == String.Empty)
				throw new Security.InvalidOMSUserException("?");

			switch (loginType)
			{
				case "OMS":	//Logs into OMS validating with dbUser and using a common SQL server login.
					sqlwhere += " where usrInits = '" + SQLRoutines.RemoveRubbish(userName) + "' or usrAlias = '" + SQLRoutines.RemoveRubbish(userName) + "'";
					break;
				case "SQL":  //Actual user name and password validates the user as a SQL server login.
					sqlwhere += " where usrSQLID = '" + SQLRoutines.RemoveRubbish(userName) + "'";
					break;
				case "AAD":
				case "NT":	//Uses NT to authenticate SQL server login then matches up with a OMS user.
					string ntUser = Environment.UserName;
					if (Session.CurrentSession.UIType == UIClientType.Web)
						ntUser = userName;
					sqlwhere += " where usrADID = system_user";
					break;
				case "ADID":	//Uses NT to authenticate SQL server login then matches up with a OMS user.
					sqlwhere += " where usrADID = '" + SQLRoutines.RemoveRubbish(userName) + "'";
					break;
				default:
					goto case "OMS";
			}
			_user = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + sqlwhere, Table, new IDataParameter[0]);
			if ((_user == null) || (_user.Rows.Count == 0)) 
				throw new Security.InvalidOMSUserException(userName);


			BuildXML();

			if (this.GetType() == typeof(User))
				Session.CurrentSession.CurrentUsers.Add(ID.ToString(), this);
		}


		/// <summary>
		/// Fetches an instance of the underlying data of the user.
		/// </summary>
		/// <param name="id"></param>
        /// <param name="merge"></param>
		private void Fetch(int id, DataRow merge)
		{
			DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where usrID = " + id.ToString(), Table, new IDataParameter[0]);
			if ((data == null) || (data.Rows.Count == 0)) 
				throw new Security.InvalidOMSUserException(id.ToString());

			if (merge != null)
				Global.Merge(data.Rows[0], merge);

            _user = data;
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
                    switch (_user.Rows[0].RowState)
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


        /// <summary>
        /// Gets a value indicating whether the user object is new and needs to be 
        /// updated to exist in the database.
        /// </summary>
        public bool IsNew
		{
			get
			{
				try
				{
					return (_user.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Gets the user identifier.
		/// </summary>
		public int ID
		{
			get
			{
				if (_id == 0)
					_id = Convert.ToInt32(_user.Rows[0]["usrid"]);
				return _id;
			}
		}


		/// <summary>
		/// Gets the user type code.
		/// </summary>
		public string UserTypeCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrtype"));
			}
		}

		/// <summary>
		///  Gets the user's OMS login initals.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string Initials
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrInits"));
			}
			set
			{
				if (value == null || value.Trim() == "")
					SetExtraInfo("usrInits", DBNull.Value);
				else
					SetExtraInfo("usrInits", value);
			}
		}
		
		
		/// <summary>
		/// Gets the user's OMS alias initals.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string Alias
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrAlias"));
			}
			set
			{
				if (value == null || value.Trim() == "")
					SetExtraInfo("usrAlias", DBNull.Value);
				else
					SetExtraInfo("usrAlias", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Home Page of the user mainly aimed at Sharepoint integration.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string HomePage
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrHomePage"));
			}
			set
			{
				SetExtraInfo("usrHomePage", value);
			}
		}




		/// <summary>
		/// Gets the active directory unqiue id or the windows integrated domain\user name of the user.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("LOGON")]
		public string ActiveDirectoryID
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrADID"));
			}
			set
			{
				if (value == null || value.Trim() == "")
					SetExtraInfo("usrADID", DBNull.Value);
				else
					SetExtraInfo("usrADID", value);
			}
		}

		/// <summary>
		/// Gets the SQL server login name that this OMS user uses to log into the SQL server.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("LOGON")]
		public string SQLServerLogin
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrSQLID"));
			}
			set
			{
				if (value == null || value.Trim() == "")
					SetExtraInfo("usrSQLID", DBNull.Value);
				else
					SetExtraInfo("usrSQLID", value);
			}
		}

		/// <summary>
		/// Gets the fullname of the user.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string FullName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrFullName"));
			}
			set
			{
				SetExtraInfo("usrFullName", value);
			}
		}

		/// <summary>
		/// Gets the current user type object.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public UserType CurrentUserType
		{
			get
			{
				return (UserType)GetOMSType();
			}
		}

		/// <summary>
		/// Gets the command centre used by the user.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public CommandCentreType CommandCentre
		{
			get
			{
				return CommandCentreType.GetCentreType(Convert.ToString(GetExtraInfo("usrCCType")));
			}
		}

		/// <summary>
		/// Gets or Sets the extension number for this user.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string ExtensionNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrExtension"));
			}
			set
			{
				SetExtraInfo("usrExtension", value);
			}
		}

		/// <summary>
		/// Gets or Sets the direct telephone number of the user within the company.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string DirectTelephoneNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrDDI"));
			}
			set
			{
				SetExtraInfo("usrDDI", value);
			}
		}

		/// <summary>
		/// Gets or Sets the direct fax number of the user within the company.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string DirectFaxNumber
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrDDIFax"));
			}
			set
			{
				SetExtraInfo("usrDDIFax", value);
			}
		}
	
		/// <summary>
		/// Gets a branch object that the use is registered at.
		/// </summary>
		[LocCategory("OMS")]
		public Branch Branch
		{
			get
			{
				if (GetExtraInfo("brid") is DBNull)
					return null;
				else
					return new Branch(Convert.ToInt32(GetExtraInfo("brid")));
			}
		}

		/// <summary>
		/// Gets / Checks whether the user is currently active on the system.
		/// </summary>
		[LocCategory("Active")]
		public bool IsActive
		{
			get
			{
				try
				{
					return Common.ConvertDef.ToBoolean(GetExtraInfo("usrActive"), false);
				}
				catch
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the prefered currency.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("LANGUAGE")]
		public string Currency
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrcurISOCode"));
			}
			set
			{
				if (value == "")
					SetExtraInfo("usrcurISOCode", DBNull.Value);
				else
				{
					SetExtraInfo("usrcurISOCode", value);
				}
			}
		}
				
		/// <summary>
		/// Gets the user's email address.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("(DETAILS)")]
		public string Email
		{
			get
			{		
				return Convert.ToString(GetExtraInfo("usrEmail"));
			}
			set
			{
				SetExtraInfo("usrEmail", value);
			}
		}


		/// <summary>
		/// Gets the date and time when the user last logged onto the system.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("STATUS")]
		public DateTimeNULL LastLoggedIn
		{
			get
			{
				return ConvertDef.ToDateTimeNULL(GetExtraInfo("usrLastLogin"),DBNull.Value);
			}
		}


		/// <summary>
		/// Gets a boolean value stating whether the user is currenlty logged into the system.
		/// </summary>
		[LocCategory("STATUS")]
		public bool IsLoggedIn
		{
			get
			{
				try
				{
					return Common.ConvertDef.ToBoolean(GetExtraInfo("usrLoggedIn"), false);
				}
				catch
				{
					return false;
				}
			}
		}


		/// <summary>
		/// Gets or Sets the number of failed login attempts the user is currently on.
		/// </summary>
		internal byte FailedLoginAttempts
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("usrFailed");
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("usrFailed", value);
			}
		}


		/// <summary>
		/// Gets the fee earner that the user works for.
		/// </summary>
		[LocCategory("(DETAILS)")]
		public FeeEarner WorksFor
		{
			get
			{
				if ((this.GetExtraInfo("usrWorksFor") is System.DBNull) == false)
					return FeeEarner.GetFeeEarner((int)GetExtraInfo("usrWorksFor"));
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the script object associated to the user.
		/// </summary>
		[LocCategory("SCRIPT")]
		public Script.ScriptGen Script
		{
			get
			{
				if (_script == null)
				{
					string scr = Convert.ToString(GetExtraInfo("usrScript"));
					if (scr == "")
						_script = null;
					else
						_script = new Script.ScriptGen(scr);
				}
				return _script;
			}
		}

		/// <summary>
		/// Gets the last terminal that this user last logged onto the system with.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("STATUS")]
		public string TerminalLastUsed
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrtermName"));
			}
		}

		/// <summary>
		/// Gets the default printer for the current user.
		/// </summary>
		[LocCategory("(DETAILS)")]
		public Printer DefaultPrinter
		{
			get
			{
				if (GetExtraInfo("usrprintid") is System.DBNull)
					return null;
				else
					return new Printer((int)GetExtraInfo("usrprintid"));
			}
		}

	

		/// <summary>
		/// Gets or Sets the registered for information.  This is for internal use only.
		/// </summary>
		private string RegisteredFor
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrregisteredfor"));
			}
			set
			{
				SetExtraInfo("usrregisteredfor", value);
			}
		}

		/// <summary>
		/// Gets or Sets RemeberLastClientnFile
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("SECURITY")]
		public bool RemberLastClientnFile
		{
			get
			{
				return Convert.ToBoolean(GetXmlProperty("RemmberLastClientNFile", true));
			}
			set
			{
				SetXmlProperty("RemmberLastClientNFile", value);
			}
		}


        /// <summary>
        /// Gets or Sets the Roles
        /// </summary>
        [EnquiryUsage(true)]
		[LocCategory("SECURITY")]
		public string Roles
		{
			get
			{
				return Convert.ToString(GetXmlProperty("Roles", ""));
			}
			set
			{
				string rolesyoucantadd;
				string rolesyoucantremove;
				if (CheckRoles(Roles, ref value, out rolesyoucantadd, out rolesyoucantremove))
				{
					SetXmlProperty("Roles", value);
				}
				else
				{
					SetXmlProperty("Roles", value);
					OnPropertyChanged(new PropertyChangedEventArgs("Roles", this.Roles));
					if (!string.IsNullOrEmpty(rolesyoucantadd))
						throw new Security.SecurityException("POWERFAIL1", "You don't have permission to add this Role '%1%'", rolesyoucantadd);
					if (!string.IsNullOrEmpty(rolesyoucantremove))
						throw new Security.SecurityException("POWERFAIL2", "You don't have permission to remove this Role '%1%'", rolesyoucantremove);

				}
			}
		}

		private bool CheckRoles(string previousroles, ref string roles, out string rolesyoucantadd, out string rolesyoucantremove)
		{
            var power = Session.CurrentSession.CurrentPowerUserSettings;
            string _powerroles = power.PowerRoles;
            if (power.IsConfigured)
			{
				if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN") == false && Session.CurrentSession.CurrentUser.IsInRoles("POWER") == true)
				{
					var oldroles = previousroles.Split(',').ToList();
					var newroles = roles.Split(',').ToList();
					List<string> removedroles = new List<string>();
					List<string> addedroles = new List<string>();
					List<string> rolestorestore = new List<string>();
					List<string> rolestoremove = new List<string>();
					foreach (var item in newroles)
					{
						if (!oldroles.Contains(item))
							addedroles.Add(item);
					}
					foreach (var item in oldroles)
					{
						if (!newroles.Contains(item))
							removedroles.Add(item);
					}
                    if (_powerroles == null)
                        _powerroles = "";
					    var powerroles = _powerroles.Split(';').ToList();

					foreach (var item in addedroles)
					{
						if (!powerroles.Contains(item))
							rolestoremove.Add(item);
					}
					foreach (var item in removedroles)
					{
						if (!powerroles.Contains(item))
							rolestorestore.Add(item);
					}

					foreach (var item in rolestoremove)
					{
						newroles.Remove(item);
					}
					foreach (var item in rolestorestore)
					{
						newroles.Add(item);
					}

					roles = string.Join(",", newroles);
					if (rolestorestore.Count > 0 || rolestoremove.Count > 0)
					{
						rolesyoucantadd = string.Join(", ", rolestoremove);
						rolesyoucantremove = string.Join(", ", rolestorestore);
						return false;
					}
				}
			}
			rolesyoucantremove = string.Empty;
			rolesyoucantadd = string.Empty;
			return true;
		}


		/// <summary>
		/// Gets the modification dates and users.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("STATUS")]
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


		[EnquiryUsage(true)]
		[LocCategory("CKINSTATUS")]
		public TriState AutoCheckInUnchangedDocuments
		{
			get
			{
				try
				{
					return (TriState) ConvertDef.ToEnum(GetXmlProperty("AutoCheckInUnchangedDocuments", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("AutoCheckInUnchangedDocuments", value);
			}
		}

		[EnquiryUsage(true)]
		public string AdditionalDocumentSaveCommands
		{
			get
			{
				return Convert.ToString(GetXmlProperty("AdditionalDocumentSaveCommands", ""));
			}
			set
			{
				SetXmlProperty("AdditionalDocumentSaveCommands", value);
			}
		}

		[EnquiryUsage(true)]
		public TriState PromptBeforeSaveAsOnResave
		{
			get
			{
				try
				{
					return (TriState)ConvertDef.ToEnum(GetXmlProperty("PromptBeforeSaveAsOnResave", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("PromptBeforeSaveAsOnResave", value);
			}
		}

		[EnquiryUsage(true)]
		public string DefaultSaveWizard
		{
			get
			{
				return Convert.ToString(GetXmlProperty("DefaultSaveWizard", null));
			}
			set
			{
				SetXmlProperty("DefaultSaveWizard", value);
			}
		}

		public int PaginationPageSize
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt32(GetXmlProperty("SearchPaginationPageSize", 50), 0);
			}
			set
			{
                if (value != PaginationPageSize)
                {
                    SetXmlProperty("SearchPaginationPageSize", value);
                    this.DoNotSetUpdatedFieldsOnNextUpdate = true;
                    try
                    {
                        this.Update();
                    }
                    catch (Security.SecurityException)
                    {

                    }
                }				    
            }
		}

        public bool DisplayInformationPanel
        {
            get
            {
                return FWBS.Common.ConvertDef.ToBoolean(GetXmlProperty("DisplayInformationPanel", true), true);
            }
            set
            {
                if (value != DisplayInformationPanel)
                {
                    SetXmlProperty("DisplayInformationPanel", value);
                    this.DoNotSetUpdatedFieldsOnNextUpdate = true;
                    try
                    {
                        this.Update();
                    }
                    catch (Security.SecurityException)
                    {

                    }
                }
            }
        }

        public bool ExpandNavigationPanel
        {
            get
            {
                return FWBS.Common.ConvertDef.ToBoolean(GetXmlProperty("ExpandNavigationPanel", true), true);
            }
            set
            {
                if (value != ExpandNavigationPanel)
                {
                    SetXmlProperty("ExpandNavigationPanel", value);
                    this.DoNotSetUpdatedFieldsOnNextUpdate = true;
                    try
                    {
                        this.Update();
                    }
                    catch (Security.SecurityException)
                    {

                    }
                }
            }
        }

        #endregion

        #region Preferences

        #region Email Preferences


        ///<summary>
        /// Gets or Sets a flag that indicates whether the user automatically saves email attachments.
        /// </summary>
        [EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public TriState AutoSaveAttachments
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrAutoSaveAttachments", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrAutoSaveAttachments", value);
			}
		}
		
		/// <summary>
		/// Gets or Sets whether the a saved email is automatically deleted or moved after saved.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public string SavedEmailOption
		{
			get
			{
				/* M = Move
				 * D = Delete
				 * L = Leave
				 */
				return Convert.ToString(GetXmlProperty("usrSavedEmailOption", "")).ToUpper();
			}
			set
			{
				SetXmlProperty("usrSavedEmailOption", value.ToUpper());
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public string SavedSentEmailOption
		{
			get
			{
				/* M = Move
				 * D = Delete
				 * L = Leave
				 */
				return Convert.ToString(GetXmlProperty("usrSavedSentEmailOption", "")).ToUpper();
			}
			set
			{
				SetXmlProperty("usrSavedSentEmailOption", value.ToUpper());
			}
		}


		/// <summary>
		/// Gets or Sets the folder to move a newly saved email to, if the auto save operation is set to move.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public string SavedEmailFolderLocation
		{
			get
			{
				return Convert.ToString(GetXmlProperty("usrSavedEmailFolderLocation", @""));
			}
			set
			{
				SetXmlProperty("usrSavedEmailFolderLocation", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public TriState EmailResolveAddress
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrEmailResolveAddress", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrEmailResolveAddress", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public TriState EmailQuickSave
		{
			get
			{
				return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrEmailQuickSave", ""), TriState.Null);
			}
			set
			{
				SetXmlProperty("usrEmailQuickSave", value);
			}
		}

		#endregion

		#region Email Profiling

		/// <summary>
		/// Gets or Sets the email profiling level.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public string EmailProfileLevel
		{
			get
			{
				/* S = Strict
				 * M = Medium
				 * D = Default
				 * C = Custom
				 * BLANK = System Fallback
				 */
				return Convert.ToString(GetXmlProperty("usrEmailProfileLevel", "")).ToUpper();
			}
			set
			{
				if (value != EmailProfileLevel)
				{
					SetXmlProperty("usrEmailProfileLevel", value.ToUpper());
					switch(value)
					{
						case "S":
							SetXmlProperty("usrEmailProfileOnClose", true);
							SetXmlProperty("usrEmailProfileOnNew", true);
							SetXmlProperty("usrEmailProfileOnReply", true);
							SetXmlProperty("usrEmailProfileOnForward", true);
							SetXmlProperty("usrEmailProfileOnDelete", true);
							SetXmlProperty("usrEmailProfileOnMove", true);
							SetXmlProperty("usrEmailProfileAllowEdit", false);
							break;
						case "M":
							SetXmlProperty("usrEmailProfileOnClose", false);
							SetXmlProperty("usrEmailProfileOnNew", true);
							SetXmlProperty("usrEmailProfileOnReply", true);
							SetXmlProperty("usrEmailProfileOnForward", true);
							SetXmlProperty("usrEmailProfileOnDelete", false);
							SetXmlProperty("usrEmailProfileOnMove", true);
							SetXmlProperty("usrEmailProfileAllowEdit", false);
							break;
						case "D":
							SetXmlProperty("usrEmailProfileOnClose", "");
							SetXmlProperty("usrEmailProfileOnNew", "");
							SetXmlProperty("usrEmailProfileOnReply", "");
							SetXmlProperty("usrEmailProfileOnForward", "");
							SetXmlProperty("usrEmailProfileOnDelete", "");
							SetXmlProperty("usrEmailProfileOnMove", "");
							SetXmlProperty("usrEmailProfileAllowEdit", "");
							break;
						case "":
							break;
					}

					if (value != "")
					{
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnClose", EmailProfileOnClose));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnMove", EmailProfileOnMove));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnNew", EmailProfileOnNew));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnReply", EmailProfileOnReply));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnForward", EmailProfileOnForward));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnDelete", EmailProfileOnDelete));
						OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileAllowEdit", EmailProfileAllowEdit));
					}
				}
			}
		}

		private void EmailProfileLevelResolve()
		{
			string val = "";
			string old = EmailProfileLevel;

			if (old != "")
			{
				if (EmailProfileOnClose && EmailProfileOnDelete && EmailProfileOnForward && EmailProfileOnNew && EmailProfileOnReply && EmailProfileOnMove && EmailProfileAllowEdit == false)
				{
					val = "S";
				}
				else if(EmailProfileOnClose == false && EmailProfileOnDelete == false && EmailProfileOnForward && EmailProfileOnNew && EmailProfileOnReply && EmailProfileOnMove && EmailProfileAllowEdit == false)
				{
					val = "M";
				}
				else if (EmailProfileOnClose == false && EmailProfileOnDelete == false && EmailProfileOnForward == false && EmailProfileOnNew == false && EmailProfileOnReply == false && EmailProfileOnMove == false && EmailProfileAllowEdit)
				{
					val = "D";
				}
				else
				{
					val = "C";
				}
			}

			if (old != val)
			{
				SetXmlProperty("usrEmailProfileLevel", val.ToUpper());
				OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileLevel", old, val));
			}

		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public bool EmailProfileOnClose
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnClose", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnClose", value);
				EmailProfileLevelResolve();
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public bool EmailProfileOnMove
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnMove", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnMove", value);
				EmailProfileLevelResolve();
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("EMAIL")]
		public bool EmailProfileOnNew
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnNew", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnNew", value);
				EmailProfileLevelResolve();
			}
		}


		[EnquiryUsage(true)]	
		[LocCategory("EMAIL")]
		public bool EmailProfileOnReply
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnReply", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnReply", value);
				EmailProfileLevelResolve();
			}
		}


		[EnquiryUsage(true)]	
		[LocCategory("EMAIL")]
		public bool EmailProfileOnForward
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnForward", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnForward", value);
				EmailProfileLevelResolve();
			}
		}


		[EnquiryUsage(true)]	
		[LocCategory("EMAIL")]
		public bool EmailProfileOnDelete
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileOnDelete", "false"), false);
			}
			set
			{
				SetXmlProperty("usrEmailProfileOnDelete", value);
				EmailProfileLevelResolve();
			}
		}

		[EnquiryUsage(true)]	
		[LocCategory("EMAIL")]
		public bool EmailProfileAllowEdit
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrEmailProfileAllowEdit", "true"), true);
			}
			set
			{
				SetXmlProperty("usrEmailProfileAllowEdit", value);
				EmailProfileLevelResolve();
			}
		}


		#endregion

		#region Default Overrides        

		/// <summary>
		/// The Action types that are displayed in the Context menu within TaskFlow
		/// Valid values are 0 = FileAndTask, 1 = FileOnly, 2= TaskOnly, 4 = None
		/// </summary>
		[EnquiryUsage(true)]
		public int ActionsInContextMenu
		{
			get
			{
				return ConvDef.ToInt32(GetXmlProperty("ActionsInContextMenu", string.Empty), 0);
			}
			set
			{
				if (ActionsInContextMenu != value)
				{
					if (!Enum.IsDefined(typeof(FileManagement.ActionsToDisplay), value))
						throw new Exception(string.Format("{0} is not a valid value for ActionsInContextMenu", Convert.ToString(value)));
					SetXmlProperty("ActionsInContextMenu", value);
				}
			}
		}

		/// <summary>
		/// The order that the available Actions are displayed in the Context menu within TaskFlow
		/// Valid values are 0= FileActions1st, 1 = TaskActions1st
		/// </summary>
		[EnquiryUsage(true)]
		public int ActionsOrderContextMenu
		{
			get
			{
				return ConvDef.ToInt32(GetXmlProperty("ActionsOrderContextMenu", string.Empty),0);
			}
			set
			{
				if (ActionsOrderContextMenu != value)
				{
					if (!Enum.IsDefined(typeof(FileManagement.ActionsOrderType), value))
						throw new Exception(string.Format("{0} is not a valid value for ActionsOrderContextMenu", Convert.ToString(value)));
					SetXmlProperty("ActionsOrderContextMenu", value);
				}
			}
		}

		[EnquiryUsage(true)]
		public string TasksAddinCommandCentreSearchListOverride
		{
			get
			{
				string val = string.Empty;
				if (DefaultSystemSearchLists.TryGetValue("SCHMYTASKS", out val))
					return string.IsNullOrEmpty(val) ? string.Empty : val.Trim();
				else
					return string.Empty;
			}
			set
			{
				if (TasksAddinCommandCentreSearchListOverride != value)
				{
					if (DefaultSystemSearchLists.ContainsKey("SCHMYTASKS"))
						DefaultSystemSearchLists["SCHMYTASKS"] = value;
					else
						DefaultSystemSearchLists.Add("SCHMYTASKS", value);
				}
			}
		}
		

		public IDictionary<string, string> DefaultSystemForms
		{
			get
			{
				return xmlprops.DefaultSystemForms;
			}
		}

		public IDictionary<string, string> DefaultSystemSearchLists
		{
			get
			{
				return xmlprops.DefaultSystemSearchLists;
			}
		}

		public IDictionary<string, string> DefaultSystemSearchListGroups
		{
			get
			{
				return xmlprops.DefaultSystemSearchListGroups;
			}
		}
		
		#endregion

		/// <summary>
		/// Gets the UI culture string ID (en-GB) for the user.  This will only be accessed by the
		/// Session class and not outside.  This is because the session object will determine
		/// the currently used UICulture setting by checking whether one exists on the
		/// branch, reginfo or user.
		/// </summary>
		[LocCategory("LANGUAGE")]
		public string PreferedCulture
		{
			get
			{
				if (GetExtraInfo("usrUICultureInfo") is DBNull)
					return "";
				else
				{
					try
					{
						string culture = Convert.ToString(GetExtraInfo("usrUICultureInfo"));
						System.Globalization.CultureInfo.CreateSpecificCulture(culture);
						return culture;
					}
					catch
					{
						return "";
					}
				}
			}
			set
			{
				if (value == "")
					SetExtraInfo("usrInits", DBNull.Value);
				else
					SetExtraInfo("usrInits", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState HideAllWelcomeWizardPages
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrHideAllWelcomeWizardPages", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrHideAllWelcomeWizardPages", value);
			}
		}


		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState HideCancellationConfirmationDialog
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrHideCancellationConfirmationDialog", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrHideCancellationConfirmationDialog", value);
			}
		}


		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState SpellingAndGrammarCheckRequired
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrSpellingAndGrammarCheckRequired", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrSpellingAndGrammarCheckRequired", value);
			}
		}


		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public string DisableStopCodeCheck
		{
			get
			{
				return Convert.ToString(GetXmlProperty("DisableStopCodeCheck", false));
			}
			set
			{
				SetXmlProperty("DisableStopCodeCheck", value);
			}
		}





		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public string CompareTool
		{
			get
			{
				return Convert.ToString(GetXmlProperty("CompareTool", null));
			}
			set
			{
				SetXmlProperty("CompareTool", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState PromptDocAlreadyOpen
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrPromptDocAlreadyOpen", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrPromptDocAlreadyOpen", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public int PromptDocAlreadyOpenTime
		{
			get
			{
					return FWBS.Common.ConvertDef.ToInt32(GetXmlProperty("usrPromptDocAlreadyOpenTime", 0), 0);
			}
			set
			{
				SetXmlProperty("usrPromptDocAlreadyOpenTime", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Welcome Wizard flag of the user.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool WelcomeWizard
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("usrwelcomewizard");
				}
				catch
				{
					return true;
				}
			}
			set
			{
				SetExtraInfo("usrwelcomewizard", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Right To Left (RTL) prefered visual state of forms.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("LANGUAGE")]
		public bool RightToLeft
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("usrRTL");
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("usrRTL", value);
			}
		}

		/// <summary>
		/// Gets or Sets the MatterSphere UserType
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("MATTERSPHERE")]
		public string MSUserType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrMSUserType"));
			}
			set
			{
				SetExtraInfo("usrMSUserType", value);
			}
		}


		/// <summary>
		/// Gets or Sets the Information Panel docked location.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public string InformationPanelDockLocation
		{
			get
			{
				return Convert.ToString(GetXmlProperty("InformationPanelDockLocation", Session.CurrentSession.InformationPanelDockLocation));
			}
			set
			{
				SetXmlProperty("InformationPanelDockLocation", value);
			}
		}

        /// <summary>
        /// Gets or Sets the succinct type display form caption value.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public TriState SuccinctTypeDisplayFormCaption
        {
            get
            {
                try
                {
                    return (TriState)ConvertDef.ToEnum(GetXmlProperty("SuccinctTypeDisplayFormCaption", ""), TriState.Null);
                }
                catch
                {
                    return TriState.Null;
                }
            }
            set
            {
                SetXmlProperty("SuccinctTypeDisplayFormCaption", value);
            }
        }

		/// <summary>
		/// Gets or Sets a flag that indicates whether the user has markup automatically displayed
		/// in a word document when opening an already saved one.
		/// </summary>
		[EnquiryUsage(false)]
		[System.ComponentModel.Browsable(false)]
		[LocCategory("OMS")]
		[Obsolete("Track change options no longer controlled by 3E MatterSphere", false)]
		public bool ShowDocumentMarkup
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrShowDocumentMarkup", false), false);
			}
			set
			{
				SetXmlProperty("usrShowDocumentMarkup", value);
			}
		}

		[EnquiryUsage(false)]
		[System.ComponentModel.Browsable(false)]
		[Obsolete("Track change options no longer controlled by 3E MatterSphere", false)]
		[LocCategory("OMS")]
		public TriState EnableTrackChanges
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrEnableTrackChanges", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrEnableTrackChanges", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState EnableTrackChangesWarning
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrEnableTrackChangesWarning", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrEnableTrackChangesWarning", value);
			}
		}


		/// <summary>
		/// Gets or Sets a flag that indicates whether the user uses the Office MRI list.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool ShowMRIList
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrShowMRIList", false), false);
			}
			set
			{
				SetXmlProperty("usrShowMRIList", value);
			}
		}


		/// <summary>
		/// Gets or Sets a the value to Minimise the Window a document was opened from on open.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState MinimiseWindowOnOpen
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("MinimiseWindowOnOpen", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("MinimiseWindowOnOpen", value.ToString());
			}
		}

		/// <summary>
		/// Gets or Sets a flag that indicates whether a document will auto print at the end of a document save.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState AutoPrint
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrAutoPrint", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrAutoPrint", value.ToString());
			}
		}


		/// <summary>
		/// Gets or Sets a flag that indicates whether a document stays open after saving.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool ContinueEditingAfterSave
		{
			get
			{
				try
				{
					return ConvDef.ToBoolean(GetXmlProperty("usrContinueEditingAfterSave", false), false);
				}
				catch
				{
					return false;
				}

			}
			set
			{
				SetXmlProperty("usrContinueEditingAfterSave", value);
			}
		}

		/// <summary>
		/// Gets or Sets a flag that indicates whether a the default associate is always used, when picking an associate.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState UseDefaultAssociate
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrUseDefaultAssociate", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}

			}
			set
			{
				SetXmlProperty("usrUseDefaultAssociate", value.ToString());
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool WorksForMatterHandler
		{
			get
			{
				return ConvDef.ToBoolean(GetXmlProperty("WorksForMatterHandler", false), false);
			}
			set
			{
				SetXmlProperty("WorksForMatterHandler", value);
			}
		}


		/// <summary>
		/// Gets or Sets a flag that indicates whether command centre will be displayed at logon.		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool DisplayCommandCentreAtLogon
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrDisplayCommandCentreAtLogon", false), false);
			}
			set
			{
				SetXmlProperty("usrDisplayCommandCentreAtLogon", value);
			}
		}

		/// <summary>
		/// Gets or Sets a flag that indicates whether a prompt appears to write a new letter after saving one previously.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public bool AutoWriteLetter
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrAutoWriteLetter", false), false);
			}
			set
			{
				SetXmlProperty("usrAutoWriteLetter", value);
			}
		}
		
		/// <summary>
		/// Gets the main precedent category that the user will use.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("PRECEDENT")]
		[Lookup("PRECATEGORY")]
		public virtual string DefaultPrecedentCategory
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrpreccat"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("usrpreccat",DBNull.Value);
				else
					SetExtraInfo("usrpreccat",value);
			}
		}

		/// <summary>
		/// Gets the default precedent sub category that the user uses.
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("PRECEDENT")]
		[Lookup("PRECSUBCAT")]
		public virtual string DefaultPrecedentSubCategory
		{
			get
			{
				return Convert.ToString(GetExtraInfo("usrprecsubcat"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("usrprecsubcat",DBNull.Value);
				else
					SetExtraInfo("usrprecsubcat",value);
			}		
		}

        /// <summary>
        /// Gets the default precedent sub category that the user uses.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("PRECEDENT")]
        [Lookup("PRECMINORCAT")]
        public virtual string DefaultPrecedentMinorCategory
        {
            get
            {
                return Convert.ToString(GetExtraInfo("usrprecminorcat"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("usrprecminorcat", DBNull.Value);
                else
                    SetExtraInfo("usrprecminorcat", value);
            }
        }

        /// <summary>
        /// Gets or Sets whether the language list is displayed on the precedent library.
        /// </summary>
        [LocCategory("PRECEDENT")]
		[Lookup("PRECLANGOPT")]
		[EnquiryUsage(true)]
		public bool ShowPrecLanguageOption
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrPrecLangOpt", false), false);
			}
			set
			{
				SetXmlProperty("usrPrecLangOpt", value);
			}
		}

		/// <summary>
		/// Gets or Sets whether the library list is displayed on the precedent library.
		/// </summary>
		[LocCategory("PRECEDENT")]
		[Lookup("PRECLIBOPT")]
		[EnquiryUsage(true)]
		public bool ShowPrecLibraryOption
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrPrecLibOpt", false), false);
			}
			set
			{
				SetXmlProperty("usrPrecLibOpt", value);
			}
		}

		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		[System.ComponentModel.DefaultValue(true)]
		public bool PromptLocalChangedDocuments
		{
			get
			{
				return ConvertDef.ToBoolean(GetXmlProperty("usrPromptLocalChangedDocuments", "true"), true);
			}
			set
			{
				SetXmlProperty("usrPromptLocalChangedDocuments", value);
			}
		}


		[EnquiryUsage(true)]
		[LocCategory("OMS")]
		public TriState NotifyOfOpeningUnsupportedFiletype
		{
			get
			{
				try
				{
					return (TriState)FWBS.Common.ConvertDef.ToEnum(GetXmlProperty("usrNotifyOfOpeningUnsupportedFiletype", ""), TriState.Null);
				}
				catch
				{
					return TriState.Null;
				}
			}
			set
			{
				SetXmlProperty("usrNotifyOfOpeningUnsupportedFiletype", value);
			}
		}



		#endregion

		#region Methods

		/// <summary>
		/// Is the User in a series of Roles
		/// </summary>
		/// <param name="codes">String array of Roles to check against</param>
		/// <returns>True if in Role</returns>
		public bool IsInRoles(string [] codes)
		{
			if ((codes == null) || (codes.Length == 1 && codes[0] == ""))
				return true;

			System.Collections.Generic.List<Security.Permissions.Permission> permlist = new System.Collections.Generic.List<FWBS.OMS.Security.Permissions.Permission>(codes.Length);

			foreach (string code in codes)
			{
				if (String.IsNullOrEmpty(code))
					continue;

				permlist.Add(new Security.Permissions.SystemPermission(code));
			}

			return Security.SecurityManager.CurrentManager.IsGranted(permlist.ToArray(), FWBS.OMS.Security.PermissionComparison.Or);
		}

		/// <summary>
		/// is the User in a specific Role
		/// </summary>
		/// <param name="codes">string of Code</param>
		/// <returns>True if in Role</returns>
		public bool IsInRoles(string codes)
		{
			if (codes == null)
				codes = String.Empty;

			return IsInRoles(codes.Split(','));
		}

		public void ValidateRoles(string [] codes)
		{
			if (codes == null)
				return;

			System.Collections.Generic.List<Security.Permissions.Permission> permlist = new System.Collections.Generic.List<FWBS.OMS.Security.Permissions.Permission>(codes.Length);

			foreach (string code in codes)
			{
				if (String.IsNullOrEmpty(code))
					continue;

				permlist.Add(new Security.Permissions.SystemPermission(code));
			}

			Security.SecurityManager.CurrentManager.CheckPermission(permlist.ToArray(), FWBS.OMS.Security.PermissionComparison.Or);
		}

		
		public void ValidateRoles(string codes)
		{
			if (codes == null)
				codes = String.Empty;

			ValidateRoles(codes.Split(','));
		}

		/// <summary>
		/// Gets an overload of the system form.
		/// </summary>
		public string GetSystemForm (SystemForms form)
		{
			string systemForm = GetSystemForm(form.ToString());
			if (string.IsNullOrEmpty(systemForm))
			{
				switch (form)
				{
					case SystemForms.SaveDocumentWizard:
						systemForm = DefaultSaveWizard;
						if (string.IsNullOrEmpty(systemForm))
							systemForm = string.Empty;
						break;
				}
			}


			return systemForm;


		}

		public string GetSystemForm(string form)
		{
			string path = String.Format("/config/defaultForms/{0}", form);

			BuildXML();

			if (xmlprops.Xml.DocumentElement.SelectNodes(path).Count > 0)
				return xmlprops.Xml.DocumentElement.SelectSingleNode(path).InnerText;

			switch (form.ToLower())
			{
				case "defaultsavewizard":
					{
						string userForm = DefaultSaveWizard;
						if (string.IsNullOrEmpty(userForm))
							userForm = string.Empty;
						return userForm;
					}
			}

			return String.Empty;
		}

		/// <summary>
		/// Gets an overload of the system lists.
		/// </summary>
		public string GetSystemSearchLists (SystemSearchLists lists)
		{
			return GetSystemForm(lists.ToString());
		}

		public string GetSystemSearchLists (string lists)
		{
			string path = String.Format("/config/defaultSearchLists/{0}", lists);

			BuildXML();

			if (xmlprops.Xml.DocumentElement.SelectNodes(path).Count > 0)
			{
				return xmlprops.Xml.DocumentElement.SelectSingleNode(path).InnerText;
			}
			else
			{
				return String.Empty;
			}
			
		}

		/// <summary>
		/// What date is numdays (Number of Days) for this user including Holidays that is registered for this user.
		/// </summary>
		/// <param name="startDate">Startdate to calculate from</param>
		/// <param name="noOfdays">Number of days Positive or Negative to Add to the Startdate including Business Days and Holidays being taken into account</param>
		/// <returns>Date</returns>
		public DateTime AddBusinessDays(DateTime startDate, int noOfdays)
		{
			// Use the Database functionality to run a Function to check the days
			Session.CurrentSession.CheckLoggedIn();
			object retval;
			string sql = "SELECT dbo.GetNextBusinessDate ( @startDate , @noDays , @usrID ,@UI) ";
			retval = Session.CurrentSession.Connection.ExecuteSQLScalar(sql, new IDataParameter[4] { Session.CurrentSession.Connection.AddParameter("usrID", SqlDbType.Int, 0, this.ID), Session.CurrentSession.Connection.AddParameter("noDays", SqlDbType.Int, 0, noOfdays), Session.CurrentSession.Connection.AddParameter("startDate", SqlDbType.DateTime, 0, startDate), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name) });
			if (retval is DBNull || retval == null)
				throw new NullReferenceException("Unable to get Business Days Function");
			else
				return (DateTime)retval;

		}

		/// <summary>
		/// How many business days are there between start and end date including the Holidays that are for the User and the Company Holidays.
		/// </summary>
		/// <param name="startDate">Startdate to calculate from</param>
		/// <param name="endDate">endDate to calculate to</param>
		/// <returns>Number of days</returns>
		public int NoOfBusinessDays(DateTime startDate, DateTime endDate)
		{
			// Use the Database functionality to run a Function to check the days
			Session.CurrentSession.CheckLoggedIn();
			object retval;
			string sql = "SELECT dbo.GetNoOfBusinessDays ( @startDate , @endDate , @usrID ,@UI) ";
			retval = Session.CurrentSession.Connection.ExecuteSQLScalar(sql, new IDataParameter[4] { Session.CurrentSession.Connection.AddParameter("usrID", SqlDbType.Int, 0, this.ID), Session.CurrentSession.Connection.AddParameter("endDate", SqlDbType.DateTime, 0, endDate), Session.CurrentSession.Connection.AddParameter("startDate", SqlDbType.DateTime, 0, startDate), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name) });
			if (retval is DBNull || retval == null)
				throw new NullReferenceException("Unable to get Business Days Function");
			else
				return (int)retval;

		}



		/// <summary>
		/// Gets an overload of the system  grouped lists.
		/// </summary>
		public string GetSystemSearchListGroups (SystemSearchListGroups group)
		{
			return GetSystemSearchListGroups(group.ToString());
		}

		public string GetSystemSearchListGroups (string group)
		{
			string path = String.Format("/config/defaultSearchListsGroups/{0}", group);

			BuildXML();

			if (xmlprops.Xml.DocumentElement.SelectNodes(path).Count > 0)
			{
				return xmlprops.Xml.DocumentElement.SelectSingleNode(path).InnerText;
			}
			else
			{
				return String.Empty;
			}
		}

		/// <summary>
		/// Allows the user to change their password by passing the old and new one through.
		/// An exception is raised if the password change fails.
		/// </summary>
		/// <param name="oldPassword">Old password to validate against.</param>
		/// <param name="newPassword">New password to set.</param>
        /// <param name="confirmPassword"></param>
		[EnquiryUsage(true)]
		public void ChangePassword(string oldPassword, string newPassword,  string confirmPassword)
		{
			if (oldPassword == "" && newPassword == "" && confirmPassword == "") return;
			if (oldPassword == FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(GetExtraInfo("usrPassword").ToString()))
			{
				if (newPassword == confirmPassword)
				{
					if (newPassword == "")
						SetExtraInfo("usrPassword", "");
					else
						SetExtraInfo("usrPassword", FWBS.Common.Security.Cryptography.Encryption.NewKeyEncrypt(newPassword, 14));
				}
				else
					throw new Security.ConfirmationPasswordDiffersException();
			}
			else
				throw new Security.OldPasswordDiffersException();
		}


		/// <summary>
		/// Resets the password of the user.
		/// </summary>
		[EnquiryUsage(true)]
		public void ResetPassword()
		{
			//TODO: Security Audit.
			SetExtraInfo("usrPassword", "");
		}

		/// <summary>
		/// GetWorkingData returns to the Webservice
		/// </summary>
		/// <returns></returns>
		public DataSet GetWorkingData()
		{
			IDataParameter[] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("USRID", System.Data.SqlDbType.BigInt, 0, Session.CurrentSession.CurrentFeeEarner.ID);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("TABLE",System.Data.SqlDbType.Int,0,0);

			try
			{
				return  Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprWorkRecord", new string[3]{"CLIENTLIST","FILELIST","TIMEACTIVITIES"}, paramlist);
			}
			catch (Exception ex)
			{
				throw new OMSException2("1002","Error Getting Working Data", ex);
			}
		}
		
		/// <summary>
		/// GetWorkingDataOverride allowing specify individual tables returns to the Webservice
		/// </summary>
		/// <returns></returns>
		public DataSet GetWorkingData(int tableNumber)
		{
			IDataParameter[] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("USRID", System.Data.SqlDbType.BigInt, 0, Session.CurrentSession.CurrentFeeEarner.ID);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("TABLE",System.Data.SqlDbType.Int,0,tableNumber);
			try
			{
				switch(tableNumber)
				{
					case 1: //Client List
						return  Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprWorkRecord", new string[1]{"CLIENTLIST"}, paramlist);
					case 2: // file list
						return  Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprWorkRecord", new string[1]{"FILELIST"}, paramlist);
					case 3: // time activities
						return  Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprWorkRecord", new string[1]{"TIMEACTIVITIES"}, paramlist);
					default: // all three
						return  Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprWorkRecord", new string[3]{"CLIENTLIST","FILELIST","TIMEACTIVITIES"}, paramlist);
				}
			}
			catch (Exception ex)
			{
				throw new OMSException2("1002","Error Getting Working Data", ex);
			}
		}


		/// <summary>
		/// Checks to see if the users password can be changed before actually changing it.  If it can be
		/// changed then follow it up using the ChangePassword method.
		/// </summary>
		/// <param name="oldPassword">Old password to validate against.</param>
		/// <param name="newPassword">New password to set.</param>
		/// <returns>Returns true if the password can be changed.</returns>
		public bool CanChangePassword(string oldPassword, string newPassword)
		{
			if (oldPassword == FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(GetExtraInfo("usrPassword").ToString()))
				return true;
			else
				return false;
		}


		/// <summary>
		/// Makes the user inactive from using the system.  The next time the user logs in
		/// a message shall appear to say that they can no longer log in.
		/// </summary>
		[EnquiryUsage(true)]
		public void MakeInactive()
		{
			SetExtraInfo("usrActive", false);
		}


		/// <summary>
		/// Resets the failed login attempts back to zero.
		/// </summary>
		public void ResetFailedAttempts()
		{
			SetExtraInfo("usrFailed", 0);
		}


		/// <summary>
		/// Validates the users entered password with the one stored in the database.  For security reasons
		/// the passed pasword is blanked out as it is passed by reference.
		/// </summary>
		/// <param name="password">Password entered from the UI side.</param>
		/// <returns>True if the password matches.</returns>
		public bool ValidatePassword(ref string password)
		{
			bool ret = false;
			if (FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(GetExtraInfo("usrPassword").ToString()) == password)
				ret = true;
			password = "";
			return ret;
		}

		/// <summary>
		/// Returns a string representation of the user object which in this case is the user's fullname.
		/// </summary>
		/// <returns>The users full name.</returns>
		public override string ToString()
		{
			return this.FullName;
		}

		
		/// <summary>
		/// Upgrades the user to a fee earner
		/// </summary>
		/// <returns>The newly created fee earner object</returns>
		public FeeEarner UpgradeToFeeEarner()
		{
			try
			{
				FeeEarner fe = null;
				string sql = "insert into dbfeeearner(feeusrid) values(@usrID)";

				//get the user ID
				int usrID = this.ID;

				//insert a record into fee earner table with this ID
				Session.CurrentSession.Connection.ExecuteSQL(sql,new IDataParameter[1]{Session.CurrentSession.Connection.AddParameter("usrID", SqlDbType.Int, 0, usrID)});

				//retrive the newly created fee earner and set active
				fe = new FeeEarner(usrID);
				fe.BaseCurrency = fe.Currency;
				fe.SetExtraInfo("feeActive",true);
				fe.Update();
				//return the fee earner
				return fe;
			}
			catch(Exception ex)
			{
				string s = ex.Message;
				int i = s.IndexOf("Violation of PRIMARY KEY constraint"); 
				if(i >= 0)
					throw new OMSException2("USERNOUPGRD","User not upgraded as already a Fee Earner.", "", ex);
				else
					throw ex;
			}
			
		}

		/// <summary>
		/// Downgrade the user to a fee earner
		/// </summary>
		/// <returns>The newly created fee earner object</returns>
		public void DowngradeToUser()
		{
			string sql = "delete from dbfeeearner where feeusrid = @usrID";

			//get the user ID
			int usrID = this.ID;

			//insert a record into fee earner table with this ID
			Session.CurrentSession.Connection.ExecuteSQL(sql,new IDataParameter[1]{Session.CurrentSession.Connection.AddParameter("usrID", SqlDbType.Int, 0, usrID)});

			Session.CurrentSession.CurrentFeeEarners.Clear();

		}
		#endregion

		#region XML Settings Methods

		/// <summary>
		/// XML type configuration settings.
		/// </summary>
		private XmlProperties  xmlprops = null;

		private void BuildXML()
		{
			if (xmlprops == null)
				xmlprops = new XmlProperties(this, "usrXml");
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

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public virtual void SetExtraInfo (string fieldName, object val)
		{
			if (fieldName.ToUpper() == "USRWORKSFOR")
			{
				if (Convert.ToInt32(_user.Rows[0]["usrID"]) == Session.CurrentSession.CurrentUser.ID)
				{
					if (val != GetExtraInfo("USRWORKSFOR"))
						Session.CurrentSession.CurrentFeeEarner = null;
				}
			}

			if (fieldName.ToUpper() == "BRID")
			{
				object origbranch = GetExtraInfo("BRID");

				if (origbranch == DBNull.Value || Convert.ToInt32(origbranch) == Session.CurrentSession.CurrentBranch.ID)
				{
					if (val != origbranch)
						Session.CurrentSession.CurrentBranch = null;
				}
			}

            this.SetExtraInfo(_user.Rows[0], fieldName, val);
        }

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{		
			object val = _user.Rows[0][fieldName];
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
				return _user.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("1001","Error Getting Extra Info Field " + fieldName + " Probably Not Initialized");
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
			return _user.Copy();
		}

		#endregion

		#region IUpdateable Implementation

		private bool IsAdminstrator()
		{
			var roleslist = this.Roles.Split(',').ToList();
			return roleslist.Contains("ADMIN");
		}

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public virtual void Update()
		{
            ObjectState state = State;
            if (this.OnExtCreatingUpdatingOrDeleting(state))
                return;

			//Reset the 'Do Not Set Updated' flag as early as possible. This is defensive measure to avoid any issue during the update process which may leave the flag switched on.
			bool skipUpdatedFields = this.DoNotSetUpdatedFieldsOnNextUpdate;
			this.DoNotSetUpdatedFieldsOnNextUpdate = false;

            if (this.ID == Session.CurrentSession.CurrentUser.ID)
                Session.CurrentSession.ClearCurrentPowerUserSettings();
            var power = Session.CurrentSession.CurrentPowerUserSettings;
            if (power.IsConfigured)
			{
				if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN") == false && Session.CurrentSession.CurrentUser.IsInRoles("POWER") == true)
				{
					if (this.IsAdminstrator())
						throw new Security.SecurityException("POWERFAIL3", "You don't have permission to update an administrator.");
				}
			}
			DataRow row = _user.Rows[0];
			bool isnew = IsNew;

			if (!isnew)
			{
				if (Convert.ToString(_user.Rows[0]["usrcurISOCode", DataRowVersion.Original]) != Convert.ToString(_user.Rows[0]["usrcurISOCode", DataRowVersion.Current]))
				{
					try
					{
						FeeEarner f = FeeEarner.GetFeeEarner(this.ID);
						if (f != null)
						{
							//ripple the change to the fee earner if it applies
							f.BaseCurrency = this.Currency;
							f.Update();
						}
					}
					catch (OMSException)
					{ }
				}
			}

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (IsDirty)
			{
				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (_user.PrimaryKey == null || _user.PrimaryKey.Length == 0)
					_user.PrimaryKey = new DataColumn[1]{_user.Columns["usrid"]};

				//Do not set the Updated fields if we have just logged in (as nothing of relevance has changed )
				//Introduced for the new User/FeeEarner cache refresh functionality
				if (!skipUpdatedFields)
				{   
					SetExtraInfo("UpdatedBy", Session.CurrentSession._currentUser.ID);
					SetExtraInfo("Updated", DateTime.Now);
				}
				if (xmlprops != null) xmlprops.Update();

				Session.CurrentSession.Connection.Update(row, "dbuser");
				_isdirty = false;
			}




			if (isnew)
			{
				if (this.GetType() == typeof(User))
					Session.CurrentSession.CurrentUsers.Add(ID.ToString(), this);

				IDataParameter[] pars = new IDataParameter[5];
				pars[0] = Session.CurrentSession.Connection.CreateParameter("userID", this.ID);
				pars[1] = Session.CurrentSession.Connection.CreateParameter("ntLogin", this.ActiveDirectoryID);
				pars[2] = Session.CurrentSession.Connection.CreateParameter("name", this.FullName);
				pars[3] = Session.CurrentSession.Connection.CreateParameter("adminUserID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID);
				pars[4] = Session.CurrentSession.Connection.CreateParameter("active", this.IsActive);
				Session.CurrentSession.Connection.ExecuteProcedure("config.AddUser", pars);
			}


			//Update all the extended data objects, if any.
			if (_extData != null)
			{
				foreach (FWBS.OMS.ExtendedData ext in _extData)
				{
					ext.UpdateExtendedData();
				}
			}

			//If the user being updated is the same as the CurrentUser, update the CurrentUserLastUpdated flag.
			if (this.ID == Session.CurrentSession.CurrentUser.ID)
			{
				Session.CurrentSession.CurrentUserLastUpdated = ConvertDef.ToDateTimeNULL(this.GetExtraInfo("UPDATED"), DateTime.UtcNow);
				//TODO - check this
				Session.CurrentSession.CurrentFeeEarnerLastUpdated = ConvertDef.ToDateTimeNULL(Session.CurrentSession.CurrentFeeEarner.GetExtraInfo("UPDATED"), System.DateTime.UtcNow);
				System.Diagnostics.Debug.WriteLine(string.Format("_currentFeeEarnerLastUpdated : {0}", Session.CurrentSession.CurrentFeeEarnerLastUpdated));
				Session.CurrentSession.CurrentFeeEarnerUserLastUpdated = ConvertDef.ToDateTimeNULL(FWBS.OMS.User.GetUser(Session.CurrentSession.CurrentFeeEarner.ID).GetExtraInfo("UPDATED"), System.DateTime.UtcNow);
				System.Diagnostics.Debug.WriteLine(string.Format("_currentFeeEarnerUserLastUpdated : {0}", Session.CurrentSession.CurrentFeeEarnerUserLastUpdated));
			}

            this.OnExtCreatedUpdatedDeleted(state);
		}


		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public virtual void Refresh()
		{
			Refresh(false);
		}

        /// <summary>
        /// Gets the changes of the current object and and refreshes the object
        /// then reapplies the changes to avoid any concurrency issues.  This is in 
        /// theory forcing any changes made to the object.
        /// </summary>
        /// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
        public virtual void Refresh(bool applyChanges)
        {
            if (IsNew)
                return;

            this.OnExtRefreshing();

            DataTable changes = _user.GetChanges();

            xmlprops = null;

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, changes.Rows[0]);
            else
                Fetch(this.ID, null);

            if (this == Session.CurrentSession.CurrentUser && Session.CurrentSession.SingleDatabaseInstance)
            {
                if (this.Branch.ID != Session.CurrentSession.CurrentBranch.ID)
                {
                    Session.CurrentSession.CurrentBranch = this.Branch;
                }
            }

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
		public virtual void Cancel()
		{
			xmlprops = null;
			_user.RejectChanges();

			if (_extData != null)
			{
				foreach (FWBS.OMS.ExtendedData ext in _extData)
				{
					ext.CancelExtendedData();
				}
			}
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		public virtual bool IsDirty
		{
			get
			{
				return (_isdirty || _user.GetChanges() != null);
			}
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
		/// Edits the current user object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public virtual Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.UserAdmin), param);
		}

		/// <summary>
		/// Edits the current user object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public virtual Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry (customForm, Parent, this, param);
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
				return null;
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

				if (_user != null)
				{
					_user.Dispose();
					_user = null;
				}

				if (_script != null)
				{
					_script.Dispose();
					_script = null;
				}

			}
			
			//Dispose unmanaged objects.
		}

		#endregion

		#region Static Methods

		public static DataTable GetUsers()
		{
			Session.CurrentSession.CheckLoggedIn();
			String sql2;
			sql2 = Sql + " WHERE NOT usrID Between -101 AND -1"; 
			return Session.CurrentSession.Connection.ExecuteSQLTable(sql2,"USERS",false,new IDataParameter[0]);
		}

			
		/// <summary>
		/// Returns a user object based on the user ID passed.
		/// </summary>
		/// <param name="userID">Specified user id.</param>
        /// <param name="DefaultTab"></param>
		/// <returns>Returns null if the user does not exist.</returns>
		static public User GetUser(int userID, string DefaultTab)
		{
			Session.CurrentSession.CheckLoggedIn();

			if (Session.CurrentSession.CurrentUser.ID == userID)
				return Session.CurrentSession.CurrentUser;
			else
			{
				User cu = Session.CurrentSession.CurrentUsers[userID.ToString()] as User;
				if (cu == null)
				{
					cu = new User(userID);
					cu.DefaultTab = DefaultTab;
				}

				return cu;
			}
		}

		static public User GetUser(int userID)
		{
			return GetUser(userID,null);
		}
		
		/// <summary>
		/// Gets the user by the NT authenticated active directory name.
		/// </summary>
		static public User GetUserByADName(string userName)
		{
			userName = userName ?? String.Empty;


			User cu = null;

			if (Session.CurrentSession.CurrentUser.ActiveDirectoryID.ToUpperInvariant() == userName.ToUpperInvariant())
				cu = Session.CurrentSession.CurrentUser;
			else
			{
				foreach (User u in Session.CurrentSession.CurrentUsers.Values)
				{

					if (u.ActiveDirectoryID.ToUpperInvariant() == userName.ToUpperInvariant())
					{
						cu = u;
						break;
					}
				}
			}

			if (cu == null)
			{
				cu = new User("ADID", userName);
			}

			return cu;
			
		}
			
		/// <summary>
		/// Returns the users full name based on a specified user id.
		/// </summary>
		/// <param name="usrID">Specified user id.</param>
        /// <param name="defaultValue"></param>
		/// <returns>Users full name.</returns>
		static public string GetUserFullName(int usrID, string defaultValue)
		{
			Session.CurrentSession.CheckLoggedIn();
			object name;
			string sql = "select usrFullName from dbuser where usrID = @usrID";
			name = Session.CurrentSession.Connection.ExecuteSQLScalar(sql, new IDataParameter[1]{Session.CurrentSession.Connection.AddParameter("usrID", SqlDbType.Int, 0, usrID)});
			if (name is DBNull || name == null)
				return defaultValue;
			else
				return (string)name;
		}

		static public User GetUser(string initials)
		{
			Session.CurrentSession.CheckLoggedIn();

			if (String.IsNullOrEmpty(initials))
				throw new ArgumentNullException("initials");

			User cu = null;
			
			if (Session.CurrentSession.CurrentUser.Initials.ToUpperInvariant() == initials.ToUpperInvariant())
				cu = Session.CurrentSession.CurrentUser;
			else
			{
				foreach (User u in Session.CurrentSession.CurrentUsers.Values)
				{
					if (u.Initials.ToUpperInvariant() == initials.ToUpperInvariant())
					{
						cu = u;
						break;
					}
				}
			}

			if (cu == null)
			{
				cu = new User("OMS", initials);
			}

			return cu;
		}

		static public string GetUserFullName(int usrID)
		{
			return GetUserFullName(usrID, "");
		}



		/// <summary>
		/// Authenticates a user with the specified user initals and password.
		/// If correct then the user object is returned.
        /// </summary>
		static public User AuthenticateUser(string userName, string password)
		{
			User usr = GetUser(userName);

			if (usr.UserTypeCode == "SERVICE")
				throw new Security.ServiceUserException(userName);

			//Check to see if the user is active.
			if (!usr.IsActive) 
				throw new Security.InactiveOMSUserException(userName);
		
			//Validate that the password is correct.
			if (!usr.ValidatePassword(ref password))
			{
				throw new Security.InvalidOMSPasswordException();
			}
			
			return usr;
		}

		#endregion

		#region IOMSType Implementation

		/// <summary>
		/// Gets an OMS Type based on the user type off this current instance of a user object.
		/// </summary>
		/// <returns>A OMSType with information needed to represented this type of user.</returns>
		public virtual OMSType GetOMSType()
		{
			return FWBS.OMS.UserType.GetUserType(UserTypeCode);
		}


		/// <summary>
		/// Gets the value to link to many potential connector type object.
		/// </summary>
		public object LinkValue
		{
			get
			{
				return ID;
			}
		}

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
		

		public bool? AutoPrintByApplication(Guid ApplicationGuid)
		{
			string setting = Convert.ToString(GetXmlProperty("AP" + ApplicationGuid.ToString(), ""));
			if (setting == "") 
				return null;
			else
				return ConvertDef.ToBoolean(setting, true);
		}

		public void SetAutoPrintByApplication(Guid ApplicationGuid, bool? value)
		{
			if (value == null)
				SetXmlProperty("AP" + ApplicationGuid.ToString(), "");
			else
				SetXmlProperty("AP" + ApplicationGuid.ToString(), value.ToString());
		}

		public static DataTable ListAutoPrintByApplication()
		{
			return ListAutoPrintByApplication(null);
		}

		public static DataTable ListAutoPrintByApplication(User user)
		{
			if (user == null) user = Session.CurrentSession.CurrentUser;
			DataTable table;
			table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT appGUID, appName FROM dbApplication", "AUTOPRINT", null);
			foreach (DataRow row in table.Rows)
			{
				bool? value = user.AutoPrintByApplication(new Guid(Convert.ToString(row["appGUID"])));
				if (value == null)
					row["appName"] = "[  ] " + Convert.ToString(row["appName"]);
				else if (value == true)
					row["appName"] = "[O] " + Convert.ToString(row["appName"]);
				else if (value == false)
					row["appName"] = "[X] " + Convert.ToString(row["appName"]);
			}
			return table;
		}

		public void SuppressADEShortcuts(DataTable dtRoles)
		{
			DataView roleview = new DataView(dtRoles, "", "", DataViewRowState.CurrentRows);

			DataTable dtFavourites = BuildUserFavouritesTable(this.ID, "usrID");

			foreach(DataRowView drv in roleview)
			{
				DataRow [] rowstodelete = dtFavourites.Select("usrFavObjParam3 = '" + drv["cdcode"].ToString() + "'");
				if(rowstodelete.Length > 0)
				{
					for(int i = 0; i < rowstodelete.Length; i++)
					{
						RemoveUserFavourite(Convert.ToInt32(rowstodelete[i]["FavID"]), "favID");
					}
				}
			}
		}

		private DataTable BuildUserFavouritesTable(int usrID, string paramName)
		{
			string sql = @"select * from dbUserFavourites where usrID = @usrID";
			return ExecuteSQLQuery(sql, paramName, usrID);
		}

		private void RemoveUserFavourite(int favID, string paramName)
		{
			string sql = @"delete from dbUserFavourites where FavID = @favID";
			ExecuteSQLQuery(sql, paramName, favID);
		}

		private DataTable ExecuteSQLQuery(string sqlQuery, string paramName, int paramValue)
		{
			IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
			List<IDataParameter> parList = new List<IDataParameter>();
			parList.Add(connection.CreateParameter(paramName, paramValue));
			System.Data.DataTable dt = connection.ExecuteSQL(sqlQuery, parList);
			return dt;
		}
		
		/// <summary>
		/// Flag to skip the Updated and UpdatedBy fields on next User Update
		/// </summary>
		internal bool DoNotSetUpdatedFieldsOnNextUpdate
		{
			get { return _doNotSetUpdatedFieldsOnNextUpdate; }
			set { _doNotSetUpdatedFieldsOnNextUpdate = value; }
		}


        public void AddRole(string role)
        {
            var userroles = new HashSet<string>(Convert.ToString(GetXmlProperty("Roles", null)).Split(','));
            userroles.Add(role);
            SetXmlProperty("Roles", string.Join(",", userroles));
        }

        public void RemoveRole(string role)
        {
            var userroles = Convert.ToString(GetXmlProperty("Roles", null)).Split(',').ToList();
            userroles.Remove(role);
            SetXmlProperty("Roles", string.Join(",", userroles));
        }

		#region IExtendedDataCompatible Implementation
		/// <summary>
		/// Gets the extended data list indexer which will expose
		/// each of the extended data objects on the particular object.
		/// </summary>
		public virtual ExtendedDataList ExtendedData 
		{
			get
			{
				if (_extData == null)
				{
					//Use the contact type configuration to initialise the extended data objects.
					UserType t = CurrentUserType;
					string [] codes = new string [t.ExtData.Count];
					int ctr = 0;
					foreach(OMSType.ExtendedData ext in t.ExtData)
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


		#region IOMSType Members
		public void SetCurrentSessions()
		{
			
		}
		#endregion
	}

}
