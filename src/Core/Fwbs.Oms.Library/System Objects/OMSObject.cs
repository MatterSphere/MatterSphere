using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using FWBS.Common;

namespace FWBS.OMS
{
    /// <summary>
    /// 6000 This object is used to expose the available objects for the compatible type
    /// Passing a type through the constructor will expose a list of available objects for this type
    /// </summary>
    public class OmsObject : LookupTypeDescriptor,IDisposable
	{
		#region Fields

		private DataTable _omsobjects = null;
		private const string Sql = "select *, dbo.GetCodeLookupDesc('OMSOBJECT', ObjCode, @UI)  as ObjDesc, dbo.GetCodeLookupHelp('OMSOBJECT', ObjCode, @UI)  as ObjHelp  from dbOMSObjects";
		private const string updateablesql = "select * from dbOMSObjects";
		private const string Table = "OMSOBJECTS";
        private const int MinTileSize = 1;
        private const int MaxTileWidth = 3;
        private const int MaxTileHeight = 2;
        private const int DefaultTilePriority = 1;
        private Size _prevSizeValue = new Size(MinTileSize, MinTileSize);
        private int _prevPriorityValue = DefaultTilePriority;
        private string _prevUserRoles;
        private CodeLookupDisplayMulti _userRoles;

        #endregion

        #region Constructors

        private OmsObject()
		{
			IDataParameter[] pars = new IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			_omsobjects = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, true, pars);
			_omsobjects.Columns["objDesc"].ReadOnly=false;
			_omsobjects.Columns["objHelp"].ReadOnly=false;
			//Add a new record.
			Global.CreateBlankRecord(ref _omsobjects, true);
		}

		/// <summary>
		/// Consructor that will Load a OMS Object from the Database.
		/// </summary>
		/// <param name="code">The OMS object code.</param>
		public OmsObject(string code)
		{
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			pars[1] = Session.CurrentSession.Connection.AddParameter("CODE", code);
			_omsobjects = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " where objcode = @CODE", Table, false, pars);

			if ((_omsobjects == null) || (_omsobjects.Rows.Count == 0)) 
			{
				throw new OMSException2("6001","OMS object of type code '%1%' is not registered within the OMS database.",null,true,code);
			}
		}
	
		#endregion

		#region Methods

		/// <summary>
		/// Sets the raw internal data object with the specified value under the specified field name.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <param name="val">Value to use.</param>
		private void SetExtraInfo (string fieldName, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			_omsobjects.Rows[0][fieldName] = val;
		}

		/// <summary>
		/// Returns a raw value from the internal data object, specified by the database field name given.
		/// </summary>
		/// <param name="fieldName">Database Field Name.</param>
		/// <returns>The current data value.</returns>
		private object GetExtraInfo(string fieldName)
		{
			object val = _omsobjects.Rows[0][fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

        private CellParameters ReadCellParameters()
        {
            if (string.IsNullOrWhiteSpace(Dashboard))
            {
                return null;
            }

            var configSetting = new ConfigSetting(Dashboard);
            configSetting.Current = "params";

            var code = configSetting.GetString("code", string.Empty);
            var height = ConvertDef.ToInt16(configSetting.GetString("height", ""), MinTileSize);
            var width = ConvertDef.ToInt16(configSetting.GetString("width", ""), MinTileSize);
            var priority = ConvertDef.ToInt16(configSetting.GetString("priority", ""), DefaultTilePriority);
            var userRoles = configSetting.GetString("user_roles", "");

            return new CellParameters { Code = code, MinimalSize = new Size(width, height), Priority = priority, UserRoles = userRoles };

        }

        private void WriteCellParameters(string code, Size? minimalSize, int? priority, string userRoles)
        {
            var cellParameters = ReadCellParameters();
            var dashboardParameters = cellParameters != null
                ? new CellParameters(code ?? cellParameters.Code, minimalSize ?? cellParameters.MinimalSize,
                    priority ?? cellParameters.Priority, userRoles ?? cellParameters.UserRoles)
                : new CellParameters(code ?? Code, minimalSize ?? _prevSizeValue, priority ?? _prevPriorityValue,
                    userRoles ?? _prevUserRoles);
            var configSettings = new ConfigSetting("<config></config>");
            var paramsItem = configSettings.AddChildItem("params");
            paramsItem.SetString("code", dashboardParameters.Code);
            paramsItem.SetString("width", dashboardParameters.MinimalSize.Width.ToString());
            paramsItem.SetString("height", dashboardParameters.MinimalSize.Height.ToString());
            paramsItem.SetString("priority", dashboardParameters.Priority.ToString());
            if (!string.IsNullOrEmpty(dashboardParameters.UserRoles))
            {
                paramsItem.SetString("user_roles", dashboardParameters.UserRoles);
            }
            Dashboard = configSettings.DocObject.OuterXml;
        }

        public void Update()
		{
			Session.CurrentSession.Connection.Update(_omsobjects, updateablesql);
		}
		
		#endregion

		#region Properties

		[LocCategory("Design")]
		[Description("Unique code that defines the OMSObject")]
		[DefaultValue("")]
		public virtual string Code
		{
			get
			{
				return Convert.ToString(GetExtraInfo("objcode"));
			}
		}
		
		[LocCategory("Design")]
		[DefaultValue("")]
		[Description("Description used to identify the OMS Object")]
		public string Description
		{
			get
			{
				return  Convert.ToString(GetExtraInfo("objdesc"));
			}
			set
			{
				SetExtraInfo("objdesc", value);
				FWBS.OMS.CodeLookup.Create("OMSOBJECT",Code,value,this.DetailedDescription,CodeLookup.DefaultCulture,true,true,true);
			}
		}

		[LocCategory("Design")]
		[DefaultValue("")]
		[Description("Detailed Description used to describe this OMS Object")]
		[Lookup("Detailed")]
		public virtual string DetailedDescription
		{
			get
			{
				return  Convert.ToString(GetExtraInfo("objhelp"));
			}
			set
			{
				SetExtraInfo("objhelp", value);
				FWBS.OMS.CodeLookup.Create("OMSOBJECT",Code,this.Description,value,CodeLookup.DefaultCulture,true,true,true);
			}
		}

		[LocCategory("Type")]
		[Description("The Type Name used it group the OMS Objects by.")]
		public virtual string TypeCompatible
		{
			get
			{
				return Convert.ToString(GetExtraInfo("objtypecompatible"));
			}
			set
			{
				SetExtraInfo("objtypecompatible", value);
			}
		}

		[Browsable(false)]
		public virtual Guid Signed
		{
			get
			{
				if (GetExtraInfo("objsigned") is DBNull)
					return Guid.Empty;
				else
					return (Guid)GetExtraInfo("objsigned");
			}
		}

		[LocCategory("Type")]
		[Description("The type of the OMS Object")]
		public virtual OMSObjectTypes ObjectType
		{
			get
			{
				return (OMSObjectTypes)Common.ConvertDef.ToEnum(GetExtraInfo("objtype"), OMSObjectTypes.Enquiry);
			}
		}

        [LocCategory("Setting")]
        [Description("Distributed Assembly Name")]
        public virtual string Assembly
        {
            get
            {
                if (_omsobjects.Columns.Contains("ObjAddinFileName"))
                    return Convert.ToString(GetExtraInfo("ObjAddinFileName"));
                else
                    return "";
            }
            set
            {
                if (_omsobjects.Columns.Contains("ObjAddinFileName"))
                    SetExtraInfo("ObjAddinFileName", value);
            }
        }
        
        [LocCategory("Setting")]
		[Description("Windows Name")]
		public virtual string Windows
		{
			get
			{
				return Convert.ToString(GetExtraInfo("objwinnamespace"));
			}
            set
            {
                SetExtraInfo("objwinnamespace", value);
            }
		}

        [Browsable(false)]
        public string Dashboard
        {
            get
            {
                return Convert.ToString(GetExtraInfo("tileparams"));
            }
            set
            {
                SetExtraInfo("tileparams", value);
            }
        }

        [LocCategory("Dashboard")]
        [Description("Code")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual string TileCode
        {
            get
            {
                return ReadCellParameters()?.Code;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    WriteCellParameters(value, null, null, null);
                }
                else
                {
                    _prevSizeValue = TileMinSize ?? new Size(MinTileSize, MinTileSize);
                    _prevPriorityValue = TilePriority ?? DefaultTilePriority;
                    _prevUserRoles = UserRoles;
                    Dashboard = null;
                }
            }
        }

        [LocCategory("Dashboard")]
        [Description("Minimal Size")]
        public virtual Size? TileMinSize
        {
            get
            {
                return ReadCellParameters()?.MinimalSize;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(Dashboard))
                {
                    if (value.HasValue)
                    {
                        var val = value.Value;
                        if (val.Width >= MinTileSize && val.Width <= MaxTileWidth && val.Height >= MinTileSize && val.Height <= MaxTileHeight)
                        {
                            WriteCellParameters(null, value, null, null);
                        }
                    }
                    else
                    {
                        throw new Exception($"The parameter value should be between {MinTileSize}x{MinTileSize} and {MaxTileWidth}x{MaxTileHeight}");

					}
                }
            }
        }

        [LocCategory("Dashboard")]
        [Description("Priority")]
        public virtual int? TilePriority
        {
            get
            {
                return ReadCellParameters()?.Priority;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(Dashboard))
                {
                    if (value.HasValue)
                    {
                        WriteCellParameters(null, null, value, null);
                    }
                    else
                    {
                        throw new Exception("The parameter is required");
                    }
                }
            }
        }

        [Lookup("USERROLES")]
        [LocCategory("Dashboard")]
        [System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
        [CodeLookupSelectorTitle("USERROLES", "User Roles")]
        public CodeLookupDisplayMulti UserRolesDisplay
        {
            get
            {
                if (_userRoles == null)
                {
                    _userRoles = new CodeLookupDisplayMulti("USRROLES")
                    {
                        Codes = this.UserRoles??string.Empty
                    };
                }

                return _userRoles;
            }
            set
            {
                _userRoles = value;
                this.UserRoles = value.Codes;
            }
        }

        [Browsable(false)]
        public string UserRoles
        {
            get
            {
                return ReadCellParameters()?.UserRoles;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(Dashboard))
                {
                    WriteCellParameters(null, null, null, value);
                }
            }
        }

        [LocCategory("Setting")]
		[Description("Web Name")]
		public virtual string WEB
		{
			get
			{
				return Convert.ToString(GetExtraInfo("objwebnamespace"));
			}
		}

		[LocCategory("Setting")]
		[Description("PDA (Personal Digital Assistant) Name")]
		public virtual string PDA
		{
			get
			{
				return Convert.ToString(GetExtraInfo("objpdanamespace"));
			}
		}


		#endregion

		#region Static
		/// <summary>
		/// Retrieves all of the all OMS Objects from the database
		/// </summary>
		public static DataTable GetOMSObjects()
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] pars = new IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql, Table, pars);
			return dt;
		}

        /// <summary>
        /// Retrieves all of the Dashboard elements from the database
        /// </summary>
        public static DataTable GetDashboardObjects()
        {
            Session.CurrentSession.CheckLoggedIn();
            var pars = new IDataParameter[0];
            var sql = "SELECT [ObjCode], [ObjType], [TileParams], [ObjTypeCompatible] FROM [dbo].[dbOMSObjects] WHERE [TileParams] IS NOT NULL";
            DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(sql, Table, pars);
            return dt;
        }

        /// <summary>
        /// Retrieves all of the all OMS Objects from the database
        /// </summary>
        public static DataTable GetOMSObjects(string typeCompatible)
		{
			return GetOMSObjects(typeCompatible,null);
		}
		
		/// <summary>
		/// Retrieves all of the all OMS Objects from the database
		/// </summary>
		public static DataTable GetOMSObjects(string typeCompatible, string Objtype)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter [] pars = new IDataParameter[3];
			pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
			if (typeCompatible == null)
				pars[1] = Session.CurrentSession.Connection.AddParameter("TYPE", DBNull.Value);
			else
				pars[1] = Session.CurrentSession.Connection.AddParameter("TYPE", typeCompatible);
			if (Objtype == null)
				pars[2] = Session.CurrentSession.Connection.AddParameter("OBJTYPE", DBNull.Value);
			else
				pars[2] = Session.CurrentSession.Connection.AddParameter("OBJTYPE", Objtype);
			DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Sql + " WHERE objtypecompatible = COALESCE(@TYPE,objtypecompatible) AND objType = COALESCE(@OBJTYPE,objtype)", Table, pars);
			return dt;
		}

		/// <summary>
		/// Is the OMS Object Registered
		/// </summary>
		public static bool IsObjectRegistered(string code, OMSObjectTypes type)
		{
            if (string.IsNullOrEmpty(code)) return false;
            Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] pars = new IDataParameter[2];
			pars[0] = Session.CurrentSession.Connection.AddParameter("TYPE", type.ToString());
			pars[1] = Session.CurrentSession.Connection.AddParameter("CODE", code);
			int count = Convert.ToInt32(Session.CurrentSession.Connection.ExecuteSQLScalar("select count(*) from dbomsobjects where ObjType = @TYPE and (ObjWinNamespace = @CODE OR ObjWebNameSpace = @CODE OR ObjPDANameSpace = @CODE)", pars));
			return (count > 0);
		}

		/// <summary>
		/// Does OMS Object Exist
		/// </summary>
		public static bool Exists(string code)
		{
			Session.CurrentSession.CheckLoggedIn();
			IDataParameter[] pars = new IDataParameter[1];
			pars[0] = Session.CurrentSession.Connection.AddParameter("CODE", code);
			int count = (int)Session.CurrentSession.Connection.ExecuteSQLScalar("select count(*) from dbomsobjects where ObjCode = @CODE", pars);
			return (count > 0);
		}
		
		/// <summary>
		/// Unregister OMS Object
		/// </summary>
		/// <param name="code">The Code</param>
		/// <returns>Succesful</returns>
		public static bool UnRegister(string code)
		{
			OmsObject n = new OmsObject(code);
			n._omsobjects.Rows[0].Delete();
			n.Update();
			return true;
		}
		
		public static bool Register(string code, OMSObjectTypes type, string compatibleType, string description, string detailedDescription, string windows, string web, string pda, string dashboard = null)
		{
			OmsObject n = new OmsObject();
			n.SetExtraInfo("objcode", code);
			n.TypeCompatible = compatibleType;
			n.SetExtraInfo("objtype", type);
			n.Description = description;
			if (pda != null) n.SetExtraInfo("objpdanamespace", pda);
			if (web != null) n.SetExtraInfo("objwebnamespace", web);
			if (windows != null) n.SetExtraInfo("objwinnamespace", windows);
            if (dashboard != null) n.SetExtraInfo("tileparams", dashboard);
            n.DetailedDescription = detailedDescription;
			n.Update();
			return true;
		}
		#endregion

		#region IDisposable Implementation

		public virtual void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (_omsobjects != null)
				{
					_omsobjects.Dispose();
					_omsobjects = null;
				}
			}

		}

        #endregion

        #region Classes

        public class CellParameters
        {
            public CellParameters() { }

            public CellParameters(string code, Size minimalSize, int priority, string userRoles)
            {
                Code = code;
                MinimalSize = minimalSize;
                Priority = priority;
                UserRoles = userRoles;
            }

            public string Code { get; set; }
            public Size MinimalSize { get; set; }
            public int Priority { get; set; }
            public string UserRoles { get; set; }
        }

        #endregion
    }
}