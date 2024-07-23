using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmOyezDialog.
    /// </summary>
    public class frmOyezDialog : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public event EventHandler OnSave;
		public event EventHandler OnClose;

		public frmOyezDialog()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOyezDialog));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(2, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(129, 22);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "XXX";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(3, 28);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(129, 22);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "XXX";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmOyezDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(134, 52);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOyezDialog";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmOyezDialog_Load);
            this.ResumeLayout(false);

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

		private void frmOyezDialog_Load(object sender, System.EventArgs e)
		{
			//set caption of button
			this.btnSave.Text = OMS.Session.CurrentSession.Resources.GetResource("SAVETOOMS","Save to OMS","").Text;
			this.btnClose.Text = OMS.Session.CurrentSession.Resources.GetResource("CLOSE","Close","").Text;
		}
		
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(OnSave != null)
                OnSave(this, new System.EventArgs());
		}
		
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			DialogResult result = DialogResult.No;

			try
			{
				//check if they want to save form
				result = MessageBox.ShowYesNoCancel("OMSSAVEPROMPT","Do you wish to save to OMS");
			}
			catch{}

			if(result == DialogResult.Yes)
			{
				//check event has been subscribed and if so call the save event
				if(OnSave != null)
					OnSave(this, new System.EventArgs());
			}
			else if(result == DialogResult.No)
			{
				OnClose(this,new System.EventArgs());
			}
		}
		
		/// <summary>
		/// Positions the form within the parent window
		/// </summary>
		public void SetPosition()
		{
			//positions form within parent window
			if(this.Parent != null)
			{
				this.Top = Parent.Top + 50;
				this.Left = Parent.Left + 300;
			}
			else
			{
				//should poistion the buttons on the taskbar
				this.SetDesktopLocation(690,2);
			}

		}
	}
}
