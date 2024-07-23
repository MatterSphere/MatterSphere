using System;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;

    internal class TaskTypeNode : FMNode
    {
        public TaskTypeNode(FMDesigner designer)
            : base(designer)
        {
            this.ImageKey = "task";
            this.SelectedImageKey = this.ImageKey;
            ContextMenuStrip = Designer.MenuStrip_TaskTypeNode;
        }

        public bool IsDynamicTask { get; set; }

        new public TaskTypeConfig Configuration
        {
            get
            {
                return (TaskTypeConfig)base.Configuration;
            }
            set
            {
                base.Configuration = value;
            }
        }

        public MilestoneTaskConfig MilestoneConfig
        {
            get
            {
                MilestoneTaskNode node = GetMilestoneTaskNode();
                if (node == null)
                    return null;

                return node.Configuration;
            }
        }

        protected internal override void SetWarnings()
        {
            ClearWarnings();

            MilestoneTaskConfig MilestoneConfig = this.MilestoneConfig;

            if (MilestoneConfig == null)
                return;

            bool isGroup = string.IsNullOrEmpty(Configuration.TaskGroup) || Configuration.TaskGroup.ToUpperInvariant() == MilestoneConfig.TaskGroup.ToUpperInvariant();
            if (!isGroup)
                AddWarning(string.Format("Task group {0} does not match Milestone group {1}", Configuration.TaskGroup, MilestoneConfig.TaskGroup));

            IsWarning = Warnings.Count > 0;
            return;
        }

        #region Methods
        internal void CreateActions()
        {
            foreach (ActionConfig conf in Configuration.Actions)
                CreateActionNode(conf);
        }

        internal ActionNode CreateActionNode(ActionConfig config)
        {
            return CreateActionNode(this, config);
        }
        internal ActionNode CreateActionNode(FMNode parent, ActionConfig config)
        {
            Designer.AppendLog(String.Format("Creating Task Type Action Node - Plan {0}, Stage {1}, {2}", config.MilestonePlan, config.MilestoneStage, config.Description));
            ActionNode node = new ActionNode(Designer, config);
            AddNode(parent, node);
            return node;
        }

        public override void RemoveNode(bool RemoveChildren)
        {
            base.RemoveNode(RemoveChildren);
            Designer.RebuildTree();
        }

        internal override void DeleteFromConfig()
        {
            Designer.Configuration.TaskTypes.Remove(this.Configuration);
        }

        #endregion

        #region Events
        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case "TaskGroup":
                    SetWarnings(true);
                    break;
                case "TaskFilter":
                    Designer.RebuildTree();
                    break;
            };


        }

        #endregion

    }

}
