using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmOyezOverlay.
    /// </summary>
    public class frmOyezOverlay : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.ToolBarButton btnSave;
		private System.Windows.Forms.ToolBarButton btnClose;
		private System.Windows.Forms.ImageList imageList1;
        private ToolBarButton btnSaveAs;
		private System.ComponentModel.IContainer components;

		public event EventHandler OnSave;
		public event EventHandler OnClose;
        public event EventHandler OnSaveAs;

		public frmOyezOverlay()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOyezOverlay));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.btnSave = new System.Windows.Forms.ToolBarButton();
            this.btnClose = new System.Windows.Forms.ToolBarButton();
            this.btnSaveAs = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.btnSave,
            this.btnClose,
            this.btnSaveAs});
            this.toolBar1.ButtonSize = new System.Drawing.Size(23, 23);
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(77, 29);
            this.toolBar1.TabIndex = 2;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
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
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "SaveHH.bmp");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "SaveAsHH.bmp");
            this.imageList1.Images.SetKeyName(3, "");
            // 
            // frmOyezOverlay
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(77, 31);
            this.ControlBox = false;
            this.Controls.Add(this.toolBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOyezOverlay";
            this.ShowInTaskbar = false;
            this.Text = "frmOyezDialog";
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

		/// <summary>
		/// Positions the form within the parent window
		/// </summary>
		public void SetPosition()
		{
            //New button overlay location
            this.SetDesktopLocation(400, 0);
            this.Height = 27;
            this.Width = 76;

            //added as subsequent runs in v10 did not display the buttons
            this.Refresh();
		}



		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            DialogResult result = DialogResult.No;

            switch(e.Button.ImageIndex)
			{
				case 0:
				{
					Save_Click();
					return;
				}
				case 1:
				{			
	
                    //check if they want to save form
    				result = MessageBox.ShowYesNoCancel("OMSSAVEPROMPT","Do you wish to save to OMS");
					
                    if(result == DialogResult.Yes)
					{
						//check event has been subscribed and if so call the save event
						Save_Click();
					}
					else if(result == DialogResult.No)
					{
						Exit_Click();
					}
					return;
				}
                default: //save as
                {
                    
                    //check event has been subscribed and if so call the save event
                    SaveAs_Click();
                    
                    return;
                }
			}
		}

		private void frmLaserDialog_Load(object sender, System.EventArgs e)
		{
			//set tool tips for buttons
			toolBar1.Buttons[0].ToolTipText = OMS.Session.CurrentSession.Resources.GetResource("SAVETOOMS","Save to OMS","").Text;
			toolBar1.Buttons[1].ToolTipText = OMS.Session.CurrentSession.Resources.GetResource("CLOSE","Close","").Text;
            toolBar1.Buttons[2].ToolTipText = OMS.Session.CurrentSession.Resources.GetResource("SAVEAS", "Save As", "").Text;
		}

		private void toolBar1_MouseHover(object sender, System.EventArgs e)
		{
			this.Focus();
		}

	}
}
