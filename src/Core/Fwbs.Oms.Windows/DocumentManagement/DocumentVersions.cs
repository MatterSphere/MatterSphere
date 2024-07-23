using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    using FWBS.OMS.Data;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using Document = OMSDocument;

    public partial class DocumentVersions : UserControl
    {
        #region Fields

        private IStorageItemVersionable versionable;
        private IStorageItem item;
        private ImageList indentImageList;

        #endregion

        #region Events

        public event EventHandler EmailExecuted;

        #endregion

        #region Constructors

        public DocumentVersions()
        {
            InitializeComponent();
            CreateCodeLookups();

            indentImageList = new ImageList() { ImageSize = new Size(20, 16) };
        }       
        

        #endregion

        #region Properties

        [Browsable(false)]
        [DefaultValue(null)]
        public IStorageItem StorageItem
        {
            get
            {
                return this.item;
            }
            set
            {
                if (this.item != value)
                {
                    this.item = value;
                    this.versionable = value as IStorageItemVersionable;
                    if (item is Document)
                    {
                        ((Document)item).Refresh();
                    }
                    Refresh();
                }
            }
        }

        [Browsable(false)]
        public IStorageItemVersion SelectedVersion
        {
            get
            {
                var ret = GetSelectedItems();

                if (ret.Length == 0)
                    return null;

                return ret[0];
            }
        }

        private IStorageItemVersion InternalSelectedVersion
        {
            get
            {
                if (treeView1.SelectedNode != null)
                    return treeView1.SelectedNode.Tag as IStorageItemVersion;
                else
                    return null;
            }
        }

        [Browsable(false)]
        public IEnumerable<IStorageItemVersion> SelectedVersions
        {
            get
            {
                return GetSelectedItems();
            }
        }

        private IEnumerable<IStorageItemVersion> RecurseSelection(TreeNode node)
        {
            if (node.Checked)
            {
                var si = node.Tag as IStorageItemVersion;
                if (si != null)
                    yield return (IStorageItemVersion)node.Tag;
            }

            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode n in node.Nodes)
                {
                    foreach (var si in RecurseSelection(n))
                        yield return si;
                }
            }

            yield break;
        }

        private IEnumerable<IStorageItemVersion> GetTickedItems()
        {
            foreach (var si in RecurseSelection(treeView1.Nodes[0]))
                        yield return si;

            yield break;
        }

        private IStorageItemVersion[] GetSelectedItems()
        {
            List<IStorageItemVersion> list = new List<IStorageItemVersion>();
            if (MultiSelect)
            {
                list.AddRange(GetTickedItems());
            }
            else
            {
                IStorageItemVersion ver = InternalSelectedVersion;
                if (ver != null)
                {
                    list.Add(ver);
                }
            }

            return list.ToArray();
        }

        public Panel InfoPanel
        {
            get
            {
                return infoPanel;
            }
        }

        [DefaultValue(false)]
        public bool MultiSelect
        {
            get
            {
                return treeView1.CheckBoxes;
            }
            set
            {
                if (treeView1.CheckBoxes != value)
                    treeView1.CheckBoxes = value;
            }
        }

      
        #endregion


       
        #region Captured Events

        
        private void btnFlagLatest_Click(object sender, EventArgs e)
        {
            try
            {
                IStorageItemVersion FlagToVersion = SelectedVersion;
                IStorageItemVersion FlagFromVersion = null;

                if (FlagToVersion != null && FlagToVersion is PrecedentVersion)
                {
                    FlagFromVersion = ((PrecedentVersion)FlagToVersion).ParentDocument.GetLatestVersion();
                }

                if (FlagToVersion != null)
                {
                    versionable.SetLatestVersion(FlagToVersion);
                    item.Update();
                }

                btnLatest.Enabled = false;
                Refresh();

                if (FlagToVersion is PrecedentVersion)
                {
                    RestoreToSelectedPrecedentVersion(FlagToVersion, FlagFromVersion);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private static void RestoreToSelectedPrecedentVersion(IStorageItemVersion FlagToVersion, IStorageItemVersion FlagFromVersion)
        {
            PrecedentVersionRestorer restorer = new PrecedentVersionRestorer(FlagToVersion, FlagFromVersion, true);
            restorer.CreatePrecedentRestorationAuditRecord();
            restorer.CheckForScriptRestoration();
            restorer.RestorePrecedentScriptVersion();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                var sel = GetSelectedItems();

                if (sel.Length == 1)
                {
                    if (MessageBox.ShowYesNoQuestion("QDELDOCVERSION", "Are you sure that you would like to delete the document version '%1%'", sel[0].Label) == DialogResult.Yes)
                    {
                        versionable.DeleteVersion(sel[0]);
                        Refresh();
                    }
                }
                else
                {
                    if (MessageBox.ShowYesNoQuestion("QDELDOCVERSIONS", "Are you sure that you would like to delete the checked versions.") == DialogResult.Yes)
                    {
                        foreach (var ver in sel)
                        {
                            versionable.DeleteVersion(ver);
                        }

                        Refresh();
                    }
                }
                
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            try
            {

                    //If outlook has a dialog open then this will need to be closed.
                if (this.ParentForm != null && this.ParentForm.Modal)
                {
                    OnEmailExecuted();
                }
                else
                {
                    SendSelectedVersions();

                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void btnViewComments_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = treeView1.SelectedNode;
                MessageBox.ShowInformation(node.Text);              
             }

            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }

        }


        private void SendSelectedVersions()
        {
            
            Services.SendDocViaEmail(this.ParentForm, GetSelectedItems(), null);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            SendSelectedVersions();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                OnSelect(treeView1.SelectedNode);
                CheckButtonStates();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            BuildVersionTree();
        }

        #endregion

        #region Methods

        private void CreateCodeLookups()
        {
            CreateResourceCodeLookup("DOCVERCOMM", "Version Comments");
        }


        private string CreateResourceCodeLookup(string Code, string Description)
        {
            return ResourceLookup.GetLookupText(Code, Description, null, null);
        }

        public override void Refresh()
        {
            BuildVersionTree();
            base.Refresh();
        }

        private void BuildVersionTree()
        {
            treeView1.Nodes.Clear();
            btnDelete.Enabled = Session.CurrentSession.CurrentUser.IsInRoles(new string[] { "DOCDELETE", User.ROLE_ADMIN });

            if (item != null)
            {
                if (!imageList1.Images.ContainsKey(item.Extension))
                    imageList1.Images.Add(item.Extension, item.GetIcon().ToBitmap());

                this.treeView1.Indent = LogicalToDeviceUnits(20);
                if (treeView1.CheckBoxes)
                {
                    if (!indentImageList.Images.ContainsKey(item.Extension))
                    {
                        var bitmap = new Bitmap(20, 16);
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            imageList1.Draw(graphics, 4, 0, imageList1.Images.IndexOfKey(item.Extension));
                        }
                        indentImageList.Images.Add(item.Extension, bitmap);
                    }

                    treeView1.ImageList = Images.ScaleList(indentImageList, LogicalToDeviceUnits(new Size(20, 16)));
                }
                else
                {
                    treeView1.ImageList = Images.ScaleList(imageList1, LogicalToDeviceUnits(new Size(16, 16)));
                }

                treeView1.ImageKey = StorageItem.Extension;

                if (item.IsNew)
                {
                    treeView1.Enabled = false;
                }
                else
                {
                    if (versionable != null)
                    {
                        IStorageItemVersion latest = versionable.GetLatestVersion();
                        IStorageItemVersion[] versions = versionable.GetVersions();
                        TreeNode root = new TreeNode("...");
                        root.Name = "ROOT";
                        root.ImageKey = "root";
                        treeView1.Nodes.Add(root);

                        Dictionary<Guid, TreeNode> link = new Dictionary<Guid, TreeNode>();

                        foreach (IStorageItemVersion ver in versions)
                        {
                            TreeNode parent = root;
                            TreeNode node = new TreeNode();
                            if (ver.Comments.Length == 0)
                                node.Text = ver.Label;
                            else
                                node.Text = String.Format("{0} ({1})", ver.Label, ver.Comments);

                            node.Name = ver.Id.ToString();
                            node.Tag = ver;
                            link.Add(ver.Id, node);

                            if (ver.IsSubVersion)
                            {
                                if (link.ContainsKey(ver.ParentId.Value))
                                    parent = link[ver.ParentId.Value];
                            }


                            parent.Nodes.Add(node);

                            if (ver.IsLatestVersion)
                            {
                                node.NodeFont = new Font(this.treeView1.Font, FontStyle.Bold);
                                treeView1.SelectedNode = node;
                                node.EnsureVisible();
                                root.Text = String.Format(ResourceLookup.GetLookupText("LATESTVERSION", "Latest Version - {0}", ""), ver.Label);
                                root.Tag = ver;
                            }
                        }

                        root.Expand();

                    }
                }
            }
        }        

        private void SetVersionDetail(IStorageItemVersion ver)
        {
            btnLatest.Enabled = false;
            lblCreatedBy.Text = "...";
            lblUpdatedBy.Text = "...";
            lblCheckedOutBy.Text = "...";
            lblComments.Text = "...";
            lblStatus.Text = "...";
            
            if (ver != null)
            {
                IStorageItemLockable lockable = ver.GetStorageProvider().GetLockableItem(ver);

                //CM 03.07.2014 - The Created and LastUpdated properties (from IStorageVersionItem) are in UTC date format and need converting [WI 4672]
                if (ver.Created.HasValue)
                    lblCreatedBy.Text = String.Format("{0} @ {1}", ver.CreatedBy, ver.Created.Value.ToLocalTime());
                if (ver.LastUpdated.HasValue)
                    lblUpdatedBy.Text = String.Format("{0} @ {1}", ver.LastUpdatedBy, ver.LastUpdated.Value.ToLocalTime());

                if (lockable != null)
                {
                    if (lockable.IsCheckedOut && lockable.CheckedOutBy != null && lockable.CheckedOutTime.HasValue)
                    {
                        lblCheckedOutBy.Text = String.Format("{0} @ {1}", lockable.CheckedOutBy.FullName, lockable.CheckedOutTime);
                    }
                }

                if (ver is OMS.DocumentManagement.DocumentVersion)
                {
                    OMS.DocumentManagement.DocumentVersion dv = (OMS.DocumentManagement.DocumentVersion)ver;
                    if (dv.Status.Length > 0)
                    {
                        lblStatus.Text = dv.StatusDescription;
                    }
                }
                else if (ver is PrecedentVersion)
                {
                    SetPrecedentVersionDetail((PrecedentVersion)ver);
                }

                if (ver.Comments.Length > 0)
                    lblComments.Text = ver.Comments;
                if (!ver.IsLatestVersion)
                {
                    btnLatest.Enabled = true;
                }
            }
        }

        private void SetPrecedentVersionDetail(PrecedentVersion PrecVersion)
        {
            lblCheckedOutBy.Hide();
            label5.Hide();
            lblStatus.Hide();
            label6.Hide();
            
            PrecedentVersion pv = (PrecedentVersion)PrecVersion;
            if (pv.Status.Length > 0)
            {
                lblStatus.Text = pv.StatusDescription;
            }
        }
        
        #endregion


        #region Event Raisers

        private void OnEmailExecuted()
        {
            EventHandler ev = EmailExecuted;
            if (ev != null)
                ev(this, EventArgs.Empty);

        }

        #endregion

        private void DocumentVersions_Load(object sender, EventArgs e)
        {
            btnEmail.Visible = EmailExecuted != null || this.ParentForm?.Modal == false;

            //Hide the Delete button if we are dealing with a precedent object
            btnDelete.Visible = !(this.StorageItem is FWBS.OMS.Precedent);
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                OnSelect(e.Node);
                CheckButtonStates();
            }
            catch(Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void OnSelect(TreeNode selected)
        {
            if (selected != null)
                SetVersionDetail(selected.Tag as IStorageItemVersion);
            else
                SetVersionDetail(null);
        }

        private void CheckButtonStates()
        {
            var sel = GetSelectedItems();


            if (sel.Length <= 0)
            {
                btnLatest.Enabled = false;
                btnEmail.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            
            if (sel.Length > 0)
            {
                btnLatest.Enabled = sel.Length == 1 && !sel[0].IsLatestVersion;
                btnEmail.Enabled = true;
                btnDelete.Enabled = Session.CurrentSession.CurrentUser.IsInRoles(new string[] { "DOCDELETE", User.ROLE_ADMIN });
            }
        }
    }
}
