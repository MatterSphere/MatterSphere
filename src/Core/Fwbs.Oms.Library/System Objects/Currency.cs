using System;
using System.ComponentModel;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{
    /// <summary>
    /// Summary description for Currency.
    /// </summary>
    [DefaultProperty("Code")]
	public class Currency : CommonObject
	{

		#region Fields
		private System.Globalization.NumberFormatInfo _currencyformat = new System.Globalization.NumberFormatInfo();
		#endregion

		public static Currency GetCurrency(string id)
		{
			Currency _currency = new Currency();
			_currency.Fetch(id);
			return _currency;
		}

		public void Fetch(string id)
		{
			base.Fetch (id);
			_currencyformat.CurrencyDecimalDigits = Convert.ToInt32(GetExtraInfo("curDecimalPlaces"));
			_currencyformat.CurrencyDecimalSeparator = Convert.ToString(GetExtraInfo("curDecimalSeperator"));
			_currencyformat.CurrencyGroupSeparator = Convert.ToString(GetExtraInfo("curGroupSeparator"));
			_currencyformat.CurrencyNegativePattern = Convert.ToInt32(GetExtraInfo("curNegativePattern"));
			_currencyformat.CurrencyPositivePattern = Convert.ToInt32(GetExtraInfo("curPositivePattern"));
			_currencyformat.CurrencySymbol = Convert.ToString(GetExtraInfo("curSign"));
		}


		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}


		public override string FieldPrimaryKey
		{
			get
			{
				return "curISOCode";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "CURRENCY";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbCurrency";
			}
		}

		protected override string FieldActive
		{
			get
			{
				return "curActive";
			}
		}


		#region Properties
		/// <summary>
		/// Gets the Currency Format
		/// </summary>
		[Browsable(false)]
		public System.Globalization.NumberFormatInfo CurrencyFormat
		{
			get
			{
				return _currencyformat; 
			}
		}
		
		/// <summary>
		/// Gets the unique Currency identifier.
		/// </summary>
		[LocCategory("(Details)")]
		public string Code
		{
			get
			{
				return Convert.ToString(base.UniqueID);
			}
			set
			{
				base.UniqueID = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Currency Name
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Currency")]
		public string CurrencyName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("curName"));
			}
			set
			{
				SetExtraInfo("curName", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Currency Symbol
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Currency")]
		public string CurrencySign
		{
			get
			{
				return Convert.ToString(GetExtraInfo("curSign"));
			}
			set
			{
				SetExtraInfo("curSign", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Currency Sign Description
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Currency")]
		[Lookup("CurrencyDesc")]
		public string CurrencySignDescription
		{
			get
			{
				return Convert.ToString(GetExtraInfo("curSignDesc"));
			}
			set
			{
				SetExtraInfo("curSignDesc", value);
			}
		}

		/// <summary>
		/// Gets or Sets any Currency Rate
		/// </summary>
		[EnquiryUsage(true)]
		[LocCategory("Currency")]
		public double CurrencyRate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToDouble(GetExtraInfo("curRate"),0);
			}
			set
			{
				SetExtraInfo("curRate", value);
			}
		}
		#endregion


		
		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		public override object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion
	}
}
