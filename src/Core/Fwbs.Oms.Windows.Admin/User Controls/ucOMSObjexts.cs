using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public class ucOMSObjexts : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.ComponentModel.IContainer components = null;
		private OmsObject _currentobj  = null;

		public ucOMSObjexts()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public ucOMSObjexts(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent,Params)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			tpEdit.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource("OMSOBJECTS","OMS Objects","").Text;
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

		protected override void LoadSingleItem(string Code)
		{
			labSelectedObject.Text = Session.CurrentSession.Resources.GetResource("OMSOBJSELECT", "OMS Object - ", "").Text + Code;
			ShowEditor(false);
			_currentobj = new OmsObject(Code);
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override bool UpdateData()
		{
			try
			{
				_currentobj.Update();
				Session.CurrentSession.ClearCache();
				this.IsDirty=false;
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
            //DO NOT REMOVE - Messagebox is displayed by base class
		}

		protected override void DeleteData(string Code)
		{
			FWBS.OMS.OmsObject.UnRegister(Code);
			Session.CurrentSession.ClearCache();
		}
		
		protected override string SearchListName
		{
			get
			{
				return "ADMOBJECTS";
			}
		}


		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.tpEdit.SuspendLayout();
			// 
			// tpList
			// 
			this.tpList.Name = "tpList";
			// 
			// tpEdit
			// 
			this.tpEdit.Controls.Add(this.propertyGrid1);
			this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
			this.tpEdit.Name = "tpEdit";
			this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
			this.tpEdit.Controls.SetChildIndex(this.propertyGrid1, 0);
			// 
			// pnlEdit
			// 
			this.pnlEdit.Name = "pnlEdit";
			// 
			// labSelectedObject
			// 
			this.labSelectedObject.Name = "labSelectedObject";
			// 
			// tbcEdit
			// 
			this.tbcEdit.Name = "tbcEdit";
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
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
			this.propertyGrid1.HelpVisible = false;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 49);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(422, 334);
			this.propertyGrid1.TabIndex = 1;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// ucOMSObjexts
			// 
			this.DockPadding.All = 8;
			this.Name = "ucOMSObjexts";
			this.tpEdit.ResumeLayout(false);

		}
		#endregion

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			IsDirty=true;
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

