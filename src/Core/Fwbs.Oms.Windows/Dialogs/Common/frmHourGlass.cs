using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmHourGlass.
    /// </summary>
    public class frmHourGlass : BaseForm
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Timer timProcess;
		public System.Windows.Forms.Label Caption;
		private System.Windows.Forms.ProgressBar progressBar1;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;
		private Control _owner;

		public frmHourGlass(Control Owner)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_owner = Owner;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHourGlass));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Caption = new System.Windows.Forms.Label();
            this.timProcess = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Caption
            // 
            this.Caption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Caption.Location = new System.Drawing.Point(64, 18);
            this.resourceLookup1.SetLookup(this.Caption, new FWBS.OMS.UI.Windows.ResourceLookupItem("PLZWAIT", "Please Wait ...", ""));
            this.Caption.Name = "Caption";
            this.Caption.Size = new System.Drawing.Size(236, 15);
            this.Caption.TabIndex = 1;
            this.Caption.Text = "Please Wait ...";
            // 
            // timProcess
            // 
            this.timProcess.Tick += new System.EventHandler(this.timProcess_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(5, 53);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(295, 13);
            this.progressBar1.TabIndex = 2;
            // 
            // frmHourGlass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(305, 71);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Caption);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmHourGlass", "Processing...", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHourGlass";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Processing...";
            this.Closed += new System.EventHandler(this.frmHourGlass_Closed);
            this.Load += new System.EventHandler(this.frmHourGlass_Load);
            this.VisibleChanged += new System.EventHandler(this.frmHourGlass_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void timProcess_Tick(object sender, System.EventArgs e)
		{
			if (progressBar1.Value+1 > progressBar1.Maximum) progressBar1.Value = progressBar1.Minimum;
			progressBar1.Value++;
			if (_owner == null || (_owner != null && _owner.Visible==false))
				this.Close();
		}

		private void frmHourGlass_VisibleChanged(object sender, System.EventArgs e)
		{
			timProcess.Enabled = this.Visible;
		}

		private void frmHourGlass_Load(object sender, System.EventArgs e)
		{
			if (_owner == null || (_owner != null && _owner.Visible==false))
				this.Close();
		}

		private void frmHourGlass_Closed(object sender, System.EventArgs e)
		{
			timProcess.Enabled = false;
		}
	}
}
