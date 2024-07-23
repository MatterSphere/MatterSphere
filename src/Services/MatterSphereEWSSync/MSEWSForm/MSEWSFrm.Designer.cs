namespace MSEWSForm
{
    partial class MSEWSFrm
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
            this.btnRunProcess = new System.Windows.Forms.Button();
            this.txtMSServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnEditConfig = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMSDB = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTimeZoneTbl = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTimeZoneCol = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtBranchCode = new System.Windows.Forms.TextBox();
            this.cmbTimeZone = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.gbMatterSphere = new System.Windows.Forms.GroupBox();
            this.cmbLoginType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSetAppointmentTypeAsCategory = new System.Windows.Forms.CheckBox();
            this.gbTimeZone = new System.Windows.Forms.GroupBox();
            this.gbExchange = new System.Windows.Forms.GroupBox();
            this.rbBasic = new System.Windows.Forms.RadioButton();
            this.rbNtlm = new System.Windows.Forms.RadioButton();
            this.rbOAuth = new System.Windows.Forms.RadioButton();
            this.cmbFreeBusy = new System.Windows.Forms.ComboBox();
            this.label46 = new System.Windows.Forms.Label();
            this.txtServiceEmail = new System.Windows.Forms.TextBox();
            this.txtWebServices = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.chkUseAutoDisc = new System.Windows.Forms.CheckBox();
            this.lblExcPassword = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.txtCertSer = new System.Windows.Forms.TextBox();
            this.chkCert = new System.Windows.Forms.CheckBox();
            this.txtExcPassword = new System.Windows.Forms.TextBox();
            this.txtExcUser = new System.Windows.Forms.TextBox();
            this.lblExcDomain = new System.Windows.Forms.Label();
            this.lblExcUser = new System.Windows.Forms.Label();
            this.txtExcDomain = new System.Windows.Forms.TextBox();
            this.lblOAuthApp = new System.Windows.Forms.Label();
            this.txtOAuthApp = new System.Windows.Forms.TextBox();
            this.lblOAuthClientSecret = new System.Windows.Forms.Label();
            this.txtOAuthClientSecret = new System.Windows.Forms.TextBox();
            this.lblOAuthTenant = new System.Windows.Forms.Label();
            this.txtOAuthTenant = new System.Windows.Forms.TextBox();
            this.gbService = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.label17 = new System.Windows.Forms.Label();
            this.btnRunDeleteProcess = new System.Windows.Forms.Button();
            this.btnCheckCertNumber = new System.Windows.Forms.Button();
            this.gbActions = new System.Windows.Forms.GroupBox();
            this.gbMatterSphere.SuspendLayout();
            this.gbTimeZone.SuspendLayout();
            this.gbExchange.SuspendLayout();
            this.gbService.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRunProcess
            // 
            this.btnRunProcess.Location = new System.Drawing.Point(10, 138);
            this.btnRunProcess.Name = "btnRunProcess";
            this.btnRunProcess.Size = new System.Drawing.Size(306, 30);
            this.btnRunProcess.TabIndex = 1;
            this.btnRunProcess.Text = "Run Process Manually";
            this.btnRunProcess.UseVisualStyleBackColor = true;
            this.btnRunProcess.Click += new System.EventHandler(this.btnRunProcess_Click);
            // 
            // txtMSServer
            // 
            this.txtMSServer.Enabled = false;
            this.txtMSServer.Location = new System.Drawing.Point(133, 21);
            this.txtMSServer.Name = "txtMSServer";
            this.txtMSServer.Size = new System.Drawing.Size(183, 20);
            this.txtMSServer.TabIndex = 32;
            this.txtMSServer.Tag = "MatterSphereServer";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "MatterSphere Server";
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(10, 21);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(306, 30);
            this.btnLoadConfig.TabIndex = 4;
            this.btnLoadConfig.Text = "Re-Load Configuration";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Enabled = false;
            this.btnSaveConfig.Location = new System.Drawing.Point(10, 95);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(306, 30);
            this.btnSaveConfig.TabIndex = 6;
            this.btnSaveConfig.Text = "Save Configuration";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEditConfig
            // 
            this.btnEditConfig.Enabled = false;
            this.btnEditConfig.Location = new System.Drawing.Point(10, 58);
            this.btnEditConfig.Name = "btnEditConfig";
            this.btnEditConfig.Size = new System.Drawing.Size(306, 30);
            this.btnEditConfig.TabIndex = 5;
            this.btnEditConfig.Text = "Edit Configuration";
            this.btnEditConfig.UseVisualStyleBackColor = true;
            this.btnEditConfig.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "MatterSphere Database";
            // 
            // txtMSDB
            // 
            this.txtMSDB.Enabled = false;
            this.txtMSDB.Location = new System.Drawing.Point(133, 46);
            this.txtMSDB.Name = "txtMSDB";
            this.txtMSDB.Size = new System.Drawing.Size(183, 20);
            this.txtMSDB.TabIndex = 34;
            this.txtMSDB.Tag = "MatterSphereDatabase";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "Time Zone Table Name";
            // 
            // txtTimeZoneTbl
            // 
            this.txtTimeZoneTbl.Enabled = false;
            this.txtTimeZoneTbl.Location = new System.Drawing.Point(133, 25);
            this.txtTimeZoneTbl.Name = "txtTimeZoneTbl";
            this.txtTimeZoneTbl.Size = new System.Drawing.Size(183, 20);
            this.txtTimeZoneTbl.TabIndex = 44;
            this.txtTimeZoneTbl.Tag = "TimeZoneTableName";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Time Zone Column";
            // 
            // txtTimeZoneCol
            // 
            this.txtTimeZoneCol.Enabled = false;
            this.txtTimeZoneCol.Location = new System.Drawing.Point(133, 50);
            this.txtTimeZoneCol.Name = "txtTimeZoneCol";
            this.txtTimeZoneCol.Size = new System.Drawing.Size(183, 20);
            this.txtTimeZoneCol.TabIndex = 46;
            this.txtTimeZoneCol.Tag = "TimeZoneColumnName";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(7, 99);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(112, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Branch Code To Sync";
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Enabled = false;
            this.txtBranchCode.Location = new System.Drawing.Point(133, 96);
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Size = new System.Drawing.Size(65, 20);
            this.txtBranchCode.TabIndex = 38;
            this.txtBranchCode.Tag = "BranchCodeToSync";
            // 
            // cmbTimeZone
            // 
            this.cmbTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeZone.Enabled = false;
            this.cmbTimeZone.FormattingEnabled = true;
            this.cmbTimeZone.Location = new System.Drawing.Point(133, 75);
            this.cmbTimeZone.Name = "cmbTimeZone";
            this.cmbTimeZone.Size = new System.Drawing.Size(183, 21);
            this.cmbTimeZone.TabIndex = 48;
            this.cmbTimeZone.Tag = "DefaultTimeZone";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 78);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 13);
            this.label24.TabIndex = 47;
            this.label24.Text = "Time Zone";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(198, 99);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(118, 13);
            this.label25.TabIndex = 39;
            this.label25.Text = "(Use 0 for All Branches)";
            // 
            // gbMatterSphere
            // 
            this.gbMatterSphere.Controls.Add(this.cmbLoginType);
            this.gbMatterSphere.Controls.Add(this.label4);
            this.gbMatterSphere.Controls.Add(this.label3);
            this.gbMatterSphere.Controls.Add(this.chkSetAppointmentTypeAsCategory);
            this.gbMatterSphere.Controls.Add(this.label25);
            this.gbMatterSphere.Controls.Add(this.label1);
            this.gbMatterSphere.Controls.Add(this.txtMSServer);
            this.gbMatterSphere.Controls.Add(this.txtMSDB);
            this.gbMatterSphere.Controls.Add(this.label2);
            this.gbMatterSphere.Controls.Add(this.txtBranchCode);
            this.gbMatterSphere.Controls.Add(this.label16);
            this.gbMatterSphere.Location = new System.Drawing.Point(12, 326);
            this.gbMatterSphere.Name = "gbMatterSphere";
            this.gbMatterSphere.Size = new System.Drawing.Size(330, 148);
            this.gbMatterSphere.TabIndex = 30;
            this.gbMatterSphere.TabStop = false;
            this.gbMatterSphere.Text = "MatterSphere Settings";
            // 
            // cmbLoginType
            // 
            this.cmbLoginType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoginType.FormattingEnabled = true;
            this.cmbLoginType.Items.AddRange(new object[] {
            "NT",
            "AAD"});
            this.cmbLoginType.Location = new System.Drawing.Point(133, 71);
            this.cmbLoginType.Name = "cmbLoginType";
            this.cmbLoginType.Size = new System.Drawing.Size(183, 21);
            this.cmbLoginType.TabIndex = 36;
            this.cmbLoginType.Tag = "MatterSphereLoginType";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "MatterSphere Login Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 40;
            this.label3.Text = "Set Category (App Type)";
            // 
            // chkSetAppointmentTypeAsCategory
            // 
            this.chkSetAppointmentTypeAsCategory.AutoSize = true;
            this.chkSetAppointmentTypeAsCategory.Enabled = false;
            this.chkSetAppointmentTypeAsCategory.Location = new System.Drawing.Point(133, 122);
            this.chkSetAppointmentTypeAsCategory.Name = "chkSetAppointmentTypeAsCategory";
            this.chkSetAppointmentTypeAsCategory.Size = new System.Drawing.Size(15, 14);
            this.chkSetAppointmentTypeAsCategory.TabIndex = 41;
            this.chkSetAppointmentTypeAsCategory.Tag = "SetAppointmentTypeAsCategory";
            this.chkSetAppointmentTypeAsCategory.UseVisualStyleBackColor = true;
            // 
            // gbTimeZone
            // 
            this.gbTimeZone.Controls.Add(this.txtTimeZoneTbl);
            this.gbTimeZone.Controls.Add(this.label8);
            this.gbTimeZone.Controls.Add(this.label24);
            this.gbTimeZone.Controls.Add(this.txtTimeZoneCol);
            this.gbTimeZone.Controls.Add(this.cmbTimeZone);
            this.gbTimeZone.Controls.Add(this.label9);
            this.gbTimeZone.Location = new System.Drawing.Point(12, 480);
            this.gbTimeZone.Name = "gbTimeZone";
            this.gbTimeZone.Size = new System.Drawing.Size(330, 106);
            this.gbTimeZone.TabIndex = 42;
            this.gbTimeZone.TabStop = false;
            this.gbTimeZone.Text = "Time Zone Settings";
            // 
            // gbExchange
            // 
            this.gbExchange.Controls.Add(this.rbBasic);
            this.gbExchange.Controls.Add(this.rbNtlm);
            this.gbExchange.Controls.Add(this.rbOAuth);
            this.gbExchange.Controls.Add(this.cmbFreeBusy);
            this.gbExchange.Controls.Add(this.label46);
            this.gbExchange.Controls.Add(this.txtServiceEmail);
            this.gbExchange.Controls.Add(this.txtWebServices);
            this.gbExchange.Controls.Add(this.label47);
            this.gbExchange.Controls.Add(this.label48);
            this.gbExchange.Controls.Add(this.label49);
            this.gbExchange.Controls.Add(this.chkUseAutoDisc);
            this.gbExchange.Controls.Add(this.lblExcPassword);
            this.gbExchange.Controls.Add(this.label50);
            this.gbExchange.Controls.Add(this.label51);
            this.gbExchange.Controls.Add(this.txtDelay);
            this.gbExchange.Controls.Add(this.label52);
            this.gbExchange.Controls.Add(this.label53);
            this.gbExchange.Controls.Add(this.txtCertSer);
            this.gbExchange.Controls.Add(this.chkCert);
            this.gbExchange.Controls.Add(this.txtExcPassword);
            this.gbExchange.Controls.Add(this.txtExcUser);
            this.gbExchange.Controls.Add(this.lblExcDomain);
            this.gbExchange.Controls.Add(this.lblExcUser);
            this.gbExchange.Controls.Add(this.txtExcDomain);
            this.gbExchange.Controls.Add(this.lblOAuthApp);
            this.gbExchange.Controls.Add(this.txtOAuthApp);
            this.gbExchange.Controls.Add(this.lblOAuthClientSecret);
            this.gbExchange.Controls.Add(this.txtOAuthClientSecret);
            this.gbExchange.Controls.Add(this.lblOAuthTenant);
            this.gbExchange.Controls.Add(this.txtOAuthTenant);
            this.gbExchange.Location = new System.Drawing.Point(348, 12);
            this.gbExchange.Name = "gbExchange";
            this.gbExchange.Size = new System.Drawing.Size(330, 308);
            this.gbExchange.TabIndex = 49;
            this.gbExchange.TabStop = false;
            this.gbExchange.Text = "Exchange Settings";
            // 
            // rbBasic
            // 
            this.rbBasic.AutoSize = true;
            this.rbBasic.Checked = true;
            this.rbBasic.Enabled = false;
            this.rbBasic.Location = new System.Drawing.Point(133, 95);
            this.rbBasic.Name = "rbBasic";
            this.rbBasic.Size = new System.Drawing.Size(51, 17);
            this.rbBasic.TabIndex = 57;
            this.rbBasic.TabStop = true;
            this.rbBasic.Tag = "OverrideExchangeCredentials";
            this.rbBasic.Text = "Basic";
            this.rbBasic.UseVisualStyleBackColor = true;
            this.rbBasic.CheckedChanged += new System.EventHandler(this.rbAuthType_CheckedChanged);
            // 
            // rbNtlm
            // 
            this.rbNtlm.AutoSize = true;
            this.rbNtlm.Enabled = false;
            this.rbNtlm.Location = new System.Drawing.Point(193, 95);
            this.rbNtlm.Name = "rbNtlm";
            this.rbNtlm.Size = new System.Drawing.Size(55, 17);
            this.rbNtlm.TabIndex = 58;
            this.rbNtlm.Text = "NTLM";
            this.rbNtlm.UseVisualStyleBackColor = true;
            this.rbNtlm.CheckedChanged += new System.EventHandler(this.rbAuthType_CheckedChanged);
            // 
            // rbOAuth
            // 
            this.rbOAuth.AutoSize = true;
            this.rbOAuth.Enabled = false;
            this.rbOAuth.Location = new System.Drawing.Point(257, 95);
            this.rbOAuth.Name = "rbOAuth";
            this.rbOAuth.Size = new System.Drawing.Size(55, 17);
            this.rbOAuth.TabIndex = 59;
            this.rbOAuth.Tag = "UseOAuth";
            this.rbOAuth.Text = "OAuth";
            this.rbOAuth.UseVisualStyleBackColor = true;
            this.rbOAuth.CheckedChanged += new System.EventHandler(this.rbAuthType_CheckedChanged);
            // 
            // cmbFreeBusy
            // 
            this.cmbFreeBusy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFreeBusy.Enabled = false;
            this.cmbFreeBusy.FormattingEnabled = true;
            this.cmbFreeBusy.Location = new System.Drawing.Point(133, 277);
            this.cmbFreeBusy.Name = "cmbFreeBusy";
            this.cmbFreeBusy.Size = new System.Drawing.Size(183, 21);
            this.cmbFreeBusy.TabIndex = 73;
            this.cmbFreeBusy.Tag = "FreeBusyStatus";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(7, 72);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(94, 13);
            this.label46.TabIndex = 54;
            this.label46.Text = "Webservices URL";
            // 
            // txtServiceEmail
            // 
            this.txtServiceEmail.Enabled = false;
            this.txtServiceEmail.Location = new System.Drawing.Point(133, 19);
            this.txtServiceEmail.Name = "txtServiceEmail";
            this.txtServiceEmail.Size = new System.Drawing.Size(183, 20);
            this.txtServiceEmail.TabIndex = 51;
            this.txtServiceEmail.Tag = "ServiceEmailAddress";
            // 
            // txtWebServices
            // 
            this.txtWebServices.Enabled = false;
            this.txtWebServices.Location = new System.Drawing.Point(133, 69);
            this.txtWebServices.Name = "txtWebServices";
            this.txtWebServices.Size = new System.Drawing.Size(183, 20);
            this.txtWebServices.TabIndex = 55;
            this.txtWebServices.Tag = "ExchangeWebServicesURL";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(7, 22);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(112, 13);
            this.label47.TabIndex = 50;
            this.label47.Text = "Service Email Address";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(7, 280);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(87, 13);
            this.label48.TabIndex = 72;
            this.label48.Text = "Free Busy Status";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(7, 48);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(96, 13);
            this.label49.TabIndex = 52;
            this.label49.Text = "Use Auto Discover";
            // 
            // chkUseAutoDisc
            // 
            this.chkUseAutoDisc.AutoSize = true;
            this.chkUseAutoDisc.Enabled = false;
            this.chkUseAutoDisc.Location = new System.Drawing.Point(133, 48);
            this.chkUseAutoDisc.Name = "chkUseAutoDisc";
            this.chkUseAutoDisc.Size = new System.Drawing.Size(15, 14);
            this.chkUseAutoDisc.TabIndex = 53;
            this.chkUseAutoDisc.Tag = "UseAutoDiscover";
            this.chkUseAutoDisc.UseVisualStyleBackColor = true;
            // 
            // lblExcPassword
            // 
            this.lblExcPassword.AutoSize = true;
            this.lblExcPassword.Location = new System.Drawing.Point(7, 149);
            this.lblExcPassword.Name = "lblExcPassword";
            this.lblExcPassword.Size = new System.Drawing.Size(104, 13);
            this.lblExcPassword.TabIndex = 62;
            this.lblExcPassword.Text = "Exchange Password";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(7, 254);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(74, 13);
            this.label50.TabIndex = 70;
            this.label50.Text = "Delay Minutes";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(7, 98);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(102, 13);
            this.label51.TabIndex = 56;
            this.label51.Text = "Authentication Type";
            // 
            // txtDelay
            // 
            this.txtDelay.Enabled = false;
            this.txtDelay.Location = new System.Drawing.Point(133, 251);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(183, 20);
            this.txtDelay.TabIndex = 71;
            this.txtDelay.Tag = "SearchAppoinmentMinutes";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(7, 228);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(72, 13);
            this.label52.TabIndex = 68;
            this.label52.Text = "Cert Serial No";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(7, 203);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(125, 13);
            this.label53.TabIndex = 66;
            this.label53.Text = "OverrideCertificateCheck";
            // 
            // txtCertSer
            // 
            this.txtCertSer.Enabled = false;
            this.txtCertSer.Location = new System.Drawing.Point(133, 225);
            this.txtCertSer.Name = "txtCertSer";
            this.txtCertSer.Size = new System.Drawing.Size(183, 20);
            this.txtCertSer.TabIndex = 69;
            this.txtCertSer.Tag = "CertificateSerialNumber";
            // 
            // chkCert
            // 
            this.chkCert.AutoSize = true;
            this.chkCert.Enabled = false;
            this.chkCert.Location = new System.Drawing.Point(133, 203);
            this.chkCert.Name = "chkCert";
            this.chkCert.Size = new System.Drawing.Size(15, 14);
            this.chkCert.TabIndex = 67;
            this.chkCert.TabStop = false;
            this.chkCert.Tag = "OverrideCertificateCheck";
            this.chkCert.UseVisualStyleBackColor = true;
            // 
            // txtExcPassword
            // 
            this.txtExcPassword.Enabled = false;
            this.txtExcPassword.Location = new System.Drawing.Point(133, 146);
            this.txtExcPassword.Name = "txtExcPassword";
            this.txtExcPassword.PasswordChar = '*';
            this.txtExcPassword.Size = new System.Drawing.Size(183, 20);
            this.txtExcPassword.TabIndex = 63;
            this.txtExcPassword.Tag = "ExcPassword";
            // 
            // txtExcUser
            // 
            this.txtExcUser.Enabled = false;
            this.txtExcUser.Location = new System.Drawing.Point(133, 119);
            this.txtExcUser.Name = "txtExcUser";
            this.txtExcUser.Size = new System.Drawing.Size(183, 20);
            this.txtExcUser.TabIndex = 61;
            this.txtExcUser.Tag = "ExcUserName";
            // 
            // lblExcDomain
            // 
            this.lblExcDomain.AutoSize = true;
            this.lblExcDomain.Location = new System.Drawing.Point(7, 176);
            this.lblExcDomain.Name = "lblExcDomain";
            this.lblExcDomain.Size = new System.Drawing.Size(94, 13);
            this.lblExcDomain.TabIndex = 64;
            this.lblExcDomain.Text = "Exchange Domain";
            // 
            // lblExcUser
            // 
            this.lblExcUser.AutoSize = true;
            this.lblExcUser.Location = new System.Drawing.Point(7, 122);
            this.lblExcUser.Name = "lblExcUser";
            this.lblExcUser.Size = new System.Drawing.Size(111, 13);
            this.lblExcUser.TabIndex = 60;
            this.lblExcUser.Text = "Exchange User Name";
            // 
            // txtExcDomain
            // 
            this.txtExcDomain.Enabled = false;
            this.txtExcDomain.Location = new System.Drawing.Point(133, 173);
            this.txtExcDomain.Name = "txtExcDomain";
            this.txtExcDomain.Size = new System.Drawing.Size(183, 20);
            this.txtExcDomain.TabIndex = 65;
            this.txtExcDomain.Tag = "ExcDomain";
            // 
            // lblOAuthApp
            // 
            this.lblOAuthApp.AutoSize = true;
            this.lblOAuthApp.Location = new System.Drawing.Point(7, 122);
            this.lblOAuthApp.Name = "lblOAuthApp";
            this.lblOAuthApp.Size = new System.Drawing.Size(106, 13);
            this.lblOAuthApp.TabIndex = 58;
            this.lblOAuthApp.Text = "OAuth Application ID";
            this.lblOAuthApp.Visible = false;
            // 
            // txtOAuthApp
            // 
            this.txtOAuthApp.Enabled = false;
            this.txtOAuthApp.Location = new System.Drawing.Point(133, 119);
            this.txtOAuthApp.Name = "txtOAuthApp";
            this.txtOAuthApp.Size = new System.Drawing.Size(183, 20);
            this.txtOAuthApp.TabIndex = 59;
            this.txtOAuthApp.Tag = "OAuthAppId";
            this.txtOAuthApp.Visible = false;
            // 
            // lblOAuthClientSecret
            // 
            this.lblOAuthClientSecret.AutoSize = true;
            this.lblOAuthClientSecret.Location = new System.Drawing.Point(7, 149);
            this.lblOAuthClientSecret.Name = "lblOAuthClientSecret";
            this.lblOAuthClientSecret.Size = new System.Drawing.Size(100, 13);
            this.lblOAuthClientSecret.TabIndex = 60;
            this.lblOAuthClientSecret.Text = "OAuth Client Secret";
            this.lblOAuthClientSecret.Visible = false;
            // 
            // txtOAuthClientSecret
            // 
            this.txtOAuthClientSecret.Enabled = false;
            this.txtOAuthClientSecret.Location = new System.Drawing.Point(133, 146);
            this.txtOAuthClientSecret.Name = "txtOAuthClientSecret";
            this.txtOAuthClientSecret.PasswordChar = '*';
            this.txtOAuthClientSecret.Size = new System.Drawing.Size(183, 20);
            this.txtOAuthClientSecret.TabIndex = 61;
            this.txtOAuthClientSecret.Tag = "OAuthClientSecret";
            this.txtOAuthClientSecret.Visible = false;
            // 
            // lblOAuthTenant
            // 
            this.lblOAuthTenant.AutoSize = true;
            this.lblOAuthTenant.Location = new System.Drawing.Point(7, 176);
            this.lblOAuthTenant.Name = "lblOAuthTenant";
            this.lblOAuthTenant.Size = new System.Drawing.Size(88, 13);
            this.lblOAuthTenant.TabIndex = 62;
            this.lblOAuthTenant.Text = "OAuth Tenant ID";
            this.lblOAuthTenant.Visible = false;
            // 
            // txtOAuthTenant
            // 
            this.txtOAuthTenant.Enabled = false;
            this.txtOAuthTenant.Location = new System.Drawing.Point(133, 173);
            this.txtOAuthTenant.Name = "txtOAuthTenant";
            this.txtOAuthTenant.Size = new System.Drawing.Size(183, 20);
            this.txtOAuthTenant.TabIndex = 63;
            this.txtOAuthTenant.Tag = "OAuthTenantId";
            this.txtOAuthTenant.Visible = false;
            // 
            // gbService
            // 
            this.gbService.Controls.Add(this.label18);
            this.gbService.Controls.Add(this.textBox6);
            this.gbService.Controls.Add(this.label5);
            this.gbService.Controls.Add(this.textBox3);
            this.gbService.Controls.Add(this.label6);
            this.gbService.Controls.Add(this.textBox4);
            this.gbService.Controls.Add(this.label7);
            this.gbService.Controls.Add(this.textBox5);
            this.gbService.Controls.Add(this.label10);
            this.gbService.Controls.Add(this.checkBox1);
            this.gbService.Controls.Add(this.checkBox2);
            this.gbService.Controls.Add(this.label11);
            this.gbService.Controls.Add(this.label12);
            this.gbService.Controls.Add(this.dateTimePicker1);
            this.gbService.Controls.Add(this.label13);
            this.gbService.Controls.Add(this.label14);
            this.gbService.Controls.Add(this.dateTimePicker2);
            this.gbService.Controls.Add(this.checkBox3);
            this.gbService.Controls.Add(this.label15);
            this.gbService.Controls.Add(this.dateTimePicker3);
            this.gbService.Controls.Add(this.dateTimePicker4);
            this.gbService.Controls.Add(this.label17);
            this.gbService.Location = new System.Drawing.Point(12, 12);
            this.gbService.Name = "gbService";
            this.gbService.Size = new System.Drawing.Size(330, 308);
            this.gbService.TabIndex = 7;
            this.gbService.TabStop = false;
            this.gbService.Text = "Service Settings";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(7, 280);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(107, 13);
            this.label18.TabIndex = 28;
            this.label18.Text = "Ignore Updates Secs";
            // 
            // textBox6
            // 
            this.textBox6.Enabled = false;
            this.textBox6.Location = new System.Drawing.Point(133, 277);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(183, 20);
            this.textBox6.TabIndex = 29;
            this.textBox6.Tag = "MatterSphereUpdateIgnore";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Delete Timer (ms)";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(133, 44);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(183, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.Tag = "DeleteServiceTimer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Add. Get Last Run Time";
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(133, 251);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(183, 20);
            this.textBox4.TabIndex = 27;
            this.textBox4.Tag = "AdditionalMinutes";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Service Timer (ms)";
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(133, 19);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(183, 20);
            this.textBox5.TabIndex = 9;
            this.textBox5.Tag = "ServiceTimerLength";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "LogToFileEnabled";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(133, 75);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Tag = "LogToFileEnabled";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(133, 97);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Tag = "LogAllToEventLog";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 228);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(113, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Do Not Process End 2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 97);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "LogAllToEventLog";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker1.Location = new System.Drawing.Point(133, 225);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(183, 20);
            this.dateTimePicker1.TabIndex = 25;
            this.dateTimePicker1.Tag = "DoNotProcessEnd2";
            this.dateTimePicker1.Value = new System.DateTime(2014, 7, 15, 0, 0, 0, 0);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 200);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(116, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Do Not Process Start 2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 119);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(115, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "DoNotProcessEnabled";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker2.Location = new System.Drawing.Point(133, 197);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(183, 20);
            this.dateTimePicker2.TabIndex = 23;
            this.dateTimePicker2.Tag = "DoNotProcessStart2";
            this.dateTimePicker2.Value = new System.DateTime(2014, 7, 15, 0, 0, 0, 0);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(133, 119);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(15, 14);
            this.checkBox3.TabIndex = 17;
            this.checkBox3.Tag = "DoNotProcessEnabled";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(7, 172);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(113, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "Do Not Process End 1";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Enabled = false;
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker3.Location = new System.Drawing.Point(133, 141);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(183, 20);
            this.dateTimePicker3.TabIndex = 19;
            this.dateTimePicker3.Tag = "DoNotProcessStart1";
            this.dateTimePicker3.Value = new System.DateTime(2014, 7, 15, 0, 0, 0, 0);
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.Enabled = false;
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker4.Location = new System.Drawing.Point(133, 169);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.Size = new System.Drawing.Size(183, 20);
            this.dateTimePicker4.TabIndex = 21;
            this.dateTimePicker4.Tag = "DoNotProcessEnd1";
            this.dateTimePicker4.Value = new System.DateTime(2014, 7, 15, 0, 0, 0, 0);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 144);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 13);
            this.label17.TabIndex = 18;
            this.label17.Text = "Do Not Process Start 1";
            // 
            // btnRunDeleteProcess
            // 
            this.btnRunDeleteProcess.Location = new System.Drawing.Point(10, 175);
            this.btnRunDeleteProcess.Name = "btnRunDeleteProcess";
            this.btnRunDeleteProcess.Size = new System.Drawing.Size(306, 30);
            this.btnRunDeleteProcess.TabIndex = 2;
            this.btnRunDeleteProcess.Text = "Run Delete Process Manually";
            this.btnRunDeleteProcess.UseVisualStyleBackColor = true;
            this.btnRunDeleteProcess.Click += new System.EventHandler(this.btnRunDeleteProcess_Click);
            // 
            // btnCheckCertNumber
            // 
            this.btnCheckCertNumber.Location = new System.Drawing.Point(10, 218);
            this.btnCheckCertNumber.Name = "btnCheckCertNumber";
            this.btnCheckCertNumber.Size = new System.Drawing.Size(306, 30);
            this.btnCheckCertNumber.TabIndex = 3;
            this.btnCheckCertNumber.Text = "Check Certificate Serial No";
            this.btnCheckCertNumber.UseVisualStyleBackColor = true;
            this.btnCheckCertNumber.Click += new System.EventHandler(this.btnCheckCertNumber_Click);
            // 
            // gbActions
            // 
            this.gbActions.Controls.Add(this.btnLoadConfig);
            this.gbActions.Controls.Add(this.btnEditConfig);
            this.gbActions.Controls.Add(this.btnSaveConfig);
            this.gbActions.Controls.Add(this.btnRunProcess);
            this.gbActions.Controls.Add(this.btnRunDeleteProcess);
            this.gbActions.Controls.Add(this.btnCheckCertNumber);
            this.gbActions.Location = new System.Drawing.Point(348, 326);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new System.Drawing.Size(330, 260);
            this.gbActions.TabIndex = 0;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            // 
            // MSEWSFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 598);
            this.Controls.Add(this.gbActions);
            this.Controls.Add(this.gbService);
            this.Controls.Add(this.gbTimeZone);
            this.Controls.Add(this.gbMatterSphere);
            this.Controls.Add(this.gbExchange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MSEWSFrm";
            this.Text = "3E MatterSphere EWS Sync Config and Manual Run";
            this.gbMatterSphere.ResumeLayout(false);
            this.gbMatterSphere.PerformLayout();
            this.gbTimeZone.ResumeLayout(false);
            this.gbTimeZone.PerformLayout();
            this.gbExchange.ResumeLayout(false);
            this.gbExchange.PerformLayout();
            this.gbService.ResumeLayout(false);
            this.gbService.PerformLayout();
            this.gbActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRunProcess;
        private System.Windows.Forms.TextBox txtMSServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnEditConfig;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMSDB;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTimeZoneTbl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTimeZoneCol;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtBranchCode;
        private System.Windows.Forms.ComboBox cmbTimeZone;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.GroupBox gbMatterSphere;
        private System.Windows.Forms.GroupBox gbTimeZone;
        private System.Windows.Forms.GroupBox gbExchange;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox txtServiceEmail;
        private System.Windows.Forms.TextBox txtWebServices;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.CheckBox chkUseAutoDisc;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.TextBox txtCertSer;
        private System.Windows.Forms.CheckBox chkCert;
        private System.Windows.Forms.TextBox txtExcPassword;
        private System.Windows.Forms.TextBox txtExcUser;
        private System.Windows.Forms.Label lblExcDomain;
        private System.Windows.Forms.Label lblExcUser;
        private System.Windows.Forms.TextBox txtExcDomain;
        private System.Windows.Forms.Label lblExcPassword;
        private System.Windows.Forms.ComboBox cmbFreeBusy;
        private System.Windows.Forms.GroupBox gbService;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnRunDeleteProcess;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button btnCheckCertNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSetAppointmentTypeAsCategory;
        private System.Windows.Forms.GroupBox gbActions;
        private System.Windows.Forms.RadioButton rbOAuth;
        private System.Windows.Forms.RadioButton rbNtlm;
        private System.Windows.Forms.RadioButton rbBasic;
        private System.Windows.Forms.TextBox txtOAuthTenant;
        private System.Windows.Forms.TextBox txtOAuthClientSecret;
        private System.Windows.Forms.TextBox txtOAuthApp;
        private System.Windows.Forms.Label lblOAuthTenant;
        private System.Windows.Forms.Label lblOAuthClientSecret;
        private System.Windows.Forms.Label lblOAuthApp;
        private System.Windows.Forms.ComboBox cmbLoginType;
        private System.Windows.Forms.Label label4;
    }
}

