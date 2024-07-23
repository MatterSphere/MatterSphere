using System;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;


    internal class FileActionsNode : FMNode
    {
        private MilestonePlanNode milestonePlanNode
        {
            get
            {
                return GetMilestonePlanNode();
            }
        }

        private MilestoneStageNode milestoneStageNode
        {
            get
            {
                return GetMilestoneStageNode();
            }
        }

        public new string MilestonePlan
        {
            get
            {
                if (milestonePlanNode == null || milestonePlanNode.Configuration == null)
                    return string.Empty;

                return milestonePlanNode.Configuration.Code;
            }
        }

        public new byte MilestoneStage
        {
            get
            {
                if (milestoneStageNode == null)
                    return 0;

                return milestoneStageNode.StageNo;
            }
        }

        public FileActionsNode(FMDesigner Designer)
            : base(Designer)
        {
            ImageKey = "action";
            SelectedImageKey = this.ImageKey;

            Configuration = this;
            ContextMenuStrip = Designer.MenuStrip_FileActionsNode;
        }

        internal ActionNode CreateActionNode(ActionConfig config)
        {
            return CreateActionNode(this, config);
        }

        internal ActionNode CreateActionNode(FMNode parent, ActionConfig config)
        {
            Designer.AppendLog(String.Format("Creating File Action Node Plan - {0}, Stage {1}, {2}", config.MilestonePlan, config.MilestoneStage, config.Description));
            ActionNode node = new ActionNode(Designer, config);
            AddNode(parent, node);
            return node;
        }
    }
}
