namespace FWBS.OMS.UI.Windows
{
    public class ucAssociateVertSelector : ucAssociateSelector
    {
        public ucAssociateVertSelector()
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
            this.pnlTitle.SuspendLayout();
            this.border.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboType
            // 
            this.cboType.CaptionTop = true;
            this.cboType.Location = new System.Drawing.Point(8, 8);
            this.cboType.Size = new System.Drawing.Size(158, 39);
            // 
            // cboHeading
            // 
            this.cboHeading.CaptionTop = true;
            this.cboHeading.Location = new System.Drawing.Point(174, 8);
            this.cboHeading.Size = new System.Drawing.Size(242, 39);
            // 
            // txtSalutation
            // 
            this.txtSalutation.CaptionTop = true;
            this.txtSalutation.Location = new System.Drawing.Point(8, 49);
            this.txtSalutation.Size = new System.Drawing.Size(158, 39);
            // 
            // txtHeading
            // 
            this.txtHeading.CaptionTop = true;
            this.txtHeading.Location = new System.Drawing.Point(174, 49);
            this.txtHeading.Size = new System.Drawing.Size(242, 80);
            // 
            // txtTheirRef
            // 
            this.txtTheirRef.CaptionTop = true;
            this.txtTheirRef.Location = new System.Drawing.Point(8, 90);
            this.txtTheirRef.Size = new System.Drawing.Size(158, 39);
            // 
            // txtAddressee
            // 
            this.txtAddressee.CaptionTop = true;
            this.txtAddressee.Location = new System.Drawing.Point(8, 131);
            this.txtAddressee.Size = new System.Drawing.Size(158, 39);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(344, 147);
            // 
            // address
            // 
            this.address.CaptionTop = true;
            this.address.Location = new System.Drawing.Point(8, 200);
            this.address.Size = new System.Drawing.Size(158, 160);
            // 
            // txtEmail
            // 
            this.txtEmail.CaptionTop = true;
            this.txtEmail.Location = new System.Drawing.Point(174, 200);
            this.txtEmail.Size = new System.Drawing.Size(242, 40);
            // 
            // txtTel
            // 
            this.txtTel.CaptionTop = true;
            this.txtTel.Location = new System.Drawing.Point(174, 240);
            this.txtTel.Size = new System.Drawing.Size(242, 40);
            // 
            // txtMobile
            // 
            this.txtMobile.CaptionTop = true;
            this.txtMobile.Location = new System.Drawing.Point(174, 280);
            this.txtMobile.Size = new System.Drawing.Size(242, 40);
            // 
            // txtFax
            // 
            this.txtFax.CaptionTop = true;
            this.txtFax.Location = new System.Drawing.Point(174, 320);
            this.txtFax.Size = new System.Drawing.Size(242, 40);
            // 
            // eCaptionLine1
            // 
            this.eCaptionLine1.Location = new System.Drawing.Point(8, 179);
            // 
            // border
            // 
            this.border.Size = new System.Drawing.Size(424, 176);
            // 
            // ucAssociateVertSelector
            // 
            this.Name = "ucAssociateVertSelector";
            this.Size = new System.Drawing.Size(424, 204);
            this.pnlTitle.ResumeLayout(false);
            this.border.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override int MaxHeight
        {
            get { return LogicalToDeviceUnits(394); }
        }

        protected override int MinHeight
        {
            get { return LogicalToDeviceUnits(204); }
        }

    }
}
