using System;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;

namespace FWBS.OMS
{

    /// <summary>
    /// A single appointment object.
    /// </summary>
    public class Appointment : CommonObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Creates a new appointment from scratch.
        /// </summary>
        internal Appointment()
            : base()
        {

            new FileActivity(Session.CurrentSession.CurrentFile, FileStatusActivityType.AppointmentCreation).Check();

            FeeEarner = Session.CurrentSession.CurrentFeeEarner;
            InSync = false;
            SetExtraInfo("appAllDay", false);
            DateTime d = DateTime.Today.AddHours(9);
            SetExtraInfo("appDate", d);
            SetExtraInfo("appEndDate", d);
            Reminder = 0;
            SetExtraInfo("clid", Session.CurrentSession.CurrentFile.ClientID);
            SetExtraInfo("fileid", Session.CurrentSession.CurrentFile.ID);
            SetExtraInfo("appmapi", true);
            SetExtraInfo("appactive", true);
        }

        /// <summary>
        /// Constructs a appointment object based on a task id.
        /// </summary>
        /// <param name="id">The identifier of the task to fetch.</param>
        [EnquiryUsage(true)]
        internal Appointment(long id)
            : base(id)
        {
        }


        /// <summary>
        /// Constructs a new appointment item based on an file and description.
        /// </summary>
        /// <param name="file">The file object associated to the task.</param>
        /// <param name="description">The description / subject of the task.</param>
        public Appointment(OMSFile file, string description)
            : this()
        {
            SetExtraInfo("clid", file.ClientID);
            SetExtraInfo("fileid", file.ID);
            SetExtraInfo("associd", DBNull.Value);
            Description = description;
        }

        /// <summary>
        /// Constructs a new appointment item based on an associate and description.
        /// </summary>
        /// <param name="assoc">The associate object associated to the task.</param>
        /// <param name="description">The description / subject of the task.</param>
        public Appointment(Associate assoc, string description)
            : this()
        {
            SetExtraInfo("clid", assoc.OMSFile.ClientID);
            SetExtraInfo("fileid", assoc.OMSFileID);
            SetExtraInfo("associd", assoc.ID);
            Description = description;
        }


        #endregion

        #region CommonObject Implementation


        protected override string DefaultForm
        {
            get
            {
                return SystemForms.AppointmentEdit.ToString();
            }
        }

        public override string FieldPrimaryKey
        {
            get
            {
                return "appID";
            }
        }

        protected override string PrimaryTableName
        {
            get
            {
                return "APPOINTMENT";
            }
        }

        protected override string SelectStatement
        {
            get
            {
                return "select * from dbappointments";
            }
        }

        protected override string FieldActive
        {
            get
            {
                return "appactive";
            }

        }

        #endregion

        #region IParent Implementation

        /// <summary>
        /// Gets the parent related object.
        /// </summary>
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
        /// Gets the unique OMS id for the task item.
        /// </summary>
        public long ID
        {
            get
            {
                return Convert.ToInt64(UniqueID);
            }
        }

        /// <summary>
        /// Gets the OMS file associated to the appointment.
        /// </summary>
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
        public long FileID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("fileid"));
            }
        }

        /// <summary>
        /// Gets the associate associated to the appointment.
        /// </summary>
        public Associate Associate
        {
            get
            {
                if (AssocID == -1)
                    return null;
                else
                    return Associate.GetAssociate(AssocID);
            }
        }

        /// <summary>
        /// Gets the file id as an integer.
        /// </summary>
        public long AssocID
        {
            get
            {
                return Common.ConvertDef.ToInt64(GetExtraInfo("assocID"), -1);
            }
        }

        /// <summary>
        /// Gets the fee earner associated to the appointment.
        /// </summary>
        public FeeEarner FeeEarner
        {
            get
            {
                return FeeEarner.GetFeeEarner(FeeEarnerID);
            }
            set
            {
                if (value == null)
                    SetExtraInfo("feeusrid", DBNull.Value);
                else
                    SetExtraInfo("feeusrid", value.ID);
            }
        }

        /// <summary>
        /// Gets the fee earner identifier.
        /// </summary>
        public int FeeEarnerID
        {
            get
            {
                return Common.ConvertDef.ToInt32(GetExtraInfo("feeusrid"), -1);
            }
        }

        /// <summary>
        /// Gets or Sets the type of the appointment.
        /// </summary>
        [EnquiryUsage(true)]
        public string Type
        {
            get
            {
                return Convert.ToString(GetExtraInfo("apptype"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("apptype", DBNull.Value);
                else
                    SetExtraInfo("apptype", value);
            }
        }

        /// <summary>
        /// Gets or Sets the description / subject of the appointment.
        /// </summary>
        [EnquiryUsage(true)]
        public string Description
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appdesc"));
            }
            set
            {
                SetExtraInfo("appdesc", value);
            }
        }

        /// <summary>
        /// Gets or Sets the description / subject of the appointment.
        /// </summary>
        [EnquiryUsage(true)]
        public string Location
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appLocation"));
            }
            set
            {
                SetExtraInfo("appLocation", value);
            }
        }

        /// <summary>
        /// Gets or Sets the date that the appointment is set for.
        /// </summary>
        [EnquiryUsage(true)]
        public DateTime StartDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(GetExtraInfo("appDate"));
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                //UTCFIX: DM - 30/11/06 - Comparisons converted to Local
                DateTime oldval = StartDate.ToLocalTime();
                if (value.ToLocalTime() != oldval)
                {
                    SetExtraInfo("appDate", value);

                    var localStartDate = value.ToLocalTime();
                    var localEndDate = EndDate.ToLocalTime();
                    if (localEndDate < localStartDate)
                    {
                        localEndDate = new DateTime(localStartDate.Year, localStartDate.Month, localStartDate.Day, localEndDate.Hour, localEndDate.Minute, localEndDate.Second, DateTimeKind.Local);

                        EndDate = localEndDate < localStartDate ? localStartDate : localEndDate;
                    }
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("StartDate", oldval, this.StartDate));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the end date that the appointment is set for.
        /// </summary>
        [EnquiryUsage(true)]
        public DateTime EndDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(GetExtraInfo("appEndDate"));
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            set
            {
                DateTime oldval = EndDate.ToLocalTime();

                //UTCFIX: DM - 30/11/06 - Comparisons converted to UTC
                if (value.ToLocalTime() != oldval)
                {
                    SetExtraInfo("appEndDate", value);

                    var localEndDate = value.ToLocalTime();
                    var localStartDate = StartDate.ToLocalTime();
                    if (localEndDate < localStartDate)
                    {
                        localStartDate = new DateTime(localEndDate.Year, localEndDate.Month, localEndDate.Day, localStartDate.Hour, localStartDate.Minute, localStartDate.Second, DateTimeKind.Local);

                        StartDate = localEndDate < localStartDate ? localEndDate : localStartDate;
                    }
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EndDate", oldval, this.EndDate));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the a flag that specifies that the appointment is an all dayer.
        /// </summary>
        [EnquiryUsage(true)]
        public bool AllDay
        {
            get
            {
                return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("appAllDay"),false);
            }
            set
            {
                bool oldval = AllDay;

                if (value != oldval)
                {
                    if (value)
                    {
                        StartDate = StartDate.Date;
                        EndDate = EndDate.Date;
                    }

                    SetExtraInfo("appAllDay", value);
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("AllDay", oldval, value));
                }

            }
        }


        /// <summary>
        /// Gets or Sets the notes / subject of the appointment.
        /// </summary>
        [EnquiryUsage(true)]
        public string Notes
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appnotes"));
            }
            set
            {
                SetExtraInfo("appnotes", value);
            }
        }

        /// <summary>
        /// Gets or Sets the Categories (this is a ; separated list)
        /// </summary>
        [EnquiryUsage(true)]
        public string Categories
        {
            get 
            {
                if (_data.Columns.Contains("appCategories") == false)
                    return string.Empty;

                return Convert.ToString(GetExtraInfo("appCategories"));
            }
            set
            {
                if (_data.Columns.Contains("appCategories") == false)
                    return;

                SetExtraInfo("appCategories", value);
            }
        }

        /// <summary>
        /// Gets or Sets the Reminder minutes before start.
        /// A value of -1 matches the code lookup of None
        /// </summary>
        [EnquiryUsage(true)]
        public int Reminder
        {
            get
            {
                if (HasReminder)
                    return Common.ConvertDef.ToInt32(GetExtraInfo("appreminder"), 0);

                return -1;
            }
            set
            {
                if (value == -1)
                {
                    HasReminder = false;
                    value = 0; //Keep this value as 0 in the database to keep it inline with Outlook for the exchange sync service
                }
                else
                {
                    HasReminder = true;
                }
                SetExtraInfo("appreminder", value);                
            }
        }

        /// <summary>
        /// Gets a boolean flag that specifies whether the appointment has a reminder on it or not.
        /// </summary>
        [EnquiryUsage(true)]
        public bool HasReminder
        {
            get
            {
                if (_data.Columns.Contains("appReminderSet") == false)
                    return (Common.ConvertDef.ToInt32(GetExtraInfo("appreminder"), 0) != 0); //Original behaviour

                return Common.ConvertDef.ToBoolean(GetExtraInfo("appReminderSet"), true);                
            }
            set
            {
                if (_data.Columns.Contains("appReminderSet") == false)
                    return;

                bool oldval = HasReminder;
                SetExtraInfo("appReminderSet", value);
                OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("HasReminder", oldval, value));
            }
        }

        /// <summary>
        /// Gets a boolean flag that specifies whether the appointment should be synchronised with Outlook when the Exchange connector is installed
        /// </summary>
        [EnquiryUsage(true)]
        public bool ExchangeSync
        {
            get
            {
                if (_data.Columns.Contains("appExchangeSync") == false)
                    return true;

                return Common.ConvertDef.ToBoolean(GetExtraInfo("appExchangeSync"), false);
            }
            set
            {
                if (_data.Columns.Contains("appExchangeSync") == false)
                    return;

                SetExtraInfo("appExchangeSync", value);
            }
        }

        /// <summary>
        /// Gets or sets a value that specifies whether the current appointment is
        /// in sync with the MAPI entry of the item.
        /// </summary>
        public bool InSync
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetExtraInfo("appdirty"), true);
            }
            set
            {
                SetExtraInfo("appdirty", !value);
            }
        }

        /// <summary>
        /// Gets the appointment MAPI entry ID if available.
        /// </summary>
        public string MAPIEntryID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appmapientryid"));
            }
        }


        #endregion

        #region Methods


        public DataTable GetAppointmentTypes()
        {
            DataTable dt = null;
            try
            {
                if (DataLists.Exists("DSAPPTYPE"))
                {
                    DataLists list = new DataLists("DSAPPTYPE");
                    dt = list.Run() as DataTable;
                    if (IsNew)
                    {
                        DataView vw = new DataView(dt);
                        vw.RowFilter = "cdcode = 'KEYDATE'";
                        if (vw.Count > 0)
                        {
                            for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                                vw[ctr].Delete();
                        }
                        dt.AcceptChanges();
                    }
                }
                else
                {
                    throw new OMSException2("ERRAPPTYPEM", "Error missing datalist 'DSAPPTYPE'", "", new Exception(), true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public override object GetExtraInfo(string fieldName)
        {
            switch (fieldName)
            {
                case "APPTIME":
                    fieldName = "APPDATE";
                    break;
                case "APPENDTIME":
                    fieldName = "APPENDDATE";
                    break;
            }
            return base.GetExtraInfo(fieldName);
        }


        /// <summary>
        /// Attaches the appointment to a mapi session.
        /// </summary>
        /// <param name="entryID">Item entry identifier.</param>
        /// <param name="folderID">The folder identifier.</param>
        /// <param name="storeID">The store identifier.</param>
        public void AttachToMAPI(string entryID, string folderID, string storeID)
        {
            SetExtraInfo("appmapi", true);
            SetExtraInfo("appmapientryid", entryID);
            SetExtraInfo("appmapifolderid", entryID);
            SetExtraInfo("appmapistoreid", entryID);
            InSync = true;
        }

        public override void Update()
        {
            if (IsNew)
            {
                new FileActivity(this.File, FileStatusActivityType.AppointmentCreation).Check();

                switch (Type)
                {
                    case "KEYDATE":
                        throw new OMSException2("ERRCANTCRTKDATE", "Cannot create a key date appointment without using the date wizard routine.");
                }
            }
            base.Update();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a appointment based on a task identifier.
        /// </summary>
        /// <param name="id">The identifier of the appointment.</param>
        /// <returns>A task object.</returns>
        public static Appointment GetAppointment(long id)
        {
            return new Appointment(id);
        }


        #endregion


    }


    /// <summary>
    /// A appointment object that will create appointments on the file.
    /// </summary>
    public class Appointments : IDisposable
    {
        #region Fields

        /// <summary>
        /// Holds the appointment schema.
        /// </summary>
        private DataTable _appointments = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
        internal const string Sql = "select * from dbappointments";

        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
        internal const string Table = "APPOINTMENT";


        /// <summary>
        /// The file that the task is related to.
        /// </summary>
        private OMSFile _file = null;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        private Appointments() { }

        /// <summary>
        /// Constructs appointments for a specific file.
        /// </summary>
        /// <param name="file">The file that is associated to the appointment.</param>
        internal Appointments(OMSFile file)
        {
            _file = file;
            Fetch(_file.ID);
        }

        protected void Fetch(long fileid)
        {
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.Connection.AddParameter("FILEID", SqlDbType.BigInt, 0, _file.ID);
            _appointments = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where fileid = @FILEID", Table, pars);

            //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
            if (_appointments.PrimaryKey == null || _appointments.PrimaryKey.Length == 0)
                _appointments.PrimaryKey = new DataColumn[1] { _appointments.Columns["appid"] };

            _appointments.Columns["appid"].AutoIncrement = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an appointment
        /// </summary>
        /// <param name="feeEarner">The fee earner who will own the appointment.</param>
        /// <param name="type">The type of appointment.</param>
        /// <param name="description">The description / subject of the appointment.</param>
        /// <param name="date">The date that the appointment is due on.</param>
        /// <param name="notes">Additional notes added to the appointment.</param>
        /// <returns>An appointment data row.</returns>
        private DataRow Add(FeeEarner feeEarner, string type, string description, string notes, DateTime date)
        {
            new FileActivity(_file, FileStatusActivityType.AppointmentCreation).Check();

            DataRow app = _appointments.NewRow();
            app["clid"] = _file.ClientID;
            app["fileid"] = _file.ID;
            app["feeusrid"] = feeEarner.ID;
            if (type == null || type == "")
                app["apptype"] = DBNull.Value;
            else
                app["apptype"] = type;
            app["appdesc"] = description;
            app["appdate"] = date.Date;
            app["appenddate"] = date.Date;
            app["appallday"] = true;
            app["appnotes"] = notes;
            app["appreminder"] = 0;
            app["appmapi"] = true;
            app["appdirty"] = true;
            app["appactive"] = true;
            app["Created"] = DateTime.Now;
            app["CreatedBy"] = Session.CurrentSession.CurrentUser.ID;
            app["appLocation"] = Session.CurrentSession.Resources.GetResource("KEYDATELOCATION", "Key Date", "").Text;
            _appointments.Rows.Add(app);
            return app;
        }

        public void Add(FeeEarner feeEarner, Guid relatedID, string type, string description, DateTime date, string notes, string timeZone = null)
        {
            DataRow app = Add(feeEarner, type, description, notes, date);
            app["apprelatedid"] = relatedID;
            if (timeZone != null)
                app["appTimeZone"] = timeZone;
        }

        public void Add(FeeEarner feeEarner, string type, string description, DateTime date, string notes)
        {
            Add(feeEarner, type, description, notes, date);
        }

        /// <summary>
        /// Persists the appointments to the database.
        /// </summary>
        public void Update()
        {
            foreach (DataRow row in _appointments.Rows)
            {
                row["clid"] = _file.ClientID;
                row["fileid"] = _file.ID;
            }
            Session.CurrentSession.Connection.Update(_appointments, Sql + " where fileid = " + _file.ID.ToString());
            Fetch(_file.ID);
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
                if (_appointments != null)
                {
                    _appointments.Dispose();
                    _appointments = null;
                }
            }

            //Dispose unmanaged objects.
        }

      

        #endregion

        #region Static Methods

        /// <summary>
        /// Fetches all the appointments and puts them in a data table.
        /// </summary>
        /// <returns>A list of appointments in data table form.</returns>
        public static DataTable GetAppointments()
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] param = new IDataParameter[1];
            param[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);

            //UTCFIX: DM - 30/11/06 - Change GetDate() to GetUTCDate()
            return Session.CurrentSession.Connection.ExecuteSQLTable("select *,dbo.GetCodeLookupDesc('APPLOC',appLocation,@UI) as Location from dbappointments where appDate >= dbo.GetStartDate(GetUTCDate()) order by feeusrid", Table, param);
        }

        /// <summary>
        /// Updates the appointments table as a bulk transaction.
        /// </summary>
        public static void UpdateAppointments(DataTable dt)
        {
            Session.CurrentSession.CheckLoggedIn();
            Session.CurrentSession.Connection.Update(dt, "select * from dbappointments");
            dt.AcceptChanges();
        }

        #endregion
    }
}
