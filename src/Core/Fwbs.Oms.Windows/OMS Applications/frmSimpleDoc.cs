using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A document which displays a simple document form.
    /// </summary>
    internal class frmSimpleDoc : frmNewBrandIdent
	{
		#region Fields

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// The enquiry form to be used.
		/// </summary>
		private EnquiryForm enquiryForm1;
		
		/// <summary>
		/// The parent application.
		/// </summary>
		private SimpleOMS _app = null;

		/// <summary>
		/// A flag that enables or disables editing the document.
		/// </summary>
		private bool _readonly = false;

		/// <summary>
		/// This form will also deal with an actual SMS object.
		/// </summary>
		private SimpleDoc _doc = null;

		private Common.UI.IBasicEnquiryControl2 _text = null;
		private Common.UI.IBasicEnquiryControl2 _assoc = null;
		private Common.UI.IBasicEnquiryControl2 _signOff = null;
		private Common.UI.IBasicEnquiryControl2 _fileRef = null;
		private Control _counter = null;
		private Common.UI.IBasicEnquiryControl2 _number = null;
		private Button _save = null;
		private Button _cancel = null;
        private EnquiryEngine.Enquiry engine;
		#endregion
		
		#region Constructors & Destructors 

		private frmSimpleDoc()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            SetIcon(Images.DialogIcons.EliteApp);
		}

		public frmSimpleDoc(SMS sms, SimpleOMS app, bool readOnly) : this()
		{
			_doc = sms;
			_app = app;
			_readonly = readOnly;
			engine = EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.SMS), sms.Associate.OMSFile, EnquiryEngine.EnquiryMode.Edit, true, null);
		}

		public frmSimpleDoc(SMS sms, Associate assoc, SimpleOMS app) : this()
		{
			_doc = sms;
			_app = app;
			sms.Associate = assoc;
			sms.Number = assoc.DefaultMobile;
			engine = EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.SMS), assoc.OMSFile, EnquiryEngine.EnquiryMode.Edit, true, null);
		}

		public frmSimpleDoc(SimpleDoc document, string code, SimpleOMS app, bool readOnly) : this()
		{
			_doc = document;
			_app = app;
			_readonly = readOnly;

			object parent = OMSFile.GetFile(_app.GetDocVariable(this, OMSApp.FILE, 0));
			engine = EnquiryEngine.Enquiry.GetEnquiry(code, parent, EnquiryEngine.EnquiryMode.Edit, true, null);
		}

		public frmSimpleDoc(SimpleDoc document, Associate assoc, string code, SimpleOMS app) : this()
		{
			_doc = document;
			_app = app;

			ActiveDocument.SetExtraInfo("_assoc", assoc.ID);
			engine = EnquiryEngine.Enquiry.GetEnquiry(code, assoc.OMSFile, EnquiryEngine.EnquiryMode.Edit, true, null);
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
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.SuspendLayout();
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(0, 0);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(292, 266);
            this.enquiryForm1.TabIndex = 0;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.Rendered += new System.EventHandler(this.enquiryForm1_Rendered);
            // 
            // frmSimpleDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.enquiryForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSimpleDoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSimpleDoc";
            this.Activated += new System.EventHandler(this.frmSimpleDoc_Activated);
            this.Load += new System.EventHandler(this.frmSimpleDoc_Load);
            this.Shown += new System.EventHandler(this.frmSimpleDoc_Shown);
            this.ResumeLayout(false);

		}

        void frmSimpleDoc_Load(object sender, EventArgs e)
        {
            enquiryForm1.Enquiry = engine;
        }
		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets the active document of the form.
		/// </summary>
		public SimpleDoc ActiveDocument
		{
			get
			{
				return _doc;
			}
		}

		/// <summary>
		/// Gets the preview text of the form.
		/// </summary>
		public string DisplayText
		{
			get
			{
				return ActiveDocument.Text;
			}
			set
			{
				ActiveDocument.Text = value;
				if (_text != null)
					_text.Value = value;
			}
		}

		#endregion

		#region Methods

		public object GetText(string name)
		{
			return ActiveDocument.GetExtraInfo(name, "");
		}

		public void SetText(string name, object val)
		{
			ActiveDocument.SetExtraInfo(name, val);
			FWBS.Common.UI.IBasicEnquiryControl2 ctrl = enquiryForm1.GetIBasicEnquiryControl2(name);
			if (ctrl != null)
				ctrl.Value = val;
			_text_Changed(_text, EventArgs.Empty);
		}


		public void RefreshXML()
		{
			System.Data.DataTable ret = enquiryForm1.Enquiry.Object as System.Data.DataTable;
			if (ret != null & ret.Rows.Count > 0)
			{
				foreach (System.Data.DataColumn col in ret.Columns)
				{
					string val = Convert.ToString(ret.Rows[0][col]);
					if (val != "")
					{
						ActiveDocument.SetExtraInfo(col.ColumnName, val); 
					}
				}
			}

			if (_app != null)
			{
				if (_assoc != null) _app.SetDocVariable(this, OMSApp.ASSOCIATE, Convert.ToInt64(_assoc.Value));
			}
		}

		#endregion

		#region Captured Events

		private void frmSimpleDoc_Activated(object sender, System.EventArgs e)
		{
			if (_app != null) _app.ActiveForm = this;
		}

		private void _save_Click(object sender, System.EventArgs e)
		{
			try
			{
                if (TopMost)
                    TopMost = false;


				Cursor = Cursors.WaitCursor;

				if (_readonly == false)
				{
					enquiryForm1.UpdateItem();

					if (ActiveDocument is SMS)
					{
						int total = Convert.ToString(_text.Value).Length;
						int pgecount = (total / SMS.MaxLength) + 1;
						if (total > SMS.MaxLength)
						{
							MessageBox msg = new MessageBox(Session.CurrentSession.Resources.GetMessage("LARGESMS", "The SMS message is too large to fit on one message. " + Environment.NewLine + "Currently, '%1%' messages will have to be sent." + Environment.NewLine + "You may want to add extra time recording entries to cover the extra cost.", "", pgecount.ToString()));
							msg.Buttons = new string[3]{"TRUNCATE", "SEND", "CANCEL"}; 
							msg.CancelButton = "CANCEL";
							msg.DefaultButton = "SEND"; 
							msg.Icon = MessageBoxIcon.Warning;
							switch (msg.Show(this))
							{
								case "TRUNCATE":
									_text.Value = Convert.ToString(_text.Value).Substring(0, SMS.MaxLength);
									break;
								case "SEND":
									break;
								case "CANCEL":
									return;
								default:
									goto case "CANCEL";
							}
						}

						//Make sure that any added or changed numbers are saved.
						Associate assoc = Associate.GetAssociate(_app.GetDocVariable(this, OMSApp.ASSOCIATE, -1));
						assoc.Update();
					}
					
					RefreshXML();

					if (_app != null)
					{
						if (_assoc != null) _app.SetDocVariable(this, OMSApp.ASSOCIATE, Convert.ToInt64(_assoc.Value));
						_app.Save(this);
					}
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		
		private void _cancel_Click(object sender, System.EventArgs e)
		{
            if (TopMost)
                TopMost = false;

			this.Close();
		}

		private void _assoc_Changed(object sender, System.EventArgs e)
		{
			try
			{
				if (_number != null && _assoc != null )
				{
					if (_assoc.Value != DBNull.Value)
					{
						Associate assoc = Associate.GetAssociate(Convert.ToInt64(_assoc.Value));
						if (_number is eNumberSelector)
						{
							eNumberSelector num = (eNumberSelector)_number;
							num.NumberType = "MOBILE";
							num.Reload(assoc.Contact, assoc.DefaultMobile);
						}
						_number.Value = assoc.DefaultMobile;
						_number.OnChanged();

						ActiveDocument.SetExtraInfo ("_assoc", Convert.ToString(assoc.ID));
						if (_doc is SMS) ((SMS)ActiveDocument).Number =  Convert.ToString(_number.Value);
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex);
			}

		}

		private void _text_Changed(object sender, EventArgs e)
		{
			if (_counter != null && _text != null) 
			{
				int ch = 0;
				int pge = 0;
				int total = 0;
				total = Convert.ToString(_text.Value).Length;
				ch = total % SMS.MaxLength;
				pge = (total / SMS.MaxLength) + 1;
				_counter.Text = (SMS.MaxLength -  ch).ToString() + "/" + pge.ToString() + " (" + total.ToString() + ")";
			}
			ActiveDocument.Text = Convert.ToString(_text.Value);
		}

		private void _signOff_Changed(object sender, EventArgs e)
		{
			if (_text != null)
			{
				string text = Convert.ToString(_text.Value);
				string signoff = " - " + Session.CurrentSession.CurrentFeeEarner.SignOff  + " / " + Session.CurrentSession.CompanyName;
				if (Common.ConvertDef.ToBoolean(_signOff.Value, false))
					text += signoff;
				else
					text = text.Replace(signoff, "");

				_text.Value = text;
				_text_Changed(_text, EventArgs.Empty);
			}
		}

		private void _fileRef_Changed(object sender, EventArgs e)
		{
			
			if (_text != null)
			{
				string text = Convert.ToString(_text.Value);
				string fileRef =  Client.GetClient(_app.GetDocVariable(this, OMSApp.CLIENT, -1)).ClientNo + "/" + OMSFile.GetFile(_app.GetDocVariable(this, OMSApp.FILE, -1)).FileNo + ": ";
				if (Common.ConvertDef.ToBoolean(_fileRef.Value, false))
					text =   fileRef + text;
				else
					text = text.Replace(fileRef, "");

				_text.Value = text;
				_text_Changed(_text, EventArgs.Empty);
			}
			
		}

		private void enquiryForm1_Rendered(object sender, System.EventArgs e)
		{
            _text = enquiryForm1.GetIBasicEnquiryControl2("_text");
			_assoc = enquiryForm1.GetIBasicEnquiryControl2("_assoc");
			_save = enquiryForm1.GetControl("_save") as Button;
			_cancel = enquiryForm1.GetControl("_cancel") as Button;
			_number = enquiryForm1.GetIBasicEnquiryControl2("_number");
			_signOff = enquiryForm1.GetIBasicEnquiryControl2("_signOff");
			_fileRef = enquiryForm1.GetIBasicEnquiryControl2("_fileRef");
			_counter = enquiryForm1.GetControl("_counter");
			
			if (_save != null)
			{
				_save.Click -= new EventHandler(_save_Click);
				_save.Click += new EventHandler(_save_Click);
			}
			if (_cancel != null)
			{
				_cancel.Click -= new EventHandler(_cancel_Click);
				_cancel.Click += new EventHandler(_cancel_Click);
			}
			if (_assoc != null)
			{
				_assoc.ActiveChanged -= new EventHandler(_assoc_Changed);
				_assoc.ActiveChanged += new EventHandler(_assoc_Changed);
			}
			if (_counter != null && _text != null)
			{
				_text.ActiveChanged -=new EventHandler(_text_Changed);
				_text.ActiveChanged +=new EventHandler(_text_Changed);
			}
			if (_signOff != null)
			{
				_signOff.ActiveChanged -=new EventHandler(_signOff_Changed);
				_signOff.ActiveChanged +=new EventHandler(_signOff_Changed);
			}
			if (_fileRef != null)
			{
				_fileRef.ActiveChanged -=new EventHandler(_fileRef_Changed);
				_fileRef.ActiveChanged +=new EventHandler(_fileRef_Changed);
			}



			foreach (System.Xml.XmlElement el in ActiveDocument.Xml.SelectSingleNode("/DOCUMENT/FIELDS").ChildNodes)
			{
				System.Xml.XmlAttribute attr = el.Attributes[0];
				FWBS.Common.UI.IBasicEnquiryControl2 ctrl = enquiryForm1.GetIBasicEnquiryControl2(attr.Name);
				if (ctrl != null)
				{
					if (attr.Name == "_assoc"  )
					{
						if (ctrl is FWBS.Common.UI.IListEnquiryControl) 
						{
							ctrl.Value = Associate.GetAssociate(Common.ConvertDef.ToInt64(attr.Value, -1)).ID;
							_assoc_Changed(_assoc, EventArgs.Empty);
						}
						else
						{
							_assoc.Changed -= new EventHandler(_assoc_Changed);
							ctrl.Value = Associate.GetAssociate(Common.ConvertDef.ToInt64(attr.Value, -1)).Contact.Name;
						}
					}
					else
						ctrl.Value = attr.Value;
					
				}
			}
			
			_text_Changed(_text, EventArgs.Empty);

			enquiryForm1.SetParentSize();

			
			this.Text = _app.GetWindowCaption(this);
			if (_readonly)
				this.Text = this.Text + " - (" + Session.CurrentSession.Resources.GetResource("READONLY", "Read Only", "").Text + ")";

			this.TopLevel = true;
		}

        private void frmSimpleDoc_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            enquiryForm1.Focus();
            this.Activate();
        }

		#endregion

	}
}
