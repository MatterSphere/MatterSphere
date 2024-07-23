using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmGenEngine.
    /// </summary>
    public class frmGenEngine : FWBS.OMS.UI.Windows.BaseForm
	{
		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private System.Windows.Forms.Panel panel1;
		public FWBS.OMS.UI.Windows.ucNavCommands pnlFavButtons;
		public FWBS.OMS.UI.Windows.ucPanelNav NavActions;
        private bool _isdirty = false;
        private IContainer components;

		public frmGenEngine()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}
		public frmGenEngine(string code)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			ucSearchControl1.SetSearchList(code,null,new FWBS.Common.KeyValueCollection());
			ucSearchControl1.ShowPanelButtons();
			this.Text = ucSearchControl1.SearchList.Description;
			if (ucSearchControl1.SearchList.Style == FWBS.OMS.SearchEngine.SearchListStyle.List)
			ucSearchControl1.Search();
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
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlFavButtons = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NavActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.panel1.SuspendLayout();
            this.NavActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.SystemColors.Window;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(171, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.pnlFavButtons;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(4);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(341, 390);
            this.ucSearchControl1.TabIndex = 0;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.OpenedOMSItem += new System.EventHandler(this.ucSearchControl1_OpenedOMSItem);
            this.ucSearchControl1.ClosedOMSItem += new System.EventHandler(this.ucSearchControl1_ClosedOMSItem);
            // 
            // pnlFavButtons
            // 
            this.pnlFavButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFavButtons.Location = new System.Drawing.Point(0, 24);
            this.pnlFavButtons.Name = "pnlFavButtons";
            this.pnlFavButtons.Padding = new System.Windows.Forms.Padding(4);
            this.pnlFavButtons.PanelBackColor = System.Drawing.Color.Empty;
            this.pnlFavButtons.Size = new System.Drawing.Size(163, 0);
            this.pnlFavButtons.TabIndex = 15;
            this.pnlFavButtons.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Controls.Add(this.NavActions);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4);
            this.panel1.Size = new System.Drawing.Size(171, 390);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // NavActions
            // 
            this.NavActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.NavActions.Controls.Add(this.pnlFavButtons);
            this.NavActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.NavActions.ExpandedHeight = 31;
            this.NavActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NavActions.HeaderColor = System.Drawing.Color.Empty;
            this.NavActions.Location = new System.Drawing.Point(4, 4);
            this.NavActions.Name = "NavActions";
            this.NavActions.Size = new System.Drawing.Size(163, 31);
            this.NavActions.TabIndex = 30;
            this.NavActions.TabStop = false;
            this.NavActions.Text = "Actions";
            this.NavActions.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // frmGenEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(512, 390);
            this.Controls.Add(this.ucSearchControl1);
            this.Controls.Add(this.panel1);
            this.Name = "frmGenEngine";
            this.Text = "Generic Admin Editor";
            this.Activated += new System.EventHandler(this.frmGenEngine_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmGenEngine_Closing);
            this.Deactivate += new System.EventHandler(this.frmGenEngine_Deactivate);
            this.Load += new System.EventHandler(this.frmGenEngine_Load);
            this.panel1.ResumeLayout(false);
            this.NavActions.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmGenEngine_Load(object sender, System.EventArgs e)
		{
		
		}

		private void frmGenEngine_Activated(object sender, System.EventArgs e)
		{
			NavActions.Visible = true;
		}

		private void frmGenEngine_Deactivate(object sender, System.EventArgs e)
		{
			NavActions.Visible = false;
		}

		private void ucSearchControl1_OpenedOMSItem(object sender, System.EventArgs e)
		{
			frmMain mainform = MdiParent as frmMain;
			mainform.OMSToolbars.GetButton("btnClose").Enabled = false;
		}

		private void ucSearchControl1_ClosedOMSItem(object sender, System.EventArgs e)
		{
			frmMain mainform = MdiParent as frmMain;
			mainform.OMSToolbars.GetButton("btnClose").Enabled = true;

		}

		private void frmGenEngine_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (IsDirty)
			{
				ucSearchControl1.CloseOMSItem();
				e.Cancel=true;
			}
            Session.CurrentSession.ClearCache();

		}

		public bool IsDirty
		{
			get
			{
				return _isdirty || ucSearchControl1.IsDirty;
			}
			set
			{
				_isdirty = value;
			}
		}
	}
}
