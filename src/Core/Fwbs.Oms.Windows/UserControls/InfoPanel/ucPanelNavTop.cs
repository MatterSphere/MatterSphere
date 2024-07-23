using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    public class ucPanelNavTop : FWBS.OMS.UI.Windows.ucPanelNav
	{
		private System.ComponentModel.IContainer components = null;
		private System.Drawing.Image _topimage; 

		public ucPanelNavTop() : base(true)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

        protected override void PropertiesChanged(bool Fast)
        {
            base.PropertiesChanged(Fast);
            if (_theme == ExtColorTheme.None)
            {
                if (_sheadercolor == Color.Empty)
                    _sheadercolor = Color.White;
            }
            else
            {
                if (ec != null)
                {
                    _headercolor = ec.TaskPainBoxTopHeaderForeColor;
                    _sheadercolor = _headercolor;
                }
            }
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (_topimage != null)
                {
                    _topimage.Dispose();
                    _topimage = null;
                }
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ucPanelNavTop));
			// 
			// labHeader
			// 
			this.labHeader.BackColor = System.Drawing.SystemColors.Window;
			this.labHeader.Name = "labHeader";
			this.labHeader.Size = new System.Drawing.Size(149, 32);
			this.labHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.labHeader_Paint);
			this.labHeader.MouseEnter += new System.EventHandler(this.labHeader_MouseEnter);
			this.labHeader.MouseLeave += new System.EventHandler(this.labHeader_MouseLeave);
			// 
			// pnlSpace
			// 
			this.pnlSpace.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSpace.Location = new System.Drawing.Point(0, 139);
			this.pnlSpace.Name = "pnlSpace";
			this.pnlSpace.Size = new System.Drawing.Size(149, 7);
			// 
			// ucPanelNavTop
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
			this.Name = "ucPanelNavTop";
			this.Size = new System.Drawing.Size(149, 146);

		}
		#endregion

		
		[Category("Header")]
		[DefaultValue(-20)]
		public override int HeaderBrightness
		{
			get
			{
				return _hdbrightness;
			}
			set
			{
				_hdbrightness = value;
				PropertiesChanged();
			}
		}


		[Category("Header")]
		public System.Drawing.Image Image
		{
			get
			{
				return _topimage;
			}
			set
			{
				_topimage = value;
				labHeader.Invalidate();
			}
		}
				
		[Category("Header")]
		[DefaultValue(typeof(System.Drawing.Color),"White")]
		public override System.Drawing.Color HeaderColor
		{
			get
			{
				return _sheadercolor;
			}
			set
			{
				_sheadercolor = value;
				PropertiesChanged();
			}
		}

		private void labHeader_MouseLeave(object sender, System.EventArgs e)
		{
			if (_theme == ExtColorTheme.None)
			{
				if (_sheadercolor != System.Drawing.Color.Empty)
					_headercolor = _sheadercolor;
				else
					_headercolor = Color.White;
			}
			else
			{
				if (_sheadercolor != System.Drawing.Color.Empty)
					_headercolor = _sheadercolor;
				else
					_headercolor = ec.TaskPainBoxTopHeaderForeColor;
			}
			labHeader.Invalidate();
		}

		private void labHeader_MouseEnter(object sender, System.EventArgs e)
		{
			if (_theme != ExtColorTheme.None)
				_headercolor = ec.TaskPainBoxTopHeaderForeColorLight;
			else
				_headercolor = Color.Black;
			labHeader.Invalidate();
		}
		
		private void labHeader_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (Parent != null)
			{
                int top = LogicalToDeviceUnits(7);
                Rectangle rect = new Rectangle(0, 0, this.labHeader.Width, top);
                using (SolidBrush b1 = new SolidBrush(Parent.BackColor))
                {
                    e.Graphics.FillRectangle(b1, rect);
                }

				if (_blendcolor1 == System.Drawing.Color.Empty) 
				{
					if (_theme == ExtColorTheme.None)
						_cblendcolor1 = ChangeBrightness(_gradientcolor,_hdbrightness); 
					else
						_cblendcolor1 = ec.TaskPainBoxTopHeaderBlend1;
				}
				else 
					_cblendcolor1 =_blendcolor1;

				if (_blendcolor2 == System.Drawing.Color.Empty) 
				{
					if (_theme == ExtColorTheme.None)
						_cblendcolor2 = _gradientcolor;  
					else
						_cblendcolor2 = ec.TaskPainBoxTopHeaderBlend2;
				}
				else 
					_cblendcolor2 =_blendcolor2;

                Size topimgSize = (_topimage != null) ? LogicalToDeviceUnits(_topimage.Size) : Size.Empty;
                using (System.Drawing.Drawing2D.LinearGradientBrush gs = new System.Drawing.Drawing2D.LinearGradientBrush(labHeader.Bounds, _cblendcolor1, _cblendcolor2, 0, true))
                {
                    rect = new Rectangle(0, top, this.labHeader.Width, this.labHeader.Height - top);
                    e.Graphics.FillRectangle(gs, rect);
                    
                    using (Font f = new Font(CurrentUIVersion.Font, CurrentUIVersion.FontSize * this.DeviceDpi / e.Graphics.DpiX, FontStyle.Bold))
                    {
                        using (SolidBrush b1 = new SolidBrush(_headercolor))
                        {
                            rect.Inflate(-3, 0);
                            if (_topimage != null)
                            {
                                rect.Offset(this.RightToLeft == RightToLeft.Yes ? -topimgSize.Width : topimgSize.Width, 0);
                            }
                            StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center };
                            if (this.RightToLeft == RightToLeft.Yes)
                            {
                                fmt.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                            }
                            e.Graphics.DrawString(_headertext, f, b1, rect, fmt);
                        }
                    }
                }
                using (Pen p1 = new Pen(Parent.BackColor))
                {
                    e.Graphics.DrawLine(p1, 0, top, 1, top);
                    e.Graphics.DrawLine(p1, 0, top, 0, top+1);
                    int w = this.labHeader.Width;
                    e.Graphics.DrawLine(p1, w, top, w - 2, top); //Across
                    e.Graphics.DrawLine(p1, w - 1, top, w - 1, top+1);   //Down
                }
                try
                {
                    var image = _images.Images[(_z == 0 ? 0 : 1) + (int)_navstyle];
                    Size imageSize = LogicalToDeviceUnits(image.Size);
                    rect = Rectangle.FromLTRB(3, top, this.labHeader.Width - 3, this.labHeader.Height);
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        if (_topimage != null)
                            e.Graphics.DrawImage(_topimage, this.labHeader.Width - topimgSize.Width, 0, topimgSize.Width, topimgSize.Height);

                        e.Graphics.DrawImage(image, rect.Left, rect.Top + (rect.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                    }
                    else
                    {
                        if (_topimage != null)
                            e.Graphics.DrawImage(_topimage, 0, 0, topimgSize.Width, topimgSize.Height);

                        e.Graphics.DrawImage(image, rect.Right - imageSize.Width, rect.Top + (rect.Height - imageSize.Height) / 2, imageSize.Width, imageSize.Height);
                    }
                }
                catch { }
			}
		}
	}
}

