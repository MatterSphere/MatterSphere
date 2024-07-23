using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// 37000 Displays a list of contacts associated to a specific client.  This is used as
    /// a IConfigurableTypeAddin, to be used in OMSDialogs.
    /// </summary>
    public class ucTimeRecording : ucBaseAddin, Interfaces.IOMSTypeAddin
	{
		#region Fields
        private IContainer components = null;
		/// <summary>
		/// The current client to work with.
		/// </summary>
		private OMSFile _omsfile = null;
		/// <summary>
		/// The Search List Object
		/// </summary>
		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private OMSToolBarButton _alltime = null;
		private OMSToolBarButton _allbilledtime = null;
		private OMSToolBarButton _allunbilledtime = null;
        #endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the user control.
		/// </summary>
		public ucTimeRecording()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                if (ucSearchControl1 != null)
                {
                    ucSearchControl1.Dispose();
                }
                if (components != null)
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
            this.components = new System.ComponentModel.Container();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(168, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(672, 490);
            this.ucSearchControl1.TabIndex = 8;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            // 
            // ucTimeRecording
            // 
            this.Controls.Add(this.ucSearchControl1);
            this.Name = "ucTimeRecording";
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region IOMSTypeAddin Implementation

		/// <summary>
		/// Allows the calling OMS dialog to connect to the addin for the configurable type object.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		/// <returns>A flag that tells the dialogthat the connection has been successfull.</returns>
		public override bool Connect(IOMSType obj)
		{
			ucSearchControl1.NewOMSTypeWindow -=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
			ucSearchControl1.NewOMSTypeWindow +=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
			_omsfile = obj as OMSFile;
			if (obj == null)
				return false;
			else
			{
				ToBeRefreshed=true;
				return true;
			}
		}

		public bool IsConnected
		{
			get
			{
				return _omsfile != null;
			}
		}

		/// <summary>
		/// Refreshes the addin visual look and data contents.
		/// </summary>
		public override void RefreshItem()
		{
			if (_omsfile != null && ToBeRefreshed)
			{
				try
				{
					ucSearchControl1.Search();
					ToBeRefreshed = false;
				}
				catch
				{
				}
			}
		}

		public override void SelectItem()
		{
			if (ucSearchControl1.SearchList == null)
			{
				ucSearchControl1.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.TimeRecording),false,_omsfile, null);

				_alltime = ucSearchControl1.GetOMSToolBarButton("cmdAllTime");
				if (_alltime == null)
				{
					ErrorBox.Show(ParentForm, new OMSException2("37001","Button name '%1%' is missing or not set to Graphical",new Exception(),false,"cmdAllTime"));
					return;
				}
				
				_allbilledtime = ucSearchControl1.GetOMSToolBarButton("cmdAllBilled");
				if (_allbilledtime == null)
				{
					ErrorBox.Show(ParentForm, new OMSException2("37001","Button name '%1%' is missing or not set to Graphical",new Exception(),false,"cmdAllBilled"));
					return;
				}

				_allunbilledtime = ucSearchControl1.GetOMSToolBarButton("cmdAllUnbilledTime");
				if (_allunbilledtime == null)
				{
					ErrorBox.Show(ParentForm, new OMSException2("37001","Button name '%1%' is missing or not set to Graphical",new Exception(),false,"cmdAllUnbilledTime"));
					return;
				}

				_alltime.Style = ToolBarButtonStyle.ToggleButton;
				_allbilledtime.Style = ToolBarButtonStyle.ToggleButton;
				_allunbilledtime.Style = ToolBarButtonStyle.ToggleButton;
				_alltime.Pushed=true;

                if (ucSearchControl1.SearchList.Style == FWBS.OMS.SearchEngine.SearchListStyle.List || ucSearchControl1.SearchList.Style == FWBS.OMS.SearchEngine.SearchListStyle.Filter)
					ucSearchControl1.Search();
			}
			if (ucSearchControl1.SearchList != null)
				ucSearchControl1.ShowPanelButtons();

		}

			
		#endregion

		#region Private

		private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			if (e.ButtonName == "cmdAllTime")
			{
				_alltime.Pushed=true;
				_allbilledtime.Pushed=false;
				_allunbilledtime.Pushed=false;
				ucSearchControl1.ExternalFilter = "";
			}
			else if (e.ButtonName == "cmdAllBilled")
			{
				_alltime.Pushed=false;
				_allbilledtime.Pushed=true;
				_allunbilledtime.Pushed=false;
				ucSearchControl1.ExternalFilter = "[timeBilled] = true";
			}
			else if (e.ButtonName == "cmdAllUnbilledTime")
			{
				_allunbilledtime.Pushed=true;
				_alltime.Pushed=false;
				_allbilledtime.Pushed=false;
				ucSearchControl1.ExternalFilter = "[timeBilled] = false";
			}
		}
		#endregion
	}
}
