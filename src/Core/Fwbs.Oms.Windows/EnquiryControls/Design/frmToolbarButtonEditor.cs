using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using FWBS.Common;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmToolbarButtonEditor.
    /// </summary>
    public class frmToolbarButtonEditor : BaseForm
	{
		#region Fields
		private FWBS.Common.ConfigSetting _buttonsxml = new ConfigSetting("");
		private int ButtonCountID = 1;
		private SortedList _buttonscollection = new SortedList();
        private ImageList _imageList;
		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labMembers;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Panel panel2;
		private FWBS.Common.UI.Windows.ToolBar ToolbarDesign;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private FWBS.OMS.UI.ListView listView1;
		private System.Windows.Forms.Button btnUp;
		private System.Windows.Forms.Button btnDn;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNav1;
		private FWBS.OMS.UI.Windows.ucNavCommands ucNavCommands1;
		private System.Windows.Forms.ColumnHeader chText;

		
		public frmToolbarButtonEditor()
		{
			InitializeComponent();
			ToolbarDesign.ShowToolTips=true;
		}

		private void LoadButtons()
		{
			LoadButtons(-1);
		}
			
		private void LoadButtons(int Pos)
		{
			int Index = 0;
			if (listView1.SelectedIndices.Count > 0)
				Index = listView1.SelectedIndices[0];
			listView1.BeginUpdate();
			listView1.Items.Clear();

  			Global.RemoveAndDisposeControls(ucNavCommands1);

			foreach (OMSToolBarButtonDesign bt in ToolbarDesign.Buttons)
			{
				listView1.Items.Add(bt.Text.Description,bt.ImageIndex);
				if (bt.PanelButtonVisible)
				{
					ucNavCommands1.Controls.Add(bt.PanelButton);
					if (bt.PanelButtonImageIndex != -1)
						bt.PanelButton.ImageIndex = bt.PanelButtonImageIndex;
					else
						bt.PanelButton.ImageIndex = bt.ImageIndex;
				}
			}
			listView1.EndUpdate();
			ucNavCommands1.Refresh();
			if (Pos != -1) Index = Pos;
			try
			{
				listView1.Items[Index].Focused=true;
				listView1.Items[Index].Selected=true;
			}
			catch
			{}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labMembers = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDn = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listView1 = new FWBS.OMS.UI.ListView();
            this.chText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.ToolbarDesign = new FWBS.Common.UI.Windows.ToolBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.ucPanelNav1 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands1 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.ucPanelNav1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 416);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 45);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(538, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(619, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // labMembers
            // 
            this.labMembers.Location = new System.Drawing.Point(8, 10);
            this.labMembers.Name = "labMembers";
            this.labMembers.Size = new System.Drawing.Size(193, 19);
            this.labMembers.TabIndex = 1;
            this.labMembers.Text = "Buttons Collection";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(240, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Properties";
            // 
            // btnUp
            // 
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUp.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnUp.Location = new System.Drawing.Point(208, 37);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 26);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "á";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDn
            // 
            this.btnDn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDn.Font = new System.Drawing.Font("Wingdings", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnDn.Location = new System.Drawing.Point(208, 65);
            this.btnDn.Name = "btnDn";
            this.btnDn.Size = new System.Drawing.Size(24, 26);
            this.btnDn.TabIndex = 5;
            this.btnDn.Text = "â";
            this.btnDn.Click += new System.EventHandler(this.btnDn_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(240, 35);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(286, 344);
            this.propertyGrid1.TabIndex = 6;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Location = new System.Drawing.Point(8, 35);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1);
            this.panel2.Size = new System.Drawing.Size(192, 313);
            this.panel2.TabIndex = 7;
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chText});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(1, 1);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(190, 311);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // chText
            // 
            this.chText.Text = "Text";
            this.chText.Width = 180;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(8, 354);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 25);
            this.btnAdd.TabIndex = 9;
            this.btnAdd.Text = "&Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemove.Location = new System.Drawing.Point(125, 354);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 25);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // ToolbarDesign
            // 
            this.ToolbarDesign.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.ToolbarDesign.DropDownArrows = true;
            this.ToolbarDesign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ToolbarDesign.Location = new System.Drawing.Point(0, 0);
            this.ToolbarDesign.Name = "ToolbarDesign";
            this.ToolbarDesign.ShowToolTips = true;
            this.ToolbarDesign.Size = new System.Drawing.Size(704, 28);
            this.ToolbarDesign.TabIndex = 10;
            this.ToolbarDesign.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.ToolbarDesign.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolbarDesign_ButtonClick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(704, 2);
            this.panel3.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(704, 1);
            this.panel4.TabIndex = 12;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labMembers);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.btnUp);
            this.panel5.Controls.Add(this.btnDn);
            this.panel5.Controls.Add(this.propertyGrid1);
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Controls.Add(this.btnAdd);
            this.panel5.Controls.Add(this.btnRemove);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel5.Location = new System.Drawing.Point(168, 31);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(536, 385);
            this.panel5.TabIndex = 13;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel6.Controls.Add(this.ucPanelNav1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel6.Location = new System.Drawing.Point(0, 31);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(7);
            this.panel6.Size = new System.Drawing.Size(168, 385);
            this.panel6.TabIndex = 14;
            // 
            // ucPanelNav1
            // 
            this.ucPanelNav1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNav1.Brightness = 100;
            this.ucPanelNav1.Controls.Add(this.ucNavCommands1);
            this.ucPanelNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNav1.ExpandedHeight = 31;
            this.ucPanelNav1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNav1.HeaderBrightness = 0;
            this.ucPanelNav1.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNav1.Location = new System.Drawing.Point(7, 7);
            this.ucPanelNav1.LockOpenClose = true;
            this.ucPanelNav1.Name = "ucPanelNav1";
            this.ucPanelNav1.Size = new System.Drawing.Size(154, 31);
            this.ucPanelNav1.TabIndex = 1;
            this.ucPanelNav1.TabStop = false;
            this.ucPanelNav1.Text = "Actions";
            this.ucPanelNav1.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavCommands1
            // 
            this.ucNavCommands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands1.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands1.Name = "ucNavCommands1";
            this.ucNavCommands1.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavCommands1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands1.Size = new System.Drawing.Size(154, 0);
            this.ucNavCommands1.TabIndex = 15;
            this.ucNavCommands1.TabStop = false;
            // 
            // frmToolbarButtonEditor
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(704, 461);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ToolbarDesign);
            this.Name = "frmToolbarButtonEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Toolbar Button Editor";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.ucPanelNav1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public ImageList ImageList
		{
			get
			{
                return _imageList;
			}
			set
			{
                _imageList = null;
                if (value != null)
                {
                    _imageList = Images.CloneList(value, "ToolbarDesign");
                    _imageList.Tag = value.Tag; // original DeviceDpi of the image list
                }
                ApplyImageList();
            }
        }

        private void ApplyImageList()
        {
            ImageList imageList = _imageList;
            if (imageList != null && DeviceDpi != (int)_imageList.Tag)
            {
                float scaleFactor = (float)DeviceDpi / (float)((int)_imageList.Tag);
                imageList = Images.CloneList(imageList, "ToolbarDesign");
                imageList.ImageSize = Size.Round(new SizeF(scaleFactor * imageList.ImageSize.Width, scaleFactor * imageList.ImageSize.Height));
            }
            ToolbarDesign.ImageList = imageList;
            listView1.SmallImageList = imageList;
            if (ucNavCommands1.ImageList == null)
                ucNavCommands1.ImageList = imageList;
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyImageList();
        }

		public ImageList PanelImageList
		{
			get
			{
				return ucNavCommands1.ImageList;
			}
			set
			{
				ucNavCommands1.ImageList = value;
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			OMSToolBarButtonDesign test = new OMSToolBarButtonDesign();
			bool success = false;
			do
			{
				try
				{
					if (test.Name == "") 
					{
						test.Name = "Button" + ButtonCountID.ToString();
						ButtonCountID++;
					}
					success = false;
					_buttonscollection.Add(test.Name,test);
					success = true;
				}
				catch
				{
					test.Name = "Button" + ButtonCountID.ToString();
					ButtonCountID++;
				}
			}
			while (success == false);
			test.ImageIndex = 0;
			ToolbarDesign.Buttons.Add(test);
			LoadButtons();
			foreach (ListViewItem i in listView1.SelectedItems)
				i.Selected = false;
			listView1.Items[listView1.Items.Count-1].Selected=true;
			listView1.Items[listView1.Items.Count-1].Focused=true;
		}


		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			LoadButtons();
		}

		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				OMSToolBarButtonDesign[] r = new OMSToolBarButtonDesign[listView1.SelectedIndices.Count];
				int u=0;
				foreach (int i in listView1.SelectedIndices)
				{
					r[u] = (OMSToolBarButtonDesign)ToolbarDesign.Buttons[i];
					u++;
				}
				propertyGrid1.SelectedObjects = r;
			}
			catch
			{}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			try
			{
				ToolbarDesign.Buttons.RemoveAt(listView1.SelectedIndices[0]);
			}
			catch
			{}
			LoadButtons();
		}

		private void btnUp_Click(object sender, System.EventArgs e)
		{
			try
			{
				ToolBarButton button = ToolbarDesign.Buttons[listView1.SelectedIndices[0]];
				int inx = listView1.SelectedIndices[0]-1;
				ToolbarDesign.Buttons.Insert(inx,CloneButton((OMSToolBarButtonDesign)button));
				ToolbarDesign.Buttons.Remove(button);
				LoadButtons(inx);
				listView1.Focus();
			}
			catch
			{}
		}

		private OMSToolBarButtonDesign CloneButton(OMSToolBarButtonDesign Button)
		{
			OMSToolBarButtonDesign but = new OMSToolBarButtonDesign();
			but.Enabled = Button.Enabled;
			but.ImageIndex = Button.ImageIndex;
			but.Pushed = Button.Pushed;
			but.Style = Button.Style;
			but.Tag = Button.Tag;
			but.Text = Button.Text;
			but.ToolTipText = Button.ToolTipText;
			but.Visible = Button.Visible;
			but.PanelButton = Button.PanelButton;
			but.PanelButtonCaption = Button.PanelButtonCaption;
			but.PanelButtonImageIndex = Button.PanelButtonImageIndex;
			but.PanelButtonVisible = Button.PanelButtonVisible;
			return but;
		}

		private void btnDn_Click(object sender, System.EventArgs e)
		{
			try
			{
				ToolBarButton button = ToolbarDesign.Buttons[listView1.SelectedIndices[0]];
				int inx = listView1.SelectedIndices[0]+2;
				ToolbarDesign.Buttons.Insert(inx,CloneButton((OMSToolBarButtonDesign)button));
				ToolbarDesign.Buttons.Remove(button);
				LoadButtons(inx -1);
				listView1.Focus();
			}
			catch
			{}

		}

		private void ToolbarDesign_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			int inx = ToolbarDesign.Buttons.IndexOf(e.Button);
			listView1.Items[inx].Selected=true;
			listView1.Items[inx].Focused=true;
		}

		public string ButtonsXML
		{
			get
			{
				_buttonsxml = new FWBS.Common.ConfigSetting("");
				foreach (OMSToolBarButtonDesign but in ToolbarDesign.Buttons)
				{
					ConfigSettingItem itmbtn = _buttonsxml.AddChildItem("Button");
					if (but.Enabled == false) itmbtn.SetString("Enabled",but.Enabled.ToString());
					if (but.ImageIndex > -1) itmbtn.SetString("ImageIndex",but.ImageIndex.ToString());
					if (but.Pushed) itmbtn.SetString("Pushed",but.Pushed.ToString());
					if (but.Style != ToolBarButtonStyle.PushButton) itmbtn.SetString("Style",but.Style.ToString());
					if (Convert.ToString(but.Tag) != "") itmbtn.SetString("Tag",Convert.ToString(but.Tag));
					if (Session.CurrentSession.IsLoggedIn)
					{
						if (but.Text.Code != "") itmbtn.SetString("Text",but.Text.Code);
					}
					else
					{
						if (but.Text.Code != "") itmbtn.SetString("Text",but.Text.Code + "¬" + but.Text.Description + "¬" + but.Text.Help);
					}	
					if (but.Visible == false) itmbtn.SetString("Visible",but.Visible.ToString());
					
					if (Session.CurrentSession.IsLoggedIn)
					{
						if (but.PanelButtonCaption.Code != "") itmbtn.SetString("PanelButtonCaption",but.PanelButtonCaption.Code);
					}
					else
					{
						if (but.PanelButtonCaption.Code != "") itmbtn.SetString("PanelButtonCaption",but.PanelButtonCaption.Code + "¬" + but.PanelButtonCaption.Description + "¬" + but.PanelButtonCaption.Help);
					}
					if (but.Name != "") itmbtn.SetString("Name",but.Name);
					if (but.Group != "") itmbtn.SetString("Group",but.Group);
					if (but.PanelButtonImageIndex != -1) itmbtn.SetString("PanelButtonImageIndex", but.PanelButtonImageIndex.ToString());
					if (but.PanelButtonVisible == true) itmbtn.SetString("PanelButtonVisible",but.PanelButtonVisible.ToString());
				}
				return _buttonsxml.ToString();
			}
			set
			{
				try
				{
					_buttonscollection.Clear();
					_buttonsxml = new FWBS.Common.ConfigSetting(value);
					foreach(ConfigSettingItem itmbtn in _buttonsxml.CurrentChildItems)
					{
						OMSToolBarButtonDesign but = new OMSToolBarButtonDesign();
						but.Enabled = Convert.ToBoolean(itmbtn.GetString("Enabled","true"));
						but.ImageIndex = Convert.ToInt32(itmbtn.GetString("ImageIndex","0"));
						but.Pushed = Convert.ToBoolean(itmbtn.GetString("Pushed","false"));
						but.Style = (ToolBarButtonStyle)ConvertDef.ToEnum(itmbtn.GetString("Style","PushButton"),ToolBarButtonStyle.PushButton);
						but.Tag = itmbtn.GetString("Tag","");
						if (Session.CurrentSession.IsLoggedIn)
						{
							but.Text.Code = itmbtn.GetString("Text","").Split('¬')[0];
						}
						else
						{
							string [] output = itmbtn.GetString("Text","").Split('¬');
							but.Text.Code = output[0];
							if (output.Length > 1) but.Text.Description = output[1];
							if (output.Length > 2) but.Text.Help = output[2];
						}
						but.Visible = Convert.ToBoolean(itmbtn.GetString("Visible","true"));
						but.Name = itmbtn.GetString("Name","");
						if (Session.CurrentSession.IsLoggedIn)
						{
							but.PanelButtonCaption.Code = itmbtn.GetString("PanelButtonCaption","").Split('¬')[0];
						}
						else
						{
							string [] output = itmbtn.GetString("PanelButtonCaption","").Split('¬');
							but.PanelButtonCaption.Code = output[0];
							if (output.Length > 1) but.PanelButtonCaption.Description = output[1];
							if (output.Length > 2) but.PanelButtonCaption.Help = output[2];
						}
						but.PanelButtonImageIndex = Convert.ToInt32(itmbtn.GetString("PanelButtonImageIndex", "-1"));
						but.PanelButtonVisible = Convert.ToBoolean(itmbtn.GetString("PanelButtonVisible","false"));
						but.Group = itmbtn.GetString("Group","");
						ToolbarDesign.Buttons.Add(but);
						bool success = false;
						do
						{
							try
							{
								if (but.Name == "") 
								{
									but.Name = "Button" + ButtonCountID.ToString();
									ButtonCountID++;
								}
								success = false;
								_buttonscollection.Add(but.Name,but);
								success = true;
							}
							catch
							{
								but.Name = "Button" + ButtonCountID.ToString();
								ButtonCountID++;
							}
						}
						while (success == false);
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex);
				}
				LoadButtons();
			}
		}

		public System.Windows.Forms.ToolBar.ToolBarButtonCollection Buttons
		{
			get
			{
				return ToolbarDesign.Buttons;
			}
		}
	}


	internal class OMSToolBarButtonDesign : ToolBarButton
	{
		private bool _panelbuttonvisible = false;
		private CodeLookupDisplay _text = new CodeLookupDisplay("SLBUTTON");
		private CodeLookupDisplay _pnltext = new CodeLookupDisplay("SLPBUTTON");
		private ucNavCmdButtons _panelbutton = new ucNavCmdButtons();
		private int _pnlimageindex = -1;
		private string _group = "";
		
		public OMSToolBarButtonDesign() : base()
		{
			_text.CodeLookupDisplayChanged +=new EventHandler(CodeLookupDisplayChanged);
			_pnltext.CodeLookupDisplayChanged +=new EventHandler(PanelCodeLookupDisplayChanged);
		}

		public new ToolBarButtonStyle Style
		{
			get
			{
				return base.Style;
			}
			set
			{
				base.Style = value;
				if (value == ToolBarButtonStyle.Separator)
				{
					this.Text.Code = "";
					base.Text = "Seperator";
					this.ImageIndex = -1;
				}
			}
		}

		public new CodeLookupDisplay Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (this.Style != ToolBarButtonStyle.Separator)
				{
					_text = value;
					base.Text = _text.Description;
					base.ToolTipText = _text.Help;
				}
				_text.CodeLookupDisplayChanged +=new EventHandler(CodeLookupDisplayChanged);
			}
		}


		[Category("(Details)")]
		[Description("Used to Set the Visiblity or Enability of a Group of Buttons")]
		public string Group
		{
			get
			{
				return _group;
			}
			set
			{
				_group = value;
			}
		}

		
		[Category("Panels Enabled")]
		public bool PanelButtonVisible
		{
			get
			{
				return _panelbuttonvisible;
			}
			set
			{
				_panelbuttonvisible = value;
			}
		}

		[Category("Panels ")]
		[CodeLookupSelectorTitle("PANELS","Panels")]
		public CodeLookupDisplay PanelButtonCaption
		{
			get
			{
				return _pnltext;
			}
			set
			{
				_pnltext = value;
				this.PanelButton.Text = _pnltext.Description;
				_pnltext.CodeLookupDisplayChanged +=new EventHandler(PanelCodeLookupDisplayChanged);
			}
		}

		[Browsable(false)]
		public ImageList ImageList
		{
			get
			{
				return this.Parent.ImageList;
			}
		}

		[Category("Panels ")]
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(IconDisplayEditor),typeof(UITypeEditor))]
		public int PanelButtonImageIndex
		{
			get
			{
				return _pnlimageindex;
			}
			set
			{
				_pnlimageindex = value;
			}
		}

		[Browsable(false)]
		public ucNavCmdButtons PanelButton
		{
			get
			{
				return _panelbutton;
			}
			set
			{
				_panelbutton = value;
			}
		}

		#region Hide Properties
		[Browsable(false)]
		public new Menu DropDownMenu
		{
			get
			{
				return base.DropDownMenu;
			}
			set
			{
				base.DropDownMenu = value;
			}
		}

		[Browsable(false)]
		public new Rectangle Rectangle
		{
			get
			{
				return base.Rectangle;
			}
		}

		[Browsable(false)]
		public new bool PartialPush
		{
			get
			{
				return base.PartialPush;
			}
			set
			{
				base.PartialPush = value;
			}
		}
		#endregion

		private void PanelCodeLookupDisplayChanged(object sender, EventArgs e)
		{
			this.PanelButton.Text = this.PanelButtonCaption.Description;
		}
		
		private void CodeLookupDisplayChanged(object sender, EventArgs e)
		{
			base.Text = _text.Description;
			base.ToolTipText = _text.Help;
		}
	}

}
