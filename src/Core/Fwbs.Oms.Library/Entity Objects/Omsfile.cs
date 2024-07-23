using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using FWBS.OMS.Data.Exceptions;
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
    public interface IFile
    {
        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <remarks></remarks>
        long ID { get; }
        /// <summary>
        /// Gets the file no.
        /// </summary>
        /// <remarks></remarks>
        string FileNo { get; }

    }




    /// <summary>
    /// An OMS file AKA Matter object holds information on tasks, documents and matter specific things
    /// for clients.
    /// This file object can be used with the enquiry engine.
    /// This object is an OMS configurable type that can appear different to other types of files.
    /// </summary>
    /// <remarks></remarks>
    [Security.SecurableType("FILE")]
    public class OMSFile : PasswordProtectedBase, IOMSType, IDisposable, IExtendedDataCompatible, IAlert, IConditional, Security.ISecurable, IFile
    {
        #region Events
        /// <summary>
        /// 
        /// </summary>
        private const string ExtFileBalDatalist = "_DSFILTIMSTATS";
        /// <summary>
        /// 
        /// </summary>
        private const string ExtClientBalDatalist = "_DSCLITIMSTATS";

        /// <summary>
        /// An event that gets raised when the user needs to be prompted for something.
        /// </summary>
        /// <remarks></remarks>
        public event PromptEventHandler Prompt = null;

        /// <summary>
        /// Occurs when [updating].
        /// </summary>
        /// <remarks></remarks>
        public event System.ComponentModel.CancelEventHandler Updating;
        /// <summary>
        /// Occurs when [updated].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler Updated;

        /// <summary>
        /// Occurs when [refreshing].
        /// </summary>
        /// <remarks></remarks>
        public event System.ComponentModel.CancelEventHandler Refreshing;
        /// <summary>
        /// Occurs when [refreshed].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler Refreshed;
        #endregion

        #region Fields
        /// <summary>
        /// A temp variable to skip the password check on a refresh.
        /// </summary>
        private bool _checkPassword = true;

        /// <summary>
        /// The Mile Stone Plan
        /// </summary>
        private Milestones_OMS2K _milestoneplan = null;
        /// <summary>
        /// Internal data source.
        /// </summary>
        private DataSet _file = null;

        /// <summary>
        /// Sets the Default Tab when called from the WinUI Layer
        /// </summary>
        private string _defaultTab = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
        internal const string Sql = "select * from dbFile";
        /// <summary>
        /// A select statement used to update file events.
        /// </summary>
        internal const string Sql_Events = "select * from dbfileevents";
        /// <summary>
        /// A select statement used to update file events.
        /// </summary>
        internal const string Sql_Phases = "select * from dbfilephase";
        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
        public const string Table = "FILE";
        /// <summary>
        /// The associates information table name.
        /// </summary>
        public const string Table_Associates = "ASSOCIATES";
        /// <summary>
        /// The Time Records information table name.
        /// </summary>
        public const string Table_TimeRecords = "TIMERECORDING";
        /// <summary>
        /// The Time Stats information table name.
        /// </summary>
        public const string Table_TimeStats = "TIMESTATS";
        /// <summary>
        /// The file phases information table name.
        /// </summary>
        public const string Table_Phases = "PHASES";
        /// <summary>
        /// Associate headings table name.
        /// </summary>
        public const string Table_Headings = "HEADINGS";

        /// <summary>
        /// The events information table name.
        /// </summary>
        public const string Table_Events = "EVENTS";
        /// <summary>
        /// Holds the different extended data sources for the file.
        /// </summary>
        private ExtendedDataList _extData = null;

        /// <summary>
        /// The fund type of the file type and this file.
        /// </summary>
        private FundType _fundType = null;

        /// <summary>
        /// A collection of associates of the file.
        /// </summary>
        private AssociateCollection _assocs = null;

        /// <summary>
        /// A collection of associates of the file.
        /// </summary>
        private TimeCollection _timerecords = null;

        /// <summary>
        /// Currency Object for the File
        /// </summary>
        private Currency _currency = new Currency();

        /// <summary>
        /// Holds any tasks that the file may need to append to.
        /// </summary>
        private Tasks _tasks = null;

        /// <summary>
        /// Holds any appointments that the file may need to append to.
        /// </summary>
        private Appointments _appointments = null;

        /// <summary>
        /// A flag that specifies whether a default jobs list is created on creation of a file.
        /// </summary>
        private bool _generateDefJobs = true;

        /// <summary>
        /// Pre-instruction / Quote file status.
        /// </summary>
        public const string PREINSTRUCTION = "LIVEPREINST";

        /// <summary>
        /// Temporary alert.  This is currently used so that the WinUI can add a message to
        /// the select client file screen.  This may be reviewed later.
        /// </summary>
        private Alert _tempAlert;

        /// <summary>
        /// Stores the default/currently used file phase.
        /// </summary>
        private FilePhase _currentPhase = null;

        /// <summary>
        /// An array list of interactive file profiles.
        /// </summary>
        private System.Collections.ArrayList _interactiveProfiles = null;


        private DataTable _fileeventrecords  = null;


        /// <summary>
        /// 
        /// </summary>
        private bool _isdirty;

        #endregion

        #region Event Methods

        /// <summary>
        /// Raises the prompt event.
        /// </summary>
        /// <param name="e">Ask event arguments.</param>
        /// <remarks></remarks>
        private void OnPrompt(PromptEventArgs e)
        {
            if (Prompt != null)
                Prompt(this, e);
            else
            {
                Session.CurrentSession.OnPrompt(this, e);
            }
        }

        /// <summary>
        /// Called when [refreshed].
        /// </summary>
        /// <remarks></remarks>
        private void OnRefreshed()
        {
            if (Refreshed != null)
                Refreshed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:Refreshing"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void OnRefreshing(System.ComponentModel.CancelEventArgs e)
        {
            if (Refreshing != null)
                Refreshing(this, e);
        }

        /// <summary>
        /// Called when [updated].
        /// </summary>
        /// <remarks></remarks>
        private void OnUpdated()
        {
            if (Updated != null)
                Updated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="E:Updating"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void OnUpdating(System.ComponentModel.CancelEventArgs e)
        {
            if (Updating != null)
                Updating(this, e);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new OMS file object.  This routine is used by the enquiry engine
        /// to create new file object.
        /// </summary>
        /// <remarks></remarks>
        internal OMSFile()
        {
            FetchSchema();
            ISOCode = Session.CurrentSession.DefaultCurrency;
            ApplyClientAssociates(Session.CurrentSession.CurrentClient);

            BuildXML();

        }

        /// <summary>
        /// Creates a file with a specified file type.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <remarks></remarks>
        public OMSFile(FileType fileType)
            : this(fileType, Session.CurrentSession.CurrentClient)
        {

        }

        /// <summary>
        /// Creates a file with a specified file type and a parented client.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="client">The client.</param>
        /// <remarks></remarks>
        public OMSFile(FileType fileType, Client client)
        {
            Session.CurrentSession.CheckLoggedIn();
            FetchSchema();
            SetExtraInfo("clid", client.ClientID);
            SetExtraInfo("filetype", fileType.Code);
            SetExtraInfo("fileno", "");
            ISOCode = Session.CurrentSession.DefaultCurrency;

            ApplyClientAssociates(client);

            if (client.IsPreClient)
            {
                Status = PREINSTRUCTION;
            }

            ApplyMultiAssociates(fileType);

            BuildXML();
        }


        /// <summary>
        /// Initialised an existing associate object with the specified identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal OMSFile(long id)
        {
            Fetch(id, null);

            //An edit contructor should add the object created to the session memory collection.
            Session.CurrentSession.CurrentFiles.Add(ID.ToString(), this);

            BuildXML();

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }


        #endregion

        #region Constructor Used Methods

        /// <summary>
        /// Applies the clients contacts as associates.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <remarks></remarks>
        private void ApplyClientAssociates(Client client)
        {
            //Set the client that is to own the file.
            if (client == null)
            {
                throw new OMSException(HelpIndexes.CurrentClientMustExist);
            }
            else
            {
                string assoctype = "CLIENT";

                //Set all the client contacts to be associates.
                int count = client.Contacts.GetLength(0);
                bool AddedFlag = false;
                foreach (Contact cont in client.Contacts)
                {
                    if (cont.AssociateAs != String.Empty)
                        assoctype = cont.AssociateAs;
                    else
                        assoctype = "CLIENT";
                    if (client.DefaultContact.ID == cont.ID) AddedFlag = true;
                    Associate assoc = new Associate(cont, this, assoctype);
                    Associates.Add(assoc);
                }

                Associate defAssoc;
                if (AddedFlag == false)
                {
                    defAssoc = new Associate(client.DefaultContact, this, "CLIENT");
                    Associates.Add(defAssoc, 0);
                }

                if (client.CurrentClientType.ContactType == "INDIVIDUAL" && client.CurrentClientType.AddCombinedAssociate)
                {
                    if (count > 1)
                    {
                        defAssoc = new Associate(client.DefaultContact, this, "CLIENT");
                        defAssoc.SetExtraInfo("associd", -1);
                        defAssoc.Addressee = client.ClientName;
                        defAssoc.Salutation = client.ClientName;
                        Associates.Add(defAssoc, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Applies the multi associates from the file type.
        /// </summary>
        /// <param name="fileType">Type of the file.</param>
        /// <remarks></remarks>
        private void ApplyMultiAssociates(FileType fileType)
        {
            //Set the client that is to own the file.
            if (fileType == null)
            {
                throw new OMSException(HelpIndexes.FileTypeDoesNotExist);
            }
            else
            {
                foreach (FileType.MultiAssociate a in fileType.MultiAssociates)
                {
                    Associate assoc = new Associate(a.Contact, this, a.AssocType);
                    if (a.DefaultSalutation != "") assoc.Salutation = a.DefaultSalutation;
                    if (a.DefaultAddressee != "") assoc.Addressee = a.DefaultAddressee;
                    if (a.DefaultDDI != "") assoc.DefaultTelephoneNumber = a.DefaultDDI;
                    if (a.DefaultMobile != "") assoc.DefaultMobile = a.DefaultMobile;
                    if (a.DefaultFax != "") assoc.DefaultFaxNumber = a.DefaultFax;
                    if (a.DefaultEmail != "") assoc.DefaultEmail = a.DefaultEmail;
                    if (a.DefaultRef != "") assoc.TheirRef = a.DefaultRef;
                    assoc.AssocHeading = a.GetAssociateHeading(this);
                    Associates.Add(assoc);
                }
            }
        }

        /// <summary>
        /// Fetches the file schema and sets some default values.
        /// </summary>
        /// <remarks></remarks>
        private void FetchSchema()
        {
            //Make sure that the parameters list is cleared after use.	
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, 0);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
            _file = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprFileRecord", new string[1] { Table }, paramlist);

            // Rename the default tables, must be kept in the same order
            NameTables();

            DataTable dt = _file.Tables[Table];
            _file.Tables[Table].Columns["fileid"].AutoIncrement = true;

            //Add a new record.
            Global.CreateBlankRecord(ref dt, true);

            //Set the created by and created date of the item.
            SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
            SetExtraInfo("Created", DateTime.Now);
            SetExtraInfo("fileguid", System.Guid.NewGuid());

            //Set other default / inherited information.
            SetExtraInfo("brid", Session.CurrentSession.ID);
            try
            {
                PrincipleFeeEarner = Session.CurrentSession.CurrentFeeEarner;
                ResponsibleFeeEarner = Session.CurrentSession.CurrentFeeEarner.ResponsibleTo;
            }
            catch (Exception ex)
            {
                throw new OMSException2("ERRNOFE", "Your user settings 'Work For' have not been set.", "", ex);
            }
            try
            {
                Branch = Session.CurrentSession.CurrentFeeEarner.Branch;
            }
            catch
            { }
        }

        /// <summary>
        /// Constructs an omfile.
        /// </summary>
        /// <param name="id">The file id to retrieve.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
        private void Fetch(long id, DataRow merge)
        {
            //Make sure that the parameters list is cleared after use.	
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, id);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
            DataSet data = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprFileRecord", new string[1] { Table }, paramlist);

            if ((data == null) || (data.Tables.Count == 0) || (data.Tables[Table].Rows.Count == 0))
            {
                throw new OMSException2(HelpIndexes.OMSFileNotFound.ToString(), "", "",  null, true, id.ToString());
            }

            if (merge != null)
                Global.Merge(data.Tables[Table].Rows[0], merge);

            _file = data;

            timestamp = DateTime.UtcNow;

            _file.Tables[Table].Columns.Add(new DataColumn("fileJointDesc", typeof(string), "[fileno] + ' : ' + [filedesc]"));

            // Rename the default tables, must be kept in the same order.
            NameTables();

            //Only check passwords when the basic security type is being used.
            if (!Session.CurrentSession.AdvancedSecurityEnabled)
            {
                //Ask for the password on open.  Perhaps extend the file object so that the
                //password can be applied to different levels of security, like viewing, changing
                //opeing, deleting etc...
                if (_checkPassword)
                {    
                    Client.ValidatePassword();
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
            }


            //Refresh the security
            SecurityManager.CurrentManager.Refresh(this);
        }



        /// <summary>
        /// Names the tables within the data set.
        /// </summary>
        /// <remarks></remarks>
        private void NameTables()
        {
            // Rename the default tables, must be kept in the same order
            _file.Tables[1].TableName = Table_Events;
            if (_file.Tables.Count > 2)
                _file.Tables[2].TableName = Table_Phases;
        }

        #endregion

        #region Tasks and Appointment Routines

        /// <summary>
        /// Gets the tasks list of the file.
        /// </summary>
        /// <remarks></remarks>
        public Tasks Tasks
        {
            get
            {
                if (_tasks == null)
                    _tasks = new Tasks(this);
                return _tasks;
            }
        }

        /// <summary>
        /// Gets the tasks list of the file.
        /// </summary>
        /// <remarks></remarks>
        public Appointments Appointments
        {
            get
            {
                if (_appointments == null)
                    _appointments = new Appointments(this);
                return _appointments;
            }
        }

        #endregion

        #region TimeRecord Specific Methods

        /// <summary>
        /// Gets the Time Recording collection in object form.
        /// </summary>
        /// <remarks></remarks>
        public TimeCollection TimeRecords
        {
            get
            {
                if (_timerecords == null)
                {
                    DownloadTimeRecords();
                    DataTable dt = _file.Tables[Table_TimeRecords];
                    _timerecords = new TimeCollection(ref dt, this);
                }
                return _timerecords;
            }
        }

        /// <summary>
        /// Gets the Time Records table for the file when it is needed.
        /// </summary>
        /// <remarks></remarks>
        private void DownloadTimeRecords()
        {
            if (_file.Tables.Contains(Table_TimeRecords) == false)
                _file.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeRecords", Table_TimeRecords, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));
        }

        /// <summary>
        /// Used to work out the balance based on the filter on the data table Table_TimeStats, if no rows returned
        /// return 0
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>value of balance</returns>
        /// <remarks></remarks>
        private decimal TimeBalance(string filter)
        {
            // Filter the table according to filter, on error return 0
            try
            {
                DownloadTimeStats();

                _file.Tables[Table_TimeStats].DefaultView.RowFilter = filter;
                if (_file.Tables[Table_TimeStats].DefaultView.Count == 0)
                {
                    // Default Row has no Rows so return 0
                    return 0;
                }
                else
                {
                    decimal tmp = 0;
                    foreach (DataRowView rw in _file.Tables[Table_TimeStats].DefaultView)
                    {
                        tmp = tmp + Convert.ToDecimal(rw["totcharge"]);
                    }
                    return tmp;
                }
            }
            catch
            {
                return 0;
            }


        }

        /// <summary>
        /// Gets the Time Stats table for the file when it is needed.
        /// </summary>
        /// <remarks></remarks>
        private void DownloadTimeStats()
        {
            CheckExternalBalances();

            if (_file.Tables.Contains(Table_TimeStats) == false)
            {
                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    _file.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeStats", Table_TimeStats, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));
                else
                {
                    try
                    {
                        Client.SetExternalBalanceOnline(this.Client);
                        DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(ExtFileBalDatalist);
                        FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                        param.Add("CLNO", this.Client.ClientNo);
                        param.Add("FILENO", this.FileNo);
                        param.Add("FILEID", this.ID);
                        dl.ChangeParameters(param);
                        DataTable dt = dl.Run() as DataTable;
                        dt.TableName = Table_TimeStats;
                        if (dt.Rows.Count > 0)
                            _file.Tables.Add(dt);
                        else
                            Client.SetExternalBalanceOffline(_file, this.Client);
                    }
                    catch (FWBS.OMS.Data.Exceptions.ConnectionException ex)
                    {
                        Trace.TraceError(ex.Message);
                        Client.SetExternalBalanceOffline(_file, this.Client);
                    }
                }
            }
        }


        /// <summary>
        /// Checks the external balances.
        /// </summary>
        /// <remarks></remarks>
        private static void CheckExternalBalances()
        {
            if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == null)
                FWBS.OMS.Session.CurrentSession.UseExternalBalances = FWBS.OMS.EnquiryEngine.DataLists.Exists(ExtClientBalDatalist) && FWBS.OMS.EnquiryEngine.DataLists.Exists(ExtFileBalDatalist);
        }
        #endregion

        #region Associate Specific Methods

        /// <summary>
        /// Gets the associate collection in object form.
        /// </summary>
        /// <remarks></remarks>
        public AssociateCollection Associates
        {
            get
            {
                if (_assocs == null)
                {
                    DownloadAssociates();
                    DataTable dt = _file.Tables[Table_Associates];
                    _assocs = new AssociateCollection(ref dt, this);
                }
                return _assocs;
            }
        }

        /// <summary>
        /// Downloads the associates.
        /// </summary>
        /// <remarks></remarks>
        private void DownloadAssociates()
        {
            DownloadAssociates(false);
        }



        /// <summary>
        /// Gets the associate table for the file when it is needed.
        /// </summary>
        /// <param name="force">Forces the download when called.</param>
        /// <remarks></remarks>
        private void DownloadAssociates(bool force)
        {
            if (force)
            {
                _assocs = null;
                if (_file.Tables.Contains(Table_Associates))
                    _file.Tables.Remove(Table_Associates);
            }
            if (_file.Tables.Contains(Table_Associates) == false)
                _file.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprAssocList", Table_Associates, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));

            _file.Tables[Table_Associates].Columns["associd"].AutoIncrement = true;
            if (_file.Tables[Table_Associates].PrimaryKey == null || _file.Tables[Table_Associates].PrimaryKey.Length == 0)
                _file.Tables[Table_Associates].PrimaryKey = new DataColumn[1] { _file.Tables[Table_Associates].Columns["associd"] };
            DownloadHeadings(Client, this);
        }

        /// <summary>
        /// Gets or sets the default associate of the file.
        /// </summary>
        /// <value>The default associate.</value>
        /// <remarks></remarks>
        public Associate DefaultAssociate
        {
            get
            {
                DownloadAssociates();

                return Associates[0];
            }
            set
            {
                DownloadAssociates();

                if (value != null)
                {
                    if (Associates.Contains(value))
                    {
                        Associates.Add(value, 0);
                    }
                    else
                    {
                        throw new OMSException2("15004", "The specified associate does not exist under the file.");
                    }

                }

            }
        }


        /// <summary>
        /// Gets the associate headings.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetAssociateHeadings()
        {
            return GetAssociateHeadings(false);
        }

        /// <summary>
        /// Gets the associated heading of the current associate.
        /// </summary>
        /// <param name="IncludeNull">if set to <c>true</c> [include null].</param>
        /// <returns>A data table.</returns>
        /// <remarks></remarks>
        public DataTable GetAssociateHeadings(bool IncludeNull)
        {
            if (_file.Tables.Contains(Table_Headings) == false)
                DownloadHeadings(Client, this);

            DataTable dt = _file.Tables[Table_Headings].Copy();
            if (IncludeNull)
            {
                DataRow nr = dt.NewRow();
                nr["fmtdesc"] = DBNull.Value;
                dt.Rows.InsertAt(nr, 0);
            }
            return dt;
        }

        /// <summary>
        /// Downloads from the database the associate headings for the current file and client of the
        /// associate.
        /// </summary>
        /// <param name="client">Client info.</param>
        /// <param name="file">File info.</param>
        /// <remarks></remarks>
        private void DownloadHeadings(Client client, OMSFile file)
        {
            //Get the associate headings.
            if (_file.Tables.Contains(Table_Headings))
                _file.Tables.Remove(Table_Headings);

            DataTable lkp = CodeLookup.GetLookups("ASSOCHEAD");
            DataTable headings = new DataTable(Table_Headings);
            headings.Columns.Add("fmtdesc", typeof(string));

            if (client.ClientName.Trim() != "")
            {
                string txt = Session.CurrentSession.Resources.GetResource("OURCLIENT", "Our %CLIENT% ", "").Text;
                if (!txt.EndsWith(" ")) txt += " ";
                txt += client.ClientName;
                headings.Rows.Add(txt);
                txt = Session.CurrentSession.Resources.GetResource("OURMUTCLIENT", "Our Mutual %CLIENT% ", "").Text;
                if (!txt.EndsWith(" ")) txt += " ";
                txt += client.ClientName;
                headings.Rows.Add(txt);
            }

            if (file.FileDescription.Trim() != "")
            {
                headings.Rows.Add(file.FileDescription);
            }

            Terminology terminology = Session.CurrentSession.Terminology;
            foreach (DataRow row in lkp.Rows)
            {
                headings.Rows.Add(terminology.Parse(Convert.ToString(row["cddesc"]), true));
            }
            headings.AcceptChanges();

            lkp.Dispose();
            lkp = null;

            _file.Tables.Add(headings);
        }

        #endregion

        #region Interactive Profiles

        /// <summary>
        /// Adds an interactive file profile to the file so that it can be created
        /// when the rest of the file gets created.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <remarks></remarks>
        internal void AddInteractiveProfile(InteractiveFile profile)
        {
            if (_interactiveProfiles == null)
                _interactiveProfiles = new System.Collections.ArrayList();
            _interactiveProfiles.Add(profile);
        }

        #endregion

        #region Events Routines

        /// <summary>
        /// Gets a data view of the events by a specific code.
        /// </summary>
        /// <param name="code">The code to filter by.</param>
        /// <returns>A data view.</returns>
        /// <remarks></remarks>
        public DataView GetEvents(string code)
        {
            DataView vw = null;
            if (_file.Tables[Table_Events].Rows.Count > 0)
            {
                 vw = new DataView(_file.Tables[Table_Events]);
            }

            else
            {
                vw = new DataView(GetEvents());
            }
            vw.RowFilter = "evtype = '" + FWBS.Common.SQLRoutines.RemoveRubbish(code) + "'";                           
            return vw;
        }

        /// <summary>
        /// Gets a data view of the events.
        /// </summary>
        /// <returns>A data view.</returns>
        /// <remarks></remarks>
        public DataTable GetEvents()
        {
            if (_file.Tables[Table_Events].Rows.Count > 0)
                return _file.Tables[Table_Events];

            else
            {
                IDataParameter[] paramlist = new IDataParameter[2];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, ID);
                paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);                                
                _fileeventrecords=Session.CurrentSession.Connection.ExecuteProcedureTable("[sprFileEventRecords]", Table_Events, paramlist);                
                return _fileeventrecords;
            } 
               
        }

        /// <summary>
        /// Adds an event to the file events list.  The entries will get updated to the database
        /// when the file gets updated.
        /// </summary>
        /// <param name="code">The event code.</param>
        /// <param name="description">The brief descriptive data of the event.</param>
        /// <param name="extendedInfo">More details extended event data information.</param>
        /// <remarks></remarks>
        public void AddEvent(string code, string description, string extendedInfo)
        {
            DataRow ev = _file.Tables[Table_Events].NewRow();
            ev["fileid"] = ID;
            ev["evtype"] = code;
            ev["evtypedesc"] = CodeLookup.GetLookup("FILEEVENT", code);
            if (description.Length > 100)
            {
                ev["evdesc"] = description.Substring(0, 96) + "...";
                this.AppendNoteText(NoteAppendingLocation.Beginning, description);
            }
            else
                ev["evdesc"] = description;

            ev["evwhen"] = DateTime.Now;
            if (extendedInfo == String.Empty || extendedInfo == null)
                ev["evextended"] = DBNull.Value;
            else
                ev["evextended"] = extendedInfo;
            ev["evusrid"] = Session.CurrentSession.CurrentUser.ID;
            ev["usrfullname"] = Session.CurrentSession.CurrentUser.FullName;
            _file.Tables[Table_Events].Rows.Add(ev);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Used to get the best fit associate dependant on Contact Type
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="assocType">Contact type to look for.</param>
        /// <returns>The best fit associate object.</returns>
        /// <remarks></remarks>
        public Associate GetBestFitAssociate(string contactType, string assocType)
        {
            return GetBestFitAssociate(contactType, assocType, null);
        }

        /// <summary>
        /// Gets the best fit associate.
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="assocType">Type of the assoc.</param>
        /// <param name="defaultAssociate">The default associate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Associate GetBestFitAssociate(string contactType, string assocType, Associate defaultAssociate)
        {
            try
            {
                contactType = contactType ?? String.Empty;
                assocType = assocType ?? String.Empty;

                DownloadAssociates();

                bool prompt = false;
                string[] filter = new string[2];
                filter[0] = contactType;
                filter[1] = assocType;

                if (String.IsNullOrEmpty(contactType) && string.IsNullOrEmpty(assocType))
                    return defaultAssociate ?? this.DefaultAssociate;

                if (contactType.ToUpperInvariant() == "CLIENT")
                {
                    if (String.IsNullOrEmpty(assocType.ToUpperInvariant()))
                        return defaultAssociate ?? this.DefaultAssociate;
                    else
                    {
                        filter[0] = "*";
                        contactType = String.Empty;
                    }

                }

                using (DataView vw = new DataView(_file.Tables[Table_Associates]))
                {
                    string assocfilter = "assocActive = true";

                    if (!String.IsNullOrEmpty(contactType))
                        assocfilter += " and ContTypeCode = '" + Common.SQLRoutines.RemoveRubbish(contactType) + "'";

                    if (!String.IsNullOrEmpty(assocType))
                        assocfilter += " and AssocType = '" + Common.SQLRoutines.RemoveRubbish(assocType) + "'";

                    vw.RowFilter = assocfilter;

                    if (vw.Count == 1)
                        return Associate.GetAssociate(Common.ConvertDef.ToInt64(vw[0]["assocID"], -1));
                    else if (vw.Count > 1)
                        prompt = true;
                    else if (vw.Count <= 0)
                    {
                        prompt = true;


                        filter[0] = "*";
                        filter[1] = "*";

                        if (!String.IsNullOrEmpty(contactType))
                        {
                            assocfilter = "ContTypeCode = '" + Common.SQLRoutines.RemoveRubbish(contactType) + "' and assocActive = true";
                            vw.RowFilter = assocfilter;
                            filter[0] = contactType;
                        }
                    }

                    if (prompt)
                    {
                        string res = "";
                        if (!String.IsNullOrEmpty(contactType))
                            res += CodeLookup.GetLookup("CONTTYPE", contactType);
                        else
                            res += Session.CurrentSession.Resources.GetResource("RESALLCONTTYPES", "All Contact Types", "").Text;

                        if (!String.IsNullOrEmpty(assocType))
                            res += " - " + CodeLookup.GetLookup("SUBASSOC", assocType);
                        else
                            res += " - " + Session.CurrentSession.Resources.GetResource("RESALLASSOCTYPE", "All Associated Types", "").Text;

                        PromptEventArgs e = new PromptEventArgs(PromptType.Search, "PICKASSOCIATE", filter, Session.CurrentSession.Resources.GetMessage("PICKASSOC", "Please pick a contact that is associated to the %FILE% as '%1%' ...", "", true, res).Text);
                        OnPrompt(e);
                        Associate assoc = e.Result as Associate;
                        if (assoc == null)
                            return null;
                        else
                            return assoc;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new OMSException2("15003", "Error Getting Default Associate", ex);
            }


        }


        /// <summary>
        /// Gets the best fit associates.
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="assocType">Type of the assoc.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Associate[] GetBestFitAssociates(string contactType, string assocType)
        {
            try
            {
                List<Associate> list = new List<Associate>();

                contactType = contactType ?? String.Empty;
                assocType = assocType ?? String.Empty;

                DownloadAssociates();

                using (DataView vw = new DataView(_file.Tables[Table_Associates]))
                {
                    string assocfilter = "assocActive = true";

                    if (!String.IsNullOrEmpty(contactType))
                        assocfilter += " and ContTypeCode = '" + Common.SQLRoutines.RemoveRubbish(contactType) + "'";

                    if (!String.IsNullOrEmpty(assocType))
                        assocfilter += " and AssocType = '" + Common.SQLRoutines.RemoveRubbish(assocType) + "'";

                    vw.RowFilter = assocfilter;

                    foreach (DataRowView r in vw)
                    {
                        list.Add(Associate.GetAssociate(Common.ConvertDef.ToInt64(vw[0]["assocID"], -1)));
                    }
                }

                return list.ToArray();
            }
            catch (Exception ex)
            {
                throw new OMSException2("15003", "Error Getting Default Associate", ex);
            }
        }

        /// <summary>
        /// Skips the conflict search and adds the information to the events table.
        /// </summary>
        /// <remarks></remarks>
        private void SkipConflictSearch()
        {
            if (IsNew)
            {
                if (GetEvents("CONFLICTDONE").Count == 0)
                {
                    if (CurrentFileType.SearchOnCreate)
                    {
                        AddEvent("CONFLICTSKIPPED", "0", "");
                        SetExtraInfo("fileconflictfound", 0);
                        SetExtraInfo("fileconflictcheck", false);
                    }
                    else
                        SetExtraInfo("fileconflictcheck", false);
                }
                else
                    SetExtraInfo("fileconflictcheck", true);
            }

        }

        /// <summary>
        /// Applies a search list to the current file.
        /// </summary>
        /// <param name="count">The number of conflicts found.</param>
        /// <param name="detail">The criteria used.</param>
        /// <remarks></remarks>
        public void ApplyConflictSearch(int count, string detail)
        {
            if (IsNew == false || (IsNew == true && CurrentFileType.SearchOnCreate))
            {
                AddEvent("CONFLICTDONE", count.ToString(), detail);
                SetExtraInfo("fileconflictfound", count);
                SetExtraInfo("fileconflictcheck", true);
            }
            else
                SetExtraInfo("fileconflictcheck", false);

        }

        /// <summary>
        /// Gets the conflicts found.
        /// </summary>
        /// <remarks></remarks>
        public int ConflictsFound
        {
            get
            {
                int count = 0;

                DataView dv = GetEvents("CONFLICTDONE");

                foreach (DataRow row in dv.Table.Rows)
                {
                    object desc = row["evDesc"];
                    if (desc == DBNull.Value || desc == null)
                        continue;

                    int val = 0;

                    if (int.TryParse((string)desc, out val))
                        count += val;

                }

                return count;
            }
        }





        /// <summary>
        /// Appends the specified text to the notes field as a new line.
        /// </summary>
        /// <param name="location">The location of where the new text is going to go.</param>
        /// <param name="text">Text to append.</param>
        /// <remarks></remarks>
        public void AppendNoteText(NoteAppendingLocation location, string text)
        {
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
            SetExtraInfo("filenotes", notes);
        }

        /// <summary>
        /// Appends the specified text to the external notes field as a new line.
        /// </summary>
        /// <param name="location">The location of where the new text is going to go.</param>
        /// <param name="text">Text to append.</param>
        /// <remarks></remarks>
        public void AppendExternalNoteText(NoteAppendingLocation location, string text)
        {
            string notes = ExternalNotes;
            //UTCFIX: DM - 30/11/06 - Note stamp show local time with time offset.
            if (location == NoteAppendingLocation.Beginning)
            {
                text += Environment.NewLine;
                text += String.Format(Session.CurrentSession.Resources.GetResource("NOTESTAMP", "Note Created On : {0} @ {1} ({2:zzz}) - By : {3}", "").Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now, Session.CurrentSession.CurrentUser.FullName);
                text += notes;
                text += Environment.NewLine;
                text += "------------------------------------------------";
                notes = text;
            }
            else if (location == NoteAppendingLocation.End)
            {
                notes += Environment.NewLine;
                notes += String.Format(Session.CurrentSession.Resources.GetResource("NOTESTAMP", "Note Created On : {0} @ {1} ({2:zzz}) - By : {3}", "").Text, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), DateTime.Now, Session.CurrentSession.CurrentUser.FullName);
                notes += Environment.NewLine;
                notes += text;
                notes += Environment.NewLine;
                notes += "------------------------------------------------";
            }
            SetExtraInfo("fileexternalnotes", notes);
        }

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="tableName">Table name within the internal dataset.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
        public object GetExtraInfo(string fieldName, string tableName)
        {
            object val = this.GetExtraInfo(fieldName, tableName, 0);
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
        public object GetExtraInfo(string fieldName, string tableName, int row)
        {
            object val = _file.Tables[tableName].Rows[row][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
        }


        /// <summary>
        /// Returns a dataset of Associates for the object.
        /// </summary>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
        public DataTable GetTimeStatsDataTable()
        {
            DownloadTimeStats();
            return _file.Tables[Table_TimeStats];
        }

        /// <summary>
        /// Returns a data table of all the phases underthe file.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public DataTable GetPhases()
        {
            return _file.Tables[Table_Phases];
        }


        /// <summary>
        /// Creates the job list of the file from the configuration information from the file type.
        /// </summary>
        /// <remarks></remarks>
        public void GenerateJobList()
        {
            try
            {
                System.Xml.XmlElement el = (System.Xml.XmlElement)CurrentFileType.GetConfigRoot().SelectSingleNode("Jobs");
                if (el != null)
                {
                    foreach (System.Xml.XmlNode nd in el.ChildNodes)
                    {
                        if (nd is System.Xml.XmlElement)
                        {
                            System.Xml.XmlElement eljob = (System.Xml.XmlElement)nd;
                            string title = eljob.GetAttribute("title");
                            string type = eljob.GetAttribute("type");
                            string lib = eljob.GetAttribute("library");
                            string cat = eljob.GetAttribute("category");
                            string subcat = eljob.GetAttribute("subcategory");
                            string minorcat = eljob.GetAttribute("minorcategory");

                            PrecSaveMode savemode = PrecSaveMode.None;
                            PrecPrintMode printmode = PrecPrintMode.None;

                            try
                            {
                                savemode = (PrecSaveMode)Common.ConvertDef.ToEnum(eljob.GetAttribute("saveMode"), PrecSaveMode.None);
                            }
                            catch { }

                            try
                            {
                                printmode = (PrecPrintMode)Common.ConvertDef.ToEnum(eljob.GetAttribute("printMode"), PrecPrintMode.None);
                            }
                            catch { }

                            Precedent prec = null;
                            long id = -1;
                            try
                            {
                                id = long.Parse(title);
                            }
                            catch { }

                            if (id == -1)
                                prec = Precedent.GetPrecedent(title, type, lib, cat, subcat, PreferedLanguage, minorcat);
                            else
                                prec = Precedent.GetPrecedent(id);

                            if (prec.IsMultiPrecedent)
                                prec.GenerateJobList(this);
                            else
                            {
                                PrecedentJob job = new PrecedentJob(prec);
                                job.Associate = GetBestFitAssociate(prec.ContactType, prec.AssocType);
                                if (job.Associate == null)
                                    continue;
                                job.PrintMode = printmode;
                                job.SaveMode = savemode;
                                Session.CurrentSession.CurrentPrecedentJobList.Add(job);
                            }
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// Used to return default precedent from the Config File.
        /// </summary>
        /// <param name="type">Precedent / document type.</param>
        /// <returns>The default precedent, null if none found.</returns>
        /// <remarks></remarks>
        public Precedent GetDefaultPrecedent(string type)
        {
            try
            {
                FileType ft = CurrentFileType;
                string xpath = "defaultTemplates/template[@type = '" + System.Xml.XmlConvert.EncodeName(type) + "']";
                System.Xml.XmlElement el = ft.GetConfigRoot();
                string ret = ft.ReadAttribute(el, xpath, "");
                string title = ft.ReadAttribute(el, xpath + "/@title", "");
                string lib = ft.ReadAttribute(el, xpath + "/@library", "");
                string cat = ft.ReadAttribute(el, xpath + "/@category", "");
                string subcat = ft.ReadAttribute(el, xpath + "/@subcategory", "");
                string minorcat = ft.ReadAttribute(el, xpath + "/@minorcategory", "");

                try
                {
                    if (ret.Trim() == String.Empty)
                        return Precedent.GetPrecedent(title, type, lib, cat, subcat, PreferedLanguage, minorcat);
                    else
                    {
                        long id = long.Parse(ret);
                        return Precedent.GetPrecedent(id);
                    }
                }
                catch
                {
                    try
                    {
                        return Precedent.GetPrecedent(title, type, lib, cat, subcat, PreferedLanguage, minorcat);
                    }
                    catch
                    {
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Gets the string representation of the object.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return string.Format("{0}/{1} - {2}", Client.ClientNo, FileNo, FileDescription);
        }

        /// <summary>
        /// Creates the next file review date.
        /// </summary>
        /// <param name="newDate">The new date for the file review.</param>
        /// <param name="notes">Notes to be added to the task.</param>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public void CreateNextReviewDate(Common.DateTimeNULL newDate, string notes)
        {
            //If the review date is different from the last one set then add a item to
            //the task.
            Common.DateTimeNULL oldval = ReviewDate;

            if (newDate == DBNull.Value)
            {
                SetExtraInfo("filereviewdate", DBNull.Value);
                OnPropertyChanged(new PropertyChangedEventArgs("ReviewDate", oldval, DBNull.Value));
            }
            else
            {
                if (newDate != oldval)
                {
                    SetExtraInfo("filereviewdate", newDate.ToObject());
                    OnPropertyChanged(new PropertyChangedEventArgs("ReviewDate", oldval, newDate.ToObject()));
                    Tasks.Add(Session.CurrentSession.CurrentFeeEarner, "FILEREVIEWDATE", Session.CurrentSession.Resources.GetResource("FILEREVIEWTXT", "Review Date", "", true, new string[] { }).Text, ReviewDate, "");

                    AddEvent("REVIEWDATE", notes, "");
                }
            }
        }


        #endregion

        #region Properties
        /// <summary>
        /// Gets the current alert message.  This corresponds to the file alert.
        /// </summary>
        /// <value>The on alert message.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string OnAlertMessage
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileAlertMessage"));
            }
            set
            {
                SetExtraInfo("fileAlertMessage", value);
            }
        }

        /// <summary>
        /// Gets the current alert message.  This corresponds to the file alert.
        /// </summary>
        /// <value>The on alert level.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Int16 OnAlertLevel
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt16(GetExtraInfo("fileAlertLevel"), 1);
            }
            set
            {

                SetExtraInfo("fileAlertLevel", value);
            }
        }


        /// <summary>
        /// Gets the current phase associated to the file.
        /// </summary>
        /// <value>The current phase.</value>
        /// <remarks></remarks>
        public FilePhase CurrentPhase
        {
            get
            {
                if (_currentPhase == null)
                {
                    if (_file.Tables[Table].Columns.Contains("phID") == false)
                        _currentPhase = null;
                    else
                    {
                        object phaseid = GetExtraInfo("phID");
                        if (phaseid == DBNull.Value)
                            _currentPhase = null;
                        else
                            _currentPhase = FilePhase.GetPhase(Convert.ToInt64(phaseid));
                    }
                }
                return _currentPhase;
            }
            set
            {
                if (value == null || value.FileID != this.ID)
                {
                    _currentPhase = null;
                    SetExtraInfo("phID", DBNull.Value);
                }
                else
                {
                    _currentPhase = value;
                    SetExtraInfo("phID", _currentPhase.ID);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the nickname of a file.
        /// </summary>
        /// <value>The nickname.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Nickname
        {
            get
            {
                try
                {
                    return Convert.ToString(GetExtraInfo("fileNickname"));
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (value == null || value == String.Empty)
                        SetExtraInfo("fileNickname", DBNull.Value);
                    else
                        SetExtraInfo("fileNickname", value);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Gets or Sets the default document storage location for the file.
        /// </summary>
        /// <value>The default storage provider.</value>
        /// <remarks></remarks>
        public short DefaultStorageProvider
        {
            get
            {
                return Common.ConvertDef.ToInt16(GetExtraInfo("fileStorageProvider"), -1);
            }
            set
            {
                if (value < 0)
                    SetExtraInfo("fileStorageProvider", DBNull.Value);
                else
                    SetExtraInfo("fileStorageProvider", value);
            }
        }

        /// <summary>
        /// Gets or Sets a flag that specifies whether a default jobs list is created on creation of a file.
        /// </summary>
        /// <value><c>true</c> if [generate jobs on creation]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public bool GenerateJobsOnCreation
        {
            get
            {
                return _generateDefJobs;
            }
            set
            {
                _generateDefJobs = value;
            }
        }

        /// <summary>
        /// Gets the next file review date.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL ReviewDate
        {
            get
            {
                if (Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("filereviewdate"), DBNull.Value) == DBNull.Value)
                {

                    //UTCFIX: DM - 30/11/06 - Specify kind of date.
                    if (CurrentFileType.EnableFileReview)
                        return DateTime.Today.AddDays(CurrentFileType.FileReviewDays);
                    else
                        return DBNull.Value;
                }
                else
                    return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("filereviewdate"), DBNull.Value);
            }
        }

        /// <summary>
        /// Gets the predicted next file review date.
        /// </summary>
        /// <value>The next default review date.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL NextDefaultReviewDate
        {
            get
            {

                //UTCFIX: DM - 30/11/06 - Specify kind of date.

                if (CurrentFileType.EnableFileReview)
                    return DateTime.Today.AddDays(CurrentFileType.FileReviewDays);
                else
                {
                    return DBNull.Value;
                }

            }
            set
            {
                if (CurrentFileType.EnableFileReview)
                {
                    if (IsNew)
                    {
                        if (value == DBNull.Value)
                            SetExtraInfo("filereviewdate", DBNull.Value);
                        else
                            SetExtraInfo("filereviewdate", value);
                    }
                }

                //Used internally by the enquiry engine.
            }
        }

        /// <summary>
        /// Gets a boolean value that specifies that the file has a file review date or not.
        /// </summary>
        /// <remarks></remarks>
        public bool HasReviewDate
        {
            get
            {
                return (ReviewDate != DBNull.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file object is new and needs to be
        /// updated to exist in the database.
        /// </summary>
        /// <remarks></remarks>
        public bool IsNew
        {
            get
            {
                try
                {
                    return (_file.Tables[Table].Rows[0].RowState == DataRowState.Added);
                }
                catch
                {
                    return false;
                }
            }
        }

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
                    switch (_file.Tables[Table].Rows[0].RowState)
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
        /// Gets or sets the ISO code.
        /// </summary>
        /// <value>The ISO code.</value>
        /// <remarks></remarks>
        public string ISOCode
        {
            get
            {
                string output = Convert.ToString(GetExtraInfo("filecurISOCode"));
                if (output == "")
                {
                    output = Session.CurrentSession.DefaultCurrency;
                    SetExtraInfo("filecurISOCode", output);
                    return output;
                }
                else
                    return output;
            }
            set
            {
                if (value == "")
                    SetExtraInfo("filecurISOCode", Session.CurrentSession.DefaultCurrency);
                else
                    SetExtraInfo("filecurISOCode", value);
            }
        }

        /// <summary>
        /// Gets the currency number format information for the file.
        /// </summary>
        /// <remarks></remarks>
        public System.Globalization.NumberFormatInfo CurrencyFormat
        {
            get
            {
                if (_currency.Code != this.ISOCode)
                    _currency.Fetch(this.ISOCode);
                return _currency.CurrencyFormat;
            }
        }

        /// <summary>
        /// Gets the datetime format information for the file.
        /// </summary>
        /// <remarks></remarks>
        public System.Globalization.DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                return System.Globalization.CultureInfo.CreateSpecificCulture(PreferedLanguage).DateTimeFormat;
            }
        }

        /// <summary>
        /// Returns True or False if a Credit Warning
        /// </summary>
        /// <remarks></remarks>
        public bool IsCreditWarning
        {
            get
            {
                if (this.TimeWIP >= this.CreditLimit * this.CreditLimitWarningPercentage / 100)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Time WIP value where TimeStats not Billed
        /// </summary>
        /// <remarks></remarks>
        public decimal TimeWIP
        {
            get
            {
                CheckExternalBalances();
                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    return TimeBalance("timebilled = 0");
                else
                {
                    DownloadTimeStats();
                    return Convert.ToDecimal(_file.Tables[Table_TimeStats].Rows[0]["TIMEWIP"]);
                }
            }
        }


        /// <summary>
        /// Time WIP value where TimeStats not Billed formatted
        /// </summary>
        /// <remarks></remarks>
        public string TimeWIPFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.TimeWIP, this.CurrencyFormat, this.DateTimeFormat, "").ToString();
            }
        }


        /// <summary>
        /// Time Billed value where TimeStats Billed
        /// </summary>
        /// <remarks></remarks>
        public decimal TimeBilled
        {
            get
            {
                CheckExternalBalances();

                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    return TimeBalance("timebilled = 1");
                else
                {
                    DownloadTimeStats();
                    return Convert.ToDecimal(_file.Tables[Table_TimeStats].Rows[0]["TimeBilled"]);
                }
            }
        }

        /// <summary>
        /// Time WIP value where TimeStats not Billed formatted
        /// </summary>
        /// <remarks></remarks>
        public string TimeBilledFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.TimeBilled, this.CurrencyFormat, this.DateTimeFormat, "").ToString();
            }
        }



        /// <summary>
        /// Gets the unique OMS file identifer.
        /// </summary>
        /// <remarks></remarks>
        public long ID
        {
            get
            {
                return (Int64)GetExtraInfo("fileid");
            }
        }

        /// <summary>
        /// Gets the file number of the OMS file object.
        /// </summary>
        /// <value>The file no.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FileNo
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileno"));
            }
            set
            {
                SetExtraInfo("fileno", value);
            }
        }

        /// <summary>
        /// Gets or Sets the external account code of the file which may be used to link
        /// a file to another instance of the file in an external database / accounting
        /// system.
        /// </summary>
        /// <value>The account code.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string AccountCode
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileacccode"));
            }
            set
            {
                SetExtraInfo("fileacccode", value);
            }
        }

        /// <summary>
        /// Gets or Sets the file description of the OMS file object.
        /// </summary>
        /// <value>The file description.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FileDescription
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileDesc"));
            }
            set
            {
                SetExtraInfo("filedesc", value);
            }
        }

        /// <summary>
        /// Gets the contact that is the source of business.
        /// </summary>
        /// <value>The source is contact.</value>
        /// <remarks></remarks>
        public Contact SourceIsContact
        {
            get
            {
                if (GetExtraInfo("filesourcecontact") == DBNull.Value)
                    return null;
                else
                    return Contact.GetContact((long)GetExtraInfo("filesourcecontact"));
            }
            set
            {
                if (value == null)
                {
                    SetExtraInfo("filesourcecontact", DBNull.Value);
                    SetExtraInfo("filesource", DBNull.Value);
                }
                else
                {
                    //Add the source of business contact to the files associates list.
                    if (IsNew)
                    {
                        if (this.SourceIsContact != null)
                        {
                            Associates.Remove(value);
                        }
                        if (Associates.Contains(value) == false)
                        {
                            string associateAs = value.AssociateAs;
                            if (string.IsNullOrEmpty(associateAs))
                                associateAs = "SOURCE";

                            Associates.Add(new Associate(value, this, associateAs));
                        }
                    }
                    SetExtraInfo("filesourcecontact", value.ID);
                    SetExtraInfo("filesource", "CONTACT");
                }
            }
        }

        /// <summary>
        /// Gets the user that is the source of business.
        /// </summary>
        /// <value>The source is user.</value>
        /// <remarks></remarks>
        public User SourceIsUser
        {
            get
            {
                if (GetExtraInfo("filesourceuser") == DBNull.Value)
                    return null;
                else
                    return User.GetUser((int)GetExtraInfo("filesourceuser"));
            }
            set
            {
                if (value == null)
                {
                    SetExtraInfo("filesourceuser", DBNull.Value);
                    SetExtraInfo("filesource", DBNull.Value);
                }
                else
                {
                    SetExtraInfo("filesourceuser", value.ID);
                    SetExtraInfo("filesource", "USER");
                }
            }
        }

        /// <summary>
        /// Gets or Sets the fee earner that is responsible of the file, the person you complain to.
        /// </summary>
        /// <value>The responsible fee earner.</value>
        /// <remarks></remarks>
        public FeeEarner ResponsibleFeeEarner
        {
            get
            {
                try
                {
                    return FeeEarner.GetFeeEarner(Convert.ToInt32(GetExtraInfo("fileresponsibleid")));
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                    SetExtraInfo("fileresponsibleid", DBNull.Value);
                else
                    SetExtraInfo("fileresponsibleid", value.ID);
            }
        }


        /// <summary>
        /// Gets or Sets the fee earner that does most of the work on the file.
        /// </summary>
        /// <value>The principle fee earner.</value>
        /// <remarks></remarks>
        public FeeEarner PrincipleFeeEarner
        {
            get
            {
                try
                {
                    return FeeEarner.GetFeeEarner(Convert.ToInt32(GetExtraInfo("fileprincipleid")));
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                    PrincipleFeeEarnerID = null;
                else
                    PrincipleFeeEarnerID = value.ID;

            }
        }

        /// <summary>
        /// Returns the Principle Fee Earners ID, to set user Princible Fee Earner
        /// </summary>
        /// <value>The principle fee earner ID.</value>
        /// <remarks></remarks>
        public int? PrincipleFeeEarnerID
        {
            get
            {
                object val = GetExtraInfo("fileprincipleid");
                if (val == DBNull.Value)
                    return null;
                else
                    return Convert.ToInt32(val);
            }
            set
            {
                if (value == null)
                    SetExtraInfo("fileprincipleid", DBNull.Value);
                else
                    SetExtraInfo("fileprincipleid", value);
            }
        }



        /// <summary>
        /// Gets or Sets the fee earner that is also associated with the file, for three level file management.
        /// </summary>
        /// <value>The manager.</value>
        /// <remarks></remarks>
        public FeeEarner Manager
        {
            get
            {
                try
                {
                    return FeeEarner.GetFeeEarner(Convert.ToInt32(GetExtraInfo("filemanagerid")));
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                    SetExtraInfo("filemanagerid", DBNull.Value);
                else
                    SetExtraInfo("filemanagerid", value.ID);
            }
        }

        /// <summary>
        /// Gets the source of business code.  If the code is CONTACT then a contact is used.
        /// If the code is USER then a user is used, otherwise a code lookup is used.
        /// </summary>
        /// <value>The source of business.</value>
        /// <remarks></remarks>
        public string SourceOfBusiness
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filesource"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("filesource", DBNull.Value);
                else
                    SetExtraInfo("filesource", value);
            }
        }

        /// <summary>
        /// Gets the localized description of the source of business.
        /// </summary>
        /// <remarks></remarks>
        public string Source
        {
            get
            {
                if (SourceIsContact != null)
                    return SourceIsContact.Name;
                else if (SourceIsUser != null)
                    return SourceIsUser.FullName;
                else if (SourceOfBusiness == "")
                    return "";
                else
                    return CodeLookup.GetLookup("SOURCE", SourceOfBusiness);

            }
        }


        /// <summary>
        /// Gets or Sets the department code associated to the file.
        /// </summary>
        /// <value>The department.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Department
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileDepartment"));
            }
            set
            {
                SetExtraInfo("fileDepartment", value);
            }
        }

        /// <summary>
        /// Gets the Department Name Cultured
        /// </summary>
        /// <remarks></remarks>
        public string DepartmentCultured
        {
            get
            {
                return CodeLookup.GetLookup("DEPT", this.Department);
            }
        }

        /// <summary>
        /// Gets the branch that the file was created at.
        /// </summary>
        /// <value>The branch.</value>
        /// <remarks></remarks>
        public Branch Branch
        {
            get
            {
                try
                {
                    return new Branch(Convert.ToInt32(GetExtraInfo("brid")));
                }
                catch
                {
                    try
                    {
                        return Session.CurrentSession.CurrentFeeEarner.Branch;
                    }
                    catch
                    {
                        return Session.CurrentSession.CurrentBranch;
                    }
                }
            }
            set
            {
                if (value != null)
                {
                    SetExtraInfo("brid", value.ID);
                }
                else
                {
                    try
                    {
                        SetExtraInfo("brid", Session.CurrentSession.CurrentFeeEarner.Branch.ID);
                    }
                    catch
                    {
                        SetExtraInfo("brid", Session.CurrentSession.CurrentBranch.ID);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the file type code of the file.
        /// </summary>
        /// <remarks></remarks>
        public string FileTypeCode
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filetype"));
            }
        }

        /// <summary>
        /// Gets the file type object associated to the file.
        /// </summary>
        /// <remarks></remarks>
        public FileType CurrentFileType
        {
            get
            {
                return (FileType)GetOMSType();
            }
        }

        /// <summary>
        /// Gets the file description of the OMS file object.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FileClientDescription
        {
            get
            {
                return Client.ClientDescription;
            }
        }
        
        private Models.Client.ClientDetails _clientDetails;
        /// <summary>
        /// Gets the Client Details
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Models.Client.ClientDetails ClientDetails
        {
            get
            {
                if (_clientDetails == null)
                    _clientDetails = new FWBS.OMS.Models.Client.ClientDetails(Client.ClientName, DefaultAssociate);

                return _clientDetails;
            }
        }

        /// <summary>
        /// Gets a client object that is associated to the file.
        /// </summary>
        /// <remarks></remarks>
        public Client Client
        {
            get
            {
                return Client.GetClient((long)GetExtraInfo("clid"));
            }
        }

        /// <summary>
        /// Gets the client identifier that is associated to the file.
        /// </summary>
        /// <remarks></remarks>
        public long ClientID
        {
            get
            {
                return (long)GetExtraInfo("clid");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file is in a state to be exported to another system.
        /// This may be set to true if the file is created or has been updated.
        /// </summary>
        /// <remarks></remarks>
        public bool NeedExport
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("fileneedexport"));
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the prefered language that the client of the file will use.
        /// </summary>
        /// <remarks></remarks>
        public string PreferedLanguage
        {
            get
            {
                if (GetExtraInfo("fileuicultureinfo") is DBNull)
                    return Client.PreferedLanguage;
                else
                {
                    try
                    {
                        string culture = Convert.ToString(GetExtraInfo("fileUICultureInfo"));
                        System.Globalization.CultureInfo.CreateSpecificCulture(culture);
                        return culture;
                    }
                    catch
                    {
                        return Client.PreferedLanguage;
                    }
                }

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

        [EnquiryUsage(true)]
        public string PrecedentMinorCategory
        {
            get
            {
                return CurrentFileType.PrecedentMinorCategory;
            }
        }

        /// <summary>
        /// Gets the Precedent Sub Category.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string PrecedentSubCategory
        {
            get
            {
                return CurrentFileType.PrecedentSubCategory;
            }
        }

        /// <summary>
        /// Gets the Precedent Category.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string PrecedentCategory
        {
            get
            {
                return CurrentFileType.PrecedentCategory;
            }
        }

        /// <summary>
        /// Gets or Sets the Precedent Library.
        /// </summary>
        /// <value>The precedent library.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string PrecedentLibrary
        {
            get
            {
                string ret = Convert.ToString(GetExtraInfo("filepreclibrary"));
                if (ret == "")
                    return CurrentFileType.PrecedentLibrary;
                else
                    return ret;
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("filepreclibrary", DBNull.Value);
                else
                    SetExtraInfo("filepreclibrary", value);
            }
        }

        /// <summary>
        /// Gets the date and time of when this file was closed.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Closed
        {
            get
            {
                if (GetExtraInfo("fileClosed") is System.DBNull)
                    return "";
                else
                    return "" + GetExtraInfo("fileClosed");
            }
        }

        /// <summary>
        /// Gets a flag value indicating whether the file is closed or not.
        /// </summary>
        /// <remarks></remarks>
        public bool IsClosed
        {
            get
            {
                return (Status == "DEAD");
            }
        }

        /// <summary>
        /// Gets a user object that closed the current file.
        /// </summary>
        /// <remarks></remarks>
        public User ClosedBy
        {
            get
            {
                try
                {
                    return User.GetUser(Convert.ToInt32(GetExtraInfo("fileClosedBy")));
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the status code of the current file.
        /// </summary>
        /// <value>The status.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Status
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileStatus"));
            }
            set
            {
                string oldval = Status;

                if (value != PREINSTRUCTION && Client.IsPreClient)
                {
                    AskEventArgs e = new AskEventArgs("CONVPRECLIENT", "Would you like to convert the pre-%CLIENT% into a standard %CLIENT%?", "", AskResult.No);
                    Session.CurrentSession.OnAsk(this, e);
                    if (e.Result == AskResult.Yes)
                    {
                        Client.ConvertPreClient();
                        Client.Update();
                    }
                    else
                    {
                        value = PREINSTRUCTION;
                    }

                }

                if (value != oldval)
                {
                    if (value == "DEAD")
                    {
                        this.SetExtraInfo("fileClosed", System.DateTime.Now);
                        this.SetExtraInfo("fileClosedBy", FWBS.OMS.Session.CurrentSession.CurrentUser.ID);
                    }
                    else
                    {
                        this.SetExtraInfo("fileClosed", DBNull.Value);
                        this.SetExtraInfo("fileClosedBy", DBNull.Value);
                    }

                    string desc = CodeLookup.GetLookup("FILESTATUS", Status) + " (" + Status + ") -> " + CodeLookup.GetLookup("FILESTATUS", value) + " (" + value + ")";
                    AddEvent("CHGFILESTATUS", desc, "");

                    SetExtraInfo("fileStatus", value);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Status", oldval, value));
            }
        }





        /// <summary>
        /// Gets or Sets the extra notes used to explain additional information on the file.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Notes
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileNotes"));
            }
            set
            {
                SetExtraInfo("fileNotes", value);
            }
        }

        /// <summary>
        /// Gets or Sets the flag that indicates whether the file will be able to be uploaded to an external
        /// system like OMS Interactive.
        /// </summary>
        /// <value><c>true</c> if [allow external]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public bool AllowExternal
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("fileallowexternal"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetExtraInfo("fileallowexternal", value);
            }
        }

        /// <summary>
        /// Gets or Sets the external notes used to explain additional information on the file.
        /// This note set will be used by external programs like OMS Interactive.
        /// </summary>
        /// <value>The external notes.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string ExternalNotes
        {
            get
            {
                return Convert.ToString(GetExtraInfo("fileexternalnotes"));
            }
            set
            {
                SetExtraInfo("fileexternalnotes", value);
            }
        }

        /// <summary>
        /// Gets or Sets the flag that indicates whether a client can access the particular files
        /// information and status from mobile text messaging.
        /// </summary>
        /// <value><c>true</c> if [SMS enabled]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public bool SMSEnabled
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("filesmsenabled"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetExtraInfo("filesmsenabled", value);
            }
        }


        /// <summary>
        /// Gets or Sets the funding object to the associated file.
        /// </summary>
        /// <value>The type of the funding.</value>
        /// <remarks></remarks>
        public FundType FundingType
        {
            get
            {
                if (_fundType == null && Convert.ToString(GetExtraInfo("filefundcode")) != "" && this.ISOCode != "")
                    _fundType = FWBS.OMS.FundType.GetFundType(Convert.ToString(GetExtraInfo("filefundcode")), this.ISOCode);
                return _fundType;
            }
            set
            {
                if ((value != null && this.FundingType == null) || (value != null && this.FundingType != null && value.Code != this.FundingType.Code))
                {
                    _fundType = value;
                    SetExtraInfo("filefundcode", _fundType.Code);
                    this.CreditLimit = Convert.ToDecimal(_fundType.DefaultCreditLimit);
                    this.OriginalCreditLimit = Convert.ToDecimal(_fundType.DefaultCreditLimit);
                    this.CreditLimitWarningPercentage = _fundType.DefaultWarningPercentage;
                    this.RateBanding = _fundType.DefaultRateBand;
                    this.RatePerUnit = _fundType.DefaultRatePerUnit;
                }
            }
        }

        /// <summary>
        /// Gets the Credit Limit for the File.
        /// </summary>
        /// <value>The credit limit.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public decimal CreditLimit
        {
            get
            {
                CheckExternalBalances();

                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    return FWBS.Common.ConvertDef.ToDecimal(GetExtraInfo("fileCreditLimit"), 0);
                else
                {
                    DownloadTimeStats();
                    return Convert.ToDecimal(_file.Tables[Table_TimeStats].Rows[0]["CreditLimit"]);
                }
            }
            set
            {
                CheckExternalBalances();

                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    SetExtraInfo("fileCreditLimit", value);
            }
        }


        /// <summary>
        /// Time WIP value where TimeStats not Billed formatted
        /// </summary>
        /// <remarks></remarks>
        public string CreditLimitFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.CreditLimit, this.CurrencyFormat, this.DateTimeFormat, "").ToString();
            }
        }



        /// <summary>
        /// Gets or Sets the files funding reference text.
        /// </summary>
        /// <value>The funding reference.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FundingReference
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filefundref"));
            }
            set
            {
                SetExtraInfo("filefundref", value);
            }
        }

        /// <summary>
        /// Gets or Sets the rate banding.
        /// </summary>
        /// <value>The rate banding.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public byte RateBanding
        {
            get
            {
                return FWBS.Common.ConvertDef.ToByte(GetExtraInfo("fileBanding"), 0);
            }
            set
            {
                byte oldval = RateBanding;
                if (oldval != value)
                {
                    SetExtraInfo("fileBanding", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("RateBanding", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the agreement date of the file.
        /// </summary>
        /// <value>The agreement date.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public FWBS.Common.DateTimeNULL AgreementDate
        {
            get
            {
                return FWBS.Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("fileagreementdate"), DBNull.Value);

            }
            set
            {
                SetExtraInfo("fileagreementdate", value.ToObject());
            }
        }

        /// <summary>
        /// Gets or Sets the credit limit warning percentage.
        /// </summary>
        /// <value>The credit limit warning percentage.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public int CreditLimitWarningPercentage
        {
            get
            {
                return Common.ConvertDef.ToInt32(GetExtraInfo("filewarningperc"), 0);
            }
            set
            {
                int oldval = CreditLimitWarningPercentage;
                if (oldval != value)
                {
                    SetExtraInfo("filewarningperc", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("CreditLimitWarningPercentage", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets the original credit limit monetary value, incase the credit limit ever changes..
        /// </summary>
        /// <value>The original credit limit.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public decimal OriginalCreditLimit
        {
            get
            {
                return FWBS.Common.ConvertDef.ToDecimal(GetExtraInfo("fileoriginallimit"), 0);
            }
            set
            {
                decimal oldval = OriginalCreditLimit;
                if (oldval != value)
                {
                    SetExtraInfo("fileoriginallimit", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("OriginalCreditLimit", oldval, value));
                }
            }
        }

        /// <summary>
        /// Original Credit limit formatted
        /// </summary>
        /// <remarks></remarks>
        public string OriginalCreditLimitFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.OriginalCreditLimit, this.CurrencyFormat, this.DateTimeFormat, "").ToString();

            }
        }



        /// <summary>
        /// Gets or Sets a free form monetary estimate cost value.
        /// </summary>
        /// <value>The estimate.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public decimal Estimate
        {
            get
            {
                return FWBS.Common.ConvertDef.ToDecimal(GetExtraInfo("fileestimate"), 0);
            }
            set
            {
                if (Estimate != value)
                {
                    SetExtraInfo("filelastestimate", Estimate);
                    SetExtraInfo("fileestimate", value);
                }
            }
        }

        /// <summary>
        /// Estimate formatted
        /// </summary>
        /// <remarks></remarks>
        public string EstimateFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.Estimate, this.CurrencyFormat, this.DateTimeFormat, "").ToString();
            }
        }



        /// <summary>
        /// Gets the last estimate.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public decimal LastEstimate
        {
            get
            {
                CheckExternalBalances();

                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    return FWBS.Common.ConvertDef.ToDecimal(GetExtraInfo("filelastestimate"), 0);
                else
                {
                    DownloadTimeStats();
                    return Convert.ToDecimal(_file.Tables[Table_TimeStats].Rows[0]["LastEstimate"]);
                }
            }
        }


        /// <summary>
        /// Last Estimate formatted
        /// </summary>
        /// <remarks></remarks>
        public string LastEstimateFormatted
        {
            get
            {
                return FieldParser.GetFormattedValue(this.LastEstimate, this.CurrencyFormat, this.DateTimeFormat, "").ToString();
            }
        }


        /// <summary>
        /// Gets or Sets the rate per unit for the file. If zero then the activity type rate is used.
        /// Otherwise all work done on the file uses the specified value.
        /// </summary>
        /// <value>The rate per unit.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public decimal RatePerUnit
        {
            get
            {
                return FWBS.Common.ConvertDef.ToDecimal(GetExtraInfo("filerateperunit"), 0);
            }
            set
            {
                Decimal oldval = RatePerUnit;
                if (oldval != value)
                {
                    SetExtraInfo("filerateperunit", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("RatePerUnit", oldval, value));
                }
            }
        }


        /// <summary>
        /// Gets or Sets free-form text about restrictions of the funding of the file may have.
        /// </summary>
        /// <value>The funding restrictions.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FundingRestrictions
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filerestrictions"));
            }
            set
            {
                SetExtraInfo("filerestrictions", value);
            }
        }

        /// <summary>
        /// Gets or Sets free-form text about the scope of the funding of the file may have.
        /// </summary>
        /// <value>The funding scope.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string FundingScope
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filescope"));
            }
            set
            {
                SetExtraInfo("filescope", value);
            }
        }

        /// <summary>
        /// Gets or Sets the Legal Aid Category for the File
        /// </summary>
        /// <value>The LA category.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Int16 LACategory
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt16(GetExtraInfo("fileLACategory"), 0);
            }
            set
            {
                SetExtraInfo("fileLACategory", value == 0 ? (object)DBNull.Value : value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        DateTime milestoneplanlastchecked;

        /// <summary>
        /// Returns a Mile Stone Plan for this File
        /// </summary>
        /// <value>The milestone plan.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public Milestones_OMS2K MilestonePlan
        {
            get
            {
                if (_milestoneplan == null && this.ID != 0)
                {
                    if (milestoneplanlastchecked == null || DateTime.Now.Subtract(milestoneplanlastchecked) > new TimeSpan(0, 0, 2))
                        if (Milestones_OMS2K.MilestonePlanExists(this.ID))
                            _milestoneplan = new Milestones_OMS2K(this);
                    milestoneplanlastchecked = DateTime.Now;
                }
                return _milestoneplan;
            }
            set
            {
                _milestoneplan = value;
            }
        }

        /// <summary>
        /// Gets or Sets the sharepoint server site.
        /// </summary>
        /// <value>The SPS site.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string SPSSite
        {
            get
            {
                string site = Convert.ToString(GetExtraInfo("fileSPSSite")).Trim();
                if (site == String.Empty)
                {
                    site = Session.CurrentSession.CurrentBranch.SPSServer;
                    if (site.EndsWith("/") == false)
                        site += "/";
                    site += "sites/";
                    site += ID.ToString();
                }

                try
                {
                    Uri url = new Uri(site);
                    return site;
                }
                catch (UriFormatException)
                {
                    return "";
                }
            }
            set
            {
                if (value == null || value == String.Empty)
                    SetExtraInfo("fileSPSSite", DBNull.Value);
                else
                    SetExtraInfo("fileSPSSite", value);
            }
        }

        /// <summary>
        /// Gets or the files document workspace.
        /// </summary>
        /// <value>The SPS document workspace.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string SPSDocumentWorkspace
        {
            get
            {
                try
                {
                    string site = Convert.ToString(GetExtraInfo("fileSPSDocWS")).Trim();
                    if (site == String.Empty)
                        return "Matter Documents";
                    else
                        return site;
                }
                catch
                {
                    return "Matter Documents";
                }
            }
            set
            {
                if (value == null || value == String.Empty)
                    SetExtraInfo("fileSPSDocWS", DBNull.Value);
                else
                    SetExtraInfo("fileSPSDocWS", value);
            }
        }
        
        private Dictionary<string, ExtendedDataList> _associateExtendedData;

        /// <summary>
        /// Returns Extended Data of Associate for created File
        /// </summary>
        /// <value>The Associate Extended Data.</value>
        /// <remarks></remarks>
        public Dictionary<string, ExtendedDataList> AssociateExtendedData
        {
            get
            {
                if (_associateExtendedData == null)
                {
                    _associateExtendedData = new Dictionary<string, ExtendedDataList>();
                }

                return _associateExtendedData;
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
            switch (fieldName.ToUpper())
            {
                case "FILENO":
                    break;

                case "FILETYPE":
                    {
                        if (IsNew)
                        {
                            FileType fileType = FileType.GetFileType(Convert.ToString(val));
                            FundingType = fileType.GetFundType();
                            Department = fileType.DefaultDepartment;

                            if (fileType.MilestonePlan != "")
                            {
                                _milestoneplan = new Milestones_OMS2K(this);
                                _milestoneplan.SetMileStonePlan(fileType.MilestonePlan, true);
                            }
                        }
                    }
                    break;
                case "PHID":
                    {
                        if (_file.Tables.Contains(Table_Phases) && _file.Tables[Table].Columns.Contains("phid"))
                        {
                            DataView phases;
                            if (val != DBNull.Value)
                            {
                                phases = new DataView(_file.Tables[Table_Phases]);
                                phases.RowFilter = "phid = " + Common.ConvertDef.ToInt64(val, -1).ToString();
                                if (phases.Count == 0)
                                {
                                    DownloadPhases();
                                    phases = new DataView(_file.Tables[Table_Phases]);
                                    phases.RowFilter = "phid = " + Common.ConvertDef.ToInt64(val, -1).ToString();
                                    if (phases.Count == 0)
                                        val = DBNull.Value;
                                }
                            }
                            _currentPhase = null;
                        }
                        else
                            return;
                    }
                    break;
            }

            this.SetExtraInfo(_file.Tables[Table].Rows[0], fieldName, val);
        }

        /// <summary>
        /// Downloads the phases.
        /// </summary>
        /// <remarks></remarks>
        private void DownloadPhases()
        {
            if (_file.Tables.Contains(Table_Phases) == true)
            {
                _file.Tables.Remove(Table_Phases);
            }

            _file.Tables.Add(Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbfilephase where fileid = @FILEID and phactive = 1 order by Created", Table_Phases, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, this.ID) }));

        }


        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
        public object GetExtraInfo(string fieldName)
        {
            if (_file.Tables[Table].Columns.Contains(fieldName))
            {
                object val = _file.Tables[Table].Rows[0][fieldName];
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
            }
            else
                return Client.GetExtraInfo(fieldName);
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
                return _file.Tables[Table].Columns[fieldName].DataType;
            }
            catch
            {
                throw new OMSException2("15001", "Error Getting Extra Info Field %1% Probably Not Initialized", new Exception(), true, fieldName);
            }
        }

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
        public DataSet GetDataset()
        {
            return _file;
        }

        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
        public DataTable GetDataTable()
        {
            return _file.Tables[Table].Copy();
        }

        #endregion

        #region IUpdateable Implementation

        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        /// <remarks></remarks>
        public void Update()
        {
            CheckPermissions();

            try
            {

                //New addin object event arguments
                ObjectState state = State;

                if (this.OnExtCreatingUpdatingOrDeleting(state))
                    return;

                System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
                e.Cancel = false;
                OnUpdating(e);
                if (e.Cancel)
                    return;

                Session.CurrentSession.Connection.Connect(true);

                DataRow row = _file.Tables[Table].Rows[0];

                bool isnew = IsNew;

                //On the creation of a new file try skipping the search conflict.
                if (isnew)
                {
                    // Make Sure data is validated on the isNew and setup Correctly.
                    if (String.IsNullOrEmpty(this.ISOCode))
                        throw new OMSException2("ERRNOCURRSETUP", "The Currency Type has not been configured Please ensure the default currency for the branch is configured!");
                    SkipConflictSearch();
                }

                //Check if there are any changes made before setting the updated
                //and updated by properties then update.
                if (IsDirty)
                {
                    //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
                    if (_file.Tables[Table].PrimaryKey == null || _file.Tables[Table].PrimaryKey.Length == 0)
                        _file.Tables[Table].PrimaryKey = new DataColumn[1] { _file.Tables[Table].Columns["fileid"] };

                    SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
                    SetExtraInfo("Updated", DateTime.Now);

                    if (xmlprops != null)
                    {
                        //DM - Big Bang Day 10/09/08 - May not be supported in earlier versions where fileXml does not
                        //exist in the database.
                        if (xmlprops.IsSupported)
                            xmlprops.Update();
                    }

                    try
                    {
                        Session.CurrentSession.Connection.Update(row, "dbfile");
                        _isdirty = false;
                    }
                    catch (ConnectionException cex)
                    {
                        System.Data.SqlClient.SqlException sqlex = cex.InnerException as SqlException;
                        if (sqlex == null)
                            throw;

                        if (sqlex.Message.IndexOf("FK_dbFile_dbFundType") > -1)
                        {
                            Currency c = Currency.GetCurrency(ISOCode);
                            throw new OMSException2("150005", "There is no Funding Entry for a Currency of '%1%' and Funding Type of '%2%'. " + Environment.NewLine + Environment.NewLine + "Please add a new entry in the admin kit.", "", sqlex, true, c.CurrencyName, this.FundingType.Description);
                        }

                        if (sqlex.Message.IndexOf("Violation of PRIMARY KEY constraint 'PK_dbMatters'") > -1)
                        {
                            throw new OMSException2("FILENOEXISTS", "A Matter already exists with the specified number.");
                        }

                        if (sqlex.Procedure == "tgrFileNumberGenerator")
                        {
                            throw new OMSException2("ERRTRIGFNG", "Problem Generating Seed Number for this file with Branch '%1%' and File Type of '%2%'", "", sqlex, false, this.Branch.BranchName, this.CurrentFileType.Description);
                        }

                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                if (isnew)
                {
                    Session.CurrentSession.CurrentFiles.Add(ID.ToString(), this);
                }

                //Update all one to may tables like the file events table and associates.
                //Make sure that the multi contact table has the correct file id.
                if (_file.Tables.Contains(Table_Associates))
                {
                    foreach (DataRow assoc in _file.Tables[Table_Associates].Rows)
                    {
                        assoc["fileid"] = this.ID;
                        if (Convert.ToString(assoc["AssocHeading"]) == "")
                            assoc["AssocHeading"] = FileDescription;
                    }

                    if (_file.Tables[Table_Associates].GetChanges() != null)
                    {
                        _file.Tables[Table_Associates].ExtendedProperties.Add("KeyInitialized", true);
                        Session.CurrentSession.Connection.Update(_file.Tables[Table_Associates], "dbAssociates", false);
                        //Make sure that the associates are downloaded from the database with their newly
                        //generated identifiers.
                        DownloadAssociates(true);
                    }
                }

                //Update the file events table.
                foreach (DataRow ev in _file.Tables[Table_Events].Rows)
                {
                    ev["fileid"] = this.ID;
                }
                if (_file.Tables[Table_Events].GetChanges() != null)
                {
                    Session.CurrentSession.Connection.Update(_file.Tables[Table_Events], Sql_Events);
                }

                if (isnew)
                {
                    // Apply any Client Type Template Security
                    TemplateSecurity tmp = new TemplateSecurity("FileType", this.FileTypeCode);
                    if (tmp.HasSecurity) tmp.ApplySecurity(this.ID);
                    Security.SecurityManager.CurrentManager.ApplyDefaultSettings(this.Client, this);
                }


                //Updates any tasks added to the file.
                if (_tasks != null)
                    _tasks.Update();

                //Updates any appointments added to the file.
                if (_appointments != null)
                    _appointments.Update();


                //Update the milestone plan.
                if (_milestoneplan != null)
                {
                    _milestoneplan.Update();
                }

                //DM - Reason for move - refer to client upate 17/09/04
                //Update all the extended data objects, if any.
                if (_extData != null)
                {
                    foreach (FWBS.OMS.ExtendedData ext in _extData)
                    {
                        ext.UpdateExtendedData();
                    }
                }

                //Update any of the cached objects that may have something to do with this contact.
                foreach (Client cl in Session.CurrentSession.CurrentClients.Values)
                {
                    if (ClientID == cl.ClientID)
                    {
                        cl.Refresh(true);
                    }
                }

                //Update any associates that may be in memory.
                foreach (Associate assoc in Session.CurrentSession.CurrentAssociates.Values)
                {
                    if (assoc.OMSFileID == this.ID)
                    {
                        assoc.Refresh(true);
                    }
                }

                //Update any InteractiveFile profiles that may be applied.
                if (_interactiveProfiles != null)
                {
                    foreach (object obj in _interactiveProfiles)
                    {
                        if (obj is InteractiveFile)
                        {
                            InteractiveFile p = (InteractiveFile)obj;
                            if (p.IsNew)
                            {
                                p.FileID = ID;
                                p.Update();
                            }
                        }
                    }
                    _interactiveProfiles.Clear();
                }

                //Generate the default jobs list.
                if (_generateDefJobs && isnew)
                {
                    GenerateJobList();
                }

                this.OnExtCreatedUpdatedDeleted(state);
            }
            finally
            {
                Session.CurrentSession.Connection.Disconnect(true);
            }

            OnUpdated();
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

            System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
            e.Cancel = false;
            OnRefreshing(e);
            if (e.Cancel)
                return;

            //Get any changes if there are any.
            DataTable changes = _file.Tables[Table].GetChanges();

            // Remove the Time Stats Table to to forse it to refresh next time
            if (_file.Tables.IndexOf(Table_TimeStats) > -1)
                _file.Tables.Remove(Table_TimeStats);
            DownloadTimeStats();

            xmlprops = null;
            _clientDetails = null;

            try
            {
                _checkPassword = false;

                if (changes != null && applyChanges && changes.Rows.Count > 0)
                {
                    if (changes.Columns.Contains("fileJointDesc"))
                    {
                        changes.Columns.Remove("fileJointDesc");
                    }
                    Fetch(this.ID, changes.Rows[0]);
                }
                else
                    Fetch(this.ID, null);
            }
            finally
            {
                _checkPassword = true;
            }


            //Refresh all the extended data sources, if any.
            if (_extData != null)
            {
                foreach (FWBS.OMS.ExtendedData ext in _extData)
                {
                    ext.RefreshExtendedData(applyChanges);
                }
            }

            //Refresh the associates list of the file.
            DownloadAssociates(true);

            this.OnExtRefreshed();

            OnRefreshed();
        }

        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        /// <remarks></remarks>
        public void Cancel()
        {
            xmlprops = null;

            if (this.ID != 0 && Milestones_OMS2K.MilestonePlanExists(this.ID))
                _milestoneplan = new Milestones_OMS2K(this);
            _file.RejectChanges();

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
                return (_isdirty || _file.GetChanges() != null);
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
        /// Edits the current file object in the form of an enquiry (if the database states that is edit compatible).
        /// </summary>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
        public Enquiry Edit(Common.KeyValueCollection param)
        {
            return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
        }

        /// <summary>
        /// Edits the current file object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
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
                return Client;
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

                if (_file != null)
                {
                    _file.Dispose();
                    _file = null;
                }

                if (_timerecords != null)
                {
                    _timerecords = null;
                }

                if (_currency != null)
                {
                    _currency.Dispose();
                    _currency = null;
                }

                if (_tasks != null)
                {
                    _tasks.Dispose();
                    _tasks = null;
                }

                if (_appointments != null)
                {
                    _appointments.Dispose();
                    _appointments = null;
                }

                if (_currentPhase != null)
                {
                    _currentPhase.Dispose();
                    _currentPhase = null;
                }
            }

            //Dispose unmanaged objects.
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a file object based on the specified file number.
        /// </summary>
        /// <param name="id">File Number Parameter.</param>
        /// <param name="DefaultTab">The default tab.</param>
        /// <returns>A OMSfile object.</returns>
        /// <remarks></remarks>
        public static OMSFile GetFile(long id, string DefaultTab)
        {
            Session.CurrentSession.CheckLoggedIn();
            OMSFile cf = Session.CurrentSession.CurrentFiles[id.ToString()] as OMSFile;

            if (cf == null)
            {
                cf = new OMSFile(id);
            }
            cf.DefaultTab = DefaultTab;
            return cf;
        }

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OMSFile GetFile(long id)
        {
            return GetFile(id, null);
        }

        /// <summary>
        /// This is used to search for a file using alternative references such as AccCode or fileGUID
        /// </summary>
        /// <param name="filealternaterefNo">The filealternateref no.</param>
        /// <param name="fieldtomatch">The fieldtomatch.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static OMSFile GetFile(string filealternaterefNo, string fieldtomatch)
        {
            if (filealternaterefNo == "")
                throw new OMSException2(HelpIndexes.OMSFileNotFound.ToString(), "", "", null, true, filealternaterefNo);

            DataSet filetmp;
            filetmp = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where " + fieldtomatch + " = '" + filealternaterefNo.Replace("'", "''") + "'", new string[1] { Table }, new IDataParameter[0]);
            if ((filetmp.Tables[Table] == null) || (filetmp.Tables[Table].Rows.Count == 0))
            {
                throw new OMSException2("OMSFILENOTFNDF", "", "", null, true, filealternaterefNo, fieldtomatch);
            }
            else
            {
                // Found a record using the match record system so pass the clno of this record set through to the normal
                // GetClient Method
                return GetFile(Convert.ToInt64(filetmp.Tables[Table].Rows[0]["fileID"]));
            }
        }

        /// <summary>
        /// Gets the file time.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DataTable GetFileTime(long id)
        {
            DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeRecords", Table_TimeRecords, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("FILEID", System.Data.SqlDbType.BigInt, 0, id), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) });
            return dt;
        }

        #endregion

        #region IOMSType Implementation

        /// <summary>
        /// Gets an OMS Type based on the file type off this current instance of a file object.
        /// </summary>
        /// <returns>A OMSType with information needed to represented this type of file.</returns>
        /// <remarks></remarks>
        public OMSType GetOMSType()
        {
            return FWBS.OMS.FileType.GetFileType(Convert.ToString(GetExtraInfo("filetype")));
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
                    bool addinfo = false;
                    FileType ft = CurrentFileType;
                    string[] codes = new string[ft.ExtData.Count];
                    int ctr = 0;
                    foreach (OMSType.ExtendedData ext in ft.ExtData)
                    {
                        if (ext.Code == "ADDFILEINFO")
                            addinfo = true;
                        codes.SetValue(ext.Code, ctr);
                        ctr++;
                    }
                    // **************************************************************
                    // Possible exception was foound when bad extended data through an
                    // exception breaking the code after the construction below. This 
                    // exception would force ths property to error and would never 
                    // successfully create the _extData Variable requied forcing the 
                    // Database to be accessed over and over for each field that needed
                    // extended data
                    // **************************************************************
                    if (codes.Length > 0)
                        _extData = new ExtendedDataList(this, codes);
                    else
                        _extData = new ExtendedDataList();

                    //For backward compatiblity check to see if the file has the ADDFILEINFO system extended
                    //data object so that the captions for the underlying table can be set.  The auto rendering
                    //form should then show these captions.
                    if (addinfo)
                    {
                        FWBS.OMS.ExtendedData additionalInfo = _extData["ADDFILEINFO"];
                        for (byte s = 1; s <= 10; s++)
                            additionalInfo.SetCaption("fileField" + s.ToString(), Convert.ToString(ft.GetExtraInfo("fileField" + s.ToString() + "Desc")));

                        for (byte d = 1; d <= 5; d++)
                            additionalInfo.SetCaption("fileDate" + d.ToString(), Convert.ToString(ft.GetExtraInfo("fileFieldDate" + d.ToString() + "Desc")));

                    }
                }

                return _extData;
            }
        }

        #endregion

        #region PasswordProtectedBase Implementation


        /// <summary>
        /// Gets or Sets the password of the storage item.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        protected override string Password
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filepassword"));
            }
            set
            {
                SetExtraInfo("filepassword", value);
            }
        }

        /// <summary>
        /// Gets or Sets the password Hint of the storage item.
        /// </summary>
        /// <value>The password hint.</value>
        /// <remarks></remarks>
        public override string PasswordHint
        {
            get
            {
                return Convert.ToString(GetExtraInfo("filepasswordhint"));
            }
            set
            {
                SetExtraInfo("filepasswordhint", value);
            }
        }


        /// <summary>
        /// Returns the string represenation of the password request screen.
        /// </summary>
        /// <returns>A string.</returns>
        /// <remarks></remarks>
        public override string ToPasswordString()
        {
            return ToString();
        }

        #endregion

        #region IAlert

        /// <summary>
        /// Gets a list of alerts for the object.
        /// </summary>
        /// <remarks></remarks>
        public Alert[] Alerts
        {
            get
            {
                Alert[] alerts = Client.Alerts;

                System.Collections.ArrayList arr = new System.Collections.ArrayList();
                try
                {
                    if (OnAlertMessage != "")
                    {
                        arr.Add(new Alert(OnAlertMessage, (FWBS.OMS.Alert.AlertStatus)FWBS.Common.ConvertDef.ToEnum(GetExtraInfo("fileAlertLevel"), FWBS.OMS.Alert.AlertStatus.Amber)));
                    }
                }
                catch (System.ArgumentException)
                {
                }


                string desc = FWBS.OMS.Session.CurrentSession.Resources.GetResource("FILESTATAL", "%FILE% Status : %1%", "", true, CodeLookup.GetLookup("FILESTATUS", Status)).Text;
                FileStatusManager statusManager = new FileStatusManager(this.ID);
                if (statusManager.FileStatusRecord().Rows[0]["fsAlertLevel"] != DBNull.Value)
                {
                    FWBS.OMS.Alert.AlertStatus alertLevel = statusManager.AlertLevel;
                    if (alertLevel != Alert.AlertStatus.Off)
                        arr.Add(new Alert(desc, alertLevel));
                }
                else
                {
                    if (Status.StartsWith("LIVE") == false)
                    {
                        if (Status == "DEAD")
                            arr.Add(new Alert(desc, FWBS.OMS.Alert.AlertStatus.Red));
                        else
                        {
                            arr.Add(new Alert(desc, FWBS.OMS.Alert.AlertStatus.Amber));
                        }
                    }
                }


                if (_tempAlert.Message != "" && _tempAlert.Status != Alert.AlertStatus.Off)
                {
                    arr.Add(_tempAlert);
                }

                Alert[] filealerts = new Alert[alerts.Length + arr.Count];
                if (alerts.Length > 0) alerts.CopyTo(filealerts, 0);
                if (arr.Count > 0) Array.Copy((Alert[])arr.ToArray(typeof(Alert)), 0, filealerts, (alerts.GetUpperBound(0) < 0 ? 0 : alerts.Length), arr.Count);

                return filealerts;
            }
        }

        /// <summary>
        /// Adds the alert.
        /// </summary>
        /// <param name="alert">The alert.</param>
        /// <remarks></remarks>
        public void AddAlert(Alert alert)
        {
            _tempAlert = alert;
        }

        /// <summary>
        /// Clears the alerts.
        /// </summary>
        /// <remarks></remarks>
        public void ClearAlerts()
        {
            _tempAlert.ChangeStatus(Alert.AlertStatus.Off);
        }

        #endregion

        #region IConditional Members

        /// <summary>
        /// Gets the conditionals.
        /// </summary>
        /// <remarks></remarks>
        public string Conditionals
        {
            get
            {
                // Add business layer code for extracting common validations;
                string returns = "";
                returns += "REMOTE;";
                if (this.FundingType.LegalAidCharged)
                    returns += "LEGALAID;";

                return returns;
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

        internal FileStatusManager FileStatusManager { get; set; }


        /// <summary>
        /// Checks the permissions.
        /// </summary>
        /// <remarks></remarks>
        private void CheckPermissions()
        {
            bool isnew = IsNew;
            bool isdirty = IsDirty;
            bool isdeleting = (_file.Tables[Table].Rows[0].RowState == DataRowState.Deleted);

            if (isnew)
            {
                new ClientPermission(Client, StandardPermissionType.CreateFile).Check();
                new ClientActivity(Client, ClientStatusActivityType.FileCreation).Check();
            }
            else if (isdeleting)
                new FilePermission(this, StandardPermissionType.Delete).Check();
            else if (isdirty)
            {
                new FilePermission(this, StandardPermissionType.Update).Check();
                new SystemPermission(StandardPermissionType.UpdateFile).Check();
            }
        }

        #endregion

        #region XML Settings Methods

        /// <summary>
        /// 
        /// </summary>
        private XmlProperties xmlprops;

        /// <summary>
        /// Builds the XML.
        /// </summary>
        /// <remarks></remarks>
        private void BuildXML()
        {
            if (xmlprops == null)
                xmlprops = new XmlProperties(this, "fileXml");
        }

        /// <summary>
        /// Gets the XML property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public object GetXmlProperty(string name, object defaultValue)
        {
            BuildXML();
            return xmlprops.GetProperty(name, defaultValue);
        }

        /// <summary>
        /// Sets the XML property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="val">The val.</param>
        /// <remarks></remarks>
        public void SetXmlProperty(string name, object val)
        {
            BuildXML();
            if (xmlprops.SetProperty(name, val))
                _isdirty = true;
        }



        #endregion


        #region IOMSType Members


        /// <summary>
        /// Sets the current sessions.
        /// </summary>
        /// <remarks></remarks>
        public void SetCurrentSessions()
        {
            if (Session.CurrentSession.CurrentFile != this)
                Session.CurrentSession.CurrentFile = this;
            if (Session.CurrentSession.CurrentClient != this.Client)
                Session.CurrentSession.CurrentClient = this.Client;
        }

        #endregion
    }

    /// <summary>
    /// A class that describes a file phase object.
    /// </summary>
    /// <remarks></remarks>
    public class FilePhase : CommonObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Default constructor that is used to create a new instance of the object.
        /// </summary>
        /// <remarks></remarks>
        internal FilePhase()
            : base()
        {
            OMSFile file = Session.CurrentSession.CurrentFile;
            if (file != null)
                SetExtraInfo("fileid", file.ID);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePhase"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        internal FilePhase(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePhase"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="description">The description.</param>
        /// <remarks></remarks>
        public FilePhase(OMSFile file, string description)
            : this()
        {
            if (file == null)
                throw new ArgumentException("file parameter cannot be null");
            SetExtraInfo("fileid", file.ID);
            Description = description;
        }


        #endregion

        #region CommonObject Implementation


        /// <summary>
        /// The default enquiry form that will be used for the object.
        /// </summary>
        /// <remarks></remarks>
        protected override string DefaultForm
        {
            get
            {
                return Session.CurrentSession.DefaultSystemForm(SystemForms.FilePhaseEdit);
            }
        }

        /// <summary>
        /// Gets the primary key field name.
        /// </summary>
        /// <remarks></remarks>
        public override string FieldPrimaryKey
        {
            get
            {
                return "phID";
            }
        }

        /// <summary>
        /// The table name of the main underlying data object.
        /// </summary>
        /// <remarks></remarks>
        protected override string PrimaryTableName
        {
            get
            {
                return "FILEPHASE";
            }
        }

        /// <summary>
        /// This is the select statement that is used to retrieve the underlying data.
        /// </summary>
        /// <remarks></remarks>
        protected override string SelectStatement
        {
            get
            {
                return "select * from dbfilephase";
            }
        }

        /// <summary>
        /// The active field name, used for temporary deletions.
        /// </summary>
        /// <remarks></remarks>
        protected override string FieldActive
        {
            get
            {
                return "phActive";
            }

        }

        #endregion

        #region IParent Implementation

        /// <summary>
        /// Gets the parent related object.
        /// </summary>
        /// <remarks></remarks>
        public override object Parent
        {
            get
            {
                return File;
            }
        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets the unique phase id for the item.
        /// </summary>
        /// <remarks></remarks>
        public long ID
        {
            get
            {
                return Convert.ToInt64(UniqueID);
            }
        }

        /// <summary>
        /// Gets the file associated to the phase.
        /// </summary>
        /// <remarks></remarks>
        public OMSFile File
        {
            get
            {
                return OMSFile.GetFile(FileID);
            }
        }

        /// <summary>
        /// Gets the file id as an integer.
        /// </summary>
        /// <remarks></remarks>
        public long FileID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("fileid"));
            }
        }


        /// <summary>
        /// Gets or Sets the description / subject of the file phase.
        /// </summary>
        /// <value>The phase no.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string PhaseNo
        {
            get
            {
                return Convert.ToString(GetExtraInfo("phNo"));
            }
            set
            {
                if (value == null || value == String.Empty)
                    SetExtraInfo("phNo", DBNull.Value);
                else
                    SetExtraInfo("phNo", value);
            }
        }

        /// <summary>
        /// Gets or Sets the description / subject of the file phase.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Description
        {
            get
            {
                return Convert.ToString(GetExtraInfo("phDesc"));
            }
            set
            {
                if (value == null || value == String.Empty)
                    SetExtraInfo("phDesc", DBNull.Value);
                else
                    SetExtraInfo("phDesc", value);
            }
        }


        #endregion

        #region Methods

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a file phase based on an identifier value.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static FilePhase GetPhase(long id)
        {
            Session.CurrentSession.CheckLoggedIn();
            return new FilePhase(id);
        }


        #endregion
    }
}

