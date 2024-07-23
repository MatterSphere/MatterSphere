using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A form that auto renders a date wizard object.
    /// </summary>
    public class DateWizardForm : FormRendererBase
	{
		#region Fields

		/// <summary>
		/// The date wizard to render.
		/// </summary>
		private DateWizard _datewiz = null;

		/// <summary>
		/// The y co-ordinate for the automatically generated controls.
		/// </summary>
		private int ycoord = 0;

		#endregion
		
		#region Control Fields

		/// <summary>
		/// Specified enquiry form.
		/// </summary>
		private EnquiryForm _form;

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DateWizardForm()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._form = new FWBS.OMS.UI.Windows.EnquiryForm();
			((System.ComponentModel.ISupportInitialize)(this._lists)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._questions)).BeginInit();
			this.SuspendLayout();
			// 
			// _form
			// 
			this._form.AutoScroll = true;
			this._form.Dock = System.Windows.Forms.DockStyle.Fill;
			this._form.IsDirty = false;
			this._form.Location = new System.Drawing.Point(0, 0);
			this._form.Name = "_form";
			this._form.Size = new System.Drawing.Size(248, 168);
			this._form.TabIndex = 0;
			this._form.ToBeRefreshed = false;
			// 
			// DateWizardForm
			// 
			this.Controls.Add(this._form);
			this.Name = "DateWizardForm";
			this.Size = new System.Drawing.Size(248, 168);
			((System.ComponentModel.ISupportInitialize)(this._lists)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._questions)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Rendering Methods

		/// <summary>
		/// Overriden render control method from the base rendering form to apply 
		/// extra property settings per question asked.
		/// </summary>
		/// <param name="ctrl">Reference to the newly created / or existing control.</param>
		/// <param name="settings">Settings data row that stores the information.</param>
		public override void RenderControl(ref Control ctrl, DataRow settings)
		{

			base.RenderControl(ref ctrl, settings);
					
			if (RightToLeft == RightToLeft.Yes)
				ctrl.Location = new Point(Width - ctrl.Width, ycoord);
			else
				ctrl.Location = new Point(0, ycoord);

			if (ctrl is IBasicEnquiryControl2)
			{
				((IBasicEnquiryControl2)ctrl).ActiveChanged -= new EventHandler(DateWizardForm_Changed);
				((IBasicEnquiryControl2)ctrl).ActiveChanged += new EventHandler(DateWizardForm_Changed);
			}
			ycoord += ctrl.Height;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates the date wizard form and persists valid data into the database.
		/// </summary>
		public void UpdateItem()
		{
			if (_datewiz != null)
			{
				if (_datewiz.HasEnquiryForm)
				{
					if (_form != null)
						_form.UpdateItem();
				}
			
				try
				{
					_datewiz.Update();
				}
				catch(EnquiryEngine.EnquiryValidationFieldException ex)
				{
					Control ctrl = null;
					ctrl = GetControl(ex.ValidatedField.FieldName);

					if (this.RightToLeft == RightToLeft.Yes)
					{
						err.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleLeft);
						req.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleLeft);
					}
					else
					{
						err.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleRight);
						req.SetIconAlignment(ctrl, ErrorIconAlignment.MiddleRight);
					}

					err.SetError(ctrl,ex.Message);
					req.SetError(ctrl, "");
					ctrl.Focus();
	
					throw ex;
	
				}
			}
		}

        internal EnquiryForm EnquiryForm
        {
            get
            {
                return _form;
            }
        }

        /// <summary>
        /// Creates key dates notes for <see cref="DateWizard"/>.
        /// </summary>
        /// <param name="additionalNotes">Additional notes for the dates.</param>
        public void CreateWizardNotes(string additionalNotes)
        {
            _datewiz.CreateNotes(additionalNotes);
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the date wizard object that is associated with the form.
		/// </summary>
		public DateWizard DateWizard
		{
			get
			{
				return _datewiz;
			}
			set
			{
	
				this.Controls.Clear();
				EnquiryControls.Clear();
				if (value == null && _datewiz != null)
				{	
					_form.Enquiry = null;
				}
                
				_datewiz = value;

				if (_datewiz != null)
				{
					ycoord = 0;

					if (_datewiz.HasEnquiryForm)
					{
						this.Controls.Clear();
						this.Controls.Add(_form);
						_form.Visible = true;
						_form.Enquiry = _datewiz.Enquiry;
					}
					else
					{
						_form.Visible = false;
						_form.Enquiry = null;
						base._questions = _datewiz.GetQUESTIONSTable();
						base.RenderControls(true);
						_datewiz.GetDATATable().ColumnChanged +=new DataColumnChangeEventHandler(DateWizardForm_ColumnChanged);
					}
				}
			}
		}

		#endregion

		#region Captured Events

		/// <summary>
		/// Captures each of the changed events of the date controls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DateWizardForm_Changed(object sender, EventArgs e)
		{
			if (_datewiz.HasEnquiryForm == false)
			{
				IBasicEnquiryControl2 bas = (IBasicEnquiryControl2)sender;
				Control ctrl = (Control)sender;
				DataTable dt = _datewiz.GetDATATable();
				dt.Rows[0][ctrl.Name] = bas.Value;
			}
		}

		#endregion

		private void DateWizardForm_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			IBasicEnquiryControl2 bas = GetIBasicEnquiryControl2(e.Column.ColumnName);
			bas.ActiveChanged -= new EventHandler(DateWizardForm_Changed);
			if (bas != null)
				bas.Value = e.ProposedValue;
			bas.ActiveChanged += new EventHandler(DateWizardForm_Changed);
		}
	}
}
