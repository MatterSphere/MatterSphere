using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    internal class frmSplash : System.Windows.Forms.Form
	{

        private System.Windows.Forms.Timer pulse;
        private Label labCopyright;
        private Panel panel2;
		private System.ComponentModel.IContainer components;

		public frmSplash()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            AssemblyCopyrightAttribute cpy = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
            labCopyright.Text = cpy.Copyright;

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            this.pulse = new System.Windows.Forms.Timer(this.components);
            this.labCopyright = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pulse
            // 
            this.pulse.Interval = 3200;
            this.pulse.Tick += new System.EventHandler(this.pulse_Tick);
            // 
            // labCopyright
            // 
            this.labCopyright.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labCopyright.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.labCopyright.Location = new System.Drawing.Point(8, 297);
            this.labCopyright.Name = "labCopyright";
            this.labCopyright.Size = new System.Drawing.Size(431, 74);
            this.labCopyright.TabIndex = 1;
            this.labCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::FWBS.OMS.UI.Properties.Resources.ELITE_3E_MatterSphere_Splash;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Controls.Add(this.labCopyright);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(8);
            this.panel2.Size = new System.Drawing.Size(447, 379);
            this.panel2.TabIndex = 2;
            // 
            // frmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(447, 379);
            this.ControlBox = false;
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSplash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSplash_Closing);
            this.Load += new System.EventHandler(this.frmSplash_Load);
            this.Click += new System.EventHandler(this.frmSplash_Click);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmSplash_KeyPress);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion



		private void frmSplash_Load(object sender, System.EventArgs e)
		{
            try
            {
                //Load the branding logo
                Image splashlogo = Branding.GetSplashLogo();
                if (splashlogo != null)
                    this.BackgroundImage = splashlogo;
                pulse.Enabled = true;
            }
            catch { }
		}

	
		private void frmSplash_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void frmSplash_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void pulse_Tick(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void frmSplash_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}
	}
}

