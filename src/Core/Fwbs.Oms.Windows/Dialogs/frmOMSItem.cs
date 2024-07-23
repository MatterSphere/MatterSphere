using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A form that is to be used when an item has been picked from a search / list and
    /// is to be edited by an enquiry form.  The inherited OK, save, cancel buttons
    /// will be overriden to do whatever needs to be saved.
    /// </summary>
    internal class frmOMSItem : frmDialog, ISupportRightToLeft, Interfaces.IShowOMSItem
	{
		#region Fields

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;

		/// <summary>
		/// Enquiry form to use on the form.
		/// </summary>
		private FWBS.OMS.UI.Windows.EnquiryForm enquiryForm1;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// All other contructors call this constructor as it runs all methods and properties that
		/// they need.
		/// </summary>
		private frmOMSItem() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			cmdBack.Visible = false;

		}

		
		/// <summary>
		/// Initialises an enquiry form.
		/// </summary>
		internal frmOMSItem(Enquiry enq) : this()
		{
			enquiryForm1.Enquiry = enq;
			ucFormStorage1.UniqueID = @"Forms\Dialogs\" + enq.Code;
			ucFormStorage1.Version = enquiryForm1.Enquiry.Version;
            this.Text = Session.CurrentSession.Terminology.Parse(enquiryForm1.Enquiry.EnquiryName, true);
            Size n = enquiryForm1.GetCanvasSize();
			if (n != Size.Empty)
				this.ClientSize = new Size(n.Width + enquiryForm1.DockPadding.Left + enquiryForm1.DockPadding.Right, n.Height + pnlTop.Height + enquiryForm1.DockPadding.Top + enquiryForm1.DockPadding.Bottom);
		}

		/// <summary>
		/// Initialises an enquiry form.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">Parent to use for the enquiry form.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal frmOMSItem(string code,  object parent, Common.KeyValueCollection param) : this()
		{
			enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, EnquiryMode.Add, param);
			ucFormStorage1.UniqueID = @"Forms\Dialogs\" + code;
			ucFormStorage1.Version = enquiryForm1.Enquiry.Version;
            this.Text = Session.CurrentSession.Terminology.Parse(enquiryForm1.Enquiry.EnquiryName, true);
            Size n = enquiryForm1.GetCanvasSize();
			if (n != Size.Empty)
				this.ClientSize = new Size(n.Width + enquiryForm1.DockPadding.Left + enquiryForm1.DockPadding.Right, n.Height + pnlTop.Height + enquiryForm1.DockPadding.Top + enquiryForm1.DockPadding.Bottom);
		}


		/// <summary>
		/// Initialises an enquiry from that adds or edits a new object.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="mode">Enquiry edit mode option.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal frmOMSItem(string code, object parent, EnquiryMode mode, Common.KeyValueCollection param) : this()
		{
			enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, mode, param);
			ucFormStorage1.UniqueID = @"Forms\Dialogs\" + code;
			ucFormStorage1.Version = enquiryForm1.Enquiry.Version;
            this.Text = Session.CurrentSession.Terminology.Parse(enquiryForm1.Enquiry.EnquiryName, true);
            Size n = enquiryForm1.GetCanvasSize();
			if (n != Size.Empty)
				this.ClientSize = new Size(n.Width + enquiryForm1.DockPadding.Left + enquiryForm1.DockPadding.Right, n.Height + pnlTop.Height + enquiryForm1.DockPadding.Top + enquiryForm1.DockPadding.Bottom);
		}
		

		/// <summary>
		/// New entity enquiry form with a specified edit mode, also specifying whether to only have it as offline
		/// so that the database does not get updated at this moment in time.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">Parent to use for the enquiry form.</param>
		/// <param name="mode">Enquiry edit mode option.</param>
		/// <param name="offline">Offline option, if true then the database will not be updated.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal frmOMSItem(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param) : this()
		{
			enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, mode, offline, param);
			ucFormStorage1.UniqueID = @"Forms\Dialogs\" + code;
			ucFormStorage1.Version = enquiryForm1.Enquiry.Version;
            this.Text = Session.CurrentSession.Terminology.Parse(enquiryForm1.Enquiry.EnquiryName, true);
            Size n = enquiryForm1.GetCanvasSize();
			if (n != Size.Empty)
                this.ClientSize = new Size(n.Width + enquiryForm1.DockPadding.Left + enquiryForm1.DockPadding.Right, n.Height + pnlTop.Height + 25);
		}
		
		/// <summary>
		/// Edits an existing object with a specified enquiry form.
		/// </summary>
		/// <param name="code">Unique enquiry form code.</param>
		/// <param name="parent">Parent to use for the enquiry form.</param>
		/// <param name="obj">Enquiry compatible object that is to be edited by the wizard.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
        internal frmOMSItem(string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible obj, Common.KeyValueCollection param)
            : this()
        {
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, obj, param);
            ucFormStorage1.UniqueID = @"Forms\Dialogs\" + code;
            ucFormStorage1.Version = enquiryForm1.Enquiry.Version;
            this.Text = Session.CurrentSession.Terminology.Parse(enquiryForm1.Enquiry.EnquiryName, true);
            Size n = enquiryForm1.GetCanvasSize();
            if (n != Size.Empty)
                this.ClientSize = new Size(n.Width + enquiryForm1.DockPadding.Left + enquiryForm1.DockPadding.Right, n.Height + pnlTop.Height + 25);
        }

		#region Windows Form Designer generated code
	    /// <summary>
	    /// Required method for Designer support - do not modify
	    /// the contents of this method with the code editor.
	    /// </summary>
	    private void InitializeComponent()
        {
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.Size = false;
            // 
            // tbLeft
            // 
            this.tbLeft.Size = new System.Drawing.Size(174, 42);
            this.tbLeft.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // tbRight
            // 
            this.tbRight.Size = new System.Drawing.Size(223, 42);
            this.tbRight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            this.tbRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tbRight_MouseDown);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnOK);
            this.pnlTop.Controls.Add(this.btnCancel);
            this.pnlTop.Location = new System.Drawing.Point(0, 2);
            this.pnlTop.Size = new System.Drawing.Size(732, 44);
            this.pnlTop.Controls.SetChildIndex(this.btnCancel, 0);
            this.pnlTop.Controls.SetChildIndex(this.btnOK, 0);
            this.pnlTop.Controls.SetChildIndex(this.quickOK, 0);
            this.pnlTop.Controls.SetChildIndex(this.quickCancel, 0);
            // 
            // quickOK
            // 
            this.quickOK.Size = new System.Drawing.Size(19, 32);
            this.quickOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // quickCancel
            // 
            this.quickCancel.Size = new System.Drawing.Size(19, 32);
            this.quickCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(0, 46);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(732, 444);
            this.enquiryForm1.TabIndex = 2;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.enquiryForm1_NewOMSTypeWindow);
            this.enquiryForm1.Rendered += new System.EventHandler(this.enquiryForm1_Rendered);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(360, 56);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(3, 3);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "O";
            this.btnOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(3, 3);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "C";
            this.btnCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // frmOMSItem
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(732, 490);
            this.Controls.Add(this.enquiryForm1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOMSItem";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmOMSItem";
            this.Load += new System.EventHandler(this.frmOMSItem_Load);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.enquiryForm1, 0);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

	    }
		#endregion

        protected override void SetResources()
        {
            cmdBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "&Back", "").Text;
            cmdRefresh.Text = Session.CurrentSession.Resources.GetResource("CMDREFRESH", "&Refresh", "").Text;
            cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            cmdCancel.Text = Session.CurrentSession.Resources.GetResource("CMDCANCEL", "Cance&l", "").Text;
            cmdOK.Text = Session.CurrentSession.Resources.GetResource("OMSIOK", "&OK", "").Text;
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the enquiry form control from this enquiry form.
		/// </summary>
		public EnquiryForm EnquiryForm
		{
			get
			{
				return enquiryForm1;
			}
		}

        public EnquiryFormSettings Settings { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Captures the click event of the toolbar buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDialogs_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			btnOK.Focus();
			Application.DoEvents();
			if (e.Button == cmdRefresh) cmdRefresh_Click(sender,e);
			if (e.Button == cmdSave) cmdSave_Click(sender,e);
			if (e.Button == cmdOK) cmdOK_Click (sender,e);
			if (e.Button == cmdCancel) cmdCancel_Click (sender,e);
		}

		
		/// <summary>
		/// Refreshes the enquiry form.
		/// </summary>
		/// <param name="sender">Refresh button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdRefresh_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				//Chek for dirty data before refreshing the data.
				if (enquiryForm1.IsDirty)
				{
					DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?",""), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				
					switch (res)
					{
						case DialogResult.Yes:
							cmdSave_Click(this, EventArgs.Empty);
							break;
						case DialogResult.No:
							break;
						case DialogResult.Cancel:
							return;
					}				
				}
				enquiryForm1.RefreshItem();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
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
				Cursor = Cursors.WaitCursor;
				enquiryForm1.UpdateItem();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
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
		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				btnOK.Focus();
				Application.DoEvents();

				Cursor = Cursors.WaitCursor;
				enquiryForm1.UpdateItem();
				this.DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}


		/// <summary>
		/// Cancels any changes for the whole of the dialog form.
		/// </summary>
		/// <param name="sender">Cancel button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (enquiryForm1.IsDirty)
				// ADDED DMB 18/6/2004 if the form is unboud do not prompt as 
				//if(enquiryForm1.IsDirty && (EnquiryForm.Enquiry.Binding != EnquiryBinding.Unbound))
				{
					DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?",""), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			
					switch (res)
					{
						case DialogResult.Yes:
							//Changed DMB 18/6/2004 Unbound items are not saved by the save_click event as 
							//Dialog Result OK is not then passed back when form is closed
							//Instead call OK click to save and close for with Dialog Result OK to force update
							//of parent control
							//Still leaves the issue of clicking save then cance will still revert to old value
							//Maybe not a problem just a bit misleading
							//cmdSave_Click(this, EventArgs.Empty);
							//break;
							cmdOK_Click(this,EventArgs.Empty);
							return;
						case DialogResult.No:
							enquiryForm1.CancelItem();
							break;
						case DialogResult.Cancel:
							return;
					}
				}			
				this.Close();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}


		#endregion

		private void tbRight_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			btnOK.Focus();
			Application.DoEvents();
		}

		private void frmOMSItem_Load(object sender, System.EventArgs e)
		{
			this.ActiveControl = enquiryForm1.FirstControl;
		}

		private void enquiryForm1_NewOMSTypeWindow(object sender, FWBS.OMS.UI.Windows.NewOMSTypeWindowEventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowOMSType(e.OMSObject,"");
		}

		private void enquiryForm1_Rendered(object sender, System.EventArgs e)
		{
			//ADDED DMB 17/6/2004 disable refresh if unbound as it has no meaning
			if(EnquiryForm.Enquiry.Binding == EnquiryBinding.Unbound)
			{	
				tbLeft.Visible = false;
				tbRight.Buttons[1].Visible = false;
				tbRight.Width = this.ClientSize.Width;
			}

		}


        public void SetRTL(Form parentform)
        {
            if (this.RightToLeft != System.Windows.Forms.RightToLeft.Yes)
                return;

            foreach (Control item in this.Controls)
            {
                Global.RightToLeftControlConverter(item, parentform);
            }
            foreach (var item in tbRight.Buttons.Reverse<ToolBarButton>())
            {
                tbRight.Buttons.Remove(item);
                tbRight.Buttons.Insert(0, item);
            }
        }
    }
}
