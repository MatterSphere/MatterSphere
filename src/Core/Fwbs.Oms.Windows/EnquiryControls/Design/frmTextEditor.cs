namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmTextEditor.
    /// </summary>
    public class frmTextEditor : BaseForm
	{
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public FWBS.Common.UI.Windows.eXPFrame grpOuput;
		private System.Windows.Forms.RadioButton rdoText;
		private System.Windows.Forms.RadioButton rdoRTF;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panel1;
        public FWBS.OMS.UI.Windows.ucRichTextEditor ucRichTextEditor1;

		public frmTextEditor()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTextEditor));
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpOuput = new FWBS.Common.UI.Windows.eXPFrame();
            this.rdoRTF = new System.Windows.Forms.RadioButton();
            this.rdoText = new System.Windows.Forms.RadioButton();
            this.ucRichTextEditor1 = new FWBS.OMS.UI.Windows.ucRichTextEditor();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpOuput.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(8, 7);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 38);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // grpOuput
            // 
            this.grpOuput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpOuput.Controls.Add(this.rdoRTF);
            this.grpOuput.Controls.Add(this.rdoText);
            this.grpOuput.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.grpOuput.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.grpOuput.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.grpOuput.Location = new System.Drawing.Point(6, 234);
            this.resourceLookup1.SetLookup(this.grpOuput, new FWBS.OMS.UI.Windows.ResourceLookupItem("Output", "Output", ""));
            this.grpOuput.Name = "grpOuput";
            this.grpOuput.Padding = new System.Windows.Forms.Padding(10, 16, 5, 5);
            this.grpOuput.Size = new System.Drawing.Size(77, 61);
            this.grpOuput.TabIndex = 15;
            this.grpOuput.Text = "Output";
            // 
            // rdoRTF
            // 
            this.rdoRTF.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoRTF.Location = new System.Drawing.Point(10, 35);
            this.resourceLookup1.SetLookup(this.rdoRTF, new FWBS.OMS.UI.Windows.ResourceLookupItem("RTF", "RTF", ""));
            this.rdoRTF.Name = "rdoRTF";
            this.rdoRTF.Size = new System.Drawing.Size(62, 19);
            this.rdoRTF.TabIndex = 1;
            this.rdoRTF.Text = "RTF";
            this.rdoRTF.Click += new System.EventHandler(this.Output_Click);
            // 
            // rdoText
            // 
            this.rdoText.Checked = true;
            this.rdoText.Dock = System.Windows.Forms.DockStyle.Top;
            this.rdoText.Location = new System.Drawing.Point(10, 16);
            this.resourceLookup1.SetLookup(this.rdoText, new FWBS.OMS.UI.Windows.ResourceLookupItem("Text", "Text", ""));
            this.rdoText.Name = "rdoText";
            this.rdoText.Size = new System.Drawing.Size(62, 19);
            this.rdoText.TabIndex = 0;
            this.rdoText.TabStop = true;
            this.rdoText.Text = "Text";
            this.rdoText.Click += new System.EventHandler(this.Output_Click);
            // 
            // ucRichTextEditor1
            // 
            this.ucRichTextEditor1.CaptionWidth = 0;
            this.ucRichTextEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRichTextEditor1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucRichTextEditor1.IsDirty = true;
            this.ucRichTextEditor1.Location = new System.Drawing.Point(4, 4);
            this.ucRichTextEditor1.Name = "ucRichTextEditor1";
            this.ucRichTextEditor1.omsDesignMode = false;
            this.ucRichTextEditor1.Size = new System.Drawing.Size(416, 303);
            this.ucRichTextEditor1.TabIndex = 16;
            this.ucRichTextEditor1.TextModeChanged += new System.EventHandler(this.ucRichTextEditor1_TextModeChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.grpOuput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(420, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(90, 303);
            this.panel1.TabIndex = 17;
            // 
            // frmTextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(514, 311);
            this.Controls.Add(this.ucRichTextEditor1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTextEditor";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Text Editor";
            this.Activated += new System.EventHandler(this.ucRichTextEditor1_TextModeChanged);
            this.grpOuput.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void ucRichTextEditor1_TextModeChanged(object sender, System.EventArgs e)
		{
			if (ucRichTextEditor1.TextMode == TextMode.Text)
				rdoText.Checked=true;
			else
				rdoRTF.Checked=true;
		}

		private void Output_Click(object sender, System.EventArgs e)
		{
			if (sender == this.rdoText)
				ucRichTextEditor1.TextMode = TextMode.Text;
			else
				ucRichTextEditor1.TextMode = TextMode.RTF;
		}
	}
}
