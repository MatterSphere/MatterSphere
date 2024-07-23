using System;
using System.Data;

namespace FWBS.OMS.Mappers
{
    class EntityMapper : FWBS.OMS.Caching.ICacheable
    {

        private string entityType;
        private string systemID;
        private string entityID;

        DataTable entityTable;

        public EntityMapper(string type, string systemID)
        {
            entityType = type;
            this.systemID = systemID;
        }

        public string GetExternalId(string internalID)
        {
            return GetEntityValue("InternalID", "ExternalID", internalID);
        }

        public string GetInternalID(string externalID)
        {
           return GetEntityValue("ExternalID", "InternalID", externalID);
        }

        private string GetEntityValue(string searchField, string returnField, string value)
        {
            if (entityTable != null)
            {
                DataView view = entityTable.DefaultView;

                view.RowFilter = string.Format("{0} = '{1}'", searchField, value);
                if (view.Count > 0)
                    return view[0][returnField].ToString();
            }

            GetEntityID();

            string sql = string.Format("Select * from fddbIntegrationMapping where SystemID = @system and EntityID = @entity and {0} = @search", searchField);

            System.Data.IDataParameter[] parameters = new System.Data.IDataParameter[3];

            parameters[0] = Session.CurrentSession.Connection.AddParameter("system", systemID);
            parameters[1] = Session.CurrentSession.Connection.AddParameter("entity", entityID);
            parameters[2] = Session.CurrentSession.Connection.AddParameter("search", value);

            DataTable result = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "ENTITYS", parameters);

            if (entityTable == null)
            {
                entityTable = result;
                entityTable.PrimaryKey = new DataColumn[] { entityTable.Columns["ID"] };
            }
            else
                entityTable.Merge(result, true, MissingSchemaAction.Add);

            if (result.Rows.Count == 0)
                return null;

            return Convert.ToString(result.Rows[0][returnField]);
        }

        private void GetEntityID()
        {
            if (!string.IsNullOrEmpty(entityID))
                return;

            string sql = "Select ID from fddbIntegrationEntity where Name = @entity";
            System.Data.IDataParameter[] parameter = new System.Data.IDataParameter[1];
            parameter[0] = Session.CurrentSession.Connection.AddParameter("entity", entityType);

            entityID = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar(sql, parameter));
            
            if (string.IsNullOrEmpty(entityID))
                throw new NotSupportedException(string.Format("'{0}' is not supported for mapping", entityType));
        }

        #region ICacheable Members

        public void Clear()
        {
            entityTable = null;
        }

        #endregion
        #region Mapping

        public void AddMapping(string OMSValue, string toValue)
        {
            AddMapping(OMSValue, toValue, null, false);
        }

        internal void AddMapping(string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            if (!string.IsNullOrEmpty(GetExternalId(OMSValue)))
                return;

            if (!string.IsNullOrEmpty(GetInternalID(toValue)))
                return;

            DataRow row = entityTable.NewRow();

            row["ID"] = Guid.NewGuid();
            row["SystemID"] = systemID;
            row["EntityID"] = entityID;
            row["InternalID"] = OMSValue;
            row["ExternalID"] = toValue;
            row["MappingType"] = mappingType;
            row["Setting"] = isSetting;

            entityTable.Rows.Add(row);
        }

        internal void EditMapping(string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            foreach (DataRow row in entityTable.Rows)
            {
                if (Convert.ToString(row["InternalID"]).Equals(OMSValue, StringComparison.OrdinalIgnoreCase))
                {
                    row["ExternalID"] = toValue;
                    row["MappingType"] = mappingType;
                    row["Setting"] = isSetting;
                }
            }
        }

        public void UpdateMappings()
        {
            //CHANGE TO DATATABLE
            //TODO: Write to the Database
            if (entityTable != null)
                Session.CurrentSession.Connection.Update(entityTable, "fddbIntegrationMapping", false);
        }

        #endregion
    }

}
