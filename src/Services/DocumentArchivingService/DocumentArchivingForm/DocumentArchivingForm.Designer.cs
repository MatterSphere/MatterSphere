namespace DocumentArchivingForm
{
    partial class DocumentArchivingForm
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
            this.SuspendLayout();
            // 
            // btnRunProcess
            // 
            this.btnRunProcess.Location = new System.Drawing.Point(42, 41);
            this.btnRunProcess.Name = "btnRunProcess";
            this.btnRunProcess.Size = new System.Drawing.Size(160, 28);
            this.btnRunProcess.TabIndex = 0;
            this.btnRunProcess.Text = "Run Process";
            this.btnRunProcess.UseVisualStyleBackColor = true;
            this.btnRunProcess.Click += new System.EventHandler(this.btnRunProcess_Click);
            // 
            // DocumentArchivingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 111);
            this.Controls.Add(this.btnRunProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentArchivingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3E MatterSphere Document Archiver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRunProcess;
    }
}

