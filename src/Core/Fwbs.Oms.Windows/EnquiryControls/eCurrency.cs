using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A read only enquiry control which generally holds a caption and a eCurrency.
    /// This particular control is a way of displaying read only information.
    /// </summary>
    public class eCurrency : eBase2, IBasicEnquiryControl2
	{

        private class CurrencyTextBox : TextBox
        {
            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                base.SetBoundsCore(x, System.Math.Max(0, y), width, height, specified);
            }
        }

		#region	Fields
		private	double _oldvalue = 0;
		private	bool _update = false;
        private System.Globalization.NumberFormatInfo _currency = NumberFormatInfo.InvariantInfo;
		private TextBox _this = new CurrencyTextBox();
		private FWBS.OMS.Currency _omscurrency = null;
		#endregion
		
		#region	Constructor
		public eCurrency() : base()
		{
			eCurrency_Leave(null,EventArgs.Empty);
			_this.Leave +=new EventHandler(eCurrency_Leave);
			_this.Enter +=new EventHandler(eCurrency_Enter);
			_this.TextChanged += new EventHandler(eCurrency_TextChanged);
			_this.KeyPress += new KeyPressEventHandler(eCurrency_KeyPress);
			_this.TextAlign = HorizontalAlignment.Right;
			_this.AcceptsTab=false;
			this.ParentChanged	+=new EventHandler(_this_ParentChanged);
			_ctrl = _this;
			_this.TabIndex = 0;
			Controls.Add(_this);
		}
		#endregion

		#region	Properties

		/// <summary>
		/// Gets or Sets the readonly state of the eCurrency.  Although a eCurrency is read only anyway, 
		/// this property still toggles the enabled state of the eCurrency rather than the whole custom control.
		/// This will appear greyed out.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public override bool ReadOnly
		{
			get
			{
				return _this.ReadOnly;
			}
			set
			{
				_this.ReadOnly = value;
			}
		}


		/// <summary>
		/// Gets or Sets the value property of the enwuiry control.
		/// </summary>
		[DefaultValue("")]
		public override object Value 
		{
			get
			{
				return _oldvalue;
			}
			set
			{
				_oldvalue = ConvertDef.ToDouble(value,0);
				IsDirty =true;
				eCurrency_Leave(null,EventArgs.Empty);
			}
		}

		[Category("Currency")]
		public string CurrencySymbol
		{
			get
			{
				return _currency.CurrencySymbol;
			}
			set
			{
                if (_currency.CurrencySymbol != value)
				{
                    if (_currency == NumberFormatInfo.InvariantInfo)
                        _currency = new NumberFormatInfo();
                    _currency.CurrencySymbol = value;
					eCurrency_Leave(null,EventArgs.Empty);
				}
			}
		}

		[Category("Currency")]
		public string CurrencyGroupSeparator
		{
			get
			{
				return _currency.CurrencyGroupSeparator;
			}
			set
			{
                if (_currency.CurrencyGroupSeparator != value)
				{
                    if (_currency == NumberFormatInfo.InvariantInfo)
                        _currency = new NumberFormatInfo();
                    _currency.CurrencyGroupSeparator = value;
					eCurrency_Leave(null,EventArgs.Empty);
				}
			}
		}

		[Category("Currency")]
		public int CurrencyDecimalDigits
		{
			get
			{
				return _currency.CurrencyDecimalDigits;
			}
			set
			{
                if (_currency.CurrencyDecimalDigits != value)
				{
                    if (_currency == NumberFormatInfo.InvariantInfo)
                        _currency = new NumberFormatInfo();
                    _currency.CurrencyDecimalDigits = value;
                    _currency.NumberDecimalDigits = value;
					eCurrency_Leave(null,EventArgs.Empty);
				}
			}
		}

		[Category("Currency")]
		public string CurrencyDecimalSeparator
		{
			get
			{
				return _currency.CurrencyDecimalSeparator;
			}
			set
			{
                if (_currency.CurrencyDecimalSeparator != value)
				{
                    if (_currency == NumberFormatInfo.InvariantInfo)
                        _currency = new NumberFormatInfo();
                    _currency.CurrencyDecimalSeparator = value;
					eCurrency_Leave(null,EventArgs.Empty);
				}
			}
		}

		public FWBS.OMS.Currency Currency
		{
			get
			{
				return _omscurrency;
			}
			set
			{
				_omscurrency = value;
				_currency = _omscurrency.CurrencyFormat;
			}
		}

        #endregion

        #region	Private	Methods
        private	void eCurrency_Leave(object	sender,	EventArgs e)
		{
			_update = true;
			_this.Text = String.Format(_currency, "{0:C}", _oldvalue);
			_update = false;
			this.OnChanged();
		}

		private	void eCurrency_Enter(object	sender,	EventArgs e)
		{
			_update = true;
            _this.Text = Convert.ToString(_oldvalue,_currency);
			_update = false;
			((TextBox)_ctrl).SelectAll();
		}

		private	void eCurrency_TextChanged(object sender, EventArgs	e)
		{
			if (_ctrl.Focused && _update == false)
			{
				_oldvalue = ConvertDef.ToDouble(_ctrl.Text,0);
				OnActiveChanged();
			}
		}

		private	void eCurrency_KeyPress(object sender, KeyPressEventArgs e)
		{
            if (e.KeyChar.ToString() == _currency.CurrencyDecimalSeparator && _ctrl.Text.IndexOf(_currency.CurrencyDecimalSeparator) > -1 && _currency.CurrencyDecimalDigits > 0)
				e.Handled=true;
			if (e.KeyChar.ToString() == "-" && (_ctrl.Text.StartsWith("-") || ((TextBox)_ctrl).SelectionStart > 1))
				e.Handled=true;
            else if (("-01234567890" + _currency.CurrencyDecimalSeparator).IndexOf(e.KeyChar.ToString()) == -1 && e.KeyChar != (char)8 && _currency.CurrencyDecimalDigits > 0)
				e.Handled=true;
            else if ("-01234567890".IndexOf(e.KeyChar.ToString()) == -1 && e.KeyChar != (char)8 && _currency.CurrencyDecimalDigits == 0)
                e.Handled = true;
        }
		#endregion

		private void _this_ParentChanged(object sender, EventArgs e)
		{
			if (this.Parent is FWBS.OMS.UI.Windows.EnquiryForm)
			{
				FWBS.OMS.UI.Windows.EnquiryForm _enqform = (FWBS.OMS.UI.Windows.EnquiryForm)this.Parent;
                if (_currency == NumberFormatInfo.InvariantInfo)
                {
                    _currency = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture).NumberFormat;
                    if (_enqform.Enquiry.Object is OMSFile)
                        _currency = ((OMSFile)_enqform.Enquiry.Object).CurrencyFormat;
                    else if (_enqform.Enquiry.Object is OMSDocument)
                        _currency = ((OMSDocument)_enqform.Enquiry.Object).OMSFile.CurrencyFormat;
                    else if (_enqform.Enquiry.Parent is OMSFile)
                        _currency = ((OMSFile)_enqform.Enquiry.Parent).CurrencyFormat;
                    else if (_enqform.Enquiry.Parent is OMSDocument)
                        _currency = ((OMSDocument)_enqform.Enquiry.Parent).OMSFile.CurrencyFormat;
                    else if (_enqform.Enquiry.Object is FeeEarner)
                        _currency = ((FeeEarner)_enqform.Enquiry.Object).CurrencyFormat;
                    else if (_enqform.Enquiry.Object is TimeRecord)
                        _currency = ((TimeRecord)_enqform.Enquiry.Object).OMSFile.CurrencyFormat;
                }
			}
		}
	}
}