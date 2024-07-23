namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    partial class LockableFetchSettingsEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLockType = new System.Windows.Forms.Panel();
            this.chkCheckOut = new System.Windows.Forms.CheckBox();
            this.pnlAlreadyLocked = new System.Windows.Forms.Panel();
            this.lblCheckedOutBy = new System.Windows.Forms.Label();
            this.pnlLockType.SuspendLayout();
            this.pnlAlreadyLocked.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.Resources.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblSpecwhlock", "Please specify whether you would like to lock the document by checking it out.", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please specify whether you would like to lock the document by checking it out.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 11);
            this.Resources.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblcheckout", "Currently Checked Out By", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Currently Checked Out By";
            // 
            // pnlLockType
            // 
            this.pnlLockType.Controls.Add(this.chkCheckOut);
            this.pnlLockType.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLockType.Location = new System.Drawing.Point(10, 90);
            this.pnlLockType.Name = "pnlLockType";
            this.pnlLockType.Size = new System.Drawing.Size(403, 50);
            this.pnlLockType.TabIndex = 8;
            // 
            // chkCheckOut
            // 
            this.chkCheckOut.AutoSize = true;
            this.chkCheckOut.Checked = true;
            this.chkCheckOut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckOut.Location = new System.Drawing.Point(18, 18);
            this.Resources.SetLookup(this.chkCheckOut, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkCheckOut", "Check Out", ""));
            this.chkCheckOut.Name = "chkCheckOut";
            this.chkCheckOut.Size = new System.Drawing.Size(77, 17);
            this.chkCheckOut.TabIndex = 0;
            this.chkCheckOut.Text = "Check Out";
            this.chkCheckOut.UseVisualStyleBackColor = true;
            // 
            // pnlAlreadyLocked
            // 
            this.pnlAlreadyLocked.Controls.Add(this.lblCheckedOutBy);
            this.pnlAlreadyLocked.Controls.Add(this.label2);
            this.pnlAlreadyLocked.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlreadyLocked.Location = new System.Drawing.Point(10, 30);
            this.pnlAlreadyLocked.Name = "pnlAlreadyLocked";
            this.pnlAlreadyLocked.Size = new System.Drawing.Size(403, 60);
            this.pnlAlreadyLocked.TabIndex = 9;
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
            // LockableFetchSettingsEditor
            // 
            this.Controls.Add(this.pnlLockType);
            this.Controls.Add(this.pnlAlreadyLocked);
            this.Controls.Add(this.label1);
            this.Name = "LockableFetchSettingsEditor";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(423, 154);
            this.pnlLockType.ResumeLayout(false);
            this.pnlLockType.PerformLayout();
            this.pnlAlreadyLocked.ResumeLayout(false);
            this.pnlAlreadyLocked.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlLockType;
        private System.Windows.Forms.Panel pnlAlreadyLocked;
        private System.Windows.Forms.Label lblCheckedOutBy;
        private System.Windows.Forms.CheckBox chkCheckOut;



    }
}
