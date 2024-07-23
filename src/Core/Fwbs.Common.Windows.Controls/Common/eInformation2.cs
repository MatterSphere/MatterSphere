using System;
using System.ComponentModel;
using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eInformation2.
    /// </summary>
    public class eInformation2 : System.Windows.Forms.UserControl
	{
        #region Fields
        private System.Windows.Forms.Label labInformation;
        private System.Windows.Forms.Panel pnlInformation;
        private System.Windows.Forms.PictureBox picInfo;
        private bool smallIcon;

        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public eInformation2()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            BorderColor = Color.FromArgb(218, 222, 214);
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
            this.labInformation = new System.Windows.Forms.Label();
            this.pnlInformation = new System.Windows.Forms.Panel();
            this.picInfo = new System.Windows.Forms.PictureBox();
            this.pnlInformation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // labInformation
            // 
            this.labInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labInformation.ForeColor = System.Drawing.Color.Black;
            this.labInformation.Location = new System.Drawing.Point(48, 5);
            this.labInformation.Margin = new System.Windows.Forms.Padding(3);
            this.labInformation.Name = "labInformation";
            this.labInformation.Size = new System.Drawing.Size(310, 51);
            this.labInformation.TabIndex = 4;
            this.labInformation.Text = "One Line Information Line";
            // 
            // pnlInformation
            // 
            this.pnlInformation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlInformation.Controls.Add(this.labInformation);
            this.pnlInformation.Controls.Add(this.picInfo);
            this.pnlInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInformation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlInformation.Location = new System.Drawing.Point(1, 1);
            this.pnlInformation.Name = "pnlInformation";
            this.pnlInformation.Padding = new System.Windows.Forms.Padding(48, 5, 0, 5);
            this.pnlInformation.Size = new System.Drawing.Size(358, 61);
            this.pnlInformation.TabIndex = 5;
            // 
            // picInfo
            // 
            this.picInfo.Location = new System.Drawing.Point(8, 5);
            this.picInfo.Name = "picInfo";
            this.picInfo.Size = new System.Drawing.Size(32, 32);
            this.picInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picInfo.TabIndex = 4;
            this.picInfo.TabStop = false;
            // 
            // eInformation2
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.pnlInformation);
            this.Name = "eInformation2";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(360, 63);
            this.pnlInformation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private Bitmap LoadIcon()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(eInformation2));
            Size iconSize = smallIcon ? new Size(16, 16) : new Size(32, 32);
            Icon icon = new Icon(resources.GetObject(smallIcon ? "InfoSmall.ico" : "InfoLarge.ico") as Icon, LogicalToDeviceUnits(iconSize));
            return icon.ToBitmap();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            picInfo.Image = LoadIcon();
        }

		#region Properties

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
        [Browsable(true)]
        new public Color BackColor
        {
            get
            {
                return pnlInformation.BackColor;
            }
            set
            {
                pnlInformation.BackColor = value;
            }
        }


        [Category("Appearance")]
        [Browsable(true)]
        public Color BorderColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool SmallIcon
        {
            get
            {
                return smallIcon;
            }
            set
            {
                if (value != smallIcon)
                {
                    smallIcon = value;

                    Size picInfoSize = smallIcon ? new Size(16, 16) : new Size(32, 32);
                    picInfo.Size = (Parent == null) ? picInfoSize : LogicalToDeviceUnits(picInfoSize);

                    int paddingLeft = smallIcon ? 36 : 48;
                    var padding = pnlInformation.Padding;
                    padding.Left = (Parent == null) ? paddingLeft : LogicalToDeviceUnits(paddingLeft);
                    pnlInformation.Padding = padding;

                    picInfo.Left = (padding.Left - picInfo.Width) / 2;
                    picInfo.Image = LoadIcon();
                }
            }
        }

		#endregion

	}
}
