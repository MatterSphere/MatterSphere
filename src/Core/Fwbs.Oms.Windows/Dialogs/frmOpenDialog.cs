using System;
using System.Collections;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmOpenDialog.
    /// </summary>
    public enum frmOpenDialogSytles{Open,Save}
    internal class frmOpenSaveDialog : BaseForm
	{
		private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.TextBox Filename;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.Button btnOpen;
		private FWBS.Common.UI.Windows.ToolBar tbTools;
		private System.Windows.Forms.ToolBarButton tbUpFolder;
		public System.Windows.Forms.ToolBarButton tbNewFolder;
		private System.Windows.Forms.ToolBarButton tbSp1;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		public System.Windows.Forms.ToolBarButton tbRename;
		public System.Windows.Forms.ToolBarButton tbDel;
		public System.Windows.Forms.ToolBarButton tbSp2;
		private System.Windows.Forms.ToolBarButton tbView;
		private System.Windows.Forms.ContextMenu mnuView;
		private FWBS.OMS.UI.ListView lstView;
		public string Caption = "";
		private System.Windows.Forms.MenuItem mnuIcon;
		private System.Windows.Forms.MenuItem mnuDetail;
		private System.Windows.Forms.ColumnHeader cDescription;
		private System.Windows.Forms.ColumnHeader cCode;
		private System.Windows.Forms.ColumnHeader cType;
		public string Folder = @"\";
		public frmOpenDialogSytles frmOpenDialogSytle = frmOpenDialogSytles.Open;
		private int LastCol = -1;
		private System.Windows.Forms.ComboBox cmbTypes;
		private System.Windows.Forms.Label labFormName;
		public FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ColumnHeader cScript;
		private System.Windows.Forms.SortOrder LastOrder = System.Windows.Forms.SortOrder.None;


        public frmOpenSaveDialog(string folder)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            Folder = folder;
            SetImageLists();
            cCode.Text = ResourceLookup.GetLookupText("FormName").Replace(" : ", "");
            cDescription.Text = ResourceLookup.GetLookupText("Description");
            cType.Text = ResourceLookup.GetLookupText("Type");
            cScript.Text = ResourceLookup.GetLookupText("Script");

            //Column headers
            this.lstView.Columns[0].Text = Session.CurrentSession.Resources.GetResource("FORMNAME", "Form Name", "").Text;
            this.lstView.Columns[1].Text = Session.CurrentSession.Resources.GetResource("DESCRIPTION", "Description", "").Text;
            this.lstView.Columns[2].Text = Session.CurrentSession.Resources.GetResource("TYPE", "Type", "").Text;
            this.lstView.Columns[3].Text = Session.CurrentSession.Resources.GetResource("SCRIPT", "Script", "").Text;
        }

        private void SetImageLists()
        {
            tbTools.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
            lstView.LargeImageList = Images.GetFolderFormsIcons((Images.IconSize)LogicalToDeviceUnits(32));
            lstView.SmallImageList = Images.GetFolderFormsIcons((Images.IconSize)LogicalToDeviceUnits(16));
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            SetImageLists();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Code Lookups"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Test Form"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbTypes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Filename = new System.Windows.Forms.TextBox();
            this.labFormName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.lstView = new FWBS.OMS.UI.ListView();
            this.cCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cScript = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbTools = new FWBS.Common.UI.Windows.ToolBar();
            this.tbUpFolder = new System.Windows.Forms.ToolBarButton();
            this.tbSp1 = new System.Windows.Forms.ToolBarButton();
            this.tbNewFolder = new System.Windows.Forms.ToolBarButton();
            this.tbRename = new System.Windows.Forms.ToolBarButton();
            this.tbDel = new System.Windows.Forms.ToolBarButton();
            this.tbSp2 = new System.Windows.Forms.ToolBarButton();
            this.tbView = new System.Windows.Forms.ToolBarButton();
            this.mnuView = new System.Windows.Forms.ContextMenu();
            this.mnuIcon = new System.Windows.Forms.MenuItem();
            this.mnuDetail = new System.Windows.Forms.MenuItem();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\OpenSaveDialog";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.cmbTypes);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Filename);
            this.panel1.Controls.Add(this.labFormName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(0, 296);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(504, 65);
            this.panel1.TabIndex = 1;
            // 
            // cmbTypes
            // 
            this.cmbTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypes.Items.AddRange(new object[] {
            "All Forms",
            "System Forms",
            "Enquiry Forms"});
            this.cmbTypes.Location = new System.Drawing.Point(85, 34);
            this.cmbTypes.Name = "cmbTypes";
            this.cmbTypes.Size = new System.Drawing.Size(180, 23);
            this.cmbTypes.TabIndex = 1;
            this.cmbTypes.SelectionChangeCommitted += new System.EventHandler(this.cmbTypes_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Type", "Type : ", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Type : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Filename
            // 
            this.Filename.BackColor = System.Drawing.SystemColors.Window;
            this.Filename.Location = new System.Drawing.Point(85, 6);
            this.Filename.MaxLength = 15;
            this.Filename.Name = "Filename";
            this.Filename.Size = new System.Drawing.Size(180, 23);
            this.Filename.TabIndex = 0;
            this.Filename.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Filename_KeyPress);
            // 
            // labFormName
            // 
            this.labFormName.AutoSize = true;
            this.labFormName.Location = new System.Drawing.Point(3, 9);
            this.resourceLookup1.SetLookup(this.labFormName, new FWBS.OMS.UI.Windows.ResourceLookupItem("FormName", "Form Name : ", ""));
            this.labFormName.Name = "labFormName";
            this.labFormName.Size = new System.Drawing.Size(79, 15);
            this.labFormName.TabIndex = 14;
            this.labFormName.Text = "Form Name : ";
            this.labFormName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(425, 34);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpen.Location = new System.Drawing.Point(425, 6);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "&Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lstView
            // 
            this.lstView.AllowDrop = true;
            this.lstView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cCode,
            this.cDescription,
            this.cType,
            this.cScript});
            this.lstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstView.FullRowSelect = true;
            this.lstView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lstView.Location = new System.Drawing.Point(0, 26);
            this.lstView.MultiSelect = false;
            this.lstView.Name = "lstView";
            this.lstView.Size = new System.Drawing.Size(504, 270);
            this.lstView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstView.TabIndex = 4;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.View = System.Windows.Forms.View.Details;
            this.lstView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstView_ColumnClick);
            this.lstView.SelectedIndexChanged += new System.EventHandler(this.lstView_SelectedIndexChanged);
            this.lstView.DoubleClick += new System.EventHandler(this.lstView_DoubleClick);
            // 
            // cCode
            // 
            this.cCode.Text = "Form Name";
            this.cCode.Width = 150;
            // 
            // cDescription
            // 
            this.cDescription.Text = "Description";
            this.cDescription.Width = 225;
            // 
            // cType
            // 
            this.cType.Text = "Type";
            this.cType.Width = 80;
            // 
            // cScript
            // 
            this.cScript.Text = "Script";
            this.cScript.Width = 150;
            // 
            // tbTools
            // 
            this.tbTools.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbUpFolder,
            this.tbSp1,
            this.tbNewFolder,
            this.tbRename,
            this.tbDel,
            this.tbSp2,
            this.tbView});
            this.tbTools.Divider = false;
            this.tbTools.DropDownArrows = true;
            this.tbTools.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbTools.Location = new System.Drawing.Point(0, 0);
            this.tbTools.Name = "tbTools";
            this.tbTools.ShowToolTips = true;
            this.tbTools.Size = new System.Drawing.Size(504, 26);
            this.tbTools.TabIndex = 3;
            this.tbTools.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbTools.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbTools_ButtonClick);
            // 
            // tbUpFolder
            // 
            this.tbUpFolder.ImageIndex = 35;
            this.resourceLookup1.SetLookup(this.tbUpFolder, new FWBS.OMS.UI.Windows.ResourceLookupItem("Parent", "Parent", ""));
            this.tbUpFolder.Name = "tbUpFolder";
            // 
            // tbSp1
            // 
            this.tbSp1.Name = "tbSp1";
            this.tbSp1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbNewFolder
            // 
            this.tbNewFolder.ImageIndex = 42;
            this.tbNewFolder.Name = "tbNewFolder";
            // 
            // tbRename
            // 
            this.tbRename.Enabled = false;
            this.tbRename.ImageIndex = 41;
            this.tbRename.Name = "tbRename";
            // 
            // tbDel
            // 
            this.tbDel.ImageIndex = 6;
            this.tbDel.Name = "tbDel";
            // 
            // tbSp2
            // 
            this.tbSp2.Name = "tbSp2";
            this.tbSp2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbView
            // 
            this.tbView.DropDownMenu = this.mnuView;
            this.tbView.ImageIndex = 26;
            this.tbView.Name = "tbView";
            this.tbView.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            // 
            // mnuView
            // 
            this.mnuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuIcon,
            this.mnuDetail});
            // 
            // mnuIcon
            // 
            this.mnuIcon.Index = 0;
            this.mnuIcon.RadioCheck = true;
            this.mnuIcon.Text = "Icon View";
            this.mnuIcon.Click += new System.EventHandler(this.mnuIcon_Click);
            // 
            // mnuDetail
            // 
            this.mnuDetail.Checked = true;
            this.mnuDetail.Index = 1;
            this.mnuDetail.RadioCheck = true;
            this.mnuDetail.Text = "Detail View";
            this.mnuDetail.Click += new System.EventHandler(this.mnuDetail_Click);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            // 
            // frmOpenSaveDialog
            // 
            this.AcceptButton = this.btnOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(504, 361);
            this.Controls.Add(this.lstView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbTools);
            this.KeyPreview = true;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("OPEN", "Open", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOpenSaveDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open ...";
            this.Load += new System.EventHandler(this.frmOpenSaveDialog_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmOpenSaveDialog_KeyUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private string DisplayFolder(string FolderName)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				lstView.BeginUpdate();
				lstView.Items.Clear();
				bool AddOK=true;
				Hashtable folders = new Hashtable();
				DataTable dt = EnquiryEngine.Enquiry.GetEnquiryHeaders();
				dt.DefaultView.RowFilter = "enqpath like '" + FolderName + "%'";
				for(int i=0; i < dt.DefaultView.Count; i++)
				{
					DataRowView rw = dt.DefaultView[i];
					string foldername = ShowFolderName(rw[2].ToString(),FolderName);
					if (foldername == "")
					{
						AddOK=false;
						if (cmbTypes.Text == "System Forms" && Convert.ToBoolean(rw["enqSystem"])) AddOK=true;
						if (cmbTypes.Text == "Enquiry Forms" && Convert.ToBoolean(rw["enqSystem"]) == false) AddOK=true;
						if (cmbTypes.Text == "All Forms") AddOK=true;
						if (AddOK)
						{
							ListViewItem li = new ListViewItem();
							li.Tag = i;
							li.Text = rw[0].ToString();
							li.SubItems.Add(rw[1].ToString());
							if (Convert.ToBoolean(rw["enqSystem"]))
							{
								li.ImageIndex = 1;
								li.SubItems.Add("System Form");
							}
							else
							{
								li.ImageIndex = 2;
								li.SubItems.Add("Form");
							}
							li.SubItems.Add(Convert.ToString(rw["enqScript"]));
							lstView.Items.Add(li);
						}
					}
					else
					{
                        if (!folders.ContainsKey(foldername))
                        {
                            folders.Add(foldername, foldername);
                            ListViewItem li = new ListViewItem();
                            li.Tag = FolderName + foldername;
                            li.Text = foldername;
                            li.SubItems.Add("");
                            li.SubItems.Add("Folder");
                            li.ImageIndex = 0;
                            lstView.Items.Add(li);
                        }
					}
				}
			}
			finally
			{
				lstView.EndUpdate();
				this.Cursor = Cursors.Default;
			}
			return FolderName;
		}

		private string ShowFolderName(string FolderName, string currentfolder)
		{
			if (currentfolder.EndsWith(@"\") == false) currentfolder = currentfolder+ @"\";
			if (FolderName.EndsWith(@"\") == false) FolderName = FolderName + @"\";
			if (FolderName == currentfolder)
				return "";
			else
			{
				string output = FolderName.Substring(currentfolder.Length);
				int r = output.IndexOf(@"\");
				if (r==-1) r = output.Length-1;
				if (output != "")
					output = output.Substring(0,r);
				return output;
			}
		}

		private void frmOpenSaveDialog_Load(object sender, System.EventArgs e)
		{
			cmbTypes.SelectedIndex =0;
			DisplayFolder(Folder);
			SorterClass sc = new SorterClass(0,System.Windows.Forms.SortOrder.Ascending); 
			LastOrder = System.Windows.Forms.SortOrder.Ascending;
			LastCol = 0;
			lstView.ListViewItemSorter = sc; 

		}

		private void btnOpen_Click(object sender, System.EventArgs e)
		{
			if (frmOpenDialogSytle == frmOpenDialogSytles.Save && EnquiryEngine.Enquiry.Exists(Filename.Text))	
			{
				if (MessageBox.Show(Filename.Text + @" already exists Do you want to Overwrite?"
					,Text,MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
				{

					if (FWBS.OMS.EnquiryEngine.Enquiry.Overwrite(Filename.Text))
					{
						Caption = FWBS.OMS.CodeLookup.GetLookup("ENQHEADER",Filename.Text);
					}
					else
					{
						MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ENQSYS", "The Enquiry : %1% is a System Enquiry and can not be deleted", "", Filename.Text).Text,Text,MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
						this.DialogResult = DialogResult.None;
						return;
					}

				}
				else
				{
					this.DialogResult = DialogResult.None;
					return;
				}
			}
			else if (Filename.Text == "") 
			{
				this.DialogResult = DialogResult.None;
				return;
			}
			else if (frmOpenDialogSytle == frmOpenDialogSytles.Open)
			{
				foreach(ListViewItem lvi in lstView.Items)
				{
					if (lvi.ImageIndex == 0 && lvi.Text.ToUpper() == Filename.Text.ToUpper())
					{
						if (Folder.EndsWith(@"\") == false) Folder = Folder + @"\";
						Folder = DisplayFolder(Folder + Filename.Text);
						this.DialogResult = DialogResult.None;
						Filename.Text = "";
						return;
					}
				}
				if (EnquiryEngine.Enquiry.Exists(Filename.Text) == false) 
				{
					this.DialogResult = DialogResult.None;
					MessageBox.ShowInformation("ENQNOTEXISTS","The Enquiry Form '%1%' does not exist ... ",Filename.Text);
					return;
				}
			}
			this.DialogResult = DialogResult.OK;
		}

		private void frmOpenSaveDialog_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//// DMB 23/02/2004 added check to make sure text box is not focussed as gives strange results
			if (e.KeyCode == Keys.Back && Filename.Focused == false)
			{
				tbTools_ButtonClick(sender,new System.Windows.Forms.ToolBarButtonClickEventArgs(tbUpFolder));
				e.Handled=true;
			}
		}

		
		private void lstView_DoubleClick(object sender, System.EventArgs e)
		{
			if (lstView.SelectedItems[0].ImageIndex == 0)
			{
				if (Folder.EndsWith(@"\") == false) Folder = Folder + @"\";
				Folder = DisplayFolder(Folder + lstView.SelectedItems[0].Text);
			}
			else
			{
				Filename.Text = lstView.SelectedItems[0].Text;
				btnOpen_Click(sender,e);
			}
		}

		private void tbTools_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbUpFolder && Folder != @"\" && Folder != "")
			{
				Folder = Folder.Substring(0,Folder.LastIndexOf(@"\"));
				Folder = DisplayFolder(Folder);

			}
			if (e.Button == tbNewFolder)
			{
                using (frmNewFolder NewForm = new frmNewFolder("Untitled"))
                {
                    NewForm.ShowDialog(this);
                    if (NewForm.DialogResult == DialogResult.OK)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Tag = Folder + NewForm.FolderName.Text;
                        li.Text = NewForm.FolderName.Text;
                        li.SubItems.Add("");
                        li.SubItems.Add("Folder");
                        li.ImageIndex = 0;
                        li.EnsureVisible();
                        lstView.Items.Add(li);
                    }
                }
			}
			if (e.Button == tbView)
			{
				if (lstView.View == View.Details)
					mnuIcon_Click(sender,e);
				else
					mnuDetail_Click(sender,e);
			}
			if (e.Button == tbDel && Filename.Text != "")
			{
                DeleteEnquiryForm();
			}
		}

        private void DeleteEnquiryForm()
        {
            if (!CheckLockStateOfEnquiryForm(Filename.Text))
            {
                if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ENQDEL", "Are you sure you wish to Delete Enquiry : %1%", "", Filename.Text).Text, Session.CurrentSession.Resources.GetResource("OMSDESIGNER", "OMS Designer", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (FWBS.OMS.EnquiryEngine.Enquiry.Delete(Filename.Text) == true)
                        {
                            lstView.Items.Remove(lstView.SelectedItems[0]);
                            Filename.Text = "";
                        }
                        else
                        {
                            MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ENQSYS", "The Enquiry : %1% is a System Enquiry and can not be deleted", "", Filename.Text).Text, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    catch (Exception ex)
                    {
                        FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
                    }
                }
            }
        }

        private bool CheckLockStateOfEnquiryForm(string code)
        {
            bool result = false;
            if (FWBS.OMS.Session.CurrentSession.ObjectLocking)
            {
                LockState ls = new LockState();
                if (ls.CheckObjectLockState(code, LockableObjects.EnquiryForm))
                    return true;
            }
            return result;
        }

		private void lstView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				Filename.Text = lstView.SelectedItems[0].Text;
			}
			catch
			{
			}
		}

		private void mnuIcon_Click(object sender, System.EventArgs e)
		{
			lstView.View = View.LargeIcon;
			tbView.ImageIndex = 26;
			mnuIcon.Checked = true;
			mnuDetail.Checked = false;
		}

		private void mnuDetail_Click(object sender, System.EventArgs e)
		{
			lstView.View = View.Details;
			tbView.ImageIndex = 29;
			mnuDetail.Checked = true;
			mnuIcon.Checked = false;
		}

		private void lstView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			SorterClass sc=null;
			if (e.Column != LastCol || LastOrder != System.Windows.Forms.SortOrder.Ascending)
			{
				sc = new SorterClass(e.Column,System.Windows.Forms.SortOrder.Ascending);
                LastOrder = System.Windows.Forms.SortOrder.Ascending;
				LastCol = e.Column;
			}
			else
			{
				sc = new SorterClass(e.Column,System.Windows.Forms.SortOrder.Descending);
                LastOrder = System.Windows.Forms.SortOrder.Descending;
				LastCol = e.Column;
			}
			lstView.ListViewItemSorter = sc; 
		}

		private void cmbTypes_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			DisplayFolder(Folder);
		}

		private void Filename_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == '|') e.Handled=true;
			if (e.KeyChar == '[') e.Handled=true;
			if (e.KeyChar == ']') e.Handled=true;
		}
	}

	public class SorterClass : IComparer 
	{ 
		private int _colNum = 0;
        private System.Windows.Forms.SortOrder _colOrder = System.Windows.Forms.SortOrder.Ascending; 
		public SorterClass(int colNum, System.Windows.Forms.SortOrder colOrder) 
		{ 
			_colNum = colNum; 
			_colOrder = colOrder;
		} 

		//this routine should return -1 if xy and 0 if x==y. 
		// for our sample we'll just use string comparison 
		public int Compare(object x, object y) 
		{ 
			System.Windows.Forms.ListViewItem item1 = (System.Windows.Forms.ListViewItem) x; 
			System.Windows.Forms.ListViewItem item2 = (System.Windows.Forms.ListViewItem) y;

            int ordering = 1;

            if (_colOrder == System.Windows.Forms.SortOrder.Descending)
                ordering = -1;

            //If no sub item exists for the selected column use the first column
            string item1Text = (_colNum < item1.SubItems.Count) ? item1.SubItems[_colNum].Text : item1.SubItems[0].Text;
            string item2Text = (_colNum < item2.SubItems.Count) ? item2.SubItems[_colNum].Text : item2.SubItems[0].Text;

            int item1Type = 1; //0 for a folder 1 for anything else
            int item2Type = 1;

            //If the item is a folder set to to type 0 and use the folder name to sort by
            if (item1.ImageIndex == 0)
            {
                item1Text = item1.Text;
                item1Type = 0;
            }

            if (item2.ImageIndex == 0)
            {
                item2Text = item2.Text;
                item2Type = 0;
            }

            if (item1Type == item2Type)
                return ordering * String.Compare(item1Text, item2Text);
            else if (item2Type > item1Type) //Display folders at the top (Windows Explorer Style)
            {
                return -1;
            }
            else
            {
                return 1;
            }
		} 
	}


    /// <summary>
    /// Summary description for frmNewFolder.
    /// </summary>
    public class frmNewFolder : BaseForm
    {
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labFolder;
        public System.Windows.Forms.TextBox FolderName;
        /// <summary>
        /// Required designer variable.
        /// </summary>

        private frmNewFolder()
        {
        }

        public frmNewFolder(string foldername)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            FolderName.Text = foldername;

        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.labFolder = new System.Windows.Forms.Label();
            this.FolderName = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(325, 40);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(325, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labFolder
            // 
            this.labFolder.Location = new System.Drawing.Point(6, 11);
            this.labFolder.Name = "labFolder";
            this.labFolder.Size = new System.Drawing.Size(84, 23);
            this.labFolder.TabIndex = 6;
            this.labFolder.Text = "Folder Name :";
            this.labFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FolderName
            // 
            this.FolderName.Location = new System.Drawing.Point(92, 11);
            this.FolderName.MaxLength = 30;
            this.FolderName.Name = "FolderName";
            this.FolderName.Size = new System.Drawing.Size(224, 23);
            this.FolderName.TabIndex = 0;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.labFolder);
            this.panel.Controls.Add(this.FolderName);
            this.panel.Controls.Add(this.btnOK);
            this.panel.Controls.Add(this.btnCancel);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(408, 73);
            this.panel.TabIndex = 7;
            // 
            // frmNewFolderTmp
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(408, 73);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewFolder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Folder";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }

}
