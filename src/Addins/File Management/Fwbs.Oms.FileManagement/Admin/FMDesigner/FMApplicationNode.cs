using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;


    internal class FMApplicationNode : FMNode
    {
        #region Properties
        internal Dictionary<string, MilestonePlanNode> MilestonePlanNodes = new Dictionary<string, MilestonePlanNode>();
        internal FileActionsNode FileActions;
        internal TasksNode DynamicTasks;
        internal FileEventsNode FileEvents;
        internal List<string> Plans = new List<string>();

        internal List<MilestoneTaskNode> MilestoneTaskNodes = new List<MilestoneTaskNode>();

        #endregion

        #region Constructors
        internal FMApplicationNode(FMDesigner designer)
            : base(designer)
        {
            Text = Designer.Configuration.Description;

            ImageKey = "structure";
            SelectedImageKey = this.ImageKey;

            designer.FMTreeView.Nodes.Add(this);

            Configuration = Designer.Configuration;

            ContextMenuStrip = Designer.MenuStrip_AppNode;

            FileActions = CreateFileActionsNode(this);
            DynamicTasks = CreateDynamicTasksNode(this);
            DynamicTasks.ContextMenuStrip = Designer.MenuStrip_MilestoneTaskNode;

            CreateMilestonePlanNodes();

            if (!string.IsNullOrEmpty(Designer.Configuration.Script.Code))
                CreateFileEvents();
        }

        internal void CreateFileEvents()
        {
            FileEvents = CreateFileEventsNode(this);

            List<string> Events = new List<string>();

            foreach (CodeMemberMethod m in new FileManagement.ApplicationScriptType().GetEvents())
            {
                Events.Add(m.Name);
            }

            Events.Sort();
            foreach (String eventName in Events)
            {
                var newNode = FileEvents.CreateFileEventNode(eventName);
                if (Designer.codesurface.HasMethod(eventName))
                    newNode.NodeFont = new Font(System.Windows.Forms.TreeView.DefaultFont, FontStyle.Bold);
            }
        }

        internal FileEventsNode CreateFileEventsNode(FMNode parent)
        {
            FileEventsNode node = new FileEventsNode(Designer);
            node.Text = Designer.FMEvents;
            node.Order = 5;
            AddNode(parent, node);

            return node;
        }

        #endregion

        #region Methods
        private FileActionsNode CreateFileActionsNode(FMNode parent)
        {
            FileActionsNode node = new FileActionsNode(Designer);
            node.Text = Designer.GlobalMatterActions;
            node.Order = 1;

            Nodes.Add(node);
            return node;
        }

        private TasksNode CreateDynamicTasksNode(FMNode parent)
        {
            TasksNode node = new TasksNode(Designer);
            node.Text = Designer.DynamicTasks;
            node.Order = 3;
            Nodes.Add(node);
            return node;
        }

        internal MilestonePlanNode GetMilestonePlanNode(string code)
        {
            if (!MilestonePlanNodes.ContainsKey(code))
                return MilestonePlanNodes[Designer.NoPlan];

            return (MilestonePlanNodes[code]);
        }

        internal MilestonePlanNode CreateMilestonePlanNode(string code)
        {
            return CreateMilestonePlanNode(code, null);
        }

        internal MilestonePlanNode CreateMilestonePlanNode(string code, FileActionsNode actionsNode)
        {
            if (string.IsNullOrEmpty(code))
                code = Designer.NoMSPlan;

            if (MilestonePlanNodes.ContainsKey(code))
                return MilestonePlanNodes[code];

            MilestonePlanNode node = new MilestonePlanNode(Designer, code, actionsNode);
            node.Order = 0;
            MilestonePlanNodes.Add(code, node);
            AddNode(node);
            return node;

        }

        internal void CreateMilestonePlanNodes()
        {
            //Create a Node to hold objects that don't have a Plan Set or the Plan was not found
            MilestonePlanNode node = CreateMilestonePlanNode(Designer.NoPlan, FileActions);

            Plans = ScanPlans();
            foreach (string code in Plans)
                CreateMilestonePlanNode(code);
        }

        internal TasksNode GetStaticTasksNode(string plan, byte stage)
        {
            if (string.IsNullOrEmpty(plan))
            {
                plan = Designer.NoPlan;
                stage = 0;
            }

            return GetMilestonePlanNode(plan).GetStageNode(stage).StaticTasks;
        }

        internal void AddPlanToList(string code, ref List<string> plans)
        {
            if (!String.IsNullOrEmpty(code) && !plans.Contains(code))
                plans.Add(code);
        }

        private List<string> ScanPlans()
        {
            List<string> plans = new List<string>();

            AddPlanToList(Designer.Configuration.DefaultMilestonePlan, ref plans);

            foreach (ActionConfig ac in Designer.Configuration.FileActions)
                AddPlanToList(ac.MilestonePlan, ref plans);

            foreach (MilestoneTaskConfig tc in Designer.Configuration.MilestoneTasks)
                AddPlanToList(tc.MilestonePlan, ref plans);

            return plans;
        }

        #endregion

        #region Events
        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case "DefaultMilestonePlan":
                    Designer.FMTreeView.SelectedNode = CreateMilestonePlanNode(Designer.Configuration.DefaultMilestonePlan);
                    break;
                case "Description":
                    this.Text = Designer.Configuration.Description;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }

}

