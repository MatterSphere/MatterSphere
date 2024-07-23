using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.StatusManagement.Activities;
using FWBS.OMS.UI.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using MsgBox = FWBS.OMS.UI.Windows.MessageBox;

namespace FWBS.OMS.FileManagement.Addins
{
    public class MilestonePlanOld : FWBS.OMS.UI.Windows.ucBaseAddin
    {
        #region Fields

        private System.Windows.Forms.Panel pnlMain;
        private OMSFile file;
        private FMApplicationInstance _application = null;
        private Milestones.MilestonePlan msplan;
        private FWBS.Common.UI.Windows.eCaptionLine eCaptionLine1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labSubject;
        private System.Windows.Forms.Label labDue;
        private System.Windows.Forms.Label labCompleted;
        private System.Windows.Forms.Label label4;
        public FWBS.OMS.UI.Windows.eToolbars menu;
        private Label label2;
        private Label label1;
        private IContainer components;
        internal ContextMenu contextMenuStage;
        internal ContextMenu contextMenuTask;
        private System.Collections.Generic.Dictionary<byte,string> stageLayout = new System.Collections.Generic.Dictionary<byte,string>(20);
        private ucNavCommands fileActions;
        internal ContextMenu contextMenu;
        private ucPanelNav pnlFileActions;

        private Interfaces.IOMSType _object;
        private bool alreadyExpanded = false;
        private byte? stagebeforerefresh;

        #endregion

        #region Constructors

        public MilestonePlanOld()
        {
            InitializeComponent();
            SetMenuImageLists();
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DetachPlanEvents();
                msplan = null;

            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MilestonePlanOld));
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.contextMenuTask = new System.Windows.Forms.ContextMenu();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.menu = new FWBS.OMS.UI.Windows.eToolbars();
            this.eCaptionLine1 = new FWBS.Common.UI.Windows.eCaptionLine();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labDue = new System.Windows.Forms.Label();
            this.labSubject = new System.Windows.Forms.Label();
            this.labCompleted = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlFileActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.fileActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(168, 122);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10, 5, 10, 5);
            this.pnlMain.Size = new System.Drawing.Size(673, 369);
            this.pnlMain.TabIndex = 9;
            // 
            // menu
            // 
            this.menu.BottomDivider = true;
            this.menu.ButtonsXML = resources.GetString("menu.ButtonsXML");
            this.menu.Divider = false;
            this.menu.DropDownArrows = true;
            this.menu.Location = new System.Drawing.Point(168, 0);
            this.menu.Name = "menu";
            this.menu.NavCommandPanel = null;
            this.menu.ShowToolTips = true;
            this.menu.Size = new System.Drawing.Size(673, 70);
            this.menu.TabIndex = 10;
            this.menu.TopDivider = false;
            this.menu.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.ButtonClick);
            // 
            // eCaptionLine1
            // 
            this.eCaptionLine1.Dock = System.Windows.Forms.DockStyle.Top;
            this.eCaptionLine1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eCaptionLine1.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine1.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine1.Location = new System.Drawing.Point(168, 70);
            this.eCaptionLine1.Name = "eCaptionLine1";
            this.eCaptionLine1.Size = new System.Drawing.Size(673, 34);
            this.eCaptionLine1.TabIndex = 9999;
            this.eCaptionLine1.Tag = "Application : {1} - Milestone Plan : {0}";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labDue);
            this.panel1.Controls.Add(this.labSubject);
            this.panel1.Controls.Add(this.labCompleted);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(168, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(673, 18);
            this.panel1.TabIndex = 10000;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(150, 0);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("TEAM", "Team", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 18);
            this.label2.TabIndex = 221;
            this.label2.Text = "Team";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 0);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("AssignedTo", "Assigned To", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 18);
            this.label1.TabIndex = 220;
            this.label1.Text = "Assigned To";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDue
            // 
            this.labDue.Dock = System.Windows.Forms.DockStyle.Right;
            this.labDue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDue.Location = new System.Drawing.Point(381, 0);
            this.resourceLookup1.SetLookup(this.labDue, new FWBS.OMS.UI.Windows.ResourceLookupItem("TargetDate", "Target Date", ""));
            this.labDue.Name = "labDue";
            this.labDue.Size = new System.Drawing.Size(112, 18);
            this.labDue.TabIndex = 217;
            this.labDue.Text = "Target Date";
            this.labDue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSubject
            // 
            this.labSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labSubject.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSubject.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.labSubject, new FWBS.OMS.UI.Windows.ResourceLookupItem("MilestoneDesc", "    Milestone Description", ""));
            this.labSubject.Name = "labSubject";
            this.labSubject.Size = new System.Drawing.Size(493, 18);
            this.labSubject.TabIndex = 216;
            this.labSubject.Text = "    Milestone Description";
            this.labSubject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labCompleted
            // 
            this.labCompleted.Dock = System.Windows.Forms.DockStyle.Right;
            this.labCompleted.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labCompleted.Location = new System.Drawing.Point(493, 0);
            this.resourceLookup1.SetLookup(this.labCompleted, new FWBS.OMS.UI.Windows.ResourceLookupItem("Achieved", "Achieved", ""));
            this.labCompleted.Name = "labCompleted";
            this.labCompleted.Size = new System.Drawing.Size(92, 18);
            this.labCompleted.TabIndex = 218;
            this.labCompleted.Text = "Achieved";
            this.labCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(585, 0);
            this.resourceLookup1.SetLookup(this.label4, new FWBS.OMS.UI.Windows.ResourceLookupItem("DaysLeft", "Days Left", ""));
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 219;
            this.label4.Text = "Days Left";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.pnlFileActions.Name = "pnlFileActions";
            this.pnlFileActions.Size = new System.Drawing.Size(152, 31);
            this.pnlFileActions.TabIndex = 3;
            this.pnlFileActions.Tag = "FMFILEACTIONS";
            this.pnlFileActions.Text = "%FILE% Actions";
            this.pnlFileActions.ModernStyle = ucPanelNav.NavStyle.ModernHeader;
            // 
            // fileActions
            // 
            this.fileActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileActions.Location = new System.Drawing.Point(0, 24);
            this.fileActions.Name = "fileActions";
            this.fileActions.Padding = new System.Windows.Forms.Padding(5);
            this.fileActions.PanelBackColor = System.Drawing.Color.Empty;
            this.fileActions.Size = new System.Drawing.Size(152, 0);
            this.fileActions.TabIndex = 15;
            this.fileActions.ModernStyle = true;
            // 
            // MilestonePlan
            // 
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.eCaptionLine1);
            this.Controls.Add(this.menu);
            this.Name = "MilestonePlan";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.menu, 0);
            this.Controls.SetChildIndex(this.eCaptionLine1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlFileActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #endregion

        #region IOMSTypeAddin Implementation

        public override void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
        {
            _object = obj;
            if (obj is OMSFile)
            {
                file = (OMSFile)obj;
                RefreshActions();
            }
        }

        public override bool Connect(Interfaces.IOMSType obj)
        {         

            if (obj is OMSFile)
            {
                file = (OMSFile)obj;
                BuildPlan();
                return true;
            }
            else
                return false;
        }

        public override void RefreshItem()
        {
            if (ToBeRefreshed)
            {
                if (msplan != null)
                    msplan.Refresh(true);
            }
            ToBeRefreshed = false;
        }

        public override void UpdateItem()
        {
            if (msplan != null)
            {
                if (msplan.IsDirty)
                    msplan.Update();
            }
        }

        public override void CancelItem()
        {
            if (msplan != null)
            {
                if (msplan.IsDirty)
                    msplan.Cancel();
            }
        }

        public override bool IsDirty
        {
            get
            {
                if (msplan != null)
                    return msplan.IsDirty;
                else
                    return false;
            }
        }

     
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

        #region Methods  

        private void SetMenuImageLists()
        {
            this.menu.ImageList = ImageListSelector.GetImageList();
            this.menu.PanelImageList = ImageListSelector.GetImageList();
        }


        private void RefreshActions()
        {
            if (file != null)
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    _application = FMApplication.GetApplicationInstance(file);

                    Action[] actions = _application.GetAvailableActions(null as Milestones.MilestoneStage);

                    Tasks.RefreshActions(FileActionsPanel, FileActionsContainer, actions, new EventHandler(btn_Click), true);

                }
            }
        }

        private void AssignPlanUI()
        {
            CheckApplication();

            FWBS.Common.KeyValueCollection ret = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(OMS.Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.SelectMilestone), new Size(300, 400), null, null);
            bool answer = false;
            if (ret != null)
            {
                if (MsgBox.ShowYesNoQuestion("NEWMILESTONE", "Would you like to base the Milestone start from today?  Select No to create from the File Created date.", false) == DialogResult.Yes)
                    answer = true;

                try
                {
                    alreadyExpanded = false;
                    Cursor = Cursors.WaitCursor;

                    Milestones.MilestonePlan.Assign(_application, Convert.ToString(ret["MSCode"].Value), answer);
                    msplan = null;

                    BuildPlan();
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
        }

        private void SetPlanTitle()
        {
                string notset = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not set)", "").Text;
                eCaptionLine1.Text = String.Format(Convert.ToString(eCaptionLine1.Tag), 
                    (file.MilestonePlan == null || file.MilestonePlan.IsClear || msplan == null ? notset : msplan.Description),
                    (_application.Parent.Code == FMApplication.NO_APP ? notset : _application.Parent.Description));
            
        }

        private void BuildPlan()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                try
                {

                    _application = FMApplication.GetApplicationInstance(file);

                    if (file.MilestonePlan == null || file.MilestonePlan.IsClear)
                    {
                        for (int ctr = pnlMain.Controls.Count - 1; ctr >= 0; ctr--)
                        {
                            MilestoneStage mss = (MilestoneStage)pnlMain.Controls[ctr];
                            mss.UnloadStage();
                            mss.Dispose();
                            pnlMain.Controls.Remove(mss);
                        }
                        SetPlanTitle();
                        panel1.Visible = false;
                    }
                    else
                    {
                        // Store the current MSPlan Layout and Expanded state
                        // Then On this being stored then when init'd then reapply these settings.

                        stageLayout.Clear();
                        foreach (MilestoneStage msorig in pnlMain.Controls)
                        {
                            if (stageLayout.Count == 0)
                                stageLayout.Add(0, msplan.InternalPlan.MSPlan);
                            stageLayout.Add(msorig.CurrentMilestoneStage.StageNumber, msorig.Expanded.ToString());
                        }

                        _application.InitialiseMilestonePlan();

                        DetachPlanEvents();

                        msplan = _application.CurrentPlan;

                        AttachPlanEvents();

                        SetPlanTitle();

                        panel1.Visible = true;
                        pnlMain.Visible = false;

                        for (int ctr = pnlMain.Controls.Count - 1; ctr >= 0; ctr--)
                        {
                            MilestoneStage mss = (MilestoneStage)pnlMain.Controls[ctr];
                            mss.UnloadStage();
                            mss.Dispose();
                            pnlMain.Controls.Remove(mss);
                        }

                        MilestoneStage lastselected = null;

                        for (byte ctr = 1; ctr <= msplan.Stages; ctr++)
                        {
                            MilestoneStage mss = new MilestoneStage();
                            mss.CurrentMilestoneStage = _application.CurrentPlan[ctr];

                            if (ctr == msplan.NextStage.StageNumber)
                            {
                                mss.Expanded = true;
                            }
                            else
                            {
                                if (stageLayout.Count > 0)
                                {
                                    // Restore the settings of the original plan setup.
                                    if (mss.CurrentMilestoneStage.Plan.Code == stageLayout[0])
                                    {
                                        mss.Expanded = Convert.ToBoolean(stageLayout[mss.CurrentMilestoneStage.StageNumber]);
                                    }
                                }
                            }
                            mss.Dock = DockStyle.Top;
                            pnlMain.Controls.Add(mss);
                            mss.BringToFront();
                            mss.SetContextMenu(contextMenuStage, contextMenuTask);

                            if (stagebeforerefresh != null && stagebeforerefresh.Value == mss.CurrentMilestoneStage.StageNumber)
                            {
                                lastselected = mss;
                                stagebeforerefresh = null;
                            }
                        }

                        if (lastselected != null)
                        {
                            if (pnlMain.VerticalScroll.Maximum >= lastselected.Top)
                                pnlMain.VerticalScroll.Value = lastselected.Top;
                            lastselected.Select();
                        }

                        pnlMain.Visible = true;
                    }

                    ToggleDisplayOfTaskFlowColumnLabels(_application.Parent.ShowAllTaskFlowColumns);
                }
                finally
                {
                    RefreshActions(null as Milestones.MilestoneStage);

                    RefreshActions(null as Milestones.Task);

                    EnableButtons();
                }
            }
        }

        private void ToggleDisplayOfTaskFlowColumnLabels(bool visible)
        {
            labCompleted.Visible = visible;
            label4.Visible = visible;
            labDue.Visible = visible;
            label1.Visible = visible;
            label2.Visible = visible;
        }

        internal void RefreshActions(FileManagement.Milestones.MilestoneStage stage)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                ucNavPanel container = FileActionsContainer;

                if (container != null)
                {
                    CheckApplication();

                    Action[] actions = _application.GetAvailableActions(stage);

                    Tasks.RefreshActions(FileActionsPanel, container, actions, new EventHandler(btn_Click), true);
                    BuildContextMenu(container, contextMenuStage, new EventHandler(btn_Click), true);
                }
            }
        }

        internal void RefreshActions(FileManagement.Milestones.Task tsk)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                ucNavPanel taskContainer = ActionsContainer;
                ucNavPanel planContainer = FileActionsContainer;

                if (taskContainer != null)
                {
                    CheckApplication();

                    if (tsk != null)
                    {
                        Action[] actions = _application.GetAvailableActions(tsk);

                        Tasks.RefreshActions(pnlActions, taskContainer, actions, new EventHandler(btn_Click), true);
                    }
                    else
                    {
                        Tasks.RefreshActions(pnlActions, taskContainer, null, new EventHandler(btn_Click), true);
                    }

                    BuildTaskContextMenu(planContainer, taskContainer, contextMenuTask, new EventHandler(btn_Click),_object, _application.Parent);
                }
            }
        }


        private static void RemoveAndDisposeMenus(Menu menu)
        {
            if (menu == null)
                return;

            for (int i=menu.MenuItems.Count-1;i>=0;i--)
            {
                MenuItem mnu = menu.MenuItems[i];
                if (mnu.Tag is Action || (Convert.ToString(mnu.Tag)=="SEPERATOR") || (Convert.ToString(mnu.Tag)=="EMPTY"))
                {
                    menu.MenuItems.Remove(mnu);
                    mnu.Dispose();
                }
            }
        }

        internal static int BuildContextMenu(ucNavPanel container, ContextMenu menu, EventHandler clickEvent, bool clear)
        {

            int affected = 0;

            if (menu != null)
            {
                if (clear)
                {
                    RemoveAndDisposeMenus(menu);
                }

                //Add a seperator if the menu already contains any items. If no item are added we will remove it
                MenuItem seperator = null;                
                if (menu.MenuItems.Count > 0)
                {
                    seperator = CreateSeperator();
                    menu.MenuItems.Add(seperator);
                }

                if (container != null)
                {                    
                    foreach (ucNavCmdButtons ctrl in container.Controls)
                    {
                        if (!(ctrl.Tag is Action))
                            continue;

                        IconMenuItem mnuitem = new IconMenuItem();
                        mnuitem.ImageList = ctrl.ImageList;
                        mnuitem.ImageIndex = ctrl.ImageIndex;
                        mnuitem.Tag = ctrl.Tag;
                        mnuitem.Text = ctrl.Text;
                        mnuitem.Click -= clickEvent;
                        mnuitem.Click += clickEvent;
                        menu.MenuItems.Add(mnuitem);
                        affected++;
                    }
                    
                    //If no items were added then remove the seperator if we added one
                    if (affected == 0 && seperator!=null)
                        menu.MenuItems.Remove(seperator);
                }
            }

            return affected;
        }

        internal static void BuildTaskContextMenu(ucNavPanel planContainer, ucNavPanel taskContainer, ContextMenu menu, EventHandler clickEvent, Interfaces.IOMSType obj, FMApplication app)
        {
            ResourceItem fileActions= Session.CurrentSession.Resources.GetMessage("NOFILEACTIONS", "No %FILE% Actions", "");
            ResourceItem taskActions = Session.CurrentSession.Resources.GetMessage("NOTASKACTIONS", "No Task Actions", "");

            Dictionary<ucNavPanel, ResourceItem> menus = new Dictionary<ucNavPanel, ResourceItem>();

            ActionsToDisplay ActionsToDisplay = FMApplication.GetActionsToDisplaySetting(obj, app);
            ActionsOrderType ActionsOrder = FMApplication.GetActionsOrderSetting(obj,app);

            switch (ActionsToDisplay)
            {
                case ActionsToDisplay.None:
                    break;

                case ActionsToDisplay.FileOnly:
                    menus.Add(planContainer, fileActions);
                    break;

                case ActionsToDisplay.TaskOnly:
                    menus.Add(taskContainer, taskActions);
                    break;

                case ActionsToDisplay.FileAndTask:
                    {
                        switch (ActionsOrder)
                        {
                            case ActionsOrderType.FileActions1st:
                                menus.Add(planContainer, fileActions);
                                menus.Add(taskContainer, taskActions);
                                break;

                            case ActionsOrderType.TaskActions1st:
                                menus.Add(taskContainer, taskActions);
                                menus.Add(planContainer, fileActions);
                                break;

                            default:
                                menus.Add(planContainer, fileActions);
                                menus.Add(taskContainer, taskActions);
                                break;
                        }
                    }

                    break;
            }


            if (menu != null)
            {
                bool clear = true;
                foreach (var mi in menus)
                {
                    if (BuildContextMenu(mi.Key, menu, clickEvent, clear) == 0)
                    {
                        CreateEmptyContextMenuWarning(menu, mi.Value);
                    }
                    clear = false;
                }

            }
        }

        private static void CreateEmptyContextMenuWarning(ContextMenu menu, ResourceItem resource)
        {
            if (menu.MenuItems.Count>0)
                menu.MenuItems.Add(CreateSeperator());
            MenuItem mi = new MenuItem(resource.Text);
            mi.Tag = "EMPTY";
            mi.Enabled = false;
            menu.MenuItems.Add(mi);
        }

        private static MenuItem CreateSeperator()
        {
            MenuItem seperator = new MenuItem("-");
            seperator.Tag = "SEPERATOR";
            seperator.Enabled = false;
            return seperator;
        }

        private void AttachPlanEvents()
        {
            if (msplan != null)
            {
                msplan.Refreshed += new EventHandler(msplan_Refreshed);
                msplan.StageChanged += new FWBS.OMS.FileManagement.Milestones.StageChangedEventHandler(msplan_StageChanged);
                msplan.Dirty += new EventHandler(msplan_Dirty);
            }
        }

        private void DetachPlanEvents()
        {
            if (msplan != null)
            {
                msplan.Refreshed -= new EventHandler(msplan_Refreshed);
                msplan.StageChanged -= new FWBS.OMS.FileManagement.Milestones.StageChangedEventHandler(msplan_StageChanged);
                msplan.Dirty -= new EventHandler(msplan_Dirty);
            }
        }

        private void EnableButtons()
        {
            if (_application != null)
            {
                bool manualtasks = _application.Parent.AllowManualTasks && Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.AddManual) && (_application.Scriptlet == null ? true : _application.Scriptlet.CanAddManualTask());

                OMSToolBarButton btnAssign = menu.GetButton("btnAssign");
                OMSToolBarButton btnResetApp = menu.GetButton("btnResetApp");
                OMSToolBarButton btnRemove = menu.GetButton("btnRemove");
                OMSToolBarButton btnPrint = menu.GetButton("btnPrint");
                OMSToolBarButton btnRefresh = menu.GetButton("btnRefresh");
                OMSToolBarButton btnReset = menu.GetButton("btnReset");
                OMSToolBarButton btnRecalc = menu.GetButton("btnRecalc");
                OMSToolBarButton btnExpand = menu.GetButton("btnExpand");
                OMSToolBarButton btnContract = menu.GetButton("btnContract");
                OMSToolBarButton btnAddTask = menu.GetButton("btnAddTask");

                if (btnAssign != null)
                    btnAssign.Enabled = false;
                if (btnRemove != null)
                    btnRemove.Enabled = false;
                if (btnPrint != null)
                    btnPrint.Enabled = false;
                if (btnRefresh != null)
                    btnRefresh.Enabled = false;
                if (btnReset != null)
                    btnReset.Enabled = false;
                if (btnResetApp != null)
                    btnResetApp.Enabled = false;
                if (btnRecalc != null)
                    btnRecalc.Enabled = false;
                if (btnExpand != null)
                    btnExpand.Enabled = false;
                if (btnContract != null)
                    btnContract.Enabled = false;
                if (btnAddTask != null)
                    btnAddTask.Visible = false;

                if (_application != null)
                {
                    if (btnResetApp != null)
                        btnResetApp.Enabled = Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.ApplicationReset);

                    if (msplan == null)
                    {
                        if (btnAssign != null)
                            btnAssign.Enabled = true;
                    }
                    else
                    {
                        bool canreset = Milestones.TaskRoles.IsInRole(Milestones.TaskRoles.MilestonePlanReset);

                        if (btnRemove != null)
                            btnRemove.Enabled = canreset;
                        if (btnRefresh != null)
                            btnRefresh.Enabled = true;
                        if (btnReset != null)
                            btnReset.Enabled = canreset;
                        if (btnRecalc != null)
                            btnRecalc.Enabled = true;
                        if (btnExpand != null)
                            btnExpand.Enabled = true;
                        if (btnContract != null)
                            btnContract.Enabled = true;

                        if (file.IsNew == false)
                        {
                            if (btnPrint != null)
                                btnPrint.Enabled = true;
                            if (btnAddTask != null)
                                btnAddTask.Visible = manualtasks;
                        }
                    }
                }

                if (!new FileActivity(file, FileStatusActivityType.TaskflowProcessing).IsAllowed())
                {
                    btnReset.Enabled = false;
                    btnAssign.Enabled = false;
                    btnRemove.Enabled = false;
                    btnResetApp.Enabled = false;
                    btnAddTask.Enabled = false;
                }
            }
        }

        
        private void CheckApplication()
        {
            if (_application == null)
                throw new FMException("NOAPPCONGIF", "There is not a valid %FILE% Management Application configured for the %FILE% type.");
        }

        #endregion

        #region Captured Events

        private void CaptureCurrentStageNumber()
        {
            MilestoneStage lastselected = MilestoneStage.LastSelected;
            if (lastselected != null)
                stagebeforerefresh = lastselected.CurrentMilestoneStage.StageNumber;
        }

        private void ScrollCurrentStageIntoView()
        {
            MilestoneStage lastselected = MilestoneStage.LastSelected;
            if (lastselected != null)
            {
                if (pnlMain.VerticalScroll.Maximum >= lastselected.Top)
                    pnlMain.VerticalScroll.Value = lastselected.Top;
            }
        }


        private void msplan_Refreshed(object sender, EventArgs e)
        {
            CaptureCurrentStageNumber();

            BuildPlan();            
        }

        private void msplan_StageChanged(object sender, FWBS.OMS.FileManagement.Milestones.StageChangedEventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (MilestoneStage mss in pnlMain.Controls)
                {
                    if (mss.CurrentMilestoneStage == e.Stage)
                    {
                        mss.LoadStage();
                    }
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


        private void msplan_Dirty(object sender, EventArgs e)
        {
            base.OnDirty(this, EventArgs.Empty);
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
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }


        private void ButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
        {
            try
            {
                switch (Convert.ToString(e.Button.Tag))
                {

                    case "RESETAPP":
                        {
                            CheckApplication();

                            if (MsgBox.ShowYesNoQuestion("MSGRESETFMAPP", "Are you sure that you would like to reset the whole taskflow application, this will also reset the current plan?", false) == DialogResult.Yes)
                            {
                                try
                                {
                                    alreadyExpanded = false;
                                    Cursor = Cursors.WaitCursor;

                                   

                                    if (msplan != null)
                                    {
                                        msplan.Remove();
                                        DetachPlanEvents();
                                        msplan = null;
                                    }

                                   

                                    _application  = FMApplication.ResetApplication(file, _application);

                                    if (_application.Parent.Code != FMApplication.NO_APP)
                                        AssignPlanUI();

                                    SetPlanTitle();
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

                        }
                        break;
                    case "ASSIGNPLAN":
                        {
                            AssignPlanUI();
                        }
                        break;
                    case "REMOVEPLAN":
                        {
                            CheckApplication();

                            bool cancel = false;
                            _application.ExecuteMilestoneRemoving(ref cancel);
                            if (cancel)
                                return;

                            if (MsgBox.ShowYesNoQuestion("DELETEMILESTONE", "Are you sure you would like to remove the milestone plan from this file?  This will cause any entered information to be lost.  Are you sure you want to remove this plan?", false) == DialogResult.Yes)
                            {
                                alreadyExpanded = false;

                                Cursor = Cursors.WaitCursor;
                                msplan.Remove();

                                DetachPlanEvents();

                                msplan = null;

                                //RefreshPlan - Remove should automatically refresh.
                                EnableButtons();

                                _application.ExecuteMilestoneRemoved();

                            }
                        }
                        break;
                    case "REFRESH":
                        {
                            try
                            {
                                CheckApplication();

                                Cursor = Cursors.WaitCursor;
                                msplan.Refresh(true);
                            }
                            catch (Win32Exception)
                            {
                            }
                        }
                        break;
                    case "RESETPLAN":
                        {
                            CheckApplication();

                            bool cancel = false;
                            _application.ExecuteMilestoneResetting(ref cancel);
                            if (cancel)
                                return;

                            if (MsgBox.ShowYesNoQuestion("RESETMSPLAN", "Re-setting the milestone plan will rollback all completed stages and sub tasks. Are you sure that you want to continue?", false) == DialogResult.Yes)
                            {
                                Cursor = Cursors.WaitCursor;

                                alreadyExpanded = false;
                                msplan.Reset();

                                _application.ExecuteMilestoneReset();

                            }
                        }
                        break;
                    case "PRINT":
                        {
                            FWBS.OMS.UI.Windows.Services.Reports.OpenReport("RPTFILTSKMAN", this.file);
                        }
                        break;
                    case "RECALCULATE":
                        {
                            MessageBox.Show("Not Implemented!");
                        }
                        break;
                    case "CONTRACT":
                        {

                            alreadyExpanded = false;
                            Cursor = Cursors.WaitCursor;

                            foreach (MilestoneStage ms in pnlMain.Controls)
                            {
                                if (ms != null)
                                {
                                    ms.Expanded = false;
                                }
                            }

                            ScrollCurrentStageIntoView();
                        }
                        break;
                    case "EXPAND":
                        {
                            Cursor = Cursors.WaitCursor;

                            alreadyExpanded = true;
                            foreach (MilestoneStage ms in pnlMain.Controls)
                            {
                                if (ms != null)
                                {
                                    ms.Expanded = true;
                                }
                            }

                            ScrollCurrentStageIntoView();
                        }
                        break;
                    case "ADDTASK":
                        {

                            CheckApplication();

                            Milestones.Task task;
                            if (_application.ShowTaskWizard(out task) == ReturnValue.Success)
                            {
                                _application.CurrentPlan.Update();

                                if (_application.Scriptlet != null)
                                    _application.Scriptlet.OnManualTaskAdded(task);
                            }
                        }
                        break;
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
        #endregion

    }

}
