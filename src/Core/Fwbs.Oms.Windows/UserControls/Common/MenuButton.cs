using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for MenuButton.
    /// </summary>
    public delegate void ButtonEventHandler(object sender, MenuButtonEventArgs e);
	public class MenuButtonEventArgs : EventArgs 
	{  
		private string _returnkey;
		private string _caption;
		private string _codelookup;
		private int  _imageindex;
		private bool _include;


		public MenuButtonEventArgs(string ReturnKey, string ButtonCaption, int ImageIndex, bool IncFavorites, string ButtonName) 
		{
			_returnkey = ReturnKey;
			_caption = ButtonCaption;
			_imageindex = ImageIndex;
			_include = IncFavorites;
			_codelookup = ButtonName;
		}

		public object ReturnKey
		{
			get
			{
				return _returnkey;
			}
		}
		public string ButtonCaption
		{
			get
			{
				return _caption;
			}
		}
		public int ImageIndex
		{
			get
			{
				return _imageindex;
			}
		}
		public bool IncFavorites
		{
			get
			{
				return _include;
			}
		}
		public string ButtonName
		{
			get
			{
				return _codelookup;
			}
		}
	}

	public class MenuButton : System.Windows.Forms.UserControl
	{
		private int _imageindex;
		private string _returnkey;
		private bool _include;
		private int _clicked;
		private bool _visibleinsidebar;
		private bool _hover = false;
		private bool _toggle = false;
		private ButtonStyle _buttonstyle;
		private bool _allowtoggle = false;

		public string CodeLookup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label pictureBox1;
		private System.Windows.Forms.Panel pnlBig;
		private System.Windows.Forms.Panel pnlSmall;

		public new event ButtonEventHandler DoubleClick;
		public new event ButtonEventHandler Click;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected virtual void OnClick(MenuButtonEventArgs e) 
		{
			if (Click != null)
				Click(this, e);
		}

		protected virtual void OnDoubleClick(MenuButtonEventArgs e) 
		{
			if (DoubleClick != null)
				DoubleClick(this, e);
		}

		public MenuButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
			
		public MenuButton(string Caption, string ReturnKey, int ImageIndex, bool Include, bool VisibleInSideBar, string Codelookup, ButtonStyle buttonStyle)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			// TODO: Add any initialization after the InitForm call
			_imageindex = ImageIndex;
			ButtonText = Caption;
			_returnkey = ReturnKey;
			_visibleinsidebar = VisibleInSideBar;
			_include = Include;
			CodeLookup = Codelookup;
			ButtonStyle = buttonStyle;

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
			this.pnlBig = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.Label();
			this.pnlSmall = new System.Windows.Forms.Panel();
			this.pnlBig.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBig
			// 
			this.pnlBig.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.label1,
																				 this.panel1});
			this.pnlBig.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlBig.Name = "pnlBig";
			this.pnlBig.Size = new System.Drawing.Size(155, 62);
			this.pnlBig.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 45);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(155, 14);
			this.label1.TabIndex = 2;
			this.label1.Text = "This is a Test";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label1.Click += new System.EventHandler(this.pictureBox1_Click);
			this.label1.DoubleClick += new System.EventHandler(this.pnlSmall_DoubleClick);
			this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSmall_MouseDown);
			// 
			// panel1
			// 
			this.panel1.Controls.AddRange(new System.Windows.Forms.Control[] {
																				 this.pictureBox1});
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(155, 45);
			this.panel1.TabIndex = 3;
			this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
			this.pictureBox1.Location = new System.Drawing.Point(58, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(38, 38);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
			this.pictureBox1.DoubleClick += new System.EventHandler(this.pnlSmall_DoubleClick);
			this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
			this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSmall_MouseDown);
			// 
			// pnlSmall
			// 
			this.pnlSmall.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSmall.DockPadding.Bottom = 4;
			this.pnlSmall.DockPadding.Left = 5;
			this.pnlSmall.DockPadding.Right = 5;
			this.pnlSmall.DockPadding.Top = 4;
			this.pnlSmall.Location = new System.Drawing.Point(0, 62);
			this.pnlSmall.Name = "pnlSmall";
			this.pnlSmall.Size = new System.Drawing.Size(155, 21);
			this.pnlSmall.TabIndex = 3;
			this.pnlSmall.Click += new System.EventHandler(this.pictureBox1_Click);
			this.pnlSmall.Resize += new System.EventHandler(this.pnlSmall_Resize);
			this.pnlSmall.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlSmall_MouseUp);
			this.pnlSmall.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSmall_Paint);
			this.pnlSmall.MouseEnter += new System.EventHandler(this.pnlSmall_MouseEnter);
			this.pnlSmall.DoubleClick += new System.EventHandler(this.pnlSmall_DoubleClick);
			this.pnlSmall.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlSmall_MouseMove);
			this.pnlSmall.MouseLeave += new System.EventHandler(this.pnlSmall_MouseLeave);
			this.pnlSmall.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlSmall_MouseDown);
			// 
			// MenuButton
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.pnlSmall,
																		  this.pnlBig});
			this.Name = "MenuButton";
			this.Size = new System.Drawing.Size(155, 84);
			this.Resize += new System.EventHandler(this.MenuButton_Resize);
			this.Load += new System.EventHandler(this.MenuButton_Load);
			this.pnlBig.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void pictureBox1_MouseEnter(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				pictureBox1.BackColor = System.Drawing.Color.LightSteelBlue;
				pictureBox1.BorderStyle = BorderStyle.FixedSingle;
			}
		}

		private void pictureBox1_MouseLeave(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				pictureBox1.BackColor = System.Drawing.SystemColors.Control;
				pictureBox1.BorderStyle = BorderStyle.None;
			}
		}

		private void MenuButton_Resize(object sender, System.EventArgs e)
		{
			pictureBox1.Left = (this.Width - pictureBox1.Width) / 2;
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				MenuButtonEventArgs ee = new MenuButtonEventArgs(_returnkey,label1.Text,_imageindex,_include,CodeLookup);
				OnClick(ee);
			}
		}

		private void MenuButton_Load(object sender, System.EventArgs e)
		{
		
		}

		private void panel1_SizeChanged(object sender, System.EventArgs e)
		{
			pictureBox1.Left = (panel1.Width - pictureBox1.Width) / 2;
		}

		private void pnlSmall_MouseEnter(object sender, System.EventArgs e)
		{
			_hover = true;
			pnlSmall.Invalidate();
		}

		private void pnlSmall_MouseLeave(object sender, System.EventArgs e)
		{
			_hover = false;
			if (_allowtoggle == false) _toggle=false;
			pnlSmall.Invalidate();
		}

		private void pnlSmall_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (pnlSmall.Visible)
			{
				int offsetX = _toggle ? 1 : 0;
				int offsetY = _toggle ? 1 : 0;
			
				if (_hover || _toggle)
                    ControlPaint.DrawBorder3D(
                        e.Graphics, 
                        1, 1, pnlSmall.Width - 1,  pnlSmall.Height - 1, 
                        _toggle ? Border3DStyle.SunkenOuter : Border3DStyle.RaisedInner, 
                        Border3DSide.All);

                if (!_hover && _toggle)
                    e.Graphics.FillRectangle(
                        SystemBrushes.ControlLight,
                        LogicalToDeviceUnits(2), 
                        LogicalToDeviceUnits(2), 
                        pnlSmall.Width - LogicalToDeviceUnits(4), 
                        pnlSmall.Height - LogicalToDeviceUnits(4));
                        
			
				if (pictureBox1.ImageList != null)
                    e.Graphics.DrawImage(
                        pictureBox1.ImageList.Images[pictureBox1.ImageIndex],
                        LogicalToDeviceUnits(5 + offsetX),
                        LogicalToDeviceUnits(3 + offsetY),
                        LogicalToDeviceUnits(16),
                        LogicalToDeviceUnits(16));

                using (SolidBrush b1 = new SolidBrush(label1.ForeColor))
                {
                    e.Graphics.DrawString(label1.Text, label1.Font, b1,
                        new Rectangle(
                            LogicalToDeviceUnits(23 + offsetX), 
                            LogicalToDeviceUnits(3 + offsetY), 
                            pnlSmall.Width - LogicalToDeviceUnits(22), 
                            Convert.ToInt32(label1.Font.GetHeight() + LogicalToDeviceUnits(2))));
                }
			}
		}

		private void pnlSmall_Resize(object sender, System.EventArgs e)
		{
			pnlSmall.Invalidate();
		}

		private void pnlSmall_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_allowtoggle)
			{
				this.Toggle = true;
				if (!DesignMode)
				{
					MenuButtonEventArgs ee = new MenuButtonEventArgs(_returnkey,label1.Text,_imageindex,_include,CodeLookup);
					OnClick(ee);
				}
			}
			else
			{
				_toggle = true;
				pnlSmall.Invalidate();
			}
			this.OnMouseDown(e);
		}

		private void pnlSmall_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_allowtoggle == false)
			{
				_toggle = false;
				pnlSmall.Invalidate();
			}
			this.OnMouseUp(e);
		}

		private void pnlSmall_DoubleClick(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				MenuButtonEventArgs ee = new MenuButtonEventArgs(_returnkey,label1.Text,_imageindex,_include,CodeLookup);
				OnDoubleClick(ee);
			}
		}

		private void pnlSmall_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		public ButtonStyle ButtonStyle
		{
			get
			{
				return _buttonstyle;
			}
			set
			{
				_buttonstyle = value;
				if (_buttonstyle == ButtonStyle.mbSmall)
				{
					pnlSmall.Visible=true;
					pnlBig.Visible = false;
					this.Height = pnlSmall.Height;
					_allowtoggle = false;
				}
				else if (_buttonstyle == ButtonStyle.mbLarge)
				{
					pnlSmall.Visible=false;
					pnlBig.Visible = true  ;
					this.Height = pnlBig.Height;
				}
				else if (_buttonstyle == ButtonStyle.mbSmallToggle)
				{
					pnlSmall.Visible=true;
					pnlBig.Visible = false;
					this.Height = pnlSmall.Height;
					_allowtoggle = true;
				}
			}
		}

		public bool VisibleInSideBar
		{
			get
			{
				return _visibleinsidebar;
			}
			set
			{
				_visibleinsidebar = value;
			}
		}
		
		public bool Include
		{
			get
			{
				return _include;
			}
			set
			{
				_include = value;
			}
		}

		public int Clicked
		{
			get
			{
				return _clicked;
			}
			set
			{
				_clicked = value;
			}
		}

		public string ReturnKey
		{
			get
			{
				return _returnkey;
			}
			set
			{
				_returnkey = value;
			}
		}

		public bool Toggle
		{
			get
			{
				return _toggle;
			}
			set
			{
				if (_toggle != value && _allowtoggle)
				{
					_toggle = value;
					pnlSmall.Invalidate();
				}
			}
		}
		
		public string ButtonText
		{
			get
			{
				return label1.Text;
			}
			set
			{
				label1.Text = value;
			}
		}

		public int ButtonImageIndex
		{
			get
			{
				return _imageindex;
			}
			set
			{
				pictureBox1.ImageIndex = value;
				_imageindex = value;
				pictureBox1.Refresh();
				pnlSmall.Invalidate();
			}
		}

		public ImageList ButtonImageList
		{
			set
			{
				pictureBox1.ImageList = value;
				if (value != null)
					pictureBox1.ImageIndex = _imageindex;
				pictureBox1.Refresh();
				pnlSmall.Invalidate();
			}
		}
	}
}
