using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple form that is used to hold a select client / file user control.  
    /// This form holds extra buttons so that a client can be found or added.
    /// </summary>
    internal class frmSelectClient : frmSelectClientFile
	{
        private ResourceLookup resourceLookup1;
        private IContainer components;
		#region Fields


        #endregion

        #region Constructors & Destructors

        /// <summary>
		/// Default constructor.
		/// </summary>
		private frmSelectClient()
		{
			InitializeComponent();
			if (Session.CurrentSession.IsLoggedIn)
			{
				cmdViewFile.Visible = false;
				cmdViewClient.Top = cmdViewFile.Top;
				ucFormStorage1.UniqueID = "Forms\\SelectClient";
			}
			SetIcon(Images.DialogIcons.Client);
		}

		/// <summary>
		/// Constructor to specifying the client to find immediately.  If null then no client
		/// is taken.
		/// </summary>
		/// <param name="client">Client to show in the select form.</param>
		public frmSelectClient(Client client) : this()
		{
			ucSelectClientFileSelector.GetClient(client);
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
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAlert
            // 
            this.pnlAlert.Location = new System.Drawing.Point(4, 5);
            this.pnlAlert.Size = new System.Drawing.Size(526, 56);
            // 
            // cmdViewFile
            // 
            this.cmdViewFile.Location = new System.Drawing.Point(5, 463);
            this.cmdViewFile.Size = new System.Drawing.Size(83, 0);
            this.cmdViewFile.Text = "&View %FILE%";
            // 
            // cmdFind
            // 
            this.cmdFind.Location = new System.Drawing.Point(5, 34);
            this.resourceLookup1.SetLookup(this.cmdFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdFind", "&Find...", ""));
            this.cmdFind.Size = new System.Drawing.Size(83, 23);
            this.cmdFind.Text = "&Find...";
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(5, 62);
            this.resourceLookup1.SetLookup(this.cmdCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdCancel", "&Cancel", ""));
            this.cmdCancel.Size = new System.Drawing.Size(83, 23);
            this.cmdCancel.Text = "&Cancel";
            // 
            // cmdViewClient
            // 
            this.cmdViewClient.Location = new System.Drawing.Point(5, 435);
            this.resourceLookup1.SetLookup(this.cmdViewClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdViewClient", "View &%CLIENT%", ""));
            this.cmdViewClient.Size = new System.Drawing.Size(83, 23);
            this.cmdViewClient.Text = "View &%CLIENT%";
            this.cmdViewClient.Visible = false;
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(5, 6);
            this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdOK", "&Proceed", ""));
            this.cmdOK.Size = new System.Drawing.Size(83, 23);
            this.cmdOK.Text = "&Proceed";
            // 
            // pnlMain
            // 
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.pnlMain.Location = new System.Drawing.Point(590, 9);
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5, 6, 4, 6);
            this.pnlMain.Size = new System.Drawing.Size(94, 471);
            // 
            // ucSelectClientFileSelector
            // 
            this.ucSelectClientFileSelector.CheckAllFileOptionVisible = true;
            this.ucSelectClientFileSelector.FavouriteHeight = 22;
            this.ucSelectClientFileSelector.Location = new System.Drawing.Point(3, 9);
            this.ucSelectClientFileSelector.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.ucSelectClientFileSelector.SelectClientFileSearchType = FWBS.OMS.UI.Windows.SelectClientFileSearchType.Client;
            this.ucSelectClientFileSelector.SelectFileVisible = false;
            this.ucSelectClientFileSelector.Size = new System.Drawing.Size(587, 471);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.UniqueID = "";
            // 
            // btnPrivateEmail
            // 
            this.btnPrivateEmail.Location = new System.Drawing.Point(5, 90);
            this.btnPrivateEmail.Size = new System.Drawing.Size(83, 23);
            // 
            // btnCreateClient
            // 
            this.btnCreateClient.Location = new System.Drawing.Point(5, 407);
            this.resourceLookup1.SetLookup(this.btnCreateClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCreateClient", "Cre&ate %CLIENT%", ""));
            this.btnCreateClient.Size = new System.Drawing.Size(83, 23);
            this.btnCreateClient.Text = "Cre&ate %CLIENT%";
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(5, 430);
            this.panel6.Size = new System.Drawing.Size(83, 5);
            // 
            // frmSelectClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(687, 485);
            this.Name = "frmSelectClient";
            this.Text = "Loading...";
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		#endregion

		#region Methods

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            SetIcon(Images.DialogIcons.Client);
        }

        protected override void UpdateCaption()
        {
            this.Text = Session.CurrentSession.Resources.GetResource("SELECTCLIENT", "Select a %CLIENT%", "").Text;
        }

		/// <summary>
		/// The load event accesses the resource strings of the buttons and form caption.
		/// </summary>
		/// <param name="sender">This select client / file form.</param>
		/// <param name="e">Empty event arguments.</param>
		private void frmSelectClient_Load(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				Global.ControlParser(this);
				ucFormStorage1.UniqueID = "Forms\\SelectClient";
			}
		}

		#endregion
	}
}
