using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmInputBox.
    /// </summary>
    internal class frmInputBox : BaseForm
	{
		public System.Windows.Forms.TextBox TextBox;
		private System.Windows.Forms.PictureBox PictureBox;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		public System.Windows.Forms.Label Question;
		private System.ComponentModel.IContainer components;
        private Panel pnlBackground;
        private bool required = false;

		internal frmInputBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            PictureBox.Image = Images.GetCoolButton(41, (Images.IconSize)LogicalToDeviceUnits(32)).ToBitmap();
		}

		internal frmInputBox(string Question, string DefaultText) : this(Question, DefaultText, false)
		{
		}

		internal frmInputBox(string Question, string DefaultText, bool required) : this(Question, DefaultText, required, !required)
		{
		}

        internal frmInputBox(string Question, string DefaultText, bool required, bool displayCancel) : this()
        {
            this.Question.Text = Question;
            this.TextBox.Text = DefaultText;

            this.required = required;

            btnCancel.Visible = displayCancel;
            this.ControlBox = displayCancel;
            if (required && string.IsNullOrEmpty(TextBox.Text))
                btnOK.Enabled = false;
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            PictureBox.Image = Images.GetCoolButton(41, (Images.IconSize)LogicalToDeviceUnits(32)).ToBitmap();
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Question = new System.Windows.Forms.Label();
            this.TextBox = new System.Windows.Forms.TextBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlBackground = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(335, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(335, 38);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cance&l";
            // 
            // Question
            // 
            this.Question.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Question.Location = new System.Drawing.Point(46, 9);
            this.Question.Name = "Question";
            this.Question.Size = new System.Drawing.Size(283, 77);
            this.Question.TabIndex = 0;
            this.Question.Text = "Enter you question here ....";
            // 
            // TextBox
            // 
            this.TextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TextBox.Location = new System.Drawing.Point(8, 98);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(403, 23);
            this.TextBox.TabIndex = 1;
            this.TextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // PictureBox
            // 
            this.PictureBox.Location = new System.Drawing.Point(8, 8);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(32, 32);
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.PictureBox);
            this.pnlBackground.Controls.Add(this.Question);
            this.pnlBackground.Controls.Add(this.btnOK);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.TextBox);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Padding = new System.Windows.Forms.Padding(8);
            this.pnlBackground.Size = new System.Drawing.Size(419, 129);
            this.pnlBackground.TabIndex = 0;
            // 
            // frmInputBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(419, 129);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Box";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (required)
                btnOK.Enabled = !string.IsNullOrEmpty(TextBox.Text);
        }

	}
}
