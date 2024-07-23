using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Data;
using FWBS.OMS.UI.Windows.Design;


namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 29000 Data List Editor for the Admin Kit
    /// </summary>
    public class ucDataLists : FWBS.OMS.UI.Windows.Admin.ucEditBase2, IObjectUnlock
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.DataGridTextBoxColumn dgcType;
        private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox grpDisplay;
		private System.Windows.Forms.Label labDisplay;
		private System.Windows.Forms.GroupBox grpValue;
		private System.Windows.Forms.Label labValue;
		private FWBS.Common.UI.Windows.eComboBoxList2 cmbComboList;
		private System.Windows.Forms.GroupBox grpTestResult;
		private System.Windows.Forms.Label labTestResult;
		private FWBS.OMS.UI.Windows.DataGridEx dataGrid1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labComboExample;

        private System.Windows.Forms.ToolBarButton tbSpVersioning;
        private System.Windows.Forms.ToolBarButton tbCheckin;
        private System.Windows.Forms.ToolBarButton tbCompare;

        private PropertyGrid propertyGrid1;
		private FWBS.OMS.UI.Windows.Admin.DataListEditor _currentobj;

        private LockState ls = new LockState();
        private IMainParent _mainparent;
        private bool objectlocking;
        bool fromSave = false;

        VersionControlSupport vsupport;
        DataListVersionDataArchiver datalistVersionDataArchiver;


		public ucDataLists()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            vsupport = new VersionControlSupport();
		}

		public ucDataLists(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection parent) : base(mainparent,editparent,parent)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
            ManageVersioningButtons(false);

            objectlocking = Session.CurrentSession.ObjectLocking;
            _mainparent = mainparent;
            vsupport = new VersionControlSupport();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

		protected override string SearchListName
		{
			get
			{
				return "ADMDATALISTS";
			}
		}
	
		
		protected override void Clone(string Code)
		{
			propertyGrid1.HelpVisible=true;
			_currentobj = FWBS.OMS.UI.Windows.Admin.DataListEditor.Clone(Code);
			propertyGrid1.SelectedObject = _currentobj;
			ComboBox cb = (ComboBox)cmbComboList.Controls[0];
			labSelectedObject.Text = string.Format("{0} - {1}", string.Empty, ResourceLookup.GetLookupText("SelectedObject", "Selected Object", ""));
			try
			{
				labValue.Text = labDisplay.Text = labTestResult.Text = ResourceLookup.GetLookupText("VALNOTSET", "(Not Set)", "");
				cb.DataSource = null;
				cb.Items.Clear();
				cb.ValueMember = "";
				cb.DisplayMember="";
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			ShowEditor(true);
			cb.SelectedIndex=-1;	
		}


		protected override bool UpdateData()
		{
			try
			{
                if (String.IsNullOrEmpty(_currentobj.Description))
                    throw new OMSException2("ERRNODESC", "The Description is required");
                _currentobj.Update();
                fromSave = true;

                if (objectlocking)
                {
                    if (!ls.CheckForOpenObjects(_currentobj.Code, LockableObjects.DataList))
                    {
                        ls.LockDataListObject(_currentobj.Code);
                        ls.MarkObjectAsOpen(_currentobj.Code, LockableObjects.DataList);
                    }
                }
				LoadSingleItem(_currentobj.Code);
				labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("SelectedObject", "Selected Object", ""));
                propertyGrid1.Refresh();
                this.IsDirty=false;
                ManageVersioningButtons(true);
				return true;
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return false;
			}
		}


		protected override void LoadSingleItem(string Code)
		{
			propertyGrid1.HelpVisible=true;
			try
			{
                if (fromSave)
                {
                    _currentobj = new FWBS.OMS.UI.Windows.Admin.DataListEditor(Code);
                    fromSave = false;
                    SetupDataList(_currentobj, Code);
                }
                else
                    CheckObjectLockingBeforeLoad(Code);
 			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
        }


        private void CheckObjectLockingBeforeLoad(string Code)
        {
            if (objectlocking)
            {
                if (!ls.CheckObjectLockState(Code, LockableObjects.DataList))
                {
                    if (!ls.CheckIfObjectIsAlreadyOpen(Code, LockableObjects.DataList))
                    {
                        _currentobj = new FWBS.OMS.UI.Windows.Admin.DataListEditor(Code);
                        ls.LockDataListObject(Code);
                        ls.MarkObjectAsOpen(Code, LockableObjects.DataList);
                        SetupDataList(_currentobj, Code);
                    }
                }
            }
            else
            {
                _currentobj = new FWBS.OMS.UI.Windows.Admin.DataListEditor(Code);
                SetupDataList(_currentobj, Code);
            }
        }


        private void SetupDataList(DataListEditor _currentobj, string Code)
        {
            if(_currentobj != null && !string.IsNullOrWhiteSpace(Code))
                ManageVersioningButtons(true);
            else
                ManageVersioningButtons(false);

			propertyGrid1.SelectedObject = _currentobj;
			ComboBox cb = (ComboBox)cmbComboList.Controls[0];
			labSelectedObject.Text = string.Format("{0} - {1}",Code, ResourceLookup.GetLookupText("SelectedObject", "Selected Object", ""));
			try
			{
                labValue.Text = labDisplay.Text = labTestResult.Text = ResourceLookup.GetLookupText("VALNOTSET", "(Not Set)", "");
				cb.DataSource = null;
				cb.Items.Clear();
				cb.ValueMember = "";
				cb.DisplayMember="";

				FWBS.Common.KeyValueCollection k = new FWBS.Common.KeyValueCollection();
				foreach(Parameter p in  _currentobj.DataBuilder.Parameters)
				{
                    if (p.TestValue == "(NULL)")
                        k.Add(p.BoundValue.Replace("%", ""), null);
                    else if (p.TestValue == "(DBNULL)")
						k.Add(p.BoundValue.Replace("%",""),DBNull.Value);
					else
						k.Add(p.BoundValue.Replace("%",""),p.TestValue);
				}
				_currentobj.ChangeParameters(k);
                if (_currentobj.DataBuilder.SourceType != SourceEngine.SourceType.Instance)
                {
                    DataTable dt = (DataTable)_currentobj.Run();
                    labValue.Text = dt.Columns[0].Caption;
                    labDisplay.Text = dt.Columns[1].Caption;
                    cmbComboList.AddItem(dt);
                    dataGrid1.DataSource = dt;
                }
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			ShowEditor(false);
			cb.SelectedIndex=-1;
            
		}


        private new void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == tbCheckin)
            {
                CreateAndArchiveData(true);
            }
            else if (e.Button == tbCompare)
            {
                tbCompare_Click();
            }
        }

        private void CreateAndArchiveData(bool CheckInOnly)
        {
            // To go wherever the ‘Save and Version’ button/menu item will be located
            var datalistVersionData = new DataSet();
            datalistVersionData.Tables.Add(BuildVersionDataTable());
            datalistVersionDataArchiver = new DataListVersionDataArchiver();
            if(!CheckInOnly)
                datalistVersionDataArchiver.Saved += new EventHandler(datalistVersionDataArchiver_Saved);
            datalistVersionDataArchiver.SaveData(datalistVersionData, _currentobj.Code, _currentobj.Version, versionID: Guid.NewGuid());
        }

        private void datalistVersionDataArchiver_Saved(object sender, EventArgs e)
        {
            datalistVersionDataArchiver.Saved -= new EventHandler(datalistVersionDataArchiver_Saved);
            vsupport.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vsupport_RestorationCompleted); 
            vsupport.OpenComparisonTool(_currentobj.Code, UI.Windows.LockableObjects.DataList);
        }

        private DataTable BuildVersionDataTable()
        {
            string sql = @"select * from dbEnquiryDataList where enqTable = @code";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", _currentobj.Code));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            dt.TableName = "dbEnquiryDataList";
            return dt;
        }

        private void tbCompare_Click()
        {
            if (vsupport.CheckObjectInIfNecessary("select * from dbDataListVersionData where Code = @code and Version = @version", _currentobj.Code, _currentobj.Version))
                CreateAndArchiveData(false);
            else
            {
                vsupport.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vsupport_RestorationCompleted); 
                vsupport.OpenComparisonTool(_currentobj.Code, UI.Windows.LockableObjects.DataList);
            }
        }

        private void vsupport_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            _currentobj = new FWBS.OMS.UI.Windows.Admin.DataListEditor(_currentobj.Code);
            SetupDataList(_currentobj, _currentobj.Code);
            propertyGrid1.Refresh();
        }

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    UnlockCurrentObject();
                    base.ShowList();
                }
            }
            else
            {
                UnlockCurrentObject();
                base.ShowList();
            }
        }

        public void UnlockCurrentObject()
        {
            if (_currentobj != null && !string.IsNullOrWhiteSpace(_currentobj.Code))
            {
                if (objectlocking)
                {
                    ls.MarkObjectAsClosed(_currentobj.Code, LockableObjects.DataList);
                    ls.UnlockDataListObject(_currentobj.Code);
                }
            }
        }

		protected override void NewData()
		{
			propertyGrid1.HelpVisible=true;
			_currentobj = new FWBS.OMS.UI.Windows.Admin.DataListEditor();
			propertyGrid1.SelectedObject = _currentobj;
			ComboBox cb = (ComboBox)cmbComboList.Controls[0];
			try
			{
				labValue.Text = "(Not Set)";
				labDisplay.Text = "(Not Set)";
				labTestResult.Text = "(Not Set)";
				cb.DataSource = null;
				cb.Items.Clear();
				cb.ValueMember = "";
				cb.DisplayMember="";
				dataGrid1.DataSource = null;
                ManageVersioningButtons(false);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			ShowEditor(true);
			cb.SelectedIndex = -1;
		}

		protected override void DeleteData(string Code)
		{
			try
			{
				if (DataListEditor.Delete(Code) == false)
				{
					ErrorBox.Show(ParentForm, new OMSException2("29001", "Failed to Delete with Code: %1%",new Exception(),true,Code));
				}
			}
			catch(Exception ex)
			{
				//enhanced to give end users a clue to what objects are usibg this object
				DataTable tbl = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiryFormsByDataList(Code);
				if(tbl.Rows.Count > 0)
				{
					string tablelist = "";

					foreach(DataRow row in tbl.Rows)
					{
						tablelist +="'" + Convert.ToString(row[0]) + "'";
						tablelist += Environment.NewLine;
					}
					string message = Environment.NewLine + "The following screens are using this data list" + Environment.NewLine + tablelist + Environment.NewLine;

					ErrorBox.Show(ParentForm, new OMSException2("29001A", "Failed to delete data list with code: %1% %2%" ,ex,true,new string[2]{Code,message}));
				}
				else
				{
					ErrorBox.Show(ParentForm, new OMSException2("29001", "Failed to Delete with Code: %1%",ex,true,Code));
				}

			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            UnlockCurrentObject(); 
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
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
            this.dgcType = new System.Windows.Forms.DataGridTextBoxColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGrid1 = new FWBS.OMS.UI.Windows.DataGridEx();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpTestResult = new System.Windows.Forms.GroupBox();
            this.labTestResult = new System.Windows.Forms.Label();
            this.grpDisplay = new System.Windows.Forms.GroupBox();
            this.labDisplay = new System.Windows.Forms.Label();
            this.cmbComboList = new FWBS.Common.UI.Windows.eComboBoxList2();
            this.grpValue = new System.Windows.Forms.GroupBox();
            this.labValue = new System.Windows.Forms.Label();
            this.labComboExample = new System.Windows.Forms.Label();
            this.tbSpVersioning = new System.Windows.Forms.ToolBarButton();
            this.tbCheckin = new System.Windows.Forms.ToolBarButton();
            this.tbCompare = new System.Windows.Forms.ToolBarButton();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpTestResult.SuspendLayout();
            this.grpDisplay.SuspendLayout();
            this.grpValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.BresourceLookup1.SetLookup(this.tpList, new FWBS.OMS.UI.Windows.ResourceLookupItem("DataLists", "Data Lists", ""));
            this.tpList.Text = "Data Lists";
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.panel2);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.propertyGrid1);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.propertyGrid1, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel2, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.BackColor = System.Drawing.Color.White;
            this.pnlEdit.Size = new System.Drawing.Size(549, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSpVersioning,
            this.tbCheckin,
            this.tbCompare});
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // dgcType
            // 
            this.dgcType.Format = "";
            this.dgcType.FormatInfo = null;
            this.dgcType.Width = -1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(343, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 333);
            this.splitter1.TabIndex = 202;
            this.splitter1.TabStop = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(349, 50);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(200, 333);
            this.propertyGrid1.TabIndex = 203;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.dataGrid1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.labComboExample);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 333);
            this.panel2.TabIndex = 204;
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackColor = System.Drawing.Color.White;
            this.dataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid1.CaptionText = "Data Grid Example Output";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Silver;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 164);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(343, 169);
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpTestResult);
            this.panel1.Controls.Add(this.grpDisplay);
            this.panel1.Controls.Add(this.cmbComboList);
            this.panel1.Controls.Add(this.grpValue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 146);
            this.panel1.TabIndex = 5;
            // 
            // grpTestResult
            // 
            this.grpTestResult.Controls.Add(this.labTestResult);
            this.grpTestResult.Location = new System.Drawing.Point(160, 40);
            this.BresourceLookup1.SetLookup(this.grpTestResult, new FWBS.OMS.UI.Windows.ResourceLookupItem("TestReturnValue", "Test Return Value", ""));
            this.grpTestResult.Name = "grpTestResult";
            this.grpTestResult.Size = new System.Drawing.Size(138, 44);
            this.grpTestResult.TabIndex = 3;
            this.grpTestResult.TabStop = false;
            this.grpTestResult.Text = "Test Return Value";
            // 
            // labTestResult
            // 
            this.labTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labTestResult.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTestResult.Location = new System.Drawing.Point(3, 19);
            this.labTestResult.Name = "labTestResult";
            this.labTestResult.Size = new System.Drawing.Size(132, 22);
            this.labTestResult.TabIndex = 0;
            this.labTestResult.Text = "(Not Set)";
            this.labTestResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpDisplay
            // 
            this.grpDisplay.Controls.Add(this.labDisplay);
            this.grpDisplay.Location = new System.Drawing.Point(10, 40);
            this.BresourceLookup1.SetLookup(this.grpDisplay, new FWBS.OMS.UI.Windows.ResourceLookupItem("DisplayMember", "Display Member", ""));
            this.grpDisplay.Name = "grpDisplay";
            this.grpDisplay.Size = new System.Drawing.Size(138, 44);
            this.grpDisplay.TabIndex = 1;
            this.grpDisplay.TabStop = false;
            this.grpDisplay.Text = "Display Member";
            // 
            // labDisplay
            // 
            this.labDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDisplay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDisplay.Location = new System.Drawing.Point(3, 19);
            this.labDisplay.Name = "labDisplay";
            this.labDisplay.Size = new System.Drawing.Size(132, 22);
            this.labDisplay.TabIndex = 0;
            this.labDisplay.Text = "(Not Set)";
            this.labDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbComboList
            // 
            this.cmbComboList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbComboList.CaptionWidth = 80;
            this.cmbComboList.IsDirty = false;
            this.cmbComboList.Location = new System.Drawing.Point(10, 8);
            this.cmbComboList.MaxLength = 0;
            this.cmbComboList.Name = "cmbComboList";
            this.cmbComboList.Size = new System.Drawing.Size(326, 23);
            this.cmbComboList.TabIndex = 0;
            this.cmbComboList.Text = "The Caption";
            this.cmbComboList.Value = "";
            this.cmbComboList.Changed += new System.EventHandler(this.cmbComboList_Changed);
            // 
            // grpValue
            // 
            this.grpValue.Controls.Add(this.labValue);
            this.grpValue.Location = new System.Drawing.Point(10, 88);
            this.BresourceLookup1.SetLookup(this.grpValue, new FWBS.OMS.UI.Windows.ResourceLookupItem("ValueMember", "Value Member", ""));
            this.grpValue.Name = "grpValue";
            this.grpValue.Size = new System.Drawing.Size(138, 44);
            this.grpValue.TabIndex = 2;
            this.grpValue.TabStop = false;
            this.grpValue.Text = "Value Member";
            // 
            // labValue
            // 
            this.labValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labValue.Location = new System.Drawing.Point(3, 19);
            this.labValue.Name = "labValue";
            this.labValue.Size = new System.Drawing.Size(132, 22);
            this.labValue.TabIndex = 0;
            this.labValue.Text = "(Not Set)";
            this.labValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labComboExample
            // 
            this.labComboExample.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labComboExample.Dock = System.Windows.Forms.DockStyle.Top;
            this.labComboExample.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labComboExample.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labComboExample.Location = new System.Drawing.Point(0, 0);
            this.labComboExample.Name = "labComboExample";
            this.labComboExample.Size = new System.Drawing.Size(343, 18);
            this.labComboExample.TabIndex = 206;
            this.labComboExample.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSpVersioning
            // 
            this.tbSpVersioning.Name = "tbSpVersioning";
            this.tbSpVersioning.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbCheckin
            // 
            this.tbCheckin.ImageIndex = 25;
            this.BresourceLookup1.SetLookup(this.tbCheckin, new FWBS.OMS.UI.Windows.ResourceLookupItem("PrecCheckin", "Check in", ""));
            this.tbCheckin.Name = "tbCheckin";
            // 
            // tbCompare
            // 
            this.tbCompare.ImageIndex = 25;
            this.BresourceLookup1.SetLookup(this.tbCompare, new FWBS.OMS.UI.Windows.ResourceLookupItem("PrecCompare", "Version Administration", ""));
            this.tbCompare.Name = "tbCompare";
            // 
            // ucDataLists
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucDataLists";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.grpTestResult.ResumeLayout(false);
            this.grpDisplay.ResumeLayout(false);
            this.grpValue.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			ComboBox cb = (ComboBox)cmbComboList.Controls[0];
			cb.DataSource = null;
			cb.Items.Clear();
			cb.ValueMember = "";
			cb.DisplayMember = "";
			try
			{
				FWBS.Common.KeyValueCollection k = new FWBS.Common.KeyValueCollection();
				foreach(Parameter p in  _currentobj.DataBuilder.Parameters)
					k.Add(p.BoundValue,p.TestValue);
				_currentobj.ChangeParameters(k);
				DataTable dt = (DataTable)_currentobj.Run();
				labValue.Text = dt.Columns[0].Caption;
				labDisplay.Text = dt.Columns[1].Caption;
				cmbComboList.AddItem(dt);
			}
			catch{}
			IsDirty=true;
		}

		private void cmbComboList_Changed(object sender, System.EventArgs e)
		{
			labTestResult.Text = Convert.ToString(cmbComboList.Value);
		}

        private void ManageVersioningButtons(bool state)
        {
            this.tbCheckin.Enabled = state;
            this.tbCompare.Enabled = state;
        }

	}
}

