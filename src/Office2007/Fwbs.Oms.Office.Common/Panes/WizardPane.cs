using System;
using System.Windows.Forms;

namespace Fwbs.Oms.Office.Common.Panes
{
    using FWBS.OMS;
    using FWBS.OMS.UI.Windows;

    public sealed class WizardPane : BasePane
    {
        private System.ComponentModel.IContainer components = null;
        private ResourceLookup resourceLookup;
        private TableLayoutPanel tableLayoutPanel;
        private Label lblTitle;
        private PictureBox pictureBox;
        private Label lblEntityNumberIs;
        private Label lblEntityNo;
        private CheckBox chkFinishAction;
        private Panel pnlSeparator;
        private Button btnDone;
        private Form _form;
        private object _omsObject;

        public WizardPane()
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
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.btnDone = new System.Windows.Forms.Button();
            this.chkFinishAction = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.lblEntityNumberIs = new System.Windows.Forms.Label();
            this.lblEntityNo = new System.Windows.Forms.Label();
            this.pnlSeparator = new System.Windows.Forms.Panel();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDone
            // 
            this.btnDone.AutoEllipsis = true;
            this.btnDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDone.FlatAppearance.BorderSize = 0;
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(284, 675);
            this.resourceLookup.SetLookup(this.btnDone, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNDONE", "Done", ""));
            this.btnDone.Margin = new System.Windows.Forms.Padding(12);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(124, 33);
            this.btnDone.TabIndex = 6;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // chkViewEntity
            // 
            this.chkFinishAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFinishAction.Location = new System.Drawing.Point(12, 675);
            this.resourceLookup.SetLookup(this.chkFinishAction, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkViewEntity", "View on completion", ""));
            this.chkFinishAction.Margin = new System.Windows.Forms.Padding(12, 12, 0, 12);
            this.chkFinishAction.Name = "chkViewEntity";
            this.chkFinishAction.Size = new System.Drawing.Size(260, 33);
            this.chkFinishAction.TabIndex = 4;
            this.chkFinishAction.Text = "View on completion";
            this.chkFinishAction.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel.Controls.Add(this.lblTitle, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.pictureBox, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lblEntityNumberIs, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.lblEntityNo, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.pnlSeparator, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.btnDone, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.chkFinishAction, 0, 6);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tableLayoutPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(420, 720);
            this.tableLayoutPanel.TabIndex = 0;
            this.tableLayoutPanel.Visible = false;
            // 
            // lblEntityCreated
            // 
            this.lblTitle.AutoEllipsis = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblEntityCreated";
            this.lblTitle.Size = new System.Drawing.Size(414, 136);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.UseMnemonic = false;
            // 
            // pictureBox
            // 
            this.tableLayoutPanel.SetColumnSpan(this.pictureBox, 2);
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 161);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(420, 260);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // lblEntityNumberIs
            // 
            this.lblEntityNumberIs.AutoEllipsis = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblEntityNumberIs, 2);
            this.lblEntityNumberIs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEntityNumberIs.Location = new System.Drawing.Point(3, 421);
            this.lblEntityNumberIs.Name = "lblEntityNumberIs";
            this.lblEntityNumberIs.Size = new System.Drawing.Size(414, 60);
            this.lblEntityNumberIs.TabIndex = 2;
            this.lblEntityNumberIs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblEntityNumberIs.UseMnemonic = false;
            // 
            // lblEntityNo
            // 
            this.tableLayoutPanel.SetColumnSpan(this.lblEntityNo, 2);
            this.lblEntityNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEntityNo.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.lblEntityNo.ForeColor = System.Drawing.Color.Black;
            this.lblEntityNo.Location = new System.Drawing.Point(3, 481);
            this.lblEntityNo.Name = "lblEntityNo";
            this.lblEntityNo.Size = new System.Drawing.Size(414, 40);
            this.lblEntityNo.TabIndex = 3;
            this.lblEntityNo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblEntityNo.UseMnemonic = false;
            // 
            // pnlSeparator
            // 
            this.pnlSeparator.BackColor = System.Drawing.Color.LightGray;
            this.tableLayoutPanel.SetColumnSpan(this.pnlSeparator, 2);
            this.pnlSeparator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSeparator.Location = new System.Drawing.Point(0, 662);
            this.pnlSeparator.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSeparator.Name = "pnlSeparator";
            this.pnlSeparator.Size = new System.Drawing.Size(420, 1);
            this.pnlSeparator.TabIndex = 5;
            // 
            // WizardPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "WizardPane";
            this.Size = new System.Drawing.Size(420, 720);
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void InternalRefresh(object activeDoc)
        {
            if (Pane == null)
            {
                using (DPIContextBlock contextBlock = SwitchDpiContext ? new DPIContextBlock(DPIAwareness.DPI_AWARENESS.SYSTEM_AWARE) : null)
                {
                    _form = OMSApp.CreateModelessWizard(Command, WizardStyle.TaskPane, _omsObject);
                    if (_form == null)
                    {
                        Dispose();
                        return;
                    }
                    _form.Dock = DockStyle.Fill;
                    _form.FormBorderStyle = FormBorderStyle.None;
                    Controls.Add(_form);
                    _form.BringToFront();
                    _form.FormClosed += OnFormClosed;
                    _form.Show();
                }

                Pane = Panes.Add(this, _form.Text);
                Pane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                Pane.DockPositionRestrict = Microsoft.Office.Core.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
                Pane.Width = LogicalToDeviceUnits(440);
                Pane.Visible = true;
            }
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _omsObject = _form.Tag;
            if (_form.DialogResult == DialogResult.OK || _form.DialogResult == DialogResult.Yes)
            {
                SetPaneInfo(_form.DialogResult == DialogResult.Yes);
                tableLayoutPanel.BringToFront();
                tableLayoutPanel.Visible = true;
            }
            else
            {
                Visible = false;
                Dispose();
            }
        }

        private void SetPaneInfo(bool existing)
        {
            Res resources = Session.CurrentSession.Resources;
            string imageName = null, newEntityName = string.Empty;
            switch (Command)
            {
                case "NEWFILE":
                    imageName = "new-matter-success";
                    newEntityName = resources.GetResource("NEWMATTER", "New Matter", "").Text;
                    lblEntityNumberIs.Text = resources.GetMessage("4005", "Your New %FILE% Number for '%2% (%3%)' is : %1%", "", true, string.Empty,
                        ((OMSFile)_omsObject).Client.ClientName, ((OMSFile)_omsObject).Client.ClientNo).Text.Trim();
                    lblEntityNo.Text = ((OMSFile)_omsObject).FileNo;
                    if (((OMSFile)_omsObject).GenerateJobsOnCreation)
                    {
                        BeginInvoke((Action)delegate { Services.CheckJobList(Addin.OMSApplication); });
                    }
                    break;

                case "NEWASSOC":
                    imageName = "new-associate-success";
                    newEntityName = resources.GetResource("NEWASSOCIATE", "New Associate", "").Text;
                    break;

                case "NEWCLIENT":
                    if (existing && ((Client)_omsObject).IsPreClient)
                        goto case "NEWPRECLIENT";
                    imageName = "new-client-success";
                    newEntityName = resources.GetResource("NEWCLIENT", "New Client", "").Text;
                    lblEntityNumberIs.Text = resources.GetMessage("4006", "Your New %CLIENT% Number is : %1%", "", true, string.Empty).Text.Trim();
                    lblEntityNo.Text = ((Client)_omsObject).ClientNo;
                    if (existing)
                    {
                        resourceLookup.SetLookup(chkFinishAction, new ResourceLookupItem("CREATEFILE", "Create %FILE%", ""));
                        chkFinishAction.Tag = "CREATEFILE";
                    }
                    break;

                case "NEWPRECLIENT":
                case "CREATEPRECLIENTCORPORATE":
                    if (existing && !((Client)_omsObject).IsPreClient)
                        goto case "NEWCLIENT";
                    imageName = "new-preclient-success";
                    newEntityName = resources.GetResource("NEWPRECLIENT", "New Pre-Client", "").Text;
                    lblEntityNumberIs.Text = resources.GetMessage("4006", "Your New %CLIENT% Number is : %1%", "", true, string.Empty).Text.Trim();
                    lblEntityNo.Text = ((Client)_omsObject).ClientNo;
                    if (existing)
                    {
                        resourceLookup.SetLookup(chkFinishAction, new ResourceLookupItem("CREATEFILE", "Create %FILE%", ""));
                        chkFinishAction.Tag = "CREATEFILE";
                    }
                    break;

                case "CREATECONTACT":
                    imageName = "new-contact-success";
                    newEntityName = resources.GetResource("NEWCONTACT", "New Contact", "").Text;
                    break;
            }

            chkFinishAction.Visible = (_omsObject != null);
            if (existing)
            {
                lblEntityNumberIs.Text = lblEntityNumberIs.Text.Replace(resources.GetResource("NEW", "New", "").Text + " ", string.Empty);
            }
            else
            {
                lblTitle.Text = newEntityName + " " + resources.GetResource("CREATED", "Created", "").Text;
            }

            if (imageName != null)
                pictureBox.Image = FWBS.OMS.UI.Windows.Images.GetNewEntityImage(imageName);
        }

        private void DoFinishAction()
        {
            if (chkFinishAction.Checked)
            {
                if (chkFinishAction.Tag as string == "CREATEFILE")
                {
                    WizardPane chainedWizard = Create<WizardPane>(Addin, "NEWFILE", Pane.Window, true);
                    chainedWizard._omsObject = _omsObject;
                    chainedWizard.Visible = true;
                }
                else if (_omsObject is OMSFile)
                    Services.ShowFile((OMSFile)_omsObject);
                else if (_omsObject is Client)
                    Services.ShowClient((Client)_omsObject);
                else if (_omsObject is Contact)
                    Services.ShowContact((Contact)_omsObject);
                else if (_omsObject is Associate)
                    Services.ShowAssociate((Associate)_omsObject);
            }
        }

        private void btnDone_Click(object sender, System.EventArgs e)
        {
            Visible = false;
            DoFinishAction();
            Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_form != null)
                {
                    _form.FormClosed -= OnFormClosed;
                    _form.DialogResult = DialogResult.Cancel;
                    _form.Close();
                    _form = null;
                }
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
