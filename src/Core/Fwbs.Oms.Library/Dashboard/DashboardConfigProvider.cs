using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using FWBS.Common;

namespace FWBS.OMS.Dashboard
{
    public class DashboardConfigProvider
    {
        #region Config examples
        /* Dashboard
        <config>
            <tiles>
                <tile row="0" column="0" omsObjCode="DSHBRDTASKTILE" dshbType="TLOMSOBJ" guid="fee3804c-b2d1-4a0d-bf10-e1c9cace310f">
                    <pages>
                        <page code="AllTasks">
                            <columns>
                                <column name="clmAssignedTo" visible="True"/>
                                <column name="clmCreatedBy" visible="True"/>
                                <column name="clmDue" visible="False"/>
                                <column name="clmFlag" visible="True"/>
                                <column name="clmMatter" visible="True" sort="Ascending"/>
                                <column name="clmStatus" visible="True"/>
                                <column name="clmTeam" visible="True"/>
                                <column name="clmType" visible="True"/>
                            </columns>
                        </page>
                        <page code="MyTeamsTasks">
                            <columns>
                                <column name="clmAssignedTo" visible="True"/>
                                <column name="clmCreatedBy" visible="True" sort="Descending"/>
                                <column name="clmDue" visible="False"/>
                                <column name="clmFlag" visible="False"/>
                                <column name="clmMatter" visible="True"/>
                                <column name="clmStatus" visible="True"/>
                                <column name="clmTeam" visible="True"/>
                                <column name="clmType" visible="True"/>
                            </columns>
                        </page>
                    </pages>
                </tile>
                <tile row="0" column="2" omsObjCode="DSHBRDMLTILE" dshbType="DSHBRDMLTILE" guid="77d224f5-7bad-49bc-a2d7-b026b31ea68b"/>
            </tiles>
        </config>
        */
        /* Tile's OmsObject
        <config>
            <params code="DSHBRDFINTILE" width="2" height="1" priority="5" user_roles="ADMIN,ACCOUNTS" />
        </config>
         */
        #endregion

        private const string CODE_ATTRIBUTE = "code";
        private const string VISIBLE_ATTRIBUTE = "visible";
        private const string NAME_ATTRIBUTE = "name";
        private const string SORT_ATTRIBUTE = "sort";
        private const string OMS_OBJECT_CODE_ATTRIBUTE = "omsObjCode";
        private const string DASHBOARD_TYPE_ATTRIBUTE = "dshbType";
        private const string GUID_ATTRIBUTE = "guid";
        private const string COLUMN_ATTRIBUTE = "column";
        private const string ROW_ATTRIBUTE = "row";
        private const string WIDTH_ATTRIBUTE = "width";
        private const string HEIGHT_ATTRIBUTE = "height";
        private const string PRIORITY_ATTRIBUTE = "priority";
        private const string USER_ROLES_ATTRIBUTE = "user_roles";
        private const string COLUMNS_PATH = "/tiles/tile[@guid='{0}']/pages/page[@code='{1}']/columns";
        private const string PAGES_PATH = "/tiles/tile[@guid='{0}']/pages";
        private const string TILES_PATH = "tiles";

        private readonly string _dashboardCode;
        private readonly bool _isConfigurationMode;
        private string _userConfig;

        public DashboardConfigProvider(string dashboardCode, bool isConfigurationMode = false)
        {
            _dashboardCode = dashboardCode;
            _isConfigurationMode = isConfigurationMode;
        }

        public static string CreateXml(string code, Size size, int priority, string userRoles = null)
        {
            var configSettings = new ConfigSetting("<config></config>");
            var paramsItem = configSettings.AddChildItem("params");
            paramsItem.SetString(CODE_ATTRIBUTE, code);
            paramsItem.SetString(WIDTH_ATTRIBUTE, size.Width.ToString());
            paramsItem.SetString(HEIGHT_ATTRIBUTE, size.Height.ToString());
            paramsItem.SetString(PRIORITY_ATTRIBUTE, priority.ToString());
            if (!string.IsNullOrEmpty(userRoles))
            {
                paramsItem.SetString(USER_ROLES_ATTRIBUTE, userRoles);
            }
            return configSettings.DocObject.OuterXml;
        }

        public List<DashboardItemInfo> LoadDashboardObjects()
        {
            Session.CurrentSession.CheckLoggedIn();
            var items = new List<DashboardItemInfo>();
            var table = OmsObject.GetDashboardObjects();
            foreach (DataRow row in table.Rows)
            {
                if (!IsCompatibleWithDashboardType(row))
                {
                    continue;
                }
                OmsObject.CellParameters cell;
                var configSetting = new ConfigSetting(row[2].ToString());
                configSetting.Current = "params";

                var code = configSetting.GetString(CODE_ATTRIBUTE, string.Empty);
                var height = ConvertDef.ToInt16(configSetting.GetString(HEIGHT_ATTRIBUTE, ""), 1);
                var width = ConvertDef.ToInt16(configSetting.GetString(WIDTH_ATTRIBUTE, ""), 1);
                var priority = ConvertDef.ToInt16(configSetting.GetString(PRIORITY_ATTRIBUTE, ""), 1);
                var userRoles = configSetting.GetString(USER_ROLES_ATTRIBUTE, "");
                using (var stringReader = new StringReader(row[2].ToString()))
                {
                    cell = new OmsObject.CellParameters { Code = code, MinimalSize = new Size(width, height), Priority = priority, UserRoles = userRoles };
                }

                var item = new DashboardItemInfo(
                    GetObjectType(row[1].ToString()),
                    row[0].ToString(),
                    cell.Code,
                    cell.Priority,
                    cell.MinimalSize,
                    cell.UserRoles);
                items.Add(item);
            }

            return items;
        }

        public List<BuilderItem> GetUserTilesSettings()
        {
            var userSettingsList = LoadUserTilesSettings();
            var dashboardObjects = LoadDashboardObjects();
            var builderItems = new List<BuilderItem>();
            foreach (var userSettings in userSettingsList)
            {
                DashboardItemInfo dashboardObject = null;
                if (!string.IsNullOrWhiteSpace(userSettings.OmsObjectCode))
                {
                    dashboardObject = dashboardObjects.FirstOrDefault(obj => obj.Code == userSettings.OmsObjectCode);
                    if (dashboardObject == null)
                    {
                        RemoveItem(userSettings.Id);
                        continue;
                    }
                }

                var builderItem = new BuilderItem(userSettings, dashboardObject);
                builderItems.Add(builderItem);
            }

            return builderItems;
        }

        public void AddItem(Guid cellGuid, string cellType, string objCode, Point location, Size size)
        {
            Session.CurrentSession.CheckLoggedIn();

            var configSetting = GetUserDashboardConfigSetting();
            configSetting.Current = TILES_PATH;
            ConfigSettingItem item = configSetting.AddChildItem("tile");
            item.SetString(ROW_ATTRIBUTE, Convert.ToString(location.Y));
            item.SetString(COLUMN_ATTRIBUTE, Convert.ToString(location.X));
            item.SetString(OMS_OBJECT_CODE_ATTRIBUTE, objCode);
            item.SetString(DASHBOARD_TYPE_ATTRIBUTE, cellType);
            item.SetString(GUID_ATTRIBUTE, Convert.ToString(cellGuid));
            item.SetString(WIDTH_ATTRIBUTE, Convert.ToString(size.Width));
            item.SetString(HEIGHT_ATTRIBUTE, Convert.ToString(size.Height));
            SetUserDashboardConfigSetting(configSetting);
        }

        public void RemoveItem(Guid cellGuid)
        {
            Session.CurrentSession.CheckLoggedIn();

            var configSetting = GetUserDashboardConfigSetting();
            configSetting.Current = TILES_PATH;
            var tiles = configSetting.CurrentChildItems;
            foreach (var tile in tiles)
            {
                var guid = ConvertDef.ToGuid(tile.GetString(GUID_ATTRIBUTE, Convert.ToString(Guid.Empty)), Guid.Empty);
                if (guid == cellGuid)
                {
                    configSetting.DocCurrent.RemoveChild(tile.Element);
                    break;
                }
            }

            SetUserDashboardConfigSetting(configSetting);
        }

        public void ResizeItem(Guid cellGuid, Point location, Size size)
        {
            Session.CurrentSession.CheckLoggedIn();

            var configSetting = GetUserDashboardConfigSetting();
            configSetting.Current = TILES_PATH;
            var tiles = configSetting.CurrentChildItems;
            foreach (var tile in tiles)
            {
                var guid = ConvertDef.ToGuid(tile.GetString(GUID_ATTRIBUTE, Convert.ToString(Guid.Empty)), Guid.Empty);
                if (guid == cellGuid)
                {
                    tile.SetString(ROW_ATTRIBUTE, Convert.ToString(location.Y));
                    tile.SetString(COLUMN_ATTRIBUTE, Convert.ToString(location.X));
                    tile.SetString(WIDTH_ATTRIBUTE, Convert.ToString(size.Width));
                    tile.SetString(HEIGHT_ATTRIBUTE, Convert.ToString(size.Height));
                    break;
                }
            }
            SetUserDashboardConfigSetting(configSetting);
        }

        protected virtual bool IsCompatibleWithDashboardType(DataRow row)
        {
            return row[3].ToString().Equals("FWBS.OMS.User", StringComparison.OrdinalIgnoreCase);
        }

        #region Columns Settings

        public IEnumerable<ColumnsSettings.Column> GetColumnsSettings(Guid cellGuid, string pageCode)
        {
            var configSetting = GetUserDashboardConfigSetting();
            try
            {
                configSetting.Current = string.Format(COLUMNS_PATH, cellGuid, pageCode);
                var columns = configSetting.CurrentChildItems;
                var result = new List<ColumnsSettings.Column>();
                foreach (var c in columns)
                {
                    var column = new ColumnsSettings.Column()
                    {
                        Name = c.GetString(NAME_ATTRIBUTE, string.Empty),
                        Visible = ConvertDef.ToBoolean(c.GetString(VISIBLE_ATTRIBUTE, bool.TrueString), true),
                        SortOrder = (SortOrder)ConvertDef.ToEnum(c.GetString(SORT_ATTRIBUTE, "None"), SortOrder.None)
                    };
                    result.Add(column);
                }
                return result;
            }
            catch(XmlException)
            {
                return null;
            }
        }

        public void UpdateColumnsSettings(Guid cellGuid, string pageCode, IEnumerable<ColumnsSettings.Column> columns)
        {
            bool updated = false;
            var configSetting = GetUserDashboardConfigSetting();
            try
            {
                UpdateColumnSettings(configSetting, cellGuid, pageCode, columns);
                updated = true;
            }
            catch (XmlException)
            {
                configSetting.Current = TILES_PATH;
                var tiles = configSetting.CurrentChildItems;
                foreach (var tile in tiles)
                {
                    var guid = ConvertDef.ToGuid(tile.GetString(GUID_ATTRIBUTE, Convert.ToString(Guid.Empty)), Guid.Empty);
                    if (guid == cellGuid)
                    {
                        CreateColumnSettings(configSetting, cellGuid, pageCode, columns);
                        updated = true;
                        break;
                    }
                }
            }
            if (updated)
            {
                SetUserDashboardConfigSetting(configSetting);
            }
        }

        private void UpdateColumnSettings(ConfigSetting configSetting, Guid cellGuid, string pageCode, IEnumerable<ColumnsSettings.Column> columns)
        {
            configSetting.Current = string.Format(COLUMNS_PATH, cellGuid, pageCode);
            configSetting.ClearElements();

            foreach (var column in columns)
            {
                ConfigSettingItem item = configSetting.AddChildItem(COLUMN_ATTRIBUTE);
                item.SetString(NAME_ATTRIBUTE, column.Name);
                item.SetString(VISIBLE_ATTRIBUTE, column.Visible.ToString());
                if (column.SortOrder != SortOrder.None)
                    item.SetString(SORT_ATTRIBUTE, column.SortOrder.ToString());
            }
        }

        private void CreateColumnSettings(ConfigSetting configSetting, Guid cellGuid, string pageCode, IEnumerable<ColumnsSettings.Column> columns)
        {
            configSetting.Current = string.Format(PAGES_PATH, cellGuid, pageCode);
            var pageItem = configSetting.AddChildItem("page");
            pageItem.SetString(CODE_ATTRIBUTE, pageCode);
            UpdateColumnSettings(configSetting, cellGuid, pageCode, columns);
        }

        #endregion

        private List<UserSettings> LoadUserTilesSettings()
        {
            Session.CurrentSession.CheckLoggedIn();
            var configSetting = GetUserDashboardConfigSetting();
            var items = new List<UserSettings>();
            configSetting.Current = TILES_PATH;
            var tiles = configSetting.CurrentChildItems;
            foreach (var tile in tiles)
            {
                var guid = ConvertDef.ToGuid(tile.GetString(GUID_ATTRIBUTE, Guid.Empty.ToString()), Guid.Empty);
                var posRow = ConvertDef.ToInt16(tile.GetString(ROW_ATTRIBUTE, "0"), 0);
                var posColumn = ConvertDef.ToInt16(tile.GetString(COLUMN_ATTRIBUTE, "0"), 0);
                var width = ConvertDef.ToInt16(tile.GetString(WIDTH_ATTRIBUTE, "0"), 0);
                var height = ConvertDef.ToInt16(tile.GetString(HEIGHT_ATTRIBUTE, "0"), 0);
                var item = new UserSettings(guid, tile.GetString(DASHBOARD_TYPE_ATTRIBUTE, string.Empty), tile.GetString(OMS_OBJECT_CODE_ATTRIBUTE, string.Empty), new Point(posColumn, posRow), new Size(width, height));
                items.Add(item);
            }

            return items;
        }

        private static OMSObjectTypes GetObjectType(string type)
        {
            switch (type)
            {
                case "Enquiry":
                    return OMSObjectTypes.Enquiry;
                case "List":
                    return OMSObjectTypes.List;
                case "Addin":
                    return OMSObjectTypes.Addin;
                default:
                    throw new ArgumentException("The type could be Enquiry or List only.");
            }
        }

        protected ConfigSetting GetUserDashboardConfigSetting()
        {
            if (string.IsNullOrEmpty(_userConfig))
            {
                var connection = Session.CurrentSession.Connection;
                var parameters = new IDataParameter[3]
                {
                    connection.CreateParameter("usrID", Session.CurrentSession.CurrentUser.ID),
                    connection.CreateParameter("dshObjCode", _dashboardCode),
                    connection.CreateParameter("isConfigurationMode", _isConfigurationMode)
                };
                _userConfig = Convert.ToString(connection.ExecuteProcedureScalar("dbo.sprGetUserDashboardCells", parameters));
            }
            return new ConfigSetting(_userConfig);
        }

        protected void SetUserDashboardConfigSetting(ConfigSetting configSetting)
        {
            _userConfig = configSetting.DocObject.OuterXml;
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[4]
            {
                connection.CreateParameter("usrID", Session.CurrentSession.CurrentUser.ID),
                connection.CreateParameter("dshObjCode", _dashboardCode),
                connection.CreateParameter("dshConfig", _userConfig),
                connection.CreateParameter("isConfigurationMode", _isConfigurationMode)
            };
            connection.ExecuteProcedureScalar("dbo.sprUpdateUserDashboards", parameters);
        }

        #region Classes

        public class DashboardItemInfo
        {
            public DashboardItemInfo(OMSObjectTypes objectType, string code, string sourceCode, int priority, Size minimalSize, string roles)
            {
                ObjectType = objectType;
                Code = code;
                SourceCode = sourceCode;
                Priority = priority;
                MinimalSize = minimalSize;
                UserRoles = roles;
            }

            public OMSObjectTypes ObjectType { get; }
            public string Code { get; }
            public string SourceCode { get; }
            public int Priority { get; }
            public Size MinimalSize { get; }
            public string UserRoles { get; }
        }

        public class UserSettings
        {
            public UserSettings(Guid id, string dashboardType, string objCode, Point location, Size size)
            {
                Id = id;
                DashboardType = dashboardType;
                OmsObjectCode = objCode;
                Location = location;
                Size = size;
            }

            public Guid Id { get; private set; }
            public string DashboardType { get; private set; }
            public string OmsObjectCode { get; private set; }
            public Point Location { get; set; }
            public Size Size { get; set; }
        }

        public class BuilderItem
        {
            public BuilderItem(UserSettings userSettings, DashboardItemInfo itemInfo)
            {
                UserSettings = userSettings;
                ItemInfo = itemInfo;
            }

            public UserSettings UserSettings { get; private set; }
            public DashboardItemInfo ItemInfo { get; private set; }
        }

        public class ColumnsSettings
        {
            public ColumnsSettings()
            {
                Pages = new List<Page>();
            }


            public List<Page> Pages { get; set; }

            public class Page
            {
                public string Code { get; set; }

                public List<Column> Columns { get; set; }
            }

            public class Column
            {
                public string Name { get; set; }

                public bool Visible { get; set; }

                public SortOrder SortOrder { get; set; } 
            }
        }

        #endregion
    }
}
