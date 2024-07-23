using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This is a globally used search from that holds the search user control.
    /// </summary>
    internal class frmSearch : frmNewBrandIdent
	{
		
		#region Fields

		private System.ComponentModel.IContainer components;
		private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private FWBS.OMS.UI.Windows.Accelerators accelerators1;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSelect;

		/// <summary>
		/// The search specific user control.
		/// </summary>
		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchCtrl;
		public FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Panel pnlBase;
		private System.Windows.Forms.Button btnClose;

		/// <summary>
		/// If true and one row only in the search list them select and close the form automatically.
		/// </summary>
		private bool _autoSelect = false;
		private bool _hidePanelOnEdit = false;
		private bool _hidePanel = false;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// A blank constructor which stops this form for being created by default.
		/// </summary>
		private frmSearch() : base()
		{
			InitializeComponent();
		}


        /// <summary>
		/// Creates an instance of the search form and assigns the default search list code.
		/// </summary>
		/// <param name="type">Search list types.</param>
		/// <param name="parent">The parent object to use for criteria.</param>
		/// <param name="param">The parmater replacement array to use.</param>
		public frmSearch(string type, object parent, FWBS.Common.KeyValueCollection param) : this(type, true, parent, param, false, Common.TriState.Null)
		{
		}


		public frmSearch(SearchEngine.SearchList searchList, bool autoSelect, Common.TriState multiSelect) : this()
		{
			ucFormStorage1.UniqueID = "Forms\\Search\\" + searchList.Code;
			ucSearchCtrl.MultiSelect = multiSelect;
			ucSearchCtrl.SetSearchList(searchList, false, null);
			lblMessage.Visible = false;
			_autoSelect = autoSelect;
		}

		/// <summary>
		/// Creates an instance of the search form and assigns the default search list code.
		/// </summary>
		/// <param name="type">Search list types.</param>
		/// <param name="parent">The parent object to use for criteria.</param>
		/// <param name="param">The parmater replacement array to use.</param>
		/// <param name="autoSelect">Auto selects a singular item if there is only one in the list.</param>
		public frmSearch(string code, bool asType, object parent, FWBS.Common.KeyValueCollection param, bool autoSelect, Common.TriState multiSelect) : this()
		{
			ucFormStorage1.UniqueID = "Forms\\Search\\" + code;
			ucSearchCtrl.MultiSelect = multiSelect;
			if (asType)
				ucSearchCtrl.SetSearchListType(code, parent, param);
			else
				ucSearchCtrl.SetSearchList(code, parent, param);
			lblMessage.Visible = false;

			_autoSelect = autoSelect;

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlBase = new System.Windows.Forms.Panel();
            this.ucSearchCtrl = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.pnlBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.lblMessage.Size = new System.Drawing.Size(715, 52);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(715, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(85, 405);
            this.panel1.TabIndex = 3;
            // 
            // btnSelect
            // 
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelect.Location = new System.Drawing.Point(5, 6);
            this.resourceLookup1.SetLookup(this.btnSelect, new FWBS.OMS.UI.Windows.ResourceLookupItem("Select", "Select", ""));
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 25);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.Location = new System.Drawing.Point(720, 3);
            this.resourceLookup1.SetLookup(this.btnClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            // 
            // pnlBase
            // 
            this.pnlBase.Controls.Add(this.btnClose);
            this.pnlBase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBase.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBase.Location = new System.Drawing.Point(0, 405);
            this.pnlBase.Name = "pnlBase";
            this.pnlBase.Size = new System.Drawing.Size(800, 35);
            this.pnlBase.TabIndex = 4;
            // 
            // ucSearchCtrl
            // 
            this.ucSearchCtrl.BackColor = System.Drawing.Color.White;
            this.ucSearchCtrl.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchCtrl.ButtonPanelVisible = true;
            this.ucSearchCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchCtrl.DoubleClickAction = "None";
            this.ucSearchCtrl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchCtrl.GraphicalPanelVisible = true;
            this.ucSearchCtrl.Location = new System.Drawing.Point(0, 52);
            this.ucSearchCtrl.Margin = new System.Windows.Forms.Padding(0);
            this.ucSearchCtrl.Name = "ucSearchCtrl";
            this.ucSearchCtrl.NavCommandPanel = null;
            this.ucSearchCtrl.Padding = new System.Windows.Forms.Padding(4);
            this.ucSearchCtrl.SearchListCode = "";
            this.ucSearchCtrl.SearchListType = "";
            this.ucSearchCtrl.Size = new System.Drawing.Size(715, 353);
            this.ucSearchCtrl.TabIndex = 0;
            this.ucSearchCtrl.ToBeRefreshed = false;
            this.ucSearchCtrl.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchCtrl_SearchButtonCommands);
            this.ucSearchCtrl.SearchTypeChanged += new System.EventHandler(this.ucSearchCtrl_SearchTypeChanged);
            this.ucSearchCtrl.StateChanged += new FWBS.OMS.UI.Windows.SearchStateChangedEventHandler(this.ucSearchCtrl_StateChanged);
            this.ucSearchCtrl.OpenedOMSItem += new System.EventHandler(this.ucSearchCtrl_OpenedOMSItem);
            this.ucSearchCtrl.ClosedOMSItem += new System.EventHandler(this.ucSearchCtrl_ClosedOMSItem);
            this.ucSearchCtrl.Load += new System.EventHandler(this.ucSearchCtrl_Load);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\Search";
            this.ucFormStorage1.Version = ((long)(1));
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(800, 440);
            this.Controls.Add(this.ucSearchCtrl);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBase);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("OMSSearch", "OMS Search", ""));
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OMS Search";
            this.panel1.ResumeLayout(false);
            this.pnlBase.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		#endregion

		#region Properties

		/// <summary>
		/// Gets the current selected values that have been picked from the searched list.
		/// </summary>
		public FWBS.Common.KeyValueCollection ReturnValues
		{
			get
			{
				return ucSearchCtrl.ReturnValues;
			}
		}

		public FWBS.Common.KeyValueCollection[] SelectedItems
		{
			get
			{
				return ucSearchCtrl.SelectedItems;
			}
		}

		/// <summary>
		/// Gets or sets the message at the top of the search form.
		/// </summary>
		public string Message
		{
			get
			{
				return lblMessage.Text;
			}
			set
			{
				lblMessage.Text = value;
				lblMessage.Visible = !(lblMessage.Text.Trim() == "");
			}
		}

		public bool ButtonPanelHiddenOnEdit
		{
			get
			{
				return _hidePanelOnEdit;
			}
			set
			{
				_hidePanelOnEdit = value;
			}
		}

		public bool ButtonPanelHidden
		{
			get
			{
				return _hidePanel;
			}
			set
			{
				_hidePanel = value;
			}
		}

		#endregion

		#region Methods

		private void btnSelect_Click(object sender, System.EventArgs e)
		{
			if (ucSearchCtrl.SelectRowItem())
				this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// This method gets called when the search type changes within the user control.
		/// </summary>
		/// <param name="sender">Search control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ucSearchCtrl_SearchTypeChanged(object sender, System.EventArgs e)
		{
			this.Text = ucSearchCtrl.CurrentSearchText;
		
		}


		/// <summary>
		/// Captures the search state changed.  This is used to set the different accept keys
		/// depending on the state.
		/// </summary>
		/// <param name="sender">Search control.</param>
		/// <param name="e">Search state event arguments.</param>
		private void ucSearchCtrl_StateChanged(object sender, FWBS.OMS.UI.Windows.SearchStateEventArgs e)
		{
			if (e.State == SearchState.Search && ucSearchCtrl.cmdSearch != null)
			{
				this.AcceptButton = ucSearchCtrl.cmdSearch;
			}
			else if (e.State == SearchState.Select)
			{
				if (ucSearchCtrl.cmdSelect == null || ucSearchCtrl.cmdSelect.Text == "")
					this.AcceptButton = btnSelect;
				else
				{
					ucSearchCtrl.cmdSelect.Click -= new EventHandler(btnSelect_Click);
					this.AcceptButton = ucSearchCtrl.cmdSelect;
					ucSearchCtrl.cmdSelect.Click += new EventHandler(btnSelect_Click);
				}
			}
		
		}

		/// <summary>
		/// Captures the form load so that the search control can have the focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucSearchCtrl_Load(object sender, System.EventArgs e)
		{
			if (ucSearchCtrl.cmdSelect == null || ucSearchCtrl.cmdSelect.Text == "")
			{
				panel1.Visible = !_hidePanel;
			}
			else
				panel1.Visible = false;

			this.ucSearchCtrl.SearchCompleted +=new SearchCompletedEventHandler(ucSearchCtrl_SearchCompleted);

			this.Text = ucSearchCtrl.CurrentSearchText;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			ucSearchCtrl.Focus();
		}

		private void ucSearchCtrl_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			if (_autoSelect)
			{
				if (ucSearchCtrl.SearchList.ResultCount == 1)
				{
					if (ucSearchCtrl.SelectRowItem())
						this.DialogResult = DialogResult.OK;
				}
			}
			_autoSelect = false;
		}


		private void ucSearchCtrl_SearchButtonCommands(object sender, SearchButtonEventArgs e)
		{
			if (e.Action == SearchEngine.ButtonActions.Select)
				this.DialogResult = DialogResult.OK;
		}
		
		
		#endregion
		
		/// <summary>
		/// displays the panel1 in case it was hidden by the 'On Edit' property
		/// </summary>
		private void ucSearchCtrl_ClosedOMSItem(object sender, System.EventArgs e)
		{
			panel1.Visible = !_hidePanel;
		}
		
		/// <summary>
		/// hides the panel if property has been set to do so
		/// </summary>
		private void ucSearchCtrl_OpenedOMSItem(object sender, System.EventArgs e)
		{
			panel1.Visible = ! _hidePanelOnEdit;
		}


	}
}
