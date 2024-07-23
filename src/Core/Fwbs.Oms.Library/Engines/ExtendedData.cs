using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    /// <summary>
    /// An object that is used to manipulate external data sources in a variety of different fashions.
    /// It could be holding a reference to a data set filled with data from a specified dynamic
    /// connection string, a linked server or an object reference to an external assembly / class.
    /// This extended data object can be used with the enquiry engine.
    /// </summary>
    /// <errorNumber>26000</errorNumber>
    public class ExtendedData : SourceEngine.Source, IUpdateable, IExtraInfo, IDisposable
	{
		#region Fields
		/// <summary>
		/// Internal object that could be a data table or an IEnquiryCompatible object.
		/// </summary>
		private object _obj = null;

		/// <summary>
		/// Extra Info object that is being edited.
		/// </summary>
		private IExtraInfo _extra = null;


		/// <summary>
		/// Internal data source for the extended data configuration.
		/// </summary>
		protected DataTable _extended = null;

		/// <summary>
		/// The source data set that sets the dynamic questions up for the extended data renderer.
		/// </summary>
		private DataSet _questionSource = null;

		/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbExtendedData";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "EXTENDEDDATA";

		/// <summary>
		/// The table name for the data object data that the extended data.
		/// </summary>
		internal const string Table_Data = "DATA";

		
		/// <summary>
		/// The table name for the questions of the dynamic the extended data object.
		/// </summary>
		internal const string Table_Question = "QUESTIONS";


		/// <summary>
		/// Static Array list of Return Fields loaded by the Static Method GetReturnFields
		/// </summary>
		private static ArrayList _returnfields = new ArrayList();

		/// <summary>
		/// the last code used by the Static Method GetReturnFields
		/// </summary>
		private static string _returnfieldscode;
		
		/// <summary>
		/// Original extended data code.
		/// </summary>
		private string _orgcode = "";

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Creates a new extended data object.  This routine is used by the enquiry engine
		/// to create new extended data object.
		/// </summary>
		public ExtendedData()
		{
			_extended = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, new IDataParameter[0]);
			
			//Set up a new empty record for the enquiry engine to manipulate.
			Global.CreateBlankRecord(ref _extended, true);
			_extended.Rows[0]["extParameters"] = "<params></params>";
			_extended.Rows[0]["extSourceType"] = "OMS";
			
			//A bit mask of the valid modes (add, edit, view + delete) available to the data source, this will reduce the risk of any errors if the underlying data source is read only.
			_extended.Rows[0]["extModes"] = 15;
			
		}


		/// <summary>
		/// Initializes an existing extended data object with the specified identifier.
		/// </summary>
		/// <param name="code">extended Data Identifier.</param>
		public ExtendedData (string code)
		{
            Fetch(code, null);
		}


		/// <summary>
		/// A constructor that the external data list uses to pass a singular record
		/// to this object for OOP purposes.
		/// </summary>
		/// <param name="linkObject">The enquiry compatible object that is being edited.</param>
		/// <param name="dt">A data table object.</param>
		/// <param name="row">Row to add.</param>
		internal ExtendedData (IExtraInfo linkObject, DataTable dt, DataRow row)
		{

			_extended = dt.Copy();
			_extended.TableName = Table;
			_extended.Clear();
			_extended.LoadDataRow(row.ItemArray, true);


			//Set the base source objects fields.
			base.SourceType = (SourceEngine.SourceType)Enum.Parse(typeof(SourceEngine.SourceType), Convert.ToString(GetExtraInfo("extsourcetype")), true);
			base.Src = Convert.ToString(GetExtraInfo("extsource"));
			base.Call  =  Convert.ToString(GetExtraInfo("extcall")) + " " + Convert.ToString(GetExtraInfo("extwhere"));
			base.Parameters =  Convert.ToString(GetExtraInfo("extparameters"));

			_extra = linkObject;
		}

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
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_obj != null)
				{
					if (_obj is IDisposable)
						((IDisposable)_obj).Dispose();

					_obj = null;
				}
				if (_extended != null)
				{
					_extended.Dispose();
					_extended = null;
				}

				if (_questionSource != null)
				{
					_questionSource.Dispose();
					_questionSource = null;
				}
			}

			//Dispose unmanaged objects.
		}

		#endregion

		#region Methods

        private void Fetch(string code, DataRow merge)
        {
            var data = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where extCode = '" + SQLRoutines.RemoveRubbish(code.ToString()) + "'", Table, new IDataParameter[0]);
            //Set the base source objects fields.
            _orgcode = code;

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _extended = data;

            base.SourceType = (SourceEngine.SourceType)Enum.Parse(typeof(SourceEngine.SourceType), Convert.ToString(GetExtraInfo("extsourcetype")), true);
            base.Src = Convert.ToString(GetExtraInfo("extsource"));
            base.Call = Convert.ToString(GetExtraInfo("extcall")) + " " + Convert.ToString(GetExtraInfo("extwhere"));
            base.Parameters = Convert.ToString(GetExtraInfo("extparameters"));

            
        }

		/// <summary>
		/// Sets the caption / description of the column to be displayed.
		/// </summary>
		/// <param name="columnName">The column name to set the caption of.</param>
		/// <param name="text">The new caption / description.</param>
		internal void SetCaption(string columnName, string text)
		{
			DataTable data = GetExtendedDataTable();
			if (data.Columns.Contains(columnName))
				data.Columns[columnName].Caption = text;
		}

		public DataTable GetDBFields()
		{
			IDataParameter[] param = new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("ExtCode", SqlDbType.NVarChar, 15, this.Code)};
			DataTable fields = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM DBFields WHERE fldExtended = @ExtCode","FIELDS",false,param);
			return fields;
		}

		public void AddDBField(string FieldName)
		{
            string codename = FieldName.GetHashCode().ToString();
            try {CodeLookup code = CodeLookup.Create("FIELDS", codename, FieldName, "", CodeLookup.DefaultCulture, true, false, true);}
            catch (System.Data.ConstraintException)
            { }
			IDataParameter[] param = new IDataParameter[4];
			param[0] = Session.CurrentSession.Connection.AddParameter("code", SqlDbType.NVarChar, 15, codename);
			string group = "";
			OmsObject obj = new OmsObject(this.Code);

            switch (obj.TypeCompatible.ToUpper())
            {
                case "FWBS.OMS.OMSFILE":
                    group = "FILE";
                    break;
                case "FWBS.OMS.CLIENT":
                    group = "CL";
                    break;
                case "FWBS.OMS.USER":
                    group = "USR";
                    break;
                case "FWBS.OMS.CONTACT":
                    group = "CONT";
                    break;
                case "FWBS.OMS.ASSOCIATE":
                    group = "ASSOC";
                    break;
                case "FWBS.OMS.FEEEARNER":
                    group = "FEE";
                    break;
            }

			param[1] = Session.CurrentSession.Connection.AddParameter("group", SqlDbType.NVarChar, 15, group);
			param[2] = Session.CurrentSession.Connection.AddParameter("extcode", SqlDbType.NVarChar, 15, this.Code);
			param[3] = Session.CurrentSession.Connection.AddParameter("fldname", SqlDbType.NVarChar, 255, FieldName);
			Session.CurrentSession.Connection.ExecuteSQL("INSERT INTO DBFields (fldType,fldCode,fldGroup,fldextended, fldname) VALUES ('$$',@code,@group,@extcode,@fldname)",param);
		}

		/// <summary>
		/// Gets the underlying extended data rendering questions and underlying data source.
		/// </summary>
		/// <param name="method">A delegate to be passed so that the calling class can be asked how the UI will display the field type.</param>
		/// <returns>A data set.</returns>
		public DataSet GetSource(FieldDisplayType method)
		{
			if (_questionSource != null)
			{
				_questionSource.Tables.Remove(Table_Data);
				_questionSource.Tables.Remove(Table_Question);
				_questionSource.Dispose();
				_questionSource = null;
			}

			_questionSource = new DataSet("EXTENDEDDATA");
			DataTable questions = GetRenderingQuestions();
			
			DataTable data = GetExtendedDataTable();
			data.TableName = Table_Data;
			_questionSource.Tables.Add(questions);
			_questionSource.Tables.Add(data);
			int tab = 0;

			//Load the exclusion fields into a dom.
			System.IO.StringReader rdr = new System.IO.StringReader(Convert.ToString(GetExtraInfo("extFields")));
			System.Xml.XPath.XPathDocument xpath = new System.Xml.XPath.XPathDocument(rdr);
			System.Xml.XPath.XPathNavigator nav = xpath.CreateNavigator();
			
			foreach (DataColumn col in data.Columns)
			{
				if (!col.AutoIncrement)
				{
					if (col.DataType != typeof(System.Guid))
						if (nav.Select("config/exclusions/field[.='" + XmlConvert.EncodeNmToken(col.ColumnName) + "']").Count == 0)
						{
							DataRow row = questions.NewRow();
							row["qufieldname"] = col.ColumnName;
							row["quname"] = col.ColumnName;
							row["qucode"] = col.ColumnName;
							row["qudesc"] = (col.Caption == String.Empty ? col.ColumnName : col.Caption);
							if (method == null)
								row["qucontrol"] = "FWBS.Common.UI.Windows.eTextBox2,EnquiryControls.WinUI";
							else
								row["qucontrol"] = method(col).AssemblyQualifiedName;
							row["qudatalist"] = DBNull.Value;
							row["qutaborder"] = tab;
							row["quminlength"] = 0;
							row["qumaxlength"] = (col.MaxLength <0 ? 0 : col.MaxLength);
							row["quwidth"] = 300;
							row["quheight"] = 23;

							row["quhidden"] = false;

							row["quhelp"] = String.Empty;
							row["qumask"] = String.Empty;
							row["quextendeddata"] = DBNull.Value;
							row["qudefault"] = col.DefaultValue;
							row["qureadonly"] = col.ReadOnly;
							row["qurequired"] = !col.AllowDBNull;
							row["qucaptionwidth"] = 150;
							row["quX"] = 0;
							row["quY"] = 23 * tab;
							row["qucustom"] = DBNull.Value;
							row["qucmdhelp"] = DBNull.Value;
							row["qucmdmethod"] = DBNull.Value;
							row["qucmdparameters"] = DBNull.Value;
							row["qucmdtype"] = DBNull.Value;
							row["qucommandretval"] = false;
							row["qucasing"] = "Normal";

							questions.Rows.Add(row);
							tab++;
						}
					}
			}
			questions.AcceptChanges();

			return _questionSource;
			
		}

		/// <summary>
		/// Returns the underlying data table of the current extended data object.
		/// </summary>
		/// <returns>A data table object.</returns>
		public DataTable GetExtendedDataTable()
		{
            // Fetch Extended Data Data only on Request
            if (_obj == null) FetchExternalData();

			if (_obj is IEnquiryCompatible)
			{
				IEnquiryCompatible enq = (IEnquiryCompatible)_obj;
				return enq.GetDataTable();
			}
			else if (_obj is DataTable)
			{
				return ((DataTable)_obj);
			}
			else
				return null;
		}

        protected void FetchExternalData()
        {
            FetchExternalData(null);
        }

		/// <summary>
		/// Fetches the external data source depending on the given type.
		/// </summary>
		protected void FetchExternalData(DataRow merge)
		{
            //Load the exclusion fields into a dom.
            try
            {
                System.Collections.Generic.List<string> locals = new System.Collections.Generic.List<string>();
                ConfigSetting config = new ConfigSetting(Convert.ToString(GetExtraInfo("extFields")));
                config.Current = "/config/localdates";
                foreach (ConfigSettingItem item in config.CurrentChildItems)
                {
                    locals.Add(item.GetString(""));
                }

                var data = base.Run(false, false, locals.ToArray());
                if (data is DataTable)
                {
                    DataTable dt = (DataTable)data;
                    if (dt.Rows.Count == 0)
                    {
                        Global.CreateBlankRecord(ref dt, true);
                    }
                    else
                    {
                    }

                    if (merge != null)
                        Global.Merge(dt.Rows[0], merge);
                }

                _obj = data;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(String.Format("ED:ERR {0}", _orgcode));
                Trace.WriteLine(ex.Message);
                Trace.WriteLine("************************************************");
            }
		}

		/// <summary>
		/// Returns the specified fields type.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public Type GetExtendedDataType(string fieldName)
		{
            // Fetch Extended Data Data only on Request
            if (_obj == null) FetchExternalData(); 
            
            if (_obj is IEnquiryCompatible)
			{
				return ((IEnquiryCompatible)_obj).GetExtraInfoType(fieldName);
			}
			else if (_obj is DataTable)
			{
				return ((DataTable)_obj).Columns[fieldName].DataType ;
			}
			else
				throw new OMSException2("26001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
		}

		/// <summary>
		/// Gets the extended information data by the field name.
		/// </summary>
		/// <param name="fieldName">Field name within internal data set.</param>
		/// <returns>An object of any data type which is held within the specified field.</returns>
		public object GetExtendedData(string fieldName)
		{
			try
			{
                // Fetch Extended Data Data only on Request
                if (_obj == null) FetchExternalData();

                object val = null;
				if (_obj is IEnquiryCompatible)
				{
					val = ((IEnquiryCompatible)_obj).GetExtraInfo(fieldName);
				}
				else if (_obj is DataTable)
				{
					val = ((DataTable)_obj).Rows[0][fieldName];
				}
				else
					val =  null;

                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;

			}
			catch (Exception ex)
			{
				string errmsg = @"Error Getting Field '%1%' in Extended Data '%2%'

Extended Data Table : %3%
-------------------
%4%";
				throw new OMSException2("EXTUPERR6",errmsg,ex,true,fieldName ,this.Code,this.Call,FWBS.Common.OMSDebug.DataTableToString((DataTable)_obj));

			}
		}

		/// <summary>
		/// Sets the extended information data by the field name.
		/// </summary>
		/// <param name="fieldName">Field name within internal data set.</param>
		/// <param name="val">Value to set the current database.</param>
		public void SetExtendedData(string fieldName, object val)
		{
            // Fetch Extended Data Data only on Request
            if (_obj == null) FetchExternalData();
            
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			if (_obj is IEnquiryCompatible)
			{
				((IEnquiryCompatible)_obj).SetExtraInfo(fieldName, val);
			}
			else if (_obj is DataTable)
			{
				DataTable dt = (DataTable)_obj;
				try
				{
					dt.Rows[0][fieldName] = val;
				}
				catch(Exception ex)
				{
					// 22/3/04 - DT Changed instead of just ignoring bad data it will now return an error
					string errmsg = @"Error setting Field '%1%' with value of '%2%' in Extended Data %3%'

Column Details
--------------
     Column Type : %4%
     Column Size : %5%";
					throw new OMSException2("EXTUPERR7",errmsg,ex,true,fieldName,Convert.ToString(val),this.Code,dt.Columns[fieldName].DataType.ToString(),dt.Columns[fieldName].MaxLength.ToString());
				}
			}
		}

		/// <summary>
		/// Updates the extended data by persisting it to the external databases.
		/// </summary>
		public void UpdateExtendedData(long id = 0)
		{
            // Fetch Extended Data Data only on Request
            if (_obj == null)
                FetchExternalData();

            var extDestLink = id == 0
                    ? _extra.GetExtraInfo(GetExtraInfo("extDestLink").ToString())
                    : id;

            if (_obj is IEnquiryCompatible)
			{
				IEnquiryCompatible enq = (IEnquiryCompatible)_obj;
                enq.SetExtraInfo(GetExtraInfo("extSourceLink").ToString(), extDestLink);
				enq.Update();
			}
			else if (_obj is DataTable)
			{
				
				DataTable dt = (DataTable)_obj;

				if (dt.Rows[0].RowState == DataRowState.Deleted)
				{
					if ((Modes | ExtendedDataMode.Delete) != Modes)
						return;
				}
				else if (dt.Rows[0].RowState == DataRowState.Added)
				{
					if ((Modes | ExtendedDataMode.Add) != Modes)
						return;
				}
				else
				{
					if ((Modes | ExtendedDataMode.Edit) != Modes)
						return;
				}

				if (Convert.ToString(GetExtraInfo("extSourceLink")) == "")
					throw new OMSException2("EXTUPERR3","Error Source Link Field not Set for Extended Data '%1%'",new Exception(),true,this.Code);
				if (Convert.ToString(GetExtraInfo("extDestLink")) == "")
					throw new OMSException2("EXTUPERR4","Error Destination Link Field not Set for Extended Data '%1%'",new Exception(),true,this.Code);
				try
				{
					SetExtendedData(GetExtraInfo("extSourceLink").ToString(), extDestLink);
				}
				catch (Exception ex)
				{
					string errmsg = @"Error setting Link Field '%1%' with Value of '%2%' in Extended Data '%3%'

Extended Data Table : %4%
-------------------
%5%";
					throw new OMSException2("EXTUPERR5",errmsg,ex,true,GetExtraInfo("extDestLink").ToString(),Convert.ToString(_extra.GetExtraInfo(GetExtraInfo("extDestLink").ToString())),this.Code, this.Call, FWBS.Common.OMSDebug.DataTableToString((DataTable)_obj));
				}
				if (base.Connector is Connection)
				{
					//Set the primary key of the underlying table if not already done so for conccurency and merging issues.
					//MNW Test DM Check
					try
					{
						if (dt.PrimaryKey == null || dt.PrimaryKey.Length == 0)
							dt.PrimaryKey = new DataColumn[1]{dt.Columns[GetExtraInfo("extSourceLink").ToString()]};

                        bool refresh = false;

                        if (dt.Rows[0].RowState == DataRowState.Added)
                        {
                            refresh = true;
                        }
                        
                        ((Connection)base.Connector).Update(dt, GetExtraInfo("extCall").ToString());
                             
                        //CM 17.10.2013 [WI.2795] - Fetch data on first insert of row, the newly created ID will be populated in the Enquiry Data Table (for example)
                        if (refresh)
                            FetchExternalData();
					}
					catch (Exception ex)
					{
						string errmsg = @"Error updating Extended Data '%1%'.

Source Link Name '%3%' and Destination Link Name '%4%' the value is '%5%'

Extended Data Table : %6%
-------------------
%7%";
						throw new OMSException2("EXTUPERR2", errmsg, "",ex,true,this.Code,ex.Message,Convert.ToString(GetExtraInfo("extSourceLink")),Convert.ToString(GetExtraInfo("extDestLink")),Convert.ToString(_extra.GetExtraInfo(Convert.ToString(GetExtraInfo("extDestLink")))),this.Call,FWBS.Common.OMSDebug.DataTableToString((DataTable)_obj));
					}
				}
				else
					throw new ExtendedDataException(HelpIndexes.ExtendedDataInvalidConnection);
			}
		}

		/// <summary>
		/// Refreshes the extended data.
		/// </summary>
        public void RefreshExtendedData()
        {
            RefreshExtendedData(false);
        }

		public void RefreshExtendedData(bool applyChanges)
		{
            // Fetch Extended Data Data only on Request
             
            
            if (_obj is IEnquiryCompatible)
			{
				IEnquiryCompatible enq = (IEnquiryCompatible)_obj;
				enq.Refresh(applyChanges);
			}
			else if (_obj is DataTable)
			{
                DataTable dt = (DataTable)_obj;
                DataTable changes = dt.GetChanges();

                if (changes != null && applyChanges && changes.Rows.Count > 0)
                    FetchExternalData(changes.Rows[0]);
                else
                    FetchExternalData(null);


			}
            else if (_obj == null)
            {
                FetchExternalData();
            }
		}

        public void CancelExtendedData()
        {
            if (_obj is IEnquiryCompatible)
            {
                IEnquiryCompatible enq = (IEnquiryCompatible)_obj;
                enq.Cancel();
            }
            else
            {
                DataTable dt = _obj as DataTable;

                if (dt == null)
                    return;

                dt.RejectChanges();
            }
            _obj = null;

        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets the IEnquiryCompatible object in the result of the extended data link. 
		/// </summary>
		[Browsable(false)]
		public IEnquiryCompatible Object
		{
			get
			{
                // Fetch Extended Data Data only on Request
                if (_obj == null) FetchExternalData(); 
                
                return _obj as IEnquiryCompatible;
			}
		}

		/// <summary>
		/// Gets the unique extended data code from the underlying data source.
		/// </summary>
		[LocCategory("(Details)")]
		public string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("extCode"));
			}
			set
			{
				if (ExtendedData.Exists(value))
				{
					if (value != _orgcode)
						throw new ExtendedDataException(HelpIndexes.ExtendedDataCodeAlreadyExists,value);
				}
				else
				{
					if (_extended.Rows[0].RowState == DataRowState.Added)
					{
						_extended.Rows[0]["extCode"] = value;
					}
					else
						throw new OMSException2("26002","The Code cannot be changed when set");
				}
			}
		}

		/// <summary>
		/// Gets the editing modes of the extended data item.
		/// </summary>
		[Description("Extended Data Modes"), LocCategory("Data")]
		public virtual ExtendedDataMode Modes
		{
			get
			{
				return (ExtendedDataMode)Enum.ToObject(typeof(ExtendedDataMode), GetExtraInfo("extModes")); 
			}
			set
			{
				SetExtraInfo("extmodes", value);
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
			Session.CurrentSession.CheckCriticalDataAccess(System.Reflection.Assembly.GetCallingAssembly(), fieldName);

            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_extended.Rows[0][fieldName] = val;
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		public object GetExtraInfo(string fieldName)
		{
			object val = _extended.Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		/// <summary>
		/// Returns the specified fields type.
		/// </summary>
		/// <param name="fieldName">Field Name.</param>
		/// <returns>Type of Field.</returns>
		public Type GetExtraInfoType(string fieldName)
		{
			try
			{
				return _extended.Columns[fieldName].DataType;
			}
			catch 
			{
				throw new OMSException2("26001","Error Getting Extra Info Field %1% Probably Not Initialized",new Exception(),true,fieldName);
			}
		}


		/// <summary>
		/// Returns a dataset representation of the object.
		/// </summary>
		/// <returns>Dataset object.</returns>
		public DataSet GetDataset()
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
		public DataTable GetDataTable()
		{
			Session.CurrentSession.CheckCriticalDataAccess(System.Reflection.Assembly.GetCallingAssembly(), "?");
			return _extended.Copy();
		}

		#endregion

		#region IUpdateable Implementation

		/// <summary>
		/// Gets a value indicating whether the object is new and needs to be 
		/// updated to exist in the database.
		/// </summary>
		[Browsable(false)]
		public bool IsNew
		{
			get
			{
				try
				{
					return (_extended.Rows[0].RowState == DataRowState.Added);
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
			if (_extended.GetChanges()!= null)
			{
				Session.CurrentSession.Connection.Update(_extended, Sql + " where extcode = '" + Common.SQLRoutines.RemoveRubbish(Code) + "'");
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


            DataTable changes = _extended.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(this.Code, changes.Rows[0]);
            else
                Fetch(this.Code, null);

        }

		/// <summary>
		/// Cancels any changes made to the object.
		/// </summary>
		public void Cancel()
		{
			_extended.RejectChanges();
		}

		/// <summary>
		/// Gets a boolean flag indicating whether any changes have been made to the object.
		/// </summary>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return (_extended.GetChanges() != null);
			}
		}

		#endregion

		#region Source Implementation

		/// <summary>
		/// An abstract method which must be implemented so that each parameter in the paramater
		/// list has its value populated.
		/// </summary>
		/// <param name="name">The name of the parameter being set.</param>
		/// <param name="value">The value to be returned to the parameter list.</param>
		protected override void SetParameter (string name, out object value)
		{
			try
			{
				value = _extra.GetExtraInfo(name);
			}
			catch
			{
				base.SetParameter(name, out value);
			}
		}

		#endregion

		#region Static

		/// <summary>
		/// Delete a Search List
		/// </summary>
		/// <param name="Code">Search List Code</param>
		/// <returns>True if Succesful</returns>
		public static bool Delete(string Code)
		{
			try
			{
				IDataParameter[] ip1 = new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)};
				Session.CurrentSession.Connection.ExecuteSQL("delete from dbExtendedData where extCode = @Code", ip1);
				IDataParameter[] ip2 = new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)};
				Session.CurrentSession.Connection.ExecuteSQL("delete from DBCodeLookup where cdcode = @Code and cdtype = 'EXTENDEDDATA';", ip2);
			}
			catch (Exception ex)
			{
				IDataParameter[] ip = new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)};
				DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT dbEnquiry.enqCode AS [Enquiry Form], dbEnquiryQuestion.quExtendedData AS [Extended Data], dbEnquiryQuestion.quFieldName AS [Field Name] FROM dbEnquiryQuestion INNER JOIN dbEnquiry ON dbEnquiryQuestion.enqID = dbEnquiry.enqID WHERE dbEnquiryQuestion.quExtendedData = @Code","ERRQUESTION",false,ip);
				throw new OMSException2("26003","Cannot Delete Extended Data '%1%' because it has links in Enquiry Form Questions. #13#10#13#10Please locate and remove questions before attempting to delete again. #13#10#13#10%2%" ,"",ex,true,Code,FWBS.Common.OMSDebug.DataTableToString(dt));
			}
			return true;
		}
		
		/// <summary>
		/// Gets a list of valid extended data objects.
		/// </summary>
		/// <returns>A data datble of extended data items.</returns>
		public static DataTable GetExtendedDatas()
		{
			Session.CurrentSession.CheckLoggedIn();
			
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprExtendedDataList", "EXTENDEDDATALIST",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name)});
			dt.DefaultView.Sort = dt.Columns[1].ColumnName;
			return dt;
		}

		/// <summary>
		/// Does the Code Exist
		/// </summary>
		/// <returns>Boolean</returns>
		public static bool Exists(string Code)
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbExtendedData where extcode = @Code", "EXISTS",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
			if (dt.Rows.Count > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Gets a list of valid extended data objects.
		/// </summary>
		/// <returns>A data datble of extended data items.</returns>
		public static DataTable GetExtendedDatas(string type)
		{

			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT ObjWinNamespace as extcode, dbo.GetCodeLookupDesc('EXTENDEDDATA',ObjWinNamespace, @UI) AS cddesc FROM dbOMSObjects WHERE ObjTypeCompatible = @Type AND objType = 'ExtData'", "EXTENDEDDATALIST",  new IDataParameter[2] {Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name),Session.CurrentSession.Connection.AddParameter("Type", SqlDbType.NVarChar, 50, type)});
			dt.DefaultView.Sort = dt.Columns[1].ColumnName;
			return dt;
		}

		/// <summary>
		/// Gets the rendering schema questions need for the base rendering form.
		/// </summary>
		/// <returns>A data table object.</returns>
		internal static DataTable GetRenderingQuestions()
		{
			Session.CurrentSession.CheckLoggedIn();
			DataTable dt = Session.CurrentSession.Connection.ExecuteProcedureTable("sprExtendedDataBuilder", Table_Question,  new IDataParameter[1]{Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentUICulture.Name)});
			return dt;
		}

		/// <summary>
		/// Gets the a ArrayList of Return Fields stored in the XML Field extFields
		/// </summary>
		/// <returns>A Arraylist of ReturnFields Object.</returns>
		public static ArrayList GetReturnFields(string Code, bool RawFormat)
		{
			if (Code != _returnfieldscode)
			{
				Hashtable _exclusionsfields = new Hashtable();
			
				XmlDocument _xmlDReturnFields;
				XmlNode _xmlCReturnFields = null;
				XmlNode _xmlReturnField;

				XmlNode _xmlExclusionsField;
			
				Session.CurrentSession.CheckLoggedIn();
				DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable("select extFields from dbExtendedData where extCode = @Code", "FIELDS",  new IDataParameter[1] {Session.CurrentSession.Connection.AddParameter("Code", SqlDbType.NVarChar, 15, Code)});
				if (dt.Rows.Count == 0)
				{
					return null;
				}
				//
				// XML Return Fields
				//
				_xmlDReturnFields = new XmlDocument();
				_xmlDReturnFields.PreserveWhitespace = true;
				try
				{
					_xmlDReturnFields.LoadXml(Convert.ToString(dt.Rows[0]["extFields"]));
					_xmlCReturnFields = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config");
				}
				catch{}
				if (_xmlCReturnFields == null)
				{
					_xmlCReturnFields = _xmlDReturnFields.CreateElement("","config","");
					_xmlDReturnFields.AppendChild(_xmlCReturnFields);
				}


				_xmlReturnField = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config/fields");
				if (_xmlReturnField == null)
				{
					_xmlReturnField = _xmlDReturnFields.CreateElement("","fields","");
					_xmlCReturnFields.AppendChild(_xmlReturnField);
				}

				_xmlExclusionsField = _xmlDReturnFields.DocumentElement.SelectSingleNode("/config/exclusions");
				if (_xmlExclusionsField == null)
				{
					_xmlExclusionsField = _xmlDReturnFields.CreateElement("","exclusions","");
					_xmlCReturnFields.AppendChild(_xmlExclusionsField);
				}
		
				_exclusionsfields.Clear();
				foreach(XmlNode dr in _xmlExclusionsField.ChildNodes)
				{
					string __mappingname = "";
					try{__mappingname = dr.InnerText;}
					catch{}
					_exclusionsfields.Add(__mappingname,__mappingname);
				}	
			
				_returnfields.Clear();
				foreach(XmlNode dr in _xmlReturnField.ChildNodes)
				{
					string __mappingname = "";
					try{__mappingname = dr.InnerText;}
					catch{}
					if (_exclusionsfields[__mappingname] == null)
					{
						if (RawFormat)
							_returnfields.Add(__mappingname);
						else
							_returnfields.Add(new ExtendedDataFields(__mappingname));
					}
				}			
				_returnfieldscode = Code;
			}
			return _returnfields;
		}
		
		#endregion

	}

	public class ExtendedDataFields
	{
		private string _field;

		public ExtendedDataFields(string Field)
		{
			_field = Field;
		}

		public string Field
		{
			get
			{
				return _field;
			}
		}
	}

	/// <summary>
	/// A class that exposes a list of extended data objects with an indexer.
	/// </summary>
	public sealed class ExtendedDataList : System.Collections.ReadOnlyCollectionBase, IDisposable
	{

		#region Fields

		/// <summary>
		/// Internal data source.
		/// </summary>
		private DataTable _extended = null;

			/// <summary>
		/// SQL statement for updating objects to the database as well as retrieving an object by any criteria.
		/// </summary>
		internal const string Sql = "select * from dbExtendedData";

		/// <summary>
		/// Table name used internally for this object.  This is used by the update command, so it knows what table to update
		/// incase a dataset with more than one table is used.
		/// </summary>
		public const string Table = "EXTENDEDDATALIST";


		
		#endregion

		#region Constructors


		/// <summary>
		/// Prevent any default construction of this object.
		/// </summary>
		internal ExtendedDataList(){}

		/// <summary>
		/// Fetch a list of extended data records to manipulate.
		/// </summary>
		/// <param name="obj">A reference to the object calling this constructor.</param>
		/// <param name="codes">A list of codes to fetch from the databse.</param>
		internal ExtendedDataList(IExtraInfo obj, params string [] codes)
		{
			string sqlcodes = string.Join("','", codes);
			_extended = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where extCode in ('" + sqlcodes + "')", Table, new IDataParameter[0]);

			foreach (DataRow row in _extended.Rows)
			{
                // ***********************************************************
                // 12th December 2007 added by DCT
                // added Try catch to the Fetching of Extended Data now will
                // surpress any bad Extrend Data and so no exception will be 
                // throwen allowing the creation of the ExtendData Collection 
                // Variable in the Object
                // ***********************************************************
                try
                {
                    ExtendedData ext = new ExtendedData(obj, _extended, row);
                    this.InnerList.Add(ext);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(String.Format("ED:ERR {0}",row[0]));
                    Trace.WriteLine(ex.Message);
                    Trace.WriteLine("************************************************");
                }
			}
		}

		
		#endregion

		#region Indexers

		/// <summary>
		/// Gets a specific extended data object by its index value.
		/// </summary>
		public ExtendedData this [int index]
		{
			get
			{
				return (ExtendedData) this.InnerList[index];
			}
		}

		/// <summary>
		/// Gets a specific extended data object by its code value.
		/// </summary>
		public ExtendedData this [string code]
		{
			get
			{
				foreach (ExtendedData ext in this.InnerList)
				{
					if (ext.Code.ToUpper() == code.ToUpper())
						return ext;
				}
				throw new ExtendedDataException(HelpIndexes.ExtendedDataDoesNotExist, code);
			}
		}

		#endregion

		#region Properties



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
		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{	
				if (_extended != null)
				{
					_extended.Dispose();
					_extended = null;
				}
			}

			//Dispose unmanaged objects.
		}


		#endregion


	}


	/// <summary>
	/// A delegate that the UI layer can use to be called back with a data field type,
	/// so that the it can return the control type needed for the field type to be displayed.
	/// </summary>
	public delegate Type FieldDisplayType (DataColumn column);


}
