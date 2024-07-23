using System;
using System.Windows.Forms;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FileManagement.Addins
{
    public class Tasks : FWBS.OMS.UI.Windows.ucBaseAddin
	{
		#region Fields

		private FMApplicationInstance _application = null;
		private Interfaces.IOMSType _object = null;
        private OMSFile _currentFile = null;
		private string _searchlistcode = "";

		#endregion

		#region Controls

		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private FWBS.OMS.UI.Windows.ucNavCommands fileActions;
        private System.ComponentModel.IContainer components;
		private ucPanelNav pnlFileActions;

		
		#endregion

		#region Events
	
		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the user control.
		/// </summary>
		public Tasks()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (_currentFile != null)
                {
                    DettachEvents(_currentFile);
                    _currentFile = null;
                }
                
                if (ucSearchControl1 != null)
                {
                    ucSearchControl1.NewOMSTypeWindow -= new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
                    ucSearchControl1.Dispose();
                }
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlFileActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.fileActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlFileActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Controls.Add(this.pnlFileActions);
            this.pnlDesign.Controls.SetChildIndex(this.pnlFileActions, 0);
            this.pnlDesign.Controls.SetChildIndex(this.pnlActions, 0);
            // 
            // pnlActions
            // 
            this.pnlActions.Location = new System.Drawing.Point(8, 39);
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.ButtonPanelVisible = false;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Location = new System.Drawing.Point(168, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.Size = new System.Drawing.Size(672, 490);
            this.ucSearchControl1.TabIndex = 8;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            this.ucSearchControl1.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.ucSearchControl1_ItemHover);
            this.ucSearchControl1.ItemHovered += new System.EventHandler(this.ucSearchControl1_ItemHovered);
            this.ucSearchControl1.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this.ucSearchControl1_SearchCompleted);
            this.ucSearchControl1.SearchListLoad += new System.EventHandler(this.ucSearchControl1_SearchListLoad);
            // 
            // pnlFileActions
            // 
            this.pnlFileActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(254)))));
            this.pnlFileActions.BlendColor1 = System.Drawing.Color.Empty;
            this.pnlFileActions.BlendColor2 = System.Drawing.Color.Empty;
            this.pnlFileActions.Controls.Add(this.fileActions);
            this.pnlFileActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFileActions.ExpandedHeight = 31;
            this.pnlFileActions.GlobalScope = true;
            this.pnlFileActions.HeaderColor = System.Drawing.Color.Empty;
            this.pnlFileActions.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.pnlFileActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("FILEACTIONS", "%FILE% Actions", ""));
            this.pnlFileActions.ModernStyle = FWBS.OMS.UI.Windows.ucPanelNav.NavStyle.ModernHeader;
            this.pnlFileActions.Name = "pnlFileActions";
            this.pnlFileActions.Size = new System.Drawing.Size(152, 31);
            this.pnlFileActions.TabIndex = 2;
            this.pnlFileActions.TabStop = false;
            this.pnlFileActions.Tag = "FMFILEACTIONS";
            this.pnlFileActions.Text = "%FILE% Actions";
            // 
            // fileActions
            // 
            this.fileActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileActions.Location = new System.Drawing.Point(0, 19);
            this.fileActions.ModernStyle = true;
            this.fileActions.Name = "fileActions";
            this.fileActions.Padding = new System.Windows.Forms.Padding(5);
            this.fileActions.PanelBackColor = System.Drawing.Color.Empty;
            this.fileActions.Size = new System.Drawing.Size(152, 5);
            this.fileActions.TabIndex = 15;
            this.fileActions.TabStop = false;
            // 
            // Tasks
            // 
            this.Controls.Add(this.ucSearchControl1);
            this.Name = "Tasks";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlFileActions.ResumeLayout(false);
            this.pnlFileActions.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		#endregion

		#region Properties


		private ucNavPanel ActionsContainer
		{
			get
			{
				foreach (Control ctrl in pnlActions.Controls)
				{
					if (ctrl is ucNavPanel)
						return (ucNavPanel)ctrl;
				}

				return null;
			}
		}

        private ucPanelNav FileActionsPanel
        {
            get
            {
                ucPanelNav pnl = GetPanel("pnlFileActions");
                if (pnl == null)
                    pnl = pnlFileActions;
                return pnl;
            }
        }

        private ucNavPanel FileActionsContainer
        {
            get
            {
                Control pnlActions = FileActionsPanel;

                if (pnlActions != null)
                {
                    foreach (Control ctrl in pnlActions.Controls)
                    {
                        if (ctrl is ucNavPanel)
                            return (ucNavPanel)ctrl;
                    }
                }

                return null;
            }
        }


		#endregion

		#region IOMSTypeAddin Implementation

		public override void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
		{
			if (obj is User)
			{
                pnlFileActions.GlobalScope = false; //RA Fix for task actions always showing in command centre
			}	   
			else if (obj is OMSFile)
			{
				OMSFile file = (OMSFile)obj;

                if (_currentFile != file)
                {
                    if (_currentFile != null)
                        DettachEvents(_currentFile);
                    AttachEvents(file);
                }

				_object = obj;

				pnlFileActions.Visible = true;
				Parent.ParentChanged += (sender, e) => { if ((sender as TabPage)?.Parent != null) RefreshActions(); };
			}
			else
				pnlFileActions.Visible = false;

            EnableButtons();
		}

		public override bool Connect(Interfaces.IOMSType obj)
		{
			if (obj is User )
			{
				_object = obj;
				if (Session.CurrentSession.IsLoggedIn)
				{
                    _searchlistcode = FMApplication.GetTasksSearchListOverride(obj);

                    if (String.IsNullOrEmpty(_searchlistcode))
                        _searchlistcode = Session.CurrentSession.DefaultSystemSearchList("SCHMYTASKS");
                    
					if (String.IsNullOrEmpty(_searchlistcode))
                        _searchlistcode = "SCHUSRTASKS";
				}
			}
			else if (obj is OMSFile)
			{
				OMSFile file = (OMSFile)obj;

				_object = obj;
                if (Session.CurrentSession.IsLoggedIn)
                {
                    _searchlistcode = _application.Parent.TasksSearchListOverride;

                    if (String.IsNullOrEmpty(_searchlistcode))
                        _searchlistcode = Session.CurrentSession.DefaultSystemSearchListGroups("SCHTASKSOVERDUE");

                    if (String.IsNullOrEmpty(_searchlistcode))
                        _searchlistcode = "SCHFILTASKDUE";
                }

			}
			else 
				return false;

			ucSearchControl1.SearchButtonCommands -=new SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);
			ucSearchControl1.SearchButtonCommands +=new SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);
			ucSearchControl1.NewOMSTypeWindow -=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
			ucSearchControl1.NewOMSTypeWindow +=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
			ucSearchControl1.ItemHovered -=new EventHandler(ucSearchControl1_ItemHovered);
			ucSearchControl1.ItemHovered +=new EventHandler(ucSearchControl1_ItemHovered);


			ToBeRefreshed=true;
			return true;
		}

		public override void RefreshItem()
		{
			if (ToBeRefreshed)
			{
				if (_object != null)
				{
                    if (ucSearchControl1.SearchList == null)
                    {
                        ucSearchControl1.SetSearchList(_searchlistcode, _object, null);
                        ucSearchControl1.ShowPanelButtons();
                    }
					ucSearchControl1.Search();
				}
					
				navCommands.Refresh();

				RefreshActions();
			}
			ToBeRefreshed=false;

			
		}
		
		public override void SelectItem()
		{
			if (ucSearchControl1.SearchList == null)
			{
				ucSearchControl1.SetSearchList(_searchlistcode,false,_object, null);
			}
			
			if (ucSearchControl1.SearchList.Style == FWBS.OMS.SearchEngine.SearchListStyle.List)
				ucSearchControl1.Search();
			
			if (ucSearchControl1.SearchList != null)
				ucSearchControl1.ShowPanelButtons();

		}


		#endregion

		#region Captured Events

        private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
        {
            try
            {

                e.Cancel = true;

                System.Collections.Generic.List<long> taskids = new System.Collections.Generic.List<long>();
                FWBS.Common.KeyValueCollection[] items = ucSearchControl1.SelectedItems;

                for (int ctr = 0; ctr < items.Length; ctr++)
                {
                    taskids.Add(Convert.ToInt64(items[ctr]["tskid"].Value));
                }

                switch (e.ButtonName.ToUpper())
                {
                    case "BTNASSIGN":
                        {
                            if (AssignTasks(taskids))
                            {
                                ucSearchControl1.RefreshItem();
                                Addin.RefreshApplications();
                            }
                        }
                        break;
                    case "BTNUNASSIGN":
                        {
                            if (taskids.Count > 0)
                            {
                                UnassignTasks(taskids);
                                ucSearchControl1.RefreshItem();
                                Addins.Addin.RefreshApplications();
                            }
                        }
                        break;
                    case "BTNADDMILESTONETASK":
                        {
                            AddMilestoneTask();
                        }
                        break;
                    case "BTNADD":
                        {
                            AddTask();
                        }
                        break;
                    case "BTNEDIT":
                        {
                            if (taskids.Count > 0)
                                EditTask(taskids[0]);
                        }
                        break;
                    case "BTNDELETE":
                        {
                            if (taskids.Count > 0)
                            {
                                var needRefresh = DeleteTask(taskids[0]);
                                if(needRefresh)
                                    ucSearchControl1.RefreshItem();
                            }
                        }
                        break;
                    case "BTNCOMPLETE":
                        {
                            if (taskids.Count > 0)
                            {
                                CompleteTask(taskids[0]);
                                ucSearchControl1.RefreshItem();
                            }
                        }
                        break;
                    default:
                        e.Cancel = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

		private void ucSearchControl1_SearchCompleted(object sender, FWBS.OMS.UI.Windows.SearchCompletedEventArgs e)
		{
            ucSearchControl1_ItemHovered(sender, EventArgs.Empty);
		}

		private void ucSearchControl1_SearchListLoad(object sender, System.EventArgs e)
		{
		}


        private int retryCounter = 0;
        private void ucSearchControl1_ItemHover(object sender, FWBS.OMS.UI.Windows.SearchItemHoverEventArgs e)
        {
            try
            {

                Cursor = Cursors.WaitCursor;

                EnableButtons();

                FWBS.Common.KeyValueCollection[] items = ucSearchControl1.SelectedItems;

                if (items.Length > 0)
                {

                    System.Collections.Generic.List<long> taskids = new System.Collections.Generic.List<long>();


                    for (int ctr = 0; ctr < items.Length; ctr++)
                    {
                        taskids.Add(Convert.ToInt64(items[ctr]["tskid"].Value));
                    }

                    GetTask(taskids);
                    ContextMenu ctxMenu = ucSearchControl1.dgSearchResults.ContextMenu;
                    MilestonePlanOld.BuildTaskContextMenu(FileActionsContainer, ActionsContainer, ctxMenu, new EventHandler(btn_Click), _object, _application.Parent);
                }

                retryCounter = 0;
            }
            catch (Exception ex)
            {
                if (isTaskRelatedArgumentNullException(ex))
                {
                    if (retryCounter < 5)
                    {
                        retryCounter++;
                        System.Diagnostics.Debug.WriteLine(string.Format("RefreshApplications() - retry attempt {0}", retryCounter), "Tasks - File Management");

                        Addin.RefreshApplications();
                        ucSearchControl1_ItemHover(sender, e);
                    }
                    else
                        ErrorBox.Show((ArgumentNullException)ex);
                }
                else
                    ErrorBox.Show(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// Condition to test if the Exception is of ArgumentNull type the parameter is "task"
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static bool isTaskRelatedArgumentNullException(Exception ex)
        {
            return ex is ArgumentNullException && ((ArgumentNullException)ex).ParamName.ToLower() == "task";
        }
        

        private void EnableButtons()
        {
          
            if (_application != null)
            {
                bool manualtasks = _application.Parent.AllowManualTasks && Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.AddManual) && (_application.Scriptlet == null ? true : _application.Scriptlet.CanAddManualTask());

                OMSToolBarButton btnAddMilestoneTask = ucSearchControl1.GetOMSToolBarButton("btnAddMilestoneTask");
                OMSToolBarButton btnOpenDocument = ucSearchControl1.GetOMSToolBarButton("btnOpenDocument");

                if (btnAddMilestoneTask != null)
                    btnAddMilestoneTask.Visible = false;

                if (_object is OMSFile)
                {
                    if (((OMSFile)_object).IsNew == false)
                    {
                        if (btnAddMilestoneTask != null)
                            btnAddMilestoneTask.Visible = manualtasks;
                    }
                }

                try
                {
                    if (btnOpenDocument != null)
                    {
                        ucSearchControl1.GetOMSToolBarButton("btnOpenDocument").Enabled = false;

                        if (Convert.ToString(ucSearchControl1.CurrentItem()["tskType"].Value) == "DOCUMENT")
                            ucSearchControl1.GetOMSToolBarButton("btnOpenDocument").Enabled = true;
                    }
                }
                catch
                {
                   
                }
            }
        }
 
		private void ucSearchControl1_ItemHovered(object sender, EventArgs e)
		{
		}

		private void btn_Click(object sender, EventArgs e)
		{
			try
			{
                CheckApplication();
                Action action = null;
                if (sender is MenuItem)
                    action = (Action)((MenuItem)sender).Tag;
                else
                    action = (Action)((Control)sender).Tag;
               
				_application.Execute(action);
                ucSearchControl1.RefreshItem();
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ex);
			}
		}


		#endregion

		#region Methods

		private void AttachEvents(OMSFile file)
		{
			OMSFile oldfile = file;
			if (_application != null)
			{
				if (_application.CurrentFile != null)
				{
					oldfile = _application.CurrentFile;

				}
			}

			Milestones_OMS2K oldplan = oldfile.MilestonePlan;
			if (oldplan != null)
			{
				oldfile.MilestonePlan.Updated-=new EventHandler(this.PlanUpdated);
			}
				

			Milestones_OMS2K plan = file.MilestonePlan;
			if (plan != null)
			{
				file.MilestonePlan.Updated-=new EventHandler(this.PlanUpdated);
				file.MilestonePlan.Updated+=new EventHandler(this.PlanUpdated);
			}

			FWBS.OMS.Tasks tasks = file.Tasks;
			FWBS.OMS.Tasks oldtasks = oldfile.Tasks;

			if (oldtasks != null)
			{
				oldtasks.Updated-=new EventHandler(this.TasksUpdated);
			}

			if (tasks != null)
			{
				tasks.Updated-=new EventHandler(this.TasksUpdated);
				tasks.Updated+=new EventHandler(this.TasksUpdated);
			}

            _currentFile = oldfile;
		}

		private void DettachEvents(OMSFile file)
		{
			Milestones_OMS2K plan = file.MilestonePlan;
			if (plan != null)
			{
				file.MilestonePlan.Updated-=new EventHandler(this.PlanUpdated);
			}

			FWBS.OMS.Tasks tasks = file.Tasks;
			if (tasks != null)
			{
				tasks.Updated-=new EventHandler(this.TasksUpdated);
			}

            _currentFile = null;
		}

		private bool refreshtasks = false;
		private void TasksUpdated(object sender, EventArgs e)
		{
			if (refreshtasks)
			{
				ucSearchControl1.Search();
				refreshtasks = false;
			}
		}

		private void PlanUpdated(object sender, EventArgs e)
		{
			refreshtasks = true;
			RefreshActions();
		}

		private void RefreshActions()
		{            
            
			if (_object is OMSFile)
			{
				OMSFile file = (OMSFile)_object;

                if (Session.CurrentSession.IsLoggedIn)
                {

                    _application = FMApplication.GetApplicationInstance(file);

                    Action[] actions = _application.GetAvailableActions(null as Milestones.MilestoneStage);

                    RefreshActions(FileActionsPanel, FileActionsContainer, actions, new EventHandler(btn_Click), true);                   

                }
			}
		}       

        private static void RemoveAndDisposeMenus(Menu menu)
        {
            if (menu == null)
                return;

            while (menu.MenuItems.Count > 0)
            {
                Menu mnu = menu.MenuItems[0];
                menu.MenuItems.RemoveAt(0);
                mnu.Dispose();
            }
            menu.MenuItems.Clear();
        }

      

        internal void RefreshActions(FileManagement.Milestones.MilestoneStage stage)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                ucNavPanel container = FileActionsContainer;
                Action[] actions = new Action[0];
                if (_application != null)
                    actions = _application.GetAvailableActions(stage);
                RefreshActions(FileActionsPanel, container, actions, new EventHandler(btn_Click), (ucSearchControl1.SelectedItems.Length == 1));
            }
        }

		#endregion

        #region Actions

        private void AddMilestoneTask()
        {
            try
            {
                new FileActivity(_currentFile, FileStatusActivityType.TaskflowProcessing).Check();
                
                Milestones.Task task;

                CheckApplication();

                if (_application.ShowTaskWizard(out task) == ReturnValue.Success)
                {
                    _application.CurrentPlan.Update();
                    ucSearchControl1.RefreshItem();
                   
                    if (_application.Scriptlet!= null)
                        _application.Scriptlet.OnManualTaskAdded(task);
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                ErrorBox.Show(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected bool AssignTasks(System.Collections.Generic.List<long> taskids)
        {
            bool wasAssigned = false;
            if (taskids.Count > 0)
            {
                UI.Assignment frm = new UI.Assignment();

                frm.ShowDialog();

                try
                {
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        if (frm.TeamId.HasValue == false && frm.AssignedTo.HasValue == false)
                        {
                            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("MSGTASKUNASSIGN", "Are you sure you want to completely unassign the task(s) from both team and user?", "") == DialogResult.No)
                                return wasAssigned;
                        }
                        FWBS.OMS.Tasks.AssignTo(taskids.ToArray(), frm.TeamId, frm.AssignedTo);
                        wasAssigned = true;
                    }

                }
                finally
                {
                    frm.Dispose();
                    frm = null;
                }
            }

            return wasAssigned;
        }

        protected void UnassignTasks(System.Collections.Generic.List<long> taskids)
        {
            if (taskids.Count > 0)
            {
                if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("MSGTASKUNASSUSR", "Are you sure you want to unassign the task(s) from the user?", "") == DialogResult.No)
                        return;
                else
                    FWBS.OMS.Tasks.AssignToUser(taskids.ToArray(), null);
            }
        }

        void itm_Close(object sender, NewOMSTypeCloseEventArgs e)
        {
            ucSearchControl1.RefreshItem();
            Addins.Addin.RefreshApplications();
        }

        private bool IsTaskOwner(Milestones.Task task)
        {

            FeeEarner feeEarner = task.FeeEarner;
            if (feeEarner != null)
            {
                if (Session.CurrentSession.CurrentUser.ID == feeEarner.ID)
                {
                    return true;
                }
            }

            User createdBy = task.CreatedBy;
            if (createdBy != null)
            {
                if (Session.CurrentSession.CurrentUser.ID == createdBy.ID)
                {
                    return true;
                }
            }
            return false;

        }

        private bool IsTeamTask(Milestones.Task task)
        {
            Team team = task.AssignedTeam;

            if (team != null)
            {
                return team.ContainsMember(Session.CurrentSession.CurrentUser.ID);
            }

            return false;
        }

        private Milestones.Task GetTask(System.Collections.Generic.List<long> taskids)
        {

            if (taskids.Count == 1)
            {
                return GetTask(taskids[0]);
            }
            else if (taskids.Count > 1)
            {
                ucNavPanel container = ActionsContainer;

                if (container != null)
                {
                    RefreshActions(pnlActions, container, new Action[0], new EventHandler(this.btn_Click), false);
                }
                RefreshActions(null);

            }

            return null;
        }

        private Milestones.Task GetTask(long taskId)
        {
            Task tsk;
            return GetTask(taskId, out tsk);
        }

        private Milestones.Task GetTask(long taskId, out Task tsk)
        {
            Milestones.Task result = null; 
        
            tsk = Task.GetTask(taskId);

            OMSFile file = tsk.File;

            string appcode = FMApplication.GetApplicationCode(file.CurrentFileType);


            ucNavPanel container = ActionsContainer;

            if (container != null)
            {
                _application = FMApplication.GetApplicationInstance(file);

                CheckApplication();

                Action[] actions = _application.GetAvailableActions(_application.CurrentTask);

                RefreshActions(pnlActions, container, actions, new EventHandler(this.btn_Click), true);

				Milestones.MilestoneStage st = GetStage(tsk);
				
				RefreshActions(st);

                result = _application.CurrentTask;


            }

            if (_currentFile != file)
            {
                if (_currentFile != null)
                    DettachEvents(_currentFile);
                AttachEvents(file);
            }


            return result;

        }

		private Milestones.MilestoneStage GetStage(Task tsk)
		{
			if (tsk == null)
				throw new ArgumentNullException("tsk");

			Milestones.MilestonePlan plan = _application.CurrentPlan;
			if (plan == null)
				return null;

			object stgNo = tsk.GetExtraInfo("tskmsstage");

			if (stgNo == null || stgNo == DBNull.Value)
				return null;


			Milestones.MilestoneStage stage = null;			

			byte stageno = Common.ConvertDef.ToByte(tsk.GetExtraInfo("tskmsstage"), 0);
			if (stageno < 1) 
				stageno = 1;

			if (stageno > plan.Stages) 
				stageno = plan.Stages;
				
			stage = plan[stageno];

			return stage;
		}


        private void AddTask()
        {
            ucOMSItemBase itm = FWBS.OMS.UI.Windows.Services.CreateTask(_object as OMSFile);
            if (itm != null)
            {
                itm.Close += new NewOMSTypeCloseEventHander(itm_Close);
                ucSearchControl1.OpenOMSItem(itm);
            }
        }

        protected bool DeleteTask(long taskId, bool askConfirmation = true)
        {
            Milestones.Task task = GetTask(taskId);
            if (task == null)
                throw new ArgumentNullException("taskId");

            if (task.Type == "MILESTONE")
                new FileActivity(task.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

            bool can = IsTaskOwner(task);

            if (can == false)
            {
                if (IsTeamTask(task))
                {
                    can = Session.CurrentSession.CurrentUser.IsInRoles(new string[] { Milestones.TaskRoles.DeleteTeam, Milestones.TaskRoles.DeleteAny });
                    if (can == false)
                        Session.CurrentSession.CurrentUser.ValidateRoles(Milestones.TaskRoles.DeleteTeam);
                }
                else
                {
                    Session.CurrentSession.CurrentUser.ValidateRoles(Milestones.TaskRoles.DeleteAny);
                }
            }

            if (!askConfirmation || FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("QDELETETASK", "Are you sure that you want to delete the task?") == DialogResult.Yes)
            {
                _application.RemoveTask(task);
                task.Update();
                return true;
            }

            return false;
        }

        protected void CompleteTask(long taskId)
        {
            Milestones.Task task = GetTask(taskId);
            if (task == null)
                throw new ArgumentNullException("taskId");

            if (task.Type == "MILESTONE")
                new FileActivity(task.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

            _application.CompleteTaskUI(task);
            task.Update();
        }

        private void EditTask(long taskId)
        {
            ucSearchControl1.OpenOMSItem(GetTaskForm(taskId));
        }

        protected ucOMSItemBase GetTaskForm(long taskId)
        {
            Task tsk;
            Milestones.Task task = GetTask(taskId, out tsk);

            if (task == null)
                throw new ArgumentNullException("taskId");

            if (task.Type == "MILESTONE")
                new FileActivity(task.CurrentFile, FileStatusActivityType.TaskflowProcessing).Check();

            bool can = IsTaskOwner(task);

            if (can == false)
            {
                if (IsTeamTask(task))
                {
                    can = Session.CurrentSession.CurrentUser.IsInRoles(new string[] { Milestones.TaskRoles.UpdateTeam, Milestones.TaskRoles.UpdateAny });
                    if (can == false)
                        Session.CurrentSession.CurrentUser.ValidateRoles(Milestones.TaskRoles.UpdateTeam);
                }
                else
                {
                    Session.CurrentSession.CurrentUser.ValidateRoles(Milestones.TaskRoles.UpdateAny);
                }
            }

            ucOMSItemBase item = FWBS.OMS.UI.Windows.Services.GetOMSItemControl(Session.CurrentSession.DefaultSystemForm(SystemForms.TaskEdit), tsk.Parent, tsk, false, null);
            item.Close += new NewOMSTypeCloseEventHander(itm_Close);

            return item;
        }


        private void CheckApplication()
        {
            if (_application == null)
                throw new FMException("NOAPPCONGIF", "There is not a valid %FILE% Management Application configured for the %FILE% type.");
        }

        #endregion

        #region Static
        internal static void RefreshActions(OMSFile omsfile, ucPanelNav parent, ucNavPanel container)
        {
  
            if (omsfile != null)
            {
                if (Session.CurrentSession.IsLoggedIn)
                {

                    FMApplicationInstance _application = FMApplication.GetApplicationInstance(omsfile);

                    Action[] actions = _application.GetAvailableActions(null as FWBS.OMS.FileManagement.Milestones.MilestoneStage);
                    container.AutoSize = false;
                    container.Visible = false;

                    EventHandler evh = delegate(object sender, EventArgs e)
                    {
                        try
                        {
                            Action action = (Action)((Control)sender).Tag;
                            _application.Execute(action);
                        }
                        catch (Exception ex)
                        {
                            FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                        }
                    };

                    FWBS.OMS.FileManagement.Addins.Tasks.RefreshActions(parent, container, actions, evh, true);
                    container.AutoSize = true;
                    container.Visible = true;
                }
            }
        }



        internal static void RefreshActions(ucPanelNav parent, ucNavPanel container, Action[] actions, EventHandler clickEvent, bool visible)
        {

            if (actions == null) actions = new Action[0];

            
            if (container != null)
            {
                container.AutoSize = false;

                ucNavCommands commands = container as ucNavCommands;

                if (commands != null && commands.ImageList == null) commands.ImageList = ImageListSelector.GetImageList();

                for (int ctr = (container.Controls.Count - 1); ctr >= 0; ctr--)
                {
                    Control ctrl = container.Controls[ctr];

                    if (ctrl is FWBS.OMS.UI.Windows.ucNavCmdButtons)
                    {
                        Action act = ctrl.Tag as Action;
                        if (act != null)
                        {
                            bool remove = true;
                            for (int ctr2 = 0; ctr2 < actions.Length; ctr2++)
                            {
                                Action a = actions[ctr2];
                                if (a != null)
                                {

                                    if (a.ActionConfiguration.Application != act.ActionConfiguration.Application)
                                    {
                                        break;
                                    }

                                    if (a.Code.ToLowerInvariant() == act.Code.ToLowerInvariant() 
										&& CompareTasks(a,act)
										&& CompareStages(a,act)
										&& visible == true)
                                    {

                                        actions[ctr2] = null;
                                        remove = false;

                                        if (a.Method.ToLowerInvariant() != act.Method.ToLowerInvariant())
                                        {
                                            ctrl.Tag = a;                                          
                                        }

                                        break;
                                    }
                                }

                            }

                            if (remove)
                            {
                                container.Controls.Remove(ctrl);
                            }
                        }


                    }

                }

                foreach (Action action in actions)
                {
                    if (action != null && visible == true)
                    {
                        FWBS.OMS.UI.Windows.ucNavCmdButtons btn = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
                        btn.ImageList = ImageListSelector.GetImageList();
                        btn.ImageIndex = action.ActionConfiguration.Glyph;
                        btn.Text = action.Description;
                        btn.Tag = action;
                        btn.Click -= clickEvent;
                        btn.Click += clickEvent;
                        btn.Dock = DockStyle.Bottom;
                        container.Controls.Add(btn, true);
						container.ToolTip.SetToolTip(btn, action.Description);
                    }


                }

                container.AutoSize = true;
                container.Refresh();

                if (parent != null)
                    parent.Visible = (container.Controls.Count > 0);
            }
        }

		private static bool CompareStages(Action A, Action B)
		{
			if (A.Stage == null && B.Stage == null)
				return true;

			if (A.Stage == null && B.Stage != null)
				return false;

			if (A.Stage != null && B.Stage == null)
				return false;

			return A.Stage.StageNumber == A.Stage.StageNumber;
		}

		private static bool CompareTasks(Action A, Action B)
		{
			if (A.Task == null && B.Task == null)
				return true;

			if (A.Task == null && B.Task != null)
				return false;

			if (A.Task != null && B.Task == null)
				return false;

			return A.Task.ID == B.Task.ID;
		}
        #endregion

       
    }
}
