using System;
using System.Collections.Generic;

namespace FWBS.OMS.Mappers
{
    class SystemMapper : FWBS.OMS.Caching.ICacheable 
    {
        Dictionary<string, EntityMapper> mappedEntity = new Dictionary<string, EntityMapper>();

        private string mapperSystem;
        private string systemID;

        public SystemMapper( string system)
        {
            mapperSystem = system;

        }

        public string GetExternalID(string type, string id)
        {

            if (!mappedEntity.ContainsKey(type))
            {
                GetSystemID();

                mappedEntity.Add(type, new EntityMapper(type, systemID));
            }

            return mappedEntity[type].GetExternalId(id);
        }

        public string GetInternalID(string type, string id)
        {
            if (!mappedEntity.ContainsKey(type))
            {
                GetSystemID();
                mappedEntity.Add(type, new EntityMapper(type, systemID));
            }

            return mappedEntity[type].GetInternalID(id);
        }


        private void GetSystemID()
        {
            if (!string.IsNullOrEmpty(systemID))
                return;

            //TODO: Goto the database and look in dbIntegrationEntity for the ID
            //create the papamerter
            string sql = "select ID from fddbIntegrationSystem where Name= @Name";

            System.Data.IDataParameter[] parameter = new System.Data.IDataParameter[1];
            parameter[0] = Session.CurrentSession.Connection.AddParameter("Name", mapperSystem);

            systemID = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar(sql, parameter));

            if (string.IsNullOrEmpty(systemID))
                throw new NotSupportedException(string.Format("'{0}' does not currently support data mapping", mapperSystem));
        }

        #region ICacheable Members

        public void Clear()
        {
            mappedEntity.Clear();
        }

        #endregion

        #region Mapping

        public void AddMapping(string type, string OMSValue, string toValue)
        {
            AddMapping(type, OMSValue, toValue, null, false);
        }

        internal void AddMapping(string type, string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            if (!mappedEntity.ContainsKey(type))
            {
                GetSystemID();
                mappedEntity.Add(type, new EntityMapper(type, systemID));
            }

            mappedEntity[type].AddMapping(OMSValue, toValue, mappingType, isSetting);
        }

        internal void EditMapping(string type, string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            if (!mappedEntity.ContainsKey(type))
            {
                GetSystemID();
                mappedEntity.Add(type, new EntityMapper(type, systemID));
            }

            mappedEntity[type].EditMapping(OMSValue, toValue, mappingType, isSetting);
        }

        public void Update(string type)
        {
            if (!mappedEntity.ContainsKey(type))
                return;

            mappedEntity[type].UpdateMappings();
        }

        #endregion
    }
}
