using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Base wizard form that holds an enquiryform control for rendering and a basic welcome page.
    /// Inherit this form to extend its functionality if custom pages are needed to be rendered.
    /// </summary>
    internal class frmWizard : frmNewBrandIdent, ISupportRightToLeft
	{
        public const string AppointmentDialogWizardCode = "SCRFILAPPT";
        public const string TaskDialogWizardCode = "SCRFILTASK";

        #region Fields
        protected readonly WizardStyle wizardStyle = WizardStyle.Dialog;
        private bool _hideWelcomePage;
        private bool _hideHeaderPanel;
        protected WizardPageTracker wizardPageTracker;

		/// <summary>
		/// The continue text label.
		/// </summary>
		private System.Windows.Forms.Label labContinue;

		/// <summary>
		/// The welcome label at the top of the wizard on the welcome page.
		/// </summary>
		protected System.Windows.Forms.Label labWelcome;
		/// <summary>
		/// Welcome description label.  This is used to display more information on the forthcoming
		/// wizard.
		/// </summary>
		protected System.Windows.Forms.RichTextBox txtDescription;
		/// <summary>
		/// A 3D panel which holds the wizard buttons at the bottom of the page.
		/// </summary>
		protected FWBS.OMS.UI.Windows.ThreeDPanel pnlNavigation;
		/// <summary>
		/// Wizard back button.
		/// </summary>
		protected  System.Windows.Forms.Button btnBack;
		/// <summary>
		/// Wizard next button.
		/// </summary>
		protected System.Windows.Forms.Button btnNext;
		/// <summary>
		/// Welcome panel that holds all the welcome labels etc...
		/// </summary>
		protected System.Windows.Forms.Panel pnlWelcome;
		/// <summary>
		/// Picture box that holds the welcome bitmap.
		/// </summary>
		protected System.Windows.Forms.PictureBox picWelcome;
		/// <summary>
		/// Page header text for each of the wizard pages.
		/// </summary>
		protected System.Windows.Forms.Label labQuestionPage;
		/// <summary>
		/// A panel that holds all the enquiry form behind the welcome panel.
		/// </summary>
		protected System.Windows.Forms.Panel pnlEnquiry;
		/// <summary>
		/// The actual enquiory form rendering control.
		/// </summary>
		protected FWBS.OMS.UI.Windows.EnquiryForm enquiryForm1;
		/// <summary>
		/// The wizard cancel button.
		/// </summary>
		protected System.Windows.Forms.Button btnCancel;
		/// <summary>
		/// The wizards finished button.
		/// </summary>
		protected System.Windows.Forms.Button btnFinished;
		/// <summary>
		/// The page by page picture item that sis in the top left.
		/// </summary>
		protected System.Windows.Forms.PictureBox picPage;
		/// <summary>
		/// A panel that holds all the header information and pictures at the top of the page.
		/// </summary>
		protected System.Windows.Forms.Panel pnlHeader;
		/// <summary>
		/// A 3D effect button that is used for the etched underline of the page header information.
		/// </summary>
		protected FWBS.OMS.UI.Windows.ThreeDPanel pnlUnderline;
		/// <summary>
		/// A check box to decide whether to show the welcome screen of the wizard or not.
		/// </summary>
		private System.Windows.Forms.CheckBox chkDisable;
		private System.ComponentModel.IContainer components;
		/// <summary>
		/// Favourites object which will decide the chkDisabled value.
		/// </summary>
		private Favourites _favourites = null;
		private FWBS.OMS.UI.Windows.ResourceLookup resLKP;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		protected FWBS.OMS.UI.Windows.Accelerators accelerators1;
        protected WizardPageTracker.ProgressBar progressBar;
        protected ucFormStorage ucFormStorage1;
		
        #endregion

		#region Constructors & Destructors

		/// <summary>
		/// All other contructors call this constructor as it runs all methods and properties that they need.
		/// </summary>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		public frmWizard(WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle == WizardStyle.Dialog ? TitleBarStyle.Small : TitleBarStyle.System)
		{
            this.wizardStyle = wizardStyle;
			InitializeComponent();
            if (this.wizardStyle != WizardStyle.Dialog) this.TopLevel = false;
            wizardPageTracker = new WizardPageTracker(progressBar, enquiryForm1);
            txtDescription.BackColor = Color.FromArgb(panel1.BackColor.ToArgb());
            pnlHeader.BackColor = Color.White;
            pnlNavigation.BackColor = Color.White;
            picWelcome.Image = FWBS.OMS.UI.Properties.Resources.V2Wizard_WelcomePageImage;
		}


        internal frmWizard(Enquiry enq) : this()
		{
            using (var trace = Fwbs.Framework.Diagnostics.TraceDuration.Start(this, "Constructor"))
            {
                enquiryForm1.Enquiry = enq;
                this.ucFormStorage1.UniqueID = "Forms\\Wizards\\" + enquiryForm1.Code;
            }
		}

		/// <summary>
		/// Initialises a wizard.
		/// </summary>
		/// <param name="WizardCode">Unique enquiry form wizard code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		internal frmWizard(string WizardCode, object parent, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) :
            this(WizardCode, parent, EnquiryMode.Add, false, param, wizardStyle)
		{
		}

		/// <summary>
		/// New entity wizard with a specified edit mode, also specifying whether to only have it as offline
		/// so that the database does not get updated at this moment in time.
		/// </summary>
		/// <param name="WizardCode">Unique enquiry form wizard code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="mode">Enquiry edit mode option.</param>
		/// <param name="offline">Offline option, if true then the database will not be updated.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		internal frmWizard(string WizardCode, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) : this(wizardStyle)
		{
            using (var trace = Fwbs.Framework.Diagnostics.TraceDuration.Start(this, "Constructor"))
            {
                string uWizardCode = GetUserDefinedWizardCode(WizardCode);
                enquiryForm1.Enquiry = Enquiry.GetEnquiry(TransformEnquiryCode(uWizardCode), parent, mode, offline, param);
                this.ucFormStorage1.UniqueID = "Forms\\Wizards\\" + enquiryForm1.Code;
            }
		}
		
		/// <summary>
		/// Edits an existing object with a specified wizard enquiry form.
		/// </summary>
		/// <param name="WizardCode">Unique enquiry form wizard code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="obj">Enquiry vompatible object that is to be edited by the wizard.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		internal frmWizard (string WizardCode, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible obj, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) :
            this(WizardCode, parent, obj, false, param, wizardStyle)
        {
		}
		
		/// <summary>
		/// Edits an existing object with a specified wizard enquiry form.
		/// </summary>
		/// <param name="WizardCode">Unique enquiry form wizard code.</param>
		/// <param name="parent">The parent to use for the enquiry form.</param>
		/// <param name="obj">Enquiry vompatible object that is to be edited by the wizard.</param>
		/// <param name="offline">Flag to tell indicate wether a new record is created.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
        /// <param name="wizardStyle">Parameter indicating Wizard style.</param>
        internal frmWizard(string WizardCode, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog)
            : this(wizardStyle)
        {
            using (var trace = Fwbs.Framework.Diagnostics.TraceDuration.Start(this, "Constructor"))
            {
                enquiryForm1.Enquiry = Enquiry.GetEnquiry(TransformEnquiryCode(WizardCode), parent, obj, offline, param);
                this.ucFormStorage1.UniqueID = "Forms\\Wizards\\" + enquiryForm1.Code;
            }
        }


		/// <summary>
		/// Edits an existing object with a specified wizard enquiry form.
		/// </summary>
		/// <param name="obj">Enquiry vompatible object that is to be edited by the wizard.</param>
		/// <param name="param">Parameters to be used to replace parameters expected within the enquiry object.</param>
		internal frmWizard (FWBS.OMS.Interfaces.IEnquiryCompatible obj, Common.KeyValueCollection param) : this()
		{
            using (var trace = Fwbs.Framework.Diagnostics.TraceDuration.Start(this, "Constructor"))
            {
                enquiryForm1.Enquiry = obj.Edit(param);
                this.ucFormStorage1.UniqueID = "Forms\\Wizards\\" + enquiryForm1.Code;
            }
		}
	
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (enquiryForm1 != null)
                    {
                        enquiryForm1.Dispose();
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		#endregion

        private string GetUserDefinedWizardCode(string code)
        {
            bool isAppointmentDialogWizardCode = code.Equals(AppointmentDialogWizardCode, StringComparison.InvariantCultureIgnoreCase);

            bool isTaskDialogWizardCode = code.Equals(TaskDialogWizardCode, StringComparison.InvariantCultureIgnoreCase);

            SystemForms? systemForms = null;
            if (isAppointmentDialogWizardCode)
                systemForms = SystemForms.AppointmentWizard;
            else if (isTaskDialogWizardCode)
                systemForms = SystemForms.TaskWizard;

            return systemForms.HasValue ? Session.CurrentSession.DefaultSystemForm(systemForms.Value) : code;
        }

        /// <summary>
        /// Checks enquiry code and transforms it in accordance with WizardStyle if necessary.
        /// </summary>
        /// <param name="enquiryCode">Enquiry code to validate.</param>
        /// <returns>Transformed enquiry code.</returns>
        protected string TransformEnquiryCode(string enquiryCode)
        {
            string scrType = "SCR";
            switch (wizardStyle)
            {
                case WizardStyle.Dialog:
                    return enquiryCode;
                case WizardStyle.TaskPane:
                    scrType = "STP";
                    break;
                case WizardStyle.InPlace:
                    scrType = "SIP";
                    break;
            }
            return Regex.Replace(enquiryCode, @"^(fd|ud|u)?SCR(.+)", "$1" + scrType + "$2", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmWizard));
            this.pnlNavigation = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinished = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labWelcome = new System.Windows.Forms.Label();
            this.chkDisable = new System.Windows.Forms.CheckBox();
            this.labContinue = new System.Windows.Forms.Label();
            this.picWelcome = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.labQuestionPage = new System.Windows.Forms.Label();
            this.picPage = new System.Windows.Forms.PictureBox();
            this.pnlEnquiry = new System.Windows.Forms.Panel();
            this.pnlUnderline = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.progressBar = new FWBS.OMS.UI.Windows.WizardPageTracker.ProgressBar();
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.resLKP = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.pnlNavigation.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPage)).BeginInit();
            this.pnlEnquiry.SuspendLayout();
            this.pnlUnderline.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.SystemColors.Control;
            this.pnlNavigation.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Top;
            this.pnlNavigation.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlNavigation.Controls.Add(this.btnCancel);
            this.pnlNavigation.Controls.Add(this.btnFinished);
            this.pnlNavigation.Controls.Add(this.btnNext);
            this.pnlNavigation.Controls.Add(this.btnBack);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNavigation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlNavigation.Location = new System.Drawing.Point(0, 367);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(604, 44);
            this.pnlNavigation.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(12, 10);
            this.resLKP.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinished
            // 
            this.btnFinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinished.Enabled = false;
            this.btnFinished.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFinished.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFinished.Location = new System.Drawing.Point(518, 10);
            this.resLKP.SetLookup(this.btnFinished, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnFinished", "&Finish", ""));
            this.btnFinished.Name = "btnFinished";
            this.btnFinished.Size = new System.Drawing.Size(75, 25);
            this.btnFinished.TabIndex = 1;
            this.btnFinished.Text = "&Finish";
            this.btnFinished.Click += new System.EventHandler(this.btnFinished_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext.Location = new System.Drawing.Point(437, 10);
            this.resLKP.SetLookup(this.btnNext, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnNext", "&Next >", ""));
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 25);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "&Next >";
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBack.Location = new System.Drawing.Point(356, 10);
            this.resLKP.SetLookup(this.btnBack, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnBack", "< &Back", ""));
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 25);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "< &Back";
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.BackColor = System.Drawing.Color.White;
            this.pnlWelcome.Controls.Add(this.panel1);
            this.pnlWelcome.Controls.Add(this.picWelcome);
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWelcome.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlWelcome.Location = new System.Drawing.Point(0, 0);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(604, 367);
            this.pnlWelcome.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.labWelcome);
            this.panel1.Controls.Add(this.chkDisable);
            this.panel1.Controls.Add(this.labContinue);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(124, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20);
            this.panel1.Size = new System.Drawing.Size(480, 367);
            this.panel1.TabIndex = 8;
            // 
            // txtDescription
            // 
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtDescription.Location = new System.Drawing.Point(24, 72);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(436, 233);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.TabStop = false;
            this.txtDescription.Text = "";
            this.txtDescription.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.txtDescription_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(20, 72);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(4, 233);
            this.panel2.TabIndex = 8;
            // 
            // labWelcome
            // 
            this.labWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.labWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labWelcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labWelcome.Location = new System.Drawing.Point(20, 20);
            this.labWelcome.Name = "labWelcome";
            this.labWelcome.Size = new System.Drawing.Size(440, 52);
            this.labWelcome.TabIndex = 2;
            this.labWelcome.Text = "Welcome to FWBS %APPNAME% Wizard";
            // 
            // chkDisable
            // 
            this.chkDisable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkDisable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDisable.Location = new System.Drawing.Point(20, 305);
            this.resLKP.SetLookup(this.chkDisable, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkDisable", "Do not show the welcome page again.", ""));
            this.chkDisable.Name = "chkDisable";
            this.chkDisable.Size = new System.Drawing.Size(440, 24);
            this.chkDisable.TabIndex = 7;
            this.chkDisable.Text = "Do not show the welcome page again.";
            // 
            // labContinue
            // 
            this.labContinue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labContinue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labContinue.Location = new System.Drawing.Point(20, 329);
            this.resLKP.SetLookup(this.labContinue, new FWBS.OMS.UI.Windows.ResourceLookupItem("labContinue", "To continue, click Next.", ""));
            this.labContinue.Name = "labContinue";
            this.labContinue.Size = new System.Drawing.Size(440, 18);
            this.labContinue.TabIndex = 1;
            this.labContinue.Text = "To continue, click Next.";
            this.labContinue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWelcome
            // 
            this.picWelcome.BackColor = System.Drawing.Color.Transparent;
            this.picWelcome.Dock = System.Windows.Forms.DockStyle.Left;
            this.picWelcome.Image = ((System.Drawing.Image)(resources.GetObject("picWelcome.Image")));
            this.picWelcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picWelcome.Location = new System.Drawing.Point(0, 0);
            this.picWelcome.Name = "picWelcome";
            this.picWelcome.Size = new System.Drawing.Size(124, 367);
            this.picWelcome.TabIndex = 6;
            this.picWelcome.TabStop = false;
            this.picWelcome.Visible = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Control;
            this.pnlHeader.Controls.Add(this.labQuestionPage);
            this.pnlHeader.Controls.Add(this.picPage);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(604, 69);
            this.pnlHeader.TabIndex = 0;
            // 
            // labQuestionPage
            // 
            this.labQuestionPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labQuestionPage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labQuestionPage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labQuestionPage.Location = new System.Drawing.Point(12, 11);
            this.labQuestionPage.Name = "labQuestionPage";
            this.labQuestionPage.Size = new System.Drawing.Size(518, 46);
            this.labQuestionPage.TabIndex = 1;
            this.labQuestionPage.Text = "Enquiry Page Question Text?";
            this.labQuestionPage.TextChanged += new System.EventHandler(this.labQuestionPage_TextChanged);
            // 
            // picPage
            // 
            this.picPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picPage.Image = ((System.Drawing.Image)(resources.GetObject("picPage.Image")));
            this.picPage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picPage.Location = new System.Drawing.Point(544, 11);
            this.picPage.Name = "picPage";
            this.picPage.Size = new System.Drawing.Size(48, 48);
            this.picPage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPage.TabIndex = 0;
            this.picPage.TabStop = false;
            this.picPage.Visible = false;
            // 
            // pnlEnquiry
            // 
            this.pnlEnquiry.Controls.Add(this.pnlUnderline);
            this.pnlEnquiry.Controls.Add(this.pnlHeader);
            this.pnlEnquiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEnquiry.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlEnquiry.Location = new System.Drawing.Point(0, 0);
            this.pnlEnquiry.Name = "pnlEnquiry";
            this.pnlEnquiry.Size = new System.Drawing.Size(604, 367);
            this.pnlEnquiry.TabIndex = 2;
            // 
            // pnlUnderline
            // 
            this.pnlUnderline.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Top;
            this.pnlUnderline.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlUnderline.Controls.Add(this.progressBar);
            this.pnlUnderline.Controls.Add(this.enquiryForm1);
            this.pnlUnderline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUnderline.Location = new System.Drawing.Point(0, 69);
            this.pnlUnderline.Name = "pnlUnderline";
            this.pnlUnderline.Padding = new System.Windows.Forms.Padding(2);
            this.pnlUnderline.Size = new System.Drawing.Size(604, 298);
            this.pnlUnderline.TabIndex = 1;
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.ControlDark;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar.ForeColor = System.Drawing.Color.Orange;
            this.progressBar.Location = new System.Drawing.Point(2, 2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(600, 4);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.ActionBack = this.btnBack;
            this.enquiryForm1.ActionCancel = this.btnCancel;
            this.enquiryForm1.ActionFinish = this.btnFinished;
            this.enquiryForm1.ActionNext = this.btnNext;
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(2, 2);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.PageHeader = this.labQuestionPage;
            this.enquiryForm1.PageHeaderPicture = this.picPage;
            this.enquiryForm1.Size = new System.Drawing.Size(600, 294);
            this.enquiryForm1.Style = FWBS.OMS.UI.Windows.EnquiryStyle.Wizard;
            this.enquiryForm1.TabIndex = 1;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.WelcomeHeader = this.labWelcome;
            this.enquiryForm1.WelcomePagePicture = this.picWelcome;
            this.enquiryForm1.WelcomeText = this.txtDescription;
            this.enquiryForm1.PageChanging += new FWBS.OMS.UI.Windows.PageChangingEventHandler(this.enquiryForm1_PageChanging);
            this.enquiryForm1.PageChanged += new FWBS.OMS.UI.Windows.PageChangedEventHandler(this.enquiryForm1_PageChanged);
            this.enquiryForm1.Finished += new System.EventHandler(this.enquiryForm1_Finished);
            // 
            // accelerators1
            // 
            this.accelerators1.Active = false;
            this.accelerators1.Form = this;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "Forms\\Wizards";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // frmWizard
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(604, 411);
            this.Controls.Add(this.pnlWelcome);
            this.Controls.Add(this.pnlEnquiry);
            this.Controls.Add(this.pnlNavigation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Wizard";
            this.Shown += new System.EventHandler(this.frmWizard_Shown);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPage)).EndInit();
            this.pnlEnquiry.ResumeLayout(false);
            this.pnlUnderline.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Methods

        protected void ShowHeaderPanel(bool visible)
        {
            this.pnlHeader.Visible = visible && !_hideHeaderPanel;
        }

		/// <summary>
		/// The forms load event.
		/// </summary>
		/// <param name="e">Empty event arguments.</param>
        protected override void OnLoad(EventArgs e)
		{
            if (!TopLevel && Parent != null)
            {
                if (wizardStyle == WizardStyle.TaskPane)
                {
                    Padding padding = this.pnlUnderline.Padding;
                    padding.Top = 0;
                    this.pnlUnderline.Padding = padding;
                    this.progressBar.SendToBack();
                    this.progressBar.Visible = true;
                }
                else if (wizardStyle == WizardStyle.InPlace)
                {
                    ModifyWizardAppearance(EnquiryForm.PageCount > 1);
                    enquiryForm1.Dirty += ValidateFormCompleted;
                    enquiryForm1.PageChanged += ValidateFormCompleted;
                }
                this.ucFormStorage1.FormToStore = null;
                this.enquiryForm1.ChangeParentFormSize = false;
                _hideWelcomePage = true;
                _hideHeaderPanel = true;
            }

            base.OnLoad(e);

            //Make sure that the title is set even when defaulting to specific page.
            this.Text = labWelcome.Text;

            //When page was being set outside of wizard then the load was setting it back to the welcome page.
            if (enquiryForm1.PageNumber < 0)
            {
                //Defaults the welcome page to be in focus first.
                this.Controls.SetChildIndex(pnlWelcome, 0);
                //Set the text of the form the same as the welcome text which is applied 
                //through the enquiry form.
                
                pnlNavigation.SendToBack();
                EnableFinish(false);

                if (_hideWelcomePage)
                {
                    pnlWelcome.Visible = false;
                }
                else if (picWelcome.Tag == null)
                {
                    SetWelcomeImage();
                }
            }
            SetIcon(EnquiryForm?.Enquiry?.Glyph >= 0 ? (Images.DialogIcons) EnquiryForm.Enquiry.Glyph : Images.DialogIcons.EliteApp);
		}

        private void SetWelcomeImage()
        {
            Bitmap welcomeImage = null;
            try
            {
                Image wizlogo = Branding.GetWizardLogo();
                if (wizlogo is Bitmap)
                    welcomeImage = (Bitmap)wizlogo;
                else if (wizlogo != null)
                    welcomeImage = new Bitmap(wizlogo);
            }
            catch { }
            finally
            {
                if (welcomeImage == null)
                    welcomeImage = Properties.Resources.V2Wizard_WelcomePageImage;
            }
            if (DeviceDpi != 96)
            {
                ScaleBitmapLogicalToDevice(ref welcomeImage);
            }
            picWelcome.Image = welcomeImage;
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            if (!_hideWelcomePage && picWelcome.Tag == null)
            {
                SetWelcomeImage();
            }
        }

		/// <summary>
		/// Captures the Finished event of the wizard style enquiry form.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		/// <param name="e">Empty event arguments.</param>
		protected virtual void enquiryForm1_Finished(object sender, System.EventArgs e)
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
		/// Gets the enquiry form control from this wizard form.
		/// </summary>
		public virtual EnquiryForm EnquiryForm
		{
			get
			{
				return enquiryForm1;
			}
		}

		/// <summary>
		/// Captures the enquiry forms after page change event which passes current page information.
		/// </summary>
		/// <param name="sender">Enquiry form object that raised the event.</param>
		/// <param name="e">Specifies the page number, page type and direction the wizard is going.</param>
		protected virtual void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
			//If the page number is less than zero then display the welcome panel.
			//Otherwise, show the panel with the enquiry form on it.
			if (e.PageType == EnquiryPageType.Start)
			{
                switch (Session.CurrentSession.CurrentUser.HideAllWelcomeWizardPages)
                {
                    case FWBS.Common.TriState.True:
                        chkDisable.Checked = true;
                        break;
                    case FWBS.Common.TriState.False:
                        chkDisable.Checked = false;
                        goto default;
                    case FWBS.Common.TriState.Null:
                        chkDisable.Checked = Session.CurrentSession.HideAllWizardWelcomePages;
                        goto default;
                    default:
                        if (_favourites == null)
                            _favourites = new Favourites("WINUIWIZARD", enquiryForm1.Code);

                        if (_favourites.Count == 0)
                            _favourites.AddFavourite(enquiryForm1.Code, "", chkDisable.Checked.ToString());

                        try
                        {
                            chkDisable.Checked = Convert.ToBoolean(_favourites.Param1(0));
                        }
                        catch
                        {
                            chkDisable.Checked = false;
                        }
                        break;
                }

				this.Controls.SetChildIndex(pnlWelcome,0);
			}
			else if (e.PageType == EnquiryPageType.Enquiry)
            {
                this.Controls.SetChildIndex(pnlEnquiry, 0);
                if (_hideWelcomePage && e.PageNumber == 0)
                    btnBack.Enabled = false;
            }
            else if (e.PageType == EnquiryPageType.Custom)
			{
				this.Controls.SetChildIndex(pnlEnquiry,0);
			}
			enquiryForm1.Focus();

			//Enable the accept button depending on what page it is on.
            if (this.AcceptButton == null || this.AcceptButton == btnNext || this.AcceptButton == btnFinished)
            {
                if ((e.PageNumber + 1) == enquiryForm1.PageCount)
                    this.AcceptButton = btnFinished;
                else
                    this.AcceptButton = btnNext;
            }

            RunPageTrackingProcess((EnquiryForm)sender, e);
			accelerators1.Execute();
		}


        /// <summary>
        /// Display the current page number and total page count to a user as they work through a wizard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RunPageTrackingProcess(EnquiryForm sender, PageChangedEventArgs e)
        {
            wizardPageTracker.ApplyPageTrackingLogic(sender, e);
        }


		/// <summary>
		/// The event that gets captures before a page change.
		/// </summary>
		/// <param name="sender">Enquiry form.</param>
		/// <param name="e">Before page change event arguments.</param>
		protected virtual void enquiryForm1_PageChanging(object sender, FWBS.OMS.UI.Windows.PageChangingEventArgs e)
		{
            if (e.PageType == EnquiryPageType.Start && e.Direction == EnquiryPageDirection.Next && Session.CurrentSession.HideAllWizardWelcomePages == false && Session.CurrentSession.CurrentUser.HideAllWelcomeWizardPages != FWBS.Common.TriState.True && chkDisable.Checked)
			{
				//Make sure that the favourites object is initialised.
				if (_favourites == null)
					_favourites = new Favourites(enquiryForm1.Code);

				//Set the first parameter to be the value of the disable welcome screen check box.
				_favourites.Param1(0, chkDisable.Checked.ToString());
			}
		}

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (!Modal)
                Close();
        }

		//Capture the finish button click and update any changes to the user favourites object for the current user.
		private void btnFinished_Click(object sender, System.EventArgs e)
		{
			btnFinished.Focus();
			Application.DoEvents();
			try
			{
				Cursor = Cursors.WaitCursor;
                if (Session.CurrentSession.HideAllWizardWelcomePages == false && Session.CurrentSession.CurrentUser.HideAllWelcomeWizardPages != FWBS.Common.TriState.True && _favourites != null)
                    _favourites.Update();
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

		private void txtDescription_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}
		#endregion

		private void labQuestionPage_TextChanged(object sender, System.EventArgs e)
		{
            ShowHeaderPanel(labQuestionPage.Text != "");
		}

        private void frmWizard_Shown(object sender, EventArgs e)
        {
            //See whether to skip the welcome screen or not.
            bool skipwelcome = _hideWelcomePage;
            User user = Session.CurrentSession.CurrentUser;
            if (user != null)
            {
                switch (user.HideAllWelcomeWizardPages)
                {
                    case FWBS.Common.TriState.True:
                        chkDisable.Checked = true;
                        break;
                    case FWBS.Common.TriState.False:
                        chkDisable.Checked = false;
                        goto default;
                    case FWBS.Common.TriState.Null:
                        chkDisable.Checked = Session.CurrentSession.HideAllWizardWelcomePages;
                        if (chkDisable.Checked == false)
                            goto default;
                        else
                            break;
                    default:
                        //Initialise the favourties object.
                        if (_favourites == null)
                            _favourites = new Favourites("WINUIWIZARD", enquiryForm1.Code);

                        try
                        {
                            chkDisable.Checked = Convert.ToBoolean(_favourites.Param1(0));
                        }
                        catch { }
                        break;
                }
            }
            skipwelcome |= chkDisable.Checked;

            //Skip the welcome if the variable turns out to be true.
            if (skipwelcome)
            {
                //Only skip the welcome page if it is currently on the welcome page.
                if (enquiryForm1.PageNumber < 0)
                    enquiryForm1.NextPage();
            }

        }

        public void SetRTL(Form parentform)
        {
            foreach (Control item in this.Controls)
            {
                Global.RightToLeftControlConverter(item, this);
            }
            SwapX(btnFinished, btnBack);
            SwapX(btnBack, btnNext);
        }

        private void SwapX(Control control1, Control control2)
        {
            var storel = control2.Left;
            control2.Left = control1.Left;
            control1.Left = storel;
        }

        #region Wizard In-Place Modifications

        protected void EnableFinish(bool isEnabled)
        {
            btnFinished.Enabled = isEnabled;
            if (wizardStyle == WizardStyle.InPlace)
            {
                btnFinished.BackColor = isEnabled ? Color.FromArgb(21, 101, 192)
                                                  : Color.FromArgb(244, 244, 244);
                btnFinished.ForeColor = isEnabled ? Color.White
                                                  : Color.FromArgb(121, 121, 121);
                btnFinished.FlatAppearance.BorderColor = isEnabled 
                                                  ? Color.FromArgb(21, 101, 192)
                                                  : Color.Black;
            }
        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            var button = sender as Button;
            var isEnabled = button.Enabled;
            button.BackColor = isEnabled ? Color.FromArgb(216, 216, 216)
                                      : Color.FromArgb(244, 244, 244);
            button.ForeColor = isEnabled ? Color.FromArgb(51, 51, 51)
                                      : Color.FromArgb(121, 121, 121);
        }

        protected virtual void ValidateFormCompleted(object sender, EventArgs e)
        {
            var formCompleted = enquiryForm1.RequiredFieldsComplete()
                    && enquiryForm1.Enquiry != null;
            EnableFinish(formCompleted);
        }

        private void ModifyWizardAppearance(bool isMultiPage)
        {
            pnlNavigation.BackColor = Color.FromArgb(237, 243, 250);
            pnlNavigation.BorderStyle = ThreeDBorder3DStyle.None;
            pnlNavigation.Height = LogicalToDeviceUnits(46);
            pnlNavigation.Font = new Font("Segoe UI", 10.5F);
            pnlUnderline.Padding = Padding.Empty;
            pnlHeader.Size = Size.Empty;
            ModifyButtonsAppearance(isMultiPage);
        }

        private void ModifyButtonsAppearance(bool isMultiPage)
        {
            var YButtonLocation = LogicalToDeviceUnits(3);
            ApplyFlatStlye(btnFinished);
            btnFinished.AutoSize = isMultiPage;
            btnFinished.Location = new Point(pnlNavigation.Width - (btnFinished.Width + LogicalToDeviceUnits(13))
                , YButtonLocation);
            btnFinished.ResetText();
            btnFinished.Text = Session.CurrentSession.Resources.GetResource("ADD", "Add", string.Empty).Text;
            btnFinished.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            EnableFinish(false);
            ApplyFlatStlye(btnCancel);
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            if (isMultiPage)
            {
                ApplyFlatStlye(btnNext);
                btnNext.Location = new Point(btnFinished.Left - (btnNext.Width + LogicalToDeviceUnits(14))
                    , YButtonLocation);
                btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnNext.EnabledChanged += Button_EnabledChanged;
                btnNext.ResetText();
                btnNext.Text = Session.CurrentSession.Resources.GetResource("NEXTBTN", "Next", string.Empty).Text;

                ApplyFlatStlye(btnBack);
                btnBack.Location = new Point(LogicalToDeviceUnits(16), YButtonLocation);
                btnBack.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                btnBack.EnabledChanged += Button_EnabledChanged;
                btnBack.ResetText();
                btnBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "Back", string.Empty).Text;
                btnCancel.Location = new Point(btnNext.Left - (btnCancel.Width + LogicalToDeviceUnits(20))
                                    , YButtonLocation);
            }
            else
            {
                btnNext.Visible = false;
                btnBack.Visible = false;
                btnCancel.Location = new Point(btnFinished.Left - (btnCancel.Width + LogicalToDeviceUnits(14))
                    , YButtonLocation);
            }
        }

        private void ApplyFlatStlye(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.Size = LogicalToDeviceUnits(new Size(100, 32));
            btn.BackColor = Color.FromArgb(216, 216, 216);
            btn.ForeColor = Color.FromArgb(51, 51, 51);
        }

        protected void UpdateFinishButtonText(string text)
        {
            btnFinished.Text = text;
            AdjustCancelNextButtons();
        }

        protected void AdjustCancelNextButtons()
        {
            var YButtonLocation = LogicalToDeviceUnits(3);
            btnNext.Location = new Point(btnFinished.Left - (btnNext.Width + LogicalToDeviceUnits(14))
                                            , YButtonLocation);
            btnCancel.Location = new Point(btnNext.Left - (btnCancel.Width + LogicalToDeviceUnits(20))
                    , YButtonLocation);
        }

        #endregion

    }
}
