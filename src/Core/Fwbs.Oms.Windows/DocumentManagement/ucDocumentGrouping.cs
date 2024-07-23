using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public partial class ucDocumentGrouping : UserControl
    {
        private enum DateSpan
        {
            Today,
            Yesterday,
            ThisWeek,
            LastWeek,
            CurrentMonth,
            LastMonth,
            ThisYear,
            Older
        }

        public string Grouping { get; set; }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.tvGroupings.BackColor = value;
            }
        }

        public ucDocumentGrouping()
        {
            InitializeComponent();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.tvGroupings.RightToLeftLayout = true;
            else
                this.tvGroupings.RightToLeftLayout = false;
        }
        
        public event ValueChangedEventHandler GroupingChanged;
        private void OnGroupingChanged(object selectedGroup)
        {

            ValueChangedEventHandler ev = GroupingChanged;
            if (ev != null)
                ev(this, new ValueChangedEventArgs("Grouping", null, selectedGroup));
        }

        public event EventHandler FilterBuilt;
        private void OnFilterBuilt()
        {
            EventHandler ev = FilterBuilt;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            tvGroupings.Indent = LogicalToDeviceUnits(20);
            base.OnDpiChangedAfterParent(e);
        }

        public void Initialise()
        {
            tvGroupings.Nodes.Clear();
            tvGroupings.Indent = LogicalToDeviceUnits(20);

            DataTable dt = CodeLookup.GetLookups(Grouping);

            foreach (DataRow row in dt.Rows)
            {
                TreeNode node = null;
                if (row["cdhelp"] != DBNull.Value && ((string)row["cdhelp"]).ToUpper() == DateType.ToUpper())
                {
                    node = DateNodes(row["cddesc"] as string, row["cdcode"] as string);
                }
                else
                {

                    node = new TreeNode(row["cddesc"] as string);
                    node.Tag = row["cdcode"];

                    TreeNode dummyNode = new TreeNode("Dummy");
                    dummyNode.Tag = "!£$%^&*()Q";

                    node.Nodes.Add(dummyNode);

                }
                tvGroupings.Nodes.Add(node);

            }
        }

        public void Clear()
        {
            Initialise();
        }

        public bool FoundGroups
        {
            get{ return tvGroupings.Nodes.Count > 0;}
        }

        public void RefreshNodes()
        {
            foreach (TreeNode node in tvGroupings.Nodes)
            {

                if (node.Tag != null && ((string)node.Tag).Contains("|"))
                    continue;

                if (node.IsExpanded)
                    OnGroupingChanged(node.Tag);
            }
        }


        public void SetGroups(List<object> groups, string nodeTag)
        {
            //get a copy of the passed in list as we remove elements from it lower down
            groups = new List<object>(groups);

            foreach (TreeNode node in tvGroupings.Nodes)
            {
                if (node.Tag != nodeTag)
                    continue;

                List<TreeNode> removeNodes = new List<TreeNode>();
                foreach (TreeNode subnode in node.Nodes)
                {
                    if (groups.Contains(subnode.Tag))
                        groups.Remove(subnode.Tag);
                    else
                        removeNodes.Add(subnode);
                }

                foreach (TreeNode subnode in removeNodes)
                {
                    if (node.Nodes.Contains(subnode))
                        node.Nodes.Remove(subnode);
                }

                foreach(object group in groups)
                {
                    string groupName = "";
                    if (group == DBNull.Value || group == null)
                        groupName = "(Not Set)";
                    else
                        groupName = group.ToString();

                    TreeNode subnode = new TreeNode(groupName);
                    subnode.Tag = group;
                    node.Nodes.Add(subnode);
                }
                break;
            }

            tvGroupings.Refresh();

            BuildFilter();
        }

        private static string DateType = "Date";

        private TreeNode DateNodes(string description,  string field)
        {
            //need a way of identifying it as a date node
            TreeNode node = CreateNode(description, field, DateType);

            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("TODAY","Today","").Text, DateSpan.Today));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("YESTERDAY","Yesterday","").Text, DateSpan.Yesterday));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("THISWEEK","This Week","").Text, DateSpan.ThisWeek));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("LASTWEEK","Last Week","").Text, DateSpan.LastWeek));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("THISMONTH","This Month","").Text, DateSpan.CurrentMonth));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("LASTMONTH","Last Month","").Text, DateSpan.LastMonth));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("THISYEAR", "This Year", "").Text, DateSpan.ThisYear));
            node.Nodes.Add(CreateNode(FWBS.OMS.Session.CurrentSession.Resources.GetResource("OLDER", "Older", "").Text, DateSpan.Older));



            return node;
        }

        private TreeNode CreateNode(string description, object field)
        {
            TreeNode node = new TreeNode(description);
            node.Tag = field;
            return node;

        }

        private TreeNode CreateNode(string description, string field, string type)
        {
            TreeNode node = new TreeNode(description);
            node.Tag = type+"|"+field;
            return node;

        }


        private void tvGroupings_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && !((string)e.Node.Tag).Contains("|"))
                OnGroupingChanged(e.Node.Tag);
        }

        private void tvGroupings_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.ByKeyboard && e.Action != TreeViewAction.ByMouse)
                return;

            if (e.Node.Nodes.Count > 0)
            {
                if (e.Node.Nodes[0].Tag == "!£$%^&*()Q")
                    OnGroupingChanged(e.Node.Tag);
            }

            SetAllSubNodes(e.Node, e.Node.Checked);
            SetParentNode(e.Node, e.Node.Checked);

            BuildFilter();
        }

        private void SetAllSubNodes(TreeNode node, bool check)
        {
            foreach (TreeNode subnode in node.Nodes)
            {
                subnode.Checked = check;
                SetAllSubNodes(subnode, check);
            }
        }

        private void SetParentNode(TreeNode node, bool check)
        {
            TreeNode parent = node.Parent;
            if (parent != null)
            {
                bool changeCheck = true;
                if (!check)
                {
                    foreach (TreeNode subNode in parent.Nodes)
                    {
                        if (subNode.Checked)
                        {
                            changeCheck = false;
                            break;
                        }
                    }
                }

                if (!changeCheck)
                    return;

                parent.Checked = check;
                SetParentNode(parent, check);
            }
        }

        private string filter;
        public string Filter
        {
            get
            {
                return filter;
            }
        }

        private void BuildFilter()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                filter = "";

                foreach (TreeNode node in this.tvGroupings.Nodes)
                {
                    string tag = node.Tag as string;

                    if (string.IsNullOrEmpty(tag))
                        continue;

                    string type = "";
                    if (tag.Contains("|"))
                    {
                        string[] tags = tag.Split('|');
                        if (tags.Length > 1)
                        {
                            type = tags[0];
                            tag = tags[1];
                        }
                        else
                            continue;
                    }

                    string prefix = "and (";
                    bool closeBracket = false;
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        if (!subNode.Checked)
                            continue;

                        object value = subNode.Tag;

                        string filterElement = value as string;
                        string equator = "=";
                        if (value == DBNull.Value || value == null)
                        {
                            equator = "is";
                            filterElement = "null";
                        }
                        else if (type.ToUpper() == DateType.ToUpper())
                            SortDateTimeFilter(tag, (DateSpan)value, ref filterElement, ref equator);
                        else if (value.GetType() == typeof(string))
                        {
                            value = ((string)value).Replace("'", "''");
                            filterElement = string.Format("'{0}'", value);
                        }


                        if (string.IsNullOrEmpty(filter))
                        {
                            filter = string.Format(@"({0} {1} {2}", tag, equator, filterElement);
                            closeBracket = true;
                            prefix = "or";
                        }
                        else
                        {
                            if (prefix == "and (")
                                closeBracket = true;
                            filter = string.Format("{0} {1} {2} {3} {4}", filter, prefix, tag, equator, filterElement);
                            prefix = "or";
                        }


                    }
                    if (closeBracket)
                        filter += ")";
                }

                OnFilterBuilt();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
            
        }

        private static void SortDateTimeFilter(string fieldName, DateSpan span, ref string filterElement, ref string equator)
        {
            DateTime today = DateTime.Today;

            switch ((span))
            {
                case DateSpan.Today:
                    equator = ">=";
                    today = today.ToUniversalTime();
                    DateTime tomorrow = today.Add(new TimeSpan(24, 0, 0));
                    filterElement = string.Format("'{0}' and {1} < '{2}'", today, fieldName, tomorrow);
                    break;
                case DateSpan.Yesterday:
                    equator = "<";
                    today = today.ToUniversalTime();
                    DateTime yesterday = today.Subtract(new TimeSpan(24, 0, 0));
                    filterElement = string.Format("'{0}' and {1} >= '{2}'", today, fieldName, yesterday);
                    break;
                case DateSpan.ThisWeek:
                    {
                        int DayOfWeek = Convert.ToInt32(today.DayOfWeek);
                        DateTime startOfWeek = today.Subtract(new TimeSpan(24 * DayOfWeek, 0, 0)).ToUniversalTime();
                        DateTime endOfWeek = startOfWeek.Add(new TimeSpan(168, 0, 0));
                        equator = ">=";
                        filterElement = string.Format("'{0}' and {1} < '{2}'", startOfWeek, fieldName, endOfWeek);
                    }
                    break;
                case DateSpan.LastWeek:
                    {

                        int DayOfWeek = Convert.ToInt32(today.DayOfWeek);
                        DateTime endOfWeek = today.Subtract(new TimeSpan(24 * DayOfWeek, 0, 0)).ToUniversalTime();
                        DateTime startOfWeek = endOfWeek.Subtract(new TimeSpan(168, 0, 0));
                        equator = ">=";
                        filterElement = string.Format("'{0}' and {1} < '{2}'", startOfWeek, fieldName, endOfWeek);
                    }
                    break;
                case DateSpan.CurrentMonth:
                    {
                        DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
                        equator = ">=";
                        filterElement = string.Format("'{0}'", firstOfMonth);

                    }
                    break;
                case DateSpan.LastMonth:
                    {
                        DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Local);
                        DateTime firstOfLastMonth = firstOfMonth.Subtract(new TimeSpan(24, 0, 0));
                        firstOfLastMonth = new DateTime(firstOfLastMonth.Year, firstOfLastMonth.Month, 1, 0, 0, 0).ToUniversalTime();
                        firstOfMonth = firstOfMonth.ToUniversalTime();
                        equator = "<";
                        filterElement = string.Format("'{0}' and {1} >= '{2}'", firstOfMonth, fieldName, firstOfLastMonth);
                    }
                    break;
                case DateSpan.ThisYear:
                    {
                        DateTime firstOfYear = new DateTime(today.Year, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
                        equator = ">=";
                        filterElement = string.Format("'{0}'", firstOfYear);
                    }
                    break;
                case DateSpan.Older:
                    {
                        DateTime firstOfYear = new DateTime(today.Year, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime();
                        equator = "<";
                        filterElement = string.Format("'{0}'", firstOfYear);
                    }
                    break;
            }
        }

    }
}
