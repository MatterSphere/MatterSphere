using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.DocumentManagement.Addins;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    internal partial class DocumentPickerForm : frmNewBrandIdent 
    {
        #region Fields

        private OMSDocument[] selected_documents;
        private Client current_client;
        private OMSFile current_file;
        private string original_title;
        private DocumentPickerType type;
        private Common.KeyValueCollection parameters;

        string _docref = String.Empty;
        string _ourref = String.Empty;
        string _doctype = String.Empty;
        string _created = String.Empty;
        string _createdby = String.Empty;

        #endregion

        #region Constructors

        public DocumentPickerForm(DocumentPickerType type, Client currentClient, OMSFile currentFile, Common.KeyValueCollection parameters)
        {

            InitializeComponent();
            SetImageLists();

            ucDocuments1.Leave += new EventHandler(ucDocuments1_Leave);
            ucDocuments1.DocumentsRefreshed += new EventHandler(ucDocuments1_DocumentsRefreshed);
            ucDocuments1.DocumentSelecting += new EventHandler(ucDocuments1_DocumentSelecting);
            ucDocuments1.DocumentSelected += new EventHandler(ucDocuments1_DocumentSelected);

            if (parameters == null)
                parameters = new FWBS.Common.KeyValueCollection();

            this.parameters = parameters;
            this.type = type;
            this.current_client = currentClient;
            this.current_file = currentFile;

            if (Session.CurrentSession.IsLoggedIn)
            {
                _docref = Session.CurrentSession.Resources.GetResource("DOCREF", "Document Ref : ", "").Text;
                _ourref = Session.CurrentSession.Resources.GetResource("OURREF", "Our Ref : ", "").Text;
                _doctype = Session.CurrentSession.Resources.GetResource("DOCTYPE", "Document Type : ", "").Text;
                _created = Session.CurrentSession.Resources.GetResource("CREATED", "Created : ", "").Text;
                _createdby = Session.CurrentSession.Resources.GetResource("CREATEDBY", "Created By : ", "").Text;
            }

            ucDocuments1.InitialiseHost(DocumentAddinHost.DocumentPicker);

            ncbAllClientFiles.Visible = ucDocuments1.SupportsView(DocumentPickerType.Search);
            ncbCheckedOut.Visible = ucDocuments1.SupportsView(DocumentPickerType.CheckedOut);
            ncbCurrentClientFiles.Visible = ucDocuments1.SupportsView(DocumentPickerType.File);
            ncbLastUsed.Visible = ucDocuments1.SupportsView(DocumentPickerType.LatestOpen);
            ncbLocal.Visible = ucDocuments1.SupportsView(DocumentPickerType.Local);
            ncbCurrentClient.Visible = ucDocuments1.SupportsView(DocumentPickerType.Client);

        }
        #endregion

        #region Captured Events

        private void txtDocID_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!Session.CurrentSession.SupportsExternalDocumentIds)
            {
                if (txtDocID.SelectionStart == 0 && (e.KeyValue == 189 || e.KeyValue == 109))
                    e.SuppressKeyPress = false;
                else if (e.KeyValue == 8 || e.KeyValue == 36 || e.KeyValue == 37 || e.KeyValue == 35 || e.KeyValue == 39 || e.KeyValue == 46)
                    e.SuppressKeyPress = false;
                else if ((e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 96 && e.KeyValue <= 106))
                    e.SuppressKeyPress = false;
                else
                    e.SuppressKeyPress = true;
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDocID.Text.Trim().Length > 0)
                {
                    selected_documents = new OMSDocument[1] { ucDocuments1.Find(txtDocID.Text) };
                }
                else
                    selected_documents = ucDocuments1.SelectedDocuments;

                DialogResult = DialogResult.OK;
            }
            catch (OMSException2 ex)
            {
                if (ex.Code != "DUPDOCCANCELLED")
                    MessageBox.Show(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
            }
        }



        private void DocumentPickerForm_Load(object sender, EventArgs e)
        {
            if (current_client == null)
                current_client = Session.CurrentSession.CurrentClient;

            if (current_file == null)
                current_file = Session.CurrentSession.CurrentFile;

            original_title = this.Text;


            ApplyView(type);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Session.CurrentSession.DuplicateDocumentIDs)
                sb.AppendLine(Session.CurrentSession.Resources.GetMessage("INFODUPDOCIDSON", "Duplicate Document Identifiers Switched On", "").Text);
            if (Session.CurrentSession.SupportsExternalDocumentIds)
                sb.AppendLine(Session.CurrentSession.Resources.GetMessage("INFOEXTDOCIDSON", "External Document Identifiers Switched On", "").Text);

            if (sb.Length > 0)
                info.SetError(txtDocID, sb.ToString());
            else
                info.SetError(txtDocID, String.Empty);
        }

        private void Link_Click(object sender, EventArgs e)
        {
            if (sender == ncbLastUsed)
                ApplyView(DocumentPickerType.LatestOpen);
            else if (sender == ncbCurrentClientFiles)
                ApplyView(DocumentPickerType.File);
            else if (sender == ncbCurrentClient)
                ApplyView(DocumentPickerType.Client);
            else if (sender == ncbAllClientFiles)
                ApplyView(DocumentPickerType.Search);
            else if (sender == ncbCheckedOut)
                ApplyView(DocumentPickerType.CheckedOut);
            else if (sender == ncbLocal)
                ApplyView(DocumentPickerType.Local);
            else if (sender == ncbChangeClient)
            {
                if (ChangeClient())
                    ApplyView(DocumentPickerType.Client);
            }
            else if (sender == ncbChangeClientFile)
            {
                if (ChangeFile())
                    ApplyView(DocumentPickerType.File);
            }
        }

        private void ucDocuments1_Leave(object sender, EventArgs e)
        {
            try
            {
                RefreshPanels();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void ucDocuments1_DocumentsRefreshed(object sender, EventArgs e)
        {
            try
            {
                if (ucDocuments1.DocumentCount > 0)
                {
                    pnDetails.Visible = true;
                    pnDetails.Expanded = true;
                }
                else
                {
                    pnDetails.Visible = false;
                    txtDocID.Text = String.Empty;
                    ucNavRichText1.ControlRich.Clear();
                    ucNavRichText1.Refresh();
                    pnDetails.Expanded = false;
                    
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }

        }


        private void ucDocuments1_DocumentSelected(object sender, EventArgs e)
        {
            try
            {
                string[] ids = ucDocuments1.SelectedDocumentIds;

                if (ids.Length == 0 || ids.Length > 1)
                    txtDocID.Text = String.Empty;
                else
                    txtDocID.Text = ids[0];

                if (txtDocID.Text.Trim().Length > 0 || ids.Length > 0)
                    btnOK.Enabled = true;
                else
                    btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ucDocuments1_DocumentSelecting(object sender, EventArgs e)
        {
            try
            {
                if (pnDetails.Expanded == false)
                    pnDetails.Expanded = true;

                ucNavRichText1.ControlRich.Clear();

                ucNavRichText1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(ucDocuments1.GetCurrentDocumentDetailsAsRTF());
                
                ucNavRichText1.Refresh();

                string[] ids = ucDocuments1.SelectedDocumentIds;

                if (ids.Length == 0 || ids.Length > 1)
                    txtDocID.Text = String.Empty;
                else
                    txtDocID.Text = ids[0];

                if (txtDocID.Text.Trim().Length > 0 || ids.Length > 0)
                    btnOK.Enabled = true;
                else
                    btnOK.Enabled = false;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        #endregion

        #region Methods

        private void SetImageLists()
        {
            var omsImageLists = ImageListSelector.GetOMSImageList();
            
            this.ncSearchTypes.Resources = omsImageLists;
            this.ncActions.Resources = omsImageLists;
        }


        private void ApplyView(DocumentPickerType type)
        {
            if (ucDocuments1.SupportsView(type))
            {
                switch (type)
                {
                    case DocumentPickerType.CheckedOut:
                        ApplyToggle(ncbCheckedOut);
                        ApplyCheckedOut();
                        break;
                    case DocumentPickerType.Client:
                        ApplyToggle(ncbCurrentClient);
                        if (current_client != null || ChangeClient())
                            ApplyCurrentClient();
                        else
                        {
                            HideView();
                            return;
                        }
                        break;
                    case DocumentPickerType.File:
                        ApplyToggle(ncbCurrentClientFiles);
                        if (current_file != null || ChangeFile())
                            ApplyCurrentFile();
                        else
                        {
                            HideView();
                            return;
                        }
                        break;
                    case DocumentPickerType.LatestOpen:
                        ApplyToggle(ncbLastUsed);
                        ApplyLastestOpen();
                        break;
                    case DocumentPickerType.Search:
                        ApplyToggle(ncbAllClientFiles);
                        ApplySearch();
                        break;
                    case DocumentPickerType.Local:
                        ApplyToggle(ncbLocal);
                        ApplyLocal();
                        break;
                }

                ShowView();
            }
            else
                HideView();
        }

        private void ShowView()
        {
            if (!ucDocuments1.Visible)
                ucDocuments1.Visible = true;
        }

        private void HideView()
        {
            ucDocuments1.Visible = false;
        }

        private void RefreshPanels()
        {
            if (current_client != null)
            {
                if (pnDetails.Expanded == false)
                    pnDetails.Expanded = true;

                try
                {
                    this.ucNavRichText1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(current_client.ClientDescription);

                }
                catch
                {
                    this.ucNavRichText1.Text = current_client.ClientDescription;

                } 
                this.ucNavRichText1.Refresh();
            }
        }

        private void ResetTitle()
        {
            // This is done twice to counter act the SetResource Component
            this.Text = original_title;
            this.Text = original_title;
        }

        private void SetTitle(string title)
        {
            // This is done twice to counter act the SetResource Component
            this.Text = original_title + " - " + title;
            this.Text = original_title + " - " + title;
        }


        private void ApplyLastestOpen()
        {
  			ResetTitle();
			pnActions.Expanded = false;
            ucDocuments1.ShowView(DocumentPickerType.LatestOpen, null);
            RefreshPanels();
			ncbChangeClient.Visible=false;
			ncbChangeClientFile.Visible=false;
			ncActions.Refresh();
            pnActions.Visible = false;
		}


        private void ApplyCheckedOut()
        {
            ResetTitle();
            pnActions.Expanded = false;
            ucDocuments1.ShowView(DocumentPickerType.CheckedOut, null);
            RefreshPanels();
            ncbChangeClient.Visible = false;
            ncbChangeClientFile.Visible = false;
            ncActions.Refresh();
            pnActions.Visible = false;
        }

        private void ApplyLocal()
        {
            ResetTitle();
            pnActions.Expanded = false;
            ucDocuments1.ShowView(DocumentPickerType.Local, null);
            RefreshPanels();
            ncbChangeClient.Visible = false;
            ncbChangeClientFile.Visible = false;
            ncActions.Refresh();
            pnActions.Visible = false;
        }

       

        private void ApplySearch()
        {
            ResetTitle();           
            pnActions.Expanded = false;
            ucDocuments1.ShowView(DocumentPickerType.Search, null);
            RefreshPanels();
            ncbChangeClient.Visible = false;
            ncbChangeClientFile.Visible = false;
            ncActions.Refresh();
            pnActions.Visible = false;
        }

        private void ApplyCurrentClient()
        {
            SetTitle(current_client.ToString());
            pnActions.Expanded = true;
            ucDocuments1.ShowView(DocumentPickerType.Client, current_client);
            RefreshPanels();
            pnActions.Visible = true;
            ncbChangeClient.Visible = true;
            ncbChangeClientFile.Visible = false;
            ncActions.Refresh();
        }


        private void ApplyCurrentFile()
        {
            SetTitle(current_file.ToString());          
            pnActions.Expanded = true;
            ucDocuments1.ShowView(DocumentPickerType.File, current_file);
            RefreshPanels();
            pnActions.Visible = true;
            ncbChangeClient.Visible = false;
            ncbChangeClientFile.Visible = true;
            ncActions.Refresh();

        }

        private void ApplyToggle(ucNavCmdButtons btn)
        {
            ncbCurrentClientFiles.ForeColor = ncSearchTypes.ForeColor;
            ncbAllClientFiles.ForeColor = ncSearchTypes.ForeColor;
            ncbCurrentClient.ForeColor = ncSearchTypes.ForeColor;
            ncbLastUsed.ForeColor = ncSearchTypes.ForeColor;
            ncbCheckedOut.ForeColor = ncSearchTypes.ForeColor;
            ncbLocal.ForeColor = ncSearchTypes.ForeColor;
            btn.ForeColor = Color.Red;
        }

        private bool ChangeClient()
        {
            Client cl = FWBS.OMS.UI.Windows.Services.SelectClient();
            if (cl != null)
            {
                current_client = cl;
                return true;
            }
            else
                return false;
        }

        private bool ChangeFile()
        {
            OMSFile file = FWBS.OMS.UI.Windows.Services.SelectFile();
            if (file != null)
            {
                current_file = file;
                current_client = file.Client;
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Properties

        public OMSDocument[] SelectedDocuments
        {
            get
            {
                return selected_documents;
            }
        }

        #endregion
    }
}