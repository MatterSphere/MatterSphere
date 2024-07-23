
namespace FWBS.OMS.UI.Windows.Admin
{
    partial class ucDashboards
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkRegDashboardObj = new System.Windows.Forms.LinkLabel();
            this.pnlDashboardContainer = new System.Windows.Forms.Panel();
            this.splitter = new System.Windows.Forms.Splitter();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.BresourceLookup1.SetLookup(this.tpList, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpList", "List", ""));
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.pnlDashboardContainer);
            this.tpEdit.Controls.Add(this.splitter);
            this.tpEdit.Controls.Add(this.panel1);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel1, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlDashboardContainer, 0);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Size = new System.Drawing.Size(549, 11);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // lstList
            // 
            this.lstList.ButtonEnabledRulesApplied += new System.EventHandler(this.lstList_ButtonEnabledRulesAppliedDsh);
            this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.lstList_SearchButtonCommandsDsh);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(300, 296);
            this.propertyGrid.TabIndex = 201;
            this.propertyGrid.ToolbarVisible = false;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 296);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(300, 3);
            this.splitter1.TabIndex = 202;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.pnlActions);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(249, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 333);
            this.panel1.TabIndex = 203;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkRegDashboardObj);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 299);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(300, 34);
            this.pnlActions.TabIndex = 203;
            // 
            // lnkRegDashboardObj
            // 
            this.lnkRegDashboardObj.BackColor = System.Drawing.Color.White;
            this.lnkRegDashboardObj.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkRegDashboardObj.Enabled = false;
            this.lnkRegDashboardObj.Location = new System.Drawing.Point(3, 15);
            this.BresourceLookup1.SetLookup(this.lnkRegDashboardObj, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkRegDshbrdObj", "Register this Dashboard Object", ""));
            this.lnkRegDashboardObj.Name = "lnkRegDashboardObj";
            this.lnkRegDashboardObj.Size = new System.Drawing.Size(294, 16);
            this.lnkRegDashboardObj.TabIndex = 1;
            this.lnkRegDashboardObj.TabStop = true;
            this.lnkRegDashboardObj.Text = "Register this Dashboard Object";
            this.lnkRegDashboardObj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegDashboardObj_LinkClicked);
            // 
            // pnlDashboardContainer
            // 
            this.pnlDashboardContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDashboardContainer.Location = new System.Drawing.Point(0, 50);
            this.pnlDashboardContainer.Name = "pnlDashboardContainer";
            this.pnlDashboardContainer.Size = new System.Drawing.Size(246, 333);
            this.pnlDashboardContainer.TabIndex = 204;
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter.Location = new System.Drawing.Point(246, 50);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 333);
            this.splitter.TabIndex = 204;
            this.splitter.TabStop = false;
            // 
            // ucDashboards
            // 
            this.Name = "ucDashboards";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private Common.UI.Windows.eXPPanel pnlActions;
        private System.Windows.Forms.LinkLabel lnkRegDashboardObj;
        private System.Windows.Forms.Panel pnlDashboardContainer;
        private System.Windows.Forms.Splitter splitter;
    }
}
