using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.FileManagement.Admin
{
    using FWBS.OMS.Data;
    using FWBS.OMS.Design.CodeBuilder;
    using FWBS.OMS.UI.Windows;
    using FWBS.OMS.UI.Windows.Admin;

    public class ApplicationEdit : ucEditBase2	
	{
		#region Fields

		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private System.Windows.Forms.ToolBarButton tbScript;
        private System.Windows.Forms.ToolBarButton tbSpVersioning;
        private System.Windows.Forms.ToolBarButton tbCheckin;
        private System.Windows.Forms.ToolBarButton tbCompare;
		private System.Windows.Forms.Splitter splitter1;
		private System.ComponentModel.IContainer components = null;
		private FMApplication _currentobj;
        private string _originalcode, _originalscript;
		private System.Windows.Forms.ToolBarButton tbSp3;
		private System.Windows.Forms.TabControl tabProperties;
		private System.Windows.Forms.TabPage tabProps;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter2;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.Windows.Forms.TabPage tabEvents;
        private FWBS.OMS.Design.CodeBuilder.DataGridEvents dataGridEvents1;
		private CodeWindow _codeWindow;
		private readonly string fileManagementAppTitle = "File Management Application";
        private readonly LockState ls;
        private FileManagementVersionDataArchiver filemgmtVersionDataArchiver;
        private VersionComparisonSelector vcs;

		#endregion

		#region Constructors

		public ApplicationEdit()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			fileManagementAppTitle = Session.CurrentSession.Resources.GetResource("FILEMANAPP", "File Management Application", null).Text;

            if (Session.CurrentSession.ObjectLocking)
                ls = new LockState();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
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
                if (_codeWindow != null)
                {
                    _codeWindow.FormClosing -= CodeWindow_FormClosing;
                    _codeWindow.Close();
                    _codeWindow.Dispose();
                    _codeWindow = null;
                }
			}
            if (_currentobj != null)
            {
                _currentobj.Dirty -= OnDirty;
                _currentobj.ScriptChanged -= _currentobj_ScriptChange;
                UnlockCurrentObject();
                _currentobj = null;
            }
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbScript = new System.Windows.Forms.ToolBarButton();
            this.tbSpVersioning = new System.Windows.Forms.ToolBarButton();
            this.tbCheckin = new System.Windows.Forms.ToolBarButton();
            this.tbCompare = new System.Windows.Forms.ToolBarButton();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tbSp3 = new System.Windows.Forms.ToolBarButton();
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.tabProps = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.dataGridEvents1 = new FWBS.OMS.Design.CodeBuilder.DataGridEvents();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.tabProps.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.tabProperties);
            this.tpEdit.Controls.Add(this.ucSearchControl1);
            this.tpEdit.Controls.Add(this.splitter1);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.tpEdit.Controls.SetChildIndex(this.tabProperties, 0);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSp3,
            this.tbScript,
            this.tbSpVersioning,
            this.tbCheckin,
            this.tbCompare});
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // tbScript
            // 
            this.tbScript.ImageIndex = 25;
            this.BresourceLookup1.SetLookup(this.tbScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditScript", "Edit Script", ""));
            this.tbScript.Name = "tbScript";
            // 
            // tbSpVersioning
            // 
            this.tbSpVersioning.Name = "tbSpVersioning";
            this.tbSpVersioning.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbCheckin
            // 
            this.tbCheckin.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbCheckin, new FWBS.OMS.UI.Windows.ResourceLookupItem("Checkin", "Check in", ""));
            this.tbCheckin.Name = "tbCheckin";
            // 
            // tbCompare
            // 
            this.tbCompare.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbCompare, new FWBS.OMS.UI.Windows.ResourceLookupItem("Compare", "Version Administration", ""));
            this.tbCompare.Name = "tbCompare";
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackGroundColor = System.Drawing.SystemColors.ControlDark;
            this.ucSearchControl1.ButtonPanelVisible = false;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSearchControl1.Location = new System.Drawing.Point(0, 50);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = null;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(544, 333);
            this.ucSearchControl1.TabIndex = 199;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(544, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 333);
            this.splitter1.TabIndex = 200;
            this.splitter1.TabStop = false;
            // 
            // tbSp3
            // 
            this.tbSp3.Name = "tbSp3";
            this.tbSp3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            this.tbSp3.Visible = false;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tabProps);
            this.tabProperties.Controls.Add(this.tabEvents);
            this.tabProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProperties.Location = new System.Drawing.Point(0, 50);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(544, 333);
            this.tabProperties.TabIndex = 201;
            // 
            // tabProps
            // 
            this.tabProps.Controls.Add(this.panel2);
            this.tabProps.Location = new System.Drawing.Point(4, 24);
            this.BresourceLookup1.SetLookup(this.tabProps, new FWBS.OMS.UI.Windows.ResourceLookupItem("Properties", "Properties", ""));
            this.tabProps.Name = "tabProps";
            this.tabProps.Size = new System.Drawing.Size(536, 305);
            this.tabProps.TabIndex = 0;
            this.tabProps.Text = "Properties";
            this.tabProps.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.pnlActions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(536, 305);
            this.panel2.TabIndex = 203;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(536, 247);
            this.propertyGrid1.TabIndex = 201;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 247);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(536, 3);
            this.splitter2.TabIndex = 203;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 250);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(536, 55);
            this.pnlActions.TabIndex = 202;
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.dataGridEvents1);
            this.tabEvents.Location = new System.Drawing.Point(4, 24);
            this.BresourceLookup1.SetLookup(this.tabEvents, new FWBS.OMS.UI.Windows.ResourceLookupItem("Events", "Events", ""));
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Size = new System.Drawing.Size(536, 305);
            this.tabEvents.TabIndex = 1;
            this.tabEvents.Text = "Events";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // dataGridEvents1
            // 
            this.dataGridEvents1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.dataGridEvents1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridEvents1.Enabled = false;
            this.dataGridEvents1.Location = new System.Drawing.Point(0, 0);
            this.dataGridEvents1.Name = "dataGridEvents1";
            this.dataGridEvents1.Padding = new System.Windows.Forms.Padding(1);
            this.dataGridEvents1.Size = new System.Drawing.Size(536, 305);
            this.dataGridEvents1.TabIndex = 1;
            this.dataGridEvents1.CodeButtonClick += new System.EventHandler(this.dataGridEvents1_CodeButtonClick);
            // 
            // ApplicationEdit
            // 
            this.Name = "ApplicationEdit";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            this.tabProps.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabEvents.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		protected override string SearchListName
		{
			get
			{
				return "ADMFMAPPS";
			}
		}

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || (_codeWindow != null && _codeWindow.IsDirty);
            }
            set
            {
                base.IsDirty = value;
                if (_codeWindow != null && value == false) _codeWindow.IsDirty = value;
            }
        }

        protected override void ShowList()
        {
            if (_codeWindow != null)
            {
                _codeWindow.Close();
                _codeWindow.FormClosing -= CodeWindow_FormClosing;
                _codeWindow = null;
            }
            if (_currentobj != null)
            {
                UnlockCurrentObject();
                _currentobj.Dirty -= OnDirty;
                _currentobj.ScriptChanged -= _currentobj_ScriptChange;
                _currentobj = null;
            }
            _originalcode = _originalscript = null;
            base.ShowList();
        }
	
		protected override bool UpdateData()
		{
			try
			{
                tabProperties.SelectedIndex = 0;
                if (!string.IsNullOrEmpty(_currentobj.ScriptName) && (_codeWindow != null) && (_codeWindow.IsDirty || _codeWindow.ScriptGen.IsDirty))
                {
                    if (!CodeWindow.SaveAndCompile())
                    {
                        CodeWindow.Show();
                        return false;
                    }
                }
				_currentobj.Update();
				this.IsDirty=false;
                UnlockCurrentObject();
				Load(_currentobj.Code,true,false);
                if (ls != null && !ls.CheckForOpenObjects(_currentobj.Code, LockableObjects.FileManagement))
                {
                    ls.LockFileManagementObject(_currentobj.Code);
                    ls.MarkObjectAsOpen(_currentobj.Code, LockableObjects.FileManagement);
                    ManageVersioningButtons(true);
                }
                else
                {
                    ManageVersioningButtons(true);
                }
				return true;
			}
			catch(Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(this,ex);
				return false;
			}
		}

		protected override bool CancelData()
		{
			try
			{
				_currentobj.Cancel();
				return true;
			}
			catch(Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(this,ex);
				return false;
			}
		}

		protected override void Clone(string Code)
		{
			Load(Code,true,true);
            if (ls != null && _currentobj != null && !string.IsNullOrEmpty(_currentobj.ScriptName))
            {
                ls.LockScriptObject(_currentobj.ScriptName);
                ls.MarkObjectAsOpen(_currentobj.ScriptName, LockableObjects.Script);
            }
            dataGridEvents1.Enabled = true;
		}
		
		protected override void LoadSingleItem(string Code)
		{
            if (ls != null)
            {
                if (!ls.CheckObjectLockState(Code, LockableObjects.FileManagement) && !ls.CheckIfObjectIsAlreadyOpen(Code, LockableObjects.FileManagement))
                {
                    Load(Code, false, false);
                    ls.LockFileManagementObject(Code);
                    ls.MarkObjectAsOpen(Code, LockableObjects.FileManagement);
                    ManageVersioningButtons(true);
                    dataGridEvents1.Enabled = true;
                }
            }
            else
            {
                Load(Code, false, false);
                ManageVersioningButtons(true);
                dataGridEvents1.Enabled = true;
            }
		}

		protected override void NewData()
		{
			Load("",true,false);
		}

		private new void Load(string Code, bool NoErrors, bool Clone, bool EnableButtons = false)
		{
			if (IsObjectDirty())
			{
				CodeWindow.Hide();

                bool isNewObject = Code == "" || Clone;
                _originalcode = _originalscript = string.Empty;
                labSelectedObject.Text = string.Format("{0} - {1}", isNewObject ? Session.CurrentSession.Resources.GetResource("Untitled", "Untitled", null).Text : Code, fileManagementAppTitle);
                tabProperties.SelectedIndex = 0;
                ShowEditor(isNewObject);
				Application.DoEvents();
				
				Cursor = Cursors.WaitCursor;
				try
				{
					if (Code == "" && Clone == false)
					{
                        _currentobj = new FileManagement.FMApplication();
					}
					else if (Code != "" && Clone)
					{
                        _currentobj = FMApplication.Clone(Code);
					}
					else
					{
                        if (Addins.Addin.LoadedFMApps.ContainsKey(Code))
                            _currentobj = (FMApplication)Addins.Addin.LoadedFMApps[Code];
                        else
                        {
                            _currentobj = new FileManagement.FMApplication();
                            _currentobj.Fetch(Code);
                        }
					}

					_currentobj.Dirty+=new EventHandler(OnDirty);
					propertyGrid1.SelectedObject = _currentobj;
					propertyGrid1.HelpVisible=true;
					_currentobj.ScriptChanged +=new EventHandler(_currentobj_ScriptChange);
                    CodeWindow.Text = Session.CurrentSession.Resources.GetResource("FMCODEBUILDER", "File Management Code Builder", "").Text;
                    CodeWindow.Load(_currentobj.Script.ScriptType, _currentobj.Script);
					((ApplicationScriptType)CodeWindow.ScriptType).SetAppObject(_currentobj);
                    dataGridEvents1.CurrentCodeSurface = CodeWindow;
                    dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
                    _originalcode = _currentobj.Code;
                    _originalscript = _currentobj.ScriptName;
					Application.DoEvents();
				}
				finally
				{
                    ManageVersioningButtons(EnableButtons);
					Application.DoEvents();
					Cursor = Cursors.Default;
				}
			}
		}

		public CodeWindow CodeWindow
		{
			get
			{
				if (_codeWindow == null)
				{
					_codeWindow = new CodeWindow();
                    _codeWindow.Init(null);
                    _codeWindow.FormClosing += CodeWindow_FormClosing;
				}
				return _codeWindow;
			}
		}

		private void _currentobj_ScriptChange(object sender, EventArgs e)
		{
			if (_currentobj.State != ObjectState.Added)
                _currentobj.NewScript();
            CodeWindow.Load(_currentobj.Script.ScriptType, _currentobj.Script);
			((ApplicationScriptType)CodeWindow.ScriptType).SetAppObject(_currentobj);
            dataGridEvents1.CurrentCodeSurface = CodeWindow;
            dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
            dataGridEvents1.Enabled = true;
		}

		private void dataGridEvents1_CodeButtonClick(object sender, System.EventArgs e)
		{
            tbScript_Click();
            if (CodeWindow.HasMethod(dataGridEvents1.SelectedMethodName))
            {
                CodeWindow.GotoMethod(dataGridEvents1.SelectedMethodName);
            }
            else
            {
                CodeWindow.GenerateHandler(dataGridEvents1.SelectedMethodName, new GenerateHandlerInfo() { DelegateType = dataGridEvents1.SelectedMethodData.Delegate });
            }
		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
            if (e.ChangedItem.PropertyDescriptor.ComponentType == typeof(FMApplication))
            {
                if (e.ChangedItem.PropertyDescriptor.Name == "Code" && !_originalcode.Equals((string)e.ChangedItem.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    ResourceItem message;
                    if (_currentobj.Exists(e.ChangedItem.Value))
                        message = Session.CurrentSession.Resources.GetMessage("FMMSGDUPLICATE", "You have entered a duplicate code.\n\nPlease enter a unique code for the new File Management Application.", "");
                    else if (_currentobj.State != ObjectState.Added)
                        message = Session.CurrentSession.Resources.GetMessage("30000", "The Code cannot be changed when set", "");

                    if (!string.IsNullOrEmpty(message.Text))
                    {
                        MessageBox.Show(message.Text, fileManagementAppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _currentobj.Code = _originalcode;
                        propertyGrid1.Refresh();
                    }

                    string code = (_currentobj.Code == "") ? Session.CurrentSession.Resources.GetResource("Untitled", "Untitled", null).Text : _currentobj.Code;
                    labSelectedObject.Text = string.Format("{0} - {1}", code, fileManagementAppTitle);
                }
                else if (ls != null && e.ChangedItem.PropertyDescriptor.Name == "ScriptName")
                {
                    string newscript = (string)e.ChangedItem.Value, prevscript = (string)e.OldValue;
                    if (!newscript.Equals(_originalscript, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (ls.CheckObjectLockState(newscript, LockableObjects.Script))
                        {
                            _currentobj.ScriptName = prevscript;
                            propertyGrid1.Refresh();
                            return;
                        }
                        else
                        {
                            ls.LockScriptObject(newscript);
                        }
                    }
                    if (!prevscript.Equals(_originalscript, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ls.UnlockScriptObject(prevscript);
                    }
                    if (CodeWindow.Visible)
                    {
                        ls.MarkObjectAsOpen(newscript, LockableObjects.Script);
                    }
                }
                ManageVersioningButtons(false);
            }
            IsDirty = true;
        }

		new private void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            if (e.Button == tbScript)
            {
                tbScript_Click();
            }
            else if (e.Button == tbCheckin)
            {
                if (CheckObjectIsValid())
                    tbCheckin_Click();
            }
            else if (e.Button == tbCompare)
            {
                if (CheckObjectIsValid())
                    tbCompare_Click();
            }
		}

        private void tbScript_Click()
        {
            if (!string.IsNullOrEmpty(_currentobj.Code) && string.IsNullOrEmpty(_currentobj.ScriptName))
            {
                _currentobj.ScriptName = Script.ScriptGen.GenerateUniqueName(_currentobj.Code);
            }
            CodeWindow.Show();
            CodeWindow.BringToFront();
            ls?.MarkObjectAsOpen(_currentobj.ScriptName, LockableObjects.Script);
            ManageVersioningButtons(false);
        }

		
		private void OnDirty(object sender, EventArgs e)
		{
			this.IsDirty=true;
		}


        // ************************************************************************************************
        //
        // CHECK IN
        //
        // ************************************************************************************************

        private void tbCheckin_Click()
        {
            CreateAndArchiveData(true);
        }

        private void CreateAndArchiveData(bool CheckInOnly)
        {
            // To go wherever the ‘Save and Version’ button/menu item will be located
            filemgmtVersionDataArchiver = new FileManagementVersionDataArchiver();
            if (!CheckInOnly)
                filemgmtVersionDataArchiver.Saved += new EventHandler(filemgmtVersionDataArchiver_Saved);
            filemgmtVersionDataArchiver.SaveData(GetFileManagementFormVersionDataSet(), _currentobj.Code, _currentobj.Version, versionID: Guid.NewGuid());
        }

        private void filemgmtVersionDataArchiver_Saved(object sender, EventArgs e)
        {
            filemgmtVersionDataArchiver.Saved -= new EventHandler(filemgmtVersionDataArchiver_Saved);
            OpenComparisonTool();
        }

        // Methods to build the DataSet for the Archiver objects to use.
        private DataTable BuildVersionDataTable()
        {
            const string sql = @"select * from dbFileManagementApplication where appCode = @code";
            IConnection connection = Session.CurrentSession.CurrentConnection;
            IDataParameter[] parList = new IDataParameter[1];
            parList[0] = connection.CreateParameter("code", _currentobj.Code);
            DataTable dt = connection.ExecuteSQL(sql, parList);
            dt.TableName = "dbFileManagementApplication";
            return dt;
        }

        private DataSet GetFileManagementFormVersionDataSet()
        {
            var versionDataToSave = new DataSet();
            var fmconfig = BuildVersionDataTable();
            versionDataToSave.Tables.Add(fmconfig);
            return versionDataToSave;
        }


        // ************************************************************************************************
        //
        // COMPARE
        //
        // ************************************************************************************************

        private void tbCompare_Click()
        {
            if (CheckObjectInIfNecessary())
                CreateAndArchiveData(false);
            else
                OpenComparisonTool();
        }

        private bool CheckObjectInIfNecessary()
        {
            const string sql = "select * from dbFileManagementVersionData where Code = @code and Version = @version";
            IConnection connection = Session.CurrentSession.CurrentConnection;
            IDataParameter[] parList = new IDataParameter[2];
            parList[0] = connection.CreateParameter("code", _currentobj.Code);
            parList[1] = connection.CreateParameter("version", _currentobj.Version);
            DataTable dt = connection.ExecuteSQL(sql, parList);
            return (dt == null || dt.Rows.Count == 0);
        }

        private void OpenComparisonTool()
        {
            vcs = new VersionComparisonSelector(_currentobj.Code, LockableObjects.FileManagement);
            vcs.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            var result = vcs.ShowDialog();
            vcs.StartPosition = FormStartPosition.CenterScreen;
            vcs.FormClosing += new FormClosingEventHandler(vcs_FormClosing);
        }

        private void vcs_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            Load(_currentobj.Code, true, false, true);
        }

        private void vcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            vcs.FormClosing -= new FormClosingEventHandler(vcs_FormClosing);
            vcs.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
        }

        private bool CheckObjectIsValid()
        {
            return (!string.IsNullOrWhiteSpace(_currentobj.Code) && FWBS.Common.ConvertDef.ToInt32(_currentobj.Version, 0) != 0);
        }


        // ************************************************************************************************
        //
        // CLOSE
        //
        // ************************************************************************************************

        protected override void CloseAndReturnToList()
        {
            if (this.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    ShowList();
                }
            }
            else
            {
                ShowList();
            }

        }

        private void UnlockCurrentObject()
        {
            if (ls == null) return;

            if (!string.IsNullOrWhiteSpace(_currentobj.ScriptName))
            {
                ls.MarkObjectAsClosed(_currentobj.ScriptName, LockableObjects.Script);
                ls.UnlockScriptObject(_currentobj.ScriptName);
            }
            if (!string.IsNullOrWhiteSpace(_currentobj.Code))
            {
                ls.MarkObjectAsClosed(_currentobj.Code, LockableObjects.FileManagement);
                ls.UnlockFileManagementObject(_currentobj.Code);
            }
            if (!string.IsNullOrEmpty(_originalscript) && !_originalscript.Equals(_currentobj.ScriptName, StringComparison.InvariantCultureIgnoreCase))
            {
                ls.UnlockScriptObject(_originalscript);
            }
        }

        private void CodeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentobj.ScriptName))
            {
                ls?.MarkObjectAsClosed(_currentobj.ScriptName, LockableObjects.Script);
                if (!IsDirty)
                    ManageVersioningButtons(true);
            }
        }

        private void ManageVersioningButtons(bool state)
        {
            this.tbCheckin.Enabled = state;
            this.tbCompare.Enabled = state;
        }

	}
}
