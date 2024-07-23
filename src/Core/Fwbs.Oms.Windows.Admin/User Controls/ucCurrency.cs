using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucCurrency.
    /// </summary>
    public class ucCurrency : FWBS.OMS.UI.Windows.Admin.ucEditBase2
    {
        #region fields

        private System.Windows.Forms.PropertyGrid propertyGrid1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.DataGridTextBoxColumn dgcSign;
		private FWBS.OMS.Currency _currentobj = new FWBS.OMS.Currency();

        #endregion

        #region Constructors

        public ucCurrency()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
        }

        public ucCurrency(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params)
            : base(mainparent, editparent, Params)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            _currentobj.UniqueIDChanged += new EventHandler(_currentobj_CodeChange);
            _currentobj.DataChanged += new EventHandler(_currentobj_Change);
            propertyGrid1.SelectedObject = _currentobj;
            propertyGrid1.PropertyValueChanged -= new PropertyValueChangedEventHandler(PropertyChanged);
            propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyChanged);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        #endregion
       
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
			this.dgcSign = new System.Windows.Forms.DataGridTextBoxColumn();
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
			this.propertyGrid1.Size = new System.Drawing.Size(389, 334);
			this.propertyGrid1.TabIndex = 2;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// dgcSign
			// 
			this.dgcSign.Format = "";
			this.dgcSign.FormatInfo = null;
			this.dgcSign.HeaderText = "Sign";
			this.BresourceLookup1.SetLookup(this.dgcSign, new FWBS.OMS.UI.Windows.ResourceLookupItem("Sign", "Sign", ""));
			this.dgcSign.MappingName = "curSign";
			this.dgcSign.ReadOnly = true;
			this.dgcSign.Width = 75;
			// 
			// ucCurrency
			// 
			this.DockPadding.All = 8;
			this.Name = "ucCurrency";
			this.tpEdit.ResumeLayout(false);

		}
		#endregion

		protected override void LoadSingleItem(string Code)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Currency", "Currency", ""), Code);
			_currentobj.Fetch(Code as string);
			ShowEditor(false);
			propertyGrid1.Refresh();
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
			labSelectedObject.Text = string.Format("{0} - [{1}]", ResourceLookup.GetLookupText("Currency", "Currency", ""), ResourceLookup.GetLookupText("Untitled", "Untitled", ""));
			_currentobj.Create();
			ShowEditor(true);
			propertyGrid1.HelpVisible=true;
			propertyGrid1.Refresh();
		}

		protected override void DeleteData(string Code)
		{
			_currentobj.Fetch(Code);
			_currentobj.Delete();
		}

		protected override bool Restore(string Code)
		{
			// Added return by MNW
			_currentobj.Fetch(Code);
			_currentobj.Restore();
			return true;
		}


		protected override string SearchListName
		{
			get
			{
				return "ADMCURRENCY";
			}
		}


		private void _currentobj_CodeChange(object sender, EventArgs e)
		{
			labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Currency", "Currency", ""), _currentobj.Code);
		}

		private void _currentobj_Change(object sender, EventArgs e)
		{
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
