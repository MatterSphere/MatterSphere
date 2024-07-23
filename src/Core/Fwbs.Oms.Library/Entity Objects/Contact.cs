using System;
using System.Data;
using System.Threading;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;


namespace FWBS.OMS
{
    using FWBS.OMS.Security;
    using FWBS.OMS.Security.Permissions;

    /// <summary>
    /// A Contact object that holds all information about people / companies names, numbers
    /// and addresses.  This contact object can be used with the enquiry engine.  This object
    /// is an OMS configurable type that can appear different to other types of contacts.
    /// </summary>
    /// <remarks></remarks>
    [Security.SecurableType("CONTACT")]
	public class Contact :  IOMSType, IDisposable, IExtendedDataCompatible, Security.ISecurable, IAlert
	{		
		#region Fields
        /// <summary>
        /// Override IsDirty
        /// </summary>
        private bool _isdirty = false;

        /// <summary>
        /// Sets the Default Tab when called from the WinUI Layer
        /// </summary>
		private string _defaultTab = null;

        /// <summary>
        /// Internal data source.
        /// </summary>
		private DataSet _contact = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string Sql = "select * from dbContact";
        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string SqlAddressLink = "select * from dbContactAddresses";
        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string SqlNumberLink = "select * from dbContactnumbers";
        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string SqlEmailLink = "select * from dbcontactemails";


        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
		public const string Table = "CONTACT";
        /// <summary>
        /// Table name for the multi addresses table.
        /// </summary>
		public const string Table_Addresses = "ADDRESSES";
        /// <summary>
        /// Table name for the multi numbers table.
        /// </summary>
		public const string Table_Numbers = "NUMBERS";
        /// <summary>
        /// Table name for the multi emails table.
        /// </summary>
		public const string Table_Emails = "EMAILS";

        /// <summary>
        /// Company extended data constant.
        /// </summary>
		public const string Ext_Company = "EXTCONTCOMP";

        /// <summary>
        /// Individual extended data constant.
        /// </summary>
		public const string Ext_Individual = "EXTCONTINDIV";

        /// <summary>
        /// Holds the different extended data sources for the contact.
        /// </summary>
		private ExtendedDataList _extData = null;

        /// <summary>
        /// Holds a reference to the default address.
        /// </summary>
		private Address _defAddress = null;

        /// <summary>
        /// A variable that will be used to associate the contact as a specific type when added to a new file.
        /// </summary>
		private string _assocas = "";

        /// <summary>
        /// A variable that will be used to associate the contact with a theirref value.
        /// </summary>
		private string _assoctheirref = "";

		#endregion

		#region Constructors

        /// <summary>
        /// Names the tables within the data set.
        /// </summary>
        /// <remarks></remarks>
		private void NameTables()
		{
			// Rename the default tables, must be kept in the same order
			_contact.Tables[1].TableName = Table_Addresses;
			_contact.Tables[2].TableName = Table_Numbers;
			_contact.Tables[3].TableName = Table_Emails;
		}

        /// <summary>
        /// Creates a new contact object.  This routine is used by the enquiry engine
        /// to create new contact object.
        /// </summary>
        /// <remarks></remarks>
		internal Contact()
		{
			//Make sure that the parameters list is cleared after use.	
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("ContID", System.Data.SqlDbType.BigInt, 0, 0);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			_contact = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprContactRecord", false, new string[1] {Table}, paramlist);
            _contact.Tables[Table].Columns["contid"].AutoIncrement = true;

			// Rename the default tables, must be kept in the same order
			NameTables();

			DataTable dt = _contact.Tables[Table];
			
			//Add a new record.
			Global.CreateBlankRecord(ref dt, true);

			//Set the created by and created date of the item.
			SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Created", DateTime.Now);
			SetExtraInfo("contguid", System.Guid.NewGuid());

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }

        /// <summary>
        /// Creates a contact with a specified contact type.
        /// </summary>
        /// <param name="contType">Type of the cont.</param>
        /// <remarks></remarks>
		public Contact (ContactType contType) : this()
		{
			Session.CurrentSession.CheckLoggedIn();
			SetExtraInfo("conttypecode", contType.Code);
		}

        /// <summary>
        /// Initialised an existing contact object with the specified identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Contact (long id)
		{
			Fetch(id, null);
			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentContacts.Add(ID.ToString(), this);

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }

        /// <summary>
        /// Constructs a contact object.
        /// </summary>
        /// <param name="id">The contact id to retrieve.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
		private void Fetch (long id, DataRow merge)
		{
			_defAddress = null;

			//Make sure that the parameters list is cleared after use.	
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("ContID", System.Data.SqlDbType.BigInt, 0, id);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			DataSet data = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprContactRecord", new string[1] {Table}, paramlist);

            if ((data == null) || (data.Tables.Count == 0) || (data.Tables[Table].Rows.Count == 0))
			{
				throw new OMSException(HelpIndexes.ContactNotFound,  id.ToString());
			}

            if (merge != null)
                Global.Merge(data.Tables[Table].Rows[0], merge);

            _contact = data;

            timestamp = DateTime.UtcNow;

			// Rename the default tables, must be kept in the same order
			NameTables();

            //Refresh the security
            SecurityManager.CurrentManager.Refresh(this);

            new ContactPermission(this, StandardPermissionType.List).Check();

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
                    switch (_contact.Tables[Table].Rows[0].RowState)
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
        /// Gets a value indicating whether the contact object is new and needs to be
        /// updated to exist in the database.
        /// </summary>
        /// <remarks></remarks>
		public bool IsNew
		{
			get
			{
				try
				{
					return (_contact.Tables[Table].Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

        /// <summary>
        /// Gets the unique contact identifer.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public long ID
		{
			get
			{
				return Convert.ToInt64(GetExtraInfo("contid"));
			}
		}

        /// <summary>
        /// Gets or Sets a denormalised database flag to determine whether the contact is currently a client or not.
        /// </summary>
        /// <value><c>true</c> if this instance is client; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		internal bool IsClient
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("contIsClient"));
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("contIsClient", value);
			}
		}

        /// <summary>
        /// Gets or Sets whether the contact is Marketable or not.
        /// </summary>
        /// <value><c>true</c> if this instance is marketable; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public bool IsMarketable
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("contMarketable"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    SetExtraInfo("contMarketable", value);
                }
                catch { }
            }
        }


        /// <summary>
        /// Gets the Proof of ID.
        /// </summary>
        /// <value>The proofof I d1.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public object ProofofID1	
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CONTIDENT"));
			}
			set
			{
				SetExtraInfo("CONTIDENT",value);
			}
		}

        /// <summary>
        /// Gets the Proof of ID.
        /// </summary>
        /// <value>The proofof I d2.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public object ProofofID2	
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CONTIDENT2"));
			}
			set
			{
				SetExtraInfo("CONTIDENT2",value);
			}
		}

        /// <summary>
        /// Gets a decriptive representation of the contact.
        /// </summary>
        /// <remarks></remarks>
		public string ContactDescription
		{
			get
			{
				System.Text.StringBuilder s = new System.Text.StringBuilder();
				s.Append (Name);
				s.Append(Environment.NewLine);
				s.Append(DefaultAddress.GetAddressString(Environment.NewLine));
				return s.ToString();
			}
		}

        /// <summary>
        /// Gets the group code of the contact.
        /// </summary>
        /// <value>The group.</value>
        /// <remarks></remarks>
		[EnquiryEngine.EnquiryUsage(true)]
		public string Group
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contGroup"));
			}
			set
			{
				if (value == String.Empty)
					SetExtraInfo("contGroup", DBNull.Value);
				else
					SetExtraInfo("contGroup", value);

			}
		}

        /// <summary>
        /// Gets the Preferred Contact Method of the contact.
        /// </summary>
        /// <value>The preferred contact method.</value>
        /// <remarks></remarks>
        [EnquiryEngine.EnquiryUsage(true)]
        public string PreferredContactMethod
        {
            get
            {
                return Convert.ToString(GetExtraInfo("contPrefContactMethod"));
            }
            set
            {
                if (value == String.Empty)
                    SetExtraInfo("contPrefContactMethod", DBNull.Value);
                else
                    SetExtraInfo("contPrefContactMethod", value);

            }
        }

        /// <summary>
        /// Gets the source code of the contact.
        /// </summary>
        /// <remarks></remarks>
		public string SourceOfBusiness
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contSource"));
			}
		}

        /// <summary>
        /// Gets the contact type of the contact.  It is this value that indicates
        /// what information will be displayed for each different contact type.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ContactTypeCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("conttypecode"));
			}
		}


        /// <summary>
        /// Gets or Sets the contacts full name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Name
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contname"));
			}
			set
			{
				SetExtraInfo("contname", value);
			}
		}

        /// <summary>
        /// Gets or Sets the default salutation used when addressing the contact.
        /// </summary>
        /// <value>The salutation.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Salutation
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contsalut")); 
			}
			set
			{
			    var salutation = value;
			    if (salutation != null && salutation.Length > 50)
			    {
                    salutation = salutation.Substring(0, 50);
			    }

				SetExtraInfo("contsalut", salutation);
			}
		}

        /// <summary>
        /// Gets or Sets the addressee text when addressing the contact.
        /// </summary>
        /// <value>The addressee.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Addressee
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contaddressee")); 
			}
			set
			{
				SetExtraInfo("contaddressee", value);
			}
		}

        /// <summary>
        /// Gets the additional filter of the contact type.
        /// </summary>
        /// <value>The additional filter.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string AdditionalFilter
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contaddfilter"));
			}
			set
			{
				SetExtraInfo("contaddfilter", value);
			}
		}

        /// <summary>
        /// Gets or Sets any additional information on the contact.
        /// </summary>
        /// <value>The additional information.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string AdditionalInformation
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contaddinfo"));
			}
			set
			{
				SetExtraInfo("contaddinfo", value);
			}
		}

        /// <summary>
        /// Gets or Sets the extra notes used to explain additional information on the contact.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Notes
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contNotes")); 
			}
			set
			{
				SetExtraInfo("contNotes", value);
			}
		}

        /// <summary>
        /// Gets a flag that specifies whether the contact has been approved as a
        /// reliable expert is to use.
        /// </summary>
        /// <value><c>true</c> if approved; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool Approved
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("contApproved")); 
				}
				catch
				{
					return true;
				}
			}
			set
			{
				SetExtraInfo("contApproved", value);
			}
		}

        /// <summary>
        /// Gets a date and time of when the approval flag has been taken away.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ApprovalRevokedOn
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contApprRevokedOn"));
			}
		}

        /// <summary>
        /// Gets or Sets a flag that specifies whether the contact is on the companies christmas
        /// card mail list.
        /// </summary>
        /// <value><c>true</c> if [X mas card]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool XMasCard
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("contXmasCard")); 
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("contXmasCard", value);
			}
		}

        /// <summary>
        /// Gets or Sets the website address of the contact.
        /// </summary>
        /// <value>The website.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Website
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contWebsite")); 
			}
			set
			{
				SetExtraInfo("contWebsite", value);
			}
		}


        /// <summary>
        /// Gets the modification dates and users.
        /// </summary>
        /// <remarks></remarks>
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
        /// Gets or Sets the grade rating of the contact.
        /// </summary>
        /// <value>The grade.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public byte Grade
		{
			get
			{
				try
				{
					return Convert.ToByte(GetExtraInfo("ContGrade"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("ContGrade", value);
			}
		}

        /// <summary>
        /// Gets or Sets the precedent library of the contact when it is source of business.
        /// </summary>
        /// <value>The precedent library.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string PrecedentLibrary
		{
			get
			{
				return Convert.ToString(GetExtraInfo("contpreclibrary"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("contpreclibrary", DBNull.Value);
				else
					SetExtraInfo("contpreclibrary", value);
			}
		}

        /// <summary>
        /// Gets the contact type object associated to the contact.
        /// </summary>
        /// <remarks></remarks>
		public ContactType CurrentContactType
		{
			get
			{
				return (ContactType)GetOMSType();
			}
		}

        /// <summary>
        /// A memory variable that will be used to automatically associate the contact as a specified type
        /// when added to a new OMS file.
        /// </summary>
        /// <value>The associate as.</value>
        /// <remarks></remarks>
		public string AssociateAs
		{
			get
			{
				return _assocas;
			}
			set
			{
				_assocas = value;
			}
		}

        /// <summary>
        /// A memory variable that will be used to automatically set the reference for the contact when added to a new OMS file.
        /// </summary>
        /// <value>The associate their ref.</value>
        /// <remarks></remarks>
		public string AssociateTheirRef
		{
			get
			{
				return _assoctheirref;
			}
			set
			{
				_assoctheirref = value;
			}
		}

        /// <summary>
        /// The user account related to this contact
        /// </summary>
        /// <value>The user.</value>
        /// <remarks></remarks>
        public User User
        {
            get
            {
                if (_contact.Tables[Table].Columns.Contains("userID"))
                {
                    int userID = ConvertDef.ToInt32(_contact.Tables[Table].Rows[0]["userID"], 0);
                    if (userID == 0)
                        return null;

                    return User.GetUser(userID);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (_contact.Tables[Table].Columns.Contains("userID"))
                {
                    if (value != null)
                        _contact.Tables[Table].Rows[0]["userID"] = value.ID;
                    else
                        _contact.Tables[Table].Rows[0]["userID"] = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Has this contact got a related User Account
        /// </summary>
        /// <remarks></remarks>
        public bool HasUserAccount
        {
            get
            {                
                return (User != null);
            }
        }


		#endregion

		#region Individual
        /// <summary>
        /// Gets or sets the individual title.
        /// </summary>
        /// <value>The individual title.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualTitle
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contTitle"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contTitle",value);
			}
		}

        /// <summary>
        /// Gets or sets the individual initials.
        /// </summary>
        /// <value>The individual initials.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualInitials
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contInitials"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contInitials",value);
			}
		}

        /// <summary>
        /// Gets or sets the individual surname.
        /// </summary>
        /// <value>The individual surname.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualSurname
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contSurname"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contSurname",value);
			}
		}

        /// <summary>
        /// Gets or sets the individual christian names.
        /// </summary>
        /// <value>The individual christian names.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualChristianNames
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contChristianNames"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contChristianNames",value);
			}
		}

        /// <summary>
        /// Gets or sets the individual occupation.
        /// </summary>
        /// <value>The individual occupation.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualOccupation
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contOccupation"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contOccupation",value);
			}
		}

        /// <summary>
        /// Gets or sets the individual DOB.
        /// </summary>
        /// <value>The individual DOB.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public DateTimeNULL IndividualDOB
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(ExtendedData["EXTCONTINDIV"].GetExtendedData("contDOB"),DBNull.Value);
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contDOB",value.ToObject());
			}
		}

        /// <summary>
        /// Gets or sets the individual DOD.
        /// </summary>
        /// <value>The individual DOD.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public DateTimeNULL IndividualDOD
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(ExtendedData["EXTCONTINDIV"].GetExtendedData("contDOD"),DBNull.Value);
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contDOD",value.ToObject());
			}
		}

        /// <summary>
        /// Gets or sets the individual placeof birth.
        /// </summary>
        /// <value>The individual placeof birth.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string IndividualPlaceofBirth
		{
			get
			{
				return Convert.ToString(ExtendedData["EXTCONTINDIV"].GetExtendedData("contPOB"));
			}
			set
			{
				ExtendedData["EXTCONTINDIV"].SetExtendedData("contPOB",value);
			}
		}
		#endregion

		#region Email Specific Methods
        /// <summary>
        /// Gets or Sets the contacts default email address.
        /// </summary>
        /// <value>The default email.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultEmail
		{
			get
			{
				return GetEmail("", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				
				DataView em = GetEmails();
				//returned view may already have a part filter
				string filter = em.RowFilter;
				
				if(filter == "")
                    em.RowFilter = "contemail = '" + value.Replace("'", "''") + "'";
				else
					em.RowFilter = filter + " and contemail = '" + value.Replace("'", "''") + "'";

				if(em.Count == 0)
					AddEmail(value, "", 0);
				else
                    AddDefaultEmail(value, Convert.ToString(em[0]["contcode"]), 0);
			}
		}

        /// <summary>
        /// Gets or Sets the contacts default home email address.
        /// </summary>
        /// <value>The default home email.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultHomeEmail
		{
			get
			{
				return GetEmail("HOME", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddEmail(value, "HOME", 0);
			}
		}

        /// <summary>
        /// Gets or Sets the contacts default work email address.
        /// </summary>
        /// <value>The default work email.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultWorkEmail
		{
			get
			{
				return GetEmail("WORK", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddEmail(value, "WORK", 0);
			}
		}

        /// <summary>
        /// Gets a list of email addresses.
        /// </summary>
        /// <returns>A data view of the numbers.</returns>
        /// <remarks></remarks>
		public DataView GetEmails()
		{
			return GetEmails("", false);
		}


        /// <summary>
        /// Gets a list of email addresses.
        /// </summary>
        /// <param name="location">The location of the email.</param>
        /// <returns>A data view of the numbers.</returns>
        /// <remarks></remarks>
		public DataView GetEmails(string location)
		{
			return GetEmails(location, false);
		}


        /// <summary>
        /// Gets the contacts data view of the client.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public DataView GetEmails(string location, bool all)
		{
			string filter = "";
			if (location == null || location == "")
				filter = "";
			else
				filter = "contcode = '" + location.Replace("'", "''") + "'";

			if (all == false) 
			{
				if (filter != "") filter += " and ";
				filter+= "contactive = true";
			}

			//Filter out any inactive contacts and sort by a prefered order.

            if (location == null || location == "")
            {
                DataView vw = new DataView(_contact.Tables[Table_Emails]);
                vw.RowFilter = filter;
                vw.Sort = "contDefaultOrder";
                return vw;
            }
            else
            {
                DataView vw = new DataView(_contact.Tables[Table_Emails]);
                vw.RowFilter = filter;
                vw.Sort = "contorder";
                return vw;
            }
		}

        /// <summary>
        /// Gets an email address in the multi contact emails table..
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="prefered">The prefered order of the type of email.</param>
        /// <returns>An email address.</returns>
        /// <remarks></remarks>
		public string GetEmail(string location, int prefered)
		{
			DataView email = GetEmails(location, false);

			if (prefered < 0) prefered = 0;
			if (email.Count < (prefered + 1))
			{
				prefered = email.Count - 1;
			}

			if (email.Count > 0)
			{
				return Convert.ToString(email[prefered]["contemail"]);
			}
			else
			{
				return String.Empty;
			}
				
		}


        /// <summary>
        /// Checks to see if the contact has a specified email address.
        /// </summary>
        /// <param name="email">An email address to check for.</param>
        /// <param name="location">The location.</param>
        /// <param name="activeOnly">Only active will be checked.</param>
        /// <returns>A flag indicating if an email exists or not.</returns>
        /// <remarks></remarks>
		public bool HasEmail(string email, string location, bool activeOnly = false)
		{
			if (email == null) return false;
			DataView em = GetEmails("", true);
			string filter = "contemail = '" + email.Replace("'", "''") + "'";
			if (location != "")
				filter += " and contcode = '" + location.Replace("'", "''") + "'";
            if (activeOnly)
                filter += " and contactive = true";
			em.RowFilter = filter;
			if (em.Count > 0)
				return true;
			else
				return false;
		}

        /// <summary>
        /// Removes an email from the contact.
        /// </summary>
        /// <param name="email">The email to remove from the contact.</param>
        /// <param name="location">The location.</param>
        /// <returns>True if the address has been successfully removed.</returns>
        /// <remarks></remarks>
		public bool RemoveEmail(string email, string location)
		{
			if (email == null) return false;
			
			bool exists = HasEmail(email, location);

			if (exists)
			{
				Common.KeyValueCollection pars = new KeyValueCollection();
				pars.Add("EMAIL", email);
				SearchEngine.SearchList sch = new SearchEngine.SearchList(Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.ContactInfoUsageCheck), this, pars);
				sch.Search(false);
				if (sch.ResultCount > 0)
				{
					Session.CurrentSession.OnShowSearch(new ShowSearchEventArgs(sch, Session.CurrentSession.Resources.GetMessage("CONTADDUSED", "The contact '%1%' cannot remove the address '%2%'.  It is associated to the following %FILES%...", "", true, Name, email).Text));
                    sch.Search(false);
                    if (sch.ResultCount > 0)
                        return false;
				}
				
				DataView vw = GetEmails("", true);
				string filter = "contemail = '" + Common.SQLRoutines.RemoveRubbish(email) + "'";
				if (location != "")
					filter += " and contcode = '" + Common.SQLRoutines.RemoveRubbish(location) + "'";

				vw.RowFilter = filter;

				//DMB 15/6/2004 added a fine tune to delete if not already assigned
				foreach (DataRowView r in vw)
				{
					if(Convert.ToInt32(r["contid"]) == 0)
						r.Delete();
					else
                        r["contactive"] = false;
				}
			}

			return true;
		}

        /// <summary>
        /// Changes the number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        /// <param name="newnumber">The newnumber.</param>
        /// <param name="newtype">The newtype.</param>
        /// <param name="newlocation">The newlocation.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ChangeNumber(string number, string type, string location, string newnumber, string newtype, string newlocation)
        {
            try
            {
                if (String.IsNullOrEmpty(number)) return false;
                if (String.IsNullOrEmpty(type)) return false;
                if (String.IsNullOrEmpty(location)) return false;
                if (String.IsNullOrEmpty(newnumber)) return false;
                if (String.IsNullOrEmpty(newtype)) return false;
                if (String.IsNullOrEmpty(newlocation)) return false;
                if (newnumber.ToLowerInvariant() == number.ToLowerInvariant() && newlocation.ToLowerInvariant() == location.ToLowerInvariant()) return false;
                DataView vw = GetNumbers(type, location, false);
                vw.RowFilter += " and contNumber = '" + number.Replace("'", "''") + "'";
                if (vw.Count == 0) return false;
                DataRow row = vw[0].Row;

                vw = GetNumbers(newtype, newlocation, true);
                vw.RowFilter += " and contNumber = '" + newnumber.Replace("'", "''") + "'";
                DataRow exists = null;
                if (vw.Count > 0)
                    exists = vw[0].Row;

                // If the New Number exists but is inactive then restore and mark the other inactive
                if (exists != null)
                {
                    exists["contactive"] = true;
                    row["contactive"] = false;
                }
                else
                {
                    row["contNumber"] = newnumber;
                    row["DisplayNumber"] = CodeLookup.GetLookup("INFOTYPE", newlocation) + ": " + newnumber;
                    row["contextracode"] = newlocation;
                    row["contCode"] = newtype;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.EditEmail()");
                return false;
            }
        }

        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="location">The location.</param>
        /// <param name="newemail">The newemail.</param>
        /// <param name="newlocation">The newlocation.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ChangeEmail(string email, string location, string newemail, string newlocation)
        {
            try
            {
                if (String.IsNullOrEmpty(email)) return false;
                if (String.IsNullOrEmpty(location)) return false;
                if (String.IsNullOrEmpty(newemail)) return false;
                if (String.IsNullOrEmpty(newlocation)) return false;
                if (newemail.ToLowerInvariant() == email.ToLowerInvariant() && newlocation.ToLowerInvariant() == location.ToLowerInvariant()) return false;
                DataView vw = GetEmails(location,false);
                vw.RowFilter += " and contEmail = '" + email.Replace("'", "''") + "'";
                if (vw.Count == 0) return false;
                DataRow row = vw[0].Row;

                vw = GetEmails(newlocation, true);
                vw.RowFilter += " and contEmail = '" + newemail.Replace("'", "''") + "'";
                DataRow exists = null;
                if (vw.Count > 0)
                    exists = vw[0].Row;

                // If the New Email address exists but is inactive then restore and mark the other inactive
                if (exists != null)
                {
                    exists["contactive"] = true;
                    row["contactive"] = false;
                }
                else
                {
                    row["contemail"] = newemail;
                    row["contCode"] = newlocation;
                    row["EmailTypeDesc"] = newlocation;
                    row["DisplayEmail"] = CodeLookup.GetLookup("INFOTYPE", newlocation) + ": " + newemail;
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.EditEmail()");
                return false;
            }
        }

        /// <summary>
        /// This Method will add a email to a contact.
        /// </summary>
        /// <param name="email">The email address to be added.</param>
        /// <param name="location">The type of address (HOME, WORK etc...)</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddEmail(string email, string location)
		{
			return AddEmail(email, location, GetEmails().Count);
		}

        public bool AddDefaultEmail(string email, string location)
        {
            return AddDefaultEmail(email, location, GetEmails().Count);
        }


        /// <summary>
        /// This Method will add an email to a contact.
        /// </summary>
        /// <param name="email">The email address to be added.</param>
        /// <param name="location">The type of address (HOME, WORK etc...)</param>
        /// <param name="priority">The priority of the address to be inserted.</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddEmail(string email, string location, int priority)
		{
			//Last Modified: DMB 10/2/2004
			try
			{
			
				if (email == "") return false;

				if (location == "") location = "HOME";

				bool exists = HasEmail(email, location);
				DataView vw = GetEmails();
				
				//Dont let a priority value go higher than the address count.
				if ((priority + 1) > vw.Count) priority = vw.Count;
				//Dont let the priority be lower than zero.
				if (priority < 0) priority = 0;


				string filter = "contorder >= '" + priority + "' and contactive = true";
				
				if (exists)
				{
					filter += " and contemail <> '" + email.Replace("'", "''") + "'";
					if (location != "")
						filter += " and contcode = '" + location.Replace("'", "''") + "'";
				}
				
				vw.RowFilter = filter;
	
				int ctr = 0;
				foreach (DataRowView row in vw)
				{
					ctr++;
					unchecked 
					{
						row["CONTORDER"] = (byte)(priority + ctr);                      
					}
				}

				// 
				//##DMB If the address already exists then update its order.
				if (exists)
				{
					DataView existing = GetEmails("", true);
					filter = "contemail = '" + email.Replace("'", "''") + "'";
					if (location != "")
						filter += " and contcode = '" +  location.Replace("'", "''") + "'";
					existing.RowFilter = filter;
					
					for (int ex = 0; ex < existing.Count; ex++)
					{
						existing[ex]["contorder"] = priority;                       
						existing[ex]["contactive"] = true;
					}
				}

				//##Add the address if it does not exist.
				if (!exists)
				{
					// Add Row to Addresses Table
					DataRow dr = _contact.Tables[Table_Emails].NewRow();
					if (location == "") location = "HOME";
					dr["CONTID"] = this.ID;
					dr["CONTEMAIL"] = email;
					dr["CONTCODE"] = location;
					dr["CONTORDER"] = priority;
                    dr["contDefaultOrder"] = priority;
					dr["CONTACTIVE"] = true;
					dr["DisplayEmail"] = CodeLookup.GetLookup("INFOTYPE",location) + ": " + email;
					_contact.Tables[Table_Emails].Rows.Add(dr);
				}

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.AddEmail()");
				return false;
			}
			
		}

        public bool AddDefaultEmail(string email, string location, int priority)
        {
            //copied from AddEmail and removed the location field as defaultemail doesn't need any location BP 24/03/2014
            try
            {

                if (email == "") return false;

                if (location == "") location = "HOME";

               bool exists = HasEmail(email, "");

                DataView vw = GetEmails();

                //Dont let a priority value go higher than the address count.
                if ((priority + 1) > vw.Count) priority = vw.Count;
                //Dont let the priority be lower than zero.
                if (priority < 0) priority = 0;

                string filter = "contDefaultOrder >= '" + priority + "' and contactive = true";
               
                if (exists)
                {
                    filter += " and contemail <> '" + email.Replace("'", "''") + "'";
                }

                vw.RowFilter = filter;

                int ctr = 0;
                foreach (DataRowView row in vw)
                {
                    ctr++;
                    unchecked
                    {
                        row["contDefaultOrder"] = (byte)(priority + ctr);                      
                    }
                }

                // 
                //##DMB If the address already exists then update its order.
                if (exists)
                {
                    DataView existing = GetEmails("", true);
                    filter = "contemail = '" + email.Replace("'", "''") + "'";                   
                    existing.RowFilter = filter;

                    for (int ex = 0; ex < existing.Count; ex++)
                    {
                        existing[ex]["contDefaultOrder"] = priority;
                        if (location.Equals(existing[ex]["contcode"]))
                            existing[ex]["contactive"] = true;
                    }
                }

                //##Add the address if it does not exist.
                if (!exists)
                {
                    // Add Row to Addresses Table
                    DataRow dr = _contact.Tables[Table_Emails].NewRow();
                    if (location == "") location = "HOME";
                    dr["CONTID"] = this.ID;
                    dr["CONTEMAIL"] = email;
                    dr["CONTCODE"] = location;
                    dr["CONTORDER"] = priority;
                    dr["CONTDEFAULTORDER"] = priority;
                    dr["CONTACTIVE"] = true;
                    dr["DisplayEmail"] = CodeLookup.GetLookup("INFOTYPE", location) + ": " + email;
                    _contact.Tables[Table_Emails].Rows.Add(dr);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.AddDefaultEmail()");
                return false;
            }

        }

		#endregion

		#region Number Specific Methods

        /// <summary>
        /// Gets or Sets the contacts default telephone number.
        /// </summary>
        /// <value>The default telephone number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultTelephoneNumber
		{
			get
			{
				return GetNumber("TELEPHONE", "", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				
				DataView nums = GetNumbers("TELEPHONE");
				//returned view may already have a part filter
				string filter = nums.RowFilter;
				
				if(filter == "")
					nums.RowFilter = "contnumber = '" + value.Replace("'", "''") + "'";
				else
					nums.RowFilter = filter + " and contnumber = '" + value.Replace("'", "''") + "'";
				
				//calls add number which handles if the number already exists and reorders priorities
				if(nums.Count == 0)
					AddNumber(value, "TELEPHONE", "", 0);
				else
					AddDefaultNumber(value, "TELEPHONE", Convert.ToString(nums[0]["contextracode"]), 0);
					
			}
		}


        /// <summary>
        /// Gets  or Sets the contacts second telephone number.  Backward compaitibility.
        /// </summary>
        /// <value>The telephone number2.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string TelephoneNumber2
		{
			get
			{
				return GetNumber("TELEPHONE", "", 1);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddNumber(value, "TELEPHONE", "", 1);
			}
		}


        /// <summary>
        /// Gets or Sets the contacts default home telephone number.
        /// </summary>
        /// <value>The default home telephone number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultHomeTelephoneNumber
		{
			get
			{
				return GetNumber("TELEPHONE", "HOME", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddNumber(value, "TELEPHONE", "HOME", 0);
			}
		}


        /// <summary>
        /// Gets or Sets the contacts default telephone number.
        /// </summary>
        /// <value>The default work telephone number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultWorkTelephoneNumber
		{
			get
			{
				return GetNumber("TELEPHONE", "WORK", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddNumber(value, "TELEPHONE", "WORK", 0);
			}
		}


        /// <summary>
        /// Gets or Sets the contacts default mobile number.
        /// </summary>
        /// <value>The default mobile number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultMobileNumber
		{
			get
			{
				return GetNumber("MOBILE", "", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				
				DataView nums = GetNumbers("MOBILE");
				//returned view may already have a part filter
				string filter = nums.RowFilter;
				
				if(filter == "")
					nums.RowFilter = "contnumber = '" + value.Replace("'", "''") + "'";
				else
					nums.RowFilter = filter + " and contnumber = '" + value.Replace("'", "''") + "'";

				//calls add number which handles if the number already exists and reorders priorities
				if(nums.Count == 0)
					AddNumber(value, "MOBILE", "", 0);
				else
					AddDefaultNumber(value, "MOBILE", Convert.ToString(nums[0]["contextracode"]), 0);
			}
		}


        /// <summary>
        /// Gets or Sets the contacts fax number.
        /// </summary>
        /// <value>The default fax number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultFaxNumber
		{
			get
			{
				return GetNumber("FAX", "", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				
				DataView nums = GetNumbers("FAX");
				//returned view may already have a part filter
				string filter = nums.RowFilter;
				
				if(filter == "")
					nums.RowFilter = "contnumber = '" + value.Replace("'", "''") + "'";
				else
					nums.RowFilter = filter + " and contnumber = '" + value.Replace("'", "''") + "'";

				//calls add number which handles if the number already exists and reorders priorities
				if(nums.Count == 0)
					AddNumber(value, "FAX", "", 0);
				else
					AddDefaultNumber(value, "FAX", Convert.ToString(nums[0]["contextracode"]), 0);
			}
		}

        /// <summary>
        /// Gets or Sets the contacts home fax number.
        /// </summary>
        /// <value>The default home fax number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultHomeFaxNumber
		{
			get
			{
				return GetNumber("FAX", "HOME", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddNumber(value, "FAX", "HOME", 0);
			}
		}


        /// <summary>
        /// Gets or Sets the contacts default work fax number.
        /// </summary>
        /// <value>The default work fax number.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string DefaultWorkFaxNumber
		{
			get
			{
				return GetNumber("FAX", "WORK", 0);
			}
			set
			{
				if (value == null || value == String.Empty) return;
				AddNumber(value, "FAX", "WORK", 0);
			}
		}


        /// <summary>
        /// get a list of active phone numbers
        /// </summary>
        /// <param name="numberType">Tel/Fax/Mobile</param>
        /// <param name="location">EG Home/Work</param>
        /// <returns>Dataview of telephone numbers</returns>
        /// <remarks></remarks>
		public DataView GetNumbers(string numberType, string location)
		{
			return GetNumbers(numberType,location,false);
		}

        /// <summary>
        /// Gets a list of telephone / fax numbers.
        /// </summary>
        /// <param name="numberType">This is type of number like TELEPHONE, FAX etc...</param>
        /// <param name="location">The second type of number part. This might be HOME, WORK etc...</param>
        /// <param name="all">flag to indicate if we should filter out active numbers</param>
        /// <returns>A data view of the numbers.</returns>
        /// <remarks></remarks>
		public DataView GetNumbers(string numberType, string location, bool all)
		{
			string filter = "";

			if (numberType == null || numberType == "")
				filter = "";
			else if (location == null || location == "")
				filter = "contcode = '" + SQLRoutines.RemoveRubbish(numberType) + "'";
			else
				filter = "contcode = '" + SQLRoutines.RemoveRubbish(numberType) + "' and contextracode = '" + SQLRoutines.RemoveRubbish(location) + "'";
			
			//DMB 4/2/2004 added to cope with addition of using active flag
			if (all == false) 
			{
				if (filter != "") filter += " and ";
				filter+= "contactive = true";
			}
            DataView tel;

            if (String.IsNullOrWhiteSpace(location))
            {
                tel = new DataView(_contact.Tables[Table_Numbers], filter, "contDefaultorder", DataViewRowState.CurrentRows);
            }
            else
            {
                 tel = new DataView(_contact.Tables[Table_Numbers], filter, "contorder", DataViewRowState.CurrentRows);
            }

 			return tel;
		}

        /// <summary>
        /// Gets a list of telephone / fax numbers.
        /// </summary>
        /// <param name="numberType">This is type of number like TELEPHONE, FAX etc...</param>
        /// <returns>A data view of the numbers.</returns>
        /// <remarks></remarks>
		public DataView GetNumbers(string numberType)
		{
			return GetNumbers(numberType, "",false);
		}

        /// <summary>
        /// Gets a list of telephone / fax numbers.
        /// </summary>
        /// <returns>A data view of the numbers.</returns>
        /// <remarks></remarks>
		public DataView GetNumbers()
		{
			return GetNumbers("", "",false);
		}


        /// <summary>
        /// Gets a telephone / fax number in the multi contact numbers table..
        /// </summary>
        /// <param name="numberType">This is type of number like TELEPHONE, FAX etc...</param>
        /// <param name="location">The second type of number part. This might be HOME, WORK etc...</param>
        /// <param name="prefered">The prefered order of the type of number.</param>
        /// <returns>A number.</returns>
        /// <remarks></remarks>
		public string GetNumber(string numberType, string location, int prefered)
		{
			DataView tel = GetNumbers(numberType, location,false);

			if (prefered < 0) prefered = 0;
			if (tel.Count < (prefered + 1))
			{
				prefered = tel.Count - 1;
			}

			if (tel.Count > 0)
			{
				return Convert.ToString(tel[prefered]["contnumber"]);
			}
			else
			{
				return String.Empty;
			}
		}


        /// <summary>
        /// Checks to see if the contact has a specified telephone number.
        /// </summary>
        /// <param name="number">An address object to check.</param>
        /// <param name="numberType">The first telephone number type.</param>
        /// <param name="location">The second telephone number type.</param>
        /// <returns>A flag indicating if a number exists or not.</returns>
        /// <remarks></remarks>
		public bool HasNumber(string number, string numberType, string location)
		{
			if (number == null || numberType == null || location == null) return false;

			DataView tel = GetNumbers("", "",true);
			string filter = "";

			if (numberType == "") numberType = "TELEPHONE";

			if (location == "")
				filter = "contnumber = '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "'";
			else
				filter = "contnumber = '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "' and contextracode = '" + location.Replace("'", "''") + "'";

			tel.RowFilter = filter;

			if (tel.Count > 0)
				return true;
			else
				return false;
		}

        /// <summary>
        /// Removes an number from the contact.
        /// </summary>
        /// <param name="numberType">The type of number, FAX, MOBILE, TELEPHONE etc...</param>
        /// <param name="location">The location of the number HOME, WORK etc...</param>
        /// <param name="number">The number to remove from the contact.</param>
        /// <returns>True if the number has been successfully removed.</returns>
        /// <remarks></remarks>
		public bool RemoveNumber(string numberType, string location, string number)
		{
			if (number == null || numberType == null || location == null) return false;
			
			bool exists = HasNumber(number, numberType, location);

			if (exists)
			{

                Common.KeyValueCollection pars = new KeyValueCollection();
                pars.Add(numberType, number);
                SearchEngine.SearchList sch = new SearchEngine.SearchList(Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.ContactInfoUsageCheck), this, pars);
                sch.Search(false);
                if (sch.ResultCount > 0)
                {
                    Session.CurrentSession.OnShowSearch(new ShowSearchEventArgs(sch, Session.CurrentSession.Resources.GetMessage("CONTNUMBERUSED", "The contact '%1%' cannot remove the number '%2%'.  It is associated to the following %FILES%...", "", true, Name, number).Text));
                    sch.Search(false);
                    if (sch.ResultCount > 0)
                        return false;
                }

				DataView vw = GetNumbers("","", true);  //gets all irrespective of active or not
				
				if (numberType == "") numberType = "TELEPHONE";

				string filter = "contnumber = '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "'";
				
				if (location != "")
					filter += " and contextracode = '" + location.Replace("'", "''") + "'";
				
				vw.RowFilter = filter;

				//DMB 15/6/2004 added a fine tune to delete if not already assigned
				foreach (DataRowView r in vw)
				{
					if(Convert.ToInt32(r["contid"]) == 0)
						r.Delete();
					else
                        r["contactive"] = false;
				}
			}

			return true;
        }

        /// <summary>
        /// This Method will add a number to a contact.
        /// </summary>
        /// <param name="number">The Address object to be added</param>
        /// <param name="numberType">The first number type (TELEPHONE, MOBILE, FAX etc...)</param>
        /// <param name="location">The second number type</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddNumber(string number, string numberType, string location)
		{
			return AddNumber(number, numberType, location, _contact.Tables[Table_Numbers].Rows.Count);
		}

        /// <summary>
        /// This Method will add a number to a contact.
        /// </summary>
        /// <param name="number">The Address object to be added</param>
        /// <param name="numberType">The first number type (TELEPHONE, MOBILE, FAX etc...)</param>
        /// <param name="location">The second number type</param>
        /// <param name="priority">The priority of the address to be inserted.</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddNumber(string number, string numberType, string location, int priority)
		{

			try
			{
			
				if (number == null || numberType == null || location== null) return false;
			
				bool exists = HasNumber(number, numberType, location);

				//Dont let a priority value go higher than the address count.
				if ((priority + 1) > _contact.Tables[Table_Numbers].Rows.Count) priority = _contact.Tables[Table_Numbers].Rows.Count;
				//Dont let the priority be lower than zero.
				if (priority < 0) priority = 0;

				DataView vw = GetNumbers();

				if (numberType == "") numberType = "TELEPHONE";
				if (location == "") location = "HOME";

				if (exists)					

                    vw.RowFilter = "contactive = true and contorder >= '" + priority + "' and contnumber <> '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "' and contextracode = '" + location.Replace("'", "''") + "'";
				else
					vw.RowFilter = "contactive = true and contorder >= '" + priority + "'";
	
				int ctr = 0;
				foreach (DataRowView row in vw)
				{
					ctr++;
					unchecked 
					{
						row["CONTORDER"] = (byte)(priority + ctr);
					}
				}

				//If the address already exists then update its order.
				if (exists)
				{
					DataView existing = GetNumbers();					

                    existing.RowFilter = "contnumber = '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "' and contextracode = '" +  location.Replace("'", "''") + "'";	

					if (Convert.ToString(existing[0]["contnumber"]) == number)
					{
						existing[0]["contorder"] = priority;
						existing[0]["contactive"] = true;
					}
				}

				//Add the address if it does not exist.
				if (!exists)
				{
					// Add Row to Addresses Table
					DataRow dr = _contact.Tables[Table_Numbers].NewRow();

					dr["CONTID"] = this.ID;
					dr["CONTNUMBER"] = number;
					dr["CONTCODE"] = numberType;
					dr["CONTEXTRACODE"] = location;
                    dr["CONTDEFAULTORDER"] = priority;
					dr["CONTORDER"] = priority;
					dr["CONTACTIVE"] = true;
					dr["DisplayNumber"] = CodeLookup.GetLookup("INFOTYPE",location) + ": " + number;
					_contact.Tables[Table_Numbers].Rows.Add(dr);
				}

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.AddNumber()");
				return false;
			}
			
		}


        public bool AddDefaultNumber(string number, string numberType, string location)
        {
            return AddDefaultNumber(number, numberType, location, _contact.Tables[Table_Numbers].Rows.Count);
        }

        public bool AddDefaultNumber(string number, string numberType, string location, int priority)
        {

            try
            {

                if (number == null || numberType == null || location == null) return false;

                bool exists = HasNumber(number, numberType, "");

                //Dont let a priority value go higher than the address count.
                if ((priority + 1) > _contact.Tables[Table_Numbers].Rows.Count) priority = _contact.Tables[Table_Numbers].Rows.Count;
                //Dont let the priority be lower than zero.
                if (priority < 0) priority = 0;

                DataView vw = GetNumbers();

                if (numberType == "") numberType = "TELEPHONE";
                if (location == "") location = "HOME";

                if (exists)
                    vw.RowFilter = "contactive = true and contDefaultOrder >= '" + priority + "' and contnumber <> '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "'"; 
                else
                    vw.RowFilter = "contactive = true and contDefaultOrder >= '" + priority + "'";

                int ctr = 0;
                foreach (DataRowView row in vw)
                {
                    ctr++;
                    unchecked
                    {
                        row["contDefaultOrder"] = (byte)(priority + ctr); 
                    }
                }

                //If the address already exists then update its order.
                if (exists)
                {
                    DataView existing = GetNumbers();
                    existing.RowFilter = "contnumber = '" + number.Replace("'", "''") + "' and contcode = '" + numberType.Replace("'", "''") + "'"; 
                    for (int ex = 0; ex < existing.Count; ex++)
                    {
                        existing[ex]["contDefaultOrder"] = priority;
                        if (location.Equals(existing[ex]["contcode"]))
                            existing[ex]["contactive"] = true;
                    }
                }

                //Add the address if it does not exist.
                if (!exists)
                {
                    // Add Row to Addresses Table
                    DataRow dr = _contact.Tables[Table_Numbers].NewRow();

                    dr["CONTID"] = this.ID;
                    dr["CONTNUMBER"] = number;
                    dr["CONTCODE"] = numberType;
                    dr["CONTEXTRACODE"] = location;
                    dr["CONTDEFAULTORDER"] = priority;
                    dr["CONTORDER"] = priority;
                    dr["CONTACTIVE"] = true;
                    dr["DisplayNumber"] = CodeLookup.GetLookup("INFOTYPE", location) + ": " + number;
                    _contact.Tables[Table_Numbers].Rows.Add(dr);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.AddDefaultNumber()");
                return false;
            }

        }      

		#endregion

		#region Address Specific Methods

        /// <summary>
        /// Gets or Sets the registered office of the contact (uses the company extended data table).
        /// </summary>
        /// <value>The registered office address.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public Address RegisteredOfficeAddress
		{
			get
			{
				return Address.GetAddress(Common.ConvertDef.ToInt64(ExtendedData[Ext_Company].GetExtendedData("contRegOffAddID"), 0));
			}
			set
			{
				if (value == null)
					ExtendedData[Ext_Company].SetExtendedData("contAddRegOffID", DBNull.Value);
				else
				{
					ExtendedData[Ext_Company].SetExtendedData("contAddRegOffID", value.ID);
					value.AddType = "WORK";
					AddAddress(value, 0);
				}
			}
		}

        /// <summary>
        /// Gets or Sets the contacts default address.  This is the first address of the in the
        /// prefered address order table.
        /// </summary>
        /// <value>The default address.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public Address DefaultAddress
		{
			get
			{
				if (_defAddress == null)
				{
					object def = GetExtraInfo("contdefaultaddress");
					if (def is DBNull)
					{
						Address add = GetAddress(0);
						if (add == null)
							_defAddress = null; //No default address.
						else
						{
							//Set the default address if one is in the multi address table.
							SetExtraInfo("contdefaultaddress", add.ID);
							_defAddress = add;
						}
					}
					else
					{
						_defAddress = Address.GetAddress((long)def);
					}
				}
				return _defAddress;
			}
			set
			{
				_defAddress = value;
				if (_defAddress == null)
					SetExtraInfo("contdefaultaddress", DBNull.Value);
				else
				{
					if (_defAddress.ID == 0)
					{
						_defAddress = null;
					}
					else
					{
						SetExtraInfo("contdefaultaddress", value.ID);
						_defAddress = value;
                        
                        IsNewRemoveLastDefault(value);
                        AddAddress(_defAddress, 0);
					}
				}
			}
		}

        private void IsNewRemoveLastDefault(Address value)
        {
            if (IsNew)
            {
                var check = new DataView(_contact.Tables[Table_Addresses], String.Format("CONTCODE = '{0}'", value.AddType), "", DataViewRowState.Added);
                for (int i = check.Count - 1; i >= 0; i--)
                {
                    check.Delete(i);
                }
            }
        }


        /// <summary>
        /// Gets the contact default home address.
        /// </summary>
        /// <value>The default home address.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public Address DefaultHomeAddress
		{
			get
			{
				return GetAddress("HOME", 0);
			}
			set
			{
				value.AddType = "HOME";
                IsNewRemoveLastDefault(value);
                AddAddress(value);
			}
		}

        /// <summary>
        /// Gets the contact default work address.
        /// </summary>
        /// <value>The default work address.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public Address DefaultWorkAddress
		{
			get
			{
				return GetAddress("WORK", 0);
			}
			set
			{
				value.AddType = "WORK";
                IsNewRemoveLastDefault(value);
                AddAddress(value);
			}
		}

        /// <summary>
        /// Changes the type of the address.
        /// </summary>
        /// <param name="AddID">The add ID.</param>
        /// <param name="AddressType">Type of the address.</param>
        /// <remarks></remarks>
		public void ChangeAddressType(Int64 AddID, string AddressType)
		{
			string filter = "contAddID = " + AddID.ToString(); 
			DataView add = new DataView(_contact.Tables[Table_Addresses], filter, "contorder", DataViewRowState.CurrentRows);
			if (add.Count == 1)
			{
				add[0]["contCode"] = AddressType;
				add[0]["ContTypeDesc"] = CodeLookup.GetLookup("INFOADDTYPE",AddressType);
                _isdirty = true;
			}
		}

        /// <summary>
        /// Gets a list of addresses.
        /// </summary>
        /// <param name="includeDefault">Includes the default null entry.</param>
        /// <returns>A data view of the addresses.</returns>
        /// <remarks></remarks>
		public DataView GetAddresses(bool includeDefault)
		{
			return GetAddresses("", includeDefault,true,true);
		}

        /// <summary>
        /// Gets a list of addresses.
        /// </summary>
        /// <param name="location">The location of the address.</param>
        /// <param name="includeDefault">Includes the default null entry.</param>
        /// <returns>A data view of the addresses.</returns>
        /// <remarks></remarks>
		public DataView GetAddresses(string location, bool includeDefault)
		{
			return GetAddresses(location,includeDefault,false,true);
		}

        /// <summary>
        /// Gets a list of addresses.
        /// </summary>
        /// <param name="location">The location of the address.</param>
        /// <param name="includeDefault">Includes the default null entry.</param>
        /// <param name="all">Include records flagged as inactive.</param>
        /// <returns>A data view of the addresses.</returns>
        /// <remarks></remarks>
        public DataView GetAddresses(string location, bool includeDefault, bool all)
        {
            return GetAddresses(location, includeDefault, all, true);
        }

        /// <summary>
        /// Gets a list of addresses.
        /// </summary>
        /// <param name="location">The location of the address.</param>
        /// <param name="includeDefault">Includes the default null entry.</param>
        /// <param name="all">Include records flagged as inactive.</param>
        /// <param name="activeFlag">Whether or not the Contact is active.</param>
        /// <returns>A data view of the addresses.</returns>
        /// <remarks></remarks>
		public DataView GetAddresses(string location, bool includeDefault,bool all, bool activeFlag)
		{
			string filter = "";
			if (location == null || location == "")
				filter += "";
			else
			{
				filter += "contcode = '" + location.Replace("'", "''") + "' ";
			}

			if (!includeDefault)
			{
				if (filter != "") filter += " and ";
				filter += "contaddid <> 0";
			}
			
			if(!all)
			{
				if (filter != "") filter += " and ";
				filter += "contactive = " + activeFlag;
			}	
			
			DataView add = new DataView(_contact.Tables[Table_Addresses], filter, "contorder", DataViewRowState.CurrentRows);
			return add;
		}


        /// <summary>
        /// 
        /// </summary>
        private bool? _isNewDataFormat;
        /// <summary>
        /// Gets a value indicating whether this instance is new data format.
        /// </summary>
        /// <remarks></remarks>
        private bool IsNewDataFormat
        {
            get
            {
                if (!_isNewDataFormat.HasValue)
                {
                        var primaryKey = Session.CurrentSession.GetPrimaryKey("dbContactAddresses");
                        _isNewDataFormat = primaryKey.Rows.Count > 2;
                }

                return _isNewDataFormat.Value;
            }
        }

        private void FilterToAddress(Address address, DataView addresses)
        {
            if (IsNewDataFormat)
            {
                if (!string.IsNullOrWhiteSpace(address.AddType))
                {
                    addresses.RowFilter = "contaddid = '" + address.ID + "' and contCode = '" + address.AddType + "'";
                    return;
                }
            }
            
            addresses.RowFilter = "contaddid = '" + address.ID + "'";
        }

        /// <summary>
        /// Checks to see if the contact has an passed address.
        /// </summary>
        /// <param name="address">An address object to check.</param>
        /// <returns>A flag indicating if an address exists or not.</returns>
        /// <remarks></remarks>
		public bool HasAddress(Address address)
		{
			if (address == null) return false;

			DataView add = GetAddresses("", true,true,true);

            FilterToAddress(address, add);
						
			if (add.Count > 0)
				return true;
			else
				return false;
		}

        /// <summary>
        /// Gets an address object using the passed type.
        /// </summary>
        /// <param name="location">This could be a home, work, or other address.</param>
        /// <param name="prefered">The prefered order of the type of address.</param>
        /// <returns>An address object.</returns>
        /// <remarks></remarks>
		public Address GetAddress(string location, int prefered)
		{
			if (location == null) return null;
			if (prefered < 0) prefered = 0;
			DataView add = GetAddresses(location, false);

			if (add.Count < (prefered + 1))
			{
				prefered = add.Count - 1;
			}

			if (add.Count > 0)
				return Address.GetAddress((long)add[prefered]["contaddid"]);
			else
				return null;
				
		}

        /// <summary>
        /// Get an address in priority order.
        /// </summary>
        /// <param name="prefered">Will select the prefered address from the contact in row (0 zero is the most prefered).</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public Address GetAddress(int prefered)
		{
			if (prefered < 0) prefered = 0;
			DataView add = GetAddresses("",true,false,true);
			long addid = 0;

			if (add.Count < (prefered + 1))
			{
				prefered = add.Count - 1;
			}
			
			add.RowFilter = "contorder = 0 and contactive = true";
			
			if(add.Count > 0)
				addid = (long)add[0]["contaddID"];

			if (prefered >= 0)
				return Address.GetAddress(addid);
			else
				return null;
		}

        /// <summary>
        /// This Method will add the an address to a contact if it does not already exist.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddAddress(Address address)
		{
			DataView vw = _contact.Tables[Table_Addresses].DefaultView;
			vw.RowFilter = "contactive = true and contaddID <> 0"; //filteractive and not the default address
			int priority = vw.Count;
			return AddAddress(address,priority);
		}

        /// <summary>
        /// This Method will add the an address to a contact if it does not already exist.
        /// </summary>
        /// <param name="address">The Address object to be added</param>
        /// <param name="priority">The priority of the address to be inserted.</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public bool AddAddress(Address address, int priority)
		{
			try
			{
				if (address == null || address.ID == 0) return false;
			
				bool exists = HasAddress(address);

				DataView dv = _contact.Tables[Table_Addresses].DefaultView;
				dv.RowFilter = "contactive = true and contaddID <> 0";
				int activerows = dv.Count;
                if ((priority) > activerows)
				{
					priority = activerows;
				}
				
				//Dont let the priority be lower than zero.
				if (priority < 0) priority = 0;

				DataView vw = GetAddresses("",true,true,true);
				
				//filter all existing rows that have higher priority than the new row but not including the new row
				// if it already exists but was previosly flagged inactive
				if (exists)
					vw.RowFilter = "contactive = true and contorder >= '" + priority + "' and contaddid <> '" + address.ID + "'";
				else
					vw.RowFilter = "contactive = true and contorder >= '" + priority + "'";
				
				//update priority value of all rows with higher priorites than new row.
				int ctr = 0;
				foreach (DataRowView row in vw)
				{
					ctr++;
					unchecked 
					{
						row["CONTORDER"] = (byte)(priority + ctr);
					}
				}

				//If the address already exists then update its order.
				if (exists)
				{
					DataView existing = GetAddresses("",true,true,true);

                    FilterToAddress(address, existing);

					if (Convert.ToInt64(existing[0]["contaddid"]) == address.ID)
					{
						//Need to update the colums of this cached row to match the underlying changed data
						DataRow dr = existing[0].Row;
						foreach(DataColumn col in dr.Table.Columns) 
						{
							string colname = col.ColumnName;
							if(colname.IndexOf("add") > -1 && colname != "addID" && colname != "contaddID")
							{
								dr[colname] = address.GetExtraInfo(colname);
							}
						}

						existing[0]["contorder"] = priority;
						existing[0]["contactive"] = true;
					}
				}

				//Add the address if it does not exist.
				if (!exists)
				{
					// Add Row to Addresses Table
					DataRow dr = _contact.Tables[Table_Addresses].NewRow();

					if (address.AddType == String.Empty) address.AddType = "HOME";
					dr["CONTID"] = this.ID;
					dr["CONTADDID"] = address.ID;
					dr["CONTCODE"] = address.AddType;
					dr["CONTTYPEDESC"] = CodeLookup.GetLookup("INFOADDTYPE", address.AddType);
					dr["CONTORDER"] = priority;
					dr["CONTACTIVE"] = true;
					_contact.Tables[Table_Addresses].Rows.Add(dr);
				}

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Contact.AddAddress()");
				return false;
			}
		}

        /// <summary>
        /// Removes an address from the contact.
        /// </summary>
        /// <param name="address">The address to remove from the contact.</param>
        /// <returns>True if the address has been successfully removed.</returns>
        /// <remarks></remarks>
		public bool RemoveAddress(Address address)
		{
			if (address == null) return false;
			
			bool exists = HasAddress(address);

			if (address == Address.Null) return false;

			if (exists)
			{
                DataView vw = GetAddresses(location: "", includeDefault: false, all: false, activeFlag: true);

				int count = vw.Count;
				if (count <= 1)
				{
					throw new OMSException2("18002","The contact must have at least one address assigned.");
				}
				else
				{
					
					Common.KeyValueCollection pars = new KeyValueCollection();
					pars.Add("ADDID", address.ID);
					SearchEngine.SearchList sch = new SearchEngine.SearchList(Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.ContactInfoUsageCheck), this, pars);
					sch.Search(false);
					if (sch.ResultCount > 0)
					{
						Session.CurrentSession.OnShowSearch(new ShowSearchEventArgs(sch, Session.CurrentSession.Resources.GetMessage("CONTADDUSED", "The contact '%1%' cannot remove the address '%2%'.  It is associated to the following %FILES%...", "", true, Name, address.Line1).Text));
                        sch.Search(false);
                        if (sch.ResultCount > 0)
                            return false;
					}

                    FilterToAddress(address, vw);

					if (vw.Count > 0)
					{
						vw[0]["contActive"] = false;
						vw[0]["contOrder"] = count;
                        //vw[0].Delete();
                        
						//Make sure that the default address is changed if the one removed is the
						//default address.
						if (_defAddress != null)
						{
							//check if default address is this address
							if (_defAddress.ID == address.ID)
							{
								//if it is the same assign another as default, filter out existing and dummy blank
								vw.RowFilter = "contaddid <> '" + address.ID + "' and contorder <> -1";
								if (vw.Count > 0)
								{
									vw[0]["contOrder"] = 0;
								}
								DefaultAddress = GetAddress(0);
							}
						}

                        /*now delete the address which was set to inactive 
                        (and any legacy records which are no longer required)*/
                        DataView addressToRemove = GetAddresses(location: "",includeDefault: false, all: false, activeFlag: false);
                        foreach (DataRowView drv in addressToRemove)
                        {
                            drv.Delete();
                        }
					}
				}
			}
			
			return true;
		}


        #endregion

        #region Remote Account Specific Methods

        



        #endregion


        #region Methods

        /// <summary>
        /// String representation of the object.
        /// </summary>
        /// <returns>Textual description.</returns>
        /// <remarks></remarks>
		public override string ToString()
		{
			return Name;
		}

        /// <summary>
        /// Creates a client object with the current contact as the first default contact.
        /// </summary>
        /// <param name="clType">Type of the cl.</param>
        /// <returns>A client object</returns>
        /// <remarks></remarks>
		public Client CreateClient(ClientType clType)
		{
			return new Client(clType, this, false);
		}

        /// <summary>
        /// Appends the specified text to the notes field as a new line.
        /// </summary>
        /// <param name="location">The location of where the new text is going to go.</param>
        /// <param name="text">Text to append.</param>
        /// <remarks></remarks>
		public void AppendNoteText(NoteAppendingLocation location, string text)
		{
            //UTCFIX: DM - 30/11/06 - Note stamp show local time with time offset.
			string notes = Notes;
            string res = String.Format(Session.CurrentSession.Resources.GetResource("NOTESTAMP", "Note Created On : {0} @ {1} UTC{2:zzz} - By : {3}", "").Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now, Session.CurrentSession.CurrentUser.FullName); 
			if (location == NoteAppendingLocation.Beginning)
			{
				text += Environment.NewLine;
                text += res;
                text += notes;
				text += Environment.NewLine;
				text += "------------------------------------------------";
				notes = text;
			}
			else if (location == NoteAppendingLocation.End)
			{
				notes += Environment.NewLine;
                notes += res;
                notes += Environment.NewLine;
				notes += text;
				notes += Environment.NewLine;
				notes += "------------------------------------------------";
			}
			SetExtraInfo("contnotes", notes);
		}


        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <param name="intable">The intable.</param>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
		public DataTable GetDataTable(String intable)
		{
			return _contact.Tables[intable].Copy();
		}

        /// <summary>
        /// Updates the specific table of the object and persists it to the database.
        /// </summary>
        /// <param name="tablename">The table name to update.</param>
        /// <param name="sql">The select statement to use.</param>
        /// <remarks></remarks>
		private void Update(String tablename,String sql)
		{
			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (_contact.Tables[tablename].GetChanges()!= null)
			{
				Session.CurrentSession.Connection.Update(_contact.Tables[tablename],sql);
			}
		}

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="tableName">Table name within the internal dataset.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName,string tableName)
		{
			object val = this.GetExtraInfo(fieldName,tableName,0);
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="tableName">Table name within the internal dataset.</param>
        /// <param name="row">Row number from the table.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName,string tableName,int row)
		{
			object val = _contact.Tables[tableName].Rows[row][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

        /// <summary>
        /// Linkeds the address.
        /// </summary>
        /// <param name="Address">The address.</param>
        /// <param name="searchListCode"></param>
        /// <returns></returns>
        /// <remarks></remarks>
		public SearchEngine.SearchList LinkedAddress(Address Address, string searchListCode)
		{
			FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
			param.Add("addid",Address.ID);
			param.Add("contid",this.ID);
            return ExecuteSearchList(searchListCode, param);
		}

        public SearchEngine.SearchList LinkedEmailAddress(string emailAddress, string searchListCode)
        {
            FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
            param.Add("emailAddress", emailAddress);
            param.Add("contID", this.ID);
            return ExecuteSearchList(searchListCode, param);
        }

        private SearchEngine.SearchList ExecuteSearchList(string searchListCode, KeyValueCollection param)
        {
            FWBS.OMS.SearchEngine.SearchList _linked = new FWBS.OMS.SearchEngine.SearchList(searchListCode, null, param);
            _linked.Search(false);
            return _linked;
        }


		#endregion

		#region IExtraInfo Implementation

        /// <summary>
        /// Sets the raw internal data object with the specified value under the specified field name.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="val">Value to use.</param>
        /// <remarks></remarks>
		public void SetExtraInfo (string fieldName, object val)
		{
			//Ad business rules for properties here
			switch (fieldName.ToUpper())
			{
				case "CONTDEFAULTADDRESS":
				{
					_defAddress = null;
				}
				break;
				case "CONTAPPROVED":
				{
					if (Convert.ToBoolean(val) == false)
						this.SetExtraInfo(_contact.Tables[Table].Rows[0], "contapprrevokedon", DateTime.Now);
				}
				break;
				case "CONTGRADE":
				{
					byte grade = (byte)val;
					if (grade < 0) grade = 0;
					if (grade > 5) grade = 5;
				}
					break;
			}
            this.SetExtraInfo(_contact.Tables[Table].Rows[0], fieldName, val);
        }

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName)
		{
			object val = _contact.Tables[Table].Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}


        /// <summary>
        /// Returns the specified fields type.
        /// </summary>
        /// <param name="fieldName">Field Name.</param>
        /// <returns>Type of Field.</returns>
        /// <remarks></remarks>
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _contact.Tables[Table].Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("18001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
		public DataSet GetDataset()
		{
			return _contact;
		}


        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
		public DataTable GetDataTable()
		{
			return _contact.Tables[Table].Copy();
		}

        #endregion

        #region IUpdateable Implmentation

        public void Update()
        {
            Update(false);
        }
        
        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        /// <remarks></remarks>
		internal void Update(bool skipUpdatePermissionCheck)
		{ 
            CheckPermissions(skipUpdatePermissionCheck);

            //New addin object event arguments
            ObjectState state = State;
            if (this.OnExtCreatingUpdatingOrDeleting(state))
                return;

            try
			{
				//Keep the connection open.
				Session.CurrentSession.Connection.Connect(true);

				DataRow row = _contact.Tables[Table].Rows[0];

				bool isnew = IsNew;
			    bool contactNameIsNull = row["contName"].Equals(DBNull.Value);
				if (isnew)
				{
					if (CurrentContactType.GeneralType == OMSTypeContactGeneralType.Individual && Salutation.Trim() == "")
					{
						if (IndividualTitle.Trim() == "" && IndividualSurname.Trim() == "")
							Salutation = Name;
						else
							Salutation = IndividualTitle + " " + IndividualSurname;
					}
				}

				//Check if there are any changes made before setting the updated
				//and updated by properties then update.
				if (_contact.Tables[Table].GetChanges()!= null && !contactNameIsNull)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					if (_contact.Tables[Table].PrimaryKey == null || _contact.Tables[Table].PrimaryKey.Length == 0)
						_contact.Tables[Table].PrimaryKey = new DataColumn[1]{_contact.Tables[Table].Columns["contid"]};

					SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
					SetExtraInfo("Updated", DateTime.Now);
					Session.CurrentSession.Connection.Update(row, "dbContact");
				}

				
				if (isnew)
				{
				    Session.CurrentSession.CurrentContacts.Add(ID.ToString(), this);
                }

				//Update the address list.
				//Make sure that the multi contact table has the correct client id.
				foreach (DataRowView address in GetAddresses("",true,false,true))
				{
					if(Convert.ToInt64(address["contid"]) != this.ID)
						address["contid"] = this.ID;
				}

				if (_contact.Tables[Table_Addresses].GetChanges() != null)
				{
					//Update(Table_Addresses, SqlAddressLink);
					//DMB 5/5/2004 altered to use Dans SUPA DUPA Update to prevent the Concurrency issues 0 rows affected
					if (_contact.Tables[Table_Addresses].PrimaryKey.Length == 0)
                        _contact.Tables[Table_Addresses].PrimaryKey = new DataColumn[3] { _contact.Tables[Table_Addresses].Columns["contid"], _contact.Tables[Table_Addresses].Columns["contaddid"], _contact.Tables[Table_Addresses].Columns["contCode"] };
					Session.CurrentSession.Connection.Update(_contact.Tables[Table_Addresses],"dbContactAddresses",true);
				}

				//Update the number lists.
				foreach (DataRowView numbers in GetNumbers())
				{
					if(Convert.ToInt64(numbers["contid"]) != this.ID)
						numbers["contid"] = this.ID;
				}
				
				if (_contact.Tables[Table_Numbers].GetChanges() != null)
				{
					//Update(Table_Numbers, SqlNumberLink);
					//DMB 14/5/2004 This one has the concurrrency error also in certain siuations so changed to new update method
					if (_contact.Tables[Table_Numbers].PrimaryKey.Length == 0)
						_contact.Tables[Table_Numbers].PrimaryKey = new DataColumn[4]{_contact.Tables[Table_Numbers].Columns["contid"], _contact.Tables[Table_Numbers].Columns["contCode"], _contact.Tables[Table_Numbers].Columns["contNumber"],_contact.Tables[Table_Numbers].Columns["contExtraCode"]};
					Session.CurrentSession.Connection.Update(_contact.Tables[Table_Numbers],"dbContactNumbers",true);
				}

				//Update the email list.
				//Update the number lists.
				foreach (DataRowView emails in GetEmails("", true))
				{
					if(Convert.ToInt64(emails["contid"]) != this.ID)
						emails["contid"] = this.ID;
				}
				
				 if (_contact.Tables[Table_Emails].GetChanges() != null && !contactNameIsNull)
				{
					//Update(Table_Emails, SqlEmailLink);
					//DMB 5/5/2004 altered to use new update to prevent the Concurrency issues 0 rows affected
					if (_contact.Tables[Table_Emails].PrimaryKey.Length == 0)
						_contact.Tables[Table_Emails].PrimaryKey = new DataColumn[3]{_contact.Tables[Table_Emails].Columns["contid"], _contact.Tables[Table_Emails].Columns["contEmail"], _contact.Tables[Table_Emails].Columns["contCode"]};
					Session.CurrentSession.Connection.Update(_contact.Tables[Table_Emails],"dbContactEmails",true);
				}

				//DM - Reason for move - refer to client upate 17/09/04
				//Update all the extended data objects, if any.
				if (_extData != null && !contactNameIsNull)
				{
					foreach (FWBS.OMS.ExtendedData ext in _extData)
					{
						ext.UpdateExtendedData();
					}
				}

				//Update any of the cached objects that may have something to do with this contact.
				foreach (Client cl in Session.CurrentSession.CurrentClients.Values)
				{
					if (cl.HasContact(this))
					{
						//Make sure that the client contact link table is persisted.
						//Otherwise if a contact has not been persisted to the database
						//then the update of the contact will rollback the contact links.
						//There may be other cases like this, only testing will show it.
						cl.Update(Client.Table_Contacts, Client.Sql_ContactLink);
						cl.Refresh(true);
					}
				}
			
				foreach (OMSFile file in Session.CurrentSession.CurrentFiles.Values)
				{
					if (file.Associates.Contains(this))
					{
						file.Refresh(true);
					}
				}

				foreach (Associate assoc in Session.CurrentSession.CurrentAssociates.Values)
				{
					if (assoc.ContactID == this.ID)
					{
						assoc.Refresh(true);
					}
				}

                if (isnew)
                {
                    // Apply any Client Type Template Security
                    TemplateSecurity tmp = new TemplateSecurity("ContactType", this.ContactTypeCode);
                    if (tmp.HasSecurity) tmp.ApplySecurity(this.ID);
                }
                _isdirty = false;

                this.OnExtCreatedUpdatedDeleted(state);

                OnUpdated();

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				Session.CurrentSession.Connection.Disconnect(true);
			}
		}

        /// <summary>
        /// Occurs when [updated].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler Updated;

        /// <summary>
        /// Called when [updated].
        /// </summary>
        /// <remarks></remarks>
        protected virtual void OnUpdated()
        {
            EventHandler ev = Updated;
            if (ev != null)
                Updated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Refreshes the current object with the one from the database to prevent
        /// any potential concurrency issues.
        /// </summary>
        /// <remarks></remarks>
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
        /// <remarks></remarks>
		public void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

            if (this.OnExtRefreshing())
                return;

            DataTable changes = _contact.Tables[Table].GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, changes.Rows[0]);
            else
                Fetch(this.ID, null);

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
        /// <remarks></remarks>
		public void Cancel()
		{
			_defAddress = null;
            _isdirty = false;
			_contact.RejectChanges();

            if (_extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in _extData)
                    ext.CancelExtendedData();
            }
		}

        /// <summary>
        /// Gets a boolean flag indicating whether any changes have been made to the object.
        /// </summary>
        /// <remarks></remarks>
		public bool IsDirty
		{
			get
			{
				return (_isdirty || _contact.GetChanges() != null);
			}
		}

        /// <summary>
        /// Override so that the entity can be hidden from external viewing in MatterSphere
        /// Normal security will still be adhered to this is only an override to hide the entity
        /// </summary>
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

		#region IEnquiryCompatible Implentation

        /// <summary>
        /// An event that gets raised when a property changes within the object.
        /// </summary>
        /// <remarks></remarks>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Raises the property changed event with the specified event arguments.
        /// </summary>
        /// <param name="e">Property Changed Event Arguments.</param>
        /// <remarks></remarks>
		protected void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

        /// <summary>
        /// Edits the current contact object in the form of an enquiry (if the database states that is edit compatible).
        /// </summary>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

        /// <summary>
        /// Edits the current contact object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
        /// </summary>
        /// <param name="customForm">Enquiry form code.</param>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}



		#endregion
		
		#region IParent Implementation

        /// <summary>
        /// Gets the parent related object.
        /// </summary>
        /// <remarks></remarks>
		public object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Static Methods


        /// <summary>
        /// Returns a contact object based on the specified contact number.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="DefaultTab">The default tab.</param>
        /// <returns>A contact object.</returns>
        /// <remarks></remarks>
		static public  Contact GetContact(long id, string DefaultTab)
		{
			Session.CurrentSession.CheckLoggedIn();
			Contact cc = Session.CurrentSession.CurrentContacts[id.ToString()] as Contact;
			
            if (cc == null)
			{
				cc = new Contact(id);
				cc.DefaultTab = DefaultTab;
			}		
			return cc;
		}

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		static public Contact GetContact(long id)
		{
			return GetContact(id,null);
		}

		#endregion

		#region IOMSType Implementation

        /// <summary>
        /// Gets an OMS Type based on the contact type off this current instance of a contact object.
        /// </summary>
        /// <returns>A OMSType with information needed to represented this type of contact.</returns>
        /// <remarks></remarks>
		public OMSType GetOMSType()
		{
			return FWBS.OMS.ContactType.GetContactType(Convert.ToString(GetExtraInfo("conttypecode")));
		}


        /// <summary>
        /// Gets the value to link to many potential connector type object.
        /// </summary>
        /// <remarks></remarks>
		public object LinkValue
		{
			get
			{
				return ID;
			}
		}

        /// <summary>
        /// Gets or sets the default tab.
        /// </summary>
        /// <value>The default tab.</value>
        /// <remarks></remarks>
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

		#region IDisposable Implementation

        /// <summary>
        /// Disposes the object immediately without waiting for the garbage collector.
        /// </summary>
        /// <remarks></remarks>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        /// <remarks></remarks>
		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_extData != null)
				{
					_extData.Dispose();
					_extData = null;
				}

				if (_contact != null)
				{
					_contact.Dispose();
					_contact = null;
				}

			}
			
			//Dispose unmanaged objects.
		}

		#endregion

		#region IExtendedDataCompatible Implementation
        /// <summary>
        /// Gets the extended data list indexer which will expose
        /// each of the extended data objects on the particular object.
        /// </summary>
        /// <remarks></remarks>
		public ExtendedDataList ExtendedData 
		{
			get
			{
				if (_extData == null)
				{
					//Use the contact type configuration to initialise the extended data objects.
					ContactType ct = CurrentContactType;
					string [] codes = new string [ct.ExtData.Count];
					int ctr = 0;
					foreach(OMSType.ExtendedData ext in ct.ExtData)
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

        #region IAlert

        /// <summary>
        /// 
        /// </summary>
        private System.Collections.Generic.List<Alert> CustomAlerts = new System.Collections.Generic.List<Alert>();

        /// <summary>
        /// Gets a list of alerts for the object.
        /// </summary>
        /// <remarks></remarks>
        public Alert[] Alerts
        {
            get
            {
                System.Collections.Generic.List<Alert> alerts = new System.Collections.Generic.List<Alert>();
                if (!Approved)
                {
                    alerts.Add(new Alert(Session.CurrentSession.Resources.GetResource("CONTNOTAPPROVED", "Contact not approved", "").Text, Alert.AlertStatus.Amber));
                }
                alerts.AddRange(CustomAlerts);

                return alerts.ToArray();
            }
        }

        /// <summary>
        /// Adds the alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <remarks></remarks>
        public void AddAlert(Alert alert)
        {
            CustomAlerts.Add( alert);
        }

        /// <summary>
        /// Clears the alerts.
        /// </summary>
        /// <remarks></remarks>
        public void ClearAlerts()
        {
            foreach (Alert alert in CustomAlerts)
            {
                alert.ChangeStatus(Alert.AlertStatus.Off);
            }
        }

        #endregion

        #region ISecurable Members

        /// <summary>
        /// Gets the security id.
        /// </summary>
        /// <remarks></remarks>
        string Security.ISecurable.SecurityId
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

        /// <summary>
        /// 
        /// </summary>
        private DateTime timestamp;
        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <remarks></remarks>
        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
        }


        /// <summary>
        /// Checks the permissions.
        /// </summary>
        private void CheckPermissions()
        {
            CheckPermissions(false);
        }
        
        /// <summary>
        /// Checks the permissions.
        /// </summary>
        /// <remarks></remarks>
        private void CheckPermissions(bool skipUpdatePermissionCheck)
        {
            bool isnew = IsNew;
            bool isdirty = IsDirty;
            bool isdeleting = (_contact.Tables[Table].Rows[0].RowState == DataRowState.Deleted);

            if (isnew)
                new SystemPermission(StandardPermissionType.CreateContact).Check();
            else if (isdeleting)
                new ContactPermission(this, StandardPermissionType.Delete).Check();
            else if (isdirty)
            {
                if (!skipUpdatePermissionCheck)
                {
                    new ContactPermission(this, StandardPermissionType.Update).Check();
                    new SystemPermission(StandardPermissionType.UpdateContact).Check();
                }
            }
        }

        #endregion

        #region IOMSType Members


        /// <summary>
        /// Sets the current sessions.
        /// </summary>
        /// <remarks></remarks>
        public void SetCurrentSessions()
        {
            if (Session.CurrentSession.CurrentContact != this)
                Session.CurrentSession.CurrentContact = this;
        }

        #endregion
    }

    /// <summary>
    /// Stores a contacts email.
    /// </summary>
    /// <remarks></remarks>
	public class ContactEmail
	{
        /// <summary>
        /// 
        /// </summary>
		string _location = "HOME";
        /// <summary>
        /// 
        /// </summary>
		string _email = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactEmail"/> class.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="email">The email.</param>
        /// <remarks></remarks>
		public ContactEmail(string location, string email)
		{
			_location = location;
			_email = email;
		}

        /// <summary>
        /// Gets the email type code, HOME, WORK etc...
        /// </summary>
        /// <remarks></remarks>
		public string Location
		{
			get
			{
				return _location;
			}
		}

        /// <summary>
        /// Gets the actual email address.
        /// </summary>
        /// <remarks></remarks>
		public string Email
		{
			get
			{
				return _email;
			}
		}
	}

    /// <summary>
    /// Stores a contacts number.
    /// </summary>
    /// <remarks></remarks>
	public class ContactNumber
	{
        /// <summary>
        /// 
        /// </summary>
		string _type = "TELEPHONE";
        /// <summary>
        /// 
        /// </summary>
		string _location = "HOME";
        /// <summary>
        /// 
        /// </summary>
		string _number = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactNumber"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="location">The location.</param>
        /// <param name="number">The number.</param>
        /// <remarks></remarks>
		public ContactNumber(string type, string location, string number)
		{
			_type = type;
			_location = location;
			_number = number;
		}

        /// <summary>
        /// Gets the number type code, TELEPHONE, FAX etc...
        /// </summary>
        /// <remarks></remarks>
		public string Type
		{
			get
			{
				return _type;
			}
		}

        /// <summary>
        /// Gets the number location type, HOME, WORK etc...
        /// </summary>
        /// <remarks></remarks>
		public string Location
		{
			get
			{
				return _location;
			}	
		}

        /// <summary>
        /// Gets the actual number.
        /// </summary>
        /// <remarks></remarks>
		public string Number
		{
			get
			{
				return _number;
			}
		}
	}
}