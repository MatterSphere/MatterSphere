namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    partial class LockableStoreSettingsEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlLockType = new System.Windows.Forms.Panel();
            this.chkCheckIn = new System.Windows.Forms.CheckBox();
            this.pnlAlreadyLocked = new System.Windows.Forms.Panel();
            this.lblCheckedOutBy = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLockType.SuspendLayout();
            this.pnlAlreadyLocked.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLockType
            // 
            this.pnlLockType.Controls.Add(this.chkCheckIn);
            this.pnlLockType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockType.Location = new System.Drawing.Point(10, 101);
            this.pnlLockType.Name = "pnlLockType";
            this.pnlLockType.Size = new System.Drawing.Size(347, 42);
            this.pnlLockType.TabIndex = 11;
            // 
            // chkCheckIn
            // 
            this.chkCheckIn.AutoSize = true;
            this.chkCheckIn.Checked = true;
            this.chkCheckIn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckIn.Location = new System.Drawing.Point(18, 15);
            this.Resources.SetLookup(this.chkCheckIn, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkCheckIn", "Check In", ""));
            this.chkCheckIn.Name = "chkCheckIn";
            this.chkCheckIn.Size = new System.Drawing.Size(69, 17);
            this.chkCheckIn.TabIndex = 0;
            this.chkCheckIn.Text = "Check In";
            this.chkCheckIn.UseVisualStyleBackColor = true;
            // 
            // pnlAlreadyLocked
            // 
            this.pnlAlreadyLocked.Controls.Add(this.lblCheckedOutBy);
            this.pnlAlreadyLocked.Controls.Add(this.label2);
            this.pnlAlreadyLocked.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlreadyLocked.Location = new System.Drawing.Point(10, 41);
            this.pnlAlreadyLocked.Name = "pnlAlreadyLocked";
            this.pnlAlreadyLocked.Size = new System.Drawing.Size(347, 60);
            this.pnlAlreadyLocked.TabIndex = 12;
            this.pnlAlreadyLocked.Visible = false;
            // 
            // lblCheckedOutBy
            // 
            this.lblCheckedOutBy.AutoSize = true;
            this.lblCheckedOutBy.Location = new System.Drawing.Point(37, 28);
            this.Resources.SetLookup(this.lblCheckedOutBy, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblCheckedOutBy", "{0} at {1}", ""));
            this.lblCheckedOutBy.Name = "lblCheckedOutBy";
            this.lblCheckedOutBy.Size = new System.Drawing.Size(50, 13);
            this.lblCheckedOutBy.TabIndex = 8;
            this.lblCheckedOutBy.Text = "{0} at {1}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 11);
            this.Resources.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblcheckoutby", "Currently Checked Out By", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Currently Checked Out By";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.Resources.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblchkcheckin", "Please tick check in to unlock the file for others to edit, otherwise leave untic" +
                                                                                                              "ked to keep checked out.", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 31);
            this.label1.TabIndex = 10;
            this.label1.Text = "Please tick check in to unlock the file for others to edit, otherwise leave untic" +
    "ked to keep checked out.";
            // 
            // LockableStoreSettingsEditor
            // 
            this.Controls.Add(this.pnlLockType);
            this.Controls.Add(this.pnlAlreadyLocked);
            this.Controls.Add(this.label1);
            this.Name = "LockableStoreSettingsEditor";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(367, 150);
            this.pnlLockType.ResumeLayout(false);
            this.pnlLockType.PerformLayout();
            this.pnlAlreadyLocked.ResumeLayout(false);
            this.pnlAlreadyLocked.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLockType;
        private System.Windows.Forms.Panel pnlAlreadyLocked;
        private System.Windows.Forms.Label lblCheckedOutBy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkCheckIn;



    }
}
