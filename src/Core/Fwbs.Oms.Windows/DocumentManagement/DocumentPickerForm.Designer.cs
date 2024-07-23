namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    partial class DocumentPickerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentPickerForm));
            this.pnlResults = new System.Windows.Forms.Panel();
            this.ucDocuments1 = new FWBS.OMS.UI.Windows.ucDocuments();
            this.lblDocID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtDocID = new System.Windows.Forms.TextBox();
            this.pnlNavContainer = new System.Windows.Forms.Panel();
            this.pnDetails = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.pnActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ncActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ncbChangeClientFile = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbChangeClient = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.pntSearchTypes = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ncSearchTypes = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ncbCurrentClient = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbCurrentClientFiles = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbAllClientFiles = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbLastUsed = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbCheckedOut = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbLocal = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.spSplitter = new FWBS.OMS.UI.Windows.omsSplitter();
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.info = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlResults.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlNavContainer.SuspendLayout();
            this.pnDetails.SuspendLayout();
            this.pnActions.SuspendLayout();
            this.ncActions.SuspendLayout();
            this.pntSearchTypes.SuspendLayout();
            this.ncSearchTypes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.info)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlResults
            // 
            this.pnlResults.Controls.Add(this.ucDocuments1);
            this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResults.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlResults.Location = new System.Drawing.Point(171, 35);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlResults.Size = new System.Drawing.Size(719, 447);
            this.pnlResults.TabIndex = 22;
            // 
            // ucDocuments1
            // 
            this.ucDocuments1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDocuments1.Location = new System.Drawing.Point(3, 0);
            this.ucDocuments1.Name = "ucDocuments1";
            this.ucDocuments1.Size = new System.Drawing.Size(713, 444);
            this.ucDocuments1.TabIndex = 0;
            this.ucDocuments1.ToBeRefreshed = false;
            // 
            // lblDocID
            // 
            this.lblDocID.Location = new System.Drawing.Point(4, 10);
            this.resLookup.SetLookup(this.lblDocID, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCUMENTID", "Document ID : ", ""));
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Size = new System.Drawing.Size(84, 14);
            this.lblDocID.TabIndex = 2;
            this.lblDocID.Text = "Document ID : ";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(561, 6);
            this.resLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(638, 6);
            this.resLookup.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSelect", "&Select", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&Select";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.txtDocID);
            this.pnlTop.Controls.Add(this.lblDocID);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTop.Location = new System.Drawing.Point(171, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.pnlTop.Size = new System.Drawing.Size(719, 35);
            this.pnlTop.TabIndex = 19;
            // 
            // txtDocID
            // 
            this.txtDocID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtDocID.Location = new System.Drawing.Point(89, 7);
            this.txtDocID.Name = "txtDocID";
            this.txtDocID.Size = new System.Drawing.Size(118, 23);
            this.txtDocID.TabIndex = 3;
            this.txtDocID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDocID_KeyDown);
            // 
            // pnlNavContainer
            // 
            this.pnlNavContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(122)))), ((int)(((byte)(215)))));
            this.pnlNavContainer.Controls.Add(this.pnDetails);
            this.pnlNavContainer.Controls.Add(this.pnActions);
            this.pnlNavContainer.Controls.Add(this.pntSearchTypes);
            this.pnlNavContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlNavContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlNavContainer.Name = "pnlNavContainer";
            this.pnlNavContainer.Padding = new System.Windows.Forms.Padding(6);
            this.pnlNavContainer.Size = new System.Drawing.Size(166, 482);
            this.pnlNavContainer.TabIndex = 20;
            // 
            // pnDetails
            // 
            this.pnDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.pnDetails.BlendColor1 = System.Drawing.Color.Empty;
            this.pnDetails.BlendColor2 = System.Drawing.Color.Empty;
            this.pnDetails.Controls.Add(this.ucNavRichText1);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDetails.Expanded = false;
            this.pnDetails.ExpandedHeight = 42;
            this.pnDetails.HeaderColor = System.Drawing.Color.Empty;
            this.pnDetails.Location = new System.Drawing.Point(6, 264);
            this.resLookup.SetLookup(this.pnDetails, new FWBS.OMS.UI.Windows.ResourceLookupItem("DETAILS", "Details", ""));
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(154, 42);
            this.pnDetails.TabIndex = 2;
            this.pnDetails.Text = "Details";
            this.pnDetails.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            this.pnDetails.Visible = false;
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Tahoma;}}\r\n" +
                                      "\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(154, 11);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // pnActions
            // 
            this.pnActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.pnActions.BlendColor1 = System.Drawing.Color.Empty;
            this.pnActions.BlendColor2 = System.Drawing.Color.Empty;
            this.pnActions.Controls.Add(this.ncActions);
            this.pnActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnActions.ExpandedHeight = 85;
            this.pnActions.HeaderColor = System.Drawing.Color.Empty;
            this.pnActions.Location = new System.Drawing.Point(6, 179);
            this.resLookup.SetLookup(this.pnActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("ACTIONS", "Actions", ""));
            this.pnActions.Name = "pnActions";
            this.pnActions.Size = new System.Drawing.Size(154, 85);
            this.pnActions.TabIndex = 1;
            this.pnActions.Text = "Actions";
            this.pnActions.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ncActions
            // 
            this.ncActions.Controls.Add(this.ncbChangeClientFile);
            this.ncActions.Controls.Add(this.ncbChangeClient);
            this.ncActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ncActions.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncActions.Location = new System.Drawing.Point(0, 24);
            this.ncActions.Name = "ncActions";
            this.ncActions.Padding = new System.Windows.Forms.Padding(5);
            this.ncActions.PanelBackColor = System.Drawing.Color.Empty;
            this.ncActions.Size = new System.Drawing.Size(154, 54);
            this.ncActions.TabIndex = 15;
            // 
            // ncbChangeClientFile
            // 
            this.ncbChangeClientFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbChangeClientFile.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbChangeClientFile.ImageIndex = 24;
            this.ncbChangeClientFile.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.ncbChangeClientFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHANGEFILE", "Change %FILE%", ""));
            this.ncbChangeClientFile.Name = "ncbChangeClientFile";
            this.ncbChangeClientFile.Size = new System.Drawing.Size(144, 22);
            this.ncbChangeClientFile.TabIndex = 0;
            this.ncbChangeClientFile.Text = "Change %FILE%";
            this.ncbChangeClientFile.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbChangeClient
            // 
            this.ncbChangeClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbChangeClient.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbChangeClient.ImageIndex = 24;
            this.ncbChangeClient.Location = new System.Drawing.Point(5, 27);
            this.resLookup.SetLookup(this.ncbChangeClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHANGECLIENT", "Change %CLIENT%", ""));
            this.ncbChangeClient.Name = "ncbChangeClient";
            this.ncbChangeClient.Size = new System.Drawing.Size(144, 22);
            this.ncbChangeClient.TabIndex = 1;
            this.ncbChangeClient.Text = "Change %CLIENT%";
            this.ncbChangeClient.Visible = false;
            this.ncbChangeClient.Click += new System.EventHandler(this.Link_Click);
            // 
            // pntSearchTypes
            // 
            this.pntSearchTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.pntSearchTypes.BlendColor1 = System.Drawing.Color.Empty;
            this.pntSearchTypes.BlendColor2 = System.Drawing.Color.Empty;
            this.pntSearchTypes.Controls.Add(this.ncSearchTypes);
            this.pntSearchTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pntSearchTypes.ExpandedHeight = 173;
            this.pntSearchTypes.HeaderBrightness = -10;
            this.pntSearchTypes.Location = new System.Drawing.Point(6, 6);
            this.resLookup.SetLookup(this.pntSearchTypes, new FWBS.OMS.UI.Windows.ResourceLookupItem("SEARCHTYPES", "Search Types", ""));
            this.pntSearchTypes.Name = "pntSearchTypes";
            this.pntSearchTypes.Size = new System.Drawing.Size(154, 173);
            this.pntSearchTypes.TabIndex = 0;
            this.pntSearchTypes.Text = "Search Types";
            this.pntSearchTypes.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ncSearchTypes
            // 
            this.ncSearchTypes.Controls.Add(this.ncbCurrentClient);
            this.ncSearchTypes.Controls.Add(this.ncbCurrentClientFiles);
            this.ncSearchTypes.Controls.Add(this.ncbAllClientFiles);
            this.ncSearchTypes.Controls.Add(this.ncbLastUsed);
            this.ncSearchTypes.Controls.Add(this.ncbCheckedOut);
            this.ncSearchTypes.Controls.Add(this.ncbLocal);
            this.ncSearchTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ncSearchTypes.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ncSearchTypes.Location = new System.Drawing.Point(0, 24);
            this.ncSearchTypes.Name = "ncSearchTypes";
            this.ncSearchTypes.Padding = new System.Windows.Forms.Padding(5);
            this.ncSearchTypes.PanelBackColor = System.Drawing.Color.Empty;
            this.ncSearchTypes.Size = new System.Drawing.Size(154, 142);
            this.ncSearchTypes.TabIndex = 15;
            // 
            // ncbCurrentClient
            // 
            this.ncbCurrentClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCurrentClient.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbCurrentClient.ImageIndex = 52;
            this.ncbCurrentClient.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.ncbCurrentClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CURRENTCLIENT", "Current %CLIENT%", ""));
            this.ncbCurrentClient.Name = "ncbCurrentClient";
            this.ncbCurrentClient.Size = new System.Drawing.Size(144, 22);
            this.ncbCurrentClient.TabIndex = 4;
            this.ncbCurrentClient.Text = "Current %CLIENT%";
            this.ncbCurrentClient.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbCurrentClientFiles
            // 
            this.ncbCurrentClientFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCurrentClientFiles.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbCurrentClientFiles.ImageIndex = 57;
            this.ncbCurrentClientFiles.Location = new System.Drawing.Point(5, 27);
            this.resLookup.SetLookup(this.ncbCurrentClientFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("CURRENTFILE", "Current %FILE%", ""));
            this.ncbCurrentClientFiles.Name = "ncbCurrentClientFiles";
            this.ncbCurrentClientFiles.Size = new System.Drawing.Size(144, 22);
            this.ncbCurrentClientFiles.TabIndex = 0;
            this.ncbCurrentClientFiles.Text = "Current %FILE%";
            this.ncbCurrentClientFiles.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbAllClientFiles
            // 
            this.ncbAllClientFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbAllClientFiles.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbAllClientFiles.ImageIndex = 51;
            this.ncbAllClientFiles.Location = new System.Drawing.Point(5, 49);
            this.resLookup.SetLookup(this.ncbAllClientFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("SEARCH", "Search", ""));
            this.ncbAllClientFiles.Name = "ncbAllClientFiles";
            this.ncbAllClientFiles.Size = new System.Drawing.Size(144, 22);
            this.ncbAllClientFiles.TabIndex = 1;
            this.ncbAllClientFiles.Text = "Search";
            this.ncbAllClientFiles.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbLastUsed
            // 
            this.ncbLastUsed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbLastUsed.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbLastUsed.ImageIndex = 20;
            this.ncbLastUsed.Location = new System.Drawing.Point(5, 71);
            this.resLookup.SetLookup(this.ncbLastUsed, new FWBS.OMS.UI.Windows.ResourceLookupItem("LASTOPENED", "Last Opened", ""));
            this.ncbLastUsed.Name = "ncbLastUsed";
            this.ncbLastUsed.Size = new System.Drawing.Size(144, 22);
            this.ncbLastUsed.TabIndex = 3;
            this.ncbLastUsed.Text = "Last Opened";
            this.ncbLastUsed.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbCheckedOut
            // 
            this.ncbCheckedOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCheckedOut.ImageIndex = 35;
            this.ncbCheckedOut.Location = new System.Drawing.Point(5, 93);
            this.resLookup.SetLookup(this.ncbCheckedOut, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHECKEDOUT", "Checked Out", ""));
            this.ncbCheckedOut.Name = "ncbCheckedOut";
            this.ncbCheckedOut.Size = new System.Drawing.Size(144, 22);
            this.ncbCheckedOut.TabIndex = 5;
            this.ncbCheckedOut.Text = "Checked Out";
            this.ncbCheckedOut.Click += new System.EventHandler(this.Link_Click);
            // 
            // ncbLocal
            // 
            this.ncbLocal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbLocal.ImageIndex = 0;
            this.ncbLocal.Location = new System.Drawing.Point(5, 115);
            this.resLookup.SetLookup(this.ncbLocal, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOCAL", "Local", ""));
            this.ncbLocal.Name = "ncbLocal";
            this.ncbLocal.Size = new System.Drawing.Size(144, 22);
            this.ncbLocal.TabIndex = 6;
            this.ncbLocal.Text = "Local";
            this.ncbLocal.Click += new System.EventHandler(this.Link_Click);
            // 
            // spSplitter
            // 
            this.spSplitter.BackColor = System.Drawing.SystemColors.Control;
            this.spSplitter.Location = new System.Drawing.Point(166, 0);
            this.spSplitter.Name = "spSplitter";
            this.spSplitter.Size = new System.Drawing.Size(5, 482);
            this.spSplitter.TabIndex = 21;
            this.spSplitter.TabStop = false;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 70;
            this.ucFormStorage1.DefaultPercentageWidth = 70;
            this.ucFormStorage1.FormToStore = null;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\DocumentPicker";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // info
            // 
            this.info.ContainerControl = this;
            this.info.Icon = ((System.Drawing.Icon)(resources.GetObject("info.Icon")));
            // 
            // DocumentPickerForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1200, 680);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.spSplitter);
            this.Controls.Add(this.pnlNavContainer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.resLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCPICKFORM", "Document Picker", ""));
            this.Name = "DocumentPickerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Picker";
            this.Load += new System.EventHandler(this.DocumentPickerForm_Load);
            this.pnlResults.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlNavContainer.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            this.pnActions.ResumeLayout(false);
            this.ncActions.ResumeLayout(false);
            this.pntSearchTypes.ResumeLayout(false);
            this.ncSearchTypes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.info)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbChangeClient;
        private FWBS.OMS.UI.Windows.ucNavCommands ncSearchTypes;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbCurrentClient;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbCurrentClientFiles;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbAllClientFiles;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbLastUsed;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbCheckedOut;
        private System.Windows.Forms.Panel pnlResults;
        private FWBS.OMS.UI.Windows.ucDocuments ucDocuments1;
        private FWBS.OMS.UI.Windows.ucNavCommands ncActions;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbChangeClientFile;
        private FWBS.OMS.UI.Windows.ucPanelNav pnActions;
        private FWBS.OMS.UI.Windows.ucPanelNav pntSearchTypes;
        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
        private System.Windows.Forms.Label lblDocID;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtDocID;
        private System.Windows.Forms.Panel pnlNavContainer;
        private FWBS.OMS.UI.Windows.omsSplitter spSplitter;
        private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private ucPanelNav pnDetails;
        private ucNavRichText ucNavRichText1;
        private ucNavCmdButtons ncbLocal;
        private System.Windows.Forms.ErrorProvider info;
    }
}