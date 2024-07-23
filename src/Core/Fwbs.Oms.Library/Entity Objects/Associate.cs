using System;
using System.Data;
using System.Text;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;



namespace FWBS.OMS
{

    using System.Runtime.InteropServices;
    using FWBS.OMS.Security;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.StatusManagement;
    using FWBS.OMS.StatusManagement.Activities;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAssociate
    {
        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <remarks></remarks>
        long ID { get; }
    }

    /// <summary>
    /// Main associate object that links contact / client to a file of a client.
    /// This object exposes functionality to get associate properties and data.
    /// This associate object can be used with the enquiry engine.
    /// </summary>
    /// <remarks></remarks>
    [Security.SecurableType("ASSOCIATE")]
    public class Associate : IEnquiryCompatible, IDisposable, IOMSType, IExtendedDataCompatible, Security.ISecurable, IAssociate
    {
        #region Fields

        /// <summary>
        /// A private / null associate object.
        /// </summary>
        public static readonly Associate Private = new Associate(true,true);

        /// <summary>
        /// Internal data source.
        /// </summary>
        private DataSet _associate = null;
        /// <summary>
        /// The data row that a singular assciate object accesses for information.
        /// </summary>
        private DataRow _data = null;

        /// <summary>
        /// The associates Default Address .
        /// </summary>
        private Address _address = null;

        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
        public const string Table = "ASSOCIATES";

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
        internal const string Sql = "select * from dbassociates";

        /// <summary>
        /// A reference to a newly created file only.
        /// </summary>
        private OMSFile _file = null;

        /// <summary>
        /// A reference to a newly created contact only.
        /// </summary>
        private Contact _contact = null;

        /// <summary>
        /// Holds the different extended data sources for the contact.
        /// </summary>
        private ExtendedDataList _extData = null;

        /// <summary>
        /// Default tab to display
        /// </summary>
        private string _defaultTab;

        #endregion

        #region Constructors

        /// <summary>
        /// Static Object Required to have no return information for the Private constructor.
        /// This is Private and shouldn't be used.
        /// </summary>
        /// <param name="statcall">if set to <c>true</c> [statcall].</param>
        /// <param name="statcall2">if set to <c>true</c> [statcall2].</param>
        /// <remarks></remarks>
        private Associate(bool statcall,bool statcall2)
        {

        }


        /// <summary>
        /// Creates a new Associate object.  This routine is used by the enquiry engine
        /// to create new associate object.
        /// </summary>
        /// <remarks></remarks>
        internal Associate()
        {
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
            dt.Columns["associd"].AutoIncrement = true;

            //Add a new record.
            Global.CreateBlankRecord(ref dt, true);

            if (_associate == null) _associate = new DataSet("ASSOCINFO");
            _associate.Tables.Add(dt);

            _data = _associate.Tables[Table].Rows[0];

            //Set the created by and created date of the item.
            dt.Columns["assocId"].ReadOnly = false;
            SetExtraInfo("assocDefAddSetting", false);
            SetExtraInfo("assocUseDX", false);
            SetExtraInfo("assocactive", true);
            SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
            SetExtraInfo("Created", DateTime.Now);

            this.OnExtLoaded();
        }

        /// <summary>
        /// Creates a new instance of an associate.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        /// <remarks></remarks>
        public Associate(Contact contact, OMSFile file, string type)
            : this()
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            if (file == null)
                throw new ArgumentNullException("file");

            SetAssociateInfo(file, contact, type);
        }

        /// <summary>
        /// Creates a new instance of an associate.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="file">The file.</param>
        /// <param name="type">The type.</param>
        /// <remarks></remarks>
        public Associate(Contact contact, OMSFile file, AssociateType type)
            : this()
        {
            if (contact == null)
                throw new ArgumentNullException("contact");

            if (file == null)
                throw new ArgumentNullException("file");

            if (type == null)
                throw new ArgumentNullException("type");



            SetAssociateInfo(file, contact, type.Code);
        }

        /// <summary>
        /// Accepts an associate data row schema.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="file">The file.</param>
        /// <remarks></remarks>

        internal Associate(DataRow row, OMSFile file)
            : this()
        {
            _file = file;
            _data = row;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Associate"/> class.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="file">The file.</param>
        /// <param name="GetCopy">if set to <c>true</c> [get copy].</param>
        /// <remarks></remarks>
        internal Associate(DataRow row, OMSFile file, bool GetCopy)
            : this()
        {
            _file = file;
            _associate.Tables[Table].Rows.Clear();
            _associate.Tables[Table].ImportRow(row);
            _data = _associate.Tables[Table].Rows[0];

            if (GetCopy)
            {
                SetExtraInfo("rowguid", Guid.NewGuid());
            }
            SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
            SetExtraInfo("Created", DateTime.Now);
        }


        /// <summary>
        /// Initialised an existing associate object with the specified identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal Associate(long id)
        {
            Fetch(id, null);

            //An edit contructor should add the object created to the session memory collection.
            Session.CurrentSession.CurrentAssociates.Add(ID.ToString(), this);

            this.OnExtLoaded();
        }

        /// <summary>
        /// Constructs an associate.
        /// </summary>
        /// <param name="id">The associate id to retrieve.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
        private void Fetch(long id, DataRow merge)
        {
            _address = null;

            //Make sure that the parameters list is cleared after use.	
            DataSet data = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where AssocID = @ASSOCID", new string[1] { Table }, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("ASSOCID", System.Data.SqlDbType.BigInt, 0, id) });

            if ((data.Tables[Table] == null) || (data.Tables[Table].Rows.Count == 0))
            {
                throw new OMSException2("21002", "Associate with AssocID :%1% Doesn't Exist", new Exception(), true, id.ToString());
            }

            if (merge != null)
                Global.Merge(data.Tables[Table].Rows[0], merge);

            _associate = data;
            _data = _associate.Tables[Table].Rows[0];

            timestamp = DateTime.UtcNow;

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
                    switch (_associate.Tables[Table].Rows[0].RowState)
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
        /// Gets a value indicating whether the associate object is new and needs to be
        /// updated to exist in the database.
        /// </summary>
        /// <remarks></remarks>
        public bool IsNew
        {
            get
            {
                try
                {
                    return (_data.RowState == DataRowState.Added);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The unique identifier number of the associate.
        /// </summary>
        /// <remarks></remarks>
        public long ID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("associd"));
            }
        }


        /// <summary>
        /// Returns the Contact Name
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal string ContactName
        {
            get
            {
                return this.Contact.Name;
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
        /// A flag that indicates whether the associated contact is a clients contact.
        /// </summary>
        /// <remarks></remarks>
        public bool IsClient
        {
            get
            {
                return OMSFile.Client.HasContact(Contact);
            }
        }

        /// <summary>
        /// Gets or Sets the associate heading description, if blank on the associate will return the FileDescription from OMSFile.
        /// </summary>
        /// <value>The assoc heading.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string AssocHeading
        {
            get
            {
                return Convert.ToString(GetExtraInfo("AssocHeading"));
            }
            set
            {
                string oldval = AssocHeading;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("AssocHeading", DBNull.Value);
                    else
                        SetExtraInfo("AssocHeading", value);

                    OnPropertyChanged(new PropertyChangedEventArgs("AssocHeading", oldval, AssocHeading));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the associate type.  This is how the contact is linked to the file it is associated to.
        /// </summary>
        /// <value>The type of the assoc.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string AssocType
        {
            get
            {
                return Convert.ToString(GetExtraInfo("AssocType"));
            }
            set
            {
                string oldval = AssocType;
                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("AssocType", "UNKNOWN");
                    else
                        SetExtraInfo("AssocType", value);

                    OnPropertyChanged(new PropertyChangedEventArgs("AssocType", oldval, AssocType));
                }
            }
        }


        /// <summary>
        /// Gets the current associate type object.
        /// </summary>
        /// <remarks></remarks>
        [System.ComponentModel.Browsable(false)]
        public AssociateType CurrentAssociateType
        {
            get
            {
                return (AssociateType)GetOMSType();
            }
        }



        /// <summary>
        /// Gets the file object based from this occurence of the associate.
        /// </summary>
        /// <remarks></remarks>
        public OMSFile OMSFile
        {
            get
            {
                if (_file == null || _file.IsNew == false)
                {
                    if (GetExtraInfo("FILEID") != DBNull.Value)
                        return OMSFile.GetFile((long)GetExtraInfo("FILEID"));
                }
                return _file;
            }
        }



        /// <summary>
        /// Gets the Date of the Last document sent to this associate
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public FWBS.Common.DateTimeNULL LastDocumentDate
        {
            /*
             * 8 Oct 2014 by GM 
             * ----------
             * changed to config schema if AdvancedSecurityEnabled to improve speed
             * this could mean that a date is returned for a document that the user does not have access to - but this is not a security risk
            */

            get
            {
                try
                {
                    string query = "";
                    if (FWBS.OMS.Session.CurrentSession.AdvancedSecurityEnabled)
                        query = "select MAX(Created) from config.dbdocument with ( nolock ) where config.dbdocument.associd = @ASSOCID and docDeleted = 0";
                    else
                        query = "select MAX(Created) from dbdocument with ( nolock ) where dbdocument.associd = @ASSOCID and docDeleted = 0";
                    object ld = Session.CurrentSession.Connection.ExecuteSQLScalar(query, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("ASSOCID", System.Data.SqlDbType.BigInt, 0, this.ID) });
                    return FWBS.Common.ConvertDef.ToDateTimeNULL(ld, DBNull.Value);
                }
                catch
                {
                    return DBNull.Value;
                }
            }
        }



        /// <summary>
        /// Gets the OMSDocument record for the last document sent to this associate
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public FWBS.OMS.OMSDocument LastDocument
        {
            /*
             * 8 Oct 2014 by GM
             * ----------
             * This method is not called from anywhere in Core or Office projects.  1 instance found in Script FDSCRFILCLOSURE - but this is a rarely used module and is only when closing a matter.  
             * maintain dbo.dbdocument for the query as otherwise this could get a document that the user does not have access to
            */
            get
            {
                try
                {
                    string query = "select top 1 docid from dbdocument with ( nolock ) where dbdocument.associd = @ASSOCID and docdeleted = 0 order by created desc";
                    object ld = Session.CurrentSession.Connection.ExecuteSQLScalar(query, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("ASSOCID", System.Data.SqlDbType.BigInt, 0, this.ID) });
                    return FWBS.OMS.OMSDocument.GetDocument(Convert.ToInt32(ld));
                }
                catch
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// Gets or Sets their file reference.
        /// </summary>
        /// <value>Their ref.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string TheirRef
        {
            get
            {
                return Convert.ToString(GetExtraInfo("ASSOCREF"));
            }
            set
            {
                string oldval = TheirRef;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("ASSOCREF", DBNull.Value);
                    else
                        SetExtraInfo("ASSOCREF", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("TheirRef", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the associate addresse information.
        /// </summary>
        /// <value>The addressee.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Addressee
        {
            get
            {
                string ret = Convert.ToString(GetExtraInfo("ASSOCADDRESSEE"));
                if (ret == String.Empty && GetExtraInfo("contid") != DBNull.Value)
                    return Contact.Addressee;
                else
                    return ret;
            }
            set
            {
                string oldval = Addressee;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("ASSOCADDRESSEE", DBNull.Value);
                    else
                    {
                        SetExtraInfo("ASSOCADDRESSEE", value);
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Addressee", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the salutation bing used by the associate.
        /// </summary>
        /// <value>The salutation.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public String Salutation
        {
            get
            {
                string ret = Convert.ToString(GetExtraInfo("ASSOCSALUT"));
                if (ret == String.Empty && GetExtraInfo("contid") != DBNull.Value)
                {
                    SetExtraInfo("ASSOCSALUT", Contact.Salutation);
                    OnPropertyChanged(new PropertyChangedEventArgs("Salutation", "", Contact.Salutation));
                    return Contact.Salutation;
                }
                else
                {
                    return ret;
                }
            }
            set
            {
                string oldval = Salutation;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("ASSOCSALUT", DBNull.Value);
                    else
                        SetExtraInfo("ASSOCSALUT", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("Salutation", oldval, value));
                }
            }
        }



        /// <summary>
        /// Gets or Sets the associates default telephone number.
        /// </summary>
        /// <value>The default telephone number.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string DefaultTelephoneNumber
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocddi"));
            }
            set
            {
                string oldval = DefaultTelephoneNumber;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("assocddi", DBNull.Value);
                    else
                    {
                        if (Contact.HasNumber(value, "TELEPHONE", "") == false)
                        {
                            Contact.AddNumber(value, "TELEPHONE", "");
                            Contact.Update();
                        }
                        SetExtraInfo("assocddi", value);
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("DefaultTelephoneNumber", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the associates default fax number.
        /// </summary>
        /// <value>The default fax number.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string DefaultFaxNumber
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocfax"));
            }
            set
            {
                string oldval = DefaultFaxNumber;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("assocfax", DBNull.Value);
                    else
                    {
                        if (Contact.HasNumber(value, "FAX", "") == false)
                        {
                            Contact.AddNumber(value, "FAX", "");
                            Contact.Update();
                        }
                        SetExtraInfo("assocfax", value);
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("DefaultFaxNumber", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the associates default fax number.
        /// </summary>
        /// <value>The default mobile.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string DefaultMobile
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocmobile"));
            }
            set
            {
                string oldval = DefaultMobile;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("assocmobile", DBNull.Value);
                    else
                    {
                        if (Contact.HasNumber(value, "MOBILE", "") == false)
                        {
                            Contact.AddNumber(value, "MOBILE", "");
                            Contact.Update();
                        }
                        SetExtraInfo("assocmobile", value);
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("DefaultMobile", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets the Preferred Contact Method of the Associate.
        /// </summary>
        /// <value>The preferred contact method.</value>
        /// <remarks></remarks>
        [EnquiryEngine.EnquiryUsage(true)]
        public string PreferredContactMethod
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocPrefContactMethod"));
            }
            set
            {
                if (value == String.Empty)
                    SetExtraInfo("assocPrefContactMethod", DBNull.Value);
                else
                    SetExtraInfo("assocPrefContactMethod", value);

            }
        }

        /// <summary>
        /// Gets or Sets the associates default eamil address number.
        /// </summary>
        /// <value>The default email.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string DefaultEmail
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocEmail"));
            }
            set
            {
                string oldval = DefaultEmail;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("assocEmail", DBNull.Value);
                    else
                    {
                        if (Contact.HasEmail(value, "", true))
                        {
                            Contact.DefaultEmail = value;
                        }
                        else
                        {
                            Contact.AddEmail(value, "HOME", 0);
                        }
                        SetExtraInfo("assocEmail", value);
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("DefaultEmail", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets the contact object that is associated to this associate.
        /// </summary>
        /// <remarks></remarks>
        public Contact Contact
        {
            get
            {
                try
                {
                    if ((_contact == null || _contact.IsNew == false) && GetExtraInfo("contid") != DBNull.Value)
                        return Contact.GetContact((long)GetExtraInfo("contid"));
                    else
                        return _contact;
                }
                catch (RowNotInTableException)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or Sets a flag that indicates whether a client contact associate will use the default address of the
        /// relevant contact or the default address of the client if an address has not been specifically set.
        /// </summary>
        /// <value><c>true</c> if [default address setting]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public bool DefaultAddressSetting
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetExtraInfo("assocDefAddSetting"), false);
            }
            set
            {
                SetExtraInfo("assocDefAddSetting", value);
            }
        }

        /// <summary>
        /// Gets or Sets the default address of the associate.
        /// </summary>
        /// <value>The default address.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal Address defaultAddress
        {
            get
            {
                if (_address == null)
                {
                    object addval = GetExtraInfo("assocdefaultaddid");
                    if (addval is DBNull)
                    {
                        _address = Address.Null;
                    }
                    else
                        _address = Address.GetAddress(Convert.ToInt64(addval));
                }
                return _address;
            }
            set
            {
                Address oldval = DefaultAddress;
                DefaultAddress = value;
                OnPropertyChanged(new PropertyChangedEventArgs("defaultAddress", oldval, value));
            }
        }

        /// <summary>
        /// Gets or Sets the default address of the associate.
        /// </summary>
        /// <value>The default address.</value>
        /// <remarks></remarks>
        public Address DefaultAddress
        {
            get
            {
                if (_address == null || _address == Address.Null)
                {
                    object addval = GetExtraInfo("assocdefaultaddid");
                    if (addval is DBNull)
                    {
                        if (IsClient)
                        {
                            if (DefaultAddressSetting == false)
                                return OMSFile.Client.DefaultAddress;
                            else
                                return Contact.DefaultAddress;
                        }
                        else
                            return Contact.DefaultAddress;
                    }
                    else
                        _address = Address.GetAddress(Convert.ToInt64(addval));
                }
                return _address;
            }
            set
            {
                Address oldval = DefaultAddress;

                _address = value;
                if (_address == null || _address == Address.Null)
                    SetExtraInfo("assocdefaultaddid", DBNull.Value);
                else
                {
                    if (Contact.HasAddress(_address))
                        SetExtraInfo("assocdefaultaddid", _address.ID);
                    else
                    {
                        _address = null;
                        throw new OMSException(HelpIndexes.AddressNotAssignedToContact);
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("DefaultAddress", oldval, value));

            }
        }


        /// <summary>
        /// Gets or Sets the notes of the associate.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Notes
        {
            get
            {
                return Convert.ToString(GetExtraInfo("assocnotes"));
            }
            set
            {
                string oldval = Notes;
                if (oldval != value)
                {
                    SetExtraInfo("assocnotes", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("Notes", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets the associates contact type (mainly for the use in enquiry forms).
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal string ContactType
        {
            get
            {
                return Contact.ContactTypeCode;
            }
        }

        /// <summary>
        /// Gets the file id that is associated to this associate.
        /// </summary>
        /// <remarks></remarks>
        public long OMSFileID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("fileid"));
            }
        }


        /// <summary>
        /// Gets the contact id that is associated to this associate.
        /// </summary>
        /// <remarks></remarks>
        internal long ContactID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("contid"));
            }
        }

        /// <summary>
        /// A flag that indicates whether the associate is active or not.
        /// </summary>
        /// <remarks></remarks>
        public bool Active
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetExtraInfo("assocactive"), true);
            }
        }

        public Guid Key
        {
            get
            {
                return Common.ConvertDef.ToGuid(GetExtraInfo("rowguid"), Guid.Empty);
            }

            private set
            {
                var oldval = Key;
                SetExtraInfo("rowguid", value);
                OnPropertyChanged(new PropertyChangedEventArgs("Key", oldval, value));
            }
        }
        #endregion

        #region IExtraInfo Implementation

        /// <summary>
        /// Sets the raw internal data object with the specified value under the specified field name.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="val">Value to use.</param>
        /// <remarks></remarks>
        public void SetExtraInfo(string fieldName, object val)
        {
            this.SetExtraInfo(_data, fieldName, val);
        }


        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
        public object GetExtraInfo(string fieldName)
        {
            object val = _data[fieldName];
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
        /// <remarks></remarks>
        public Type GetExtraInfoType(string fieldName)
        {
            try
            {
                return _data.Table.Columns[fieldName].DataType;
            }
            catch
            {
                throw new OMSException2("21001", "Error Getting Extra Info Field %1% Probably Not Initialized", new Exception(), true, fieldName);
            }
        }

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
        public DataSet GetDataset()
        {
            if (_associate != null)
                return _associate;
            else
                throw new OMSException2("21003", "This method is not available when the Associate is a memeber of the AssociateCollection", "");

        }


        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
        public DataTable GetDataTable()
        {
            if (_associate != null)
                return _associate.Tables[Table].Copy();
            else
                throw new OMSException2("21003", "This method is not available when the Associate is a memeber of the AssociateCollection", "");
        }

        #endregion

        #region IUpdateable Implementation

        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        /// <remarks></remarks>
        public void Update()
        {
            try
            {
                CheckPermissions();

                ObjectState state = State;
                if (this.OnExtCreatingUpdatingOrDeleting(state))
                    return;

                // Update the Contact Object
                this.Contact.Update();

                Session.CurrentSession.Connection.Connect(true);

                DataRow row = _data;

                bool isnew = IsNew;


                if (AssocHeading.Trim() == "")
                    AssocHeading = OMSFile.FileDescription;



                //Check if there are any changes made before setting the updated
                //and updated by properties then update.
                if (_associate.Tables[Table].GetChanges() != null)
                {
                    //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
                    if (row.Table.PrimaryKey == null || row.Table.PrimaryKey.Length == 0)
                        row.Table.PrimaryKey = new DataColumn[1] { row.Table.Columns["associd"] };

                    SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
                    SetExtraInfo("Updated", DateTime.Now);
                    Session.CurrentSession.Connection.Update(row, "dbassociates");
                }

                //Replace the old data with the new so that the unique identifier is now specified.
                if (isnew)
                {
                    Session.CurrentSession.CurrentAssociates.Add(ID.ToString(), this);
                }

                if (_extData != null)
                {
                    foreach (FWBS.OMS.ExtendedData ext in _extData)
                    {
                        ext.UpdateExtendedData();
                    }
                }

                //Update any of the cached objects that may have something to do with this contact.
                foreach (OMSFile file in Session.CurrentSession.CurrentFiles.Values)
                {
                    if (this.OMSFileID == file.ID)
                    {
                        file.Refresh(true);
                    }
                }

                this.OnExtCreatedUpdatedDeleted(state);
            }
            finally
            {
                Session.CurrentSession.Connection.Disconnect(true);
            }
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

            this.OnExtRefreshing();

            if (_associate != null)
            {
                _address = null;
                DataTable changes = _associate.Tables[Table].GetChanges();

                if (changes != null && applyChanges && changes.Rows.Count > 0)
                    Fetch(this.ID, changes.Rows[0]);
                else
                    Fetch(this.ID, null);

                //Refresh all the extended data sources, if any.
                if (_extData != null)
                {
                    foreach (FWBS.OMS.ExtendedData ext in _extData)
                    {
                        ext.RefreshExtendedData(applyChanges);
                    }
                }

                this.OnExtRefreshed();
            }
            else
                throw new OMSException2("21003", "This method is not available when the Associate is a memeber of the AssociateCollection", "");
        }

        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        /// <remarks></remarks>
        public void Cancel()
        {
            _address = null;
            _data.Table.RejectChanges();

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
        /// <remarks></remarks>
        public bool IsDirty
        {
            get
            {
                return (_data != null && _data.Table.GetChanges() != null);
            }
        }

        #endregion

        #region IEnquiryCompatible Implementation

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
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        /// <summary>
        /// Edits the current associate object in the form of an enquiry (if the database states that is edit compatible).
        /// </summary>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
        public Enquiry Edit(Common.KeyValueCollection param)
        {
            return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.AssociateEdit), param);
        }

        /// <summary>
        /// Edits the current associate object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
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
                return OMSFile;
            }
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes the object immediately without waiting for the garbage collector.
        /// </summary>
        /// <remarks></remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        /// <remarks></remarks>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_extData != null)
                {
                    _extData.Dispose();
                    _extData = null;
                }

                _data = null;

                if (_associate != null)
                {
                    _associate.Dispose();
                    _associate = null;
                }

            }
            //Dispose unmanaged objects.
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns an associate object based on the specified associate id.
        /// </summary>
        /// <param name="id">Associate ID Parameter.</param>
        /// <param name="defaultTab">The default tab.</param>
        /// <returns>An associate object.</returns>
        /// <remarks></remarks>
        public static Associate GetAssociate(long id, string defaultTab)
        {
            Session.CurrentSession.CheckLoggedIn();
            Associate ca = Session.CurrentSession.CurrentAssociates[id.ToString()] as Associate;
            if (ca == null)
            {
                ca = new Associate(id);
            }
            ca.DefaultTab = defaultTab;
            return ca;
        }

        /// <summary>
        /// Gets the associate.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Associate GetAssociate(long id)
        {
            return GetAssociate(id, null);
        }

        /// <summary>
        /// Gets the default associate from the specified file.
        /// </summary>
        /// <param name="file">The file object to get the associate from.</param>
        /// <returns>An associate object.</returns>
        /// <remarks></remarks>
        public static Associate GetDefaultAssociate(OMSFile file)
        {
            Session.CurrentSession.CheckLoggedIn();
            return file.DefaultAssociate;
        }

        /// <summary>
        /// Gets a list of associated types based on a specified contact type.
        /// </summary>
        /// <param name="contactType">Specific contact type to add to the add link filter.</param>
        /// <param name="includeNull">Includes a null item.</param>
        /// <returns>A data Table list.</returns>
        /// <remarks></remarks>
        public static DataTable GetAssociateTypes(string contactType, bool includeNull)
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] paramlist = new IDataParameter[3];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("ContactType", SqlDbType.NVarChar, 15, contactType);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            paramlist[2] = Session.CurrentSession.Connection.AddParameter("IncludeNull", SqlDbType.Bit, 0, includeNull);
            DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprAssociateTypes", "SUBASSOC", paramlist);

            return dt;
        }

        /// <summary>
        /// Gets all associate types.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable GetAllAssociateTypes()
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select typeCode, dbo.GetCodeLookupDesc('SUBASSOC', typecode, @UI) as typeDesc from dbassociatetype order by typedesc", "SUBASSOC", paramlist);

            return dt;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Gets the current associates valid associated formats.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAssociateTypes()
        {
            return GetAssociateTypes(true);
        }

        /// <summary>
        /// Gets the current associates valid associated formats.
        /// </summary>
        /// <param name="includeNull">Includes the DBNull if specified.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAssociateTypes(bool includeNull)
        {
            return Associate.GetAssociateTypes(Contact.ContactTypeCode, includeNull);
        }

        /// <summary>
        /// Get Precedents based on base prec type and File information
        /// </summary>
        /// <param name="baseprectype">Base Precedent Type (Letterhead)</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetPrecedents(string baseprectype)
        {
            return FWBS.OMS.Precedent.GetAssocPrecedents(this, baseprectype).Copy();
        }

        /// <summary>
        /// Sets the file and contact for the associate object for when a blank associate is created.
        /// </summary>
        /// <param name="file">The file that the contact will be associated to.</param>
        /// <param name="contact">The contact to be associated.</param>
        /// <param name="associateType">The default associated type to use.</param>
        /// <remarks></remarks>
        public void SetAssociateInfo(OMSFile file, Contact contact, string associateType)
        {
            if (IsNew)
            {
                if (file == null || contact == null)
                {
                    throw new OMSException(HelpIndexes.AvailableWhenNew, "SetAssociateInfo", "Associate");
                }

                new FileActivity(file, FileStatusActivityType.AssociateCreation).Check();

                if (file.IsNew) _file = file;
                if (contact.IsNew) _contact = contact;
                SetExtraInfo("fileid", file.ID);
                SetExtraInfo("contid", contact.ID);
                SetExtraInfo("assocorder", file.Associates.Count);

                // Set the Default address to be the Contact Address not the client Address
                if (IsClient && file.Client.CurrentClientType.AddAssociateAsContactAddress)
                {
                    //Make sure that the client contacts
                    DefaultAddressSetting = true;
                }

                defaultAddress = Address.Null;
                AssocType = associateType;
                Addressee = contact.Addressee;
                Salutation = contact.Salutation;
                DefaultTelephoneNumber = contact.DefaultTelephoneNumber;
                DefaultFaxNumber = contact.DefaultFaxNumber;
                DefaultMobile = contact.DefaultMobileNumber;
                DefaultEmail = contact.DefaultEmail;

                if (contact.AssociateTheirRef != String.Empty)
                    TheirRef = contact.AssociateTheirRef;
            }
        }

        /// <summary>
        /// Clones the current associate data into a given row by reference.
        /// </summary>
        /// <param name="row">The row to update.</param>
        /// <remarks></remarks>
        internal void Clone(ref DataRow row)
        {
            if (_associate != null)
            {
                foreach (DataColumn col in _associate.Tables[Table].Columns)
                {
                    if (col.ColumnName.ToLower() != "associd")
                        row[col.ColumnName] = _associate.Tables[Table].Rows[0][col.ColumnName];
                }
                if (row.Table.Columns.Contains("ContName"))
                {
                    row["ContName"] = Contact.Name;
                }
                if (row.Table.Columns.Contains("ContTypeCode"))
                {
                    row["ContTypeCode"] = Contact.ContactTypeCode;
                }
            }
            else
                throw new OMSException2("21003", "This method is not available when the Associate is a memeber of the AssociateCollection", "");

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
            SetExtraInfo("assocnotes", notes);
        }

        /// <summary>
        /// Get the Default Address from Associate
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public String GetAssocAddress(string delimiter)
        {
            // Build up the String with additional information that maybe required such as if client name is to be included etc etc
            System.Text.StringBuilder ret = new System.Text.StringBuilder();

            if (delimiter == "")
                delimiter = Environment.NewLine;

            //check work type for Contact Links logic
            if (CheckContactLinksAddressingRegistryKey(delimiter, ret))
            {
                ContactLinksAddressBuilder(delimiter,ret);
            }
            else
            {
                // Using a Reg Key will allow us to Facilitate old code and new in one process.
                FWBS.Common.Reg.ApplicationSetting oldAddressMethod = null;
                oldAddressMethod = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Switches", "OldAddressMethod");

                if (FWBS.Common.ConvDef.ToBoolean(oldAddressMethod.GetSetting(false), true)) // Old Code first
                {
                    TestContactAndClientType(delimiter, ret);
                }
                else
                {   //-------------------------------------------------
                    //New Code to resolve Addressing Issues 
                    //-------------------------------------------------
                    if (this.IsClient)
                    {
                        AppendClientAddressee(delimiter, ret);
                        DetermineContactNameAddition(delimiter, ret);
                    }
                    else //This is NOT a client contact
                    {
                        AppendAddresseeOrContactName(delimiter, ret);
                        CheckNamingRegistryKey(delimiter, ret);
                    }
                }
                BuildAddress(delimiter, ret);
            }
            return ret.ToString();
        }


        private void TestContactAndClientType(string delimiter, System.Text.StringBuilder ret)
        {
            //New Code to resolve Addressing Issues - GM - 12/09/2003
            if (this.Contact.ContactTypeCode == "INDIVIDUAL" && this.OMSFile.Client.CurrentClientType.ContactType == "INDIVIDUAL")
            {
                AppendAddresseeOrContactName(delimiter, ret);
            }
            else
            {
                DetermineNamingSectionForContact(delimiter, ret);
            }
        }


        private void DetermineNamingSectionForContact(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.IsClient)
            {
                IsClientAppendAddresseeOrContactName(delimiter, ret);
                IsClientAppendClientName(delimiter, ret);
            }
            else
            {
                IsNotClientAppendAddresseeIfDifferentFromName(delimiter, ret);
            }
        }


        private void IsNotClientAppendAddresseeIfDifferentFromName(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.Addressee.Trim() != this.Contact.Name.Trim())
            {
                ret.Append(this.Addressee.Trim());
                ret.Append(delimiter);
            }
            ret.Append(this.Contact.Name);
            ret.Append(delimiter);
        }


        private void IsClientAppendClientName(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.Contact.Name != this.OMSFile.Client.ClientName && this.DefaultAddressSetting == false)
            {
                ret.Append(this.OMSFile.Client.ClientName);
                ret.Append(delimiter);
            }
        }


        private void IsClientAppendAddresseeOrContactName(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.Addressee.Trim() != "")
            {
                ret.Append(this.Addressee.Trim());
                ret.Append(delimiter);
                //GM added 09062004 to fix addressing issues
                ret.Append(this.ContactName.Trim());    //
                ret.Append(delimiter);					//
                //GM 09062004							//
            }
            else
            {
                ret.Append(this.Contact.Name);
                ret.Append(delimiter);
            }
        }


        private void AppendClientAddressee(string delimiter, System.Text.StringBuilder ret)
        {
            //This is a client contact
            //Always use the addressee if it is filled in
            if (this.Addressee.Trim() != "")
            {
                ret.Append(this.Addressee.Trim());
                ret.Append(delimiter);
            }
        }


        private void DetermineContactNameAddition(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.OMSFile.Client.CurrentClientType.ContactType.ToUpper().Trim() == "INDIVIDUAL")
            {   // Individual Client, use contact name if addressee is blank.
                if (this.Addressee.Trim() == "")
                {
                    ret.Append(this.Contact.Name.Trim());
                    ret.Append(delimiter);
                }
            }
            else // Corporate Client
            {
                CorporateAppendClientOrContactName(delimiter, ret);
            }
        }


        private void CorporateAppendClientOrContactName(string delimiter, System.Text.StringBuilder ret)
        {
            //If addressee is blank, and the contact being written to is an individual
            if (this.Addressee.Trim() == "" && this.Contact.ContactTypeCode.ToUpper().Trim() == "INDIVIDUAL")
            {
                ret.Append(this.Contact.Name.Trim());
                ret.Append(delimiter);
            }
            //Append the client name if the addressee is different (which is typically the case)
            if (this.DefaultAddressSetting == false && this.Addressee.ToUpper().Trim() != this.OMSFile.Client.ClientName.ToUpper().Trim())
            {
                ret.Append(this.OMSFile.Client.ClientName.Trim());
                ret.Append(delimiter);
            }
        }


        private void AppendAddresseeOrContactName(string delimiter, System.Text.StringBuilder ret)
        {
            if (this.Addressee.Trim() != "")
            {
                ret.Append(this.Addressee.Trim());
                ret.Append(delimiter);
            }
            else
            {
                ret.Append(this.Contact.Name.Trim());
                ret.Append(delimiter);
            }
        }


        private void ContactLinksAddressBuilder(string delimiter, System.Text.StringBuilder ret)
        {
            AppendAddresseeOrContactName(delimiter, ret);
            string employername = CheckForContactLinksEmployers();
            if (!string.IsNullOrEmpty(employername))
            {
                ret.Append(employername);
                ret.Append(delimiter);
            }
            BuildAddress(delimiter, ret);
        }


        private string GetAssociateDefaultAddressType()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            IDataParameter[] pars = new IDataParameter[2];
            pars[0] = Session.CurrentSession.CurrentConnection.CreateParameter("contID", this.Contact.ID);
            pars[1] = Session.CurrentSession.CurrentConnection.CreateParameter("defaddID", this.DefaultAddress.ID);
            string sql = "select contCode from dbContactAddresses where contID = @contID and contaddID = @defaddID";
            System.Data.DataTable dtcontCode = connection.ExecuteSQL(sql, pars);
            return Convert.ToString(dtcontCode.Rows[0]["contCode"]);
        }


        public event EventHandler<KeyValueCollectionEventArgs> MultiEmployersPromptHandler;


        private string CheckForContactLinksEmployers()
        {
            KeyValueCollection kvc = new KeyValueCollection() { {"contID", this.Contact.ID} };
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.CurrentConnection.CreateParameter("contID", this.Contact.ID);

            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            string sql = "select c.contName, cl.contLinkID from dbContactLinks cl inner join dbContact c on c.contID = cl.contLinkID where cl.contID = @contID and cl.contLinkCode = 'EMPR'";
            System.Data.DataTable dt = connection.ExecuteSQL(sql, pars);
            if (dt != null && dt.Rows.Count > 0)
            {
                switch (dt.Rows.Count)
                {
                    case 1:
                        return Convert.ToString(dt.Rows[0]["contName"]);

                    default:
                        if (MultiEmployersPromptHandler != null)
                        {
                            var args = new KeyValueCollectionEventArgs();
                            MultiEmployersPromptHandler(this, args);
                            if (args.Collection != null)
                                return Convert.ToString(args.Collection["contName"].Value);
                            else
                                return null;
                        }
                        else
                            return null;
                }
            }
            return "";
        }


        private void BuildAddress(string delimiter, System.Text.StringBuilder ret)
        {
            //Now build the address
            ret.Append(DefaultAddress.GetAddressString(delimiter));
            ret.Append(delimiter);
            ret.Replace(delimiter + delimiter, delimiter);
        }


        private void CheckNamingRegistryKey(string delimiter, System.Text.StringBuilder ret)
        {
            ApplicationSetting stdIndividualContactAddressing = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "stdIndividualContactAddressing", "False");
            if (!stdIndividualContactAddressing.ToBoolean())
            {
                if (this.Contact.CurrentContactType.GeneralType.ToString().ToUpper().Trim() != "INDIVIDUAL")
                    TestAddresseeIsNotContactName(delimiter, ret);
            }
            else
            {
                if (this.Contact.ContactTypeCode.ToUpper().Trim() != "INDIVIDUAL")
                    TestAddresseeIsNotContactName(delimiter, ret);
            }
        }


        private void TestAddresseeIsNotContactName(string delimiter, StringBuilder ret)
        {
            if (this.Addressee.ToUpper().Trim() != "" && this.Addressee.ToUpper().Trim() != this.Contact.Name.ToUpper().Trim())
            {
                ret.Append(this.Contact.Name.Trim());
                ret.Append(delimiter);
            }
        }


        private bool CheckContactLinksAddressingRegistryKey(string delimiter, System.Text.StringBuilder ret)
        {
            ApplicationSetting ContactLinksAddressing = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "ContactLinksAddressing", "False");
            if (ContactLinksAddressing.ToBoolean())
            {
                if (GetAssociateDefaultAddressType() == "WORK")
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }


        /// <summary>
        /// Gets the fax numbers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataView GetFaxNumbers()
        {
            return Contact.GetNumbers("FAX");
        }


        /// <summary>
        /// Gets the mobile numbers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataView GetMobileNumbers()
        {
            return Contact.GetNumbers("MOBILE");
        }

        /// <summary>
        /// Gets the telephone numbers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataView GetTelephoneNumbers()
        {
            return Contact.GetNumbers("TELEPHONE");
        }


        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <remarks></remarks>
        public void Delete()
        {
            SetExtraInfo("assocactive", false);
        }

        /// <summary>
        /// Restores this instance.
        /// </summary>
        /// <remarks></remarks>
        public void Restore()
        {
            SetExtraInfo("assocactive", true);
        }

        /// <summary>
        /// Textual representation of the associate.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.Contact.Name;
        }

        /// <summary>
        /// Updates Extended Data for created File.
        /// </summary>
        /// <remarks></remarks>
        public void UpdateExtendedData(ExtendedDataList extData, long id)
        {
            if (extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in extData)
                {
                    ext.UpdateExtendedData(id);
                }
            }
        }

        /// <summary>
        /// Initializes rowguid for created Associate.
        /// </summary>
        /// <remarks></remarks>
        public void InitializeKey()
        {
            if (Key == Guid.Empty)
            {
                Key = Guid.NewGuid();
            }
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
                    AssociateType t = CurrentAssociateType;
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

        #region IOMSType


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

        /// <summary>
        /// Gets an OMS Type based on the associate type off this current instance of an associate object.
        /// </summary>
        /// <returns>A OMSType with information needed to represented this type of associate.</returns>
        /// <remarks></remarks>
        public virtual OMSType GetOMSType()
        {
            return FWBS.OMS.AssociateType.GetAssociateType(AssocType);
        }

        #endregion

        #region ISecurable Members

        /// <summary>
        /// Gets the security id.
        /// </summary>
        /// <remarks></remarks>
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
                throw new NotSupportedException("Security Flags are not valid on an Associate");
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
        /// <remarks></remarks>
        private void CheckPermissions()
        {
            bool isnew = IsNew;
            bool isdirty = IsDirty;

            if (isnew)
            {
                new FilePermission(OMSFile, StandardPermissionType.CreateAssociate).Check();
                new SystemPermission(StandardPermissionType.CreateAssociate).Check();
                new FileActivity(OMSFile, FileStatusActivityType.AssociateCreation).Check();
            }
            else if (isdirty)
            {
                new FilePermission(OMSFile, StandardPermissionType.UpdateAssociate).Check();
                new SystemPermission(StandardPermissionType.UpdateAssociate).Check();
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
            if (Session.CurrentSession.CurrentAssociate != this)
                Session.CurrentSession.CurrentAssociate = this;
            if (Session.CurrentSession.CurrentFile != this.OMSFile)
                Session.CurrentSession.CurrentFile = this.OMSFile;
            if (Session.CurrentSession.CurrentClient != this.OMSFile.Client)
                Session.CurrentSession.CurrentClient = this.OMSFile.Client;
        }

        #endregion
    }

    /// <summary>
    /// Implments a collection of associates.
    /// </summary>
    /// <remarks></remarks>
    public class AssociateCollection : System.Collections.IEnumerator, System.Collections.IEnumerable
    {
        #region Fields

        /// <summary>
        /// An internal data table that holds a collection of associates.
        /// </summary>
        private DataTable _associates = null;

        /// <summary>
        /// All Associates created are a Copy not a reference
        /// </summary>
        private bool _copyonly = false;

        /// <summary>
        /// A reference to the file being created.
        /// </summary>
        private OMSFile _file = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor not used.
        /// </summary>
        /// <remarks></remarks>
        private AssociateCollection() { }


        /// <summary>
        /// Constructor to allow an already formed data table to be passed to it
        /// and become the data source.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="file">The file.</param>
        /// <remarks></remarks>
        internal AssociateCollection(ref DataTable dt, OMSFile file)
        {
            _associates = dt;
            _file = file;
            //Filter out any inactive contacts and sort by a prefered order.
            DataView vw = _associates.DefaultView;
            vw.RowFilter = "assocActive = true";
            vw.Sort = "assocorder";
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the Associate object based on the given contact.
        /// This is the same existence check as the Contains method.
        /// </summary>
        /// <remarks></remarks>
        private Associate this[Contact contact]
        {
            get
            {
                if (contact == null) return null;
                DataView cont = new DataView(_associates);
                cont.RowFilter = "contid = '" + contact.ID + "'";
                if (cont.Count > 0)
                {
                    DataRow current = cont[0].Row;
                    if (_copyonly)
                        return new Associate(current, _file, _copyonly);
                    else
                        return new Associate(current, _file);
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets an associate using are prefered order ordinal value.
        /// </summary>
        /// <remarks></remarks>
        public Associate this[int order]
        {
            get
            {
                if (Count < (order + 1))
                {
                    order = Count - 1;
                }

                if (order < 0)
                {
                    order = 0;
                }

                //Get an associate object based on the row number.
                DataRow current = _associates.DefaultView[order].Row;
                if (_copyonly)
                    return new Associate(current, _file, _copyonly);
                else
                    return new Associate(current, _file);
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Adds an associate to the end of the collection.
        /// </summary>
        /// <param name="associate">Associate to add.</param>
        /// <returns>True, if the item was successfully added.</returns>
        /// <remarks></remarks>
        public bool Add(Associate associate)
        {
            return Add(associate, Count);
        }

        /// <summary>
        /// Changes the order of an associate within the collection.
        /// </summary>
        /// <param name="associate">The associate to change the order of.</param>
        /// <param name="order">The new ord id.</param>
        /// <remarks></remarks>
        public void ChangeOrder(Associate associate, int order)
        {
            if (Contains(associate))
            {
                Add(associate, order);
            }
        }

        /// <summary>
        /// Adds an associate to a specific position within the collection.
        /// </summary>
        /// <param name="associate">Associate to add.</param>
        /// <param name="order">The prefered ordinal position.</param>
        /// <returns>True, if the item was successfully added.</returns>
        /// <remarks></remarks>
        public bool Add(Associate associate, int order)
        {
            try
            {
                if (associate == null || associate.Contact == null) return false;

                bool exists = Contains(associate);

                //Dont let a priority value go higher than the associate count.
                if ((order + 1) > Count) order = Count;
                //Dont let the priority be lower than zero.
                if (order < 0) order = 0;

                DataView vw = new DataView(_associates);
                vw.Sort = "assocorder";
                if (exists)
                    vw.RowFilter = "assocorder >= '" + order + "' and contid <> '" + associate.Contact.ID + "'";
                else
                    vw.RowFilter = "assocorder >= '" + order + "'";

                int ctr = 0;
                foreach (DataRowView row in vw)
                {
                    ctr++;
                    unchecked
                    {
                        row["assocorder"] = (byte)(order + ctr);
                    }
                }

                //If the associate already exists then update its order.
                if (exists)
                {
                    DataView existing = new DataView(_associates);
                    existing.RowFilter = String.Format("associd = '{0}'", associate.ID);
                    if (existing.Count > 0)
                    {
                        existing[0]["assocorder"] = order;
                    }
                    else
                    {
                        existing.RowFilter = String.Format("contid = '{0}'", associate.Contact.ID);
                        if (existing.Count > 0)
                        {
                            existing[0]["assocorder"] = order;
                        }
                    }
                }

                //Add the associate if it does not exist.
                if (!exists)
                {
                    // Add Row to Associates Table
                    DataRow dr = _associates.NewRow();
                    associate.Clone(ref dr);
                    dr["assocorder"] = order;
                    dr["assocactive"] = true;
                    
                    _associates.Rows.Add(dr);
                }

                associate.SetAssociateInfo(_file, associate.Contact, associate.AssocType);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes an associate at the specific prefered order.
        /// </summary>
        /// <param name="order">The ordinal value to remove.</param>
        /// <remarks></remarks>
        public void RemoveAt(int order)
        {
            _associates.DefaultView.Delete(order);
        }

        /// <summary>
        /// Removes the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <remarks></remarks>
        public void Remove(Contact contact)
        {
            if (contact == null) return;
            DataView cont = new DataView(_associates);
            cont.RowFilter = "contid = '" + contact.ID + "'";
            if (cont.Count > 0)
                cont[0].Row.Delete();
        }

        /// <summary>
        /// Checks to see if a specified associate already exists within the
        /// collection.
        /// </summary>
        /// <param name="associate">The associate to check for.</param>
        /// <returns>True, if the associate already exists.</returns>
        /// <remarks></remarks>
        public bool Contains(Associate associate)
        {
            if (associate == null) return false;
            DataView assoc = new DataView(_associates);
            assoc.RowFilter = "associd = '" + associate.ID + "' and contid = '" + associate.Contact.ID + "'";
            if (assoc.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks to see if a specified contact already exists within the
        /// associates collection.
        /// </summary>
        /// <param name="contact">The contact to check for.</param>
        /// <returns>True, if the contact already exists.</returns>
        /// <remarks></remarks>
        public bool Contains(Contact contact)
        {
            if (contact == null) return false;
            DataView cont = new DataView(_associates);
            cont.RowFilter = "contid = '" + contact.ID + "'";
            if (cont.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Clears the whole associate list.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            _associates.Clear();
        }

        /// <summary>
        /// Gets the associate data view of the file.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataView GetActiveAssociates()
        {
            //Filter out any inactive contacts and sort by a prefered order.
            DataView vw = new DataView(_associates);
            vw.RowFilter = "assocActive = true";
            vw.Sort = "assocorder";
            return vw;
        }


        /// <summary>
        /// Gets all the associates within the collection.
        /// </summary>
        /// <returns>A data table of associates.</returns>
        /// <remarks></remarks>
        public DataTable GetAssociates()
        {
            return _associates;
        }

        /// <summary>
        /// Copies only those associates within the selectedAssociates id array.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <param name="selectedAssociates">The selected associates.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CopyTo(OMSFile destination, long[] selectedAssociates)
        {
            if (_file.ClientID != destination.ClientID)
                throw new OMSException2("ERRASSOCCOPYTO", "%ASSOCIATE% copying cannot continue as the source and destination %FILES% belong to different %CLIENTS%", "");

            int init_count = destination.Associates.Count;

            if (selectedAssociates == null)
            {
                using (DataView dvassocs = GetActiveAssociates())
                {

                    foreach (DataRowView drv in dvassocs)
                    {
                        Associate orig_assoc = new Associate(drv.Row, _file);

                        if (destination.Associates.Contains(orig_assoc.Contact))
                        {
                            Associate assoc = destination.Associates[orig_assoc.Contact];
                            if (assoc != null)
                                assoc.SetExtraInfo("assocActive", true);
                        }
                        else
                        {
                            Associate assoc = new Associate(drv.Row, destination, true);
                            assoc.SetExtraInfo("assocActive", true);
                            destination.Associates.Add(assoc);
                        }
                    }
                }
            }
            else
            {
                using (DataView dvassocs = new DataView(_associates))
                {

                    foreach (long id in selectedAssociates)
                    {
                        dvassocs.RowFilter = "assocID = " + Common.SQLRoutines.RemoveRubbish(id.ToString());
                        if (dvassocs.Count > 0)
                        {
                            DataRowView drv = dvassocs[0];

                            Associate orig_assoc = new Associate(drv.Row, _file);

                            if (destination.Associates.Contains(orig_assoc.Contact))
                            {
                                Associate assoc = destination.Associates[orig_assoc.Contact];
                                if (assoc != null)
                                    assoc.SetExtraInfo("assocActive", true);
                            }
                            else
                            {
                                Associate assoc = new Associate(drv.Row, destination, true);
                                destination.Associates.Add(assoc);
                            }
                        }
                    }
                }

            }

            return destination.Associates.Count - init_count;
        }

        /// <summary>
        /// Copies all associates to the specified destination file.
        /// It does not copy CLIENT associates that already exist.
        /// </summary>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CopyTo(OMSFile destination)
        {
            return CopyTo(destination, null as long[]);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private int position = -1;
        /// <summary>
        /// Gets or sets a value indicating whether [copy only].
        /// </summary>
        /// <value><c>true</c> if [copy only]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool CopyOnly
        {
            get
            {
                return _copyonly;
            }
            set
            {
                _copyonly = value;
            }
        }
        /// <summary>
        /// Gets the number of associates currently in the collection.
        /// </summary>
        /// <remarks></remarks>
        public int Count
        {
            get
            {
                return _associates.DefaultView.Count;
            }
        }

        #endregion

        #region IEnumerator Implementation

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>The current element in the collection.</returns>
        ///   
        /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
        /// <remarks></remarks>
        public object Current
        {
            get
            {
                return this[position];
            }
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <remarks></remarks>
        public void Reset()
        {
            position = -1;
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
        /// <remarks></remarks>
        public bool MoveNext()
        {
            position++;
            if (position < Count)
                return true;
            else
                return false;
        }

        #endregion

        #region IEnumerable Implementation

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        /// <remarks></remarks>
        public System.Collections.IEnumerator GetEnumerator()
        {
            Reset();
            return (System.Collections.IEnumerator)this;
        }

        #endregion


        #region Static Methods


        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int CopyTo(OMSFile source, OMSFile destination)
        {
            return source.Associates.CopyTo(destination);
        }


        #endregion
    }

    public class KeyValueCollectionEventArgs : EventArgs
    {
        public KeyValueCollection Collection { get; set; }
    }

}

