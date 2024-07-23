using System;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{

    /// <summary>
    /// A single complaint item.
    /// </summary>
    public class Complaint : CommonObject
	{
		#region Constructors & Destructors

		/// <summary>
		/// Creates a new financial log item.
		/// </summary>
		internal Complaint () : base()
		{
			SetExtraInfo("clid", Session.CurrentSession.CurrentClient.ClientID);
		}

		
		/// <summary>
		/// Constructs a Complaint object based on an id.
		/// </summary>
		/// <param name="id">The identifier of the task to fetch.</param>
		[EnquiryUsage(true)]
		internal Complaint(int id) : base (id)
		{

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
				return "compID";
			}
		}

		
		protected override string PrimaryTableName
		{
			get
			{
				return "COMPLAINT";
			}
		}

		
		protected override string SelectStatement
		{
			get
			{
				return "select * from dbComplaints";
			}
		}

		
		protected override string FieldActive
		{
			get
			{
				return "compActive";
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
				return Client;
			}
		}

		#endregion

		#region Fields

		private bool _blnAddTask;

		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets the unique OMS id for the complaint item.
		/// </summary>
		public int ID
		{
			get
			{
				return Convert.ToInt32(UniqueID);
			}
		}
		
				
		/// <summary>
		/// Gets the Client ID as an integer
		/// </summary>
		public long ClientID
		{
			get
			{
				return Common.ConvertDef.ToInt64(GetExtraInfo("clID"), -1);
			}
		}
		
		/// <summary>
		/// Gets or Sets the transaction date of the financial item.
		/// </summary>
		[EnquiryUsage(true)]
		public string Reference
		{
			get
			{
				return Convert.ToString(GetExtraInfo("compRef"));
			}
			set
			{
				SetExtraInfo("compRef", value);
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
				return Convert.ToString(GetExtraInfo("compDesc"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("compDesc", DBNull.Value);
				else
					SetExtraInfo("compDesc", value);
			}
		}

		
		/// <summary>
		/// Gets the clinet that the complaint is associated to.
		/// </summary>
		[EnquiryUsage(true)]
		public Client Client
		{
			get
			{
				long id = Common.ConvertDef.ToInt32(GetExtraInfo("clID"), -1);
				if (id == -1)
					return null;
				else
					return Client.GetClient(id);
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
				int id = Common.ConvertDef.ToInt32(GetExtraInfo("compfeeID"), -1);
				if (id ==-1)
					return null;
				else
					return FeeEarner.GetFeeEarner(id);
			}
		}
		

		/// <summary>
		/// The reference used by originator of this log
		/// </summary>
		[EnquiryUsage(true)]
		public string Note
		{
			get
			{
				return Convert.ToString(GetExtraInfo("compNote"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("compNote", DBNull.Value);
				else
					SetExtraInfo("compNote", value);
			}			
		}
			
		
		/// <summary>
		/// Gets or Sets the date that the financiel item is expected to be paid.
		/// </summary>
		[EnquiryUsage(true)]
		public Common.DateTimeNULL Completed
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("compCompleted"),new Common.DateTimeNULL());
										
			}
			set
			{
				SetExtraInfo("compCompleted", value.ToObject());
			}
		}
		
		/// <summary>
		/// Gets the fee earner that completed the complaint, otherwise a null reference is returned.
		/// </summary>
		[EnquiryUsage(true)]
		public FeeEarner CompletedBy
		{
			get
			{
				int id = Common.ConvertDef.ToInt32(GetExtraInfo("compCompletedusrID"), -1);
				if (id ==-1)
					return null;
				else
					return FeeEarner.GetFeeEarner(id);
			}
		}
		
		/// <summary>
		/// Gets or Sets the date that the financiel item is expected to be paid.
		/// </summary>
		[EnquiryUsage(true)]
		public Common.DateTimeNULL EstimatedCompletion
		{
			get
			{
				return Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("compEstCompDate"),new Common.DateTimeNULL());
			}
			set
			{
				SetExtraInfo("compEstCompDate", value.ToObject());
			}
		}

		/// <summary>
		/// The complaint type
		/// </summary>
		[EnquiryUsage(true)]
		public string ComplaintType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("compType"));
			}
			set
			{
				if (value == null || value == String.Empty)
					SetExtraInfo("compType", DBNull.Value);
				else
					SetExtraInfo("compType", value);
			}			
		}

		/// <summary>
		/// Is the complaint active 
		/// </summary>
		[EnquiryUsage(true)]
		public bool Active
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("compActive"));
			}
			set
			{
				SetExtraInfo("compActive", value);
			}			
		}

		/// <summary>
		/// Is the complaint active 
		/// </summary>
		[EnquiryUsage(true)]
		public bool CreateTask
		{
			get
			{
				//implementation needed
				return _blnAddTask;
			}
			set
			{
				//implementation needed
				_blnAddTask = value;
			}
		}



		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a finance log item based on an identifier.
		/// </summary>
		/// <param name="id">The identifier of the item.</param>
		/// <returns></returns>
		public static Complaint GetComplaintItem(int id)
		{
			return new Complaint(id);
		}

		
		#endregion

	}
}
