using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// Summary description for UFN.
	/// </summary>
	public class UFN : CommonObject
	{
		#region Fields
		private FWBS.OMS.OMSFile _file = null;
		private string _ufncode = "";
		private string _headufncode = "";
		private FWBS.Common.DateTimeNULL _dateufnfrom = DBNull.Value;
		private SearchEngine.SearchList _search = null;
		#endregion
		
		#region Constructors
		public UFN(long FileID)
		{
			_file = FWBS.OMS.OMSFile.GetFile(FileID);
			_ufncode = Convert.ToString(_file.ExtendedData["EXTFILLA"].GetExtendedData("MatLAUFN"));
			if (_ufncode == "" || Exists(_ufncode) == false)
				Create();
			else
				throw new OMSException2("ERRUFNALREADYSET","Cannot create a new UFN one is already set. Please delete and try again.","");
			SetExtraInfo("clid",_file.ClientID);
		}

		public UFN(FWBS.OMS.OMSFile File)
		{
			_file = File;
			_ufncode = Convert.ToString(_file.ExtendedData["EXTFILLA"].GetExtendedData("MatLAUFN"));
			if (_ufncode == "" || Exists(_ufncode) == false)
				Create();
			else
				throw new OMSException2("ERRUFNALREADYSET","Cannot create a new UFN one is already set. Please delete and try again.","");
			SetExtraInfo("clid",_file.ClientID);
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public UFN(string UFNCode)
		{
            if (string.IsNullOrWhiteSpace(UFNCode))
            {
                throw new OMSException("ERRUFNNOTEXISTS", "UFN information is not associated with the selected %FILE%");
            }
            else
            {
                FetchHeadUFN(UFNCode);
                while (Convert.ToString(GetExtraInfo("ufnCode")) != Convert.ToString(GetExtraInfo("ufnHeadCode")))
                {
                    FetchHeadUFN(Convert.ToString(GetExtraInfo("ufnHeadCode")));
                }
                _ufncode = UFNCode;
                _headufncode = Convert.ToString(GetExtraInfo("ufnHeadCode"));
            }
		}

 		public UFN(string UFNCode, FWBS.OMS.OMSFile File)
		{
			_file = File;
			FetchHeadUFN(UFNCode);
			while (Convert.ToString(GetExtraInfo("ufnCode")) != Convert.ToString(GetExtraInfo("ufnHeadCode")))
			{
				FetchHeadUFN(Convert.ToString(GetExtraInfo("ufnHeadCode")));
			}
			_ufncode = UFNCode;
			_headufncode = Convert.ToString(GetExtraInfo("ufnHeadCode"));
		}

		public override void Update()
		{
			if (_ufncode == "")
			{
				// Create a UFN Code
				System.Data.IDataParameter [] param = new System.Data.IDataParameter[4];
				param[0] = Session.CurrentSession.Connection.AddParameter("@feeID", System.Data.SqlDbType.BigInt, 15, Session.CurrentSession.CurrentFeeEarner.ID);
				param[1] = Session.CurrentSession.Connection.AddParameter("@indate", System.Data.SqlDbType.DateTime, 15, this.DateUFNFrom);
				param[2] = Session.CurrentSession.Connection.AddParameter("@clid", System.Data.SqlDbType.BigInt, 15, _file.ClientID);
				param[3] = Session.CurrentSession.Connection.AddParameter("@ufnheadcode", System.Data.SqlDbType.NVarChar, 10, this.HeadUFNCode);
				System.Data.DataTable output = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCDSNextNumber","NEW",param);
				_ufncode = Convert.ToString(output.Rows[0][0]);
				if (_headufncode == "") 
				{
					_headufncode = _ufncode;
					SetExtraInfo("UFNHEADCODE",_headufncode);
				}
				if (Convert.ToString(GetExtraInfo("UFNCode")) == "")
					SetExtraInfo("UFNCode",_ufncode);
				}
			if (_file != null) 
			{
				_file.ExtendedData["EXTFILLA"].SetExtendedData("MatLAUFN",_ufncode);
				_file.Update();
			}
			base.Update();
		}

		#endregion

		#region Overrides

		public override void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

			DataTable changes = _data.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(UFNCode, changes.Rows[0]);
            else
                Fetch(UFNCode, null);
		}

		/// <summary>
		/// No Enquiry form is used to edit this object
		/// </summary>
		protected override string DefaultForm
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// The Primary Field
		/// </summary>
		public override string FieldPrimaryKey
		{
			get
			{
				return "ufnid";
			}
		}

		protected override string PrimaryTableName
		{
			get
			{
				return "UFN";
			}
		}

		protected override string SelectStatement
		{
			get
			{
				return "select * from dbUfn";
			}
		}

		public override void Create()
		{
			base.Create ();
		}




		/// <summary>
		/// Fetches an instance of the object.
		/// </summary>
		/// <param name="id">The unique identifier value of the object to be fetched.</param>
		protected override void Fetch(object id)
		{
            Fetch(id, null);
		}

        private void Fetch(object id, DataRow merge)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("ID", id);
            DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(SelectStatement + " where UFNCode = @ID", PrimaryTableName, paramlist);
            data.PrimaryKey = new DataColumn[] { data.Columns[this.FieldPrimaryKey] };
            data.DefaultView.RowStateFilter = DataViewRowState.OriginalRows;
            data.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
            if ((data == null) || (data.Rows.Count == 0))
            {
                throw GetMissingException(id);
            }

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _data = data;
        }


		public void FetchHeadUFN(string UFNCode)
		{
			Fetch(UFNCode);
			_headufncode = Convert.ToString(GetExtraInfo("ufnHeadCode"));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("ClaimCode",this.ClaimCode));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("OffenceCode",this.OffenceCode));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("ClassOfWork",this.ClassOfWork));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("OffenceCode",this.OffenceCode));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("OutcomeCode",this.OutcomeCode));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("TotalSuspects",this.TotalSuspects));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("TotalAttendances",this.TotalAttendances));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("PoliceCourtIdent",this.PoliceCourtIdent));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("DutySolicitor",this.DutySolicitor));
			OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("YouthCourt",this.YouthCourt));
		}

		#endregion

		#region Properties
		[EnquiryEngine.EnquiryUsage(true)]
		public string UFNCode
		{
			get
			{
				return _ufncode;
			}
			set
			{
				_ufncode = value;
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public string HeadUFNCode
		{
			get
			{
				return _headufncode;
			}
			set
			{
				if (_headufncode != value)
				{
					_headufncode = value;
					FetchHeadUFN(_headufncode);
				}
			}
		}
		
		[EnquiryEngine.EnquiryUsage(true)]
		public string WhatIsThisUFN
		{
			get
			{
				if (_headufncode.ToUpper() == _ufncode.ToUpper())
					return FWBS.OMS.Session.CurrentSession.Resources.GetResource("LEADUFN","This is a Lead UFN","").Text;
				else
					return FWBS.OMS.Session.CurrentSession.Resources.GetResource("SECFN","This is a Secondary UFN","").Text;
			}
		}
		
		[EnquiryEngine.EnquiryUsage(true)]
		public FWBS.Common.DateTimeNULL DateUFNFrom
		{
			get
			{
				return _dateufnfrom;
			}
			set
			{
				_dateufnfrom = value;
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public string ClassOfWork
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ufnClass"));
			}
			set
			{
				SetExtraInfo("ufnClass",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public string OffenceCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ufnOffCode"));
			}
			set
			{
				SetExtraInfo("ufnOffCode",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public string ClaimCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ufnCLCode"));
			}
			set
			{
				SetExtraInfo("ufnCLCode",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public string OutcomeCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ufnOutCode"));
			}
			set
			{
				SetExtraInfo("ufnOutCode",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public int TotalSuspects
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("ufnSuspects"),0);
			}
			set
			{
				SetExtraInfo("ufnSuspects",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public int TotalAttendances
		{
			get
			{
				return FWBS.Common.ConvertDef.ToInt32(GetExtraInfo("ufnAttendances"),0);
			}
			set
			{
				SetExtraInfo("ufnAttendances",value);
			}
		}
		
		[EnquiryEngine.EnquiryUsage(true)]
		public string PoliceCourtIdent
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ufnIdentifier"));
			}
			set
			{
				SetExtraInfo("ufnIdentifier",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public bool DutySolicitor
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("ufnDuty"),false);
			}
			set
			{
				SetExtraInfo("ufnDuty",value);
			}
		}

		[EnquiryEngine.EnquiryUsage(true)]
		public bool YouthCourt
		{
			get
			{
				return FWBS.Common.ConvertDef.ToBoolean(GetExtraInfo("ufnYouth"),false);
			}
			set
			{
				SetExtraInfo("ufnYouth",value);
			}
		}

		public FWBS.OMS.SearchEngine.SearchList SearchList
		{
			get
			{
				FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
				param.Add("UFNCODE",this.HeadUFNCode);
				_search = new FWBS.OMS.SearchEngine.SearchList("SCHFILUFNLNK",_file,param);
				return _search;
			}
		}

		public bool IsLeadUFN
		{
			get
			{
				return (_headufncode.ToUpper() == _ufncode.ToUpper());
			}
		}

		public int LinkedUFNS
		{
			get
			{
				if (this.IsLeadUFN)
				{
					DataTable _linked = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT dbo.dbFileLegal.fileID, dbo.dbUfn.ufnHeadCode FROM dbo.dbUfn INNER JOIN dbo.dbFileLegal ON dbo.dbUfn.ufnCode = dbo.dbFileLegal.MatLAUFN WHERE ufnHeadCode = @Code","LINKED",false,new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 20, this.HeadUFNCode)});
					return _linked.Rows.Count;	
				}
				else
				{
					return 0;
				}
			}
		}

		public int LinkedCDSClaim
		{
			get
			{
				DataTable _linked = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT DISTINCT UFNID FROM dbCDSClaim WHERE ufnCode = @Code","LINKED",false,new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 20, this.HeadUFNCode)});
				return _linked.Rows.Count;	
			}
		}
		#endregion

		#region Methods
		public new void Delete()
		{
			AskEventArgs askarg = new FWBS.OMS.AskEventArgs("RUSUFNFILE","Are you sure you wish to Remove the UFN from the %FILE%?","",AskResult.Yes);
			FWBS.OMS.Session.CurrentSession.OnAsk(this,askarg);
			if (askarg.Result == AskResult.Yes)
			{
				// Validate the ability to remove the UFN by checking basic elements are in place.
				if (this.IsLeadUFN && this.LinkedUFNS > 1)
				{
					throw new OMSException("UFNLINKDEL","This UFN has linked UFN's that must be deleted first!");
				}

				if (this.LinkedCDSClaim > 0)
				{
					throw new OMSException("UFNCDS11ONFILE", "This UFN has " + Convert.ToString(this.LinkedCDSClaim) + " CDS 11 Claim(s) on file You can't delete!");
				}

				if (_file != null) 
				{
					_file.ExtendedData["EXTFILLA"].SetExtendedData("MatLAUFN",DBNull.Value);
					_file.Update();
				}

				try
				{
					Session.CurrentSession.Connection.ExecuteSQL("Delete from DBUFN where UFNCODE = @ufncode",new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("UFNCODE", SqlDbType.NVarChar, 20, this.UFNCode)});
				}
				catch (Exception ex)
				{
					throw new OMSException(ex,"ERRDELUFN","Error Deleting UFN : " + this.UFNCode);
				}

				base.Update();

                
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
