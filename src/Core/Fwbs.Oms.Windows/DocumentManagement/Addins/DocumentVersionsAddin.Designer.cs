namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    partial class DocumentVersionsAddin
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentVersionsAddin));
            this.dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("485afa68-1cc5-4ea7-a426-d32878367dd4"));
            this.dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("e8473790-0c7f-4378-a2af-851e6d37143b"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("485afa68-1cc5-4ea7-a426-d32878367dd4"), -1);
            this.versions = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentVersions();
            this.ucDocumentPreviewer1 = new FWBS.OMS.UI.Windows.DocumentManagement.ucDocumentPreviewer();
            this.omsDockManager1 = new FWBS.OMS.UI.omsDockManager(this.components);
            this._ucBaseAddinUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).BeginInit();
            this._ucBaseAddinAutoHideControl.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Size = new System.Drawing.Size(168, 605);
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // versions
            // 
            this.versions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.versions.Location = new System.Drawing.Point(168, 0);
            this.versions.Name = "versions";
            this.versions.Padding = new System.Windows.Forms.Padding(10);
            this.versions.Size = new System.Drawing.Size(692, 605);
            this.versions.TabIndex = 0;
            // 
            // ucDocumentPreviewer1
            // 
            this.ucDocumentPreviewer1.CultureProperties = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("ucDocumentPreviewer1.CultureProperties")));
            this.ucDocumentPreviewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDocumentPreviewer1.Location = new System.Drawing.Point(0, 18);
            this.ucDocumentPreviewer1.Name = "ucDocumentPreviewer1";
            this.ucDocumentPreviewer1.Size = new System.Drawing.Size(333, 587);
            this.ucDocumentPreviewer1.TabIndex = 0;
            // 
            // omsDockManager1
            // 
            this.omsDockManager1.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.VerticalSplit;
            dockableControlPane1.Control = this.ucDocumentPreviewer1;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(333, -1);
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(583, 123, 188, 292);
            dockableControlPane1.Pinned = false;
            dockableControlPane1.Key = "DocVersAddin";
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Document Preview";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(333, 605);
            this.omsDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.omsDockManager1.HostControl = this;
            this.omsDockManager1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.PaneDisplayed += new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.omsDockManager1_PaneDisplayed);
            this.omsDockManager1.PaneHidden += new Infragistics.Win.UltraWinDock.PaneHiddenEventHandler(this.omsDockManager1_PaneHidden);
            // 
            // _ucBaseAddinUnpinnedTabAreaLeft
            // 
            this._ucBaseAddinUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ucBaseAddinUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucBaseAddinUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._ucBaseAddinUnpinnedTabAreaLeft.Name = "_ucBaseAddinUnpinnedTabAreaLeft";
            this._ucBaseAddinUnpinnedTabAreaLeft.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 605);
            this._ucBaseAddinUnpinnedTabAreaLeft.TabIndex = 9;
            // 
            // _ucBaseAddinUnpinnedTabAreaRight
            // 
            this._ucBaseAddinUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ucBaseAddinUnpinnedTabAreaRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucBaseAddinUnpinnedTabAreaRight.Location = new System.Drawing.Point(860, 0);
            this._ucBaseAddinUnpinnedTabAreaRight.Name = "_ucBaseAddinUnpinnedTabAreaRight";
            this._ucBaseAddinUnpinnedTabAreaRight.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaRight.Size = new System.Drawing.Size(21, 605);
            this._ucBaseAddinUnpinnedTabAreaRight.TabIndex = 10;
            // 
            // _ucBaseAddinUnpinnedTabAreaTop
            // 
            this._ucBaseAddinUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ucBaseAddinUnpinnedTabAreaTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucBaseAddinUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._ucBaseAddinUnpinnedTabAreaTop.Name = "_ucBaseAddinUnpinnedTabAreaTop";
            this._ucBaseAddinUnpinnedTabAreaTop.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaTop.Size = new System.Drawing.Size(881, 0);
            this._ucBaseAddinUnpinnedTabAreaTop.TabIndex = 11;
            // 
            // _ucBaseAddinUnpinnedTabAreaBottom
            // 
            this._ucBaseAddinUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ucBaseAddinUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucBaseAddinUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 605);
            this._ucBaseAddinUnpinnedTabAreaBottom.Name = "_ucBaseAddinUnpinnedTabAreaBottom";
            this._ucBaseAddinUnpinnedTabAreaBottom.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaBottom.Size = new System.Drawing.Size(881, 0);
            this._ucBaseAddinUnpinnedTabAreaBottom.TabIndex = 12;
            // 
            // _ucBaseAddinAutoHideControl
            // 
            this._ucBaseAddinAutoHideControl.Controls.Add(this.dockableWindow1);
            this._ucBaseAddinAutoHideControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ucBaseAddinAutoHideControl.Location = new System.Drawing.Point(822, 0);
            this._ucBaseAddinAutoHideControl.Name = "_ucBaseAddinAutoHideControl";
            this._ucBaseAddinAutoHideControl.Owner = this.omsDockManager1;
            this._ucBaseAddinAutoHideControl.Size = new System.Drawing.Size(38, 605);
            this._ucBaseAddinAutoHideControl.TabIndex = 13;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Dock = System.Windows.Forms.DockStyle.Right;
            this.windowDockingArea1.Location = new System.Drawing.Point(522, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.omsDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(338, 605);
            this.windowDockingArea1.TabIndex = 14;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.ucDocumentPreviewer1);
            this.dockableWindow1.Location = new System.Drawing.Point(-10000, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.omsDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(333, 605);
            this.dockableWindow1.TabIndex = 15;
            // 
            // DocumentVersionsAddin
            // 
            this.Controls.Add(this._ucBaseAddinAutoHideControl);
            this.Controls.Add(this.versions);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaLeft);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaTop);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaBottom);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaRight);
            this.Name = "DocumentVersionsAddin";
            this.Size = new System.Drawing.Size(881, 605);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaRight, 0);
            this.Controls.SetChildIndex(this.windowDockingArea1, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaBottom, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaTop, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaLeft, 0);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.versions, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinAutoHideControl, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).EndInit();
            this._ucBaseAddinAutoHideControl.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private DocumentVersions versions;

        #endregion

        private ucDocumentPreviewer ucDocumentPreviewer1;
        private omsDockManager omsDockManager1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ucBaseAddinAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1;
        private Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1;

    }
}
