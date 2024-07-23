using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmImportProgress.
    /// </summary>
    public class frmImportProgress : FWBS.OMS.UI.Windows.BaseForm
	{
		#region Events
		public event EventHandler Cancelled = null;
		#endregion
		
		#region Fields
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labInfo;
		#endregion
        protected ResourceLookup resourceLookup1;
        private IContainer components;

		#region Constructors
		public frmImportProgress()
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
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labInfo = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(356, 75);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 48);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(424, 16);
            this.progressBar1.TabIndex = 1;
            // 
            // labInfo
            // 
            this.labInfo.Location = new System.Drawing.Point(8, 10);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(424, 23);
            this.labInfo.TabIndex = 2;
            // 
            // frmImportProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(440, 110);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Progress";
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties
		[Browsable(false)]
		public string Label
		{
			get
			{
				return labInfo.Text;
			}
			set
			{
				labInfo.Text = value;
			}
		}

		[Browsable(false)]
		public ProgressBar ProgressBar
		{
			get
			{
				return progressBar1;
			}
		}

		public bool CanCancel
		{
			set
			{
				btnCancel.Enabled = value;
			}
		}

		#endregion

		#region Protected
		protected void OnCancelled()
		{
			if (Cancelled != null)
				Cancelled(this,EventArgs.Empty);
		}
		#endregion

		#region Private
		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			OnCancelled();
		}
		#endregion
	}
}
