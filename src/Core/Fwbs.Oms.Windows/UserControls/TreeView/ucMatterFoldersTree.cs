using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement;
using FWBS.OMS.UI.UserControls.TreeView;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucMatterFoldersTree : UserControl, IBasicEnquiryControl2
    {

        #region Fields

        private Windows8Theme windows8Theme1;
        private DMTreeViewManager DMTVManager;
        private RadTreeViewEx fileTreeView;
        private OMSFile _file = null;
        private bool _checkBoxes;
        private bool _readOnly;

        #endregion

        #region Properties

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Guid SelectedFolderGuid
        {
            get
            {
                if (fileTreeView.SelectedNode == null)
                {
                    return Guid.Empty;
                }
                return DMTVManager.GetTagFolderGUID(fileTreeView.SelectedNode);
            }
            set
            {
                if (value == Guid.Empty)
                {
                    fileTreeView.SelectedNode = null;
                }
                else
                {
                    SelectFolderByGuid(value);
                }
            }
        }

        #endregion

        #region Constructor

        public ucMatterFoldersTree()
        {
            InitializeComponent();
        }

        #endregion

        #region Public methods

        public void InitializeTreeView(OMSFile file)
        {
            SetupTreeView(file);
        }

        public void InitializeTreeView(long fileId)
        {
            var file = OMSFile.GetFile(Convert.ToInt64(fileId));
            SetupTreeView(file);
        }

        public void UnselectFolders()
        {
            fileTreeView.SelectedNode = null;
        }

        public void SelectDefaultFolder(IStorageItem storageItem)
        {
            if (storageItem != null)
            {
                TreeViewSystemNodeGuids systemNodeGuids = DMTreeViewManager.GetSystemFolderGUIDs(fileTreeView);
                SelectedFolderGuid = storageItem.Extension.ToUpper() == "MSG" 
                    ? systemNodeGuids.EmailGuid 
                    : systemNodeGuids.CorrespondenceGuid;
            }
        }

        public void ClearCheckedFolders()
        {
            if (fileTreeView != null && fileTreeView.Nodes.Count > 0)
            {
                var checkedNodes = fileTreeView.FindNodes(n => n.Checked);

                foreach (var node in checkedNodes)
                {
                    node.Checked = false;
                }
            }
        }

        public string GetFolderDescriptionByGuid(Guid guid)
        {
            try
            {
                var nodeMatch = GetTreeNodeByFolderGuid(guid);
                if (nodeMatch != null)
                {
                    var folderDescriptionCode = DMTVManager.GetTagFolderCode(nodeMatch);
                    return FWBS.OMS.CodeLookup.GetLookup("DFLDR_MATTER", folderDescriptionCode);
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        private void SelectFolderByGuid(Guid guid)
        {
            try
            {
                var nodeMatch = GetTreeNodeByFolderGuid(guid);

                fileTreeView.SelectedNode = nodeMatch ?? null;
            }
            catch
            {

            }
        }

        private RadTreeNode GetTreeNodeByFolderGuid(Guid guid)
        {
            var folderGuid = Convert.ToString(guid);
            return string.IsNullOrEmpty(folderGuid) 
                ? null 
                : fileTreeView.Find(n => n.Name == folderGuid);
        }

        private void SetupTreeView(OMSFile file)
        {
            fileTreeView.Nodes.Clear();

            if (file == null)
            {
                return;
            }

            _file = file;
            
            //disassemble XML to build tree
            CreateTreeViewNodes();

            //attach to tree events
            SetupTreeEvents();

            //create and attach context menu
            DMTVManager = new DMTreeViewManager(fileTreeView);

            if (!_readOnly)
            {
                DMTVManager.SetupTreeContextMenu(_checkBoxes);
            }

            DMTVManager.FolderTreeChanged -= new EventHandler<EventArgs>(DMTVManager_FolderTreeChanged);
            DMTVManager.FolderTreeChanged += new EventHandler<EventArgs>(DMTVManager_FolderTreeChanged);

            fileTreeView.DragOverNode -= new EventHandler<RadTreeViewDragCancelEventArgs>(radTreeView1_DragOverNode);
            fileTreeView.DragOverNode += new EventHandler<RadTreeViewDragCancelEventArgs>(radTreeView1_DragOverNode);
        }

        private void CreateTreeViewNodes(){

            fileTreeView.ImageList = DMTreeViewManager.DocumentFolderImageList();

            // Build routine
            var rootString = string.Format("{0}/{1}", _file.Client.ClientNo, _file.FileNo);
            IDocumentFolderBuilder dfsBuilder = DocumentFolderFactory.GetBuilder(_file.GetType());
            if (dfsBuilder != null)
            {
                dfsBuilder.Build(_file.ID, fileTreeView, rootString, _checkBoxes);
                fileTreeView.Nodes[0].Expand();
            }
        }

        private void SetupTreeEvents()
        {
            this.fileTreeView.NodeFormatting -= FWBS.OMS.UI.Windows.TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.fileTreeView.NodeFormatting += FWBS.OMS.UI.Windows.TreeViewNavigation.TreeViewFormatter.NodeFormatting;

            this.fileTreeView.NodeCheckedChanged -= FileTreeView_NodeCheckedChanged;
            this.fileTreeView.NodeCheckedChanged += FileTreeView_NodeCheckedChanged;

            this.fileTreeView.SelectedNodeChanged -= FileTreeView_SelectedNodeChanged;
            this.fileTreeView.SelectedNodeChanged += FileTreeView_SelectedNodeChanged;

            this.fileTreeView.DragOver -= new DragEventHandler(fileTreeView_DragOver);
            this.fileTreeView.DragOver += new DragEventHandler(fileTreeView_DragOver);
        }

        private void FileTreeView_SelectedNodeChanged(object sender, RadTreeViewEventArgs e)
        {
            OnSelectedNodeChanged(this, EventArgs.Empty);
        }

        private void fileTreeView_DragOver(object sender, DragEventArgs e)
        {
            RadTreeNode node = GetNodeUnderMouseCursor(sender, e);
            if (node != null && node.Nodes != null)
                node.Expand();
        }

        private void FileTreeView_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            CheckAllChildNodesInBranch(e.Node);
            OnNodeCheckedChanged(new NodeCheckedChangedEventArgs { CheckedFolderList = BuildUpCheckedFolderList() });
        }

        private void CheckAllChildNodesInBranch(RadTreeNode Node)
        {
            foreach (RadTreeNode node in Node.Nodes)
            {
                node.Checked = Node.Checked;
                if (node.Nodes.Count > 0)
                {
                    CheckAllChildNodesInBranch(node);
                }
            }
        }

        private string BuildUpCheckedFolderList()
        {
            var checkedFolderGUIDs = new StringBuilder();
            foreach (RadTreeNode n in CollectCheckedNodes(fileTreeView.Nodes))
            {
                checkedFolderGUIDs.Append(string.IsNullOrEmpty(checkedFolderGUIDs.ToString())
                    ? Convert.ToString(DMTVManager.GetTagFolderGUID(n))
                    : "," + Convert.ToString(DMTVManager.GetTagFolderGUID(n)));
            }

            return checkedFolderGUIDs.Length != 0 ? checkedFolderGUIDs.ToString() : null;
        }

        private IEnumerable<RadTreeNode> CollectCheckedNodes(RadTreeNodeCollection nodes)
        {
            foreach (RadTreeNode node in nodes)
            {
                if (node.Checked)
                {
                    yield return node;
                }

                foreach (var child in CollectCheckedNodes(node.Nodes))
                    if (child.Checked)
                    {
                        yield return child;
                    }
            }
        }

        private void SaveCurrentTreeView()
        {
            if (_file != null)
            {
                IDocumentFolderSaver dfsSaver = DocumentFolderFactory.GetSaver(_file.GetType());
                if (dfsSaver != null)
                {
                    dfsSaver.Save(_file.ID, fileTreeView);
                }
            }
        }

        private void fileTreeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void fileTreeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                Point pt = ((RadTreeView)sender).PointToClient(new Point(e.X, e.Y));
                var destinationNode = ((RadTreeView)sender).GetNodeAt(pt);
                if (destinationNode != null && destinationNode != fileTreeView.Nodes[0])
                {
                    if (DragIsId(e))
                    {
                        OnFoldersDrop(fileTreeView, new FoldersDropEventArgs
                        {
                            DestinationNodeGuid = DMTVManager.GetTagFolderGUID(destinationNode),
                            FoldersDropType = FoldersDropType.Id,
                            DragEventArgs = e
                        });
                    }
                    else if (DragIsExternalFile(e))
                    {
                        OnFoldersDrop(fileTreeView, new FoldersDropEventArgs
                        {
                            DestinationNodeGuid = DMTVManager.GetTagFolderGUID(destinationNode),
                            FoldersDropType = FoldersDropType.ExternalFile,
                            DragEventArgs = e
                        });
                    }
                    else
                    {
                        throw new Exception(Session.CurrentSession.Resources.GetResource("UCBUILTIN_1",
                            "The object you are attempting to drag and drop onto the tree is not supported.", "").Text);
                    }

                    OnNodeCheckedChanged(new NodeCheckedChangedEventArgs
                    {
                        CheckedFolderList = BuildUpCheckedFolderList()
                    });
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private bool DragIsExternalFile(DragEventArgs e)
        {
            return e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private bool DragIsId(DragEventArgs e)
        {
            return e.Data.GetDataPresent("System.String");
        }

        private void fileTreeView_DragEnding(object sender, RadTreeViewDragCancelEventArgs e)
        {
            //if anything needs to be prohibited when dragging &dropping place it here
            if (e.TargetNode.TreeView != e.TreeView || e.TargetNode.Level == 0)
            {
                e.Cancel = true;
            }
            else
            {
                var node = e.TargetNode.Parent != null &&
                           (e.DropPosition == DropPosition.AfterNode || e.DropPosition == DropPosition.BeforeNode)
                    ? e.TargetNode.Parent
                    : e.TargetNode;
                e.Cancel = node.Nodes.Any(it => it.Text == e.Node.Text && it != e.Node) || node == e.Node;
            }
        }

        protected void FeedbackToUserDuplicateFolderName()
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("DMAF-DUPNAME", "There is already a folder with the same description at this level in the folder tree.", "").Text,
                Branding.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DMTVManager_FolderTreeChanged(object sender, EventArgs e)
        {
            SaveCurrentTreeView();
        }

        void radTreeView1_DragOverNode(object sender, RadTreeViewDragCancelEventArgs e)
        {
            var node = e.TargetNode.Parent != null &&
                       (e.DropPosition == DropPosition.AfterNode || e.DropPosition == DropPosition.BeforeNode)
                ? e.TargetNode.Parent
                : e.TargetNode;
            e.Cancel = node.Nodes.Any(it => it.Text == e.Node.Text && it != e.Node) || node == e.Node || GetNodeParents(node).Any(it => it == e.Node);
        }

        private RadTreeNode GetNodeUnderMouseCursor(object sender, DragEventArgs e)
        {
            Point pt = ((RadTreeView)sender).PointToClient(new Point(e.X, e.Y));
            return ((RadTreeView)sender).GetNodeAt(pt);
        }

        private List<RadTreeNode> GetNodeParents(RadTreeNode node)
        {
            var parents = new List<RadTreeNode>();
            AddParents(node, parents);
            return parents;
        }

        private void AddParents(RadTreeNode node, List<RadTreeNode> parents)
        {
            if (node.Parent != null)
            {
                parents.Add(node.Parent);
                AddParents(node.Parent, parents);
            }
        }

        #region IBasicEnquiryControl2

        [Category("Action")]
        public event EventHandler ActiveChanged;

        [Category("Action")]
        public event EventHandler Changed;

        public object Control
        {
            get { return this; }
        }

        [Browsable(false), DefaultValue(0)]
        public int CaptionWidth { get; set; }
        [Browsable(false), DefaultValue(false)]
        public bool CaptionTop { get; set; }

        [Browsable(false)]
        public bool LockHeight { get; }

        [Browsable(false), DefaultValue(false)]
        public bool Required { get; set; }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                //Workaround: assignment true to AllowDragDrop property in TreeView Telerik control assigns AllowDrop property to true also.
                var fileTreeViewAllowDropTmpValue = AllowDrop;
                _readOnly = value;
                fileTreeView.AllowDragDrop = !_readOnly;
                AllowDrop = fileTreeViewAllowDropTmpValue;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool omsDesignMode { get; set; }

        public object Value
        {
            get { return _file.ID; }
            set
            {
                try
                {
                    var file = OMSFile.GetFile(Convert.ToInt64(value));
                    SetupTreeView(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDirty { get; set; }

        public void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        public void OnActiveChanged()
        {
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        #endregion

        [Category("OMS")]
        [DefaultValue(false)]
        public bool CheckBoxes
        {
            get { return _checkBoxes; }
            set
            {
                if (fileTreeView != null && value != _checkBoxes)
                {
                    _checkBoxes = value;
                    if (_file != null)
                    {
                        SetupTreeView(_file);
                    }
                }
            }
        }

        [Category("OMS")]
        [DefaultValue(true)]
        public override bool AllowDrop
        {
            get
            {
                return fileTreeView != null ? fileTreeView.AllowDrop : false;
            }
            set
            {
                if (fileTreeView != null)
                {
                    fileTreeView.AllowDrop = value;
                }
            }
        }

        #region NodeCheckedChangedEvent

        public delegate void NodeCheckedChangedEventHandler(object sender, NodeCheckedChangedEventArgs e);

        [Category("Action")]
        public event NodeCheckedChangedEventHandler NodeCheckedChanged;

        private void OnNodeCheckedChanged(NodeCheckedChangedEventArgs args)
        {
            if (NodeCheckedChanged != null)
                NodeCheckedChanged(this, args);
        }

        #endregion

        #region FoldersDropEvent

        public delegate void FoldersDropEventHandler(object sender, FoldersDropEventArgs e);

        [Category("Action")]
        public event FoldersDropEventHandler FoldersDrop;

        private void OnFoldersDrop(object sender, FoldersDropEventArgs args)
        {
            if (FoldersDrop != null)
                FoldersDrop(sender, args);
        }

        #endregion

        #region SelectedNodeChangedEvent

        [Category("Action")]
        public event EventHandler SelectedNodeChangedEvent;

        private void OnSelectedNodeChanged(object sender, EventArgs eventArgs)
        {
            SelectedNodeChangedEvent?.Invoke(sender, eventArgs);
        }

        #endregion

    }
    
    public class NodeCheckedChangedEventArgs : EventArgs
    {
        public string CheckedFolderList { get; set; }
    }

    public class FoldersDropEventArgs : EventArgs
    {
        public FoldersDropType FoldersDropType { get; set; }

        public DragEventArgs DragEventArgs { get; set; }

        public Guid DestinationNodeGuid { get; set; }
    }

    public enum FoldersDropType
    {
        Id,
        ExternalFile
    }
}
