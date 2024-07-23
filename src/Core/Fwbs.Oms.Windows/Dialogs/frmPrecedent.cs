using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmPrecedent.
    /// </summary>
    internal class frmPrecedent : frmNewBrandIdent
	{
		internal FWBS.OMS.UI.Windows.ucPrecedent Precedent;
		protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private FWBS.OMS.UI.Windows.Accelerators accelerators1;
		private FWBS.OMS.UI.Windows.ResourceLookup res;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button button1 = new Button();

		internal frmPrecedent()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.Controls.Add(button1);
            button1.Location = new Point(-100, -100);
            SetIcon(Images.DialogIcons.Precedent);

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
            this.Precedent = new FWBS.OMS.UI.Windows.ucPrecedent();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.SuspendLayout();
            // 
            // Precedent
            // 
            this.Precedent.AdditionalInfoVisible = true;
            this.Precedent.AddJobVisible = true;
            this.Precedent.ButtonCancelVisible = true;
            this.Precedent.ButtonContinueVisible = true;
            this.Precedent.ButtonEditVisible = true;
            this.Precedent.ButtonPrintVisible = true;
            this.Precedent.ButtonViewVisible = true;
            this.Precedent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Precedent.JobListVisible = true;
            this.Precedent.Location = new System.Drawing.Point(0, 0);
            this.Precedent.Name = "Precedent";
            this.Precedent.Padding = new System.Windows.Forms.Padding(3);
            this.Precedent.ShowLibraryCategory = false;
            this.Precedent.Size = new System.Drawing.Size(784, 481);
            this.Precedent.TabIndex = 0;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 80;
            this.ucFormStorage1.DefaultPercentageWidth = 80;
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\Precedent";
            this.ucFormStorage1.Version = ((long)(1));
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // frmPrecedent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 481);
            this.Controls.Add(this.Precedent);
            this.KeyPreview = true;
            this.res.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECMANAGER", "Precedent Manager", ""));
            this.Name = "frmPrecedent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Precedent Library";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmPrecedent_Closing);
            this.Load += new System.EventHandler(this.frmPrecedent_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmPrecedent_Load(object sender, System.EventArgs e)
		{
            try
            {
                new SystemPermission(StandardPermissionType.UpdatePrecedent).Check();
            }
            catch
            {
                Precedent.ButtonEditVisible = false;
            }


		}

		private void frmPrecedent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            button1.Focus();
            e.Cancel = true;
			this.Visible = false;
		}

    }
}
