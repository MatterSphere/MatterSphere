namespace FWBS.OMS.UI.Windows
{
    partial class frmSaveWithVersionData
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
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.LinkedItemList = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.spacer4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.spacer3 = new System.Windows.Forms.Panel();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.spacer2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.spacer1 = new System.Windows.Forms.Panel();
            this.infoDescription = new FWBS.Common.UI.Windows.eInformation2();
            this.pnlButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(8, 376);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(568, 27);
            this.pnlButtons.TabIndex = 25;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(493, 0);
            this.resourceLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 24;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(412, 0);
            this.resourceLookup.SetLookup(this.btnSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSave", "&Save", ""));
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.LinkedItemList);
            this.pnlMain.Controls.Add(this.spacer4);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.spacer3);
            this.pnlMain.Controls.Add(this.txtComments);
            this.pnlMain.Controls.Add(this.spacer2);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.spacer1);
            this.pnlMain.Controls.Add(this.infoDescription);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(8, 8);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(2);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(568, 368);
            this.pnlMain.TabIndex = 6;
            // 
            // LinkedItemList
            // 
            this.LinkedItemList.CaptionWidth = 0;
            this.LinkedItemList.Dock = System.Windows.Forms.DockStyle.Top;
            this.LinkedItemList.Enabled = false;
            this.LinkedItemList.IsDirty = false;
            this.LinkedItemList.Location = new System.Drawing.Point(0, 245);
            this.LinkedItemList.Margin = new System.Windows.Forms.Padding(2);
            this.LinkedItemList.MaxLength = 0;
            this.LinkedItemList.Name = "LinkedItemList";
            this.LinkedItemList.ReadOnly = true;
            this.LinkedItemList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LinkedItemList.Size = new System.Drawing.Size(568, 112);
            this.LinkedItemList.TabIndex = 0;
            this.LinkedItemList.Text = " ";
            // 
            // spacer4
            // 
            this.spacer4.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacer4.Location = new System.Drawing.Point(0, 239);
            this.spacer4.Margin = new System.Windows.Forms.Padding(2);
            this.spacer4.Name = "spacer4";
            this.spacer4.Size = new System.Drawing.Size(568, 6);
            this.spacer4.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(0, 224);
            this.resourceLookup.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("labLnkObjChckIn", "Linked Objects Also Checked-in", ""));
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Linked Objects Also Checked-in";
            // 
            // spacer3
            // 
            this.spacer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacer3.Location = new System.Drawing.Point(0, 218);
            this.spacer3.Margin = new System.Windows.Forms.Padding(2);
            this.spacer3.Name = "spacer3";
            this.spacer3.Size = new System.Drawing.Size(568, 6);
            this.spacer3.TabIndex = 19;
            // 
            // txtComments
            // 
            this.txtComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtComments.Location = new System.Drawing.Point(0, 78);
            this.txtComments.Margin = new System.Windows.Forms.Padding(2);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(568, 140);
            this.txtComments.TabIndex = 18;
            // 
            // spacer2
            // 
            this.spacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacer2.Location = new System.Drawing.Point(0, 72);
            this.spacer2.Margin = new System.Windows.Forms.Padding(2);
            this.spacer2.Name = "spacer2";
            this.spacer2.Size = new System.Drawing.Size(568, 6);
            this.spacer2.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 57);
            this.resourceLookup.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("labChangesMade", "Changes Made", ""));
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "Changes Made";
            // 
            // spacer1
            // 
            this.spacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.spacer1.Location = new System.Drawing.Point(0, 48);
            this.spacer1.Margin = new System.Windows.Forms.Padding(2);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(568, 9);
            this.spacer1.TabIndex = 15;
            // 
            // infoDescription
            // 
            this.infoDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.infoDescription.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(222)))), ((int)(((byte)(214)))));
            this.infoDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.infoDescription.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup.SetLookup(this.infoDescription, new FWBS.OMS.UI.Windows.ResourceLookupItem("labCheckInDescr", "This will save the current version of the object being viewed and the current ver" +
            "sions of objects linked to it.", ""));
            this.infoDescription.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.infoDescription.Name = "infoDescription";
            this.infoDescription.Padding = new System.Windows.Forms.Padding(1);
            this.infoDescription.Size = new System.Drawing.Size(568, 48);
            this.infoDescription.TabIndex = 11;
            this.infoDescription.Text = "This will save the current version of the object being viewed and the current ver" +
    "sions of objects linked to it.";
            // 
            // frmSaveWithVersionData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("CheckInCurVer", "Check-in Current Version", ""));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmSaveWithVersionData";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Text = "Check-in Current Version";
            this.pnlButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Panel pnlMain;
        private Common.UI.Windows.eInformation2 infoDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private Common.UI.Windows.eMultiTextBox2 LinkedItemList;
        private System.Windows.Forms.Panel spacer4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel spacer3;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.Panel spacer2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel spacer1;
    }
}