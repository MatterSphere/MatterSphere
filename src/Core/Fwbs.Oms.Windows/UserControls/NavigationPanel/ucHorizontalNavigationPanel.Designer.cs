using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    partial class ucHorizontalNavigationPanel
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
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.FlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.moreButton = new FWBS.Common.UI.Windows.RightImageMenuItem();
            this.SuspendLayout();
            // 
            // FlowLayoutPanel
            // 
            this.FlowLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.FlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FlowLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.FlowLayoutPanel.Location = new System.Drawing.Point(0, 1);
            this.FlowLayoutPanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.FlowLayoutPanel.Name = "FlowLayoutPanel";
            this.FlowLayoutPanel.Size = new System.Drawing.Size(585, 40);
            this.FlowLayoutPanel.TabIndex = 0;
            this.FlowLayoutPanel.TabStop = true;
            this.FlowLayoutPanel.VisibleChanged += new System.EventHandler(this.FlowLayoutPanel_VisibleChanged);
            // 
            // moreButton
            // 
            this.moreButton.AutoSize = true;
            this.moreButton.BackColor = System.Drawing.Color.Transparent;
            this.moreButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.moreButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.moreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moreButton.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.moreButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.moreButton.Location = new System.Drawing.Point(585, 1);
            this.resLookup.SetLookup(this.moreButton, new FWBS.OMS.UI.Windows.ResourceLookupItem("MORE", "More", ""));
            this.moreButton.Margin = new System.Windows.Forms.Padding(0);
            this.moreButton.Name = "moreButton";
            this.moreButton.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.moreButton.PreferredHeight = 40;
            this.moreButton.Size = new System.Drawing.Size(70, 40);
            this.moreButton.TabIndex = 1;
            this.moreButton.Text = "More";
            this.moreButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.moreButton.UseVisualStyleBackColor = false;
            this.moreButton.Click += new System.EventHandler(this.moreButton_Click);
            // 
            // ucHorizontalNavigationPanel
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.FlowLayoutPanel);
            this.Controls.Add(this.moreButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(0, 42);
            this.Name = "ucHorizontalNavigationPanel";
            this.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.Size = new System.Drawing.Size(655, 42);
            this.SizeChanged += new System.EventHandler(this.Size_Changed);
            this.VisibleChanged += new System.EventHandler(this.ucHorizontalNavigationPanel_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
        public System.Windows.Forms.FlowLayoutPanel FlowLayoutPanel;
        private RightImageMenuItem moreButton;
    }
}
