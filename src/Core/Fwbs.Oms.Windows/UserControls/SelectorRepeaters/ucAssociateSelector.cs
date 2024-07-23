using System;
using System.ComponentModel;
using System.Linq;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Allows the selection a contact to be associated to the current file. Contacts / Associates may be added or created during the process.
    /// </summary>
    public class ucAssociateSelector : ucSelectorClass
	{
		#region Controls

        protected FWBS.Common.UI.Windows.eXPComboBox cboType;
        protected FWBS.Common.UI.Windows.eTextBox2 txtSalutation;
        protected FWBS.Common.UI.Windows.eTextBox2 txtTheirRef;
        protected FWBS.Common.UI.Windows.eTextBox2 txtAddressee;
        protected FWBS.OMS.UI.Windows.eNumberSelector txtTel;
        protected FWBS.OMS.UI.Windows.eNumberSelector txtFax;
        protected FWBS.OMS.UI.Windows.eNumberSelector txtMobile;
        protected FWBS.OMS.UI.Windows.eEmailSelector txtEmail;
        protected FWBS.OMS.UI.Windows.eAddressSelector address;
        protected FWBS.Common.UI.Windows.eXPComboBox cboHeading;
        protected FWBS.Common.UI.Windows.eMultiTextBox2 txtHeading;
        protected System.Windows.Forms.Button btnAdvanced;
        protected FWBS.OMS.UI.Windows.ResourceLookup res;
        protected FWBS.Common.UI.Windows.eCaptionLine eCaptionLine1;

		#endregion
	
		#region Fields

        protected virtual int MaxHeight
        {
            get { return LogicalToDeviceUnits(304); }
        }

        protected virtual int MinHeight
        {
            get { return LogicalToDeviceUnits(136); }
        }

		/// <summary>
		/// The contact that is associated.
		/// </summary>
		private Associate _associate = null;

		/// <summary>
		/// The client that the associate is associated to.
		/// </summary>
		private Client _client = null;
		private System.ComponentModel.IContainer components;


		/// <summary>
		/// The that the associate should be associated to.
		/// </summary>
		private OMSFile _file = null;


		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ucAssociateSelector()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			cboType_Changed(cboType, EventArgs.Empty);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucAssociateSelector));
            this.cboType = new FWBS.Common.UI.Windows.eXPComboBox();
            this.cboHeading = new FWBS.Common.UI.Windows.eXPComboBox();
            this.txtHeading = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.txtSalutation = new FWBS.Common.UI.Windows.eTextBox2();
            this.txtTheirRef = new FWBS.Common.UI.Windows.eTextBox2();
            this.txtTel = new FWBS.OMS.UI.Windows.eNumberSelector();
            this.txtFax = new FWBS.OMS.UI.Windows.eNumberSelector();
            this.txtMobile = new FWBS.OMS.UI.Windows.eNumberSelector();
            this.txtEmail = new FWBS.OMS.UI.Windows.eEmailSelector();
            this.txtAddressee = new FWBS.Common.UI.Windows.eTextBox2();
            this.address = new FWBS.OMS.UI.Windows.eAddressSelector();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.eCaptionLine1 = new FWBS.Common.UI.Windows.eCaptionLine();
            this.pnlTitle.SuspendLayout();
            this.border.SuspendLayout();
            this.SuspendLayout();
            // 
            // border
            // 
            this.border.Controls.Add(this.txtTel);
            this.border.Controls.Add(this.txtMobile);
            this.border.Controls.Add(this.txtFax);
            this.border.Controls.Add(this.txtEmail);
            this.border.Controls.Add(this.cboType);
            this.border.Controls.Add(this.btnAdvanced);
            this.border.Controls.Add(this.txtTheirRef);
            this.border.Controls.Add(this.txtSalutation);
            this.border.Controls.Add(this.txtHeading);
            this.border.Controls.Add(this.cboHeading);
            this.border.Controls.Add(this.address);
            this.border.Controls.Add(this.txtAddressee);
            this.border.Controls.Add(this.eCaptionLine1);
            this.border.Size = new System.Drawing.Size(424, 109);
            // 
            // cboType
            // 
            this.cboType.ActiveSearchEnabled = true;
            this.cboType.CaptionWidth = 80;
            this.cboType.IsDirty = false;
            this.cboType.Location = new System.Drawing.Point(8, 8);
            this.res.SetLookup(this.cboType, new FWBS.OMS.UI.Windows.ResourceLookupItem("FORMAT", "Format :", ""));
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(238, 22);
            this.cboType.TabIndex = 1;
            this.cboType.Text = "Format :";
            this.cboType.Changed += new System.EventHandler(this.cboType_Changed);
            // 
            // cboHeading
            // 
            this.cboHeading.ActiveSearchEnabled = true;
            this.cboHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHeading.CaptionWidth = 85;
            this.cboHeading.IsDirty = false;
            this.cboHeading.Location = new System.Drawing.Point(254, 8);
            this.res.SetLookup(this.cboHeading, new FWBS.OMS.UI.Windows.ResourceLookupItem("QUICKHEAD", "Quick Heading :", ""));
            this.cboHeading.Name = "cboHeading";
            this.cboHeading.Size = new System.Drawing.Size(162, 22);
            this.cboHeading.TabIndex = 5;
            this.cboHeading.Text = "Quick Heading :";
            this.cboHeading.ActiveChanged += new System.EventHandler(this.cboFormat_Changed);
            // 
            // txtHeading
            // 
            this.txtHeading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHeading.CaptionWidth = 85;
            this.txtHeading.IsDirty = false;
            this.txtHeading.Location = new System.Drawing.Point(254, 32);
            this.res.SetLookup(this.txtHeading, new FWBS.OMS.UI.Windows.ResourceLookupItem("HEADING", "Heading :", ""));
            this.txtHeading.MaxLength = 255;
            this.txtHeading.Name = "txtHeading";
            this.txtHeading.Size = new System.Drawing.Size(162, 46);
            this.txtHeading.TabIndex = 6;
            this.txtHeading.TabStop = true;
            this.txtHeading.Text = "Heading :";
            // 
            // txtSalutation
            // 
            this.txtSalutation.CaptionWidth = 80;
            this.txtSalutation.IsDirty = false;
            this.txtSalutation.Location = new System.Drawing.Point(8, 32);
            this.res.SetLookup(this.txtSalutation, new FWBS.OMS.UI.Windows.ResourceLookupItem("SALUTATION", "Salutation :", ""));
            this.txtSalutation.MaxLength = 35;
            this.txtSalutation.Name = "txtSalutation";
            this.txtSalutation.Size = new System.Drawing.Size(238, 22);
            this.txtSalutation.TabIndex = 2;
            this.txtSalutation.TabStop = true;
            this.txtSalutation.Text = "Salutation :";
            // 
            // txtTheirRef
            // 
            this.txtTheirRef.CaptionWidth = 80;
            this.txtTheirRef.IsDirty = false;
            this.txtTheirRef.Location = new System.Drawing.Point(8, 56);
            this.res.SetLookup(this.txtTheirRef, new FWBS.OMS.UI.Windows.ResourceLookupItem("THEIRREF", "Their Ref :", ""));
            this.txtTheirRef.MaxLength = 50;
            this.txtTheirRef.Name = "txtTheirRef";
            this.txtTheirRef.Size = new System.Drawing.Size(238, 22);
            this.txtTheirRef.TabIndex = 3;
            this.txtTheirRef.TabStop = true;
            this.txtTheirRef.Text = "Their Ref :";
            // 
            // txtTel
            // 
            this.txtTel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTel.CaptionWidth = 80;
            this.txtTel.EditLinkVisible = false;
            this.txtTel.FindLinkVisible = false;
            this.txtTel.IsDirty = false;
            this.txtTel.Location = new System.Drawing.Point(256, 200);
            this.res.SetLookup(this.txtTel, new FWBS.OMS.UI.Windows.ResourceLookupItem("TELEPHONE", "Telephone :", ""));
            this.txtTel.Name = "txtTel";
            this.txtTel.NumberLocation = "";
            this.txtTel.omsDesignMode = false;
            this.txtTel.Size = new System.Drawing.Size(160, 22);
            this.txtTel.TabIndex = 10;
            this.txtTel.Value = ((object)(resources.GetObject("txtTel.Value")));
            // 
            // txtFax
            // 
            this.txtFax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFax.CaptionWidth = 80;
            this.txtFax.EditLinkVisible = false;
            this.txtFax.FindLinkVisible = false;
            this.txtFax.IsDirty = false;
            this.txtFax.Location = new System.Drawing.Point(256, 248);
            this.res.SetLookup(this.txtFax, new FWBS.OMS.UI.Windows.ResourceLookupItem("FAX", "Fax :", ""));
            this.txtFax.Name = "txtFax";
            this.txtFax.NumberLocation = "";
            this.txtFax.NumberType = "FAX";
            this.txtFax.omsDesignMode = false;
            this.txtFax.Size = new System.Drawing.Size(160, 22);
            this.txtFax.TabIndex = 12;
            this.txtFax.Value = ((object)(resources.GetObject("txtFax.Value")));
            // 
            // txtMobile
            // 
            this.txtMobile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMobile.CaptionWidth = 80;
            this.txtMobile.EditLinkVisible = false;
            this.txtMobile.FindLinkVisible = false;
            this.txtMobile.IsDirty = false;
            this.txtMobile.Location = new System.Drawing.Point(256, 224);
            this.res.SetLookup(this.txtMobile, new FWBS.OMS.UI.Windows.ResourceLookupItem("MOBILE", "Mobile :", ""));
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.NumberLocation = "";
            this.txtMobile.NumberType = "MOBILE";
            this.txtMobile.omsDesignMode = false;
            this.txtMobile.Size = new System.Drawing.Size(160, 22);
            this.txtMobile.TabIndex = 11;
            this.txtMobile.Value = ((object)(resources.GetObject("txtMobile.Value")));
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.CaptionWidth = 80;
            this.txtEmail.EditLinkVisible = false;
            this.txtEmail.EmailLocation = "";
            this.txtEmail.FindLinkVisible = false;
            this.txtEmail.IsDirty = false;
            this.txtEmail.Location = new System.Drawing.Point(256, 176);
            this.res.SetLookup(this.txtEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("EMAIL", "Email :", ""));
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.omsDesignMode = false;
            this.txtEmail.Size = new System.Drawing.Size(160, 22);
            this.txtEmail.TabIndex = 9;
            this.txtEmail.Value = ((object)(resources.GetObject("txtEmail.Value")));
            // 
            // txtAddressee
            // 
            this.txtAddressee.CaptionWidth = 80;
            this.txtAddressee.IsDirty = false;
            this.txtAddressee.Location = new System.Drawing.Point(8, 80);
            this.res.SetLookup(this.txtAddressee, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDRESSEE", "Addressee :", ""));
            this.txtAddressee.MaxLength = 50;
            this.txtAddressee.Name = "txtAddressee";
            this.txtAddressee.Size = new System.Drawing.Size(238, 22);
            this.txtAddressee.TabIndex = 4;
            this.txtAddressee.TabStop = true;
            this.txtAddressee.Text = "Addressee :";
            // 
            // address
            // 
            this.address.CaptionWidth = 80;
            this.address.EditLinkVisible = false;
            this.address.FindLinkVisible = false;
            this.address.IsDirty = false;
            this.address.Location = new System.Drawing.Point(8, 136);
            this.res.SetLookup(this.address, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDRESS", "Address :", ""));
            this.address.Name = "address";
            this.address.omsDesignMode = false;
            this.address.Size = new System.Drawing.Size(272, 133);
            this.address.TabIndex = 8;
            this.address.UseValueAsAddress = true;
            this.address.Value = null;
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdvanced.Location = new System.Drawing.Point(344, 80);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(72, 23);
            this.btnAdvanced.TabIndex = 7;
            this.btnAdvanced.Text = ">>>";
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            // 
            // eCaptionLine1
            // 
            this.eCaptionLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eCaptionLine1.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine1.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine1.Location = new System.Drawing.Point(8, 112);
            this.eCaptionLine1.Name = "eCaptionLine1";
            this.eCaptionLine1.Size = new System.Drawing.Size(408, 17);
            this.eCaptionLine1.TabIndex = 9999;
            this.eCaptionLine1.Text = "Advanced";
            // 
            // ucAssociateSelector
            // 
            this.Name = "ucAssociateSelector";
            this.Size = new System.Drawing.Size(424, 136);
            this.RightToLeftChanged += new System.EventHandler(this.ucContactSelector_RightToLeftChanged);
            this.pnlTitle.ResumeLayout(false);
            this.border.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion


		#endregion

		#region Methods

		/// <summary>
		/// Captures any change in the right to left visual format.
		/// </summary>
		/// <param name="sender">The currentcontrol instance.</param>
		/// <param name="e">Empt event arguments.</param>
		private void ucContactSelector_RightToLeftChanged(object sender, System.EventArgs e)
		{
			Global.RightToLeftControlConverter(this, ParentForm);
		}


		#endregion

		#region ISelectorRepeater Implementation


		/// <summary>
		/// Sets dynaminc parameters to the derived control.
		/// </summary>
		/// <param name="parameters">Parameters to be passed.</param>
		public override void SetInfo (object [] parameters)
		{
			foreach (object par in parameters)
			{
				if (par is Client)
				{
					_client = (Client)par;
				}
				else if (par is OMSFile)
				{
					_file = (OMSFile)par;
				}
			}
		}

		/// <summary>
		/// Checks to see if this type of selector control supports certain methods.
		/// </summary>
		/// <param name="methodType">Method type to check for.</param>
		/// <returns>A true / false value.</returns>
		public override bool HasMethod(SelectorRepeaterMethods methodType)
		{
			switch (methodType)
			{
				case SelectorRepeaterMethods.Assign:
					break;
				case SelectorRepeaterMethods.Revoke:
					break;
				case SelectorRepeaterMethods.Find:
					break;
			}
			return false;
		}

		/// <summary>
		/// Runs the specific type of method.
		/// </summary>
		/// <param name="methodType">>Method type to check for.</param>
		public override void RunMethod(SelectorRepeaterMethods methodType)
		{
			switch (methodType)
			{
				case SelectorRepeaterMethods.New:
				{
					Associate assoc = FWBS.OMS.UI.Windows.Services.Wizards.CreateAssociate(_file,null,true);
					if (assoc != null)
					{
						assoc.Contact.Update();
                        Object = assoc.Contact; 
						_associate = assoc;
						cboType.Value = _associate.AssocType;
						cboHeading.Value = _associate.AssocHeading;
						txtSalutation.Value = _associate.Salutation;
						txtTheirRef.Value = _associate.TheirRef;
						txtAddressee.Value = _associate.Addressee;
						txtHeading.Value = _associate.AssocHeading;
						address.Reload(assoc.Contact,_associate.DefaultAddress);
						txtEmail.Reload(assoc.Contact, _associate.DefaultEmail);
						txtFax.Reload(assoc.Contact, _associate.DefaultFaxNumber);
						txtTel.Reload(assoc.Contact, _associate.DefaultTelephoneNumber);
						txtMobile.Reload(assoc.Contact, _associate.DefaultMobile);
					}
					else
					{
						Object = null;
					}
				}
				break;
			}
			
		}

		/// <summary>
		/// Gets or sets the current contact object for this control.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public override object Object
		{
			get
			{
				if (_associate != null) 
				{
					_associate.AssocType = Convert.ToString(cboType.Value);
					_associate.AssocHeading = Convert.ToString(cboHeading.Value);
					_associate.Salutation = Convert.ToString(txtSalutation.Value);
					_associate.TheirRef = Convert.ToString(txtTheirRef.Value);
					_associate.Addressee = Convert.ToString(txtAddressee.Value);
					_associate.AssocHeading = Convert.ToString(txtHeading.Value);
					_associate.DefaultAddress = address.Value as Address;
					_associate.DefaultEmail = Convert.ToString(txtEmail.Value);
					_associate.DefaultFaxNumber = Convert.ToString(txtFax.Value);
					_associate.DefaultTelephoneNumber = Convert.ToString(txtTel.Value);
					_associate.DefaultMobile = Convert.ToString(txtMobile.Value);
				}
				return _associate;
			}
			set
			{
				if (value is Contact)
					_associate = new Associate((Contact)value, _file, "");
				else
					_associate = value as Associate;
				
				SetAppearance();

			}
		}

	
		/// <summary>
		/// Changes the associate repeaters caption.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboType_Changed(object sender, System.EventArgs e)
		{
            string text = (Session.CurrentSession.Resources?.GetResource("ASSOCIATE", "Associate", "").Text ?? "Associate") + " - " + Convert.ToString(cboType.DisplayValue);
            if (_associate != null)
            {
                text += " - " + _associate.Contact.Name;
            }
			this.Text = text;
		}

		/// <summary>
		/// Adds a quick heading to the full heading box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboFormat_Changed(object sender, System.EventArgs e)
		{
            string quickHeading = Convert.ToString(cboHeading.Value);
            if (quickHeading.Length > 0)
            {
                string heading = Convert.ToString(txtHeading.Value);
                if (heading.Length == 0)
                {
                    txtHeading.Value = quickHeading;
                }
                else
                {
                    string[] headings = heading.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (!headings.Any(h => h.StartsWith(quickHeading.TrimEnd(), StringComparison.CurrentCultureIgnoreCase)))
                    {
                        heading += (Environment.NewLine + quickHeading);
                        if (heading.Length <= 255)
                        {
                            txtHeading.Value = heading;
                        }
                    }
                }
            }
		}

		#endregion

		private void btnAdvanced_Click(object sender, System.EventArgs e)
		{
			if (btnAdvanced.Tag == null || Convert.ToString(btnAdvanced.Tag) == ">>>")
			{
				btnAdvanced.Text = "<<<";
				this.Height = MaxHeight;
				btnAdvanced.Tag = "<<<";
			}
			else
			{
				btnAdvanced.Text = ">>>";
                this.Height = MinHeight;
				btnAdvanced.Tag = ">>>";
			}

		}

		/// <summary>
		/// Sets the appearance of the  object, or empties it.
		/// </summary>
		private void SetAppearance()
		{
			this.Height = MinHeight;
			cboType.Enabled = true;
			if (_associate == null)
			{
				cboType_Changed(cboType, EventArgs.Empty);
				cboHeading.Visible = false;
				txtHeading.Visible = false;
				txtSalutation.Visible = false;
				txtAddressee.Visible = false;
				txtTheirRef.Visible = false;
				txtTel.Visible = false;
				txtFax.Visible = false;
				txtMobile.Visible = false;
				txtEmail.Visible = false;
				address.Visible = false;

				btnAdvanced.Tag = "<<<";
				btnAdvanced_Click(btnAdvanced, EventArgs.Empty);
				btnAdvanced.Visible = false;

			}
			else
			{
				cboHeading.Visible = true;
				txtHeading.Visible = true;
				txtSalutation.Visible = true;
				txtAddressee.Visible = true;
				txtTheirRef.Visible = true;
				txtTel.Visible = true;
				txtFax.Visible = true;
				txtMobile.Visible = true;
				txtEmail.Visible = true;
				address.Visible = true;
				btnAdvanced.Visible = true;

				Contact cont = _associate.Contact;

				cboType.AddItem(_associate.GetAssociateTypes(false), "typecode", "typedesc");

                if (_associate.IsClient && _associate.AssocType == "")
					cboType.SelectedValue = "CLIENT";
				else
					cboType.SelectedValue = _associate.AssocType;

				cboType_Changed(cboType, EventArgs.Empty);
				cboHeading.ActiveChanged -= new EventHandler(cboFormat_Changed);
				cboHeading.AddItem(_file.GetAssociateHeadings(true), "fmtDesc", "fmtDesc");
				cboHeading.ActiveChanged += new EventHandler(cboFormat_Changed);

				txtSalutation.Value = _associate.Salutation;
				txtAddressee.Value = _associate.Addressee;
				txtTheirRef.Value = _associate.TheirRef;
				txtHeading.Value = _associate.AssocHeading;

				address.Reload(_associate);
				txtEmail.Reload(cont, cont.DefaultEmail);
				txtTel.Reload(cont, cont.DefaultTelephoneNumber);
				txtFax.Reload(cont, cont.DefaultFaxNumber);
				txtMobile.Reload(cont, cont.DefaultMobileNumber);
			}
		}

	}
}
