using System;
using System.Windows.Forms;

namespace Fwbs.Oms.Office.Common.Panes
{
    using FWBS.OMS;

    public partial class AssociatedDocumentsPane : BasePane
    {
        #region Fields

        private Associate associate;

        #endregion

        #region Constructors

        public AssociatedDocumentsPane()
        {
            InitializeComponent();
            try
            {
                lblStatus.Visible = true;
                lblStatus.Text = "Connecting Please Wait...";

                if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
                    FWBS.OMS.Session.CurrentSession.Connect();
            }
            catch
            {
                lblStatus.Text = "Error Connecting...";
                eLastEmailsForAssociates1.Visible = false;
                return;
            }
        }

        #endregion

        #region Properties


        public Associate Associate
        {
            get
            {
                return associate;
            }
            set
            {
                associate = value;

                if (eLastEmailsForAssociates1.CurrentAssociate != associate)
                {
                    UpdateInfo();
                }
            }
        }

        #endregion

        #region Methods

        private void UpdateInfo()
        {
            object o  = this.Addin;

            if (associate == null)
            {
                return;
            }

            
            if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                lblStatus.Text = "You are not Logged In.";
                return;
            }

            lblStatus.Visible = false;
            try
            {
                rtBox.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(associate.OMSFile.FileClientDescription);
            }
            catch
            {
                rtBox.Text = associate.OMSFile.FileClientDescription;
            }

            rtBox.Refresh();

            try
            {
                eLastEmailsForAssociates1.CurrentAssociate = associate;
                eLastEmailsForAssociates1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Captured Events

        private void eLastEmailsForAssociates1_OpenClicked(object sender, FWBS.OMS.UI.Windows.SelectedIDEventArgs e)
        {
            if (e.SelectedType == FWBS.OMS.UI.Windows.SelectedType.Associate)
                SetCC(e.SelectedID);
            else
                FWBS.OMS.UI.Windows.Services.OpenDocument(FWBS.OMS.OMSDocument.GetDocument(e.SelectedID), FWBS.OMS.DocOpenMode.View);
        }

        

        #endregion

        #region BasePane

        protected override void InternalRefresh(object activeDoc)
        {
            Associate assoc = Addin.OMSApplication.GetCurrentAssociate(activeDoc);

            if (assoc != null)
            {
                bool changed = false;

                if (Pane == null)
                {
                    this.Associate = assoc;
                    Pane = Panes.Add(this, Session.CurrentSession.Resources.GetResource("ASSOCIATEDOCS", "Associate Documents", "").Text);
                    changed = true;
                }
                else
                {
                    Associate oldassoc = this.Associate;
                    if (oldassoc.ID != assoc.ID)
                    {
                        this.Associate = assoc;
                        changed = true;
                    }


                }

                if (changed)
                {

                    if (Visible)
                    {
                        Pane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                        Pane.Width = LogicalToDeviceUnits(500);
                        Pane.Visible = true;
                    }
                    else
                        Pane.Visible = false;

                }
                return;
            }
            else
            {

                if (Pane != null)
                {
                    Pane.Visible = false;
                    Panes.Remove(Pane);
                    Pane.Dispose();
                    Pane = null;
                }
            }
        }

        #endregion

        private void eLastEmailsForAssociates1_AttachClicked(object sender, FWBS.OMS.UI.Windows.SelectedIDEventArgs e)
        {
            if (e.SelectedType == FWBS.OMS.UI.Windows.SelectedType.Associate)
                SetBCC(e.SelectedID);
            else
                AttachFile(e.SelectedID);
        }

        protected virtual void AttachFile(int FileID)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetBCC(int AssociateID)
        {
            throw new NotImplementedException();
        }
        protected virtual void SetCC(int AssociateID)
        {
            throw new NotImplementedException();
        }

    }
}
