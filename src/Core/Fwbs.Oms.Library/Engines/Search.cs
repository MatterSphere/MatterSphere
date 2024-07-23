using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Script;

namespace FWBS.OMS.SearchEngine
{

    /// <summary>
	/// A search and listing class that holds the configuration data on filtering data from
	/// a configured location then displaying it in a customised format.  This object also
	/// knows how the entity within the searched list is to be edited, or a new one added. 
	/// </summary>
	public class SearchList : SourceEngine.Source, IDisposable
	{
		#region Events

		/// <summary>
		/// An event that gets raised before 
		/// </summary>
		public event System.ComponentModel.CancelEventHandler Searching = null;
		
		/// <summary>
		/// An event that gets raised after the search has completed successfully.
		/// </summary>
		public event SearchedEventHandler Searched = null;

		/// <summary>
		/// An event that gets raised if the search performs an error so that the calling
		/// thread knows about the error.
		/// </summary>
		public event MessageEventHandler Error = null;


		#endregion

		#region Event Methods

		/// <summary>
		/// Raises the searching (Before Search) event.
		/// </summary>
		/// <param name="e">Cancel event arguments.</param>
		public void OnSearching(System.ComponentModel.CancelEventArgs e)
		{
			if (Searching != null)
				Searching(this, e);
		}

		/// <summary>
		/// Raises the searched event (Search Finished).
		/// </summary>
		/// <param name="e">Searched event arguments.</param>
		public void OnSearched(SearchedEventArgs e)
		{
			
			ApplyActiveFilter(_state);

			if (Searched != null)
				Searched(this, e);
		}

		/// <summary>
		/// Raises the error event so that the calling thread knows what has gone wrong.
		/// </summary>
		/// <param name="e">Message event arguments.</param>
		public void OnError(MessageEventArgs e)
		{
			if (Error != null)
				Error(this, e);
		}

		#endregion

		#region Fields

		/// <summary>
		/// The unique Search List Code.
		/// </summary>
		protected string _code = "";

		/// <summary>
		/// SQL statement needed to retrieve and update the Search List information.
		/// </summary>
		internal const string Sql = "select * from dbSearchListConfig";

		/// <summary>
		/// Holds the configuration data of the currently selected search list.
		/// </summary>
		protected DataSet _searchList = null;

		/// <summary>
		/// Table name of the search list header table.
		/// </summary>
		protected internal const string Table = "SEARCHLIST";

		/// <summary>
		/// Table name for the list view column configuration.
		/// </summary>
		protected internal const string Table_ListView = "LISTVIEW";

		/// <summary>
		/// Table name for the list view buttons.
		/// </summary>
		protected internal const string Table_Buttons = "BUTTONS";

		/// <summary>
		/// Table name for the list view header.
		/// </summary>
		protected internal const string Table_ListViewHeader = "LISTVIEW_HEADER";

        /// <summary>
        /// Table name for the User customizations.
        /// </summary>
        protected internal const string Table_User_Customization = "USER_CUSTOMIZATION";

		/// <summary>
		/// A flag that specifies that the current search has been used from the local computer
		/// or extracted directly from the database.
		/// </summary>
		private bool _local = false;

		/// <summary>
		/// The enquiry form used for the criteria form.
		/// </summary>
		private Enquiry _criteriaForm = null;

		/// <summary>
		/// The currently searched data object.
		/// </summary>
		private DataTable _data = null;

		/// <summary>
		/// The currently searched data object.
		/// </summary>
		private DataSet _dataset = null;

		/// <summary>
		/// The Search Thread.
		/// </summary>
		private Thread _thread = null;

		/// <summary>
		/// Additional external filter.
		/// </summary>
		private string _externalfilter = "";

		/// <summary>
		/// Trash can filter.
		/// </summary>
		private string _trashfilter = "";

		/// <summary>
		/// Any other internal filter.
		/// </summary>
		private string _internalfilter = "";

		/// <summary>
		/// Filter state.
		/// </summary>
		private ActiveState _state = ActiveState.Active;

        /// <summary>
        /// List of Local Date Columns
        /// </summary>
        private System.Collections.Generic.List<string> localDateColumns = new System.Collections.Generic.List<string>();

        [Browsable(false)]
        public virtual bool InDesignMode { get { return false; }}

        protected virtual void RaiseDesignModeException(Exception ex)
        {
        }

		#endregion

		#region Constructors
	
		/// <summary>
		/// Creates a default template for a search list configuration object.
		/// </summary>
		protected SearchList()
		{
			Session.CurrentSession.CheckLoggedIn();
			//Allow an execution error to escalate through the stack.
			_searchList = Session.CurrentSession.Connection.ExecuteSQLDataSet("select  SL.*,'' as schdesc from dbsearchlistconfig SL where schcode = ''", true, new string[1]{Table}, null);

			DataTable dt = _searchList.Tables[Table];
			
			//Add a new record.
			Global.CreateBlankRecord(ref dt, true);

			//Give the new search list some default values.
			dt.Columns["schdesc"].MaxLength = -1;
			dt.Columns["schdesc"].ReadOnly = false;

			SetExtraInfo("schSourceParameters", "<params></params>");
			SetExtraInfo("schSourceType", "OMS");
			SetExtraInfo("schStyle", SearchListStyle.List);
			SetExtraInfo("schVersion", 0);
			SetExtraInfo("schSecurityLevel", 0);
			SetExtraInfo("schActive", true);
			SetExtraInfo("schListView", @"<searchList><buttons></buttons></searchList>");
			SetExtraInfo("schReturnField", "<fields></fields>");
			SetExtraInfo("CreatedBy",Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Created",DateTime.Now);
            SetExtraInfo("schApiExclude", true);
            SetExtraInfo("schActionButton", false);
            SetExtraInfo("schPagination", false);
        }


        /// <summary>
        /// Fetches the configuration data for the specified search list.
        /// </summary>
        /// <param name="code">Search list code.</param>
        /// <param name="parent">A parent reference for named parameters.</param>
        /// <param name="parameters">A list of extra named parameters.</param>
        public SearchList (string code, object parent, FWBS.Common.KeyValueCollection parameters) : base()
		{
			SearchListInternal(code, parent, parameters);
		}


		/// <summary>
		/// Fetches the configuration data for the specified search list.
		/// </summary>
		/// <param name="code">Search list code.</param>
		/// <param name="parent">A parent reference for name parameters.</param>
		/// <param name="parameters">A list of extra named parameters.</param>
		internal protected void SearchListInternal(string code,  object parent, FWBS.Common.KeyValueCollection parameters) 
		{
			Session.CurrentSession.CheckLoggedIn();
			
			_code = code;

			DataSet ds = null;		//Internal schema used.
			DataSet fds = null;		//Cached schema version.
			long version = 0;       //Version specifier.

            //Loads the cached version of the search list schema and gets the version
            //from the header information.  If there is an error opening the file
            //then set the version to zero.  The enquiry form will then be completely
            //refreshed from the databse.

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            try
			{
				fds = Global.GetCache(@"searchlist\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + @"\" + Session.CurrentSession.Edition,  code +  "." + Global.CacheExt);
                if (fds != null)
                    version = (long)fds.Tables[Table].Rows[0]["schversion"];
                else
                    version = 0;
			}
			catch
			{
				fds = null;
				version = 0;
			}
					
	
			//Run the sprSearchListBuilder stored procedure and pass it the found version 
			//number.  If there is a newer version then cache the newly generated schema.
			IDataParameter [] paramlist = new IDataParameter[4];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Version", System.Data.SqlDbType.BigInt, 0, (Session.CurrentSession._designMode ? 0 : version));
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("Force", System.Data.SqlDbType.Bit, 0, 0);
			paramlist[3] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);

			//Allow an execution error to escalate through the stack.
			ds = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprSearchListBuilder", new string[1]{Table}, paramlist);
				
			//Make sure that there is a valid search list returned.  
			//If not then use the already cached version of the enquiry form.
			if ((ds == null) || (ds.Tables.Count == 0))
			{
				if ((fds == null) || (fds.Tables[Table] == null))
				{
					//The returned data set schema is invalid and there is not cached version
					//to rely on.
					throw new SearchException(HelpIndexes.SearchListDoesNotExist, code); 
				}
				else
				{
					//The locally cached version is being used.
					Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using local version of search list '" + code + "'", "BAL.SearchList()");
					ds = fds;
					_local = true;
				}
			}
			else
			{
				Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using database version of search list '" + code + "'", "BAL.SearchList()");
			}		

			//Set the name of the other tables within the dataset.
			ds.Tables[1].TableName = Table_ListView;
			ds.Tables[2].TableName = Table_Buttons;
			ds.Tables[3].TableName = Table_ListViewHeader;
            ds.Tables[4].TableName = Table_User_Customization;

			//Terminology parse the list views column names.
			foreach (DataRow row in ds.Tables[1].Rows)
			{
                if (row.Table.Columns.Contains("sourceIs") && Convert.ToString(row["sourceIs"]) == "Local") localDateColumns.Add(Convert.ToString(row["lvmapping"]));
                row["lvdesc"] = Session.CurrentSession.Terminology.Parse(row["lvdesc"].ToString(), true);
			}
			ds.Tables[1].AcceptChanges();

			
			//Terminology parse the list views column names.
			foreach (DataRow row in ds.Tables[2].Rows)
			{
				row["btndesc"] = Session.CurrentSession.Terminology.Parse(row["btndesc"].ToString(), false);
				row["btnhelp"] = Session.CurrentSession.Terminology.Parse(row["btnhelp"].ToString(), true);
			}
			ds.Tables[2].AcceptChanges();

			//Terminology parse the list views grid caption.
			foreach (DataRow row in ds.Tables[3].Rows)
			{
				row["lvcaptiondesc"] = Session.CurrentSession.Terminology.Parse(row["lvcaptiondesc"].ToString(), false);
			}
			ds.Tables[3].AcceptChanges();

            foreach (DataRow row in ds.Tables[4].Rows)
            {
                if (row.Table.Columns.Contains("sourceIs") && Convert.ToString(row["sourceIs"]) == "Local") localDateColumns.Add(Convert.ToString(row["lvmapping"]));
                row["lvdesc"] = Session.CurrentSession.Terminology.Parse(row["lvdesc"].ToString(), true);
            }

            ds.Tables[4].AcceptChanges();
		
            //Cache the enquiry item to the users application data folder.
			//(e.g., "%APPDATA%\fwbs\oms\enquiries\user.xml").
			if (!_local) 
			{
				Global.Cache(ds, @"searchlist\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + @"\" + Session.CurrentSession.Edition, code + "." + Global.CacheExt);
			}

			ds.AcceptChanges();

			//Set the main search list data set to the one just built.
			_searchList = ds;

			//Set the base source objects fields so that the source object knows how to search
			//for its data.
			base.SourceType = (SourceEngine.SourceType)Enum.Parse(typeof(SourceEngine.SourceType), Convert.ToString(GetExtraInfo("schsourcetype")), true);
			base.Src = Convert.ToString(GetExtraInfo("schsource"));
			base.Call  = Convert.ToString(GetExtraInfo("schsourcecall"));
			base.Parameters =  Convert.ToString(GetExtraInfo("schsourceparameters"));

			//Sets the extra named parameters to the source object for extra field replacement.
			ChangeParameters(parameters);

			//Sets the parent object of the source object for extra field replacement.
			//***** THIS IS VITAL FOR SET_PARAMETER TO WORK WITH A PARENT OBJECTS *****
			ChangeParent(parent);

			//Reference any script that maybe available to the enquiry.
			if (HasScript)
			{
                try
                {
                    Script.Load();
                }
                catch (Exception ex)
                {
                    if (!InDesignMode)
                        throw;
                    else
                    {
                        RaiseDesignModeException(ex);
                    }
                }
				SearchListScriptType schscr =  _script.Scriptlet as SearchListScriptType;
				if (schscr != null)
				{
					schscr.SetSearchObject(this);
				}
			}

		}

		#endregion

		#region Properties
        internal string[] LocalDateColumns
        {
            get
            {
                return localDateColumns.ToArray();
            }
        }


		/// <summary>
		/// Gets a value indicating whether the search list object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (_searchList.Tables[Table].Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}

		[Browsable(false)]
		public DataTable DataTable
		{
			get
			{
				return _data;
			}
		}
        
		/// <summary>
		/// Gets the enquiry form to render for the search criteria screen.
		/// </summary>
		[Browsable(false)]
		public Enquiry CriteriaForm
		{
			get
			{
				if (_criteriaForm == null)
				{
					if (!Convert.IsDBNull(GetExtraInfo("schEnquiry")))
						_criteriaForm = Enquiry.GetEnquiry(Convert.ToString(GetExtraInfo("schEnquiry")), Parent, EnquiryMode.Search, this.ReplacementParameters);
				}
				return _criteriaForm;
			}
		}


		/// <summary>
		/// Gets the search list type code.
		/// </summary>
		[Browsable(false)]
		public string SearchListType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("schtype"));
			}
		}

		/// <summary>
		/// Gets or Sets the current search description.
		/// </summary>
		[Browsable(false)]
		public virtual string Description
		{
			get
			{
				return Convert.ToString(Session.CurrentSession.Terminology.Parse(GetExtraInfo("schdesc").ToString(),true));
			}
			set
			{
				SetExtraInfo("schdesc", value);
			}
		}

		/// <summary>
		/// Gets a boolean value that indicates whether the search list configuration information has bee
		/// extracted from a cached local version (true) or from the database (false).
		/// </summary>
		[Browsable(false)]
		public bool Local
		{
			get
			{
				return _local;
			}
		}

		[Browsable(false)]
		public string[] Tables
		{
			get
			{
				if (Convert.ToBoolean(GetExtraInfo("schIsReport")))
					return Convert.ToString(GetExtraInfo("schReturnField")).Split(',');
				else
					return new string[0];
			}
		}

		/// <summary>
		/// Gets a data table that holds the information on how to display the search results in
		/// a list of some kind.
		/// </summary>
		[Browsable(false)]
		public DataTable ListView
		{
			get
			{
				return _searchList.Tables[Table_ListView];
			}
		}

		/// <summary>
		/// Gets the list view header configuration information.
		/// </summary>
		[Browsable(false)]
		public DataTable ListViewHeader
		{
			get
			{
				return _searchList.Tables[Table_ListViewHeader];
			}
		}


		/// <summary>
		/// Gets a data table that holds the information on how to display the buttons.
		/// </summary>
		[Browsable(false)]
		public DataTable ButtonsTable
		{
			get
			{
				return _searchList.Tables[Table_Buttons];
			}
		}

        /// <summary>
        /// Gets a data table that holds the information on how to display the search results in
        /// a list of some kind depending on user customizations.
        /// </summary>
        [Browsable(false)]
        public DataTable UserCustomizaitons
        {
            get
            {
                return _searchList.Tables[Table_User_Customization];
            }
        }

        /// <summary>
        /// Shows whether search list was customized by specific user
        /// </summary>
        [Browsable(false)]
        public bool IsUserCutomized
        {
            get
            {
                return _searchList.Tables[Table_User_Customization].Rows.Count > 0;
            }
        }

		/// <summary>
		/// Gets the field names that the search is going to use to return the value back to the
		/// calling client.
		/// </summary>
		[Browsable(false)]
		public string [] ReturnFields
		{
			get
			{
				FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(Convert.ToString(GetExtraInfo("schreturnfield")));
				cfg.Current = "fields";
				FWBS.Common.ConfigSettingItem [] itms = cfg.CurrentChildItems;
				int cnt = itms.Length;
				string [] fields = new string[cnt];
				for (int ctr = 0; ctr < cnt; ctr++)
				{
					fields[ctr] = itms[ctr].Element.InnerText;
				}
				return  fields;
			}
		}

        [Browsable(false)]
        public bool IsOrderingSupported
        {
            get
            {
                var cfg = new FWBS.Common.ConfigSetting(Convert.ToString(GetExtraInfo("schSourceParameters")));
                cfg.Current = "params";
                Common.ConfigSettingItem[] itms = cfg.CurrentChildItems;
                foreach (var item in itms)
                {
                    if (item.Element.GetAttribute("name").ToUpper() == "@ORDERBY")
                    {
                        return true;
                    }
                }
                return false;
            }
        }

		/// <summary>
		/// Gets the record count of the last search applied.
		/// </summary>
		[Browsable(false)]
		public int ResultCount
		{
			get
			{
				if (_data == null)
					return 0;
				else
					return _data.DefaultView.Count;
			}

		}

		/// <summary>
		/// Returns Bool if the Search List has not been Run
		/// </summary>
		[Browsable(false)]
		public bool NotSearched
		{
			get
			{
				return (_data == null);
			}

		}

		/// <summary>
		/// Gets the Version of the Search List
		/// </summary>
		[Browsable(false)]
		public virtual int Version
		{
			get
			{
				return Convert.ToInt32(GetExtraInfo("schVersion"));
			}

		}
		
		/// <summary>
		/// Gets the loclaized caption text of the search list list view.
		/// </summary>
		[Browsable(false)]
		public string Caption
		{
			get
			{
                if (ListViewHeader != null && ListViewHeader.Rows.Count > 0)
                    return Convert.ToString(ListViewHeader.Rows[0]["lvcaptiondesc"]);
                else
                    return String.Empty;
			}
		}

		/// <summary>
		/// Gets the search list row height
		/// </summary>
        [Browsable(false)]
        public int RowHeight
        {
            get
            {
                if (_searchList.Tables.Count > 3 && _searchList.Tables[3].Columns.Contains("lvrowheight") && _searchList.Tables[3].Rows.Count > 0)
                {
                    string value = Convert.ToString(_searchList.Tables[3].Rows[0]["lvrowheight"]).ToLower();
                    if (value == "r2lines")
                        return 2;
                    else if (value == "r3lines")
                        return 3;
                    else if (value == "r1line")
                        return 1;
                    else
                        return FWBS.Common.ConvertDef.ToInt32(value, 1);
                }
                else
                    return 1;
            }
        }

        /// <summary>
        /// Gets the search list style type.
        /// </summary>
        [Browsable(false)]
        public SearchListStyle Style
		{
			get
			{
				if (Convert.ToByte(GetExtraInfo("schStyle")) == 0)
					return SearchListStyle.Search;
				else if (Convert.ToByte(GetExtraInfo("schStyle")) == 2)
					return SearchListStyle.Filter;
				else
					return SearchListStyle.List;
			}
		}

        /// <summary>
        /// Gets the search list action button property.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool ActionButtonVisible
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("schActionButton"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetExtraInfo("schActionButton", value);
            }
        }

        /// <summary>
        /// Gets the search list pagination property
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool PaginationVisible
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetExtraInfo("schPagination"));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetExtraInfo("schPagination", value);
            }
        }


        /// <summary>
        /// Gets or Sets the visiblity of the result grid caption.
        /// </summary>
        [Browsable(false)]
		[DefaultValue(true)]
		public virtual bool CaptionVisible
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ListViewHeader.Rows[0]["lvcaptionvisible"]);
				}
				catch
				{
					return true;
				}
			}
			set
			{
				ListViewHeader.Rows[0]["lvcaptionvisible"] = value;
			}
		}

		/// <summary>
		/// Gets First Column Image Index
		/// </summary>
		[Browsable(false)]
		public int ColumnImageIndex
		{
			get
			{
				try
				{
					return Convert.ToInt32(ListViewHeader.Rows[0]["lvimageindex"]);
				}
				catch
				{
					return -1;
				}
			}
		}

		/// <summary>
		/// Gets First Column Image Column
		/// </summary>
		[Browsable(false)]
		public string ColumnImageColumn
		{
			get
			{
				try
				{
					return Convert.ToString(ListViewHeader.Rows[0]["lvimagecolumn"]);
				}
				catch
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Gets First Column Image Column
		/// </summary>
		[Browsable(false)]
		public string ColumnImageResource
		{
			get
			{
				try
				{
					return Convert.ToString(ListViewHeader.Rows[0]["lvimageresouce"]);
				}
				catch
				{
					return "None";
				}
			}
		}

		/// <summary>
		/// Gets or Sets the visibility of the quick search text box.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		[Lookup("SCHQUICKSRCH")]
		public virtual bool QuickSearch
		{
			get
			{
				try
				{
					return Convert.ToBoolean(ListViewHeader.Rows[0]["lvquicksearchvisible"]);
				}
				catch
				{
					return true;
				}
			}
			set
			{
				ListViewHeader.Rows[0]["lvquicksearchvisible"] = value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the Quick Search Prefix
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public virtual string QuickSearchPrefix
		{
			get
			{
				try
				{
					return Convert.ToString(ListViewHeader.Rows[0]["lvquicksearchprefix"]);
				}
				catch
				{
					return "";
				}
			}
			set
			{
				ListViewHeader.Rows[0]["lvquicksearchprefix"] = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Search List Code.
		/// </summary>
		[Browsable(false)]
		public virtual string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("schcode"));
			}
			set
			{
				SetExtraInfo("schcode", value);
			}
		}
        /// <summary>
        /// Gets or Sets the help file name
        /// </summary>
        [Browsable(false)]
        [DefaultValue(true)]
        [Lookup("SCHHELPFILENAME")]
        public virtual string HelpFileName
        {
            get
            {
                try
                {
                    return Convert.ToString(ListViewHeader.Rows[0]["lvhelpfilename"]);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ListViewHeader.Rows[0]["lvhelpfilename"] = value;
            }
        }

        /// <summary>
        /// Gets or Sets the help file name
        /// </summary>
        [Browsable(false)]
        [DefaultValue(true)]
        [Lookup("SCHHELPKEYWORD")]
        public virtual string HelpKeyword
        {
            get
            {
                try
                {
                    return Convert.ToString(ListViewHeader.Rows[0]["lvhelpkeyword"]);
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                ListViewHeader.Rows[0]["lvhelpkeyword"] = value;
            }
        }


        /// <summary>
        /// Gets or Sets the save search type for this searchlist
        /// </summary>
        [Browsable(false)]
        [DefaultValue(true)]
        [Lookup("SCHSAVESEARCH")]
        public virtual string  SaveSearch
        {
            get
            {
                try
                {
                    switch (Convert.ToString(ListViewHeader.Rows[0]["lvsaveSearch"]))
                    {

                        case "Manual":
                            return Convert.ToString(SaveSearchType.Manual);
                        case "Always":
                            return Convert.ToString(SaveSearchType.Always);
                        default:
                            return Convert.ToString(SaveSearchType.Never);
                    }
                }
                catch
                {
                    return Convert.ToString(SaveSearchType.Never); 
                }
            }
    
            set
            {
                ListViewHeader.Rows[0]["lvsaveSearch"] = value;
            }
        }



		/// <summary>
		/// Specifies whether the search list object is system based.
		/// </summary>
		[Browsable(false)]
		public bool IsSystem
		{
			get
			{
				try
				{
					return Convert.ToBoolean(GetExtraInfo("schsystem"));
				}
				catch
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Gets the default double click action of the search list results.
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public virtual string DoubleClickAction
		{
			get
			{
				return ListViewHeader.Rows.Count > 0 ? Convert.ToString(ListViewHeader.Rows[0]["lvdblclickaction"]) : "None";
			}
			set
			{
				SetExtraInfo("lvdblclickaction", value);
			}
		}

		/// <summary>
		/// Gets or Sets the multi select option.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(false)]
		public virtual bool MultiSelect
		{
			get
			{
				if (ListViewHeader.Columns.Contains("lvmultiselect") && ListViewHeader.Rows.Count > 0)
					return Common.ConvertDef.ToBoolean(ListViewHeader.Rows[0]["lvmultiselect"], false);
				else
					return false;
			}
			set
			{
				if (ListViewHeader.Columns.Contains("lvmultiselect"))
					SetExtraInfo("lvmultiselect", value);
			}
		}
		
		/// <summary>
		/// Gets the trash can state of the search list.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(ActiveState.Active)]
		public ActiveState ActiveState 
		{
			get
			{
				return _state;
			}
		}

		/// <summary>
		/// Gets the active button reference.
		/// </summary>
		private SearchButtonArgs ActiveButton
		{
			get
			{
				return GetButton(ButtonActions.ViewActive);
			}
		}

		/// <summary>
		/// Gets the inactive button reference.
		/// </summary>
		private SearchButtonArgs InactiveButton
		{
			get
			{
				return GetButton(ButtonActions.ViewTrash);
			}
		}

		/// <summary>
		/// Gets the button reference by the action type.
		/// </summary>
		/// <returns></returns>
		private SearchButtonArgs GetButton(ButtonActions action)
		{
			DataTable dt = ButtonsTable;
			DataView vw = new DataView(dt);
			vw.RowFilter = "mode = '" + action.ToString() + "'";
			if (vw.Count > 0)
			{
				SearchButtonArgs btn = new SearchButtonArgs((ButtonActions)FWBS.Common.ConvertDef.ToEnum(vw[0]["mode"],ButtonActions.None),Convert.ToString(vw[0]["parameter"]));
				return btn;
			}
			else
				return null;
		}

		#endregion

		#region Threading


		/// <summary>
		/// Stops the searching thread.
		/// </summary>
		public void CancelSearch()
		{
			if (_thread != null && _thread.IsAlive)
			{
				_thread.Abort();
			}
		}

        /// <summary>
        /// Searches the configured data source with the criteria form that the search is based around.
        /// </summary>
        /// <param name="asynchronous">If true, the search is performed on a separate thread.</param>
        /// <returns>A data table of searched data.</returns>
        public DataTable Search(bool asynchronous)
        {
            _lasterror = null;
            if (!asynchronous)
                return Search();

            //If the thread is null or not currently alive then start it.
            if (_thread == null || !_thread.IsAlive)
            {
                ThreadStart oTS = new ThreadStart(this.ThreadSearch);
                _thread = new Thread(oTS);
                _thread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                _thread.CurrentUICulture = _thread.CurrentCulture;
                _thread.Name = this.Code;
                _thread.Start();
            }

            return null;

        }

        /// <summary>
        /// The method that the search thread calls when the thread starts.
        /// </summary>
        private void ThreadSearch()
        {
            Search();
        }

        private Exception _lasterror;

        /// <summary>
        /// The Base Search
        /// </summary>
        /// <returns>Returns the First Table</returns>
		private DataTable Search()
		{
			//A data object that could hold any list compatible data.
			CancelEventArgs cancel = new CancelEventArgs(false);
			OnSearching(cancel);

            try
            {
                if (cancel.Cancel)
                {
                    CancelSearch();
                    return null;
                }

                //A data object that could hold any list compatible data.
                string defsort = (_data == null ? "" : _data.DefaultView.Sort);
                object _dataobj = base.Run(false, true, this.LocalDateColumns);
                _dataset = _dataobj as DataSet;
                _data = _dataobj as DataTable;
                if (_dataset != null)
                {
                    _data = _dataset.Tables[0];

                    if (_dataset.Tables.Count == 1)
                        SetupResultsTable(defsort);
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(defsort))
                            _data.DefaultView.Sort = defsort;
                        OnSearched(new SearchedEventArgs(_dataset, _data));
                    }
                }
                else if (_data != null)
                    SetupResultsTable(defsort);
                else
                    OnError(new MessageEventArgs(new OMSException(HelpIndexes.SearchNotCompatibleResultset)));

                return _data;

            }
            catch (Exception ex)
            {
                if (_lasterror == null || _lasterror.Message != ex.Message)
                {
                    if (ex is System.InvalidOperationException || (ex is System.Reflection.TargetInvocationException && ex.InnerException != null && ex.InnerException.InnerException is System.InvalidOperationException))
                    {
                        _lasterror = ex;
                        System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 2, 0));
                        return Search();
                    }
                }

                if (ex is ThreadAbortException)
                {
                    System.Diagnostics.Trace.WriteLine("The thread '" + this.Code + "' has been aborted");
                }
                else
                {
                    OnError(new MessageEventArgs(ex));
                }
                return null;
            }
		}

        private void SetupResultsTable(string defsort)
        {
            _data.TableName = "RESULTS";
            if (!string.IsNullOrWhiteSpace(defsort))
                _data.DefaultView.Sort = defsort;
            OnSearched(new SearchedEventArgs(_data));
        }

       
		#endregion

		#region Methods

		/// <summary>
		/// Clears the result set.
		/// </summary>
		public void ClearResults()
		{
			if (_data != null)
			{
				_data.Rows.Clear();
				_data.AcceptChanges();
			}
		}

		/// <summary>
		/// Updates the object and persists it to the database.
		/// </summary>
		public virtual void Update()
		{
			SetExtraInfo("UpdatedBy",Session.CurrentSession.CurrentUser.ID);
			SetExtraInfo("Updated",DateTime.Now);
			Session.CurrentSession.Connection.Update(_searchList, Table, Sql);
		}
		
		/// <summary>
		/// Deletes a row from the searched list.
		/// </summary>
		/// <param name="index">The row indesx to delete.</param>
		public void DeleteRow(int index)
		{
			if (_data != null && Call.ToUpper().StartsWith("SELECT"))
			{
				_data.DefaultView[index].Delete();
			}
		}

		/// <summary>
		/// Update the underling data from the Source Object
		/// </summary>
		public virtual void UpdateData()
		{
			if (_data != null && Call.ToUpper().StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, Call);
			}
		}

		//TODO: Think about the select statement.
		/// <summary>
		/// Update the underling data with this Select Statemente
		/// </summary>
		/// <param name="select">The select SQL statement to use.</param>
		public virtual void UpdateData(string select)
		{
			if (_data != null && select.ToUpper().StartsWith("SELECT"))
			{
				Session.CurrentSession.Connection.Update(_data, select);
			}
		}

		//TODO: Think about the select statement.
		/// <summary>
		/// Makes an record item inactive
		/// </summary>
		/// <param name="index">The row index to make inactive.</param>
		/// <param name="fieldName">The field name that is the active / inactive flag.</param>
		/// <param name="value">The value to set the field name for the specified row index.</param>
		public void TrashField(int index, string fieldName, object value)
		{
			DataView view = _data.DefaultView;
			view[index].Row[fieldName] = Convert.ChangeType(value,view.Table.Columns[fieldName].DataType);	
		}


		/// <summary>
		/// Selects the specified item within the data table and returns back the values that
		/// are configured to be returned.
		/// </summary>
		/// <param name="index">Row index to select.</param>
		/// <returns>A value that uniquely identifies the record.</returns>
		public FWBS.Common.KeyValueCollection Select(int index)
		{
			FWBS.Common.KeyValueCollection values = new FWBS.Common.KeyValueCollection();

			string [] fields = this.ReturnFields;
		
			if (_data == null)
			{
				throw new SearchException(HelpIndexes.SearchNoItemSelected);
			}
			else
			{
				foreach(string field in fields)
				{
					if (_data.Columns.Contains(field) == false)
						throw new SearchException(HelpIndexes.SearchInvalidReturnField, field);
					
					try
					{
						values.Add(field, _data.DefaultView[index].Row[field]);
					}
					catch (Exception ex)
					{
						throw new SearchException(ex, HelpIndexes.SearchNoItemSelected);
					}

				}
			}

			return values;
		}

		/// <summary>
		/// Selects the specified items within the data table and returns back the values that
		/// are configured to be returned.
		/// </summary>
		/// <param name="indexes">Row index array.</param>
		public FWBS.Common.KeyValueCollection[] Select(int [] indexes)
		{
			FWBS.Common.KeyValueCollection [] values = new FWBS.Common.KeyValueCollection[indexes.Length];
		
			if (_data == null)
			{
				throw new SearchException(HelpIndexes.SearchNoItemSelected);
			}
			else
			{
				for(int ctr = 0; ctr < indexes.Length; ctr++)
				{
					FWBS.Common.KeyValueCollection val = Select(indexes[ctr]);
					values[ctr] = val;
				}
			}

			return values;
		}

		/// <summary>
		/// Gets a value from the underlying data set based on the field name passed.
		/// </summary>
		/// <param name="fieldName">Field name to get the data from.</param>
		/// <returns>A piece of data extracted from the dataset.</returns>
		protected object GetExtraInfo(string fieldName)
		{
			object val =  _searchList.Tables[Table].Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		/// <summary>
		/// Sets a value in the underlying data set based on the field name and value passed.
		/// </summary>
		/// <param name="fieldName">Field name to get the data from.</param>
		/// <param name="val">The value to set the field to.</param>
		protected void SetExtraInfo(string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_searchList.Tables[Table].Rows[0][fieldName] = val;
		}

		#endregion

		#region Filtering


		public void ApplyFilters()
		{
			try
			{
				string output = "";
				if (_trashfilter != "")
				{
					output += _trashfilter + " and ";
				}
				if (_internalfilter != "")
				{
					output += "(" + _internalfilter + ") and ";
				}
				if (_externalfilter != "")
				{
					output += "(" + _externalfilter + ") and ";
				}
				if (output.EndsWith(" and "))
				{
					output = output.Substring(0,output.Length -5);
				}
				if (_data != null)
					_data.DefaultView.RowFilter = output;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

		}

        public bool IsValueInColumn(string columnName, string value)
        {
            return IsValueInColumn(_data, columnName, value);
        }

        public bool IsValueInColumn(DataTable data, string columnName, string value)
        {
            if (data != null)
            {
                using (DataView dv = new DataView(data))
                {
                    try
                    {
                        dv.RowFilter = string.Format("{0} = '{1}'", columnName, value);
                    }
                    catch
                    {
                        dv.RowFilter = string.Format("{0} = {1}", columnName, value);
                    }
                    return dv.Count > 0;
                }
            }
            else
                return false;
        }

		
		[Browsable(false)]
		public string ExternalFilter
		{
			get
			{
				return _externalfilter;
			}
			set
			{
				_externalfilter = value;
				ApplyFilters();
			}
		}

		public void Filter (string text)
		{
			_internalfilter = text;		
			ApplyFilters();
		}

		public void ApplyActiveFilter(ActiveState state)
		{
			SearchButtonArgs b = null;
			

			switch (state)
			{
				case ActiveState.Active:
					b = ActiveButton;;
					break;
				case ActiveState.Inactive:
					b = InactiveButton;
					break;
				default:
					b =null;
					break;
			}

			if (b != null)
			{
				FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(b.Parameters);
				string fieldname = cfg.GetSetting("trashCan","fieldname","");
				string fieldequals = cfg.GetSetting("trashCan","changeValue","");

				if (fieldname != "" && fieldequals != "")
				{
					if (fieldequals.IndexOfAny("*%".ToCharArray()) != -1 )
						_trashfilter = fieldname + " LIKE '" + fieldequals + "'";
					else
						_trashfilter = fieldname + " = " + fieldequals;
				}
			}

			ApplyFilters();

			_state = state;
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
				if (_data != null)
				{
					_data.Dispose();
					_data = null;
				}
				
				if (_dataset != null)
				{
					_dataset.Dispose();
					_dataset = null;
				}

				//Destroy the underlying data source.
				if (_searchList != null)
				{
					_searchList.Dispose();
					_searchList = null;
				}

				//Destroy the criteria enquiry.
				if (_criteriaForm != null)
				{
					_criteriaForm.Dispose();
					_criteriaForm = null;
				}

				if (_script != null)
				{
					_script.Dispose();
					_script = null;
				}

			}
		}


		#endregion

		#region Source Implementation

		/// <summary>
		/// An abstract method which must be implemented so that each parameter in the paramater
		/// list has its value populated.
		/// </summary>
		/// <param name="name">The name of the parameter being set.</param>
		/// <param name="value"></param>
		protected override void SetParameter (string name, out object value)
		{
			//TODO: Lookin to removing Try Catch in future - DCT
            try 
			{
				if (_criteriaForm != null)
                    value = _criteriaForm.Source.Tables[Enquiry.Table_Data].Rows[0][name];
                else
                    base.SetParameter(name, out value);
			}
			catch
			{
				base.SetParameter(name, out value);
			}
		}

		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes a search list from the database.
		/// </summary>
		/// <param name="Code">Search List Code</param>
		/// <returns>True if Succesful;</returns>
		public static bool Delete(string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			try
			{
				Session.CurrentSession.Connection.ExecuteSQL("delete from DBCodeLookup where cdcode = @Code and cdtype = 'OMSSEARCH';delete from DBReport where rptCode = @Code;delete from DBSearchListConfig where schCode = @Code", new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
				return true;
			}
			catch
			{
				return false;
			}
		}
		
		/// <summary>
		/// Checks to see if a specific search list already exists.
		/// </summary>
		/// <param name="code">The code for the search list being queried.</param>
		/// <returns>True if the search list already exists.</returns>
		public static bool Exists(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from DBSearchListConfig where schCode = @Code", "EXISTS",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, code)});
			if (dt.Rows.Count > 0)
				return true;
			else
				return false;
		}

        public static bool GroupExists(string groupcode)
        {
            Session.CurrentSession.CheckLoggedIn();

            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from DBSearchListConfig where schType = @Code", "EXISTS", new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, groupcode) });
            return dt.Rows.Count > 0;

        }

        /// <summary>
        /// Retrieves all of the active search lists from the database.
        /// </summary>
        /// <returns>A data table to list the search lists.</returns>
        public static DataTable GetSearchLists()
		{
			return GetSearchLists("", "", 2);
		}

		/// <summary>
		/// Retrieves all of the active search lists from the database with the specified search type.
		/// </summary>
		/// <param name="type">Type code for the search.</param>
		/// <returns>A data table to list the search lists.</returns>
		public static DataTable GetSearchLists(string type)
		{
			return GetSearchLists("", type, 1);
		}
		
		/// <summary>
		/// Retrieves all of the active search lists from the database with the specified search type.
		/// </summary>
        /// <param name="group"></param>
		/// <param name="type">Type code for the search.</param>
		/// <returns>A data table to list the search lists.</returns>
		public static DataTable GetSearchLists(string group, string type)
		{
			return GetSearchLists(group, type, 1);
		}

		/// <summary>
		/// Retrieves all of the all search lists from the database with the specified search type.
		/// </summary>
        /// <param name="group"></param>
		/// <param name="type"></param>
		/// <param name="active"></param>
		/// <returns></returns>
		public static DataTable GetSearchLists(string group, string type, int active)
		{
			return GetSearchLists("sprGetSearchLists",group, type, active);
		}

        // ********************************************************************************************************
        // ********************************************************************************************************
        // ********************************************************************************************************

        /// <summary>
        /// Private Retrieves all of the all search lists from the database with the specified search type.
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="group"></param>
        /// <param name="type">Type code for the search.</param>
        /// <param name="active"></param>
        /// <returns>A data table to list the search lists.</returns>
        internal static DataTable GetSearchLists(string ProcName, string group, string type, int active)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[4];
			if (group == "")
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("Group", System.Data.SqlDbType.NVarChar, 15, DBNull.Value);
			else
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("Group", System.Data.SqlDbType.NVarChar, 15, group);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Type", System.Data.SqlDbType.NVarChar, 15, type);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);
			paramlist[3] = Session.CurrentSession.Connection.AddParameter("Active", System.Data.SqlDbType.SmallInt, 1, active);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable(ProcName, "SEARCHLISTS", paramlist);
			DataColumn c = new DataColumn("schTextStyle",typeof(System.String),"IIF(schStyle = 1,'List',IIF(schStyle = 2,'Filter','Search'))");
			dt.Columns.Add(c);
			//Terminology parse each of the items.
			foreach (DataRow row in dt.Rows)
			{
				row["schdesc"] = Session.CurrentSession.Terminology.Parse(row["schdesc"].ToString(), true);
			}
			dt.AcceptChanges();

			return dt;
		}

		/// <summary>
		/// Retrieves a distinct list of SearchTypes
		/// </summary>
		/// <returns>A data table to list the search lists.</returns>
		public static DataTable GetSearchTypes()
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT DISTINCT schType FROM dbSearchListConfig", "SEARCHTYPES", new IDataParameter[0]);
			return dt;
		}

		#endregion

		#region Scripting

		/// <summary>
		/// An event that gets raised when the script has be changed within the object.
		/// </summary>
		public event EventHandler ScriptChanged = null;

		/// <summary>
		/// Holds a reference to the script object for performing form and control events
		/// for the form when rendered to screen.
		/// </summary>
		protected ScriptGen _script = null;

		/// <summary>
		/// Create a New Script object for the Enquiry Form
		/// </summary>
		public void NewScript()
		{
			_script = null;
		}

		/// <summary>
		/// Calls the script changed event.
		/// </summary>
		protected virtual void OnScriptChanged() 
		{
			if (ScriptChanged != null)
				ScriptChanged(this, EventArgs.Empty);
		}	

		/// <summary>
		/// Gets or Sets the script name.
		/// </summary>
		[LocCategory("Script")]
		public virtual string ScriptName
		{
			get
			{
				return Convert.ToString(GetExtraInfo("schScript"));
			}
			set
			{
				if (value.Length >= 15)
					value = value.Substring(0, 15);
				SetExtraInfo("schScript",value);
				this.Script.Code=value;
				OnScriptChanged();
			}
		}

		/// <summary>
		/// Gets a boolean value that indicates whether the enquiry form ahas a script assopciated
		/// with it.
		/// </summary>
		[Browsable(false)]
		public bool HasScript
		{
			get
			{
				try
				{
					if (Convert.ToString(GetExtraInfo("schScript")) == "")
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
		/// Gets the enquiry script type of the of the current enquiry form.
		/// </summary>
		[Browsable(false)]
		public ScriptGen Script
		{
			get
			{
				if (_script == null)
				{
					if (HasScript && ScriptGen.Exists(Convert.ToString(GetExtraInfo("schScript"))))
					{
						_script = ScriptGen.GetScript(Convert.ToString(GetExtraInfo("schScript")));
					}
					else
					{
                        _script = new ScriptGen(Convert.ToString(GetExtraInfo("schScript")), "SEARCHLIST");
					}
				}
				return _script;
			}
		}

		#endregion


	}

	/// <summary>
	/// A delegate that describes a method that is used when a search has finished.
	/// </summary>
	public delegate void SearchedEventHandler (object sender, SearchedEventArgs e);

	/// <summary>
	/// The delegates Searched event arguments.
	/// </summary>
	public class SearchedEventArgs : EventArgs
	{
		/// <summary>
		/// The data table returned from the search when used within a threaded environment.
		/// </summary>
		private DataTable _data;
		private DataSet _dataset;
		
		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private SearchedEventArgs(){}
		
		/// <summary>
		/// The constructor used to return back the resultset.
		/// </summary>
		/// <param name="data">The data resultset.</param>
		internal SearchedEventArgs(DataTable data)
		{
			_data = data;
		}

		/// <summary>
		/// The constructor used to return back the resultset.
		/// </summary>
        /// <param name="dataset"></param>
		/// <param name="data">The data resultset.</param>
		internal SearchedEventArgs(DataSet dataset, DataTable data)
		{
			try 
			{
				_dataset = dataset;
				_data = data;
			}
			catch
			{}
		}

		/// <summary>
		/// Gets the returned resultset.
		/// </summary>
		public DataSet DataSet
		{
			get
			{
				return _dataset;
			}
		}

		/// <summary>
		/// Gets the returned resultset.
		/// </summary>
		public DataTable Data
		{
			get
			{
				return _data;
			}
		}
	}

	/// <summary>
	/// Event arguments for the search buttons.  These allow parameters and action
	/// types to be sent through a button click.
	/// </summary>
	public class SearchButtonArgs : EventArgs
	{
		/// <summary>
		/// The button action (Add, Edit etc...).
		/// </summary>
		private ButtonActions _action;

		/// <summary>
		/// Any extra parameters it may need
		/// </summary>
		private string _parameters;
		
		/// <summary>
		/// Creates an instance of the event argumetns specifying the buttons actions and any parameters
		/// needed to carry out a command.
		/// </summary>
		/// <param name="action">Button action.</param>
		/// <param name="parameters">Additional parameters.</param>
		public SearchButtonArgs(ButtonActions action, string parameters)
		{
			_action = action;
			_parameters = parameters;
		}

		/// <summary>
		/// Gets the action type of the button thats just been clicked.
		/// </summary>
		public ButtonActions Action
		{
			get
			{
				return _action;
			}
		}

		/// <summary>
		/// Gets the parameters of the current button being clicked.
		/// </summary>
		public string Parameters
		{
			get
			{
				return _parameters;
			}
		}
	}


    public enum SaveSearchType
    { 
        Never,
        Manual,
        Always    
    }


	/// <summary>
	/// Type / style of search list.
	/// </summary>
	public enum SearchListStyle
	{
		Search,
		List,
		Filter
	}

	public enum ActiveState
	{
		All,
		Active,
		Inactive
	}

	/// <summary>
	/// Search Button Actions.
	/// </summary>
	public enum ButtonActions 
	{
		Add,
		AddFrom,
		Edit,
		EditWizard,  // DMB 23/02/2004 added 
		EditDialog,
		Service,
		Search,
		Select,
		Delete,
		TrashDelete,
		Clone,
		ViewTrash,
		ViewActive,
		Restore,
		Seperator,
		Report,
        ReportingServer, // DT 23/02/2006
		ReportMulti,
        SaveSearch, //CM 12/06/14
        OpenSearch, //CM 12/06/14
		SearchList,
        Workflow,
        FilterSwing,
		None
	}

	/// <summary>
	/// Search Columns Format
	/// </summary>
	public enum SearchColumnsFormat 
	{
		Standard,
		Currency,
		Number,
		RightAlign,
		DateRgt,
		DateLft,
		DateLongLft,
		DateTimeRgt,
		DateTimeLft,
		TimeRgt,
		TimeLft
	}

    public enum SearchColumnsDateIs
    {
        NotApplicable,
        UTC,
        Local
    }

    public enum SearchParameterDateIs
    {
        NotApplicable,
        UTC,
        Local,
        UTCEndOfDay,
        LocalEndOfDay,
        LocalStartOfDay,
        UTCStartOfDay,
        V2UTCEndOfDay,
        V2LocalEndOfDay,
    }

}

