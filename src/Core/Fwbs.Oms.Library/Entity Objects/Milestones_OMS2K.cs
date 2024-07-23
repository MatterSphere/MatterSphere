using System;
using System.ComponentModel;
using System.Data;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS
{
    /// <summary>
    /// Summary description for Milestones_OMS2K.
    /// </summary>

    public class Milestones_OMS2K : CommonObject
	{
		#region Fields

		private OMSFile file;
        private bool[] ticked = new bool[MAX_STAGES];
		private int finalstage = 0;

		private static DataTable msconfigdata = null;
		
        private bool calculating = false;
        private bool loading = false;
        private bool updating = false;

        private const int MAX_STAGES = 30;

		#endregion
		
        #region Constructors

        /// <summary>
        /// Constructs a milestone object using a file id.
        /// </summary>
        /// <param name="fileId">The file id to construct a milestone plan against.</param>
        [EnquiryUsage(true)]
        public Milestones_OMS2K(long fileId)
        {
            file = OMSFile.GetFile(fileId);
            InternalFetch();
        }
        
        /// <summary>
        /// Constructs a milestone plan against a specified file.
        /// </summary>
        /// <param name="file"></param>
        public Milestones_OMS2K(OMSFile file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            this.file = file;
            InternalFetch();
        }

        private void InternalFetch()
        {
            if (base.Exists(file.ID) && file.ID != 0)
                base.Fetch(file.ID);
            else
                Create();

            if (!file.ID.Equals(UniqueID))
                UniqueID = file.ID;
        }

        #endregion

        #region CommonObject       

        public override void Cancel()
        {
            base.Cancel();
            InternalFetch();
        }
        /// <summary>
        /// Updates the object and persists it to the database.
        /// </summary>
        public override void Update()
        {

            

            try
            {
                new FilePermission(this.OMSFile, StandardPermissionType.Update).Check();
                new SystemPermission(StandardPermissionType.UpdateFile).Check();

                updating = true;

                if (!IsClear && IsDirty)
                {
                    if (_data.Rows[0].RowState != DataRowState.Deleted)
                    {
                        if (string.IsNullOrWhiteSpace(MSPlan))
                            return;

                        object nextstagedate = null;
                        int nextstage = 0;

                        int? lastdays = null;

                        for (int s = 1; s <= Stages; s++)
                        {
                            DateTime? achieved = GetStageAchieved(s);

                            if (achieved == null)
                            {
                                int? days = GetStageCalcDays(s);

                                if (days.HasValue)
                                {


                                    if (days < lastdays || lastdays == null)
                                    {
                                        lastdays = days.Value;

                                        DateTime? due = GetStageDue(s);
                                        if (due == null)
                                            nextstagedate = DBNull.Value;
                                        else
                                            nextstagedate = due.Value;

                                        nextstage = s;
                                    }
                                }
                            }
                        }

                        if (nextstage > 0)
                        {
                            SetExtraInfo("MSNEXTDUEDATE", nextstagedate);
                            SetExtraInfo("MSNEXTDUESTAGE", nextstage);
                        }
                        else
                        {
                            SetExtraInfo("MSNEXTDUEDATE", DBNull.Value);
                            SetExtraInfo("MSNEXTDUESTAGE", DBNull.Value);
                        }


                        if (!file.ID.Equals(UniqueID))
                            UniqueID = file.ID;

                        base.Update();
                        Fetch(UniqueID);
                    }
                    else
                        base.Update();
                }
            }
            finally
            {
                updating = false;
            }

        }

        /// <summary>
        /// The default edit enquiry form.
        /// </summary>
        protected override string DefaultForm
        {
            get
            {
                return Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.Milestones);
            }
        }
        /// <summary>
        /// The primary key field.
        /// </summary>
        public override string FieldPrimaryKey
        {
            get
            {
                return "FILEID";
            }
        }
        /// <summary>
        /// The primary database table of the object.
        /// </summary>
        protected override string PrimaryTableName
        {
            get
            {
                return "DBMSDATA_OMS2K";
            }
        }

        protected override bool RefreshOnUpdate
        {
            get
            {
                return false;
            }
        }
        protected override string DatabaseTableName
        {
            get
            {
                return "DBMSDATA_OMS2K";
            }
        }

        /// <summary>
        /// The select SQL statement of the object.
        /// </summary>
        protected override string SelectStatement
        {
            get
            {
                return @"SELECT     
					  dbMSData_OMS2K.fileID, dbMSData_OMS2K.MSCode, dbMSData_OMS2K.MSNextDueDate, dbMSData_OMS2K.MSNextDueStage, 
                      dbMSData_OMS2K.MSStage1Due, dbMSData_OMS2K.MSStage2Due, dbMSData_OMS2K.MSStage3Due, dbMSData_OMS2K.MSStage4Due, 
                      dbMSData_OMS2K.MSStage5Due, dbMSData_OMS2K.MSStage6Due, dbMSData_OMS2K.MSStage7Due, dbMSData_OMS2K.MSStage8Due, 
                      dbMSData_OMS2K.MSStage9Due, dbMSData_OMS2K.MSStage10Due, dbMSData_OMS2K.MSStage11Due, dbMSData_OMS2K.MSStage12Due, 
                      dbMSData_OMS2K.MSStage13Due, dbMSData_OMS2K.MSStage14Due, dbMSData_OMS2K.MSStage15Due, dbMSData_OMS2K.MSStage16Due, 
                      dbMSData_OMS2K.MSStage17Due, dbMSData_OMS2K.MSStage18Due, dbMSData_OMS2K.MSStage19Due, dbMSData_OMS2K.MSStage20Due, 
					  dbMSData_OMS2K.MSStage21Due, dbMSData_OMS2K.MSStage22Due, dbMSData_OMS2K.MSStage23Due, dbMSData_OMS2K.MSStage24Due, 
					  dbMSData_OMS2K.MSStage25Due, dbMSData_OMS2K.MSStage26Due, dbMSData_OMS2K.MSStage27Due, dbMSData_OMS2K.MSStage28Due, 
					  dbMSData_OMS2K.MSStage29Due, dbMSData_OMS2K.MSStage30Due, 
                      dbMSData_OMS2K.MSStage1Achieved, dbMSData_OMS2K.MSStage2Achieved, dbMSData_OMS2K.MSStage3Achieved, 
                      dbMSData_OMS2K.MSStage4Achieved, dbMSData_OMS2K.MSStage5Achieved, dbMSData_OMS2K.MSStage6Achieved, 
                      dbMSData_OMS2K.MSStage7Achieved, dbMSData_OMS2K.MSStage8Achieved, dbMSData_OMS2K.MSStage9Achieved, 
                      dbMSData_OMS2K.MSStage10Achieved, dbMSData_OMS2K.MSStage11Achieved, dbMSData_OMS2K.MSStage12Achieved, 
                      dbMSData_OMS2K.MSStage13Achieved, dbMSData_OMS2K.MSStage14Achieved, dbMSData_OMS2K.MSStage15Achieved, 
                      dbMSData_OMS2K.MSStage16Achieved, dbMSData_OMS2K.MSStage17Achieved, dbMSData_OMS2K.MSStage18Achieved, 
                      dbMSData_OMS2K.MSStage19Achieved, dbMSData_OMS2K.MSStage20Achieved, 
					  
					  dbMSData_OMS2K.MSStage21Achieved, dbMSData_OMS2K.MSStage22Achieved, dbMSData_OMS2K.MSStage23Achieved, dbMSData_OMS2K.MSStage24Achieved, 
					  dbMSData_OMS2K.MSStage25Achieved, dbMSData_OMS2K.MSStage26Achieved, dbMSData_OMS2K.MSStage27Achieved, dbMSData_OMS2K.MSStage28Achieved, 
					  dbMSData_OMS2K.MSStage29Achieved, dbMSData_OMS2K.MSStage30Achieved, 
					  
					  dbMSData_OMS2K.MSStage1AchievedBy, 
                      dbMSData_OMS2K.MSStage2AchievedBy, dbMSData_OMS2K.MSStage3AchievedBy, dbMSData_OMS2K.MSStage4AchievedBy, 
                      dbMSData_OMS2K.MSStage5AchievedBy, dbMSData_OMS2K.MSStage6AchievedBy, dbMSData_OMS2K.MSStage7AchievedBy, 
                      dbMSData_OMS2K.MSStage8AchievedBy, dbMSData_OMS2K.MSStage9AchievedBy, dbMSData_OMS2K.MSStage10AchievedBy, 
                      dbMSData_OMS2K.MSStage11AchievedBy, dbMSData_OMS2K.MSStage12AchievedBy, dbMSData_OMS2K.MSStage13AchievedBy, 
                      dbMSData_OMS2K.MSStage14AchievedBy, dbMSData_OMS2K.MSStage15AchievedBy, dbMSData_OMS2K.MSStage16AchievedBy, 
                      dbMSData_OMS2K.MSStage17AchievedBy, dbMSData_OMS2K.MSStage18AchievedBy, dbMSData_OMS2K.MSStage19AchievedBy, 
                      dbMSData_OMS2K.MSStage20AchievedBy, 
					  
					  dbMSData_OMS2K.MSStage21AchievedBy, dbMSData_OMS2K.MSStage22AchievedBy, dbMSData_OMS2K.MSStage23AchievedBy, dbMSData_OMS2K.MSStage24AchievedBy, 
					  dbMSData_OMS2K.MSStage25AchievedBy, dbMSData_OMS2K.MSStage26AchievedBy, dbMSData_OMS2K.MSStage27AchievedBy, dbMSData_OMS2K.MSStage28AchievedBy, 
					  dbMSData_OMS2K.MSStage29AchievedBy, dbMSData_OMS2K.MSStage30AchievedBy, 
					  
					  dbMSData_OMS2K.MSActive, dbMSData_OMS2K.MSCreated, dbMSConfig_OMS2K.MSCode, 
                      dbMSConfig_OMS2K.MSDescription, dbMSConfig_OMS2K.MSDept, dbMSConfig_OMS2K.MSAll, dbMSConfig_OMS2K.MSStage1Desc, 
                      dbMSConfig_OMS2K.MSStage2Desc, dbMSConfig_OMS2K.MSStage3Desc, dbMSConfig_OMS2K.MSStage4Desc, dbMSConfig_OMS2K.MSStage5Desc, 
                      dbMSConfig_OMS2K.MSStage6Desc, dbMSConfig_OMS2K.MSStage7Desc, dbMSConfig_OMS2K.MSStage8Desc, dbMSConfig_OMS2K.MSStage9Desc, 
                      dbMSConfig_OMS2K.MSStage10Desc, dbMSConfig_OMS2K.MSStage11Desc, dbMSConfig_OMS2K.MSStage12Desc, 
                      dbMSConfig_OMS2K.MSStage13Desc, dbMSConfig_OMS2K.MSStage14Desc, dbMSConfig_OMS2K.MSStage15Desc, 
                      dbMSConfig_OMS2K.MSStage16Desc, dbMSConfig_OMS2K.MSStage17Desc, dbMSConfig_OMS2K.MSStage18Desc, 
                      dbMSConfig_OMS2K.MSStage19Desc, dbMSConfig_OMS2K.MSStage20Desc, 
					  
					  dbMSConfig_OMS2K.MSStage21Desc, dbMSConfig_OMS2K.MSStage22Desc, dbMSConfig_OMS2K.MSStage23Desc, dbMSConfig_OMS2K.MSStage24Desc, 
					  dbMSConfig_OMS2K.MSStage25Desc, dbMSConfig_OMS2K.MSStage26Desc, dbMSConfig_OMS2K.MSStage27Desc, dbMSConfig_OMS2K.MSStage28Desc, 
					  dbMSConfig_OMS2K.MSStage29Desc, dbMSConfig_OMS2K.MSStage30Desc, 
					  
					  dbMSConfig_OMS2K.MSStage1Days, 
                      dbMSConfig_OMS2K.MSStage2Days, dbMSConfig_OMS2K.MSStage3Days, dbMSConfig_OMS2K.MSStage4Days, dbMSConfig_OMS2K.MSStage5Days, 
                      dbMSConfig_OMS2K.MSStage6Days, dbMSConfig_OMS2K.MSStage7Days, dbMSConfig_OMS2K.MSStage8Days, dbMSConfig_OMS2K.MSStage9Days, 
                      dbMSConfig_OMS2K.MSStage10Days, dbMSConfig_OMS2K.MSStage11Days, dbMSConfig_OMS2K.MSStage12Days, 
                      dbMSConfig_OMS2K.MSStage13Days, dbMSConfig_OMS2K.MSStage14Days, dbMSConfig_OMS2K.MSStage15Days, 
                      dbMSConfig_OMS2K.MSStage16Days, dbMSConfig_OMS2K.MSStage17Days, dbMSConfig_OMS2K.MSStage18Days, 
                      dbMSConfig_OMS2K.MSStage19Days, dbMSConfig_OMS2K.MSStage20Days, 
					  
					  dbMSConfig_OMS2K.MSStage21Days, dbMSConfig_OMS2K.MSStage22Days, dbMSConfig_OMS2K.MSStage23Days, dbMSConfig_OMS2K.MSStage24Days, 
					  dbMSConfig_OMS2K.MSStage25Days, dbMSConfig_OMS2K.MSStage26Days, dbMSConfig_OMS2K.MSStage27Days, dbMSConfig_OMS2K.MSStage28Days, 
					  dbMSConfig_OMS2K.MSStage29Days, dbMSConfig_OMS2K.MSStage30Days, 

					  dbMSConfig_OMS2K.MSStage1LongDesc, 
                      dbMSConfig_OMS2K.MSStage2LongDesc, dbMSConfig_OMS2K.MSStage3LongDesc, dbMSConfig_OMS2K.MSStage4LongDesc, 
                      dbMSConfig_OMS2K.MSStage5LongDesc, dbMSConfig_OMS2K.MSStage6LongDesc, dbMSConfig_OMS2K.MSStage7LongDesc, 
                      dbMSConfig_OMS2K.MSStage8LongDesc, dbMSConfig_OMS2K.MSStage9LongDesc, dbMSConfig_OMS2K.MSStage10LongDesc, 
                      dbMSConfig_OMS2K.MSStage11LongDesc, dbMSConfig_OMS2K.MSStage12LongDesc, dbMSConfig_OMS2K.MSStage13LongDesc, 
                      dbMSConfig_OMS2K.MSStage14LongDesc, dbMSConfig_OMS2K.MSStage15LongDesc, dbMSConfig_OMS2K.MSStage16LongDesc, 
                      dbMSConfig_OMS2K.MSStage17LongDesc, dbMSConfig_OMS2K.MSStage18LongDesc, dbMSConfig_OMS2K.MSStage19LongDesc, 
                      dbMSConfig_OMS2K.MSStage20LongDesc, 
					  
					  dbMSConfig_OMS2K.MSStage21LongDesc, dbMSConfig_OMS2K.MSStage22LongDesc, dbMSConfig_OMS2K.MSStage23LongDesc, dbMSConfig_OMS2K.MSStage24LongDesc, 
					  dbMSConfig_OMS2K.MSStage25LongDesc, dbMSConfig_OMS2K.MSStage26LongDesc, dbMSConfig_OMS2K.MSStage27LongDesc, dbMSConfig_OMS2K.MSStage28LongDesc, 
					  dbMSConfig_OMS2K.MSStage29LongDesc, dbMSConfig_OMS2K.MSStage30LongDesc, 
					  
					  dbMSConfig_OMS2K.MSStage2Calcfrom, dbMSConfig_OMS2K.MSStage3Calcfrom, 
                      dbMSConfig_OMS2K.MSStage4Calcfrom, dbMSConfig_OMS2K.MSStage5Calcfrom, dbMSConfig_OMS2K.MSStage6Calcfrom, 
                      dbMSConfig_OMS2K.MSStage7Calcfrom, dbMSConfig_OMS2K.MSStage8Calcfrom, dbMSConfig_OMS2K.MSStage9Calcfrom, 
                      dbMSConfig_OMS2K.MSStage10Calcfrom, dbMSConfig_OMS2K.MSStage11Calcfrom, dbMSConfig_OMS2K.MSStage12Calcfrom, 
                      dbMSConfig_OMS2K.MSStage13Calcfrom, dbMSConfig_OMS2K.MSStage14Calcfrom, dbMSConfig_OMS2K.MSStage15Calcfrom, 
                      dbMSConfig_OMS2K.MSStage16Calcfrom, dbMSConfig_OMS2K.MSStage17Calcfrom, dbMSConfig_OMS2K.MSStage18Calcfrom, 
                      dbMSConfig_OMS2K.MSStage19Calcfrom, dbMSConfig_OMS2K.MSStage20Calcfrom, 
					  
					  dbMSConfig_OMS2K.MSStage21CalcFrom, dbMSConfig_OMS2K.MSStage22CalcFrom, dbMSConfig_OMS2K.MSStage23CalcFrom, dbMSConfig_OMS2K.MSStage24CalcFrom, 
					  dbMSConfig_OMS2K.MSStage25CalcFrom, dbMSConfig_OMS2K.MSStage26CalcFrom, dbMSConfig_OMS2K.MSStage27CalcFrom, dbMSConfig_OMS2K.MSStage28CalcFrom, 
					  dbMSConfig_OMS2K.MSStage29CalcFrom, dbMSConfig_OMS2K.MSStage30CalcFrom, 

					  dbMSConfig_OMS2K.MSStage1ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage2ReallyLongDesc, dbMSConfig_OMS2K.MSStage3ReallyLongDesc, dbMSConfig_OMS2K.MSStage4ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage5ReallyLongDesc, dbMSConfig_OMS2K.MSStage6ReallyLongDesc, dbMSConfig_OMS2K.MSStage7ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage8ReallyLongDesc, dbMSConfig_OMS2K.MSStage9ReallyLongDesc, dbMSConfig_OMS2K.MSStage10ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage11ReallyLongDesc, dbMSConfig_OMS2K.MSStage12ReallyLongDesc, dbMSConfig_OMS2K.MSStage13ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage14ReallyLongDesc, dbMSConfig_OMS2K.MSStage15ReallyLongDesc, dbMSConfig_OMS2K.MSStage16ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage17ReallyLongDesc, dbMSConfig_OMS2K.MSStage18ReallyLongDesc, dbMSConfig_OMS2K.MSStage19ReallyLongDesc, 
                      dbMSConfig_OMS2K.MSStage20ReallyLongDesc, 
					  
					  dbMSConfig_OMS2K.MSStage21ReallyLongDesc, dbMSConfig_OMS2K.MSStage22ReallyLongDesc, dbMSConfig_OMS2K.MSStage23ReallyLongDesc, dbMSConfig_OMS2K.MSStage24ReallyLongDesc, 
					  dbMSConfig_OMS2K.MSStage25ReallyLongDesc, dbMSConfig_OMS2K.MSStage26ReallyLongDesc, dbMSConfig_OMS2K.MSStage27ReallyLongDesc, dbMSConfig_OMS2K.MSStage28ReallyLongDesc, 
					  dbMSConfig_OMS2K.MSStage29ReallyLongDesc, dbMSConfig_OMS2K.MSStage30ReallyLongDesc, 
					  
					  dbMSConfig_OMS2K.MSStage1Action, dbMSConfig_OMS2K.MSStage2Action, 
                      dbMSConfig_OMS2K.MSStage3Action, dbMSConfig_OMS2K.MSStage4Action, dbMSConfig_OMS2K.MSStage5Action, 
                      dbMSConfig_OMS2K.MSStage6Action, dbMSConfig_OMS2K.MSStage7Action, dbMSConfig_OMS2K.MSStage8Action, 
                      dbMSConfig_OMS2K.MSStage9Action, dbMSConfig_OMS2K.MSStage10Action, dbMSConfig_OMS2K.MSStage11Action, 
                      dbMSConfig_OMS2K.MSStage12Action, dbMSConfig_OMS2K.MSStage13Action, dbMSConfig_OMS2K.MSStage14Action, 
                      dbMSConfig_OMS2K.MSStage15Action, dbMSConfig_OMS2K.MSStage16Action, dbMSConfig_OMS2K.MSStage17Action, 
                      dbMSConfig_OMS2K.MSStage18Action, dbMSConfig_OMS2K.MSStage19Action, dbMSConfig_OMS2K.MSStage20Action, 

					  dbMSConfig_OMS2K.MSStage21Action, dbMSConfig_OMS2K.MSStage22Action, dbMSConfig_OMS2K.MSStage23Action, dbMSConfig_OMS2K.MSStage24Action, 
					  dbMSConfig_OMS2K.MSStage25Action, dbMSConfig_OMS2K.MSStage26Action, dbMSConfig_OMS2K.MSStage27Action, dbMSConfig_OMS2K.MSStage28Action, 
					  dbMSConfig_OMS2K.MSStage29Action, dbMSConfig_OMS2K.MSStage30Action

			FROM      dbMSData_OMS2K RIGHT OUTER JOIN
                      dbMSConfig_OMS2K ON dbMSConfig_OMS2K.MSCode = dbMSData_OMS2K.MSCode";
            }
        }
      


        /// <summary>
        /// Sets the undlerying data by field name.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="value">Value to use.</param>
        public override void SetExtraInfo(string name, object value)
        {
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (value is DateTime)
            {
                DateTime dteval = (DateTime)value;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    value = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

            object original = GetExtraInfo(name);
            string prop = GetPropertyName(name);

            if (original.Equals(value))
                return;

            ValueChangingEventArgs changing = new ValueChangingEventArgs(prop, original, value);
            OnValueChanging(changing);
            if (changing.Cancel)
                return;

            DataRow r = _data.Rows[0];

            r[name] = value;

            OnDataChanged();

            if (name.EndsWith("Achieved"))
            {
                if (value == DBNull.Value)
                    SetExtraInfo(name.Replace("Achieved", "AchievedBy"), DBNull.Value);
                else
                    SetExtraInfo(name.Replace("Achieved", "AchievedBy"), Session.CurrentSession.CurrentUser.ID);
            }

            if (name.EndsWith("Achieved") || name.EndsWith("Due")) 
                Recalculate(false);

            if (name == FieldPrimaryKey)
                OnUniqueIDChanged();

            ValueChangedEventArgs changed = new ValueChangedEventArgs(prop, original, value);
            OnValueChanged(changed);
        }

       
        #endregion

        #region Common Methods

        private DateTime? GetStageAchieved(int stage)
        {
            object val = GetExtraInfo(String.Format("MSStage{0}Achieved", stage));
            if (val == DBNull.Value)
            {
                return null;
            }
            return (DateTime)val;
        }
        private void SetStageAchieved(int stage, object value)
        {
            if (!(value is DateTime))
                value = DBNull.Value;

            object original = GetExtraInfo(String.Format("MSStage{0}Achieved", stage));

            SetExtraInfo(String.Format("MSStage{0}Achieved", stage), value);
            
            value = GetExtraInfo(String.Format("MSStage{0}Achieved", stage));

            if (!original.Equals(value))
            {
                if (value == DBNull.Value)
                    ticked[stage - 1] = false;
                else
                    ticked[stage - 1] = true;

                OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs(String.Format("MSStage{0}Achieved", stage), original, value));
            }
        }

        private DateTime? GetStageDue(int stage)
        {
            object val = GetExtraInfo(String.Format("MSStage{0}Due", stage));
            if (val == DBNull.Value)
                return null;
            else
                return (DateTime)val;
        }

        private void SetStageDue(int stage, object value)
        {
            if (value is DateTime)
                SetExtraInfo(String.Format("MSStage{0}Due", stage), value);
            else
                SetExtraInfo(String.Format("MSStage{0}Due", stage), DBNull.Value);
        }

        private string GetStageDesc(int stage)
        {
            return Convert.ToString(GetExtraInfo(String.Format("MSStage{0}Desc", stage)));
        }

        private bool GetStageChecked(int stage)
        {
            DateTime? achieved = GetStageAchieved(stage);
            if (achieved.HasValue)
                ticked[stage - 1] = true;
            else
                ticked[stage - 1] = false;

            return ticked[stage - 1];
        }

        private void SetStageChecked(int stage, bool value)
        {
            if (ticked[stage - 1] != value)
            {
                if (value)
                    SetStageAchieved(stage, DateTime.Now);
                else
                    SetStageAchieved(stage, DBNull.Value);
            }
        }

        private string GetStageCalcDaysDesc(int stage)
        {
            DateTime? due = GetStageDue(stage);
            if (due.HasValue == false)
            {
                if (ticked[stage - 1])
                    return "COMP";
                else
                    return "Not Set";
            }
            else
            {
                if (ticked[stage - 1])
                    return "COMP";
                else
                {
                    //UTCFIX: DM - 30/11/06 - Subtracting Local Times
                    TimeSpan tsp = due.Value.ToLocalTime().Date - DateTime.Today;
                    return Convert.ToString(Convert.ToInt32(tsp.TotalDays));
                }
            }
        }

        private int? GetStageCalcDays(int stage)
        {
            DateTime? due = GetStageDue(stage);
            if (due.HasValue == false)
                return null;
            else
            {
                if (ticked[stage - 1])
                    return null;
                else
                {
                    //UTCFIX: DM - 30/11/06 - Subtracting Local Times
                    TimeSpan tsp = due.Value.ToLocalTime().Date  - DateTime.Today;
                    return Convert.ToInt32(tsp.TotalDays);
                }
            }
        }

        private string GetCalc(int stage)
        {
            if (stage == 1)
                return "S";
            else
            {
                string val = Convert.ToString(GetExtraInfo(String.Format("MSStage{0}CalcFrom", stage)));
                switch (val)
                {
                    case "0":
                        return "O";
                    case "1":
                        return "D";
                    case "2":
                        return "A";
                    default:
                        return "O";
                }
            }
        }

        private int GetStageDays(int stage)
        {
            return ConvertDef.ToInt32(GetExtraInfo(String.Format("MSStage{0}Days", stage)), 0);
        }

        #endregion

        #region MSStageDesc

        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage1Desc
        {
            get
            {
                return GetStageDesc(1);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage2Desc
        {
            get
            {
                return GetStageDesc(2);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage3Desc
        {
            get
            {
                return GetStageDesc(3);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage4Desc
        {
            get
            {
                return GetStageDesc(4);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage5Desc
        {
            get
            {
                return GetStageDesc(5);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage6Desc
        {
            get
            {
                return GetStageDesc(6);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage7Desc
        {
            get
            {
                return GetStageDesc(7);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage8Desc
        {
            get
            {
                return GetStageDesc(8);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage9Desc
        {
            get
            {
                return GetStageDesc(9);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage10Desc
        {
            get
            {
                return GetStageDesc(10);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage11Desc
        {
            get
            {
                return GetStageDesc(11);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage12Desc
        {
            get
            {
                return GetStageDesc(12);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage13Desc
        {
            get
            {
                return GetStageDesc(13);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage14Desc
        {
            get
            {
                return GetStageDesc(14);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage15Desc
        {
            get
            {
                return GetStageDesc(15);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage16Desc
        {
            get
            {
                return GetStageDesc(16);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage17Desc
        {
            get
            {
                return GetStageDesc(17);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage18Desc
        {
            get
            {
                return GetStageDesc(18);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage19Desc
        {
            get
            {
                return GetStageDesc(19);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage20Desc
        {
            get
            {
                return GetStageDesc(20);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage21Desc
        {
            get
            {
                return GetStageDesc(21);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage22Desc
        {
            get
            {
                return GetStageDesc(22);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage23Desc
        {
            get
            {
                return GetStageDesc(23);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage24Desc
        {
            get
            {
                return GetStageDesc(24);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage25Desc
        {
            get
            {
                return GetStageDesc(25);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage26Desc
        {
            get
            {
                return GetStageDesc(26);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage27Desc
        {
            get
            {
                return GetStageDesc(27);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage28Desc
        {
            get
            {
                return GetStageDesc(28);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage29Desc
        {
            get
            {
                return GetStageDesc(29);
            }
        }
        /// <summary>
        /// Gets the stage description.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage30Desc
        {
            get
            {
                return GetStageDesc(30);
            }
        }

        #endregion

        #region MSStageDue

        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage1Due
        {
            get
            {
                DateTime? val = GetStageDue(1);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(1, value);
            }

        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage2Due
        {
            get
            {
                DateTime? val = GetStageDue(2);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(2, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage3Due
        {
            get
            {
                DateTime? val = GetStageDue(3);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(3, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage4Due
        {
            get
            {
                DateTime? val = GetStageDue(4);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(4, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage5Due
        {
            get
            {
                DateTime? val = GetStageDue(5);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(5, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage6Due
        {
            get
            {
                DateTime? val = GetStageDue(6);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(6, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage7Due
        {
            get
            {
                DateTime? val = GetStageDue(7);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(7, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage8Due
        {
            get
            {
                DateTime? val = GetStageDue(8);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(8, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage9Due
        {
            get
            {
                DateTime? val = GetStageDue(9);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(9, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage10Due
        {
            get
            {
                DateTime? val = GetStageDue(10);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(10, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage11Due
        {
            get
            {
                DateTime? val = GetStageDue(11);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(11, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage12Due
        {
            get
            {
                DateTime? val = GetStageDue(12);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(12, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage13Due
        {
            get
            {
                DateTime? val = GetStageDue(13);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(13, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage14Due
        {
            get
            {
                DateTime? val = GetStageDue(14);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(14, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage15Due
        {
            get
            {
                DateTime? val = GetStageDue(15);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(15, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage16Due
        {
            get
            {
                DateTime? val = GetStageDue(16);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(16, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage17Due
        {
            get
            {
                DateTime? val = GetStageDue(17);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(17, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage18Due
        {
            get
            {
                DateTime? val = GetStageDue(18);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(18, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage19Due
        {
            get
            {
                DateTime? val = GetStageDue(19);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(19, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage20Due
        {
            get
            {
                DateTime? val = GetStageDue(20);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(20, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage21Due
        {
            get
            {
                DateTime? val = GetStageDue(21);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(21, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage22Due
        {
            get
            {
                DateTime? val = GetStageDue(22);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(22, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage23Due
        {
            get
            {
                DateTime? val = GetStageDue(23);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(23, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage24Due
        {
            get
            {
                DateTime? val = GetStageDue(24);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(24, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage25Due
        {
            get
            {
                DateTime? val = GetStageDue(25);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(25, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage26Due
        {
            get
            {
                DateTime? val = GetStageDue(26);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(26, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage27Due
        {
            get
            {
                DateTime? val = GetStageDue(27);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(27, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage28Due
        {
            get
            {
                DateTime? val = GetStageDue(28);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(28, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage29Due
        {
            get
            {
                DateTime? val = GetStageDue(29);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(29, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage due date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage30Due
        {
            get
            {
                DateTime? val = GetStageDue(30);
                if (val.HasValue)
                    return val.Value;
                else
                    return DBNull.Value;
            }
            set
            {
                SetStageDue(30, value);
            }
        }
        #endregion

        #region MSStageAcheived

        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage1Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(1);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(1, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage2Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(2);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(2, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage3Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(3);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(3, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage4Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(4);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(4, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage5Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(5);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(5, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage6Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(6);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(6, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage7Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(7);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(7, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage8Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(8);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(8, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage9Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(9);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(9, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage10Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(10);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(10, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage11Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(11);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(11, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage12Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(12);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(12, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage13Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(13);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(13, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage14Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(14);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(14, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage15Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(15);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(15, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage16Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(16);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(16, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage17Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(17);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(17, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage18Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(18);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(18, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage19Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(19);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(19, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage20Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(20);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(20, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage21Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(21);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(21, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage22Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(22);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(22, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage23Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(23);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(23, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage24Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(24);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(24, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage25Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(25);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(25, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage26Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(26);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(26, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage27Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(27);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(27, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage28Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(28);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(28, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage29Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(29);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(29, value);
            }
        }
        /// <summary>
        /// Gets or sets the stage achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public object MSStage30Achieved
        {
            get
            {
                DateTime? date = GetStageAchieved(30);
                if (date == null)
                    return DBNull.Value;
                else
                    return date.Value;
            }
            set
            {
                SetStageAchieved(30, value);
            }
        }
        #endregion

        #region MSStageChecked

        /// <summary>
        /// Gets or sets the stage checked flags.
        /// </summary>
        [EnquiryUsage(true)]
        public bool MSStage1Checked
        {
            get
            {
                return GetStageChecked(1);
            }
            set
            {
                SetStageChecked(1, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage2Checked
        {
            get
            {
                return GetStageChecked(2);
            }
            set
            {
                SetStageChecked(2, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage3Checked
        {
            get
            {
                return GetStageChecked(3);
            }
            set
            {
                SetStageChecked(3, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage4Checked
        {
            get
            {
                return GetStageChecked(4);
            }
            set
            {
                SetStageChecked(4, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage5Checked
        {
            get
            {
                return GetStageChecked(5);
            }
            set
            {
                SetStageChecked(5, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage6Checked
        {
            get
            {
                return GetStageChecked(6);
            }
            set
            {
                SetStageChecked(6, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage7Checked
        {
            get
            {
                return GetStageChecked(7);
            }
            set
            {
                SetStageChecked(7, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage8Checked
        {
            get
            {
                return GetStageChecked(8);
            }
            set
            {
                SetStageChecked(8, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage9Checked
        {
            get
            {
                return GetStageChecked(9);
            }
            set
            {
                SetStageChecked(9, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage10Checked
        {
            get
            {
                return GetStageChecked(10);
            }
            set
            {
                SetStageChecked(10, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage11Checked
        {
            get
            {
                return GetStageChecked(11);
            }
            set
            {
                SetStageChecked(11, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage12Checked
        {
            get
            {
                return GetStageChecked(12);
            }
            set
            {
                SetStageChecked(12, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage13Checked
        {
            get
            {
                return GetStageChecked(13);
            }
            set
            {
                SetStageChecked(13, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage14Checked
        {
            get
            {
                return GetStageChecked(14);
            }
            set
            {
                SetStageChecked(14, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage15Checked
        {
            get
            {
                return GetStageChecked(15);
            }
            set
            {
                SetStageChecked(15, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage16Checked
        {
            get
            {
                return GetStageChecked(16);
            }
            set
            {
                SetStageChecked(16, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage17Checked
        {
            get
            {
                return GetStageChecked(17);
            }
            set
            {
                SetStageChecked(17, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage18Checked
        {
            get
            {
                return GetStageChecked(18);
            }
            set
            {
                SetStageChecked(18, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage19Checked
        {
            get
            {
                return GetStageChecked(19);
            }
            set
            {
                SetStageChecked(19, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage20Checked
        {
            get
            {
                return GetStageChecked(20);
            }
            set
            {
                SetStageChecked(20, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage21Checked
        {
            get
            {
                return GetStageChecked(21);
            }
            set
            {
                SetStageChecked(21, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage22Checked
        {
            get
            {
                return GetStageChecked(22);
            }
            set
            {
                SetStageChecked(22, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage23Checked
        {
            get
            {
                return GetStageChecked(23);
            }
            set
            {
                SetStageChecked(23, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage24Checked
        {
            get
            {
                return GetStageChecked(24);
            }
            set
            {
                SetStageChecked(24, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage25Checked
        {
            get
            {
                return GetStageChecked(25);
            }
            set
            {
                SetStageChecked(25, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage26Checked
        {
            get
            {
                return GetStageChecked(26);
            }
            set
            {
                SetStageChecked(26, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage27Checked
        {
            get
            {
                return GetStageChecked(27);
            }
            set
            {
                SetStageChecked(27, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage28Checked
        {
            get
            {
                return GetStageChecked(28);
            }
            set
            {
                SetStageChecked(28, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage29Checked
        {
            get
            {
                return GetStageChecked(29);
            }
            set
            {
                SetStageChecked(29, value);
            }
        }
        [EnquiryUsage(true)]
        public bool MSStage30Checked
        {
            get
            {
                return GetStageChecked(30);
            }
            set
            {
                SetStageChecked(30, value);
            }
        }
        #endregion

        #region MSCalcDays

        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage1CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(1);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage2CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(2);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage3CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(3);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage4CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(4);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage5CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(5);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage6CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(6);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage7CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(7);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage8CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(8);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage9CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(9);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage10CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(10);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage11CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(11);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage12CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(12);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage13CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(13);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage14CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(14);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage15CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(15);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage16CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(16);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage17CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(17);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage18CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(18);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage19CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(19);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage20CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(20);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage21CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(21);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage22CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(22);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage23CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(23);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage24CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(24);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage25CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(25);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage26CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(26);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage27CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(27);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage28CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(28);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage29CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(29);
            }
        }
        /// <summary>
        /// Gets the number of days the stage is due to be achieved.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSStage30CalcDays
        {
            get
            {
                return GetStageCalcDaysDesc(30);
            }
        }
        #endregion

        #region Calc

        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc1
        {
            get
            {
                return GetCalc(1);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc2
        {
            get
            {
                return GetCalc(2);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc3
        {
            get
            {
                return GetCalc(3);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc4
        {
            get
            {
                return GetCalc(4);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc5
        {
            get
            {
                return GetCalc(5);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc6
        {
            get
            {
                return GetCalc(6);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc7
        {
            get
            {
                return GetCalc(7);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc8
        {
            get
            {
                return GetCalc(8);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc9
        {
            get
            {
                return GetCalc(9);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc10
        {
            get
            {
                return GetCalc(10);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc11
        {
            get
            {
                return GetCalc(11);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc12
        {
            get
            {
                return GetCalc(12);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc13
        {
            get
            {
                return GetCalc(13);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc14
        {
            get
            {
                return GetCalc(14);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc15
        {
            get
            {
                return GetCalc(15);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc16
        {
            get
            {
                return GetCalc(16);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc17
        {
            get
            {
                return GetCalc(17);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc18
        {
            get
            {
                return GetCalc(18);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc19
        {
            get
            {
                return GetCalc(19);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc20
        {
            get
            {
                return GetCalc(20);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc21
        {
            get
            {
                return GetCalc(21);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc22
        {
            get
            {
                return GetCalc(22);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc23
        {
            get
            {
                return GetCalc(23);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc24
        {
            get
            {
                return GetCalc(24);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc25
        {
            get
            {
                return GetCalc(25);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc26
        {
            get
            {
                return GetCalc(26);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc27
        {
            get
            {
                return GetCalc(27);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc28
        {
            get
            {
                return GetCalc(28);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc29
        {
            get
            {
                return GetCalc(29);
            }
        }
        /// <summary>
        /// Gets the calculation type for the due date calculation, O = From opening / start date, D = from previous due date, A = from previous achieved date.
        /// </summary>
        [EnquiryUsage(true)]
        public string Calc30
        {
            get
            {
                return GetCalc(30);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the total number of stages valid for the plan.
        /// </summary>
        [Browsable(false)]
		public int Stages
		{
			get
			{
                if (finalstage <= 0)
                {
                    finalstage = 0;
                    for (int i = 1; i <= MAX_STAGES; i++)
                    {
                        string desc = GetStageDesc(i);
                        if (String.IsNullOrEmpty(desc))
                            break;
                        finalstage++;
                    }
                }

                return finalstage;
			}
		}

        /// <summary>
        /// Checks to see if the milestone plan is clear / unitialised.
        /// </summary>
        public bool IsClear
        {
            get
            {
                return _data.Rows.Count == 0;
            }
        }

        /// <summary>
        /// Gets the unique milestone plan code.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSPlan
        {
            get
            {
                if (IsClear)
                    return String.Empty;
                else
                    return Convert.ToString(GetExtraInfo("MSCode"));
            }
        }

        /// <summary>
        /// Gets the desctiption of the milestone plan.
        /// </summary>
        [EnquiryUsage(true)]
        public string MSDescription
        {
            get
            {
                if (IsClear)
                    return String.Empty;
                else
                    return Convert.ToString(GetExtraInfo("MSDescription"));
            }
        }

        /// <summary>
        /// Gets the next stage number
        /// </summary>
        public int NextStage
        {
            get
            {
                int stage = ConvertDef.ToInt32(GetExtraInfo("MSNEXTDUESTAGE"), 0);
                if (stage < 0)
                    stage = 0;

                if (stage > Stages)
                    stage = Stages;

                return stage;
            }
        }

        /// <summary>
        /// Gets the Next stage due date.
        /// </summary>
        public object NextStageDueDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(GetExtraInfo("MSNEXTDUEDATE"));
                }
                catch
                {
                    return DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Gets the file object based from this occurence of the associate.
        /// </summary>
        public OMSFile OMSFile
        {
            get
            {
                return file;
            }
        }

    	#endregion

        #region Methods

        /// <summary>
        /// Gets the stages in a normalised list of stages from the current milestone.
        /// </summary>
        /// <param name="includeNull">Includes a null at the top of the list if set to true.</param>
        /// <returns></returns>
        public DataTable GetMileStoneStages(bool includeNull)
        {
            DataTable msstages = new DataTable("STAGES");

            msstages.Columns.Add("Value", typeof(int));
            msstages.Columns.Add("Description", typeof(string));
            DataRow drms = null;

            if (includeNull)
            {
                drms = msstages.NewRow();
                drms["Value"] = DBNull.Value;
                drms["Description"] = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(Not Set)", "").Text;
                msstages.Rows.Add(drms);
            }

            for (int i = 1; i <= MAX_STAGES; i++)
            {
                string desc = GetStageDesc(i);
                if (String.IsNullOrEmpty(desc) == false)
                {
                    drms = msstages.NewRow();
                    drms["Value"] = i;
                    drms["Description"] = GetStageDesc(i);
                    msstages.Rows.Add(drms);
                }
            }

            msstages.AcceptChanges();

            return msstages;
        }


        /// <summary>
        /// Removes the milestone plan from the file.
        /// </summary>
        public void RemoveMileStonePlan()
        {                      
            if (!IsClear)
                OMSFile.AddEvent("MSPLANREMOVED", String.Format("{0} ({1})", MSDescription, MSPlan), String.Empty);
            
            Delete();
            finalstage = 0;
            Create();
        }




        public void SetMileStonePlan(string Plan, bool FromToday)
        {
           
            try
            {
                loading = true;

                if (_data.Rows.Count == 0) Create();

                if (!file.ID.Equals(UniqueID))
                    UniqueID = file.ID;

                DataTable plans  = GetMilestonePlans(true);
                
                
                DataView vw = new DataView(plans, String.Format("MSCODE='{0}'", Plan), "", DataViewRowState.CurrentRows);
                
                if (vw.Count > 0)
                {
                    DataRowView newplan = vw[0];

                    foreach (DataColumn col in plans.Columns)
                    {
                        if (_data.Columns.Contains(col.ColumnName))
                            _data.Rows[0][col.ColumnName] = newplan[col.ColumnName];
                    }

                    //Calculate Stage 1 first!
                    DateTime startdate;
                    if (FromToday)
                    {
                        startdate = DateTime.Today;
                    }
                    else
                    {
                        startdate = OMSFile.TrackingStamp.Created.Date;
                    }

                   
                    finalstage = 0;

                    for (int s = 1; s <= MAX_STAGES; s++)
                    {
                        SetStageDue(s, DBNull.Value);

                        string desc = Convert.ToString(newplan[String.Format("MSStage{0}Desc", s)]);
                                        
                        if (String.IsNullOrEmpty(desc))
                            continue;

                        if (s == 1)
                        {
                            int calcdays = ConvertDef.ToInt32(newplan[String.Format("MSStage{0}Days", 1)], 0);
                            SetStageDue(s, startdate.AddDays(calcdays));
                            continue;
                        }
                    }
                    SetExtraInfo("MSCODE", Plan);
                    SetExtraInfo("MSNEXTDUESTAGE", 1);
                    SetExtraInfo("MSNEXTDUEDATE", MSStage1Due);

                    this.OMSFile.AddEvent("MSPLANADDED", String.Format("{0} ({1})", MSDescription, Plan), String.Empty);
                    
                }
            }
            finally
            {
                loading = false;
            }
        }

        /// <summary>
        /// Recalculates the dates.
        /// </summary>
        public void Recalculate()
        {
            Recalculate(true);
        }
        
		
		private void Recalculate(bool refresh)
		{           
			if (calculating == false && updating == false)
			{
				try
				{
                    calculating = true;

					for (int i = 1 ; i <= Stages; i++)
					{
						if (i > 1)
						{
							string calc = GetCalc(i);
                            int nodays = GetStageDays(i);
                            DateTime? due = GetStageDue(i);
                            object newdatedue = DBNull.Value;
                            bool changed = false;
                            
							switch (calc)
							{
                                case "O": // From Opening Date
                                    {
                                       
                                        DateTime? stage1due = GetStageDue(1);
                                        int stage1days = GetStageDays(1);
                                        

                                        if (stage1due.HasValue)
                                        {
                                            if (due == null || refresh)
                                            {
                                                //Calculate the original start days!
                                                stage1due = stage1due.Value.AddDays(-stage1days);
                                                newdatedue = stage1due.Value.AddDays(nodays);
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            if (due == null || refresh)
                                            {
                                                newdatedue = DBNull.Value;
                                                changed = true;
                                            }
                                        }

                                    }
                                    break;
                                case "D": // Due Date
                                    {
                                        DateTime? previousdue  = GetStageDue(i-1);
                                        DateTime? previousachieved = GetStageAchieved(i - 1);

                                        if (previousdue.HasValue)
                                        {
                                            if (due == null || refresh)
                                            {
                                                newdatedue = previousdue.Value.AddDays(nodays);
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            if (due == null || refresh)
                                            {
                                                newdatedue = DBNull.Value;
                                                changed = true;
                                            }
                                        }
                                    }
                                    break;
                                case "A": // Acheived Date
                                    {
                                        DateTime? previousachieved = GetStageAchieved(i - 1);

                                        if (previousachieved.HasValue)
                                        {
                                            if (due == null || refresh)
                                            {
                                                newdatedue = previousachieved.Value.AddDays(nodays);
                                                changed = true;
                                            }
                                        }
                                        else
                                        {
                                            if (due == null || refresh)
                                            {
                                                newdatedue = DBNull.Value;
                                                changed = true;
                                            }
                                        }
                                    }
                                    break;
							}

                            if (changed)
                            {
                                SetStageDue(i, newdatedue);
                                OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs(String.Format("MSStage{0}Due", i), due, newdatedue));
                            }

						}
					}	

					for (int i = 1 ; i <= Stages; i++)
					{
                        DateTime? achieved = GetStageAchieved(i);
                        DateTime? due = GetStageDue(i);

                        string days = String.Empty;
						if (achieved.HasValue)
                            days = "COMP";
						else if (due == null)
							days = "Not Set";
						else
						{
                            //UTCFIX: DM - 30/11/06 - Subtracting Local Times
                            TimeSpan tsp = due.Value.ToLocalTime().Date - DateTime.Today;
							days = Convert.ToString(Convert.ToInt32(tsp.TotalDays));
						}

                        OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs(String.Format("MSStage{0}CalcDays", i), days));
					}
				}
				finally
				{
					calculating = false;
				}
			}

		}

		#endregion

		#region Static Methods

        /// <summary>
        /// Gets a list of milestone plans from the system.
        /// </summary>
        /// <param name="refresh">Refreshses the cached list if  set to true.</param>
        /// <returns></returns>
		public static DataTable GetMilestonePlans(bool refresh)
		{
			string sql = "SELECT * FROM dbMSConfig_OMS2K";

			if (msconfigdata == null || refresh)
				msconfigdata = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "MSPLAN", null);

            return msconfigdata;
		}

        /// <summary>
        /// Checks to see if a milestoneplan exists against a file.
        /// </summary>
        /// <param name="fileId">The file id used to check against.</param>
        /// <returns></returns>
		public static bool MilestonePlanExists(long fileId)
		{
			IDataParameter[] pars = new IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("ID",fileId);

			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT fileid FROM dbMSData_OMS2K WHERE fileID = @ID", "EXISTS", pars);
			 
            if ((dt == null) || (dt.Rows.Count == 0))
				return false;
			else
				return true;

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
				return OMSFile;
			}
		}

		#endregion
	}
}
