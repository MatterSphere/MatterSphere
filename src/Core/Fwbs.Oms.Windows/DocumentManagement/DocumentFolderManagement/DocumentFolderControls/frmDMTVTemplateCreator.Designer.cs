namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    partial class frmDMTVTemplateCreator
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
            this.ucTreeViewTemplateCreator = new FWBS.OMS.UI.DocumentManagement.Addins.ucDMTVTemplateCreator();
            this.SuspendLayout();
            // 
            // ucTreeViewTemplateCreator
            // 
            this.ucTreeViewTemplateCreator.BackColor = System.Drawing.SystemColors.Control;
            this.ucTreeViewTemplateCreator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTreeViewTemplateCreator.Location = new System.Drawing.Point(0, 0);
            this.ucTreeViewTemplateCreator.Margin = new System.Windows.Forms.Padding(2);
            this.ucTreeViewTemplateCreator.Name = "ucTreeViewTemplateCreator";
            this.ucTreeViewTemplateCreator.Size = new System.Drawing.Size(686, 561);
            this.ucTreeViewTemplateCreator.TabIndex = 0;
            this.ucTreeViewTemplateCreator.TemplateID = 0;
            // 
            // frmDMTVTemplateCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(686, 561);
            this.Controls.Add(this.ucTreeViewTemplateCreator);
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmDMTVTmpltCrt", "Template Management", ""));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmDMTVTemplateCreator";
            this.Text = "Template Management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDMTVTemplateCreator_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private ucDMTVTemplateCreator ucTreeViewTemplateCreator;
    }
}