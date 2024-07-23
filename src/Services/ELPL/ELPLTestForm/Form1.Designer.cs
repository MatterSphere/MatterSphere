namespace ELPLTestForm
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tpLoginDetails = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetRTAServices1 = new System.Windows.Forms.Button();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtAsUser = new System.Windows.Forms.TextBox();
            this.lblAsUser = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tpNotifications = new System.Windows.Forms.TabPage();
            this.lblRowCountValue = new System.Windows.Forms.Label();
            this.lblRowCount = new System.Windows.Forms.Label();
            this.dgNotifications = new System.Windows.Forms.DataGridView();
            this.btnGetNotifications = new System.Windows.Forms.Button();
            this.tpGetClaim = new System.Windows.Forms.TabPage();
            this.txtClaimXML = new System.Windows.Forms.TextBox();
            this.lblClaimNumber = new System.Windows.Forms.Label();
            this.txtClaimID = new System.Windows.Forms.TextBox();
            this.btnGetClaim = new System.Windows.Forms.Button();
            this.tpTransferredClaims = new System.Windows.Forms.TabPage();
            this.btnGetTransferredNotifications = new System.Windows.Forms.Button();
            this.txtTransferredClaimData = new System.Windows.Forms.TextBox();
            this.btnGetTransferredClaimData = new System.Windows.Forms.Button();
            this.tpSearchCompensators = new System.Windows.Forms.TabPage();
            this.dgCompensators = new System.Windows.Forms.DataGridView();
            this.txtCompensator = new System.Windows.Forms.TextBox();
            this.btnSearchCompensators = new System.Windows.Forms.Button();
            this.lblCompensator = new System.Windows.Forms.Label();
            this.tpGetClaims = new System.Windows.Forms.TabPage();
            this.dgClaims = new System.Windows.Forms.DataGridView();
            this.btnGetClaims = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.tpLoginDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpNotifications.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNotifications)).BeginInit();
            this.tpGetClaim.SuspendLayout();
            this.tpTransferredClaims.SuspendLayout();
            this.tpSearchCompensators.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCompensators)).BeginInit();
            this.tpGetClaims.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgClaims)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tpLoginDetails);
            this.tabs.Controls.Add(this.tpNotifications);
            this.tabs.Controls.Add(this.tpGetClaim);
            this.tabs.Controls.Add(this.tpTransferredClaims);
            this.tabs.Controls.Add(this.tpSearchCompensators);
            this.tabs.Controls.Add(this.tpGetClaims);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1184, 710);
            this.tabs.TabIndex = 3;
            // 
            // tpLoginDetails
            // 
            this.tpLoginDetails.Controls.Add(this.groupBox1);
            this.tpLoginDetails.Location = new System.Drawing.Point(4, 22);
            this.tpLoginDetails.Name = "tpLoginDetails";
            this.tpLoginDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpLoginDetails.Size = new System.Drawing.Size(1176, 684);
            this.tpLoginDetails.TabIndex = 0;
            this.tpLoginDetails.Text = "Login Details";
            this.tpLoginDetails.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSetRTAServices1);
            this.groupBox1.Controls.Add(this.txtURL);
            this.groupBox1.Controls.Add(this.lblURL);
            this.groupBox1.Controls.Add(this.txtAsUser);
            this.groupBox1.Controls.Add(this.lblAsUser);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.lblUsername);
            this.groupBox1.Location = new System.Drawing.Point(8, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 201);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login Details";
            // 
            // btnSetRTAServices1
            // 
            this.btnSetRTAServices1.Location = new System.Drawing.Point(96, 139);
            this.btnSetRTAServices1.Name = "btnSetRTAServices1";
            this.btnSetRTAServices1.Size = new System.Drawing.Size(100, 51);
            this.btnSetRTAServices1.TabIndex = 8;
            this.btnSetRTAServices1.Text = "Set ELPLServices1 Object";
            this.btnSetRTAServices1.UseVisualStyleBackColor = true;
            this.btnSetRTAServices1.Click += new System.EventHandler(this.btnSetRTAServices1_Click);
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(96, 20);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(300, 20);
            this.txtURL.TabIndex = 7;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(18, 24);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 6;
            this.lblURL.Text = "URL";
            // 
            // txtAsUser
            // 
            this.txtAsUser.Location = new System.Drawing.Point(96, 110);
            this.txtAsUser.Name = "txtAsUser";
            this.txtAsUser.Size = new System.Drawing.Size(100, 20);
            this.txtAsUser.TabIndex = 5;
            // 
            // lblAsUser
            // 
            this.lblAsUser.AutoSize = true;
            this.lblAsUser.Location = new System.Drawing.Point(18, 114);
            this.lblAsUser.Name = "lblAsUser";
            this.lblAsUser.Size = new System.Drawing.Size(44, 13);
            this.lblAsUser.TabIndex = 4;
            this.lblAsUser.Text = "As User";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(96, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(18, 84);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(96, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(18, 54);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username";
            // 
            // tpNotifications
            // 
            this.tpNotifications.Controls.Add(this.lblRowCountValue);
            this.tpNotifications.Controls.Add(this.lblRowCount);
            this.tpNotifications.Controls.Add(this.dgNotifications);
            this.tpNotifications.Controls.Add(this.btnGetNotifications);
            this.tpNotifications.Location = new System.Drawing.Point(4, 22);
            this.tpNotifications.Name = "tpNotifications";
            this.tpNotifications.Padding = new System.Windows.Forms.Padding(3);
            this.tpNotifications.Size = new System.Drawing.Size(1176, 684);
            this.tpNotifications.TabIndex = 1;
            this.tpNotifications.Text = "Notifications";
            this.tpNotifications.UseVisualStyleBackColor = true;
            // 
            // lblRowCountValue
            // 
            this.lblRowCountValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowCountValue.AutoSize = true;
            this.lblRowCountValue.Location = new System.Drawing.Point(1146, 15);
            this.lblRowCountValue.Name = "lblRowCountValue";
            this.lblRowCountValue.Size = new System.Drawing.Size(0, 13);
            this.lblRowCountValue.TabIndex = 6;
            // 
            // lblRowCount
            // 
            this.lblRowCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(1079, 16);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(60, 13);
            this.lblRowCount.TabIndex = 5;
            this.lblRowCount.Text = "Row Count";
            // 
            // dgNotifications
            // 
            this.dgNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgNotifications.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgNotifications.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgNotifications.Location = new System.Drawing.Point(8, 36);
            this.dgNotifications.Name = "dgNotifications";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgNotifications.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgNotifications.Size = new System.Drawing.Size(1160, 640);
            this.dgNotifications.TabIndex = 4;
            // 
            // btnGetNotifications
            // 
            this.btnGetNotifications.Location = new System.Drawing.Point(8, 7);
            this.btnGetNotifications.Name = "btnGetNotifications";
            this.btnGetNotifications.Size = new System.Drawing.Size(117, 23);
            this.btnGetNotifications.TabIndex = 3;
            this.btnGetNotifications.Text = "Get Notifications";
            this.btnGetNotifications.UseVisualStyleBackColor = true;
            this.btnGetNotifications.Click += new System.EventHandler(this.btnGetNotifications_Click);
            // 
            // tpGetClaim
            // 
            this.tpGetClaim.Controls.Add(this.txtClaimXML);
            this.tpGetClaim.Controls.Add(this.lblClaimNumber);
            this.tpGetClaim.Controls.Add(this.txtClaimID);
            this.tpGetClaim.Controls.Add(this.btnGetClaim);
            this.tpGetClaim.Location = new System.Drawing.Point(4, 22);
            this.tpGetClaim.Name = "tpGetClaim";
            this.tpGetClaim.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetClaim.Size = new System.Drawing.Size(1176, 684);
            this.tpGetClaim.TabIndex = 2;
            this.tpGetClaim.Text = "Get Claim";
            this.tpGetClaim.UseVisualStyleBackColor = true;
            // 
            // txtClaimXML
            // 
            this.txtClaimXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClaimXML.Location = new System.Drawing.Point(11, 43);
            this.txtClaimXML.Multiline = true;
            this.txtClaimXML.Name = "txtClaimXML";
            this.txtClaimXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtClaimXML.Size = new System.Drawing.Size(1146, 633);
            this.txtClaimXML.TabIndex = 3;
            // 
            // lblClaimNumber
            // 
            this.lblClaimNumber.AutoSize = true;
            this.lblClaimNumber.Location = new System.Drawing.Point(8, 16);
            this.lblClaimNumber.Name = "lblClaimNumber";
            this.lblClaimNumber.Size = new System.Drawing.Size(74, 13);
            this.lblClaimNumber.TabIndex = 2;
            this.lblClaimNumber.Text = "Enter Claim ID";
            // 
            // txtClaimID
            // 
            this.txtClaimID.Location = new System.Drawing.Point(88, 13);
            this.txtClaimID.Name = "txtClaimID";
            this.txtClaimID.Size = new System.Drawing.Size(152, 20);
            this.txtClaimID.TabIndex = 1;
            // 
            // btnGetClaim
            // 
            this.btnGetClaim.Location = new System.Drawing.Point(246, 12);
            this.btnGetClaim.Name = "btnGetClaim";
            this.btnGetClaim.Size = new System.Drawing.Size(81, 23);
            this.btnGetClaim.TabIndex = 0;
            this.btnGetClaim.Text = "Get Claim";
            this.btnGetClaim.UseVisualStyleBackColor = true;
            this.btnGetClaim.Click += new System.EventHandler(this.btnGetClaim_Click);
            // 
            // tpTransferredClaims
            // 
            this.tpTransferredClaims.Controls.Add(this.btnGetTransferredNotifications);
            this.tpTransferredClaims.Controls.Add(this.txtTransferredClaimData);
            this.tpTransferredClaims.Controls.Add(this.btnGetTransferredClaimData);
            this.tpTransferredClaims.Location = new System.Drawing.Point(4, 22);
            this.tpTransferredClaims.Name = "tpTransferredClaims";
            this.tpTransferredClaims.Padding = new System.Windows.Forms.Padding(3);
            this.tpTransferredClaims.Size = new System.Drawing.Size(1176, 684);
            this.tpTransferredClaims.TabIndex = 3;
            this.tpTransferredClaims.Text = "Transferred Claims";
            this.tpTransferredClaims.UseVisualStyleBackColor = true;
            // 
            // btnGetTransferredNotifications
            // 
            this.btnGetTransferredNotifications.Location = new System.Drawing.Point(329, 6);
            this.btnGetTransferredNotifications.Name = "btnGetTransferredNotifications";
            this.btnGetTransferredNotifications.Size = new System.Drawing.Size(159, 41);
            this.btnGetTransferredNotifications.TabIndex = 2;
            this.btnGetTransferredNotifications.Text = "Get Transferred Claim Data from Notifications";
            this.btnGetTransferredNotifications.UseVisualStyleBackColor = true;
            this.btnGetTransferredNotifications.Click += new System.EventHandler(this.btnGetTransferredNotifications_Click);
            // 
            // txtTransferredClaimData
            // 
            this.txtTransferredClaimData.Location = new System.Drawing.Point(8, 53);
            this.txtTransferredClaimData.Multiline = true;
            this.txtTransferredClaimData.Name = "txtTransferredClaimData";
            this.txtTransferredClaimData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTransferredClaimData.Size = new System.Drawing.Size(480, 307);
            this.txtTransferredClaimData.TabIndex = 1;
            // 
            // btnGetTransferredClaimData
            // 
            this.btnGetTransferredClaimData.Location = new System.Drawing.Point(8, 6);
            this.btnGetTransferredClaimData.Name = "btnGetTransferredClaimData";
            this.btnGetTransferredClaimData.Size = new System.Drawing.Size(139, 41);
            this.btnGetTransferredClaimData.TabIndex = 0;
            this.btnGetTransferredClaimData.Text = "Get Transferred Claim Data From Claim XML";
            this.btnGetTransferredClaimData.UseVisualStyleBackColor = true;
            this.btnGetTransferredClaimData.Click += new System.EventHandler(this.btnGetTransferredClaimData_Click);
            // 
            // tpSearchCompensators
            // 
            this.tpSearchCompensators.Controls.Add(this.dgCompensators);
            this.tpSearchCompensators.Controls.Add(this.txtCompensator);
            this.tpSearchCompensators.Controls.Add(this.btnSearchCompensators);
            this.tpSearchCompensators.Controls.Add(this.lblCompensator);
            this.tpSearchCompensators.Location = new System.Drawing.Point(4, 22);
            this.tpSearchCompensators.Name = "tpSearchCompensators";
            this.tpSearchCompensators.Padding = new System.Windows.Forms.Padding(3);
            this.tpSearchCompensators.Size = new System.Drawing.Size(1176, 684);
            this.tpSearchCompensators.TabIndex = 4;
            this.tpSearchCompensators.Text = "Search Compensators";
            this.tpSearchCompensators.UseVisualStyleBackColor = true;
            // 
            // dgCompensators
            // 
            this.dgCompensators.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCompensators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCompensators.Location = new System.Drawing.Point(11, 40);
            this.dgCompensators.Name = "dgCompensators";
            this.dgCompensators.Size = new System.Drawing.Size(1148, 626);
            this.dgCompensators.TabIndex = 3;
            // 
            // txtCompensator
            // 
            this.txtCompensator.Location = new System.Drawing.Point(83, 12);
            this.txtCompensator.Name = "txtCompensator";
            this.txtCompensator.Size = new System.Drawing.Size(148, 20);
            this.txtCompensator.TabIndex = 2;
            // 
            // btnSearchCompensators
            // 
            this.btnSearchCompensators.Location = new System.Drawing.Point(237, 11);
            this.btnSearchCompensators.Name = "btnSearchCompensators";
            this.btnSearchCompensators.Size = new System.Drawing.Size(140, 23);
            this.btnSearchCompensators.TabIndex = 1;
            this.btnSearchCompensators.Text = "Search Compensators";
            this.btnSearchCompensators.UseVisualStyleBackColor = true;
            this.btnSearchCompensators.Click += new System.EventHandler(this.btnSearchCompensators_Click);
            // 
            // lblCompensator
            // 
            this.lblCompensator.AutoSize = true;
            this.lblCompensator.Location = new System.Drawing.Point(8, 15);
            this.lblCompensator.Name = "lblCompensator";
            this.lblCompensator.Size = new System.Drawing.Size(69, 13);
            this.lblCompensator.TabIndex = 0;
            this.lblCompensator.Text = "Compensator";
            // 
            // tpGetClaims
            // 
            this.tpGetClaims.Controls.Add(this.dgClaims);
            this.tpGetClaims.Controls.Add(this.btnGetClaims);
            this.tpGetClaims.Location = new System.Drawing.Point(4, 22);
            this.tpGetClaims.Name = "tpGetClaims";
            this.tpGetClaims.Padding = new System.Windows.Forms.Padding(3);
            this.tpGetClaims.Size = new System.Drawing.Size(1176, 684);
            this.tpGetClaims.TabIndex = 5;
            this.tpGetClaims.Text = "Get Claims";
            this.tpGetClaims.UseVisualStyleBackColor = true;
            // 
            // dgClaims
            // 
            this.dgClaims.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgClaims.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgClaims.Location = new System.Drawing.Point(8, 35);
            this.dgClaims.Name = "dgClaims";
            this.dgClaims.Size = new System.Drawing.Size(1160, 641);
            this.dgClaims.TabIndex = 1;
            // 
            // btnGetClaims
            // 
            this.btnGetClaims.Location = new System.Drawing.Point(8, 6);
            this.btnGetClaims.Name = "btnGetClaims";
            this.btnGetClaims.Size = new System.Drawing.Size(124, 23);
            this.btnGetClaims.TabIndex = 0;
            this.btnGetClaims.Text = "Get Claims";
            this.btnGetClaims.UseVisualStyleBackColor = true;
            this.btnGetClaims.Click += new System.EventHandler(this.btnGetClaims_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 710);
            this.Controls.Add(this.tabs);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabs.ResumeLayout(false);
            this.tpLoginDetails.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpNotifications.ResumeLayout(false);
            this.tpNotifications.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNotifications)).EndInit();
            this.tpGetClaim.ResumeLayout(false);
            this.tpGetClaim.PerformLayout();
            this.tpTransferredClaims.ResumeLayout(false);
            this.tpTransferredClaims.PerformLayout();
            this.tpSearchCompensators.ResumeLayout(false);
            this.tpSearchCompensators.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCompensators)).EndInit();
            this.tpGetClaims.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgClaims)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tpLoginDetails;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtAsUser;
        private System.Windows.Forms.Label lblAsUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TabPage tpNotifications;
        private System.Windows.Forms.DataGridView dgNotifications;
        private System.Windows.Forms.Button btnGetNotifications;
        private System.Windows.Forms.TabPage tpGetClaim;
        private System.Windows.Forms.TextBox txtClaimXML;
        private System.Windows.Forms.Label lblClaimNumber;
        private System.Windows.Forms.TextBox txtClaimID;
        private System.Windows.Forms.Button btnGetClaim;
        private System.Windows.Forms.TabPage tpTransferredClaims;
        private System.Windows.Forms.TextBox txtTransferredClaimData;
        private System.Windows.Forms.Button btnGetTransferredClaimData;
        private System.Windows.Forms.Label lblRowCountValue;
        private System.Windows.Forms.Label lblRowCount;
        private System.Windows.Forms.Button btnSetRTAServices1;
        private System.Windows.Forms.Button btnGetTransferredNotifications;
        private System.Windows.Forms.TabPage tpSearchCompensators;
        private System.Windows.Forms.DataGridView dgCompensators;
        private System.Windows.Forms.TextBox txtCompensator;
        private System.Windows.Forms.Button btnSearchCompensators;
        private System.Windows.Forms.Label lblCompensator;
        private System.Windows.Forms.TabPage tpGetClaims;
        private System.Windows.Forms.DataGridView dgClaims;
        private System.Windows.Forms.Button btnGetClaims;

    }
}

