namespace FWBS.OMS.UI.Windows
{
    partial class ucOMSItemWizard
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_form != null)
                {
                    _form.FormClosed -= WizardClosed;
                    _form.Close();
                    _form.Dispose();
                    _form = null;
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.btnDone = new System.Windows.Forms.Button();
            this.lblTypeCreated = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlSecondPage = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlSecondPage.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.None;
            this.enquiryForm1.Visible = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblHeaderText);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.pnlHeader.Size = new System.Drawing.Size(800, 49);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btnClose.Location = new System.Drawing.Point(752, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(48, 48);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeaderText.Font = new System.Drawing.Font("Segoe UI Semibold", 12.75F);
            this.lblHeaderText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.lblHeaderText.Location = new System.Drawing.Point(0, 0);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Padding = new System.Windows.Forms.Padding(12, 12, 0, 0);
            this.lblHeaderText.Size = new System.Drawing.Size(12, 35);
            this.lblHeaderText.TabIndex = 0;
            // 
            // btnDone
            // 
            this.btnDone.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnDone.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDone.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Location = new System.Drawing.Point(334, 349);
            this.resourceLookup1.SetLookup(this.btnDone, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNDONE", "Done", ""));
            this.btnDone.Margin = new System.Windows.Forms.Padding(0);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(132, 32);
            this.btnDone.TabIndex = 0;
            this.btnDone.UseVisualStyleBackColor = false;
            this.btnDone.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTypeCreated
            // 
            this.lblTypeCreated.AutoEllipsis = true;
            this.lblTypeCreated.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTypeCreated.Font = new System.Drawing.Font("Segoe UI", 24F);
            this.lblTypeCreated.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.lblTypeCreated.Location = new System.Drawing.Point(0, 0);
            this.lblTypeCreated.Name = "lblTypeCreated";
            this.lblTypeCreated.Size = new System.Drawing.Size(800, 56);
            this.lblTypeCreated.TabIndex = 1;
            this.lblTypeCreated.Text = "New Type Created";
            this.lblTypeCreated.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTypeCreated.UseMnemonic = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(230, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(340, 264);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pnlSecondPage
            // 
            this.pnlSecondPage.AutoScroll = true;
            this.pnlSecondPage.Controls.Add(this.pnlContent);
            this.pnlSecondPage.Controls.Add(this.lblTypeCreated);
            this.pnlSecondPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSecondPage.Location = new System.Drawing.Point(0, 49);
            this.pnlSecondPage.Name = "pnlSecondPage";
            this.pnlSecondPage.Size = new System.Drawing.Size(800, 585);
            this.pnlSecondPage.TabIndex = 4;
            this.pnlSecondPage.Visible = false;
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.pictureBox1);
            this.pnlContent.Controls.Add(this.btnDone);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContent.Location = new System.Drawing.Point(0, 56);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 421);
            this.pnlContent.TabIndex = 4;
            // 
            // ucOMSItemWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(243)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlSecondPage);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucOMSItemWizard";
            this.Size = new System.Drawing.Size(800, 634);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.enquiryForm1, 0);
            this.Controls.SetChildIndex(this.pnlSecondPage, 0);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlSecondPage.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHeaderText;
        private Windows.ResourceLookup resourceLookup1;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlSecondPage;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblTypeCreated;
    }
}
