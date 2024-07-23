namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmAdministrator2.
    /// </summary>
    public class frmAdminDesktop : BaseForm
	{
		public System.Windows.Forms.ImageList imgTools;
		private System.ComponentModel.IContainer components;
		private FWBS.OMS.UI.Windows.Accelerators accelerators1;
		public ucEditBase2 editbase2;
		public ucEditBase editbase;

		public frmAdminDesktop()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdminDesktop));
            this.imgTools = new System.Windows.Forms.ImageList(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.SuspendLayout();
            // 
            // imgTools
            // 
            this.imgTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTools.ImageStream")));
            this.imgTools.TransparentColor = System.Drawing.Color.Red;
            this.imgTools.Images.SetKeyName(0, "");
            this.imgTools.Images.SetKeyName(1, "");
            this.imgTools.Images.SetKeyName(2, "");
            this.imgTools.Images.SetKeyName(3, "");
            this.imgTools.Images.SetKeyName(4, "");
            this.imgTools.Images.SetKeyName(5, "");
            this.imgTools.Images.SetKeyName(6, "");
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // frmAdminDesktop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(657, 434);
            this.Name = "frmAdminDesktop";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Administrator Desktop";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmAdminDesktop_Closing);
            this.ResumeLayout(false);

		}
		#endregion
		
		private void frmAdminDesktop_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if(editbase != null)
				e.Cancel = !editbase.IsObjectDirty();
			else if(editbase2 != null && editbase2.ListMode == false)
				e.Cancel = !editbase2.IsObjectDirty();
        }
	}
}
