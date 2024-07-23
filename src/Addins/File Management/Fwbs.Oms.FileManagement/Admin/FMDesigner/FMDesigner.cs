using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.Design.CodeBuilder;
    using FWBS.OMS.FileManagement.Configuration;

    public partial class FMDesigner : UserControl
    {
        internal FWBS.OMS.Design.CodeBuilder.ICodeSurface code;

        internal FMApplicationNode AppNode = null;

        private List<FMNode> foundNodes = new List<FMNode>();
        private int foundNodePtr = 0;
        internal Dictionary<string, OldActionsNode> actionsNodes = new Dictionary<string, OldActionsNode>();

        internal List<FMNode> AllNodes = new List<FMNode>();

        internal readonly string NoPlan = "No Plan Specified";
        internal readonly string GlobalMatterActions = "Global Matter Actions";
        internal readonly string PlanMatterActions = "Plan Actions";
        internal readonly string StageMatterActions = "Stage Actions";
        internal readonly string StaticTasks = "Static Tasks";
        internal readonly string DynamicTasks = "Dynamic Tasks";
        internal readonly string NoMSPlan = "No Milestone Plan";
        internal readonly string FMEvents = "Application Events";

        internal List<string> ExpandedNodes = new List<string>();
        internal string LastSelectedNode;
        internal string PreviousSelectedNode;

        internal FMNode CurrentNode = null;

        #region Events

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            if (factor.Width != 1)
            {
                this.FMTreeView.Indent = LogicalToDeviceUnits(20);
                this.FMTreeView.ImageList = FWBS.OMS.UI.Windows.Images.ScaleList(this.imageList1, LogicalToDeviceUnits(new Size(16, 16)));
            }
        }

        public event EventHandler<FMSelectedItemEventArgs> SelectedItemsChanged;

        protected void OnSelectedItemsChanged(object[] items)
        {
            var ev = SelectedItemsChanged;
            if (ev != null)
                ev(this, new FMSelectedItemEventArgs(items));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var fmnode = FMTreeView.SelectedNode as FMNode;

            if (fmnode != null)
                LastSelectedNode = fmnode.FullPath;

            if (fmnode == null || fmnode.Configuration == null)
                OnSelectedItemsChanged(new object[0]);
            else
                OnSelectedItemsChanged(new object[] { fmnode.Configuration });
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var hti = FMTreeView.HitTest(e.Location);
                if (hti == null)
                    return;

                var node = hti.Node as ActionNode;
                if (node != null)
                {
                    var m = node.Configuration.Method;
                    code.GenerateHandler(m, new GenerateHandlerInfo());
                    code.GotoMethod(m);
                    return;
                }

                var node2 = hti.Node as FileEventNode;
                if (node2 != null)
                {
                    string m = node2.Text;
                    code.GenerateHandler(m, new GenerateHandlerInfo());
                    code.GotoMethod(m);
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(new Exception("Cannot create method. Please check the Application has a script set.", ex));
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string search = searchTextBox.Text;
            PerformSearch(search);
        }
        private void PerformSearch(string search)
        {
            //Clear Previous Search
            foreach (var node in foundNodes)
            {
                node.BackColor = FMTreeView.BackColor;
            }
            foundNodes.Clear();
            foundNodesLabel.Text = string.Empty;

            if (search.Length < 2)
                return;

            //Perform Search 
            FindNodes(FMTreeView.Nodes[0], search.ToUpperInvariant());

            //Highlight all nodes found and then select the first node and update the label to show how many matches were found
            foreach (FMNode node1 in foundNodes)
            {
                node1.BackColor = Color.LightGreen;
            }

            if (foundNodes.Count > 0)
            {
                foundNodesLabel.Text = string.Format("{0} of {1}", 1, foundNodes.Count.ToString());
                FMTreeView.SelectedNode = foundNodes[0];
            }
            else
                foundNodesLabel.Text = string.Empty;
        }

        private void config_ScriptChange(object sender, EventArgs e)
        {
            config.NewScript();
            UnloadCode();

            FileEventsNode node = AppNode.FileEvents;
            if (node != null)
                node.RemoveNode(true);

            LoadCode();
            AppNode.CreateFileEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RebuildTree();
        }

        internal IEnumerable<TreeNode> ChildNodes(TreeNode node)
        {
            if (node == null)
                yield break;

            yield return node;
            foreach (TreeNode n in node.Nodes)
            {
                foreach (TreeNode n1 in ChildNodes(n))
                    yield return n1;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            ActionNode node = CurrentNode as ActionNode;

            if (node == null)
                return;

            AppendLog("Milestone Plan: " + node.MilestonePlan);
            AppendLog("Milestone Stage: " + node.MilestoneStage);
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            if (btn.Tag.ToString() == "Previous")
                foundNodePtr--;
            else
                foundNodePtr++;

            int count = foundNodes.Count;
            if (foundNodes == null || count < 1)
                return;

            if (foundNodePtr > count - 1)
                foundNodePtr = 0;

            if (foundNodePtr < 0)
                foundNodePtr = count - 1;

            FMNode node = foundNodes[foundNodePtr];
            FMTreeView.SelectedNode = node;
            node.EnsureVisible();
            FMTreeView.Select();
            foundNodesLabel.Text = string.Format("{0} of {1}", (foundNodePtr + 1).ToString(), count.ToString());
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            CurrentNode = null;
            if (e.Button == MouseButtons.Right)
            {
                TreeViewHitTestInfo hti = FMTreeView.HitTest(e.Location);
                CurrentNode = hti.Node as FMNode;
            }
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((System.Windows.Forms.ContextMenuStrip)sender).Close();
            string message = string.Format("{0}", e.ClickedItem.Tag);

            AppendLog(message);

            string clickedItem = Convert.ToString(e.ClickedItem.Tag);
            switch (clickedItem)
            {
                case "ActionNode_DeleteAction":
                    DeleteNode();
                    break;
                case "ActionNode_NewAction":
                    CreateNewActionNode(CurrentNode);
                    break;
                case "AppNode_NewPlan":
                    AppNodeNewPlan();
                    break;
                case "TaskTypeNode_NewAction":
                case "FileActionNode_NewAction":
                    CreateNewActionNode(CurrentNode);
                    break;
                case "TaskTypeNode_NewWorkflowAction":
                case "FileActionNode_NewWorkflowAction":
                    CreateNewWorkflowActionNode(CurrentNode);
                    break;
                case "MilestoneTaskNode_NewTaskType":
                    CreateNewTaskTypeNode(CurrentNode);
                    break;
                case "MilestoneTaskNode_Delete":
                    DeleteNode();
                    break;
                case "TasksNode_NewTaskType":
                    CreateNewTaskTypeNode(CurrentNode);
                    break;
                case "TaskTypeNode_Delete":
                    DeleteNode();
                    break;
                case "TasksNode_NewMilestoneTask":
                    CreateNewMilestoneTaskNode(CurrentNode);
                    break;
                case "FileEventsNode_NewWorkflowAction":
                    try
                    {
                        var node2 = CurrentNode as FileEventNode;
                        if (node2 != null)
                        {
                            if (!code.HasMethod(node2.Text))
                            {
                                FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.WorkflowPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
                                if (retvals != null)
                                {
                                    var workflowname = Convert.ToString(retvals[0].Value);
                                    string m = node2.Text;
                                    code.GenerateHandler(m, new GenerateHandlerInfo() { Workflow = workflowname });
                                    code.GotoMethod(m);
                                }
                            }
                            else
                                FWBS.OMS.UI.Windows.ErrorBox.Show(new Exception("Event already exists. Please delete the method before Inserting Workflow Event."));
                        }
                    }
                    catch (Exception ex)
                    {
                        FWBS.OMS.UI.Windows.ErrorBox.Show(new Exception("Cannot create method. Please check the Application has a script set.", ex));
                    }
                    break;
            }
        }
        #region Menu Methods

        private void AppNodeNewPlan()
        {
            FWBS.Common.KeyValueCollection ret = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(OMS.Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.SelectMilestone), new Size(300, 400), null, null);
            if (ret == null)
                return;

            string msplan = Convert.ToString(ret["MSCode"].Value);

            FMTreeView.SelectedNode = AppNode.CreateMilestonePlanNode(msplan);

        }

        private void ActionNodeNewAction()
        {
            CreateNewActionNode(CurrentNode);
        }

        internal MilestoneTaskNode CreateNewMilestoneTaskNode(FMNode node)
        {
            //If called from an action node then the Parent Node of the Action is where we need to add the new node to
            FMNode parent = node as MilestoneTaskNode;
            if (parent == null)
                parent = node;
            else
                parent = node.Parent as MilestoneTaskNode;

            MilestoneTaskConfig newMilestoneTaskConfig = new MilestoneTaskConfig(this.Configuration);
            MilestoneTaskNode newNode = new MilestoneTaskNode(this);

            parent.AddNode(newNode);

            //Determine the correct MilestonePlan and MilestoneStage for the Action 
            //based on its position in the tree and then set the nodes Action Configuration
            newMilestoneTaskConfig.MilestonePlan = newNode.MilestonePlan;
            newMilestoneTaskConfig.MilestoneStage = newNode.MilestoneStage;
            newNode.Configuration = newMilestoneTaskConfig;

            Configuration.MilestoneTasks.Add(newMilestoneTaskConfig);

            //Select the new node in the tree
            FMTreeView.SelectedNode = newNode;

            return newNode;
        }

        internal TaskTypeNode CreateNewTaskTypeNode(FMNode node)
        {
            //If called from an action node then the Parent Node of the Action is where we need to add the new node
            FMNode parent = node.GetMilestoneTaskNode();
            if (parent == null)
                parent = node;

            MilestoneTaskConfig mstConf = parent.Configuration as MilestoneTaskConfig;

            TaskTypeConfig newTaskTypeConfig = new TaskTypeConfig(this.Configuration);
            TaskTypeNode newNode = new TaskTypeNode(this);

            if (mstConf != null)
            {
                newTaskTypeConfig.TaskFilter = mstConf.TaskFilter;
                newTaskTypeConfig.TaskGroup = mstConf.TaskGroup;
                newTaskTypeConfig.TaskType = mstConf.TaskType;
            }

            newNode.Configuration = newTaskTypeConfig;
            newNode.Text = newTaskTypeConfig.TaskFilter;

            parent.AddNode(newNode);

            //Add to the task types collection
            Configuration.TaskTypes.Add(newTaskTypeConfig);

            //Select the new node in the tree
            FMTreeView.SelectedNode = newNode;

            return newNode;
        }

        internal ActionNode CreateNewWorkflowActionNode(FMNode node)
        {
            FWBS.Common.KeyValueCollection retvals = FWBS.OMS.UI.Windows.Services.Searches.ShowSearch(null, Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.WorkflowPicker), false, new Size(-1, -1), null, new FWBS.Common.KeyValueCollection());
            if (retvals != null)
            {
                //If called from an action node then the Parent Node of the Action is where we need to add the new node to
                FMNode parent = node as ActionNode;
                if (parent == null)
                    parent = node;
                else
                    parent = node.Parent as FMNode;

                //Create an ActionConfig and the new node and add it to the tree
                ActionConfig newActionConfig = new ActionConfig(this.Configuration);
                ActionNode newActionNode = new ActionNode(this, null); //Do not set the nodes configuration yet as that would Attach the events
                parent.AddNode(newActionNode);

                //Determine the correct MilestonePlan and MilestoneStage for the Action 
                //based on its position in the tree and then set the nodes Action Configuration
                newActionConfig.MilestonePlan = newActionNode.MilestonePlan;
                newActionConfig.MilestoneStage = newActionNode.MilestoneStage;
                newActionNode.Configuration = newActionConfig;

                var workflowname = Convert.ToString(retvals[0].Value);

                newActionNode.Configuration.Method = workflowname;

                //Add the Action config to the relevant collection
                ActionConfigCollection actionsCollection = null;

                if (newActionNode.IsMatterAction)
                    actionsCollection = this.Configuration.FileActions;
                else
                    actionsCollection = node.GetTaskTypeNode().Configuration.Actions;

                if (actionsCollection == null)
                    throw new Exception("Unable to add an Action node. No Action Collection Exists");

                actionsCollection.Add(newActionConfig);

                //Select the new node in the tree
                FMTreeView.SelectedNode = newActionNode;
                this.code.GenerateHandler(workflowname, new GenerateHandlerInfo() { Workflow = workflowname });
                return newActionNode;
            }
            return null;
        }

        internal ActionNode CreateNewActionNode(FMNode node)
        {
            //If called from an action node then the Parent Node of the Action is where we need to add the new node to
            FMNode parent = node as ActionNode;
            if (parent == null)
                parent = node;
            else
                parent = node.Parent as FMNode;

            //Create an ActionConfig and the new node and add it to the tree
            ActionConfig newActionConfig = new ActionConfig(this.Configuration);
            ActionNode newActionNode = new ActionNode(this, null); //Do not set the nodes configuration yet as that would Attach the events
            parent.AddNode(newActionNode);

            //Determine the correct MilestonePlan and MilestoneStage for the Action 
            //based on its position in the tree and then set the nodes Action Configuration
            newActionConfig.MilestonePlan = newActionNode.MilestonePlan;
            newActionConfig.MilestoneStage = newActionNode.MilestoneStage;
            newActionNode.Configuration = newActionConfig;

            //Add the Action config to the relevant collection
            ActionConfigCollection actionsCollection = null;

            if (newActionNode.IsMatterAction)
                actionsCollection = this.Configuration.FileActions;
            else
                actionsCollection = node.GetTaskTypeNode().Configuration.Actions;

            if (actionsCollection == null)
                throw new Exception("Unable to add an Action node. No Action Collection Exists");

            actionsCollection.Add(newActionConfig);

            //Select the new node in the tree
            FMTreeView.SelectedNode = newActionNode;

            return newActionNode;
        }

        internal void DeleteNode()
        {
            DeleteNode(CurrentNode);
        }

        internal void DeleteNode(FMNode node)
        {
            if (node == null)
            {
                throw new Exception("A Node was Expected");
            }

            string message = string.Format("Are you sure you wish to delete the Node: {0}", node.Text);
            DialogResult result = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion(message);
            if (result == DialogResult.Yes)
            {
                node.RemoveNode(false);
            }
        }

        #endregion

        private void addActionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileActionsNode parent = CurrentNode as FileActionsNode;
            if (parent == null)
                return;

            AppendLog(string.Format("Add Action Clicked, on {0}", parent.FullPath));
            AppendLog(string.Format("\t Plan {0}, Stage {1}", parent.MilestonePlan, parent.MilestoneStage));

            ActionConfig ac = new ActionConfig(Configuration);
            ac.MilestonePlan = parent.MilestonePlan;
            ac.MilestoneStage = parent.MilestoneStage;

            Configuration.FileActions.Add(ac);

            ActionNode node = new ActionNode(this, ac);
            parent.AddNode(node);

            FMTreeView.SelectedNode = node;
        }

        #endregion

        public FMDesigner()
        {
            InitializeComponent();
            this.tabControl1.TabPages.Remove(Log);
            FMTreeView.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
            code = codesurface;

            Res res = Session.CurrentSession.Resources;
            NoPlan = res.GetResource("FM_NoPlanSpecif", NoPlan, "").Text;
            GlobalMatterActions = res.GetResource("FM_GloblActions", GlobalMatterActions, "").Text;
            PlanMatterActions = res.GetResource("FM_PlanActions", PlanMatterActions, "").Text;
            StageMatterActions = res.GetResource("FM_StageActions", StageMatterActions, "").Text;
            StaticTasks = res.GetResource("FM_StaticTasks", StaticTasks, "").Text;
            DynamicTasks = res.GetResource("FM_DynamicTasks", DynamicTasks, "").Text;
            NoMSPlan = res.GetResource("FM_NoMlstnPlan", NoMSPlan, "").Text;
            FMEvents = res.GetResource("FM_AppEvents", FMEvents, "").Text;
        }

        private FMApplication config;

        public FMApplication Configuration
        {
            get
            {
                return config;
            }
            set
            {

                var selectednode = FMTreeView.SelectedNode as FMNode;
                if (selectednode != null)
                    selecteditem = selectednode.Name;

                if (config != value)
                {
                    ClearData();
                    UnloadCode();

                    config = value;

                    if (value != null)
                    {
                        LoadCode();
                        PopulateTree();
                        AttachEvents();
                    }
                }
            }
        }

        private string selecteditem;

        #region Methods



        internal void DetachEvents()
        {
            if (config != null)
                config.ScriptChanged -= new EventHandler(config_ScriptChange);

            if (AppNode != null)
                AppNode.DetachEvents();

        }

        internal void AttachEvents()
        {
            if (config != null)
                config.ScriptChanged += new EventHandler(config_ScriptChange);

            if (FMTreeView.Nodes.Count > 0)
            {
                FMApplicationNode appNode = this.FMTreeView.Nodes[0] as FMApplicationNode;
                if (appNode != null)
                    appNode.AttachEvents();
            }

        }

        internal void RebuildTree()
        {
            FMTreeView.BeginUpdate();
            //Detach All Events and Record Which Nodes are Expanded and Which was the last selected
            ExpandedNodes.Clear();
            if (FMTreeView != null && FMTreeView.SelectedNode != null)
                PreviousSelectedNode = FMTreeView.SelectedNode.FullPath;
            foreach (FMNode node in ChildNodes(AppNode))
            {
                node.DetachEvents();
                if (node.IsExpanded)
                    ExpandedNodes.Add(node.FullPath);
            }

            PopulateTree();
            PerformSearch();

            //Expand all Nodes that were previously expanded and select the last selected node if possible
            foreach (FMNode node in ChildNodes(AppNode))
            {
                if (ExpandedNodes.Contains(node.FullPath))
                {
                    node.Expand();
                    node.EnsureVisible();
                }
                if (node.FullPath.Equals(PreviousSelectedNode))
                {
                    FMTreeView.SelectedNode = node;
                }
            }

            if (FMTreeView.SelectedNode != null)
            {
                FMTreeView.SelectedNode.EnsureVisible();
                FMTreeView.SelectedNode.Expand();
            }

            FMTreeView.EndUpdate();
        }

        internal void PopulateTree()
        {
            FMTreeView.BeginUpdate();

            FMTreeView.Nodes.Clear();
            LogText.Clear();

            CreateTreeOutline();
            PopulateTreeData();

            FMTreeView.TreeViewNodeSorter = new FMNode.FMNodeSorter();
            FMTreeView.Sort();

            FMTreeView.EndUpdate();
        }

        internal void PopulateTreeData()
        {
            AppendLog("Populate Tree Data");
            CreateMilestoneTasks();
            CreateFileActions();
            CreateTaskTypes();
            AppNode.Expand();
        }

        internal void CreateTaskTypes()
        {
            AppendLog("Creating Task Types");
            foreach (TaskTypeConfig conf in Configuration.TaskTypes)
            {
                bool matched = false;
                foreach (MilestoneTaskNode node in AppNode.MilestoneTaskNodes)
                {
                    if (node.Configuration.TaskFilter.ToUpperInvariant() == conf.TaskFilter.ToUpperInvariant())
                    {
                        matched = true;
                        node.CreateTaskTypeNode(conf);
                    }
                }
                if (!matched)
                {
                    MilestoneTaskNode mstn = new MilestoneTaskNode(this);
                    mstn.CreateTaskTypeNode(AppNode.DynamicTasks, conf);
                }

            }

        }

        internal void CreateFileActions()
        {
            AppendLog("Creating File Actions");
            foreach (ActionConfig conf in Configuration.FileActions)
                AppNode.GetMilestonePlanNode(conf.MilestonePlan).GetStageNode(conf.MilestoneStage).FileActions.CreateActionNode(conf);
        }

        internal void CreateMilestoneTasks()
        {
            AppendLog("Creating Milestone Tasks");
            foreach (MilestoneTaskConfig conf in Configuration.MilestoneTasks)
                AppNode.GetMilestonePlanNode(conf.MilestonePlan).GetStageNode(conf.MilestoneStage).StaticTasks.CreateTaskNode(conf);
        }

        internal FMApplicationNode CreateTreeOutline()
        {
            AppendLog("Creating Tree Outline");
            AppNode = new FMApplicationNode(this);

            return AppNode;
        }

        internal void AppendLog(string log)
        {
            bool isLog = true;
            if (!isLog)
                return;
            LogText.AppendText(log + Environment.NewLine);
        }

        internal void ClearTreeData()
        {
            FMTreeView.Nodes.Clear();
        }

        internal void ClearListData()
        {
            foundNodes.Clear();
            if (AppNode != null)
            {
            }

        }

        internal void ClearData()
        {
            this.DetachEvents();
            ClearTreeData();
            ClearListData();
        }

        private void FindNodes(TreeNode node, string text)
        {
            foundNodes.Clear();
            foundNodePtr = 0;
            foreach (TreeNode n in ChildNodes(node))
            {
                if (n.Text.ToUpperInvariant().Contains(text))
                    foundNodes.Add((FMNode)n);
            }

            return;
        }

        private void UnloadCode()
        {
            if (config != null)
            {
                code.Unload();
            }
        }

        private void LoadCode()
        {
            if (!code.IsInitialized)
                code.Init(this.ParentForm);

            var st = new FileManagement.ApplicationScriptType();
            st.SetAppObject(config);

            if (!String.IsNullOrEmpty(config.ScriptName) && !config.Script.AdvancedScript)
                config.Script.ConvertToAdvanced();

            code.Load(st, config.Script);
        }

        #endregion
    }

    public class FMSelectedItemEventArgs : EventArgs
    {
        private readonly object[] items;
        public FMSelectedItemEventArgs(object[] selectedItems)
        {
            if (selectedItems == null)
                selectedItems = new object[0];

            this.items = selectedItems;
        }

        public object[] SelectedItems
        {
            get
            {
                return this.items;
            }
        }
    }
}
