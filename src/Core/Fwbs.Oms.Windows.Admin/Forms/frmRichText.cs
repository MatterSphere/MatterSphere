using System;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmRichText.
    /// </summary>
    public class frmRichText : FWBS.OMS.UI.Windows.BaseForm
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNav1;
		private System.Windows.Forms.ImageList imageList1;
		private FWBS.OMS.UI.Windows.ucPanelNavTop ucPanelNavTop1;
		private FWBS.OMS.UI.Windows.ucNavPanel ucNavPanel1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private FWBS.OMS.UI.Windows.eToolbars eToolbars1;
		private FWBS.OMS.UI.Windows.ucNavCommands ucNavCommands1;
		private FWBS.OMS.UI.Windows.ucDateTimePicker ucDateTimePicker1;
		private FWBS.OMS.UI.Windows.ucDateTimePicker ucDateTimePicker2;
		private FWBS.OMS.UI.Windows.ucDateTimePicker ucDateTimePicker3;
		private FWBS.OMS.UI.Windows.ucDateTimePicker ucDateTimePicker4;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.Button btnPreClient;

		private System.ComponentModel.IContainer components;
		public frmRichText()
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
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRichText));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucPanelNav1 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands1 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucPanelNavTop1 = new FWBS.OMS.UI.Windows.ucPanelNavTop();
            this.ucNavPanel1 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.ucDateTimePicker1 = new FWBS.OMS.UI.Windows.ucDateTimePicker();
            this.eToolbars1 = new FWBS.OMS.UI.Windows.eToolbars();
            this.ucDateTimePicker2 = new FWBS.OMS.UI.Windows.ucDateTimePicker();
            this.ucDateTimePicker3 = new FWBS.OMS.UI.Windows.ucDateTimePicker();
            this.ucDateTimePicker4 = new FWBS.OMS.UI.Windows.ucDateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.btnPreClient = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.ucPanelNav1.SuspendLayout();
            this.ucPanelNavTop1.SuspendLayout();
            this.ucNavPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(314, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(122)))), ((int)(((byte)(215)))));
            this.panel1.Controls.Add(this.ucPanelNav1);
            this.panel1.Controls.Add(this.ucPanelNavTop1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(5, 39);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(177, 457);
            this.panel1.TabIndex = 6;
            // 
            // ucPanelNav1
            // 
            this.ucPanelNav1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ucPanelNav1.Brightness = 100;
            this.ucPanelNav1.Controls.Add(this.ucNavCommands1);
            this.ucPanelNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNav1.ExpandedHeight = 107;
            this.ucPanelNav1.HeaderBrightness = 0;
            this.ucPanelNav1.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNav1.Location = new System.Drawing.Point(8, 165);
            this.ucPanelNav1.Name = "ucPanelNav1";
            this.ucPanelNav1.Size = new System.Drawing.Size(161, 107);
            this.ucPanelNav1.TabIndex = 0;
            this.ucPanelNav1.TabStop = false;
            this.ucPanelNav1.Text = "Test Header";
            // 
            // ucNavCommands1
            // 
            this.ucNavCommands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands1.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands1.Name = "ucNavCommands1";
            this.ucNavCommands1.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavCommands1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands1.Size = new System.Drawing.Size(161, 76);
            this.ucNavCommands1.TabIndex = 15;
            this.ucNavCommands1.TabStop = false;
            // 
            // ucPanelNavTop1
            // 
            this.ucPanelNavTop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavTop1.Brightness = 115;
            this.ucPanelNavTop1.Controls.Add(this.ucNavPanel1);
            this.ucPanelNavTop1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavTop1.ExpandedHeight = 157;
            this.ucPanelNavTop1.HeaderBrightness = -50;
            this.ucPanelNavTop1.Image = ((System.Drawing.Image)(resources.GetObject("ucPanelNavTop1.Image")));
            this.ucPanelNavTop1.Location = new System.Drawing.Point(8, 8);
            this.ucPanelNavTop1.Name = "ucPanelNavTop1";
            this.ucPanelNavTop1.Size = new System.Drawing.Size(161, 157);
            this.ucPanelNavTop1.TabIndex = 1;
            this.ucPanelNavTop1.Text = "ucPanelNavTop1";
            this.ucPanelNavTop1.Load += new System.EventHandler(this.ucPanelNavTop1_Load);
            // 
            // ucNavPanel1
            // 
            this.ucNavPanel1.Controls.Add(this.linkLabel1);
            this.ucNavPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel1.Location = new System.Drawing.Point(0, 32);
            this.ucNavPanel1.Name = "ucNavPanel1";
            this.ucNavPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavPanel1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel1.Size = new System.Drawing.Size(161, 118);
            this.ucNavPanel1.TabIndex = 15;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.Location = new System.Drawing.Point(8, 15);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 27);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            this.imageList1.Images.SetKeyName(4, "");
            this.imageList1.Images.SetKeyName(5, "");
            this.imageList1.Images.SetKeyName(6, "");
            this.imageList1.Images.SetKeyName(7, "");
            this.imageList1.Images.SetKeyName(8, "");
            this.imageList1.Images.SetKeyName(9, "");
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(594, 39);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.SelectedObject = this.ucDateTimePicker1;
            this.propertyGrid1.Size = new System.Drawing.Size(248, 457);
            this.propertyGrid1.TabIndex = 12;
            this.propertyGrid1.Click += new System.EventHandler(this.propertyGrid1_Click);
            // 
            // ucDateTimePicker1
            // 
            this.ucDateTimePicker1.AllowNull = true;
            this.ucDateTimePicker1.CaptionWidth = 50;
            this.ucDateTimePicker1.DateTimeLayout = FWBS.OMS.UI.Windows.DateTimePickerLayout.dtpSameLine;
            this.ucDateTimePicker1.DateVisible = true;
            this.ucDateTimePicker1.DefaultTime = "09:00:00";
            this.ucDateTimePicker1.DisplayDateIn = System.DateTimeKind.Local;
            this.ucDateTimePicker1.GreaterThanToday = false;
            this.ucDateTimePicker1.IsDirty = true;
            this.ucDateTimePicker1.LessThanToday = false;
            this.ucDateTimePicker1.Location = new System.Drawing.Point(199, 66);
            this.ucDateTimePicker1.Margin = new System.Windows.Forms.Padding(0);
            this.ucDateTimePicker1.Name = "ucDateTimePicker1";
            this.ucDateTimePicker1.Size = new System.Drawing.Size(279, 24);
            this.ucDateTimePicker1.SlaveDatePicker = "";
            this.ucDateTimePicker1.SpecialOptionsVisible = true;
            this.ucDateTimePicker1.TabIndex = 1;
            this.ucDateTimePicker1.TimeLabelVisible = true;
            this.ucDateTimePicker1.TimeVisible = true;
            this.ucDateTimePicker1.Value = new System.DateTime(2003, 5, 1, 9, 0, 0, 0);
            // 
            // eToolbars1
            // 
            this.eToolbars1.BackColor = System.Drawing.SystemColors.Control;
            this.eToolbars1.BottomDivider = true;
            this.eToolbars1.ButtonsXML = resources.GetString("eToolbars1.ButtonsXML");
            this.eToolbars1.Divider = false;
            this.eToolbars1.DropDownArrows = true;
            this.eToolbars1.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons24;
            this.eToolbars1.Location = new System.Drawing.Point(5, 5);
            this.eToolbars1.Name = "eToolbars1";
            this.eToolbars1.NavCommandPanel = this.ucNavCommands1;
            this.eToolbars1.PanelImageLists = FWBS.OMS.UI.Windows.omsImageLists.ToolsButton16;
            this.eToolbars1.ShowToolTips = true;
            this.eToolbars1.Size = new System.Drawing.Size(837, 34);
            this.eToolbars1.TabIndex = 13;
            this.eToolbars1.TopDivider = false;
            this.eToolbars1.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.eToolbars1_ButtonClick);
            // 
            // ucDateTimePicker2
            // 
            this.ucDateTimePicker2.AllowNull = true;
            this.ucDateTimePicker2.CaptionWidth = 50;
            this.ucDateTimePicker2.DateTimeLayout = FWBS.OMS.UI.Windows.DateTimePickerLayout.dtpSameLine;
            this.ucDateTimePicker2.DateVisible = true;
            this.ucDateTimePicker2.DefaultTime = "09:00:00";
            this.ucDateTimePicker2.DisplayDateIn = System.DateTimeKind.Local;
            this.ucDateTimePicker2.GreaterThanToday = false;
            this.ucDateTimePicker2.IsDirty = true;
            this.ucDateTimePicker2.LessThanToday = false;
            this.ucDateTimePicker2.Location = new System.Drawing.Point(199, 97);
            this.ucDateTimePicker2.Margin = new System.Windows.Forms.Padding(0);
            this.ucDateTimePicker2.Name = "ucDateTimePicker2";
            this.ucDateTimePicker2.Size = new System.Drawing.Size(279, 24);
            this.ucDateTimePicker2.SlaveDatePicker = "";
            this.ucDateTimePicker2.SpecialOptionsVisible = true;
            this.ucDateTimePicker2.TabIndex = 2;
            this.ucDateTimePicker2.TimeLabelVisible = true;
            this.ucDateTimePicker2.TimeVisible = true;
            this.ucDateTimePicker2.Value = new System.DateTime(2003, 5, 1, 9, 0, 0, 0);
            // 
            // ucDateTimePicker3
            // 
            this.ucDateTimePicker3.AllowNull = true;
            this.ucDateTimePicker3.CaptionWidth = 50;
            this.ucDateTimePicker3.DateTimeLayout = FWBS.OMS.UI.Windows.DateTimePickerLayout.dtpSameLine;
            this.ucDateTimePicker3.DateVisible = true;
            this.ucDateTimePicker3.DefaultTime = "09:00:00";
            this.ucDateTimePicker3.DisplayDateIn = System.DateTimeKind.Local;
            this.ucDateTimePicker3.GreaterThanToday = false;
            this.ucDateTimePicker3.IsDirty = true;
            this.ucDateTimePicker3.LessThanToday = false;
            this.ucDateTimePicker3.Location = new System.Drawing.Point(199, 127);
            this.ucDateTimePicker3.Margin = new System.Windows.Forms.Padding(0);
            this.ucDateTimePicker3.Name = "ucDateTimePicker3";
            this.ucDateTimePicker3.Size = new System.Drawing.Size(279, 24);
            this.ucDateTimePicker3.SlaveDatePicker = "";
            this.ucDateTimePicker3.SpecialOptionsVisible = true;
            this.ucDateTimePicker3.TabIndex = 3;
            this.ucDateTimePicker3.TimeLabelVisible = true;
            this.ucDateTimePicker3.TimeVisible = true;
            this.ucDateTimePicker3.Value = new System.DateTime(2003, 5, 1, 9, 0, 0, 0);
            // 
            // ucDateTimePicker4
            // 
            this.ucDateTimePicker4.AllowNull = true;
            this.ucDateTimePicker4.CaptionWidth = 50;
            this.ucDateTimePicker4.DateTimeLayout = FWBS.OMS.UI.Windows.DateTimePickerLayout.dtpSameLine;
            this.ucDateTimePicker4.DateVisible = true;
            this.ucDateTimePicker4.DefaultTime = "09:00:00";
            this.ucDateTimePicker4.DisplayDateIn = System.DateTimeKind.Local;
            this.ucDateTimePicker4.GreaterThanToday = false;
            this.ucDateTimePicker4.IsDirty = true;
            this.ucDateTimePicker4.LessThanToday = false;
            this.ucDateTimePicker4.Location = new System.Drawing.Point(199, 158);
            this.ucDateTimePicker4.Margin = new System.Windows.Forms.Padding(0);
            this.ucDateTimePicker4.Name = "ucDateTimePicker4";
            this.ucDateTimePicker4.Size = new System.Drawing.Size(279, 24);
            this.ucDateTimePicker4.SlaveDatePicker = "";
            this.ucDateTimePicker4.SpecialOptionsVisible = true;
            this.ucDateTimePicker4.TabIndex = 4;
            this.ucDateTimePicker4.TimeLabelVisible = true;
            this.ucDateTimePicker4.TimeVisible = true;
            this.ucDateTimePicker4.Value = new System.DateTime(2003, 5, 1, 9, 0, 0, 0);
            // 
            // comboBox1
            // 
            this.comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBox1.Location = new System.Drawing.Point(274, 247);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.Text = "comboBox1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(246, 345);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 15;
            this.textBox1.Text = "textBox1";
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(214, 375);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 108);
            this.listBox1.TabIndex = 16;
            // 
            // listBox2
            // 
            this.listBox2.Location = new System.Drawing.Point(369, 373);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 108);
            this.listBox2.TabIndex = 17;
            // 
            // btnPreClient
            // 
            this.btnPreClient.Location = new System.Drawing.Point(471, 254);
            this.btnPreClient.Name = "btnPreClient";
            this.btnPreClient.Size = new System.Drawing.Size(75, 26);
            this.btnPreClient.TabIndex = 18;
            this.btnPreClient.Text = "PreClient";
            this.btnPreClient.Click += new System.EventHandler(this.btnPreClient_Click);
            // 
            // frmRichText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(847, 501);
            this.Controls.Add(this.btnPreClient);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ucDateTimePicker4);
            this.Controls.Add(this.ucDateTimePicker3);
            this.Controls.Add(this.ucDateTimePicker2);
            this.Controls.Add(this.ucDateTimePicker1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.eToolbars1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmRichText";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "0";
            this.Load += new System.EventHandler(this.frmRichText_Load);
            this.panel1.ResumeLayout(false);
            this.ucPanelNav1.ResumeLayout(false);
            this.ucPanelNavTop1.ResumeLayout(false);
            this.ucNavPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void frmRichText_Load(object sender, System.EventArgs e)
		{
		}

		private void eXPComboBox1_Changed(object sender, System.EventArgs e)
		{
		}

		private void ucPanelNavTop1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void ucNavCommands1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void omsComboImageBox1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void eToolbars1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void eToolbars1_ButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
		{
			MessageBox.Show(e.Button.Text);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			ucDateTimePicker1.Value = DBNull.Value;
		}

		private void propertyGrid1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void btnPreClient_Click(object sender, System.EventArgs e)
		{

		}

	}
}
