using System;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;

    internal class ActionNode : FMNode
    {
        #region Properties
        new public ActionConfig Configuration
        {
            get
            {
                return (ActionConfig)base.Configuration;
            }
            set
            {
                base.Configuration = value;
            }
        }

        #endregion

        #region Constructors
        public ActionNode(FMDesigner Designer, ActionConfig config)
            : base(Designer)
        {
            ImageKey = "action";
            SelectedImageKey = this.ImageKey;
            Configuration = config;
            ContextMenuStrip = Designer.MenuStrip_ActionNode;
            if (config != null)
            {
                Text = config.Description;
                Name = config.Description;
            }
        }
        #endregion

        #region Events
        protected override void OnPropertyChanged(string name)
        {
            switch (name)
            {
                case "Code":
                    this.Text = this.Configuration.Description;
                    break;
                case "MilestonePlan":
                case "MilestoneStage":
                    MilestonePlanOrStageChange();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods

        internal override void DeleteFromConfig()
        {
            if (IsMatterAction)
                Designer.Configuration.FileActions.Remove(this.Configuration);
            else
                TaskConfig.Actions.Remove(this.Configuration);
        }

        private void MilestonePlanOrStageChange()
        {
            if (IsMatterAction)
                Designer.RebuildTree();
            else
                SetWarnings();
        }

        protected internal override void SetWarnings()
        {
            ClearWarnings();

            if (MilestoneTaskConfig == null)
                return;

            bool isPlan = String.IsNullOrEmpty(Configuration.MilestonePlan) || Configuration.MilestonePlan.Equals(MilestoneTaskConfig.MilestonePlan, StringComparison.OrdinalIgnoreCase);
            bool isStage = Configuration.MilestoneStage <= 0 || Configuration.MilestoneStage.Equals(MilestoneTaskConfig.MilestoneStage);

            if (!isPlan)
                AddWarning(string.Format("Milestone Plan {0} on this Action does not match Milestone Plan {1} on the task", Configuration.MilestonePlan, MilestoneTaskConfig.MilestonePlan));
            if (!isStage)
                AddWarning(string.Format("Milestone Stage {0} on this Action does not match Milestone Stage {1} on the task", Configuration.MilestoneStage, MilestoneTaskConfig.MilestoneStage));

            IsWarning = Warnings.Count > 0;
            return;
        }

        #endregion
    }

}
