using System;
using System.Collections;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucOMSTypes.
    /// </summary>
    public class ucFeeEarners : ucEditBase2
	{
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Disposal
		/// </summary>
		private ArrayList _dispose = new ArrayList();
		private FWBS.OMS.UI.Windows.ucOMSTypeDisplay ucOMSTypeDisplay;
		private string _code = "";
		private FWBS.OMS.FeeEarner _fee = null;

		/// <summary>
		/// Gets the Form OMS Type
		/// </summary>

		#endregion

		#region Constructors
		public ucFeeEarners()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ucFeeEarners(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent,Params)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ucOMSTypeDisplay = new FWBS.OMS.UI.Windows.ucOMSTypeDisplay();
			this.tpEdit.SuspendLayout();
			// 
			// tpList
			// 
			this.tpList.Name = "tpList";
			// 
			// tpEdit
			// 
			this.tpEdit.Controls.Add(this.ucOMSTypeDisplay);
			this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
			this.tpEdit.Name = "tpEdit";
			this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
			this.tpEdit.Controls.SetChildIndex(this.ucOMSTypeDisplay, 0);
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
			// ucOMSTypeDisplay
			// 
			this.ucOMSTypeDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucOMSTypeDisplay.InformationPanelVisible = true;
			this.ucOMSTypeDisplay.ipc_BackColor = System.Drawing.SystemColors.ControlDark;
			this.ucOMSTypeDisplay.ipc_Visible = true;
			this.ucOMSTypeDisplay.ipc_Width = 175;
			this.ucOMSTypeDisplay.Location = new System.Drawing.Point(0, 49);
			this.ucOMSTypeDisplay.Name = "ucOMSTypeDisplay";
			this.ucOMSTypeDisplay.SearchManagerCloseVisible = false;
			this.ucOMSTypeDisplay.SearchManagerVisible = false;
			this.ucOMSTypeDisplay.Size = new System.Drawing.Size(549, 334);
			this.ucOMSTypeDisplay.TabIndex = 2;
			this.ucOMSTypeDisplay.ToBeRefreshed = false;
            this.ucOMSTypeDisplay.InfoPanelCloseVisible = false;
			this.ucOMSTypeDisplay.Dirty += new System.EventHandler(this.OnDirty);
			// 
			// ucFeeEarners
			// 
			this.DockPadding.All = 8;
			this.Name = "ucFeeEarners";
			this.tpEdit.ResumeLayout(false);

		}
		#endregion

		#region Overrides
		protected override string SearchListName
		{
			get
			{
				return "ADMFEELIST";
			}
		}
	
		protected override void LoadSingleItem(string Code)
		{
			_code = Code;
			_fee = FWBS.OMS.FeeEarner.GetFeeEarner(Convert.ToInt32(Code));
			ucOMSTypeDisplay.Open(_fee);
            ucOMSTypeDisplay.Dirty += new EventHandler(OnDirty);
			labSelectedObject.Text = _fee.FullName + " - " + FWBS.OMS.Session.CurrentSession.Resources.GetResource("FEEMGT","Fee Earner Management","").Text;
			ShowEditor(false);
			this.IsDirty=false;
		}

		protected override bool UpdateData()
		{
			ucOMSTypeDisplay.UpdateItem();
			ucOMSTypeDisplay.RefreshItem(true);
			return true;
		}

		protected override void NewData()
		{
			FWBS.OMS.UI.Windows.Services.Wizards.CreateFeeEarner();
			lstList.Search();
		}

		protected override bool CancelData()
		{
			_fee.Cancel();
			return true;
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

		#endregion

		#region Private
		private void OnDirty(object sender, EventArgs e)
		{
			this.IsDirty=true;
		}

        protected override void DeleteData(string Code)
        {
        }

        protected override bool Restore(string Code)
        {
            return true;
        }
		#endregion

	}
}
