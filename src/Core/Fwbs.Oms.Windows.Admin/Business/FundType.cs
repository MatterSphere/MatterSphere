using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Design;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 31000 FundType Business OMS for the Admin Kit
    /// </summary>
    public class FundType : FWBS.OMS.FundType
	{
		#region Events
		public event EventHandler CodeChange;
		#endregion
		
		#region Fields
		private CodeLookupDisplay _fndclcode = new CodeLookupDisplay("FTCLDESC");
		private CodeLookupDisplay _fndref = new CodeLookupDisplay("FTREFDESC");
		private CodeLookupDisplay _fndagreement = new CodeLookupDisplay("FTAGREEMENT");
		private string _description = "";
		private string _orgcode;
		#endregion

		#region Constructors
		/// <summary>
		/// Initialised an existing fund type object with the specified identifier.
		/// </summary>
		public FundType (string code) : base(code)
		{
			_fndclcode.Code = base.CreditLimitCode;
			_fndref.Code = base.RefCode;
			_fndagreement.Code = base.AgreementCode;
			_description = CodeLookup.GetLookup("FUNDTYPE",GetCode(code,0));
			_orgcode = code;
		}

		public FundType() : base()
		{
			_fndclcode.Code = base.CreditLimitCode;
			_fndref.Code = base.RefCode;
			_fndagreement.Code = base.AgreementCode;
		}

		/// <summary>
		/// Initialised an existing fund type object with the specified identifier cloned from another.
		/// </summary>
		public FundType (FundType clone) : base(clone)
		{
			_fndclcode.Code = base.CreditLimitCode;
			_fndref.Code = base.RefCode;
			_fndagreement.Code = base.AgreementCode;
			_description = CodeLookup.GetLookup("FUNDTYPE",GetCode(clone.Code,0));
			_orgcode = clone.Code;
		}

		#endregion

		#region Methods
		public override void Update()
		{
			base.CreditLimitCode = _fndclcode.Code;
			base.RefCode = _fndref.Code;
			base.AgreementCode = _fndagreement.Code;
			base.Update ();
		}
		#endregion

		#region Properties
		[LocCategory("(Details)")]
		[Browsable(true)]
		[RefreshProperties(RefreshProperties.All)]
		public override string Code
		{
			get{return base.Code;}
			set
			{
				Description = CodeLookup.GetLookup("FUNDTYPE",value);
				if (FWBS.OMS.FundType.Exists(value + "|" + this.CurrencyCode))
				{
					if (value != _orgcode)
						MessageBox.Show("The Fund Type Code [" + value + "] and Currency Code [" + this.CurrencyCode + "] already exists..","OMS Admin",MessageBoxButtons.OK,MessageBoxIcon.Stop);
					else
					{
						base.Code = value;
						if (CodeChange != null)
							this.CodeChange(this,EventArgs.Empty);
					}
				}
				else
				{
					if (IsNew)
					{
						SetExtraInfo("ftCode", value);
						base.Code = value;
						if (CodeChange != null)
							this.CodeChange(this,EventArgs.Empty);
					}
					else
						throw new OMSException2("31001","The Code cannot be changed when set");
				}
			}
		}

		[LocCategory("(Details)")]
		[Lookup("FTDescription")]
		[System.ComponentModel.Browsable(true)]
		public new string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (base.Code == "")
				{
				}
				else
				{
					if (_description != value)
					{
						_description = value;
						FWBS.OMS.CodeLookup.Create("FUNDTYPE",base.Code,value,"",CodeLookup.DefaultCulture,true,true,true);
					}
				}
			}
		}
		
		[LocCategory("CreditLimit")]
		[Lookup("CreditLimitDesc")]
		[CodeLookupSelectorTitle("CREDLIMITS","Credit Limit Descriptions")]
		public CodeLookupDisplay CreditLimitDesc
		{
			get
			{
				return _fndclcode;
			}
			set
			{
				_fndclcode = value;
			}
		}

		[LocCategory("Data")]
		[CodeLookupSelectorTitle("REFERENCES","References")]
		public CodeLookupDisplay Reference
		{
			get
			{
				return _fndref;
			}
			set
			{
				_fndref = value;
			}
		}
		
		[LocCategory("Agreement")]
		[Lookup("AgreeDateDesc")]
		[CodeLookupSelectorTitle("AGREEMENTS","Agreement Dates")]
		public CodeLookupDisplay AgreementDateDesc
		{
			get
			{
				return _fndagreement;
			}
			set
			{
				_fndagreement = value;
			}
		}

		[TypeConverter(typeof(CurrencyLister))]
		public override string CurrencyCode
		{
			get
			{
				return base.CurrencyCode;
			}
			set
			{
				base.CurrencyCode = value;
			}
		}

		#endregion

		#region Static
		/// <summary>
		/// Clones a specified client type and creates a new one based on the found one.
		/// </summary>
		/// <param name="CodeAndCurrencyCode">The Fund Code and Currency Code Seperated by a Pipe (|) e.g TEST|GBP</param>
		/// <returns>A new Cloned Fund Type</returns>
		public static FundType Clone(string CodeAndCurrencyCode)
		{
			Session.CurrentSession.CheckLoggedIn();
			FundType o = new FundType(CodeAndCurrencyCode);
			FundType n = new FundType(o);
			return n;
		}
		#endregion
	}
}
