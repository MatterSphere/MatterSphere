namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;


    internal class MilestoneStageNode : FMNode
    {

        internal FileActionsNode FileActions;
        internal TasksNode StaticTasks;

        internal MilestoneStageNode(FMDesigner Designer)
            : base(Designer)
        {
            this.ImageKey = "stage";
            this.SelectedImageKey = this.ImageKey;
        }

        internal MilestoneStageNode(FMDesigner Designer, byte stage, string description, FileActionsNode actions)
            : this(Designer)
        {
            Name = stage.ToString();
            Text = description;

            StaticTasks = CreateStaticTasksNode(this);

            if (actions == null)
                FileActions = CreateMatterActionsNode(this);
            else
                FileActions = actions;
        }

        private FileActionsNode CreateMatterActionsNode(FMNode parent)
        {
            FileActionsNode node = new FileActionsNode(Designer);
            node.Text = Designer.StageMatterActions;
            node.Order = 2;
            AddNode(parent, node);
            return node;
        }

        private TasksNode CreateStaticTasksNode(FMNode parent)
        {
            TasksNode node = new TasksNode(Designer);
            node.Text = Designer.StaticTasks;
            node.Order = 1;
            AddNode(parent, node);
            return node;
        }

        public byte StageNo { get; set; }

        internal MilestoneTaskNode CreateTaskNode(FMNode parent, MilestoneTaskConfig config)
        {
            var text = config.Description;
            MilestoneTaskNode node = new MilestoneTaskNode(Designer);
            node.Configuration = config;
            node.Name = text + config.TaskFilter;
            node.ShowChildCount = true;
            node.Text = text;
            AddNode(parent, node);
            return node;
        }

    }

    #region Old Nodes

    internal class OldDynamicTasksNode : FMNode
    {
        internal OldDynamicTasksNode(FMDesigner Designer)
            : base(Designer)
        {
            Text = "Dynamic Tasks";
            ImageKey = "task";
            SelectedImageKey = this.ImageKey;
        }

    }

    internal class OldActionsNode : FMNode
    {
        private readonly string milestonePlan;
        private byte milestoneStage;

        public new string MilestonePlan
        {
            get { return milestonePlan; }
        }

        public new byte MilestoneStage
        {
            get { return milestoneStage; }
        }


        internal OldActionsNode(FMDesigner Designer, string milestonePlan, byte milestoneStage)
            : base(Designer)
        {
            Text = Designer.StageMatterActions;
            ImageKey = "action";
            SelectedImageKey = this.ImageKey;


            Configuration = this;

            this.milestonePlan = milestonePlan;
            this.milestoneStage = milestoneStage;

            Designer.actionsNodes.Add(string.Format("{0}-{1}", milestonePlan, milestoneStage), this);
        }
    }

    #endregion

}

