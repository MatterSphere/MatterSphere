using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 28000 Extendeded Data Editor for the Admin Kit
    /// </summary>
    public class ucExtendedData : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		private System.Windows.Forms.Splitter splitter1;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter2;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private FWBS.OMS.UI.Windows.DataGridEx dataGrid1;
		private System.Windows.Forms.LinkLabel lnkCreateFieldCodes;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.LinkLabel lnkRegister;
		private ExtendedDataEditor _currentobj  = null;

		public ucExtendedData()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ucExtendedData(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent, Params)
		{
			if (frmMain.PartnerAccess == false || Session.CurrentSession.IsLicensedFor("SDKALL") == true)
				if (Session.CurrentSession.IsLicensedFor("SDKEXD") == false)
					Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			tpEdit.Text = Session.CurrentSession.Resources.GetResource("EXTDATA","Extended Data","").Text;
			tpList.Text = tpEdit.Text;
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
				return "ADMEXTENDED";
			}
		}


		protected override void LoadSingleItem(string Code)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("EXTDATA", "Extended Data", ""), Code);
			ShowEditor(false);
			_currentobj = new ExtendedDataEditor(Code);
			_currentobj.Exclusions.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
			try
			{
				object output = _currentobj.Run();
                if (output is DataTable)
                    dataGrid1.DataSource = output;
                else
                {
                    FWBS.OMS.Interfaces.IExtraInfo iei = output as FWBS.OMS.Interfaces.IExtraInfo;
                    dataGrid1.DataSource = iei.GetDataTable();
                }
			}
			catch
			{
			}
			lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.ExtData));
			lnkCreateFieldCodes.Enabled = (OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.ExtData));
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override bool UpdateData()
		{
			try
			{
				Session.CurrentSession.ClearCache();
				_currentobj.Update();
				LoadSingleItem(_currentobj.Code);
				return true;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return false;
			}
		}

		protected override void NewData()
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("EXTDATA", "Extended Data", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
			ShowEditor(true);
			try
			{
				_currentobj = new ExtendedDataEditor();
				_currentobj.Exclusions.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
				dataGrid1.DataSource = null;
				lnkRegister.Enabled = false;
			}
			catch {}
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override void Clone(string Code)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("EXTDATA", "Extended Data", ""), string.Empty);
			ShowEditor(true);
			try
			{
				_currentobj = ExtendedDataEditor.Clone(Code);
				_currentobj.Exclusions.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
				lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.ExtData));
			}
			catch
			{
			}
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override void DeleteData(string Code)
		{
			try
			{
				ExtendedData.Delete(Code);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
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



        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.lnkCreateFieldCodes = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.dataGrid1 = new FWBS.OMS.UI.Windows.DataGridEx();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.BresourceLookup1.SetLookup(this.tpList, new FWBS.OMS.UI.Windows.ResourceLookupItem("List", "List", ""));
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.dataGrid1);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.panel2);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel2, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.dataGrid1, 0);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(343, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 334);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.pnlActions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(349, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 334);
            this.panel2.TabIndex = 203;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(200, 266);
            this.propertyGrid1.TabIndex = 201;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 266);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(200, 3);
            this.splitter2.TabIndex = 203;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkRegister);
            this.pnlActions.Controls.Add(this.lnkCreateFieldCodes);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 269);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(200, 65);
            this.pnlActions.TabIndex = 202;
            // 
            // lnkRegister
            // 
            this.lnkRegister.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkRegister.Location = new System.Drawing.Point(3, 4);
            this.BresourceLookup1.SetLookup(this.lnkRegister, new FWBS.OMS.UI.Windows.ResourceLookupItem("RegisterED", "Register Extended Data with OMS Objects", ""));
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(194, 29);
            this.lnkRegister.TabIndex = 1;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "Register Extended Data in OMS Objects";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);
            // 
            // lnkCreateFieldCodes
            // 
            this.lnkCreateFieldCodes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkCreateFieldCodes.Location = new System.Drawing.Point(3, 33);
            this.BresourceLookup1.SetLookup(this.lnkCreateFieldCodes, new FWBS.OMS.UI.Windows.ResourceLookupItem("CFieldCD", "Create Fields Codes for Document Merging", ""));
            this.lnkCreateFieldCodes.Name = "lnkCreateFieldCodes";
            this.lnkCreateFieldCodes.Size = new System.Drawing.Size(194, 29);
            this.lnkCreateFieldCodes.TabIndex = 0;
            this.lnkCreateFieldCodes.TabStop = true;
            this.lnkCreateFieldCodes.Text = "Create Field Codes for Document Merging";
            this.lnkCreateFieldCodes.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCreateFieldCodes_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(0, 0);
            this.BresourceLookup1.SetLookup(this.linkLabel1, new FWBS.OMS.UI.Windows.ResourceLookupItem("RegisterED", "Register Extended Data with OMS Objects", ""));
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(100, 23);
            this.linkLabel1.TabIndex = 0;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AlternatingBackColor = System.Drawing.Color.White;
            this.dataGrid1.BackColor = System.Drawing.Color.White;
            this.dataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid1.CaptionVisible = false;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Silver;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 49);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(343, 334);
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.TabIndex = 204;
            // 
            // ucExtendedData
            // 
            this.Name = "ucExtendedData";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("EXTDATA", "Extended Data", ""), _currentobj.Code);
			IsDirty=true;
		}

		private void ClearedColumns()
		{
			IsDirty = true;
		}

		private void lnkRegister_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			DataTable dt = null;	
			FWBS.Common.KeyValueCollection _param = new FWBS.Common.KeyValueCollection();
			_param.Add("objCode",_currentobj.Code);
			_param.Add("objHelp","[ExtData]");
			dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.RegisterOMSObject),null,EnquiryMode.Add,false, _param) as DataTable;
			
			if (dt != null)
			{
				string _code = Convert.ToString(dt.Rows[0]["objCode"]);
				string _description = Convert.ToString(dt.Rows[0]["objdescription"]);
				string _type = Convert.ToString(dt.Rows[0]["objType"]);
				string _web = Convert.ToBoolean(dt.Rows[0]["chkWEB"]) == true ? _currentobj.Code : "";
				string _win = Convert.ToBoolean(dt.Rows[0]["chkWindows"]) == true ? _currentobj.Code : "";
				string _pda = Convert.ToBoolean(dt.Rows[0]["chkPDA"]) == true ? _currentobj.Code : "";
				try
				{
					OmsObject.Register(_code,OMSObjectTypes.ExtData,_type,_description,Convert.ToString(dt.Rows[0]["objHelp"]),_win,_web,_pda);
					lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.ExtData));
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}				
			}
		}

		private void lnkCreateFieldCodes_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				_currentobj.BuidDBFieldsRows();
				MessageBox.ShowInformation("FLDCODES","Fields created successfully");
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}				
		}
	}
}

