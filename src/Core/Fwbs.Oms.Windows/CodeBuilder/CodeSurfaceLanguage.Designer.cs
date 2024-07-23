namespace FWBS.OMS.Design.CodeBuilder
{
    partial class CodeSurfaceLanguage
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
            this.chkDontAsk = new System.Windows.Forms.CheckBox();
            this.ResourceObject = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.labChooseLang = new System.Windows.Forms.Label();
            this.btnVB = new System.Windows.Forms.Button();
            this.btnCSharp = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkDontAsk
            // 
            this.chkDontAsk.AutoSize = true;
            this.chkDontAsk.Location = new System.Drawing.Point(15, 52);
            this.ResourceObject.SetLookup(this.chkDontAsk, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkDontAsk", "Don\'t ask again", ""));
            this.chkDontAsk.Name = "chkDontAsk";
            this.chkDontAsk.Size = new System.Drawing.Size(107, 19);
            this.chkDontAsk.TabIndex = 0;
            this.chkDontAsk.Text = "Don\'t ask again";
            this.chkDontAsk.UseVisualStyleBackColor = true;
            // 
            // labChooseLang
            // 
            this.labChooseLang.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labChooseLang.Location = new System.Drawing.Point(12, 9);
            this.ResourceObject.SetLookup(this.labChooseLang, new FWBS.OMS.UI.Windows.ResourceLookupItem("labChooseLang", "You are about to create a new Script. Please choose the language you wish to deve" +
            "lop in.", ""));
            this.labChooseLang.Name = "labChooseLang";
            this.labChooseLang.Size = new System.Drawing.Size(423, 37);
            this.labChooseLang.TabIndex = 3;
            this.labChooseLang.Text = "You are about to create a new Script. Please choose the language you wish to deve" +
    "lop in.";
            // 
            // btnVB
            // 
            this.btnVB.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnVB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVB.Location = new System.Drawing.Point(360, 10);
            this.ResourceObject.SetLookup(this.btnVB, new FWBS.OMS.UI.Windows.ResourceLookupItem("VB", "VB", ""));
            this.btnVB.Name = "btnVB";
            this.btnVB.Size = new System.Drawing.Size(75, 25);
            this.btnVB.TabIndex = 4;
            this.btnVB.Text = "VB";
            this.btnVB.UseVisualStyleBackColor = true;
            // 
            // btnCSharp
            // 
            this.btnCSharp.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnCSharp.Location = new System.Drawing.Point(279, 10);
            this.ResourceObject.SetLookup(this.btnCSharp, new FWBS.OMS.UI.Windows.ResourceLookupItem("C#", "C#", ""));
            this.btnCSharp.Name = "btnCSharp";
            this.btnCSharp.Size = new System.Drawing.Size(75, 25);
            this.btnCSharp.TabIndex = 3;
            this.btnCSharp.Text = "C#";
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.SystemColors.Control;
            this.pnlButtons.Controls.Add(this.btnVB);
            this.pnlButtons.Controls.Add(this.btnCSharp);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 80);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(444, 45);
            this.pnlButtons.TabIndex = 4;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.labChooseLang);
            this.pnlMain.Controls.Add(this.chkDontAsk);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(444, 80);
            this.pnlMain.TabIndex = 5;
            // 
            // CodeSurfaceLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(444, 125);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.ResourceObject.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHOOSELNG", "Choose Language", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CodeSurfaceLanguage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Language";
            this.pnlButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDontAsk;
        private UI.Windows.ResourceLookup ResourceObject;
        private System.Windows.Forms.Label labChooseLang;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnVB;
        private System.Windows.Forms.Button btnCSharp;
        private System.Windows.Forms.Panel pnlMain;
    }
}