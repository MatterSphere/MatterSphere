using System;
using System.Data;

namespace FWBS.OMS
{
	/// <summary>
	/// A date wizard object will auto generate a form that performs simple caluclation 
	/// of dates from a base date.  The date marked as key dates will be stored for later use.
	/// </summary>
	public class DateWizard : IDisposable
	{
		#region Fields

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataSet _datewiz = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbdatewizdates";

		/// <summary>
		/// SQL statment used to update key dates.
		/// </summary>
		internal const string Sql_KeyDates = "select * from dbkeydates";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		internal const string Table = "DATEWIZARD";
	
		/// <summary>
		/// The actual date configuration table name.
		/// </summary>
		internal const string Table_Dates = "DATES";

		/// <summary>
		/// The table name for the table that contains the key dates.
		/// </summary>
		internal const string Table_KeyDates = "KEYDATES";
	
		/// <summary>
		/// The table name for the table that contains the questions.
		/// </summary>
		internal const string Table_Questions = "QUESTIONS";

		/// <summary>
		/// The table name for the table that contains the data.
		/// </summary>
		internal const string Table_Data = "DATA";

		/// <summary>
		/// The tasks created associated to the date wizard.
		/// </summary>
		private Tasks _tasks = null;

		/// <summary>
		/// The appointments created associated to the date wizard.
		/// </summary>
		private Appointments _apps =null;

		/// <summary>
		/// The file object that will own the key dates.
		/// </summary>
		private OMSFile _file = null;

		/// <summary>
		/// The fee earner that is to be used.
		/// </summary>
		private FeeEarner _feeEarner = null;

		/// <summary>
		/// Additional notage.
		/// </summary>
		private string _additionalNotes = "";

		/// <summary>
		/// A unique identifier to group key dates together.
		/// </summary>
		private Guid _guid = Guid.NewGuid();

		/// <summary>
		/// Client file note footer information.
		/// </summary>
		private System.Text.StringBuilder _notes = new System.Text.StringBuilder();

		/// <summary>
		/// If exists, the enquiry used for the date wizard.
		/// </summary>
		private EnquiryEngine.Enquiry _form = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		private DateWizard()
		{
		}

		/// <summary>
		/// Fetches an existing date wizard from the database.
		/// </summary>
		/// <param name="code">The date wizard code to pull back.</param>
		/// <param name="file">The file associated to the date wizard.</param>
		/// <param name="feeEarner">The fee earner that owns the date.</param>
        private DateWizard(string code, OMSFile file, FeeEarner feeEarner)
		{
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, code);
			pars[1] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			_datewiz = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprDateWizardBuilder", false, new string[1]{Table}, pars);

			if (_datewiz == null || _datewiz.Tables[Table].Rows.Count ==0)
			{
				throw new OMSException2("ERRDATEWIZMISS", "The specified datewizard does not appear to exist.");
			}

			_datewiz.Tables[1].TableName = Table_Dates;
			_datewiz.Tables[2].TableName = Table_KeyDates;
			_datewiz.Tables[3].TableName = Table_Questions;

			DataTable data = new DataTable(Table_Data);
			foreach (DataRow row in _datewiz.Tables[Table_Questions].Rows)
			{
				DataColumn col = new DataColumn(Convert.ToString(row["quname"]), typeof(Common.DateTimeNULL));
				data.Columns.Add(col);
			}
			data.Rows.Add(data.NewRow());
			data.ColumnChanged += new DataColumnChangeEventHandler(data_ColumnChanged);
			_datewiz.Tables.Add(data);
			_datewiz.AcceptChanges();

			_tasks = new Tasks(file);
			_apps = new Appointments(file);

			_file = file;
			_feeEarner = feeEarner;
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the date wizard code.
		/// </summary>
		public string Code 
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typecode"));
			}
		}

		/// <summary>
		/// Gets the description of the current date wizard.
		/// </summary>
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("typedesc"));
			}
		}

		/// <summary>
		/// The enquiry form based on the date wizard item.
		/// </summary>
		public EnquiryEngine.Enquiry Enquiry
		{
			get
			{
				if (_form == null)
				{
					Common.KeyValueCollection pars = new FWBS.Common.KeyValueCollection();
					pars.Add("DATEWIZARD", this);
					_form = EnquiryEngine.Enquiry.GetEnquiry(Convert.ToString(GetExtraInfo("typeenquiry")), this, EnquiryEngine.EnquiryMode.Add, pars);
				}
				return _form;
			}
		}

		/// <summary>
		/// Gets a flag that specifies whether this date wizard is a built in system form.
		/// </summary>
		public bool IsSystem
		{
			get
			{
				return Convert.ToBoolean(GetExtraInfo("typeSystem"));
			}
		}

		/// <summary>
		/// Gets a flag the specifies whether the date wizard has its own custom enquiry form.
		/// </summary>
		public bool HasEnquiryForm
		{
			get
			{
				return (GetExtraInfo("typeEnquiry") != DBNull.Value);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the questions data table of the date wizards structure so that the 
		/// calling UI application can render the wizard.
		/// </summary>
		/// <returns>A data table full of the rendering questions.</returns>
		public DataTable GetQUESTIONSTable()
		{
			return _datewiz.Tables[Table_Questions];
		}

		/// <summary>
		/// Gets the DATA data table of the date wizards structure.
		/// </summary>
		/// <returns>A data table full the data to be contained.</returns>
		public DataTable GetDATATable()
		{
			return _datewiz.Tables[Table_Data];
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		private object GetExtraInfo(string fieldName)
		{
			object val = _datewiz.Tables[Table].Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}
	

		/// <summary>
		/// Creates a key date.
		/// </summary>
		/// <param name="description">The description of the key date.</param>
		/// <param name="date">The date at which the key date occurs.</param>
		/// <param name="createAppointment">Creates an appointment reminder.</param>
		/// <param name="createTask">Creates a task reminder.</param>
		/// <param name="createKeyDate">Is an actual key date.</param>
		public void CreateDate(string description, DateTime date, bool createKeyDate, bool createTask, bool createAppointment)
		{

			if (createKeyDate)
			{
				DataRow kd = _datewiz.Tables[Table_KeyDates].NewRow();
				kd["fileid"] = _file.ID;
				kd["kdrelatedid"] = _guid;
				kd["kdType"] = Code;
				kd["kddesc"] = description;
				kd["kddate"] = date;
				kd["kdactive"] = true;
				kd["Created"] = DateTime.Now;
				kd["CreatedBy"] = Session.CurrentSession.CurrentUser.ID;
				_datewiz.Tables[Table_KeyDates].Rows.Add(kd);
			}

			//Add to the tasks.
			if (createTask)
			{
				_tasks.Add(_feeEarner, _guid, "KEYDATE", description, date, _notes.ToString());
			}
					
			//Add to appointments.
			if (createAppointment)
			{
                _apps.Add(_feeEarner, _guid, "KEYDATE", description, date, _notes.ToString(), GetUserTimeZone());
			}

		}

        /// <summary>
        /// Gets local time zone of user or related to ExchangeSync settings
        /// </summary>
        /// <returns>time zone identifier</returns>
        private string GetUserTimeZone()
        {
            string timeZone = TimeZoneInfo.Local.Id;
            if (Session.CurrentSession.IsPackageInstalled("ExchangeSync"))
            {
                string sql = "SELECT usrTimeZone FROM fdExchangeSync WHERE usrID = @UserID";
                IDataParameter[] sqlParams = new IDataParameter[1] { Session.CurrentSession.Connection.CreateParameter("UserID", Session.CurrentSession.CurrentUser.ID) };
                string exchangeTimeZone = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar(sql, sqlParams));
                if (!string.IsNullOrWhiteSpace(exchangeTimeZone))
                    timeZone = exchangeTimeZone;
            }
            return timeZone;
        }

		/// <summary>
		/// Creates the key dates from the dates that they have entered.
		/// </summary>
		private void AutoCreateDates()
		{
			if (!HasEnquiryForm)
			{
				foreach (DataRow row in _datewiz.Tables[Table_Dates].Rows)
				{
					string code = Convert.ToString(row["datecode"]);
					string desc = "";
 
					DataView q = new DataView(GetQUESTIONSTable());
					q.RowFilter = "quname = '" + Common.SQLRoutines.RemoveRubbish(code) + "'";
					if (q.Count > 0)
					{
						desc = Convert.ToString(q[0]["qudesc"]);
					}
		
		
					if (_datewiz.Tables[Table_Data].Rows[0][code] is DBNull && Convert.ToBoolean(row["dateeditable"]))
					{
						throw new EnquiryEngine.EnquiryValidationFieldException(HelpIndexes.EnquiryRequiredField, new EnquiryEngine.ValidatedField(code, desc, ""));
					}

					Common.DateTimeNULL date = Common.ConvertDef.ToDateTimeNULL(_datewiz.Tables[Table_Data].Rows[0][code], DBNull.Value);

					if (!date.IsNull)
						CreateDate(desc, (DateTime)date, Common.ConvertDef.ToBoolean(row["datekey"], false), Common.ConvertDef.ToBoolean(row["dateastask"], false), Common.ConvertDef.ToBoolean(row["dateasappointment"], false));

				}
			}
		}

		/// <summary>
		/// Updates the date wizard to update key dates, tasks and appointments.
		/// </summary>
		public void Update()
		{
			try
			{
				AutoCreateDates();

				Session.CurrentSession.Connection.Connect(true);
				if (_datewiz.Tables[Table_KeyDates].GetChanges() != null)
					Session.CurrentSession.Connection.Update(_datewiz.Tables[Table_KeyDates], Sql_KeyDates);
			
				_tasks.Update();
				_apps.Update();
			}
			finally
			{
				Session.CurrentSession.Connection.Disconnect(true);
			}
		}

        /// <summary>
        /// Creates notes for key dates.
        /// </summary>
        /// <param name="additionalNotes">Additional notes for the dates.</param>
        public void CreateNotes(string additionalNotes)
        {
            _additionalNotes = additionalNotes;

            _notes.Append(additionalNotes);
            _notes.Append(Environment.NewLine);
            _notes.Append("---------------------------------");
            _notes.Append(Environment.NewLine);
            _notes.Append(Session.CurrentSession.Resources.GetResource("FILEDESC", "%FILE% Description:", "").Text);
            _notes.Append(" ");
            _notes.Append(_file.FileDescription);
            _notes.Append(Environment.NewLine);
            _notes.Append(Session.CurrentSession.Resources.GetResource("CLNAME", "%CLIENT% Name:", "").Text);
            _notes.Append(" ");
            _notes.Append(_file.Client.ClientName);
            _notes.Append(Environment.NewLine);
            _notes.Append(Session.CurrentSession.Resources.GetResource("TELNO", "Tel No:", "").Text);
            _notes.Append(" ");
            _notes.Append(_file.Client.DefaultContact.DefaultTelephoneNumber);
            _notes.Append(Environment.NewLine);
            _notes.Append(Session.CurrentSession.Resources.GetResource("FAX", "Fax:", "").Text);
            _notes.Append(" ");
            _notes.Append(_file.Client.DefaultContact.DefaultFaxNumber);
        }

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets an existing date wizard from the system.
		/// </summary>
		/// <param name="code">The date wizard code to use.</param>
		/// <param name="file">The file to be used for the date wizards.</param>
		/// <param name="feeEarner">The Fee Earner that will own the dates.</param>
		/// <returns>A date wizard object ready to be rendered.</returns>
		public static DateWizard GetDateWizard(string code, OMSFile file, FeeEarner feeEarner)
		{
			Session.CurrentSession.CheckLoggedIn();
			return new DateWizard(code, file, feeEarner);
		}

		/// <summary>
		/// Trash Delete Key Date and Trash Delete Tasks
		/// </summary>
        /// <param name="fileid"></param>
		/// <param name="kdid">The Key Date ID</param>
		public static bool DeleteKeyDate(Int64 fileid, Int64 kdid)
		{
			AskEventArgs askargs = new AskEventArgs("AREYOUSURE","Are you sure you wish to Delete ?","",FWBS.OMS.AskResult.Yes);
			Session.CurrentSession.OnAsk(null,askargs);
			if (askargs.Result == FWBS.OMS.AskResult.Yes)
			{
				IDataParameter[] pars = new IDataParameter[2];
				pars[0] = Session.CurrentSession.Connection.AddParameter("KDID", SqlDbType.BigInt, 15, kdid);
				Session.CurrentSession.Connection.ExecuteProcedure("sprDeleteDateWizard", pars);
				OMSFile file = FWBS.OMS.OMSFile.GetFile(fileid);
				file.AddEvent("KEYDEL","Key Date Deleted","");
				file.Update();
                return true;
            }

            return false;
        }

		/// <summary>
		/// Trash Restore Key Date and Restore Trashed Tasks
		/// </summary>
		/// <param name="kdid">The Key Date ID</param>
		public static void RestoreKeyDate(Int64 kdid)
		{
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("KDID", SqlDbType.BigInt, 15, kdid);
			Session.CurrentSession.Connection.ExecuteProcedure("sprRestoreDateWizard", pars);
		}
		#endregion

		#region Captured Events

		/// <summary>
		/// Captures the column changed event of the date wizard item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void data_ColumnChanged(object sender, DataColumnChangeEventArgs e)
		{
			DataView vw = new DataView (_datewiz.Tables[Table_Dates]);
			vw.RowFilter = "dateeditable = 'true' and datecode = '" + Common.SQLRoutines.RemoveRubbish(e.Column.ColumnName) + "'";
			if (vw.Count > 0)
			{
				if (Convert.ToString(vw[0]["datecode"]) == e.Column.ColumnName)
				{
					CalculateDates(e.Column.ColumnName, e.ProposedValue);
				}
			}
		}


		#endregion

		#region System Date Calculations

		/// <summary>
		/// Calculates the dates from the base editable date.
		/// </summary>
		/// <param name="code">The base date configuration row.</param>
		/// <param name="proposed">The proposed date.</param>
		private void CalculateDates(string code, object proposed)
		{
			foreach (DataRow row in _datewiz.Tables[Table_Dates].Rows)
			{
				string datecode = Convert.ToString(row["datecode"]);
				if (datecode == code || Convert.ToBoolean(row["dateeditable"]))
					continue;

				if (proposed != DBNull.Value)
				{
					object obj = _datewiz.Tables[Table_Data].Rows[0][Convert.ToString(row["datecalcfrom"])];
					Common.DateTimeNULL newDate = Common.ConvertDef.ToDateTimeNULL(obj, DBNull.Value);
					Common.DateTimeNULL calcDate = newDate;
					string measure = Convert.ToString(row["datemeasure"]);
					double units = Convert.ToDouble(row["dateunits"]);
					switch (measure)
					{
						case "D":	//Day
							calcDate = newDate.AddDays(units);
							break;
						case "W":	//Week
							calcDate = newDate.AddDays(units * 7d);
							break;
						case "M":	//Month
							calcDate = newDate.AddMonths(Convert.ToInt32(units));
							break;
						case "Q":	//Quarter
							calcDate = newDate.AddMonths(Convert.ToInt32(units) * 3);
							break;
						case "Y":;	//Year
							calcDate = newDate.AddYears(Convert.ToInt32(units));
							break;
					}
					_datewiz.Tables[Table_Data].Rows[0][datecode] = calcDate;
				}
				else
					_datewiz.Tables[Table_Data].Rows[0][datecode] = DBNull.Value;
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
				if (_datewiz != null)
				{
					_datewiz.Dispose();
					_datewiz = null;
				}

				_form = null;
				_tasks = null;
				_apps = null;
				_feeEarner = null;
			}
			
			//Free any unmanaged objects.

		}

	

		#endregion

	}
}
