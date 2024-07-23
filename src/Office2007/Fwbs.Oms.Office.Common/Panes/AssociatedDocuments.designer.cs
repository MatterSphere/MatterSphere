namespace Fwbs.Oms.Office.Common.Panes
{
    partial class AssociatedDocumentsPane
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
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucPanelHistory = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavPanel2 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.eLastEmailsForAssociates1 = new FWBS.OMS.UI.Windows.eLastEmailsForAssociates();
            this.lblStatus = new System.Windows.Forms.Label();
            this.ucNavClientDetails = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.rtBox = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.ucPanelHistory.SuspendLayout();
            this.ucNavPanel2.SuspendLayout();
            this.ucNavClientDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucPanelHistory
            // 
            this.ucPanelHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelHistory.BlendColor1 = System.Drawing.Color.Empty;
            this.ucPanelHistory.BlendColor2 = System.Drawing.Color.Empty;
            this.ucPanelHistory.Controls.Add(this.ucNavPanel2);
            this.ucPanelHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelHistory.ExpandedHeight = 283;
            this.ucPanelHistory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucPanelHistory.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelHistory.Location = new System.Drawing.Point(5, 86);
            this.resourceLookup.SetLookup(this.ucPanelHistory, new FWBS.OMS.UI.Windows.ResourceLookupItem("HISTORY", "History", ""));
            this.ucPanelHistory.Name = "ucPanelHistory";
            this.ucPanelHistory.Size = new System.Drawing.Size(318, 283);
            this.ucPanelHistory.TabIndex = 11;
            this.ucPanelHistory.Text = "History";
            // 
            // ucNavPanel2
            // 
            this.ucNavPanel2.Controls.Add(this.eLastEmailsForAssociates1);
            this.ucNavPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel2.Location = new System.Drawing.Point(0, 24);
            this.ucNavPanel2.Name = "ucNavPanel2";
            this.ucNavPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavPanel2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel2.Size = new System.Drawing.Size(318, 252);
            this.ucNavPanel2.TabIndex = 15;
            // 
            // eLastEmailsForAssociates1
            // 
            this.eLastEmailsForAssociates1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eLastEmailsForAssociates1.DoubleClickAction = FWBS.OMS.UI.Windows.DoubleClickAction.Attach;
            this.eLastEmailsForAssociates1.Location = new System.Drawing.Point(5, 8);
            this.eLastEmailsForAssociates1.Name = "eLastEmailsForAssociates1";
            this.eLastEmailsForAssociates1.Size = new System.Drawing.Size(308, 239);
            this.eLastEmailsForAssociates1.TabIndex = 7;
            this.eLastEmailsForAssociates1.OpenClicked += new System.EventHandler<FWBS.OMS.UI.Windows.SelectedIDEventArgs>(this.eLastEmailsForAssociates1_OpenClicked);
            this.eLastEmailsForAssociates1.AttachClicked += new System.EventHandler<FWBS.OMS.UI.Windows.SelectedIDEventArgs>(this.eLastEmailsForAssociates1_AttachClicked);
            // 
            // lblStatus
            // 
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(5, 5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Padding = new System.Windows.Forms.Padding(2);
            this.lblStatus.Size = new System.Drawing.Size(318, 23);
            this.lblStatus.TabIndex = 3;
            // 
            // ucNavClientDetails
            // 
            this.ucNavClientDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucNavClientDetails.BlendColor1 = System.Drawing.Color.Empty;
            this.ucNavClientDetails.BlendColor2 = System.Drawing.Color.Empty;
            this.ucNavClientDetails.Controls.Add(this.rtBox);
            this.ucNavClientDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucNavClientDetails.ExpandedHeight = 58;
            this.ucNavClientDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucNavClientDetails.HeaderColor = System.Drawing.Color.Empty;
            this.ucNavClientDetails.Location = new System.Drawing.Point(5, 28);
            this.resourceLookup.SetLookup(this.ucNavClientDetails, new FWBS.OMS.UI.Windows.ResourceLookupItem("CLIENTDETAILS", "Client Details", ""));
            this.ucNavClientDetails.Name = "ucNavClientDetails";
            this.ucNavClientDetails.Size = new System.Drawing.Size(318, 58);
            this.ucNavClientDetails.TabIndex = 12;
            this.ucNavClientDetails.Text = "Client Details";
            // 
            // rtBox
            // 
            this.rtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtBox.Location = new System.Drawing.Point(0, 24);
            this.rtBox.Name = "rtBox";
            this.rtBox.Padding = new System.Windows.Forms.Padding(3);
            this.rtBox.PanelBackColor = System.Drawing.Color.Empty;
            this.rtBox.Rtf = "{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;}}\r\n\\viewkind4\\uc1\\pard\\la" +
    "ng2057\\f0\\fs18 richTextBox1\\par\r\n}\r\n";
            this.rtBox.Size = new System.Drawing.Size(318, 27);
            this.rtBox.TabIndex = 15;
            this.rtBox.Text = "richTextBox1";
            // 
            // AssociatedDocumentsPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.ucPanelHistory);
            this.Controls.Add(this.ucNavClientDetails);
            this.Controls.Add(this.lblStatus);
            this.Name = "AssociatedDocumentsPane";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(328, 600);
            this.ucPanelHistory.ResumeLayout(false);
            this.ucNavPanel2.ResumeLayout(false);
            this.ucNavClientDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelHistory;
        private FWBS.OMS.UI.Windows.ucNavPanel ucNavPanel2;
        private System.Windows.Forms.Label lblStatus;
        private FWBS.OMS.UI.Windows.ucPanelNav ucNavClientDetails;
        private FWBS.OMS.UI.Windows.ucNavRichText rtBox;
        protected FWBS.OMS.UI.Windows.eLastEmailsForAssociates eLastEmailsForAssociates1;

    }
}
