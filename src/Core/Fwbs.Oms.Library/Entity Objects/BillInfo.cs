using System;

namespace FWBS.OMS
{
	/// <summary>
	/// An object that exposes and deals with the logging of Billing Information.
	/// </summary>
	public class BillingInfo : CommonObject
	{
		#region Constructors

		/// <summary>
		/// Creates a new billing information item from scratch.
		/// </summary>
		[EnquiryEngine.EnquiryUsage(true)]
		internal BillingInfo () : this(Session.CurrentSession.CurrentAssociate)
		{
		}

		public BillingInfo(Associate assoc) : base()
		{
			Session.CurrentSession.CheckLoggedIn();
			SetExtraInfo("fileID", assoc.OMSFile.ID);
			SetExtraInfo("assocID", assoc.ID);
			BillDate = System.DateTime.Now;
			OnAccountAmount = 0;
			VATAmount = 0;
			ProfessionalFees = 0;
			PaidDisbursments = 0;
			UnpaidDisbursments = 0;
		}

		#endregion

		#region CommonObject Implementation

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
				return "billID";
			}
		}

		public override object Parent
		{
			get
			{
				return OMSFile;
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "BILLINFO";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbbillinfo";
			}
		}




		#endregion

		#region Properties

		/// <summary>
		/// Gets the unique identifier of the billing information item.
		/// </summary>
		public int ID
		{
			get
			{
				return Convert.ToInt32(GetExtraInfo(FieldPrimaryKey));
			}
		}

		/// <summary>
		/// Gets the file that the item belongs to.
		/// </summary>
		public OMSFile OMSFile
		{
			get
			{
				return OMSFile.GetFile(Convert.ToInt64(GetExtraInfo("fileID")));
			}
		}

		/// <summary>
		/// Gets the generated billing information 
		/// </summary>
		public string BillNo
		{
			get
			{
				return Convert.ToString(GetExtraInfo("billno"));
			}
		}

		/// <summary>
		/// Gets or Sets the date at which the bill was created.
		/// </summary>
		public System.DateTime BillDate
		{
			get
			{
				return Convert.ToDateTime(GetExtraInfo("billDate"));
			}
			set
			{
				SetExtraInfo("billDate", value);
			}
		}

		/// <summary>
		/// Gets or Sets the cateory code of the biliing information item.
		/// </summary>
		public string Category
		{
			get
			{
				return Convert.ToString(GetExtraInfo("billCategory"));
			}
			set
			{
				if (value == null && value == "")
					SetExtraInfo("billCategory", DBNull.Value);
				else
					SetExtraInfo("billCategory", value);
			}
		}

		
		/// <summary>
		/// Gets the current VAT rate of the bill.
		/// </summary>
		public float VATRate
		{
			get
			{
				return Session.CurrentSession.SalesTaxRate;
			}
		}

		/// <summary>
		/// Gets or Sets the amount that has already been paid on the account.
		/// </summary>
		public decimal OnAccountAmount
		{
			get
			{
				return Convert.ToDecimal(GetExtraInfo("billOnAccount"));
			}
			set
			{
				SetExtraInfo("billOnAccount", value);
			}
		}

		/// <summary>
		/// Gets or Sets the amount of paid disbursments.
		/// </summary>
		public decimal PaidDisbursments
		{
			get
			{
				return Convert.ToDecimal(GetExtraInfo("billPaidDisb"));
			}
			set
			{
				SetExtraInfo("billPaidDisb", value);
			}
		}

		/// <summary>
		/// Gets or Sets the amount of unpaid disbursments.
		/// </summary>
		public decimal UnpaidDisbursments
		{
			get
			{
				return Convert.ToDecimal(GetExtraInfo("billNYPDisb"));
			}
			set
			{
				SetExtraInfo("billNYPDisb", value);
			}
		}

		/// <summary>
		/// Gets or Sets the total VAT amount of the bill.
		/// </summary>
		public decimal VATAmount
		{
			get
			{
				return Convert.ToDecimal(GetExtraInfo("billVAT"));
			}
			set
			{
				SetExtraInfo("billVAT", value);
			}
		}

		/// <summary>
		/// Gets or Sets the amount of professional fees used..
		/// </summary>
		public decimal ProfessionalFees
		{
			get
			{
				return Convert.ToDecimal(GetExtraInfo("billProCosts"));
			}
			set
			{
				SetExtraInfo("billProCosts", value);
			}
		}

		/// <summary>
		/// Gets the total amount of disbursments.
		/// </summary>
		public decimal TotalDisbursments
		{
			get
			{
				return PaidDisbursments + UnpaidDisbursments;
			}
		}

		/// <summary>
		/// Gets the total amount of costs for the bill.
		/// </summary>
		public decimal TotalCost
		{
			get
			{ 
				return TotalDisbursments + ProfessionalFees + VATAmount;
			}
		}

		/// <summary>
		/// Gets the cost of the bill excluding the VAT.
		/// </summary>
		public decimal NetCost
		{
			get
			{
				return TotalCost - VATAmount;
			}
		}

		/// <summary>
		/// Gets the amount of money owed for the bill, taking into account what is already held in the account.
		/// </summary>
		public decimal TotalOutstanding
		{
			get
			{
				return TotalCost - OnAccountAmount;
			}
		}


		#endregion

		#region Methods

		public override void Update()
		{
			SetExtraInfo("billexVat", NetCost);
			SetExtraInfo("billTotal", TotalOutstanding);
			base.Update ();
		}

		#endregion
	}
}
