namespace FWBS.OMS.Design.CodeBuilder
{
    partial class CodeWindow
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
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.codeSurface1 = new FWBS.OMS.Design.CodeBuilder.CodeSurface();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "Forms\\CodeBuilder";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // codeSurface1
            // 
            this.codeSurface1.BackColor = System.Drawing.SystemColors.Window;
            this.codeSurface1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeSurface1.IsDirty = false;
            this.codeSurface1.Location = new System.Drawing.Point(0, 0);
            this.codeSurface1.Name = "codeSurface1";
            this.codeSurface1.Size = new System.Drawing.Size(747, 371);
            this.codeSurface1.TabIndex = 0;
            // 
            // CodeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(747, 371);
            this.Controls.Add(this.codeSurface1);
            this.Name = "CodeWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Code Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeWindow_FormClosing);
            this.ResumeLayout(false);

        }

        private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;

        #endregion
        private CodeSurface codeSurface1;
    }
}