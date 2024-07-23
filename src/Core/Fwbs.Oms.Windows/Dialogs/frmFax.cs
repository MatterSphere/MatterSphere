using System;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmFax.
    /// </summary>
    internal class frmFax : BaseForm
	{
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.PictureBox picFAX;
		private FWBS.OMS.UI.Windows.EnquiryForm enqfax;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.ComponentModel.IContainer components;
		private FWBS.OMS.Associate _assoc = null;

		private string _faxto ;
		private string _faxcompname ;
		private string _faxtime ;
		protected FWBS.OMS.UI.Windows.Accelerators accelerators1;
        private Panel pnlBack;
		private string _faxnumber ;

		/// <summary>
		/// Form Fax will display a fax fillin form with all of the information
		/// to store and send a fax.
		/// </summary>
		/// <param name="assocobj">Passing the Associate will then be used to get the fax/tel lists</param>
		public frmFax(FWBS.OMS.Associate assocobj) : base()
		{
			_assoc = assocobj;
			InitializeComponent();
			enqfax.Enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.FaxTransmission),null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
		}

		/// <summary>
		/// Form Fax will display a fax fillin form with all of the information
		/// to store and send a fax.
		/// </summary>
		public frmFax() : base()
		{
			InitializeComponent();
			enqfax.Enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.FaxTransmissionNoAssociate),null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picFAX = new System.Windows.Forms.PictureBox();
            this.enqfax = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.pnlBack = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picFAX)).BeginInit();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(432, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 47;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(432, 34);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "Cance&l";
            // 
            // picFAX
            // 
            this.picFAX.Location = new System.Drawing.Point(4, 4);
            this.picFAX.Name = "picFAX";
            this.picFAX.Size = new System.Drawing.Size(48, 48);
            this.picFAX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFAX.TabIndex = 45;
            this.picFAX.TabStop = false;
            // 
            // enqfax
            // 
            this.enqfax.AutoScroll = true;
            this.enqfax.IsDirty = false;
            this.enqfax.Location = new System.Drawing.Point(60, 4);
            this.enqfax.Name = "enqfax";
            this.enqfax.Size = new System.Drawing.Size(365, 128);
            this.enqfax.TabIndex = 46;
            this.enqfax.ToBeRefreshed = false;
            // 
            // accelerators1
            // 
            this.accelerators1.Active = false;
            this.accelerators1.Form = this;
            this.accelerators1.WaitInterval = 500;
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.picFAX);
            this.pnlBack.Controls.Add(this.enqfax);
            this.pnlBack.Controls.Add(this.btnOK);
            this.pnlBack.Controls.Add(this.btnCancel);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBack.Location = new System.Drawing.Point(0, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(514, 137);
            this.pnlBack.TabIndex = 49;
            // 
            // frmFax
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(514, 137);
            this.Controls.Add(this.pnlBack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("FAXFORM", "Fax Options...", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFax";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fax Options...";
            this.Load += new System.EventHandler(this.frmFax_Load);
            this.Shown += new System.EventHandler(this.frmFax_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picFAX)).EndInit();
            this.pnlBack.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmFax_Load(object sender, System.EventArgs e)
		{
			picFAX.Image = FWBS.OMS.UI.Windows.Images.Fax;
			// Check the FaxTo internal variable and update form
			UpdateEnquiryForm();

		}

		public void UpdateEnquiryForm()
		{
            FWBS.Common.UI.IBasicEnquiryControl2 ctrlto = enqfax.GetIBasicEnquiryControl2("FAXTO");
            FWBS.Common.UI.IBasicEnquiryControl2 ctrlcompname = enqfax.GetIBasicEnquiryControl2("FAXCOMPNAME");
            FWBS.Common.UI.IBasicEnquiryControl2 ctrltime = enqfax.GetIBasicEnquiryControl2("FAXTIME");
            FWBS.Common.UI.IBasicEnquiryControl2 ctrlnumber = enqfax.GetIBasicEnquiryControl2("FAXNUMBER");


			if (ctrlto != null)
				ctrlto.Value = _faxto;

			if (ctrlcompname!= null)
			    ctrlcompname.Value = _faxcompname;
			
            if (ctrltime != null)
    			ctrltime.Value = _faxtime;

			if (ctrlnumber != null)
			    ctrlnumber.Value = _faxnumber;

            ctrlnumber.ActiveChanged += new EventHandler(fax_ActiveChanged);

			enqfax.ReBind();
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				enqfax.UpdateItem();

                if (_assoc != null)
                {
                    _assoc.Contact.Update();
                    _assoc.Update();
                }

                this.DialogResult = DialogResult.OK;
			}
			catch
			{
			}
		}


		#region Properties

		public string FaxTo
		{
			get
			{
				if (enqfax.GetControl("FAXTO") != null)
				{
					return Convert.ToString(((FWBS.Common.UI.IBasicEnquiryControl2)enqfax.GetControl("FAXTO")).Value);
				}
                return _faxto;
			}
			set
			{
				_faxto = value;
			}
		}

		public string FaxCompName
		{
			get
			{
				if (enqfax.GetControl("FAXCOMPNAME") != null)
				{
					return Convert.ToString(((FWBS.Common.UI.IBasicEnquiryControl2)enqfax.GetControl("FAXCOMPNAME")).Value);
				}
                return _faxcompname;
			}
			set
			{
				_faxcompname = value;
			}
		}

		public string FaxTime
		{
			get
			{
				if (enqfax.GetControl("FAXTIME") != null)
				{
					return Convert.ToString(((FWBS.Common.UI.IBasicEnquiryControl2)enqfax.GetControl("FAXTIME")).Value);
				}
                return _faxtime;
			}
			set
			{
				_faxtime = value;
			}
		}

		public string FaxNumber
		{
			get
			{
				if (enqfax.GetControl("FAXNUMBER") != null)
				{
					return Convert.ToString(((FWBS.Common.UI.IBasicEnquiryControl2)enqfax.GetControl("FAXNUMBER")).Value);
				}
                return _faxnumber;
			}
			set
			{
				_faxnumber = value;
			}
		}

		#endregion

        private void fax_ActiveChanged(object sender, EventArgs e)
        {
            IBasicEnquiryControl2 fax = (IBasicEnquiryControl2)sender;
            btnOK.Enabled = !String.IsNullOrEmpty(Convert.ToString(fax.Value));
        }

        private void frmFax_Shown(object sender, EventArgs e)
        {
            if (_assoc != null) // Associate has been passed then fillin details
            {
                if (enqfax.GetControl("FAXNUMBER") != null)
                {
                    Control ctrl = enqfax.GetControl("FAXNUMBER");
                    if (ctrl is eNumberSelector)
                    {
                        ((eNumberSelector)ctrl).Reload(_assoc.Contact);
                        if (String.IsNullOrEmpty(_assoc.DefaultFaxNumber))
                            ((eNumberSelector)ctrl).Value = _assoc.Contact.DefaultFaxNumber;
                        else
                            ((eNumberSelector)ctrl).Value = _assoc.DefaultFaxNumber;
                    }
                    btnOK.Enabled = !String.IsNullOrEmpty(Convert.ToString(((eNumberSelector)ctrl).Value));
                }
            }
        }
	}
}
