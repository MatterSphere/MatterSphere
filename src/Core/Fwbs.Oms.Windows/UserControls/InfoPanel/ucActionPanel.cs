using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.InfoPanel
{
    partial class ucActionPanel : UserControl
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label Header;
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
        private System.Windows.Forms.Panel Line;
        private System.Windows.Forms.Panel ActionsContainer;
        private System.Windows.Forms.Panel VerticalLine;

        public ucActionPanel()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Header = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.Line = new System.Windows.Forms.Panel();
            this.ActionsContainer = new System.Windows.Forms.Panel();
            this.VerticalLine = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.Header.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Header.Location = new System.Drawing.Point(1, 0);
            this.resourceLookup1.SetLookup(this.Header, new FWBS.OMS.UI.Windows.ResourceLookupItem("INFORMATION", "Information", ""));
            this.Header.Name = "Header";
            this.Header.Padding = new System.Windows.Forms.Padding(10, 12, 0, 0);
            this.Header.Size = new System.Drawing.Size(149, 48);
            this.Header.TabIndex = 0;
            // 
            // Line
            // 
            this.Line.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.Line.Dock = System.Windows.Forms.DockStyle.Top;
            this.Line.Location = new System.Drawing.Point(1, 48);
            this.Line.Margin = new System.Windows.Forms.Padding(0);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(149, 1);
            this.Line.TabIndex = 1;
            // 
            // ActionsContainer
            // 
            this.ActionsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActionsContainer.Location = new System.Drawing.Point(1, 49);
            this.ActionsContainer.Margin = new System.Windows.Forms.Padding(0);
            this.ActionsContainer.Name = "ActionsContainer";
            this.ActionsContainer.Size = new System.Drawing.Size(149, 101);
            this.ActionsContainer.TabIndex = 2;
            // 
            // VerticalLine
            // 
            this.VerticalLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.VerticalLine.Dock = System.Windows.Forms.DockStyle.Left;
            this.VerticalLine.Location = new System.Drawing.Point(0, 0);
            this.VerticalLine.Name = "VerticalLine";
            this.VerticalLine.Size = new System.Drawing.Size(1, 150);
            this.VerticalLine.TabIndex = 3;
            // 
            // ucActionPanel
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.Controls.Add(this.ActionsContainer);
            this.Controls.Add(this.Line);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.VerticalLine);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucActionPanel";
            this.ResumeLayout(false);

        }

        #endregion

        public void SetActions(Panel infoPanel, int width)
        {
            ActionsContainer.Controls.Clear();
            this.Width = width;

            if (infoPanel != null)
            {
                ActionsContainer.Controls.Add(infoPanel);
                ActionsContainer.Visible = true;
            }
        }

        public bool ActionsVisible
        {
            get { return ActionsContainer.Visible; }
            set { ActionsContainer.Visible = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
