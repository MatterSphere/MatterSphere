namespace FWBS.OMS.UI.UserControls.Common
{
    partial class ucLabelField
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
            this.Panel = new System.Windows.Forms.Panel();
            this.Value = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.Link = new System.Windows.Forms.LinkLabel();
            this.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel
            // 
            this.Panel.BackColor = System.Drawing.Color.Transparent;
            this.Panel.Controls.Add(this.Link);
            this.Panel.Controls.Add(this.Value);
            this.Panel.Controls.Add(this.Title);
            this.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel.Location = new System.Drawing.Point(0, 0);
            this.Panel.Name = "Panel";
            this.Panel.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.Panel.Size = new System.Drawing.Size(150, 50);
            this.Panel.TabIndex = 0;
            // 
            // Value
            // 
            this.Value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Value.AutoSize = true;
            this.Value.ForeColor = System.Drawing.SystemColors.GrayText;
            this.Value.Location = new System.Drawing.Point(5, 22);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(34, 13);
            this.Value.TabIndex = 1;
            this.Value.Text = "Value";
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(5, 5);
            this.Title.Margin = new System.Windows.Forms.Padding(0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(27, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "Title";
            // 
            // Link
            // 
            this.Link.AutoSize = true;
            this.Link.Location = new System.Drawing.Point(5, 22);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(56, 13);
            this.Link.TabIndex = 2;
            this.Link.TabStop = true;
            this.Link.Text = "Link Label";
            // 
            // ucLabelField
            //
            this.AutoSize = true;
            this.Controls.Add(this.Panel);
            this.Name = "ucLabelField";
            this.Size = new System.Drawing.Size(150, 50);
            this.Panel.ResumeLayout(false);
            this.Panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel;
        private System.Windows.Forms.Label Value;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.LinkLabel Link;
    }
}
