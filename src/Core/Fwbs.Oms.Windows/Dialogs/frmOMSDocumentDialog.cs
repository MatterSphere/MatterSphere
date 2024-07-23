using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.DocumentManagement;
using FWBS.OMS.UI.Windows.DocumentManagement.Addins;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmOMSDialog.
    /// </summary>
    internal class frmOMSDocumentDialog : frmNewBrandIdent
    {
        #region Fields

        private string _oldtitle = "";
        /// <summary>
        /// A possible controlling application reference.
        /// </summary>
        private FWBS.OMS.Interfaces.IOMSApp _controlApp = null;

        private Client current_client;
        private OMSFile current_file;

        #endregion

        #region Controls

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TextBox txtDocID;
        private System.Windows.Forms.Label lblDocID;
        private System.Windows.Forms.Panel pnlNavContainer;
        private FWBS.OMS.UI.Windows.ucPanelNav pnActions;
        private FWBS.OMS.UI.Windows.ucNavCommands ncActions;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbChangeClientFile;
        private FWBS.OMS.UI.Windows.ucPanelNavTop pntSearchTypes;
        private FWBS.OMS.UI.Windows.ucNavCommands ncSearchTypes;
        private FWBS.OMS.UI.Windows.omsSplitter spSplitter;
        private System.Windows.Forms.Panel pnlResults;
        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;
        private FWBS.OMS.UI.Windows.ucDocuments ucDocuments1;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbCurrentClientFiles;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbAllClientFiles;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbLastUsed;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbLastUpdated;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbCurrentClient;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ncbChangeClient;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnManual;
        public System.Windows.Forms.OpenFileDialog ManualDialog;
        protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private ucNavCmdButtons ncbCheckedOut;
        private ucPanelNav pnDetails;
        private ucNavRichText ucNavRichText1;
        private ucNavCmdButtons ncbLocal;
        private ucPanelNav pnlGrouping;
        private ucNavPanel ucNavPanel2;
        private ucDocumentGrouping DocumentGrouping;
        private ErrorProvider info;

        #endregion

        #region Constructors

        internal frmOMSDocumentDialog(FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            InitializeComponent();
            SetImages();
            SetIcon(Images.DialogIcons.Document);

            if (current_client == null)
                current_client = Session.CurrentSession.CurrentClient;

            if (current_file == null)
                current_file = Session.CurrentSession.CurrentFile;

            _controlApp = controlApp;
            btnManual.Enabled = (_controlApp != null);

            //Initialise the documents addin.
            ucDocuments1.InitialiseHost(DocumentAddinHost.OpenDialog);

            ncbAllClientFiles.Visible = ucDocuments1.SupportsView(DocumentPickerType.Search);
            ncbCheckedOut.Visible = ucDocuments1.SupportsView(DocumentPickerType.CheckedOut);
            ncbCurrentClientFiles.Visible = ucDocuments1.SupportsView(DocumentPickerType.File);
            ncbLastUsed.Visible = ucDocuments1.SupportsView(DocumentPickerType.Latest);
            ncbLocal.Visible = ucDocuments1.SupportsView(DocumentPickerType.Local);
            ncbCurrentClient.Visible = ucDocuments1.SupportsView(DocumentPickerType.Client);
            ncbLastUpdated.Visible = ucDocuments1.SupportsView(DocumentPickerType.LatestUpdate);
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOMSDocumentDialog));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnManual = new System.Windows.Forms.Button();
            this.txtDocID = new System.Windows.Forms.TextBox();
            this.lblDocID = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlNavContainer = new System.Windows.Forms.Panel();
            this.pnDetails = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.pnlGrouping = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavPanel2 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.DocumentGrouping = new FWBS.OMS.UI.Windows.DocumentManagement.ucDocumentGrouping();
            this.pnActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ncActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ncbChangeClientFile = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbChangeClient = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.pntSearchTypes = new FWBS.OMS.UI.Windows.ucPanelNavTop();
            this.ncSearchTypes = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ncbCurrentClient = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbCurrentClientFiles = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbAllClientFiles = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbLastUsed = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbLastUpdated = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbCheckedOut = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ncbLocal = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.pnlResults = new System.Windows.Forms.Panel();
            this.ucDocuments1 = new FWBS.OMS.UI.Windows.ucDocuments();
            this.ManualDialog = new System.Windows.Forms.OpenFileDialog();
            this.spSplitter = new FWBS.OMS.UI.Windows.omsSplitter();
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.info = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlNavContainer.SuspendLayout();
            this.pnDetails.SuspendLayout();
            this.pnlGrouping.SuspendLayout();
            this.ucNavPanel2.SuspendLayout();
            this.pnActions.SuspendLayout();
            this.ncActions.SuspendLayout();
            this.pntSearchTypes.SuspendLayout();
            this.ncSearchTypes.SuspendLayout();
            this.pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.info)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnManual);
            this.pnlTop.Controls.Add(this.txtDocID);
            this.pnlTop.Controls.Add(this.lblDocID);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTop.Location = new System.Drawing.Point(185, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.pnlTop.Size = new System.Drawing.Size(639, 37);
            this.pnlTop.TabIndex = 0;
            // 
            // btnManual
            // 
            this.btnManual.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnManual.Location = new System.Drawing.Point(226, 6);
            this.resLookup.SetLookup(this.btnManual, new FWBS.OMS.UI.Windows.ResourceLookupItem("MANUAL", "&Manual", ""));
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(75, 24);
            this.btnManual.TabIndex = 4;
            this.btnManual.Text = "&Manual";
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // txtDocID
            // 
            this.txtDocID.Location = new System.Drawing.Point(97, 7);
            this.txtDocID.Name = "txtDocID";
            this.txtDocID.Size = new System.Drawing.Size(118, 23);
            this.txtDocID.TabIndex = 3;
            this.txtDocID.TextChanged += new System.EventHandler(this.txtDocID_TextChanged);
            this.txtDocID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocID_KeyPress);
            // 
            // lblDocID
            // 
            this.lblDocID.Location = new System.Drawing.Point(4, 11);
            this.resLookup.SetLookup(this.lblDocID, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCUMENTID", "Document ID : ", ""));
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Size = new System.Drawing.Size(87, 15);
            this.lblDocID.TabIndex = 2;
            this.lblDocID.Text = "Document ID : ";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(481, 6);
            this.resLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Enabled = false;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(558, 6);
            this.resLookup.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlNavContainer
            // 
            this.pnlNavContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlNavContainer.Controls.Add(this.pnDetails);
            this.pnlNavContainer.Controls.Add(this.pnlGrouping);
            this.pnlNavContainer.Controls.Add(this.pnActions);
            this.pnlNavContainer.Controls.Add(this.pntSearchTypes);
            this.pnlNavContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlNavContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlNavContainer.Name = "pnlNavContainer";
            this.pnlNavContainer.Padding = new System.Windows.Forms.Padding(6);
            this.pnlNavContainer.Size = new System.Drawing.Size(180, 541);
            this.pnlNavContainer.TabIndex = 1;
            // 
            // pnDetails
            // 
            this.pnDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnDetails.Controls.Add(this.ucNavRichText1);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDetails.Expanded = false;
            this.pnDetails.ExpandedHeight = 42;
            this.pnDetails.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnDetails.HeaderColor = System.Drawing.Color.Empty;
            this.pnDetails.Location = new System.Drawing.Point(6, 466);
            this.resLookup.SetLookup(this.pnDetails, new FWBS.OMS.UI.Windows.ResourceLookupItem("DETAILS", "Details", ""));
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(168, 42);
            this.pnDetails.TabIndex = 2;
            this.pnDetails.TabStop = false;
            this.pnDetails.Text = "Details";
            this.pnDetails.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;" +
    "}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(168, 11);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // pnlGrouping
            // 
            this.pnlGrouping.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlGrouping.Controls.Add(this.ucNavPanel2);
            this.pnlGrouping.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrouping.ExpandedHeight = 160;
            this.pnlGrouping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlGrouping.HeaderColor = System.Drawing.Color.Empty;
            this.pnlGrouping.Location = new System.Drawing.Point(6, 306);
            this.pnlGrouping.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.pnlGrouping.Name = "pnlGrouping";
            this.pnlGrouping.Size = new System.Drawing.Size(168, 160);
            this.pnlGrouping.TabIndex = 5;
            this.pnlGrouping.TabStop = false;
            this.pnlGrouping.Text = "Document Filtering";
            this.pnlGrouping.VisibleChanged += new System.EventHandler(this.pnlGrouping_VisibleChanged);
            // 
            // ucNavPanel2
            // 
            this.ucNavPanel2.Controls.Add(this.DocumentGrouping);
            this.ucNavPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel2.Location = new System.Drawing.Point(0, 24);
            this.ucNavPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.ucNavPanel2.Name = "ucNavPanel2";
            this.ucNavPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ucNavPanel2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel2.Size = new System.Drawing.Size(168, 129);
            this.ucNavPanel2.TabIndex = 15;
            // 
            // DocumentGrouping
            // 
            this.DocumentGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DocumentGrouping.Grouping = "DOCGROUPING";
            this.DocumentGrouping.Location = new System.Drawing.Point(0, 0);
            this.DocumentGrouping.Margin = new System.Windows.Forms.Padding(0);
            this.DocumentGrouping.Name = "DocumentGrouping";
            this.DocumentGrouping.Size = new System.Drawing.Size(168, 128);
            this.DocumentGrouping.TabIndex = 9;
            // 
            // pnActions
            // 
            this.pnActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnActions.Controls.Add(this.ncActions);
            this.pnActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnActions.ExpandedHeight = 88;
            this.pnActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnActions.HeaderColor = System.Drawing.Color.Empty;
            this.pnActions.Location = new System.Drawing.Point(6, 218);
            this.resLookup.SetLookup(this.pnActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("ACTIONS", "Actions", ""));
            this.pnActions.Name = "pnActions";
            this.pnActions.Size = new System.Drawing.Size(168, 88);
            this.pnActions.TabIndex = 1;
            this.pnActions.TabStop = false;
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
            this.ncActions.Size = new System.Drawing.Size(168, 57);
            this.ncActions.TabIndex = 15;
            this.ncActions.TabStop = false;
            // 
            // ncbChangeClientFile
            // 
            this.ncbChangeClientFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbChangeClientFile.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbChangeClientFile.ImageIndex = 24;
            this.ncbChangeClientFile.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.ncbChangeClientFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("CNGCLIENTFILE", "Change %CLIENT% & %FILE%", ""));
            this.ncbChangeClientFile.Name = "ncbChangeClientFile";
            this.ncbChangeClientFile.Size = new System.Drawing.Size(158, 23);
            this.ncbChangeClientFile.TabIndex = 0;
            this.ncbChangeClientFile.Text = "Change %CLIENT% & %FILE%";
            this.ncbChangeClientFile.Click += new System.EventHandler(this.ncbChangeClientFile_Click);
            // 
            // ncbChangeClient
            // 
            this.ncbChangeClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbChangeClient.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbChangeClient.ImageIndex = 24;
            this.ncbChangeClient.Location = new System.Drawing.Point(5, 28);
            this.resLookup.SetLookup(this.ncbChangeClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHANGECLIENT", "Change %CLIENT%", ""));
            this.ncbChangeClient.Name = "ncbChangeClient";
            this.ncbChangeClient.Size = new System.Drawing.Size(158, 24);
            this.ncbChangeClient.TabIndex = 1;
            this.ncbChangeClient.Text = "Change %CLIENT%";
            this.ncbChangeClient.Visible = false;
            this.ncbChangeClient.LinkClicked += new System.EventHandler(this.ncbChangeClient_LinkClicked);
            // 
            // pntSearchTypes
            // 
            this.pntSearchTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pntSearchTypes.Controls.Add(this.ncSearchTypes);
            this.pntSearchTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pntSearchTypes.ExpandedHeight = 212;
            this.pntSearchTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pntSearchTypes.HeaderBrightness = -10;
            this.pntSearchTypes.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pntSearchTypes.Image = null;
            this.pntSearchTypes.Location = new System.Drawing.Point(6, 6);
            this.resLookup.SetLookup(this.pntSearchTypes, new FWBS.OMS.UI.Windows.ResourceLookupItem("SEARCHTYPES", "Search Types", ""));
            this.pntSearchTypes.Name = "pntSearchTypes";
            this.pntSearchTypes.Size = new System.Drawing.Size(168, 212);
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
            this.ncSearchTypes.Controls.Add(this.ncbLastUpdated);
            this.ncSearchTypes.Controls.Add(this.ncbCheckedOut);
            this.ncSearchTypes.Controls.Add(this.ncbLocal);
            this.ncSearchTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ncSearchTypes.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ncSearchTypes.Location = new System.Drawing.Point(0, 32);
            this.ncSearchTypes.Name = "ncSearchTypes";
            this.ncSearchTypes.Padding = new System.Windows.Forms.Padding(5);
            this.ncSearchTypes.PanelBackColor = System.Drawing.Color.Empty;
            this.ncSearchTypes.Size = new System.Drawing.Size(168, 173);
            this.ncSearchTypes.TabIndex = 15;
            this.ncSearchTypes.TabStop = false;
            // 
            // ncbCurrentClient
            // 
            this.ncbCurrentClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCurrentClient.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbCurrentClient.ImageIndex = 52;
            this.ncbCurrentClient.Location = new System.Drawing.Point(5, 5);
            this.resLookup.SetLookup(this.ncbCurrentClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("CURRENTCLIENT", "Current %CLIENT%", ""));
            this.ncbCurrentClient.Name = "ncbCurrentClient";
            this.ncbCurrentClient.Size = new System.Drawing.Size(158, 23);
            this.ncbCurrentClient.TabIndex = 4;
            this.ncbCurrentClient.Text = "Current %CLIENT%";
            this.ncbCurrentClient.LinkClicked += new System.EventHandler(this.ncbCurrentClient_LinkClicked);
            // 
            // ncbCurrentClientFiles
            // 
            this.ncbCurrentClientFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCurrentClientFiles.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbCurrentClientFiles.ImageIndex = 57;
            this.ncbCurrentClientFiles.Location = new System.Drawing.Point(5, 28);
            this.resLookup.SetLookup(this.ncbCurrentClientFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("CURFILECLIENT", "Current %CLIENT% & %FILE%", ""));
            this.ncbCurrentClientFiles.Name = "ncbCurrentClientFiles";
            this.ncbCurrentClientFiles.Size = new System.Drawing.Size(158, 24);
            this.ncbCurrentClientFiles.TabIndex = 0;
            this.ncbCurrentClientFiles.Text = "Current %CLIENT% & %FILE%";
            this.ncbCurrentClientFiles.LinkClicked += new System.EventHandler(this.ncbCurrentClientFiles_LinkClicked);
            // 
            // ncbAllClientFiles
            // 
            this.ncbAllClientFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbAllClientFiles.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbAllClientFiles.ImageIndex = 51;
            this.ncbAllClientFiles.Location = new System.Drawing.Point(5, 52);
            this.resLookup.SetLookup(this.ncbAllClientFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("ALLCLIENTSFILES", "All %CLIENTS% & %FILES%", ""));
            this.ncbAllClientFiles.Name = "ncbAllClientFiles";
            this.ncbAllClientFiles.Size = new System.Drawing.Size(158, 23);
            this.ncbAllClientFiles.TabIndex = 1;
            this.ncbAllClientFiles.Text = "All %CLIENTS% & %FILES%";
            this.ncbAllClientFiles.LinkClicked += new System.EventHandler(this.ncbAllClientFiles_LinkClicked);
            // 
            // ncbLastUsed
            // 
            this.ncbLastUsed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbLastUsed.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbLastUsed.ImageIndex = 20;
            this.ncbLastUsed.Location = new System.Drawing.Point(5, 75);
            this.resLookup.SetLookup(this.ncbLastUsed, new FWBS.OMS.UI.Windows.ResourceLookupItem("LASTCREATED", "Last Created", ""));
            this.ncbLastUsed.Name = "ncbLastUsed";
            this.ncbLastUsed.Size = new System.Drawing.Size(158, 23);
            this.ncbLastUsed.TabIndex = 3;
            this.ncbLastUsed.Text = "Last Created";
            this.ncbLastUsed.Click += new System.EventHandler(this.ncbLastUsed_Click);
            // 
            // ncbLastUpdated
            // 
            this.ncbLastUpdated.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbLastUpdated.ForeColor = System.Drawing.SystemColors.Highlight;
            this.ncbLastUpdated.ImageIndex = 20;
            this.ncbLastUpdated.Location = new System.Drawing.Point(5, 98);
            this.resLookup.SetLookup(this.ncbLastUpdated, new FWBS.OMS.UI.Windows.ResourceLookupItem("LASTUPDATED", "Last Updated", ""));
            this.ncbLastUpdated.Name = "ncbLastUpdated";
            this.ncbLastUpdated.Size = new System.Drawing.Size(158, 23);
            this.ncbLastUpdated.TabIndex = 3;
            this.ncbLastUpdated.Text = "Last Updated";
            this.ncbLastUpdated.Click += new System.EventHandler(this.ncbLastUpdated_Click);
            // 
            // ncbCheckedOut
            // 
            this.ncbCheckedOut.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbCheckedOut.ImageIndex = 35;
            this.ncbCheckedOut.Location = new System.Drawing.Point(5, 121);
            this.resLookup.SetLookup(this.ncbCheckedOut, new FWBS.OMS.UI.Windows.ResourceLookupItem("CHECKEDOUT", "Checked Out", ""));
            this.ncbCheckedOut.Name = "ncbCheckedOut";
            this.ncbCheckedOut.Size = new System.Drawing.Size(158, 24);
            this.ncbCheckedOut.TabIndex = 5;
            this.ncbCheckedOut.Text = "Checked Out";
            this.ncbCheckedOut.Click += new System.EventHandler(this.ncbCheckedOut_Click);
            // 
            // ncbLocal
            // 
            this.ncbLocal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ncbLocal.ImageIndex = 0;
            this.ncbLocal.Location = new System.Drawing.Point(5, 145);
            this.resLookup.SetLookup(this.ncbLocal, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOCAL", "Local", ""));
            this.ncbLocal.Name = "ncbLocal";
            this.ncbLocal.Size = new System.Drawing.Size(158, 23);
            this.ncbLocal.TabIndex = 6;
            this.ncbLocal.Text = "Local";
            this.ncbLocal.Click += new System.EventHandler(this.ncbLocal_Click);
            // 
            // pnlResults
            // 
            this.pnlResults.Controls.Add(this.ucDocuments1);
            this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResults.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlResults.Location = new System.Drawing.Point(185, 37);
            this.pnlResults.Name = "pnlResults";
            this.pnlResults.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlResults.Size = new System.Drawing.Size(639, 504);
            this.pnlResults.TabIndex = 18;
            // 
            // ucDocuments1
            // 
            this.ucDocuments1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDocuments1.Location = new System.Drawing.Point(3, 0);
            this.ucDocuments1.Name = "ucDocuments1";
            this.ucDocuments1.Size = new System.Drawing.Size(633, 501);
            this.ucDocuments1.TabIndex = 0;
            this.ucDocuments1.DocumentSelecting += new System.EventHandler(this.ucDocuments1_DocumentSelecting);
            this.ucDocuments1.DocumentsRefreshed += new System.EventHandler(this.ucDocuments1_DocumentsRefreshed);
            this.ucDocuments1.Leave += new System.EventHandler(this.ucDocuments1_Leave);
            // 
            // ManualDialog
            // 
            this.ManualDialog.Filter = "Word Documents|*.doc|All Files|*.*";
            this.ManualDialog.Title = "Manual Open";
            // 
            // spSplitter
            // 
            this.spSplitter.BackColor = System.Drawing.SystemColors.Control;
            this.spSplitter.Location = new System.Drawing.Point(180, 0);
            this.spSplitter.Name = "spSplitter";
            this.spSplitter.Size = new System.Drawing.Size(5, 541);
            this.spSplitter.TabIndex = 17;
            this.spSplitter.TabStop = false;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 70;
            this.ucFormStorage1.DefaultPercentageWidth = 70;
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\OpenDocument";
            this.ucFormStorage1.Version = ((long)(1));
            // 
            // info
            // 
            this.info.ContainerControl = this;
            this.info.Icon = ((System.Drawing.Icon)(resources.GetObject("info.Icon")));
            // 
            // frmOMSDocumentDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(824, 541);
            this.Controls.Add(this.pnlResults);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.spSplitter);
            this.Controls.Add(this.pnlNavContainer);
            this.resLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("OMSDOCDIALOG", "OMS Document Dialog", ""));
            this.Name = "frmOMSDocumentDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OMS Document Dialog";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlNavContainer.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            this.pnlGrouping.ResumeLayout(false);
            this.ucNavPanel2.ResumeLayout(false);
            this.pnActions.ResumeLayout(false);
            this.ncActions.ResumeLayout(false);
            this.pntSearchTypes.ResumeLayout(false);
            this.ncSearchTypes.ResumeLayout(false);
            this.pnlResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.info)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion

        #region Methods

        private void SetImages()
        {
            pntSearchTypes.Image = null;

            this.ncSearchTypes.Resources = ImageListSelector.GetOMSImageList();
            this.ncActions.Resources = ImageListSelector.GetOMSImageList();
        }

        private void SetOpenFilter()
        {
            this.ManualDialog.Filter = "All Files|*.*";

            if (_controlApp != null)
            {
                string addfilter = _controlApp.GetOpenFileFilter();
                if (!String.IsNullOrEmpty(addfilter))
                    this.ManualDialog.Filter = addfilter + "|" + this.ManualDialog.Filter;
            }

        }

        private void SetClientDescription()
        {
            if (current_client != null)
            {
                if (pnDetails.Expanded == false)
                    pnDetails.Expanded = true;

                try
                {
                    this.ucNavRichText1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(current_client.ClientDescription);
                }
                catch
                {
                    this.ucNavRichText1.Text = current_client.ClientDescription;
                }

                this.ucNavRichText1.Refresh();
            }
        }

        private void ResetTitle()
        {
            this.Text = _oldtitle;
            this.Text = _oldtitle;
        }

        private void SetTitle(string text)
        {
            // This is done twice to counter act the SetResource Component
            this.Text = _oldtitle + " - " + text;
            this.Text = _oldtitle + " - " + text;
        }


        private void ApplyView(DocumentPickerType type)
        {
            ncSearchTypes.Enabled = false;
            try
            {
                if (ucDocuments1.SupportsView(type))
                {
                    switch (type)
                    {
                        case DocumentPickerType.CheckedOut:
                            ResetToggleTo(ncbCheckedOut);
                            ApplyCheckedOutDocumentView();
                            break;
                        case DocumentPickerType.Client:
                            ResetToggleTo(ncbCurrentClient);
                            if (current_client != null || ChangeClient())
                                ApplyClientDocumentView();
                            else
                            {
                                HideView();
                                ncSearchTypes.Enabled = true;
                                return;
                            }
                            break;
                        case DocumentPickerType.File:
                            ResetToggleTo(ncbCurrentClientFiles);
                            if (current_file != null || ChangeFile())
                                ApplyCurrentFileDocumentView();
                            else
                            {
                                HideView();
                                ncSearchTypes.Enabled = true;
                                return;
                            }
                            break;
                        case DocumentPickerType.Latest:
                            ResetToggleTo(ncbLastUsed);
                            ApplyLastestDocumentView();
                            break;
                        case DocumentPickerType.LatestUpdate:
                            ResetToggleTo(ncbLastUpdated);
                            ApplyLatestUpdateView();
                            break;
                        case DocumentPickerType.Search:
                            ResetToggleTo(ncbAllClientFiles);
                            ApplySearchDocumentView();
                            break;
                        case DocumentPickerType.Local:
                            ResetToggleTo(ncbLocal);
                            ApplyLocalDocumentView();
                            break;
                    }

                    ShowView();
                }
                else
                    HideView();
            }
            catch
            {
                ncSearchTypes.Enabled = true;
                throw;
            }
        }

        private void ShowView()
        {
            if (!ucDocuments1.Visible)
                ucDocuments1.Visible = true;
        }

        private void HideView()
        {
            ucDocuments1.Visible = false;
        }

        private void ApplyViewAppearance(DocumentPickerType type, FWBS.OMS.Interfaces.IOMSType obj, string title, bool expandActions, bool changeClient, bool changeFile, bool actionsVisible)
        {
            if (string.IsNullOrEmpty(title))
                ResetTitle();
            else
                SetTitle(title);

            pnActions.Expanded = expandActions;
            ucDocuments1.ShowView(type, obj);
            SetClientDescription();
            ncbChangeClient.Visible = changeClient;
            ncbChangeClientFile.Visible = changeFile;
            ncActions.Refresh();
            pnActions.Visible = actionsVisible;

            populatedGroups.Clear();

            if (ucDocuments1.UIElement is ucBuiltInDocuments)
            {

                DocumentGrouping.Initialise();
                pnlGrouping.Visible = DocumentGrouping.FoundGroups;
                if (DocumentGrouping.FoundGroups)
                {
                    DocumentGrouping.GroupingChanged -= new ValueChangedEventHandler(DocumentGrouping_GroupingChanged);
                    DocumentGrouping.GroupingChanged += new ValueChangedEventHandler(DocumentGrouping_GroupingChanged);
                    DocumentGrouping.FilterBuilt -= new EventHandler(DocumentGrouping_FilterBuilt);
                    DocumentGrouping.FilterBuilt += new EventHandler(DocumentGrouping_FilterBuilt);
                }
            }
            else
                pnlGrouping.Visible = false;

        }

        void DocumentGrouping_FilterBuilt(object sender, EventArgs e)
        {
            ucBuiltInDocuments builtindocs = ucDocuments1.UIElement as ucBuiltInDocuments;

            if (builtindocs != null)
                builtindocs.SetExternalFilter(DocumentGrouping.Filter);
        }

        private Dictionary<string, List<object>> populatedGroups = new Dictionary<string, List<object>>();

        void DocumentGrouping_GroupingChanged(object sender, ValueChangedEventArgs e)
        {
            string column = e.ProposedValue as string;

            if (populatedGroups.ContainsKey(column))
            {
                DocumentGrouping.SetGroups(populatedGroups[column], column);
                return;
            }

            ucBuiltInDocuments builtindocs = ucDocuments1.UIElement as ucBuiltInDocuments;

            if (builtindocs == null)
                return;

            List<object> groups = builtindocs.GetGroupElements(column);

            populatedGroups.Add(column, groups);
            DocumentGrouping.SetGroups(groups, column);

        }


        private void ApplyLastestDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.Latest, null, null, false, false, false, false);
        }

        private void ApplyLatestUpdateView()
        {
            ApplyViewAppearance(DocumentPickerType.LatestUpdate, null, null, false, false, false, false);
        }

        private void ApplyCurrentFileDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.File, current_file, current_file.ToString(), true, false, true, true);
        }

        private void ApplySearchDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.Search, null, null, false, false, false, false);
        }

        private void ApplyClientDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.Client, current_client, current_client.ToString(), true, true, false, true);
        }

        private void ApplyCheckedOutDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.CheckedOut, null, null, false, false, false, false);
        }

        private void ApplyLocalDocumentView()
        {
            ApplyViewAppearance(DocumentPickerType.Local, null, null, false, false, false, false);
        }

        private bool ChangeClient()
        {
            Client cl = FWBS.OMS.UI.Windows.Services.SelectClient();
            if (cl != null)
            {
                current_client = cl;
                return true;
            }
            else
                return false;
        }

        private bool ChangeFile()
        {
            OMSFile file = FWBS.OMS.UI.Windows.Services.SelectFile();
            if (file != null)
            {
                current_file = file;
                current_client = file.Client;
                return true;
            }
            else
                return false;
        }

        private void ResetToggleTo(ucNavCmdButtons btn)
        {
            ncbCurrentClientFiles.ForeColor = ncSearchTypes.ForeColor;
            ncbAllClientFiles.ForeColor = ncSearchTypes.ForeColor;
            ncbCurrentClient.ForeColor = ncSearchTypes.ForeColor;
            ncbLastUsed.ForeColor = ncSearchTypes.ForeColor;
            ncbCheckedOut.ForeColor = ncSearchTypes.ForeColor;
            ncbLocal.ForeColor = ncSearchTypes.ForeColor;
            ncbLastUpdated.ForeColor = ncSearchTypes.ForeColor;
            btn.ForeColor = Color.Red;
        }

        #endregion

        protected override void OnLoad(EventArgs e)
        {
            _oldtitle = this.Text;
            base.OnLoad(e);

            SetOpenFilter();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Session.CurrentSession.DuplicateDocumentIDs)
                sb.AppendLine(Session.CurrentSession.Resources.GetMessage("INFODUPDOCIDSON", "Duplicate Document Identifiers Switched On", "").Text);
            if (Session.CurrentSession.SupportsExternalDocumentIds)
                sb.AppendLine(Session.CurrentSession.Resources.GetMessage("INFOEXTDOCIDSON", "External Document Identifiers Switched On", "").Text);
            info.SetError(txtDocID, sb.ToString());
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            try
            {
                ApplyView(ucDocuments1.DefaultView);

                //DM - 02/11/04 - Added this as the In/Out filter buttons have never worked in the open dialog.
                ucDocuments1.SelectItem();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void ucDocuments1_Leave(object sender, EventArgs e)
        {
            try
            {
                SetClientDescription();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void ncbAllClientFiles_LinkClicked(object sender, System.EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.Search);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbCurrentClientFiles_LinkClicked(object sender, System.EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.File);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbCurrentClient_LinkClicked(object sender, System.EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.Client);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void ncbLastUsed_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.Latest);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbCheckedOut_Click(object sender, System.EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.CheckedOut);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbChangeClient_LinkClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (ChangeClient())
                    ApplyView(DocumentPickerType.Client);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbChangeClientFile_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ChangeFile())
                    ApplyView(DocumentPickerType.File);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void ncbLocal_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.Local);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void btnOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                OMSDocument doc = ucDocuments1.Find(txtDocID.Text);

                string infomsg = String.Empty;

                FWBS.OMS.DocumentManagement.Storage.IStorageItemLockable lockableitem = doc.GetStorageProvider().GetLockableItem(doc);

                User checkedoutby = lockableitem.CheckedOutBy;
                if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
                {
                    infomsg += Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, doc.DisplayID, checkedoutby.FullName).Text;
                }

                ucDocuments1.UnloadPreview();
                if (Services.OpenDocument(doc, DocOpenMode.Edit))
                    this.Close();
                else
                    ucDocuments1.LoadPreview();

                if (infomsg.Length > 0)
                {
                    MessageBox.Show(infomsg, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    List<OMSDocument> docList = new List<OMSDocument>() { doc };
                    FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docList, true);
                }
            }
            catch (OMSException2 ex)
            {
                if (ex.Code != "DUPDOCCANCELLED")
                    MessageBox.Show(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
            }
        }

        private void ucDocuments1_DocumentsRefreshed(object sender, EventArgs e)
        {
            try
            {
                if (ucDocuments1.DocumentCount > 0)
                {
                    pnDetails.Visible = true;
                    pnDetails.Expanded = true;
                }
                else
                {
                    pnDetails.Visible = false;
                    txtDocID.Text = "";
                    ucNavRichText1.ControlRich.Clear();
                    ucNavRichText1.Refresh();
                    pnDetails.Expanded = false;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                ncSearchTypes.Enabled = true;
            }
        }

        private void ucDocuments1_DocumentSelecting(object sender, EventArgs e)
        {
            try
            {



                ucNavRichText1.ControlRich.Clear();

                try
                {
                    ucNavRichText1.ControlRich.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(ucDocuments1.GetCurrentDocumentDetailsAsRTF());
                    
                }
                catch
                {
                    ucNavRichText1.ControlRich.Text = ucDocuments1.GetCurrentDocumentDetailsAsRTF();
                }
                if (pnDetails.Expanded == false)
                    pnDetails.Expanded = true;

                ucNavRichText1.Refresh();

                string[] ids = ucDocuments1.SelectedDocumentIds;
                if (ids.Length == 0)
                    txtDocID.Text = String.Empty;
                else
                    txtDocID.Text = ids[0];
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void btnManual_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (ManualDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtDocID.Text = "";
                    _controlApp.Open(new System.IO.FileInfo(ManualDialog.FileName));
                    this.DialogResult = DialogResult.Cancel;

                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch
            {
            }
        }

        private void ncbLastUpdated_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyView(DocumentPickerType.LatestUpdate);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void pnlGrouping_VisibleChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("WHA");
        }

        private const Char backspace = (Char)8;
        private const Char copy = (Char)3;
        private const Char paste = (Char)22;
        private bool afterpaste = false;

        private void txtDocID_TextChanged(object sender, System.EventArgs e)
        {
            btnOK.Enabled = txtDocID.Text != "";
            string vals = "";
            if (afterpaste)
            {
                foreach (char item in txtDocID.Text.ToCharArray())
                {
                    if (item.ToString() == "-" && String.IsNullOrEmpty(vals))
                        vals += item.ToString();
                    else if (System.Text.RegularExpressions.Regex.IsMatch(item.ToString(), "\\d+"))
                        vals += item.ToString();
                }
                txtDocID.Text = vals;
                txtDocID.SelectionStart = txtDocID.Text.Length;
                afterpaste = false;
            }
        }

        private void txtDocID_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case copy:
                case backspace:
                    return;
                case paste:
                    afterpaste = true;
                    return;
                default:
                    if ((txtDocID.SelectionStart == 0 && e.KeyChar.ToString() == "-") || System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), "\\d+"))
                        return;
                    e.Handled = true;
                    return;
            }
        }

    }
}
