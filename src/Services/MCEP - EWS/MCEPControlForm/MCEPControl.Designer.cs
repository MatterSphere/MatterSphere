namespace MCEPControlForm
{
    partial class MCEPControl
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
            this.btnRunProfiler = new System.Windows.Forms.Button();
            this.btnRunStorer = new System.Windows.Forms.Button();
            this.btnImportMissingUsers = new System.Windows.Forms.Button();
            this.btnEditUsers = new System.Windows.Forms.Button();
            this.btnEnableManualRun = new System.Windows.Forms.Button();
            this.txtDefaultFolderName = new System.Windows.Forms.TextBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRunProfiler
            // 
            this.btnRunProfiler.Location = new System.Drawing.Point(95, 189);
            this.btnRunProfiler.Name = "btnRunProfiler";
            this.btnRunProfiler.Size = new System.Drawing.Size(180, 28);
            this.btnRunProfiler.TabIndex = 0;
            this.btnRunProfiler.Text = "Run Profiler Process";
            this.btnRunProfiler.UseVisualStyleBackColor = true;
            this.btnRunProfiler.Visible = false;
            this.btnRunProfiler.Click += new System.EventHandler(this.btnRunProfiler_Click);
            // 
            // btnRunStorer
            // 
            this.btnRunStorer.Location = new System.Drawing.Point(95, 223);
            this.btnRunStorer.Name = "btnRunStorer";
            this.btnRunStorer.Size = new System.Drawing.Size(180, 28);
            this.btnRunStorer.TabIndex = 1;
            this.btnRunStorer.Text = "Run Storer Process";
            this.btnRunStorer.UseVisualStyleBackColor = true;
            this.btnRunStorer.Visible = false;
            this.btnRunStorer.Click += new System.EventHandler(this.btnRunStorer_Click);
            // 
            // btnImportMissingUsers
            // 
            this.btnImportMissingUsers.Location = new System.Drawing.Point(95, 12);
            this.btnImportMissingUsers.Name = "btnImportMissingUsers";
            this.btnImportMissingUsers.Size = new System.Drawing.Size(180, 28);
            this.btnImportMissingUsers.TabIndex = 2;
            this.btnImportMissingUsers.Text = "Import Missing Users";
            this.btnImportMissingUsers.UseVisualStyleBackColor = true;
            this.btnImportMissingUsers.Click += new System.EventHandler(this.btnImportMissingUsers_Click);
            // 
            // btnEditUsers
            // 
            this.btnEditUsers.Location = new System.Drawing.Point(95, 88);
            this.btnEditUsers.Name = "btnEditUsers";
            this.btnEditUsers.Size = new System.Drawing.Size(180, 28);
            this.btnEditUsers.TabIndex = 3;
            this.btnEditUsers.Text = "Edit Imported Users";
            this.btnEditUsers.UseVisualStyleBackColor = true;
            this.btnEditUsers.Click += new System.EventHandler(this.btnEditUsers_Click);
            // 
            // btnEnableManualRun
            // 
            this.btnEnableManualRun.Location = new System.Drawing.Point(95, 155);
            this.btnEnableManualRun.Name = "btnEnableManualRun";
            this.btnEnableManualRun.Size = new System.Drawing.Size(180, 28);
            this.btnEnableManualRun.TabIndex = 4;
            this.btnEnableManualRun.Text = "Enable Manual Run";
            this.btnEnableManualRun.UseVisualStyleBackColor = true;
            this.btnEnableManualRun.Click += new System.EventHandler(this.btnEnableManualRun_Click);
            // 
            // txtDefaultFolderName
            // 
            this.txtDefaultFolderName.Location = new System.Drawing.Point(95, 62);
            this.txtDefaultFolderName.Name = "txtDefaultFolderName";
            this.txtDefaultFolderName.Size = new System.Drawing.Size(180, 20);
            this.txtDefaultFolderName.TabIndex = 5;
            this.txtDefaultFolderName.Text = "OMS";
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(95, 45);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(130, 13);
            this.lbl1.TabIndex = 6;
            this.lbl1.Text = "Default Root Folder Name";
            // 
            // MCEPControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 261);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.txtDefaultFolderName);
            this.Controls.Add(this.btnEnableManualRun);
            this.Controls.Add(this.btnEditUsers);
            this.Controls.Add(this.btnImportMissingUsers);
            this.Controls.Add(this.btnRunStorer);
            this.Controls.Add(this.btnRunProfiler);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MCEPControl";
            this.Text = "3E MatterSphere MCEP Admin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunProfiler;
        private System.Windows.Forms.Button btnRunStorer;
        private System.Windows.Forms.Button btnImportMissingUsers;
        private System.Windows.Forms.Button btnEditUsers;
        private System.Windows.Forms.Button btnEnableManualRun;
        private System.Windows.Forms.TextBox txtDefaultFolderName;
        private System.Windows.Forms.Label lbl1;
    }
}

