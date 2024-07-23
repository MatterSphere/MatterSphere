using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmLaserDialog.
    /// </summary>
    public class frmLaserDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton btnSave;
		private System.Windows.Forms.ToolBarButton btnClose;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;
        private ToolBarButton btnSaveAs;
		private string _caption;//has to be caption as index value can change as objects are removed from collection

		public event EventHandler OnSave;
		public event EventHandler OnClose;
        public event EventHandler OnSaveAs;
        
		public frmLaserDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLaserDialog));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.btnSave = new System.Windows.Forms.ToolBarButton();
            this.btnClose = new System.Windows.Forms.ToolBarButton();
            this.btnSaveAs = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.btnSave,
            this.btnClose,
            this.btnSaveAs});
            this.toolBar1.ButtonSize = new System.Drawing.Size(20, 20);
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(72, 28);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            this.toolBar1.LostFocus += new System.EventHandler(this.toolBar1_LostFocus);
            this.toolBar1.MouseHover += new System.EventHandler(this.toolBar1_MouseHover);
            // 
            // btnSave
            // 
            this.btnSave.ImageIndex = 0;
            this.btnSave.Name = "btnSave";
            // 
            // btnClose
            // 
            this.btnClose.ImageIndex = 1;
            this.btnClose.Name = "btnClose";
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.ImageIndex = 2;
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.ToolTipText = "Save As";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "02.ICO");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "SaveAsHH.bmp");
            // 
            // frmLaserDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(72, 28);
            this.ControlBox = false;
            this.Controls.Add(this.toolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLaserDialog";
            this.ShowInTaskbar = false;
            this.Text = "frmLaserDialog";
            this.Load += new System.EventHandler(this.frmLaserDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ExStyle |= 0x00000080; // WS_EX_TOOLWINDOW
                return param;
            }
        }

		#region Properties
		
		/// <summary>
		/// used to track a particular instance when multiple versions are loaded
		/// </summary>
		public string FormCaption
		{
			get
			{
				return _caption;
			}
			set
			{
				_caption = value;
			}
		}
		
		

		#endregion

		
		#region Event Handlers
	
		
		private void Save_Click()
		{
			if(OnSave != null)
				OnSave(this,new System.EventArgs());
		}

		private void Exit_Click()
		{
			if(OnClose != null)
                OnClose(this,new System.EventArgs());
		}

        private void SaveAs_Click()
        {
            if (OnSaveAs != null)
                OnSaveAs(this, new System.EventArgs());
        }


		#endregion
		
		/// <summary>
		/// Positions the form within the parent window to hide existing buttons
		/// </summary>
		public void SetPosition(string version)
		{
			//positions form within parent window
			if(version == "8.7.215")
			{
				this.SetDesktopLocation(55,0);
				this.Height = 27;
				this.Width = 50;
			}
			else if(version == "8.8.122")
			{
				//should position the buttons on the taskbar
				this.SetDesktopLocation(0,0);
				this.Height = 21;
				this.Width = 100;
			}
			else if(version == "8.8.140") //sp5
			{
				//should position the buttons on the taskbar
				//Need to watch this as they now use flexible forms and an active X control
				//I belive positioning is going to vary on a form per form basis
				this.SetDesktopLocation(0,0);
				this.Height = 22;
				this.Width = 100;
			}
            else if (version.StartsWith("9.4"))
            {
                this.SetDesktopLocation(8, 32);
                this.Height = 20;
                this.Width = 90;
            }
			else //all others
			{
				//should position the buttons on the taskbar
				//Need to watch this as they now use flexible forms and an active X control
				//I belive positioning is going to vary on a form per form basis
				this.SetDesktopLocation(0,0);
				this.Height = 22;
				this.Width = 100;
			}
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(e.Button.ImageIndex)
			{
				case 0:
				{
					Save_Click();
					return;
				}
				case 1:
				{
					Exit_Click();
					return;
				}
                default:
                {
                    SaveAs_Click();
                    return;
                }
			}
		}



		private void frmLaserDialog_Load(object sender, System.EventArgs e)
		{
			//set tool tips for buttons
			toolBar1.Buttons[0].ToolTipText = OMS.Session.OMS.Resources.GetResource("SAVETOOMS","Save to OMS","").Text;
			toolBar1.Buttons[1].ToolTipText = OMS.Session.OMS.Resources.GetResource("CLOSE","Close","").Text;
		}

		private void toolBar1_MouseHover(object sender, System.EventArgs e)
		{
			this.Focus();
		}

		private void toolBar1_LostFocus(object sender, EventArgs e)
		{
			
		}
	}
}
