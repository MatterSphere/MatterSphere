namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    partial class DocumentPreviewAddin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentPreviewAddin));
            this.txtPreview = new System.Windows.Forms.RichTextBox();
            this.previewer1 = new FWBS.OMS.UI.Windows.DocumentManagement.ucDocumentPreviewer();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Location = new System.Drawing.Point(5, 25);
            this.pnlDesign.Size = new System.Drawing.Size(168, 461);
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // txtPreview
            // 
            this.txtPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreview.Location = new System.Drawing.Point(173, 25);
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.Size = new System.Drawing.Size(663, 461);
            this.txtPreview.TabIndex = 10;
            this.txtPreview.Text = "";
            this.txtPreview.Visible = false;
            // 
            // previewer1
            // 
            this.previewer1.AdditionalProperties = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("previewer1.AdditionalProperties")));
            this.previewer1.CultureProperties = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("previewer1.CultureProperties")));
            this.previewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewer1.Location = new System.Drawing.Point(5, 25);
            this.previewer1.Name = "previewer1";
            this.previewer1.Size = new System.Drawing.Size(831, 461);
            this.previewer1.TabIndex = 11;
            // 
            // tbDescription
            // 
            this.tbDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbDescription.Location = new System.Drawing.Point(5, 5);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.ReadOnly = true;
            this.tbDescription.Size = new System.Drawing.Size(831, 20);
            this.tbDescription.TabIndex = 12;
            // 
            // lblError
            // 
            this.lblError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(173, 25);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(663, 461);
            this.lblError.TabIndex = 13;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblError.Visible = false;
            // 
            // DocumentPreviewAddin
            // 
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.txtPreview);
            this.Controls.Add(this.previewer1);
            this.Controls.Add(this.tbDescription);
            this.Name = "DocumentPreviewAddin";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Controls.SetChildIndex(this.tbDescription, 0);
            this.Controls.SetChildIndex(this.previewer1, 0);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.txtPreview, 0);
            this.Controls.SetChildIndex(this.lblError, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtPreview;
        private ucDocumentPreviewer previewer1;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lblError;
    }
}
