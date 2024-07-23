namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple search form which will conduct a conflict search on a file 
    /// that has already been created.
    /// </summary>
    internal class frmConflictSearch : frmNewBrandIdent
	{
		#region Fields

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private ucSearchControl _conflict;

		private OMSFile _file;

		#endregion

		#region Constructors

		private frmConflictSearch(){}

		public frmConflictSearch(OMSFile file)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_file = file;
			_conflict.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.ClientConflict), file.Parent,null);
			this.Text = Session.CurrentSession.Resources.GetResource("FILECONFLICTSCH","Conflict Search","").Text;
			SetIcon(Images.DialogIcons.Client);

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
            this._conflict = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.SuspendLayout();
            // 
            // _conflict
            // 
            this._conflict.BackColor = System.Drawing.Color.White;
            this._conflict.BackGroundColor = System.Drawing.Color.White;
            this._conflict.ButtonPanelVisible = true;
            this._conflict.Dock = System.Windows.Forms.DockStyle.Fill;
            this._conflict.DoubleClickAction = "None";
            this._conflict.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._conflict.GraphicalPanelVisible = true;
            this._conflict.Location = new System.Drawing.Point(0, 0);
            this._conflict.Name = "_conflict";
            this._conflict.NavCommandPanel = null;
            this._conflict.Padding = new System.Windows.Forms.Padding(4);
            this._conflict.RefreshOnEnquiryFormRefreshEvent = false;
            this._conflict.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this._conflict.SearchListCode = "";
            this._conflict.SearchListType = "";
            this._conflict.Size = new System.Drawing.Size(719, 450);
            this._conflict.TabIndex = 0;
            this._conflict.ToBeRefreshed = false;
            this._conflict.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this._conflict_SearchCompleted);
            // 
            // frmConflictSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(719, 450);
            this.Controls.Add(this._conflict);
            this.Name = "frmConflictSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmConflictSearch";
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Methods

		
		/// <summary>
		/// Captures the search completed event of the conflict search screen.
		/// </summary>
		/// <param name="sender">The conflict search list control.</param>
		/// <param name="e">The search result parameters.</param>
		private void _conflict_SearchCompleted (object sender, SearchCompletedEventArgs e)
		{
			_file.ApplyConflictSearch(e.Count, e.Criteria);
			_file.Update();
		}

		#endregion


	}
}
