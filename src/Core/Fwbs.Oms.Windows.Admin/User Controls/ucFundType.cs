using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucFundType.
    /// </summary>
    public class ucFundType : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.DataGridTextBoxColumn dgcDesc2;
		private FundType _currentobj  = null;

		public ucFundType()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		protected override string SearchListName
		{
			get
			{
				return "ADMFUNDTYPES";
			}
		}


		public ucFundType(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent,Params)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			tpEdit.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource("FUNDTYPES","Fund Types","").Text;
			tpList.Text = tpEdit.Text;
            propertyGrid1.PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyChanged);
            propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyChanged);
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
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.dgcDesc2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.tpEdit.SuspendLayout();
			// 
			// tpList
			// 
			this.BresourceLookup1.SetLookup(this.tpList, new FWBS.OMS.UI.Windows.ResourceLookupItem("List", "List", ""));
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
			// lstList
			// 
			this.lstList.Name = "lstList";
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
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Left;
			this.propertyGrid1.HelpVisible = false;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 49);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(389, 334);
			this.propertyGrid1.TabIndex = 2;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// dgcDesc2
			// 
			this.dgcDesc2.Format = "";
			this.dgcDesc2.FormatInfo = null;
			this.dgcDesc2.HeaderText = "Description";
			this.BresourceLookup1.SetLookup(this.dgcDesc2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
			this.dgcDesc2.MappingName = "ftdesc";
			this.dgcDesc2.ReadOnly = true;
			this.dgcDesc2.Width = 400;
			// 
			// ucFundType
			// 
			this.DockPadding.All = 8;
			this.Name = "ucFundType";
			this.tpEdit.ResumeLayout(false);

		}
		#endregion

		protected override void LoadSingleItem(string Code)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("DEFFUNDTYPE", "Fund Type", ""), Code);
			ShowEditor(false);
			_currentobj = new FundType(Code);
			_currentobj.CodeChange +=new EventHandler(_currentobj_CodeChange);
			_currentobj.Change +=new EventHandler(_currentobj_Change);
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override bool UpdateData()
		{
			try
			{
				_currentobj.Update();
				lstList.Search();
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
			labSelectedObject.Text = string.Format("{0} - [{1}]", ResourceLookup.GetLookupText("DEFFUNDTYPE", "Fund Type", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
			ShowEditor(true);
			_currentobj = new FundType();
			_currentobj.CodeChange +=new EventHandler(_currentobj_CodeChange);
			_currentobj.Change +=new EventHandler(_currentobj_Change);
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		protected override void DeleteData(string Code)
		{
			FWBS.OMS.FundType.DeleteType(Code);
		}

		protected override bool Restore(string Code)
		{
			// Added return by MNW
			return FWBS.OMS.FundType.RestoreType(Code);
		}

		protected override void Clone(string Code)
		{
			labSelectedObject.Text = string.Format("{0} - [{1}]", ResourceLookup.GetLookupText("DEFFUNDTYPE", "Fund Type", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
			ShowEditor(true);
			_currentobj = FundType.Clone(Code);
			_currentobj.CodeChange +=new EventHandler(_currentobj_CodeChange);
			_currentobj.Change +=new EventHandler(_currentobj_Change);
			propertyGrid1.SelectedObject = _currentobj;
			propertyGrid1.HelpVisible=true;
		}

		private void _currentobj_CodeChange(object sender, EventArgs e)
		{
			labSelectedObject.Text = string.Format("{0} - [{1}]", ResourceLookup.GetLookupText("DEFFUNDTYPE", "Fund Type", ""), _currentobj.Code);
		}

		private void _currentobj_Change(object sender, EventArgs e)
		{
			IsDirty=true;
		}

        private void PropertyChanged(object sender, PropertyValueChangedEventArgs e)
        {
            base.IsDirty = true;
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
