using System;
using System.Windows.Forms;
using FWBS.OMS.Design.CodeBuilder;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows.Design;


namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Reports Editor for the Admin Kit
    /// </summary>
    public class ucReports : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Panel panel2;
		private FWBS.OMS.UI.Windows.Reports.ucReportsView ucReportsView1;
		private System.Windows.Forms.ToolBarButton tbEditReport;
		private System.Windows.Forms.ToolBarButton tbSp9;
		private System.Windows.Forms.ToolBarButton tpSp10;
		private System.Windows.Forms.ToolBarButton tbScript;
		private ReportListEditor _currentobj;
		private System.Windows.Forms.ToolBarButton tbReload;
		private CodeWindow _codewindow;
		private System.Windows.Forms.ToolBarButton tbEditForm;
		private string _group;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.Windows.Forms.SaveFileDialog dlgExportPkg;
		private long _enqversion = -1;
		private string _parenttitle = "";

		public ucReports()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ucReports(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Group) : base(mainparent,editparent,Group)
		{
			if (frmMain.PartnerAccess == false)
				if (Session.CurrentSession.IsLicensedFor("SDKRPT") == false)
					Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			_parenttitle = editparent.Text;
			tpEdit.Text = Session.CurrentSession.Resources.GetResource("REPORTLSTED","Report Editor","").Text;
			tpList.Text = tpEdit.Text;
			_group = Convert.ToString(Group["Group"].Value);
			if (Session.CurrentSession.IsLicensedFor("SDKALL") == false && frmMain.PartnerAccess == false)
				if (Session.CurrentSession.IsLicensedFor("SDKRPT") == true)
				{
					tbScript.Visible = false;

				}
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
			}
			base.Dispose( disposing );
		}

		protected override string SearchListName
		{
			get
			{
				return "ADMREPORTS";
			}
		}
	
		
		protected override bool UpdateData()
		{
			try
			{
				_currentobj.Update();
				this.IsDirty=false;
				LoadReportList(_currentobj.Code,true,false);
				return true;
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return false;
			}
		}

		protected override void Clone(string Code)
		{
			LoadReportList(Code,true,true);
			tbEditReport.Enabled=true;
		}
		
		protected override void LoadSingleItem(string Code)
		{
			LoadReportList(Code,false,false);
			CodeWindow.Text = Session.CurrentSession.Resources.GetResource("RPTCODEBLD", "Report Code Builder", "").Text;
			CodeWindow.Load(_currentobj.ReportObject.Script.ScriptType,_currentobj.ReportObject.Script);
			tbEditReport.Enabled=true;
		}

		protected override void NewData()
		{
			LoadReportList("",true,false);
			tbEditReport.Enabled=false;
		}

		public CodeWindow CodeWindow
		{
			get
			{
				if (_codewindow == null)
				{
					_codewindow = new CodeWindow();
                    _codewindow.Init(null);
                }
				return _codewindow;
			}
		}


		protected override void DeleteData(string Code)
		{
			if (FWBS.OMS.SearchEngine.SearchList.Delete(Code) == false)
			{
				ErrorBox.Show(ParentForm, new OMSException2("27001","Failed to Delete with code : %1%",new Exception(),true,Code));
			}
		}
		
		private void LoadReportList(string Code, bool NoErrors, bool Clone)
		{
			if (IsObjectDirty())
			{
                var editParentForm = this.EditParent as frmAdminDesktop;
                if (editParentForm != null)
                {
                    editParentForm.Activated -= new EventHandler(ParentForm_Activated);
                    editParentForm.Activated += new EventHandler(ParentForm_Activated);
                }
				ucReportsView1.Reset();
				
                if (Code == "" || Clone)
                {
                    labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Untitled", "Untitled", ""), ResourceLookup.GetLookupText("ReportLists", "Report Lists", ""));
                    tbEditReport.Enabled = false;
                    ShowEditor(true);
                }
                else
                {
                    labSelectedObject.Text = string.Format("{0} - {1}", Code, ResourceLookup.GetLookupText("ReportLists", "Report Lists", ""));
                    ShowEditor(false);
                }
				Application.DoEvents();
				Cursor = Cursors.WaitCursor;
				try
				{
						if (Code == "" && Clone == false)
						_currentobj  = new ReportListEditor();
					else if (Code != "" && Clone)
						_currentobj = ReportListEditor.Clone(Code);
					else
						_currentobj  = new ReportListEditor(Code);


					propertyGrid1.SelectedObject = _currentobj;
					propertyGrid1.HelpVisible=true;
					Application.DoEvents();
					ucReportsView1.Report = _currentobj.ReportObject;
					ucReportsView1.Report.ScriptChanged +=new EventHandler(Report_ScriptChanged);
					if (_currentobj.EnquiryForm != "")
					{
						tbEditForm.Visible = true;
					}
					else
					{
						tbEditForm.Visible = false;
					}


				}
				finally
				{
					Application.DoEvents();
					Cursor = Cursors.Default;
				}
			}
		}

		private void ClearedColumns()
		{
			IsDirty = true;
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.panel2 = new System.Windows.Forms.Panel();
			this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
			this.ucReportsView1 = new FWBS.OMS.UI.Windows.Reports.ucReportsView();
			this.tbEditReport = new System.Windows.Forms.ToolBarButton();
			this.tbSp9 = new System.Windows.Forms.ToolBarButton();
			this.tpSp10 = new System.Windows.Forms.ToolBarButton();
			this.tbScript = new System.Windows.Forms.ToolBarButton();
			this.tbReload = new System.Windows.Forms.ToolBarButton();
			this.tbEditForm = new System.Windows.Forms.ToolBarButton();
			this.dlgExportPkg = new System.Windows.Forms.SaveFileDialog();
			this.tpEdit.SuspendLayout();
			this.panel2.SuspendLayout();
			// 
			// tpList
			// 
			this.tpList.Name = "tpList";
			// 
			// tpEdit
			// 
			this.tpEdit.Controls.Add(this.ucReportsView1);
			this.tpEdit.Controls.Add(this.splitter1);
			this.tpEdit.Controls.Add(this.panel2);
			this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
			this.tpEdit.Name = "tpEdit";
			this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
			this.tpEdit.Controls.SetChildIndex(this.panel2, 0);
			this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
			this.tpEdit.Controls.SetChildIndex(this.ucReportsView1, 0);
			// 
			// pnlEdit
			// 
			this.pnlEdit.Name = "pnlEdit";
			this.pnlEdit.Size = new System.Drawing.Size(549, 51);
			// 
			// labSelectedObject
			// 
			this.labSelectedObject.Name = "labSelectedObject";
			// 
			// tbcEdit
			// 
			this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																					   this.tbSp9,
																					   this.tbEditReport,
																					   this.tpSp10,
																					   this.tbScript,
																					   this.tbReload,
																					   this.tbEditForm});
			this.tbcEdit.Name = "tbcEdit";
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
			this.lstList.DockPadding.All = 5;
			this.lstList.Name = "lstList";
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitter1.Location = new System.Drawing.Point(344, 77);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(5, 306);
			this.splitter1.TabIndex = 200;
			this.splitter1.TabStop = false;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.HelpVisible = false;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(200, 283);
			this.propertyGrid1.TabIndex = 201;
			this.propertyGrid1.Text = "6";
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.propertyGrid1);
			this.panel2.Controls.Add(this.pnlActions);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(349, 77);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(200, 306);
			this.panel2.TabIndex = 202;
			// 
			// pnlActions
			// 
			this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
			this.pnlActions.BorderLine = true;
			this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlActions.DockPadding.All = 3;
			this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
			this.pnlActions.Location = new System.Drawing.Point(0, 283);
			this.pnlActions.Name = "pnlActions";
			this.pnlActions.Size = new System.Drawing.Size(200, 23);
			this.pnlActions.TabIndex = 203;
			// 
			// ucReportsView1
			// 
			this.ucReportsView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucReportsView1.Location = new System.Drawing.Point(0, 77);
			this.ucReportsView1.Name = "ucReportsView1";
			this.ucReportsView1.Report = null;
			this.ucReportsView1.Size = new System.Drawing.Size(344, 306);
			this.ucReportsView1.TabIndex = 203;
			// 
			// tbEditReport
			// 
			this.tbEditReport.Enabled = false;
			this.tbEditReport.ImageIndex = 24;
			this.BresourceLookup1.SetLookup(this.tbEditReport, new FWBS.OMS.UI.Windows.ResourceLookupItem("EDITREPORT", "Edit Report", ""));
			// 
			// tbSp9
			// 
			this.tbSp9.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tpSp10
			// 
			this.tpSp10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tbScript
			// 
			this.tbScript.ImageIndex = 25;
			this.BresourceLookup1.SetLookup(this.tbScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("Script", "Edit Script", ""));
			// 
			// tbReload
			// 
			this.tbReload.ImageIndex = 22;
			this.BresourceLookup1.SetLookup(this.tbReload, new FWBS.OMS.UI.Windows.ResourceLookupItem("Reload", "Reload", ""));
			// 
			// tbEditForm
			// 
			this.tbEditForm.ImageIndex = 24;
			this.BresourceLookup1.SetLookup(this.tbEditForm, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditForm", "Edit Form", ""));
			this.tbEditForm.Visible = false;
			// 
			// dlgExportPkg
			// 
			this.dlgExportPkg.DefaultExt = "manifest";
			// 
			// ucReports
			// 
			this.DockPadding.All = 8;
			this.Name = "ucReports";
			this.tpEdit.ResumeLayout(false);
			this.panel2.ResumeLayout(false);

		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			tbEditReport.Enabled = false;
			if (_currentobj.Code == "")
			{
				labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Untitled", "Untitled", ""), ResourceLookup.GetLookupText("ReportLists", "Report Lists", ""));
			}
			else
			{
				labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("ReportLists", "Report Lists", ""));
			}
			if (_currentobj.Code != "" && _currentobj.DataBuilder.Call != "")
				tbEditReport.Enabled = true;
			IsDirty=true;
		}

		new private void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbEditReport)
			{
				if (_currentobj.Code == "")
				{
					ErrorBox.Show(ParentForm, new OMSException2("50001","You must set the Code before editing the Report"));
					return;
				}
				try
				{
					_currentobj.EditReport();
					IsDirty=true;
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}
			else if (e.Button == tbScript)
			{
				if (_currentobj.ScriptName == "")
				{
					if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ADDSCRIPT","Are you sure you wish to Add Scripting to this Report?",""),"OMS Administration",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
					{
					    _currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.Code);
					}
					else
					{
						return;
					}
				}
				CodeWindow.Show();
				CodeWindow.BringToFront();
			}
			else if (e.Button == tbReload)
			{
				LoadReportList(_currentobj.Code,true,false);
			}
			else if (e.Button == tbEditForm)
			{
				FWBS.OMS.Design.frmDesigner frmDesigner = MainParent.Action("ED","") as FWBS.OMS.Design.frmDesigner;
				frmDesigner.frmDesigner_Initialize(true,_currentobj.EnquiryForm,"","");
				_enqversion = frmDesigner.EnquiryFormVersion;
			}
		}

		private void Report_ScriptChanged(object sender, EventArgs e)
		{
			this.IsDirty=true;
            if (_currentobj.Script.State != ObjectState.Added)
                _currentobj.NewScript(); 
			CodeWindow.Load(_currentobj.ReportObject.Script.ScriptType,_currentobj.ReportObject.Script);
		}

		private void OMSToolbars_ParentChanged(object sender, System.EventArgs e)
		{
		}

		private void ParentForm_Activated(object sender, EventArgs e)
		{
			if (_currentobj.EnquiryForm != "")
			{
				long _enqnewversion = Enquiry.GetEnquiryFormVersion(_currentobj.EnquiryForm);
				if (_enqversion != -1 && _enqnewversion > _enqversion)
				{
					if (MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("ENQFCHANGED","Changes have been detected for the Enquiry Form. Do you wish to Update?",""), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
					{
						LoadReportList(_currentobj.Code,false,false);
						_enqversion = _enqnewversion;
					}
				}
			}
		}

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    base.ShowList();
                }
            }
            else
            {
                base.ShowList();
            }
        }

	}
}

