using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class IconMenuItem : MenuItem
	{

		private int _icon = -1;
		private ImageList _imagelist = null;

		public IconMenuItem() : this("", null, -1, null, Shortcut.None ) { }


		public IconMenuItem(string text, ImageList ImageList, int ImageIndex, EventHandler onClick, Shortcut shortcut) : base( text, onClick, shortcut )
		{
			OwnerDraw = true;
			_imagelist = ImageList;
			_icon = ImageIndex;
		}

		[DefaultValue(-1)]
		public int ImageIndex
		{
			get 
			{ 
				return _icon; 
			}
			set 
			{ 
				_icon = value;
			}
		}

		[DefaultValue(null)]
		public ImageList ImageList
		{
			get 
			{ 
				return _imagelist; 
			}
			set 
			{ 
				_imagelist = value;
			}
		}

        private int LogicalToDeviceUnits(int value)
        {
            Control control = GetContextMenu()?.SourceControl;
            return (control != null) ? control.LogicalToDeviceUnits(value) : value;
        }

        private int DeviceDpi
        {
            get
            {
                Control control = GetContextMenu()?.SourceControl;
                return (control != null) ? control.DeviceDpi : 96;
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center };
			sf.HotkeyPrefix = HotkeyPrefix.Show;
			sf.SetTabStops( 60, new float[] { 0 } );

			base.OnMeasureItem( e );

            e.ItemHeight = LogicalToDeviceUnits(22);
			e.ItemWidth = LogicalToDeviceUnits(24) + Size.Ceiling(e.Graphics.MeasureString(GetRealText(), SystemInformation.GetMenuFontForDpi(DeviceDpi), 10000, sf)).Width;
            int minWidth = LogicalToDeviceUnits(150);
            if (e.ItemWidth < minWidth) e.ItemWidth = minWidth;
			sf.Dispose();
			sf = null;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
            Rectangle rcBk = e.Bounds;
			rcBk.Width = LogicalToDeviceUnits(24);
			e.Graphics.FillRectangle(SystemBrushes.Control,rcBk);

			rcBk = e.Bounds;
			if ( (e.State & DrawItemState.Selected) != 0 ) 
			{
				if (this.Enabled)
				{
					rcBk.Inflate(-1,-1);
                    using (Brush br = new SolidBrush(Color.FromArgb(193, 210, 238)))
                    {
                        e.Graphics.FillRectangle(br, rcBk);
                    }
                    e.Graphics.DrawRectangle(SystemPens.Highlight, rcBk);
                    rcBk.Inflate(1, 1);
				}
                rcBk.Offset(LogicalToDeviceUnits(27), 0);
            }
			else
			{
				rcBk.Offset(LogicalToDeviceUnits(24), 0);
				e.Graphics.FillRectangle(SystemBrushes.Window, rcBk);
                rcBk.Offset(LogicalToDeviceUnits(3), 0);
            }

            if (_icon > -1 && _imagelist != null && _icon < _imagelist.Images.Count)
			{
                Image image = _imagelist.Images[_icon];
                int pos = LogicalToDeviceUnits(4);
                int size = LogicalToDeviceUnits(16);

                if (this.Enabled)
                    e.Graphics.DrawImage(image, e.Bounds.X + pos, e.Bounds.Y + pos, size, size);
                else
					Images.DrawGrayedImage(e.Graphics, image, e.Bounds.X + pos, e.Bounds.Y + pos, size, size);
            }

            StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center };
			sf.HotkeyPrefix = HotkeyPrefix.Show;
			sf.SetTabStops(60, new float[] { 0 } );

            using (Brush br2 = new SolidBrush(this.Enabled ? Color.FromArgb(51, 51, 51) : SystemColors.GrayText))
            {
                e.Graphics.DrawString(GetRealText(), SystemInformation.GetMenuFontForDpi(DeviceDpi), br2, rcBk, sf);
            }
			sf.Dispose();
			sf = null;
		}

		private string GetRealText()
		{
			string s = Text;

			// Append shortcut if one is set and it should be visible
			if ( ShowShortcut && (Shortcut != Shortcut.None) ) 
			{
				// To get a string representation of a Shortcut value, cast
				// it into a Keys value and use the KeysConverter class (via TypeDescriptor).
				Keys k = (Keys) Shortcut;
				s += "\t" + TypeDescriptor.GetConverter( typeof(Keys) ).ConvertToString( k );
			}
  
			return s;
		}
    }
}