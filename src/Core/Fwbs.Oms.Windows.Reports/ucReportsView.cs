using System;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for ucReportsView.
    /// </summary>
    public class ucReportsView : System.Windows.Forms.UserControl, IOBjectDirty
	{
		#region Auto Controls
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
		private FWBS.OMS.UI.Windows.Reports.ucReportsManager ucReportsManager1;
		private FWBS.OMS.UI.Windows.omsSplitter splitter1;
		private System.ComponentModel.IContainer components;
		#endregion

		#region Fields
		private System.Windows.Forms.Timer timButUpdate;
		private FWBS.OMS.Report _report = null;

		private FWBS.OMS.UI.Windows.OMSToolBarButton tbFirstPage = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbPrevPage = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbNextPage = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbLastPage = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbPrint = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbRefresh = null;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbExport = null;
        protected Panel pnlMain;
        private eToolbars OMSToolBar;
		private FWBS.OMS.UI.Windows.OMSToolBarButton tbZoom = null;


		#endregion

		#region Contructors
		public ucReportsView()
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
                ucReportsView_ReportClosing();
                if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportsView));
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.timButUpdate = new System.Windows.Forms.Timer(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.OMSToolBar = new FWBS.OMS.UI.Windows.eToolbars();
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.ucReportsManager1 = new FWBS.OMS.UI.Windows.Reports.ucReportsManager();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayBackgroundEdge = false;
            this.crystalReportViewer1.DisplayToolbar = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(235, 34);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowExportButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowPageNavigateButtons = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowPrintButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.ShowZoomButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(703, 473);
            this.crystalReportViewer1.TabIndex = 2;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // timButUpdate
            // 
            this.timButUpdate.Interval = 200;
            this.timButUpdate.Tick += new System.EventHandler(this.timButUpdate_Tick);
            // 
            // pnlMain
            // 
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMain.Controls.Add(this.OMSToolBar);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.pnlMain.Size = new System.Drawing.Size(938, 34);
            this.pnlMain.TabIndex = 17;
            // 
            // OMSToolBar
            // 
            this.OMSToolBar.BottomDivider = true;
            this.OMSToolBar.ButtonsXML = resources.GetString("OMSToolBar.ButtonsXML");
            this.OMSToolBar.Divider = false;
            this.OMSToolBar.DropDownArrows = true;
            this.OMSToolBar.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.OMSToolBar.Location = new System.Drawing.Point(4, 0);
            this.OMSToolBar.Name = "OMSToolBar";
            this.OMSToolBar.NavCommandPanel = null;
            this.OMSToolBar.ShowToolTips = true;
            this.OMSToolBar.Size = new System.Drawing.Size(930, 34);
            this.OMSToolBar.TabIndex = 0;
            this.OMSToolBar.TopDivider = false;
            this.OMSToolBar.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.OMSToolbars_OMSButtonClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(230, 34);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 473);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            this.splitter1.Visible = false;
            // 
            // ucReportsManager1
            // 
            this.ucReportsManager1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(212)))), ((int)(((byte)(243)))));
            this.ucReportsManager1.CloseVisible = true;
            this.ucReportsManager1.CrystalReportViewer = this.crystalReportViewer1;
            this.ucReportsManager1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucReportsManager1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucReportsManager1.Heading = "Report Manager";
            this.ucReportsManager1.Location = new System.Drawing.Point(0, 34);
            this.ucReportsManager1.Name = "ucReportsManager1";
            this.ucReportsManager1.Reports = null;
            this.ucReportsManager1.Size = new System.Drawing.Size(230, 473);
            this.ucReportsManager1.TabIndex = 3;
            this.ucReportsManager1.Visible = false;
            this.ucReportsManager1.ReportedShowed += new System.EventHandler(this.ucReportsManager1_ReportedShowed_1);
            this.ucReportsManager1.CursorChanged += new System.EventHandler(this.ucReportsManager1_CursorChanged);
            this.ucReportsManager1.VisibleChanged += new System.EventHandler(this.ucReportsManager1_VisibleChanged);
            // 
            // ucReportsView
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.ucReportsManager1);
            this.Controls.Add(this.pnlMain);
            this.Name = "ucReportsView";
            this.Size = new System.Drawing.Size(938, 507);
            this.Load += new System.EventHandler(this.ucReportsView_Load);
            this.ParentChanged += new System.EventHandler(this.ucReportsView_ParentChanged);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public void Reset()
		{
			ucReportsManager1.Visible=false;
			crystalReportViewer1.ReportSource = null;
			crystalReportViewer1.Show();
		}

		private void UpdateButtons()
		{
			try
			{
                ToolStrip tb = crystalReportViewer1.Controls[4] as ToolStrip;
				tbFirstPage.Enabled = tb.Items[4].Enabled;
                tbPrevPage.Enabled = tb.Items[5].Enabled;
                tbNextPage.Enabled = tb.Items[6].Enabled;
                tbLastPage.Enabled = tb.Items[7].Enabled;
                tbPrint.Enabled = tb.Items[1].Enabled;
                tbRefresh.Enabled = tb.Items[2].Enabled;
                tbExport.Enabled = tb.Items[0].Enabled;
                tbZoom.Enabled = tb.Items[11].Enabled;
			}
			catch(Exception)
			{
				timButUpdate.Enabled = false;
			}
		}

		public void Run()
		{
			try
			{
				ucReportsManager1.Run();
			}
			catch{}
		}

		#region Properties

		public System.Windows.Forms.Button ApplyButton
		{
			get
			{
				return ucReportsManager1.ApplyButton;
			}
		}

		public FWBS.OMS.Report Report
		{
			get
			{
				return _report;
			}
			set
			{
                _report = value;
                if (_report != null && Parent != null)
                {
                    LoadReport();
                }
			}
		}

        private void LoadReport()
        {
            try
            {
                _report.OnLoad();
                ucReportsManager1.Reports = _report;
                _report.Runed += new EventHandler(_report_Runed);
                if (ParentForm != null && _report.SearchList != null)
                    ParentForm.Text = _report.SearchList.Description;
                ucReportsManager1.ReportedShowed += new EventHandler(ucReportsManager1_ReportedShowed);
                if (_report.SearchList == null || _report.SearchList.CriteriaForm == null)
                {
                    try
                    {
                        if (this.OMSToolBar != null && this.OMSToolBar.GetButton("tbFilter") != null)
                        {
                            this.OMSToolBar.GetButton("tbFilter").Visible = false;
                            this.OMSToolBar.GetButton("tbFilter").Group = "Hidden";
                        }
                        ucReportsManager1.Run();
                    }
                    catch { }
                }
                else
                {
                    if (this.OMSToolBar != null && this.OMSToolBar.GetButton("tbFilter") != null)
                    {
                        this.OMSToolBar.GetButton("tbFilter").Visible = true;
                        this.OMSToolBar.GetButton("tbFilter").Group = "Reports";
                    }
                    ucReportsManager1.Visible = true;
                }
                _report.OnShow();
                crystalReportViewer1.Zoom(1);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

		#endregion

		#region Private


		private void ucReportsManager1_VisibleChanged(object sender, System.EventArgs e)
		{
			splitter1.Visible = ucReportsManager1.Visible;
		}

		private void frmReports_Activated(object sender, System.EventArgs e)
		{
			if (this.OMSToolBar != null)
			{
				this.timButUpdate.Enabled = true;
			}
		}

        private void OMSToolbars_OMSButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
        {
            if (e.Button.Name == "tbFilter")
            {
                ucReportsManager1.Visible = e.Button.Pushed;
            }
            else if (e.Button.Name == "tbFirstPage")
            {
                crystalReportViewer1.ShowFirstPage();
            }
            else if (e.Button.Name == "tbPrevPage")
            {
                crystalReportViewer1.ShowPreviousPage();
            }
            else if (e.Button.Name == "tbNextPage")
            {
                crystalReportViewer1.ShowNextPage();
            }
            else if (e.Button.Name == "tbLastPage")
            {
                crystalReportViewer1.ShowLastPage();
            }
            else if (e.Button.Name == "tbPrint")
            {
                crystalReportViewer1.PrintReport();
            }
            else if (e.Button.Name == "tbRefresh")
            {
                crystalReportViewer1.RefreshReport();
            }
            else if (e.Button.Name == "tbExport")
            {
                crystalReportViewer1.ExportReport();
            }
            else if (e.Button.Name == "tbZoom")
            {
                int n = FWBS.Common.ConvertDef.ToInt32(e.Button.Tag, 0) + 1;
                if (n > e.Button.DropDownMenu.MenuItems.Count - 1) n = 0;
                Zoom_Menu_Click(e.Button.DropDownMenu.MenuItems[n], EventArgs.Empty);
                e.Button.Tag = n;
            }

        }


        private void frmReports_Deactivate(object sender, System.EventArgs e)
        {
            if (this.OMSToolBar != null)
            {
                this.OMSToolBar.OMSButtonClick -= new OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);
                this.timButUpdate.Enabled = false;
            }
        }

        private void ucReportsManager1_ReportedShowed(object sender, EventArgs e)
        {
            if (Session.CurrentSession.IsLoggedIn)
                timButUpdate.Enabled = true;
        }

        private void timButUpdate_Tick(object sender, System.EventArgs e)
        {
            UpdateButtons();
        }
        #endregion

        private void ucReportsView_Load(object sender, System.EventArgs e)
        {
            if (Session.CurrentSession.IsLoggedIn && OMSToolBar != null)
            {
                OMSToolBar.GroupVisible("Reports", true);
                OMSToolBar.GroupVisible("Reports2", true);
                tbFirstPage = OMSToolBar.GetButton("tbFirstPage");
                tbPrevPage = OMSToolBar.GetButton("tbPrevPage");
                tbNextPage = OMSToolBar.GetButton("tbNextPage");
                tbLastPage = OMSToolBar.GetButton("tbLastPage");
                tbPrint = OMSToolBar.GetButton("tbPrint");
                tbRefresh = OMSToolBar.GetButton("tbRefresh");
                tbExport = OMSToolBar.GetButton("tbExport");
                tbZoom = OMSToolBar.GetButton("tbZoom");
                ContextMenu menus = new ContextMenu();
                MenuItem mitem = new MenuItem();
                mitem.Text = "Fit To Width";
                mitem.Tag = 1;
                mitem.Checked = true;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);
                menus.Tag = mitem;

                mitem = new MenuItem();
                mitem.Text = "Fit To Page";
                mitem.Tag = 2;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                mitem = new MenuItem();
                mitem.Text = "400%";
                mitem.Tag = 400;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                mitem = new MenuItem();
                mitem.Text = "300%";
                mitem.Tag = 300;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                mitem = new MenuItem();
                mitem.Text = "200%";
                mitem.Tag = 200;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                mitem = new MenuItem();
                mitem.Text = "100%";
                mitem.Tag = 100;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                mitem = new MenuItem();
                mitem.Text = "50%";
                mitem.Tag = 50;
                mitem.Click += new EventHandler(Zoom_Menu_Click);
                menus.MenuItems.Add(mitem);

                tbZoom.DropDownMenu = menus;

                UpdateButtons();
            }
        }


        public void ucReportsView_ReportClosing()
        {
            if (this.OMSToolBar != null)
            {
                this.OMSToolBar.OMSButtonClick -= new OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);
            }
        }


        void Zoom_Menu_Click(object sender, EventArgs e)
        {
            if (((MenuItem)sender).Parent.Tag != null)
                ((MenuItem)((MenuItem)sender).Parent.Tag).Checked = false;
            crystalReportViewer1.Zoom(Convert.ToInt32(((MenuItem)sender).Tag));
            ((MenuItem)sender).Checked = true;
            ((MenuItem)sender).Parent.Tag = ((MenuItem)sender);
        }

        private void _report_Runed(object sender, EventArgs e)
        {
            this.Run();
        }

        private void ucReportsView_ParentChanged(object sender, System.EventArgs e)
        {
            if (_report != null && Parent != null)
            {
                LoadReport();
            }
            if (FWBS.OMS.UI.Windows.Global.GetParentForm(this) != null)
            {
                FWBS.OMS.UI.Windows.Global.GetParentForm(this).KeyPreview = true;
                FWBS.OMS.UI.Windows.Global.GetParentForm(this).KeyDown += new KeyEventHandler(ParentForm_KeyDown);
            }
        }

        private void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageUp) crystalReportViewer1.ShowPreviousPage();
            else if (e.KeyCode == Keys.PageDown) crystalReportViewer1.ShowNextPage();
            else if (e.KeyCode == Keys.End) crystalReportViewer1.ShowLastPage();
            else if (e.KeyCode == Keys.Home) crystalReportViewer1.ShowFirstPage();

        }

        private void ucReportsManager1_CursorChanged(object sender, System.EventArgs e)
        {
            crystalReportViewer1.Cursor = ucReportsManager1.Cursor;
        }

        private void ucReportsManager1_ReportedShowed_1(object sender, System.EventArgs e)
        {
            try
            {
                crystalReportViewer1.Zoom(Convert.ToInt32(((MenuItem)tbZoom.DropDownMenu.Tag).Tag));
            }
            catch
            {
                crystalReportViewer1.Zoom(1);
            }
        }

        #region IObjectDirty Implementation

        private bool isdirty;
        public bool IsDirty { get { return isdirty; } }

        public bool IsObjectDirty()
        {
            return true;
        }

        #endregion
    }
}
