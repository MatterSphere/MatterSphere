namespace FWBS.OMS.FileManagement.Design
{
    partial class FMDesigner
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMDesigner));
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.FMTreeView = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.foundNodesLabel = new System.Windows.Forms.Label();
            this.Prev = new System.Windows.Forms.Button();
            this.Next = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.scriptTab = new System.Windows.Forms.TabPage();
            this.codesurface = new FWBS.OMS.Design.CodeBuilder.CodeSurface();
            this.Log = new System.Windows.Forms.TabPage();
            this.LogText = new System.Windows.Forms.TextBox();
            this.MenuStrip_AppNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AppNode_NewPlan = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_FileActionsNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FileActionNode_NewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.FileActionNode_NewWorkflowAction = new System.Windows.Forms.ToolStripMenuItem();
            this.addActionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_ActionNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ActionNode_NewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.ActionNode_DeleteAction = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_MilestoneTaskNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MilestoneTaskNode_NewTaskType = new System.Windows.Forms.ToolStripMenuItem();
            this.MilestoneTaskNode_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_TaskTypeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TaskTypeNode_NewAction = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskTypeNode_NewWorkflowAction = new System.Windows.Forms.ToolStripMenuItem();
            this.TaskTypeNode_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_TasksNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TasksNode_NewMilestoneTask = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip_FileEvents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FileEventsNode_NewWorkflowAction = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.scriptTab.SuspendLayout();
            this.Log.SuspendLayout();
            this.MenuStrip_AppNode.SuspendLayout();
            this.MenuStrip_FileActionsNode.SuspendLayout();
            this.MenuStrip_ActionNode.SuspendLayout();
            this.MenuStrip_MilestoneTaskNode.SuspendLayout();
            this.MenuStrip_TaskTypeNode.SuspendLayout();
            this.MenuStrip_TasksNode.SuspendLayout();
            this.MenuStrip_FileEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.FMTreeView);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(771, 393);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 1;
            // 
            // FMTreeView
            // 
            this.FMTreeView.AllowDrop = true;
            this.FMTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FMTreeView.ImageIndex = 0;
            this.FMTreeView.ImageList = this.imageList1;
            this.FMTreeView.Indent = 20;
            this.FMTreeView.Location = new System.Drawing.Point(0, 50);
            this.FMTreeView.Name = "FMTreeView";
            this.FMTreeView.SelectedImageIndex = 0;
            this.FMTreeView.ShowNodeToolTips = true;
            this.FMTreeView.Size = new System.Drawing.Size(245, 343);
            this.FMTreeView.TabIndex = 1;
            this.FMTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.FMTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            this.FMTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "action");
            this.imageList1.Images.SetKeyName(1, "plan");
            this.imageList1.Images.SetKeyName(2, "stage");
            this.imageList1.Images.SetKeyName(3, "task");
            this.imageList1.Images.SetKeyName(4, "app");
            this.imageList1.Images.SetKeyName(5, "event");
            this.imageList1.Images.SetKeyName(6, "structure");
            this.imageList1.Images.SetKeyName(7, "tasks");
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 27);
            this.resourceLookup.SetLookup(this.button1, new FWBS.OMS.UI.Windows.ResourceLookupItem("REFRESHTREE", "Refresh Tree", ""));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(245, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh Tree";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchTextBox);
            this.panel1.Controls.Add(this.foundNodesLabel);
            this.panel1.Controls.Add(this.Prev);
            this.panel1.Controls.Add(this.Next);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel1.Size = new System.Drawing.Size(245, 27);
            this.panel1.TabIndex = 5;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTextBox.Location = new System.Drawing.Point(0, 0);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(201, 20);
            this.searchTextBox.TabIndex = 3;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // foundNodesLabel
            // 
            this.foundNodesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foundNodesLabel.AutoSize = true;
            this.foundNodesLabel.Location = new System.Drawing.Point(121, 6);
            this.foundNodesLabel.Name = "foundNodesLabel";
            this.foundNodesLabel.Size = new System.Drawing.Size(0, 13);
            this.foundNodesLabel.TabIndex = 6;
            // 
            // Prev
            // 
            this.Prev.Dock = System.Windows.Forms.DockStyle.Right;
            this.Prev.Location = new System.Drawing.Point(201, 0);
            this.Prev.Name = "Prev";
            this.Prev.Size = new System.Drawing.Size(22, 21);
            this.Prev.TabIndex = 4;
            this.Prev.Tag = "Previous";
            this.Prev.Text = "<";
            this.Prev.UseVisualStyleBackColor = true;
            this.Prev.Click += new System.EventHandler(this.Prev_Click);
            // 
            // Next
            // 
            this.Next.Dock = System.Windows.Forms.DockStyle.Right;
            this.Next.Location = new System.Drawing.Point(223, 0);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(22, 21);
            this.Next.TabIndex = 5;
            this.Next.Tag = "Next";
            this.Next.Text = ">";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Prev_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.scriptTab);
            this.tabControl1.Controls.Add(this.Log);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(522, 393);
            this.tabControl1.TabIndex = 0;
            // 
            // scriptTab
            // 
            this.scriptTab.Controls.Add(this.codesurface);
            this.scriptTab.Location = new System.Drawing.Point(4, 22);
            this.resourceLookup.SetLookup(this.scriptTab, new FWBS.OMS.UI.Windows.ResourceLookupItem("SCRIPT", "Script", ""));
            this.scriptTab.Name = "scriptTab";
            this.scriptTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptTab.Size = new System.Drawing.Size(514, 367);
            this.scriptTab.TabIndex = 0;
            this.scriptTab.Text = "Script";
            this.scriptTab.UseVisualStyleBackColor = true;
            // 
            // codesurface
            // 
            this.codesurface.BackColor = System.Drawing.SystemColors.Window;
            this.codesurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codesurface.IsDirty = false;
            this.codesurface.Location = new System.Drawing.Point(3, 3);
            this.codesurface.Name = "codesurface";
            this.codesurface.Size = new System.Drawing.Size(508, 361);
            this.codesurface.TabIndex = 0;
            // 
            // Log
            // 
            this.Log.Controls.Add(this.LogText);
            this.Log.Location = new System.Drawing.Point(4, 22);
            this.resourceLookup.SetLookup(this.Log, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOG", "Log", ""));
            this.Log.Name = "Log";
            this.Log.Padding = new System.Windows.Forms.Padding(3);
            this.Log.Size = new System.Drawing.Size(514, 367);
            this.Log.TabIndex = 1;
            this.Log.Text = "Log";
            this.Log.UseVisualStyleBackColor = true;
            // 
            // LogText
            // 
            this.LogText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogText.Location = new System.Drawing.Point(3, 3);
            this.LogText.Multiline = true;
            this.LogText.Name = "LogText";
            this.LogText.Size = new System.Drawing.Size(508, 361);
            this.LogText.TabIndex = 0;
            // 
            // MenuStrip_AppNode
            // 
            this.MenuStrip_AppNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AppNode_NewPlan});
            this.MenuStrip_AppNode.Name = "AppNodeMenuStrip";
            this.MenuStrip_AppNode.Size = new System.Drawing.Size(125, 26);
            this.MenuStrip_AppNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // AppNode_NewPlan
            // 
            this.resourceLookup.SetLookup(this.AppNode_NewPlan, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewPlan", "New Plan", ""));
            this.AppNode_NewPlan.Name = "AppNode_NewPlan";
            this.AppNode_NewPlan.Size = new System.Drawing.Size(124, 22);
            this.AppNode_NewPlan.Tag = "AppNode_NewPlan";
            this.AppNode_NewPlan.Text = "New Plan";
            this.AppNode_NewPlan.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // MenuStrip_FileActionsNode
            // 
            this.MenuStrip_FileActionsNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileActionNode_NewAction,
            this.FileActionNode_NewWorkflowAction});
            this.MenuStrip_FileActionsNode.Name = "MenuStrip_ActionsNode";
            this.MenuStrip_FileActionsNode.Size = new System.Drawing.Size(191, 48);
            this.MenuStrip_FileActionsNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // FileActionNode_NewAction
            // 
            this.resourceLookup.SetLookup(this.FileActionNode_NewAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewAction", "New Action", ""));
            this.FileActionNode_NewAction.Name = "FileActionNode_NewAction";
            this.FileActionNode_NewAction.Size = new System.Drawing.Size(190, 22);
            this.FileActionNode_NewAction.Tag = "FileActionNode_NewAction";
            this.FileActionNode_NewAction.Text = "New Action";
            // 
            // FileActionNode_NewWorkflowAction
            // 
            this.resourceLookup.SetLookup(this.FileActionNode_NewWorkflowAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewWrkflwAct", "New Workflow Action", ""));
            this.FileActionNode_NewWorkflowAction.Name = "FileActionNode_NewWorkflowAction";
            this.FileActionNode_NewWorkflowAction.Size = new System.Drawing.Size(190, 22);
            this.FileActionNode_NewWorkflowAction.Tag = "FileActionNode_NewWorkflowAction";
            this.FileActionNode_NewWorkflowAction.Text = "New Workflow Action";
            // 
            // addActionToolStripMenuItem
            // 
            this.addActionToolStripMenuItem.Name = "addActionToolStripMenuItem";
            this.addActionToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // MenuStrip_ActionNode
            // 
            this.MenuStrip_ActionNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ActionNode_NewAction,
            this.ActionNode_DeleteAction});
            this.MenuStrip_ActionNode.Name = "MenuStrip_ActionNode";
            this.MenuStrip_ActionNode.Size = new System.Drawing.Size(146, 48);
            this.MenuStrip_ActionNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // ActionNode_NewAction
            // 
            this.resourceLookup.SetLookup(this.ActionNode_NewAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewAction", "New Action", ""));
            this.ActionNode_NewAction.Name = "ActionNode_NewAction";
            this.ActionNode_NewAction.Size = new System.Drawing.Size(145, 22);
            this.ActionNode_NewAction.Tag = "ActionNode_NewAction";
            this.ActionNode_NewAction.Text = "New Action";
            this.ActionNode_NewAction.Visible = false;
            // 
            // ActionNode_DeleteAction
            // 
            this.resourceLookup.SetLookup(this.ActionNode_DeleteAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_DeleteAction", "Delete Action", ""));
            this.ActionNode_DeleteAction.Name = "ActionNode_DeleteAction";
            this.ActionNode_DeleteAction.Size = new System.Drawing.Size(145, 22);
            this.ActionNode_DeleteAction.Tag = "ActionNode_DeleteAction";
            this.ActionNode_DeleteAction.Text = "Delete Action";
            // 
            // MenuStrip_MilestoneTaskNode
            // 
            this.MenuStrip_MilestoneTaskNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MilestoneTaskNode_NewTaskType,
            this.MilestoneTaskNode_Delete});
            this.MenuStrip_MilestoneTaskNode.Name = "MenuStrip_MilestoneTaskNode";
            this.MenuStrip_MilestoneTaskNode.Size = new System.Drawing.Size(151, 48);
            this.MenuStrip_MilestoneTaskNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // MilestoneTaskNode_NewTaskType
            // 
            this.resourceLookup.SetLookup(this.MilestoneTaskNode_NewTaskType, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewTaskType", "New Task Type", ""));
            this.MilestoneTaskNode_NewTaskType.Name = "MilestoneTaskNode_NewTaskType";
            this.MilestoneTaskNode_NewTaskType.Size = new System.Drawing.Size(150, 22);
            this.MilestoneTaskNode_NewTaskType.Tag = "MilestoneTaskNode_NewTaskType";
            this.MilestoneTaskNode_NewTaskType.Text = "New Task Type";
            // 
            // MilestoneTaskNode_Delete
            // 
            this.resourceLookup.SetLookup(this.MilestoneTaskNode_Delete, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_Delete", "Delete", ""));
            this.MilestoneTaskNode_Delete.Name = "MilestoneTaskNode_Delete";
            this.MilestoneTaskNode_Delete.Size = new System.Drawing.Size(150, 22);
            this.MilestoneTaskNode_Delete.Tag = "MilestoneTaskNode_Delete";
            this.MilestoneTaskNode_Delete.Text = "Delete";
            // 
            // MenuStrip_TaskTypeNode
            // 
            this.MenuStrip_TaskTypeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TaskTypeNode_NewAction,
            this.TaskTypeNode_NewWorkflowAction,
            this.TaskTypeNode_Delete});
            this.MenuStrip_TaskTypeNode.Name = "MenuStrip_TaskTypeNode";
            this.MenuStrip_TaskTypeNode.Size = new System.Drawing.Size(191, 70);
            this.MenuStrip_TaskTypeNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // TaskTypeNode_NewAction
            // 
            this.resourceLookup.SetLookup(this.TaskTypeNode_NewAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewAction", "New Action", ""));
            this.TaskTypeNode_NewAction.Name = "TaskTypeNode_NewAction";
            this.TaskTypeNode_NewAction.Size = new System.Drawing.Size(190, 22);
            this.TaskTypeNode_NewAction.Tag = "TaskTypeNode_NewAction";
            this.TaskTypeNode_NewAction.Text = "New Action";
            // 
            // TaskTypeNode_NewWorkflowAction
            // 
            this.resourceLookup.SetLookup(this.TaskTypeNode_NewWorkflowAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewWrkflwAct", "New Workflow Action", ""));
            this.TaskTypeNode_NewWorkflowAction.Name = "TaskTypeNode_NewWorkflowAction";
            this.TaskTypeNode_NewWorkflowAction.Size = new System.Drawing.Size(190, 22);
            this.TaskTypeNode_NewWorkflowAction.Tag = "TaskTypeNode_NewWorkflowAction";
            this.TaskTypeNode_NewWorkflowAction.Text = "New Workflow Action";
            // 
            // TaskTypeNode_Delete
            // 
            this.resourceLookup.SetLookup(this.TaskTypeNode_Delete, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_Delete", "Delete", ""));
            this.TaskTypeNode_Delete.Name = "TaskTypeNode_Delete";
            this.TaskTypeNode_Delete.Size = new System.Drawing.Size(190, 22);
            this.TaskTypeNode_Delete.Tag = "TaskTypeNode_Delete";
            this.TaskTypeNode_Delete.Text = "Delete";
            // 
            // MenuStrip_TasksNode
            // 
            this.MenuStrip_TasksNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TasksNode_NewMilestoneTask});
            this.MenuStrip_TasksNode.Name = "MenuStrip_TasksNode";
            this.MenuStrip_TasksNode.Size = new System.Drawing.Size(179, 26);
            this.MenuStrip_TasksNode.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // TasksNode_NewMilestoneTask
            // 
            this.resourceLookup.SetLookup(this.TasksNode_NewMilestoneTask, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewMlstnTask", "New Milestone Task", ""));
            this.TasksNode_NewMilestoneTask.Name = "TasksNode_NewMilestoneTask";
            this.TasksNode_NewMilestoneTask.Size = new System.Drawing.Size(178, 22);
            this.TasksNode_NewMilestoneTask.Tag = "TasksNode_NewMilestoneTask";
            this.TasksNode_NewMilestoneTask.Text = "New Milestone Task";
            // 
            // MenuStrip_FileEvents
            // 
            this.MenuStrip_FileEvents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileEventsNode_NewWorkflowAction});
            this.MenuStrip_FileEvents.Name = "MenuStrip_TasksNode";
            this.MenuStrip_FileEvents.Size = new System.Drawing.Size(191, 26);
            this.MenuStrip_FileEvents.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
            // 
            // FileEventsNode_NewWorkflowAction
            // 
            this.resourceLookup.SetLookup(this.FileEventsNode_NewWorkflowAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("FM_NewWrkflwAct", "New Workflow Action", ""));
            this.FileEventsNode_NewWorkflowAction.Name = "FileEventsNode_NewWorkflowAction";
            this.FileEventsNode_NewWorkflowAction.Size = new System.Drawing.Size(190, 22);
            this.FileEventsNode_NewWorkflowAction.Tag = "FileEventsNode_NewWorkflowAction";
            this.FileEventsNode_NewWorkflowAction.Text = "New Workflow Action";
            // 
            // FMDesigner
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "FMDesigner";
            this.Size = new System.Drawing.Size(771, 393);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.scriptTab.ResumeLayout(false);
            this.Log.ResumeLayout(false);
            this.Log.PerformLayout();
            this.MenuStrip_AppNode.ResumeLayout(false);
            this.MenuStrip_FileActionsNode.ResumeLayout(false);
            this.MenuStrip_ActionNode.ResumeLayout(false);
            this.MenuStrip_MilestoneTaskNode.ResumeLayout(false);
            this.MenuStrip_TaskTypeNode.ResumeLayout(false);
            this.MenuStrip_TasksNode.ResumeLayout(false);
            this.MenuStrip_FileEvents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage scriptTab;
        internal System.Windows.Forms.TreeView FMTreeView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Log;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button Prev;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label foundNodesLabel;
        private System.Windows.Forms.TextBox LogText;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_AppNode;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_FileActionsNode;
        internal System.Windows.Forms.ToolStripMenuItem addActionToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_ActionNode;
        internal System.Windows.Forms.ToolStripMenuItem ActionNode_DeleteAction;
        private System.Windows.Forms.ToolStripMenuItem ActionNode_NewAction;
        internal System.Windows.Forms.ToolStripMenuItem AppNode_NewPlan;
        private System.Windows.Forms.ToolStripMenuItem FileActionNode_NewAction;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_MilestoneTaskNode;
        private System.Windows.Forms.ToolStripMenuItem MilestoneTaskNode_NewTaskType;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_TaskTypeNode;
        private System.Windows.Forms.ToolStripMenuItem TaskTypeNode_NewAction;
        private System.Windows.Forms.ToolStripMenuItem TaskTypeNode_Delete;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_TasksNode;
        private System.Windows.Forms.ToolStripMenuItem TasksNode_NewMilestoneTask;
        private System.Windows.Forms.ToolStripMenuItem MilestoneTaskNode_Delete;
        internal FWBS.OMS.Design.CodeBuilder.CodeSurface codesurface;
        private System.Windows.Forms.ToolStripMenuItem TaskTypeNode_NewWorkflowAction;
        private System.Windows.Forms.ToolStripMenuItem FileActionNode_NewWorkflowAction;
        internal System.Windows.Forms.ContextMenuStrip MenuStrip_FileEvents;
        private System.Windows.Forms.ToolStripMenuItem FileEventsNode_NewWorkflowAction;
    }
}
