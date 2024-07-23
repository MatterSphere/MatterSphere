using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.UserControls.ConflictSearch;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This wizard dialog is derived from frmWizard.  It is adapted to allow an extra
    /// page after the welcome page for the choice of a type for the object being created. 
    /// This  may then decide what enquiry form the rest of the wizard will use.
    /// </summary>
    internal class frmOMSTypeWizard : frmWizard
	{
		#region Fields

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Holds the type picker of type adder enquiry form depending 
		/// on which one is not being used at the time.
		/// </summary>
		protected EnquiryForm custom = null;

        /// <summary>
        /// The enquiry form to use for the custom form.
        /// </summary>
        private string _enqCustom = "";

        /// <summary>
        /// Holds a numerous number of conflict searches that may have been performed.
        /// </summary>
        protected ArrayList _conflictSearches = new ArrayList();

        /// <summary>
        /// The conflict searcher control.
        /// </summary>
        protected IConflictSearcher _conflict = null;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor that creates an instance of this wizard.
		/// </summary>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
        protected frmOMSTypeWizard(WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            wizardPageTracker = new WizardPageTracker(progressBar, enquiryForm1, custom);
            if (wizardStyle == WizardStyle.InPlace)
            {
                custom.Dirty += ValidateFormCompleted;
                custom.PageChanged += ValidateFormCompleted;
            }
			SetCustomButtons();
			SetMainFormButtons();

		}

        protected void InitializeEnquiryForm(SystemForms sysForm, object parent, bool offline, Common.KeyValueCollection param)
        {
            string enquiryCode = TransformEnquiryCode(Session.CurrentSession.DefaultSystemForm(sysForm));
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(enquiryCode, parent, EnquiryMode.Add, offline, param);
            ucFormStorage1.UniqueID = "Forms\\Wizards\\" + enquiryForm1.Code;

            _conflict = enquiryForm1.GetControl("_conflict") as IConflictSearcher;
            //Gets the conflict searching search list control.
            if (_conflict != null)
            {
                _conflict.ItemSelected += new EventHandler(this._conflict_ItemSelected);
                _conflict.SearchCompleted += new SearchCompletedEventHandler(this._conflict_SearchCompleted);
                _conflict.StateChanged += new SearchStateChangedEventHandler(this._conflict_StateChanged);
            }
        }

        protected void InitializeCustomForm(string enquiryCode, SystemForms sysForm, IOMSType obj, bool force)
        {
            if (string.IsNullOrEmpty(enquiryCode))
                enquiryCode = Session.CurrentSession.DefaultSystemForm(sysForm);

            enquiryCode = TransformEnquiryCode(enquiryCode);
            if (enquiryCode != _enqCustom || force)
            {
                if (custom.Enquiry != null)
                {
                    Global.RemoveAndDisposeControls(custom);
                    custom.Enquiry.Dispose();
                    custom.Enquiry = null;
                }
                _enqCustom = enquiryCode;
                custom.Enquiry = obj.Edit(enquiryCode, null);
            }
        }

        protected void SetConflictSearchList(SystemSearchListGroups group, object parent)
        {
            ShowHeaderPanel(false);
            if (_conflict.SearchList == null)
            {
                _conflict.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(group), parent, null);
            }
            _conflict.AutoScroll = false;
            _conflict.Focus();
            _conflict.AutoScroll = true;
        }

        protected void SetConflictSearchDefaults(Common.KeyValueCollection defaults)
        {
            if (_conflict.EnquiryForm != null)
            {
                foreach (string controlName in defaults)
                {
                    Common.UI.IBasicEnquiryControl2 ctrl = _conflict.EnquiryForm.GetControl(controlName) as Common.UI.IBasicEnquiryControl2;
                    if (ctrl != null)
                    {
                        ctrl.Value = defaults[controlName].Value;
                    }
                }
            }
            this.AcceptButton = _conflict.cmdSearch;
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
                this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmOMSTypeWizard_Closing);
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
            this.custom = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.pnlNavigation.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).BeginInit();
            this.pnlEnquiry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPage)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlUnderline.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUnderline
            // 
            this.pnlUnderline.Controls.Add(this.custom);
            this.pnlUnderline.Controls.SetChildIndex(this.custom, 0);
            this.pnlUnderline.Controls.SetChildIndex(this.enquiryForm1, 0);
            this.pnlUnderline.Controls.SetChildIndex(this.progressBar, 0);
            // 
            // custom
            // 
            this.custom.ActionBack = this.btnBack;
            this.custom.ActionFinish = this.btnFinished;
            this.custom.ActionNext = this.btnNext;
            this.custom.AutoScroll = true;
            this.custom.ChangeParentFormSize = false;
            this.custom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.custom.IsDirty = false;
            this.custom.Location = new System.Drawing.Point(2, 2);
            this.custom.Name = "custom";
            this.custom.PageHeader = this.labQuestionPage;
            this.custom.Size = new System.Drawing.Size(600, 294);
            this.custom.Style = FWBS.OMS.UI.Windows.EnquiryStyle.Wizard;
            this.custom.TabIndex = 2;
            this.custom.ToBeRefreshed = false;
            this.custom.Visible = false;
            this.custom.WelcomeHeader = this.labWelcome;
            this.custom.WelcomeText = this.txtDescription;
            // 
            // frmOMSTypeWizard
            // 
            this.Name = "frmOMSTypeWizard";
            this.Text = "frmOMSTypeWizard";
            this.Shown += new System.EventHandler(this.FrmOMSTypeWizard_Shown);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).EndInit();
            this.pnlEnquiry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPage)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlUnderline.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private void FrmOMSTypeWizard_Shown(object sender, EventArgs e)
        {
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmOMSTypeWizard_Closing);
        }

	    #endregion

		#endregion

		#region Methods

		/// <summary>
		/// Captures the enquiry forms after page change event which passes current page information.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		/// <param name="e">Specifies the page number, page type and direction the wizard is going.</param>
		protected override void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
			try
			{
				//Run the base forms event first.
				base.enquiryForm1_PageChanged(sender, e);
				
				//If the wizard is on its last custom page then show the associated enquiry form.
				if (e.PageType == EnquiryPageType.Custom && e.Direction == EnquiryPageDirection.Next)
				{

					SetCustomButtons();

					custom.PageChanging -= new PageChangingEventHandler(this.custom_PageChanging);
					custom.PageChanging += new PageChangingEventHandler(this.custom_PageChanging);
					custom.PageChanged -= new PageChangedEventHandler(this.custom_PageChanged);
					custom.PageChanged += new PageChangedEventHandler(this.custom_PageChanged);
					custom.Finished -= new EventHandler(this.custom_Finished);
					custom.Finished += new EventHandler(this.custom_Finished);
					custom.Finishing -=new CancelEventHandler(custom_Finishing);
					custom.Finishing +=new CancelEventHandler(custom_Finishing);

					custom.ClearPageFlowHistory();
					custom.GotoWelcomePage();
					custom.GotoPage((short)0,false,false,false);

					//Make the main form invisible and the custom one visible.
					enquiryForm1.Visible = false;
					custom.Visible = true;
					custom.FirstControl.Focus();
				}
					//Refresh the welcome page if the welcome page is reselected.
				else if (e.PageType == EnquiryPageType.Start && e.Direction == EnquiryPageDirection.Back)
				{
					//When going back to the start page of the entire wizard make sure
					//that the welcome page is refreshed so that the main forms welcome
					//text is available rather that the custom forms.
					enquiryForm1.RefreshWizardWelcomePage();
				}

			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		protected void custom_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
            RunPageTrackingProcess((EnquiryForm)sender, e);
			accelerators1.Execute();
		}

		/// <summary>
		/// Captures the enquiry forms after page change event which passes current page information.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		/// <param name="e">Specifies the page number, page type and direction the wizard is going.</param>
		protected void custom_PageChanging(object sender, FWBS.OMS.UI.Windows.PageChangingEventArgs e)
		{
			if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Back && e.PageNumber == 0)
			{
				SetMainFormButtons();
				enquiryForm1.PreviousPage();
				enquiryForm1.SetParentSize();
				enquiryForm1.Visible = true;
				custom.Visible = false;
			}
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next)
			{
				enquiryForm1.Visible = false;
				custom.Visible = true;
				enquiryForm1.ClearForwardHistory();
			}
		}

		/// <summary>
		/// Captures the enquiry forms finished event.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		protected virtual void custom_Finished(object sender, EventArgs e)
		{
            if (!Modal)
            {
                if (IsHandleCreated)
                    BeginInvoke(new Action(this.Close));
                else
                    Close();
            }
		}

		/// <summary>
		/// Captures the enquiry forms finishing event.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		protected virtual void custom_Finishing(object sender, CancelEventArgs e)
		{
		}
		
		/// <summary>
		/// References all of the custom forms buttons and de-assigns the main enquiry form buttons.
		/// </summary>
		protected void SetCustomButtons()
		{
			enquiryForm1.ActionBack = null;
			enquiryForm1.ActionNext = null;
			enquiryForm1.ActionFinish = null;

			custom.ActionBack = btnBack;
			custom.ActionNext = btnNext;
			custom.ActionFinish = btnFinished;
		}

		/// <summary>
		/// References all of the main enquiry form  buttons and de-assigns the custom form buttons.
		/// </summary>
		protected void SetMainFormButtons()
		{
			custom.ActionBack = null;
			custom.ActionNext = null;
			custom.ActionFinish = null;

			enquiryForm1.ActionBack = btnBack;
			enquiryForm1.ActionNext = btnNext;
			enquiryForm1.ActionFinish = btnFinished;
		}

        protected bool IsConflictSearchVisible()
        {
            return _conflict != null && _conflict.Visible;
        }
        
        /// <summary>
        /// This method gets run after an item has been selected from the conflict search list.
        /// </summary>
        /// <param name="sender">Search control</param>
        /// <param name="e">Empty event arguments.</param>
        private void _conflict_ItemSelected(object sender, System.EventArgs e)
        {
            OnConflictItemSelected(sender, _conflict.ReturnValues);
        }

        protected virtual void OnConflictItemSelected(object sender, Common.KeyValueCollection returnValues)
        {
        }

        /// <summary>
        /// Captures the search state changed.  This is used to set the different accept keys
        /// depending on the state.
        /// </summary>
        /// <param name="sender">Search control.</param>
        /// <param name="e">Search state event arguments.</param>
        private void _conflict_StateChanged(object sender, FWBS.OMS.UI.Windows.SearchStateEventArgs e)
        {
            if (e.State == SearchState.Search && _conflict.cmdSearch != null)
            {
                this.AcceptButton = _conflict.cmdSearch;
            }
            else if (e.State == SearchState.Select && _conflict.cmdSelect != null)
            {
                this.AcceptButton = _conflict.cmdSelect;
            }
        }

        /// <summary>
        /// Captures the search completed event of the conflict search screen.
        /// </summary>
        /// <param name="sender">The conflict search list control.</param>
        /// <param name="e">The search result parameters.</param>
        private void _conflict_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            _conflictSearches.Add(e);
        }

        protected override void ValidateFormCompleted(object sender, EventArgs e)
        {
            var formCompleted = custom.RequiredFieldsComplete()
                    && custom.Enquiry != null;
            EnableFinish(!enquiryForm1.Visible && formCompleted);
        }

        private void frmOMSTypeWizard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                if (!TopLevel || MessageBox.ShowYesNoQuestion("RUSUREQUIT", "Are you sure you wish to cancel?") == DialogResult.Yes)
                {
                    if (Session.CurrentSession.IsConnected)
                        custom.CancelItem();

                    this.Closing -= this.frmOMSTypeWizard_Closing;
                }
                else
                {
                    e.Cancel = true;
                    btnCancel.Enabled = true;
                }
            }

        }

        #endregion

    }
}
