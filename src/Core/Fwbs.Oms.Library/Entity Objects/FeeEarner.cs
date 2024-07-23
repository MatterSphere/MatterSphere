using System;
using System.Data;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;


namespace FWBS.OMS
{

    //TODO: feeExtID
    //TODO: feeCVFile
    //TODO: feeLAGrade
    //TODO: feeLARef
    //TODO: feeCDSStartNum

    /// <summary>
    /// 16000 Fee earner class that inherits of the user class to expand on the existing user functionality.
    /// This object can be used by the enquiry engine.
    /// </summary>
    public sealed class FeeEarner : User, IEnquiryCompatible, IDisposable
	{

		#region Fields

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _feeEarner = null;
		
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		new internal const string Sql = "select * from dbfeeearner";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		new public const string Table = "FEEEARNER";

				/// <summary>
		/// Holds the different extended data sources for the contact.
		/// </summary>
		private ExtendedDataList _extData = null;
		
		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new fee earner with a specified type.
		/// </summary>
		/// <param name="type"></param>
		public FeeEarner(FeeEarnerType type)
		{
			SetExtraInfo("feetype", type.Code);
		}

		/// <summary>
		/// Creates a new user and fee earner object.  This routine is used by the enquiry engine
		/// to create new fee earner object.
		/// </summary>
		internal FeeEarner() : base()
		{
			_feeEarner = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);

			Global.CreateBlankRecord(ref _feeEarner, true);

			SetExtraInfo("feetype", "STANDARD");
			SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Created", DateTime.Now);
		}

		/// <summary>
		/// Initialised an existing user / fee earner object with the specified identifier.
		/// </summary>
		/// <param name="id">User / fee earner Identifier.</param>
        [EnquiryUsage(true)]
        internal FeeEarner(int id)
            : base(id) 
		{
			_feeEarner = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where feeusrID = " + id.ToString(), Table, new IDataParameter[0]);
			if ((_feeEarner == null) || (_feeEarner.Rows.Count == 0)) 
				throw new OMSException(HelpIndexes.UserNotAFeeEarner, base.FullName);

            Session.CurrentSession.CurrentFeeEarners.Add(ID.ToString(), this);

		}

        internal FeeEarner(DataTable data, DataTable userdata) : base(userdata, "")
        {
            //Only used for loggin in.
            if (data == null)
                throw new ArgumentNullException("data");

            this._feeEarner = data.Copy();

            if ((_feeEarner == null) || (_feeEarner.Rows.Count == 0))
                throw new OMSException(HelpIndexes.UserNotAFeeEarner, base.FullName);

            Session.CurrentSession.CurrentFeeEarners.Add(ID.ToString(), this);

        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the fee earner type code.
		/// </summary>
		public string FeeEarnerTypeCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feetype"));
			}	
		}


		/// <summary>
		/// Gets the fee earner type of the current fee earner object.
		/// </summary>
		public FeeEarnerType CurrentFeeEarnerType
		{
			get
			{
				return (FeeEarnerType)GetOMSType();
			}
		}

		/// <summary>
		/// Gets or Sets the default department that the fee earner works under.
		/// </summary>
		[EnquiryUsage(true)]
		public string DefaultDepartment
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feedepartment"));
			}
			set
			{
				SetExtraInfo("feedepartment", value);
			}
		}


		/// <summary>
		/// Gets or Sets the default file type object that the fee earner works on.
		/// </summary>
		[EnquiryUsage(true)]
		public string DefaultFileType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feefiletype"));
			}
			set
			{
				SetExtraInfo("feefiletype", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Sign off text for the end of a letter e.g, Mike Walker.
		/// </summary>
		[EnquiryUsage(true)]
		public string SignOff
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feeSignOff"));
			}
			set
			{
				SetExtraInfo("feeSignOff", value);
			}
		}

		/// <summary>
		/// Gets the additional reference that is put on the top of a letter.
		/// </summary>
		[EnquiryUsage(true)]
		public string AdditionalReference
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feeAddRef"));
			}
			set
			{
				SetExtraInfo("feeAddRef", value);
			}
		}

		
		/// <summary>
		/// Gets or Sets the additional sign off that goes at the bottom of a letter e.g, Development Director.
		/// </summary>
		[EnquiryUsage(true)]
		public string AdditionalSignOff
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feeAddSignOff"));
			}
			set
			{
				SetExtraInfo("feeAddSignOff", value);
			}
		}


		/// <summary>
		/// Gets or Sets a boolean value, true if the fee earner is able to be responsible of sombody else.
		/// </summary>
		[EnquiryUsage(true)]
		public bool IsResponsible
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("feeResponsible");
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("feeResponsible", value);
			}
		}


		/// <summary>
		/// Gets or Sets the fee earner object who is resposnible of this fee earner.
		/// </summary>
		public FeeEarner ResponsibleTo
		{
			get
			{
				if (GetExtraInfo("feeResponsibleTo") is DBNull)
					return null;
				else
					return FeeEarner.GetFeeEarner ((int)GetExtraInfo("feeResponsibleTo"));
			}
			set
			{
				if (value == null)
					SetExtraInfo("feeresponsibleto", DBNull.Value);
				else
				{
					if (value.IsResponsible)
						SetExtraInfo("feeresponsibleto", value.ID);
					else
						throw new OMSException(HelpIndexes.FeeEarnerNotResponsible);
				}
			}
		}

		/// <summary>
		/// Gets or Sets the target number of hourcs a week the fee earner should be charging for.
		/// </summary>
		[EnquiryUsage(true)]
		public byte TagetHoursPerWeek
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("feetargethoursweek");
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feetargethoursweek", value);
			}
		}

		/// <summary>
		/// Gets or Sets the number of hours a day the fee earner should be charging.
		/// </summary>
		[EnquiryUsage(true)]
		public byte TagetHoursPerDay
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("feetargethoursday");
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feetargethoursday", value);
			}
		}

		/// <summary>
		/// Gets or sets the assistant name for the fee earner.  This is a text field as it can
		/// be used within a letter where ever positioned.
		/// </summary>
		[EnquiryUsage(true)]
		public string Assistant
		{
			get
			{
				return Convert.ToString(GetExtraInfo("feeassistant"));
			}
			set
			{
				SetExtraInfo("feeassistant", value);
			}

		}
		
		/// <summary>
		/// Gets a flag value that indicates whether the current fee earner is an active fee earner.
		/// This hides the user's IsActive property, to check if this fee earner active as a user
		/// then cast this object to an user object.
		/// </summary>
		new public bool IsActive
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("feeActive");
				}
				catch
				{
					return false;
				}
			}
		}
		
		
		/// <summary>
		/// Gets the base currency that the cost and rate band rate are based on.
		/// </summary>
		public System.Globalization.NumberFormatInfo CurrencyFormat
		{
			get
			{
				Currency currency = new Currency();
				currency.Fetch(Convert.ToString(BaseCurrency));
				return currency.CurrencyFormat;
			}
		}

		[EnquiryUsage(true)]
		internal string BaseCurrency
		{
			get
			{
				if (Convert.ToString(GetExtraInfo("feecurISOCode")) != "")
					return Convert.ToString(GetExtraInfo("feecurISOCode"));
                else if (!string.IsNullOrEmpty(this.Currency))
                    return this.Currency;
				else
					return Session.CurrentSession.CurrentBranch.Currency;
			}
			set
			{
                if (value == "")
                {
                    if (string.IsNullOrEmpty(this.Currency))
                        SetExtraInfo("feecurISOCode", Session.CurrentSession.CurrentBranch.Currency);
                    else
                        SetExtraInfo("feecurISOCode", this.Currency);
                }
                else
                    SetExtraInfo("feecurISOCode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the hourly overhead that this fee earner costs the company.
		/// </summary>
		[EnquiryUsage(true)]
		public decimal Cost
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feecost"));                                      
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feecost", value);
			}
		}

		/// <summary>
		/// Gets or Sets the lowest rate that this fee earner charges.
		/// </summary>
		[EnquiryUsage(true)]
        public decimal RateBand1
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feerateband1")); 
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feerateband1", value);
			}
		}

		/// <summary>
		/// Gets or Sets the lower rate that this fee earner charges.
		/// </summary>
		[EnquiryUsage(true)]
		public decimal RateBand2
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feerateband2"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feerateband2", value);
			}
		}

		/// <summary>
		/// Gets or Sets the standard rate that this fee earner charges.
		/// </summary>
		[EnquiryUsage(true)]
		public decimal RateBand3
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feerateband3"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feerateband3", value);
			}
		}

		/// <summary>
		/// Gets or Sets the higher rate that this fee earner charges.
		/// </summary>
		[EnquiryUsage(true)]
		public decimal RateBand4
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feerateband4"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feerateband4", value);
			}
		}

		/// <summary>
		/// Gets or Sets the highest rate that this fee earner charges.
		/// </summary>
		[EnquiryUsage(true)]
		public decimal RateBand5
		{
			get
			{
				try
				{
                    return Convert.ToDecimal(GetExtraInfo("feerateband5"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("feerateband5", value);
			}
		}


		/// <summary>
		/// Gets the modification dates and users.
		/// </summary>
		[EnquiryUsage(true)]
		public override ModificationData TrackingStamp
		{
			get
			{
				return new ModificationData(
					Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("Created"), DBNull.Value),
					Common.ConvertDef.ToInt32(GetExtraInfo("CreatedBy"), 0),
					Common.ConvertDef.ToDateTimeNULL(GetExtraInfo("Updated"), DBNull.Value),
					Common.ConvertDef.ToInt32(GetExtraInfo("UpdatedBy"), 0));
			}
		}

		/// <summary>
		/// Gets or Sets the signature of the FeeEarner.
		/// </summary>
		[EnquiryUsage(true)]
		public Signature Signature
		{
			get
			{
                //TODO: Check Role Permission
				if (GetExtraInfo("feeSignature") == DBNull.Value)
					return Signature.Empty; 
				else
					return new Signature((byte[])GetExtraInfo("feeSignature"), this.ID.ToString());
			}
			set
			{
				//TODO: Check Role Permission
				if (value == null || value.Equals(Signature.Empty) || value == Signature.Empty)
					SetExtraInfo("feeSignature", DBNull.Value);
				else
				{
					SetExtraInfo("feeSignature", value.ToByteArray());
				}
			}
		}

        
		/// <summary>
		/// Gets or Sets the Users who can access the Signature...
		/// </summary>
        [EnquiryUsage(true)]
        [LocCategory("SECURITY")]
        public string SignatureSecurity
        {
            get
            {
                return Convert.ToString(GetXmlProperty("signatureSecurity", ""));
            }
            set
            {
                SetXmlProperty("signatureSecurity", value);
            }
        }
			

		#endregion

		#region Methods

		/// <summary>
		/// Makes the fee earner inactive from being a fee earner.  This hides the users MakeInactive method.
		/// To make the fee earner inactive as a user please cast this object to a user object.
		/// </summary>
		new public void MakeInactive()
		{
			SetExtraInfo("feeactive", false);
		}

        /// <summary>
        /// Validates the Current user for use of the Fee Earner Signature
        /// </summary>
        public bool CanUseSignature()
        {
            if (this.SignatureSecurity != "" && Session.CurrentSession.CurrentUser.ID != this.ID && this.SignatureSecurity.IndexOf(Session.CurrentSession.CurrentUser.ID.ToString()) < 0)
                return false;
            else
                return true;
        }

		#endregion

		#region Static Methods

		/// <summary>
		/// Returns a fee earner object based on the fee eanre ID passed.
		/// </summary>
		/// <param name="id">Specified fee earner id.</param>
		/// <returns>Returns a fee earner object.</returns>
		static public FeeEarner GetFeeEarner(int id)
		{
            FeeEarner cf = Session.CurrentSession.CurrentFeeEarners[id.ToString()] as FeeEarner;
            if (cf == null)
            {
                cf = new FeeEarner(id);
            }

            return cf;
		}

		/// <summary>
		/// Gets a datatable list of fee earners.
		/// </summary>
		/// <returns></returns>
		static public DataTable GetFeeEarners()
		{
			Session.CurrentSession.CheckLoggedIn();
			return Session.CurrentSession.Connection.ExecuteSQLTable("select dbfeeearner.*, usrFullName from dbfeeearner inner join dbuser on usrid = feeusrid order by usrfullname","FEEEARNERS",false,new IDataParameter[0]);
		}

		#endregion

		#region IExtraInfo Implementation


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

			if (_feeEarner != null && _feeEarner.Columns.Contains(fieldName))
				_feeEarner.Rows[0][fieldName] = val;
			else
				base.SetExtraInfo(fieldName, val);
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public override object GetExtraInfo(string fieldName)
		{
            if (_feeEarner != null && _feeEarner.Columns.Contains(fieldName))
            {
                object val = _feeEarner.Rows[0][fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
            }
            else
                return base.GetExtraInfo(fieldName);
		}

		/// <summary>
		/// Returns the specified fields type.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public override Type GetExtraInfoType(string fieldName)
		{
			try
			{
				if (_feeEarner != null && _feeEarner.Columns.Contains(fieldName))
					return _feeEarner.Columns[fieldName].DataType;
				else
					return base.GetExtraInfoType(fieldName);
			}
			catch 
			{
				throw new OMSException2("16001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>		
		public override DataSet GetDataset()
		{
			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}

		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public override DataTable GetDataTable()
		{
			return _feeEarner;
		}

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public override void Update()
		{
            bool isnew = IsNew;

			base.Update();

			SetExtraInfo("feeusrid", base.ID);

			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (_feeEarner.GetChanges()!= null)
			{

				SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
				SetExtraInfo("Updated", DateTime.Now);

				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (_feeEarner.PrimaryKey == null || _feeEarner.PrimaryKey.Length == 0)
					_feeEarner.PrimaryKey = new DataColumn[1]{_feeEarner.Columns["feeusrid"]};


				Session.CurrentSession.Connection.Update(_feeEarner, Sql + " where feeusrid = " + this.ID.ToString());
			}


            if (isnew)
            {
                Session.CurrentSession.CurrentFeeEarners.Add(ID.ToString(), this);
            }

			//Update all the extended data objects, if any.
			if (_extData != null)
			{
				foreach (FWBS.OMS.ExtendedData ext in _extData)
				{
					ext.UpdateExtendedData();
				}
			}

		}

		//TODO: Refresh

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public override void Cancel()
		{
			base.Cancel();
			_feeEarner.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		public override bool IsDirty
		{
			get
			{
				bool ret = base.IsDirty;
				if (ret == false)
					ret = (_feeEarner.GetChanges() != null);
				return ret;
			}
		}

		#endregion

		#region IEnquiryCompatible Implementation		



		/// <summary>
		/// Edits the current fee earner object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public override Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.FeeEarnerAdmin), param);
		}

		/// <summary>
		/// Edits the current fee earner object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public override Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return Enquiry.GetEnquiry(customForm, Parent, this, param);
		}


		#endregion

		#region IDisposable Implementation


		/// <summary>
		/// Disposes the fee earner object immediately witout waiting for the garbage collector.
		/// </summary>
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by the fee earner object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		protected override void Dispose(bool disposing) 
		{
            try
            {
                if (disposing)
                {
                    if (_extData != null)
                    {
                        _extData.Dispose();
                        _extData = null;
                    }

                    if (_feeEarner != null)
                    {
                        _feeEarner.Dispose();
                        _feeEarner = null;
                    }
                }

            }
            finally
            {
                // Changed by MNW, to stop stack overflow, from the Parent
                base.Dispose(true);
            }
		}


		#endregion

		#region IOMSType Implementation

		/// <summary>
		/// Gets an OMS Type based on the fee earner type off this current instance of a fee earner object.
		/// </summary>
		/// <returns>A OMSType with information needed to represented this type of fee earner.</returns>
		public override OMSType GetOMSType()
		{
			return FWBS.OMS.FeeEarnerType.GetFeeEarnerType(FeeEarnerTypeCode);
		}

		#endregion

		#region IExtendedDataCompatible Implementation
		/// <summary>
		/// Gets the extended data list indexer which will expose
		/// each of the extended data objects on the particular object.
		/// </summary>
		public override ExtendedDataList ExtendedData 
		{
			get
			{
				if (_extData == null)
				{
					//Use the contact type configuration to initialise the extended data objects.
					FeeEarnerType t = CurrentFeeEarnerType;
					string [] codes = new string [t.ExtData.Count];
					int ctr = 0;
					foreach(OMSType.ExtendedData ext in t.ExtData)
					{
						codes.SetValue(ext.Code, ctr);
						ctr++;
					}
					if (codes.Length > 0)
						_extData = new ExtendedDataList(this, codes);
					else
						_extData = new ExtendedDataList();
				}
				return _extData;
			}
		}

		#endregion

	
	}

}
