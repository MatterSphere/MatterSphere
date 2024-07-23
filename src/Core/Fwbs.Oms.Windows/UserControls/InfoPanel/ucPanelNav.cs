using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucPanelNav.
    /// </summary>

    public delegate void OpenCloseEventHandler(object sender, System.EventArgs e);

	[Designer(typeof(FWBS.OMS.UI.Windows.Design.PanelDesigner))]
	public class ucPanelNav : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Timer timFadeOut;
		private System.Windows.Forms.Timer timFadeIn;
		private System.ComponentModel.IContainer components;
        private bool _animating = false;
		public bool LoadFired = false;
		public Control pContainer = null;
		protected internal System.Windows.Forms.PictureBox labHeader;
        protected internal System.Windows.Forms.Label modernHeader;
        protected Color _shrinkstart;
		protected ExtColorTheme _theme;
		protected ExtColor ec;
		public event OpenCloseEventHandler OpenClose;

		protected ImageList _images;
		protected int _originalsize = 146;
		protected int _z = 0;
		protected float _czr = 0;
		protected float _czg = 0;
		protected float _czb = 0;

		protected System.Drawing.Color _blendcolor1 = Color.Empty;
		protected System.Drawing.Color _blendcolor2= Color.Empty;
		protected System.Drawing.Color _cblendcolor1= Color.Empty;
		protected System.Drawing.Color _cblendcolor2= Color.Empty;
		protected int _hdbrightness = -10;

		protected float _zzr = 0;
		protected float _zzg = 0;
		protected float _zzb = 0;

		protected bool _wz = false;
		protected bool _lockopenclose = false;
		protected bool _expanded = true;
		protected int _increasebrightness = 105;
		protected NavButtonStyle _navstyle = NavButtonStyle.Grey;

		protected string _headertext = "";
		protected System.Drawing.Color _originalcolor;
		protected internal System.Windows.Forms.Panel pnlSpace;
		protected System.Drawing.Color _headercolor = SystemColors.Highlight;
		protected System.Drawing.Color _sheadercolor = Color.Empty;
		protected System.Drawing.Color _gradientcolor;

		private bool _globalscope = false;
        private NavStyle _modernStyle = NavStyle.Classic;
        private string _globalname = String.Empty;
        protected Control _parent = null;

		#region Constructors
		public ucPanelNav(bool NoHeader)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            _images = ExpandCollapseIconSelector.GetExpandCollapseIcons();
			this.ControlAdded += new ControlEventHandler(PanelAdded);

			// Get the ucPanelBack if the control is created at Design Time and assign it to the variable
			// pContainer
			//
			if (pContainer == null)
				foreach (Control ctl in this.Controls)
					if (ctl is ucNavPanel)
						pContainer = (ucNavPanel)ctl;
		}

		public ucPanelNav()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.TabStop = false;
			this.labHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.labHeader_Paint);
			this.labHeader.MouseEnter += new System.EventHandler(this.labHeader_MouseEnter);
			this.labHeader.MouseLeave += new System.EventHandler(this.labHeader_MouseLeave);
            _images = ExpandCollapseIconSelector.GetExpandCollapseIcons();
			this.ControlAdded += new ControlEventHandler(PanelAdded);

			// Get the ucPanelBack if the control is created at Design Time and assign it to the variable
			// pContainer
			//
			if (pContainer == null)
				foreach (Control ctl in this.Controls)
					if (ctl is ucNavPanel)
						pContainer = (ucNavPanel)ctl;
		}

		/// <summary>
		/// Code Created Control
		/// </summary>
		/// <param name="LabelHeader"></param>
		public ucPanelNav(string LabelHeader, String RichText, int Height, bool Expanded)
		{
			InitializeComponent();

			this.labHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.labHeader_Paint);
			this.labHeader.MouseEnter += new System.EventHandler(this.labHeader_MouseEnter);
			this.labHeader.MouseLeave += new System.EventHandler(this.labHeader_MouseLeave);
            _images = ExpandCollapseIconSelector.GetExpandCollapseIcons();

            // Set the Expanded Property
            this.Expanded = Expanded;

			// Create the ucNavPanel at Runtime
			this.pContainer = new ucNavRichText();
			this.pContainer.Location = new System.Drawing.Point(0,this.labHeader.Height);
			this.pContainer.Size = new System.Drawing.Size(this.Width,this.Height - this.labHeader.Height);
			this.pContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Controls.Add(this.pContainer);

			// If the Height is different from -1 which is Auto Expand set height 
			if (Height != -1) this.Height = Height;
			this.Text = LabelHeader;
			// 
			// innerrichbox
			// 
			if (pContainer is ucNavRichText)
			{
				ucNavRichText ptemp = (ucNavRichText)pContainer;
				try
				{
					ptemp.Rtf = RichText;
				}
				catch
				{
					ptemp.Text = RichText;
				}
			}
            var navPanel = pContainer as ucNavPanel;
            if (navPanel != null)
            {
                navPanel.ModernStyle = _modernStyle != NavStyle.Classic;
            }
			_originalsize = this.Height;
		}

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (factor.Height != 1 && (specified & BoundsSpecified.Height) != 0)
            {
                _originalsize = Convert.ToInt32(_originalsize * factor.Height);
            }
            base.ScaleControl(factor, specified);
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (_images != null)
                        _images.Dispose();

                    if (pContainer != null)
                        pContainer.Dispose();

                    if (_parent != null)
                    {
                        _parent.BackColorChanged -= new System.EventHandler(this.ucPanelNav_BackColorChanged);
                        _parent = null;
                    }

                    if (components != null)
                    {
                        components.Dispose();
                    }

                    for (int i = this.Controls.Count-1 ; i >= 0; i-- )
                    {
                        Control c = this.Controls[i];
                        if (!c.IsDisposed)
                            c.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timFadeOut = new System.Windows.Forms.Timer(this.components);
			this.timFadeIn = new System.Windows.Forms.Timer(this.components);
			this.labHeader = new System.Windows.Forms.PictureBox();
            this.modernHeader = new System.Windows.Forms.Label();
            this.pnlSpace = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// timFadeOut
			// 
			this.timFadeOut.Interval = 10;
			this.timFadeOut.Tick += new System.EventHandler(this.timFadeOut_Tick);
			// 
			// timFadeIn
			// 
			this.timFadeIn.Interval = 10;
			this.timFadeIn.Tick += new System.EventHandler(this.timFadeIn_Tick);
			// 
			// labHeader
			// 
			this.labHeader.BackColor = System.Drawing.Color.White;
			this.labHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.labHeader.Location = new System.Drawing.Point(0, 0);
			this.labHeader.Name = "labHeader";
			this.labHeader.Size = new System.Drawing.Size(137, 24);
			this.labHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.labHeader.TabIndex = 13;
			this.labHeader.TabStop = false;
			this.labHeader.Click += new System.EventHandler(this.labHeader_Click);
			this.labHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labHeader_MouseDown);
            // 
            // modernHeader
            //
            this.modernHeader.AutoSize = true;
            this.modernHeader.BackColor = System.Drawing.Color.Transparent;
            this.modernHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.modernHeader.ForeColor = Color.FromArgb(51, 51, 51);
            this.modernHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.modernHeader.Location = new System.Drawing.Point(0, 0);
            this.modernHeader.Name = "modernHeader";
            this.modernHeader.Padding = new System.Windows.Forms.Padding(3,0,0,0);
            this.modernHeader.TabIndex = 14;
            this.modernHeader.TabStop = false;
            this.modernHeader.Visible = false;
            // 
            // pnlSpace
            // 
            this.pnlSpace.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlSpace.Location = new System.Drawing.Point(0, 151);
			this.pnlSpace.Name = "pnlSpace";
			this.pnlSpace.Size = new System.Drawing.Size(137, 7);
			this.pnlSpace.TabIndex = 14;
			// 
			// ucPanelNav
			// 
			this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.Controls.Add(this.pnlSpace);
			this.Controls.Add(this.labHeader);
            this.Controls.Add(this.modernHeader);
            this.Name = "ucPanelNav";
			this.Size = new System.Drawing.Size(137, 158);
			this.SystemColorsChanged += new System.EventHandler(this.ucPanelNav_SystemColorsChanged);
			this.Load += new System.EventHandler(this.ucPanelNav_Load);
			this.SizeChanged += new System.EventHandler(this.ucPanelNav_SizeChanged);
			this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Private
		protected void PropertiesChanged()
		{
			PropertiesChanged(false);
		}

		protected virtual void PropertiesChanged(bool Fast)
		{
			if (LoadFired)
			{
                if (_theme != ExtColorTheme.None)
                {
                    ec = new ExtColor(ExtColorPresets.TaskPainBoxBackColor, _theme);
                    if (Parent != null)
                    {
                        Parent.BackColor = ec.TaskPainBackColor;
                        if (_parent != Parent) _parent = Parent;
                        if (_parent != null)
                        {
                            _parent.BackColorChanged -= new System.EventHandler(this.ucPanelNav_BackColorChanged);
                        }
                    }
                    _originalcolor = ec.TaskPainBoxBackColor;
                    if (ec["TaskPainBoxHeaderBlend1"] != Color.Empty)
                    {
                        _blendcolor1 = ec["TaskPainBoxHeaderBlend1"];
                        _blendcolor2 = ec["TaskPainBoxHeaderBlend2"];
                    }
                    _gradientcolor = ec.TaskPainBoxHeaderBackColor;
                    if (_sheadercolor == System.Drawing.Color.Empty)
                        _headercolor = ec.TaskPainBoxHeaderForeColor;
                    else
                        _headercolor = _sheadercolor;
                    this.BackColor = ec.TaskPainBoxBackColor;
                    this.ForeColor = ec.TaskPainForeColor;
                    this.labHeader.ForeColor = ec.TaskPainBoxHeaderForeColor;
                    this.pnlSpace.BackColor = ec.TaskPainBackColor;
                    this.Invalidate();
                }
                else
                {
                    if (Parent != null)
                    {
                        ucPanelNav_BackColorChanged(null, new EventArgs());
                        if (_parent != Parent) _parent = Parent;
                        if (_parent != null)
                        {
                            _parent.BackColorChanged += new System.EventHandler(this.ucPanelNav_BackColorChanged);
                        }
                    }
                }

				if (_expanded == false )
				{
					_shrinkstart = this.BackColor;

					_zzr = this.BackColor.R - Parent.BackColor.R;
					_zzb = this.BackColor.B - Parent.BackColor.B;
					_zzg = this.BackColor.G - Parent.BackColor.G;

					int size = (this.Height - (this.labHeader.Height + this.pnlSpace.Height)); 

					_czr = _zzr / size;
					_czb = _zzb / size;
					_czg = _zzg / size;

					_zzr = this.BackColor.R;
					_zzb = this.BackColor.B;
					_zzg = this.BackColor.G;
					if (Fast)
						ShrinkFast();
					else
						timFadeOut.Enabled=true;
				}
                if (_modernStyle != NavStyle.Classic)
                {
                    BackColor = Color.FromArgb(244, 244, 244);
                }
			}
		}

        private void ApplyVisualChanges()
        {
            if (_modernStyle != NavStyle.Classic)
            {
                this.BackColor = Color.FromArgb(244, 244, 244);
                this.ForeColor = Color.FromArgb(51, 51, 51);
                labHeader.Visible = false;
                modernHeader.Visible = (_modernStyle == NavStyle.ModernHeader);
            }
        }

        private void PanelAdded(object sender,ControlEventArgs e)
		{
			e.Control.BringToFront();
		}

		private void ucPanelNav_SizeChanged(object sender, System.EventArgs e)
		{
			if (DesignMode && _expanded && timFadeIn.Enabled==false && timFadeOut.Enabled==false)
			{
				ExpandedHeight = this.Height;
			}
		}

		private void ucPanelNav_BackColorChanged(object sender, System.EventArgs e)
		{
			if (Parent != null)
			{
				if (_modernStyle == NavStyle.Classic && _theme == ExtColorTheme.None)
				{
					this.BackColor = ChangeBrightness(Parent.BackColor,_increasebrightness);
					_originalcolor= this.BackColor;
					_gradientcolor = ChangeBrightness(_originalcolor,_hdbrightness);
					if (_sheadercolor == System.Drawing.Color.Empty)
						_headercolor = SystemColors.Highlight;
					else
						_headercolor = _sheadercolor;
					labHeader.BackColor = Parent.BackColor;
					pnlSpace.BackColor = Parent.BackColor;
				}
				else
				{
					this.Theme = _theme;
				}
			}
		}

		private void labHeader_Click(object sender, System.EventArgs e)
		{
			this.OnClick(e);
		}

		private void ucPanelNav_SystemColorsChanged(object sender, System.EventArgs e)
		{
			if (LoadFired == false) return;

			if (_theme != ExtColorTheme.None)
			{
				ec.RefreshColors();
				this.Theme = _theme;
				PropertiesChanged();
				this.Refresh();
			}
		}

		protected virtual void OnOpenClose(EventArgs e) 
		{
			if (OpenClose != null)
				OpenClose(this, e);
		}
		#endregion

		#region GDI Effects
		public void ExpandFast()
		{
			this.Height = this.ExpandedHeight;
		}

		public void ShrinkFast()
		{
			int h = this.ExpandedHeight;
			if (Parent != null)
			{
				
				while(true)
				{
					_expanded = false;
					if (h < labHeader.Height + pnlSpace.Height)
					{
						this.Height = pnlSpace.Height + labHeader.Height;
						labHeader.Invalidate();
						break;
					}

					h = h - _z;
				
					_zzr = _zzr - (_czr * _z);
					_zzg = _zzg - (_czg * _z);
					_zzb = _zzb - (_czb * _z);

					if (_wz == false)
					{
						_z++;
						_wz = true;
					}
					else
						_wz = false;
				}
			}
		}

		private void timFadeOut_Tick(object sender, System.EventArgs e)
		{
			timFadeOut.Enabled=false;
			if (Parent != null && this.Height > labHeader.Height + pnlSpace.Height)
			{
				timFadeIn.Enabled=false;
				this.Height = this.Height - _z;
				
				_zzr = _zzr - (_czr * _z);
				_zzg = _zzg - (_czg * _z);
				_zzb = _zzb - (_czb * _z);

                if (_zzr < 0) _zzr = 0;
                if (_zzg < 0) _zzg = 0;
                if (_zzb < 0) _zzb = 0;
                _animating = true;
                this.BackColor = Color.FromArgb((int)_zzr, (int)_zzg, (int)_zzb);

				if (_wz == false)
				{
					_z++;
					_wz = true;
				}
				else
					_wz = false;
				labHeader.Invalidate();
			}
			else
			{
				_expanded = false;
                _animating = false;
                this.Height = labHeader.Height + pnlSpace.Height;
				OnOpenClose(EventArgs.Empty);
				labHeader.Invalidate();
				return;
			}
			timFadeOut.Enabled=true;
		}

		private void timFadeIn_Tick(object sender, System.EventArgs e)
		{
			timFadeIn.Enabled=false;
			if (Parent != null && _expanded == false)
			{
				timFadeOut.Enabled=false;
				this.BackColor = _originalcolor;
				this.Height = this.Height + _z;
                _animating = true;
				if (_wz == false)
				{
					_z--;
					//DMB 11/8/2004 added a check to make sure number didn't go into minus and cause
					//control to disappear
					if(_z < 2)
						_z = 2;
					_wz = true;
				}
				else
					_wz = false;
				if (this.Height >= _originalsize)
				{
					this.Height = _originalsize;
					this.BackColor = _originalcolor;
					_expanded = true;
                    _animating = false;
					OnOpenClose(EventArgs.Empty);
					_z=0;
					labHeader.Invalidate();
					return;
				}
				labHeader.Invalidate();
			}
			timFadeIn.Enabled=true;
		}

		public void ToggleExpand()
		{
			labHeader_MouseDown(this,new System.Windows.Forms.MouseEventArgs(MouseButtons.Left,0,0,0,0));
		}


		private void labHeader_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_lockopenclose == false && e.Button == MouseButtons.Left)
			{
				if (Expanded)
				{
					_shrinkstart = this.BackColor;
					_zzr = this.BackColor.R - Parent.BackColor.R;
					_zzb = this.BackColor.B - Parent.BackColor.B;
					_zzg = this.BackColor.G - Parent.BackColor.G;

					int size = (this.Height - (this.labHeader.Height + this.pnlSpace.Height)); 

					_czr = _zzr / size;
					_czb = _zzb / size;
					_czg = _zzg / size;

					_zzr = this.BackColor.R;
					_zzb = this.BackColor.B;
					_zzg = this.BackColor.G;
					timFadeOut.Enabled=true;
				}
				else
					timFadeIn.Enabled=true;
			}
		}

		private void ucPanelNav_Load(object sender, System.EventArgs e)
		{
			this.pnlSpace.SendToBack();
			this.labHeader.SendToBack();
			LoadFired=true;
			PropertiesChanged(true);
		}
		
		private void labHeader_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (!LoadFired || Parent == null)
                return;

            if (_blendcolor1 == System.Drawing.Color.Empty) 
			{
				if (_theme == ExtColorTheme.None)
					_cblendcolor1 = Color.White;
				else
					_cblendcolor1 = Color.White;
			}
			else 
				_cblendcolor1 =_blendcolor1;

			if (_blendcolor2 == System.Drawing.Color.Empty) 
			{
				if (_theme == ExtColorTheme.None)
					_cblendcolor2 = _gradientcolor;  
				else
					_cblendcolor2 = ec.TaskPainBoxHeaderBlend;
			}
			else 
				_cblendcolor2 =_blendcolor2;

			using (System.Drawing.Drawing2D.LinearGradientBrush b1 = new System.Drawing.Drawing2D.LinearGradientBrush(new Point(0,0),new Point((int)(this.labHeader.Width / 2),0),_cblendcolor1,_cblendcolor2))
            {
                e.Graphics.FillRectangle(b1, (int)(this.labHeader.Width / 2), 0, this.labHeader.Width, this.labHeader.Height);
            }
			using (SolidBrush b2 = new System.Drawing.SolidBrush(_cblendcolor1))
            {
                e.Graphics.FillRectangle(b2,0,0,(int)(this.labHeader.Width / 2)+1,this.labHeader.Height);
            }
			using(SolidBrush b3 = new System.Drawing.SolidBrush(_cblendcolor2))
            {
                e.Graphics.FillRectangle(b3,this.labHeader.Width-2,0,2,this.labHeader.Height);
            }
            using (Font f = new Font(CurrentUIVersion.Font, CurrentUIVersion.FontSize * this.DeviceDpi / e.Graphics.DpiX, FontStyle.Bold))
            {
                this._headercolor = Color.Black;

                using (SolidBrush b4 = new SolidBrush(_headercolor))
                {
                    Rectangle rect = Rectangle.FromLTRB(3, 0, this.labHeader.Width - 3, this.labHeader.Height);
                    StringFormat fmt = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center };
                    if (this.RightToLeft == RightToLeft.Yes)
                    {
                        fmt.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    }
                    e.Graphics.DrawString(_headertext, f, b4, rect, fmt);
                }
            }

            using (Pen p1 = new Pen(Parent.BackColor))
            {
                e.Graphics.DrawLine(p1, 0, 0, 1, 0);
                e.Graphics.DrawLine(p1, 0, 0, 0, 1);
                int w = this.labHeader.Width;
                e.Graphics.DrawLine(p1, w, 0, w - 2, 0); //Across
                e.Graphics.DrawLine(p1, w - 1, 0, w - 1, 1);   //Down
            }

            try
			{
                var image = _images.Images[(_expanded ? 0 : 1) + (int)_navstyle];
                float imgWidth = LogicalToDeviceUnits(image.Width);
                float imgHeight = LogicalToDeviceUnits(image.Height);
                Rectangle rect = Rectangle.FromLTRB(3, 0, this.labHeader.Width - 3, this.labHeader.Height);

                if (this.RightToLeft == RightToLeft.Yes)
                    e.Graphics.DrawImage(image, rect.Left, rect.Top + (rect.Height - imgHeight)/2, imgWidth, imgHeight);
                else
					e.Graphics.DrawImage(image, rect.Right - imgWidth, rect.Top + (rect.Height - imgHeight) / 2, imgWidth, imgHeight);
			}
			catch{}
        }

		private void labHeader_MouseLeave(object sender, System.EventArgs e)
		{
            _headercolor = Color.Black;
            labHeader.Invalidate();
		}

		private void labHeader_MouseEnter(object sender, System.EventArgs e)
		{
            _headercolor = Color.Black;
            labHeader.Invalidate();
		}
		#endregion

		#region Properties
        public bool Animating
        {
            get { return _animating; }
        }

        [DefaultValue(NavStyle.Classic)]
        [Browsable(false)]
        public NavStyle ModernStyle
        {
            get
            {
                return _modernStyle;
            }
            set
            {
                _modernStyle = value;
                ApplyVisualChanges();
            }
        }

        public enum NavStyle
        {
            Classic,
            NoHeader,
            ModernHeader
        }

        [DefaultValue(false)]
		public bool GlobalScope
		{
			get
			{
				return _globalscope;
			}
			set
			{
				_globalscope = value;
			}
		}


		[Browsable(false)]
		public override System.Drawing.Color BackColor
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

		[Category("Header")]
		[DefaultValue(typeof(System.Drawing.Color),"Empty")]
		public virtual System.Drawing.Color HeaderColor
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

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool Expanded
		{
			get
			{
				return _expanded;
			}
			set
			{
				if (_expanded != value)
				{
					_expanded = value;
					PropertiesChanged();
				}
			}
		}
		
		[Browsable(false)]
		new public Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}

		}

		[Category("Appearance")]
		[DefaultValue(146)]
		public int ExpandedHeight
		{
			get
			{
				return _originalsize;
			}
			set
			{
				_originalsize = value;
			}
		}
		
		[Category("Appearance")]
		[DefaultValue("Header Text")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return _headertext;
			}
			set
			{
				if (_headertext != value)
				{
					_headertext = value;
                    modernHeader.Text = value;
					PropertiesChanged();
					OnTextChanged(new EventArgs());
					labHeader.Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool LockOpenClose
		{
			get
			{
				return _lockopenclose;
			}
			set
			{
				_lockopenclose = value;
			}
		}
		
		[Category("Appearance")]
		[DefaultValue(NavButtonStyle.Grey)]
		public NavButtonStyle ButtonStyle
		{
			get
			{
				return _navstyle;
			}
			set
			{
				if (_navstyle != value)
				{
					_navstyle = value;
					PropertiesChanged();
				}
			}
		}

		[Category("Theme")]
		[DefaultValue(ExtColorTheme.None)]
        [Browsable(false)]
        public ExtColorTheme Theme
		{
			get
			{
				return _theme;
			}
			set
			{
				if (_theme != value)
				{
					_theme=value;
					PropertiesChanged();
				}
			}
		}


		[Category("Appearance")]
		[DefaultValue(105)]
		public int Brightness
		{
			get
			{
				return _increasebrightness;
			}
			set
			{
				if (_theme == ExtColorTheme.None)
				{
					if (_increasebrightness != value)
					{
						_increasebrightness = value;
						PropertiesChanged();
					}
				}
			}
		}

		[Category("Header")]
		[DefaultValue(-10)]
		public virtual int HeaderBrightness
		{
			get
			{
				return _hdbrightness;
			}
			set
			{
				if (_hdbrightness != value)
				{
					_hdbrightness = value;
					PropertiesChanged();
				}
			}
		}

		[Category("Header")]
		[DefaultValue(typeof(System.Drawing.Color),"Empty")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Drawing.Color BlendColor1
		{
			get
			{
				return _blendcolor1;
			}
			set
			{
				if (_blendcolor1 != value && Theme == ExtColorTheme.None)
				{
					_blendcolor1 = value;
					PropertiesChanged();
				}
			}
		}

		[Category("Header")]
		[DefaultValue(typeof(System.Drawing.Color),"Empty")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Drawing.Color BlendColor2
		{
			get
			{
				return _blendcolor2;
			}
			set
			{
                if (_blendcolor1 != value && Theme == ExtColorTheme.None)
                {
					_blendcolor2 = value;
					PropertiesChanged();
				}
			}
		}
        #endregion

        #region Color Functions

        public void SetBackColor(Color color)
        {
            if (this.Controls[0].Controls.Count > 0)
            {
                this.Controls[0].Controls[0].BackColor = color;
            }
        }

        public static System.Drawing.Color ChangeBrightness(System.Drawing.Color FromColor, Single Brightness)
		{
			HSL b = RGBtoHSL(FromColor.R,FromColor.G,FromColor.B);
			b.luminance = b.luminance + Brightness;
			return HSLtoRGB(b.hue,b.saturation,b.luminance);
		}

		public static HSL RGBtoHSL(Single Red, Single Green, Single Blue)
		{
			Single pRed;
			Single pGreen;
			Single pBlue;
			HSL RetVal = new HSL(0,0,0);
			Single pMax =255;
			Single pMin =0;
			Single pLum =0;
			Single pSat =0;
			Single pHue =0;

			pRed = Red / 255;
			pGreen = Green / 255;
			pBlue = Blue / 255;

			if (pRed > pGreen) 
				if (pRed > pBlue)
					pMax = pRed;
				else
					pMax = pBlue;
			else if (pGreen > pBlue)
				pMax = pGreen;
			else
				pMax = pBlue;

			if (pRed < pGreen)
				if (pRed < pBlue)
					pMin = pRed;
				else
					pMin = pBlue;
			else if (pGreen < pBlue)
				pMin = pGreen;
			else
				pMin = pBlue;
    
			pLum = (pMax + pMin) / 2;

			if (pMax == pMin)
			{
				pSat = 0;
				pHue = 0;
			}
			else
			{
				if (pLum < 0.5)
					pSat = (pMax - pMin) / (pMax + pMin);
				else
					pSat = (pMax - pMin) / (2 - pMax - pMin);
        
				if (pMax == pRed)
					pHue = (pGreen - pBlue) / (pMax - pMin);
				if (pMax == pGreen)
					pHue = 2 + (pBlue - pRed) / (pMax - pMin);
				if (pMax == pBlue)
					pHue = 4 + (pRed - pGreen) / (pMax - pMin);
			}

			RetVal.hue = (int)(pHue * 239 / 6);
			if (RetVal.hue < 0) RetVal.hue = RetVal.hue + 240;
    
			RetVal.saturation = (int)(pSat * 239);
			RetVal.luminance = (int)(pLum * 239);
			return RetVal;
		}

		public static System.Drawing.Color HSLtoRGB(Single Hue, Single Saturation, Single Luminance)
		{
			Single pHue = 0;
			Single pSat = 0;
			Single pLum = 0;
			System.Drawing.Color RetVal;
			Single pRed = 0;
			Single pGreen = 0;
			Single pBlue = 0;
			Single temp2 = 0;
			Single[] temp3 = new Single[3];
			Single temp1 = 0;

			pHue = Hue / 239;
			pSat = Saturation / 239;
			pLum = Luminance / 239;

			if (pSat == 0)
			{
				pRed = pLum;
				pGreen = pLum;
				pBlue = pLum;
			}
			else
			{
				if (pLum < 0.5)
					temp2 = pLum * (1 + pSat);
				else
					temp2 = pLum + pSat - pLum * pSat;
				temp1 = 2 * pLum - temp2;

				temp3[0] = pHue + ((Single)1 / (Single)3);
				temp3[1] = pHue;
				temp3[2] = pHue - ((Single)1 / (Single)3);
				for (int n = 0; n < 3; n++)
				{
					if (temp3[n] < 0) temp3[n] = temp3[n] + 1;
					if (temp3[n] > 1) temp3[n] = temp3[n] - 1;

					if (6 * temp3[n] < 1)
						temp3[n] = temp1 + (temp2 - temp1) * 6 * temp3[n];
					else
					{
						if (2 * temp3[n] < 1)
							temp3[n] = temp2;
						else
						{
							if (3 * temp3[n] < 2)
								temp3[n] = temp1 + (temp2 - temp1) * (((Single)2 / (Single)3) - temp3[n]) * 6;
							else
								temp3[n] = temp1;
						}
					}

				}
				pRed = temp3[0];
				pGreen = temp3[1];
				pBlue = temp3[2];
			}
			int fRed = (int)(pRed * 255)-7;
			int fGreen = (int)(pGreen * 255)-7;
			int fBlue = (int)(pBlue * 255)-7;

			if (fRed > 255) {fRed=255;}
			if (fGreen > 255) {fGreen=255;}
			if (fBlue > 255) {fBlue=255;}

			if (fRed < 0) {fRed=0;}
			if (fGreen < 0) {fGreen=0;}
			if (fBlue < 0) {fBlue=0;}

			RetVal = Color.FromArgb(fRed,fGreen,fBlue);
			return RetVal;
		}
	}

	public class HSL
	{
		public Single hue;
		public Single saturation;
		public Single luminance;
		public HSL(Single Hue, Single Saturation, Single Luminance)
		{
			hue = Hue;
			saturation = Saturation;
			luminance =Luminance;
		}
	}
	#endregion
}
