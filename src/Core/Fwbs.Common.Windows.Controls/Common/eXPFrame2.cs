using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public class eXPFrame2 : System.Windows.Forms.Panel, IeXPFrame
	{
		private System.ComponentModel.IContainer components = null;
		private string _text = "";
		private ExtColor _forecolor;
		private ExtColor _fraforecolor;
		private ExtColor _backcolor;
        private Color linecolor;
        private Color headerbackcolor;

		public eXPFrame2()
		{
            InitializeComponent();
            SetDefaultLookAndFeel();
            SetupUpdateEvents();
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// eXPFrame
			// 
			this.SystemColorsChanged += new System.EventHandler(this.eXPFrame2_SystemColorsChanged);
			this.Resize += new System.EventHandler(this.eXPFrame2_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.eXPFrame2_Paint);

		}
		#endregion


        private void SetupUpdateEvents()
        {
            _backcolor.SettingsChanged += new EventHandler(UpdateSettings);
            _fraforecolor.SettingsChanged += new EventHandler(UpdateSettings);
            _forecolor.SettingsChanged += new EventHandler(UpdateSettings);
        }

        private void SetDefaultLookAndFeel()
        {
            _forecolor = new ExtColor(ExtColorPresets.FrameForeColor, ExtColorTheme.Auto);
            _fraforecolor = new ExtColor(ExtColorPresets.FrameLineForeColor, ExtColorTheme.Auto);

            if (this._fraforecolor.CurrentTheme == ExtColorTheme.Windows8)
                base.BackColor = this.FrameForeColor["FrameBackColor"];
            _backcolor = new ExtColor(SystemColors.Control);
            linecolor = Color.FromArgb(218, 222, 214);
            headerbackcolor = Color.FromArgb(245, 245, 245);
        }

		private void UpdateSettings(object sender, EventArgs e)
		{
            base.BackColor = this.FrameBackColor.Color;
            this.Invalidate();
		}

		[Browsable(true)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				_text = value;
				Invalidate();
			}
		}

		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
			}
		}

		[Category("Appearance")]
        [Browsable(false)]
		public ExtColor FrameBackColor
		{
			get
			{
				return _backcolor;
			}
			set
			{
				_backcolor = value;
				_backcolor.SettingsChanged +=new EventHandler(UpdateSettings);
			}
		}

        [Category("Appearance")]
        public Color LineColor
        {
            get
            {
                return linecolor;
            }
            set
            {
                linecolor = value;
                this.Invalidate();
            }
        }

        [Category("Appearance")]
        public Color HeaderBackColor
        {
            get
            {
                return headerbackcolor;
            }
            set
            {
                headerbackcolor = value;
                this.Invalidate();
            }
        }

		[Category("Appearance")]
        [Browsable(false)]
		public ExtColor FontColor
		{
			get
			{
				return _forecolor;
			}
			set
			{
				_forecolor = value;
				_forecolor.SettingsChanged +=new EventHandler(UpdateSettings);
			}
		}

		[Category("Appearance")]
        [Browsable(false)]
		public ExtColor FrameForeColor
		{
			get
			{
				return _fraforecolor;
			}
			set
			{
				_fraforecolor = value;
				_fraforecolor.SettingsChanged +=new EventHandler(UpdateSettings);
			}
		}

		private void PropertyChangeEvent(object sender, EventArgs e)
		{
			this.Invalidate();
		}

        private void eXPFrame2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            PainteXPFrame(e);
        }

        public void PainteXPFrame(System.Windows.Forms.PaintEventArgs e)
        {
            eXPFramePainter painter = new eXPFramePainter(this, _text, _backcolor, _fraforecolor, base.BackColor, eXPFrameType.eXPFrame2, linecolor, headerbackcolor);
            painter.PainteXPFrame(e);
        }

		private void eXPFrame2_Resize(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		private void eXPFrame2_SystemColorsChanged(object sender, System.EventArgs e)
		{
			_backcolor.RefreshColors();
			_forecolor.RefreshColors();
			_fraforecolor.RefreshColors();
		}
	}
}

