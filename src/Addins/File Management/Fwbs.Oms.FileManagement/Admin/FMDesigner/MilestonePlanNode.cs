using System;
using System.Collections.Generic;

namespace FWBS.OMS.FileManagement.Design
{
    internal class MilestonePlanNode : FMNode
    {

        #region Properties
        //Nodes
        internal Dictionary<byte, MilestoneStageNode> StageNodes = new Dictionary<byte, MilestoneStageNode>();
        internal FileActionsNode FileActions;

        new public FWBS.OMS.MilestoneConfig Configuration
        {
            get
            {
                return (MilestoneConfig)base.Configuration;
            }
            set
            {
                base.Configuration = value;
            }
        }
        #endregion

        #region Constructors
        public MilestonePlanNode(FMDesigner Designer)
            : base(Designer)
        {
            this.ImageKey = "plan";
            this.SelectedImageKey = this.ImageKey;
        }


        internal MilestonePlanNode(FMDesigner Designer, string code, FileActionsNode actionsNode)
            : base(Designer)
        {

            this.ImageKey = "plan";
            this.SelectedImageKey = this.ImageKey;
            Name = code;

            try
            {
                Configuration = new MilestoneConfig(code);
                Text = Convert.ToString(Configuration.GetExtraInfo("msdescription"));
            }
            catch (Exception)
            {
                Text = string.Format(code);
            }

            //If no Actions Node was passed in the constructor create one, otherwise link to it
            if (actionsNode == null)
            {
                FileActions = CreateActionsNode(this);
                FileActions.Order = 2;
            }
            else
            {
                FileActions = actionsNode;
            }

            //Create The Stage Nodes
            CreateStageNodes();
        }


        #endregion

        #region Methods
        internal MilestoneStageNode GetStageNode(byte stage)
        {
            if (!StageNodes.ContainsKey(stage))
            {
                string message = string.Format("Stage {0} does not exist in this plan some information will be added to Stage 0", stage);
                stage = 0;
            }
            return (StageNodes[stage]);
        }

        internal MilestoneStageNode CreateStageNode(FMNode parent, byte stage, string description, FileActionsNode actions)
        {
            MilestoneStageNode node = new MilestoneStageNode(Designer, stage, description, actions);
            parent.Nodes.Add(node);
            node.Order = 0;
            node.Order_2 = stage;
            node.StageNo = stage;
            StageNodes.Add(stage, node);
            return node;
        }

        internal void CreateStageNodes()
        {
            //Milestone Stage 0 to catch anything that has not got a stage set
            Res res = Session.CurrentSession.Resources;
            string Stage = res.GetResource("STAGE", "Stage", null).Text;
            ResourceItem noStage = res.GetResource("NOSTAGE", "Stage Not Specified", "Stage has not been specified, these Tasks will not be rendered.");
            MilestoneStageNode node = CreateStageNode(this, 0, noStage.Text, FileActions);
            node.ToolTipText = noStage.Help;

            if (Configuration == null)
                return;

            //Milestone Stages 1 to 30
            for (byte stage = 1; stage <= 30; stage++)
            {
                string stagePreFix = string.Format("{0} {1}", Stage, stage);
                string stageDescription = Convert.ToString(Configuration.GetExtraInfo(String.Format("msstage{0}desc", stage)));
                string description = string.Format("{0} - {1}", stagePreFix, stageDescription);
                if (!string.IsNullOrEmpty(stageDescription))
                {
                    CreateStageNode(this, stage, description, null);
                }
            }


        }

        private FileActionsNode CreateActionsNode(FMNode parent)
        {
            FileActionsNode node = new FileActionsNode(Designer);
            node.Text = Designer.PlanMatterActions;
            Nodes.Add(node);
            return node;
        }


        #endregion


    }

}

