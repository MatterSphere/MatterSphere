namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;


    internal class MilestoneTaskNode : FMNode
    {
        public MilestoneTaskNode(FMDesigner Designer)
            : base(Designer)
        {
            this.ImageKey = "task";
            this.SelectedImageKey = this.ImageKey;
            ContextMenuStrip = Designer.MenuStrip_MilestoneTaskNode;
        }

        new public MilestoneTaskConfig Configuration
        {
            get
            {
                return (MilestoneTaskConfig)base.Configuration;
            }
            set
            {
                base.Configuration = value;
            }
        }

        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case "Description":
                    Text = Configuration.Description;
                    break;
                case "MilestonePlan":
                case "MilestoneStage":
                case "TaskFilter":
                    Designer.RebuildTree();

                    break;
                default:
                    break;
            }
        }

        private void UpdateWarnings(FMNode parent)
        {
            ActionNode an = parent as ActionNode;
            if (an != null)
                an.SetWarnings();

            foreach (FMNode node in parent.Nodes)
                UpdateWarnings(node);
        }

        internal TaskTypeNode CreateTaskTypeNode(TaskTypeConfig config)
        {
            return CreateTaskTypeNode(this, config);
        }

        internal TaskTypeNode CreateTaskTypeNode(FMNode parent, TaskTypeConfig config)
        {
            TaskTypeNode node = new TaskTypeNode(Designer);
            node.Text = config.TaskFilter;
            node.Name = config.TaskFilter;
            node.Configuration = config;
            AddNode(parent, node);
            node.CreateActions();
            node.SetWarnings(true);

            return node;
        }

        internal override void DeleteFromConfig()
        {
            Designer.Configuration.MilestoneTasks.Remove(this.Configuration);
        }

        public override void RemoveNode(bool RemoveChildren)
        {
            base.RemoveNode(RemoveChildren);
            Designer.RebuildTree();
        }

    }

}