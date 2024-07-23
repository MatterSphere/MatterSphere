namespace FWBS.OMS.Design.CodeBuilder
{
	internal partial class NewWFActivityForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxActivityType = new System.Windows.Forms.ListBox();
            this.textBoxClassName = new System.Windows.Forms.TextBox();
            this.textBoxReturnTypeName = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.resourceLookup.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("ActivityType", "Activity Type:", ""));
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Activity Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 96);
            this.resourceLookup.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("ClassName", "Class Name:", ""));
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 101;
            this.label2.Text = "Class Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 123);
            this.resourceLookup.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("ReturnType", "Return Type:", ""));
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 100;
            this.label3.Text = "Return Type:";
            // 
            // listBoxActivityType
            // 
            this.listBoxActivityType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxActivityType.FormattingEnabled = true;
            this.listBoxActivityType.ItemHeight = 15;
            this.listBoxActivityType.Location = new System.Drawing.Point(114, 10);
            this.listBoxActivityType.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxActivityType.Name = "listBoxActivityType";
            this.listBoxActivityType.Size = new System.Drawing.Size(240, 79);
            this.listBoxActivityType.TabIndex = 1;
            // 
            // textBoxClassName
            // 
            this.textBoxClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClassName.Location = new System.Drawing.Point(114, 93);
            this.textBoxClassName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxClassName.Name = "textBoxClassName";
            this.textBoxClassName.Size = new System.Drawing.Size(240, 23);
            this.textBoxClassName.TabIndex = 2;
            this.textBoxClassName.TextChanged += new System.EventHandler(this.textBoxClassName_TextChanged);
            // 
            // textBoxReturnTypeName
            // 
            this.textBoxReturnTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReturnTypeName.Location = new System.Drawing.Point(114, 120);
            this.textBoxReturnTypeName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxReturnTypeName.Name = "textBoxReturnTypeName";
            this.textBoxReturnTypeName.Size = new System.Drawing.Size(240, 23);
            this.textBoxReturnTypeName.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(199, 152);
            this.resourceLookup.SetLookup(this.buttonOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 25);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "&OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(278, 152);
            this.resourceLookup.SetLookup(this.buttonCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 25);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cance&l";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.listBoxActivityType);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxClassName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxReturnTypeName);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 185);
            this.panel1.TabIndex = 102;
            // 
            // NewWFActivityForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(364, 185);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.resourceLookup.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("WFActivityForm", "Select New Activity Template", ""));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewWFActivityForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select New Activity Template";
            this.Load += new System.EventHandler(this.NewWFActivityForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listBoxActivityType;
		private System.Windows.Forms.TextBox textBoxClassName;
		private System.Windows.Forms.TextBox textBoxReturnTypeName;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
    }
}