using System;

namespace Fwbs.Oms.Office.Common.Panes
{
    using FWBS.OMS;
    using FWBS.OMS.EnquiryEngine;
    using FWBS.OMS.UI.Windows;

    public sealed class DocumentPropertyPane : BasePane
    {
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private EnquiryForm form;

        public DocumentPropertyPane()
        {
            InitializeComponent();

            btnSave.Text = Session.CurrentSession.Resources.GetResource("btnSave", "&Save", "").Text;
        }

        protected override void InternalRefresh(object activeDoc)
        {
            OMSDocument doc;
            try { doc = Addin.OMSApplication.GetCurrentDocument(activeDoc); } catch { doc = null; }
            if (doc != null)
            {
                bool changed = (Pane == null) || (doc.ID != ((OMSDocument)form.Enquiry.Object).ID);

                if (changed)
                {
                    using (DPIContextBlock contextBlock = SwitchDpiContext ? new DPIContextBlock(DPIAwareness.DPI_AWARENESS.SYSTEM_AWARE) : null)
                    {
                        form.Enquiry = Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.DocumentEdit), doc.OMSFile, doc, null);
                    }

                    if (Pane == null)
                    {
                        Pane = Panes.Add(this, form.Description);
                    }

                    if (Visible)
                    {
                        Pane.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionRight;
                        Pane.Width = LogicalToDeviceUnits(500);
                        Pane.Visible = true;
                    }
                    else
                    {
                        Pane.Visible = false;
                    }
                }
            }
            else
            {
                Pane.Visible = false;
            }
        }

        protected override void OnVisibleChanged()
        {

            if (!Visible)
            {
                form.CancelItem();
                form.RefreshItem();

            }

            Addin.RefreshUIControl("TabOMSDocument_View_DocPropToggle");
        }

        private void InitializeComponent()
        {
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.form = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(0, 118);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(150, 32);
            this.pnlButtons.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(70, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(76, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // form
            // 
            this.form.Dock = System.Windows.Forms.DockStyle.Fill;
            this.form.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.form.Location = new System.Drawing.Point(0, 0);
            this.form.Name = "form";
            this.form.Size = new System.Drawing.Size(150, 118);
            this.form.TabIndex = 1;
            // 
            // DocumentPropertyPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.form);
            this.Controls.Add(this.pnlButtons);
            this.Name = "DocumentPropertyPane";
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            form.UpdateItem();
        }

    }
}
