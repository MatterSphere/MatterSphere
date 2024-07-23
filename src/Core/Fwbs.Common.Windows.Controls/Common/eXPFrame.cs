using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public class eXPFrame : System.Windows.Forms.Panel, IeXPFrame
	{
		private System.ComponentModel.IContainer components = null;
		private string _text = "";
		private ExtColor _forecolor;
		private ExtColor _fraforecolor;
		private ExtColor _backcolor;

		public eXPFrame()
		{
            InitializeComponent();
            // This call is required by the Windows Form Designer.
			_forecolor = new ExtColor(ExtColorPresets.FrameForeColor,ExtColorTheme.Auto);
			_fraforecolor = new ExtColor(ExtColorPresets.FrameLineForeColor,ExtColorTheme.Auto);

            if(this._fraforecolor.CurrentTheme == ExtColorTheme.Windows8)
                base.BackColor = this.FrameForeColor["FrameBackColor"];
			_backcolor = new ExtColor(SystemColors.Control);

			_backcolor.SettingsChanged += new EventHandler(UpdateSettings);
			_fraforecolor.SettingsChanged += new EventHandler(UpdateSettings);
			_forecolor.SettingsChanged += new EventHandler(UpdateSettings);
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
			this.SystemColorsChanged += new System.EventHandler(this.eXPFrame_SystemColorsChanged);
			this.Resize += new System.EventHandler(this.eXPFrame_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.eXPFrame_Paint);

		}
		#endregion

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

		public void eXPFrame_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            PainteXPFrame(e);
        }

        public void PainteXPFrame(System.Windows.Forms.PaintEventArgs e)
        {
            eXPFramePainter painter = new eXPFramePainter(this, _text, _backcolor, _fraforecolor, base.BackColor, eXPFrameType.eXPFrame);
            painter.PainteXPFrame(e);
        }

		private void eXPFrame_Resize(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		private void eXPFrame_SystemColorsChanged(object sender, System.EventArgs e)
		{
			_backcolor.RefreshColors();
			_forecolor.RefreshColors();
			_fraforecolor.RefreshColors();
		}
	}

	public class ColorDisplayEditor : UITypeEditor
	{
		public ColorDisplayEditor()
		{
		}

		public override bool GetPaintValueSupported ( ITypeDescriptorContext ctx )
		{
			return true ;
		}

		public override void PaintValue ( PaintValueEventArgs e )
		{
			try
			{
				ExtColor ec = (ExtColor)e.Value;
                using (SolidBrush b1 = new SolidBrush(ec.Color))
                {
                    e.Graphics.FillRectangle(b1, e.Bounds);
                }
			}
			catch
			{}
		}
	}
}

