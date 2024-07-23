using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.FileManagement.Admin
{
    using Data;
    using FWBS.OMS.UI.Windows.Admin;
    using OMS.UI.Windows;
    using Script;

    public class ApplicationEditV2 : ucEditBase2
	{
		#region Fields

		private System.ComponentModel.IContainer components = null;
		private FMApplication _currentobj;
        private string _originalcode, _originalscript;
        private ToolBarButton tbSpVersioning;
        private ToolBarButton tbCheckin;
        private ToolBarButton tbCompare;
		private TabControl tabProperties;
		private TabPage tabProps;
		private Panel panel2;
		private PropertyGrid propertyGrid1;
		private Splitter splitter2;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private Splitter splitter1;
		private readonly string fileManagementAppTitle = "File Management Application";
        private readonly LockState ls;
        private FileManagementVersionDataArchiver filemgmtVersionDataArchiver;
        private VersionComparisonSelector vcs;
		private FWBS.OMS.FileManagement.Design.FMDesigner taskflowDesigner1;
		#endregion

		#region Constructors

		public ApplicationEditV2()
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
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
            if (_currentobj != null)
            {
                _currentobj.Dirty -= OnDirty;
                UnlockCurrentObject();
                _currentobj = null;
            }
			base.Dispose(disposing);
		}


		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbSpVersioning = new System.Windows.Forms.ToolBarButton();
            this.tbCheckin = new System.Windows.Forms.ToolBarButton();
            this.tbCompare = new System.Windows.Forms.ToolBarButton();
            this.tabProps = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.tabProps.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpList.Location = new System.Drawing.Point(8, 8);
            this.tpList.Size = new System.Drawing.Size(664, 409);
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.tabProperties);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(638, 388);
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.tabProperties, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(638, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Size = new System.Drawing.Size(638, 22);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSpVersioning,
            this.tbCheckin,
            this.tbCompare});
            this.tbcEdit.Size = new System.Drawing.Size(638, 26);
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
            // lstList
            // 
            this.lstList.Size = new System.Drawing.Size(664, 409);
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
            // tabProps
            // 
            this.tabProps.Controls.Add(this.panel2);
            this.tabProps.Location = new System.Drawing.Point(4, 24);
            this.BresourceLookup1.SetLookup(this.tabProps, new FWBS.OMS.UI.Windows.ResourceLookupItem("Properties", "Properties", ""));
            this.tabProps.Name = "tabProps";
            this.tabProps.Size = new System.Drawing.Size(246, 310);
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
            this.panel2.Size = new System.Drawing.Size(246, 310);
            this.panel2.TabIndex = 203;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(246, 252);
            this.propertyGrid1.TabIndex = 201;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 252);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(246, 3);
            this.splitter2.TabIndex = 203;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 255);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(246, 55);
            this.pnlActions.TabIndex = 202;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tabProps);
            this.tabProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabProperties.Location = new System.Drawing.Point(384, 50);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(254, 338);
            this.tabProperties.TabIndex = 201;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(381, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 338);
            this.splitter1.TabIndex = 203;
            this.splitter1.TabStop = false;
            // 
            // ApplicationEditV2
            // 
            this.Name = "ApplicationEditV2";
            this.Size = new System.Drawing.Size(680, 425);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.tabProps.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
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
				return (base.IsDirty || this.IsScriptDirty());
			}
			set
			{
				base.IsDirty = value;
			}
		}

		protected override bool UpdateData()
		{
			try
			{
				if (!this.SaveAndCompileScript())
				{
					return false;
				}

				_currentobj.Update();

				IsDirty = false;
                UnlockCurrentObject();
                if (ls != null && !ls.CheckForOpenObjects(_currentobj.Code, LockableObjects.FileManagement))
                {
                    ls.LockFileManagementObject(_currentobj.Code);
                    ls.MarkObjectAsOpen(_currentobj.Code, LockableObjects.FileManagement);
                    ls.MarkObjectAsOpen(_currentobj.ScriptName, LockableObjects.Script);
                    ManageVersioningButtons(true);
                }
                else
                {
                    ManageVersioningButtons(true);
                }
				return true;
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
				return false;
			}
		}

		protected override bool CancelData()
		{
			try
			{
				_currentobj.Cancel();
				if (this._currentobj.Script != null)
				{
					this._currentobj.Script.Cancel();
				}

				return true;
			}
			catch (Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
				return false;
			}
		}

		protected override void ShowList()
		{
			Addins.Addin.LoadedFMApps.Remove(_currentobj.Code);
            if (_currentobj != null)
            {
                UnlockCurrentObject();
                _currentobj.Dirty -= OnDirty;
                _currentobj = null;
            }
            _originalcode = _originalscript = null;
			RemoveDesigner();
			base.ShowList();
		}


		protected override void Clone(string Code)
		{
			if (!string.IsNullOrWhiteSpace(Code))
			{
				LoadClone(Code, true, true);
                if (ls != null && _currentobj != null && !string.IsNullOrEmpty(_currentobj.ScriptName))
                {
                    ls.LockScriptObject(_currentobj.ScriptName);
                    ls.MarkObjectAsOpen(_currentobj.ScriptName, LockableObjects.Script);
                }
			}
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
                    ls.MarkObjectAsOpen(_currentobj.ScriptName, LockableObjects.Script);
                    ManageVersioningButtons(true);
                }
            }
            else
            {
                Load(Code, false, false);
                ManageVersioningButtons(true);
            }
		}

		protected override void NewData()
		{
			Load("", true, false);
		}

		private new void Load(string Code, bool NoErrors, bool Clone, bool EnableButtons = false)
		{
			if (IsObjectDirty())
			{
				ShowEditor();

				string code = (Code == "" || Clone) ? Session.CurrentSession.Resources.GetResource("Untitled", "Untitled", null).Text : Code;
				labSelectedObject.Text = string.Format("{0} - {1}", code, fileManagementAppTitle);
                _originalcode = _originalscript = string.Empty;
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
						_currentobj = new FileManagement.FMApplication();
						_currentobj.Fetch(Code);
					}

					propertyGrid1.SelectedObject = _currentobj;
					propertyGrid1.HelpVisible = true;
					BuildDesigner();
					taskflowDesigner1.Configuration = _currentobj;
					_currentobj.Dirty += new EventHandler(OnDirty);
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


		private void LoadClone(string Code, bool NoErrors, bool Clone)
		{
			FMApplication donorApp = FMApplication.GetApplication(Code);
			string newCode = GetCloneApplicationCode(donorApp);
			if (string.IsNullOrWhiteSpace(newCode))
			{
				return;
			}
			else
			{
				cloning = true;
				labSelectedObject.Text = string.Format("{0} - {1}", newCode, fileManagementAppTitle);
                _originalcode = _originalscript = string.Empty;
				ShowEditor();

				Application.DoEvents();

				Cursor = Cursors.WaitCursor;
				try
				{
					_currentobj = FMApplication.Clone(Code);
					_currentobj.Code = newCode;
					_currentobj.Description = donorApp.Description;
					base.IsDirty = true;
					if (!string.IsNullOrWhiteSpace(donorApp.Script.Code))
					{
						GetCloneApplicationScript(donorApp, _currentobj);
					}

					_currentobj.Dirty += new EventHandler(OnDirty);
					propertyGrid1.SelectedObject = _currentobj;
					propertyGrid1.HelpVisible = true;
					BuildDesigner();
					taskflowDesigner1.Configuration = _currentobj;
                    _originalcode = _currentobj.Code;
                    _originalscript = _currentobj.ScriptName;
					Application.DoEvents();
				}
				finally
				{
					Application.DoEvents();
					Cursor = Cursors.Default;
				}
			}
		}


		private string GetCloneApplicationCode(FMApplication donorApp)
		{
			Res resources = Session.CurrentSession.Resources;
			string code = InputBox.Show(
				resources.GetMessage("FMSPECFYNEWCODE", "Please specify a new Code for the new File Management Application.", "").Text,
				resources.GetResource("FMAPPCLONING", "Application Cloning", "").Text, string.Empty, 15);
			if (code == InputBox.CancelText)
				return "";
			else
			{
				if (donorApp.Exists(code))
				{
					System.Windows.Forms.MessageBox.Show(
						resources.GetMessage("FMMSGDUPLICATE", "You have entered a duplicate code.\n\nPlease enter a unique code for the new File Management Application.", "").Text,
						resources.GetResource("FMDUPLICATECODE", "Duplicate Code Entered", "").Text,
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					code = "";
				}
				return code;
			}
		}


		private void GetCloneApplicationScript(FMApplication donorApp, FMApplication _currentobj)
		{
			FWBS.OMS.UI.Windows.MessageBox show = new FWBS.OMS.UI.Windows.MessageBox(
				Session.CurrentSession.Resources.GetMessage("FMMSGCLNDSCRIPT", "The File Management Application being cloned has a Script.\n\nWhat would you like to do?", ""));
			string new_script = Session.CurrentSession.Resources.GetResource("NEWSCRIPT", "New Script", "").Text;
			string copy_script = Session.CurrentSession.Resources.GetResource("COPYSCRIPT", "Copy Script", "").Text;
			show.Buttons = new string[2] { new_script, copy_script };
			show.Caption = FWBS.OMS.Branding.APPLICATION_NAME;
			show.Icon = MessageBox.MessageBoxIconGear;
			string ret = show.Show();
			if (ret == new_script)
			{
				CreateNewScript(_currentobj);
			}
			else if (ret == copy_script)
			{
				CopyDonorScript(donorApp, _currentobj);
			}
		}


		private void CreateNewScript(FMApplication _currentobj)
		{
			_currentobj.NewScript();
			_currentobj.ScriptName = "";
		}


		private void CopyDonorScript(FMApplication donorApp, FMApplication _currentobj)
		{
			string scriptname = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.Code);
			CopyScript(donorApp, _currentobj, scriptname);
			_currentobj.ScriptName = scriptname;
		}


		public override void DeleteDescriptionCodeLookup()
		{
			CodeLookup.Delete("FMAPPLICATION", _currentobj.Code, DBNull.Value);
		}

		/// <summary>
		/// Creates a copy of a script object when cloning an application
		/// </summary>
		/// <param name="NewName"></param>
		public void CopyScript(FMApplication donorApp, FMApplication _currentobj, string NewName)
		{
			if (ScriptGen.Exists(NewName))
				ScriptGen.Delete(NewName);
			ScriptGen _script = ScriptGen.GetScript(donorApp.Script.Code);
			string oldcode = donorApp.Script.Code;
			_script = donorApp.Script.Clone();
			_script.Code = NewName;
			if (_script.AdvancedScript)
				_script.RenameClass(oldcode, NewName);
			_script.Update();
			_script = ScriptGen.GetScript(NewName);
			_currentobj.ScriptName = NewName;
		}


		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
            if (e.ChangedItem.PropertyDescriptor.ComponentType == typeof(FMApplication))
            {
                if (e.ChangedItem.PropertyDescriptor.Name == "Code" && !_currentobj.Code.Equals(_originalcode, StringComparison.InvariantCultureIgnoreCase))
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
                    ls.MarkObjectAsOpen(newscript, LockableObjects.Script);
                }
            }
            ManageVersioningButtons(false);
            IsDirty = true;
		}

		new private void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            if (e.Button == tbCheckin)
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


		private void OnDirty(object sender, EventArgs e)
		{
			this.IsDirty = true;
		}

		private void taskflowDesigner1_SelectedItemsChanged(object sender, FWBS.OMS.FileManagement.Design.FMSelectedItemEventArgs e)
		{
			if (e.SelectedItems.Length == 0)
				propertyGrid1.SelectedObject = null;
			else
				propertyGrid1.SelectedObjects = e.SelectedItems;
		}

		private bool SaveAndCompileScript()
		{
			if (this.IsScriptDirty())
			{
				this.taskflowDesigner1.codesurface.SaveAndCompile();
			}
			if (this.IsScriptDirty())
			{
				FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FMSCRIPTNOTSAVE", "The current script is not saved. Please save the script and make sure it compiles successfully before saving the task flow.", new string[0]);
				return false;
			}
			return true;
		}

		private bool IsScriptDirty()
		{
			if (this._currentobj == null)
			{
				return false;
			}
			if (!this._currentobj.HasScript)
			{
				return false;
			}
			return this.taskflowDesigner1.codesurface.IsDirty;
		}

		private void BuildDesigner()
		{
			this.taskflowDesigner1 = new Design.FMDesigner();
			this.taskflowDesigner1.Configuration = null;
			this.taskflowDesigner1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.taskflowDesigner1.Location = new System.Drawing.Point(0, 49);
			this.taskflowDesigner1.Name = "taskflowDesigner1";
			this.taskflowDesigner1.Size = new System.Drawing.Size(422, 379);
			this.taskflowDesigner1.TabIndex = 202;
			this.taskflowDesigner1.SelectedItemsChanged += new System.EventHandler<FWBS.OMS.FileManagement.Design.FMSelectedItemEventArgs>(this.taskflowDesigner1_SelectedItemsChanged);
			this.tpEdit.Controls.Add(this.taskflowDesigner1, true);
			this.taskflowDesigner1.BringToFront();
		}

		private void RemoveDesigner()
		{
			this.taskflowDesigner1.codesurface.Unload();
			this.taskflowDesigner1.SelectedItemsChanged -= new System.EventHandler<FWBS.OMS.FileManagement.Design.FMSelectedItemEventArgs>(this.taskflowDesigner1_SelectedItemsChanged);
			this.tpEdit.Controls.Remove(this.taskflowDesigner1);
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
			if (this.IsDirty || (this.IsScriptDirty()))
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

        private void ManageVersioningButtons(bool state)
        {
            this.tbCheckin.Enabled = state;
            this.tbCompare.Enabled = state;
        }

	}
}
