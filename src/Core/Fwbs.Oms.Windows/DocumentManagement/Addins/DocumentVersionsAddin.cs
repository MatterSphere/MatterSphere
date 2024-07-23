using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    public partial class DocumentVersionsAddin : ucBaseAddin
    {
        private OMSDocument doc;

        #region Constructors

        public DocumentVersionsAddin()
        {
            InitializeComponent();
            SetDockingFormat();
        }

        private void SetDockingFormat()
        {
            var dockableControlConfiguration = new DockableControlConfiguration();

            dockableControlConfiguration.SetDockManagerStyle(omsDockManager1, new DockManagerSettings()
            {
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent
            });

            dockableControlConfiguration.SetDockPanelStyle(dockableControlPane1, new DockPanelSettings()
            {
                TabSettings = new DockPanelTabSettings
                {
                    TextTab = dockableControlPane1.Text,
                    BackColor = ColorTranslator.FromHtml("#005A84"),
                    BackColor2 = ColorTranslator.FromHtml("#005A84")
                },
                CaptionSettings = new DockPanelCaptionSettings
                {
                    BackColor = ColorTranslator.FromHtml("#005A84")
                }
            });
        }

        #endregion

        #region Addin

        public override void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
        {
            doc = obj as OMSDocument;

            base.Initialise(obj);

            versions.InfoPanel.Visible = false;
            versions.MultiSelect = true;
        }

        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            doc = obj as OMSDocument;
            ToBeRefreshed = true;
            AttatchEvents();
            return doc != null;
        }
       
        public override void RefreshItem()
        {
            if (ToBeRefreshed)
            {
                if (doc != null)
                {
                    versions.StorageItem = doc;
                    versions.Refresh();
                }
            }
            ToBeRefreshed = false;
        }

        public override void SelectItem()
        {
        }

        private void DetatchEvents()
        {
            this.versions.treeView1.AfterSelect -= new TreeViewEventHandler(treeView1_AfterSelect);
        }

        
      
        private void AttatchEvents()
        {
            DetatchEvents();
            this.versions.treeView1.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (omsDockManager1.PaneFromControl(ucDocumentPreviewer1).Pinned)
                    PreviewDocument(true);
            }
            catch
            {
            }
        }




        private void PreviewDocument(bool SpecificVersion = false)
        {
            if (this.versions.treeView1.SelectedNode != null)
                this.ucDocumentPreviewer1.PreviewFile(this.versions.treeView1.SelectedNode.Tag as FWBS.OMS.DocumentManagement.Storage.IStorageItem, SpecificVersion);
        }

        #endregion

        private void omsDockManager1_PaneDisplayed(object sender, Infragistics.Win.UltraWinDock.PaneDisplayedEventArgs e)
        {
            try
            {
                if (e.Pane == omsDockManager1.PaneFromControl(ucDocumentPreviewer1))
                    PreviewDocument();
            }
            catch { }
        }

        private void omsDockManager1_PaneHidden(object sender, Infragistics.Win.UltraWinDock.PaneHiddenEventArgs e)
        {
            try
            {
                this.ucDocumentPreviewer1.PreviewFile(null);
            }
            catch { }
        }

        internal DocumentVersions DocumentVersionsControl
        {
            get
            {
                return versions;
            }
        }
    }
}
