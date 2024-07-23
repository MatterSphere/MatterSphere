using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using Telerik.WinControls.UI;


namespace FWBS.OMS.UI.Windows
{
    public partial class VersionComparisonSelector : BaseForm
    {
        private int selectedversionA = 0;
        private int selectedversionB = 0;

        private bool hideversions;

        private LockState ls;
        private ObjectLinkRetriever retriever;

        private IObjectComparison selectedobject;

        DataTable currentversions = null;

        Telerik.WinControls.UI.RadContextMenu parentMenu = new Telerik.WinControls.UI.RadContextMenu();
        Telerik.WinControls.UI.RadContextMenu childMenu = new Telerik.WinControls.UI.RadContextMenu();

        ObjectVersionDataSelector objectVDS;

        public event EventHandler<RestorationCompletedEventArgs> RestorationCompleted;

        public VersionComparisonSelector(string Code, LockableObjects Type)
        {
            InitializeComponent();

            hideversions = false;

            ls = new LockState();
            retriever = new ObjectLinkRetriever();

            this.scSearch.Visible = hideversions;
            this.objectInformation.Text = "";

            selectedobject = ObjectComparisonFactory.CreateIObjectComparison(Code, Type.ToString(), 0, 0);

            CreateObjectListRootNode();
            BuildCurrentVersionTable(); 
            BuildObjectList();

            this.FormClosing += new FormClosingEventHandler(VersionComparisonSelector_FormClosing);
        }


        private void VersionComparisonSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (System.IO.Directory.Exists(selectedobject.xmlFileLocation))
            {
                string[] filePaths = System.IO.Directory.GetFiles(selectedobject.xmlFileLocation, "MSComp-*");
                foreach (string filePath in filePaths)
                    System.IO.File.Delete(filePath);
            }
            if(objectVDS != null)
                objectVDS.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(objectVDS_RestorationCompleted);
        }


        private void BuildCurrentVersionTable()
        {
            currentversions = new DataTable();
            currentversions.Columns.Add("Code", typeof(string));
            currentversions.Columns.Add("Type", typeof(string));
            currentversions.Columns.Add("CurrentVersionNumber", typeof(string));
        }


        private void BuildObjectList()
        {
            try
            {
                SetupTreeView();
                System.Data.DataTable dt = retriever.RetrieveObjectLinks(selectedobject);
                if (dt != null && dt.Rows.Count > 0)
                    foreach (System.Data.DataRow r in dt.Rows)
                    {
                        IObjectComparison obj = ObjectComparisonFactory.CreateIObjectComparison(Convert.ToString(r["objectcode"]), Convert.ToString(r["objecttype"]), 0, 0);
                        AddNode(obj);
                        obj = null;
                    }
                if (tvLinkedObjects.Nodes.Count > 0)
                    tvLinkedObjects.SelectedNode = tvLinkedObjects.Nodes[0];
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void CreateObjectListRootNode()
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = "Object List:";
            node.ImageIndex = 24;
            this.tvLinkedObjects.Nodes.Add(node);
            this.tvLinkedObjects.SelectedNode = node;
        }


        private void AddNode(IObjectComparison obj)
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = obj.code;
            node.ImageIndex = obj.image;
            node.Tag = PopulateNodeTag(obj);
            this.tvLinkedObjects.SelectedNode.Nodes.Add(node);
        }


        private void SetupTreeView()
        {
            tvLinkedObjects.ImageList = FWBS.OMS.UI.Windows.Images.AdminMenu16();
            SetupTreeEvents();
            tvLinkedObjects.ExpandAll();
            tvLinkedObjects.RootElement.Select();
        }


        private void SetupTreeEvents()
        {
            this.tvLinkedObjects.KeyPress -= new KeyPressEventHandler(treeView_KeyPress);
            this.tvLinkedObjects.KeyPress += new KeyPressEventHandler(treeView_KeyPress);
            this.tvLinkedObjects.MouseUp -= new MouseEventHandler(treeView_MouseUp);
            this.tvLinkedObjects.MouseUp += new MouseEventHandler(treeView_MouseUp);
            this.tvLinkedObjects.MouseClick -= new MouseEventHandler(treeView_MouseClick);
            this.tvLinkedObjects.MouseClick += new MouseEventHandler(treeView_MouseClick);
            this.tvLinkedObjects.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.tvLinkedObjects.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
        }


        private void treeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (tvLinkedObjects.SelectedNode.Tag != null)
                    SetupVersionSearchList(Convert.ToString(tvLinkedObjects.SelectedNode.Text), (objectdata)tvLinkedObjects.SelectedNode.Tag);
            }
        }


        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.tvLinkedObjects.Nodes.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if(tvLinkedObjects.SelectedNode.Tag != null)
                        SetupVersionSearchList(Convert.ToString(tvLinkedObjects.SelectedNode.Text), (objectdata)tvLinkedObjects.SelectedNode.Tag);
                }
            }
        }


        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {

        }


        private void btnCompare_Click(object sender, EventArgs e)
        {
            RunComparisonProcess();
        }


        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (scSearch.SelectedItems.Count() > 1)
                    RestorationSelectionWarningMessage();
                else
                {
                    RadTreeNode node = tvLinkedObjects.SelectedNode;
                    if (node.Tag != null && GetSelectedVersionNumber())
                    {
                        objectdata tag = (objectdata)node.Tag;
                        if (Convert.ToInt32(tag.objectversion) == selectedversionA)
                        {
                            RestorationProductionVersionWarningMessage();
                        }
                        else
                        {
                            if (objectVDS == null)
                            {
                                objectVDS = new ObjectVersionDataSelector(tag.objectcode, tag.objecttype, selectedversionA, currentversions);
                                objectVDS.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(objectVDS_RestorationCompleted);
                                objectVDS.FormClosing += new FormClosingEventHandler(objectVDS_FormClosing);
                                var result = objectVDS.ShowDialog(this);
                                objectVDS.StartPosition = FormStartPosition.CenterScreen;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        void objectVDS_FormClosing(object sender, FormClosingEventArgs e)
        {
            objectVDS.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(objectVDS_RestorationCompleted);
            this.Close();
        }

        private void objectVDS_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            if (this.RestorationCompleted != null)
                RestorationCompleted(this, e);
        }

        private void RunComparisonProcess()
        {
            try
            {
                if (scSearch.SelectedItems.Count() != 2)
                    ComparisonSelectionWarningMessage();
                else
                {
                    selectedversionA = ConvertDef.ToInt32(scSearch.SelectedItems[0]["Version"].Value, 0);
                    selectedversionB = ConvertDef.ToInt32(scSearch.SelectedItems[1]["Version"].Value, 0);

                    RadTreeNode node = tvLinkedObjects.SelectedNode;
                    objectdata tag = (objectdata)node.Tag;
                    if (selectedversionA != 0 && selectedversionB != 0)
                    {
                        selectedobject = ObjectComparisonFactory.CreateIObjectComparison(tag.objectcode, tag.objecttype, selectedversionA, selectedversionB);
                        selectedobject.RunComparisonProcess();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("VCSSELDIFFOBJ"), ResourceLookup.GetLookupText("VCSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private bool GetSelectedVersionNumber()
        {
            bool result = true;
            if (ConvertDef.ToInt32(scSearch.dgSearchResults.VisibleRowCount, 0) != 0)
            {
                if (scSearch.DataTable.Rows.Count > 0)
                    selectedversionA = ConvertDef.ToInt32(scSearch.SelectedItems[0]["Version"].Value, 0);
            }

            if (selectedversionA == 0)
            {
                System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("VCSNOCURVERSION"), ResourceLookup.GetLookupText("VCSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                result = false;
            }
            return result;
        }

        private void scSearch_ItemHover(object sender, SearchItemHoverEventArgs e)
        {
        }

        private void ComparisonSelectionWarningMessage()
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("VCSELECTION", "You must select two versions from the list for the comparison process to continue.\n\nPlease amend your selection and try again.", "").Text, ResourceLookup.GetLookupText("VCSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RestorationSelectionWarningMessage()
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("VRSELECTION", "You have selected more than one version from the list for restoration.\n\nPlease amend your selection and try again.", "").Text, ResourceLookup.GetLookupText("OVDSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RestorationProductionVersionWarningMessage()
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("VRPRODSELECT", "You have selected the current production version for restoration.\n\nPlease amend your selection and try again.", "").Text, ResourceLookup.GetLookupText("OVDSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetupVersionSearchList(string code, objectdata tag)
        {
            switch (tag.objecttype)
            {
                case "EnquiryForm":
                    SetVersionList(code, "LEFVERSIONDATA");
                    SetObjectInformation(tag);
                    break;
                case "DataList":
                    SetVersionList(code, "LDLVERSIONDATA");
                    SetObjectInformation(tag);
                    break;
                case "Script":
                    SetVersionList(code, "LSCRVERSIONDATA");
                    SetObjectInformation(tag);
                    break;
                case "SearchList":
                    SetVersionList(code, "LSLVERSIONDATA");
                    SetObjectInformation(tag);
                    break;
                case "Precedent":
                    SetVersionList(code, "");
                    SetObjectInformation(tag);
                    break;
                case "FileManagement":
                    SetVersionList(code, "LFMVERSIONDATA");
                    SetObjectInformation(tag);
                    break;
            }
            this.scSearch.Visible = hideversions;
        }


        private void SetVersionList(string code, string searchlistcode)
        {
            if (!string.IsNullOrWhiteSpace(searchlistcode))
            {
                Common.KeyValueCollection kvc = new Common.KeyValueCollection();
                kvc.Add("code", code);
                this.scSearch.SetSearchList(searchlistcode, null, kvc);
                this.scSearch.Search(true, true, true);
                hideversions = true;
                ManageCompareRestoreButtons(hideversions);
            }
            else
            {
                hideversions = false;
                ManageCompareRestoreButtons(hideversions);
            }
        }


        private void SetObjectInformation(objectdata tag)
        {
            this.objectInformation.Text = "Current Production Version:\n\nObject Type :  " + tag.objecttype + "\nVersion :           " + tag.objectversion + "\nCreated :          " + tag.objectcreated + "\nCreated By :    " + tag.objectcreatedby;
        }


        private void ManageCompareRestoreButtons(bool state)
        {
            this.btnCompare.Visible = state;
            this.btnRestore.Visible = state;
        }


        private objectdata PopulateNodeTag(IObjectComparison obj)
        {
            objectdata tag = new objectdata();
            try
            {
                DataRow r = currentversions.NewRow();
                System.Data.DataTable dt = obj.ExecuteDataGather();

                if (dt != null && dt.Rows.Count > 0)
                {
                    tag.objectcode = obj.code;
                    r["Code"] = obj.code;
                    tag.objecttype = obj.type;
                    r["Type"] = obj.type;
                    tag = GetObjectVersion(obj, tag, r, dt);
                    if (Convert.ToString(ConvertDef.ToDateTime(dt.Rows[0]["Created"], DateTime.MinValue)) != DateTime.MinValue.ToString())
                        tag.objectcreated = Convert.ToDateTime(dt.Rows[0]["Created"]).ToShortDateString();
                    if (ConvertDef.ToInt32(dt.Rows[0]["CreatedBy"], 0) != 0)
                        tag.objectcreatedby = FWBS.OMS.User.GetUser(Convert.ToInt32(dt.Rows[0]["CreatedBy"])).FullName;

                    currentversions.Rows.Add(r);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            return tag;
        }

        private static objectdata GetObjectVersion(IObjectComparison obj, objectdata tag, DataRow r, System.Data.DataTable dt)
        {
            if (obj.type != "Precedent")
            {
                if (Convert.ToString(ConvertDef.ToInt32(dt.Rows[0]["Version"], 0)) != "0")
                {
                    tag.objectversion = Convert.ToString(ConvertDef.ToInt32(dt.Rows[0]["Version"], 0));
                    r["CurrentVersionNumber"] = Convert.ToString(ConvertDef.ToInt32(dt.Rows[0]["Version"], 0));
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["Version"])))
                {
                    tag.objectversion = Convert.ToString(dt.Rows[0]["Version"]);
                    r["CurrentVersionNumber"] = Convert.ToString(dt.Rows[0]["Version"]);
                }
            }
            return tag;
        }

    }


    public struct objectdata
    {
        public string objectcode;
        public string objecttype;
        public string objectversion;
        public string objectcreated;
        public string objectcreatedby;
    }


}


