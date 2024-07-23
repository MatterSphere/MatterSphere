using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for eXPPanel.
    /// </summary>
    public class eXPPanel : System.Windows.Forms.Panel
	{
		private ExtColor _backcolor = new ExtColor(System.Drawing.Color.White);
		private ExtColor _forecolor = new ExtColor(SystemColors.ControlDark);
		private bool _borderline = false;
		
		public eXPPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			_backcolor.SettingsChanged +=new EventHandler(Refresh);
			_forecolor.SettingsChanged +=new EventHandler(Refresh);
			this.Resize +=new EventHandler(eXPPanel_Resize);
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

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool BorderLine
		{
			get
			{
				return _borderline;
			}
			set
			{
				_borderline = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		public ExtColor Backcolor
		{
			get
			{
				return _backcolor;
			}
			set
			{
				_backcolor = value;
				this.Refresh();
				_backcolor.SettingsChanged +=new EventHandler(Refresh);
			}
		}

		[Category("Appearance")]
		public ExtColor Forecolor
		{
			get
			{
				return _forecolor;
			}
			set
			{
				_forecolor = value;
				this.Refresh();
				_forecolor.SettingsChanged +=new EventHandler(Refresh);
			}
		}

		private void Refresh(object sender, EventArgs e)
		{
			this.Refresh();
		}
		
		public new void Refresh()
		{
			base.BackColor = this.Backcolor.Color;
			this.Paint -=new PaintEventHandler(eXPPanel_Paint);
			if (_borderline)
				this.Paint +=new PaintEventHandler(eXPPanel_Paint);
			base.Refresh();
			this.Invalidate();
		}

		private void eXPPanel_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle(new System.Drawing.Pen(_forecolor.Color,1),0,0,this.Width-1,this.Height-1);
		}

		private void eXPPanel_Resize(object sender, EventArgs e)
		{
			this.Invalidate();
		}
	}
}
