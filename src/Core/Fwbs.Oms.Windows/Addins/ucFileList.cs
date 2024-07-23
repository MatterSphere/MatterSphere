using System;
using System.ComponentModel;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Displays a list of contacts associated to a specific client.  This is used as
    /// a IConfigurableTypeAddin, to be used in OMSDialogs.
    /// </summary>
    public class ucFileList : ucBaseAddin, Interfaces.IOMSTypeAddin
    {
        #region Fields

        private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
        private IContainer components;

        private Client _client = null;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Default constructor of the user control.
        /// </summary>
        public ucFileList()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ucSearchControl1 != null)
                {
                    ucSearchControl1.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(168, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(672, 490);
            this.ucSearchControl1.TabIndex = 8;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            // 
            // ucFileList
            // 
            this.Controls.Add(this.ucSearchControl1);
            this.Name = "ucFileList";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region IOMSTypeAddin Implementation

        /// <summary>
        /// Allows the calling OMS dialog to connect to the addin for the configurable type object.
        /// </summary>
        /// <param name="obj">OMS Configurable type object to use.</param>
        /// <returns>A flag that tells the dialogthat the connection has been successfull.</returns>
        public override bool Connect(IOMSType obj)
        {
            _client = obj as Client;

            if (obj == null)
                return false;
            else
            {
                ucSearchControl1.NewOMSTypeWindow -= new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
                ucSearchControl1.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
                ToBeRefreshed = true;
                return true;
            }

        }

        /// <summary>
        /// Refreshes the addin visual look and data contents.
        /// </summary>
        public override void RefreshItem()
        {
            if (_client != null && ToBeRefreshed)
            {
                if (ucSearchControl1.SearchList == null)
                    ucSearchControl1.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.File), false, _client, null);

                _client.Refresh(ClientTables.Files);
                ucSearchControl1.Search();
                ucSearchControl1.ShowPanelButtons();
                ToBeRefreshed = false;
            }
        }

        #endregion

        #region Private

        private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
        {
            try
            {
                if (e.ButtonName == "cmdViewFile")
                {
                    FWBS.OMS.OMSFile tmpfile = FWBS.OMS.OMSFile.GetFile((long)ucSearchControl1.CurrentItem()["FILEID"].Value);
                    NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(tmpfile);
                    OnNewOMSTypeWindow(this, eva);
                }
                if (e.ButtonName == "cmdAdd")
                {
                    FWBS.OMS.OMSFile omsfile = FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(_client);
                    ToBeRefreshed = true;
                    if (_client != null)
                        _client.Refresh(true);
                    this.RefreshItem();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }
        #endregion
    }
}
