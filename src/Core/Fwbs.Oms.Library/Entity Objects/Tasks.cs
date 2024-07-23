using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{


    /// <summary>
    /// A single task object.
    /// </summary>
    public class Task : CommonObject
    {
        #region Constructors & Destructors

        /// <summary>
        /// Creates a new appointment from scratch.
        /// </summary>
        internal Task () : base()
        {
            FeeEarner = Session.CurrentSession.CurrentFeeEarner;
            InSync = false;
            SetExtraInfo("fileid", Session.CurrentSession.CurrentFile.ID);
            SetExtraInfo("tskmapi", true);
            SetExtraInfo("tskactive", true);
            SetExtraInfo("tskcomplete", false);
            Type = "GENERAL";
        }

        /// <summary>
        /// Constructs a task object based on a task id.
        /// </summary>
        /// <param name="id">The identifier of the task to fetch.</param>
        [EnquiryUsage(true)]
        internal Task (long id) : base (id)
        {
        }


        /// <summary>
        /// Constructs a new task item based on a file and description.
        /// </summary>
        /// <param name="file">The file object associated to the task.</param>
        /// <param name="description">The description / subject of the task.</param>
        public Task (OMSFile file, string description) : this()
        {
            SetExtraInfo("fileid", file.ID);
            Description = description;
        }


        #endregion
    
        #region CommonObject Implementation


        protected override string DefaultForm
        {
            get
            {
                return SystemForms.TaskEdit.ToString();
            }
        }

        public override string FieldPrimaryKey
        {
            get
            {
                return "tskID";
            }
        }

        protected override string PrimaryTableName
        {
            get
            {
                return "TASK";
            }
        }

        protected override string SelectStatement
        {
            get
            {
                return "select * from dbtasks";
            }
        }

        protected override string FieldActive
        {
            get
            {
                return "tskactive";
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
        /// Gets the OMS file associated to the task.
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
        /// Gets the fee earner associated to the task.
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
        /// Gets or Sets the type of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Type
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tsktype"));
            }
            set
            {
                if (value == null || value == "")
                    SetExtraInfo("tsktype", DBNull.Value);
                else
                {
                    SetExtraInfo("tsktype", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the sub type of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Group
        {
            get
            {
                if (_data.Columns.Contains("tskgroup"))
                    return Convert.ToString(GetExtraInfo("tskgroup"));
                else
                    return String.Empty;
            }
            set
            {
                if (_data.Columns.Contains("tskgroup"))
                {
                    if (value == null || value == "")
                        SetExtraInfo("tskgroup", DBNull.Value);
                    else
                    {
                        SetExtraInfo("tskgroup", value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Role for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Role
        {
            get
            {
                if (_data.Columns.Contains("tskRole"))
                    return Convert.ToString(GetExtraInfo("tskRole"));
                else
                    return String.Empty;
            }
            set
            {
                if (_data.Columns.Contains("tskRole"))
                {
                    if (value == null || value == "")
                        SetExtraInfo("tskRole", DBNull.Value);
                    else
                    {
                        SetExtraInfo("tskRole", value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Role for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public int Mileage
        {
            get
            {
                if (_data.Columns.Contains("tskMileage"))
                    return Convert.ToInt16(GetExtraInfo("tskMileage"));
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("tskMileage"))
                {
                    SetExtraInfo("tskMileage", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the TimeEstimate for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public int TimeEstimate
        {
            get
            {
                if (_data.Columns.Contains("tskTimeEstimate"))
                    return Convert.ToInt16(GetExtraInfo("tskTimeEstimate"));
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("tskTimeEstimate"))
                {
                    SetExtraInfo("tskTimeEstimate", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the TimeTaken for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public int TimeTaken
        {
            get
            {
                if (_data.Columns.Contains("tskTimeTaken"))
                    return Convert.ToInt16(GetExtraInfo("tskTimeTaken"));
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("tskTimeTaken"))
                {
                        SetExtraInfo("tskTimeTaken", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Importance for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public short Importance
        {
            get
            {
                if (_data.Columns.Contains("tskImportance"))
                    return Convert.ToSByte(GetExtraInfo("tskImportance"));
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("tskImportance"))
                {
                    SetExtraInfo("tskImportance", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Priority for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public short Priority
        {
            get
            {
                if (_data.Columns.Contains("tskPriority"))
                    return Convert.ToSByte(GetExtraInfo("tskPriority"));
                else
                    return 0;
            }
            set
            {
                if (_data.Columns.Contains("tskPriority"))
                {
                    SetExtraInfo("tskPriority", value);
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Related/Escalated Task.
        /// </summary>
        [EnquiryUsage(true)]
        public Task EscalatedTask
        {
            get
            {
                if (EscalatedID != -1)
                {
                    return Task.GetTask(EscalatedID);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    EscalatedID = value.ID;
                }
                else
                {
                    EscalatedID = -1;
                }

            }
        }

        /// <summary>
        /// Gets or Sets the EscalatedID for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public long EscalatedID
        {
            get
            {
                if (_data.Columns.Contains("tskEscalatedID"))
                    return Convert.ToInt64(GetExtraInfo("tskEscalatedID"));
                else
                    return -1;
            }
            set
            {
                if (_data.Columns.Contains("tskEscalatedID"))
                {
                    if (value == -1)
                        SetExtraInfo("tskEscalatedID", DBNull.Value);
                    else
                    {
                        SetExtraInfo("tskEscalatedID", value);
                    }
                }
            }
        }





        
        private string phasedesc;
        
        /// <summary>
        /// Gets the Phase Description from the configured PhaseID for this task
        /// </summary>
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
                        phasedesc = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar("select phdesc from dbfilephase where phID = @id", pars));
                    }
                    return phasedesc;
                }
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Gets and Sets the phID Phase ID normally set based on the Phase of the OMSFile.
        /// </summary>
        public long? PhaseId
        {
            get
            {
                object id = GetExtraInfo("phID");
                if (id == DBNull.Value || id == null)
                    return null;
                else
                    return Convert.ToInt64(id);
            }
            set
            {
                if (_data.Columns.Contains("phID"))
                {
                    if (value == null)
                        SetExtraInfo("phID", DBNull.Value);
                    else
                    {
                        SetExtraInfo("phID", value);
                    }
                }

            }
        }





        /// <summary>
        /// Gets or Sets the Category for the Task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Category
        {
            get
            {
                if (_data.Columns.Contains("tskCategory"))
                    return Convert.ToString(GetExtraInfo("tskCategory"));
                else
                    return String.Empty;
            }
            set
            {
                if (_data.Columns.Contains("tskCategory"))
                {
                    if (value == null || value == "")
                        SetExtraInfo("tskCategory", DBNull.Value);
                    else
                    {
                        SetExtraInfo("tskCategory", value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the Suspended Date.
        /// </summary>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL Suspended
        {
            get
            {
                if (_data.Columns.Contains("tskSuspended"))
                    return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("tskSuspended"), DBNull.Value);
                else
                    return DBNull.Value;
            }
            set
            {
                if (_data.Columns.Contains("tskSuspended"))
                {
                    SetExtraInfo("tskSuspended", value.ToObject());
                    // Set the Suspended By Field.
                    SuspendedBy = Session.CurrentSession.CurrentUser;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the user that the task is suspended by.
        /// </summary>
        public User SuspendedBy
        {
            get
            {
                if (_data.Columns.Contains("tskSuspendedBy"))
                {
                    object userid = GetExtraInfo("tskSuspendedBy");
                    if (userid == DBNull.Value)
                        return null;
                    else
                        return User.GetUser(Convert.ToInt32(userid));
                }
                else
                {
                    return null;
                }

            }
            set
            {
                if (_data.Columns.Contains("tskSuspendedBy"))
                {
                    if (value == null)
                        SetExtraInfo("tskSuspendedBy", DBNull.Value);
                    else
                        SetExtraInfo("tskSuspendedBy", value.ID);
                }
            }
        }




        /// <summary>
        /// Gets or Sets the description / subject of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Description
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tskdesc"));
            }
            set
            {
                if (value == null) value = String.Empty;
                SetExtraInfo("tskdesc", value);
            }
        }

        /// <summary>
        /// Gets or Sets the date that the task is due to be completed.
        /// </summary>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL Due
        {
            get
            {
                return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("tskDue"), DBNull.Value);
            }
            set
            {
                SetExtraInfo("tskdue", value.ToObject());
            }
        }

        /// <summary>
        /// Gets or Sets the notes / subject of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public string Notes
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tsknotes"));
            }
            set
            {
                if (value == null) value = String.Empty;
                SetExtraInfo("tsknotes", value);
            }
        }

        /// <summary>
        /// Gets or Sets the date of completion of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL Completed
        {
            get
            {
                return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("tskcompleted"), DBNull.Value);
            }
            set
            {
                if (value == DBNull.Value)
                {
                    SetExtraInfo("tskcomplete", false);
                    if (_data.Columns.Contains("tskcompletedby"))
                        SetExtraInfo("tskcompletedby", DBNull.Value);
                }
                else
                {
                    SetExtraInfo("tskcomplete", true);
                    if (_data.Columns.Contains("tskcompletedby"))
                        SetExtraInfo("tskcompletedby", Session.CurrentSession.CurrentUser.ID);
                }

                SetExtraInfo("tskcompleted", value.ToObject());
            }
        }



        /// <summary>
        /// Gets or Sets a boolean flag that specifies whether the task has been completed or not.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetExtraInfo("tskcomplete"), false);
            }
            set
            {
                if (value)
                {
                    SetExtraInfo("tskcompleted", DateTime.Now);
                    if (_data.Columns.Contains("tskcompletedby"))
                        SetExtraInfo("tskcompletedby", Session.CurrentSession.CurrentUser.ID);
                }
                else
                {
                    SetExtraInfo("tskcompleted", DBNull.Value);
                    if (_data.Columns.Contains("tskcompletedby"))
                        SetExtraInfo("tskcompletedby", DBNull.Value);
                }

                SetExtraInfo("tskcomplete", value);

            }
        }


        /// <summary>
        /// Gets or Sets the Reminder date of the task.
        /// </summary>
        [EnquiryUsage(true)]
        public Common.DateTimeNULL Reminder
        {
            get
            {
                return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("tskreminder"), DBNull.Value);
            }
            set
            {
                SetExtraInfo("tskreminder", value.ToObject());
            }
        }

        /// <summary>
        /// Gets a boolean flag that specifies whether the task has a reminder on it or not.
        /// </summary>
        public bool HasReminder
        {
            get
            {
                return (!Reminder.IsNull);
            }
        }
        
        /// <summary>
        /// Gets or sets a value that specifies whether the current task is
        /// in sync with the MAPI entry of the item.
        /// </summary>
        public bool InSync
        {
            get
            {
                return Common.ConvertDef.ToBoolean(GetExtraInfo("tskdirty"), true);
            }
            set
            {
                SetExtraInfo("tskdirty", !value);
            }
        }

        /// <summary>
        /// Gets the tasks MAPI entry ID if available.
        /// </summary>
        public string MAPIEntryID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tskmapientryid"));
            }
        }

        /// <summary>
        /// Gets or Sets the team.
        /// </summary>
        public FileManagement.Team AssignedTeam
        {
            get
            {
                object tmid = GetExtraInfo("tmid");
                if (tmid == DBNull.Value)
                    return null;
                else
                    return FileManagement.Team.GetTeam(Convert.ToInt32(tmid));

            }
            set
            {
                if (value == null)
                    SetExtraInfo("tmid", DBNull.Value);
                else
                    SetExtraInfo("tmid", value.ID);
            }
        }

       /// <summary>
       /// Gets or Setsthe user that the task is assigned to.
       /// </summary>
        public User AssignedTo
        {
            get
            {
                object userid = GetExtraInfo("usrid");
                if (userid == DBNull.Value)
                    return null;
                else
                    return User.GetUser(Convert.ToInt32(userid));
            }
            set
            {
                if (value == null)
                    SetExtraInfo("usrid", DBNull.Value);
                else
                    SetExtraInfo("usrid", value.ID);
            }
        }


        [EnquiryUsage(true)]
        public string WorkflowGroupID
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tskConfigId"));
            }
            set
            {
                if (value == null) value = String.Empty;
                SetExtraInfo("tskConfigId", value);
            }
        }

        #endregion

        #region Methods

        public DataTable GetTaskTypes()
        {
            DataTable dt = null;
            if (DataLists.Exists("DSTASKTYPE") == true)
            {
                DataLists list = new DataLists("DSTASKTYPE");
                dt = list.Run() as DataTable;
                if (IsNew && dt != null)
                {
                    DataView vw = new DataView(dt);
                    vw.RowFilter = "cdcode = 'KEYDATE' or cdcode = 'FILEREVIEWDATE' or cdcode = 'DOCUMENT' or cdcode = 'MILESTONE'";
                    if (vw.Count > 0)
                    {
                        for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                            vw[ctr].Delete();
                    }
                    dt.AcceptChanges();
                }
            }
            else
                throw new OMSException2("ERRDSTTMIS","Error 'DSTASKTYPE' data list is missing.","");
             
            return dt;
        }

        /// <summary>
        /// Attaches the task to a mapi session.
        /// </summary>
        /// <param name="entryID">Item entry identifier.</param>
        /// <param name="folderID">The folder identifier.</param>
        /// <param name="storeID">The store identifier.</param>
        public void AttachToMAPI(string entryID, string folderID, string storeID)
        {
            SetExtraInfo("tskmapi", true);
            SetExtraInfo("tskmapientryid", entryID);
            SetExtraInfo("tskmapifolderid", entryID);
            SetExtraInfo("tskmapistoreid", entryID);
            InSync = true;
        }

        public override void Update()
        {
            if (IsNew)
            {
                switch (Type)
                {
                    case "KEYDATE":
                        throw new OMSException2("ERRTSKKEYDATECH", "Cannot create a key date task without using the date wizard routine.");
                    case "FILEREVIEWDATE":
                        throw new OMSException2("ERRTSKFILEREVCH", "Cannot create a file review date without using the file review date wizard.");
                    case "DOCUMENT":
                        throw new  OMSException2("ERRTSKDOCCH", "Cannot create a document task without saving a document to the system.");
                }
                if (IsNew && _data.Columns.Contains("phID"))
                {
                    object inval;
                    inval = this.File.GetExtraInfo("phID");
                    if (inval != DBNull.Value)
                    {
                        PhaseId = Convert.ToInt64(inval) ;
                    }
                }
            }

            if (Type == "FILEREVIEWDATE" && IsCompleted)
            {
                throw new OMSException2("ERRCANTCOMPFREV", "Cannot complete a file review date without using the file review date wizard.");
            }
            
            base.Update();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a task based on a task identifier.
        /// </summary>
        /// <param name="id">The identifier of the task.</param>
        /// <returns>A task object.</returns>
        public static Task GetTask(long id)
        {
            return new Task(id);
        }

        
        #endregion

    }

    /// <summary>
    /// A task object that will create tasks on the session.
    /// </summary>
    public class Tasks : IDisposable
    {
        #region Events

        /// <summary>
        /// An event that gets raised before an update occurs.
        /// </summary>
        public event System.ComponentModel.CancelEventHandler Updating;


        #region Updated

        private Fwbs.Framework.ComponentModel.WeakEvent<EventHandler, EventArgs> _updated;
        /// <summary>
        /// An event that get fired after an update has occurred.
        /// </summary>
        public event EventHandler Updated
        {
            add
            {
                _updated.Add(value);
            }
            remove
            {
                _updated.Remove(value);
            }
        }

        private void RaiseUpdated(EventHandler ev, object sender, EventArgs e)
        {
            ev(sender, e);
        }

 
        protected void OnUpdated()
        {
            _updated.Raise(this, EventArgs.Empty); 
        }


        #endregion

        #endregion

        #region Event Methods

        /// <summary>
        /// Raises the Updating event.
        /// </summary>
        protected void OnUpdating(System.ComponentModel.CancelEventArgs e)
        {
            if (Updating != null)
                Updating(this, e);
        }



        #endregion

        #region Fields

        /// <summary>
        /// Holds the task schema.
        /// </summary>
        private DataTable _tasks = null;

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
        internal const string Sql = "select * from dbtasks";

        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
        internal const string Table = "TASKS";
    

        /// <summary>
        /// The file that the task is related to.
        /// </summary>
        private OMSFile _file = null;

        #endregion

        #region Properties

        public bool IsDirty
        {
            get
            {
                return (_tasks.GetChanges() != null);
            }
        }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Default Constructor.
        /// </summary>
        private Tasks()    
        {
            _updated = new Fwbs.Framework.ComponentModel.WeakEvent<EventHandler, EventArgs>(RaiseUpdated);
        }

        /// <summary>
        /// Constructs a tasks list for a specific file.
        /// </summary>
        /// <param name="file">The file that is associated to the task.</param>
        internal Tasks(OMSFile file) 
            : this()
        {
            _file = file;
            Fetch(_file.ID);

            OnObjectEvent(Extensibility.ObjectEvent.Loaded);
        }

        private DateTime? _lastFetchTimeStamp;

        /// <summary>
        /// The updated time stamp of the most recently updated task as set in the database
        /// This is used for subsequent fetches of data so only tasks that have been updated are fetched
        /// </summary>
        private DateTime? LastFetchTimeStamp
        {
            get
            {
                return _lastFetchTimeStamp;
            }
            
        }

        protected void Fetch(long fileID)
        {
            List<IDataParameter> pars = new List<IDataParameter>();
            pars.Add(Session.CurrentSession.Connection.AddParameter("FILEID", SqlDbType.BigInt, 0, _file.ID));

            DateTime? timestamp = LastFetchTimeStamp;

            if (_tasks == null || timestamp == null)
            {
                _tasks = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where fileid = @FILEID", Table, pars.ToArray());
                SetPrimaryKeys(_tasks);
                SetSortOrder(_tasks);
                SetAutoIncrement(_tasks);
            }
            else
            {
                pars.Add(Session.CurrentSession.Connection.AddParameter("TIMESTAMP", timestamp.Value.ToUniversalTime()));
                DataTable newTasks = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where fileid = @FILEID AND (Updated > @TIMESTAMP or Created > @TIMESTAMP)", Table, pars.ToArray());
                SetPrimaryKeys(newTasks);
                _tasks.Merge(newTasks);
                SetAutoIncrement(_tasks);
            }

            SetLastFetchTimeStamp();
            
        }

        private void SetLastFetchTimeStamp()
        {
            if (_tasks == null)
            {
                _lastFetchTimeStamp = null;
                return;
            }

            var updated = _tasks.AsEnumerable().Max((row) => row.Field<DateTime?>("Updated"));
            var created = _tasks.AsEnumerable().Max((row) => row.Field<DateTime?>("Created"));
        
            _lastFetchTimeStamp = GetLatestDate(updated, created);       

        }
       
        private static DateTime? GetLatestDate(DateTime? a, DateTime? b)
        {
            if (a == null && b == null)
                return null;

            if (a == null)
                return b.Value;

            if (b == null)
                return a.Value;           

            if (a.Value > b.Value)
                return a;
            else
                return b;
        }

        private static void SetPrimaryKeys(DataTable table)
        {
            //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
            if (table.PrimaryKey == null || table.PrimaryKey.Length == 0)
                table.PrimaryKey = new []{ table.Columns["tskid"] };                      
        }

        private static void SetAutoIncrement(DataTable table)
        {
            var coltaskid = table.Columns["tskid"];
            coltaskid.AutoIncrement = true;

            long seed = -1; //Use negative seed to avoid constraint issues (database identity should start at 1)

            long? minimumId = table.AsEnumerable().Min((row) => row.Field<long?>(coltaskid));

            if (minimumId.HasValue && minimumId.Value < seed)
                seed = minimumId.Value - 1;                     

            coltaskid.AutoIncrementSeed = seed;
            coltaskid.AutoIncrementStep = -1;  //Use negative step to avoid constraint issues (database identity is increasing so do opposite to avoid clashes)
        }

        private static void SetSortOrder(DataTable table)
        {
            table.DefaultView.Sort = "tskdue ASC";                   
        }

        #endregion

        #region Static
        public static void ToggleCompleted(int TaskID)
        {
            AskEventArgs askargs = new AskEventArgs("TASKTOG","Do you wish to Toggle the Completed Status?","",FWBS.OMS.AskResult.Yes);
            Session.CurrentSession.OnAsk(null,askargs);
            if (askargs.Result == FWBS.OMS.AskResult.Yes) 
            {
                IDataParameter[] pars = new IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.AddParameter("TaskID", SqlDbType.BigInt, 0, TaskID);
                //UTCFIX: DM - 30/11/06 - Change GetDate() to GetUTCDate()
                Session.CurrentSession.Connection.ExecuteSQL("declare @Toggle bit; declare @ToggleDate datetime; select @Toggle = CASE tskComplete WHEN 1 THEN 0 ELSE 1 END, @ToggleDate = CASE tskComplete WHEN 1 THEN null ELSE GetUTCDate() END FROM dbTasks where tskID = @TaskID; update dbtasks set [tskComplete] = @Toggle, tskCompleted = @ToggleDate where tskID = @TaskID", pars); 
            }
        }

        public static void Completed(int TaskID, bool Yes)
        {
            AskEventArgs askargs = null;
            if (Yes)
                askargs = new AskEventArgs("TASKCMP","Do you wish to Complete the Task?","",FWBS.OMS.AskResult.Yes);
            else
                askargs = new AskEventArgs("TASKUCMP","Do you wish to Uncomplete the Task?","",FWBS.OMS.AskResult.Yes);

            Session.CurrentSession.OnAsk(null,askargs);
            if (askargs.Result == FWBS.OMS.AskResult.Yes) 
            {
                IDataParameter[] pars = new IDataParameter[3];
                pars[0] = Session.CurrentSession.Connection.AddParameter("TaskID", SqlDbType.BigInt, 0, TaskID);
                pars[1] = Session.CurrentSession.Connection.AddParameter("Yes", SqlDbType.Bit, 0, Yes);
                if (Yes)
                    pars[2] = Session.CurrentSession.Connection.AddParameter("YesDate", SqlDbType.DateTime, 0, DateTime.Now);
                else
                    pars[2] = Session.CurrentSession.Connection.AddParameter("YesDate", SqlDbType.DateTime, 0, DBNull.Value);

                Session.CurrentSession.Connection.ExecuteSQL("update dbtasks set [tskComplete] = @Yes, tskCompleted = @YesDate where tskID = @TaskID", pars); 
            }
        }

        public static void AssignTo(long[] tasks, int? teamId, int? userId)
        {
            Session.CurrentSession.CheckLoggedIn();

            try
            {
                Session.CurrentSession.Connection.Connect(true);

                Session.CurrentSession.Connection.BeginTransaction();
                AssignToTeam(tasks, teamId);
                AssignToUser(tasks, userId);
                Session.CurrentSession.Connection.CommitTransaction();
            }
            catch
            {
                Session.CurrentSession.Connection.RollbackTransaction();
                throw;
            }
            finally
            {
                Session.CurrentSession.Connection.Disconnect();
            }
        }

        public static void AssignToUser(long[] tasks, int? userId)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (tasks == null)
                throw new ArgumentNullException("tasks");

            //UTCFIX: DM - 30/11/06 - Change GetDate() to GetUTCDate()
            string sql = "update dbtasks set usrid = @ASSIGNEDTO, updatedby = @UPDATEDBY, updated = GetUTCDate() where tskid in ('{0}')";
            string[] vals = Array.ConvertAll<long, string>(tasks, new Converter<long, string>(delegate(long val) { return val.ToString(); }));
            sql = String.Format(sql, String.Join("', '", vals));
            IDataParameter[] pars = new IDataParameter[2];
            if (userId.HasValue)
                pars[0] = Session.CurrentSession.Connection.AddParameter("ASSIGNEDTO", userId.Value);
            else
                pars[0] = Session.CurrentSession.Connection.AddParameter("ASSIGNEDTO", DBNull.Value);

            pars[1] = Session.CurrentSession.Connection.AddParameter("UPDATEDBY", Session.CurrentSession.CurrentUser.ID);

            Session.CurrentSession.Connection.ExecuteSQL(sql, pars);
        }

        public static void AssignToTeam(long[] tasks, int? teamId)
        {
            Session.CurrentSession.CheckLoggedIn();

            if (tasks == null)
                throw new ArgumentNullException("tasks");

            //UTCFIX: DM - 30/11/06 - Change GetDate() to GetUTCDate()
            string sql = "update dbtasks set [tmid] = @TEAM, updatedby = @UPDATEDBY, updated = GetUTCDate() where tskid in ('{0}')";
            string[] vals = Array.ConvertAll<long, string>(tasks, new Converter<long, string>(delegate(long val) { return val.ToString(); }));
            sql = String.Format(sql, String.Join("', '", vals));
            IDataParameter[] pars = new IDataParameter[2];
            if (teamId.HasValue)
                pars[0] = Session.CurrentSession.Connection.AddParameter("TEAM", teamId.Value);
            else
                pars[0] = Session.CurrentSession.Connection.AddParameter("TEAM", DBNull.Value);

            pars[1] = Session.CurrentSession.Connection.AddParameter("UPDATEDBY", Session.CurrentSession.CurrentUser.ID);

            Session.CurrentSession.Connection.ExecuteSQL(sql, pars); 
        }


        #endregion

        #region Methods

        /// <summary>
        /// Adds a task to the task list.
        /// </summary>
        /// <param name="feeEarner">The fee earner who will own the task.</param>
        /// <param name="type">The type of task to add.</param>
        /// <param name="description">The description / subject of the task.</param>
        /// <param name="due">The date that the task is due on.</param>
        /// <param name="notes">Additional notes added to the task.</param>
        /// <returns>The new task data row.</returns>
        private DataRow Add(FeeEarner feeEarner, string type, string description, string notes, DateTime due)
        {
            DataRow tsk = _tasks.NewRow();
            tsk["fileid"] = _file.ID;
            tsk["feeusrid"] = feeEarner.ID;
            if (type == null || type == "")
                tsk["tsktype"] = DBNull.Value;
            else
                tsk["tsktype"] = type;
            tsk["tskdesc"] = description;
            tsk["tsknotes"] = notes;
            tsk["tskdue"] = due;
            tsk["tskcompleted"] = DBNull.Value;
            tsk["tskcomplete"] = false;
            tsk["tskreminder"] =  due;
            tsk["tskMAPI"] = true;
            tsk["tskdirty"] = true;
            tsk["tskactive"] = true;
            tsk["Created"] = DateTime.Now;
            tsk["CreatedBy"] = Session.CurrentSession.CurrentUser.ID;

            //If a file review date has been added make sure that there is only one available.
            if (type == "FILEREVIEWDATE")
            {
                DataView filereview = new DataView(_tasks);
                filereview.RowFilter = "tsktype = 'FILEREVIEWDATE' and tskactive = true and tskcomplete = false";
                foreach (DataRowView row in filereview)
                {
                    Common.DateTimeNULL taskdue = Common.ConvertDef.ToDateTimeNULL(row["tskdue"], DBNull.Value);
                    //UTCFIX: DM - 30/11/06 - Comparison is converted to common date within DateTimeNULL object.
                    if (due > taskdue)
                    {

                        row["tskCompleted"] = System.DateTime.Now;
                        row["tskComplete"] = true; 
                    }
                    else
                    {
                        row["tskactive"] = false;
                    }
                }

            }

            _tasks.Rows.Add(tsk);
            return tsk;
        }

        public void Add(FeeEarner feeEarner, string type, string description, DateTime due, string notes)
        {
            Add(feeEarner, type, description, notes, due);
        }

        public void Add(FeeEarner feeEarner, Guid relatedID, string type, string description, DateTime due, string notes)
        {
            Add(feeEarner, type, description, notes, due)["tskrelatedid"] = relatedID;
        }

        public void Add(FeeEarner feeEarner, OMSDocument document, string type, string description, DateTime due, string notes)
        {
            Add(feeEarner, type, description, notes, due)["docid"] = document.ID;
        }

        public DataView Find(string filter)
        {
            DataView vw = new DataView(_tasks);
            vw.RowFilter = filter;
            return vw;
        }

        /// <summary>
        /// Refresh the tasks collection with any newly added or updated tasks
        /// </summary>
        public void Refresh()
        {
            if (_file != null)
                Fetch(_file.ID);
        }

        /// <summary>
        /// Refresh the tasks collection
        /// </summary>
        /// <param name="full">Force a complete refresh instead of only merging new or updated tasks with the existing collection</param>
        public void Refresh(bool full)
        {
            if (full)
            {
                _tasks = null;
                _lastFetchTimeStamp = null;
            }

            Refresh();
        }

        /// <summary>
        /// Persists the tasks to the database.
        /// </summary>
        public void Update()
        {
            //New addin object event arguments
            Extensibility.ObjectEvent oev;
            oev = Extensibility.ObjectEvent.Updating;
            Extensibility.ObjectEventArgs oea = new Extensibility.ObjectEventArgs(this, oev, true);
            OnObjectEvent(oea);
            if (oea.Cancel)
                return;

            System.ComponentModel.CancelEventArgs cancel = new System.ComponentModel.CancelEventArgs();        
            OnUpdating(cancel);
            if (cancel.Cancel)
                return;

            foreach(DataRow row in _tasks.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                    row["fileid"] = _file.ID;
            }

            Session.CurrentSession.Connection.Update(_tasks, "dbtasks", true);
            Fetch(_file.ID);

            oev = Extensibility.ObjectEvent.Updated;
            oea = new Extensibility.ObjectEventArgs(this, oev, false);
            OnObjectEvent(oea);

            OnUpdated();
        }

        public void Cancel()
        {
            if (_tasks != null)
                _tasks.RejectChanges();
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
                if (_tasks != null)
                {
                    _tasks.Dispose();
                    _tasks = null;
                }
            }
            
            //Dispose unmanaged objects.
        }


        #endregion

        #region Extensibility Object Events


        protected Extensibility.ObjectEventArgs OnObjectEvent(Extensibility.ObjectEvent ev)
        {
            //Call the extensibility event for addins.
            Extensibility.ObjectEventArgs e = new Extensibility.ObjectEventArgs(this, ev);
            Session.CurrentSession.OnObjectEvent(e);
            return e;
        }

        protected void OnObjectEvent(Extensibility.ObjectEventArgs e)
        {
            Session.CurrentSession.OnObjectEvent(e);
        }

        #endregion

    }
}
