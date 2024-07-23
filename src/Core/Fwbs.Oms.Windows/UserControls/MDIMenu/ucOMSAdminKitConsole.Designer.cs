namespace FWBS.OMS.UI.Windows.Admin
{
    partial class ucOMSAdminKitConsole
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("56e23b98-af4b-485e-a223-52b38fd857a0"));
            Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("e54277f1-6d64-4f04-9037-13516d0c64db"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("56e23b98-af4b-485e-a223-52b38fd857a0"), -1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucOMSAdminKitConsoleAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.windowDockingArea1.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.radTreeView1);
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 482);
            this.panel1.TabIndex = 0;
            // 
            // radTreeView1
            // 
            this.radTreeView1.BackColor = System.Drawing.Color.White;
            this.radTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView1.EnableKeyMap = true;
            this.radTreeView1.ExpandAnimation = Telerik.WinControls.UI.ExpandAnimation.None;
            this.radTreeView1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radTreeView1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.radTreeView1.HideSelection = false;
            this.radTreeView1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            this.radTreeView1.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.radTreeView1.Location = new System.Drawing.Point(0, 0);
            this.radTreeView1.Margin = new System.Windows.Forms.Padding(0);
            this.radTreeView1.Name = "radTreeView1";
            this.radTreeView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radTreeView1.Size = new System.Drawing.Size(196, 482);
            this.radTreeView1.SpacingBetweenNodes = 5;
            this.radTreeView1.TabIndex = 0;
            this.radTreeView1.Text = "radTreeView1";
            this.radTreeView1.ThemeName = "Windows8";
            this.radTreeView1.ToggleMode = Telerik.WinControls.UI.ToggleMode.SingleClick;
            this.radTreeView1.TreeIndent = 10;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView1.GetChildAt(0))).ExpandAnimation = Telerik.WinControls.UI.ExpandAnimation.None;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView1.GetChildAt(0))).LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(251)))), ((int)(((byte)(252)))));
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView1.GetChildAt(0))).LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView1.GetChildAt(0))).TreeIndent = 10;
            ((Telerik.WinControls.UI.RadTreeViewElement)(this.radTreeView1.GetChildAt(0))).NodeSpacing = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(201, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1249, 502);
            this.panel2.TabIndex = 1;
            // 
            // ultraDockManager1
            // 
            dockableControlPane1.Control = this.panel1;
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(3, 3, 200, 496);
            appearance1.BackColor = System.Drawing.Color.LightGray;
            appearance1.BackColor2 = System.Drawing.Color.LightGray;
            appearance1.ForeColor = System.Drawing.Color.Black;
            dockableControlPane1.Settings.ActiveCaptionAppearance = appearance1;
            dockableControlPane1.Settings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.Color.LightGray;
            appearance2.BackColor2 = System.Drawing.Color.LightGray;
            appearance2.ForeColor = System.Drawing.Color.Black;
            dockableControlPane1.Settings.CaptionAppearance = appearance2;
            dockableControlPane1.Settings.DoubleClickAction = Infragistics.Win.UltraWinDock.PaneDoubleClickAction.None;
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Navigation";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(196, 502);
            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.ShowCloseButton = false;
            this.ultraDockManager1.ShowMenuButton = Infragistics.Win.DefaultableBoolean.False;
            this.ultraDockManager1.UseDefaultContextMenus = false;
            this.ultraDockManager1.WindowStyle = Infragistics.Win.UltraWinDock.WindowStyle.VisualStudio2005;
            // 
            // _ucOMSAdminKitConsoleUnpinnedTabAreaLeft
            // 
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Name = "_ucOMSAdminKitConsoleUnpinnedTabAreaLeft";
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 502);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft.TabIndex = 2;
            // 
            // _ucOMSAdminKitConsoleUnpinnedTabAreaRight
            // 
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Location = new System.Drawing.Point(1450, 0);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Name = "_ucOMSAdminKitConsoleUnpinnedTabAreaRight";
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 502);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaRight.TabIndex = 3;
            // 
            // _ucOMSAdminKitConsoleUnpinnedTabAreaTop
            // 
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Name = "_ucOMSAdminKitConsoleUnpinnedTabAreaTop";
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.Size = new System.Drawing.Size(1450, 0);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaTop.TabIndex = 4;
            // 
            // _ucOMSAdminKitConsoleUnpinnedTabAreaBottom
            // 
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 502);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Name = "_ucOMSAdminKitConsoleUnpinnedTabAreaBottom";
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1450, 0);
            this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom.TabIndex = 5;
            // 
            // _ucOMSAdminKitConsoleAutoHideControl
            // 
            this._ucOMSAdminKitConsoleAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucOMSAdminKitConsoleAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._ucOMSAdminKitConsoleAutoHideControl.Name = "_ucOMSAdminKitConsoleAutoHideControl";
            this._ucOMSAdminKitConsoleAutoHideControl.Owner = this.ultraDockManager1;
            this._ucOMSAdminKitConsoleAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._ucOMSAdminKitConsoleAutoHideControl.TabIndex = 6;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Controls.Add(this.dockableWindow1);
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Left;
            this.windowDockingArea1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.ultraDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(201, 502);
            this.windowDockingArea1.TabIndex = 7;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.panel1);
            this.dockableWindow1.Location = new System.Drawing.Point(0, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.ultraDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(196, 502);
            this.dockableWindow1.TabIndex = 8;
            // 
            // ucOMSAdminKitConsole
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._ucOMSAdminKitConsoleAutoHideControl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._ucOMSAdminKitConsoleUnpinnedTabAreaTop);
            this.Controls.Add(this._ucOMSAdminKitConsoleUnpinnedTabAreaBottom);
            this.Controls.Add(this._ucOMSAdminKitConsoleUnpinnedTabAreaLeft);
            this.Controls.Add(this._ucOMSAdminKitConsoleUnpinnedTabAreaRight);
            this.Name = "ucOMSAdminKitConsole";
            this.Size = new System.Drawing.Size(1450, 502);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.windowDockingArea1.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ucOMSAdminKitConsoleAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucOMSAdminKitConsoleUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucOMSAdminKitConsoleUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucOMSAdminKitConsoleUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucOMSAdminKitConsoleUnpinnedTabAreaRight;
        private Telerik.WinControls.UI.RadTreeView radTreeView1;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;

    }
}
