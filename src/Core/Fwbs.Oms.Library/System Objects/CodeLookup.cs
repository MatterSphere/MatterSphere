using System;
using System.Data;
using System.Globalization;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{

    /// <summary>
    /// 11000 Code lookup object that manipulates and retrieves lookup items.  This will hold all 
    /// localized versions of the lookup using various different indexers.
    /// This object is not intended to be used with enquiries.
    /// </summary>
    public sealed class CodeLookup : IUpdateable, IDisposable
	{

		#region Fields

		/// <summary>
		/// Lookup data source.
		/// </summary>
		private DataTable _lookups = null;

		
		/// <summary>
		/// Bulk data table manipulation.  This is used by the enquiry engine to manipulate code lookups
		/// for the enquiry questions, pages etc.. in bulk so that one update can be used.
		/// </summary>
		private bool _bulk = false;
		
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbcodelookup";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public static string Table = "LOOKUPS";

		/// <summary>
		/// Default culture string.
		/// </summary>
		public const string DefaultCulture = "{default}";

		/// <summary>
		/// Parent of all parent code lookup codes.
		/// </summary>
		public const string SystemCode = "OMS";

		#endregion

		#region Constructors


		/// <summary>
		/// Passes an existing reference of a code lookup table to code lookups so that they can be 
		/// manipulated in bulk and also cached.  The enquiry designer will use this to update
		/// code lookups in bulk, hence why it is an internal constructor.  PLEASE NEVER MAKE THIS PUBLIC
		/// I HAVE NO IDEA WHAT WOULD HAPPEN IF THIS GOT LOSE IN THE OUTSIDE WORLD.
		/// </summary>
		/// <param name="lookups">Existing lookups table.</param>
		/// <param name="type">Default code lookup type.</param>
		/// <param name="code">Default code lookup code.</param>
		internal CodeLookup(DataTable lookups, string type, string code)
		{
			_bulk = true;
			_lookups = lookups;
			_lookups.TableName = Table;
			SetDataStructure(type, code);
			FindCodeLookup(type, code);
		}

		/// <summary>
		/// Retrieves an existing single code lookup item or creates a new one if it does not exist,
		/// using the first 15 characters of the passed code.  The code can therefore act as the default
		/// code lookup description as it is trimmed down.
		/// </summary>
		/// <param name="type">Code lookup parent type.</param>
		/// <param name="code">Code lookup code.</param>
		public CodeLookup(string type, string code)
		{	
			Session.CurrentSession.CheckLoggedIn();
			_bulk = false;
			FindCodeLookup(type, code);
		}

		public CodeLookup(string type)
		{
			Session.CurrentSession.CheckLoggedIn();
			_lookups = Session.CurrentSession.Connection.ExecuteSQLTable(Sql,"NEWCODE",true,new IDataParameter[0]);
			SetDataStructure(type, "");
			_bulk = true;
		}



		#endregion
		
		#region Indexers


		/// <summary>
		/// Returns a localized code lookup using a row index (the item will be in alphabetical order by culture name (en-GB);
		/// </summary>
		public CodeLookupLocalized this [int index]
		{
			get
			{
				try
				{
					return new CodeLookupLocalized(_lookups.DefaultView[index].Row);
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Returns a localized code lookup item by an actual culture object.  If it does not exist then
		/// the {default} lookup is used.
		/// </summary>
		public CodeLookupLocalized this [CultureInfo culture]
		{
			get
			{
				int ctr = 0;
				foreach (DataRow row in _lookups.Rows)
				{
					if (row["cduicultureinfo"].ToString() == culture.Name)
					{
						return this[ctr];
					}
					ctr++;
				}
				return this.Default;
			}
		}

		/// <summary>
		/// Returns a localized code lookup item by a culture string name (en-GB), again if it does not exist
		/// then the {default} lookup is returned.
		/// </summary>
		public CodeLookupLocalized this [string cultureName]
		{
			get
			{
				int ctr = 0;
				foreach (DataRow row in _lookups.Rows)
				{
					if (row["cduicultureinfo"].ToString() == cultureName)
					{
						return this[ctr];
					}
					ctr++;
				}

				return this.Default;
			}
		}

		#endregion

		#region Properties
		

		/// <summary>
		/// Gets the main code lookup code.
		/// </summary>
		public string Code
		{
			get
			{
				return GetExtraInfo("cdcode").ToString();
			}
		}

		/// <summary>
		/// Gets the Code lookup type / parent code.
		/// </summary>
		public string CodeType
		{
			get
			{
				return GetExtraInfo("cdtype").ToString();
			}
		}

		/// <summary>
		/// Gets the default localized lookup.
		/// </summary>
		public CodeLookupLocalized Default
		{
			get
			{
				return this[0];
			}
		}

		/// <summary>
		/// Gets the bulk flag.  If True then bulk code lookups can be manipulated
		/// internally before updated to the database.
		/// </summary>
		public bool Bulk
		{
			get
			{
				return _bulk;
			}
		}

		#endregion

		#region Static Methods

		public static bool Delete(object type, object code, object addlink)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, type);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, code);
            paramlist[2] = Session.CurrentSession.Connection.AddParameter("AddLink", SqlDbType.NVarChar, 15, addlink);
			int n;
			if (addlink == DBNull.Value)
                n = Session.CurrentSession.Connection.ExecuteSQL("DELETE FROM DBCodeLookup WHERE cdType = @Type and cdCode = @Code and cdSystem=0 and cdaddlink is null", paramlist);
			else
                n = Session.CurrentSession.Connection.ExecuteSQL("DELETE FROM DBCodeLookup WHERE cdType = @Type and cdCode = @Code and cdSystem=0 and cdaddlink = @AddLink", paramlist);
			if (n == 0)
				return false;
			else
				return true;
		}

        /// <summary>
        /// Creates a new code lookup object or overwrites an existing one depending on the parameter values passed.
        /// </summary>
        /// <param name="type">Code lookup type / grouping category.</param>
        /// <param name="code">Code lookup code under the group type.</param>
        /// <param name="description">Localized description / caption of the code lookup entry.</param>
        /// <param name="help">Localized help text of the code lookup entry.</param>
        /// <param name="culture">ui code of the code lookup entry.</param>
        /// <param name="useDefault">True uses the default culture, False uses the currently logged in culture.</param>
        /// <param name="overwrite">Overwrites a description / caption if the code lookup already exists.</param>
        /// <param name="save">Saves the code lookup item to the database.</param>
        /// <returns>A code lookup object to manipulate of update if the save parameter was set to false.</returns>
        public static CodeLookup Create (string type, string code, string description, string help, string culture, bool useDefault, bool overwrite, bool save)
		{
			return Create(type,code,description,help,culture,useDefault,false,DBNull.Value, overwrite,save);
		}
			
		public static CodeLookup Create (string type, string code, string description, string help, string culture, bool useDefault, bool group, object addlink, bool overwrite, bool save)
		{
			CodeLookup lkp = new CodeLookup(type, code);
			lkp.Add(description, help, culture, useDefault, group, addlink, overwrite);
			if (save)
				lkp.Update();
			return lkp;
		}

		/// <summary>
		/// Returns all code lookups with just the captions to display, in all languages.
		/// </summary>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetAllLookups()
		{
			return GetLookups(null, null,null, true);
		}

		/// <summary>
		/// Returns all the parent OMS system groups captions, in all languages.
		/// </summary>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetParentLookups()
		{
			return GetLookups(null, SystemCode, null, true);
		}

		/// <summary>
		/// Returns a singular parent OMS system group caption, in all languages
		/// </summary>
		/// <param name="code">Codelookup code type.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetParentLookups(string code)
		{
			return GetLookups(null, SystemCode, code, true);
		}


		/// <summary>
		/// Returns all the code lookups under a specified group / type, and displays their captions, in all languages.
		/// </summary>
		/// <param name="type">Code lookup parent type code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetAllLookups(string type)
		{
			return GetLookups(null, type, null, true);
		}

		/// <summary>
		/// Returns a specific code lookups caption in all languages.
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetAllLookups(string type, string code)
		{
			return GetLookups(null, type, code, true);
		}

		/// <summary>
		/// Returns all code lookup captions under the current logged in culture.
		/// </summary>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups()
		{
			return GetLookups(System.Threading.Thread.CurrentThread.CurrentUICulture, null,null, true);
		}

		/// <summary>
		/// Returns code lookup captions under a specified code lookup type under the current logged in culture.
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups(string type)
		{
			return GetLookups(System.Threading.Thread.CurrentThread.CurrentUICulture, type, null, true);
		}

		/// <summary>
		/// Returns a singular code lookup caption under the current logged in culture.
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups(string type, string code)
		{
			return GetLookups(System.Threading.Thread.CurrentThread.CurrentUICulture, type, code, true);
		}

		/// <summary>
		/// Returns all code lookup captions in a specific language.
		/// </summary>
		/// <param name="culture">Specified culture object.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups(CultureInfo culture)
		{
			return GetLookups(culture, null, null, true);
		}

		/// <summary>
		/// Returns code lookup captions under a specified code lookup type in a specific language.
		/// </summary>
		/// <param name="culture">Specified culture object.</param>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups(CultureInfo culture, string type)
		{
			return GetLookups(culture, type, null, true);
		}

		/// <summary>
		/// Returns a singular code lookup caption in a specific language.
		/// </summary>
		/// <param name="culture">Specified culture object.</param>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
		/// <returns>A DataTable object.</returns>
		public static DataTable GetLookups(CultureInfo culture, string type, string code)
		{
			return GetLookups(culture, type, code, true);
		}

		/// <summary>
		/// GetLookup
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
		/// <returns>Desription</returns>
		public static string GetLookup(string type, string code)
		{
			DataTable dt = GetLookups(type,code);
			if (dt.Rows.Count>0)
				return Convert.ToString(dt.Rows[0]["cddesc"]);
			else
				return "";
		}

		public static string GetLookupHelp(string type, string code)
		{
			DataTable dt = GetLookups(type,code);
			if (dt.Rows.Count>0)
				return Convert.ToString(dt.Rows[0]["cdhelp"]);
			else
				return "";
		}

		/// <summary>
		/// GetLookup
		/// </summary>
        /// <param name="culture"></param>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
        /// <param name="defaultcaption"></param>
		/// <returns>Desription</returns>
		public static string GetLookup(string culture, string type, string code, string defaultcaption)
		{
			System.Globalization.CultureInfo cult =  System.Globalization.CultureInfo.CreateSpecificCulture(culture);
			DataTable dt = GetLookups(cult, type,code);
			if (dt.Rows.Count>0)
			{
				if (Convert.ToString(dt.Rows[0]["cddesc"]).StartsWith("~"))
					return defaultcaption;
				else
					return Convert.ToString(dt.Rows[0]["cddesc"]);
			}
			else
				return "";
		}


		/// <summary>
		/// GetLookup
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
        /// <param name="defaultcaption"></param>
		/// <returns>Desription</returns>
		public static string GetLookup(string type, string code,string defaultcaption)
		{
			return GetLookup(System.Threading.Thread.CurrentThread.CurrentCulture.Name, type, code, defaultcaption);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="culture">Specified culture object.</param>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="code">Code lookup code.</param>
		/// <param name="captionOnly">True for captions only, false for the whole record.</param>
		/// <returns>A DataTable object</returns>
		public static DataTable GetLookups(CultureInfo culture, string type, string code, bool captionOnly)
		{	
			Session.CurrentSession.CheckLoggedIn();

            System.Collections.Generic.List<IDataParameter> paramlist = new System.Collections.Generic.List<IDataParameter>();
			if (type != null)
                paramlist.Add(Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, type));

            if (code != null)
                paramlist.Add(Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, code));
				
			if (culture != null)
                paramlist.Add(Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, culture.Name));
            else
                paramlist.Add(Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, DBNull.Value));

            paramlist.Add(Session.CurrentSession.Connection.AddParameter("Brief", SqlDbType.Bit, 0, captionOnly));

            
            return Session.CurrentSession.Connection.ExecuteProcedureTable("sprCodeLookupList", Table, paramlist.ToArray());
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <returns>A DataTable object</returns>
		public static DataTable GetLookupsAndAddLinks(string type)
		{	
			Session.CurrentSession.CheckLoggedIn();

			IDataParameter [] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, type);

			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCodeLookupListAddLink", Table, paramlist);

			return dt;
		}

		/// <summary>
		/// GetLookupsCompare
		/// </summary>
		/// <param name="type">Code lookup parent group type code.</param>
		/// <param name="s_culture">Source Culture</param>
		/// <param name="d_culture">Destination Culture</param>
		/// <returns>A DataTable object</returns>
		public static DataTable GetLookupsCompare(string type, string s_culture, string d_culture)
		{	
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = null;
			if (type != null) 
			{
				IDataParameter[] paramlist = new IDataParameter[3];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, type);
				paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI_S", SqlDbType.NVarChar, 15, s_culture);
				paramlist[2] = Session.CurrentSession.Connection.AddParameter("UI_D", SqlDbType.NVarChar, 15, d_culture);
				dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCodeLookupCompare", "LOOKUPSCOMPARE", paramlist);
			}
			else
			{
				IDataParameter[] paramlist = new IDataParameter[2];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI_S", SqlDbType.NVarChar, 15, s_culture);
				paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI_D", SqlDbType.NVarChar, 15, d_culture);
				dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCodeLookupCompare", "LOOKUPSCOMPARE", paramlist);
			}
			return dt;
		}


		/// <summary>
		/// Returns a table full of all the supported cultures, if specified.
		/// </summary>
		/// <param name="supportedOnly">Returns only those cultures which are flagged as supported.</param>
		/// <param name="includeDefault">Includes a default item at the top (e.g., {default} or null string).</param>
		/// <returns>A DataTable full of cultures.</returns>
		public static DataTable GetCultures(bool supportedOnly, bool includeDefault)
		{
			return GetCultures(supportedOnly, includeDefault, true);
		}


		/// <summary>
		/// Returns a table full of all the supported cultures, if specified.
		/// </summary>
		/// <param name="supportedOnly">Returns only those cultures which are flagged as supported.</param>
		/// <param name="includeDefault">Includes a default item at the top (e.g., {default} or null string).</param>
		/// <param name="codeLookupDefault">The code lookup default item is {default}, otherwise an empty null string is used.</param>
		/// <returns>A DataTable full of cultures.</returns>
		public static DataTable GetCultures(bool supportedOnly, bool includeDefault, bool codeLookupDefault)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[3];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("SupportedOnly", SqlDbType.Bit, 0, supportedOnly);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("IncludeDefault", SqlDbType.Bit, 0, includeDefault);
			paramlist[2] = Session.CurrentSession.Connection.AddParameter("CodeLookupDefault", SqlDbType.Bit, 0, codeLookupDefault);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprGetCultures", "CULTURES", paramlist);
			return dt;
		}

		/// <summary>
		/// Returns a table full of all the supported cultures, if specified.
		/// </summary>
		/// <param name="Type">Returns only those cultures which are flagged as supported.</param>
		/// <param name="Code">Includes a default item at the top (e.g., {default} or null string).</param>
		/// <returns>A DataTable full of cultures.</returns>
		public static DataTable GetCultures(string Type, string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] paramlist = new IDataParameter[2];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, Type);
			paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code);
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprGetCultures", "CULTURES", paramlist);
			return dt;
		}



		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		public bool IsNew
		{
			get
			{
				try
				{
					return (_lookups.Rows[0].RowState == DataRowState.Added);
				}
				catch
				{
					return false;
				}
			}
		}
			
		/// <summary>
		/// Updates the whole code lookup and persists all changes to localized lookup items to the dtabase.
		/// </summary>
		public void Update()
		{
			string type = this.CodeType;
			string code = this.Code;

			DataTable changes = _lookups.GetChanges();

            
            if (changes != null)
            {
                //check for any empty codes in the changes datatable
                foreach (DataRow r in changes.Rows)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(r["cdcode"])))
                    {
                        throw new Exception(Session.CurrentSession.Resources.GetResource("CLNOCODE", "The code for a Code Lookup has been found to be empty.\n\nPlease check that all Code Lookups have the necessary values before attempting to add them to the system.", "").Text);
                    }
                }

                //Run this update command to automatically, insert, delete and update commands.
                Session.CurrentSession.Connection.Update(_lookups, Sql);

                //Run the following stored procedure to make sure that a default item is added if not already present.
                DataView dv = new DataView(changes, "", "cdcode", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    string prevcode = "";
                    foreach (DataRowView item in dv)
                    {
                            if (Convert.ToString(item["cdcode"]) == prevcode)
                                continue;

                            CodeLookupLocalized loc = this.Default;
                            IDataParameter[] paramlist = new IDataParameter[10];
                            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, item["cdType"]);
                            paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, item["cdCode"]);
                            paramlist[2] = Session.CurrentSession.Connection.AddParameter("Description", SqlDbType.NVarChar, 500, item["cddesc"]);
                            paramlist[3] = Session.CurrentSession.Connection.AddParameter("Help", SqlDbType.NVarChar, 500, item["cdhelp"]);
                            paramlist[4] = Session.CurrentSession.Connection.AddParameter("Notes", SqlDbType.NVarChar, 500, item["cdnotes"]);
                            paramlist[5] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, CodeLookup.DefaultCulture);
                            paramlist[6] = Session.CurrentSession.Connection.AddParameter("System", SqlDbType.Bit, 0, item["cdsystem"]);
                            paramlist[7] = Session.CurrentSession.Connection.AddParameter("Deletable", SqlDbType.Bit, 0, item["cddeletable"]);
                            if (String.IsNullOrEmpty(Convert.ToString(item["cdaddlink"])))
                                paramlist[8] = Session.CurrentSession.Connection.AddParameter("AddLink", SqlDbType.NVarChar, 15, DBNull.Value);
                            else
                                paramlist[8] = Session.CurrentSession.Connection.AddParameter("AddLink", SqlDbType.NVarChar, 15, item["cdaddlink"]);
                            paramlist[9] = Session.CurrentSession.Connection.AddParameter("Group", SqlDbType.Bit, 0, item["cdgroup"]);
                            Session.CurrentSession.Connection.ExecuteProcedure("sprCreateCodeLookup", paramlist);
                            prevcode = Convert.ToString(item["cdcode"]);
                    }

                    Caching.IQueryCache cache = Session.CurrentSession.CachedQueries.Get<Caching.Queries.ICodeLookupQueryCache>();
                    if (cache != null)
                        cache.Clear(String.Format("sprCodeLookupList#Type={0}", this.CodeType), Caching.CacheSearch.StartsWith);
                }
            }

			//Re-find the lookup to make sure it exists in the database.
			if (_lookups.TableName != "NEWCODE")
                FindCodeLookup(type, code);
		}


		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Gets the changes of the current object and and refreshes the object
		/// then reapplies the changes to avoid any concurrency issues.  This is in 
		/// theory forcing any changes made to the object.
		/// </summary>
		/// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
		public void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

			DataTable changes = _lookups.GetChanges();

			//Re-find the lookup to make sure it exists in the database.
			FindCodeLookup(this.CodeType, this.Code);
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			_lookups.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return (_lookups.GetChanges() != null);
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Checks to see if a localized culture version already exists.
		/// </summary>
		/// <param name="cultureName">Culture name to compare (en-GB etc...)</param>
		/// <returns>True if the localized code lookup exists.</returns>
		public bool HasCulture(string cultureName)
		{
			foreach (DataRow row in _lookups.Rows)
			{
				if (Convert.ToString(row["cduicultureinfo"]).ToLower() == cultureName.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Recieves the actual code lookup filtered list whether it is being cached or retrieved directly from the database.
		/// </summary>
		/// <param name="type">Code lookup type.</param>
		/// <param name="code">Code lookup code.</param>
		public void FindCodeLookup (string type, string code)
		{
			if (_lookups == null) 
			{
				_bulk = false;
				_lookups = new DataTable(Table);
			}
			else
			{
				_lookups.DefaultView.RowFilter = "cdtype = '" + type.Replace("'", "''") + "' and cdcode = '" + code.Replace("'", "''") + "'";
			}


			//In bulk mode, if there are no filtered record add a ew record with the default values.
			if (_bulk)
			{
				SetDataStructure(type, code);

			}
			else
			{
                if (_lookups.DefaultView.Count == 0 || _lookups.Rows.Count == 0 || Session.CurrentSession._designMode)
				{
					_lookups = CodeLookup.GetLookups(null, type, code, false);
					_lookups.TableName = Table;
					SetDataStructure(type, code);

					//Filter the existing data table down by using a data view.
					_lookups.DefaultView.RowFilter = "cdtype = '" + type.Replace("'", "''") + "' and cdcode = '" + code.Replace("'", "''") + "'";
				}
			}
		}

		/// <summary>
		/// Sets the data structure / schema of the internal data set.
		/// </summary>
		/// <param name="type">Default code lookup type.</param>
		/// <param name="code">Default code lookup code.</param>
		private void SetDataStructure(string type, string code)
		{
			_lookups.Columns["cdtype"].DefaultValue = type;
			_lookups.Columns["cdcode"].DefaultValue = code.ToUpper();
			_lookups.Columns["cdaddlink"].DefaultValue = DBNull.Value;
			_lookups.Columns["cduicultureinfo"].DefaultValue = CodeLookup.DefaultCulture;
			
			if (code.Length > 15)
				_lookups.Columns["cdcode"].DefaultValue = code.Substring(0, 15);

			_lookups.Columns["cdsystem"].DefaultValue = false;
			_lookups.Columns["cddeletable"].DefaultValue = true;
			_lookups.Columns["cdgroup"].DefaultValue = false;
			_lookups.Columns["cduicultureinfo"].AllowDBNull = false;
			
			_lookups.DefaultView.AllowDelete = false;

			if (!_lookups.Constraints.Contains("UNIQUE"))
				_lookups.Constraints.Add("UNIQUE", new DataColumn[] {_lookups.Columns["cdtype"], _lookups.Columns["cdCode"], _lookups.Columns["cduicultureinfo"], _lookups.Columns["cdaddlink"]}, false);
		}


		/// <summary>
		/// Method used for capturing the row changed event of the data table.  This is used to 
		/// make sure no obvious changes are made to system type code lookups.
		/// </summary>
		/// <param name="sender">DataTable that called the method.</param>
		/// <param name="e">Event arguments that can be used to check the changed row characteristics.</param>
		private void RowChanged (object sender, DataRowChangeEventArgs e)
		{ 
		}

		/// <summary>
		/// Returns a string representation of the user object which in this case is the lookups code.
		/// </summary>
		/// <returns>Lookups code</returns>
		public override string ToString()
		{
			return this.Code;
		}

		/// <summary>
		/// Adds a localized code lookup to the current code lookup collection.
		/// </summary>
		/// <param name="codeLookup">Localized code lookup object.</param>
		public void Add (CodeLookupLocalized codeLookup)
		{			
			if (codeLookup !=null)
			{
				DataTable dt = codeLookup.GetDataTable();			
				if (dt != null && dt.Rows.Count > 0)
				{
					_lookups.LoadDataRow(dt.Rows[0].ItemArray, false);
					
					//Filter the existing data table down by using a data view.
					//Check to see if the {default} item already exists or not.
					DataView vw = new DataView(_lookups);
					vw.RowFilter = "cdtype = '" + codeLookup.CodeType.Replace("'", "''") + "' and cdcode = '" + codeLookup.Code.Replace("'", "''") + "' and cduicultureinfo = '" + CodeLookup.DefaultCulture.Replace("'", "''") + "'";

					if (vw.Count == 0)
					{
						DataRow defaultcode = dt.Rows[0];
						defaultcode["cduicultureinfo"] = CodeLookup.DefaultCulture;
						_lookups.LoadDataRow(defaultcode.ItemArray, false);
					}

					vw.Dispose();
					vw = null;
				}
			}
		}

	
		/// <summary>
		/// Adds a localized code lookup to the current code lookup collection.
		/// This will create a default code lookup entry, or overwrite an existing default value.
		/// </summary>
		/// <param name="description">Description / caption of the code lookup.</param>
		public void Add(string description)
		{			
			Add(description, null, true, false);
		}

		/// <summary>
		/// Adds a localized code lookup to the current code lookup collection.
		/// </summary>
		/// <param name="description">Description / caption of the code lookup.</param>
		/// <param name="help">Help Text of the code lookup.</param>
		/// <param name="culture">Description / caption of the code lookup.</param>
		/// <param name="useDefault"> This will create a default code lookup entry.</param>
        /// <param name="group"></param>
        /// <param name="addlink"></param>
		/// <param name="overwrite">Edits the existing item if it already exists.</param>
		public void Add(string description, string help, string culture, bool useDefault, bool group, object addlink, bool overwrite)
		{
			if (overwrite)
				overwrite = this.HasCulture(culture);

			if (overwrite)
			{
				this[culture].Caption = description;
				this[culture].HelpText = help;
			}
			else
			{
				DataRow rw = _lookups.NewRow();
				rw["cduicultureinfo"] = culture;
				if (help != null)
					rw["cdhelp"] = help;
				rw["cddesc"] = description;
				rw["cdgroup"] = group;
				rw["cdaddlink"] = addlink;
				_lookups.Rows.Add(rw);
			}
		}

		/// <summary>
		/// Adds a localized code lookup to the current code lookup collection.
		/// </summary>
		/// <param name="description">Description / caption of the code lookup.</param>
		/// <param name="help">Help Text of the code lookup.</param>
		/// <param name="useDefault"> This will create a default code lookup entry.</param>
		/// <param name="overwrite">Edits the existing item if it already exists.</param>
		public void Add(string description, string help, bool useDefault, bool overwrite)
		{
			string culture = CodeLookup.DefaultCulture;

			if (!useDefault)
				culture = Session.CurrentSession.DefaultCulture;

			if (overwrite)
				overwrite = this.HasCulture(culture);

			if (overwrite)
			{
				this[culture].Caption = description;
				this[culture].HelpText = help;
			}
			else
			{
				DataRow rw = _lookups.NewRow();
				rw["cduicultureinfo"] = culture;
				rw["cddesc"] = description;
				rw["cdhelp"] = help;
				_lookups.Rows.Add(rw);
			}
		}


		/// <summary>
		/// Returns a data view with the filtered down records of the specific code lookup.
		/// </summary>
		/// <returns>A DataView object</returns>
		public DataView GetDataView()
		{
			return _lookups.DefaultView;
		}

		/// <summary>
		/// Returns a data Table
		/// </summary>
		/// <returns>A DataTable object</returns>
		public DataTable GetDataTable()
		{
			return _lookups;
		}
		
		/// <summary>
		/// Normally externally accessed but not in this case as this object is not ideal for enquiry integration.
		/// Returns the first record data specified by the given database field.
		/// </summary>
		/// <param name="fieldName">Database field name.</param>
		/// <returns>A database value.</returns>
		private object GetExtraInfo(string fieldName)
		{
			object val = GetExtraInfo(fieldName, 0);
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		
		/// <summary>
		/// Returns a database value from a specific field index with a specific field name.
		/// </summary>
		/// <param name="fieldName">Database field name.</param>
		/// <param name="index">Row index.</param>
		/// <returns>A dtabase value.</returns>
		private object GetExtraInfo(string fieldName, int index)
		{
            object val;
            if (_lookups.DefaultView.Count > index)
                val = _lookups.DefaultView[index].Row[fieldName];
            else if (_lookups.DefaultView.Count == 1)
                val = _lookups.DefaultView[0].Row[fieldName];
            else
                val = string.Empty;

            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		/// <summary>
		/// Returns all the valid codes with the specified type code.
		/// </summary>
		/// <param name="type">Parent type to filter by.</param>
		/// <returns>A DataView object.</returns>
		public DataView GetLookupCodes(string type)
		{
			if (_bulk)
			{
				DataView codes = new DataView(_lookups);
				codes.RowFilter = "cdtype = '" + type.Replace("'", "''") + "' and cduicultureinfo = '" + CodeLookup.DefaultCulture.Replace("'", "''") + "'";
				codes.Sort = "cddesc";
				return codes;
			}
			else
				return CodeLookup.GetLookups(type).DefaultView;


		}

		#endregion

		#region IDisposable Implementation


		/// <summary>
		/// Disposes the code lookup object immediately witout waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			_lookups.Dispose();
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by the code lookup object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
                if (!_bulk)
                {
                    if (_lookups != null)
                        _lookups.Dispose();
                    _lookups = null;
                }
			}
		}

		#endregion

	}

	/// <summary>
	/// Localized code lookup settings.  This code lookup object can be used with the enquiry engine.
	/// </summary>
	public sealed class CodeLookupLocalized : IEnquiryCompatible, IDisposable
	{
		#region Fields

		/// <summary>
		/// Internal data source.  In this object, this could be a DataRow or a DataTable with a singular row.
		/// </summary>
		private object _data;
		
		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbcodelookup";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "LOOKUPS";


		/// <summary>
		/// Default enquiry form string.
		/// </summary>
		public const string QuickCodeCreateQ1 = "ENQADDCODE_Q1";

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new localized code lookup item.  This routine is used by the enquiry engine
		/// to create new localized code lookup object.
		/// </summary>
		internal CodeLookupLocalized()
		{
			//Get the structure of the relevant table.
			DataTable lookup = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Allow DBNulls on each column for the time beign
			foreach (DataColumn col in lookup.Columns)
				if (!col.AllowDBNull) col.AllowDBNull = true;

			//Set a new row up ready for the enquiry engine to add into.
			lookup.Rows.Add(lookup.NewRow());

			//Assign the table to the internal data source.
			_data = lookup;
		}

		/// <summary>
		/// Initialised an existing localized code lookup object with the specified identifier.
		/// </summary>
		/// <param name="ID">Code Lookup Identifier.</param>
		internal CodeLookupLocalized (long ID)
		{
			DataTable lookup = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where cdid = " + ID.ToString(), Table, new IDataParameter[0]);
			if ((lookup == null) || (lookup.Rows.Count == 0)) 
				throw new OMSException (HelpIndexes.CodeLookupDoesNotExist, ID.ToString());
			else
				_data = lookup;
		}

		/// <summary>
		/// Forms a new code lookup object using a passed lookup row structure.  The CodeLookup object
		/// uses this functionality to display each localized lookup under the one code lookup code.
		/// </summary>
		/// <param name="row">Lookup row structure based on dbCodeLookup Table.</param>
		internal CodeLookupLocalized(DataRow row)
		{
			_data = row;
		}

		#endregion

		#region Properties


		/// <summary>
		/// Gets the numerical unique identifier of the code lookup entry.
		/// </summary>
		public long ID
		{
			get
			{
				return (long)GetExtraInfo("cdid");
			}
		}

		/// <summary>
		/// Gets the code of the code lookup.
		/// </summary>
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cdcode"));
			}

		}

		/// <summary>
		/// Gets the code type of the code lookup.
		/// </summary>
		public string CodeType
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cdtype"));
			}
		}

		/// <summary>
		/// Gets the culture name is a string format ({default}, en-GB etc..).
		/// </summary>
		public string Culture
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cduicultureinfo"));
			}
		}

		/// <summary>
		/// Gets or Sets the lLocalized caption / text of the code lookup.
		/// </summary>
		public string Caption
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cddesc"));
			}
			set
			{
				SetExtraInfo("cddesc", value);
				//Make sure that there is a code lookup code set.
				CheckValidCode();
			}
		}

		/// <summary>
		/// Gets or Sets the localized help text / tool tip information.
		/// </summary>
		public string HelpText
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cdhelp"));
			}
			set
			{
				SetExtraInfo("cdhelp", value);
			}
		}


		/// <summary>
		/// Gets the system flag value.  This will stop the lookup item from being changed and deleted unwillingly.
		/// </summary>
		public bool System
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("cdsystem");
				}
				catch
				{
					return false;
				}
			}
		}


		/// <summary>
		/// Gets the deleteable flag.  This will stop any unwilling deletions of lookups flagged to true.
		/// </summary>
		public bool Deletable
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("cddeletable");
				}
				catch
				{
					return true;
				}
			}
		}


		/// <summary>
		/// Gets the group flag.  This indicates that the code lookup may contain others.
		/// </summary>
		public bool Group
		{
			get
			{
				try
				{
					return (bool)GetExtraInfo("cdgroup");
				}
				catch
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Gets the additional lookup information on an existing lookup.  This is used for backward compatibility
		/// and is part of the code lookups unqique key along with Type, Culture and Code. 
		/// </summary>
		public string AdditionalLink
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cdaddlink"));
			}
		}

		/// <summary>
		/// Get the internal notes on the lookup that explains in more detail what the lookup is about.
		/// </summary>
		public string Notes
		{
			get
			{
				return Convert.ToString(GetExtraInfo("cdnotes"));
			}
		}

		/// <summary>
		/// Gets all localizable lookups of the current code lookup.
		/// </summary>
		public CodeLookup CodeLookup
		{
			get
			{
				return new CodeLookup(this.CodeType,this.Code);
			}
		}

		#endregion

		#region IExtraInfo Implementation
		
	
		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		public void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			//Set the row directly if it is of the type DataRow.
			//Otherwise, set the first row in the data table.
			if (_data is DataRow)
				((DataRow)_data)[fieldName] = val;
			else if (_data is DataTable)
				((DataTable)_data).Rows[0][fieldName] = val;
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{
            object val;
			if (_data is DataRow)
				val = ((DataRow)_data)[fieldName];
			else if (_data is DataTable)
				val = ((DataTable)_data).Rows[0][fieldName];
			else
				val = null;

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
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
				if (_data is DataRow)
					return ((DataRow)_data).Table.Columns[fieldName].DataType;
				else if (_data is DataTable)
				{
					return ((DataTable)_data).Columns[fieldName].DataType;
				}
				else
					return null;
				
			}
			catch 
			{
				throw new OMSException2("11001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}

		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public DataSet GetDataset()
		{
			//Creates a new data set object and adds a newly created table to it
			//ready for return.
			DataSet ds = new DataSet();
			ds.Tables.Add (GetDataTable());
			return ds;
		}


		/// <summary>
		/// Returns a data table representation of the object.  The data table is copied
		/// so that it can be added to another dataset without confusion of an existing dataset.
		/// </summary>
		/// <returns>DataTable object.</returns>
		public DataTable GetDataTable()
		{
			DataTable dt = new DataTable(Table);
			
			//If the internal data object is already a data table then
			//return a copy of that table. 
			if (_data is DataTable)
				dt = ((DataTable)_data).Copy();
			else if (_data is DataRow)
			{
				//If the internal data object is a data row and it has a parent table that owns it
				//then return a copy of that parent table.
				dt = ((DataRow)_data).Table;
			}

			dt.TableName = Table;

			return dt;
		}


		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		public bool IsNew
		{
			get
			{
				try
				{
					//Only update the data if it is modified in some sort of way.
					DataRowState state = DataRowState.Unchanged;

					//If the internal data object is a data row then get its row state directly.
					//Otherwise, if it is a data table then return the first rows row state.
					if (_data is DataRow)
						state = ((DataRow)_data).RowState;
					else if (_data is DataTable)
						state = ((DataTable)_data).Rows[0].RowState;
					
					return (state == DataRowState.Added);
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
		public void Update()
		{
			//Only update the data if it is modified in some sort of way.
			DataRowState state = DataRowState.Unchanged;

			//If the internal data object is a data row then get its row state directly.
			//Otherwise, if it is a data table then return the first rows row state.
			if (_data is DataRow)
				state = ((DataRow)_data).RowState;
			else if (_data is DataTable)
				state = ((DataTable)_data).Rows[0].RowState;


			//Make sure that there is a code lookup code set.
			CheckValidCode();

			//If the state is being added then run the stored procedure that creates the code lookup and a default one if need be.
			if (state == DataRowState.Added) 
			{
				IDataParameter [] paramlist = new IDataParameter[10];
				paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 15, GetExtraInfo("cdtype").ToString());
				paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, GetExtraInfo("cdcode").ToString());
				paramlist[2] = Session.CurrentSession.Connection.AddParameter("Description", SqlDbType.NVarChar, 500, Caption);
				paramlist[3] = Session.CurrentSession.Connection.AddParameter("Help", SqlDbType.NVarChar, 500, HelpText);
				paramlist[4] = Session.CurrentSession.Connection.AddParameter("Notes", SqlDbType.NVarChar, 500, Notes);
				paramlist[5] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, Culture);
				paramlist[6] = Session.CurrentSession.Connection.AddParameter("System", SqlDbType.Bit, 0, System);
				paramlist[7] = Session.CurrentSession.Connection.AddParameter("Deletable", SqlDbType.Bit, 0, Deletable);
				if (AdditionalLink == String.Empty)
					paramlist[8] = Session.CurrentSession.Connection.AddParameter("AddLink", SqlDbType.NVarChar, 15, DBNull.Value);
				else
					paramlist[8] = Session.CurrentSession.Connection.AddParameter("AddLink", SqlDbType.NVarChar, 15, AdditionalLink);
				paramlist[9] = Session.CurrentSession.Connection.AddParameter("Group", SqlDbType.Bit, 0, Group);

				Session.CurrentSession.Connection.ExecuteProcedure("sprCreateCodeLookup", paramlist);
			}
			else if (state != DataRowState.Unchanged)
			{
				//If any changes have been made then save them to the databse using the SQL statement.				
				DataTable dt;

				if (_data is DataTable)
				{
					dt = (DataTable)_data;
					Session.CurrentSession.Connection.Update(dt, Sql);
				}
				else if (_data is DataRow)
				{
					dt = ((DataRow)_data).Table;
					Session.CurrentSession.Connection.Update(dt, Sql);
				}
					
			}
		}


		/// <summary>
		/// Refreshes the current object with the one from the database to prevent 
		/// any potential concurrency issues.
		/// </summary>
		public void Refresh()
		{
			Refresh(false);
		}

		/// <summary>
		/// Gets the changes of the current object and and refreshes the object
		/// then reapplies the changes to avoid any concurrency issues.  This is in 
		/// theory forcing any changes made to the object.
		/// </summary>
		/// <param name="applyChanges">Applies / merges the current changes to the refreshed data.</param>
		public void Refresh(bool applyChanges)
		{
            if (IsNew)
                return;

			if (_data is DataTable)
			{
				DataTable dt = (DataTable)_data;
				DataTable changes = dt.GetChanges();

				CodeLookupLocalized refresh = new CodeLookupLocalized(this.ID); 
				DataTable temp = refresh.GetDataTable();
				refresh.Dispose();
				refresh = null;

				if (temp.Rows.Count > 0)
				{
					dt = temp;
				}
				
				if (applyChanges)
				{
					if (changes.Rows.Count > 0)
						dt.Rows[0].ItemArray = changes.Rows[0].ItemArray;
				}
			}
		}

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			if (_data is DataTable)
				((DataTable)_data).RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				if (_data is DataTable)
					return (((DataTable)_data).GetChanges() != null);
				else
					return false;
			}
		}

		#endregion

		#region IEnquiryCompatible Implementation

		/// <summary>
		/// An event that gets raised when a property changes within the object.
		/// </summary>
		public event EnquiryEngine.PropertyChangedEventHandler PropertyChanged = null;

		/// <summary>
		/// Raises the property changed event with the specified event arguments.
		/// </summary>
		/// <param name="e">Property Changed Event Arguments.</param>
		private void OnPropertyChanged (PropertyChangedEventArgs e)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, e);
		}

		/// <summary>
		/// Edits the current code lookup object in the form of an enquiry (if the database states that is edit compatible).
		/// </summary>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(Common.KeyValueCollection param)
		{
			return Edit(Session.CurrentSession.DefaultSystemForm(SystemForms.None), param);
		}

		/// <summary>
		/// Edits the current code lookup object in the form of an enquiry (if the database states that is edit compatible) with a custom form code.
		/// </summary>
		/// <param name="customForm">Enquiry form code.</param>
		/// <param name="param">Named parameter collection.</param>
		/// <returns>Enquiry object ready to be rendered.</returns>
		public Enquiry Edit(string customForm, Common.KeyValueCollection param)
		{
			return  Enquiry.GetEnquiry(customForm, Parent, this, param);
		}


		#endregion

		
		#region IParent Implementation

		/// <summary>
		/// Gets the parent related object.
		/// </summary>
		public object Parent
		{
			get
			{
				return null;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Makes sure that a codel lookup code code is valid.
		/// </summary>
		private void CheckValidCode ()
		{
			//Check to see if the code has been populated, if not take the first 15 characters of the description.
			object code = GetExtraInfo("cdcode");
			string desc = GetExtraInfo("cddesc").ToString();

			if (FWBS.Common.SQLRoutines.IsNullString(code))
			{
				if (desc.Length >= 15)
					code = desc.Substring(0, 15);
				else
					code = desc;
				SetExtraInfo("cdcode", code.ToString());
			}
		}

		/// <summary>
		/// Returns a string representation of the user object which in this case is the lookups caption.
		/// </summary>
		/// <returns>Localized lookup caption.</returns>
		public override string ToString()
		{
			return this.Caption;
		}

		#endregion

		#region IDisposable Implementation


		/// <summary>
		/// Disposes the code lookup object immediately witout waiting for the garbage collector.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		/// <summary>
		/// Disposes all internal objects used by the code lookup object.
		/// </summary>
		/// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
                if (_data is DataTable)
                {
                    ((DataTable)_data).Dispose();
                    _data = null;
                }
			}
			
			
		}


		#endregion

	}
}

