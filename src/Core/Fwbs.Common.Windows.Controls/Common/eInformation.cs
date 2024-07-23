using System;
using System.ComponentModel;
using System.Drawing;
using FWBS.OMS;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eInformation.
    /// </summary>
    public class eInformation : System.Windows.Forms.UserControl
	{
		#region Fields
		private System.Windows.Forms.Panel pnlInformation;
		private System.Windows.Forms.PictureBox picInfo;
		private System.Windows.Forms.Label labTitle;
		private System.Windows.Forms.Label labInformation;
		private System.Windows.Forms.Panel pnlLine;
		private System.Windows.Forms.Panel pnlSpacer;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public eInformation()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            if (Session.CurrentSession.IsConnected)
                labTitle.Text = Session.CurrentSession.Resources.GetResource("TIP", "Tip", "").Text;
            picInfo.Image = LoadIcon();
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlInformation = new System.Windows.Forms.Panel();
            this.labInformation = new System.Windows.Forms.Label();
            this.pnlSpacer = new System.Windows.Forms.Panel();
            this.pnlLine = new System.Windows.Forms.Panel();
            this.labTitle = new System.Windows.Forms.Label();
            this.picInfo = new System.Windows.Forms.PictureBox();
            this.pnlInformation.SuspendLayout();
            this.pnlSpacer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInformation
            // 
            this.pnlInformation.BackColor = System.Drawing.SystemColors.Info;
            this.pnlInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInformation.Controls.Add(this.labInformation);
            this.pnlInformation.Controls.Add(this.pnlSpacer);
            this.pnlInformation.Controls.Add(this.labTitle);
            this.pnlInformation.Controls.Add(this.picInfo);
            this.pnlInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInformation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlInformation.Location = new System.Drawing.Point(0, 0);
            this.pnlInformation.Name = "pnlInformation";
            this.pnlInformation.Padding = new System.Windows.Forms.Padding(37, 3, 7, 3);
            this.pnlInformation.Size = new System.Drawing.Size(357, 45);
            this.pnlInformation.TabIndex = 0;
            // 
            // labInformation
            // 
            this.labInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labInformation.ForeColor = System.Drawing.Color.Black;
            this.labInformation.Location = new System.Drawing.Point(37, 22);
            this.labInformation.Name = "labInformation";
            this.labInformation.Size = new System.Drawing.Size(311, 18);
            this.labInformation.TabIndex = 2;
            this.labInformation.Text = "One Line Information Line";
            // 
            // pnlSpacer
            // 
            this.pnlSpacer.Controls.Add(this.pnlLine);
            this.pnlSpacer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer.Location = new System.Drawing.Point(37, 20);
            this.pnlSpacer.Name = "pnlSpacer";
            this.pnlSpacer.Size = new System.Drawing.Size(311, 2);
            this.pnlSpacer.TabIndex = 4;
            // 
            // pnlLine
            // 
            this.pnlLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLine.Location = new System.Drawing.Point(0, 1);
            this.pnlLine.Name = "pnlLine";
            this.pnlLine.Size = new System.Drawing.Size(311, 1);
            this.pnlLine.TabIndex = 3;
            this.pnlLine.Layout += new System.Windows.Forms.LayoutEventHandler(this.pnlLine_Layout);
            // 
            // labTitle
            // 
            this.labTitle.AutoEllipsis = true;
            this.labTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labTitle.ForeColor = System.Drawing.Color.Black;
            this.labTitle.Location = new System.Drawing.Point(37, 3);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new System.Drawing.Size(311, 17);
            this.labTitle.TabIndex = 1;
            this.labTitle.Text = "Tip";
            // 
            // picInfo
            // 
            this.picInfo.Location = new System.Drawing.Point(3, 3);
            this.picInfo.Name = "picInfo";
            this.picInfo.Size = new System.Drawing.Size(32, 32);
            this.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picInfo.TabIndex = 0;
            this.picInfo.TabStop = false;
            // 
            // eInformation
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.pnlInformation);
            this.Name = "eInformation";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.Size = new System.Drawing.Size(360, 48);
            this.pnlInformation.ResumeLayout(false);
            this.pnlSpacer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            picInfo.Image = LoadIcon();
        }

        private Bitmap LoadIcon()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(eInformation2));
            Icon icon = new Icon(resources.GetObject("InfoLarge.ico") as Icon, LogicalToDeviceUnits(new Size(32, 32)));
            return icon.ToBitmap();
        }

		private void pnlLine_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
        {
            if (e.AffectedProperty == "Bounds" && e.AffectedControl.Height < 1)
            {
                e.AffectedControl.Height = 1;
            }
        }

		#region Properties
		[Category("Appearance")]
		[DefaultValue("Title")]
		public string Title
		{
			get
			{
				return labTitle.Text;
			}
			set
			{
				labTitle.Text = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return labInformation.Text;
			}
			set
			{
				labInformation.Text = value;
				this.OnTextChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool UnderlineTitle
		{
			get
			{
				return pnlLine.Visible;
			}
			set
			{
				pnlLine.Visible = value;
				pnlSpacer.Visible = value;
			}
		}
		#endregion
	}
}
