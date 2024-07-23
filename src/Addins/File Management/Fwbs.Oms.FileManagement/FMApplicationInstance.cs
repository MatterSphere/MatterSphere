using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using FWBS.OMS.FileManagement.Configuration;
using FWBS.OMS.Script;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;

namespace FWBS.OMS.FileManagement
{
    public sealed class FMApplicationInstance : IDisposable, INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields

        /// <summary>
        /// A reference to the parent file application configuration.
        /// </summary>
        private FMApplication _parent;

        /// <summary>
        /// Holds a reference to the script object for performing work flow actions actions.
        /// </summary>
        private Script.ScriptGen script = null;

        /// <summary>
        /// A reference to the current file.
        /// </summary>
        private OMSFile _currentFile = null;

        /// <summary>
        /// A reference to the current Task;
        /// </summary>
        private Milestones.Task _currentTask = null;

        /// <summary>
        /// A reference to the current milestone plan.
        /// </summary>
        private Milestones.MilestonePlan _currentPlan = null;

        #endregion

        #region Constructors

        internal FMApplicationInstance(FMApplication parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            this.Parent = parent;
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Gets the file application configuration.
        /// </summary>
        [Browsable(false)]
        public FMApplication Parent
        {
            get
            {
                return this._parent;
            }
            private set
            {
                if (this._parent == value)
                    return;
                this._parent = value;
                RaisePropertyChanged("Parent");
            }
        }

        /// <summary>
        /// Gets the current file.
        /// </summary>
        [Browsable(false)]
        public OMSFile CurrentFile
        {
            get
            {
                return _currentFile;
            }
            private set
            {
                if (_currentFile == value)
                    return;

                _currentFile = value;
                RaisePropertyChanged("CurrentFile");
            }
        }

        /// <summary>
        /// Gets the current task.
        /// </summary>
        [Browsable(false)]
        public Milestones.Task CurrentTask
        {
            get
            {
                return _currentTask;
            }
            private set
            {
                if (_currentTask == value)
                    return;

                _currentTask = value;
                RaisePropertyChanged("CurrentTask");
            }
        }

        /// <summary>
        /// Gets the current milestone plan.
        /// </summary>
        [Browsable(false)]
        public Milestones.MilestonePlan CurrentPlan
        {
            get
            {
                return _currentPlan;
            }
            internal set
            {
                if (_currentPlan == value)
                    return;
                RaisePropertyChanging("CurrentPlan");
                _currentPlan = value;
                RaisePropertyChanged("CurrentPlan");
            }
        }

        #endregion

        #region Scripting

        /// <summary>
        /// Gets the scriptlet of the current FileManagement application.
        /// </summary>
        [Browsable(false)]
        public ScriptGen Script
        {
            get
            {
                if (script == null)
                {
                    string scriptype = Convert.ToString(Parent.GetExtraInfo("appscript"));

                    if (scriptype.Length > 0 && ScriptGen.Exists(scriptype))
                    {
                        script = ScriptGen.GetScript(scriptype);
                    }

                    if (script != null)
                    {
                        ApplicationScriptType st = script.ScriptType as ApplicationScriptType;
                        if (st != null)
                            st.SetAppObject(this);
                    }
                }
                return script;
            }
        }

        /// <summary>
        /// Gets the currently loaded scriptlet.
        /// </summary>
        internal ApplicationScriptType Scriptlet
        {
            get
            {
                if (script == null)
                    return null;
                else
                    return script.Scriptlet as ApplicationScriptType;
            }
        }

        public void UnloadScript()
        {
            if (script != null)
            {
                script.Dispose();
                script = null;
            }
        }

        /// <summary>
        /// Loads the scriptlet into memory.
        /// </summary>
        /// <param name="designMode">Specifies whether the script should be loaded if it isin design mode.</param>
        public void LoadScript(bool force)
        {
            if (force)
            {
                UnloadScript();
            }

            if (Parent.HasScript)
            {
                Script.Load(false);
                if (script != null)
                {
                    ApplicationScriptType st = script.Scriptlet as ApplicationScriptType;
                    if (st != null)
                        st.SetAppObject(this);
                }
            }
        }

        #endregion

        #region Initialisation


        /// <summary>
        /// Initialises the FileManagement application with a OMSFile instance.
        /// </summary>
        public void Initialise(OMSFile file)
        {
            CurrentFile = file;

            ValidateFile();

            if (script == null)
                LoadScript(false);

        }

        /// <summary>
        /// Initialises the FileManagement application with the OMSFile from the task given.
        /// </summary>
        private void Initialise(Milestones.Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            CurrentTask = task;
            CurrentFile = task.CurrentFile;

            ValidateFile();
            ValidateTask(task);

            if (script == null)
                LoadScript(false);
        }

        public void InitialiseMilestonePlan()
        {
            Debug.WriteLine("InitialiseMilestonePlan()");

            ValidateFile();

            Milestones_OMS2K plan = CurrentFile.MilestonePlan;
            Milestones.MilestonePlan msplan = CurrentPlan;

            if (plan == null)
                return;

            if (msplan == null || (msplan.Code != plan.MSPlan || msplan.CurrentFile.ID != CurrentFile.ID))
            {
                msplan = new Milestones.MilestonePlan(this, plan);
                Initialise(msplan);
            }

            if (msplan.TasksNeedGenerating)
                msplan.GenerateTasks();
            Debug.WriteLine("END InitialiseMilestonePlan()");

        }

        public void InitialiseMilestonePlan(FWBS.OMS.Milestones_OMS2K plan)
        {
            Debug.WriteLine("InitialiseMilestonePlan " + plan.MSDescription);

            ValidateFile();

            if (plan == null)
                return;

            Milestones.MilestonePlan msplan = CurrentPlan;

            if (msplan == null || (msplan.Code != plan.MSPlan))
            {
                msplan = new Milestones.MilestonePlan(this, plan);
                Initialise(msplan);
            }

            if (msplan.TasksNeedGenerating)
                msplan.GenerateTasks();
            Debug.WriteLine("End InitialiseMilestonePlan " + plan.MSDescription);
        }

        public void InitialiseTask(FWBS.OMS.Task task)
        {
            Debug.WriteLine("InitialiseTask " + task.Description);

            ValidateFile();
            InitialiseMilestonePlan();

            Milestones.Task t;
            if (CurrentPlan == null)
                t = new FileManagement.Milestones.Task(this, task, null);
            else
            {
                Milestones.MilestoneStage stage = CurrentPlan[task];
                if (stage == null)
                {
                    t = new FileManagement.Milestones.Task(this, task, null);
                }
                else if ((t = stage.Tasks[task.ID]) == null)
                {
                    stage.Tasks.Refresh(true);
                    t = stage.Tasks[task.ID];
                }
            }

            Initialise(t);
            Debug.WriteLine("END InitialiseTask " + task.Description);

        }

        /// <summary>
        /// Initialises the FileManagement application with the OMSFile from the task given.
        /// </summary>
        internal void Initialise(Milestones.MilestonePlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            CurrentTask = null;
            CurrentFile = plan.CurrentFile;
            CurrentPlan = plan;

            ValidateFile();
            ValidateMilestonePlan();

            if (script == null)
                LoadScript(false);
        }

        #endregion

        #region Validation

        private void ValidateFile()
        {
            if (CurrentFile == null)
                throw new FMException("FMFILEINITSERR", "A %FILE% has not been initialised and bound to the File Management application.");
        }

        private void ValidateMilestonePlan()
        {
            ValidateFile();

            if (CurrentPlan == null)
                throw new FMException("FMMSPLANERR", "A milestone plan has not been initialised and bound to the %FILE% and File Management application.");


        }

        private void ValidateTask(Milestones.Task task)
        {
            ValidateFile();

            if (task == null)
                throw new ArgumentNullException("task");

            if (task.CurrentFile.ID != CurrentFile.ID)
                throw new FMException("FMTASKBADPAR", "The specified task '%1%' does not belong to the current %FILE%.", null, true, task.Description);

        }

        internal byte ValidateStage(byte stage)
        {
            ValidateFile();
            ValidateMilestonePlan();


            if (stage < 1)
                return 1;
            if (stage > CurrentPlan.Stages)
                return Convert.ToByte(CurrentPlan.Stages);

            return stage;
        }

        #endregion

        #region Task Assignment

        internal void AssignTaskToTeam(string team, params string[] filters)
        {
            if (String.IsNullOrEmpty(team))
                throw new ArgumentNullException("team");

            Team t = Team.GetTeam(team);

            AssignTaskToTeam(t, filters);
        }

        internal void AssignTaskToTeam(Team team, params string[] filters)
        {
            if (team == null)
                throw new ArgumentNullException("team");

            if (filters == null)
                throw new ArgumentNullException("filters");

            if (filters.Length == 0)
                throw new ArgumentException("At least one filter id must be specified.", "filters");

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.Null));

            foreach (System.Data.DataRowView r in taskvw)
            {
                Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                if (task != null)
                {
                    task.AssignedTeam = team;
                }
            }
        }

        internal void AssignTaskToUser(string initials, params string[] filters)
        {
            if (String.IsNullOrEmpty(initials))
                throw new ArgumentNullException("initials");

            User usr = User.GetUser(initials);

            AssignTaskToUser(usr, filters);
        }


        internal void AssignTaskToUser(User user, params string[] filters)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (filters == null)
                throw new ArgumentNullException("filters");

            if (filters.Length == 0)
                throw new ArgumentException("At least one filter id must be specified.", "filters");

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.Null));

            foreach (System.Data.DataRowView r in taskvw)
            {
                Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                if (task != null)
                {
                    task.AssignedTo = user;
                }
            }
        }



        #endregion

        #region Action Specifics


        /// <summary>
        /// Executes an action.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void Execute(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            new FileActivity(_currentFile, FileStatusActivityType.TaskflowProcessing).Check();

            ValidateFile();
            Parent.ValidateAction(action.ActionConfiguration);

            if (Scriptlet != null)
            {
                Scriptlet.Execute(action);
            }
            else
            {
                throw new FMException("FMEXENOHANDLER", "The action '%1%' has not been handled by the File Management script.", null, true, action.ActionConfiguration.Code);
            }
        }

        /// <summary>
        /// Gets the available actions for the current file instance and user login session.
        /// </summary>
        /// <returns></returns>
        public Action[] GetAvailableActions(Milestones.MilestoneStage stage)
        {
            ValidateFile();

			if (stage == null)
				stage = GetNextStage();

            System.Collections.ArrayList list = new System.Collections.ArrayList();

            for (int ctr = 0; ctr < Parent.FileActions.Count; ctr++)
            {
                Configuration.ActionConfig action = Parent.FileActions[ctr];
                Action act = new Action(action, ActionType.File, stage);
                if (IsActionAvailable(act, stage))
                {
                    list.Add(act);
                }
            }

            Action[] array = new Action[list.Count];
            list.CopyTo(array, 0);

            return array;
        }

		private Milestones.MilestoneStage GetNextStage()
		{
			byte stageno = 0;
			try
			{
				if (_currentPlan != null)
					stageno = FWBS.Common.ConvertDef.ToByte(CurrentFile.MilestonePlan.NextStage, 0);
			}
			catch { }

			if (stageno == 0)
				return null;

			return CurrentPlan[stageno];
		}

        /// <summary>
        /// Gets the available actions for the current file instance and user login session.
        /// </summary>
        /// <returns></returns>
        public Action[] GetAvailableActions(Milestones.Task task)
        {
            ValidateFile();
            ValidateTask(task);

            System.Collections.ArrayList list = new System.Collections.ArrayList();

            Configuration.TaskTypeConfig[] tts = Parent.TaskTypes.Match(task);

            foreach (var tt in tts)
            {
                for (int ctr = 0; ctr < tt.Actions.Count; ctr++)
                {
                    Configuration.ActionConfig action = tt.Actions[ctr];
                    Action act = new Action(action, ActionType.Task, task.Stage, task);
                    if (IsActionAvailable(act, task.Stage))
                    {
                        list.Add(act);
                    }
                }

            }

            Action[] array = new Action[list.Count];
            list.CopyTo(array, 0);

            return array;
        }

        /// <summary>
        /// Find out whether a particular action is available.
        /// </summary>
        private bool IsActionAvailable(Action action, Milestones.MilestoneStage stage)
        {

            if (action == null)
                return false;

            int stageno = 0;
            if (stage == null)
            {
                try
                {
                    stageno = FWBS.Common.ConvertDef.ToInt32(CurrentFile.MilestonePlan.NextStage, 0);
                }
                catch { }
            }
            else
                stageno = stage.StageNumber;

            try
            {
                ValidateFile();
                Parent.ValidateAction(action.ActionConfiguration);

                if (Session.CurrentSession.ValidateConditional(action, action.ActionConfiguration.Conditional) == false)
                    return false;

                if (Session.CurrentSession.CurrentUser.IsInRoles(action.ActionConfiguration.UserRoles) == false)
                    return false;

                if (Scriptlet != null)
                {
                    ActionVisibility av = Scriptlet.IsActionAvailable(action);
                    switch (av)
                    {
                        case ActionVisibility.Hidden:
                            return false;
                        case ActionVisibility.Visible:
                            return true;
                        default:
                            break;
                    }
                }

                //Check milestone plan and next stage filter.
                if (action.ActionConfiguration.MilestonePlan != Configuration.ActionConfig.GLOBAL_MILESTONE_PLAN)
                {
                    if (CurrentFile.MilestonePlan == null || CurrentFile.MilestonePlan.IsClear)
                    {
                        return false;
                    }
                    else
                    {
                        if (CurrentFile.MilestonePlan.MSPlan.ToUpper() != action.ActionConfiguration.MilestonePlan.ToUpper())
                        {
                            return false;
                        }
                    }
                }

                if (action.ActionConfiguration.MilestoneStage > Configuration.ActionConfig.GLOBAL_MILESTONE_STAGE)
                {
                    if (CurrentFile.MilestonePlan == null || CurrentFile.MilestonePlan.IsClear)
                    {
                        return false;
                    }
                    else
                    {
                        if (stageno != action.ActionConfiguration.MilestoneStage)
                        {
                            return false;
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }

            return true;
        }



        #endregion

        #region Task Specifics

        #region Completion Checks

        internal bool CanCompleteStage(byte stage, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            stage = ValidateStage(stage);

            return CanCompleteStage(CurrentPlan[stage], reasons);
        }

        internal bool CanCompleteStage(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (reasons == null)
                reasons = new System.Collections.ArrayList();

            bool can = true;

            if (Scriptlet != null)
            {
                can = Scriptlet.CanComplete(stage, reasons);
            }

            return can;
        }


        internal bool CanUnCompleteStage(byte stage, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();


            stage = ValidateStage(stage);

            return CanUnCompleteStage(CurrentPlan[stage], reasons);
        }

        internal bool CanUnCompleteStage(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (reasons == null)
                reasons = new System.Collections.ArrayList();

            bool can = true;

            if (Scriptlet != null)
            {
                can = Scriptlet.CanUnComplete(stage, reasons);
            }


            return can;
        }


        internal bool CanCompleteTask(string filter, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            System.Data.DataView taskvw = CurrentFile.Tasks.Find(GetTaskFilter(new string[] { filter }, Common.TriState.Null, Common.TriState.Null));
            if (taskvw.Count > 0)
            {
                Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(taskvw[0]["tskid"])];
                return CanCompleteTask(task, reasons);
            }
            return false;
        }

        internal bool CanCompleteTask(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            if (reasons == null)
                reasons = new System.Collections.ArrayList();
            bool can = true;
            if (Scriptlet != null)
            {
                can = Scriptlet.CanComplete(task, reasons);
            }

            return can;
        }


        internal bool CanUnCompleteTask(string filter, System.Collections.ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            System.Data.DataView taskvw = CurrentFile.Tasks.Find(GetTaskFilter(new string[] { filter }, Common.TriState.Null, Common.TriState.Null));
            if (taskvw.Count > 0)
            {
                Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(taskvw[0]["tskid"])];
                return CanUnCompleteTask(task, reasons);
            }
            return false;
        }

        internal bool CanUnCompleteTask(Milestones.Task task, System.Collections.ArrayList reasons)
        {
            if (reasons == null)
                reasons = new System.Collections.ArrayList();
            bool can = true;
            if (Scriptlet != null)
            {
                can = Scriptlet.CanUnComplete(task, reasons);
            }

            return can;
        }

		internal bool CanChangeStageDueUI(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
		{
			if (reasons == null)
				reasons = new System.Collections.ArrayList();

			bool can = true;
			if (Scriptlet != null)
			{
				can = Scriptlet.CanChangeStageDueUI(stage, reasons);
			}

			return can;
		}

		internal bool CanChangeStageDue(Milestones.MilestoneStage stage, System.Collections.ArrayList reasons)
		{
			if (reasons == null)
				reasons = new System.Collections.ArrayList();

			bool can = true;
			if (Scriptlet != null)
			{
				can = Scriptlet.CanChangeStageDue(stage, reasons);
			}

			return can;
		}

        internal bool IsStageCompleted(params byte[] stages)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (stages == null)
                throw new ArgumentNullException("stages");

            if (stages.Length == 0)
                throw new ArgumentException("At least one stage must be specified.", "stages");

            foreach (byte stage in stages)
            {
                Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];
                if (st.IsCompleted == false)
                    return false;
            }

            return true;

        }

        internal bool AreStageTasksCompleted(params byte[] stages)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (stages == null)
                throw new ArgumentNullException("stages");

            if (stages.Length == 0)
                throw new ArgumentException("At least one stage must be specified.", "stages");

            foreach (byte stage in stages)
            {
                Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];
                if (st.Tasks.AreCompleted == false)
                    return false;
            }

            return true;

        }

        internal bool AreGroupedTasksCompleted(params string[] groups)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (groups == null)
                throw new ArgumentNullException("groups");

            if (groups.Length == 0)
                throw new ArgumentException("At least one group must be specified.", "groups");

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView alltasks = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.Null, Common.TriState.True));
            System.Data.DataView comptasks = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.True, Common.TriState.True));

            if (alltasks.Count > 0 && comptasks.Count > 0)
                return (alltasks.Count == comptasks.Count);
            else
                return false;
        }

        internal bool IsTaskCompleted(params string[] filters)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (filters == null)
                throw new ArgumentNullException("filters");

            if (filters.Length == 0)
                throw new ArgumentException("At least one filter id must be specified.", "filters");

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView alltasks = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.True));
            System.Data.DataView comptasks = tasks.Find(GetTaskFilter(filters, Common.TriState.True, Common.TriState.True));

            if (alltasks.Count > 0 && comptasks.Count > 0)
                return (alltasks.Count == comptasks.Count);
            else
                return false;
        }



        #endregion

        #region Filters

        internal string GetTaskFilter(string[] filters, Common.TriState completed, Common.TriState active)
        {

            System.Text.StringBuilder filter = new System.Text.StringBuilder();
            filter.Append(String.Format("(tskType = '{0}' and tskmsplan = '{1}')", Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE, CurrentPlan.Code));
            filter.Append(" and (");

            for (int ctr = 0; ctr < filters.Length; ctr++)
            {

                if (ctr > 0)
                    filter.Append(" or ");
                filter.Append(String.Format("tskfilter = '{0}'", filters[ctr]));

                switch (completed)
                {
                    case Common.TriState.True:
                        filter.Append(" and tskcomplete = true");
                        break;
                    case Common.TriState.False:
                        filter.Append(" and tskcomplete = false");
                        break;
                }

                switch (active)
                {
                    case Common.TriState.True:
                        filter.Append(" and tskactive = true");
                        break;
                    case Common.TriState.False:
                        filter.Append(" and tskactive = false");
                        break;
                }
            }

            filter.Append(")");

            return filter.ToString();
        }

        private string GetTaskGroupFilter(string[] groups, Common.TriState completed, Common.TriState active)
        {

            System.Text.StringBuilder filter = new System.Text.StringBuilder();
            filter.Append(String.Format("(tskType = '{0}' and tskmsplan = '{1}')", Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE, CurrentPlan.Code));
            filter.Append(" and (");

            for (int ctr = 0; ctr < groups.Length; ctr++)
            {
                if (ctr > 0)
                    filter.Append(" or ");
                filter.Append(String.Format("tskgroup = '{0}'", groups[ctr]));

                switch (completed)
                {
                    case Common.TriState.True:
                        filter.Append(" and tskcomplete = true");
                        break;
                    case Common.TriState.False:
                        filter.Append(" and tskcomplete = false");
                        break;
                }

                switch (active)
                {
                    case Common.TriState.True:
                        filter.Append(" and tskactive = true");
                        break;
                    case Common.TriState.False:
                        filter.Append(" and tskactive = false");
                        break;
                }

            }

            filter.Append(")");

            return filter.ToString();
        }

        #endregion

        #region Removing Tasks

        internal void RemoveGroupedTasks(string[] groups)
        {
            if (groups == null)
                return;

            if (groups.Length == 0)
                return;

            OMSFile file = CurrentFile;
            FWBS.OMS.Tasks tasks = file.Tasks;
            System.Data.DataView vw = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.False, Common.TriState.True));
            for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
            {
                vw[ctr]["tskactive"] = false;
            }

        }

        internal void RemoveTask(string[] filters)
        {
            if (filters == null)
                return;

            if (filters.Length == 0)
                return;

            OMSFile file = CurrentFile;
            FWBS.OMS.Tasks tasks = file.Tasks;
            System.Data.DataView vw = tasks.Find(GetTaskFilter(filters, Common.TriState.False, Common.TriState.True));
            for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
            {
                vw[ctr]["tskactive"] = false;
            }

        }

        internal void RemoveTask(Milestones.Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            task.Remove();

        }

        #endregion

        #region Adding Tasks

        internal ReturnValue ShowTaskWizard(out Milestones.Task task)
        {
			ValidateMilestonePlan();

            const string Due = "Due";
            const string FilterId = "FilterId";
            const string AssignedTeam = "AssignedTeam";
            const string AssignedTo = "AssignedTo";
            const string Group = "Group";
            const string Stage = "Stage";
            const string Description = "Description";
            const string Notes = "Notes";
            const string FeeEarner = "FeeEarner";
            const string Type = "Type";

            System.Data.DataTable data;
            Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
            pars.Add("PLAN", CurrentPlan.Code);
            pars.Add(Stage.ToUpper(), CurrentPlan.NextStage.StageNumber);
            pars.Add(Due.ToUpper(), CurrentPlan.NextStage.Due);
            pars.Add(FilterId.ToUpper(), Guid.NewGuid().ToString());
            pars.Add(Type.ToUpper(), Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE);

            ReturnValue ret;

            FWBS.OMS.EnquiryEngine.Enquiry enq = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry("SCRMSTASK", CurrentFile.Parent, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, pars);
            FWBS.OMS.UI.Windows.Services.Wizards wiz = new FWBS.OMS.UI.Windows.Services.Wizards(enq);
            data = wiz.Show() as System.Data.DataTable;

            if (data == null)
                ret = ReturnValue.Failed;
            else
                ret = ReturnValue.Success;


            if (ret == ReturnValue.Success)
            {
                System.Data.DataRow r = data.Rows[0];

                byte stageno = CurrentPlan.NextStage.StageNumber;
                if (r.Table.Columns.Contains(Stage))
                {
                    if (r[Stage] != DBNull.Value)
                    {
                        stageno = Convert.ToByte(r[Stage]);
                    }
                }

                string filterId = Guid.NewGuid().ToString();
                if (r.Table.Columns.Contains(FilterId))
                {
                    filterId = Convert.ToString(r[FilterId]);
                }


                task = AddTask(string.Empty, stageno, String.Empty, filterId);
                try
                {
                    if (r.Table.Columns.Contains(Description))
                    {
                        task.Description = Convert.ToString(r[Description]);
                    }

                    if (r.Table.Columns.Contains(Due))
                    {
                        if (r[Due] != DBNull.Value)
                        {
                            task.Due = Convert.ToDateTime(r[Due]);
                        }
                    }

                    if (r.Table.Columns.Contains(AssignedTeam))
                    {
                        if (r[AssignedTeam] != DBNull.Value)
                        {
                            Team team = Team.GetTeam(Convert.ToInt32(r[AssignedTeam]));
                            task.AssignedTeam = team;
                        }
                    }

                    if (r.Table.Columns.Contains(AssignedTo))
                    {
                        if (r[AssignedTo] != DBNull.Value)
                        {
                            User user = User.GetUser(Convert.ToInt32(r[AssignedTo]));
                            task.AssignedTo = user;
                        }
                    }

                    if (r.Table.Columns.Contains(Group))
                    {
                        if (r[Group] != DBNull.Value)
                        {
                            task.Group = Convert.ToString(r[Group]);
                        }
                    }


                    task.Manual = true;

                    if (r.Table.Columns.Contains(Notes))
                    {
                        if (r[Notes] != DBNull.Value)
                        {
                            task.Notes = Convert.ToString(r[Notes]);
                        }
                    }

                    if (r.Table.Columns.Contains(FeeEarner))
                    {
                        if (r[FeeEarner] != DBNull.Value)
                        {
                            task.FeeEarner = FWBS.OMS.FeeEarner.GetFeeEarner(Convert.ToInt32(r[FeeEarner]));
                        }
                    }

                    if (r.Table.Columns.Contains(Type))
                    {
                        if (r[Type] != DBNull.Value)
                        {
                            task.Type = Convert.ToString(r[Type]);
                        }
                    }
                }
                catch (Exception)
                {
                    //If any exception occurs then delete the task, do not allow it to be added.
                    task.Delete();
                    throw;

                }

            }
            else
                task = null;

            return ret;
        }

        internal Milestones.Task AddTask(string description, byte stage, string group, string filter)
        {
            ValidateFile();
            ValidateMilestonePlan();

            stage = ValidateStage(stage);

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            bool taskschanged = false;

            Milestones.MilestoneStage st = CurrentPlan[stage];
            DateTime? stagedue = null;
            if (st.IsNull(st.Due) == false && !CurrentPlan.Application.Parent.AllowNullDueDate)
                stagedue = st.Due.Value;

            return AddTask(CurrentFile, CurrentPlan.Code, stage, tasks, description, group, filter, stagedue, ref taskschanged, false, null, null, false, true);

        }

        internal Milestones.Task AddTask(string team, string description, byte stage, string group, string filter, DateTime? due)
        {
            ValidateFile();
            ValidateMilestonePlan();

            stage = ValidateStage(stage);

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            Team tm = null;
            if (String.IsNullOrEmpty(team) == false)
                tm = Team.GetTeam(team);

            bool taskschanged = false;
			return AddTask(CurrentFile, CurrentPlan.Code, stage, tasks, description, group, filter, due, ref taskschanged, false, (tm == null ? null : (int?)tm.ID), null, true, true);
        }

        internal Milestones.Task AddTask(OMSFile file, string plan, int stage, FWBS.OMS.Tasks tasks, string description, string taskGroup, string taskFilter, DateTime? due, ref bool taskschanged, bool manual, int? team, int? assignedTo, bool forcedue, bool refresh)
        {
			System.Diagnostics.Debug.WriteLine(string.Format("internal Milestones.Task AddTask() {0}", description), "FMApp");

            if (plan == null) plan = String.Empty;
            if (description == null) description = String.Empty;
            if (taskGroup == null) taskGroup = String.Empty;
            if (taskFilter == null) taskFilter = String.Empty;

            bool updated = false;
            bool isnewtask = false;

            if (taskFilter.Length == 0)
                taskFilter = Guid.NewGuid().ToString();

            System.Data.DataView vw = tasks.Find(GetTaskFilter(new string[] { taskFilter }, Common.TriState.Null, Common.TriState.Null));

            System.Data.DataRowView r;
            if (vw.Count == 0)
            {
                r = vw.AddNew();
                r["Created"] = System.DateTime.Now;
                r["CreatedBy"] = file.PrincipleFeeEarnerID;
                r["tskcomplete"] = false;
                r["tskactive"] = true;
                isnewtask = true;
            }
            else if (vw.Count == 1)
            {
                r = vw[0];
            }
            else
            {
                int i = 0;
                while (vw.Count > 1)
                {
                    for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                    {
                        if (Convert.ToBoolean(vw[ctr]["tskactive"]) == false)
                            vw[ctr].Delete();
                    }

                    if (vw.Count > 1)
                    {
                        i++;
                        vw[vw.Count - 1]["tskfilter"] = string.Format("{0}{1}", taskFilter, i);
                    }
                }
                r = vw[0];
            }

            if (r["tskdue"] == DBNull.Value)
            {
                if (due.HasValue)
                {
					r["tskdue"] = due.Value.AddDays(-GetTaskDueOffset(taskFilter)); 
                    updated = true;
                }
            }
            else
            {
                if (isnewtask || forcedue)
                {
                    if (due.HasValue == false)
                    {
                        r["tskdue"] = DBNull.Value;
                        updated = true;
                    }
                    else                    //UTCFIX: DM - 05/12/06 - Compare local dates.
                    {
						if (!forcedue)		//Don't apply offset if force is used otherwise we could be continuously subtracting the offset
							due = due.Value.AddDays(-GetTaskDueOffset(taskFilter));

                        if (Convert.ToDateTime(r["tskdue"]).ToLocalTime() != due.Value.ToLocalTime())
                        {
                            r["tskdue"] = due.Value;
                            updated = true;
                        }
                    }
                }

            }



            if (Convert.ToString(r["tsktype"]).ToUpper() != Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE)
                r["tsktype"] = Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE;

            if (!file.ID.Equals(r["fileid"]))
                r["fileid"] = file.ID;

            if (Convert.ToString(r["tskfilter"]).ToUpper() != taskFilter.ToUpper())
            {
                if (taskFilter == String.Empty)
                    r["tskfilter"] = DBNull.Value;
                else
                    r["tskfilter"] = taskFilter;
                updated = true;
            }

            if (Convert.ToString(r["tskdesc"]).ToUpper() != description.ToUpper())
            {
                System.Text.StringBuilder notes = new System.Text.StringBuilder(Convert.ToString(r["tsknotes"]));
                string desc = Convert.ToString(r["tskdesc"]);
                if (notes.Length > 0 && desc.Length > 0)
                {
                    notes.Append(Environment.NewLine);
                    notes.Append(Environment.NewLine);
                    notes.Append(desc);
                }

                r["tskdesc"] = description;
                updated = true;
            }

            if (Convert.ToString(r["tskgroup"]).ToUpper() != taskGroup.ToUpper())
            {
                if (taskGroup == String.Empty)
                    r["tskgroup"] = DBNull.Value;
                else
                    r["tskgroup"] = taskGroup;
                updated = true;
            }

            if (Convert.ToString(r["tskmsplan"]).ToUpper() != plan.ToUpper())
            {
                if (plan == String.Empty)
                    r["tskmsplan"] = DBNull.Value;
                else
                    r["tskmsplan"] = plan;
                updated = true;
            }

            if (Common.ConvertDef.ToInt32(r["tskmsstage"], 0) != stage)
            {
                if (stage <= 0)
                    r["tskmsstage"] = DBNull.Value;
                else
                    r["tskmsstage"] = stage;
                updated = true;
            }

            if (r["feeusrid"] == DBNull.Value)
            {
                r["feeusrid"] = file.PrincipleFeeEarnerID;
                updated = true;
            }

            if (r["tskmanual"] == DBNull.Value || Common.ConvertDef.ToBoolean(r["tskmanual"], true) != manual)
            {
                r["tskmanual"] = manual;
                updated = true;
            }

            if (team.HasValue)
            {
                if (r["tmid"] == DBNull.Value && team == 0)
                {
                }
                else
                {
                    if (team.Equals(r["tmid"]) == false)
                    {
                        if (team == 0)
                            r["tmid"] = DBNull.Value;
                        else
                            r["tmid"] = team;
                        updated = true;
                    }
                }
            }


            if (assignedTo.HasValue)
            {
                if (r["usrid"] == DBNull.Value && assignedTo.Value == 0)
                {
                }
                else
                {
                    if (assignedTo.Equals(r["usrid"]) == false)
                    {
                        if (assignedTo.Value == 0)
                            r["usrid"] = DBNull.Value;
                        else
                            r["usrid"] = assignedTo;
                        updated = true;
                    }
                }
            }

            if (r["tskcomplete"] == DBNull.Value)
            {
                r["tskcomplete"] = false;
                updated = true;
            }

            if (updated)
            {
                taskschanged = true;
                r["Updated"] = System.DateTime.Now;
                r["UpdatedBy"] = Session.CurrentSession.CurrentUser.ID;
                r.EndEdit();
            }

            Milestones.Task newtask = new Milestones.Task(this, r.Row, null);			

            if (refresh)
            {
                Milestones.MilestoneStage st = CurrentPlan[r];
                if (st != null)
                {
                    st.Tasks.Refresh();
                }

                CurrentPlan.Tasks.Refresh();
            }
            return newtask;
        }

        #endregion

        #region Complete Tasks

        internal void CompleteStageUI(Milestones.MilestoneStage stage)
        {
            if (stage == null)
            {
                throw new ArgumentNullException("stage");
            }

			ArrayList reasons = new ArrayList();
			if (Scriptlet == null)
			{
				CompleteStage(stage, reasons);
			}
			else if (Scriptlet.CanCompleteStageUI(stage, reasons) && Scriptlet.CanCompleteStage(stage, reasons))
			{
				CompleteStage(stage, reasons);
			}
			else
			{
				stage.RaisePropertyChanged("Status");
				throw Utils.ThrowCantCompleteStage(stage.StageNumber, reasons);
			}
        }

        internal void CompleteStage(ErrorOption option, params byte[] stages)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (stages == null)
                return;

            if (stages.Length == 0)
                return;


            Array.Sort(stages);

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();


            foreach (byte stage in stages)
            {
                reasons.Clear();

                try
                {
                    Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];
                    if (st != null)
                    {
                        CompleteStage(st, reasons);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
                Utils.ThrowMultipleExceptions(exceptions);
        }

        internal void CompleteStage(Milestones.MilestoneStage stage, ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (reasons == null)
                reasons = new ArrayList();

            if (stage == null)
                throw new ArgumentNullException("stage");

            if (CanCompleteStage(stage, reasons))
            {
                if (stage.IsCompleted == false)
                    stage.Complete();
            }
            else
            {
                RaisePropertyChanged("Status");
                throw Utils.ThrowCantCompleteStage(stage.StageNumber, reasons);
            }


        }


        internal void CompleteStageTasks(byte stage, ErrorOption option)
        {
            ValidateFile();
            ValidateMilestonePlan();

            stage = ValidateStage(stage);

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];

            if (CanCompleteStage(st, reasons))
            {

                foreach (Milestones.Task tsk in st.Tasks)
                {
                    reasons.Clear();

                    try
                    {
                        CompleteTask(tsk, reasons);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex.Message);
                        if (option == ErrorOption.Fail)
                            break;
                        else
                            continue;
                    }
                }
            }
            else
            {
                throw Utils.ThrowCantCompleteStageTasks(stage, reasons);
            }

            if (exceptions.Count > 0)
            {
                throw Utils.ThrowMultipleExceptions(exceptions);
            }
        }

        internal void CompleteTaskUI(Milestones.Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            new FileActivity(task.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

            ArrayList reasons = new ArrayList();
			if (Scriptlet == null)
			{
				CompleteTask(task, reasons);
			}
            if (Scriptlet != null)
            {
                if (Scriptlet.CanCompleteTaskUI(task, reasons) && Scriptlet.CanCompleteTask(task, reasons))
                {
                    CompleteTask(task, reasons);
                }
                else
                {
                    task.RaisePropertyChanged("IsCompleted"); 
                    throw Utils.ThrowCantCompleteTask(task, reasons);
                }
            }
            else
            {
                task.Complete();
            }
        }

        internal void CompleteTask(ErrorOption option, params string[] filters)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (filters == null)
                return;

            if (filters.Length == 0)
                return;

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.True));

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            foreach (System.Data.DataRowView r in taskvw)
            {
                reasons.Clear();

                try
                {
                    Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                    if (task != null)
                    {
                        CompleteTask(task, reasons);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
            {
                throw Utils.ThrowMultipleExceptions(exceptions);
            }
        }

        internal void CompleteTask(Milestones.Task task, ArrayList reasons)
        {
            ValidateFile();

			if (task.Type.Equals(Configuration.MilestoneTaskConfig.MILESTONE_TASK_TYPE, StringComparison.InvariantCultureIgnoreCase))
				ValidateMilestonePlan();

            if (reasons == null)
                reasons = new ArrayList();

            if (task == null)
                throw new ArgumentNullException("task");

            if (task.IsCompleted == false)
            {
                if (CanCompleteTask(task, reasons))
                    task.Complete();
                else
                    throw Utils.ThrowCantCompleteTask(task, reasons);
            }
        }

        internal void CompleteGroupedTasks(ErrorOption option, params string[] groups)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (groups == null)
                return;

            if (groups.Length == 0)
                return;

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.Null, Common.TriState.True));

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            foreach (System.Data.DataRowView r in taskvw)
            {
                reasons.Clear();

                try
                {
                    Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                    if (task != null)
                    {
                        CompleteTask(task, reasons);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
                throw Utils.ThrowMultipleExceptions(exceptions);
        }

        #endregion

        #region UnComplete Tasks

        internal void UnCompleteStageUI(Milestones.MilestoneStage stage)
        {
            if (stage == null)
            {
                throw new ArgumentNullException("stage");
            }

            ArrayList reasons = new ArrayList();

			if (Scriptlet == null)
			{
				UnCompleteStage(stage, reasons);
			}
            else if (Scriptlet.CanUnCompleteStageUI(stage, reasons) && Scriptlet.CanUnCompleteStage(stage, reasons))
            {
                UnCompleteStage(stage, reasons);
            }
            else
            {
                stage.RaisePropertyChanged("Status");
                throw Utils.ThrowCantUnCompleteStage(stage.StageNumber, reasons);
            }
        }

        internal void UnCompleteStage(ErrorOption option, params byte[] stages)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (stages == null)
                return;

            if (stages.Length == 0)
                return;

            Array.Sort(stages);

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            foreach (byte stage in stages)
            {
                reasons.Clear();

                try
                {
                    Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];
                    if (st != null)
                    {
                        UnCompleteStage(st, reasons);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
                throw Utils.ThrowMultipleExceptions(exceptions);
        }

        internal void UnCompleteStage(Milestones.MilestoneStage stage, ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (stage == null)
                throw new ArgumentNullException("stage");

            if (reasons == null)
                reasons = new ArrayList();

            if (CanUnCompleteStage(stage, reasons))
            {
                if (stage.IsCompleted)
                    stage.UnComplete();
            }
            else
                throw Utils.ThrowCantUnCompleteStage(stage.StageNumber, reasons);
        }

        internal void UnCompleteStageTasks(byte stage, ErrorOption option)
        {
            ValidateFile();
            ValidateMilestonePlan();

            stage = ValidateStage(stage);

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            Milestones.MilestoneStage st = CurrentPlan[ValidateStage(stage)];


            if (CanUnCompleteStage(st, reasons))
            {
                foreach (Milestones.Task tsk in st.Tasks)
                {
                    reasons.Clear();

                    try
                    {
                        UnCompleteTask(tsk, reasons);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex.Message);
                        if (option == ErrorOption.Fail)
                            break;
                        else
                            continue;
                    }
                }
            }
            else
                throw Utils.ThrowCantUnCompleteStageTasks(stage, reasons);

            if (exceptions.Count > 0)
                throw Utils.ThrowMultipleExceptions(exceptions);
        }

        internal void UnCompleteTaskUI(Milestones.Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            ArrayList reasons = new ArrayList();
			if (Scriptlet == null)
			{
				UnCompleteTask(task, reasons);
			}
            else if (Scriptlet.CanUnCompleteTaskUI(task, reasons) && Scriptlet.CanUnCompleteTask(task, reasons))
            {
                UnCompleteTask(task, reasons);
            }
            else
            {
                task.RaisePropertyChanged("IsCompleted");
                throw Utils.ThrowCantUnCompleteTask(task, reasons);
            }
        }

        internal void UnCompleteTask(ErrorOption option, params string[] filters)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (filters == null)
                return;

            if (filters.Length == 0)
                return;

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.True));

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            foreach (System.Data.DataRowView r in taskvw)
            {
                reasons.Clear();

                try
                {
                    Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                    if (task != null)
                    {
                        UnCompleteTask(task, reasons);
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
                throw Utils.ThrowMultipleExceptions(exceptions);
        }


        internal void UnCompleteTask(Milestones.Task task, ArrayList reasons)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (task == null)
                throw new ArgumentNullException("task");

            if (reasons == null)
                reasons = new ArrayList();


            if (task.IsCompleted)
            {
                if (CanUnCompleteTask(task, reasons))
                    task.UnComplete();
                else
                    throw Utils.ThrowCantUnCompleteTask(task, reasons);
            }

        }

        internal void UnCompleteGroupedTasks(ErrorOption option, params string[] groups)
        {
            ValidateFile();
            ValidateMilestonePlan();

            if (groups == null)
                return;

            if (groups.Length == 0)
                return;

            FWBS.OMS.Tasks tasks = CurrentFile.Tasks;

            System.Data.DataView taskvw = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.Null, Common.TriState.True));

            ArrayList exceptions = new ArrayList();
            ArrayList reasons = new ArrayList();

            foreach (System.Data.DataRowView r in taskvw)
            {
                reasons.Clear();

                try
                {
                    Milestones.Task task = CurrentPlan.Tasks[Convert.ToInt64(r["tskid"])];
                    if (task != null)
                    {
                        UnCompleteTask(task, reasons);
                    }
                }
                catch (Exception ex)
                {
                    reasons.Add(ex.Message);
                    if (option == ErrorOption.Fail)
                        break;
                    else
                        continue;
                }
            }

            if (exceptions.Count > 0)
                throw Utils.ThrowMultipleExceptions(exceptions);
        }

        #endregion

        #region Removing Tasks

        internal void RestoreGroupedTasks(string[] groups)
        {
            if (groups == null)
                return;

            if (groups.Length == 0)
                return;

            OMSFile file = CurrentFile;
            FWBS.OMS.Tasks tasks = file.Tasks;
            System.Data.DataView vw = tasks.Find(GetTaskGroupFilter(groups, Common.TriState.Null, Common.TriState.Null));
            for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
            {
                vw[ctr]["tskactive"] = true;
            }

        }

        internal void RestoreTask(string[] filters)
        {
            if (filters == null)
                return;

            if (filters.Length == 0)
                return;

            OMSFile file = CurrentFile;
            FWBS.OMS.Tasks tasks = file.Tasks;
            System.Data.DataView vw = tasks.Find(GetTaskFilter(filters, Common.TriState.Null, Common.TriState.Null));
            for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
            {
                vw[ctr]["tskactive"] = true;
            }

        }



        #endregion

        #endregion

        #region Object Events


        /// <summary>
        /// Executes a file event.
        /// </summary>
        /// <param name="ev"></param>
        public void ExecuteFileEvent(OMSFile file, Extensibility.ObjectEventArgs e)
        {
            bool handled = false;
            if (Scriptlet != null)
            {
                Scriptlet.OnFileEvent(e, ref handled);
                if (e.CanCancel && e.Cancel)
                {
                    return;
                }
            }

            if (handled == false)
            {
                if (e.Event == Extensibility.ObjectEvent.Created)
                {
                    // File just created
                    if (e.Sender is OMSFile)
                    {
                        // Generate the Tasks as File is now created.
                        if (CurrentPlan != null)
                        {
                            CurrentPlan.GenerateTasks();
                        }
                    }
                }
                if (e.Event == Extensibility.ObjectEvent.Loaded)
                {
                }
            }
        }



        /// <summary>
        /// Executes a task event.
        /// </summary>
        /// <param name="ev"></param>
        public void ExecuteTaskEvent(Milestones.Task task, Extensibility.ObjectEventArgs e)
        {
            Debug.WriteLine("ExecuteTaskEvent : Task (" + task.Description + ") Event:" + e.ToString());
            bool handled = false;

            if (Scriptlet != null)
            {
                Debug.WriteLine("ExecuteTaskEventFireScript : Task (" + task.Description + ") Event:" + e.ToString());
                Scriptlet.OnTaskEvent(e, ref handled);
                if (e.CanCancel && e.Cancel)
                {
                    Debug.WriteLine("ExecuteTaskEventFireScriptReturn : Task (" + task.Description + ") Event:" + e.ToString());
                    return;
                }
            }

            if (handled == false)
            {
                if (e.Event == Extensibility.ObjectEvent.ValueChanged)
                {
                    Debug.WriteLine("ExecuteTaskEventHandledValueChanged : Task (" + task.Description + ") Event:" + e.ToString());
                    ValueChangedEventArgs changed = e.Arguments as ValueChangedEventArgs;
                    if (changed != null)
                    {
                        Debug.WriteLine("ExecuteTaskEventHandledValueChanged !=NULL : Task (" + task.Description + ") Event:" + e.ToString());
                        if (changed.Name.ToLower().EndsWith("complete"))
                        {
                            if (Common.ConvertDef.ToBoolean(changed.ProposedValue, false) == false)
                            {
                                if (task.IsMilestoneStageTask)
                                {
                                    if (task.Stage.IsCompleted)
                                        task.Stage.UnComplete();
                                }
                            }
                            else
                            {
                                if (task.IsMilestoneStageTask)
                                {
                                    if (AreStageTasksCompleted(task.Stage.StageNumber))
                                    {
                                        task.Stage.Complete();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (e.Event == Extensibility.ObjectEvent.ValueChanging)
                {
                    ValueChangingEventArgs changing = e.Arguments as ValueChangingEventArgs;
                    if (changing != null)
                    {
                        if (changing.Name.ToLower().EndsWith("complete"))
                        {
                            if (Common.ConvertDef.ToBoolean(changing.ProposedValue, false) == false)
                            {

                                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                                if (CanUnCompleteTask(task, reasons) == false)
                                {
                                    Exception ex = Utils.ThrowCantUnCompleteTask(task, reasons);
                                    Utils.MsgBox(ex);
                                    changing.Cancel = true;
                                    return;
                                }
                            }
                            else
                            {
                                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                                if (CanCompleteTask(task, reasons) == false)
                                {
                                    Exception ex = Utils.ThrowCantCompleteTask(task, reasons);
                                    Utils.MsgBox(ex);
                                    changing.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes a milestone event.
        /// </summary>
        /// <param name="ev"></param>
        public void ExecuteMilestoneEvent(Milestones.MilestonePlan plan, Extensibility.ObjectEventArgs e)
        {
            Debug.WriteLine("ExecuteMilestoneEvent : Plan (" + plan.Description + ") Event:" + e.ToString());
            bool handled = false;

            if (Scriptlet != null)
            {
                Scriptlet.OnMilestoneEvent(e, ref handled);
                if (e.CanCancel && e.Cancel)
                    return;
            }

            if (handled == false)
            {
                if (e.Event == Extensibility.ObjectEvent.ValueChanging)
                {
                    ValueChangingEventArgs changing = e.Arguments as ValueChangingEventArgs;

                    if (changing != null)
                    {
                        byte stageno = 0;
                        string name = changing.Name.ToLower();

                        stageno = FMApplication.GetStageFromName(name);

                        stageno = ValidateStage(stageno);

                        Milestones.MilestoneStage stage = plan[stageno];

                        if (name.EndsWith("due"))
                        {
                            if (plan != null && stage != null)  //check stage == null when resetting plan
                            {
                                DateTime? propval = (changing.ProposedValue == DBNull.Value ? null : (DateTime?)changing.ProposedValue);
                                DateTime? origval = (changing.OriginalValue == DBNull.Value ? null : (DateTime?)changing.OriginalValue);

                                if (propval.HasValue)
                                {
                                    foreach (Milestones.Task tsk in stage.Tasks)
                                    {
                                        //Only set the proposed value if the due date has not yet been specified.
                                        if (tsk.Due == null && tsk.IsCompleted == false)
                                        {
											if (!plan.Application.Parent.AllowNullDueDate)
											{
												int offset = GetTaskDueOffset(tsk.FilterId);
												tsk.Due = propval.Value.AddDays(-offset);
											}
                                        }
                                    }
                                }
                            }
                        }
                        if (name.EndsWith("achieved"))
                        {

                            if (changing.ProposedValue == DBNull.Value)
                            {
                                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                                if (CanUnCompleteStage(stageno, reasons) == false)
                                {
                                    Exception ex = Utils.ThrowCantUnCompleteStage(stageno, reasons);
                                    Utils.MsgBox(ex);
                                    changing.Cancel = true;
                                    return;
                                }
                            }
                            else
                            {
                                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                                if (CanCompleteStage(stageno, reasons) == false)
                                {
                                    Exception ex = Utils.ThrowCantCompleteStage(stageno, reasons);
                                    Utils.MsgBox(ex);
                                    changing.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            }

        }
		
		internal int GetTaskDueOffset(string filter)
		{	
			if (string.IsNullOrWhiteSpace(filter))
				return 0;

			int ret = 0;

			foreach (MilestoneTaskConfig cnf in Parent.MilestoneTasks)
			{
				if (filter == cnf.TaskFilter)
				{
					ret = cnf.DueDateOffset;
					break; 
				}
			}			

			return ret;
		}

		internal void ExecuteMilestonePlanTasksApplied()
		{
			if (Scriptlet != null)
			{
				Scriptlet.OnMilestonePlanTasksApplied();				
			}
		}

        internal void ExecuteMilestoneResetting(ref bool cancel)
        {
            if (Scriptlet != null)
            {
                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                Scriptlet.OnMilestoneResetting(ref cancel, reasons);

                if (cancel)
                {
                    Exception ex = Utils.ThrowCantResetPlan(reasons);
                    Utils.MsgBox(ex);
                    return;
                }
            }
        }

        internal void ExecuteMilestoneReset()
        {
            if (Scriptlet != null)
            {
                Scriptlet.OnMilestoneReset();
            }
        }

        internal void ExecuteMilestoneRemoving(ref bool cancel)
        {
            if (Scriptlet != null)
            {
                System.Collections.ArrayList reasons = new System.Collections.ArrayList();
                Scriptlet.OnMilestoneRemoving(ref cancel, reasons);

                if (cancel)
                {
                    Exception ex = Utils.ThrowCantRemovePlan(reasons);
                    Utils.MsgBox(ex);
                    return;
                }
            }
        }

        internal void ExecuteMilestoneRemoved()
        {
            if (Scriptlet != null)
            {
                Scriptlet.OnMilestoneRemoved();
            }
        }



        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            UnloadScript();

            _currentFile = null;
            _currentTask = null;
            _currentPlan = null;

        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }


        public event PropertyChangingEventHandler PropertyChanging;
        private void RaisePropertyChanging(string property)
        {
            var ev = PropertyChanging;
            if (ev != null)
                ev(this, new PropertyChangingEventArgs(property));
        }
    }
}
