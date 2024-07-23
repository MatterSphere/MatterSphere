using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An abstract form that holds a OK, save cancel button logic for forms that may want this
    /// functionality.
    /// </summary>
    internal class frmDialog : BaseForm, ISupportRightToLeft
	{

		#region Control Fields

		protected System.Windows.Forms.ToolTip toolTip1;
		protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		protected FWBS.Common.UI.Windows.ToolBar tbLeft;
		protected FWBS.Common.UI.Windows.ToolBar tbRight;
		private System.Windows.Forms.Panel pnlTBRight;
		private System.Windows.Forms.Panel pnlTBLeft;
		protected FWBS.OMS.UI.Windows.ThreeDPanel pnlTop;
		protected System.Windows.Forms.ToolBarButton cmdBack;
		protected System.Windows.Forms.ToolBarButton cmdRefresh;
		protected System.Windows.Forms.ToolBarButton cmdSave;
		protected System.Windows.Forms.ToolBarButton cmdCancel;
		protected System.Windows.Forms.ToolBarButton cmdOK;
		private System.Windows.Forms.ToolBarButton tbBalance;
		protected System.Windows.Forms.Button quickOK;
        protected System.Windows.Forms.Button quickCancel;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default contructor.
		/// </summary>
		public frmDialog()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			tbLeft.ImageList = FWBS.OMS.UI.Windows.Images.GetCoolButtons24();
			tbRight.ImageList = FWBS.OMS.UI.Windows.Images.GetCoolButtons24();
		}

			

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
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
            this.components = new System.ComponentModel.Container();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmdBack = new System.Windows.Forms.ToolBarButton();
            this.cmdRefresh = new System.Windows.Forms.ToolBarButton();
            this.cmdSave = new System.Windows.Forms.ToolBarButton();
            this.cmdCancel = new System.Windows.Forms.ToolBarButton();
            this.cmdOK = new System.Windows.Forms.ToolBarButton();
            this.pnlTop = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.quickCancel = new System.Windows.Forms.Button();
            this.quickOK = new System.Windows.Forms.Button();
            this.pnlTBRight = new System.Windows.Forms.Panel();
            this.tbRight = new FWBS.Common.UI.Windows.ToolBar();
            this.tbBalance = new System.Windows.Forms.ToolBarButton();
            this.pnlTBLeft = new System.Windows.Forms.Panel();
            this.tbLeft = new FWBS.Common.UI.Windows.ToolBar();
            this.pnlTop.SuspendLayout();
            this.pnlTBRight.SuspendLayout();
            this.pnlTBLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "Forms\\Dialogs";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // cmdBack
            // 
            this.cmdBack.ImageIndex = 17;
            this.cmdBack.Name = "cmdBack";
            this.cmdBack.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.cmdBack.Tag = "cmdBack";
            this.cmdBack.Text = "&Back";
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.ImageIndex = 22;
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Tag = "cmdRefresh";
            this.cmdRefresh.Text = "&Refresh";
            // 
            // cmdSave
            // 
            this.cmdSave.ImageIndex = 2;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Tag = "cmdSave";
            this.cmdSave.Text = "&Save";
            // 
            // cmdCancel
            // 
            this.cmdCancel.ImageIndex = 21;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Tag = "cmdCancel";
            this.cmdCancel.Text = "Cance&l   ";
            // 
            // cmdOK
            // 
            this.cmdOK.ImageIndex = 7;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Tag = "cmdOK";
            this.cmdOK.Text = "&OK";
            // 
            // pnlTop
            // 
            this.pnlTop.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Bottom;
            this.pnlTop.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlTop.Controls.Add(this.quickCancel);
            this.pnlTop.Controls.Add(this.quickOK);
            this.pnlTop.Controls.Add(this.pnlTBRight);
            this.pnlTop.Controls.Add(this.pnlTBLeft);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlTop.Size = new System.Drawing.Size(728, 45);
            this.pnlTop.TabIndex = 9;
            this.pnlTop.TabStop = false;
            // 
            // quickCancel
            // 
            this.quickCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quickCancel.Location = new System.Drawing.Point(352, -34);
            this.quickCancel.Name = "quickCancel";
            this.quickCancel.Size = new System.Drawing.Size(19, 33);
            this.quickCancel.TabIndex = 3;
            this.quickCancel.TabStop = false;
            this.quickCancel.Text = "C";
            // 
            // quickOK
            // 
            this.quickOK.Location = new System.Drawing.Point(328, -34);
            this.quickOK.Name = "quickOK";
            this.quickOK.Size = new System.Drawing.Size(19, 33);
            this.quickOK.TabIndex = 2;
            this.quickOK.TabStop = false;
            this.quickOK.Text = "O";
            // 
            // pnlTBRight
            // 
            this.pnlTBRight.Controls.Add(this.tbRight);
            this.pnlTBRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTBRight.Location = new System.Drawing.Point(400, 0);
            this.pnlTBRight.Name = "pnlTBRight";
            this.pnlTBRight.Size = new System.Drawing.Size(328, 41);
            this.pnlTBRight.TabIndex = 1;
            // 
            // tbRight
            // 
            this.tbRight.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbRight.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbBalance,
            this.cmdSave,
            this.cmdCancel,
            this.cmdOK});
            this.tbRight.Divider = false;
            this.tbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRight.DropDownArrows = true;
            this.tbRight.Location = new System.Drawing.Point(0, 0);
            this.tbRight.Name = "tbRight";
            this.tbRight.ShowToolTips = true;
            this.tbRight.Size = new System.Drawing.Size(328, 34);
            this.tbRight.TabIndex = 0;
            this.tbRight.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbRight.Wrappable = false;
            // 
            // tbBalance
            // 
            this.tbBalance.Name = "tbBalance";
            this.tbBalance.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbBalance.Text = "Balance";
            this.tbBalance.Visible = false;
            // 
            // pnlTBLeft
            // 
            this.pnlTBLeft.Controls.Add(this.tbLeft);
            this.pnlTBLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTBLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTBLeft.Name = "pnlTBLeft";
            this.pnlTBLeft.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlTBLeft.Size = new System.Drawing.Size(394, 41);
            this.pnlTBLeft.TabIndex = 0;
            // 
            // tbLeft
            // 
            this.tbLeft.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbLeft.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdBack,
            this.cmdRefresh});
            this.tbLeft.Divider = false;
            this.tbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLeft.DropDownArrows = true;
            this.tbLeft.Location = new System.Drawing.Point(5, 0);
            this.tbLeft.Name = "tbLeft";
            this.tbLeft.ShowToolTips = true;
            this.tbLeft.Size = new System.Drawing.Size(389, 34);
            this.tbLeft.TabIndex = 0;
            this.tbLeft.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbLeft.Wrappable = false;
            // 
            // frmDialog
            // 
            this.AcceptButton = this.quickOK;
            this.CancelButton = this.quickCancel;
            this.ClientSize = new System.Drawing.Size(728, 579);
            this.Controls.Add(this.pnlTop);
            this.DoubleBuffered = true;
            this.Name = "frmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OMS Dialog";
            this.Load += new System.EventHandler(this.frmDialog_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTBRight.ResumeLayout(false);
            this.pnlTBRight.PerformLayout();
            this.pnlTBLeft.ResumeLayout(false);
            this.pnlTBLeft.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		private void frmDialog_Load(object sender, System.EventArgs e)
		{
            if (OMS.Session.CurrentSession.IsLoggedIn)
                SetResources();
            int _pnlTBRight = 5;
            foreach (ToolBarButton tb in tbRight.Buttons)
            {
                if (tb.Visible)
                    _pnlTBRight = _pnlTBRight + tb.Rectangle.Width;
            }
            int _pnlTBLeft = 5;
            foreach (ToolBarButton tb in tbLeft.Buttons)
            {
                if (tb.Visible)
                    _pnlTBLeft = _pnlTBLeft + tb.Rectangle.Width;
            }
            pnlTBRight.Width = _pnlTBRight;
            pnlTBLeft.Width = _pnlTBLeft;
            pnlTop.Height = tbLeft.Height + 2;
        }

        protected virtual void SetResources()
        {
            cmdBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "&Back", "").Text;
            cmdRefresh.Text = Session.CurrentSession.Resources.GetResource("CMDREFRESH", "&Refresh", "").Text;
            cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            cmdCancel.Text = Session.CurrentSession.Resources.GetResource("CMDCANCEL", "Cance&l", "").Text;
            cmdOK.Text = Session.CurrentSession.Resources.GetResource("CMDOK", "&OK", "").Text;
        }
	
		[DefaultValue(@"Forms\Dialogs")]
		public string FormStorageID
		{
			get
			{
				return ucFormStorage1.UniqueID;
			}
			set
			{
				if (value != "")
					ucFormStorage1.UniqueID = "Forms\\Dialogs\\" + value;
			}
		}

        public void SetRTL(Form parentform)
        {
            if (this.RightToLeft != System.Windows.Forms.RightToLeft.Yes)
                return;

            foreach (Control item in this.Controls)
            {
                Global.RightToLeftControlConverter(item, parentform);
            }
            Global.ConvertToolBarRTL(tbLeft);
            Global.ConvertToolBarRTL(tbRight);
        }


	}
}

