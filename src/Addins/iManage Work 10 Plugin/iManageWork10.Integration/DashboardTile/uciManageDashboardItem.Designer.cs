
using FWBS.OMS.UI.UserControls.Browsers;

namespace iManageWork10.Integration.DashboardTile
{
    partial class uciManageDashboardItem
    {
        private ucRichBrowserControl browser;
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
            this.browser = new FWBS.OMS.UI.UserControls.Browsers.ucRichBrowserControl();
            components = new System.ComponentModel.Container();
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            // 
            // uciManageDashboardItem
            // 
            this.Controls.Add(browser);
        }

        #endregion
    }
}
