using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for frmOpenReports.
    /// </summary>
    internal class frmOpenReports : FWBS.OMS.UI.Windows.BaseForm
    {
		public FWBS.OMS.UI.Windows.Reports.ucReportsView ReportsView;
        private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private IContainer components;

		internal frmOpenReports()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.AcceptButton = ReportsView.ApplyButton;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpenReports));
            this.ReportsView = new FWBS.OMS.UI.Windows.Reports.ucReportsView();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.SuspendLayout();
            // 
            // ReportsView
            // 
            this.ReportsView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ReportsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReportsView.Location = new System.Drawing.Point(0, 0);
            this.ReportsView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ReportsView.Name = "ReportsView";
            this.ReportsView.Padding = new System.Windows.Forms.Padding(4);
            this.ReportsView.Report = null;
            this.ReportsView.Size = new System.Drawing.Size(781, 554);
            this.ReportsView.TabIndex = 2;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 90;
            this.ucFormStorage1.DefaultPercentageWidth = 90;
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ucFormStorage1.UniqueID = "Forms\\OpenReports";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // frmOpenReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(781, 554);
            this.Controls.Add(this.ReportsView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmOpenReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Open Report";
            this.ResumeLayout(false);

		}
		#endregion

	}
}
