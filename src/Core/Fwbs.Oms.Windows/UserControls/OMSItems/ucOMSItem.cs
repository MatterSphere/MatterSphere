using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A user control similar to frmOMSItem that is to be used when an item has been picked 
    /// from a search / list and is to be edited by an enquiry form.  
    /// </summary>
    public class ucOMSItem : ucOMSItemBase
	{

		#region Fields

		/// <summary>
		/// A panel that holds all of the data manipulation buttons.
		/// </summary>
        private System.Windows.Forms.Panel pnlButtons;
		/// <summary>
		/// A refresh button to refresh the data within the enquiry form.
		/// </summary>
		protected System.Windows.Forms.Button cmdRefresh;
		/// <summary>
		/// Saves the data within the enquiry form.
		/// </summary>
		protected System.Windows.Forms.Button cmdSave;
		/// <summary>
		/// Cancels any update of the form.
		/// </summary>
		protected System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// Saves and closes the enquiry form.
		/// </summary>
        protected System.Windows.Forms.Button btnOK;
        private IContainer components;
        private Timer timRunOnce;
        private ResourceLookup resourceLookup1;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the control.
		/// </summary>
		public ucOMSItem()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
		}

		/// <summary>
		/// Initialises an enquiry form that creates a business object entity.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal ucOMSItem(string code,  object parent, Common.KeyValueCollection param) : this(code, parent, EnquiryMode.Add, false, param)
		{
		}


		/// <summary>
		/// Initialises an enquiry from that adds or edits a new object.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="mode">Enquiry edit mode option.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal ucOMSItem(string code, object parent, EnquiryMode mode, Common.KeyValueCollection param) : this(code, parent, mode, false, param)
		{
		}
		

		/// <summary>
		/// New entity enquiry form with a specified edit mode, also specifying whether to only have it as offline
		/// so that the database does not get updated at this moment in time.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="mode">Enquiry edit mode option.</param>
		/// <param name="offline">Offline option, if true then the database will not be updated.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal ucOMSItem(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param) : this()
		{
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, mode, offline, param);
			enquiryForm1.Dirty += new EventHandler(OnDirty);
            enquiryForm1.NewOMSTypeWindow +=new NewOMSTypeWindowEventHandler(OnNewOMSTypeWindow);
			enquiryForm1.SetCanvasSize();
            enquiryForm1.AutoScroll = false;
            timRunOnce.Enabled = true;
        }
		
		/// <summary>
		/// Edits an existing object with a specified enquiry form.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="obj">Enquiry compatible object that is to be edited by the wizard.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal ucOMSItem (string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param) : this()
		{
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, obj, param);
			enquiryForm1.Enquiry.Offline = offline;
			enquiryForm1.Dirty += new EventHandler(OnDirty);
            enquiryForm1.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(OnNewOMSTypeWindow); 
            enquiryForm1.SetCanvasSize();
            enquiryForm1.AutoScroll = false;
            timRunOnce.Enabled = true;
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
            this.components = new System.ComponentModel.Container();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.timRunOnce = new System.Windows.Forms.Timer(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.cmdRefresh);
            this.pnlButtons.Controls.Add(this.cmdSave);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(364, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(4);
            this.pnlButtons.Size = new System.Drawing.Size(84, 280);
            this.pnlButtons.TabIndex = 0;
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdRefresh.Location = new System.Drawing.Point(4, 76);
            this.resourceLookup1.SetLookup(this.cmdRefresh, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdRefresh", "&Refresh", ""));
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(76, 24);
            this.cmdRefresh.TabIndex = 10;
            this.cmdRefresh.Text = "&Refresh";
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdSave.Location = new System.Drawing.Point(4, 52);
            this.resourceLookup1.SetLookup(this.cmdSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdSave", "&Save", ""));
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(76, 24);
            this.cmdSave.TabIndex = 11;
            this.cmdSave.Text = "&Save";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(4, 28);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(4, 4);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("UOMOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // timRunOnce
            // 
            this.timRunOnce.Interval = 500;
            this.timRunOnce.Tick += new System.EventHandler(this.timRunOnce_Tick);
            // 
            // ucOMSItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnlButtons);
            this.Name = "ucOMSItem";
            this.Size = new System.Drawing.Size(448, 280);
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Refreshes the enquiry form.
		/// </summary>
		/// <param name="sender">Refresh button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdRefresh_Click(object sender, System.EventArgs e)
		{
            cmdRefresh.Focus();
            try
			{
				Cursor = Cursors.WaitCursor;
				if (enquiryForm1.IsDirty)
				{
					DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?","").Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			
					switch (res)
					{
						case DialogResult.Yes:
							enquiryForm1.UpdateItem();
							break;
						case DialogResult.No:
							CancelItem();
							break;
						case DialogResult.Cancel:
							return;
					}

					RefreshItem();
				}

			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Updates and saves the enquiry form data to the data source.
		/// </summary>
		/// <param name="sender">Save button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdSave_Click(object sender, System.EventArgs e)
		{
			try
			{
                cmdSave.Focus();
                Cursor = Cursors.WaitCursor;
				UpdateItem();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Attempts to dave the enquiry form data to the database and then closes the form.
		/// </summary>
		/// <param name="sender">OK button object.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
                btnOK.Focus();
                Cursor = Cursors.WaitCursor;
				enquiryForm1.UpdateItem();
                if (!enquiryForm1.IsFormDirty)
                {
                    OnClose(ClosingWhy.Saved);
                }
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Allows a Public Cancel Item with all the UI Accouterments
		/// http://dictionary.reference.com/search?r=2&q=Accouterments
		/// </summary>
		public override void CancelUIItem()
		{
			btnCancel.Focus();
			Application.DoEvents();
			cmdCancel_Click(this,EventArgs.Empty);
		}

		/// <summary>
		/// Closes the enquiry form.
		/// </summary>
		/// <param name="sender">Close button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				if (enquiryForm1.IsDirty)
				{
					DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?","").Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		
					switch (res)
					{
						case DialogResult.Yes:
                            btnOK_Click(sender, e);
                            return;
						case DialogResult.No:
							CancelItem();
							break;
						case DialogResult.Cancel:
							return;
					}
				}
				OnClose(ClosingWhy.Cancel);
				

			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}

		}

		#endregion

        private void timRunOnce_Tick(object sender, EventArgs e)
        {
            timRunOnce.Enabled = false;
            enquiryForm1.AutoScroll = true;
        }

    }
}
