using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.Common.UI.Windows
{
    
    public delegate void HeaderButtonClickEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Summary description for MenuBar.
    /// </summary>
	internal class MenuBar : System.Windows.Forms.UserControl
	{
		public int ButtonHeight = 77;
		private System.Windows.Forms.Button btnDown;
		private System.Windows.Forms.Button btnUp;
		private int _totalbuttonheight =0;
		private int _buttonhidden =0;
		private bool _downbutton = false;
		private bool _upbutton = false;
		private System.Windows.Forms.Button btnHeader;
		public string CodeLookup = "";

        private Size ButtonsUpDownSize => LogicalToDeviceUnits(new Size(19, 19)); 

		public event HeaderButtonClickEventHandler HeaderButtonClick;

		protected virtual void OnHeaderButtonClick(EventArgs e) 
		{
			if (HeaderButtonClick != null)
				HeaderButtonClick(this, e);
		}
		
		public MenuBar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			btnUp = new Button();
			btnUp.Name = "btnUp";
			btnUp.Size = ButtonsUpDownSize;
			btnUp.TabIndex = 4;
			btnUp.Click += new System.EventHandler(this.btnUp_Click);

			btnDown = new Button();
			btnDown.Name = "btnDown";
			btnDown.Size = ButtonsUpDownSize;
			btnDown.TabIndex = 4;
			btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // TODO: Add any initialization after the InitForm call

        }
        

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnHeader = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnHeader
			// 
			this.btnHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnHeader.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnHeader.Location = new System.Drawing.Point(0, 0);
			this.btnHeader.Name = "btnHeader";
			this.btnHeader.Size = new System.Drawing.Size(89, 24);
			this.btnHeader.TabIndex = 5;
			this.btnHeader.TabStop = false;
			this.btnHeader.Text = "Header";
			this.btnHeader.Click += new System.EventHandler(this.btnHeader_Click);
			// 
			// MenuBar
			// 
			this.Controls.Add(this.btnHeader);
			this.Name = "MenuBar";
			this.Size = new System.Drawing.Size(89, 101);
			this.Resize += new System.EventHandler(this.MenuBar_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		
		private void btnHeader_Click(object sender, System.EventArgs e)
		{
			OnHeaderButtonClick(EventArgs.Empty);
		}


		public string HeaderText
		{
			get
			{
				return btnHeader.Text;
			}
			set
			{
				btnHeader.Text = value;
			}
		}

		public bool DownButton
		{
			get
			{
				return _downbutton;

			}
			set
			{
				if (_downbutton != value)
				{
					_downbutton = value;
					if (value)
                    {
						btnDown.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
						btnDown.ImageList = Images.ScaleList(((MenuCollection)Parent).NavIcons, LogicalToDeviceUnits(new Size(16, 16)));
                        btnDown.ImageIndex = 3;
                        btnDown.Location = new System.Drawing.Point(this.Width - ButtonsUpDownSize.Width, this.Height - ButtonsUpDownSize.Height);
                        btnDown.Size = ButtonsUpDownSize;
                        this.Controls.Add(btnDown);
						this.Controls.SetChildIndex(btnDown,0);
					}
					else
					{
						this.Controls.Remove(btnDown);
					}
				}
			}
		}

		public bool UpButton
		{
			get
			{
				return _upbutton;

			}
			set
			{
				if (_upbutton != value)
				{
					_upbutton = value;
					if (value)
					{
                        btnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                        btnUp.ImageList = Images.ScaleList(((MenuCollection)Parent).NavIcons, LogicalToDeviceUnits(new Size(16, 16)));
                        btnUp.ImageIndex = 2;
						btnUp.Location = new System.Drawing.Point(this.Width - ButtonsUpDownSize.Width , btnHeader.Height);
                        btnUp.Size = ButtonsUpDownSize;
                        this.Controls.Add(btnUp);
						this.Controls.SetChildIndex(btnUp,0);
					}
					else
					{
						this.Controls.Remove(btnUp);
					}
				}
			}
		}

		public void HideButton()
		{
			btnDown_Click(null,System.EventArgs.Empty);
		}

		private void btnDown_Click(object sender, System.EventArgs e)
		{
			for(int t = this.Controls.Count-1; this.Controls.Count > 0; t--)
			{
				if (this.Controls[t] is MenuButton)
				{
					if (this.Controls[t].Height > 0)
					{
						this.Controls[t].Height = 0;
						_buttonhidden++;
						_totalbuttonheight =_totalbuttonheight - ButtonHeight;
						this.UpButton= true;
						if (_totalbuttonheight <= this.Height - ButtonHeight)
							this.DownButton = false;
						break;
					}
				}
			}
		}

		public void ShowButton()
		{
			btnUp_Click(null,System.EventArgs.Empty);
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			for(int t = 0; t < this.Controls.Count ; t++)
			{
				if (this.Controls[t] is MenuButton)
				{
					if (this.Controls[t].Height == 0)
					{
						this.Controls[t].Height = ButtonHeight;
						_totalbuttonheight =_totalbuttonheight + ButtonHeight;
						_buttonhidden--;
						if (_totalbuttonheight >= this.Height - ButtonHeight)
							this.DownButton = true;
						if (_buttonhidden==0) 
							this.UpButton=false;
						break;
					}
				}
			}
		}

		private void MenuBar_Resize(object sender, System.EventArgs e)
		{
			if (this.Height <= (btnHeader.Height + btnDown.Height))
			{
                btnDown.Visible = false;
			}
			else
			{
                btnDown.Visible = true;
			}		
		}

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            btnUp.Location = new System.Drawing.Point(this.Width - ButtonsUpDownSize.Width, btnHeader.Height);
            btnDown.Location = new System.Drawing.Point(this.Width - ButtonsUpDownSize.Width, this.Height - ButtonsUpDownSize.Height);
            btnUp.ImageList = btnDown.ImageList = Images.ScaleList(((MenuCollection) Parent).NavIcons, LogicalToDeviceUnits(new Size(16, 16)));
        }

        [Browsable(false)]
		public int HiddenButtons
		{
			get
			{
				return _buttonhidden;
			}
		}

		[Browsable(false)]
		public int TotalButtonHeight
		{
			get
			{
				return _totalbuttonheight;
			}
			set
			{
				_totalbuttonheight = value;
			}
		}
	}

}
