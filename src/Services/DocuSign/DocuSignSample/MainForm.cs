using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using FWBS.OMS.DocuSign;

namespace DocuSignSample
{
    public partial class MainForm : Form
    {
        private IDocuSignService _service;
        private string _apiUrl;
        private string _accountId;
        private string _integratorsKey;
        private string _serviceAccountEmail;
        private string _serviceAccountPassword;
        private string _userEmail;
        private string _userName;

        private Doc _doc1;
        private Doc _doc2;
        private Guid _envelopeId;
        private EnvelopeInfo _envelopeInfo;

        private readonly List<string> _recipientSecurityItems = new List<string> { "None", "AccessCode" };
        private IDocuSignService Service
        {
            get
            {
                if (_service == null)
                {
                    _service = new DocuSignService(_apiUrl, _accountId, _integratorsKey, _serviceAccountEmail, _serviceAccountPassword);
                }

                return _service;
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtApiBaseUrl.Text = ConfigurationManager.AppSettings["__DOCUSIGN_ACCOUNT_BASE_URI"];
            txtApiAccountId.Text = ConfigurationManager.AppSettings["__DOCUSIGN_API_ACCOUNT_ID"];
            txtServiceAccountLogin.Text = ConfigurationManager.AppSettings["__DOCUSIGN_USER_ID"];
            txtServiceAccountPassword.Text = ConfigurationManager.AppSettings["__DOCUSIGN_APP_PASSWORD"];
            txtIntegrationKey.Text = ConfigurationManager.AppSettings["__DOCUSIGN_INTEGRATION_KEY"];
            
            txtUserName.Text = ConfigurationManager.AppSettings["UserName"]; 
            txtUserEmail.Text = ConfigurationManager.AppSettings["UserEmail"];

            _doc1 = new Doc();
            _doc2 = new Doc();

            cbRecipientType1.SelectedIndex = 0;
            cbRecipientType2.SelectedIndex = 0;
            
        }

        private void btnSaveAdmin_Click(object sender, EventArgs e)
        {
            _apiUrl = txtApiBaseUrl.Text;
            _accountId = txtApiAccountId.Text;
            _serviceAccountEmail = txtServiceAccountLogin.Text;
            _serviceAccountPassword = txtServiceAccountPassword.Text;
            _integratorsKey = txtIntegrationKey.Text;

            if (_apiUrl != "" &&
                _accountId != "" &&
                _serviceAccountEmail != "" &&
                _serviceAccountPassword != "" &&
                _integratorsKey != "")
            {
                btnTestConnectionAdmin.Enabled = true;
            }

        }

        private void btnTestConnectionAdmin_Click(object sender, EventArgs e)
        {
            var result = false;
            try
            {
                result = Service.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            lblResultAdmin.Text = result ? "Connected!" : "Connection failed!";
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            _userEmail = txtUserEmail.Text;
            _userName = txtUserName.Text;
            if (_userEmail != "")
            {
                btnTestConnectionUser.Enabled = true;
            }

            Service.Impersonate(_userEmail);
        }

        private void btnTestConnectionUser_Click(object sender, EventArgs e)
        {
            var result = false;
            try
            {
                result = Service.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            lblResultUser.Text = result ? "Connected!" : "Connection failed!";
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {
            UploadFile(_doc1);
            lblUploadFilePath.Text = _doc1.Name;
        }

        private void btnUploadFile2_Click(object sender, EventArgs e)
        {
            UploadFile(_doc2);
            lblUploadFilePath2.Text = _doc2.Name;
        }

        private void btnCreateEnvelope_Click(object sender, EventArgs e)
        {
            Expiration expiration = null;

            if (chkReminders.Checked)
            {
                expiration = new Expiration(dtpExpiration.Value, 2);
            }

            Reminder reminder = null;
            if (chkReminders.Checked)
            {
                reminder = new Reminder(dtpReminders.Value, 2);
            }

            try
            {
                _envelopeId = Service.CreateEnvelope(txtSubject.Text, txtBlurb.Text, GetDocuments(), GetRecipients(), reminder, expiration).EnvelopeId;
                lblEnvelopeId.Text = _envelopeId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkReminders_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            dtpReminders.Visible = chk.Checked;
        }

        private void tabDocuSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabControl = (TabControl) sender;

            if (tabControl.SelectedIndex == 2)
            {
                txtEnvelopeId.Text = _envelopeId.ToString();
            }
        }

        private void btnGetEditUrl_Click(object sender, EventArgs e)
        {
            string url;
            try
            {
                url = Service.GetEditEnvelopeUrl(Guid.Parse(txtEnvelopeId.Text), txtReturnUrl.Text);
                txtEditUrl.Text = url;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetEnvelopeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                var status = Service.GetEnvelopeStatus(Guid.Parse(txtEnvelopeId.Text));

                lblEnvelopeStatus.Text = status.Code.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
        }

        private void btnDownloadInOne_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = @"pdf files (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Summary.pdf";

            try
            {
                byte[] body = Service.GetSignedDocumentsInOneFile(Guid.Parse(txtEnvelopeId.Text), false).Data;

                if (body != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var documentStream = saveFileDialog1.OpenFile();
                    documentStream.Write(body, 0, body.Length);
                    documentStream.Close();

                    lblDownloadedFile.Text = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDownloadDocuments_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = @"pdf files (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            try
            {
                var documents = Service.GetSignedDocuments(Guid.Parse(txtEnvelopeId.Text), true);

                foreach (var document in documents)
                {
                    var ext = Path.GetExtension(document.Description).ToLower();
                    if (string.IsNullOrEmpty(ext) || string.IsNullOrEmpty(document.Description) || ext == ".pdf" )
                    {
                        saveFileDialog1.FileName =  document.Description ;
                    }
                    else
                    {
                        saveFileDialog1.FileName = document.Description.Replace(ext, ".pdf");
                    }
                    
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var documentStream = saveFileDialog1.OpenFile();
                        documentStream.Write(document.Data, 0, document.Data.Length);
                        documentStream.Close();

                        lblDownloadedFiles.Text += saveFileDialog1.FileName + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetEnvelope_Click(object sender, EventArgs e)
        {
            try
            {
                _envelopeInfo = Service.GetEnvelope(Guid.Parse(txtEnvelopeId.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            lblEnvvelopeSubject.Text = _envelopeInfo.Subject;
            lblBlurb.Text = _envelopeInfo.EmailBlurb;

            if (_envelopeInfo.Recipients != null && _envelopeInfo.Recipients.Length > 0)
            {
                txtCorrectionRecipientName1.Text = _envelopeInfo.Recipients[0].UserName;
                txtCorrectionRecipientEmail1.Text = _envelopeInfo.Recipients[0].Email;
            }

            if (_envelopeInfo.Recipients != null && _envelopeInfo.Recipients.Length > 1)
            {
                txtCorrectionRecipientName2.Text = _envelopeInfo.Recipients[1].UserName;
                txtCorrectionRecipientEmail2.Text = _envelopeInfo.Recipients[1].Email;
            }
        }

        private void chkExpiration_CheckedChanged(object sender, EventArgs e)
        {
            dtpExpiration.Visible = chkExpiration.Checked;
        }

        private void btnVoidEnvelope_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Service.VoidEnvelope(Guid.Parse(txtEnvelopeId.Text), txtVoidEnvelopeReason.Text);
                MessageBox.Show(result ? "Success!" : "Failed!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = @"pdf files (*.pdf)|*.pdf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Summary.pdf";

            try
            {
                byte[] body = Service.GetSignedDocumentsInOneFile(Guid.Parse(txtEnvelopeId.Text), true).Data;

                if (body != null && saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var documentStream = saveFileDialog1.OpenFile();
                    documentStream.Write(body, 0, body.Length);
                    documentStream.Close();

                    lblDownloadedFileWithCert.Text = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCreateAndSendEnvelope_Click(object sender, EventArgs e)
        {
            Expiration expiration = null;

            if (chkReminders.Checked)
            {
                expiration = new Expiration(dtpExpiration.Value, 2);
            }

            Reminder reminder = null;
            if (chkReminders.Checked)
            {
                reminder = new Reminder(dtpReminders.Value, 2);
            }

            try
            {
                _envelopeId = Service.CreateAndSendEnvelope(txtSubject.Text, txtBlurb.Text, GetDocuments(), GetRecipients(), reminder, expiration).EnvelopeId;
                lblEnvelopeId.Text = _envelopeId.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private RecipientType GetRecipientTypeCode(object value)
        {
            return (RecipientType)Enum.Parse(typeof(RecipientType), value.ToString(), true);
        }

        private DocumentInfo[] GetDocuments()
        {
            var documents = new List<DocumentInfo>();
            uint i = 1;
            if (!string.IsNullOrEmpty(_doc1.Name))
            {
                var doc = new DocumentInfo
                {
                    Data = _doc1.Body,
                    Description = _doc1.Name,
                    Order = i,
                    FileExtension = Path.GetExtension(_doc1.Name).ToLower(),
                    Id = i * 10000
                };

                if (_doc1.Body != null)
                {
                    documents.Add(doc);
                    i++;
                }

            }

            if (!string.IsNullOrEmpty(_doc2.Name))
            {
                var doc = new DocumentInfo
                {
                    Data = _doc2.Body,
                    Description = _doc2.Name,
                    Order = i,
                    FileExtension = Path.GetExtension(_doc2.Name).ToLower(),
                    Id = i * 10000
                };

                if (_doc2.Body != null)
                {
                    documents.Add(doc);
                }
            }

            return documents.ToArray();
        }

        private RecipientInfo[] GetRecipients()
        {
            var runningList = new List<RecipientInfo>();
            uint i = 1;
            if (!string.IsNullOrEmpty(txtRecipientEmail1.Text))
            {
                var recipient = new RecipientInfo
                {
                    UserName = txtRecipientName1.Text,
                    Email = txtRecipientEmail1.Text,
                    Id = i,
                    Type = cbRecipientType1.SelectedValue != null
                        ? GetRecipientTypeCode(cbRecipientType1.SelectedValue)
                        : 0,
                    Tag = "Test User"
                };
                
                runningList.Add(recipient);
                i++;
            }

            if (!string.IsNullOrEmpty(txtRecipientEmail2.Text))
            {
                var recipient = new RecipientInfo
                {
                    UserName = txtRecipientName2.Text,
                    Email = txtRecipientEmail2.Text,
                    Id = i,
                    Type = cbRecipientType2.SelectedValue != null
                        ? GetRecipientTypeCode(cbRecipientType2.SelectedValue)
                        : 0,
                };

                runningList.Add(recipient);
            }

            return runningList.ToArray();
        }

        private void UploadFile(Doc doc)
        {
            openFileDialog1.Title = @"Select file to be upload.";
            openFileDialog1.Filter = @"Select Valid Document(*.pdf; *.doc; *.xlsx; *.html)|*.pdf; *.docx; *.xlsx; *.html";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "";

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        var path = Path.GetFullPath(openFileDialog1.FileName);
                        var byteArray = File.ReadAllBytes(path);
                        doc.Name = openFileDialog1.SafeFileName;
                        doc.Body = byteArray;
                    }
                }
                else
                {
                    MessageBox.Show(@"Please Upload document.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class Doc
    {
        public byte[] Body { get; set; }
        public string Name { get; set; }
    }
}
