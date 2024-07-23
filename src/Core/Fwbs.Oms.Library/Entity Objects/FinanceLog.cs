using System;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{


    /// <summary>
    /// A single finance log item.
    /// </summary>
    public class FinanceLog : CommonObject
	{

		
		#region Constructors & Destructors

		/// <summary>
		/// Creates a new financial log item.
		/// </summary>
		internal FinanceLog () : base()
		{
			TransactionDate = DateTime.Now;
			Paid = false;
			if (this.FileID == -1)
				SetExtraInfo("fileid", Session.CurrentSession.CurrentFile.ID);
		}

		/// <summary>
		/// Constructs a finance log object based on an id.
		/// </summary>
		/// <param name="id">The identifier of the task to fetch.</param>
		[EnquiryUsage(true)]
		internal FinanceLog(long id) : base (id)
		{
		}


		/// <summary>
		/// Public Constructor for Finance Logger
		/// </summary>
		/// <param name="omsfile"></param>
		public FinanceLog(OMSFile omsfile) : base()
		{
			SetExtraInfo("fileid", omsfile.ID);
		}



		#endregion
	
		#region CommonObject Implementation


		protected override string DefaultForm
		{
			get
			{
				return SystemForms.PrecedentEdit.ToString();
			}
		}

		public override string FieldPrimaryKey
		{
			get
			{
				return "finLogID";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "FINLOG";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbfinancialledger";
			}
		}

		protected override string FieldActive
		{
			get
			{
				return "";
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
				return File;
			}
		}

		#endregion

		#region Properties
	

		/// <summary>
		/// Gets the unique OMS id for the task item.
		/// </summary>
		public long ID
		{
			get
			{
				return Convert.ToInt64(UniqueID);
			}
		}
		
		
		/// <summary>
		/// Gets the OMS file associated to the task.
		/// </summary>
		public OMSFile File
		{
			get
			{
				if (FileID == -1)
					return null;
				else
					return OMSFile.GetFile(FileID);
			}
		}

		
		/// <summary>
		/// Gets the file id as an integer.
		/// </summary>
		public long FileID
		{
			get
			{
				return Common.ConvertDef.ToInt64(GetExtraInfo("fileid"), -1);
			}
		}

		
		/// <summary>
		/// Gets or Sets the transaction date of the financial item.
		/// </summary>
		[EnquiryUsage(true)]
		public DateTime TransactionDate
		{
			get
			{
                try
                {
                    return Convert.ToDateTime(GetExtraInfo("finItemDate"));
                }
                catch
                {
                    return DateTime.MinValue;
                }
			}
			set
			{
                DateTime oldval = TransactionDate;
                if (oldval != value)
                {
                    SetExtraInfo("finItemDate", value);
                    OnPropertyChanged(new PropertyChangedEventArgs("TransactionDate", oldval, TransactionDate));
                }
			}
		}

		
		/// <summary>
		/// Gets or Sets the posting description.
		/// </summary>
		[EnquiryUsage(true)]
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("finDesc"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("finDesc", DBNull.Value);
				else
					SetExtraInfo("finDesc", value);
			}
		}

		
		/// <summary>
		/// Gets the associate that the item is associated to.
		/// </summary>
		[EnquiryUsage(true)]
		public Associate Associate
		{
			get
			{
				long id = Common.ConvertDef.ToInt32(GetExtraInfo("finAssocId"), -1);
				if (id == -1)
					return null;
				else
					return Associate.GetAssociate(id);
			}
		}

		
		/// <summary>
		/// gets the name of the transactions payee or payer
		/// </summary>
		[EnquiryUsage(true)]
		public string PayName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("finPayName"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("finPayName", DBNull.Value);
				else
					SetExtraInfo("finPayName", value);
			}
			
		}
		

		/// <summary>
		/// The reference used by originator of this log
		/// </summary>
		[EnquiryUsage(true)]
		public string TheirReference
		{
			get
			{
				return Convert.ToString(GetExtraInfo("finTheirRef"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("finTheirRef", DBNull.Value);
				else
					SetExtraInfo("finTheirRef", value);
			}			
		}
		

		
		/// <summary>
		/// less VAT value of transaction
		/// </summary>
		[EnquiryUsage(true)]
		public decimal Net
		{
			get
			{
				return FWBS.Common.Math.RoundUp(Common.ConvertDef.ToDecimal(GetExtraInfo("finValue"),decimal.Zero));
			}
			set
			{
				decimal newvalue = Common.ConvertDef.ToDecimal(value,0);
				
				SetExtraInfo("finValue",newvalue);
			}
		}
		
		
		/// <summary>
		/// VAT value of this transaction
		/// </summary>
		[EnquiryUsage(true)]
		public decimal Vat
		{
			get
			{
				return FWBS.Common.Math.RoundUp(Common.ConvertDef.ToDecimal(GetExtraInfo("finVat"), decimal.Zero));
				
			}
			set
			{
				//if vat changes work on a cost wins basis
				decimal newvat = Common.ConvertDef.ToDecimal(value,0);
				
				SetExtraInfo("finVat",newvat);
			}
		}
        
		
		/// <summary>
		/// full value of this transaction including vat
		/// </summary>
		[EnquiryUsage(true)]
		public decimal Gross
		{
			get
			{
				return FWBS.Common.Math.RoundUp(Common.ConvertDef.ToDecimal(GetExtraInfo("finGross"), decimal.Zero));
			}
			set
			{
				//calculate vat values and total from new net value
				decimal newgross = Common.ConvertDef.ToDecimal(value,0);
				SetExtraInfo("finGross",newgross);
			}
		}

		/// <summary>
		/// Gets the fee earner that authorised the item, otherwise a null reference is returned.
		/// </summary>
		[EnquiryUsage(true)]
		public FeeEarner AuthorisedBy
		{
			get
			{
				int id = Common.ConvertDef.ToInt32(GetExtraInfo("finAuthByFeeId"), -1);
				if (id ==-1)
					return null;
				else
					return FeeEarner.GetFeeEarner(id);
			}
		}

		
		/// <summary>
		/// Gets or Sets a flag that indicates that the transaction has been paid.
		/// </summary>
		[EnquiryUsage(true)]
		public bool Paid
		{
			get
			{
				return Common.ConvertDef.ToBoolean(GetExtraInfo("finPaid"), false);
			}
			set
			{
				SetExtraInfo("finPaid", value);
			}
		}

		/// <summary>
		/// Gets or Sets the date that the financiel item is expected to be paid.
		/// </summary>
		[EnquiryUsage(true)]
		public Common.DateTimeNULL PaidByDate
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("finExpectedPayment"),new Common.DateTimeNULL());
			}
			set
			{
				SetExtraInfo("finExpectedPayment", value.ToObject());
			}
		}

		




		#endregion

		#region Methods

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a finance log item based on an identifier.
		/// </summary>
		/// <param name="id">The identifier of the item.</param>
		/// <returns></returns>
		public static FinanceLog GetLogItem(long id)
		{
			return new FinanceLog(id);
		}

		
		#endregion
		
		#region IUpdateable Implementation

		public override void Update()
		{
			//check that the totals are OK before saving
			if((Net + Vat) != Gross)
				throw new OMSException2("ERRFINLOGTOTALS","Values do not balance, please check.");
			
			base.Update();
			
		}
		
		#endregion


	}
}
