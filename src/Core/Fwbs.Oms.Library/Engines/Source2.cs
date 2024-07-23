using System;
using System.Data;
using System.Reflection;
using FWBS.Common;
using FWBS.OMS.Data;


namespace FWBS.OMS.SourceEngine
{

    /// <summary>
    /// A delegate used to call the Set parameter method for those class / objects that
    /// do not inherit from Source but may use RunSource instead.
    /// </summary>
    public delegate void SetParameterHandler(string name, out object value);

    /// <summary>
    /// 24000 This is an object that decides how to connect to a datasource be it the current OMS
    /// databse connection, external database connection, or event static or object instances of
    /// .NET objects. Parameters will also be able to be passed to whatever object as both database
    /// calls and .NET method / constructor calls both have parameter logic beteen them.
    /// </summary>	public class Source2
    public class Source : LookupTypeDescriptor, FWBS.OMS.Data.IDatabaseSchema
    {
        #region Fields
        /// <summary>
        /// The Object Type required by the Parent Property
        /// </summary>
        private string _typerequired = "";
        /// <summary>
        /// The type of connected source.
        /// </summary>
        private SourceType _type = SourceType.OMS;

        /// <summary>
        /// The source string that is used.
        /// This could be an assembly name (.NET) or a database connection string (DB).
        /// </summary>
        private string _source = "";
        /// <summary>
        /// The call to be made against the source object.
        /// This could be a method name (.NET) or a select statement / store procedure (DB).
        /// </summary>
        private string _call = "";
        /// <summary>
        /// XML representation of the parameters to be passed to the call.
        /// </summary>
        private string _parameters = "";

        /// <summary>
        /// An object that may be a Type, object Instance or a database Connection.
        /// </summary>
        private object _connector = null;

        /// <summary>
        /// The field parser used to replace information within the parameters with
        /// current state data.
        /// </summary>
        private FieldParser _paramParser = null;

        /// <summary>
        /// Set parameter delegate method handler for replacing parameters.
        /// </summary>
        private SetParameterHandler _setParameter = null;

        /// <summary>
        /// Holds a collection of key name pairs.
        /// </summary>
        private FWBS.Common.KeyValueCollection _replacementParameters = null;

        /// <summary>
        /// List of Local Date Parameters
        /// </summary>
        private System.Collections.Generic.List<string> localDateParameters = new System.Collections.Generic.List<string>();

        /// <summary>
        /// A parent object that may get manipulated for data.
        /// </summary>
        private object _parent = null;


        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected Source()
        {
            ParameterHandler = null;
        }


        /// <summary>
        /// Creates an instance of the source object passing the necessary data.
        /// </summary>
        /// <param name="sourceType">Source2 type (where to get the data from).</param>
        /// <param name="source">Assembly type object.</param>
        /// <param name="call">Method / SQL statement to call.</param>
        /// <param name="parameters">Parameter definitions in string format.</param>
        public Source(SourceType sourceType, string source, string call, string parameters)
        {
            _type = sourceType;
            _source = source;
            _call = call;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Creates an instance of the source object passing the necessary data but accepting a string source type.
        /// </summary>
        /// <param name="sourceType">String representation of the source type.</param>
        /// <param name="source">Assembly type object.</param>
        /// <param name="call">Method / SQL statement to call.</param>
        /// <param name="parameters">Parameter definitions in string format.</param>
        public Source(string sourceType, string source, string call, string parameters)
            : this((SourceType)Enum.Parse(typeof(SourceType), sourceType, true), source, call, parameters)
        {
        }

        public Source(object objInstance, string method, string parameters)
        {
            _type = SourceType.Instance;
            _source = "";
            _connector = objInstance;
            _call = method;
            this.Parameters = parameters;
        }



        #endregion

        #region Methods

        /// <summary>
        /// Resets the internal source connector
        /// </summary>
        public void ReBind()
        {
            SetInternalSourceConnector();
        }

        /// <summary>
        /// Sets the key value paired parameters which get looked at when setting the parameters.
        /// </summary>
        /// <param name="parameters">The parameters to be used.</param>
        public void ChangeParameters(FWBS.Common.KeyValueCollection parameters)
        {
            _replacementParameters = parameters;
        }

        /// <summary>
        /// Changes the parent object.
        /// </summary>
        /// <param name="parent">The new parent.</param>
        public void ChangeParent(object parent)
        {
            _parent = parent;

            //For object instance sources only...
            //This will enable the search list call an instance method of the object 
            //that is passed.
            if (this.SourceType == SourceType.Instance) this.Connector = parent;
            ParameterParser.ChangeObject(_parent);
        }

        /// <summary>
        /// Sets the internal connector object to the source connector type (Connection / or Object Type).
        /// </summary>
        private void SetInternalSourceConnector()
        {
            Session.CurrentSession.CheckLoggedIn();

            switch (_type)
            {
                case SourceType.OMS:		//Uses the connection to the OMS.NET database.
                    _connector = Session.CurrentSession.Connection;
                    return;
                case SourceType.Dynamic:	//Uses an OLEDB connection string to connect to another database.
                    _connector = FWBS.OMS.Data.Connection.GetConnection(_source);
                    return;
                case SourceType.Class:		//Assembly class reference.
                    if (_source == "")
                    {
                        _connector = null;
                        throw new SourceException(HelpIndexes.SourceNoSourceSet);
                    }
                    _connector = Session.CurrentSession.TypeManager.Load(_source);
                    return;
                case SourceType.Linked:		//Uses the existing OMS.NET connection to call off a linked server.
                    _connector = Session.CurrentSession.Connection;
                    return;
                case SourceType.Object:		//Assembly class reference.
                    if (_source == "")
                    {
                        _connector = null;
                        throw new SourceException(HelpIndexes.SourceNoSourceSet);
                    }
                    _connector = Session.CurrentSession.TypeManager.Load(_source);
                    return;
                case SourceType.Instance:	//The object instance must already be passed to the source object.
                    if (_connector == null)
                    {
                        throw new SourceException(HelpIndexes.SourceNoSourceSet);
                    }
                    return;
            }
        }


        /// <summary>
        /// Runs the source connector with all the specified parameters.
        /// </summary>
        /// <returns>An object return value of any kind.</returns>
        public object Run()
        {
            return Run(false, false);
        }

        /// <summary>
        /// Runs the source command with all the specified parameters.
        /// </summary>
        /// <param name="schemaOnly">If true, only returns the schema of the data table returned.</param>
        /// <returns>An object return value of any kind.</returns>
        public virtual object Run(bool schemaOnly)
        {
            return Run(schemaOnly, false);
        }

        public object Run(bool schemaOnly, bool returnDataSet)
        {
            return Run(schemaOnly, returnDataSet, new string[0] { });
        }

        /// <summary>
        /// Runs the source command with all the specified parameters.
        /// </summary>
        /// <param name="schemaOnly">If true, only returns the schema of the data table returned.</param>
        /// <param name="returnDataSet">Returns a dataset rather than a datatable when running queries of a database connection source type.</param>
        /// <param name="LocalDateColumns"></param>
        /// <returns>An object return value of any kind.</returns>
        public object Run(bool schemaOnly, bool returnDataSet, string[] LocalDateColumns)
        {
            //The return value of the connector call.
            object retVal = null;

            FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(_parameters);
            cfg.Current = "params";

            //Default the parameters Xml.
            if (_parameters == null || _parameters == String.Empty) this.Parameters = "<params></params>";


            if (this.ParentTypeRequired != "" && this.Parent == null)
            {
                throw new OMSException2("24004", "Parent not set. Parent should be %1%", new Exception(), false, _typerequired);
            }
            else if (this.ParentTypeRequired != "" && this.Parent != null && this.Parent.GetType().FullName != _typerequired)
                throw new OMSException2("24003", "Invalid Parent object. Parent should be %1%", new Exception(), false, _typerequired);


            //Compare the source type code and fetch the data from multiple of different sources.
            switch (_type)
            {
                //Default to a database connection.  This would be used more frequently.
                default:
                    {
                        using (Connection cnn = (Connection)((Connection)_connector).Clone())
                        {
                            retVal = FetchData(schemaOnly, returnDataSet, LocalDateColumns, cfg, cnn);
                        }
                    }
                    break;
                case SourceType.OMS:	//Uses the connection to the OMS.NET database to access tables within the same database.
                    if (_connector == null) _connector = Session.CurrentSession.Connection;
                    goto default;
                case SourceType.Dynamic:	//Uses an OLEDB connection string to connect to another database.
                    if (_connector == null) _connector = FWBS.OMS.Data.Connection.GetConnection(_source);
                    goto default;
                case SourceType.Linked:	//Uses the existing OMS.NET connection to call off a linked server.
                    if (_connector == null) _connector = Session.CurrentSession.Connection;
                    goto default;
                case SourceType.Class:	//Uses an object and runs a static method off it.
                    {
                        //Grabs the type information of the given object type.
                        Type objtype = null;
                        if (_connector == null)
                        {
                            objtype = Session.CurrentSession.TypeManager.Load(_source);
                            _connector = objtype;
                        }
                        else
                        {
                            objtype = (Type)_connector;
                        }

                        Type[] args;
                        object[] vals;
                        BuildDotNetParameters(cfg, out args, out vals);

                        //Find the method based on the passed types and values and invoke it.
                        MethodInfo mth = objtype.GetMethod(_call, BindingFlags.Public | BindingFlags.Static, null, args, null);
                        if (mth == null)
                        {
                            string propsandvals = "";
                            for (int i = 0; i < vals.Length; i++)
                                propsandvals += "[" + args[i].Name + "] " + Convert.ToString(vals[i]) + ", ";
                            propsandvals = propsandvals.TrimEnd(", ".ToCharArray());
                            throw new SourceException(new Exception("Static Source Invoke Error ..."), HelpIndexes.SourceMethodInvokeError, _call, objtype.FullName, propsandvals);
                        }

                        retVal = mth.Invoke(null, vals);

                    }
                    break;
                case SourceType.Object:	//Creates an object by calling its contructor with the specified parameters.
                    {
                        //Grabs the type information of the given object type.
                        Type objtype = null;
                        if (_connector == null)
                        {
                            objtype = Session.CurrentSession.TypeManager.Load(_source);
                            _connector = objtype;
                        }
                        else
                        {
                            objtype = (Type)_connector;
                        }

                        Type[] args;
                        object[] vals;
                        BuildDotNetParameters(cfg, out args, out vals);

                        //Get the constructor information that matches the contrcutors parameter types.		
                        ConstructorInfo construct = null;
                        //Find the default constructor if the number of parameters used is zero.
                        if (args.GetLength(0) == 0)
                        {
                            construct = objtype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                        }
                        else
                            construct = objtype.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);


                        //Throw an exception if the constructor with the specified parameter types cannot
                        //be found.
                        if (construct == null)
                        {
                            throw new SourceException(HelpIndexes.SourceConstructorInvokeError, _source, _parameters);
                        }
                        retVal = construct.Invoke(vals);
                    }
                    break;
                case SourceType.Instance:
                    {
                        MemberInfo member = null;

                        //Grabs the type information of the given object.
                        if (_connector == null) throw new OMSException2("24001", "The Instance of the Object has not been assigned...");
                        Type objtype = _connector.GetType();

                        Type[] args;
                        object[] vals;
                        BuildDotNetParameters(cfg, out args, out vals);

                        //First try and get a method type base on the method name given.
                        member = objtype.GetMethod(_call, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, args, null);

                        //If the method does not exist, see if there is a property with the specified name.
                        if (member == null)
                            member = objtype.GetProperty(_call, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        if (member == null)
                            throw new SourceException(HelpIndexes.SourceMethodInvokeError, _call, objtype.FullName);

                        //Depending if the method is a method or property object then run their
                        //equivalent invoke methods.
                        if (member is PropertyInfo)
                        {
                            if (vals.Length == 0)
                                retVal = ((PropertyInfo)member).GetValue(_connector, null);
                            else
                                ((PropertyInfo)member).SetValue(_connector, vals[0], null);
                        }
                        else if (member is MethodInfo)
                            retVal = ((MethodInfo)member).Invoke(_connector, vals);
                        else
                            throw new SourceException(HelpIndexes.SourceMethodInvokeError, _call, objtype.FullName);

                    }
                    break;
            }

            return retVal;
        }

        private object FetchData(bool schemaOnly, bool returnDataSet, string[] LocalDateColumns, ConfigSetting cfg, Connection cnn)
        {
            object retVal;
            IDataParameter[] paramlist = new IDataParameter[cfg.CurrentChildItems.Length];

            //Loop through each of the parameters,
            int ctr = 0;
            foreach (FWBS.Common.ConfigSettingItem itm in cfg.CurrentChildItems)
            {
                //Get the SqlDataType used (NVarChar).  This will be parsed into the enumeration.
                string datatype = itm.GetString("type", "");
                //Get the name of the parameter (in SQL this is the @parma_name = "param_name").
                string name = itm.GetString("name", "");

                //Gets the value to be passed to the parameter list.
                string par = itm.GetString("");
                string sourceIs = itm.GetString("kind", "");
                if (sourceIs.ToLower().Contains("local"))
                    localDateParameters.Add(name.TrimStart('@'));
                object val = null;

                if (par.StartsWith("%") && par.EndsWith("%"))
                {
                    try
                    {
                        SetParameter(par.Replace("%", ""), out val);
                    }
                    catch
                    {
                    }
                }
                else
                    val = par;

                // UTCFIX: If incoming param is not a DATETIME type then create if possible as a new DateTime Object Kind=Local else error and leave as string
                if (datatype == "DateTime" && val != null)
                {
                    if ((val is DateTime) == false)
                    {
                        try
                        {
                            DateTime newval = DateTime.SpecifyKind(Convert.ToDateTime(val), DateTimeKind.Local);
                            val = newval;
                        }
                        catch { }
                    }
                }

                if (val is DateTime)
                {
                    val = TransformDateParameter(sourceIs, (DateTime)val);
                }

                //Add the parameter to the parameters collection.
                try
                {
                    paramlist[ctr] = cnn.AddParameter(name, (SqlDbType)FWBS.Common.ConvertDef.ToEnum(datatype, SqlDbType.NVarChar), 0, val);
                }
                catch (Exception ex)
                {
                    throw new OMSException2("24002", "Error Setting Parameters : %1%", ex, true, name);
                }
                ctr++;
            }


            //Check to see if the calling SQL statement is a SELECT statement or a stored procedure.


            if (_call.Length == 0)
                throw new Exception("You must complete the data builder");

            ConnectionExecuteParameters pars = new ConnectionExecuteParameters();

            pars.Parameters = paramlist;
            pars.ShemaOnly = schemaOnly;
            pars.TableNames = new string[1] { "SOURCE" };
            pars.Sql = _call;
            pars.LocalDateColumns = LocalDateColumns;
            pars.LocalDateParameters = localDateParameters.ToArray();

            if (returnDataSet)
            {
                if (_call.ToUpper().StartsWith("SELECT"))
                {
                    pars.CommandType = CommandType.Text;
                    retVal = cnn.ExecuteSQLDataSet(pars);
                }
                else
                {
                    pars.CommandType = CommandType.StoredProcedure;
                    retVal = cnn.ExecuteSQLDataSet(pars);
                }
            }
            else
            {
                if (_call.ToUpper().StartsWith("SELECT"))
                {
                    pars.CommandType = CommandType.Text;
                    retVal = cnn.ExecuteSQLTable(pars);
                }
                else
                {
                    pars.CommandType = CommandType.StoredProcedure;
                    retVal = cnn.ExecuteSQLTable(pars);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Test Method - DO NOT USE
        /// </summary>
        /// <param name="sourceIs">The date parameter type</param>
        /// <param name="val">The date to transform</param>
        /// <returns>Val transformed to the sourceIs type</returns>
        [Obsolete("Test Method - DO NOT USE")]
        public static DateTime TransformDateParameter(FWBS.OMS.SearchEngine.SearchParameterDateIs sourceIs, DateTime val)
        {
            return TransformDateParameter(Convert.ToString(sourceIs), val);
        }

        internal static DateTime TransformDateParameter(string sourceIs, DateTime val)
        {
            DateTime date = (DateTime)val;
            switch (sourceIs.ToLower())
            {
                case "localstartofday":
                    date = DateTime.SpecifyKind(date.ToLocalTime().Date, DateTimeKind.Local);
                    break;
                case "utcstartofday":
                    date = date.ToLocalTime().Date;
                    date = date.ToUniversalTime();
                    break;
                case "localendofday":
                    date = DateTime.SpecifyKind(date.ToLocalTime().Date, DateTimeKind.Local);
                    date = date.ToLocalTime();
                    date = date.AddDays(1);
                    break;
                case "utcendofday":
                    date = date.ToLocalTime().Date;
                    date = date.AddDays(1);
                    date = date.ToUniversalTime();
                    break;
                case "v2utcendofday":
                    date = date.ToLocalTime().Date;
                    date = date.AddDays(1);
                    date = date.AddMilliseconds(-3);
                    date = date.ToUniversalTime();
                    break;
                case "v2localendofday":
                    date = DateTime.SpecifyKind(date.ToLocalTime().Date, DateTimeKind.Local);
                    date = date.AddDays(1);
                    date = date.AddMilliseconds(-3);
                    break;
                default:
                    break;
            }

            return date;
        }


        /// <summary>
        /// Builds the parameters of a .NET object.
        /// </summary>
        /// <param name="cfg">The parameter configuration document.</param>
        /// <param name="args">The types of arguments for the method call.</param>
        /// <param name="vals">The actual values of the method call.</param>
        private void BuildDotNetParameters(FWBS.Common.ConfigSetting cfg, out Type[] args, out object[] vals)
        {
            //Count the number of parameters to be used.
            int cnt = cfg.CurrentChildItems.Length;

            //Dimensionize the arrays.
            args = new Type[cnt];
            vals = new object[cnt];

            //Loop through each of the parameters,
            //for (int ctr = 0; ctr < cnt; ctr++)
            int ctr = 0;
            foreach (FWBS.Common.ConfigSettingItem itm in cfg.CurrentChildItems)
            {
                //Get the .NET data type used (string).
                //Type datatype = Global.GetType(cfg.DocObject.SelectNodes("/params/param")[ctr].Attributes["type"].InnerText);
                Type datatype = Session.CurrentSession.TypeManager.Load(itm.GetString("type", ""));
                //if the data type cannot be found then default it to a string.
                if (datatype == null)
                    datatype = typeof(string);

                args[ctr] = datatype;


                //Gets the value to be passed to the parameter list.
                string par = itm.GetString("");
                object val = null;

                //Check the base types of the parameter and see if there are any
                //enumerations or arrays used.
                //Add the parameter to the parameters collection.
                //Convert the passed filter link value to the specified type above.
                if (args[ctr].BaseType == typeof(System.Array))
                {
                    //INFO: Semi colon delimited array items.  This only uses constants at the moment.
                    System.Array arr = par.Split(';');
                    vals[ctr] = Convert.ChangeType(arr, args[ctr]);

                    for (int parctr = 0; parctr < arr.GetLength(0); parctr++)
                    {
                        object parval = null;
                        string par2 = Convert.ToString(arr.GetValue(parctr));
                        if (par2.StartsWith("%") && par2.EndsWith("%"))
                        {
                            SetParameter(par2.Replace("%", ""), out parval);
                        }
                        else
                            parval = par2;
                        arr.SetValue(Convert.ChangeType(parval, arr.GetValue(parctr).GetType()), parctr);
                    }
                }
                else
                {
                    if (par.StartsWith("%") && par.EndsWith("%"))
                    {
                        SetParameter(par.Replace("%", ""), out val);
                    }
                    else
                        val = par;

                    if (args[ctr].BaseType == typeof(System.Enum))
                        vals[ctr] = Convert.ChangeType(Enum.Parse(args[ctr], Convert.ToString(val)), args[ctr]);
                    else if (args[ctr] == typeof(System.Object))
                        vals[ctr] = val;
                    else
                    {
                        try
                        {
                            vals[ctr] = Convert.ChangeType(val, args[ctr]);
                        }
                        catch
                        {
                            vals[ctr] = val;
                        }
                    }

                }
                ctr++;
            }
        }


        /// <summary>
        /// A method which populates each parameter in the parameter list with the session data 
        /// that needs to be used.
        /// </summary>
        /// <param name="name">The name of the parameter being set.</param>
        /// <param name="value">The value to be returned for the parameter listing.</param>
        protected virtual void SetParameter(string name, out object value)
        {
            if (_setParameter != null)
            {
                try
                {
                    _setParameter(name, out value);
                    return;
                }
                catch
                {
                }
            }

            if (_replacementParameters != null && _replacementParameters.Contains(name))
                value = _replacementParameters[name].Value;
            else
                value = ParameterParser.Parse(name, true);
        }

        /// <summary>
        /// Parses a source string.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The result of the string replacement.</returns>
        protected string ParseString(string text)
        {
            string txt = text;
            for (int ctr = 0; ctr < ReplacementParameters.Count; ctr++)
            {
                Common.KeyValueItem val = _replacementParameters[ctr];
                txt = txt.Replace("%" + val.Key + "%", Convert.ToString(val.Value));
            }
            txt = ParameterParser.ParseString(txt, true);
            return txt;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets the Required Parent Type if Required
        /// </summary>
        [Lookup("ParentReq")]
        [System.ComponentModel.Browsable(false)]
        public virtual string ParentTypeRequired
        {
            get
            {
                return _typerequired;
            }
            set
            {
                _typerequired = value;
            }
        }

        /// <summary>
        /// Gets the current sources field / parameter parser.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DefaultValue(null)]
        public FieldParser ParameterParser
        {
            get
            {
                if (_paramParser == null)
                    _paramParser = new FieldParser(_parent);

                return _paramParser;
            }
        }

        /// <summary>
        /// Gets a reference to the parent object.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public object Parent
        {
            get
            {
                return _parent;
            }
        }

        /// <summary>
        /// Gets and Set reference to the replacement parameters collection.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public Common.KeyValueCollection ReplacementParameters
        {
            get
            {
                if (_replacementParameters == null)
                    _replacementParameters = new Common.KeyValueCollection();
                return _replacementParameters;
            }
        }

        /// <summary>
        /// Gets or Sets an overriding parameter handler delegate.
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DefaultValue(null)]
        public SetParameterHandler ParameterHandler
        {
            get
            {
                return _setParameter;
            }
            set
            {
                _setParameter = value;
            }
        }

        /// <summary>
        /// Gets or Sets the connector object.
        /// </summary>
        protected object Connector
        {
            get
            {
                return _connector;
            }
            set
            {
                _connector = value;
            }
        }

        /// <summary>
        /// Gets or Sets the source type.
        /// </summary>
        protected SourceType SourceType
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        /// <summary>
        /// Gets or Sets the source.
        /// </summary>
        protected string Src
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        /// <summary>
        /// Gets or Sets the method / database call.
        /// </summary>
        protected string Call
        {
            get
            {
                return _call;
            }
            set
            {
                _call = value;
            }
        }

        /// <summary>
        /// Gets or Sets the XML parameters definition.
        /// </summary>
        protected string Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;

                FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(_parameters);
                cfg.Current = "params";

                //Check the Parent Type Required Value
                this.ParentTypeRequired = cfg.GetString("parentRequired", "");

            }
        }

        #endregion

        #region IDataBaseSchema Implementation

        /// <summary>
        /// Returns a list of tables within the database.
        /// </summary>
        /// <returns>A data table of tables.</returns>
        public DataTable GetTables()
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
            {
                var dt = ((Connection)_connector).GetTables();

                using (var vw = new DataView(dt))
                {
                    vw.RowFilter = "SCHEMA = 'dbo' OR SCHEMA is null";
                    return vw.ToTable();
                }

            }
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());
        }

        /// <summary>
        /// Returns a list of views within the database.
        /// </summary>
        /// <returns>A data table of views.</returns>
        public DataTable GetViews()
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
            {
                var dt = ((Connection)_connector).GetViews();

                using (var vw = new DataView(dt))
                {
                    vw.RowFilter = "SCHEMA = 'dbo' OR SCHEMA is null";
                    return vw.ToTable();
                }
            }
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());

        }

        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="objectName">Table / view name.</param>
        /// <returns>A data table of columns.</returns>
        public DataTable GetColumns(string objectName)
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
                return ((Connection)_connector).GetColumns(objectName);
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());
        }

        /// <summary>
        /// Returns a list of stored procedures within the database.
        /// </summary>
        /// <returns>A data table of stored procedures.</returns>
        public DataTable GetProcedures()
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
            {
                var dt = ((Connection)_connector).GetProcedures();

                using (var vw = new DataView(dt))
                {
                    vw.RowFilter = "SCHEMA = 'dbo' OR SCHEMA is null";
                    return vw.ToTable();
                }
            }
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());
        }

        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="procedureName">Procedure name.</param>
        /// <returns>A data table of parameters.</returns>
        public DataTable GetParameters(string procedureName)
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
            {
                var dt = ((Connection)_connector).GetParameters(procedureName);

                using (var vw = new DataView(dt))
                {
                    vw.RowFilter = String.Format("PARAMETER_NAME <> '{0}'", DatabaseInformation.UserContextParameter);
                    return vw.ToTable();
                }
            }
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());
        }

        /// <summary>
        /// Fetches the primary key field name of a particular table.
        /// </summary>
        /// <param name="tableName">Table name within the database.</param>
        /// <returns>Data table of primary key information.</returns>
        public DataTable GetPrimaryKey(string tableName)
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
                return ((Connection)_connector).GetPrimaryKey(tableName);
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());
        }

        /// <summary>
        /// Fetches the list of columns that the stored procedure is going to return back.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <returns>A data table of columns.</returns>
        public DataTable GetProcedureColumns(string procedureName)
        {
            SetInternalSourceConnector();
            if (_connector is Connection)
                return ((Connection)_connector).GetProcedureColumns(procedureName);
            else
                throw new SourceException(HelpIndexes.SourceTypeDoesNotSupportSchema, _type.ToString());

        }


        #endregion

    }


    /// <summary>
    /// Connects to an external OLEDB database.
    /// </summary>
    public class ExternalDataSource : Source
    {
        /// <summary>
        /// A source constructor which connects to an external OLEDB data source.
        /// </summary>
        /// <param name="connectionString">The OLEDB connection string.</param>
        /// <param name="sql">The Select statement or Stored procedure name.</param>
        /// <param name="parameters">The parameters xml.</param>
        public ExternalDataSource(string connectionString, string sql, string parameters)
            : base(SourceType.Dynamic, connectionString, sql, parameters)
        {
        }
    }

    /// <summary>
    /// Connects to a linked server from the existing OMS connection.
    /// </summary>
    public class LinkedServerSource : Source
    {
        /// <summary>
        /// A source constructor which connects to a linked server on the existing OMS connection.
        /// </summary>
        /// <param name="serverName">Linked server name.</param>
        /// <param name="sql">The Select statement or Stored procedure name.</param>
        /// <param name="parameters">The parameters xml.</param>
        public LinkedServerSource(string serverName, string sql, string parameters)
            : base(SourceType.Linked, serverName, sql, parameters)
        {
        }
    }


    /// <summary>
    /// Connects to the existing OMS connection.
    /// </summary>
    public class OMSDataSource : Source
    {
        /// <summary>
        /// A source constructor which connects to the existing OMS connection.
        /// </summary>
        /// <param name="sql">The Select statement or Stored procedure name.</param>
        /// <param name="parameters">The parameters xml.</param>
        public OMSDataSource(string sql, string parameters)
            : base(SourceType.OMS, "", sql, parameters)
        {
        }
    }

    /// <summary>
    /// Runs a static method from a specified class type.
    /// </summary>
    public class StaticMethodSource : Source
    {
        /// <summary>
        /// A source constructor which runs a static method.
        /// </summary>
        /// <param name="assemblyType">Fully qualified assembly type name.</param>
        /// <param name="method">The method to call.</param>
        /// <param name="parameters">The parameters xml.</param>
        public StaticMethodSource(string assemblyType, string method, string parameters)
            : base(SourceType.Class, assemblyType, method, parameters)
        {
        }

        /// <summary>
        /// A source constructor which runs a static method.
        /// </summary>
        /// <param name="assemblyType">The actual assebmly type.</param>
        /// <param name="method">The method to call.</param>
        /// <param name="parameters">The parameters xml.</param>
        public StaticMethodSource(Type assemblyType, string method, string parameters)
            : base(SourceType.Class, assemblyType.FullName, method, parameters)
        {
            Connector = assemblyType;
        }
    }

    /// <summary>
    /// Creates an object instance from its type name.
    /// </summary>
    public class ObjectCreationSource : Source
    {
        /// <summary>
        /// A source constructor which creates an object instance.
        /// </summary>
        /// <param name="assemblyType">Fully qualified assembly type name.</param>
        /// <param name="parameters">The parameters xml.</param>
        public ObjectCreationSource(string assemblyType, string parameters)
            : base(SourceType.Object, assemblyType, "", parameters)
        {
        }
    }

    /// <summary>
    /// Runs a method of an specified objects instance.
    /// </summary>
    public class InstanceMethodSource : Source
    {
        /// <summary>
        /// A source constructor which runs a method off an objects intance.
        /// </summary>
        /// <param name="objInstance">The object to manipulate.</param>
        /// <param name="method">The method to run.</param>
        /// <param name="parameters">The parameters xml.</param>
        public InstanceMethodSource(object objInstance, string method, string parameters)
            : base(objInstance, method, parameters)
        {
        }
    }
}
