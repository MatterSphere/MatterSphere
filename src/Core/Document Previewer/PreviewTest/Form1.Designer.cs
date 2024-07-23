namespace PreviewTest
{
	partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.previewHandlerHost1 = new Fwbs.Documents.Preview.PreviewerControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.automateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewHandlerHost1
            // 
            this.previewHandlerHost1.AdditionalProperties = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("previewHandlerHost1.AdditionalProperties")));
            this.previewHandlerHost1.Container = null;
            this.previewHandlerHost1.CultureProperties = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("previewHandlerHost1.CultureProperties")));
            this.previewHandlerHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.previewHandlerHost1.Location = new System.Drawing.Point(220, 24);
            this.previewHandlerHost1.Name = "previewHandlerHost1";
            this.previewHandlerHost1.Size = new System.Drawing.Size(404, 357);
            this.previewHandlerHost1.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 24);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(220, 357);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sheToolStripMenuItem,
            this.automateToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sheToolStripMenuItem
            // 
            this.sheToolStripMenuItem.Name = "sheToolStripMenuItem";
            this.sheToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.sheToolStripMenuItem.Text = "Open (Shell)";
            this.sheToolStripMenuItem.Click += new System.EventHandler(this.sheToolStripMenuItem_Click);
            // 
            // automateToolStripMenuItem
            // 
            this.automateToolStripMenuItem.Name = "automateToolStripMenuItem";
            this.automateToolStripMenuItem.Size = new System.Drawing.Size(123, 20);
            this.automateToolStripMenuItem.Text = "Open (Automation)";
            this.automateToolStripMenuItem.Click += new System.EventHandler(this.automateToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(624, 381);
            this.Controls.Add(this.previewHandlerHost1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Fwbs.Documents.Preview.PreviewerControl previewHandlerHost1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem sheToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem automateToolStripMenuItem;

	}
}

