using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 29000 Data List Editor for the Admin Kit
    /// </summary>
    public class ucPackageData : FWBS.OMS.UI.Windows.Admin.ucEditBase2, IOBjectDirty
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.DataGridTextBoxColumn dgcType;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Panel panel2;
		private FWBS.OMS.UI.Windows.DataGridEx dataGrid1;
		private System.Windows.Forms.Label labComboExample;
		private FWBS.OMS.UI.Windows.Admin.PackageData _currentobj;


		public ucPackageData()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ucPackageData(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection parent) : base(mainparent,editparent,parent)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
				return "ADMDATAPACKAGES";
			}
		}
	
		
		protected override void Clone(string Code)
		{
			propertyGrid1.HelpVisible=true;
			_currentobj = FWBS.OMS.UI.Windows.Admin.PackageData.Clone(Code);
			propertyGrid1.SelectedObject = _currentobj;
			labSelectedObject.Text = string.Format("{0} - {1}", string.Empty, ResourceLookup.GetLookupText("DataPackages", "Data Packages", ""));
			ShowEditor(true);
		}
		
		protected override bool UpdateData()
		{
			try
			{
				_currentobj.Update();
				LoadSingleItem(_currentobj.Code);
				labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("DataPackages", "Data Packages", ""));
				this.IsDirty=false;
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
				_currentobj = new FWBS.OMS.UI.Windows.Admin.PackageData(Code);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			propertyGrid1.SelectedObject = _currentobj;
			labSelectedObject.Text = string.Format("{0} - {1}", Code, ResourceLookup.GetLookupText("SelectedObject", "Selected Object", ""));
			try
			{
				FWBS.Common.KeyValueCollection k = new FWBS.Common.KeyValueCollection();
				foreach(Parameter p in  _currentobj.DataBuilder.Parameters)
					k.Add(p.BoundValue.Replace("%",""),p.TestValue);
				_currentobj.ChangeParameters(k);

				DataTable dt = (DataTable)_currentobj.Run();
				dataGrid1.DataSource = dt;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			ShowEditor(false);
		}

		protected override void NewData()
		{
			propertyGrid1.HelpVisible=true;
			_currentobj = new FWBS.OMS.UI.Windows.Admin.PackageData();
			propertyGrid1.SelectedObject = _currentobj;
			try
			{
				dataGrid1.DataSource = null;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			ShowEditor(true);
		}

		protected override void DeleteData(string Code)
		{
			try
			{
				if (PackageData.Delete(Code) == false)
				{
					ErrorBox.Show(ParentForm, new OMSException2("29001", "Failed to Delete with Code: %1%",new Exception(),true,Code));
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, new OMSException2("29001", "Failed to Delete with Code: %1%",ex,true,Code));
			}
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
            this.labComboExample = new System.Windows.Forms.Label();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
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
            this.pnlEdit.Size = new System.Drawing.Size(549, 50);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
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
            this.panel2.Controls.Add(this.dataGrid1);
            this.panel2.Controls.Add(this.labComboExample);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 333);
            this.panel2.TabIndex = 204;
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid1.CaptionText = "Data to be Packaged";
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 18);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(343, 315);
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.TabIndex = 4;
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
            // ucPackageData
            // 
            this.Name = "ucPackageData";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlEdit.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			try
			{
				FWBS.Common.KeyValueCollection k = new FWBS.Common.KeyValueCollection();
				foreach(Parameter p in  _currentobj.DataBuilder.Parameters)
					k.Add(p.BoundValue,p.TestValue);
				_currentobj.ChangeParameters(k);
				DataTable dt = (DataTable)_currentobj.Run();
			}
			catch{}
			IsDirty=true;
		}

        #region IObjectDirty Implementation

        private bool _isdirty;
        bool IOBjectDirty.IsDirty
        {
            get { return _isdirty; }
        }

        bool IOBjectDirty.IsObjectDirty()
        {
            return true;
        }

        #endregion

        #region CloseAndReturnToList Override

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
                ShowList();
            }
        }

        #endregion
    }
}

