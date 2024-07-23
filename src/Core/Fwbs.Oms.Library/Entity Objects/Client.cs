using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using FWBS.OMS.Security;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.StatusManagement;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IClient
    {
        /// <summary>
        /// Gets the client ID.
        /// </summary>
        /// <remarks></remarks>
        long ClientID { get; }
        /// <summary>
        /// Gets the client no.
        /// </summary>
        /// <remarks></remarks>
        string ClientNo { get; }
    }


    /// <summary>
    /// A Client object that holds all information about a group of clients that make a client
    /// entity.  This client object can be used with the enquiry engine.  This object
    /// is an OMS configurable type that can appear different to other types of clients.
    /// </summary>
    /// <remarks></remarks>
    [Security.SecurableType("CLIENT")]
	public class Client : PasswordProtectedBase, IOMSType, IDisposable,IExtendedDataCompatible, IAlert, Security.ISecurable, IClient
	{
		#region Events

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

		#endregion

		#region Fields
        /// <summary>
        /// 
        /// </summary>
        private const string ExtFileBalDatalist = "_DSFILTIMSTATS";
        /// <summary>
        /// 
        /// </summary>
        private const string ExtClientBalDatalist = "_DSCLITIMSTATS";

        /// <summary>
        /// A temp variable to skip the password check on a refresh.
        /// </summary>
		private bool _checkPassword = true;

        /// <summary>
        /// Sets the Default Tab when called from the WinUI Layer
        /// </summary>
		private string _defaultTab = null;

        /// <summary>
        /// Internal data source.
        /// </summary>
		private DataSet _client = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string Sql = "select * from dbclient";
        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
		internal const string Sql_ContactLink = "select * from dbclientcontacts";
        /// <summary>
        /// The Time Records information table name.
        /// </summary>
		public const string Table_TimeRecords = "TIMERECORDING";
        /// <summary>
        /// The Time Stats information table name.
        /// </summary>
		public const string Table_TimeStats = "TIMESTATS";
        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
		public const string Table = "CLIENT";
        /// <summary>
        /// The table name of the clients collection of files.
        /// </summary>
		public const string Table_Files = "FILES";
        /// <summary>
        /// The table name of the clients collection of file phases.
        /// </summary>
		public const string Table_FilePhases = "FILEPHASES";
        /// <summary>
        /// The table name of the default contact information.
        /// </summary>
		public const string Table_DefContact = "DEFAULTCONTACT";
        /// <summary>
        /// The table name of the default contacts multi addresses.
        /// </summary>
		public const string Table_DefContactAddresses = "DEFAULTCONTACTADDRESS";
        /// <summary>
        /// The table name of the default contacts multi telephone numbers.
        /// </summary>
		public const string Table_DefContactNumbers = "DEFAULTCONTACTNUMBERS";
        /// <summary>
        /// The table name of the default contacts multi email addresses.
        /// </summary>
		public const string Table_DefContactEmails = "DEFAULTCONTACTEMAILS";
        /// <summary>
        /// The table name of the table containing all the valid contacts under the client.
        /// </summary>
		public const string Table_Contacts = "CONTACTS";

        /// <summary>
        /// A reference to the current session object.
        /// </summary>
		private FWBS.OMS.Session _session = null;

        /// <summary>
        /// Holds the different extended data sources for the file.
        /// </summary>
		private ExtendedDataList _extData = null;

        /// <summary>
        /// Holds a reference to the default address.
        /// </summary>
		private Address _defAddress = null;

        /// <summary>
        /// A collection of associates of the file.
        /// </summary>
		private TimeCollection _timerecords = null;

		//Pre-Client file information.
        /// <summary>
        /// 
        /// </summary>
		private string _preclientfiletype = "";
        /// <summary>
        /// 
        /// </summary>
		private string _preclientdept = "";
        /// <summary>
        /// 
        /// </summary>
		private string _preclientfiledesc = "";

        /// <summary>
        /// Temporary alert.  This is currently used so that the WinUI can add a message to
        /// the select client file screen.  This may be reviewed later.
        /// </summary>
		private Alert _tempAlert;

        /// <summary>
        /// Gets a value indicating whether ClientName has been changed.
        /// </summary>
        private bool _isClientNameDirty;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new client object.  This routine is used by the enquiry engine
        /// to create new client object.
        /// </summary>
        /// <remarks></remarks>
        internal Client()
		{
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
            dt.Columns["clid"].AutoIncrement = true;
            
            //Add a new record.
			Global.CreateBlankRecord(ref dt, true);
			
			if (_client == null) _client = new DataSet("CLIENTINFO");
			_client.Tables.Add(dt);

			//Set the created by and created date of the item.
            SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Created", DateTime.Now);

			//Set all the other default / inherited data.
			//TODO: Allow for non configured Fee Earners and Branch settings
			SetExtraInfo("brid", Session.CurrentSession.ID);
			SetExtraInfo("clguid", System.Guid.NewGuid());
			try
			{
				SetExtraInfo("feeusrid", Session.CurrentSession.CurrentFeeEarner.ID);
			}
			catch(Exception ex)
			{
				throw new OMSException2("ERRNOFE","Your user settings 'Work For' have not been set.","",ex);
			}

            try
            {
                Branch = Session.CurrentSession.CurrentFeeEarner.Branch;
            }
            catch
            { }

			this.ClientInit();

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }


        /// <summary>
        /// Creates a client with a specified client type and default contact.
        /// </summary>
        /// <param name="clType">Type of the cl.</param>
        /// <param name="defaultContact">The default contact.</param>
        /// <param name="asPreClient">if set to <c>true</c> [as pre client].</param>
        /// <remarks></remarks>
		public Client (ClientType clType, Contact defaultContact, bool asPreClient) : this()
		{
			Session.CurrentSession.CheckLoggedIn();
			SetExtraInfo("cltypecode", clType.Code);
			SetExtraInfo("clpreclient", asPreClient);
			DefaultContact = defaultContact;
			GenerateSearchKeywords();
		}

        /// <summary>
        /// Creates a client with a specified client type.
        /// </summary>
        /// <param name="clType">Type of the cl.</param>
        /// <param name="asPreClient">if set to <c>true</c> [as pre client].</param>
        /// <remarks></remarks>
		public Client (ClientType clType, bool asPreClient) : this()
		{
			Session.CurrentSession.CheckLoggedIn();
			SetExtraInfo("cltypecode", clType.Code);
			SetExtraInfo("clpreclient", asPreClient);
		}

        /// <summary>
        /// Initialised an existing client object with the specified identifier.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Client (long id)
		{
			Fetch(id, null);

			//An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentClients.Add(ClientID.ToString(), this);

            try
            {
                Contact dummy = this.DefaultContact;
            }
            catch (Exception ex)
            {
                throw new PermissionsException(ex, "EXPERMDENIED2", "Permission Denied", true);
            }

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }

        /// <summary>
        /// Initialised an existing client object with the specified string identifier.
        /// </summary>
        /// <param name="clno">The clno.</param>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Client (string clno)
		{
			if (string.IsNullOrWhiteSpace(clno))
				throw new OMSException2(HelpIndexes.ClientNotFound.ToString(), "", "", null,true, clno);

			IDataParameter[] pars = new IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.CreateParameter("CLNO", clno);
			_client = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clNO = @CLNO", new string [1] {Table}, pars);
			if ((_client.Tables[Table] == null) || (_client.Tables[Table].Rows.Count == 0)) 
			{
				FWBS.OMS.OMSFile _tmp = null;
				// Code to decide what tables to use to filter and gather Client record or details.
				if (Session.CurrentSession.CurrentBranch.SchFileAccFirst == true)
				{
					_tmp = FWBS.OMS.OMSFile.GetFile(clno,"fileacccode");
					if (_tmp == null)
					{
						_client = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clAccCode = @CLNO", new string [1] {Table}, pars);
						if ((_client.Tables[Table] == null) || (_client.Tables[Table].Rows.Count == 0)) 
						{
                            throw new OMSException2(HelpIndexes.ClientNotFound.ToString(), "", "", null, true, clno);
                        }
					}
					else
					{
						FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative = true;
						pars[0] = Session.CurrentSession.Connection.CreateParameter("CLNO", _tmp.Client.ClientNo);
						_client = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clNo = @CLNO", new string [1] {Table}, pars);
					}
				}
				else
				{
					_client = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clAccCode = @CLNO", new string [1] {Table}, pars);
                    if ((_client.Tables[Table] == null) || (_client.Tables[Table].Rows.Count == 0))
                    {
                        try
                        {
                            // Try via FileAccCode
                            _tmp = FWBS.OMS.OMSFile.GetFile(clno, "fileacccode");
                        }
                        catch (OMSException ex)
                        {
                            if (ex.HelpID != HelpIndexes.OMSFileNotFound)
                                throw;


                        }

                        if (_tmp == null)
                        {
                            throw new OMSException2(HelpIndexes.ClientNotFound.ToString(), "", "", null, true, clno);
                        }
                        else
                        {
                            FWBS.OMS.Session.CurrentSession.CurrentFile = _tmp;
                            FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative = true;
                            pars[0] = Session.CurrentSession.Connection.CreateParameter("CLNO", _tmp.Client.ClientNo);
                            _client = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clNo = @CLNO", new string[1] { Table }, pars);
                        }

                    }
				}
			}
			this.ClientInit();

            try
            {
                Contact dummy = this.DefaultContact;
            }
            catch (Exception ex)
            {
                throw new PermissionsException(ex, "EXPERMDENIED2", "Permission Denied", true);
            }
            
            //An edit contructor should add the object created to the session memory collection.
			Session.CurrentSession.CurrentClients.Add(ClientID.ToString(), this);

            //Call the extensibility event for addins.
            this.OnExtLoaded();
        }

        /// <summary>
        /// Constructs a client object.
        /// </summary>
        /// <param name="id">The client id to retrieve.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
		private void Fetch (long id, DataRow merge)
		{
			_defAddress = null;
			DataSet data = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where clID = " + id.ToString(), new String [1] {Table}, new IDataParameter[0]);
            if ((data.Tables[Table] == null) || (data.Tables[Table].Rows.Count == 0)) 
			{
                throw new OMSException2(HelpIndexes.ClientNotFound.ToString(), "", "", null, true, id.ToString());
            }

            if (merge != null)
                Global.Merge(data.Tables[Table].Rows[0], merge);

            _client = data;

			this.ClientInit();
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
                    _isNewDataFormat = Session.CurrentSession.IsProcedureInstalled(newClientRecord);
                }

                return _isNewDataFormat.Value;
            }
        }

        /// <summary>
        /// Gets the query parameters.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private IDataParameter[] GetQueryParameters()
        {
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = _session.Connection.AddParameter("@CLID", System.Data.SqlDbType.BigInt, 0, this.ClientID);
            paramlist[1] = _session.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);

            return paramlist;
        }

        private IDataParameter[] GetFileQueryParameters(bool allFiles)
        {
            var parameters = GetQueryParameters();

            if (!allFiles)
            {
                var retParams = new IDataParameter[3];
                retParams[0] = parameters[0];
                retParams[1] = parameters[1];
                retParams[2] = _session.Connection.AddParameter("@RECORDCOUNT", System.Data.SqlDbType.BigInt, 10, DefaultFileReturnCount);

                return retParams;
            }

            return parameters;
        }

        private static long DefaultFileReturnCount
        {
            get
            {
                return Session.CurrentSession.DefaultClientFileReturnCount;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const string originalClientRecord = "sprClientRecord";
        /// <summary>
        /// 
        /// </summary>
        private const string newClientRecord = "sprClientRecord2";
        /// <summary>
        /// Gets the client data.
        /// </summary>
        /// <param name="tableNames">The table names.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private DataSet GetClientData(string[] tableNames)
        {
            var paramList = GetQueryParameters();

            string procedure = originalClientRecord;
            if (IsNewDataFormat)
            {
                procedure = newClientRecord;
               
                var tNames = new List<string>(tableNames);
                tNames.Remove(Table_Files);
                tableNames = tNames.ToArray();

                _hasFiles = HasFiles.No;
            }
            else
                _hasFiles = HasFiles.All;

            return Session.CurrentSession.Connection.ExecuteProcedureDataSet(procedure, tableNames, paramList);
        }

        /// <summary>
        /// Initialisation code once the client has been found.  This will add extra
        /// informational tables to the underlying data set to save repetetive database
        /// connectivity operations.
        /// </summary>
        /// <remarks></remarks>
		private void ClientInit()
		{
			// Take current CLTYPEID and populate the contacts
			try
			{
				_session = Session.OMS;

				_client.Merge(GetClientData(new string[] {Table_Files}),true);

                timestamp = DateTime.UtcNow;

				// Rename the default tables, must be kept in the same order
				NameTables();
			
			}
			catch(Exception ex)
			{
				throw new OMSException2("19005","Error generating Client Information DataSet for CLID : %1%", ex ,true,this.ClientID.ToString());
			}

            //Only check passwords when the basic security type is being used.
            if (!Session.CurrentSession.AdvancedSecurityEnabled)
            {
                //Ask for the password on open.  Perhaps extend the file object so that the
                //password can be applied to different levels of security, like viewing, changing
                //opeing, deleting etc...
                if (_checkPassword)
                {
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
        /// Gets the status code of the current client.
        /// </summary>
        /// <value>The status.</value>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string Status
        {
            get
            {
                if (_client.Tables[Table].Columns.Contains("clStatus"))
                    return Convert.ToString(GetExtraInfo("clStatus"));
                else
                    return "";
            }
            set
            {
                string oldval = Status;
                if (value != oldval)
                {
                    if (_client.Tables[Table].Columns.Contains("clStatus"))
                    {
                        SetExtraInfo("clStatus", value);
                        OnPropertyChanged(new PropertyChangedEventArgs("Status", oldval, value));
                    }
                }
                
            }
        }


        /// <summary>
        /// Names the tables within the underlying data set.
        /// </summary>
        /// <remarks></remarks>
		private void NameTables()
		{
			// Rename the default tables, must be kept in the same order
            int tablePos = 1;
            int phasesTable = 7;
            if (IsNewDataFormat)
            {
                phasesTable = 6;
            }
            else
            {
                _client.Tables[tablePos++].TableName = Table_Files;
            }

            _client.Tables[tablePos++].TableName = Table_DefContact;
            _client.Tables[tablePos++].TableName = Table_DefContactAddresses;
            _client.Tables[tablePos++].TableName = Table_DefContactNumbers;
            _client.Tables[tablePos++].TableName = Table_DefContactEmails;
            _client.Tables[tablePos++].TableName = Table_Contacts;
            if (_client.Tables.Count > phasesTable)
			{
                _client.Tables[phasesTable].TableName = Table_FilePhases;
				_client.Tables[Table_FilePhases].Columns["phid"].AllowDBNull = true;
				DataRow r = _client.Tables[Table_FilePhases].NewRow();
				r["phid"] = DBNull.Value;
				r["phdesc"] = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not set)", "").Text;
                r["Created"] = DateTime.MinValue.ToLocalTime();
				_client.Tables[Table_FilePhases].Rows.Add(r);
				_client.Tables[Table_FilePhases].AcceptChanges();
			}
		}

		#endregion

		#region Contact Specific Methods

        /// <summary>
        /// Gets the combined addresses of all contacts.
        /// </summary>
        /// <param name="includeDefault">if set to <c>true</c> [include default].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public DataView GetAddresses(bool includeDefault)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("addid", typeof(long));
			dt.Columns.Add("addline1", typeof(string));
			dt.Columns.Add("addline2", typeof(string));
			dt.Columns.Add("addline3", typeof(string));
			dt.Columns.Add("addline4", typeof(string));
			dt.Columns.Add("addline5", typeof(string));
			dt.Columns.Add("addpostcode", typeof(string));
			dt.Columns.Add("refcount", typeof(int)).DefaultValue = 0;
			dt.Columns.Add("notes", typeof(string));

			Contact[] arr = Contacts;

			foreach (Contact cont in arr)
			{
				DataView vw = cont.GetAddresses("", includeDefault, false,true);
				
				foreach (DataRowView r in vw)
				{
					DataView exists = new DataView(dt);

					exists.RowFilter = "addid = " + Convert.ToString(r["contaddid"]);
					if (exists.Count > 0)
					{
						exists[0]["refcount"] = Convert.ToInt32(exists[0]["refcount"]) + 1;
						exists[0]["notes"] = Convert.ToString(exists[0]["notes"]) +", " + cont.Name + " - " + Convert.ToString(r["ContTypeDesc"]);
					}
					else
					{
						DataRow add = dt.NewRow();
						
						add["addid"] = r["contaddid"];

						if (r["contaddid"] == DBNull.Value || Convert.ToInt64(r["contaddid"]) == 0)
							add["addline1"] = r["conttypedesc"];
						else
							add["addline1"] = r["addline1"];
						
						add["addline2"] = r["addline2"];
						add["addline3"] = r["addline3"];
						add["addline4"] = r["addline4"];
						add["addline5"] = r["addline5"];
						add["addpostcode"] = r["addpostcode"];
						add["refcount"] = 1;
						add["notes"] = cont.Name + " - " + Convert.ToString(r["ContTypeDesc"]);
						dt.Rows.Add(add);
					}
				}
			}

			//Add the default address to the list even if it is still no longer used / made inactive by
			//the contact that used to use it.
			DataView active = new DataView(dt);
			Address a = defaultAddress;
			active.RowFilter = "addid = " + Convert.ToString(_client.Tables[Table_DefContactAddresses].Rows[0]["addid"]);
			if (active.Count == 0)
			{
				DataRow add = dt.NewRow();
				add["addid"] = a.GetExtraInfo("addid");
				add["addline1"] = Session.CurrentSession.Resources.GetResource("DELETED", "Deleted", "").Text;
				add["addline2"] = a.GetExtraInfo("addline2");
				add["addline3"] = a.GetExtraInfo("addline3");
				add["addline4"] = a.GetExtraInfo("addline4");
				add["addline5"] = a.GetExtraInfo("addline5");
				add["addpostcode"] = a.GetExtraInfo("addpostcode");
				add["refcount"] = 1;
				add["notes"] = add["addline1"];
				dt.Rows.Add(add);
			}

			return dt.DefaultView;
		}

        /// <summary>
        /// Gets the specified contacts default address on the client.
        /// </summary>
        /// <param name="contact">Contact row position.</param>
        /// <returns>An address object.</returns>
        /// <remarks></remarks>
		public Address GetContactAddress(int contact)
		{
			// Check current Client Is initialised
			if ((_client == null) || (_client.Tables.Contains(Table_Contacts)))
				throw new OMSException(HelpIndexes.ClientNotInitialised);
			
			if  (contact > _client.Tables[Table_Contacts].Rows.Count) 
				contact = _client.Tables[Table_Contacts].Rows.Count - 1;
			
			if (contact > -1)
			{
				Contact tmpcontact = Contact.GetContact((long)_client.Tables[Table_Contacts].Rows[contact]["CONTID"]);
				return tmpcontact.DefaultAddress;
			}
			else
				return null;
		}

        /// <summary>
        /// Gets the contacts collection in object form.
        /// </summary>
        /// <remarks></remarks>
		public Contact[] Contacts
		{
			get
			{
				DataView vw = GetContacts();
				Contact [] conts = new Contact[vw.Count];
				for (int ctr = 0; ctr < conts.Length; ctr++)
				{
					conts[ctr] = Contact.GetContact((long)vw[ctr]["contid"]);
				}
				return conts;
			}
		}

        /// <summary>
        /// Gets the omsfiles collection in object form.
        /// </summary>
        /// <remarks></remarks>
        [Obsolete]
        public OMSFile[] OMSFiles
        {
            get
            {
                DataView vw = GetFiles(true);
                OMSFile[] omsfiles = new OMSFile[vw.Count];
                for (int ctr = 0; ctr < omsfiles.Length; ctr++)
                {
                    omsfiles[ctr] = OMSFile.GetFile((long)vw[ctr]["fileid"]);
                }
                return omsfiles;
            }
        }

        /// <summary>
        /// Gets or Sets the clients default contact.
        /// </summary>
        /// <value>The default contact.</value>
        /// <remarks></remarks>
		public Contact DefaultContact
		{
			get
			{
				ClientContactLink link = null;
				Contact cont = null;
				object def = GetExtraInfo("cldefaultcontact");
				if (def is DBNull)
				{
					link = GetContact(0);
					if (link != null) cont = link.Contact;
					if (cont != null)
					{
						//Set the default address if one is in the multi address table.
						SetExtraInfo("cldefaultcontact", cont.ID);
					}
				}
				else
				{
					cont = Contact.GetContact((long)def);
				}
				
				return cont;
			}
			set
			{
                if (value == null)
                    SetExtraInfo("cldefaultcontact", DBNull.Value);
                else
                {
                    Contact oldval = DefaultContact;
                    if (oldval == null || oldval.ID != value.ID)
                    {

                        SetExtraInfo("cldefaultcontact", value.ID);
                        //DefaultAddress = value.DefaultAddress;
                        AddContact(new ClientContactLink(this, value));
                    }
                }
			}
		}

        /// <summary>
        /// Sets the default contact and relation ship.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="RelationCode">The relation code.</param>
        /// <remarks></remarks>
		public void SetDefaultContactAndRelationShip(Contact value, string RelationCode)
		{
			if (value == null)
			{
				SetExtraInfo("cldefaultcontact", DBNull.Value);
			}
			else
			{
				SetExtraInfo("cldefaultcontact", value.ID);
				//DefaultAddress = value.DefaultAddress;
				ClientContactLink link = new ClientContactLink(this, value);
				link.RelationCode = RelationCode;
				AddContact(link);
			}
		}

        /// <summary>
        /// Gets the contacts data view of the client.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
		public DataView GetContacts()
		{
			return GetContacts(false);
		}

        /// <summary>
        /// Gets the contacts data view of the client.
        /// </summary>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public DataView GetContacts(bool all)
		{
			//Filter out any inactive contacts and sort by a prefered order.
			DataView vw = new DataView(_client.Tables[Table_Contacts]);
			if (all == false) vw.RowFilter = "clactive = true";
			return vw;
		}

        /// <summary>
        /// Checks to see if the client already has a contact associated.
        /// </summary>
        /// <param name="contact">A contact object to check.</param>
        /// <returns>A flag indicating if the specified contact exists or not.</returns>
        /// <remarks></remarks>
		public bool HasContact(Contact contact)
		{
			//Throw a not initialised exception if the contacts table does not exist.
			if (_client.Tables.Contains(Table_Contacts) == false)
				return false;

			if (contact == null) return false;
			DataView cont = GetContacts(true);
			cont.RowFilter ="contid = '" + contact.ID + "'";
			if (cont.Count > 0)
				return true;
			else
				return false;
		}

        /// <summary>
        /// Clears all the contacts within the contacts table.
        /// </summary>
        /// <remarks></remarks>
		public void ClearContacts()
		{
			_client.Tables[Table_Contacts].Clear();
			DefaultContact = null;
		}

        /// <summary>
        /// Get a contact in a prefered priority order.
        /// </summary>
        /// <param name="prefered">Will select the prefered contact from the client in row (0 zero is the most prefered).</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public ClientContactLink GetContact(int prefered)
		{
			//Throw a not initialised exception if the contacts table does not exist.
			if (_client.Tables.Contains(Table_Contacts) == false)
				throw new OMSException(HelpIndexes.ClientNotInitialised);


			//If the prefered number is greater than the number of available records
			//then set the prefered index to the last in the list.
			DataView vw = GetContacts();
			if (vw.Count < (prefered + 1))
			{
				prefered = vw.Count - 1;
			}

			//Get a contact object based on the row number.  If the prefered number is less
			//than zero then return a null reference, meaning that the client has no contact
			//in its contact list.
			if (prefered > -1)
			{
				DataRow row = vw[prefered].Row;
				return new ClientContactLink(row);
			}
			else
				return null;

		}

        /// <summary>
        /// Get a contact by its id.
        /// </summary>
        /// <param name="id">Will select the contact from the client.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public ClientContactLink GetContact(long id)
		{
			//Throw a not initialised exception if the contacts table does not exist.
			if (_client.Tables.Contains(Table_Contacts) == false)
				throw new OMSException(HelpIndexes.ClientNotInitialised);


			DataView vw = GetContacts(true);
			vw.RowFilter = "contid = " + id.ToString();

			//Get a contact object based on the row number.  If the prefered number is less
			//than zero then return a null reference, meaning that the client has no contact
			//in its contact list.
			if (vw.Count > 0)
			{
				DataRow row = vw[0].Row;
				return new ClientContactLink(row);
			}
			else
				return null;

		}


        /// <summary>
        /// This Method will add a contact to a client if it does not already exist.
        /// </summary>
        /// <param name="contactInfo">The contact object to be added.</param>
        /// <returns>A boolean value to indicate whether it has been added or not.</returns>
        /// <remarks></remarks>
		public void AddContact(ClientContactLink contactInfo)
		{	
			bool exists = HasContact(contactInfo.Contact);
				//Add the contact if it does not exist.
			if (!exists)
			{

				//Check to see if the client has a maximum number of contacts already.
				Validate(true, true);


                DataTable contacts = _client.Tables[Table_Contacts];
                if (contacts.Columns.Contains("ID"))
                    contacts.Columns["ID"].AutoIncrement = true;

				// Add Row to the contact Table
				DataRow dr = contacts.NewRow();

				Contact cont = contactInfo.Contact;

				dr["clid"] = this.ClientID;
				dr["contid"] = cont.ID;
				dr["clposition"] = "";
                if (String.IsNullOrEmpty(contactInfo.RelationCode))
                    dr["clrelation"] = DBNull.Value;
                else
                    dr["clrelation"] = contactInfo.RelationCode;
				dr["clnotepad"] = contactInfo.Notes;
				dr["clactive"] = true;
				//Because the database has not been updated and the sprClientRecord procedure
				//has not been run, update the static contact record as good as possible.
				dr["ContactTypeDesc"] = cont.CurrentContactType.Description;
				DataTable dt = cont.GetDataTable();
				foreach (DataColumn col in dt.Columns)
				{
					if (dr.Table.Columns.Contains(col.ColumnName))
						dr[col.ColumnName] = dt.Rows[0][col];
				}
				
				contacts.Rows.Add(dr);

			}
			else
			{
				contactInfo = GetContact(contactInfo.Contact.ID);
				contactInfo.Restore();
			}

				//TODO: May have to put this within the update event incase a client being created gets cancelled.
				//Flag the contact to be a client.
			if (contactInfo.Contact.IsClient == false)
			{
				contactInfo.Contact.IsClient = true;
				contactInfo.Contact.Update(true);
			}

			if (IsNew == false)
			{
				DataView dv = new DataView(GetFiles(true).Table);
				dv.RowFilter = "filestatus <> 'DEAD'";
				if (!exists && dv.Count > 0)
				{
					AskEventArgs askargs = new AskEventArgs("CONTASS","Do you wish to Associate this %CLIENT% Contact to all Active %FILES%","",AskResult.Yes);
					FWBS.OMS.Session.CurrentSession.OnAsk(this,askargs);
					if (askargs.Result == AskResult.Yes)
					{
						FWBS.OMS.Session.CurrentSession.OnProgressStart();
						FWBS.Common.ProgressEventArgs _progress = new FWBS.Common.ProgressEventArgs(GetFiles(true).Table.Rows.Count);
						FWBS.OMS.Session.CurrentSession.OnProgress(_progress);
						//Loops Around all the Active Files and Add this Contact as an Assoicate
						//Ask to be added by Mike when Micrsoft required for the Features

                        try
                        {
                            foreach (DataRowView dr in dv)
                            {
                                FWBS.OMS.OMSFile file = null;
                                try
                                {
                                    file = FWBS.OMS.OMSFile.GetFile(Convert.ToInt64(dr["fileid"]));
                                    _progress.Message = "Processing : " + file.FileDescription;
                                    FWBS.OMS.Associate ass = new FWBS.OMS.Associate(contactInfo.Contact, file, "CLIENT");
                                    ass.Update();
                                }
                                catch (FWBS.OMS.OMSException omsex)
                                {
                                    if (omsex.HelpID != HelpIndexes.PasswordRequestCancelled)
                                        throw;
                                }
                                
                                _progress.Current++;
                                FWBS.OMS.Session.CurrentSession.OnProgress(_progress);
                            }
                        }
                        finally
                        {
                            FWBS.OMS.Session.CurrentSession.OnProgressFinished();
                        }
					}
				}
			}
		}

        /// <summary>
        /// Removes a contact from the client.
        /// </summary>
        /// <param name="contact">The contact to remove from the client.</param>
        /// <returns>True if the contact has been successfully deleted.</returns>
        /// <remarks></remarks>
		public void RemoveContact(Contact contact)
		{	
			ClientContactLink link = GetContact(contact.ID);
			if (link != null)
			{
				link.Delete();
			}

		}


		#endregion

		#region File Specific Methods

        /// <summary>
        /// Gets a list of file phases for the specified file id.
        /// </summary>
        /// <param name="fileid">The fileid.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public DataView GetFilePhases(long fileid)
		{
			DataView vw = new DataView(_client.Tables[Table_FilePhases]);
			vw.RowFilter = "phid is null or fileid = " + fileid.ToString();
			vw.Sort = "Created";
			return vw;
		}

        /// <summary>
        /// Validates whether the client owns a specific file object.
        /// </summary>
        /// <param name="file">The file object being tested.</param>
        /// <returns>True if the file object is owned by the client.</returns>
        /// <remarks></remarks>
		public bool HasFile(OMSFile file)
		{
			if (file == null)
                return false;
            var hasFile = _HasFile(file, false);

            if (!hasFile && _hasFiles != HasFiles.All)
            {
                return _HasFile(file, true);
            }

            return hasFile;

		}

        private bool _HasFile(OMSFile file, bool searchAllFiles)
        {
            DataView files = GetFiles(searchAllFiles);
			files.RowFilter = "fileid = '" + file.ID + "'";
            return files.Count > 0;
        }

        private enum HasFiles
        {
            [System.ComponentModel.Description("No Matters")]
            No,
            [System.ComponentModel.Description("Top N Live Matters")]
            Top,
            [System.ComponentModel.Description("All Live Matters")]
            Live,
            [System.ComponentModel.Description("All The Matters")]
            All
        }

        private HasFiles _hasFiles;

        public bool HasAllLiveFiles
        {
            get { return _hasFiles == HasFiles.Live || _hasFiles == HasFiles.All; }
        }

        /// <summary>
        /// 
        /// </summary>
        private const string FilesStoredProc = "sprClientRecord_Files";
        /// <summary>
        /// Gets the files table.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private DataTable GetFilesTable(bool allFiles)
        {
            if (IsNewDataFormat)
            {
                if (allFiles && _hasFiles != HasFiles.All)
                {
                    AddAllFilesTable();
                }
                if (!_client.Tables.Contains(Table_Files))
                {
                    AddFilesTable();
                }
            }

            return _client.Tables[Table_Files];
        }

        private void AddAllFilesTable()
        {

            var data = GetFilesDataTable(true);

            if (_client.Tables.Contains(Table_Files))
                _client.Tables.Remove(Table_Files);

            _client.Tables.Add(data.Tables[0].Copy());
            _hasFiles = HasFiles.All;
        }

        private void AddFilesTable()
        {
            var data = GetFilesDataTable(false);

            var fileTable = data.Tables[0].Copy();

            //CM 15.04.14 - Should check if a "FILES" DataTable exists already and remove before adding a new one
            if (_client.Tables.Contains(Table_Files))
                _client.Tables.Remove(Table_Files);
                        
            _client.Tables.Add(fileTable);

            if (DefaultFileReturnCount == 0)
                _hasFiles = HasFiles.All;
            else if (fileTable.Rows.Count < DefaultFileReturnCount)
                _hasFiles = HasFiles.Live;
            else
                _hasFiles = HasFiles.Top;
        }

        private DataSet GetFilesDataTable(bool allFiles)
        {
            var paramlist = GetFileQueryParameters(allFiles);

            return Session.CurrentSession.Connection.ExecuteProcedureDataSet(FilesStoredProc, new string[] { Table_Files }, paramlist);
        }
        /// <summary>
        /// Returns a list of files for the client.
        /// </summary>
        /// <returns>A data view of files.</returns>
        /// <remarks></remarks>
        public DataView GetFiles()
        {
            return GetFiles(true);
        }

		private DataView GetFiles(bool all)
		{
            var fileTable = GetFilesTable(all);

            DataView vw = new DataView(fileTable);
			return vw;
		}

		#endregion

		#region Methods

        /// <summary>
        /// Find a file by file number under the client.
        /// </summary>
        /// <param name="fileno">The fileno.</param>
        /// <param name="includeDeadFiles">if set to <c>true</c> [include dead files].</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public OMSFile FindFile(string fileno, bool includeDeadFiles)
		{
            var file = FindFile(fileno, includeDeadFiles, false);
            if (file == null && _hasFiles != HasFiles.All)
                file = FindFile(fileno, includeDeadFiles, true);

            return file;
		}


        private OMSFile FindFile(string fileno, bool includeDeadFiles, bool searchAllFiles)
        {
            DataView vw = GetFiles(searchAllFiles);
            string filter = "fileno='" + Common.SQLRoutines.RemoveRubbish(fileno) + "'";
            if (includeDeadFiles == false) filter += " and filestatus like 'LIVE%'";

            vw.RowFilter = filter;

            if (vw.Count > 0)
            {
                return OMSFile.GetFile(Convert.ToInt64(vw[0]["fileid"]));
            }
            return null;
        }
       

        /// <summary>
        /// Converts a pre-client to a full client.
        /// </summary>
        /// <remarks></remarks>
		public void ConvertPreClient()
		{
			//Conversion date gets set on SetExtraInfo.
			SetExtraInfo("clpreclient", false);
		}

        /// <summary>
        /// Generates the best guess search keywords of the client based on the client
        /// and contact names.
        /// </summary>
        /// <param name="overwrite">Overwrites any existing key words.</param>
        /// <remarks></remarks>
		public void GenerateSearchKeywords(bool overwrite)
		{
			string inString = ClientName;
			string [] words = inString.Split(new char[] {' ', '(', ')', '.', ';', ','});

			int idx = 0;
			foreach (string wordRO in words)
			{
				string word = wordRO.Trim();
				switch (word.ToUpper())
				{
					case "":
						word = "";
						break;

					case "PLC": goto case "";
					case "MR": goto case "";
					case "MRS": goto case "";
					case "CORP": goto case "";
					case "INC": goto case "";
					case "CORPORATION": goto case "";
					case "MISS": goto case "";
					case "MS": goto case "";
					case "DR": goto case "";
					case "SIR": goto case "";
					case "MADAM": goto case "";
					case "PROF": goto case "";
					case "PROFESSOR": goto case "";
					case "ESQ": goto case "";
					case "ESQ.": goto case "";
					case "ESQUIRE": goto case "";
					case "MP": goto case "";
					case "BSC": goto case "";
					case "OBE": goto case "";
					case "MBE": goto case "";
					case "HONS": goto case "";
					case "PHD": goto case "";
					case "SON": goto case "";
					case "SONS": goto case "";
					case "BARONESS": goto case "";
					case "DOCTOR": goto case "";
					case "AND": goto case "";
					case "&": goto case "";
					case "LTD": goto case "";
					case "LIMITED": goto case "";
					case "T/A": goto case "";
					case "T/AS": goto case "";
					case "TA": goto case "";
					case "UK": goto case "";
					case "ASSOC": goto case "";
					case "ASSOCIATES": goto case "";
					case "BS": goto case "";
					case "PARTNER": goto case "";
					case "PARTNERS": goto case "";
					case "MESSRS": goto case "";
					case "THE": goto case "";
					case "TRUSTEES": goto case "";
					case "CO": goto case "";
					default:
						string filter = _session.GetSessionConfigSetting("/config/clientSearch/keywordFilter/filter[.='" + XmlConvert.EncodeNmToken(word) + "']", "");
						if (filter != String.Empty)
							goto case "";
						break;

				}

				if (word.Length <= 1)
					continue;
				else
				{
					idx++;
					switch (idx)
					{
						case 1:
							if (SearchKeyword1 == String.Empty || overwrite)
								SearchKeyword1 = word;
							else
								goto case 2;
							break;
						case 2:
							if (SearchKeyword2 == String.Empty || overwrite)
								SearchKeyword2 = word;
							else
								goto case 3;
							break;
						case 3:
							if (SearchKeyword3 == String.Empty || overwrite)
								SearchKeyword3 = word;
							else
								goto case 4;
							break;
						case 4:
							if (SearchKeyword4 == String.Empty || overwrite)
								SearchKeyword4 = word;
							else
								goto case 5;
							break;
						case 5:
							if (SearchKeyword5 == String.Empty || overwrite)
								SearchKeyword5 = word;
							return;
					}
				}
			}
		}

        /// <summary>
        /// Clears the search keywords.
        /// </summary>
        /// <remarks></remarks>
		public void ClearSearchKeywords()
		{
			SearchKeyword1 = "";
			SearchKeyword2 = "";
			SearchKeyword3 = "";
			SearchKeyword4 = "";
			SearchKeyword5 = "";
		}

        /// <summary>
        /// Generates the best guess search keywords of the client based on the client
        /// and contact names.  It will not overwrite any existing keywords that have
        /// already been set.
        /// </summary>
        /// <remarks></remarks>
		public void  GenerateSearchKeywords()
		{
			GenerateSearchKeywords(false);
		}

        /// <summary>
        /// Updates the specific table of the object and persists it to the database.
        /// </summary>
        /// <param name="tablename">The table name to update.</param>
        /// <param name="sql">The select statement to use.</param>
        /// <remarks></remarks>
		internal void Update(String tablename,String sql)
		{
			if (_client.Tables.Contains(tablename) == false)
				throw new OMSException(HelpIndexes.ClientNotInitialised);

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (_client.Tables[tablename].GetChanges()!= null)
			{
				_session.Connection.Update(_client.Tables[tablename],sql);
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
			SetExtraInfo("clnotes", notes);
		}

        /// <summary>
        /// Gets the quick file description of a specified file under the client.
        /// </summary>
        /// <param name="row">Data row corresponding to the file.</param>
        /// <returns>A quick file description.</returns>
        /// <remarks></remarks>
        public string FileQuickDescription(DataRowView row)
        {
            if (!_client.Tables.Contains(Table_Contacts))
                throw new OMSException(HelpIndexes.ClientNotInitialised);

            FileType ft = FileType.GetFileType(Convert.ToString(row["filetype"]));

            try
            {
                try
                {
                    OMSFile file = OMSFile.GetFile(Convert.ToInt64(row["fileid"]));
                    
                    string text = ft.FileDetailsConfig;

                    if (text == String.Empty)
                    {
                        text = GetTextFromResFile("DefaultFileDetails");
                    }
                        
                    return ft.ParseDynamicProperty(file, text);
                }
                catch (OMSException ex)
                {
                    if (ex.HelpID == HelpIndexes.PasswordRequestCancelled)
                    {
                        throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
                    }
                }

                return "";
            }
            catch (OMSException ex)
            {
                if (ex.HelpID == HelpIndexes.PasswordRequestCancelled)
                {
                    throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
                }
                else
                {
                    string xml_desc = ft.ReadAttribute(ft.GetConfigRoot(), "FileDescription", FWBS.OMS.Global.GetResString("FileDescriptionDefault", true));
                    return OriginalClientParseString(xml_desc, row, Table_Files, "");
                }
            }
        }

        private string GetTextFromResFile(string resFileDataTagName)
        {
            return (FWBS.OMS.Global.GetResString(resFileDataTagName, true).Replace("%FONT%", CurrentUIVersion.Font)).Replace("%FONTSIZE%", Convert.ToString(CurrentUIVersion.FontSize * 2));
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
				ClientType ct = CurrentClientType;
				string xpath = "defaultTemplates/template[@type = '" + System.Xml.XmlConvert.EncodeName(type) + "']";
				System.Xml.XmlElement el = ct.GetConfigRoot();
				string ret = ct.ReadAttribute(el, xpath,"");
				string title = ct.ReadAttribute(el, xpath + "/@title", "");
				string lib = ct.ReadAttribute(el, xpath + "/@library", "");
				string cat = ct.ReadAttribute(el, xpath + "/@category", "");
				string subcat = ct.ReadAttribute(el, xpath + "/@subcategory", "");
                string minorcat = ct.ReadAttribute(el, xpath + "/@minorcategory", "");

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
			catch{}
			return null;
		}

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="tablename">Table name within the internal dataset.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName,string tablename)
		{
			object val = this.GetExtraInfo(fieldName,tablename,0);
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
        /// <param name="tablename">Table name within the internal dataset.</param>
        /// <param name="row">Row number from the table.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName,string tablename,int row)
		{
			if (!_client.Tables.Contains(tablename))
				throw new OMSException(HelpIndexes.ClientNotInitialised);

			object val = _client.Tables[tablename].Rows[row][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

        /// <summary>
        /// Returns a data table within the underlying dataset.
        /// </summary>
        /// <param name="tablename">The tablename.</param>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public DataTable GetDataTable(string tablename)
		{
            DataTable table = null;
            if (IsNewDataFormat)
            {
                switch (tablename)
                {
                    case Table_Files:
                        {
                            table = GetFilesTable(true);
                            break;
                        }
                }
            }

            if (table == null)
            {
                if (!_client.Tables.Contains(tablename))
                    throw new OMSException(HelpIndexes.ClientNotInitialised);
                table= _client.Tables[tablename].Copy();

            }

            return table.Copy();


			
		}

        /// <summary>
        /// Returns a data view of a specified table within the underlying data.
        /// </summary>
        /// <param name="tablename">Table name to create a view on.</param>
        /// <param name="filter">Filter to use on the view.</param>
        /// <returns>A DataView object.</returns>
        /// <remarks></remarks>
		public DataView GetDataView(string tablename,string filter)
		{
			return this.GetDataView(tablename,filter,"");
		}

        /// <summary>
        /// Returns a data view of a specified table within the underlying data.
        /// </summary>
        /// <param name="tablename">Table name to create a view on.</param>
        /// <param name="filter">Filter to use on the view.</param>
        /// <param name="sort">Field to sort by.</param>
        /// <returns>A DataView object.</returns>
        /// <remarks></remarks>
        public DataView GetDataView(string tablename, string filter, string sort)
        {
            DataTable table = null;
            if (IsNewDataFormat)
            {
                switch (tablename)
                {
                    case Table_Files:
                        {
                            table = GetFilesTable(true);
                            break;
                        }
                }
            }

            if (table == null)
            {

                if (!_client.Tables.Contains(tablename))
                    throw new OMSException(HelpIndexes.ClientNotInitialised);

                table = _client.Tables[tablename];

            }
            DataView tmpview = new DataView(table, filter, sort, DataViewRowState.OriginalRows);
            return tmpview;

        }

        /// <summary>
        /// Return a DataView of the Files table
        /// </summary>
        /// <param name="filter">Filter to use on the View</param>
        /// <param name="sort">Sort to be applied to the view</param>
        /// <param name="allFiles">should force a fetch of all tables</param>
        /// <returns>DataView of the Files Table</returns>
        public DataView GetFilesDataView(string filter, string sort, bool allFiles)
        {
            var table = GetFilesTable(allFiles);

              if (table == null)
                    throw new OMSException(HelpIndexes.ClientNotInitialised);

            DataView tmpview = new DataView(table, filter, sort, DataViewRowState.OriginalRows);
            return tmpview;
        }

        /// <summary>
        /// Overriden method that returns the client textual representation.
        /// </summary>
        /// <returns>Text representation of the client.</returns>
        /// <remarks></remarks>
		public override string ToString()
		{
			return string.Format("{0} - {1}", ClientNo, ClientName);
		}


        /// <summary>
        /// Validates the specified preempt.
        /// </summary>
        /// <param name="preempt">if set to <c>true</c> [preempt].</param>
        /// <param name="max">if set to <c>true</c> [max].</param>
        /// <remarks></remarks>
		public void Validate(bool preempt, bool max)
		{
			int count  = GetContacts().Count;

			if (preempt)
			{
				if (max)
				{
					if (count >= CurrentClientType.MaximumContactCount)
					{
						if (CurrentClientType.MaximumContactCount >= CurrentClientType.MinimumContactCount)
							throw new OMSException2("19004","The maximum number of contacts exceeded for the client.");
					}
				}
				else
				{
					if (count <= CurrentClientType.MinimumContactCount)
						throw new OMSException2("19003","The minimum number of contacts already exist.");
				}
			}
			else
			{
				if (max)
				{
					if (count > CurrentClientType.MaximumContactCount)
					{
						if (CurrentClientType.MaximumContactCount >= CurrentClientType.MinimumContactCount)
							throw new OMSException2("19004","The maximum number of contacts exceeded for the client.");
					}
				}
				else
				{
					if (count < CurrentClientType.MinimumContactCount)
						throw new OMSException2("19003","The minimum number of contacts already exist.");
				}
			}
		}

        private void UpdateSearchFieldsIfChanged()
        {
            if (_isClientNameDirty)
            {
                AskResult res = AskResult.Yes;
                AskEventArgs askargs = new AskEventArgs("UPDSRCHKEYS", "The %CLIENT% name has changed. Do you want to update the Search Fields?", "", res, "");
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == AskResult.Yes)
                {
                    this.GenerateSearchKeywords(true);
                }
                _isClientNameDirty = false;
            }
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
                    switch (_client.Tables[Table].Rows[0].RowState)
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
					return Convert.ToString(GetExtraInfo("clNickname"));
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
						SetExtraInfo("clNickname", DBNull.Value);
					else
						SetExtraInfo("clNickname", value);
				}
				catch
				{
				}
			}
		}

        /// <summary>
        /// Gets a value indicating whether the client object is new and needs to be
        /// updated to exist in the database.
        /// </summary>
        /// <remarks></remarks>
		public bool IsNew
		{
			get
			{
				try
				{
					return (_client.Tables[Table].Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
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
                    return Convert.ToDecimal(_client.Tables[Table_TimeStats].Rows[0]["TimeWIP"]);
                }
			}
		}


        /// <summary>
        /// Time Credit Limit over all Files
        /// </summary>
        /// <returns>Time Credit Limit</returns>
        /// <remarks></remarks>
		public decimal TimeCreditLimit
		{
			get
			{
                DownloadTimeStats();
                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                {
                    try
                    {
                        return Convert.ToDecimal(_client.Tables[Table_TimeStats].Rows[0]["CreditLimitTotal"]);
                    }
                    catch
                    {
                        return 0;
                    }
                }
                else
                {
                    return Convert.ToDecimal(_client.Tables[Table_TimeStats].Rows[0]["CreditLimit"]);
                }
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
                    return Convert.ToDecimal(_client.Tables[Table_TimeStats].Rows[0]["TimeBilled"]);
                }
			}
		}

        /// <summary>
        /// Gets the clients unique identifier.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public long ClientID
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt64(GetExtraInfo("CLID"),0);
			}
		}

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public long ID
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt64(GetExtraInfo("CLID"), 0);
            }
        }



        /// <summary>
        /// Gets or Sets the Client Number of the current client object.
        /// </summary>
        /// <value>The client no.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ClientNo
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CLNO"));
			}
			set
			{
				SetExtraInfo("clno", value);
			}
		}

        /// <summary>
        /// Gets the descriptive client name of the currently  initialized client.
        /// </summary>
        /// <value>The name of the client.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ClientName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CLName"));
			}
			set
			{
                
				if (string.IsNullOrEmpty(value))
					return;

                string oldval = ClientName;

                if (oldval != value)
                {
                    _isClientNameDirty = IsNew == false && !value.Equals(ClientName, StringComparison.CurrentCultureIgnoreCase);
                    SetExtraInfo("CLNAME", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("ClientName", oldval, value));
                }
			}
		}

        /// <summary>
        /// Gets the Default Contact Proof of ID
        /// </summary>
        /// <value>The default contact proofof I d1.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public object DefaultContactProofofID1
		{
			get
			{
				return DefaultContact.ProofofID1;
			}
			set
			{
				DefaultContact.ProofofID1 = value;
				DefaultContact.Update();
			}
		}

        /// <summary>
        /// Gets the Default Contact Proof of ID
        /// </summary>
        /// <value>The default contact proofof I d2.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public object DefaultContactProofofID2
		{
			get
			{
				return DefaultContact.ProofofID2;
			}
			set
			{
				DefaultContact.ProofofID2 = value;
				DefaultContact.Update();
			}
		}


        /// <summary>
        /// Gets the Client type.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ClientTypeCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cltypecode"));
			}
		}

        /// <summary>
        /// Gets the group code of the client.
        /// </summary>
        /// <remarks></remarks>
		public string Group
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clGroup"));
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
				return Convert.ToString(GetExtraInfo("clsource"));
			}
			set
			{
				if (value == null || value == "")
					SetExtraInfo("clsource", DBNull.Value);
				else
					SetExtraInfo("clsource", value);
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
        /// Gets the contact that is the source of business.
        /// </summary>
        /// <value>The source is contact.</value>
        /// <remarks></remarks>
		public Contact SourceIsContact
		{
			get
			{
				if (GetExtraInfo("clsourcecontact") == DBNull.Value)
					return null;
				else
					return Contact.GetContact((long)GetExtraInfo("clsourcecontact"));
			}
			set
			{
				if (value == null)
				{
					SetExtraInfo("clsourcecontact", DBNull.Value);
					SetExtraInfo("clsource", DBNull.Value);
				}
				else
				{
					SetExtraInfo("clsourcecontact", value.ID);
					SetExtraInfo("clsource", "CONTACT");
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
				if (GetExtraInfo("clsourceuser") == DBNull.Value)
					return null;
				else
					return User.GetUser((int)GetExtraInfo("clsourceuser"));
			}
			set
			{
				if (value == null)
				{
					SetExtraInfo("clsourceuser", DBNull.Value);
					SetExtraInfo("clsource", DBNull.Value);
				}
				else
				{
					SetExtraInfo("clsourceuser", value.ID);
					SetExtraInfo("clsource", "USER");
				}
			}
		}

        /// <summary>
        /// Gets a flag indicating whether the client was created through an automated process.
        /// </summary>
        /// <remarks></remarks>
		public bool HasBeenAutoCreated
		{
			get
			{
				return (AutoCreatedSource != String.Empty);
			}
		}

        /// <summary>
        /// Gets the source code of the program that created the client though an automated process.
        /// This might be a web service.
        /// </summary>
        /// <remarks></remarks>
		public string AutoCreatedSource
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clautosource"));
			}
		}

        /// <summary>
        /// Gets the data type code of the program that created the client though an automated process.
        /// This might be XML, CSV etc...
        /// </summary>
        /// <remarks></remarks>
		public string AutoCreatedDataType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clautotype"));
			}
		}

        /// <summary>
        /// Gets the date and time of when the client was first created automaitcally.  This may be before the
        /// item was inserted into the OM database.
        /// </summary>
        /// <remarks></remarks>
		public string AutoCreatedDate
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clautocreated"));
			}
		}

        /// <summary>
        /// Gets the prefered language that the client will use.
        /// </summary>
        /// <remarks></remarks>
		public string PreferedLanguage
		{
			get
			{
				if (GetExtraInfo("cluicultureinfo") is DBNull)
					return Session.CurrentSession.DefaultCulture;
				else
				{
					try
					{
						string culture = Convert.ToString(GetExtraInfo("cluicultureinfo"));
						System.Globalization.CultureInfo.CreateSpecificCulture(culture);
						return culture;
					}
					catch
					{
						return Session.CurrentSession.DefaultCulture;
					}
				}
			}
		}

        /// <summary>
        /// Gets or Sets the external account code of the client which may be used to link
        /// a client to another instance of the client in an external database / accounting
        /// system.
        /// </summary>
        /// <value>The account code.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string AccountCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clacccode"));
			}
			set
			{
				SetExtraInfo("clacccode", value);
			}
		}

        /// <summary>
        /// Gets or Sets a keyword search item.
        /// </summary>
        /// <value>The search keyword1.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string SearchKeyword1
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clsearch1"));
			}
			set
			{
                string oldval = SearchKeyword1;
                if (oldval != value)
                {
                    SetExtraInfo("clsearch1", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("SearchKeyword1", oldval, value));
                }
			}
		}

        /// <summary>
        /// Gets or Sets a keyword search item.
        /// </summary>
        /// <value>The search keyword2.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string SearchKeyword2
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clsearch2"));
			}
			set
			{
                string oldval = SearchKeyword2;
                if (oldval != value)
                {
                    SetExtraInfo("clsearch2", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("SearchKeyword2", oldval, value));
                }
			}
		}

        /// <summary>
        /// Gets or Sets a keyword search item.
        /// </summary>
        /// <value>The search keyword3.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string SearchKeyword3
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clsearch3"));
			}
			set
			{
                string oldval = SearchKeyword3;
                if (oldval != value)
                {
                    SetExtraInfo("clsearch3", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("SearchKeyword3", oldval, value));
                }
			}
		}

        /// <summary>
        /// Gets or Sets a keyword search item.
        /// </summary>
        /// <value>The search keyword4.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string SearchKeyword4
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clsearch4"));
			}
			set
			{
                string oldval = SearchKeyword4;
                if (oldval != value)
                {
                    SetExtraInfo("clsearch4", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("SearchKeyword4", oldval, value));
                }
			}
		}

        /// <summary>
        /// Gets or Sets a keyword search item.
        /// </summary>
        /// <value>The search keyword5.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string SearchKeyword5
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clsearch5"));
			}
			set
			{
                string oldval = SearchKeyword5;
                if (oldval != value)
                {
                    SetExtraInfo("clsearch5", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("SearchKeyword5", oldval, value));
                }
			}
		}


        /// <summary>
        /// Gets a branch object that the client was originated from.
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
        /// Gets the fee earner that is the clients contact within the firm.
        /// </summary>
        /// <remarks></remarks>
		public FeeEarner FirmContact
		{
			get
			{
				if (GetExtraInfo("feeusrid") == DBNull.Value)
					return null;
				else
					return FeeEarner.GetFeeEarner((int)GetExtraInfo("feeusrid"));
			}
		}


        /// <summary>
        /// Gets or Sets the flag that enables marketing mail shots for a client.
        /// </summary>
        /// <value><c>true</c> if marketing; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool Marketing
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("clmarket"));
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("clmarket", value);
			}
		}

        /// <summary>
        /// Gets or Sets the extra notes used to explain additional information on the client.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Notes
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clnotes")); 
			}
			set
			{
				SetExtraInfo("clnotes", value);
			}
		}

        /// <summary>
        /// Gets or Sets any additional information on the client.
        /// </summary>
        /// <value>The additional information.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string AdditionalInformation
		{
			get
			{
				return Convert.ToString(GetExtraInfo("claddinfo"));
			}
			set
			{
				SetExtraInfo("claddinfo", value);
			}
		}

        /// <summary>
        /// Gets a flag that specifies whether the client is a pre-client.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool IsPreClient
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("clpreclient")); 
				}
				catch
				{
					return false;
				}
			}	
		}

        /// <summary>
        /// Gets or Sets the file description of a pre-client pre-instruction file.
        /// </summary>
        /// <value>The pre client file desc.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
		public string PreClientFileDesc
		{
			get
			{
				return _preclientfiledesc;
			}
			set
			{
				_preclientfiledesc = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> preclientfileSetExtraInfo = new Dictionary<string, object>();

        /// <summary>
        /// Gets or Sets the file description of a pre-client pre-instruction file.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
        public Dictionary<string,object> PreClientFileSetExtraInfo
		{
			get
			{
                return preclientfileSetExtraInfo;
			}
		}



        /// <summary>
        /// Gets or Sets the file type of a pre-client pre-instruction file.
        /// </summary>
        /// <value>The type of the pre client file.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
		public string PreClientFileType
		{
			get
			{
				return _preclientfiletype;
			}
			set
			{
				_preclientfiletype = value;
			}
		}

        /// <summary>
        /// Gets or Sets the department of a pre-client pre-instruction file.
        /// </summary>
        /// <value>The pre client file department.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		[System.ComponentModel.Browsable(false)]
		public string PreClientFileDepartment
		{
			get
			{
				return _preclientdept;
			}
			set
			{
				_preclientdept = value;
			}
		}

        /// <summary>
        /// Gets a date value that displays the date and time of when a pre-client was converted
        /// to a full client.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ConvertedDate
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clpreclientconvdate"));
			}
		}


        /// <summary>
        /// Gets a value indicating whether the client is in a state to be exported to another system.
        /// This may be set to true if the client is created or has been updated.
        /// </summary>
        /// <remarks></remarks>
		public bool NeedExport
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("clneedexport"));
				}
				catch
				{
					return false;
				}
			}
		}

        /// <summary>
        /// Gets the quick multi-line description display of client details.
        /// This is built up by the client type configuration file.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ClientDescription
		{
			get
			{
				try
				{
					try
					{
						string text = CurrentClientType.ClientDetailsConfig;
                        
                        if (text == String.Empty) 
                        {
                            text = GetTextFromResFile("DefaultClientDetails");
                        }
                    
						return CurrentClientType.ParseDynamicProperty(this, text);
					}
					catch
					{
                        string text = CurrentClientType.ReadAttribute(CurrentClientType.GetConfigRoot(), "ClientDescription", FWBS.OMS.Global.GetResString("DefaultClientDetails", true));
						return OriginalClientParseString(text);
					}
				}
				catch
				{
					return "N/A";
				}

			}
		}

        /// <summary>
        /// Gets the quick multi-line description display of client details.
        /// This is built up by the client type configuration file.
        /// </summary>
        /// <remarks></remarks>
        [EnquiryUsage(true)]
        public string ClientDescriptionInfoPanel
        {
            get
            {
                try
                {
                    try
                    {
                        string text = CurrentClientType.ClientDetailsInfoPanelConfig;
                        
                        if (text == String.Empty)
                        {
                            text = GetTextFromResFile("DefaultClientDetailsInfoPanel");
                        }

                        return CurrentClientType.ParseDynamicProperty(this, text);
                    }
                    catch (Exception)
                    {
                        string text = CurrentClientType.ReadAttribute(CurrentClientType.GetConfigRoot(), "DefaultClientDetailsInfoPanel", FWBS.OMS.Global.GetResString("DefaultClientDetailsInfoPanel", true));
                        return OriginalClientParseString(text);
                    }
                }
                catch
                {
                    return "N/A";
                }

            }
        }

        /// <summary>
        /// Gets or Sets the Default Address object relating to the client, Depends on having a Default Address Specified.
        /// </summary>
        /// <value>The default address.</value>
        /// <returns>Address Object</returns>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal Address defaultAddress
		{
			get
			{
				if (_defAddress == null)
				{
					object def = GetExtraInfo("clDefaultAddress");
					if (def != DBNull.Value && def != null)
					{
						_defAddress = Address.GetAddress((long)def);
					}
					else
					{
						_defAddress = Address.Null;
					}
				}
				return _defAddress;
			}
			set
			{
				DefaultAddress = value;
			}

		}

        /// <summary>
        /// Gets or Sets the Default Address object relating to the client, Depends on having a Default Address Specified.
        /// </summary>
        /// <value>The default address.</value>
        /// <returns>Address Object</returns>
        /// <remarks></remarks>
		public Address DefaultAddress
		{
			get
			{
				if (_defAddress == null || _defAddress == Address.Null)
				{
					object def = GetExtraInfo("clDefaultAddress");
					if (def != DBNull.Value && def != null)
					{
						_defAddress = Address.GetAddress((long)def);
					}
					else
					{
						return DefaultContact.DefaultAddress;
					}
				}
				return _defAddress;
			}
			set
			{
                Address oldval = DefaultAddress;

				_defAddress = value;
				if (_defAddress == null || _defAddress == Address.Null)
				{
					SetExtraInfo("cldefaultaddress", DBNull.Value);
					OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("defaultAddress",oldval, _defAddress));
				}
				else
				{
					SetExtraInfo("cldefaultaddress", _defAddress.ID);
					OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("defaultAddress",oldval, _defAddress));
				}
			}
		}

        /// <summary>
        /// Gets the localized client type description.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string ClientTypeDescription
		{
			get
			{
				return CurrentClientType.Description;
			}
		}


        /// <summary>
        /// Gets the current alert message.  This corresponds to the client on-stop reason.
        /// </summary>
        /// <value>The on stop message.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string OnStopMessage
		{
			get
			{
				return Convert.ToString(GetExtraInfo("CLSTOPREASON"));
			}
			set
			{

				SetExtraInfo("CLSTOPREASON", value);
			}
		}

        /// <summary>
        /// Gets a bolloean value that indicates whether the client is currently on-stop.
        /// </summary>
        /// <value><c>true</c> if [on stop]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool OnStop
		{
			get
			{
				if (OnStopMessage == String.Empty)
					return false;
				else
					return Common.ConvertDef.ToBoolean(GetExtraInfo("CLSTOP"), false) ;
			}
			set
			{
				SetExtraInfo("CLSTOP", value);
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
        /// Gets the client type object associated to the client.
        /// </summary>
        /// <remarks></remarks>
		public ClientType CurrentClientType
		{
			get
			{
				return (ClientType)GetOMSType();
			}
		}

        /// <summary>
        /// Gets or Sets the default document storage location for the client.
        /// </summary>
        /// <value>The default storage provider.</value>
        /// <remarks></remarks>
		public short DefaultStorageProvider
		{
			get
			{
				return Common.ConvertDef.ToInt16(GetExtraInfo("clStorageProvider"), -1);
			}
			set
			{
				if (value < 0)
					SetExtraInfo("clStorageProvider", DBNull.Value);
				else
					SetExtraInfo("clStorageProvider", value);
			}
		}


        /// <summary>
        /// 
        /// </summary>
        private System.Collections.Generic.List<ConflictSearches> _conflictSearches = new System.Collections.Generic.List<ConflictSearches>();

        /// <summary>
        /// Applies a search list to the current file.
        /// </summary>
        /// <param name="count">The number of conflicts found.</param>
        /// <param name="detail">The criteria used.</param>
        /// <remarks></remarks>
        public void PreClientApplyConflictSearch(int count, string detail)
        {
            _conflictSearches.Add(new ConflictSearches(count, detail));
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
					DataTable dt = _client.Tables[Table_TimeRecords];
					_timerecords = new TimeCollection(ref dt);
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
			if (_client.Tables.Contains(Table_TimeRecords) == false) 
				_client.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeRecords",Table_TimeRecords, new IDataParameter[2]{Session.CurrentSession.Connection.AddParameter("CLID", System.Data.SqlDbType.BigInt, 0, this.ClientID),Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name)}));
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
				
				_client.Tables[Table_TimeStats].DefaultView.RowFilter = filter;
				if (_client.Tables[Table_TimeStats].DefaultView.Count == 0)
				{
					// Default Row has no Rows so return 0
					return 0;
				}
				else
				{
					decimal tmp = 0;
					foreach (DataRowView rw in _client.Tables[Table_TimeStats].DefaultView)
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

            if (_client.Tables.Contains(Table_TimeStats) == false)
            {
                if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == false)
                    _client.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeStats", Table_TimeStats, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("CLID", System.Data.SqlDbType.BigInt, 0, this.ClientID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));
                else
                {
                    try
                    {
                        SetExternalBalanceOnline(this);
                        DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(ExtClientBalDatalist);
                        FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                        param.Add("CLNO", this.ClientNo);
                        param.Add("CLID", this.ClientID);
                        dl.ChangeParameters(param);
                        DataTable dt = dl.Run() as DataTable;
                        dt.TableName = Table_TimeStats;
                        if (dt.Rows.Count > 0)
                            _client.Tables.Add(dt);
                        else
                            throw new Exception();
                    }
                    catch (FWBS.OMS.Data.Exceptions.ConnectionException ex)
                    {
                        Trace.TraceError(ex.Message);
                        SetExternalBalanceOffline(_client, this);
                    }
                }
            }
        }


        /// <summary>
        /// Sets the external balance online.
        /// </summary>
        /// <param name="alerts">The alerts.</param>
        /// <remarks></remarks>
        internal static void SetExternalBalanceOnline(IAlert alerts)
        {
            alerts.ClearAlerts();
        }

        /// <summary>
        /// Sets the external balance offline.
        /// </summary>
        /// <param name="_file">The _file.</param>
        /// <param name="alerts">The alerts.</param>
        /// <remarks></remarks>
        internal static void SetExternalBalanceOffline(DataSet _file, IAlert alerts)
        {
            DataTable dt = new DataTable(Table_TimeStats);
            dt.Columns.Add("TimeBilled", typeof(decimal));
            dt.Columns.Add("LastEstimate", typeof(decimal));
            dt.Columns.Add("CreditLimit", typeof(decimal));
            dt.Columns.Add("TimeWIP", typeof(decimal));
            DataRow dr = dt.NewRow();
            dr["TimeBilled"] = 0;
            dr["LastEstimate"] = 0;
            dr["CreditLimit"] = 0;
            dr["TimeWIP"] = 0;
            dt.Rows.Add(dr);
            if (_file.Tables.Contains(Table_TimeStats) == false)
                _file.Tables.Add(dt);
            alerts.AddAlert(new Alert(Session.CurrentSession.Resources.GetResource("ERREXTBALOFFLNE", "Unable to retrieve external balance. All balances will be represented as 0.00", "").Text, Alert.AlertStatus.Amber));
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

		#region Static Methods


        /// <summary>
        /// Returns a client object based on the unique client number.
        /// </summary>
        /// <param name="clNo">Unique Client Identifier.</param>
        /// <returns>A client object.</returns>
        /// <remarks></remarks>
		public static Client GetClient(string clNo)
		{
			Session.CurrentSession.CheckLoggedIn();

			Client cc = null;
			foreach (Client cl in Session.CurrentSession.CurrentClients.Values)
			{
				if (cl.ClientNo == clNo)
				{
					cc = (Client)Session.CurrentSession.CurrentClients[cl.ClientID.ToString()];
					break;
				}
			}

			if (cc == null)
			{
				cc = new Client(clNo);
			}		
	
			return cc;
	
		}


        /// <summary>
        /// Initialised an existing client object with the specified string identifier.
        /// </summary>
        /// <param name="clalternaterefNo">Alternative Client Number Code.</param>
        /// <param name="fieldtomatch">Field Name to search for Match Against</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public static Client GetClient(string clalternaterefNo,string fieldtomatch)
		{
			if (clalternaterefNo == "")
                throw new OMSException2(HelpIndexes.ClientNotFound.ToString(), "", null, true, clalternaterefNo);

            DataSet cltmp;
			cltmp = Session.CurrentSession.Connection.ExecuteSQLDataSet(Sql + " where " + fieldtomatch + " = '" + clalternaterefNo + "'", new string [1] {Table}, new IDataParameter[0]);
			if ((cltmp.Tables[Table] == null) || (cltmp.Tables[Table].Rows.Count == 0)) 
			{
                throw new OMSException2("CLIENTNOTFNDF", "", null, true, clalternaterefNo, fieldtomatch);
            }
			else
			{
				// Found a record using the match record system so pass the clno of this record set through to the normal
				// GetClient Method
				return GetClient(Convert.ToInt64(cltmp.Tables[Table].Rows[0]["clID"]));
			}
		}




        /// <summary>
        /// Returns a client object bsaed on the specified client number.
        /// </summary>
        /// <param name="clId">Client Number Parameter.</param>
        /// <param name="DefaultTab">The default tab.</param>
        /// <returns>A client object.</returns>
        /// <remarks></remarks>
		public static Client GetClient(long clId, string DefaultTab)
		{
			Session.CurrentSession.CheckLoggedIn();
			Client cc = Session.CurrentSession.CurrentClients[clId.ToString()] as Client;
			if (cc == null)
			{
				cc = new Client(clId);
				cc.DefaultTab = DefaultTab;
			}		
			return cc;
		}

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="clId">The cl id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public static Client GetClient(long clId)
		{
			return GetClient(clId,null);
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
			switch (fieldName.ToUpper())
			{
				case "":
				{
					break;
				}
				case "CLNO":
					break;
				case "CLNAME":
					break;
				case "CLTYPECODE":
					break;
				case "CLPRECLIENT":
				{
					if (IsPreClient && ((bool)val) == false)
					{
						this.SetExtraInfo(_client.Tables[Table].Rows[0], "clpreclientconvdate", DateTime.Now);
					}
				}
					break;
				case "FEEUSRID":
					// Set the FeeUsrID (Firm Contact if null)
					if (GetExtraInfo("feeusrid").ToString() != val.ToString())
					{
						if (GetExtraInfo("feeusrid") != DBNull.Value)		
						{				
							int oldid = (int)GetExtraInfo("feeusrID");
							// Log to Captains Log this event
							FWBS.OMS.Logging.CaptainsLog.CreateClientEntry(ClientLogType.ClientFirmContact,"Changed From : ",oldid.ToString());
						}
						else
						{
							FWBS.OMS.Logging.CaptainsLog.CreateClientEntry(ClientLogType.ClientFirmContact,"Set","");
						}
					}
					break;

			}
            this.SetExtraInfo(_client.Tables[Table].Rows[0], fieldName, val);
        }

        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        /// <remarks></remarks>
		public object GetExtraInfo(string fieldName)
		{
            object val = this.GetExtraInfo(fieldName,Table,0);
            if (fieldName.ToLower() == "clid" && val == DBNull.Value) val = 0;
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
				return _client.Tables[Table].Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("19001","Error Getting Extra Info Field %1% Probably Not Initialized", new Exception(),true,fieldName);
			}
		}

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
		public DataSet GetDataset()
		{
			return _client;
		}

        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <returns>DataTable object.</returns>
        /// <remarks></remarks>
		public DataTable GetDataTable()
		{
			return _client.Tables[Table].Copy();
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

            UpdateSearchFieldsIfChanged();

            //New addin object event arguments
            ObjectState state = State;

            if (this.OnExtCreatingUpdatingOrDeleting(state))
                return;

            System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
			e.Cancel = false;
			OnUpdating(e);
			if (e.Cancel)
				return;


			bool exceptionhappened = false;
			try
			{
				Session.CurrentSession.Connection.Connect(true);

				DataRow row = _client.Tables[Table].Rows[0];

				bool isnew = IsNew;

				//Check if there are any changes made before setting the updated
				//and updated by properties then update.
				if (_client.Tables[Table].GetChanges()!= null)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					if (_client.Tables[Table].PrimaryKey == null || _client.Tables[Table].PrimaryKey.Length == 0)
						_client.Tables[Table].PrimaryKey = new DataColumn[1]{_client.Tables[Table].Columns["clid"]};
    
					SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
					SetExtraInfo("Updated", DateTime.Now);
					try
					{
						Session.CurrentSession.Connection.Update(row, "dbclient");
					}
					catch (Exception ex)
					{
                        throw ex;
					}
				}



                if (isnew)
                {
                    Session.CurrentSession.CurrentClients.Add(ClientID.ToString(), this);

                }
				

				//Make sure that the multi contact table has the correct client id.
				DataView cnt = GetContacts(true);
				cnt.RowStateFilter = DataViewRowState.OriginalRows;
				cnt.RowStateFilter = DataViewRowState.CurrentRows;
				foreach (DataRowView contact in cnt)
				{
					contact["clid"] = this.ClientID;
				}

				if (_client.Tables[Table_Contacts].GetChanges() != null)
				{
					try
					{
						this.Update(Table_Contacts,Sql_ContactLink);
					}
					catch (Exception ex)
					{
						throw new Exception("Error Creating Contacts for Client." + Environment.NewLine + Environment.NewLine + FWBS.Common.OMSDebug.DataTableToString(_client.Tables[Table_Contacts]),ex);
					}
				}
				


				//Add the pre-instruction file by default if the client is a pre-client being added.
				if (isnew)
				{
					if (IsPreClient)
					{
						FileType ft = new FileType(_preclientfiletype);
						OMSFile preinstruction = new OMSFile(ft, this);
						preinstruction.Department = _preclientdept;
						preinstruction.FileDescription= _preclientfiledesc;
                        foreach (var item in preclientfileSetExtraInfo)
                        {
                            preinstruction.SetExtraInfo(item.Key, item.Value);
                        }
                        foreach (ConflictSearches cs in _conflictSearches)
                            preinstruction.ApplyConflictSearch(cs.Count, cs.Info);
						preinstruction.Update();
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

				// call the update of the default contact to update these addresses
				DefaultContact.Update();

				//Update any of the cached objects that may have something to do with this contact.
				foreach (OMSFile file in Session.CurrentSession.CurrentFiles.Values)
				{
					if (file.ClientID == this.ClientID)
					{
						file.Refresh(true);
					}
				}


                if (isnew)
                {
                    // Apply any Client Type Template Security
                    TemplateSecurity tmp = new TemplateSecurity("ClientType", this.ClientTypeCode);
                    if (tmp.HasSecurity) tmp.ApplySecurity(this.ClientID);
                }

                this.OnExtCreatedUpdatedDeleted(state);
            }
			catch (Exception ex)
			{
				exceptionhappened = true;
				throw ex;
			}
			finally
			{
				Session.CurrentSession.Connection.Disconnect(true);
				if (exceptionhappened == false) 
					this.Fetch(this.ClientID, null); 
			}

			OnUpdated();
		}

        /// <summary>
        /// Refreshes the File List within the Client Object
        /// </summary>
        /// <param name="tables">The tables.</param>
        /// <remarks></remarks>
        public void Refresh(ClientTables tables)
        {
            DataSet ds = GetClientData(new string[6] { Table_Files, Table_DefContact, Table_DefContactAddresses, Table_DefContactNumbers, Table_DefContactEmails, Table_Contacts });

            if ((tables & ClientTables.Files) == ClientTables.Files)
            {
                if (_client.Tables.Contains(Table_Files))
                    _client.Tables.Remove(Table_Files);

                if (!IsNewDataFormat)
                    _client.Tables.Add(ds.Tables[Table_Files].Copy());
            }
            if ((tables & ClientTables.ContactAddresses) == ClientTables.ContactAddresses)
            {
                if (_client.Tables.Contains(Table_DefContactAddresses))
                    _client.Tables.Remove(Table_DefContactAddresses);
                _client.Tables.Add(ds.Tables[Table_DefContactAddresses].Copy());
            }
            if ((tables & ClientTables.ContactEmails) == ClientTables.ContactEmails)
            {
                if (_client.Tables.Contains(Table_DefContactEmails))
                    _client.Tables.Remove(Table_DefContactEmails);
                _client.Tables.Add(ds.Tables[Table_DefContactEmails].Copy());
            }
            if ((tables & ClientTables.ContactNumbers) == ClientTables.ContactNumbers)
            {
                if (_client.Tables.Contains(Table_DefContactNumbers))
                    _client.Tables.Remove(Table_DefContactNumbers);
                _client.Tables.Add(ds.Tables[Table_DefContactNumbers].Copy());
            }
            if ((tables & ClientTables.Contacts) == ClientTables.Contacts)
            {
                if (_client.Tables.Contains(Table_Contacts))
                    _client.Tables.Remove(Table_Contacts);
                _client.Tables.Add(ds.Tables[Table_Contacts].Copy());
            }
            if ((tables & ClientTables.DefaultContacts) == ClientTables.DefaultContacts)
            {
                if (_client.Tables.Contains(Table_DefContact))
                    _client.Tables.Remove(Table_DefContact);
                _client.Tables.Add(ds.Tables[Table_DefContact].Copy());
            }
            if ((tables & ClientTables.TimeRecords) == ClientTables.TimeRecords)
            {
                if (_client.Tables.Contains(Table_TimeRecords))
                    _client.Tables.Remove(Table_TimeRecords);
                _client.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeRecords", Table_TimeRecords, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("CLID", System.Data.SqlDbType.BigInt, 0, this.ClientID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));
            }
            if ((tables & ClientTables.TimeStats) == ClientTables.TimeStats)
            {
                if (_client.Tables.Contains(Table_TimeStats))
                    _client.Tables.Remove(Table_TimeStats);
                _client.Tables.Add(Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeStats", Table_TimeStats, new IDataParameter[2] { Session.CurrentSession.Connection.AddParameter("CLID", System.Data.SqlDbType.BigInt, 0, this.ClientID), Session.CurrentSession.Connection.AddParameter("@UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) }));
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

            if (this.OnExtRefreshing())
                return;

            System.ComponentModel.CancelEventArgs e = new System.ComponentModel.CancelEventArgs();
            e.Cancel = false;
            OnRefreshing(e);
            if (e.Cancel)
                return;
            
            DataTable changes = _client.Tables[Table].GetChanges();

			try
			{
				_checkPassword = false;

                if (changes != null && applyChanges && changes.Rows.Count > 0)
                    Fetch(this.ClientID, changes.Rows[0]);
                else
                    Fetch(this.ClientID, null);
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

            this.OnExtRefreshed();

            OnRefreshed();
		}



        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        /// <remarks></remarks>
		public void Cancel()
		{
			_defAddress = null;
			_client.RejectChanges();

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
				return (_client.GetChanges() != null);
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
        /// <remarks></remarks>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

        /// <summary>
        /// Edits the current client object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
        /// </summary>
        /// <param name="customForm">Enquiry form code.</param>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>
        /// <remarks></remarks>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return  Enquiry.GetEnquiry(customForm, Parent, this, param);
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
				return DefaultContact;
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

				if (_client != null)
				{
					_client.Dispose();
					_client = null;
				}

				_session = null;
			}
			
			//Dispose unmanaged objects.
		}

		#endregion

		#region IOMSType Implementation

        /// <summary>
        /// Gets an OMS Type based on the client type off this current instance of a client object.
        /// </summary>
        /// <returns>A OMSType with information needed to represented this type of client.</returns>
        /// <remarks></remarks>
		public OMSType GetOMSType()
		{
			return FWBS.OMS.ClientType.GetClientType(Convert.ToString(GetExtraInfo("cltypecode")));
		}

        /// <summary>
        /// Gets the value to link to many potential connector type object.
        /// </summary>
        /// <remarks></remarks>
		public object LinkValue
		{
			get
			{
				return this.ClientID;
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
					//Use the client type configuration to initialise the extended data objects.
					ClientType ct = CurrentClientType;
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
				return Convert.ToString(GetExtraInfo("clpassword"));
			}
			set
			{
				SetExtraInfo("clpassword", value);
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
				return Convert.ToString(GetExtraInfo("clpasswordhint"));
			}
			set
			{
				SetExtraInfo("clpasswordhint", value);
			}
		}


        /// <summary>
        /// Returns the string represenation of the password request screen.
        /// </summary>
        /// <returns>A string.</returns>
        /// <remarks></remarks>
		public override string ToPasswordString()
		{
			return this.ClientNo + "/" + ToString();
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
				System.Collections.ArrayList arr = new System.Collections.ArrayList();
				if (OnStop)
				{
					arr.Add(new Alert(OnStopMessage, FWBS.OMS.Alert.AlertStatus.Red));
				}
				if (IsPreClient)
				{
					arr.Add(new Alert(Session.CurrentSession.Resources.GetMessage("PRECLIENTALERT", "Pre-%CLIENT%", "", true).Text, FWBS.OMS.Alert.AlertStatus.Green));
				}
				if (_tempAlert.Message != "" && _tempAlert.Status != Alert.AlertStatus.Off)
				{
					arr.Add(_tempAlert);
				}

				Alert[] alerts = new Alert[arr.Count];
				alerts = (Alert[])arr.ToArray(typeof(Alert));		

				return alerts;
			}
		}

        /// <summary>
        /// Files the alerts.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public Alert[] FileAlerts(DataRowView row)
		{

			//This exact code is duplicated within File.. This is that the alert can be shown
			//without having to construct the file so that the select Client file scren does
			//not ask for a password etc...
			string status = Convert.ToString(row["filestatus"]);
			Alert[] alerts = Alerts;

			System.Collections.ArrayList arr = new System.Collections.ArrayList();
            try
            {
                if (Convert.ToString(row["fileAlertMessage"]) != "")
                {
                    arr.Add(new Alert(Convert.ToString(row["fileAlertMessage"]), (FWBS.OMS.Alert.AlertStatus)FWBS.Common.ConvertDef.ToEnum(row["fileAlertLevel"], FWBS.OMS.Alert.AlertStatus.Amber)));
                }
            }
            catch (System.ArgumentException)
            {
            }
            
            string desc = FWBS.OMS.Session.CurrentSession.Resources.GetResource("FILESTATAL", "%FILE% Status : %1%", "", true, CodeLookup.GetLookup("FILESTATUS", status)).Text;
            FileStatusManager statusManager = new FileStatusManager(Convert.ToInt64(row["fileID"]));
            if (statusManager.FileStatusRecord().Rows[0]["fsAlertLevel"] != DBNull.Value)
            {
                FWBS.OMS.Alert.AlertStatus alertLevel = statusManager.AlertLevel;
                if (alertLevel != Alert.AlertStatus.Off)
                    arr.Add(new Alert(desc, alertLevel));
            }
            else
            {
                if (status.StartsWith("LIVE") == false)
                {
                    if (status == "DEAD")
                        arr.Add(new Alert(desc, FWBS.OMS.Alert.AlertStatus.Red));
                    else
                    {
                        arr.Add(new Alert(desc, FWBS.OMS.Alert.AlertStatus.Amber));
                    }
                }
            }

			Alert[] filealerts = new Alert[alerts.Length + arr.Count];
			if (alerts.Length > 0) alerts.CopyTo(filealerts, 0);
			if (arr.Count > 0) Array.Copy((Alert[])arr.ToArray(typeof(Alert)), 0, filealerts, (alerts.GetUpperBound(0) < 0 ?0 : alerts.Length), arr.Count);
				
			return filealerts;

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

		#region Detail XML Parser

        /// <summary>
        /// Parses an XML file based string logic using regular expressions to display
        /// information in a particular format.
        /// </summary>
        /// <param name="toparse">XML string.</param>
        /// <returns>A displayable string displaying client information.</returns>
        /// <remarks></remarks>
		private string OriginalClientParseString(string toparse)
		{
			return this.OriginalClientParseString(toparse,null,"","");
		}

        /// <summary>
        /// Parses an XML file based string logic using regular expressions to display
        /// information in a particular format.
        /// </summary>
        /// <param name="toparse">XML string.</param>
        /// <param name="rowin">The rowin.</param>
        /// <param name="tableloop">The tableloop.</param>
        /// <param name="newlineoveride">The new line character / string combination to use as an override.</param>
        /// <returns>A displayable string displaying client information.</returns>
        /// <remarks></remarks>
		private string OriginalClientParseString(string toparse,DataRowView rowin,string tableloop,string newlineoveride)
		{
			// Patterns used with Regex
			string ptnif = @"<(?i:if)[: ]\""(?<ifname>\w+)\""[: ](?<table>\w+)(?:|:)(?<field>|\w+) (?<expression>[=!]|[!][=]) (?<evaluate>\w+)>(?<toparseloop>.*)(?i:</endif \""\k<ifname>\"">)";
			string ptnloop = @"<(?i:loop)[: ](?<table>\w+)>(?<toparseloop>.*)</(?i:loop:\k<table>)>";
			string ptninner = @"<(?<table>\w+)(?:|:)(?<field>|\w+)>";
			string ptninneroptions = @"<(?<table>\w+)(?:|:)(?<field>|\w+)@(?<option>[^>]*)>";
			string newline;

			if (newlineoveride != "")
			{
				newline  = newlineoveride;
			}
			else
			{
				// Check if RTF Format if so change newline
				if (toparse.StartsWith(@"{\rtf") == true) 
				{
				
					newline = @"\par";
					newlineoveride = newline;
				}
				else
				{
					newline = Environment.NewLine;
				}
			}

			



			// Process Loop based for If Expressions
			try
			{
				// Search for IF Containers
				MatchCollection mcClParse = Regex.Matches(toparse, ptnif, RegexOptions.Multiline | RegexOptions.Singleline );
				foreach (Match m in mcClParse)
				{
					try
					{
						System.Text.StringBuilder strb = new System.Text.StringBuilder();
						string evalstr;

						// Match so evaluate expression 
						string tmpparse = m.Groups["toparseloop"].Value;
						
						// Look @ Evaluate Group match and then change null variants to null string
						switch ((string)m.Groups["evaluate"].Value.ToUpper())
						{
							case "NULL":
							case "DBNULL":
								evalstr = "";
								break;

							default:
								evalstr = m.Groups["evaluate"].Value;
								break;
						}
						
						switch (m.Groups["expression"].Value)
						{
							case "=":
								if (m.Groups["table"].Value == tableloop)
								{
									// Use rowin as the tableloop is consistant
									if (rowin[m.Groups["field"].Value].ToString() == evalstr)
									{
										tmpparse = OriginalClientParseString(tmpparse,rowin, m.Groups["table"].Value,newlineoveride);
									}
									else
									{
										tmpparse = "";
									}
								}
								else
								{
									if ( _client.Tables[m.Groups["table"].Value].DefaultView[0][m.Groups["field"].Value].ToString() == evalstr)
									{
										tmpparse = OriginalClientParseString(tmpparse,null , m.Groups["table"].Value,newlineoveride);
									}
									else
									{
										tmpparse = "";
									}
								}
								strb.Append(tmpparse);
								break;
							case "!":
							case "!=":
								if (m.Groups["table"].Value == tableloop)
								{
									// Use rowin as the tableloop is consistant
									if (rowin[m.Groups["field"].Value].ToString() != evalstr)
									{
										tmpparse = OriginalClientParseString(tmpparse,rowin , m.Groups["table"].Value,newlineoveride);
									}
									else
									{
										tmpparse = "";
									}
								}
								else
								{
									if ( _client.Tables[m.Groups["table"].Value].DefaultView[0][m.Groups["field"].Value].ToString() != evalstr)
									{
										tmpparse = OriginalClientParseString(tmpparse,null , m.Groups["table"].Value,newlineoveride);
									}
									else
									{
										tmpparse = "";
									}
								}
								strb.Append(tmpparse);

								break;
						}
						
						toparse = toparse.Replace(m.Groups[0].Value,strb.ToString());
					}
					catch {}
				}
			}
			catch 
			{				
			}


			// Process Loop based Tables First
			try
			{
				// Search for Loop Containers
				MatchCollection mcClParse = Regex.Matches(toparse, ptnloop,RegexOptions.Multiline | RegexOptions.Singleline);
				foreach (Match m in mcClParse)
				{
					try
					{
						// Loop through Each InnerLoop and execute 
						// Table Loop Code in Place, 
						System.Text.StringBuilder strb = new System.Text.StringBuilder();
						foreach (DataRowView rw in _client.Tables[m.Groups["table"].Value].DefaultView)
						{
							string tmpparse = m.Groups["toparseloop"].Value;
							try
							{
								if (!Convert.ToBoolean(rw["contActive"]))
								{
									// Contactive Field Exists and is set to false then skip
									continue;
								}
							}
							catch
							{}
							tmpparse = OriginalClientParseString(tmpparse,rw , m.Groups["table"].Value,newlineoveride);
							strb.Append(tmpparse);
						}
						toparse = toparse.Replace(m.Groups[0].Value,strb.ToString());
					}
					catch {}
				}
			}
			catch 
			{				
			}

			// Normal Loop Replacing Fields which are available
			try
			{
				MatchCollection mcClParse = Regex.Matches(toparse, ptninner, RegexOptions.Multiline | RegexOptions.Singleline);
				foreach (Match m in mcClParse)
				{
					try
					{

						if (m.Groups["table"].Value == tableloop)
						{
							// Use rowin as the tableloop is consistant
							toparse = toparse.Replace(m.Groups[0].Value,(string)"" + rowin[m.Groups["field"].Value].ToString());
						}
						else
						{
							toparse = toparse.Replace(m.Groups[0].Value,(string)"" + _client.Tables[m.Groups["table"].Value].DefaultView[0][m.Groups["field"].Value].ToString());
						}
					}
					catch 
					{
						if (m.Groups[0].Value.ToUpper() != "<BS>") 
							toparse = toparse.Replace(m.Groups[0].Value,m.Groups[0].Value + "Not Found!");
					}
				}
			}
			catch 
			{				
			}


			// Normal Loop Replacing Fields which are available
			try
			{
				MatchCollection mcClParse = Regex.Matches(toparse, ptninneroptions, RegexOptions.Multiline | RegexOptions.Singleline);
				foreach (Match m in mcClParse)
				{
					try
					{
						if (m.Groups["table"].Value == tableloop)
						{
							// Use rowin as the tableloop is consistant
							toparse = toparse.Replace(m.Groups[0].Value,string.Format(m.Groups["option"].Value ,rowin[m.Groups["field"].Value]));
						}
						else
						{
							toparse = toparse.Replace(m.Groups[0].Value,string.Format( m.Groups["option"].Value , _client.Tables[m.Groups["table"].Value].DefaultView[0][m.Groups["field"].Value]));
						}
					}
					catch 
					{
						if (m.Groups[0].Value.ToUpper() != "<BS>") 
							toparse = toparse.Replace(m.Groups[0].Value,m.Groups[0].Value + "Not Found!");
					}
				}
			}
			catch 
			{				
			}
			// Remove <BS> switches
			toparse = toparse.Replace(newlineoveride + Environment.NewLine + "<BS>","");
			toparse = toparse.Replace(newline + "<BS>","");
			toparse = toparse.Replace("<BS>","");
			return toparse;
		}


		#endregion

        #region ISecurable Members

        /// <summary>
        /// Gets the security id.
        /// </summary>
        /// <remarks></remarks>
        string Security.ISecurable.SecurityId
        {
            get { return ClientID.ToString(); }
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
        /// Checks the permissions.
        /// </summary>
        /// <remarks></remarks>
        private void CheckPermissions()
        {
            bool isnew = IsNew;
            bool isdirty = IsDirty;
            bool isdeleting = (_client.Tables[Table].Rows[0].RowState == DataRowState.Deleted);

            if (isnew)
                new SystemPermission(StandardPermissionType.CreateClient).Check();
            else if (isdeleting)
                new ClientPermission(this, StandardPermissionType.Delete).Check();
            else if (isdirty)
            {
                new SystemPermission(StandardPermissionType.UpdateClient).Check();
                new ClientPermission(this, StandardPermissionType.Update).Check();
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

        #endregion

        #region IOMSType Members


        /// <summary>
        /// Sets the current sessions.
        /// </summary>
        /// <remarks></remarks>
        public void SetCurrentSessions()
        {
            if (Session.CurrentSession.CurrentClient != this)
                Session.CurrentSession.CurrentClient = this;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class ConflictSearches
    {
        /// <summary>
        /// 
        /// </summary>
        public int Count;
        /// <summary>
        /// 
        /// </summary>
        public string Info;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictSearches"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="info">The info.</param>
        /// <remarks></remarks>
        public ConflictSearches(int count, string info)
        {
            Count = count;
            Info = info;
        }
    }

    /// <summary>
    /// Holds inforation about a contact when it is linked
    /// agains a client.
    /// </summary>
    /// <remarks></remarks>
	public class ClientContactLink : IEnquiryCompatible, IDisposable
	{

		#region Fields

        /// <summary>
        /// 
        /// </summary>
		private DataTable _common = null;
        /// <summary>
        /// 
        /// </summary>
		private DataRow _data = null;


        /// <summary>
        /// 
        /// </summary>
		public const string Table = "CONTACTS";
        /// <summary>
        /// 
        /// </summary>
		internal const string Sql = "select * from dbclientcontacts";

        /// <summary>
        /// 
        /// </summary>
		private Client _client = null;
        /// <summary>
        /// 
        /// </summary>
		private Contact _contact = null;
		
		#endregion

		#region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
		internal ClientContactLink()
		{
		}


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContactLink"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
		internal ClientContactLink(DataRow data)
		{
            if (data == null)
                throw new ArgumentNullException("data");

            SetData(data);
		}


        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContactLink"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="contact">The contact.</param>
        /// <remarks></remarks>
		public ClientContactLink(Client client, Contact contact)
		{
            if (client != null && contact != null)
            {
                ClientContactLink link = client.GetContact(contact.ID);
                if (link != null)
                {
                    SetData(link._data);
                }
                else
                    Create();
            }
            else
                Create();

			_client = client;
			_contact = contact;
				
			Contact = contact;
            if (client != null)
                SetExtraInfo("clid", client.ClientID);

            Init();


		}

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <remarks></remarks>
        private void Create()
        {
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
            DataRow data = Global.CreateBlankRecord(ref dt, true);
            SetData(data);
            Init();
        }

        /// <summary>
        /// Inits this instance.
        /// </summary>
        /// <remarks></remarks>
        private void Init()
        {
            if (_common.PrimaryKey.Length == 0)
                _common.PrimaryKey = new DataColumn[] { _common.Columns["ID"] };

           //Set the created by and created date of the item.
            _common.Columns["ID"].ReadOnly = false;
            
            SetExtraInfo("clactive", true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientContactLink"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		internal ClientContactLink (long id)
		{
			Fetch(id, null);
		}

        /// <summary>
        /// Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="merge">The merge.</param>
        /// <remarks></remarks>
		private void Fetch (long id, DataRow merge)
		{
			//Make sure that the parameters list is cleared after use.	
			DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where ID = @ID", Table,  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("ID", System.Data.SqlDbType.BigInt, 0, id)});

            if ((data == null) || (data.Rows.Count == 0)) 
			{
				throw new OMSException2("NOCONTLINK","Client Contact with ID :%1% Doesn't Exist.",new Exception(),true,id.ToString());
			}

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _common = data;

			SetData(_common.Rows[0]);
		}
		
		#endregion

		#region Properties

        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <remarks></remarks>
        private void SetData(DataRow data)
        {
            if (data == null)
            {
                _data = null;
                _common = null;
            }
            else
            {
                _data = data;
                _common = _data.Table;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is new.
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
        /// Gets the ID.
        /// </summary>
        /// <remarks></remarks>
		public long ID
		{
			get
			{	
				return Convert.ToInt64(GetExtraInfo("id"));
			}
		}

        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>The contact.</value>
        /// <remarks></remarks>
		public Contact Contact
		{
			get
			{
                if (_contact == null || _contact.IsNew == false)
                {
                    object id = GetExtraInfo("contid");
                    if (id == DBNull.Value)
                        return null;
                    else
                        return Contact.GetContact(Convert.ToInt64(id));
                }
                else
                    return _contact;
			}
			set
			{
                Contact oldval = Contact;


                if (value == null)
                    SetExtraInfo("contid", DBNull.Value);
                else
                {
                    if (oldval == null || oldval.ID != value.ID)
                        SetExtraInfo("contid", value.ID);
                }
			}
		}

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <remarks></remarks>
		public Client Client
		{
			get
			{
				if (_client == null || _client.IsNew == false)
					return Client.GetClient(Convert.ToInt64(GetExtraInfo("clid")));
				else
					return _client;
				
			}
		}

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Name
		{
			get
			{
				return Contact.Name;
			}
		}

        /// <summary>
        /// Gets or sets the relation code.
        /// </summary>
        /// <value>The relation code.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string RelationCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clrelation"));
			}
			set
			{
                string oldval = RelationCode;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                    {
                        SetExtraInfo("clrelation", DBNull.Value);
                        if (_common.Columns.Contains("ContactRelationDesc"))
                            // Fix as if the database has a null or blank entry not setting blank field and or may be an error. MNW
                            SetExtraInfo("ContactRelationDesc", DBNull.Value);
                    }
                    else
                    {
                        SetExtraInfo("clrelation", value);
                        if (_common.Columns.Contains("ContactRelationDesc"))
                            SetExtraInfo("ContactRelationDesc", CodeLookup.GetLookup("CONTRELATION", value));
                       
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs("RelationCode", oldval, value));

                }
			}
		}

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public string Notes
		{
			get
			{
				return Convert.ToString(GetExtraInfo("clnotepad"));
			}
			set
			{
                string oldval = Notes;

                if (oldval != value)
                {
                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("clnotepad", DBNull.Value);
                    else
                        SetExtraInfo("clnotepad", value);

                    OnPropertyChanged(new PropertyChangedEventArgs("Notes", oldval, value));

                }
			}
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ClientContactLink"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
		[EnquiryUsage(true)]
		public bool Active
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetExtraInfo("clactive"), true);
			}
			set
			{
                bool oldval = Active;

                if (oldval != value)
                {
                    SetExtraInfo("clactive", value);
        
                    OnPropertyChanged(new PropertyChangedEventArgs("Active", oldval, value));

                }
			}
		}

		#endregion

		#region IExtraInfo Implementation

        /// <summary>
        /// Sets any information within a dataset based on its field name.
        /// </summary>
        /// <param name="fieldName">Field name within dataset.</param>
        /// <param name="val">Value with the type depending on the field type.</param>
        /// <remarks></remarks>
		public void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_data[fieldName] = val;
		}


        /// <summary>
        /// Gets any extra information from a dataset based on the fieldname.
        /// </summary>
        /// <param name="fieldName">Fieldname within database</param>
        /// <returns>Any object value depending on field type.</returns>
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
				throw new OMSException2("21001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        /// <remarks></remarks>
		public DataSet GetDataset()
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
        /// <remarks></remarks>
		public DataTable GetDataTable()
		{
			return _data.Table.Copy();
		}

		#endregion

		#region IUpdateable Implementation


        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <remarks></remarks>
		public void Update()
		{
            Update(false);
		}

        /// <summary>
        /// Updates the specified preempt max.
        /// </summary>
        /// <param name="preemptMax">if set to <c>true</c> [preempt max].</param>
        /// <remarks></remarks>
        public void Update(bool preemptMax)
        {
            try
            {
                this.Client.Validate(preemptMax, true);
                this.Client.Validate(false, false);

                Session.CurrentSession.Connection.Connect(true);

                DataRow row = _data;
                bool isnew = IsNew;

                if (_common.PrimaryKey.Length == 0)
                    _common.PrimaryKey = new DataColumn[] { _common.Columns["ID"] };

                if (IsDirty)
                    Session.CurrentSession.Connection.Update(row, "dbclientcontacts");



            }
            finally
            {
                Session.CurrentSession.Connection.Disconnect(true);
            }
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        /// <remarks></remarks>
		public void Refresh()
		{
			Refresh(false);
		}

        /// <summary>
        /// Refreshes the specified apply changes.
        /// </summary>
        /// <param name="applyChanges">if set to <c>true</c> [apply changes].</param>
        /// <remarks></remarks>
        public void Refresh(bool applyChanges)
        {
            if (IsNew)
                return;

            DataRow changes = _data;

            if (changes != null && applyChanges)
                Fetch(this.ID, changes);
            else
                Fetch(this.ID, null);
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        /// <remarks></remarks>
		public void Cancel()
		{
			_data.RejectChanges();				
		}

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <remarks></remarks>
		public bool IsDirty
		{
			get
			{
				return (_data.RowState != DataRowState.Unchanged);
			}
		}

		#endregion

		#region IEnquiryCompatible Implementation

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        /// <remarks></remarks>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

        /// <summary>
        /// Raises the <see cref="E:PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
		protected void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

        /// <summary>
        /// Edits the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.ClientContactEdit), param);
		}

        /// <summary>
        /// Edits the specified custom form.
        /// </summary>
        /// <param name="customForm">The custom form.</param>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}
		
		#endregion
		
		#region IParent Implementation

        /// <summary>
        /// Gets the parent.
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks></remarks>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks></remarks>
		protected void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				_data = null;
				_common = null;
				_client = null;
				_contact = null;
			}
			//Dispose unmanaged objects.
		}


		#endregion

		#region Methods

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <remarks></remarks>
		public void Delete()
		{
			if (Active)
			{
				this.Client.Validate(true, false);
				if (Contact.ID == Client.DefaultContact.ID)
					throw new OMSException2("CANTDELDEFCONT", "The default contact of the %CLIENT% cannot be deleted!", "");
				SetExtraInfo("clactive", false);
			}
		}

        /// <summary>
        /// Restores this instance.
        /// </summary>
        /// <remarks></remarks>
		public void Restore()
		{
			if (!Active)
			{
				this.Client.Validate(true, true);
				SetExtraInfo("clactive", true);
			}
		}

        /// <summary>
        /// Gets the contact.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
		public static ClientContactLink GetContact(long id)
		{
			return new ClientContactLink(id);
		}


		#endregion
	}

    /// <summary>
    /// All the log type codes / id's.
    /// </summary>
    /// <remarks></remarks>
	public enum ClientLogType
	{
        /// <summary>
        /// 
        /// </summary>
		ClientFirmContact = 20,
        /// <summary>
        /// 
        /// </summary>
		ClientCreated = 21
	}


}


