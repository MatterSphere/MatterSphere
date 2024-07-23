using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucMenuItem.
    /// </summary>
    public class ucMenuItem : System.Windows.Forms.UserControl
	{
		private bool _popoutvisible = false;
		private ucMenuContainer _childmenucontainer = null;
		private ucMenuContainer _parentmenucontainer = null;
		private DataRow _row = null;
		private int _menuTextWidth = 0;
		public ucMenuSplit Split = null;
		private string _text = "";
		private bool _marked = false;
		private bool _imagevisible = true;
        private Color _iconbackcolor = SystemColors.Window;
        private static readonly string _sPopOut = "\u23F5"; // right arrow in "Segoe UI Symbol" font
        private Image _image = null;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ucMenuItem()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            Height = LogicalToDeviceUnits(24);
            _image = Images.GetAdminMenuItem(33, (Images.IconSize)LogicalToDeviceUnits(16)).ToBitmap();
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            _image = Images.GetAdminMenuItem(33, (Images.IconSize)LogicalToDeviceUnits(16)).ToBitmap();
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (_image != null)
                {
                    _image.Dispose();
                    _image = null;
                }
                if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ucMenuItem
            // 
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.Name = "ucMenuItem";
            this.Size = new System.Drawing.Size(248, 24);
            this.FontChanged += new System.EventHandler(this.ucMenuItem_FontChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucMenuItem_Paint);
            this.ParentChanged += new System.EventHandler(this.ucMenuItem_ParentChanged);
            this.ResumeLayout(false);

		}
		private void txtText_TextChanged(object sender, System.EventArgs e)
		{
			OnTextChanged(e);
		}
		#endregion

		private int MeasureDisplayStringWidth(string text, Font font)
		{
            int width = LogicalToDeviceUnits(8);
            using (Graphics graphics = this.CreateGraphics())
            {
                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap) { HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show })
                    width += (int)Math.Ceiling(graphics.MeasureString(text, font, 1000, sf).Width);
            }
            return width;
        }

        private void ucMenuItem_FontChanged(object sender, EventArgs e)
        {
            _menuTextWidth = MeasureDisplayStringWidth(_text, this.Font);
        }

		private void ucMenuItem_ParentChanged(object sender, System.EventArgs e)
		{
			if (this.Parent != null)
			{
				this.Invalidate();
			}
		}

		private void ucMenuItem_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            int imageAreaWidth = LogicalToDeviceUnits(24);
            using (SolidBrush b1 = new SolidBrush(_iconbackcolor))
            {
                e.Graphics.FillRectangle(b1, 0, 0, imageAreaWidth, this.Height);
            }
			if (_imagevisible)
				e.Graphics.DrawImage(_image, (imageAreaWidth - _image.Size.Width)/2, (this.Height - _image.Size.Height)/2);

            // ********************************************************************************************
            // * Sets the Text Format Settings
            // ********************************************************************************************
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap)
            {
                HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
                LineAlignment = StringAlignment.Center
            };
            if (this.RightToLeft == RightToLeft.Yes)
            {
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }
            // ********************************************************************************************
            using (SolidBrush br = new SolidBrush(this.ForeColor))
            {
                sf.SetTabStops(0, new float[] { LogicalToDeviceUnits(28) });
                e.Graphics.DrawString('\t' + _text, this.Font, br, ClientRectangle, sf);

                if (_popoutvisible)
                {
                    using (Font font = new Font("Segoe UI Symbol", this.Font.Size))
                    {
                        sf.Alignment = StringAlignment.Far;
                        sf.FormatFlags &= ~StringFormatFlags.DirectionRightToLeft;
                        e.Graphics.DrawString(_sPopOut, font, br, ClientRectangle, sf);
                    }
                }
            }

            if (_marked)
            {
                using (Brush b2 = new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.Percent25, SystemColors.Highlight, Color.Transparent))
                {
                    e.Graphics.FillRectangle(b2, 0, 0, this.Width, this.Height);
                }
            }

            sf.Dispose();
		}

		[Category("Appearance")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
                _menuTextWidth = MeasureDisplayStringWidth(_text, this.Font);
				this.Invalidate();
				OnTextChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance")]
		public bool IconVisible
		{
			get
			{
				return _imagevisible;
			}
			set
			{
				_imagevisible = value;
				this.Invalidate();
			}
		}

		[Browsable(false)]
		public bool Marked
		{
			get
			{
				return _marked;
			}
			set
			{
				_marked = value;
				this.Invalidate();
			}
		}

		[Category("Appearance")]
		public bool PopoutVisibe
		{
			get
			{
				return _popoutvisible;
			}
			set
			{
				if (_popoutvisible != value)
				{
					_popoutvisible = value;
					if (value)
					{
						_childmenucontainer = new ucMenuContainer();
						_childmenucontainer.CaptionVisible = false;
						_childmenucontainer.ParentPopoutMenu = this;
					}
					else
						_childmenucontainer = null;
					ucMenuItem_ParentChanged(this,EventArgs.Empty);
				}
			}
		}

		[Category("Appearance")]
		public bool Selected
		{
			get
			{
				return (BackColor == SystemColors.Highlight);
			}
			set
			{
				if (value)
				{
					this.BackColor = SystemColors.Highlight;
					this.ForeColor = SystemColors.HighlightText;
					_iconbackcolor = SystemColors.Highlight;
				}
				else
				{
					this.BackColor = SystemColors.Menu;
					this.ForeColor = SystemColors.MenuText;
					_iconbackcolor = SystemColors.Window;
				}
				this.Invalidate();
			}
		}

		[Browsable(false)]
		public ucMenuContainer ChildMenu
		{
			get
			{
				if (_childmenucontainer != null)
				{
					Point loc = this.Location;
					loc.X += this.Width + this.Parent.Left;
					loc.Y += this.Parent.Top;
					if (_childmenucontainer.Location != loc)
						_childmenucontainer.Location = loc;
				}
				return _childmenucontainer;
			}
		}

		[Browsable(false)]
		public ucMenuContainer ParentMenu
		{
			get
			{
				return _parentmenucontainer;
			}
			set
			{
				_parentmenucontainer = value;
			}
		}

		[Browsable(false)]
		public DataRow DataRow
		{
			get
			{
				return _row;
			}
			set
			{
				_row = value;
			}
		}

		[Browsable(false)]
		public int MenuTextWidth
		{
			get
			{
				return _menuTextWidth + LogicalToDeviceUnits(40);
			}
		}

        /// <summary>
        /// Hides the DockPadding Property
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ScrollableControl.DockPaddingEdges DockPadding
        {
            get
            {
                return base.DockPadding;
            }
        }

    }
}
