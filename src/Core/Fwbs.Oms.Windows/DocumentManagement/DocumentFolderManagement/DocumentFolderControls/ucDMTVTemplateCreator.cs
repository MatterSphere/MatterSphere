using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FWBS.OMS.Data;
using FWBS.OMS.UI.Dialogs;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement;
using FWBS.OMS.UI.Windows;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    public partial class ucDMTVTemplateCreator : UserControl
    {
        private object _templateID;

        private bool IsTemplateSelected
        {
            get
            {
                return !String.IsNullOrEmpty(cboTemplates.SelectedValue.ToString());
            }
        }


        public ucDMTVTemplateCreator()
        {
            InitializeComponent();
            ConfigureTreeView();
            ConfigureDisplay();
            SubscribeToTreeViewEvents();
        }


        private void SubscribeToTreeViewEvents()
        { 
            this.templateTreeView.DragEnding -= new Telerik.WinControls.UI.RadTreeView.DragEndingHandler(templateTreeView_DragEnding);
            this.templateTreeView.DragEnding += new Telerik.WinControls.UI.RadTreeView.DragEndingHandler(templateTreeView_DragEnding);

            this.templateTreeView.DragOverNode -= new EventHandler<RadTreeViewDragCancelEventArgs>(templateTreeView_DragEnding);
            this.templateTreeView.DragOverNode += new EventHandler<RadTreeViewDragCancelEventArgs>(templateTreeView_DragEnding);
        }


        private void templateTreeView_DragEnding(object sender, RadTreeViewDragCancelEventArgs e)
        {
            if (e.TargetNode.TreeView != e.TreeView || e.TargetNode.Level == 0 || e.Node.Level == 0)
            {
                e.Cancel = true;
            }
            else
            {
                var node = e.TargetNode.Parent != null &&
                    (e.DropPosition == DropPosition.AfterNode || e.DropPosition == DropPosition.BeforeNode)
                    ? e.TargetNode.Parent
                    : e.TargetNode;
                e.Cancel = node.Nodes.Any(it => it.Text == e.Node.Text && it != e.Node) || IsAncestorNode(node, e.Node);
            }
        }
        
        private bool IsAncestorNode(RadTreeNode node, RadTreeNode parentNode)
        {
            do
            {
                if (node == parentNode)
                    return true;
                
                node = node.Parent;
            } while (node != null);

            return false;
        }

        public object TemplateID
        {
            get { return _templateID; }
            set
            {
                _templateID = value;

                if (_templateID != null)
                {
                    cboTemplates.SelectedValue = _templateID;
                }
            }
        }
        
        public bool MigrateWalletsToFoldersOnSave
        {
            get { return chbMigrateWalletsToFoldersOnSave.Checked; }
        }

        private void ConfigureTreeView()
        {
            //Note: Refactor treeview configuration into their own class against a single treeview object?
            AddTreeViewImages();
        }

        private void AddTreeViewImages()
        {
            this.templateTreeView.ImageList = DMTreeViewManager.DocumentFolderImageList();
        }

        private void ConfigureDisplay()
        {
            SetButtonEnablement(false);

            if (_templateID != null)
            {
                cboTemplates.SelectedValue = _templateID;
            }
        }

        private void SetButtonEnablement(bool enable)
        {
            this.btnSaveAsNewTemplate.Enabled = enable;
            this.btnSaveTemplate.Enabled = enable;
            this.btnDeleteTemplate.Enabled = enable;
        }


        private void cboTemplates_ActiveChanged(object sender, EventArgs e)
        {
            SetButtonEnablement(IsTemplateSelected);
            if (IsTemplateSelected)
            {
                UnloadTemplate();
                LoadNewTemplate(Convert.ToString(this.cboTemplates.SelectedValue), this.cboTemplates.SelectedText);

                DMTreeViewManager dmmanager = new DMTreeViewManager(this.templateTreeView);
                dmmanager.SetupTreeContextMenu(false);
            }
            else
            {
                //NOTE: CLEAR THE TREE IF WE HAVE NOT SELECTED ANYTHING...
                UnloadTemplate();
            }

            _templateID = this.cboTemplates.SelectedValue.ToString();
        }


        #region Button / ComboBox Processes


        private void btnNewTemplate_Click(object sender, EventArgs e)
        {
            CreateNewTemplate();
        }


        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            SaveCurrentTemplate();
        }


        private void btnSaveAsNewTemplate_Click(object sender, EventArgs e)
        {
            CreateNewTemplateFromExistingTemplate();
        }


        private void btnDeleteTemplate_Click(object sender, EventArgs e)
        {
            DeleteTemplate();
        }
        


        private string GetNewTemplateCode()
        {
            string templateDescription = InputBox.Show(
                Session.CurrentSession.Resources.GetResource("DMTVTemplate_5", "Enter the description for the new template.", "").Text,
                Session.CurrentSession.Resources.GetResource("DMTVTemplate_6", "New Document Folder Template", "").Text,
                null);

            return (templateDescription != InputBox.CancelText) ? templateDescription : null;
        }



        private void CreateNewTemplate()
        {
            string templateDescription = GetNewTemplateCode();
            if (!string.IsNullOrWhiteSpace(templateDescription))
            {
                try
                {
                    UnloadTemplate();
                    string templateCode = DMTreeViewManager.CreateNewCodeLookupForFolder(templateDescription, "DFLDR_TEMPLATE");
                    LoadNewTemplate(templateCode, templateDescription);
                    ReviseTemplateList(templateCode);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
        }

        private void CreateNewTemplateFromExistingTemplate()
        {
            string templateDescription = GetNewTemplateCode();
            if (!string.IsNullOrWhiteSpace(templateDescription))
            {
                try
                {
                    MatterTemplateSaverXML saver = new MatterTemplateSaverXML();
                    IDocumentFolderSaver dfsaver = DocumentFolderFactory.GetSaver(saver.GetType());
                    string templateCode = DMTreeViewManager.CreateNewCodeLookupForFolder(templateDescription, "DFLDR_TEMPLATE");
                    dfsaver.Save(templateCode, this.templateTreeView);
                    ReviseTemplateList(templateCode);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
        }


        private void SaveCurrentTemplate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(this.cboTemplates.SelectedValue)))
                {
                    MatterTemplateSaverXML saver = new MatterTemplateSaverXML();
                    IDocumentFolderSaver dfsaver = DocumentFolderFactory.GetSaver(saver.GetType());
                    dfsaver.Save(this.cboTemplates.SelectedValue.ToString(), this.templateTreeView);
                }
                //feedback to user
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void DeleteTemplate()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(this.cboTemplates.SelectedValue)))
                {
                    //check if the template is in use by one or more Matter types
                    if (!CheckIfTemplateIsInUse(Convert.ToString(this.cboTemplates.SelectedValue)))
                    {
                        DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();
                        repository.DeleteFolderTreeTemplate(Convert.ToInt64(this.cboTemplates.SelectedValue));
                        //remove all nodes from RadTreeView - i.e. no template loaded
                        UnloadTemplate();
                        //delete the code lookup for the deleted template
                        FWBS.OMS.CodeLookup.Delete("DFLDR_TEMPLATE", this.cboTemplates.SelectedValue, DBNull.Value);
                        //reload template list - so that deleted template is no longer shown
                        RefreshTemplateList();
                        //select the first template in the list
                        cboTemplates.SelectedIndex = 0;
                        cboTemplates.OnActiveChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void ReviseTemplateList(string templateCode)
        {
            RefreshTemplateList();
            this.cboTemplates.SelectedValue = templateCode;
        }


        private bool CheckIfTemplateIsInUse(string templateCode)
        {
            string matterTypes = "";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            DataTable dtFileTypes = connection.ExecuteSQL("select TypeCode from dbFileType", null);
            if (dtFileTypes != null && dtFileTypes.Rows.Count > 0)
            {
                foreach (DataRow r in dtFileTypes.Rows)
                {
                    FWBS.OMS.FileType ft = FWBS.OMS.FileType.GetFileType(Convert.ToString(r["typecode"]));
                    if (ft.TemplateCode == templateCode)
                    {
                        matterTypes += string.IsNullOrWhiteSpace(matterTypes) ? ft.Description : Environment.NewLine + ft.Description;
                    }
                }

                if(!string.IsNullOrWhiteSpace(matterTypes))
                {
                    using (frmMultiLineText frm = new frmMultiLineText(
                        Session.CurrentSession.Resources.GetResource("MSGTEMPLDELETE", "Document Folder Template", "").Text,
                        Session.CurrentSession.Resources.GetResource("MSGTEMPINUSE", "The template you are trying to delete is in use with the following Matter types and cannot be deleted.", "").Text,
                        matterTypes))
                    {
                        frm.ShowDialog();
                    }
                    return true;
                }
            }

            return false;
        }


        private void LoadNewTemplate(string templateCode, string templateDescription)
        {
            MatterTemplateBuilderXML builder = new MatterTemplateBuilderXML();
            IDocumentFolderBuilder dfbuilder = DocumentFolderFactory.GetBuilder(builder.GetType());
            dfbuilder.Build(templateCode, this.templateTreeView, templateDescription);
            this.folderProperties.SelectedObject = null;
            this.templateTreeView.Nodes[0].Expand();
        }


        private void UnloadTemplate()
        {
            this.templateTreeView.Nodes.Clear();
            this.templateTreeView.Controls.Clear();
        }


        private void RefreshTemplateList()
        {
            DataTable dt = CodeLookup.GetLookups(this.cboTemplates.Type);

            dt.Columns["cdCode"].AllowDBNull = true;
            DataRow ndr = dt.NewRow();
            ndr["cdcode"] = DBNull.Value;
            ndr["cddesc"] = Session.CurrentSession.Resources.GetResource("RESNOTSET", "(not specified)", "").Text;
            dt.Rows.InsertAt(ndr, 0);

            this.cboTemplates.DataSource = dt;
        }

        #endregion


        #region RadTreeView Code

        private void templateTreeView_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            if (this.templateTreeView.SelectedNode != null && this.templateTreeView.SelectedNode.Tag != null)
            {
                propertyGridObject properties = new propertyGridObject((DMTreeNodeTagData)this.templateTreeView.SelectedNode.Tag, this.templateTreeView);
                folderProperties.SelectedObject = properties;
            }
        }

        #endregion
        
    }



    internal class propertyGridObject : LookupTypeDescriptor
    {
        [Browsable(true)]
        [ReadOnly(true)]
        [LocCategory("FOLDER")]
        [Lookup("SYSTEMFOLDER")]
        public bool System { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [LocCategory("FOLDER")]
        [Lookup("FOLDERCODE")]
        public string FolderCode { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [LocCategory("WALLET")]
        [Lookup("WALLETMAPPING")]
        [System.ComponentModel.Editor("FWBS.OMS.UI.DocumentManagement.Addins.FolderWalletMappingEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(DocFolderMappingConverter))]
        public string DocWallets
        {
            get
            {
                return _tag.docWallets;
            }
            set
            {
                _tag.docWallets = value;
            }
        }

        [Browsable(false)]
        public RadTreeView TreeView
        {
            get
            {
                return _treeView;
            }
            set
            {
                _treeView = value;
            }
        }

        private DMTreeNodeTagData _tag;
        private RadTreeView _treeView;

        public propertyGridObject(DMTreeNodeTagData Tag, RadTreeView TreeView)
        {
            _treeView = TreeView;
            _tag = Tag;
            System = Tag.system;
            FolderCode = Tag.folderCode;
            DocWallets = Tag.docWallets;
        }
    }



    internal class FolderTreeTemplateEditor : UITypeEditor
    {
        public FolderTreeTemplateEditor() { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iWFES;
            iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            frmDMTVTemplateCreator frmDMTVTemplateCreator = new frmDMTVTemplateCreator();
            frmDMTVTemplateCreator.TemplateID = Convert.ToString(value);

            iWFES.ShowDialog(frmDMTVTemplateCreator);
            if (frmDMTVTemplateCreator.DialogResult == DialogResult.OK)
            {
                value = frmDMTVTemplateCreator.TemplateID;
                var fileType = context.Instance as FileType;
                if (fileType != null)
                {
                    fileType.MigrateWalletsToFoldersOnSave = frmDMTVTemplateCreator.MigrateWalletsToFoldersOnSave;
                }
            }
            frmDMTVTemplateCreator.Dispose();
            return value;
        }
    }


    internal class FolderWalletMappingEditor : UITypeEditor
    {
        public FolderWalletMappingEditor() { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService iWFES;
            iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            RadTreeView tv = ((propertyGridObject)context.Instance).TreeView;

            frmFolderWalletMappingEditor mappingEditor = new frmFolderWalletMappingEditor();

            DataTable availableMaps = FilterWalletList(FWBS.OMS.CodeLookup.GetLookups("WALLET"), tv);

            mappingEditor.eclDocWallets.SelectionList = availableMaps;
            mappingEditor.eclDocWallets.ValueMember = availableMaps.Columns[0].ColumnName;
            mappingEditor.eclDocWallets.DisplayMember = availableMaps.Columns[1].ColumnName;
            mappingEditor.eclDocWallets.ValueSplit = ",";
            mappingEditor.eclDocWallets.Value = Convert.ToString(value);

            iWFES.ShowDialog(mappingEditor);

            if (mappingEditor.DialogResult == DialogResult.OK)
            {
                value = mappingEditor.eclDocWallets.Value;
            }
            mappingEditor.Dispose();

            return value;
        }

        private DataTable FilterWalletList(DataTable lookups, RadTreeView treeview)
        {
            DataRow[] filteredWallets = lookups.Select("cdCode not in (" + BuildMappedWalletList(treeview) + ")");

            DataTable dtFilteredWallets = lookups.Clone();
            foreach (DataRow row in filteredWallets)
            {
                dtFilteredWallets.ImportRow(row);
            }

            return dtFilteredWallets;
        }


        private string BuildMappedWalletList(RadTreeView treeview)
        {
            string walletmappings = "";
            DMTreeViewManager dmtvManager = new DMTreeViewManager(treeview);
            foreach (RadTreeNode n in CollectMappedWallets(treeview.Nodes))
            {
                string docwallets = dmtvManager.GetTagDocWallets(n);
                if (!string.IsNullOrWhiteSpace(docwallets))
                {
                    if (n != treeview.SelectedNode)
                    walletmappings += walletmappings == "" ? Convert.ToString(docwallets) : "," + Convert.ToString(docwallets);
                }
            }

            walletmappings = "'" + walletmappings.Replace(",","','") + "'";

            return walletmappings;
        }


        public static IEnumerable<RadTreeNode> CollectMappedWallets(RadTreeNodeCollection nodes)
        {
            foreach (RadTreeNode node in nodes)
            {
                if (node.Tag != null)
                {
                    yield return node;
                }

                foreach (var child in CollectMappedWallets(node.Nodes))
                {
                    if (child.Tag != null)
                    {
                        yield return child;
                    }
                }
            }
        }
    }


    internal class DocFolderMappingConverter : StringConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext ctx, Type sourceType)
        {
            return true;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string strConvertedMaps = "";
            if (!String.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                string[] maps = value.ToString().Split(',');
                foreach (string strMap in maps)
                {
                    strConvertedMaps += strConvertedMaps == "" ? FWBS.OMS.CodeLookup.GetLookup("WALLET", strMap) : "," + FWBS.OMS.CodeLookup.GetLookup("WALLET", strMap);
                }
            }
            return strConvertedMaps;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return false;
        }
    }

}
