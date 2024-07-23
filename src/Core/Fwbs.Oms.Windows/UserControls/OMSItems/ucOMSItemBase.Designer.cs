namespace FWBS.OMS.UI.Windows
{
    partial class ucOMSItemBase
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
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.SuspendLayout();
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(0, 0);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(150, 150);
            this.enquiryForm1.TabIndex = 0;
            this.enquiryForm1.ToBeRefreshed = false;
            // 
            // ucOMSItemBase
            // 
            this.Controls.Add(this.enquiryForm1);
            this.Name = "ucOMSItemBase";
            this.ResumeLayout(false);

        }

        #endregion

        protected Windows.EnquiryForm enquiryForm1;
    }
}
