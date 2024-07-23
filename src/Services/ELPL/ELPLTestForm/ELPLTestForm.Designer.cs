namespace ELPLTestForm
{
    partial class ELPLTestForm
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
            this.txtPostCode = new System.Windows.Forms.TextBox();
            this.lblPostCode = new System.Windows.Forms.Label();
            this.cmdCheckPostcode = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblAsUser = new System.Windows.Forms.Label();
            this.txtAsUser = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.grpBxLogin = new System.Windows.Forms.GroupBox();
            this.grpBxAcknowledgeRejected = new System.Windows.Forms.GroupBox();
            this.btnSendAcknowledgement = new System.Windows.Forms.Button();
            this.txtActivityEngineGuid = new System.Windows.Forms.TextBox();
            this.lblActivityEngineGuid = new System.Windows.Forms.Label();
            this.txtClaimID = new System.Windows.Forms.TextBox();
            this.lblClaimID = new System.Windows.Forms.Label();
            this.btnGetRejectedReason = new System.Windows.Forms.Button();
            this.grpBxLogin.SuspendLayout();
            this.grpBxAcknowledgeRejected.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPostCode
            // 
            this.txtPostCode.Location = new System.Drawing.Point(74, 12);
            this.txtPostCode.Name = "txtPostCode";
            this.txtPostCode.Size = new System.Drawing.Size(112, 20);
            this.txtPostCode.TabIndex = 0;
            // 
            // lblPostCode
            // 
            this.lblPostCode.AutoSize = true;
            this.lblPostCode.Location = new System.Drawing.Point(12, 19);
            this.lblPostCode.Name = "lblPostCode";
            this.lblPostCode.Size = new System.Drawing.Size(56, 13);
            this.lblPostCode.TabIndex = 1;
            this.lblPostCode.Text = "Post Code";
            // 
            // cmdCheckPostcode
            // 
            this.cmdCheckPostcode.Location = new System.Drawing.Point(12, 38);
            this.cmdCheckPostcode.Name = "cmdCheckPostcode";
            this.cmdCheckPostcode.Size = new System.Drawing.Size(174, 20);
            this.cmdCheckPostcode.TabIndex = 2;
            this.cmdCheckPostcode.Text = "Check Post Code";
            this.cmdCheckPostcode.UseVisualStyleBackColor = true;
            this.cmdCheckPostcode.Click += new System.EventHandler(this.cmdCheckPostcode_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(82, 60);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 20);
            this.txtUserName.TabIndex = 3;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(16, 64);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 13);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "User Name";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(16, 94);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(82, 90);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // lblAsUser
            // 
            this.lblAsUser.AutoSize = true;
            this.lblAsUser.Location = new System.Drawing.Point(16, 124);
            this.lblAsUser.Name = "lblAsUser";
            this.lblAsUser.Size = new System.Drawing.Size(44, 13);
            this.lblAsUser.TabIndex = 8;
            this.lblAsUser.Text = "As User";
            // 
            // txtAsUser
            // 
            this.txtAsUser.Location = new System.Drawing.Point(82, 120);
            this.txtAsUser.Name = "txtAsUser";
            this.txtAsUser.Size = new System.Drawing.Size(100, 20);
            this.txtAsUser.TabIndex = 7;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(16, 34);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 10;
            this.lblURL.Text = "URL";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(82, 30);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(250, 20);
            this.txtURL.TabIndex = 9;
            // 
            // grpBxLogin
            // 
            this.grpBxLogin.Controls.Add(this.txtAsUser);
            this.grpBxLogin.Controls.Add(this.lblURL);
            this.grpBxLogin.Controls.Add(this.txtUserName);
            this.grpBxLogin.Controls.Add(this.txtURL);
            this.grpBxLogin.Controls.Add(this.lblUserName);
            this.grpBxLogin.Controls.Add(this.lblAsUser);
            this.grpBxLogin.Controls.Add(this.txtPassword);
            this.grpBxLogin.Controls.Add(this.lblPassword);
            this.grpBxLogin.Location = new System.Drawing.Point(15, 244);
            this.grpBxLogin.Name = "grpBxLogin";
            this.grpBxLogin.Size = new System.Drawing.Size(354, 158);
            this.grpBxLogin.TabIndex = 11;
            this.grpBxLogin.TabStop = false;
            this.grpBxLogin.Text = "Login Details";
            // 
            // grpBxAcknowledgeRejected
            // 
            this.grpBxAcknowledgeRejected.Controls.Add(this.btnSendAcknowledgement);
            this.grpBxAcknowledgeRejected.Controls.Add(this.txtActivityEngineGuid);
            this.grpBxAcknowledgeRejected.Controls.Add(this.lblActivityEngineGuid);
            this.grpBxAcknowledgeRejected.Controls.Add(this.txtClaimID);
            this.grpBxAcknowledgeRejected.Controls.Add(this.lblClaimID);
            this.grpBxAcknowledgeRejected.Location = new System.Drawing.Point(385, 244);
            this.grpBxAcknowledgeRejected.Name = "grpBxAcknowledgeRejected";
            this.grpBxAcknowledgeRejected.Size = new System.Drawing.Size(293, 158);
            this.grpBxAcknowledgeRejected.TabIndex = 12;
            this.grpBxAcknowledgeRejected.TabStop = false;
            this.grpBxAcknowledgeRejected.Text = "Acknowledge Rejected Claim";
            // 
            // btnSendAcknowledgement
            // 
            this.btnSendAcknowledgement.Location = new System.Drawing.Point(128, 90);
            this.btnSendAcknowledgement.Name = "btnSendAcknowledgement";
            this.btnSendAcknowledgement.Size = new System.Drawing.Size(138, 32);
            this.btnSendAcknowledgement.TabIndex = 13;
            this.btnSendAcknowledgement.Text = "Send Acknowledgement";
            this.btnSendAcknowledgement.UseVisualStyleBackColor = true;
            this.btnSendAcknowledgement.Click += new System.EventHandler(this.btnSendAcknowledgement_Click);
            // 
            // txtActivityEngineGuid
            // 
            this.txtActivityEngineGuid.Location = new System.Drawing.Point(128, 57);
            this.txtActivityEngineGuid.Name = "txtActivityEngineGuid";
            this.txtActivityEngineGuid.Size = new System.Drawing.Size(138, 20);
            this.txtActivityEngineGuid.TabIndex = 11;
            // 
            // lblActivityEngineGuid
            // 
            this.lblActivityEngineGuid.AutoSize = true;
            this.lblActivityEngineGuid.Location = new System.Drawing.Point(22, 61);
            this.lblActivityEngineGuid.Name = "lblActivityEngineGuid";
            this.lblActivityEngineGuid.Size = new System.Drawing.Size(102, 13);
            this.lblActivityEngineGuid.TabIndex = 12;
            this.lblActivityEngineGuid.Text = "Activity Engine Guid";
            // 
            // txtClaimID
            // 
            this.txtClaimID.Location = new System.Drawing.Point(128, 28);
            this.txtClaimID.Name = "txtClaimID";
            this.txtClaimID.Size = new System.Drawing.Size(138, 20);
            this.txtClaimID.TabIndex = 9;
            // 
            // lblClaimID
            // 
            this.lblClaimID.AutoSize = true;
            this.lblClaimID.Location = new System.Drawing.Point(22, 32);
            this.lblClaimID.Name = "lblClaimID";
            this.lblClaimID.Size = new System.Drawing.Size(46, 13);
            this.lblClaimID.TabIndex = 10;
            this.lblClaimID.Text = "Claim ID";
            // 
            // btnGetRejectedReason
            // 
            this.btnGetRejectedReason.Location = new System.Drawing.Point(513, 135);
            this.btnGetRejectedReason.Name = "btnGetRejectedReason";
            this.btnGetRejectedReason.Size = new System.Drawing.Size(138, 44);
            this.btnGetRejectedReason.TabIndex = 13;
            this.btnGetRejectedReason.Text = "Get Rejected Reason";
            this.btnGetRejectedReason.UseVisualStyleBackColor = true;
            this.btnGetRejectedReason.Click += new System.EventHandler(this.btnGetRejectedReason_Click);
            // 
            // ELPLTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 423);
            this.Controls.Add(this.btnGetRejectedReason);
            this.Controls.Add(this.grpBxAcknowledgeRejected);
            this.Controls.Add(this.grpBxLogin);
            this.Controls.Add(this.cmdCheckPostcode);
            this.Controls.Add(this.lblPostCode);
            this.Controls.Add(this.txtPostCode);
            this.Name = "ELPLTestForm";
            this.Text = "ELPL Test Form";
            this.grpBxLogin.ResumeLayout(false);
            this.grpBxLogin.PerformLayout();
            this.grpBxAcknowledgeRejected.ResumeLayout(false);
            this.grpBxAcknowledgeRejected.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPostCode;
        private System.Windows.Forms.Label lblPostCode;
        private System.Windows.Forms.Button cmdCheckPostcode;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblAsUser;
        private System.Windows.Forms.TextBox txtAsUser;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.GroupBox grpBxLogin;
        private System.Windows.Forms.GroupBox grpBxAcknowledgeRejected;
        private System.Windows.Forms.Button btnSendAcknowledgement;
        private System.Windows.Forms.TextBox txtActivityEngineGuid;
        private System.Windows.Forms.Label lblActivityEngineGuid;
        private System.Windows.Forms.TextBox txtClaimID;
        private System.Windows.Forms.Label lblClaimID;
        private System.Windows.Forms.Button btnGetRejectedReason;
    }
}

