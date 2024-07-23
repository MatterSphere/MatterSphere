using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    partial class ucMatterCentricDashboard
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
            this.ucNavCmdButtonSelectMatter = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavRichTextSelectedMatter = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.ucPanelNavSelectedMatter = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.navCommands.SuspendLayout();
            this.ucPanelNavSelectedMatter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Controls.Add(this.ucPanelNavSelectedMatter);
            this.pnlDesign.Controls.SetChildIndex(this.pnlActions, 0);
            this.pnlDesign.Controls.SetChildIndex(this.ucPanelNavSelectedMatter, 0);
            // 
            // pnlActions
            // 
            this.pnlActions.ExpandedHeight = 46;
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Size = new System.Drawing.Size(152, 46);
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            this.navCommands.Controls.Add(this.ucNavCmdButtonSelectMatter);
            this.navCommands.Size = new System.Drawing.Size(152, 39);
            // 
            // ucNavCmdButtonSelectMatter
            // 
            this.ucNavCmdButtonSelectMatter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtonSelectMatter.ImageIndex = -1;
            this.ucNavCmdButtonSelectMatter.Location = new System.Drawing.Point(5, 12);
            this.resourceLookup1.SetLookup(this.ucNavCmdButtonSelectMatter, new FWBS.OMS.UI.Windows.ResourceLookupItem("SELECTFILE", "Select %FILE%", ""));
            this.ucNavCmdButtonSelectMatter.ModernStyle = true;
            this.ucNavCmdButtonSelectMatter.Name = "ucNavCmdButtonSelectMatter";
            this.ucNavCmdButtonSelectMatter.Size = new System.Drawing.Size(142, 22);
            this.ucNavCmdButtonSelectMatter.TabIndex = 0;
            this.ucNavCmdButtonSelectMatter.Text = "Select %FILE%";
            this.ucNavCmdButtonSelectMatter.Click += new System.EventHandler(this.ucNavCmdButtonSelectMatter_Click);
            // 
            // ucNavRichTextSelectedMatter
            // 
            this.ucNavRichTextSelectedMatter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichTextSelectedMatter.Location = new System.Drawing.Point(0, 0);
            this.ucNavRichTextSelectedMatter.ModernStyle = true;
            this.ucNavRichTextSelectedMatter.Name = "ucNavRichTextSelectedMatter";
            this.ucNavRichTextSelectedMatter.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;}}" +
    "\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichTextSelectedMatter.Size = new System.Drawing.Size(152, 51);
            this.ucNavRichTextSelectedMatter.TabIndex = 15;
            // 
            // ucPanelNavSelectedMatter
            // 
            this.ucPanelNavSelectedMatter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(217)))), ((int)(((byte)(238)))));
            this.ucPanelNavSelectedMatter.Brightness = 70;
            this.ucPanelNavSelectedMatter.Controls.Add(this.ucNavRichTextSelectedMatter);
            this.ucPanelNavSelectedMatter.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavSelectedMatter.ExpandedHeight = 58;
            this.ucPanelNavSelectedMatter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ucPanelNavSelectedMatter.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNavSelectedMatter.Location = new System.Drawing.Point(8, 54);
            this.resourceLookup1.SetLookup(this.ucPanelNavSelectedMatter, new FWBS.OMS.UI.Windows.ResourceLookupItem("SELECTEDFILE", "Selected %FILE%", ""));
            this.ucPanelNavSelectedMatter.ModernStyle = FWBS.OMS.UI.Windows.ucPanelNav.NavStyle.NoHeader;
            this.ucPanelNavSelectedMatter.Name = "ucPanelNavSelectedMatter";
            this.ucPanelNavSelectedMatter.Size = new System.Drawing.Size(152, 58);
            this.ucPanelNavSelectedMatter.TabIndex = 2;
            this.ucPanelNavSelectedMatter.TabStop = false;
            this.ucPanelNavSelectedMatter.Text = "Selected %FILE%";
            // 
            // ucMatterCentricDashboard
            // 
            this.Name = "ucMatterCentricDashboard";
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.navCommands.ResumeLayout(false);
            this.ucPanelNavSelectedMatter.ResumeLayout(false);
            this.ucPanelNavSelectedMatter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.ucNavCmdButtons ucNavCmdButtonSelectMatter;
        private FWBS.OMS.UI.Windows.ucNavRichText ucNavRichTextSelectedMatter;
        private ucPanelNav ucPanelNavSelectedMatter;
    }
}
