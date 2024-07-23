using System;
using System.Data;
using System.Data.SqlClient;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    /// <summary>
    /// 7000 A class that describes the fund type information within the system. 
    /// Fund types will be used to populate default information into a new OMSfile
    /// object.
    /// </summary>
    public class FundType : LookupTypeDescriptor, IExtraInfo, IUpdateable, IDisposable
	{
		#region Events
		public event EventHandler Change;
		#endregion
		
		#region Fields

		/// <summary>
		/// Internal data source.
		/// </summary>
		protected DataTable _fundType = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbfundtype";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "FUNDTYPE";

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new fund type object.
		/// </summary>
		public FundType()
		{
			IDataParameter [] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", System.Data.SqlDbType.NVarChar, 15, "");
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("CURRENCY", System.Data.SqlDbType.Char, 3, "");
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			_fundType = Session.CurrentSession.Connection.ExecuteProcedureTable("sprFundType", Table, true, paramlist);
			
			//Add a new record.
			_fundType.Columns["ftcurISOCode"].DefaultValue = "GBP";
			_fundType.Columns["ftActive"].DefaultValue = true;
			_fundType.Columns["ftAgreementCode"].DefaultValue = "AGREEMENTDATE";
			_fundType.Columns["ftRatePerUnit"].DefaultValue = 0;
			_fundType.Columns["ftLegalAidCharged"].DefaultValue = false;
			_fundType.Columns["ftWarningPerc"].DefaultValue = 100;
			_fundType.Columns["ftBand"].DefaultValue = 3;
			_fundType.Columns["ftRefCode"].DefaultValue = "CLIENTREF";
			_fundType.Columns["ftCLCode"].DefaultValue = "CREDITLIMIT";
			Global.CreateBlankRecord(ref _fundType, true);
		}

		/// <summary>
		/// Initialised an existing fund type object with the specified identifier cloned from another.
		/// </summary>
		protected internal FundType (FundType clone) : this()
		{
			_fundType.Columns["ftcldesc"].ReadOnly = false;
			_fundType.Columns["ftdesc"].ReadOnly = false;
			_fundType.Columns["ftrefdesc"].ReadOnly = false;
			_fundType.Columns["ftagreementdesc"].ReadOnly = false;
			_fundType.Rows[0].ItemArray = clone.GetDataTable().Rows[0].ItemArray;
		}
		
		/// <summary>
		/// Initialised an existing fund type object with the specified identifier.
		/// </summary>
		/// <param name="CodeAndCurrencyCode">The Fund Code &amp; Currency Code Seperated by a Pipe (|) e.g TEST|GBP</param>
		protected internal FundType (string CodeAndCurrencyCode)
		{
            Fetch(CodeAndCurrencyCode, null);
	    }

		/// <summary>
		/// Initialised an existing fund type object with the specified identifier.
		/// </summary>
		/// <param name="code">Fund Type code.</param>
		/// <param name="currencyCode">The currency ISO code.</param>
		protected internal FundType (string code, string currencyCode)
		{
            Fetch(code, currencyCode, null);

	    }

        private void Fetch(string CodeAndCurrencyCode, DataRow merge)
        {
            IDataParameter[] paramlist = new IDataParameter[3];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", System.Data.SqlDbType.NVarChar, 15, GetCode(CodeAndCurrencyCode, 0));
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("CURRENCY", System.Data.SqlDbType.Char, 3, GetCode(CodeAndCurrencyCode, 1));
            paramlist[2] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            var data = Session.CurrentSession.Connection.ExecuteProcedureTable("sprFundType", Table, paramlist);

            if ((data == null) || (data.Rows.Count == 0))
                throw new OMSException(HelpIndexes.FundTypeDoesNotExist, CodeAndCurrencyCode);

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _fundType = data;
        }

        private void Fetch(string code, string currencyCode, DataRow merge)
        {
            IDataParameter [] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("CODE", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("CURRENCY", System.Data.SqlDbType.Char, 3, currencyCode);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			var data = Session.CurrentSession.Connection.ExecuteProcedureTable("sprFundType", Table, paramlist);

			if ((data == null) || (data.Rows.Count == 0)) 
				throw new OMSException(HelpIndexes.FundTypeDoesNotExist, new string[] {code,currencyCode});

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _fundType = data;
        }

		#endregion
       
		#region Properties
		/// <summary>
		/// Description of the FundType
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftdesc"));
			}
		}

		/// <summary>
		/// Gets or Sets the Legal Aid Charged
		/// </summary>
		[LocCategory("LegalAid")]
		[Lookup("LegalAidChrg")]
		public virtual bool LegalAidCharged
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("ftLegalAidCharged"));
				}
				catch
				{
					return false;
				}
			}
			set
			{
				SetExtraInfo("ftLegalAidCharged", value);
			}
		}

				
		/// <summary>
		/// Gets or Sets the Agreement Code
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual string AgreementCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftAgreementCode"));
			}
			set
			{
				SetExtraInfo("ftAgreementCode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the Reference Code.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual string RefCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftRefCode"));
			}
			set
			{
				SetExtraInfo("ftRefCode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the CreditLimit Code.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public virtual string CreditLimitCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftCLCode"));
			}
			set
			{
				SetExtraInfo("ftCLCode", value);
			}
		}

		/// <summary>
		/// Gets the unique fund type code.
		/// </summary>
		[LocCategory("(Details)")]
		[Lookup("FTCode")]
		public virtual string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftcode"));
			}
			set
			{
				SetExtraInfo("ftcode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default credit limit.
		/// </summary>
		[LocCategory("CreditLimit")]
		[Lookup("DefCredLimit")]
		public virtual double DefaultCreditLimit
		{
			get
			{
				try
				{
					return Convert.ToDouble(GetExtraInfo("ftcreditlimit"));
				}
				catch
				{
					return 0f;
				}
			}
			set
			{
				SetExtraInfo("ftcreditlimit", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default rate band.
		/// </summary>
		[LocCategory("LegalAid")]
		[Lookup("DefRateBand")]
		public virtual byte DefaultRateBand
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("ftBand");
				}
				catch
				{
					return 3;
				}
			}
			set
			{
				SetExtraInfo("ftband", value);
			}
		}

		/// <summary>
		/// Gets or Sets the default credit limit warning percentage.
		/// </summary>
		[LocCategory("CreditLimit")]
		[Lookup("DefWarnPerc")]
		public virtual byte DefaultWarningPercentage
		{
			get
			{
				try
				{
					return (byte)GetExtraInfo("ftwarningperc");
				}
				catch
				{
					return 100;
				}
			}
			set
			{
				SetExtraInfo("ftwarningperc", value);
			}
		}


		/// <summary>
		/// Gets or Sets the external account code of the fund type which may be used to link
		/// a fund type to another instance of the fund type in an external database / accounting 
		/// system.
		/// </summary>
		[LocCategory("Data")]
		public virtual string AccountCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftacccode"));
			}
			set
			{
				SetExtraInfo("ftacccode", value);
			}
		}

		/// <summary>
		/// Gets or Sets the the default monetary rate per unit.
		/// </summary>
		[LocCategory("Currency")]
		[Lookup("DefRatePerUnit")]
		public virtual decimal DefaultRatePerUnit
		{
			get
			{
				try
				{
					return Convert.ToDecimal(GetExtraInfo("ftrateperunit"));
				}
				catch
				{
					return 0;
				}
			}
			set
			{
				SetExtraInfo("ftrateperunit", value);
			}
		}

		/// <summary>
		/// Gets the currency code of the fund type.
		/// </summary>
		[LocCategory("Currency")]
		public virtual string CurrencyCode
		{
			get
			{
				return Convert.ToString(GetExtraInfo("ftcurISOcode"));
			}
			set
			{
				SetExtraInfo("ftcurISOcode", value);
			}
		}



		#endregion

		#region IExtraInfo Implementation
		
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public virtual void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_fundType.Rows[0][fieldName] = val;
			if (Change != null)
				Change(this,EventArgs.Empty);
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public virtual object GetExtraInfo(string fieldName)
		{
			object val = _fundType.Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}


		/// <summary>
		/// Returns the specified fields data.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public virtual Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _fundType.Columns[fieldName].DataType;
			}
			catch (Exception ex)
			{
				throw new OMSException2("7001","Error Getting Extra Info Field %1% Probably Not Initialized",ex,true,fieldName);
			}
		}
		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public virtual DataSet GetDataset()
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
		public virtual DataTable GetDataTable()
		{
			return _fundType.Copy();
		}

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (_fundType.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public virtual void Update()
		{
			//Check if there are any changes made before setting the updated
			//and updated by properties then update.
			if (_fundType.GetChanges()!= null)
			{
				//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
				if (_fundType.PrimaryKey == null || _fundType.PrimaryKey.Length == 0)
					_fundType.PrimaryKey = new DataColumn[1]{_fundType.Columns["ftcode"]};
				try
				{
					Session.CurrentSession.Connection.Update(_fundType, Sql);
				}
				catch (ConnectionException cex)
				{
					SqlException sqlex = cex.InnerException as SqlException;
					if (sqlex != null)
						throw new OMSException2("7002", "The Fund Type with the Code '%1%' and Currency Code of '%2%' already exists please change one or the other", "", sqlex, true, this.Code, this.CurrencyCode);
						
					throw;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}


		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public virtual void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Gets the changes of the current object and and refreshes the object
		/// then reapplies the changes to avoid any concurrency issues.  This is in 
		/// theory forcing any changes made to the object.
		/// </summary>
		/// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
		public virtual void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

			DataTable changes = _fundType.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.Code, this.CurrencyCode, changes.Rows[0]);
            else
                Fetch(this.Code, this.CurrencyCode, null);

		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			_fundType.RejectChanges();
		}


		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return (_fundType.GetChanges() != null);
			}
		}

		#endregion

		#region IDisposable Implementation


		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_fundType!= null)
				{
					_fundType.Dispose();
					_fundType = null;
				}
			}
			
			//Dispose unmanaged objects.
		}


		#endregion

		#region Static Methods
		/// <summary>
		/// Returns a fund type object based on the fund type code passed.
		/// </summary>
		/// <param name="CodeAndCurrencyCode">The Fund Code &amp; Currency Code Seperated by a Pipe (|) e.g TEST|GBP</param>
		/// <returns>Returns a fund type object.</returns>
		public static FundType GetFundType(string CodeAndCurrencyCode)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new FundType(CodeAndCurrencyCode);
		}
		
		/// <summary>
		/// Returns a fund type object based on the fund type code passed.
		/// </summary>
		/// <param name="code">Fund type code.</param>
		/// <param name="currencyCode">The currency code to check for.</param>
		/// <returns>Returns a fund type object.</returns>
		public static FundType GetFundType(string code, string currencyCode)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new FundType(code,currencyCode);
		}

		/// <summary>
		/// Retrieves all of the all FundTypes from the database
		/// </summary>
		public static DataTable GetFundTypes()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			string lstsql = "select * ,[ftCode] + '|' + [ftcurISOCode] as ftFundComb, dbo.GetCodeLookupDesc('FUNDTYPE', ftcode, @UI) as [ftdesc], dbo.GetCodeLookupDesc('FTCLDESC', ftclcode, @UI) as [ftcldesc],	dbo.GetCodeLookupDesc('FTREFDESC', ftrefcode, @UI) as [ftrefdesc], dbo.GetCodeLookupDesc('FTAGREEMENT', ftagreementcode, @UI) as [ftagreementdesc] from dbfundtype";
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(lstsql, Table, paramlist);
			return dt;
		}
		
		/// <summary>
		/// Retrieves all of the all FundTypes from the database
		/// </summary>
		public static DataTable GetFundTypes(bool Active)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Active", System.Data.SqlDbType.Bit, 1,Active);
			string lstsql = "select * ,[ftCode] + '|' + [ftcurISOCode] as ftFundComb, dbo.GetCodeLookupDesc('FUNDTYPE', ftcode, @UI) as [ftdesc], dbo.GetCodeLookupDesc('FTCLDESC', ftclcode, @UI) as [ftcldesc],	dbo.GetCodeLookupDesc('FTREFDESC', ftrefcode, @UI) as [ftrefdesc], dbo.GetCodeLookupDesc('FTAGREEMENT', ftagreementcode, @UI) as [ftagreementdesc] from dbfundtype where [ftActive] = @Active";
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(lstsql, Table, paramlist);
			return dt;
		}

		/// <summary>
		/// Does the Code Exist
		/// </summary>
		/// <returns>Boolean.</returns>
		public static bool Exists(string CodeISO)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,0));
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("ISO", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,1));
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbfundtype where ftCode = @Code AND ftcurISOCode = @ISO" ,"EXISTS", paramlist);
			if (dt.Rows.Count > 0)
				return true;
			else
				return false;
		}

		public static bool DeleteType(string CodeISO)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,0));
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("ISO", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,1));
			Session.CurrentSession.Connection.ExecuteSQL("update dbfundtype set ftActive=0 where ftCode = @Code AND ftcurISOCode = @ISO" ,paramlist);
			return true;
		}

		public static bool RestoreType(string CodeISO)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,0));
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("ISO", System.Data.SqlDbType.VarChar, 10, GetCode(CodeISO,1));
			Session.CurrentSession.Connection.ExecuteSQL("update dbfundtype set ftActive=1 where ftCode = @Code AND ftcurISOCode = @ISO" , paramlist);
			return true;
		}

		protected static string GetCode(string Code,int pos)
		{
			return Code.Split("|".ToCharArray())[pos];
		}


		#endregion

	}
}
