namespace FWBS.OMS.Design.CodeBuilder
{
    /// <summary>
    /// Summary description for frmInputBox.
    /// </summary>
    internal class frmCodeBuilder_ControlPicker : FWBS.OMS.UI.Windows.BaseForm
	{
		private System.Windows.Forms.PictureBox PictureBox;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		public System.Windows.Forms.Label Question;
		public System.Windows.Forms.ComboBox cmbControls;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		public System.Windows.Forms.ComboBox cmbIfMissing;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMain;
        private System.ComponentModel.IContainer components;

		internal frmCodeBuilder_ControlPicker()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        public bool MissingVisible
        {
            get { return cmbIfMissing.Visible; }
            set { label1.Visible = value; cmbIfMissing.Visible = value; }
        }
	

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeBuilder_ControlPicker));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Question = new System.Windows.Forms.Label();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbControls = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbIfMissing = new System.Windows.Forms.ComboBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(316, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(316, 36);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // Question
            // 
            this.Question.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Question.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Question.Location = new System.Drawing.Point(50, 9);
            this.resourceLookup1.SetLookup(this.Question, new FWBS.OMS.UI.Windows.ResourceLookupItem("labChooseCtrl", "Please choose the Control to Add Script Code for", ""));
            this.Question.Name = "Question";
            this.Question.Size = new System.Drawing.Size(260, 51);
            this.Question.TabIndex = 2;
            this.Question.Text = "Please choose the Control to Add Script Code for";
            // 
            // PictureBox
            // 
            this.PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox.Image")));
            this.PictureBox.Location = new System.Drawing.Point(8, 8);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(32, 32);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(8, 0);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("labIfMissing", "If Missing : ", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "If Missing : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbControls
            // 
            this.cmbControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbControls.Location = new System.Drawing.Point(8, 0);
            this.cmbControls.Name = "cmbControls";
            this.cmbControls.Size = new System.Drawing.Size(383, 23);
            this.cmbControls.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbControls);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 71);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.panel1.Size = new System.Drawing.Size(399, 28);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbIfMissing);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel2.Location = new System.Drawing.Point(0, 99);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.panel2.Size = new System.Drawing.Size(399, 32);
            this.panel2.TabIndex = 6;
            // 
            // cmbIfMissing
            // 
            this.cmbIfMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbIfMissing.Items.AddRange(new object[] {
            "Exception - Show Exception Dialog",
            "Create - Surpress Exception",
            "None - Throws Exception for you to Handle"});
            this.cmbIfMissing.Location = new System.Drawing.Point(73, 0);
            this.cmbIfMissing.Name = "cmbIfMissing";
            this.cmbIfMissing.Size = new System.Drawing.Size(318, 23);
            this.cmbIfMissing.TabIndex = 1;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.PictureBox);
            this.pnlMain.Controls.Add(this.Question);
            this.pnlMain.Controls.Add(this.btnOK);
            this.pnlMain.Controls.Add(this.btnCancel);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
            this.pnlMain.Size = new System.Drawing.Size(399, 71);
            this.pnlMain.TabIndex = 7;
            // 
            // frmCodeBuilder_ControlPicker
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 131);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmCtrlPicker", "Control Picker", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCodeBuilder_ControlPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Control Picker";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

	}
}
