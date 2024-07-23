using System;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;


namespace FWBS.OMS
{
    /// <summary>
    /// Main TimeRecord object, from TimeCollection or direct ID
    /// </summary>
    /// <remarks></remarks>
    public class TimeRecord : IEnquiryCompatible, IDisposable
    {
        #region Events
        /// <summary>
        /// Occurs when [dirty].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler Dirty;

        /// <summary>
        /// Called when [dirty].
        /// </summary>
        /// <remarks></remarks>
        protected void OnDirty()
        {
            if (Dirty != null)
                Dirty(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when [cancel time].
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler CancelTime;

        /// <summary>
        /// Called when [cancel time].
        /// </summary>
        /// <remarks></remarks>
        protected void OnCancelTime()
        {
            if (CancelTime != null)
                CancelTime(this, EventArgs.Empty);
        }
        #endregion

        #region Fields
        /// <summary>
        /// Cache version of the FeeEarner
        /// </summary>
        private FeeEarner _feeearner = null;
        /// <summary>
        /// False and the Time Record is Written on Update else Time is written by the Batch
        /// </summary>
        private bool _batchmode = false;

        /// <summary>
        /// Internal data source.
        /// </summary>
        private DataTable _timerecord = null;
        /// <summary>
        /// The data row that a singular Time Record object accesses for information.
        /// </summary>
        private DataRow _data = null;

        /// <summary>
        /// The TimeRecords OMSFile .
        /// </summary>
        private OMSFile _file = null;

        /// <summary>
        /// The TimeRecords OMSDocument .
        /// </summary>
        private OMSDocument _doc = null;

        /// <summary>
        /// Used to store the Time Recording Description
        /// </summary>
        private string _timedesc = "";


        /// <summary>
        /// Table name used internally for this object.  This is used by the update command, so it knows what table to update
        /// incase a dataset with more than one table is used.
        /// </summary>
        public const string Table = "TIMERECORD";

        /// <summary>
        /// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
        /// </summary>
        internal const string Sql = "select * from dbTimeLedger";

        /// <summary>
        /// Time Record Activity
        /// </summary>
        private Activities _activity = null;

        /// <summary>
        /// Is the Time Recording Dirty
        /// </summary>
        private bool _isdirty = false;

        /// <summary>
        /// 
        /// </summary>
        private TimeCollection _parent = null;

        /// <summary>
        /// 
        /// </summary>
        private int _parentinx = -1;

        #endregion

        #region Constructors

        // *************************************************************************************************
        // *************************************************************************************************
        // CREATING CONSTRUCTORS
        // *************************************************************************************************
        // *************************************************************************************************

        /// <summary>
        /// Creates a new Time Record object.  This routine is used by the enquiry engine
        /// to create new Time Record object.
        /// </summary>
        public TimeRecord()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                _timerecord = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
                _timerecord.Columns.Add("timeActivityCodeDesc");
                //Add a new record.
                //Set the created by and created date of the item.
                _timerecord.Columns["timeRecorded"].DefaultValue = DateTime.Now;
                _timerecord.Columns["feeusrID"].DefaultValue = Session.CurrentSession.CurrentFeeEarner.ID;
                _timerecord.Columns["CreatedBy"].DefaultValue = Session.CurrentSession.CurrentUser.ID;
                _timerecord.Columns["Created"].DefaultValue = DateTime.Now;
                _timerecord.Columns["timeFormat"].DefaultValue = 0;
                _timerecord.Columns["timeBilled"].DefaultValue = false;
                _timerecord.Columns["timeStatus"].DefaultValue = 0;
                _timerecord.Columns["timeTransferred"].DefaultValue = false;
                _timerecord.Columns["ID"].AutoIncrement = true;
                Global.CreateBlankRecord(ref _timerecord, true);
                _data = _timerecord.Rows[0];

                this.OnExtLoaded();
            }
        }

        /// <summary>
        /// Create a new instance of a TimeRecord
        /// </summary>
        /// <param name="file">The file to link the Time Recording to.</param>
        public TimeRecord(OMSFile file)
            : this(file, false, DBNull.Value, null, -1)
        {
            new FileActivity(file, FileStatusActivityType.TimeEntry).Check();
        }

        /// <summary>
        /// Creates a new instance of a TimeRecord.
        /// </summary>
        /// <param name="file">The file to link the Time Recording to.</param>
        /// <param name="BatchMode">When the Update is fired.</param>
        /// <param name="lastdate"></param>
        /// <param name="lastfee"></param>
        /// <param name="lastlegalaid"></param>
        public TimeRecord(OMSFile file, bool BatchMode, FWBS.Common.DateTimeNULL lastdate, FWBS.OMS.FeeEarner lastfee, int lastlegalaid)
            : this()
        {
            new FileActivity(file, FileStatusActivityType.TimeEntry).Check();
            
            _batchmode = BatchMode;
            _file = file;
            SetExtraInfo("fileid", _file.ID);
            SetExtraInfo("clID", _file.ClientID);

            if (_file.FundingType.LegalAidCharged == true)
            {
                if (lastlegalaid == -1)
                    SetExtraInfo("timeLegalAidCat", _file.LACategory);
                else
                    SetExtraInfo("timeLegalAidCat", lastlegalaid);
            }

            if (lastfee == null)
            {
                if (Session.CurrentSession.CurrentUser.WorksForMatterHandler)
                    this.FeeEarnerID = file.PrincipleFeeEarnerID;
                else
                    this.FeeEarnerID = Session.CurrentSession.CurrentFeeEarner.ID;
            }
            else
                this.FeeEarnerID = lastfee.ID;

            int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);
            if (_file.FundingType.LegalAidCharged == true)
                _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
            else
                _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
            if (lastdate != DBNull.Value) this.TimeDate = lastdate;
            RefreshCostCharge();
            IsDirty = false;
        }


        /// <summary>
        /// Creates a new instance of an TimeRecord.
        /// </summary>
        /// <param name="doc">The Document to link the Time Recording to</param>
        /// <param name="BatchMode"></param>
        public TimeRecord(OMSDocument doc, bool BatchMode)
            : this()
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            _batchmode = BatchMode;
            _doc = doc;
            _file = doc.OMSFile;
            SetExtraInfo("fileid", doc.OMSFileID);
            SetExtraInfo("clID", doc.OMSFile.ClientID);
            SetExtraInfo("assocID", doc.AssocID);

            if (_file.FundingType.LegalAidCharged == true)
                SetExtraInfo("timeLegalAidCat", _file.LACategory);

            TimeActualMins = doc.EditingTime;
            TimeMins = TimeActualMins;

            if (doc.LastPrecedent != null)
            {
                if (doc.LastPrecedent.TimeRecordingUnits != 0)
                {
                    TimeUnits = doc.LastPrecedent.TimeRecordingUnits;
                    SetExtraInfo("timeFormat", 1);
                }
            }
            else
            {
                if (doc.BasePrecedent != null)
                {
                    if (doc.BasePrecedent.TimeRecordingUnits != 0)
                    {
                        TimeUnits = doc.BasePrecedent.TimeRecordingUnits;
                        SetExtraInfo("timeFormat", 1);
                    }
                }
            }

            if (TimeUnits != 0 && TimeMins == 0)
            {
                //
                // Calulates the Minutes from the Units
                //
                TimeMins = Convert.ToInt32(TimeUnits * Session.CurrentSession.TimeUnitValue); // 2 * 6
            }
            else if (TimeMins != 0 && TimeUnits == 0)
            {
                //
                // Calulates the Units from the Minutes
                //
                int unitvalue = 0;
                if ((_doc.EditingTime / Session.CurrentSession.TimeUnitValue) >= 1)
                {
                    int roundvalue;
                    System.Math.DivRem(_doc.EditingTime, Session.CurrentSession.TimeUnitValue, out roundvalue);
                    unitvalue = Convert.ToInt32(_doc.EditingTime / Session.CurrentSession.TimeUnitValue);

                    if (roundvalue != 0)
                        unitvalue++;
                }
                else
                {
                    unitvalue = 1;
                }
                TimeUnits = unitvalue;
            }

            if ((doc.LastPrecedent != null) && (doc.LastPrecedent.GetTimeActivityCode(doc.Direction) != ""))
            {
                // This must be set before the FeeEarnerID is set
                SetExtraInfo("timeActivityCode", doc.LastPrecedent.GetTimeActivityCode(doc.Direction));
                SetExtraInfo("timeFormat", 1);
            }

            if (Convert.ToString(this.ActivityCode) == "")
            {
                if (doc.BasePrecedent != null)
                {
                    SetExtraInfo("timeActivityCode", doc.BasePrecedent.GetTimeActivityCode(doc.Direction));
                    SetExtraInfo("timeFormat", 1);
                }
            }

            if (Session.CurrentSession.CurrentUser.WorksForMatterHandler)
                this.FeeEarnerID = _file.PrincipleFeeEarnerID;

            int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);
            try
            {
                if (_file.FundingType.LegalAidCharged == true)
                    _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                else
                    _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
            }
            catch { }
            if (_activity == null)
                _activity = new Activities(this.OMSFileID, fid, true);

            RefreshCostCharge();

            if ((doc.LastPrecedent != null) && (doc.LastPrecedent.TimeRecordingDescription != ""))
            {
                this.TimeDescription = doc.LastPrecedent.TimeRecordingDescription;
                SetExtraInfo("timeFormat", 1);
            }
            else
            {
                if (doc.BasePrecedent != null)
                {
                    this.TimeDescription = doc.BasePrecedent.TimeRecordingDescription;
                    SetExtraInfo("timeFormat", 1);
                }
            }

            if (this.TimeDescription == "") this.TimeDescription = doc.Description;


            IsDirty = false;

        }



        // *************************************************************************************************
        // *************************************************************************************************
        // LOADING CONSTRUCTORS
        // *************************************************************************************************
        // *************************************************************************************************

        /// <summary>
        /// Accepts an Time Record data row schema.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Index"></param>
        internal TimeRecord(TimeCollection Parent, int Index)
            : this()
        {
            _batchmode = true;
            _parentinx = Index;
            _parent = Parent;
            IsDirty = false;
        }

        /// <summary>
        /// Initialised an existing TimeRecord object with the specified identifier.
        /// </summary>
        /// <param name="id">TimeID identifier.</param>
        /// <param name="BatchMode"></param>
        internal TimeRecord(long id, bool BatchMode)
            : this()
        {
            Fetch(id, BatchMode, null);

        }

        // *************************************************************************************************
        // *************************************************************************************************
        // QUICK CREATE CONSTRUCTORS
        // *************************************************************************************************
        // *************************************************************************************************

        /// <summary>
        /// Creates a new timerecord object using an OMSFile.
        /// </summary>
        /// <param name="obj">The omsfile or associate object or that is being assigned to the newly created Time Record.</param>
        /// <param name="description">The friendly description of the Time Record.</param>
        /// <param name="timeactivitycode">Time Activity Code</param>
        /// <param name="timecharge">Time Charge Value</param>
        /// <param name="timecost">Time Cost Value</param>
        /// <param name="timemins">Time Mins</param>
        /// <param name="timeunits">Time Units</param>
        /// <param name="feeearner">Fee Earner Object for this time record</param>
        /// <param name="documentID">The Document ID</param>
        internal TimeRecord(object obj, string description, string timeactivitycode, int timemins, int timeunits, decimal timecharge, decimal timecost, FeeEarner feeearner, long documentID)
            : this()
        {
            Global.CreateBlankRecord(ref _timerecord, true);
            if (obj is Associate)
            {
                _file = (obj as Associate).OMSFile;
            }
            else if (obj is OMSFile)
            {
                _file = obj as OMSFile;
            }
            else
            {
                throw new OMSException2("12003", "The TimeRecord can only be created using a OMSFile or associate object.");
            }

            //Client and Matter Status activity check
            if (_file != null)
                new FileActivity(_file, FileStatusActivityType.TimeEntry).Check();

            if (feeearner == null)
            {
                // If not set or passed through as a parameter then try using the Principle Fee Earner
                feeearner = _file.PrincipleFeeEarner;
            }

            SetExtraInfo("clid", _file.ClientID);
            SetExtraInfo("fileid", _file.ID);

            if (feeearner != null) // Fee Earner is not null specify
                SetExtraInfo("feeusrid", feeearner.ID);

            if (_file.FundingType.LegalAidCharged == true)
                SetExtraInfo("timeLegalAidCat", _file.LACategory);

            SetExtraInfo("timedesc", description);
            SetExtraInfo("timeactivitycode", timeactivitycode);
            // Change to be the Object description
            _timedesc = timeactivitycode;

            SetExtraInfo("timemins", timemins);
            SetExtraInfo("timeunits", timeunits);
            SetExtraInfo("timecharge", timecharge);
            SetExtraInfo("timecost", timecost);
            IsDirty = false;

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
                    switch (_timerecord.Rows[0].RowState)
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

        public TimeCollection TimeParent
        {
            get
            {
                return _parent;
            }
        }

        public int Index
        {
            get
            {
                return _parentinx;
            }
        }

        public FeeEarner FeeEarner
        {
            get
            {
                int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);
                if (fid != 0)
                {
                    if (_feeearner == null)
                        _feeearner = FeeEarner.GetFeeEarner(fid);
                    return _feeearner;
                }
                else
                    return null;
            }
        }

        [EnquiryUsage(true)]
        public string FundTypeDescription
        {
            get
            {
                return this.OMSFile.FundingType.Description;
            }
        }

        [EnquiryUsage(true)]
        public object FeeEarnerID
        {
            get
            {
                if (Session.CurrentSession.IsLoggedIn == false) return null;
                return GetExtraInfo("feeusrID");
            }
            set
            {
                if (Session.CurrentSession.IsLoggedIn == false) return;



                if (this.OMSFile == null)
                    throw new OMSException2("12002", "Cannot create Time without a File");


                if (FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("feeusrID"), 0) != FWBS.Common.ConvertDef.ToInt32(value, 0))
                {
                    decimal oldcharge = this.TimeCharge;
                    decimal oldcost = this.TimeCost;
                    int fid = FWBS.Common.ConvertDef.ToInt32(value, 0);
                    _feeearner = null;
                    SetExtraInfo("feeusrID", fid);
                    if (_activity == null || (_activity != null && _activity.FeeEarnerID != fid) || (_activity != null && _activity.FileID != this.OMSFileID))
                    {
                        if (_file.FundingType.LegalAidCharged == true)
                            _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                        else
                            _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                    }
                    RefreshCostCharge();

                    // Moved inside the Bracket IF as no code needed to be fired
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeCharge", oldcharge, this.TimeCharge));
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeCost", oldcost, this.TimeCost));
                }
            }
        }

        /// <summary>
        /// Returns the Activities for the Time Record
        /// </summary>
        public FWBS.OMS.Activities Activities
        {
            get
            {
                int fid = FWBS.Common.ConvertDef.ToInt32(FeeEarnerID, 0);
                if (_activity == null || (_activity != null && _activity.FeeEarnerID != fid) || (_activity != null && _activity.FileID != this.OMSFileID))
                {
                    if (_file.FundingType.LegalAidCharged == true)
                        _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                    else
                        _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                }
                return _activity;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Time Record object is new and needs to be 
        /// updated to exist in the database.
        /// </summary>
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
        /// The unique identifier number of the TimeRecord.
        /// </summary>
        public long ID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("ID"));
            }
        }

        public long DocumentID
        {
            get
            {
                return Convert.ToInt64(GetExtraInfo("docID"));
            }
            internal set
            {
                SetExtraInfo("docID", value);
            }
        }

        [EnquiryUsage(true)]
        public int LegalAidCategory
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("timeLegalAidCat"), -1);
            }
            set
            {

                if (this.OMSFile == null)
                    throw new OMSException2("12002", "Cannot create Time without a File");

                int oldval = LegalAidCategory;

                decimal oldcharge = TimeCharge;
                decimal oldcost = TimeCost;

                if (oldval != value)
                {
                    SetExtraInfo("timeLegalAidCat", value);
                    if (_file.FundingType.LegalAidCharged == true)
                        _activity = new Activities(this.OMSFileID, Convert.ToInt32(this.FeeEarnerID), true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                    else
                        _activity = new Activities(this.OMSFileID, Convert.ToInt32(this.FeeEarnerID), true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                    
                    RefreshCostCharge();
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("LegalAidCategory", oldval, value));

                }
                OnPropertyChanged(new PropertyChangedEventArgs("TimeCharge", oldcharge, this.TimeCharge));
                OnPropertyChanged(new PropertyChangedEventArgs("TimeCost", oldcost, this.TimeCost));

            }
        }

        [EnquiryUsage(true)]
        public int TimeUnits
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("timeUnits"), 0);
            }
            set
            {
                int oldval = TimeUnits;
                if (oldval != value)
                {
                    SetExtraInfo("timeUnits", value);
                    TimeMins = Convert.ToInt32(TimeUnits * Session.CurrentSession.TimeUnitValue); // 2 * 6
                    RefreshCostCharge();
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeUnits", oldval, value));

                }
            }
        }

        public TimeStatuses TimeStatus
        {
            get
            {
                return (TimeStatuses)FWBS.Common.ConvertDef.ToEnum(GetExtraInfo("timeStatus"), TimeStatuses.Unbilled);
            }
            set
            {
                TimeStatuses oldval = TimeStatus;
                if (oldval != TimeStatus)
                {
                    SetExtraInfo("timeStatus", Convert.ToInt64(value));
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeStatus", oldval, value));

                }
            }
        }

        [EnquiryUsage(true)]
        public int TimeMins
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("timeMins"), 0);
            }
            set
            {
                int oldval = TimeMins;
                if (oldval != value)
                {
                    SetExtraInfo("timeMins", value);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeMins", oldval, value));

                }
            }
        }

        [EnquiryUsage(true)]
        public int TimeActualMins
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("timeActualMins"), 0);
            }
            set
            {
                int oldval = TimeActualMins;
                if (oldval != value)
                {
                    SetExtraInfo("timeActualMins", value);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeActualMins", oldval, value));

                }
            }
        }

        [EnquiryUsage(true)]
        public FWBS.Common.DateTimeNULL TimeDate
        {
            get
            {
                return FWBS.Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("timeRecorded"), DBNull.Value);
            }
            set
            {
                Common.DateTimeNULL oldval = TimeDate;
                if (oldval != value)
                {
                    // MNW CODE ADDED FOR NEW sprTimeActivities to allow for date range.
                    if (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate)
                    {
                        decimal oldcharge = this.TimeCharge;
                        decimal oldcost = this.TimeCost;
                        int fid = Convert.ToInt32(this.FeeEarnerID);
                        if (_activity == null || (_activity != null && this.TimeDate != value.DateTime) || (_activity != null && _activity.FileID != this.OMSFileID))
                        {
                            if (_file.FundingType.LegalAidCharged == true)
                                _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                            else
                                _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? value : new Common.DateTimeNULL()));
                        }
                        RefreshCostCharge();

                        OnPropertyChanged(new PropertyChangedEventArgs("TimeCharge", oldcharge, this.TimeCharge));
                        OnPropertyChanged(new PropertyChangedEventArgs("TimeCost", oldcost, this.TimeCost));

                    }
                    // MNW CODE CHANGE END


                    SetExtraInfo("timeRecorded", value.DateTime);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeDate", oldval, value));
                }
            }
        }

        [EnquiryUsage(true)]
        public decimal TimeCharge
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(GetExtraInfo("timeCharge"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Decimal oldval = TimeCharge;
                if (oldval != value)
                {
                    SetExtraInfo("timeCharge", value);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeCharge", oldval, value));
                }
            }
        }

        [EnquiryUsage(true)]
        public decimal TimeActualCost
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(GetExtraInfo("timeActualCost"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Decimal oldval = TimeActualCost;
                if (oldval != value)
                {
                    SetExtraInfo("timeActualCost", value);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("timeActualCost", oldval, value));
                }
            }
        }

        [EnquiryUsage(true)]
        public decimal TimeCost
        {
            get
            {
                try
                {
                    return Convert.ToDecimal(GetExtraInfo("timeCost"));
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                Decimal oldval = TimeCost;
                if (oldval != value)
                {
                    SetExtraInfo("timeCost", value);
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("TimeCost", oldval, value));
                }
            }
        }


        /// <summary>
        /// Gets the user object that created this TimeRecord.
        /// </summary>
        public User CreatedBy
        {
            get
            {
                if (GetExtraInfo("CreatedBy") is System.DBNull)
                    return null;
                else
                    return new User((int)GetExtraInfo("CreatedBy"));

            }
        }

        [EnquiryUsage(true)]
        public string TimeDescription
        {
            get
            {
                return Convert.ToString(GetExtraInfo("timeDesc"));
            }
            set
            {
                string oldval = TimeDescription;
                if (oldval != value)
                {
                    IsDirty = true;

                    if (String.IsNullOrEmpty(value))
                        SetExtraInfo("timeDesc", DBNull.Value);
                    else
                    {
                        if (value.Length > 150)
                            value = value.Substring(0, 150);

                        SetExtraInfo("timeDesc", value);
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs("TimeDescription", oldval, TimeDescription));

                }
            }
        }

        /// <summary>
        /// Gets the modification dates and users.
        /// </summary>
        [EnquiryUsage(true)]
        public ModificationData TrackingStamp
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
        /// Gets or Sets the TimeRecord Description , if blank on will return the Description from Activity Code.
        /// </summary>
        public string ActivityDescription
        {
            get
            {
                return _timedesc;
            }
        }

        /// <summary>
        /// Gets or Sets the Activity Code.
        /// </summary>
        [EnquiryUsage(true)]
        public object ActivityCode
        {
            get
            {
                return GetExtraInfo("timeActivityCode");
            }
            set
            {
                object oldval = ActivityCode;
                if (oldval != value)
                {
                    SetExtraInfo("timeActivityCode", value);

                    DataView dv = new DataView(Activities.GetDataTable());
                    dv.RowFilter = "actCode = '" + value + "'";
                    if (dv.Count > 0)
                        SetExtraInfo("timeActivityCodeDesc", dv[0]["actCodeDesc"]);

                    RefreshCostCharge();
                    IsDirty = true;
                    OnPropertyChanged(new PropertyChangedEventArgs("timeActivityCode", oldval, value));
                }
            }
        }

        /// <summary>
        /// Gets or Sets the file object based from this occurence of the timerecord.
        /// </summary>
        public OMSFile OMSFile
        {
            get
            {
                if (_file == null && this.OMSFileID != 0)
                    _file = OMSFile.GetFile((long)GetExtraInfo("FILEID"));

                return _file;
            }
            internal set
            {
                if (_file != value)
                {
                    _file = value;
                    OMSFileID = _file.ID;
                    IsDirty = true;
                    int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);

                    if (_activity == null || (_activity != null && _activity.FeeEarnerID != fid) || (_activity != null && _activity.FileID != this.OMSFileID))
                    {
                        if (_file.FundingType.LegalAidCharged == true)
                        {
                            var obj = GetExtraInfo("timeLegalAidCat");
                            if (obj == DBNull.Value)
                                throw new OMSException2("ERRNOLEGAIDCAT", "No Legal Aid category defined for current Time Record.");

                            _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(obj), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                        }
                        else
                            _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                    }
                }
            }
        }


        /// <summary>
        /// Gets the file id that is associated to this TimeRecord
        /// </summary>
        internal long OMSFileID
        {
            get
            {
                return FWBS.Common.ConvertDef.ToInt64(GetExtraInfo("fileid"), 0);
            }
            set
            {
                IsDirty = true;
                SetExtraInfo("fileid", value);
            }
        }


        #endregion

        #region IExtraInfo Implementation

        /// <summary>
        /// Sets the raw internal data object with the specified value under the specified field name.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <param name="val">Value to use.</param>
        public void SetExtraInfo(string fieldName, object val)
        {
            if (Session.CurrentSession.IsLoggedIn == false) return;

            this.SetExtraInfo(_data, fieldName, val);
        }


        /// <summary>
        /// Returns a raw value from the internal data object, specified by the database field name given.
        /// </summary>
        /// <param name="fieldName">Database Field Name.</param>
        /// <returns>The current data value.</returns>
        public object GetExtraInfo(string fieldName)
        {
            if (Session.CurrentSession.IsLoggedIn == false) return null;

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
        public Type GetExtraInfoType(string fieldName)
        {
            try
            {
                return _data.Table.Columns[fieldName].DataType;
            }
            catch
            {
                throw new OMSException2("12001", "Error Getting Extra Info Field %1% Probably Not Initialized", new Exception(), true, fieldName);
            }
        }

        /// <summary>
        /// Returns a dataset representation of the object.
        /// </summary>
        /// <returns>Dataset object.</returns>
        public DataSet GetDataset()
        {
            return _timerecord.DataSet;
        }


        /// <summary>
        /// Returns a data table representation of the object.  The data table is copied
        /// so that it can be added to another dataset without confusion of an existing dataset.
        /// </summary>
        /// <returns>DataTable object.</returns>
        public DataTable GetDataTable()
        {
            return _timerecord.Copy();
        }

        /// <summary>
        /// Returns the Live Data Row so it can be compared by the Time Collection
        /// </summary>
        /// <returns></returns>
        internal DataRow GetDataRow()
        {
            return _data;
        }
        #endregion

        #region IUpdateable Implementation

        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        public virtual void Update()
        {
            new FileActivity(OMSFile, FileStatusActivityType.TimeEntry).Check();

            ObjectState state = State;
            if (this.OnExtCreatingUpdatingOrDeleting(state))
                return;

            if (IsDirty)
            {
                if (this.TimeUnits == 0)
                    throw new OMSException2("2001", "You must enter a Unit Value other than (Zero)", "TimeUnits");
                if (this.FeeEarner == null)
                    throw new OMSException2("2002", "You must select a Fee Earner", "FeeEarnerID");
                if (this.ActivityCode == DBNull.Value || this.ActivityCode == null || Convert.ToString(this.ActivityCode) == String.Empty)
                    throw new OMSException2("2003", "You must select a Activity Code", "ActivityCode");
                if (this.Activities.Exists(this.ActivityCode) == false)
                    throw new OMSException2("2004a", "Activity Code is not available for this Fee Earner. Please check the Fee Earner is correctly configured. e.g for Legal Aid", "ActivityCode");

                IsDirty = false;

                if (this.OMSFile.FundingType.LegalAidCharged)
                {
                    if (this.OMSFile.LACategory <= 0)
                    {
                        this.OMSFile.LACategory = (Int16)this.LegalAidCategory;
                        this.OMSFile.Update();
                    }
                }

                if (this.TimeActualMins == 0) this.TimeActualMins = this.TimeMins;
                if (this.TimeActualCost == 0) this.TimeActualCost = this.TimeCost;

                if (IsNew)
                {
                    //Set the currently used phase id.
                    try
                    {
                        if (_timerecord.Columns.Contains("phID"))
                            SetExtraInfo("phID", _file.GetExtraInfo("phID"));
                    }
                    catch { }
                }

                if (_batchmode == false)
                {
                    //Check if there are any changes made before setting the updated
                    //and updated by properties then update.
                    if (_timerecord.GetChanges() != null)
                    {
                        //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
                        if (_timerecord.PrimaryKey == null || _timerecord.PrimaryKey.Length == 0)
                            _timerecord.PrimaryKey = new DataColumn[1] { _timerecord.Columns["id"] };

                        SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
                        SetExtraInfo("Updated", DateTime.Now);
                        Session.CurrentSession.Connection.Update(_data, "dbTimeLedger");
                    }
                }

                this.OnExtCreatedUpdatedDeleted(state);
            }
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

            DataTable changes = _timerecord.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.ID, false, changes.Rows[0]);
            else
                Fetch(this.ID, false, null);

            this.OnExtRefreshed();
        }

        /// <summary>
        /// Added to force a time units calculation when only minutes is supplied AKA Mobile time logger
        /// </summary>
        public void CalculateUnitsFromTime()
        {
            if (TimeMins != 0 && TimeUnits == 0)
            {
                //
                // Calulates the Units from the Minutes
                //
                int unitvalue = 0;

                if ((TimeMins / Session.CurrentSession.TimeUnitValue) > 0)
                {
                    unitvalue = Convert.ToInt32(TimeMins / Session.CurrentSession.TimeUnitValue);
                    if (Math.Round(Convert.ToDouble(TimeMins / Session.CurrentSession.TimeUnitValue)) != 0)
                        unitvalue++;
                }
                else
                {
                    unitvalue = 1;
                }
                TimeUnits = unitvalue;
            }
        }



        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        public void Cancel()
        {
            OnCancelTime();
            this.IsDirty = false;
        }

        /// <summary>
        /// Gets or sets a boolean flag indicating whether any changes have been made to the object.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return _isdirty;
            }
            set
            {
                _isdirty = value;
                OnDirty();
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
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        /// <summary>
        /// Edits the current Time Record object in the form of an enquiry (if the database states that is edit compatible).
        /// </summary>
        /// <param name="param">Named parameter collection.</param>
        /// <returns>Enquiry object ready to be rendered.</returns>	
        public Enquiry Edit(Common.KeyValueCollection param)
        {
            Enquiry e = null;
            e = Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.TimeRecordEdit), param);
            return e;
        }

        /// <summary>
        /// Edits the current Time Record object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_file != null)
                {
                    _file.Dispose();
                    _file = null;
                }


                _data = null;
                if (_timerecord != null)
                {
                    _timerecord.Dispose();
                    _timerecord = null;
                }
            }
            //Dispose unmanaged objects.
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Returns TimeRecord object based on the specified time id.
        /// </summary>
        /// <param name="id">TimeRecord ID Parameter.</param>
        /// <returns>An TimeRecord object.</returns>
        public static TimeRecord GetTimeRecord(long id)
        {
            Session.CurrentSession.CheckLoggedIn();
            return new TimeRecord(id, false);
        }

        #endregion

        #region Methods

        private void Fetch(long id, bool BatchMode, DataRow merge)
        {
            _batchmode = BatchMode;
            //Make sure that the parameters list is cleared after use.	
            var data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where ID = @TIMEID", Table, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("TIMEID", System.Data.SqlDbType.BigInt, 0, id) });
            
            if ((data == null) || (data.Rows.Count == 0))
            {
                throw new OMSException2("12003", "Timerecord with TimeID : %1% Doesn't Exist", new Exception(), true, id.ToString());
            }

            if (!data.Columns.Contains("timeActivityCodeDesc"))
                data.Columns.Add("timeActivityCodeDesc");

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _timerecord = data;
            _data = _timerecord.Rows[0];
            int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);
            try
            {
                if (this.OMSFile.FundingType.LegalAidCharged == true)
                    _activity = new Activities(this.OMSFileID, fid, true, Convert.ToInt32(GetExtraInfo("timeLegalAidCat")), (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
                else
                    _activity = new Activities(this.OMSFileID, fid, true, -1, (FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate ? this.TimeDate : new Common.DateTimeNULL()));
            }
            catch { }
            if (_activity == null)
                _activity = new Activities(this.OMSFileID, fid, true);
            IsDirty = false;


        }

        /// <summary>
        /// Clones the current TimeRecord data into a given row by reference.
        /// </summary>
        /// <param name="row">The row to update.</param>
        internal void Clone(ref DataRow row)
        {
            foreach (DataColumn col in _timerecord.Columns)
            {
                if (col.AutoIncrement == false)
                    row[col.ColumnName] = _timerecord.Rows[0][col.ColumnName];
            }
        }

        internal void SetIndex(int Index)
        {
            _parentinx = Index;
        }

        internal void SetParent(TimeCollection Parent)
        {
            _parent = Parent;
        }

        internal void Import(DataRow row)
        {
            foreach (DataColumn col in _timerecord.Columns)
            {
                if (col.AutoIncrement == false)
                    _data[col.ColumnName] = row[col.ColumnName];
            }
        }


        private void RefreshCostCharge()
        {
            int fid = FWBS.Common.ConvertDef.ToInt32(this.FeeEarnerID, 0);
            if (_activity != null && this.OMSFileID != 0 && fid != 0)
            {
                if (_activity.Exists(this.ActivityCode))
                {
                    this.TimeCost = (_activity.Cost / 60) * (this.TimeMins);
                    this.TimeActualCost = (_activity.Cost / 60) * (this.TimeActualMins);
                    if (this.OMSFile.FundingType.Code == "NOCHG")
                    {
                        this.TimeCharge = 0;
                    }
                    else
                    {
                        if (_activity.ActivityStyle == ActivityStyles.FixedRateLegalAid)
                        {
                            if (this.TimeUnits > 0)
                                this.TimeCharge = _activity.Charge;
                            else
                                this.TimeCharge = (_activity.Charge * -1);
                        }
                        else if ((_activity.ActivityStyle == ActivityStyles.NotLegalAid) & (_activity.FixedValue > 0))
                        {
                            this.TimeCharge = Convert.ToDecimal((_activity.FixedValue) * (this.TimeUnits));
                        }
                        else
                        {
                            this.TimeCharge = Convert.ToDecimal((_activity.Charge / 60) * ((this.TimeUnits) * Session.CurrentSession.TimeUnitValue));
                        }
                    }
                }
            }
        }
        #endregion

        #region Static
        /// <summary>
        /// Creates a new timerecord object using an OMSFile.
        /// </summary>
        /// <param name="obj">The omsfile or associate object or that is being assigned to the newly created Time Record.</param>
        /// <param name="description">The friendly description of the Time Record.</param>
        /// <param name="timeactivitycode">Time Activity Code</param>
        /// <param name="timecharge">Time Charge Value</param>
        /// <param name="timecost">Time Cost Value</param>
        /// <param name="timemins">Time Mins</param>
        /// <param name="timeunits">Time Units</param>
        public static TimeRecord CreateTimeRecord(object obj, string description, string timeactivitycode, int timemins, int timeunits, decimal timecharge, decimal timecost)
        {
            TimeRecord tc = new TimeRecord(obj, description, timeactivitycode, timemins, timeunits, timecharge, timecost, null, 0);
            tc.Update();
            tc.Refresh();
            return tc;
        }

        /// <summary>
        /// Added DMB 23/6/2004 Overload to be used by web service relies on calling CalculateUnitsFromTime before saving
        /// </summary>
        /// <param name="obj">file or associate</param>
        /// <param name="description">The friendly description of the Time Record.</param>
        /// <param name="timeactivitycode">Time Activity Code</param>
        /// <param name="timemins">Time Mins</param>
        /// <returns></returns>
        public static TimeRecord CreateTimeRecord(object obj, string description, string timeactivitycode, int timemins)
        {
            TimeRecord tc = new TimeRecord(obj, description, timeactivitycode, timemins, 0, 0, 0, null, 0);

            tc.CalculateUnitsFromTime();

            tc.Update();
            tc.Refresh();
            return tc;
        }


        /// <summary>
        /// Creates a new timerecord object using an OMSFile.
        /// </summary>
        /// <param name="obj">The omsfile or associate object or that is being assigned to the newly created Time Record.</param>
        /// <param name="description">The friendly description of the Time Record.</param>
        /// <param name="timeactivitycode">Time Activity Code</param>
        /// <param name="timecharge">Time Charge Value</param>
        /// <param name="timecost">Time Cost Value</param>
        /// <param name="timemins">Time Mins</param>
        /// <param name="timeunits">Time Units</param>
        /// <param name="feeearner">Fee Earner Object for this time record</param>
        public static TimeRecord CreateTimeRecord(object obj, string description, string timeactivitycode, int timemins, int timeunits, decimal timecharge, decimal timecost, FeeEarner feeearner)
        {

            TimeRecord tc = new TimeRecord(obj, description, timeactivitycode, timemins, timeunits, timecharge, timecost, feeearner, 0);
            tc.Update();
            tc.Refresh();
            return tc;
        }
        #endregion
    }
}
