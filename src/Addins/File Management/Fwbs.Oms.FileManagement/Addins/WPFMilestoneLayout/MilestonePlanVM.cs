using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class ModelProxy : INotifyPropertyChanged
    {
        private MilestonePlanVM _model;

        public MilestonePlanVM Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged("Model");
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }
    }

    public class StageViewStateManager : INotifyPropertyChanged
    {
        public string Description
        {
            get;
            set;
        }


        

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }
        


        public event PropertyChangedEventHandler  PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }

    }

    public class MilestonePlanVM : IDisposable, INotifyPropertyChanged
    {

        internal void UpdateStageState(Milestones.MilestoneStage stage, bool isChecked)
        {
            var viewStage = StageStates.FirstOrDefault(s=>s.Description ==stage.Description);
            if(viewStage == null)
            {
                viewStage = new StageViewStateManager();
                viewStage.Description = stage.Description;

                StageStates.Add(viewStage);
            }
                
            viewStage.IsExpanded = isChecked;
        }

        private ActionsOrderType _actionsOrderType;

        public ActionsOrderType ActionsOrderType
        {
            get { return _actionsOrderType; }
            set
            {
                _actionsOrderType = value;
            }
        }

        public bool ShowAllTaskFlowColumns
        {
            get; set;
        }
        public bool ShowTaskFlowCheckboxes
        {
            get; set;
        }

        private ActionsToDisplay _actionsToDisplay;

        public ActionsToDisplay ActionsToDisplay
        {
            get { return _actionsToDisplay; }
            set
            {
                _actionsToDisplay = value;
            }
        }


        ObservableCollection<StageViewStateManager> _stageStates = new ObservableCollection<StageViewStateManager>();
        public ObservableCollection<StageViewStateManager> StageStates
        {
            get
            {
                return _stageStates;
            }
        }

        public string QuickSearchText
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("SEARCHTXTWPF", "_Quick Search :", "").Text;
            }
        }

        public string StatusText
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("STATUSTXTWPF", "Status :", "").Text;
            }
        }

        public bool IsDirty
        {
            get
            {

                if (Application == null || Application.CurrentPlan == null || Application.CurrentPlan.InternalPlan == null)
                    return false;

                if (string.IsNullOrWhiteSpace(Application.CurrentPlan.InternalPlan.MSPlan)) //If the MSPlan is blank then consider the Milestone Addin not to be dirty to stop it preventing the OMSType window closing and trying to update the msplan.
                    return false;

                if (Application != null && Application.CurrentPlan != null)
                    return Application.CurrentPlan.IsDirty;

                return false;
            }
           
        }

        private readonly ObservableCollection<StatusWrapper> _statusFilters;

        public ObservableCollection<StatusWrapper> StatusFilters
        {
            get { return _statusFilters; }
        }



        private ObservableCollection<IMenuItem> menuItems = new ObservableCollection<IMenuItem>();
        public ObservableCollection<IMenuItem> MenuItems
        {
            get { return menuItems; }
        }

        private Cursor _currentCursor = Cursors.Arrow;
        public Cursor CurrentCursor
        {
          get { return _currentCursor; }
          set 
          {
            _currentCursor = value; 
            RaisePropertyChanged("CurrentCursor");
          }
        }

        private bool? _expandAll;

        public bool? ExpandAll
        {
            get { return _expandAll; }
            set
            {
                _expandAll = value;
                RaisePropertyChanged("ExpandAll");
            }
        }

        UIMenuItem resetApp;
        UIMenuItem assignPlan;
        UIMenuItem removePlan;
        UIMenuItem resetPlan;
        UIMenuItem refreshPlan;
        UIMenuItem expandTasks;
        UIMenuItem contractTasks;
        UIMenuItem printPlan;
        UIMenuItem addTask;

        private void BuildMenu()
        {
            MenuItems.Clear();

            resetApp = new UIMenuItem(){ Command="RESETAPP", Title=GetCultureString("RESETAPP", "Reset Application")};
            assignPlan = new UIMenuItem() { Command = "ASSIGNPLAN", Title = GetCultureString("ASSIGNPLAN", "Assign Plan") };
            removePlan = new UIMenuItem() { Command = "REMOVEPLAN", Title = GetCultureString("REMOVEPLAN", "Remove Plan") };
            resetPlan = new UIMenuItem() { Command = "RESETPLAN", Title = GetCultureString("RESETPLAN", "Reset Plan") };
            refreshPlan = new UIMenuItem() { Command = "REFRESH", Title = GetCultureString("REFRESH", "Refresh") };
            expandTasks = new UIMenuItem() { Command = "EXPAND", Title = GetCultureString("EXPAND", "Expand") };
            contractTasks = new UIMenuItem() { Command = "CONTRACT", Title = GetCultureString("SHRINK", "Shrink") };
            printPlan = new UIMenuItem() { Command = "PRINT", Title = GetCultureString("PRINT", "Print") };
            addTask = new UIMenuItem() { Command = "ADDTASK", Title = GetCultureString("ADDTASK", "Add Task") };

            MenuItems.Add(resetApp);
            MenuItems.Add(new UISeparator());
            MenuItems.Add(assignPlan);
            MenuItems.Add(removePlan);
            MenuItems.Add(resetPlan);
            MenuItems.Add(refreshPlan);
            MenuItems.Add(new UISeparator());
            MenuItems.Add(printPlan);
            MenuItems.Add(expandTasks);
            MenuItems.Add(contractTasks);
            MenuItems.Add(addTask);

            SetMenuState();
        }


        private void SetMenuState()
        {
            if (Application == null)
                return;

            bool manualtasks = Application.Parent.AllowManualTasks && Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.AddManual) && (Application.Scriptlet == null ? true : Application.Scriptlet.CanAddManualTask());

            bool hasPlan = false;
            bool canReset = false;
            bool canResetApp = false;
            bool isNewFile = true;
                if (Application != null)
                {
                   canResetApp = Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.ApplicationReset);

                    hasPlan =  Application.CurrentPlan != null;
                    canReset = Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.MilestonePlanReset);

                    isNewFile = Application.CurrentFile.IsNew;

                }

                resetApp.Enabled = canResetApp;
                assignPlan.Enabled = !hasPlan;
                removePlan.Enabled = hasPlan && canReset;
                resetPlan.Enabled = hasPlan && canReset;
                refreshPlan.Enabled = hasPlan;
                expandTasks.Enabled = hasPlan;
                contractTasks.Enabled = hasPlan;
                printPlan.Enabled = !isNewFile && hasPlan;
                addTask.Visible = !isNewFile && hasPlan && manualtasks;


                if (!new FileActivity(file, FileStatusActivityType.TaskflowProcessing).IsAllowed())
                {
                    resetPlan.Enabled = false;
                    assignPlan.Enabled = false;
                    removePlan.Enabled = false;
                    resetApp.Enabled = false;
                    addTask.Enabled = false;
                }
        }

        private string GetCultureString(string code, string text)
        {
            
            string desc = CodeLookup.GetLookup("SLBUTTON", code);

            if (string.IsNullOrEmpty(desc))
            {
                desc = text;
                CodeLookup.Create("SLBUTTON", code, text, "", CodeLookup.DefaultCulture, true, true, true);
            }
            return desc;
        }

        public MilestonePlanVM()
        {
            SetupCommands();
            BuildMenu();

            _statusFilters = StatusWrapper.GetStatuses();
        }

        #region commands
        private void SetupCommands()
        {
            StageCheckedCommand = new Command<Milestones.MilestoneStage>(OnStageCheckedCommand);
            TaskCheckedCommand = new Command<Milestones.Task>(OnTaskCheckedCommand);
            MenuCommand = new Command<string>(OnMenuCommand);
           
        }

        public ICommand MenuCommand
        {
            get;
            private set;
        }

         private string CheckApplication()
        {
            if (Application == null)
                throw new FMException("NOAPPCONGIF", "There is not a valid %FILE% Management Application configured for the %FILE% type.");
            else
                return Application.Parent.Code;
        }

        public void OnMenuCommand(string command)
          {
            try
            {
                switch (command)
                {

                    case "RESETAPP":
                        {
                            ResetApp();
                            break;
                        }
                    case "ASSIGNPLAN":
                        {
                            AssignPlan();
                            break;
                        }
                    case "REMOVEPLAN":
                        {
                            RemovePlan();
                            break;
                        }

                    case "REFRESH":
                        {
                            CheckApplication();
                            ExpandAll = null;
                            CurrentCursor = Cursors.Wait;
                            Application.CurrentPlan.Refresh(true);

                            break;
                        }
                    case "RESETPLAN":
                        {
                            CheckApplication();

                            bool cancel = false;
                            Application.ExecuteMilestoneResetting(ref cancel);
                            if (cancel)
                                return;

                            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("RESETMSPLAN", "Re-setting the milestone plan will rollback all completed stages and sub tasks. Are you sure that you want to continue?", false) != System.Windows.Forms.DialogResult.Yes)
                                return;

                            CurrentCursor = Cursors.Wait;
                            ExpandAll = null;
                            Application.CurrentPlan.Reset();

                            Application.ExecuteMilestoneReset();

                            break;
                        }
                    case "PRINT":
                        {
                            FWBS.OMS.UI.Windows.Services.Reports.OpenReport("RPTFILTSKMAN", this.file);
                            break;
                        }
                    case "CONTRACT":
                        {

                            ExpandAll = false;
                            break;
                        }
                    case "EXPAND":
                        {
                            ExpandAll = true;
                            break;
                        }
                    case "ADDTASK":
                        {

                            CheckApplication();

                            Milestones.Task task;
                            if (Application.ShowTaskWizard(out task) != ReturnValue.Success)
                                return;
                            Application.CurrentPlan.Update();

                            if (Application.Scriptlet != null)
                                Application.Scriptlet.OnManualTaskAdded(task);


                            break;
                        }
                       
                }
            }
            catch (Exception ex)
            {
                CurrentCursor = Cursors.Arrow;
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
                CurrentCursor = Cursors.Arrow;
            }

        }

        private void RemovePlan()
        {
            CheckApplication();

            bool cancel = false;
            Application.ExecuteMilestoneRemoving(ref cancel);
            if (cancel)
                return;

            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("DELETEMILESTONE", "Are you sure you would like to remove the milestone plan from this file?  This will cause any entered information to be lost.  Are you sure you want to remove this plan?", false) != System.Windows.Forms.DialogResult.Yes)
                return;

            DetachPlanEvents();
            Application.CurrentPlan.Remove();
            Application.CurrentPlan = null;
            Application.ExecuteMilestoneRemoved();

            SetMenuState();
        }

        private void ResetApp()
        {
            CheckApplication();

            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("MSGRESETFMAPP", "Are you sure that you would like to reset the whole taskflow application, this will also reset the current plan?", false) != System.Windows.Forms.DialogResult.Yes)
                return;

            try
            {
                CurrentCursor = Cursors.Wait;
                ExpandAll = null;

                Milestones.MilestonePlan msplan = Application.CurrentPlan;
                if (msplan != null)
                {
                    DetachPlanEvents();
                    msplan.Remove();                    
                    msplan = null;
                }

                Application = FMApplication.ResetApplication(file, Application);

                if (_application.Parent.Code != FMApplication.NO_APP)
                    AssignPlan();
            }
            catch (Exception ex)
            {
                CurrentCursor = Cursors.Arrow;
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
                CurrentCursor = Cursors.Arrow;
            }
        }
        private void AssignPlan()
        {
            string FMCode = "";
            FMCode = CheckApplication();
            FWBS.Common.KeyValueCollection kvc = new Common.KeyValueCollection();
            kvc.Add("FMCode", FMCode);
            FWBS.Common.KeyValueCollection ret = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(OMS.Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.FMSelectMilestone), new System.Drawing.Size(300, 400), null, kvc);

            if (ret == null)
                return;

            bool answer = (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("NEWMILESTONE", "Would you like to base the Milestone start from today?  Select No to create from the File Created date.", false) == System.Windows.Forms.DialogResult.Yes);

            try
            {
                CurrentCursor = Cursors.Wait;

                Milestones.MilestonePlan.Assign(Application, Convert.ToString(ret["planCode"].Value), answer);			

                BuildPlan();
            }
            catch (Exception ex)
            {
                CurrentCursor = Cursors.Arrow;
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
                CurrentCursor = Cursors.Arrow;
            }

        }

        public ICommand TaskCheckedCommand
        {
            get;
            private set;
        }
        public void OnTaskCheckedCommand(Milestones.Task task)
        {
            try
            {
                new FileActivity(task.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

                if (task.IsCompleted)
                    task.Application.UnCompleteTaskUI(task);
                else
                    task.Application.CompleteTaskUI(task);

            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }

        }

        public ICommand StageCheckedCommand
        {
            get;
            private set;
        }
		
        public void OnStageCheckedCommand(Milestones.MilestoneStage stage)
        {
            try
            {

                if (stage.IsCompleted)
                    stage.Application.UnCompleteStageUI(stage);
                else
                    stage.Application.CompleteStageUI(stage);

                                
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }

        }

		public ICommand StageDueDateChanged
		{
			get;
			private set;
		}
		
		public void OnStageDueDateChanged(Milestones.MilestoneStage stage)
		{
			try
			{
				if (stage.IsCompleted)
					stage.Application.UnCompleteStageUI(stage);
				else
					stage.Application.CompleteStageUI(stage);



			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

		}

        #endregion

     
        private Interfaces.IOMSType omsObject;
        private OMSFile file;
        private FMApplicationInstance _application = null;

        public FMApplicationInstance Application
        {
            get { return _application; }
            set
            {
                if (_application == value)
                    return;
                DetachEvents();
                _application = value;
                AttachEvents();
                AttachPlanEvents();
                RaisePropertyChanged("Application");
            }
        }

        private void SelectFirstItem()
        {
            if (Application == null)
                return;

            if (Application.CurrentPlan == null || Application.CurrentPlan.Count() == 0)
                return;

            SelectedStage = Application.CurrentPlan.NextStage;
        }


        private void DetachEvents()
        {
            if (Application == null)
                return;

            Application.PropertyChanging -= Application_PropertyChanging;
            Application.PropertyChanged -= Application_PropertyChanged;
            
        }

        private void AttachEvents()
        {
            if (Application == null)
                return;

            Application.PropertyChanging += Application_PropertyChanging;
            Application.PropertyChanged += Application_PropertyChanged;
            
        }

        void Application_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentPlan":
                    {
                        DetachPlanEvents();

                        break;
                    }
            }
        }

        void CurrentPlan_Dirty(object sender, EventArgs e)
        {
            RaisePropertyChanged("IsDirty");
        }

        void Application_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentPlan":
                    {
                        AttachPlanEvents();

                        break;
                    }
            }
        }

        void AttachPlanEvents()
        {
            if (Application.CurrentPlan == null)
                return;

            Application.CurrentPlan.Dirty += CurrentPlan_Dirty;
        }

       
        void DetachPlanEvents()
        {
            if (Application.CurrentPlan == null)
                return;

            Application.CurrentPlan.Dirty -= CurrentPlan_Dirty;
        }

        private Milestones.MilestoneStage _selectedStage;
        public Milestones.MilestoneStage SelectedStage
        {
            get { return _selectedStage; }
            set
            {
                if (_selectedStage == value)
                    return;
                _selectedStage = value;

                CheckValidTaskSelected();

                RaisePropertyChanged("SelectedStage");
            }
        }

        private void CheckValidTaskSelected()
        {
            if (SelectedTask == null)
                return;

            if (SelectedStage == null)
                return;

            foreach (var task in SelectedStage.Tasks)
            {
                if (SelectedTask == task)
                    return;
            }

            SelectedTask = null;
        }


        private Milestones.Task _selectedTask;
        public Milestones.Task SelectedTask
        {
            get { return _selectedTask; }
            set {
                if (_selectedTask == value)
                    return;
                _selectedTask = value;
                if (_selectedTask != null)
                    SelectedStage = _selectedTask.Stage;
                RaisePropertyChanged("SelectedTask");
            }
        }

        private string _notSetText;
        public string NotSetText
        {
            get { return _notSetText; }
            set
            {
                _notSetText = value;
                RaisePropertyChanged("NotSetText");
            }
        }
        void GetNotSetText()
        {
            if (string.IsNullOrEmpty(NotSetText))
                NotSetText = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not set)", "").Text;
        }


        public void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
        {
            omsObject = obj;
            file = obj as OMSFile;
            IsCheckable = new FWBS.OMS.StatusManagement.FileActivity(file, StatusManagement.Activities.FileStatusActivityType.TaskflowProcessing).IsAllowed();
        }


        private bool _isCheckable;
        private const string IsCheckableProperty = "IsCheckable";
        public bool IsCheckable
        {
            get { return _isCheckable; }
            set
            {


                if (_isCheckable == value)
                    return;
                _isCheckable = value;
                RaisePropertyChanged(IsCheckableProperty);
            }
        }

        public bool Connect(Interfaces.IOMSType obj)
        {

            if (obj is OMSFile)
            {
                file = (OMSFile)obj;
                IsCheckable = new FWBS.OMS.StatusManagement.FileActivity(file, StatusManagement.Activities.FileStatusActivityType.TaskflowProcessing).IsAllowed();
                BuildPlan();
                
                ShowAllTaskFlowColumns = Application.Parent.ShowAllTaskFlowColumns;
                ShowTaskFlowCheckboxes = Application.Parent.ShowTaskFlowCheckboxes;

                return true;
            }
            else
                return false;
        }

        private void BuildPlan()
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return;

            try
            {

                Application = FMApplication.GetApplicationInstance(file);
                
                Application.InitialiseMilestonePlan();

                StageStates.Clear();

                SelectFirstItem();

                SetContextMenuOptions();
                 
            }
            finally
            {

                SetMenuState();
            }

        }

        private void SetContextMenuOptions()
        {
            if (Application == null)
                return;

            if (Application.Parent == null)
                return;

            ActionsOrderType = FMApplication.GetActionsOrderSetting(file, Application.Parent);
            ActionsToDisplay = FMApplication.GetActionsToDisplaySetting(file, Application.Parent);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }



        internal void RefreshItem()
        {
            if (Application != null && Application.CurrentPlan != null)
                Application.CurrentPlan.Refresh(true);

            IsCheckable = new FWBS.OMS.StatusManagement.FileActivity(this.file, StatusManagement.Activities.FileStatusActivityType.TaskflowProcessing).IsAllowed();
        }

        internal void UpdateItem()
        {
            if (IsDirty)
                Application.CurrentPlan.Update();

            IsCheckable = new FWBS.OMS.StatusManagement.FileActivity(this.file, StatusManagement.Activities.FileStatusActivityType.TaskflowProcessing).IsAllowed();
        }

        internal void CancelItem()
        {
            if (Application != null && Application.CurrentPlan != null)
                Application.CurrentPlan.Cancel();
        }

        public void Dispose()
        {
            DetachPlanEvents();
            DetachEvents();
        }

        internal void ActionFMAction(Action action)
        {
            this.Application.Execute(action);
            
        }
	}

    public interface IMenuItem
    {
    }

    public class UIMenuItem : IMenuItem, INotifyPropertyChanged
    {
        public string Title{get;set;}
        public int Image{get;set;} = -1; // No icons in MatterSphere V2. Return -1 so no icon applied.
        public string Command{get;set;}
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                RaisePropertyChanged("Enabled");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string property)
        {
            var ev = PropertyChanged;
            if (ev != null)
                ev(this, new PropertyChangedEventArgs(property));
        }

        private bool visible =true;
        public bool Visible
        {
            get
            { return visible; }
            set
            {
                visible = value;
                RaisePropertyChanged("Visible");
            }
        }
    }
    public class UISeparator : IMenuItem
    {
    }

    public class StatusWrapper
    {
        public StatusWrapper(FilterStatus status, string resCode, string defText)
        {

            Status = status;
            DisplayName = Session.CurrentSession.Resources.GetResource(resCode, defText,"").Text;
        }

        public string DisplayName { get; private set; }
        public FilterStatus Status { get; private set; }

        public static ObservableCollection<StatusWrapper> GetStatuses()
        {
            var s = new ObservableCollection<StatusWrapper>();

            s.Add(new StatusWrapper(FilterStatus.All,"NOTSET", "(Not Specified)"));
            s.Add(new StatusWrapper(FilterStatus.Due, "MSSTATFLTDUE", "Due"));
            s.Add(new StatusWrapper(FilterStatus.Completed, "MSSTATFLTCOMP", "Completed"));


            return s;
        }
    }

    public enum FilterStatus
    {
        All,
        Due,
        Completed
        
    }
}
