using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.FileManagement.Design
{
    using FWBS.OMS.FileManagement.Configuration;


    internal class FMNode : TreeNode, IComparable
    {
        #region fields
        private int childCount;
        private string text;
        private object config;
        private List<string> warnings = new List<string>();
        private FMDesigner designer;

        #endregion

        #region Properties

        public int Order { get; set; }
        public int Order_2 { get; set; }

        internal TaskTypeConfig TaskConfig
        {
            get
            {
                TaskTypeNode node = GetTaskTypeNode();
                if (node == null)
                    return null;

                return node.Configuration;
            }
        }

        internal MilestoneTaskConfig MilestoneTaskConfig
        {
            get
            {
                MilestoneTaskNode node = GetMilestoneTaskNode();
                if (node == null)
                    return null;

                return node.Configuration;
            }
        }

        internal bool IsMatterAction
        {
            get { return MilestoneTaskConfig == null && TaskConfig == null; }
        }

        internal string MilestonePlan
        {
            get
            {
                if (MilestoneTaskConfig != null)
                    return MilestoneTaskConfig.MilestonePlan;

                MilestonePlanNode msn = GetMilestonePlanNode();

                if (msn == null || msn.Configuration == null)
                    return string.Empty;

                return msn.Configuration.Code;
            }
        }

        internal byte MilestoneStage
        {
            get
            {
                if (MilestoneTaskConfig != null)
                    return MilestoneTaskConfig.MilestoneStage;

                MilestoneStageNode msn = GetMilestoneStageNode();

                if (msn == null)
                    return 0;

                return msn.StageNo;
            }
        }

        internal FMDesigner Designer
        {
            get
            {
                return designer;
            }
            set
            {
                if (designer != value)
                    designer = value;
            }
        }

        internal List<FMNode> AllNodes
        {
            get
            {
                return Designer.AllNodes;
            }
        }

        internal FMApplicationNode AppNode
        {
            get
            {
                return Designer.AppNode;
            }
        }

        public List<string> Warnings
        {
            get { return warnings; }
        }

        public bool IsVirtual { get; set; }
        new public string Text
        {
            get { return base.Text; }
            set
            {

                text = value;

                if (ShowChildCount && ChildCount > 0)
                    base.Text = string.Format("{1} ({0})", ChildCount.ToString(), text);
                else
                    base.Text = text;
            }
        }

        public bool ShowChildCount { get; set; }

        private bool isWarning;
        public bool IsWarning
        {
            get
            {
                return isWarning;
            }
            set
            {
                if (value != isWarning)
                {
                    isWarning = value;

                    if (isWarning)
                        this.NodeFont = new Font(System.Windows.Forms.TreeView.DefaultFont, FontStyle.Strikeout);
                    else
                        this.NodeFont = new Font(System.Windows.Forms.TreeView.DefaultFont, FontStyle.Regular);
                }
            }
        }

        public void AddWarning(string warning)
        {
            if (string.IsNullOrEmpty(warning))
                return;

            if (!warnings.Contains(warning))
            {
                warnings.Add(warning);
                base.ToolTipText += warning + Environment.NewLine;
            }
            IsWarning = true;
        }

        public void AddWarning(List<string> warnings)
        {
            if (warnings == null || warnings.Count == 0)
                return;

            foreach (string w in warnings)
                this.AddWarning(w);
        }

        public void ClearWarnings()
        {
            warnings.Clear();
            IsWarning = false;
            ToolTipText = string.Empty;
        }

        public object Configuration
        {
            get
            {
                return config;
            }
            set
            {
                if (value != config)
                {
                    if (config != null)
                        DetachEvents();

                    config = value;

                    if (config != null)
                        AttachEvents();
                }
            }
        }

        public int ChildCount
        {
            get
            {
                return childCount;
            }
            set
            {
                if (value != childCount)
                {
                    childCount = value;
                    Text = text;
                }
            }
        }

        #endregion

        #region Constructors
        public FMNode(FMDesigner designer)
        {
            this.designer = designer;
        }

        #endregion
        #region Methods

        internal void AddNode(FMNode parent, FMNode child)
        {
            if (!AllNodes.Contains(child))
                AllNodes.Add(child);

            parent.Nodes.Add(child);
        }

        internal void AddNode(FMNode child)
        {
            AddNode(this, child);
        }

        public virtual void RemoveNode(bool RemoveChildren)
        {
            if (RemoveChildren)
            {
                foreach (TreeNode node in Nodes)
                {
                    FMNode fmNode = node as FMNode;
                    if (fmNode == null)
                        return;
                    fmNode.RemoveNode(RemoveChildren);
                }
            }

            DetachEvents();

            AllNodes.Remove(this);

            DeleteFromConfig();

            base.Remove();
        }

        internal virtual void DeleteFromConfig()
        {
        }

        protected internal virtual void AttachEvents()
        {
            DetachEvents();

            var pc = config as INotifyPropertyChanged;
            if (pc != null)
                pc.PropertyChanged += OnPropertyChanged;
        }

        protected internal virtual void DetachEvents()
        {
            var pc = config as INotifyPropertyChanged;
            if (pc != null)
                pc.PropertyChanged -= OnPropertyChanged;

        }

        protected internal virtual void SetWarnings()
        {
        }

        internal void SetWarnings(bool incChildren)
        {
            SetWarnings();
            foreach (TreeNode node in Nodes)
            {
                FMNode fmNode = node as FMNode;
                if (fmNode != null)
                    fmNode.SetWarnings(true);
            }
        }

        internal MilestoneTaskNode GetMilestoneTaskNode()
        {
            return GetMilestoneTaskNode(this);
        }
        protected static internal MilestoneTaskNode GetMilestoneTaskNode(FMNode node)
        {
            return FindAncestor(node, typeof(MilestoneTaskNode)) as MilestoneTaskNode;
        }

        internal TaskTypeNode GetTaskTypeNode()
        {
            return GetTaskTypeNode(this);
        }
        protected static internal TaskTypeNode GetTaskTypeNode(FMNode node)
        {
            return FindAncestor(node, typeof(TaskTypeNode)) as TaskTypeNode;
        }

        internal MilestoneStageNode GetMilestoneStageNode()
        {
            return GetMilestoneStageNode(this);
        }
        protected static internal MilestoneStageNode GetMilestoneStageNode(FMNode node)
        {
            return FindAncestor(node, typeof(MilestoneStageNode)) as MilestoneStageNode;
        }

        internal MilestonePlanNode GetMilestonePlanNode()
        {
            return GetMilestonePlanNode(this);
        }
        protected static internal MilestonePlanNode GetMilestonePlanNode(FMNode node)
        {
            return FindAncestor(node, typeof(MilestonePlanNode)) as MilestonePlanNode;
        }

        protected static internal FMNode FindAncestor(FMNode node, Type type)
        {
            if (node.GetType() == type)
                return node;

            FMNode ancestor = node.Parent as FMNode;

            if (ancestor == null)
                return null;

            if (type == ancestor.GetType())
            {
                return ancestor;
            }

            return FindAncestor(ancestor, type);
        }

        #endregion

        #region Events
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        protected virtual void OnPropertyChanged(string name)
        {
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            FMNode node = obj as FMNode;
            if (node == null)
                return 0;

            if (Order.CompareTo(node.Order) == 0)
                return (Order_2.CompareTo(node.Order_2));

            return Order.CompareTo(node.Order);
        }

        #endregion
        #region Comparer

        internal class FMNodeSorter : System.Collections.IComparer
        {

            #region IComparer<FMNode> Members

            public int Compare(object x, object y)
            {
                var cx = x as IComparable;
                var cy = y as IComparable;

                if (cx != null)
                    return cx.CompareTo(y);

                if (cy != null)
                    return -cy.CompareTo(x);

                return 0;

            }

            #endregion
        }

        #endregion
    }

}

