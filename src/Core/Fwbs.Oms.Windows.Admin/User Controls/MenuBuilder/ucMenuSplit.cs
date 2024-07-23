using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucMenuSplit.
    /// </summary>
    public class ucMenuSplit : Control
	{
		private MenuSplitAlignment _align;
		private int _visibleheight;
		private int _visiblewidth;
		private bool _visible = true;

        public ucMenuSplit()
        {
            this.BackColor = SystemColors.Window;
            this.LineAlignment = MenuSplitAlignment.Veritcal;
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            this.LineAlignment = _align;
            base.OnDpiChangedAfterParent(e);
        }

        public MenuSplitAlignment LineAlignment
		{
			get
			{
				return _align;
			}
			set
			{
				_align = value;
				if (_align == MenuSplitAlignment.Veritcal)
				{
					this.Size = LogicalToDeviceUnits(new Size(4, 24));
				}
				else
				{
					this.Size = LogicalToDeviceUnits(new Size(24, 2));
				}
                _visibleheight = Height;
                _visiblewidth = Width;
                this.Invalidate();
			}
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            int x, offset = LogicalToDeviceUnits(4);
            base.OnPaint(e);

            if (_align == MenuSplitAlignment.Veritcal)
			{
                x = (this.Width + 1) / 2;
				e.Graphics.DrawLine(SystemPens.ControlDark, x - 1, offset, x - 1, this.Height - offset - 1);
				e.Graphics.DrawLine(SystemPens.ControlLight, x, offset, x, this.Height - offset - 1);
			}
			else
			{
                x = LogicalToDeviceUnits(24);
                e.Graphics.DrawLine(SystemPens.ControlDark, x, 0, this.Width - offset, 0);
                e.Graphics.DrawLine(SystemPens.ControlLight, x, 1, this.Width - offset, 1);
       		}
		}

		public new bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				_visible = value;
				if (LineAlignment == MenuSplitAlignment.Horizontal)
				{
					this.Height = _visible ? _visibleheight : 0;
				}
				else
				{
					this.Width = _visible ? _visiblewidth : 0;
				}
			}
		}

	}

	public enum MenuSplitAlignment {Veritcal,Horizontal};
}
