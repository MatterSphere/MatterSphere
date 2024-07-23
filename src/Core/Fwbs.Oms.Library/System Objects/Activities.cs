using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// Summary description for Activity.
	/// </summary>
	public class Activities : CommonObject
	{
		#region Fields
		private Int64 _fileid =0;
		private Int64 _feeearnerid = 0;
		#endregion

		#region Constructors
		public Activities(Int64 FileID, Int64 FeeEarnerID) : this(FileID,FeeEarnerID,false,-1)
		{
		}

		public Activities(Int64 FileID, Int64 FeeEarnerID, bool IncludeNotSet) : this(FileID,FeeEarnerID,IncludeNotSet,-1)
		{
		}

 		public Activities(Int64 FileID, Int64 FeeEarnerID, bool IncludeNotSet, int LegalCategory) : this(FileID,FeeEarnerID,IncludeNotSet,LegalCategory, null)
    	{
		}

		public Activities(Int64 FileID, Int64 FeeEarnerID, bool IncludeNotSet, int LegalCategory, DateTime? TimeRecorded) : base()
		{
			_fileid = FileID;
			_feeearnerid = FeeEarnerID;
            IDataParameter[] iparams;
            if (TimeRecorded.HasValue == true && FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate)
                iparams = new IDataParameter[5];
            else
                iparams = new IDataParameter[4];

            iparams[0] = Session.CurrentSession.Connection.AddParameter("FILEID", SqlDbType.BigInt, 0, FileID);
			iparams[1] = Session.CurrentSession.Connection.AddParameter("FEEUSRID", SqlDbType.BigInt, 0, FeeEarnerID);
			iparams[2] = Session.CurrentSession.Connection.AddParameter("INCLNull", SqlDbType.Bit, 0, IncludeNotSet);
			iparams[3] = Session.CurrentSession.Connection.AddParameter("LEGAIDCAT", SqlDbType.SmallInt, 0, LegalCategory);
            
            // Added System Override to allow for a new TimeRecorded Property to be used.
            if (TimeRecorded.HasValue == true && FWBS.OMS.Session.CurrentSession.UseTimeRecordedDate)
                iparams[4] = Session.CurrentSession.Connection.AddParameter("TIMERECORDED", SqlDbType.DateTime, 0, TimeRecorded);

			_data = Session.CurrentSession.Connection.ExecuteProcedureTable("sprTimeActivities", "LOTSOFACTIVITIES", false, iparams);
		}
		#endregion

		public void GetActivities(string Code)
		{
			_data.DefaultView.RowFilter = "actCode = '" + Code + "'";
		}

		#region Overrides
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
				return "actCode";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "LOTSOFACTIVITIES";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "SELECT * FROM dbActivities";
			}
		}

		protected override string FieldActive
		{
			get
			{
				return "actActive";
			}
		}
		#endregion

		#region ExtraInfo
		/// <summary>
		/// Checks to see if an item of the specified id exists within the database.
		/// </summary>
		/// <param name="id">The unique id to search for.</param>
		/// <returns>True if the item exists, otherwise false.</returns>
		public override bool Exists(object id)
		{
			try
			{
				this.GetActivities(Convert.ToString(id));
				if (_data.DefaultView.Count > 0)
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public override void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_data.DefaultView[0].Row[fieldName] = val;
			OnDataChanged();
			if (fieldName == FieldPrimaryKey)
				OnUniqueIDChanged();
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public override object GetExtraInfo(string fieldName)
		{
			object val = _data.DefaultView[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}
		#endregion

		#region Properties
		[System.ComponentModel.Browsable(false)]
		public Int64 FileID
		{
			get
			{
				return _fileid;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public Int64 FeeEarnerID 
		{
			get
			{
				return _feeearnerid;
			}
		}

		[LocCategory("(Details)")]
		public string AccCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("actAccCode"));
			}
			set
			{
				SetExtraInfo("actAccCode",value);
			}
		}

		[LocCategory("Data")]
		public bool Chargeable
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("actChargeable"),true);
			}
			set
			{
				SetExtraInfo("actChargeable",value);
			}
		}

		[LocCategory("Data")]
		public bool FixedRate
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("actFixedRate"),false);
			}
			set
			{
				SetExtraInfo("actFixedRate",value);
			}
		}

		[LocCategory("Old")]
		public string TemplateMatch
		{
			get
			{
				return Convert.ToString(GetExtraInfo("actTemplateMatch"));
			}
			set
			{
				SetExtraInfo("actTemplateMatch",value);
			}
		}

		[LocCategory("Data")]
		public string LegalAidFilter
		{
			get
			{
				return Convert.ToString(GetExtraInfo("actLegalAidFilter"));
			}
			set
			{
				SetExtraInfo("actLegalAidFilter",value);
			}
		}

		[LocCategory("Money")]
		public decimal FixedValue
		{
			get
			{
				try
				{
					return Convert.ToDecimal(GetExtraInfo("actFixedValue"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				if (value != 0)
					SetExtraInfo("actFixedValue",value);
				else
					SetExtraInfo("actFixedValue",DBNull.Value);
			}
		}

		public ActivityStyles ActivityStyle
		{
			get
			{
				if (_data.Columns.Contains("actFixedRateLegal") && Convert.ToBoolean(GetExtraInfo("actFixedRateLegal")) == true) 
					return ActivityStyles.FixedRateLegalAid;
				else if (_data.Columns.Contains("actFixedRateLegal") && Convert.ToBoolean(GetExtraInfo("actFixedRateLegal")) == false) 
					return ActivityStyles.LegalAid;
				else 
					return ActivityStyles.NotLegalAid;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public decimal Charge
		{
			get
			{
				try
				{
					return Convert.ToDecimal(GetExtraInfo("actCharge"));
				}
				catch
				{
					return 0;
				}
			}
		}

		[System.ComponentModel.Browsable(false)]
		public decimal Cost
		{
			get
			{
				try
				{
					return Convert.ToDecimal(GetExtraInfo("actCost"));
				}
				catch
				{
					return 0;
				}
			}
		}

		[LocCategory("(Details)")]
		public bool Active
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("actActive"),false);
			}
			set
			{
				SetExtraInfo("actActive",value);
			}
		}

		[LocCategory("Money")]
		public string ISOCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("actcurISOCode"));
			}
			set
			{
				SetExtraInfo("actcurISOCode",value);
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
				return OMSFile.GetFile(FileID);
			}
		}

		#endregion
	}
}
