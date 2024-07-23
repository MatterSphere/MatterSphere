using System;
using System.ComponentModel;
using System.Data;
using FWBS.OMS.Script;

namespace FWBS.OMS
{
    /// <summary>
    /// A simple object that fetches and reports from the OMS database.
    /// These reports will be cached to the client and version stamped.
    /// </summary>
    public class Report : IDisposable
	{
		#region Fields

		/// <summary>
		/// Underlying schema definition for the report.
		/// </summary>
		protected DataSet _report = null;

		/// <summary>
		/// Primary table name for the underlying data source.
		/// </summary>
		private const string Table = "REPORT";

		/// <summary>
		/// A SQL select statement used to update changes from the report definition.
		/// </summary>
		private const string Sql = "select * from dbreport";

		/// <summary>
		/// The report location.
		/// </summary>
		private System.IO.FileInfo _reportLocation = null;

		/// <summary>
		/// Static data location.
		/// </summary>
		private System.IO.FileInfo _dataLocation = null;

		/// <summary>
		/// The Search List Object
		/// </summary>
		private SearchEngine.SearchList _searchlist = null;

		/// <summary>
		/// Has the Report been Modified
		/// </summary>
		private bool _isreportdirty = false;

		/// <summary>
		/// Holds a reference to the script object for performing Precedent Events.
		/// </summary>
		private Script.ScriptGen _script = null;

        private bool _overrideversion = false;
		#endregion

		#region Constructors

		/// <summary>
		/// The default contructor will create a new rport object.
		/// </summary>
		public Report()
		{
			Session.CurrentSession.CheckLoggedIn();
			Fetch("");
			DataTable dt = _report.Tables[Table];
			Global.CreateBlankRecord(ref dt, true);
			_report.Tables[Table].Columns["rptdesc"].ReadOnly = false;
			SetExtraInfo("Created", System.DateTime.Now);
			SetExtraInfo("CreatedBy", Session.CurrentSession.CurrentUser.ID);
		}

		/// <summary>
		/// Creates an instance of the report object and fetches an existing one from the database.
		/// </summary>
		/// <param name="code">A report code.</param>
		private Report (string code)
		{	
			Fetch(code);
		}

		#endregion

		#region Properties


		/// <summary>
		/// Gets the rport code.
		/// </summary>
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptcode"));
			}
			set
			{
				SetExtraInfo("rptCode",value);
			}
		}

		/// <summary>
		/// Gets the rport code.
		/// </summary>
		public string Notes
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptnotes"));
			}
			set
			{
				SetExtraInfo("rptnotes",value);
			}
		}

		[LocCategory("Design")]
		[Description("Key Words")]
		[Browsable(true)]
		public string[] Keywords
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptKeywords")).Split('|');
			}
			set
			{
				SetExtraInfo("rptKeywords",String.Join("|",value));
			}
		}

        [LocCategory("Design")]
        [Description("Save Search")]
        [Browsable(true)]
        public string SaveSearch
        {
            get
            {
                return this.SearchList.SaveSearch;
            }
            set
            {
                this.SearchList.SaveSearch = value;
            }
        }


		public SearchEngine.SearchList SearchList
		{
			get
			{
				if (_searchlist == null && this.Code != "")
					_searchlist = new SearchEngine.SearchList(this.Code,null,new FWBS.Common.KeyValueCollection());
				return _searchlist;
			}
		}

		/// <summary>
		/// Gets the localized description of the report.
		/// </summary>
		public string Description
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptdesc"));
			}
		}

		/// <summary>
		/// Gets the current version number of the type.
		/// This is used for caching purposes on the client machine.
		/// </summary>
		public long Version 
		{
			get
			{
				try
				{
					return Convert.ToInt64(GetExtraInfo("rptversion"));
				}
				catch
				{
					return 0;
				}
			}
            set
            {
                if (value != Version)
                {
                    SetExtraInfo("rptversion", value);
                    _overrideversion = true;
                }
            }
		}

		/// <summary>
		/// Gets the publishers name of the report.
		/// </summary>
		public string Publisher
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptpubname"));
			}
		}


		public bool HasReport
		{
			get
			{
				return GetExtraInfo("rptblob") != DBNull.Value;
			}
		}

		/// <summary>
		/// Gets the file schema
		/// </summary>
		public System.IO.FileInfo ReportLocation
		{
			get
			{
				return _reportLocation;
			}
			set
			{
				System.IO.FileStream file = ((System.IO.FileInfo)value).OpenRead();
				byte[] bytes = new byte[file.Length];
				file.Read(bytes, 0, bytes.Length);
				file.Close();
				SetExtraInfo("rptblob", bytes);
				FetchReportfromDatabase(false);
			}
		}

		/// <summary>
		/// Gets the file schema
		/// </summary>
		public System.IO.FileInfo XMLLocation
		{
			get
			{
				if (_dataLocation == null)
				{
					string dl = Global.GetCachePath() + "reports";
					dl = dl + this.Code + "." + Global.ReportDataExt;
					_dataLocation  = new System.IO.FileInfo(dl);
				}
				return _dataLocation;
			}
			set
			{
				System.IO.FileStream file = ((System.IO.FileInfo)value).OpenRead();
				byte[] bytes = new byte[file.Length];
				file.Read(bytes, 0, bytes.Length);
				file.Close();
				SetExtraInfo("rptXML", bytes);
				FetchDatafromDatabase(false);
			}
		}

		/// <summary>
		/// Gets a string value that indicates whether the precedent has an attached script
		/// </summary>
		[Browsable(false)]
		public virtual bool HasScript
		{
			get
			{
				try
				{
					if (Convert.ToString(GetExtraInfo("rptScript")) == "")
						return false;
					else
						return true;
				}
				catch
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Gets or Sets the script name.
		/// </summary>
		[LocCategory("Script")]
		public virtual string ScriptName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("rptScript"));
			}
			set
			{
				SetExtraInfo("rptScript",value);
				this.Script.Code=value;
				OnScriptChanged();
			}
		}

		/// <summary>
		/// Gets the enquiry script type of the of the current enquiry form.
		/// </summary>
		[Browsable(false)]
		public ScriptGen Script
		{
			get
			{
				if (_script == null)
				{
					if (HasScript && ScriptGen.Exists(Convert.ToString(GetExtraInfo("rptScript"))))
					{
						_script = ScriptGen.GetScript(Convert.ToString(GetExtraInfo("rptScript")));
					}
					else
					{
						_script = new ScriptGen(Convert.ToString(GetExtraInfo("rptScript")),"REPORT");
					}
				}
				return _script;
			}
		}

		public DataRow DataRow
		{
			get
			{
				return _report.Tables[Table].Rows[0];
			}
		}

		/// <summary>
		/// Create a New Script object for the Precedent
		/// </summary>
		public void NewScript()
		{
			_script = null;
		}
		#endregion

		#region Methods

		public virtual void EditReport()
		{
			FetchDatafromDatabase(false);
			if (this.ReportLocation == null)
			{
				Report r = Report.GetReport(Global.ReportTemplate,null,null);
				this.ReportLocation = r.ReportLocation;
				r = null;
			}

		    var startInfo = new System.Diagnostics.ProcessStartInfo(this.ReportLocation.FullName);
		    if (string.IsNullOrEmpty(startInfo.Arguments))
		    {
		        System.Diagnostics.Process process = new System.Diagnostics.Process {StartInfo = startInfo};
		        process.Start();
		        _isreportdirty = true;
            }
		}

		/// <summary>
		/// Updates and persist the report data schema into the database.
		/// </summary>
		public void Update ()
		{
			if (_report.Tables[Table].GetChanges() != null)
			{
				if (_isreportdirty)
				{
					_isreportdirty = false;
					this.ReportLocation = new System.IO.FileInfo(_reportLocation.FullName);
				}
				SetExtraInfo("Updated", System.DateTime.Now);
				SetExtraInfo("UpdatedBy", Session.CurrentSession.CurrentUser.ID);
				if (_overrideversion == false)
                    SetExtraInfo("rptversion", Version + 1);
                _overrideversion = false;

				Session.CurrentSession.Connection.Update(_report, Table, Sql);
			}
		}

		/// <summary>
		/// Gets a value from the underlying data.
		/// </summary>
		/// <param name="fieldName">The field name to access.</param>
		/// <returns>The value to be returned.</returns>
		private object GetExtraInfo(string fieldName)
		{
			try
			{
				object val = _report.Tables[Table].Rows[0][fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Fetches the Report from the Database
		/// </summary>
		private void FetchDatafromDatabase(bool local)
		{
			string dl = Global.GetCachePath() + "reports";
			dl = dl + this.Code + "." + Global.ReportDataExt;
			_dataLocation  = new System.IO.FileInfo(dl);
			
			if (_report.Tables[Table].Rows[0]["rptXML"] != DBNull.Value)
			{
				byte[] bytes = (byte[])_report.Tables[Table].Rows[0]["rptXML"];
				// Open a stream for the image and write the bytes into it
				System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes, true);
				stream.Write(bytes, 0, bytes.Length);

				System.IO.FileStream file = new System.IO.FileStream(dl, System.IO.FileMode.OpenOrCreate);
				stream.WriteTo(file);
				file.Close();
				stream.Close();
			}
			else
				throw new OMSException2("42001","The DataBuilder has not been tested or Completed","");
		}

		/// <summary>
		/// Fetches the Report from the Database
		/// </summary>
		private void FetchReportfromDatabase(bool local)
		{
			string rl = Global.GetCachePath() + "reports";
			rl = rl + this.Code + "." + Global.ReportExt;
			
			if (_report.Tables[Table].Rows[0]["rptblob"] != DBNull.Value)
			{
				if (System.IO.File.Exists(rl) == false || local == false)
				{
					byte[] bytes = (byte[])_report.Tables[Table].Rows[0]["rptblob"];

					// Open a stream for the image and write the bytes into it
					System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes, true);
					stream.Write(bytes, 0, bytes.Length);

					System.IO.FileStream file = new System.IO.FileStream(rl, System.IO.FileMode.OpenOrCreate);
					stream.WriteTo(file);
					file.Close();
					stream.Close();
				}
				_reportLocation = new System.IO.FileInfo(rl);
			}
		}


		/// <summary>
		/// Sets a value in the underlying data. 
		/// </summary>
		/// <param name="fieldName">The field name to set.</param>
		/// <param name="val">The value to set.</param>
		private void SetExtraInfo(string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_report.Tables[Table].Rows[0][fieldName] = val;
		}

		/// <summary>
		/// Gets a data set with a few tables explaining how to layout the configurable type.
		/// </summary>
		/// <param name="code">Configurable type code.</param>
		/// <returns>A data set object.</returns>
		private void Fetch(string code)
		{

			DataSet ds = null;		//Internal schema used.
			DataSet fds = null;		//Cached schema version.
			long version = 0;		//Version specifier.
			bool local = false;

			//Loads the cached version of the report schema and gets the version
			//from the header information.  If there is an error opening the file
			//then set the version is set to zero.  The enquiry form will then be 
			//completely refreshed from the database.
			try
			{
				fds = Global.GetCache("reports" + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name,  code.ToString() + "." + Global.CacheExt);
                if (fds != null)
                    version = (long)fds.Tables["REPORT"].Rows[0]["rptversion"];
                else
                    version = 0;
			}
			catch
			{
				fds = null;
				version = 0;
			}
		
	
			//Run the stored procedure and pass it the found version 
			//number.  If there is a newer version then cache the newly generated schema.
			IDataParameter [] parlist = new IDataParameter[4];
			parlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			parlist[1] = Session.CurrentSession.Connection.AddParameter("Version", System.Data.SqlDbType.BigInt, 0, (Session.CurrentSession._designMode ? 0 : version));
			parlist[2] = Session.CurrentSession.Connection.AddParameter("Force", System.Data.SqlDbType.Bit, 0, 0);
			parlist[3] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			
			//Allow an execution error to escalate through the stack.
			ds = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprFetchReport", (code == String.Empty), new string[1] {Table}, parlist);
								
	
			//Make sure that there is a valid configurable type returned.  
			//If not then use the already cached version of the client type.
			if ((ds == null) || (ds.Tables.Count == 0))
			{
				if ((fds == null) || (fds.Tables[Table] == null))
				{
					if (code != String.Empty)
					{
						throw new OMSException2("ERRNOREPOBJ","The specified report '%1%' does not exist within the database.","",new Exception(),true,code);
					}
				}
				else
				{
					//The locally cached version is being used.
					System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using local version of report '" + code + "'", "BAL.Fetch()");
					ds = fds;
					local = true;
				}
			}
			else
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using database version of report '" + code + "'", "BAL.Fetch()");
		
				//Name the data set for neatnes reasons.
				ds.DataSetName = "REPORT";
				
			}

			_report = ds;

			if (ds.Tables[Table].Rows.Count > 0)
			{
				//Cache the client type item to the users application data folder.
				if (!local) 
				{
					Global.Cache(ds, "reports" + @"\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name, code + "." + Global.CacheExt);
				}

				FetchReportfromDatabase(local);

			}
			ds.AcceptChanges();
		}

		#endregion

		#region Events
		/// <summary>
		/// An event that gets raised when the Run Method is activated
		/// </summary>
		public event EventHandler Runed = null;

		protected internal void OnRuned()
		{
			if (Runed != null)
				Runed(this,EventArgs.Empty);
		}
		
		/// <summary>
		/// An event thatgets raised when the precedent has been loaded.
		/// </summary>
		public event EventHandler Load = null;

		/// <summary>
		/// Calls the precedent load event.
		/// </summary>
		public virtual void OnLoad()
		{
			if (Load != null)
				Load(this, EventArgs.Empty);
		}
		
		/// <summary>
		/// An event thatgets raised when the precedent has been loaded.
		/// </summary>
		public event EventHandler Show = null;

		/// <summary>
		/// Calls the precedent Show event.
		/// </summary>
		public virtual void OnShow()
		{
			if (Show != null)
				Show(this, EventArgs.Empty);
		}

		/// <summary>
		/// An event that gets raised when the script has be changed within the object.
		/// </summary>
		public event EventHandler ScriptChanged = null;

		
		/// <summary>
		/// Calls the script changed event.
		/// </summary>
		protected virtual void OnScriptChanged() 
		{
			if (ScriptChanged != null)
				ScriptChanged(this, EventArgs.Empty);
		}	

		#endregion

		#region Static Methods
		// ********************************************************************************************************
		// ********************************************************************************************************
		// ********************************************************************************************************

		/// <summary>
		/// Retrieves all of the active Reports lists from the database with the specified search type.
		/// </summary>
		/// <returns>A data table to list the search lists.</returns>
		public static DataTable GetReportLists()
		{
			return SearchEngine.SearchList.GetSearchLists("sprGetReportsLists","","",2);
		}

		/// <summary>
		/// Retrieves all of the active Reports lists from the database with the specified search type.
		/// </summary>
		/// <param name="type">Type code for the search.</param>
		/// <returns>A data table to list the search lists.</returns>
		public static DataTable GetReportLists(string type)
		{
			return SearchEngine.SearchList.GetSearchLists("sprGetReportsLists","", type, 1);
		}

		/// <summary>
		/// Retrieves all of the all Reports lists from the database with the specified search type.
		/// </summary>
        /// <param name="group"></param>
		/// <param name="type"></param>
		/// <param name="active"></param>
		/// <returns></returns>
		public static DataTable GetReportLists(string group, string type, int active)
		{
			return SearchEngine.SearchList.GetSearchLists("sprGetReportsLists", group, type, active);
		}

		// ********************************************************************************************************
		// ********************************************************************************************************
		// ********************************************************************************************************

		/// <summary>
		/// Gets a report from the database.
		/// </summary>
		/// <param name="code"></param>
        /// <param name="parent"></param>
        /// <param name="param"></param>
		/// <returns></returns>
		public static Report GetReport(string code, object parent, FWBS.Common.KeyValueCollection param)
		{
			Session.CurrentSession.CheckLoggedIn();

			Report _report = new Report(code);

			if (parent != null)
				_report.SearchList.ChangeParent(parent);
			if (param != null)
				_report.SearchList.ChangeParameters(param); 

			try
			{
				if (_report.HasScript)
				{
					_report.Script.Load();
					ReportScriptType reportscr =  _report.Script.Scriptlet as ReportScriptType;
					if (reportscr != null)
					{
						reportscr.SetReportObject(_report);
					}
				}
			}
			catch{}

			return _report;
		}

		public void Refresh()
		{
			FetchReportfromDatabase(false);
		}

		public void Run()
		{
			OnRuned();
		}
		#endregion

		#region IDisposable Implementation

		/// <summary>
		/// Disposes the object immediately without waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by this object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				//Destroy the underlying data source.
				if (_searchlist != null)
				{
					_searchlist.Dispose();
					_searchlist = null;
				}

				if (_report != null)
				{
					_report.Dispose();
					_report = null;
				}
			}
		}


		#endregion
	}
}
