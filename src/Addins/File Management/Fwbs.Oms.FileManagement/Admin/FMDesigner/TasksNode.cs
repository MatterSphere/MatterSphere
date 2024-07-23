namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;

    internal class TasksNode : FMNode
    {

        public TasksNode(FMDesigner designer)
            : base(designer)
        {
            Text = "Static Tasks";
            ImageKey = "tasks";
            SelectedImageKey = this.ImageKey;
            ContextMenuStrip = Designer.MenuStrip_TasksNode;
        }

        internal MilestoneTaskNode CreateTaskNode(MilestoneTaskConfig config)
        {
            return CreateTaskNode(this, config);
        }

        internal MilestoneTaskNode CreateTaskNode(FMNode parent, MilestoneTaskConfig config)
        {
            var text = config.Description;
            MilestoneTaskNode node = new MilestoneTaskNode(Designer);
            node.Configuration = config;
            node.Name = text + config.TaskFilter;
            node.ShowChildCount = true;
            node.Text = text;
            AddNode(parent, node);
            RegisterMilestoneTaskNode(node);
            return node;
        }

        private void RegisterMilestoneTaskNode(MilestoneTaskNode node)
        {
            AppNode.MilestoneTaskNodes.Add(node);
        }

        internal FileActionsNode CreateActionNode(ActionConfig config)
        {
            return CreateActionNode(this, config);
        }

        internal FileActionsNode CreateActionNode(FMNode parent, ActionConfig config)
        {
            FileActionsNode node = new FileActionsNode(Designer);
            node.Configuration = config;
            AddNode(parent, node);
            return node;

        }
    }
}
