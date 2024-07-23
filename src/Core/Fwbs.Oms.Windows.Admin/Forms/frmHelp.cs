namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmHelp.
    /// </summary>
    public class frmHelp : FWBS.OMS.UI.Windows.BaseForm
	{
		private FWBS.OMS.UI.Windows.Admin.ucHelp ucHelp1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmHelp(frmMain MainParent)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			ucHelp1.MainParent = MainParent;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp));
            this.ucHelp1 = new FWBS.OMS.UI.Windows.Admin.ucHelp();
            this.SuspendLayout();
            // 
            // ucHelp1
            // 
            this.ucHelp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucHelp1.Location = new System.Drawing.Point(0, 0);
            this.ucHelp1.Name = "ucHelp1";
            this.ucHelp1.Size = new System.Drawing.Size(593, 383);
            this.ucHelp1.TabIndex = 0;
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(593, 383);
            this.Controls.Add(this.ucHelp1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHelp";
            this.Text = "Help";
            this.ResumeLayout(false);

		}
		#endregion

		public string HelpSearch
		{
			get
			{
				return ucHelp1.HelpSearch;
			}
			set
			{
				ucHelp1.HelpSearch = value;
			}
		}

		public int HelpID
		{
			get
			{
				return ucHelp1.HelpID;
			}
			set
			{
				ucHelp1.HelpID = value;
			}
		}
	}
}
