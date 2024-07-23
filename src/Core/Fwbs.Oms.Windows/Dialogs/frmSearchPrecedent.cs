using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    internal class frmSearchPrecedent : BaseForm
	{
		private System.ComponentModel.IContainer components;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private FWBS.OMS.UI.Windows.Accelerators accelerators1;
        private Panel pnlButtons;
		private ucPrecedent ucPrecedent1 = null;

		public frmSearchPrecedent()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			ucPrecedent1.SetDefaults(null, "");
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ucPrecedent1 = new FWBS.OMS.UI.Windows.ucPrecedent();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(4, 4);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(4, 32);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            // 
            // ucPrecedent1
            // 
            this.ucPrecedent1.AdditionalInfoVisible = false;
            this.ucPrecedent1.AddJobVisible = false;
            this.ucPrecedent1.ButtonCancelVisible = false;
            this.ucPrecedent1.ButtonContinueVisible = false;
            this.ucPrecedent1.ButtonEditVisible = false;
            this.ucPrecedent1.ButtonPrintVisible = false;
            this.ucPrecedent1.ButtonViewVisible = false;
            this.ucPrecedent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrecedent1.JobListVisible = false;
            this.ucPrecedent1.Location = new System.Drawing.Point(0, 0);
            this.ucPrecedent1.Name = "ucPrecedent1";
            this.ucPrecedent1.Padding = new System.Windows.Forms.Padding(3);
            this.ucPrecedent1.ShowLibraryCategory = false;
            this.ucPrecedent1.Size = new System.Drawing.Size(620, 441);
            this.ucPrecedent1.TabIndex = 0;
            this.ucPrecedent1.SelectedItemDoubleClick += new System.EventHandler(this.ucPrecedent1_SelectedItemDoubleClick);
            this.ucPrecedent1.DoubleClick += new System.EventHandler(this.ucPrecedent1_DoubleClick);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\SearchPrecedent";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(620, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(84, 441);
            this.pnlButtons.TabIndex = 3;
            // 
            // frmSearchPrecedent
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(704, 441);
            this.Controls.Add(this.ucPrecedent1);
            this.Controls.Add(this.pnlButtons);
            this.KeyPreview = true;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("SearchPrecedent", "Search Precedent", ""));
            this.Name = "frmSearchPrecedent";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Search Precedent";
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties


		public Common.KeyValueCollection ReturnValues
		{
			get
			{
				return ucPrecedent1.SelectedItem;
			}
		}

		#endregion

		#region Methods

		private void ucPrecedent1_SelectedItemDoubleClick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

        private void ucPrecedent1_DoubleClick(object sender, EventArgs e)
        {
            if (this.AcceptButton == null)
                MessageBox.ShowInformation("No Accept Button");
            else
                MessageBox.ShowInformation("Accept Button");
        }
        #endregion
    }
}
