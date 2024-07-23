using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucMenuContainer.
    /// </summary>
    [System.ComponentModel.Designer(typeof(ucMenuContainerDesigner))]
	public class ucMenuContainer : System.Windows.Forms.Panel
	{
		#region Fields

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private bool _captionvisible = true;
		private string _caption = "Menu Title";
		private Color _selectedcolor = SystemColors.Control;
		private int _menuHeaderWidth = 0;
		private ucMenuContainer _parentmenu;
		private ucMenuContainer _popout;
		private bool _begingroup = false;
		public ucMenuSplit Split = null;
		private ucMenuItem _parentpopmenu = null;
		private int DefaultHeight { get { return LogicalToDeviceUnits(24); } }
        #endregion

		#region Constructors
		public ucMenuContainer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            Height = DefaultHeight;
            base.DockPadding.All = 1;
			base.DockPadding.Top += Height;
        }
        #endregion

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            base.DockPadding.Top = _captionvisible ? DefaultHeight + 1 : 1;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // 
            // ucMenuContainer
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Size = new System.Drawing.Size(150, 24);
            this.FontChanged += new System.EventHandler(this.ucMenuContainer_FontChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ucMenuContainer_Paint);
            this.Resize += new System.EventHandler(this.ucMenuContainer_Resize);

		}
        #endregion

        #region Private
        private void ucMenuContainer_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_captionvisible)
			{
                // ********************************************************************************************
                // * Sets the Text Format Settings
                // ********************************************************************************************
                StringFormat sf = new StringFormat(StringFormatFlags.NoWrap)
                {
                    HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                }

                // ********************************************************************************************
                int height = LogicalToDeviceUnits(24);

                using (SolidBrush br = new SolidBrush(this.ForeColor))
                {
                    e.Graphics.DrawString(_caption, this.Font, br, Rectangle.FromLTRB(0, 0, _menuHeaderWidth, height), sf);
                }

                using (Pen pen = new Pen(_selectedcolor, 1))
                {
                    if (this.Controls.Count == 0 || IsShrunk)
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, _menuHeaderWidth, height - 1); // Draw Rectangle around the Menu Caption
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(pen, 0, 0, _menuHeaderWidth, height); // Draw Rectangle around the Menu Caption
                        e.Graphics.DrawRectangle(pen, 0, height, this.Width - 1, this.Height - height - 1); // Draws a Rectangle around the Menu Items
                        e.Graphics.DrawLine(SystemPens.Control, 1, height, _menuHeaderWidth - 1, height); // Draw a Line underneath the Menu Caption
                    }
                }
                sf.Dispose();
			}
			else
			{
				e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.Width-1, this.Height-1); // Draws a Rectangle around the Menu Items
			}
		}

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
        
        private void ucMenuContainer_FontChanged(object sender, EventArgs e)
        {
            _menuHeaderWidth = MeasureDisplayStringWidth(_caption, this.Font);
        }

        private void ucMenuContainer_Resize(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		#endregion

		#region Properties
		[DefaultValue(true)]
		[Category("Appearance")]
		public bool CaptionVisible
		{
			get
			{
				return _captionvisible;
			}
			set
			{
				_captionvisible = value;
				base.DockPadding.Top = value ? DefaultHeight + 1 : 1;
				this.Invalidate();
			}
		}

		[DefaultValue("Menu Title")]
		[Category("Appearance")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public new string Text
		{
			get
			{
				return _caption;
			}
			set
			{
                _caption = value;
                _menuHeaderWidth = MeasureDisplayStringWidth(_caption, this.Font);
                if (_menuHeaderWidth > this.Width) this.Width = _menuHeaderWidth + LogicalToDeviceUnits(20);
				this.Invalidate();
				this.Refresh();
				this.OnTextChanged(EventArgs.Empty);
			}
		}

		[Category("Appearance")]
		[Browsable(false)]
		public bool BeginGroup
		{
			get
			{
				return _begingroup;
			}
			set
			{
				_begingroup = value;
			}
		}

		[Category("Appearance")]
		public bool Selected
		{
			get
			{
				return (_selectedcolor == Color.Gray);
			}
			set
			{
				_selectedcolor = value ? Color.Gray : SystemColors.Control;
				this.Invalidate();
			}
		}

		[Browsable(false)]
		public int HeaderTextWidth
		{
			get
			{
				return _menuHeaderWidth;
			}
		}
		
		[Browsable(false)]
		public ucMenuContainer ParentMenu
		{
			get
			{
				return _parentmenu;
			}
			set
			{
				_parentmenu = value;
			}
		}

		[Browsable(false)]
		public ucMenuItem ParentPopoutMenu
		{
			get
			{
				return _parentpopmenu;
			}
			set
			{
				_parentpopmenu = value;
			}
		}

		[Browsable(false)]
		public ucMenuContainer PopoutMenu
		{
			get
			{
				return _popout;
			}
			set
			{
				_popout = value;
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

		#endregion

		#region Methods
		public void Expand()
		{
            if (this.Controls.Count > 0)
            {
                int height = this.CaptionVisible ? DefaultHeight + 2 : 2;
                foreach (Control c in this.Controls)
                {
                    if (c.Visible) height += c.Height;
                    c.Dock = DockStyle.Top;
                }
                this.Height = height;
            }
            else
            {
                this.Height = DefaultHeight;
            }
		}

		public ucMenuItem AddMenuItem()
		{
			ucMenuItem _menu = new ucMenuItem();
			_menu.Dock = DockStyle.Top;
			_menu.ParentMenu = this;
			this.Controls.Add(_menu);
			_menu.BringToFront();
			return _menu;
		}
		
		public void Shrink()
		{
			this.Height = DefaultHeight;
		}

		public bool IsShrunk
		{
			get
			{
				return (this.Height == DefaultHeight);
			}
		}

		#endregion
	}

	/// <summary>
	/// Fixed Control Size in Visual Studio
	/// </summary>
	internal class ucMenuContainerDesigner : System.Windows.Forms.Design.ParentControlDesigner
	{
		public override SelectionRules SelectionRules 
		{
			get
			{
				if (this.Control != null)
				{
					int h = Control.LogicalToDeviceUnits(24) + 2;
					foreach (Control c in this.Control.Controls)
					{
						h += c.Height;
						c.Dock = DockStyle.Top;
					}
					this.Control.Height = h;
				}
				return SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.RightSizeable | SelectionRules.LeftSizeable;
			}
		}

	}
}
