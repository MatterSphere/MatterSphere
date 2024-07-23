using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Security;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple form that is used to hold a select client / file user control.  
    /// This form holds extra buttons so that a client / file can be found or added.
    /// </summary>
    internal class frmSelectClientFileAssociate : frmNewBrandIdent
    {
        #region Fields

        protected ucAlert pnlAlert;
        protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private Associate _curassoc;
        private FWBS.OMS.UI.Windows.ResourceLookup reslookup;
        private System.ComponentModel.IContainer components;
        protected Panel pnlMain;
        private TabControl tcSelector;
        private TabPage tpClientFile;
        private ucSelectClientFile ucSelectClientFileSelector;
        protected Button cmdCancel;
        protected Button cmdFind;
        protected Button cmdOK;
        protected Button cmdViewClient;
        private TabPage tpAssocLst;
        private ucSearchControl ucAssocLst;
        private Panel pnlAssInfo;
        private ucPanelNav pntSearchTypes;
        private ucNavRichText ucNavRichText1;
        protected Panel panel1;
        protected Button btnCreateClient;
        protected Panel panel6;
        protected Button btnPrivateEmail;
        private Panel panel4;
        private Panel panel2;
        private Panel panel3;
        private Panel panel7;
        protected Button cmdViewFile;
        private Panel panel8;
        private Panel panel9;
        private Accelerators accelerators1;


        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Default constructor of the form.
        /// </summary>
        public frmSelectClientFileAssociate() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // This is very important make sure the cmdOK Click event is set before the ucSelectClientFileSelector OKButton
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            this.ucSelectClientFileSelector.OKButton = this.cmdOK;

            this.ucAssocLst.dgSearchResults.Click += new EventHandler(dgSearchResults_Click);
            SetIcon(Images.DialogIcons.Associate);
        }

        private void dgSearchResults_Click(object sender, EventArgs e)
        {
            SetAllAlerts();
        }

        private void SetAllAlerts()
        {
            if (ucAssocLst.CurrentItem() != null)
            {
                Associate assoc = Associate.GetAssociate((long)ucAssocLst.CurrentItem()["ASSOCID"].Value);
                if (assoc != null)
                {
                    System.Collections.Generic.List<Alert> alerts = new System.Collections.Generic.List<Alert>();
                    alerts.AddRange(assoc.Contact.Alerts);
                    alerts.AddRange(ExistingAlerts);

                    pnlAlert.SetAlerts(alerts.ToArray());
                }
            }
        }

        /// <summary>
        /// Constructor to specifying the file to find immediately.  If null then no file
        /// is taken.
        /// </summary>
        /// <param name="file">File to show in the select form.</param>
        public frmSelectClientFileAssociate(Associate assoc)
            : this()
        {
            if (assoc != null)
            {
                ucSelectClientFileSelector.GetFile(assoc.OMSFile);
            }

        }

        public frmSelectClientFileAssociate(OMSFile file)
            : this()
        {
            if (file != null)
            {
                ucSelectClientFileSelector.GetFile(file);
            }
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tcSelector = new FWBS.OMS.UI.TabControl();
            this.tpClientFile = new System.Windows.Forms.TabPage();
            this.ucSelectClientFileSelector = new FWBS.OMS.UI.Windows.ucSelectClientFile();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdFind = new System.Windows.Forms.Button();
            this.cmdViewClient = new System.Windows.Forms.Button();
            this.cmdViewFile = new System.Windows.Forms.Button();
            this.tpAssocLst = new System.Windows.Forms.TabPage();
            this.ucAssocLst = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.panel9 = new System.Windows.Forms.Panel();
            this.pnlAssInfo = new System.Windows.Forms.Panel();
            this.pntSearchTypes = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.cmdOK = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCreateClient = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnPrivateEmail = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pnlAlert = new FWBS.OMS.UI.Windows.ucAlert();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.reslookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.tcSelector.SuspendLayout();
            this.tpClientFile.SuspendLayout();
            this.tpAssocLst.SuspendLayout();
            this.pnlAssInfo.SuspendLayout();
            this.pntSearchTypes.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcSelector
            // 
            this.tcSelector.Controls.Add(this.tpClientFile);
            this.tcSelector.Controls.Add(this.tpAssocLst);
            this.tcSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSelector.HotTrack = true;
            this.tcSelector.Location = new System.Drawing.Point(5, 5);
            this.tcSelector.Name = "tcSelector";
            this.tcSelector.SelectedIndex = 0;
            this.tcSelector.Size = new System.Drawing.Size(606, 534);
            this.tcSelector.TabIndex = 1;
            this.tcSelector.SelectedIndexChanged += new System.EventHandler(this.tcSelector_SelectedIndexChanged);
            // 
            // tpClientFile
            // 
            this.tpClientFile.BackColor = System.Drawing.Color.White;
            this.tpClientFile.Controls.Add(this.ucSelectClientFileSelector);
            this.tpClientFile.Location = new System.Drawing.Point(4, 24);
            this.reslookup.SetLookup(this.tpClientFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("SelCliAssoc", "Select %CLIENT% and %FILE%", ""));
            this.tpClientFile.Name = "tpClientFile";
            this.tpClientFile.Size = new System.Drawing.Size(598, 506);
            this.tpClientFile.TabIndex = 0;
            this.tpClientFile.Text = "Select %CLIENT% and %FILE%";
            // 
            // ucSelectClientFileSelector
            // 
            this.ucSelectClientFileSelector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucSelectClientFileSelector.CancelButton = this.cmdCancel;
            this.ucSelectClientFileSelector.CheckAllFiles = false;
            this.ucSelectClientFileSelector.ClientFileState = FWBS.OMS.UI.Windows.ClientFileState.None;
            this.ucSelectClientFileSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSelectClientFileSelector.FavouriteHeight = 38;
            this.ucSelectClientFileSelector.FindButton = this.cmdFind;
            this.ucSelectClientFileSelector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSelectClientFileSelector.Location = new System.Drawing.Point(0, 0);
            this.ucSelectClientFileSelector.Name = "ucSelectClientFileSelector";
            this.ucSelectClientFileSelector.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ucSelectClientFileSelector.SelectClientFileSearchType = FWBS.OMS.UI.Windows.SelectClientFileSearchType.File;
            this.ucSelectClientFileSelector.SelectFileVisible = true;
            this.ucSelectClientFileSelector.Size = new System.Drawing.Size(598, 506);
            this.ucSelectClientFileSelector.TabIndex = 0;
            this.ucSelectClientFileSelector.ViewClientButton = this.cmdViewClient;
            this.ucSelectClientFileSelector.ViewFileButton = this.cmdViewFile;
            this.ucSelectClientFileSelector.Alert += new FWBS.OMS.AlertEventHandler(this.ucClientSelector_Alert);
            this.ucSelectClientFileSelector.StateChanged += new FWBS.OMS.UI.Windows.ClientFileStateChangedEventHandler(this.ucSelectClientFileSelector_StateChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdCancel.Location = new System.Drawing.Point(5, 61);
            this.reslookup.SetLookup(this.cmdCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdCancel", "Cancel", ""));
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(91, 23);
            this.cmdCancel.TabIndex = 31;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdFind
            // 
            this.cmdFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdFind.Location = new System.Drawing.Point(5, 33);
            this.reslookup.SetLookup(this.cmdFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdFind", "Find...", ""));
            this.cmdFind.Name = "cmdFind";
            this.cmdFind.Size = new System.Drawing.Size(91, 23);
            this.cmdFind.TabIndex = 30;
            this.cmdFind.Text = "Find...";
            // 
            // cmdViewClient
            // 
            this.cmdViewClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdViewClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdViewClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdViewClient.Location = new System.Drawing.Point(5, 476);
            this.reslookup.SetLookup(this.cmdViewClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdViewClient", "View %CLIENT%", ""));
            this.cmdViewClient.Name = "cmdViewClient";
            this.cmdViewClient.Size = new System.Drawing.Size(91, 23);
            this.cmdViewClient.TabIndex = 34;
            this.cmdViewClient.Text = "View %CLIENT%";
            // 
            // cmdViewFile
            // 
            this.cmdViewFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdViewFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdViewFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdViewFile.Location = new System.Drawing.Point(5, 504);
            this.reslookup.SetLookup(this.cmdViewFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdViewFile", "View %FILE%", ""));
            this.cmdViewFile.Name = "cmdViewFile";
            this.cmdViewFile.Size = new System.Drawing.Size(91, 23);
            this.cmdViewFile.TabIndex = 33;
            this.cmdViewFile.Text = "View %FILE%";
            // 
            // tpAssocLst
            // 
            this.tpAssocLst.Controls.Add(this.ucAssocLst);
            this.tpAssocLst.Controls.Add(this.panel9);
            this.tpAssocLst.Controls.Add(this.pnlAssInfo);
            this.tpAssocLst.Location = new System.Drawing.Point(4, 24);
            this.tpAssocLst.Name = "tpAssocLst";
            this.tpAssocLst.Size = new System.Drawing.Size(598, 506);
            this.tpAssocLst.TabIndex = 1;
            this.tpAssocLst.Text = "%ASSOCIATE%";
            // 
            // ucAssocLst
            // 
            this.ucAssocLst.BackColor = System.Drawing.Color.White;
            this.ucAssocLst.BackGroundColor = System.Drawing.Color.White;
            this.ucAssocLst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucAssocLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucAssocLst.DoubleClickAction = "None";
            this.ucAssocLst.GraphicalPanelVisible = true;
            this.ucAssocLst.Location = new System.Drawing.Point(176, 0);
            this.ucAssocLst.Name = "ucAssocLst";
            this.ucAssocLst.NavCommandPanel = null;
            this.ucAssocLst.Padding = new System.Windows.Forms.Padding(2);
            this.ucAssocLst.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucAssocLst.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucAssocLst.SearchListCode = "";
            this.ucAssocLst.SearchListType = "";
            this.ucAssocLst.SearchPanelVisible = false;
            this.ucAssocLst.Size = new System.Drawing.Size(422, 506);
            this.ucAssocLst.TabIndex = 0;
            this.ucAssocLst.Tag = "";
            this.ucAssocLst.ToBeRefreshed = false;
            this.ucAssocLst.TypeSelectorVisible = false;
            this.ucAssocLst.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucAssocLst_SearchButtonCommands);
            this.ucAssocLst.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.ucAssocLst_ItemHover);
            this.ucAssocLst.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this.ucAssocLst_SearchCompleted);
            this.ucAssocLst.OpenedOMSItem += new System.EventHandler(this.ucAssocLst_OpenedOMSItem);
            this.ucAssocLst.ClosedOMSItem += new System.EventHandler(this.ucAssocLst_ClosedOMSItem);
            this.ucAssocLst.SearchListLoad += new System.EventHandler(this.ucAssocLst_SearchListLoad);
            this.ucAssocLst.FilterChanged += new System.EventHandler(this.ucAssocLst_FilterChanged);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Transparent;
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(172, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(4, 506);
            this.panel9.TabIndex = 9;
            // 
            // pnlAssInfo
            // 
            this.pnlAssInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlAssInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAssInfo.Controls.Add(this.pntSearchTypes);
            this.pnlAssInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlAssInfo.Name = "pnlAssInfo";
            this.pnlAssInfo.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pnlAssInfo.Size = new System.Drawing.Size(172, 506);
            this.pnlAssInfo.TabIndex = 1;
            // 
            // pntSearchTypes
            // 
            this.pntSearchTypes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pntSearchTypes.Controls.Add(this.ucNavRichText1);
            this.pntSearchTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.pntSearchTypes.ExpandedHeight = 55;
            this.pntSearchTypes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pntSearchTypes.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pntSearchTypes.Location = new System.Drawing.Point(8, 7);
            this.reslookup.SetLookup(this.pntSearchTypes, new FWBS.OMS.UI.Windows.ResourceLookupItem("DETAILS", "Details", ""));
            this.pntSearchTypes.Name = "pntSearchTypes";
            this.pntSearchTypes.Size = new System.Drawing.Size(154, 55);
            this.pntSearchTypes.TabIndex = 1;
            this.pntSearchTypes.TabStop = false;
            this.pntSearchTypes.Text = "Details";
            this.pntSearchTypes.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.reslookup.SetLookup(this.ucNavRichText1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Details", "Details", ""));
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;}}" +
    "\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(154, 24);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // cmdOK
            // 
            this.cmdOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdOK.Location = new System.Drawing.Point(5, 5);
            this.reslookup.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdOK", "Proceed", ""));
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(91, 23);
            this.cmdOK.TabIndex = 33;
            this.cmdOK.Text = "Proceed";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tcSelector);
            this.pnlMain.Controls.Add(this.panel8);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.pnlAlert);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMain.Size = new System.Drawing.Size(725, 544);
            this.pnlMain.TabIndex = 5;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Transparent;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(611, 5);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(6, 534);
            this.panel8.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCreateClient);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.btnPrivateEmail);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.cmdViewClient);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.cmdFind);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.cmdViewFile);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(617, 5);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(103, 534);
            this.panel1.TabIndex = 7;
            // 
            // btnCreateClient
            // 
            this.btnCreateClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCreateClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreateClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreateClient.Location = new System.Drawing.Point(5, 448);
            this.reslookup.SetLookup(this.btnCreateClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCreateClient", "Create %CLIENT%", ""));
            this.btnCreateClient.Name = "btnCreateClient";
            this.btnCreateClient.Size = new System.Drawing.Size(91, 23);
            this.btnCreateClient.TabIndex = 42;
            this.btnCreateClient.Text = "Create %CLIENT%";
            this.btnCreateClient.Click += new System.EventHandler(this.btnCreateClient_Click);
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(5, 471);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(91, 5);
            this.panel6.TabIndex = 43;
            // 
            // btnPrivateEmail
            // 
            this.btnPrivateEmail.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnPrivateEmail.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPrivateEmail.Enabled = false;
            this.btnPrivateEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrivateEmail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrivateEmail.Location = new System.Drawing.Point(5, 89);
            this.reslookup.SetLookup(this.btnPrivateEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnPrivateEmail", "Private &Email", ""));
            this.btnPrivateEmail.Name = "btnPrivateEmail";
            this.btnPrivateEmail.Size = new System.Drawing.Size(91, 23);
            this.btnPrivateEmail.TabIndex = 39;
            this.btnPrivateEmail.Text = "Private &Email";
            this.btnPrivateEmail.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(5, 84);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(91, 5);
            this.panel4.TabIndex = 40;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(91, 5);
            this.panel2.TabIndex = 36;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(5, 499);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(91, 5);
            this.panel3.TabIndex = 38;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(5, 28);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(91, 5);
            this.panel7.TabIndex = 37;
            // 
            // pnlAlert
            // 
            this.pnlAlert.BackColor = System.Drawing.Color.White;
            this.pnlAlert.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlert.Location = new System.Drawing.Point(5, 5);
            this.pnlAlert.Name = "pnlAlert";
            this.pnlAlert.Size = new System.Drawing.Size(715, 56);
            this.pnlAlert.TabIndex = 4;
            this.pnlAlert.Visible = false;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\SelectClientFileAssoc";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // frmSelectClientFileAssociate
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(725, 544);
            this.Controls.Add(this.pnlMain);
            this.reslookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("SelAssociate", "Select %ASSOCIATE%", ""));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectClientFileAssociate";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select %ASSOCIATE%";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSelectClientFileAssociate_Closing);
            this.Load += new System.EventHandler(this.frmSelectClientFileAssociate_Load);
            this.tcSelector.ResumeLayout(false);
            this.tpClientFile.ResumeLayout(false);
            this.tpAssocLst.ResumeLayout(false);
            this.pnlAssInfo.ResumeLayout(false);
            this.pntSearchTypes.ResumeLayout(false);
            this.pntSearchTypes.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Private Methods

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            SetIcon(Images.DialogIcons.Associate);
        }

        /// <summary>
        /// Cancels the select form and closes it.
        /// </summary>
        /// <param name="sender">Cancel command button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void cmdCancel_Click(object sender, System.EventArgs e)
        {
            this.Dispose(true);
            this.Close();
        }


        /// <summary>
        /// If a client has been picked then close the form with a success dialog code.
        /// </summary>
        /// <param name="sender">OK button control.</param>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void cmdOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (ucSelectClientFileSelector.SelectedFile == null && ucSelectClientFileSelector.SelectFileVisible)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Confirm;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.None)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.Confirm)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.Proceed)
                {
                    // State 3
                    if ((ucAssocLst.SearchList == null) | (tcSelector.SelectedTab != tpAssocLst))
                    {
                        // Assoc List never ran
                        cmdOK.Enabled = false;
                        tcSelector.SelectedTab = tpAssocLst;
                        ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                        return;
                    }
                    if (ucAssocLst.Tag.ToString() != ucSelectClientFileSelector.SelectedFile.ID.ToString())
                    {
                        // Assoc List is different client need to refresh
                        if (ucAssocLst.SearchList.ResultCount == 0)
                            cmdOK.Enabled = false;
                        tcSelector.SelectedTab = tpAssocLst;
                        return;
                    }

                    try
                    {
                        if (ucAssocLst.CurrentItem() != null)
                        {
                            _curassoc = Associate.GetAssociate((long)ucAssocLst.CurrentItem()["ASSOCID"].Value);
                        }
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("SELASSOC", "Please select an %ASSOCIATE%", "").Text, FWBS.OMS.Session.CurrentSession.Terminology.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //added DMB 12/8/2003 if selected associate is inactive prompt if they would like to make active
                    if (!_curassoc.Active)
                    {
                        DialogResult res = MessageBox.ShowYesNoQuestion("ASSOCMAKEACTIVE", "Selected associate is inactive, would you like to make it active.", null);

                        if (res == DialogResult.Yes)
                        {
                            _curassoc.SetExtraInfo("assocActive", true);
                            _curassoc.Update();
                        }
                        else
                        {
                            // Dont proceed as the associate is inactive.
                            return;
                        }
                    }

                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                    ucSelectClientFileSelector.AddToTop10();
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowAssociates()
        {

            string asssearchlistshort = Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.AssociatesShort);
            string asssearchlist = Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Associates);
            if (SearchEngine.SearchList.Exists(asssearchlistshort))
                asssearchlist = asssearchlistshort;

            if (ucAssocLst.Tag == null)
            {
                ucAssocLst.SetSearchList(asssearchlist, false, ucSelectClientFileSelector.SelectedFile, null);
                ucAssocLst.Tag = ucSelectClientFileSelector.SelectedFile.ID;
                ucAssocLst.Search();
                if (ucAssocLst.QuickFilterContol.Visible)
                    ucAssocLst.QuickFilterContol.Focus();

            }
            else
            {
                if (ucSelectClientFileSelector.SelectedFile.ID.ToString() != ucAssocLst.Tag.ToString())
                {
                    ucAssocLst.SetSearchList(asssearchlist, false, ucSelectClientFileSelector.SelectedFile, null);
                    ucAssocLst.Tag = ucSelectClientFileSelector.SelectedFile.ID;
                    ucAssocLst.Search();


                }
                SetAllAlerts();
            }

            ApplyButtonState();
        }

        private void ApplyButtonState()
        {
        }

        private Alert[] ExistingAlerts;

        /// <summary>
        /// Capture the alert event of the select client/file user control and display it in a 
        /// big red label at the top of the form.
        /// </summary>
        /// <param name="sender">Client / file selector control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void ucClientSelector_Alert(object sender, AlertEventArgs e)
        {
            pnlAlert.SetAlerts(e.Alerts);
            ExistingAlerts = e.Alerts;
        }

        /// <summary>
        /// Captures the state changed event of the client / file control
        /// Use the passed event argument to determine how the proceed / OK
        /// button executes.
        /// </summary>
        /// <param name="sender">Client / file user control object.</param>
        /// <param name="e">State changed event arguments.</param>
        private void ucSelectClientFileSelector_StateChanged(object sender, FWBS.OMS.UI.Windows.ClientFileStateEventArgs e)
        {
            if (e.State == ClientFileState.None)
            {
                cmdOK.Text = Session.CurrentSession.Resources.GetResource("CONFIRM", "Confirm", "").Text;
            }
            if (e.State == ClientFileState.Confirm)
            {
                cmdOK.Text = Session.CurrentSession.Resources.GetResource("PROCEED", "Proceed", "").Text;
            }
            if (e.State == ClientFileState.Proceed)
            {
                cmdOK.Text = Session.CurrentSession.Resources.GetResource("PROCEED", "Proceed", "").Text;
            }
            if (AutoConfirm && ucSelectClientFileSelector.SelectedFile != null)
            {
                if (tcSelector.SelectedTab != tpAssocLst)
                    tcSelector.SelectedTab = tpAssocLst;
                AutoConfirm = false;
            }
            AcceptButton = this.cmdOK;
            CancelButton = this.cmdCancel;
        }

        /// <summary>
        /// The load event accesses the resource strings of the buttons and form caption.
        /// </summary>
        /// <param name="sender">This select client / file form.</param>
        /// <param name="e">Empty event arguments.</param>
        private void frmSelectClientFileAssociate_Load(object sender, System.EventArgs e)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                if (!DesignMode)
                {
                    pnlMain.Visible = false;
                    Global.ControlParser(this);
                    pnlMain.Visible = true;
                }

                Common.ApplicationSetting ccl = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "CreateClientButton", "True");
                btnCreateClient.Visible = ccl.ToBoolean();

                Common.ApplicationSetting vwcl = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "ShowViewClientButton", "True");
                Common.ApplicationSetting vwf = new Common.ApplicationSetting(vwcl, "ShowViewFileButton", "True");
                cmdViewClient.Visible = vwcl.ToBoolean();
                cmdViewFile.Visible = vwf.ToBoolean();

                btnPrivateEmail.Visible = (AllowPrivateAssociate && !Session.CurrentSession.CurrentUser.IsInRoles("STOPPRIVEMAIL"));
                btnPrivateEmail.Enabled = AllowPrivateAssociate;

                ucSelectClientFileSelector.txtClientNo.Focus();

            }
        }

        #endregion

        #region Properties

        public bool AllowPrivateAssociate { get; set; }
        public bool AutoConfirm { get; set; }

        #endregion

        private void tcSelector_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (tcSelector.SelectedTab == tpAssocLst)
                {
                    cmdOK.Enabled = (ucAssocLst.SearchList != null && ucAssocLst.SearchList.ResultCount > 0);
                    cmdFind.Enabled = false;

                }
                else
                {
                    cmdFind.Enabled = true;
                }

                if (tcSelector.SelectedTab == tpClientFile)
                    return;

                if (ucSelectClientFileSelector.SelectedFile == null)
                {
                    MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("NOCLIENT", "You must select a %CLIENT% and %FILE%", "").Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    tcSelector.SelectedTab = tpClientFile;
                    return;
                }

                ShowAssociates();
            }
            catch (Exception ex)
            {
                tcSelector.SelectedTab = tpClientFile;
                ErrorBox.Show(this, ex);
            }

        }


        private void ucAssocLst_ItemHover(object sender, FWBS.OMS.UI.Windows.SearchItemHoverEventArgs e)
        {
            ucNavRichText1.ControlRich.Clear();
            ucNavRichText1.ControlRich.ClearUndo();
            if (ucAssocLst.SearchList.ResultCount > 0)
            {
                Associate ass = Associate.GetAssociate(Convert.ToInt64(e.ItemList["associd"].Value));
                var associateDetailsData = GetAssociateDetailsPanelData(ass);
                var associatedetailsformatting = new Version2DocumentDetails();
                associatedetailsformatting.Set(ucNavRichText1, associateDetailsData);
                ucNavRichText1.Refresh();
                cmdOK.Enabled = true;
                SetAllAlerts();
            }
        }


        private Dictionary<string, string> GetAssociateDetailsPanelData(FWBS.OMS.Associate ass)
        {
            var documentDetailsData = new Dictionary<string, string>();

            documentDetailsData.Add(Session.CurrentSession.Resources.GetMessage("DEFADD", "Default Address", "").Text,ass.DefaultAddress.ToString());

            if (!string.IsNullOrWhiteSpace(ass.TheirRef))
                documentDetailsData.Add(Session.CurrentSession.Resources.GetMessage("REF", "Ref : ", "").Text, ass.TheirRef);

            if (!string.IsNullOrWhiteSpace(ass.DefaultTelephoneNumber))
                documentDetailsData.Add(Session.CurrentSession.Resources.GetMessage("TEL", "Tel : ", "").Text,ass.DefaultTelephoneNumber);

            if (!string.IsNullOrWhiteSpace(ass.DefaultFaxNumber))
                documentDetailsData.Add(Session.CurrentSession.Resources.GetMessage("FAX", "Fax : ", "").Text, ass.DefaultFaxNumber);

            if (ass.LastDocumentDate != DBNull.Value)
                documentDetailsData.Add(Session.CurrentSession.Resources.GetMessage("LASTDOC", "Last Document : ", "").Text, ass.LastDocumentDate.ToString());

            return documentDetailsData;
        }


        private void ucAssocLst_OpenedOMSItem(object sender, System.EventArgs e)
        {
            pnlAssInfo.Visible = false;
        }

        private void ucAssocLst_ClosedOMSItem(object sender, System.EventArgs e)
        {
            pnlAssInfo.Visible = true;
        }

        private void ucAssocLst_SearchCompleted(object sender, FWBS.OMS.UI.Windows.SearchCompletedEventArgs e)
        {
            cmdOK.Enabled = (ucAssocLst.SearchList != null && ucAssocLst.SearchList.ResultCount > 0);
        }

        private void ucAssocLst_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {

            try
            {
                switch (e.ButtonName.ToUpper())
                {
                    case "CMDADD":
                    case "BTNADD":
                        {
                            if (FWBS.OMS.UI.Windows.Services.Wizards.CreateAssociate(ucSelectClientFileSelector.SelectedFile) != null)
                            {
                                ucAssocLst.Search(false, true, false);
                                try
                                {
                                    ucAssocLst.dgSearchResults.CurrentRowIndex = ucAssocLst.DataTable.DefaultView.Count - 1;
                                }
                                catch { }
                            }

                            e.Cancel = true;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }


        }

        private void ucAssocLst_FilterChanged(object sender, System.EventArgs e)
        {
            cmdOK.Enabled = (ucAssocLst.SearchList != null && ucAssocLst.SearchList.ResultCount > 0);
        }

        #region Properties

        /// <summary>
        /// Gets the client that has been selected from the client / file selection control.
        /// </summary>
        [DefaultValue(null)]
        public Client SelectedClient
        {
            get
            {
                if (ucSelectClientFileSelector != null)
                    return ucSelectClientFileSelector.SelectedClient;
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the file id that has been selected from the client / file selection control.
        /// </summary>
        [DefaultValue(null)]
        public Associate SelectedAssociate
        {
            get
            {
                if (ucSelectClientFileSelector != null)
                {
                    return _curassoc;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the file id that has been selected from the client / file selection control.
        /// </summary>
        [DefaultValue(null)]
        public OMSFile SelectedFile
        {
            get
            {
                if (ucSelectClientFileSelector != null)
                {
                    return ucSelectClientFileSelector.SelectedFile;
                }
                else
                    return null;
            }
        }
        #endregion

        private void btnCreateClient_Click(object sender, EventArgs e)
        {
            try
            {
                Client n = FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
                if (n != null)
                {
                    ucSelectClientFileSelector.GetClient(n);
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucSelectClientFileSelector.SelectedClient != null)
                {
                    OMSFile n = FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(ucSelectClientFileSelector.SelectedClient);
                    if (n != null)
                    {
                        ucSelectClientFileSelector.GetClient(ucSelectClientFileSelector.SelectedClient);
                        ucSelectClientFileSelector.GetFile(n);
                        ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                    }
                }
                else
                {
                    MessageBox.ShowInformation("ERRSELCLT", "Please select a Client first ...");
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmSelectClientFileAssociate_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    ucFormStorage1.SaveNow();
                    if (this.DialogResult == DialogResult.OK)
                    {
                        ucSelectClientFileSelector.SetFilePhase();
                        ucSelectClientFileSelector.AddToTop10();
                    }
                    Session.CurrentSession.CurrentFavourites.Update();
                }
            }
            catch { }
        }

        private void ucAssocLst_SearchListLoad(object sender, EventArgs e)
        {
            try
            {
                bool obj = true;
                if (ucSelectClientFileSelector.SelectedFile != null)
                    obj = SecurityManager.CurrentManager.IsGranted(new FilePermission(ucSelectClientFileSelector.SelectedFile, StandardPermissionType.DeleteAssociate));
                bool sys = SecurityManager.CurrentManager.IsGranted(new SystemPermission(StandardPermissionType.DeleteAssociate));
                if (obj == false || sys == false)
                    ucAssocLst.GetOMSToolBarButton("cmdDelete").Visible = false;
            }
            catch
            {

            }
        }

    }
}
