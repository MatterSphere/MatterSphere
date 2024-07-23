
namespace DocuSignSample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabDocuSign = new System.Windows.Forms.TabControl();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnTestConnectionUser = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lblResultUser = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtUserEmail = new System.Windows.Forms.TextBox();
            this.btnSaveUser = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnTestConnectionAdmin = new System.Windows.Forms.Button();
            this.txtApiBaseUrl = new System.Windows.Forms.TextBox();
            this.txtApiAccountId = new System.Windows.Forms.TextBox();
            this.lblResultAdmin = new System.Windows.Forms.Label();
            this.txtIntegrationKey = new System.Windows.Forms.TextBox();
            this.txtServiceAccountLogin = new System.Windows.Forms.TextBox();
            this.txtServiceAccountPassword = new System.Windows.Forms.TextBox();
            this.btnSaveAdmin = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tabCreateEnvelope = new System.Windows.Forms.TabPage();
            this.btnCreateAndSendEnvelope = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpExpiration = new System.Windows.Forms.DateTimePicker();
            this.chkExpiration = new System.Windows.Forms.CheckBox();
            this.chkReminders = new System.Windows.Forms.CheckBox();
            this.dtpReminders = new System.Windows.Forms.DateTimePicker();
            this.lblEnvelopeId = new System.Windows.Forms.Label();
            this.btnCreateEnvelope = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbRecipientType2 = new System.Windows.Forms.ComboBox();
            this.txtRecipientEmail2 = new System.Windows.Forms.TextBox();
            this.txtRecipientName2 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cbRecipientType1 = new System.Windows.Forms.ComboBox();
            this.txtRecipientEmail1 = new System.Windows.Forms.TextBox();
            this.txtRecipientName1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblUploadFilePath2 = new System.Windows.Forms.Label();
            this.btnUploadFile2 = new System.Windows.Forms.Button();
            this.lblUploadFilePath = new System.Windows.Forms.Label();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.txtBlurb = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tabEditEnvelope = new System.Windows.Forms.TabPage();
            this.label26 = new System.Windows.Forms.Label();
            this.lblDownloadedFileWithCert = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtVoidEnvelopeReason = new System.Windows.Forms.TextBox();
            this.btnVoidEnvelope = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblBlurb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblEnvvelopeSubject = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtCorrectionRecipientEmail2 = new System.Windows.Forms.TextBox();
            this.txtCorrectionRecipientName2 = new System.Windows.Forms.TextBox();
            this.txtCorrectionRecipientEmail1 = new System.Windows.Forms.TextBox();
            this.txtCorrectionRecipientName1 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnGetEnvelope = new System.Windows.Forms.Button();
            this.lblDownloadedFiles = new System.Windows.Forms.Label();
            this.btnDownloadDocuments = new System.Windows.Forms.Button();
            this.lblDownloadedFile = new System.Windows.Forms.Label();
            this.btnDownloadInOne = new System.Windows.Forms.Button();
            this.lblEnvelopeStatus = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btnGetEnvelopeStatus = new System.Windows.Forms.Button();
            this.txtEditUrl = new System.Windows.Forms.TextBox();
            this.btnGetEditUrl = new System.Windows.Forms.Button();
            this.txtReturnUrl = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtEnvelopeId = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabDocuSign.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabCreateEnvelope.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabEditEnvelope.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDocuSign
            // 
            this.tabDocuSign.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDocuSign.Controls.Add(this.tabSettings);
            this.tabDocuSign.Controls.Add(this.tabCreateEnvelope);
            this.tabDocuSign.Controls.Add(this.tabEditEnvelope);
            this.tabDocuSign.Location = new System.Drawing.Point(12, 12);
            this.tabDocuSign.Name = "tabDocuSign";
            this.tabDocuSign.SelectedIndex = 0;
            this.tabDocuSign.Size = new System.Drawing.Size(1080, 564);
            this.tabDocuSign.TabIndex = 1;
            this.tabDocuSign.SelectedIndexChanged += new System.EventHandler(this.tabDocuSign_SelectedIndexChanged);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.groupBox3);
            this.tabSettings.Controls.Add(this.groupBox2);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(1072, 538);
            this.tabSettings.TabIndex = 0;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnTestConnectionUser);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblResultUser);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtUserName);
            this.groupBox3.Controls.Add(this.txtUserEmail);
            this.groupBox3.Controls.Add(this.btnSaveUser);
            this.groupBox3.Location = new System.Drawing.Point(555, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(499, 301);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "User Settings";
            // 
            // btnTestConnectionUser
            // 
            this.btnTestConnectionUser.Enabled = false;
            this.btnTestConnectionUser.Location = new System.Drawing.Point(212, 168);
            this.btnTestConnectionUser.Name = "btnTestConnectionUser";
            this.btnTestConnectionUser.Size = new System.Drawing.Size(118, 23);
            this.btnTestConnectionUser.TabIndex = 20;
            this.btnTestConnectionUser.Text = "Test Connection";
            this.btnTestConnectionUser.UseVisualStyleBackColor = true;
            this.btnTestConnectionUser.Click += new System.EventHandler(this.btnTestConnectionUser_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Enabled = false;
            this.label8.Location = new System.Drawing.Point(22, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "User Name";
            // 
            // lblResultUser
            // 
            this.lblResultUser.AutoSize = true;
            this.lblResultUser.Location = new System.Drawing.Point(22, 223);
            this.lblResultUser.Name = "lblResultUser";
            this.lblResultUser.Size = new System.Drawing.Size(25, 13);
            this.lblResultUser.TabIndex = 22;
            this.lblResultUser.Text = " .....";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "User Email";
            // 
            // txtUserName
            // 
            this.txtUserName.Enabled = false;
            this.txtUserName.Location = new System.Drawing.Point(101, 19);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(301, 20);
            this.txtUserName.TabIndex = 15;
            // 
            // txtUserEmail
            // 
            this.txtUserEmail.Location = new System.Drawing.Point(101, 46);
            this.txtUserEmail.Name = "txtUserEmail";
            this.txtUserEmail.Size = new System.Drawing.Size(301, 20);
            this.txtUserEmail.TabIndex = 16;
            // 
            // btnSaveUser
            // 
            this.btnSaveUser.Location = new System.Drawing.Point(101, 168);
            this.btnSaveUser.Name = "btnSaveUser";
            this.btnSaveUser.Size = new System.Drawing.Size(75, 23);
            this.btnSaveUser.TabIndex = 18;
            this.btnSaveUser.Text = "Save";
            this.btnSaveUser.UseVisualStyleBackColor = true;
            this.btnSaveUser.Click += new System.EventHandler(this.btnSaveUser_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnTestConnectionAdmin);
            this.groupBox2.Controls.Add(this.txtApiBaseUrl);
            this.groupBox2.Controls.Add(this.txtApiAccountId);
            this.groupBox2.Controls.Add(this.lblResultAdmin);
            this.groupBox2.Controls.Add(this.txtIntegrationKey);
            this.groupBox2.Controls.Add(this.txtServiceAccountLogin);
            this.groupBox2.Controls.Add(this.txtServiceAccountPassword);
            this.groupBox2.Controls.Add(this.btnSaveAdmin);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(19, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(523, 301);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Admin settings";
            // 
            // btnTestConnectionAdmin
            // 
            this.btnTestConnectionAdmin.Enabled = false;
            this.btnTestConnectionAdmin.Location = new System.Drawing.Point(262, 168);
            this.btnTestConnectionAdmin.Name = "btnTestConnectionAdmin";
            this.btnTestConnectionAdmin.Size = new System.Drawing.Size(118, 23);
            this.btnTestConnectionAdmin.TabIndex = 19;
            this.btnTestConnectionAdmin.Text = "Test Connection";
            this.btnTestConnectionAdmin.UseVisualStyleBackColor = true;
            this.btnTestConnectionAdmin.Click += new System.EventHandler(this.btnTestConnectionAdmin_Click);
            // 
            // txtApiBaseUrl
            // 
            this.txtApiBaseUrl.Location = new System.Drawing.Point(160, 19);
            this.txtApiBaseUrl.Name = "txtApiBaseUrl";
            this.txtApiBaseUrl.Size = new System.Drawing.Size(301, 20);
            this.txtApiBaseUrl.TabIndex = 1;
            // 
            // txtApiAccountId
            // 
            this.txtApiAccountId.Location = new System.Drawing.Point(160, 46);
            this.txtApiAccountId.Name = "txtApiAccountId";
            this.txtApiAccountId.Size = new System.Drawing.Size(301, 20);
            this.txtApiAccountId.TabIndex = 2;
            // 
            // lblResultAdmin
            // 
            this.lblResultAdmin.AutoSize = true;
            this.lblResultAdmin.Location = new System.Drawing.Point(14, 223);
            this.lblResultAdmin.Name = "lblResultAdmin";
            this.lblResultAdmin.Size = new System.Drawing.Size(25, 13);
            this.lblResultAdmin.TabIndex = 21;
            this.lblResultAdmin.Text = " .....";
            // 
            // txtIntegrationKey
            // 
            this.txtIntegrationKey.Location = new System.Drawing.Point(160, 73);
            this.txtIntegrationKey.Name = "txtIntegrationKey";
            this.txtIntegrationKey.Size = new System.Drawing.Size(301, 20);
            this.txtIntegrationKey.TabIndex = 4;
            // 
            // txtServiceAccountLogin
            // 
            this.txtServiceAccountLogin.Location = new System.Drawing.Point(160, 100);
            this.txtServiceAccountLogin.Name = "txtServiceAccountLogin";
            this.txtServiceAccountLogin.Size = new System.Drawing.Size(301, 20);
            this.txtServiceAccountLogin.TabIndex = 5;
            // 
            // txtServiceAccountPassword
            // 
            this.txtServiceAccountPassword.Location = new System.Drawing.Point(160, 127);
            this.txtServiceAccountPassword.Name = "txtServiceAccountPassword";
            this.txtServiceAccountPassword.Size = new System.Drawing.Size(301, 20);
            this.txtServiceAccountPassword.TabIndex = 6;
            // 
            // btnSaveAdmin
            // 
            this.btnSaveAdmin.Location = new System.Drawing.Point(160, 168);
            this.btnSaveAdmin.Name = "btnSaveAdmin";
            this.btnSaveAdmin.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAdmin.TabIndex = 17;
            this.btnSaveAdmin.Text = "Save";
            this.btnSaveAdmin.UseVisualStyleBackColor = true;
            this.btnSaveAdmin.Click += new System.EventHandler(this.btnSaveAdmin_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "API Base Url";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Account Id";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Integration Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Service Account Login";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Service Account Password";
            // 
            // tabCreateEnvelope
            // 
            this.tabCreateEnvelope.Controls.Add(this.btnCreateAndSendEnvelope);
            this.tabCreateEnvelope.Controls.Add(this.label7);
            this.tabCreateEnvelope.Controls.Add(this.dtpExpiration);
            this.tabCreateEnvelope.Controls.Add(this.chkExpiration);
            this.tabCreateEnvelope.Controls.Add(this.chkReminders);
            this.tabCreateEnvelope.Controls.Add(this.dtpReminders);
            this.tabCreateEnvelope.Controls.Add(this.lblEnvelopeId);
            this.tabCreateEnvelope.Controls.Add(this.btnCreateEnvelope);
            this.tabCreateEnvelope.Controls.Add(this.groupBox1);
            this.tabCreateEnvelope.Controls.Add(this.lblUploadFilePath2);
            this.tabCreateEnvelope.Controls.Add(this.btnUploadFile2);
            this.tabCreateEnvelope.Controls.Add(this.lblUploadFilePath);
            this.tabCreateEnvelope.Controls.Add(this.btnUploadFile);
            this.tabCreateEnvelope.Controls.Add(this.txtBlurb);
            this.tabCreateEnvelope.Controls.Add(this.txtSubject);
            this.tabCreateEnvelope.Controls.Add(this.label11);
            this.tabCreateEnvelope.Controls.Add(this.label10);
            this.tabCreateEnvelope.Location = new System.Drawing.Point(4, 22);
            this.tabCreateEnvelope.Name = "tabCreateEnvelope";
            this.tabCreateEnvelope.Padding = new System.Windows.Forms.Padding(3);
            this.tabCreateEnvelope.Size = new System.Drawing.Size(1072, 538);
            this.tabCreateEnvelope.TabIndex = 1;
            this.tabCreateEnvelope.Text = "Create Envelope";
            this.tabCreateEnvelope.UseVisualStyleBackColor = true;
            // 
            // btnCreateAndSendEnvelope
            // 
            this.btnCreateAndSendEnvelope.Location = new System.Drawing.Point(204, 472);
            this.btnCreateAndSendEnvelope.Name = "btnCreateAndSendEnvelope";
            this.btnCreateAndSendEnvelope.Size = new System.Drawing.Size(159, 23);
            this.btnCreateAndSendEnvelope.TabIndex = 20;
            this.btnCreateAndSendEnvelope.Text = "Create and Send Envelope";
            this.btnCreateAndSendEnvelope.UseVisualStyleBackColor = true;
            this.btnCreateAndSendEnvelope.Click += new System.EventHandler(this.btnCreateAndSendEnvelope_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Envelope Id";
            // 
            // dtpExpiration
            // 
            this.dtpExpiration.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpiration.Location = new System.Drawing.Point(180, 187);
            this.dtpExpiration.Name = "dtpExpiration";
            this.dtpExpiration.Size = new System.Drawing.Size(103, 20);
            this.dtpExpiration.TabIndex = 18;
            this.dtpExpiration.Visible = false;
            // 
            // chkExpiration
            // 
            this.chkExpiration.AutoSize = true;
            this.chkExpiration.Location = new System.Drawing.Point(28, 190);
            this.chkExpiration.Name = "chkExpiration";
            this.chkExpiration.Size = new System.Drawing.Size(94, 17);
            this.chkExpiration.TabIndex = 17;
            this.chkExpiration.Text = "Add Expiration";
            this.chkExpiration.UseVisualStyleBackColor = true;
            this.chkExpiration.CheckedChanged += new System.EventHandler(this.chkExpiration_CheckedChanged);
            // 
            // chkReminders
            // 
            this.chkReminders.AutoSize = true;
            this.chkReminders.Location = new System.Drawing.Point(28, 165);
            this.chkReminders.Name = "chkReminders";
            this.chkReminders.Size = new System.Drawing.Size(124, 17);
            this.chkReminders.TabIndex = 16;
            this.chkReminders.Text = "Add Daily Reminders";
            this.chkReminders.UseVisualStyleBackColor = true;
            this.chkReminders.CheckedChanged += new System.EventHandler(this.chkReminders_CheckedChanged);
            // 
            // dtpReminders
            // 
            this.dtpReminders.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpReminders.Location = new System.Drawing.Point(180, 161);
            this.dtpReminders.Name = "dtpReminders";
            this.dtpReminders.Size = new System.Drawing.Size(103, 20);
            this.dtpReminders.TabIndex = 14;
            this.dtpReminders.Visible = false;
            // 
            // lblEnvelopeId
            // 
            this.lblEnvelopeId.AutoSize = true;
            this.lblEnvelopeId.Location = new System.Drawing.Point(95, 139);
            this.lblEnvelopeId.Name = "lblEnvelopeId";
            this.lblEnvelopeId.Size = new System.Drawing.Size(16, 13);
            this.lblEnvelopeId.TabIndex = 10;
            this.lblEnvelopeId.Text = "...";
            // 
            // btnCreateEnvelope
            // 
            this.btnCreateEnvelope.Location = new System.Drawing.Point(25, 472);
            this.btnCreateEnvelope.Name = "btnCreateEnvelope";
            this.btnCreateEnvelope.Size = new System.Drawing.Size(159, 23);
            this.btnCreateEnvelope.TabIndex = 9;
            this.btnCreateEnvelope.Text = "Create Envelope";
            this.btnCreateEnvelope.UseVisualStyleBackColor = true;
            this.btnCreateEnvelope.Click += new System.EventHandler(this.btnCreateEnvelope_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbRecipientType2);
            this.groupBox1.Controls.Add(this.txtRecipientEmail2);
            this.groupBox1.Controls.Add(this.txtRecipientName2);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.cbRecipientType1);
            this.groupBox1.Controls.Add(this.txtRecipientEmail1);
            this.groupBox1.Controls.Add(this.txtRecipientName1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(28, 218);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(689, 248);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recipients";
            // 
            // cbRecipientType2
            // 
            this.cbRecipientType2.FormattingEnabled = true;
            this.cbRecipientType2.Items.AddRange(new object[] {
            "Signer",
            "InPersonSigner",
            "CarbonCopy",
            "CertifyDelivery",
            "Agent",
            "Editor",
            "Intermediary",
            "SigningHost"});
            this.cbRecipientType2.Location = new System.Drawing.Point(515, 90);
            this.cbRecipientType2.Name = "cbRecipientType2";
            this.cbRecipientType2.Size = new System.Drawing.Size(121, 21);
            this.cbRecipientType2.TabIndex = 17;
            // 
            // txtRecipientEmail2
            // 
            this.txtRecipientEmail2.Location = new System.Drawing.Point(237, 92);
            this.txtRecipientEmail2.Name = "txtRecipientEmail2";
            this.txtRecipientEmail2.Size = new System.Drawing.Size(271, 20);
            this.txtRecipientEmail2.TabIndex = 13;
            // 
            // txtRecipientName2
            // 
            this.txtRecipientName2.Location = new System.Drawing.Point(25, 92);
            this.txtRecipientName2.Name = "txtRecipientName2";
            this.txtRecipientName2.Size = new System.Drawing.Size(198, 20);
            this.txtRecipientName2.TabIndex = 12;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(515, 26);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "Recipient Type";
            // 
            // cbRecipientType1
            // 
            this.cbRecipientType1.FormattingEnabled = true;
            this.cbRecipientType1.Items.AddRange(new object[] {
            "Signer",
            "InPersonSigner",
            "CarbonCopy",
            "CertifyDelivery",
            "Agent",
            "Editor",
            "Intermediary",
            "SigningHost"});
            this.cbRecipientType1.Location = new System.Drawing.Point(515, 51);
            this.cbRecipientType1.Name = "cbRecipientType1";
            this.cbRecipientType1.Size = new System.Drawing.Size(121, 21);
            this.cbRecipientType1.TabIndex = 10;
            // 
            // txtRecipientEmail1
            // 
            this.txtRecipientEmail1.Location = new System.Drawing.Point(237, 53);
            this.txtRecipientEmail1.Name = "txtRecipientEmail1";
            this.txtRecipientEmail1.Size = new System.Drawing.Size(271, 20);
            this.txtRecipientEmail1.TabIndex = 5;
            // 
            // txtRecipientName1
            // 
            this.txtRecipientName1.Location = new System.Drawing.Point(25, 53);
            this.txtRecipientName1.Name = "txtRecipientName1";
            this.txtRecipientName1.Size = new System.Drawing.Size(198, 20);
            this.txtRecipientName1.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(236, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 1;
            this.label13.Text = "Email";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(22, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Name";
            // 
            // lblUploadFilePath2
            // 
            this.lblUploadFilePath2.AutoSize = true;
            this.lblUploadFilePath2.Location = new System.Drawing.Point(560, 59);
            this.lblUploadFilePath2.Name = "lblUploadFilePath2";
            this.lblUploadFilePath2.Size = new System.Drawing.Size(16, 13);
            this.lblUploadFilePath2.TabIndex = 7;
            this.lblUploadFilePath2.Text = "...";
            // 
            // btnUploadFile2
            // 
            this.btnUploadFile2.Location = new System.Drawing.Point(461, 49);
            this.btnUploadFile2.Name = "btnUploadFile2";
            this.btnUploadFile2.Size = new System.Drawing.Size(75, 23);
            this.btnUploadFile2.TabIndex = 6;
            this.btnUploadFile2.Text = "Upload File";
            this.btnUploadFile2.UseVisualStyleBackColor = true;
            this.btnUploadFile2.Click += new System.EventHandler(this.btnUploadFile2_Click);
            // 
            // lblUploadFilePath
            // 
            this.lblUploadFilePath.AutoSize = true;
            this.lblUploadFilePath.Location = new System.Drawing.Point(559, 26);
            this.lblUploadFilePath.Name = "lblUploadFilePath";
            this.lblUploadFilePath.Size = new System.Drawing.Size(16, 13);
            this.lblUploadFilePath.TabIndex = 5;
            this.lblUploadFilePath.Text = "...";
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Location = new System.Drawing.Point(460, 16);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(75, 23);
            this.btnUploadFile.TabIndex = 4;
            this.btnUploadFile.Text = "Upload File";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // txtBlurb
            // 
            this.txtBlurb.Location = new System.Drawing.Point(84, 55);
            this.txtBlurb.Multiline = true;
            this.txtBlurb.Name = "txtBlurb";
            this.txtBlurb.Size = new System.Drawing.Size(334, 63);
            this.txtBlurb.TabIndex = 3;
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(84, 20);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(334, 20);
            this.txtSubject.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(31, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Blurb";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(22, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Subject";
            // 
            // tabEditEnvelope
            // 
            this.tabEditEnvelope.Controls.Add(this.label26);
            this.tabEditEnvelope.Controls.Add(this.lblDownloadedFileWithCert);
            this.tabEditEnvelope.Controls.Add(this.button2);
            this.tabEditEnvelope.Controls.Add(this.txtVoidEnvelopeReason);
            this.tabEditEnvelope.Controls.Add(this.btnVoidEnvelope);
            this.tabEditEnvelope.Controls.Add(this.groupBox4);
            this.tabEditEnvelope.Controls.Add(this.lblDownloadedFiles);
            this.tabEditEnvelope.Controls.Add(this.btnDownloadDocuments);
            this.tabEditEnvelope.Controls.Add(this.lblDownloadedFile);
            this.tabEditEnvelope.Controls.Add(this.btnDownloadInOne);
            this.tabEditEnvelope.Controls.Add(this.lblEnvelopeStatus);
            this.tabEditEnvelope.Controls.Add(this.label20);
            this.tabEditEnvelope.Controls.Add(this.btnGetEnvelopeStatus);
            this.tabEditEnvelope.Controls.Add(this.txtEditUrl);
            this.tabEditEnvelope.Controls.Add(this.btnGetEditUrl);
            this.tabEditEnvelope.Controls.Add(this.txtReturnUrl);
            this.tabEditEnvelope.Controls.Add(this.label19);
            this.tabEditEnvelope.Controls.Add(this.label18);
            this.tabEditEnvelope.Controls.Add(this.txtEnvelopeId);
            this.tabEditEnvelope.Location = new System.Drawing.Point(4, 22);
            this.tabEditEnvelope.Name = "tabEditEnvelope";
            this.tabEditEnvelope.Padding = new System.Windows.Forms.Padding(3);
            this.tabEditEnvelope.Size = new System.Drawing.Size(1072, 538);
            this.tabEditEnvelope.TabIndex = 2;
            this.tabEditEnvelope.Text = "Edit Envelope";
            this.tabEditEnvelope.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(539, 95);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 13);
            this.label26.TabIndex = 19;
            this.label26.Text = "Void Reason:";
            // 
            // lblDownloadedFileWithCert
            // 
            this.lblDownloadedFileWithCert.AutoSize = true;
            this.lblDownloadedFileWithCert.Location = new System.Drawing.Point(219, 153);
            this.lblDownloadedFileWithCert.Name = "lblDownloadedFileWithCert";
            this.lblDownloadedFileWithCert.Size = new System.Drawing.Size(16, 13);
            this.lblDownloadedFileWithCert.TabIndex = 17;
            this.lblDownloadedFileWithCert.Text = "...";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 143);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Dawnload Docs With Cert All in One";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtVoidEnvelopeReason
            // 
            this.txtVoidEnvelopeReason.Location = new System.Drawing.Point(539, 119);
            this.txtVoidEnvelopeReason.Multiline = true;
            this.txtVoidEnvelopeReason.Name = "txtVoidEnvelopeReason";
            this.txtVoidEnvelopeReason.Size = new System.Drawing.Size(188, 69);
            this.txtVoidEnvelopeReason.TabIndex = 15;
            // 
            // btnVoidEnvelope
            // 
            this.btnVoidEnvelope.Location = new System.Drawing.Point(539, 56);
            this.btnVoidEnvelope.Name = "btnVoidEnvelope";
            this.btnVoidEnvelope.Size = new System.Drawing.Size(188, 23);
            this.btnVoidEnvelope.TabIndex = 14;
            this.btnVoidEnvelope.Text = "Void Envelope";
            this.btnVoidEnvelope.UseVisualStyleBackColor = true;
            this.btnVoidEnvelope.Click += new System.EventHandler(this.btnVoidEnvelope_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblBlurb);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label25);
            this.groupBox4.Controls.Add(this.lblEnvvelopeSubject);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.btnGetEnvelope);
            this.groupBox4.Location = new System.Drawing.Point(22, 193);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(980, 320);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Correct Envelope";
            // 
            // lblBlurb
            // 
            this.lblBlurb.Location = new System.Drawing.Point(276, 50);
            this.lblBlurb.Multiline = true;
            this.lblBlurb.Name = "lblBlurb";
            this.lblBlurb.ReadOnly = true;
            this.lblBlurb.Size = new System.Drawing.Size(649, 62);
            this.lblBlurb.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(217, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Blurb";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(214, 16);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(43, 13);
            this.label25.TabIndex = 24;
            this.label25.Text = "Subject";
            // 
            // lblEnvvelopeSubject
            // 
            this.lblEnvvelopeSubject.AutoSize = true;
            this.lblEnvvelopeSubject.Location = new System.Drawing.Point(273, 16);
            this.lblEnvvelopeSubject.Name = "lblEnvvelopeSubject";
            this.lblEnvvelopeSubject.Size = new System.Drawing.Size(19, 13);
            this.lblEnvvelopeSubject.TabIndex = 23;
            this.lblEnvvelopeSubject.Text = "....";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtCorrectionRecipientEmail2);
            this.groupBox5.Controls.Add(this.txtCorrectionRecipientName2);
            this.groupBox5.Controls.Add(this.txtCorrectionRecipientEmail1);
            this.groupBox5.Controls.Add(this.txtCorrectionRecipientName1);
            this.groupBox5.Controls.Add(this.label23);
            this.groupBox5.Controls.Add(this.label24);
            this.groupBox5.Location = new System.Drawing.Point(6, 118);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(919, 178);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Recipients";
            // 
            // txtCorrectionRecipientEmail2
            // 
            this.txtCorrectionRecipientEmail2.Location = new System.Drawing.Point(237, 92);
            this.txtCorrectionRecipientEmail2.Name = "txtCorrectionRecipientEmail2";
            this.txtCorrectionRecipientEmail2.Size = new System.Drawing.Size(320, 20);
            this.txtCorrectionRecipientEmail2.TabIndex = 13;
            // 
            // txtCorrectionRecipientName2
            // 
            this.txtCorrectionRecipientName2.Location = new System.Drawing.Point(25, 92);
            this.txtCorrectionRecipientName2.Name = "txtCorrectionRecipientName2";
            this.txtCorrectionRecipientName2.Size = new System.Drawing.Size(198, 20);
            this.txtCorrectionRecipientName2.TabIndex = 12;
            // 
            // txtCorrectionRecipientEmail1
            // 
            this.txtCorrectionRecipientEmail1.Location = new System.Drawing.Point(237, 53);
            this.txtCorrectionRecipientEmail1.Name = "txtCorrectionRecipientEmail1";
            this.txtCorrectionRecipientEmail1.Size = new System.Drawing.Size(320, 20);
            this.txtCorrectionRecipientEmail1.TabIndex = 5;
            // 
            // txtCorrectionRecipientName1
            // 
            this.txtCorrectionRecipientName1.Location = new System.Drawing.Point(25, 53);
            this.txtCorrectionRecipientName1.Name = "txtCorrectionRecipientName1";
            this.txtCorrectionRecipientName1.Size = new System.Drawing.Size(198, 20);
            this.txtCorrectionRecipientName1.TabIndex = 4;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(236, 26);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(32, 13);
            this.label23.TabIndex = 1;
            this.label23.Text = "Email";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(22, 26);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(35, 13);
            this.label24.TabIndex = 0;
            this.label24.Text = "Name";
            // 
            // btnGetEnvelope
            // 
            this.btnGetEnvelope.Location = new System.Drawing.Point(15, 33);
            this.btnGetEnvelope.Name = "btnGetEnvelope";
            this.btnGetEnvelope.Size = new System.Drawing.Size(173, 23);
            this.btnGetEnvelope.TabIndex = 0;
            this.btnGetEnvelope.Text = "Get Envelope";
            this.btnGetEnvelope.UseVisualStyleBackColor = true;
            this.btnGetEnvelope.Click += new System.EventHandler(this.btnGetEnvelope_Click);
            // 
            // lblDownloadedFiles
            // 
            this.lblDownloadedFiles.AutoSize = true;
            this.lblDownloadedFiles.Location = new System.Drawing.Point(216, 124);
            this.lblDownloadedFiles.Name = "lblDownloadedFiles";
            this.lblDownloadedFiles.Size = new System.Drawing.Size(16, 13);
            this.lblDownloadedFiles.TabIndex = 12;
            this.lblDownloadedFiles.Text = "...";
            // 
            // btnDownloadDocuments
            // 
            this.btnDownloadDocuments.Location = new System.Drawing.Point(22, 114);
            this.btnDownloadDocuments.Name = "btnDownloadDocuments";
            this.btnDownloadDocuments.Size = new System.Drawing.Size(188, 23);
            this.btnDownloadDocuments.TabIndex = 11;
            this.btnDownloadDocuments.Text = "Download Documents";
            this.btnDownloadDocuments.UseVisualStyleBackColor = true;
            this.btnDownloadDocuments.Click += new System.EventHandler(this.btnDownloadDocuments_Click);
            // 
            // lblDownloadedFile
            // 
            this.lblDownloadedFile.AutoSize = true;
            this.lblDownloadedFile.Location = new System.Drawing.Point(216, 95);
            this.lblDownloadedFile.Name = "lblDownloadedFile";
            this.lblDownloadedFile.Size = new System.Drawing.Size(16, 13);
            this.lblDownloadedFile.TabIndex = 10;
            this.lblDownloadedFile.Text = "...";
            // 
            // btnDownloadInOne
            // 
            this.btnDownloadInOne.Location = new System.Drawing.Point(22, 85);
            this.btnDownloadInOne.Name = "btnDownloadInOne";
            this.btnDownloadInOne.Size = new System.Drawing.Size(188, 23);
            this.btnDownloadInOne.TabIndex = 9;
            this.btnDownloadInOne.Text = "Download Documents All in One";
            this.btnDownloadInOne.UseVisualStyleBackColor = true;
            this.btnDownloadInOne.Click += new System.EventHandler(this.btnDownloadInOne_Click);
            // 
            // lblEnvelopeStatus
            // 
            this.lblEnvelopeStatus.AutoSize = true;
            this.lblEnvelopeStatus.Location = new System.Drawing.Point(263, 66);
            this.lblEnvelopeStatus.Name = "lblEnvelopeStatus";
            this.lblEnvelopeStatus.Size = new System.Drawing.Size(16, 13);
            this.lblEnvelopeStatus.TabIndex = 8;
            this.lblEnvelopeStatus.Text = "...";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(216, 66);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(40, 13);
            this.label20.TabIndex = 7;
            this.label20.Text = "Status:";
            // 
            // btnGetEnvelopeStatus
            // 
            this.btnGetEnvelopeStatus.Location = new System.Drawing.Point(22, 56);
            this.btnGetEnvelopeStatus.Name = "btnGetEnvelopeStatus";
            this.btnGetEnvelopeStatus.Size = new System.Drawing.Size(188, 23);
            this.btnGetEnvelopeStatus.TabIndex = 6;
            this.btnGetEnvelopeStatus.Text = "Get Envelope Status";
            this.btnGetEnvelopeStatus.UseVisualStyleBackColor = true;
            this.btnGetEnvelopeStatus.Click += new System.EventHandler(this.btnGetEnvelopeStatus_Click);
            // 
            // txtEditUrl
            // 
            this.txtEditUrl.Location = new System.Drawing.Point(754, 88);
            this.txtEditUrl.Name = "txtEditUrl";
            this.txtEditUrl.Size = new System.Drawing.Size(288, 20);
            this.txtEditUrl.TabIndex = 5;
            // 
            // btnGetEditUrl
            // 
            this.btnGetEditUrl.Location = new System.Drawing.Point(754, 56);
            this.btnGetEditUrl.Name = "btnGetEditUrl";
            this.btnGetEditUrl.Size = new System.Drawing.Size(147, 23);
            this.btnGetEditUrl.TabIndex = 4;
            this.btnGetEditUrl.Text = "Get Edit Envelope URL";
            this.btnGetEditUrl.UseVisualStyleBackColor = true;
            this.btnGetEditUrl.Click += new System.EventHandler(this.btnGetEditUrl_Click);
            // 
            // txtReturnUrl
            // 
            this.txtReturnUrl.Location = new System.Drawing.Point(410, 18);
            this.txtReturnUrl.Name = "txtReturnUrl";
            this.txtReturnUrl.Size = new System.Drawing.Size(236, 20);
            this.txtReturnUrl.TabIndex = 3;
            this.txtReturnUrl.Text = "http://localhost/index.html";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(340, 25);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 13);
            this.label19.TabIndex = 2;
            this.label19.Text = "Return URL";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(16, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 13);
            this.label18.TabIndex = 1;
            this.label18.Text = "Envelope Id";
            // 
            // txtEnvelopeId
            // 
            this.txtEnvelopeId.Location = new System.Drawing.Point(83, 18);
            this.txtEnvelopeId.Name = "txtEnvelopeId";
            this.txtEnvelopeId.Size = new System.Drawing.Size(231, 20);
            this.txtEnvelopeId.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 588);
            this.Controls.Add(this.tabDocuSign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "DocuSign Samples";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabDocuSign.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabCreateEnvelope.ResumeLayout(false);
            this.tabCreateEnvelope.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabEditEnvelope.ResumeLayout(false);
            this.tabEditEnvelope.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabDocuSign;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabCreateEnvelope;
        private System.Windows.Forms.TextBox txtApiAccountId;
        private System.Windows.Forms.TextBox txtApiBaseUrl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServiceAccountPassword;
        private System.Windows.Forms.TextBox txtServiceAccountLogin;
        private System.Windows.Forms.TextBox txtIntegrationKey;
        private System.Windows.Forms.TextBox txtUserEmail;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnTestConnectionUser;
        private System.Windows.Forms.Button btnTestConnectionAdmin;
        private System.Windows.Forms.Button btnSaveUser;
        private System.Windows.Forms.Button btnSaveAdmin;
        private System.Windows.Forms.Label lblResultAdmin;
        private System.Windows.Forms.Label lblResultUser;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.TextBox txtBlurb;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblUploadFilePath;
        private System.Windows.Forms.Label lblUploadFilePath2;
        private System.Windows.Forms.Button btnUploadFile2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtRecipientEmail1;
        private System.Windows.Forms.TextBox txtRecipientName1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cbRecipientType2;
        private System.Windows.Forms.TextBox txtRecipientEmail2;
        private System.Windows.Forms.TextBox txtRecipientName2;
        private System.Windows.Forms.Label lblEnvelopeId;
        private System.Windows.Forms.Button btnCreateEnvelope;
        private System.Windows.Forms.ComboBox cbRecipientType1;
        private System.Windows.Forms.DateTimePicker dtpReminders;
        private System.Windows.Forms.CheckBox chkReminders;
        private System.Windows.Forms.CheckBox chkExpiration;
        private System.Windows.Forms.DateTimePicker dtpExpiration;
        private System.Windows.Forms.TabPage tabEditEnvelope;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtEnvelopeId;
        private System.Windows.Forms.TextBox txtEditUrl;
        private System.Windows.Forms.Button btnGetEditUrl;
        private System.Windows.Forms.TextBox txtReturnUrl;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblEnvelopeStatus;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnGetEnvelopeStatus;
        private System.Windows.Forms.Button btnDownloadInOne;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblDownloadedFile;
        private System.Windows.Forms.Label lblDownloadedFiles;
        private System.Windows.Forms.Button btnDownloadDocuments;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnGetEnvelope;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtCorrectionRecipientEmail2;
        private System.Windows.Forms.TextBox txtCorrectionRecipientName2;
        private System.Windows.Forms.TextBox txtCorrectionRecipientEmail1;
        private System.Windows.Forms.TextBox txtCorrectionRecipientName1;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblEnvvelopeSubject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button btnVoidEnvelope;
        private System.Windows.Forms.TextBox txtVoidEnvelopeReason;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblDownloadedFileWithCert;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnCreateAndSendEnvelope;
        private System.Windows.Forms.TextBox lblBlurb;
        private System.Windows.Forms.Label label1;
    }
}

