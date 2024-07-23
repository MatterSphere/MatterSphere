using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The job list specialised repeater control container.
    /// </summary>
    public class ucJoblistRepeaterContainer : ucSelectorRepeaterContainer
	{
        private IContainer components;
		#region Control Fields

        private System.Windows.Forms.ToolStripSeparator tbSep3;
		private System.Windows.Forms.ToolStripButton tbSave;
		private System.Windows.Forms.ErrorProvider err;

		/// <summary>
		/// An enquiry form that may be the parent of this control.
		/// </summary>
		private FWBS.OMS.UI.Windows.EnquiryForm _enq = null;

		/// <summary>
		/// A combo box that will display the owners of job lists.
		/// </summary>
		private FWBS.Common.UI.Windows.eXPComboBox _owners;

		#endregion

		#region Fields

		/// <summary>
		/// Sets the controls first load flag.
		/// </summary>
		private PrecedentJobList _joblist = null;


		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Creates an instance of the job list repeater control.
		/// </summary>
		public ucJoblistRepeaterContainer()
		{
			InitializeComponent();

			if (Session.CurrentSession.IsLoggedIn)
			{
				this.SelectorRepeaterType = typeof(ucPrecJobSelector);
				this.SetCountBounds(0, 1000);
				_joblist = Session.CurrentSession.CurrentPrecedentJobList;
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tbSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbSave = new System.Windows.Forms.ToolStripButton();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this._owners = new FWBS.Common.UI.Windows.eXPComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Location = new System.Drawing.Point(3, 28);
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(3, 3, 15, 0);
            this.pnlContainer.Size = new System.Drawing.Size(410, 161);
            // 
            // toolBar1
            // 
            this.toolBar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSep3,
            this.tbSave});
            // 
            // tbAdd
            // 
            this.tbAdd.Visible = false;
            // 
            // tbSep3
            // 
            this.tbSep3.Name = "tbSep3";
            this.tbSep3.Size = new System.Drawing.Size(6, 23);
            // 
            // tbSave
            // 
            this.tbSave.Enabled = false;
            this.resLKP.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.tbSave.Name = "tbSave";
            this.tbSave.Padding = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.tbSave.Size = new System.Drawing.Size(23, 6);
            this.tbSave.Click += new System.EventHandler(this.toolBar1_SaveButtonClick);
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // _owners
            // 
            this._owners.ActiveSearchEnabled = true;
            this._owners.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._owners.CaptionWidth = 105;
            this._owners.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._owners.IsDirty = false;
            this._owners.Location = new System.Drawing.Point(152, 4);
            this.resLKP.SetLookup(this._owners, new FWBS.OMS.UI.Windows.ResourceLookupItem("REMOTEJOBS", "Remote Job Lists :", ""));
            this._owners.MaxLength = 0;
            this._owners.Name = "_owners";
            this._owners.Size = new System.Drawing.Size(260, 23);
            this._owners.TabIndex = 4;
            this._owners.Text = "Remote Job Lists :";
            this._owners.ActiveChanged += new System.EventHandler(this._owners_Changed);
            // 
            // ucJoblistRepeaterContainer
            // 
            this.Controls.Add(this._owners);
            this.Name = "ucJoblistRepeaterContainer";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.SelectorRemoved += new FWBS.OMS.UI.Windows.SelectorRepeaterChangedEventHandler(this.ucJoblistRepeaterContainer_SelectorRemoved);
            this.SelectorMoved += new FWBS.OMS.UI.Windows.SelectorRepeaterChangedEventHandler(this.ucJoblistRepeaterContainer_SelectorMoved);
            this.Load += new System.EventHandler(this.ucJoblist_Load);
            this.ParentChanged += new System.EventHandler(this._this_ParentChanged);
            this.Controls.SetChildIndex(this._owners, 0);
            this.Controls.SetChildIndex(this.pnlContainer, 0);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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


		#endregion

		#region Properties

		/// <summary>
		/// Gets or sEts the visibility of the toolbar.
		/// </summary>
		public override bool ShowToolBar
		{
			get
			{
				return base.ShowToolBar;
			}
			set
			{
				base.ShowToolBar = value;
				_owners.Visible = value;
			}
		}

		/// <summary>
		/// Gets the current job list which is used to render the selector container control.
		/// </summary>
		public PrecedentJobList JobList
		{
			get
			{
				return _joblist;
			}
		}

		#endregion
		
		#region Methods

		/// <summary>
		/// Will Refresh the Current Control's Job List
		/// </summary>
		public void RefreshJobList(PrecedentJobList jobList)
		{
			_joblist = jobList;
			RefreshJobList();
		}

		/// <summary>
		/// Will Refresh the Current Control's Job List
		/// </summary>
		public void RefreshJobList()
		{
            var msg = Message.Create(Handle, 0x000B /*WM_SETREDRAW*/, IntPtr.Zero, IntPtr.Zero);
            DefWndProc(ref msg);
            Clear();

            if (_joblist.LiveCount > 0)
            {
                for (int a = 0; a < _joblist.Count; a++)
                {
                    Control ctrl = null;
                    if (_joblist[a].Completed == false)
                    {
                        ctrl = Add(_joblist[a]);
                    }
                    if (_joblist[a].HasError)
                    {
                        err.SetError(ctrl, _joblist[a].ErrorMessage);
                    }
                }
            }

            msg.WParam = (IntPtr)1;
            DefWndProc(ref msg);
            Invalidate(true);
            Update();
        }

		/// <summary>
		/// Enables or disables the toolbar buttons depending on the current selected
		/// item situation.
		/// </summary>
		protected override void CheckButtonState()
		{
			base.CheckButtonState();
			if (tbSave != null) 
			{
				tbSave.Enabled = true;
			}
		}

		#endregion

		#region Captured Events

		private void ucJoblist_Load(object sender, System.EventArgs e)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				this.RefreshJobList(_joblist);
				_owners.AddItem(PrecedentJobList.GetAvailableJobListOwners(), "usrid", "usrfullname");
				tbRemoveAll.Visible = true;
				CheckButtonState();
			}
		}

		private void ucJoblistRepeaterContainer_SelectorRemoved(FWBS.OMS.UI.Windows.ucSelectorRepeaterContainer sender, FWBS.OMS.UI.Windows.SelectorRepeaterChangedEventArgs e)
		{
			PrecedentJob precjob = e.Selector.Object as PrecedentJob;
			if (_joblist.Count <= 1) this.Focus();
			if (precjob != null)
				_joblist.Remove(precjob);
		}

		private void ucJoblistRepeaterContainer_SelectorMoved(FWBS.OMS.UI.Windows.ucSelectorRepeaterContainer sender, FWBS.OMS.UI.Windows.SelectorRepeaterChangedEventArgs e)
		{
			PrecedentJob precjob = e.Selector.Object as PrecedentJob;
			if (precjob != null)
				_joblist.Insert(e.Ordinal, precjob);
		}

        private void toolBar1_SaveButtonClick(object sender, EventArgs e)
        {
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (sender == tbSave)
				{
					_joblist.Save();
					MessageBox.ShowInformation("JOBSSAVED", "Current Job list has been saved.");
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void _this_ParentChanged(object sender, EventArgs e)
		{
			if (this.Parent is FWBS.OMS.UI.Windows.EnquiryForm)
			{
				_enq = (FWBS.OMS.UI.Windows.EnquiryForm)this.Parent;
			}
		}

		private void _this_Load(object sender, EventArgs e)
		{
			if (_enq != null)
			{
				OMSFile file = _enq.Enquiry.Object as OMSFile;
				if (file != null)
					file.GenerateJobList();
			}
		}

		private void _owners_Changed(object sender, System.EventArgs e)
		{
			int usrid = -1;
			try
			{
				usrid = int.Parse(Convert.ToString(_owners.Value));
				_owners.Value = DBNull.Value;
				if (Services.SelectJobs(User.GetUser(Convert.ToInt32(usrid))))
				{
					_owners.AddItem(PrecedentJobList.GetAvailableJobListOwners(), "usrid", "usrfullname");
					RefreshJobList();
				}

			}
			catch
			{}
		}

		#endregion

	}
}
