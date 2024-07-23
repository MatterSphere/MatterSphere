namespace FWBS.OMS.FileManagement.UI
{
    partial class Assignment
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
            this.components = new System.ComponentModel.Container();
            this.btnAssign = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.cboUser = new FWBS.Common.UI.Windows.eXPComboBox();
            this.cboTeam = new FWBS.Common.UI.Windows.eXPComboBox();
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.pboxIcon = new System.Windows.Forms.PictureBox();
            this.pnlBackground.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAssign
            // 
            this.btnAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnAssign.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAssign.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnAssign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAssign.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnAssign.ForeColor = System.Drawing.Color.White;
            this.btnAssign.Location = new System.Drawing.Point(302, 12);
            this.res.SetLookup(this.btnAssign, new FWBS.OMS.UI.Windows.ResourceLookupItem("Assign", "&Assign", ""));
            this.btnAssign.Name = "btnAssign";
            this.btnAssign.Size = new System.Drawing.Size(100, 32);
            this.btnAssign.TabIndex = 3;
            this.btnAssign.Text = "&Assign";
            this.btnAssign.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnCancel.Location = new System.Drawing.Point(193, 12);
            this.res.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("CANCELBTN", "&Cancel", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblHeaderText.ForeColor = System.Drawing.Color.White;
            this.lblHeaderText.Location = new System.Drawing.Point(46, 10);
            this.res.SetLookup(this.lblHeaderText, new FWBS.OMS.UI.Windows.ResourceLookupItem("TASKASSIGNMENT", "Task Assignment", ""));
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(113, 19);
            this.lblHeaderText.TabIndex = 7;
            this.lblHeaderText.Text = "Task Assignment";
            this.lblHeaderText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeaderText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // cboUser
            // 
            this.cboUser.ActiveSearchEnabled = true;
            this.cboUser.CaptionTop = true;
            this.cboUser.CaptionWidth = 0;
            this.cboUser.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cboUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cboUser.IsDirty = false;
            this.cboUser.Location = new System.Drawing.Point(44, 116);
            this.res.SetLookup(this.cboUser, new FWBS.OMS.UI.Windows.ResourceLookupItem("ASSIGNTO", "Assign To", ""));
            this.cboUser.MaxLength = 0;
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(327, 51);
            this.cboUser.TabIndex = 1;
            // 
            // cboTeam
            // 
            this.cboTeam.ActiveSearchEnabled = true;
            this.cboTeam.CaptionTop = true;
            this.cboTeam.CaptionWidth = 0;
            this.cboTeam.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.cboTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cboTeam.IsDirty = false;
            this.cboTeam.Location = new System.Drawing.Point(44, 57);
            this.res.SetLookup(this.cboTeam, new FWBS.OMS.UI.Windows.ResourceLookupItem("ASSIGNEDTEAM", "Assigned Team", ""));
            this.cboTeam.MaxLength = 0;
            this.cboTeam.Name = "cboTeam";
            this.cboTeam.Size = new System.Drawing.Size(327, 51);
            this.cboTeam.TabIndex = 0;
            this.cboTeam.Changed += new System.EventHandler(this.cboTeam_SelectedIndexChanged);
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackColor = System.Drawing.Color.White;
            this.pnlBackground.Controls.Add(this.cboTeam);
            this.pnlBackground.Controls.Add(this.cboUser);
            this.pnlBackground.Controls.Add(this.pnlButtons);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackground.Location = new System.Drawing.Point(1, 1);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(413, 242);
            this.pnlBackground.TabIndex = 10000;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnAssign);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 186);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.pnlButtons.Size = new System.Drawing.Size(413, 56);
            this.pnlButtons.TabIndex = 6;
            this.pnlButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlButtons_Paint);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblHeaderText);
            this.pnlHeader.Controls.Add(this.pboxIcon);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(1, 1);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(413, 40);
            this.pnlHeader.TabIndex = 6;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Webdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(373, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "r";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pboxIcon
            // 
            this.pboxIcon.Location = new System.Drawing.Point(5, 0);
            this.pboxIcon.Name = "pboxIcon";
            this.pboxIcon.Size = new System.Drawing.Size(40, 40);
            this.pboxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pboxIcon.TabIndex = 0;
            this.pboxIcon.TabStop = false;
            this.pboxIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form_MouseDown);
            // 
            // Assignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(415, 244);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlBackground);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.res.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("TaskAssignement", "Task Assignment", ""));
            this.Name = "Assignment";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Task Assignment";
            this.Load += new System.EventHandler(this.Assignment_Load);
            this.pnlBackground.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAssign;
        private System.Windows.Forms.Button btnCancel;
        private FWBS.OMS.UI.Windows.ResourceLookup res;
        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.PictureBox pboxIcon;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        private FWBS.Common.UI.Windows.eXPComboBox cboTeam;
        private Common.UI.Windows.eXPComboBox cboUser;
    }
}