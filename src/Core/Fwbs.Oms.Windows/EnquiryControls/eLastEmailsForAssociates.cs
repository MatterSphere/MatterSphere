using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for eLockIcon.
    /// </summary>
    public class eLastEmailsForAssociates : System.Windows.Forms.UserControl
    {
        private IContainer components;
        private EnquiryForm _parent;
        private FWBS.OMS.UI.TreeView treeView1;
        private Button btnOpen;
        private Button btnAttach;
        private ImageList imageList1;
        private FWBS.OMS.OMSFile _file;
        private Panel pnlOptions;
        private Panel panel2;
        private PictureBox picClose;
        private Label label1;
        private GroupBox grpOptions;
        private RadioButton rdoEmailsOnly;
        private RadioButton rdoAll;
        private Label labDocRetPerAss;
        private NumericUpDown numDocs;
        private Button btnCancel;
        private Button btnOK;
        private Button btnRefresh;
        private Button btnViewAssoc;
        private Button btnOptions;
        private ToolTip toolTip1;
        private ResourceLookup resourceLookup1;
        private Panel pnlButtons;
        private Associate _associate;

        public event EventHandler<SelectedIDEventArgs> OpenClicked;
        public event EventHandler<SelectedIDEventArgs> AttachClicked;

        public eLastEmailsForAssociates()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
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

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Outbound Emial - 26/10/2006", 0, 0);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Inbound Emial - 26/10/2006", 1, 1);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Mr Associate", 2, 2, new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(eLastEmailsForAssociates));
            this.treeView1 = new FWBS.OMS.UI.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnAttach = new System.Windows.Forms.Button();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labDocRetPerAss = new System.Windows.Forms.Label();
            this.numDocs = new System.Windows.Forms.NumericUpDown();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoEmailsOnly = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnViewAssoc = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDocs)).BeginInit();
            this.grpOptions.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 0;
            treeNode1.Name = "Node1";
            treeNode1.SelectedImageIndex = 0;
            treeNode1.Text = "Outbound Emial - 26/10/2006";
            treeNode2.ImageIndex = 1;
            treeNode2.Name = "Node2";
            treeNode2.SelectedImageIndex = 1;
            treeNode2.Text = "Inbound Emial - 26/10/2006";
            treeNode3.ImageIndex = 2;
            treeNode3.Name = "Ass";
            treeNode3.SelectedImageIndex = 2;
            treeNode3.Text = "Mr Associate";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(360, 316);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "49.ICO");
            this.imageList1.Images.SetKeyName(1, "22.ICO");
            // 
            // btnOpen
            // 
            this.btnOpen.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOpen.Enabled = false;
            this.btnOpen.Location = new System.Drawing.Point(2, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(68, 24);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "&Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnAttach
            // 
            this.btnAttach.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAttach.Enabled = false;
            this.btnAttach.Location = new System.Drawing.Point(70, 4);
            this.btnAttach.Name = "btnAttach";
            this.btnAttach.Size = new System.Drawing.Size(68, 24);
            this.btnAttach.TabIndex = 2;
            this.btnAttach.Text = "&Attach";
            this.btnAttach.UseVisualStyleBackColor = true;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);
            // 
            // pnlOptions
            // 
            this.pnlOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOptions.Controls.Add(this.btnCancel);
            this.pnlOptions.Controls.Add(this.btnOK);
            this.pnlOptions.Controls.Add(this.labDocRetPerAss);
            this.pnlOptions.Controls.Add(this.numDocs);
            this.pnlOptions.Controls.Add(this.grpOptions);
            this.pnlOptions.Controls.Add(this.panel2);
            this.pnlOptions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlOptions.Location = new System.Drawing.Point(18, 20);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(323, 169);
            this.pnlOptions.TabIndex = 3;
            this.pnlOptions.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(91, 136);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "&Cancel", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(10, 136);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labDocRetPerAss
            // 
            this.labDocRetPerAss.AutoSize = true;
            this.labDocRetPerAss.Location = new System.Drawing.Point(10, 107);
            this.resourceLookup1.SetLookup(this.labDocRetPerAss, new FWBS.OMS.UI.Windows.ResourceLookupItem("labDocRetPerAss", "No. Documents per Associate", ""));
            this.labDocRetPerAss.Name = "labDocRetPerAss";
            this.labDocRetPerAss.Size = new System.Drawing.Size(163, 15);
            this.labDocRetPerAss.TabIndex = 3;
            this.labDocRetPerAss.Text = "No. Documents per Associate";
            // 
            // numDocs
            // 
            this.numDocs.Location = new System.Drawing.Point(178, 105);
            this.numDocs.Name = "numDocs";
            this.numDocs.Size = new System.Drawing.Size(51, 23);
            this.numDocs.TabIndex = 2;
            this.numDocs.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // grpOptions
            // 
            this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptions.Controls.Add(this.rdoAll);
            this.grpOptions.Controls.Add(this.rdoEmailsOnly);
            this.grpOptions.Location = new System.Drawing.Point(6, 31);
            this.resourceLookup1.SetLookup(this.grpOptions, new FWBS.OMS.UI.Windows.ResourceLookupItem("DocumentTypes", "Document Types", ""));
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(309, 69);
            this.grpOptions.TabIndex = 1;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Document Types";
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Location = new System.Drawing.Point(7, 43);
            this.resourceLookup1.SetLookup(this.rdoAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("AllDocuments", "All Documents", ""));
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(103, 19);
            this.rdoAll.TabIndex = 1;
            this.rdoAll.Text = "All Documents";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // rdoEmailsOnly
            // 
            this.rdoEmailsOnly.AutoSize = true;
            this.rdoEmailsOnly.Checked = true;
            this.rdoEmailsOnly.Location = new System.Drawing.Point(7, 20);
            this.resourceLookup1.SetLookup(this.rdoEmailsOnly, new FWBS.OMS.UI.Windows.ResourceLookupItem("EmailsOnly", "Emails Only", ""));
            this.rdoEmailsOnly.Name = "rdoEmailsOnly";
            this.rdoEmailsOnly.Size = new System.Drawing.Size(87, 19);
            this.rdoEmailsOnly.TabIndex = 0;
            this.rdoEmailsOnly.TabStop = true;
            this.rdoEmailsOnly.Text = "Emails Only";
            this.rdoEmailsOnly.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.picClose);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(321, 24);
            this.panel2.TabIndex = 0;
            // 
            // picClose
            // 
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = ((System.Drawing.Image)(resources.GetObject("picClose.Image")));
            this.picClose.Location = new System.Drawing.Point(303, 0);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(16, 22);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picClose.TabIndex = 1;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(2, 3);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("Options", "Options", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Options";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefresh.ImageIndex = 1;
            this.btnRefresh.ImageList = this.imageList1;
            this.btnRefresh.Location = new System.Drawing.Point(334, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(24, 24);
            this.btnRefresh.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnRefresh, "Refresh");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnViewAssoc
            // 
            this.btnViewAssoc.AutoSize = true;
            this.btnViewAssoc.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnViewAssoc.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnViewAssoc.Location = new System.Drawing.Point(138, 4);
            this.resourceLookup1.SetLookup(this.btnViewAssoc, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnViewAssoc", "&View %ASSOCIATE%", ""));
            this.btnViewAssoc.Name = "btnViewAssoc";
            this.btnViewAssoc.Size = new System.Drawing.Size(124, 24);
            this.btnViewAssoc.TabIndex = 3;
            this.btnViewAssoc.Text = "&View %ASSOCIATE%";
            this.btnViewAssoc.UseVisualStyleBackColor = true;
            this.btnViewAssoc.Visible = false;
            this.btnViewAssoc.Click += new System.EventHandler(this.viewAssociateToolStripMenuItem_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOptions.Location = new System.Drawing.Point(266, 4);
            this.resourceLookup1.SetLookup(this.btnOptions, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOptions", "&Options", ""));
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(68, 24);
            this.btnOptions.TabIndex = 4;
            this.btnOptions.Text = "&Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnViewAssoc);
            this.pnlButtons.Controls.Add(this.btnAttach);
            this.pnlButtons.Controls.Add(this.btnOpen);
            this.pnlButtons.Controls.Add(this.btnOptions);
            this.pnlButtons.Controls.Add(this.btnRefresh);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 316);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.pnlButtons.Size = new System.Drawing.Size(360, 32);
            this.pnlButtons.TabIndex = 6;
            // 
            // eLastEmailsForAssociates
            // 
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.pnlButtons);
            this.Name = "eLastEmailsForAssociates";
            this.Size = new System.Drawing.Size(360, 348);
            this.Load += new System.EventHandler(this.eLastEmailsForAssociates_Load);
            this.ParentChanged += new System.EventHandler(this.eLastEmailsForAssociates_ParentChanged);
            this.pnlOptions.ResumeLayout(false);
            this.pnlOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDocs)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        private void eLastEmailsForAssociates_ParentChanged(object sender, System.EventArgs e)
        {
            if (this.Parent != null)
            {
                if (this.Parent is EnquiryForm)
                {
                    _parent = this.Parent as EnquiryForm;
                    if (_parent.Enquiry.Object is FWBS.OMS.OMSFile)
                        _file = _parent.Enquiry.Object as FWBS.OMS.OMSFile;
                }
            }
            else
            {
                _file = null;
                _parent = null;
            }
        }

        [DefaultValue(null)]
        public Associate CurrentAssociate
        {
            get { return _associate; }
            set
            {
                _associate = value;
                _file = _associate.OMSFile;
                if (this.Parent != null)
                {
                    eLastEmailsForAssociates_Load(this, EventArgs.Empty);
                }
            }
        }

        [DefaultValue(null)]
        public OMSFile CurrentFile
        {
            get { return _file; }
            set 
            { 
                _file = value;
                if (this.Parent != null)
                {
                    eLastEmailsForAssociates_Load(this, EventArgs.Empty);
                }
            }
        }


        private DoubleClickAction doubleclickaction = DoubleClickAction.Attach;

        public DoubleClickAction DoubleClickAction
        {
            get { return doubleclickaction; }
            set { doubleclickaction = value; }
        }
	


        private void eLastEmailsForAssociates_Load(object sender, EventArgs e)
        {
            if (_file != null)
            {
                Favourites fav = new Favourites("ASSDOCS");
                try
                {
                    if (Convert.ToBoolean(fav.Param1(0)))
                    {
                        rdoEmailsOnly.Checked = true;
                        rdoAll.Checked = false;
                    }
                    else
                    {
                        rdoEmailsOnly.Checked = false;
                        rdoAll.Checked = true;
                    }
                    numDocs.Text = fav.Param2(0);
                }
                catch
                { }


                ReportingServer Active = new ReportingServer("FWBS Limited 2005");
                treeView1.Nodes.Clear();

                btnOpen.Enabled = false;
                btnAttach.Enabled = false;
                foreach (Associate ass in _file.Associates)
                {
                    TreeNode n = new TreeNode();

                    n.Text = ass.Addressee;
                    if (n.Text == "")
                        n.Text = ass.Contact.ToString();

                    System.Text.StringBuilder oString = new System.Text.StringBuilder();
                    oString.Append(n.Text);
                    int nChars =1+ Convert.ToInt32(Math.Floor((decimal)oString.Length / 4));
                    oString.Append(' ', nChars);
                    n.Text = oString.ToString();

                    if (_associate != null && ass.ID == _associate.ID)
                        n.NodeFont = new Font(treeView1.Font, FontStyle.Bold);

                    n.Tag = ass.ID;
                    n.ImageIndex = 0;
                    n.SelectedImageIndex = 0;

                    IDataParameter[] pars = new IDataParameter[1];
                    pars[0] = Active.Connection.AddParameter("associd", ass.ID);
                    DataTable doc;
                    if (rdoEmailsOnly.Checked)
                        doc = Active.Connection.ExecuteSQLTable("SELECT TOP " + numDocs.Text + " * FROM DBDocument WHERE associd = @associd AND docType='EMAIL' and docdeleted = 0 ORDER BY docid DESC", "DOCS", pars);
                    else
                        doc = Active.Connection.ExecuteSQLTable("SELECT TOP " + numDocs.Text + " * FROM DBDocument WHERE associd = @associd and docdeleted = 0 ORDER BY docid DESC", "DOCS", pars);
                    foreach (DataRow d in doc.Rows)
                    {
                        TreeNode dn = new TreeNode();
                        dn.Text = d["docDesc"] + " - " + String.Format("{0:d}", d["Created"]);
                        dn.Tag = d["docid"];
                        if (imageList1.Images.IndexOfKey(Convert.ToString(d["docExtension"])) == -1)
                            imageList1.Images.Add(Convert.ToString(d["docExtension"]), Common.IconReader.GetFileIcon(String.Format("test.{0}", Convert.ToString(d["docExtension"])), Common.IconReader.IconSize.Small, false).ToBitmap());
                        dn.ImageIndex = imageList1.Images.IndexOfKey(Convert.ToString(d["docExtension"]));
                        dn.SelectedImageIndex = dn.ImageIndex;
                        n.Nodes.Add(dn);
                    }

                    if (n.Nodes.Count > 0)
                        treeView1.Nodes.Add(n);

                }
            }
            btnRefresh.ImageList = treeView1.ImageList = (DeviceDpi == 96) ? imageList1 : Images.ScaleList(imageList1, LogicalToDeviceUnits(imageList1.ImageSize));
            SetButtonsAsOpenAttach();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    SelectedIDEventArgs args = new SelectedIDEventArgs();
                    args.SelectedType = SelectedType.Associate;
                    args.SelectedID = Convert.ToInt32(treeView1.SelectedNode.Tag);
                    if (OpenClicked != null)
                        OpenClicked(this, args);
                }
                else
                {
                    SelectedIDEventArgs args = new SelectedIDEventArgs();
                    args.SelectedType = SelectedType.Document;
                    args.SelectedID = Convert.ToInt32(treeView1.SelectedNode.Tag);
                    if (OpenClicked != null)
                        OpenClicked(this, args);
                }
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    SelectedIDEventArgs args = new SelectedIDEventArgs();
                    args.SelectedType = SelectedType.Associate;
                    args.SelectedID = Convert.ToInt32(treeView1.SelectedNode.Tag);
                    if (AttachClicked != null)
                    {
                        AttachClicked(this, args);
                    }
                }
                else
                {
                    SelectedIDEventArgs args = new SelectedIDEventArgs();
                    args.SelectedType = SelectedType.Document;
                    args.SelectedID = Convert.ToInt32(treeView1.SelectedNode.Tag);
                    if (AttachClicked != null)
                    {
                        AttachClicked(this, args);
                    }
                }
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (doubleclickaction == DoubleClickAction.Attach)
                {
                    if (btnAttach.Enabled)
                        btnAttach_Click(sender, e);
                }
                else
                {
                    if (btnOpen.Enabled)
                        btnOpen_Click(sender, e);
                }
            }

        }

        public bool DisableEmailActions = true;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (e.Node.Parent == null)
            {
                if (DisableEmailActions)
                {
                    btnOpen.Enabled = false;
                    btnAttach.Enabled = false;
                }
                else
                {
                    btnOpen.Enabled = true;
                    btnAttach.Enabled = true;
                }

                btnViewAssoc.Visible = true;
                
                SetOpenAttachText("btnCC", "&CC", "btnBCC", "&BCC");
            }
            else
            {
                btnOpen.Enabled = true;
                btnAttach.Enabled = !DisableEmailActions;
                btnViewAssoc.Visible = false;
                SetButtonsAsOpenAttach();
            }       

        }

        private void SetButtonsAsOpenAttach()
        {
            SetOpenAttachText("btnOpen", "&Open", "btnAttach", "&Attach");
        }

        private void SetOpenAttachText(string btnOpenRes, string btnOpenDefault, string btnAttachRes, string btnAttachDefault)
        {
            if (Session.CurrentSession.Resources != null)
            {
                btnOpen.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource(btnOpenRes, btnOpenDefault, "").Text;
                btnAttach.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource(btnAttachRes, btnAttachDefault, "").Text;
            }
        }

        private void viewAssociateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FWBS.OMS.Associate newassociate = FWBS.OMS.Associate.GetAssociate(Convert.ToInt32(treeView1.SelectedNode.Tag));
            FWBS.OMS.UI.Windows.Services.ShowOMSItem(null, Session.CurrentSession.DefaultSystemForm(SystemForms.AssociateEdit), null, newassociate, new FWBS.Common.KeyValueCollection());
        }

        private bool bOpenEnabled;
        private bool bAttachEnabled;
        private string cnumDocs;

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Favourites fav = new Favourites("ASSDOCS");
                if (Convert.ToBoolean(fav.Param1(0)))
                {
                    rdoEmailsOnly.Checked = true;
                    rdoAll.Checked = false;
                }
                else
                {
                    rdoEmailsOnly.Checked = false;
                    rdoAll.Checked = true;
                }
                numDocs.Text = fav.Param2(0);
            }
            catch { }
            pnlOptions.Visible = true;
            grpOptions.Focus();
            treeView1.Enabled = false;

            bOpenEnabled = btnOpen.Enabled;
            bAttachEnabled = btnAttach.Enabled;
            cnumDocs = numDocs.Text;

            btnAttach.Enabled = false;
            btnOpen.Enabled = false;

            btnViewAssoc.Enabled = false;
            btnOptions.Enabled = false;
            btnRefresh.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            treeView1.Focus();
            pnlOptions.Visible = false;
            treeView1.Enabled = true;

            btnAttach.Enabled = bOpenEnabled;
            btnOpen.Enabled = bAttachEnabled;
            numDocs.Text = cnumDocs;

            btnViewAssoc.Enabled = true;
            btnOptions.Enabled = true;
            btnRefresh.Enabled = true;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Favourites fav = new Favourites("ASSDOCS");
            if (fav.Count > 0)
            fav.RemoveFavourite(0);
            fav.AddFavourite("Options","",rdoEmailsOnly.Checked.ToString(),numDocs.Text);
            fav.Update();
            eLastEmailsForAssociates_Load(sender, e);
            btnCancel_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                eLastEmailsForAssociates_Load(this, EventArgs.Empty);
            }
        }

    }

    public enum DoubleClickAction {Attach, Open};
    public enum SelectedType { Associate, Document };

    public class SelectedIDEventArgs : EventArgs
    {
        private int id;

        public int SelectedID
        {
            get { return id; }
            set { id = value; }
        }

        private SelectedType type;

        public SelectedType SelectedType
        {
            get { return type; }
            set { type = value; }
        }
	
	
    }

}