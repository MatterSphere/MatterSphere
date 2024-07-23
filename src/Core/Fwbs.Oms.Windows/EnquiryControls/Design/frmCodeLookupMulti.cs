using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmCodeLookupMulti.
    /// </summary>
    public class frmCodeLookupMulti : BaseForm
	{
		private FWBS.OMS.UI.Windows.eCLCollectionSelector eCLCollectionSelector1;
		private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel panel;
        private IContainer components;

		public frmCodeLookupMulti()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.eCLCollectionSelector1 = new FWBS.OMS.UI.Windows.eCLCollectionSelector();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // eCLCollectionSelector1
            // 
            this.eCLCollectionSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eCLCollectionSelector1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.eCLCollectionSelector1.IsDirty = true;
            this.eCLCollectionSelector1.Location = new System.Drawing.Point(8, 8);
            this.eCLCollectionSelector1.Name = "eCLCollectionSelector1";
            this.eCLCollectionSelector1.Size = new System.Drawing.Size(485, 295);
            this.eCLCollectionSelector1.TabIndex = 0;
            this.eCLCollectionSelector1.Value = "";
            this.eCLCollectionSelector1.ValueSplit = ",";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(8, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.btnOK);
            this.panel.Controls.Add(this.btnCancel);
            this.panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel.Location = new System.Drawing.Point(493, 8);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(83, 295);
            this.panel.TabIndex = 3;
            // 
            // frmCodeLookupMulti
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 311);
            this.Controls.Add(this.eCLCollectionSelector1);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCodeLookupMulti";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OMS Mutli Selector";
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public FWBS.OMS.UI.Windows.eCLCollectionSelector CollectionSelector
		{
			get
			{
				return eCLCollectionSelector1;
			}
		}
	}
}
