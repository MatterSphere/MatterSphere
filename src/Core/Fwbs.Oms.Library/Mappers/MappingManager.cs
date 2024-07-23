using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FWBS.OMS.Mappers
{
    [ComVisible(true)]
    [InterfaceType( ComInterfaceType.InterfaceIsDual)]
    [Guid("4DE6A826-60D7-4dd3-AB63-26A598261D5E")]
    public interface IMappingManager
    {
        [DispId(1)]
        string GetExternalID(string system, string type, string id);
        [DispId(2)]
        string GetInternalID(string system, string type, string id);
    }

    [Guid("4575E84D-D046-43a1-9DAC-D807AB0392E0")]
    public class MappingManager : IMappingManager, FWBS.OMS.Caching.ICacheable
    {

        private static MappingManager _mm;

        public static MappingManager GetMappingManager
        {
            get
            {
                if (_mm == null)
                    _mm = new MappingManager();

                return _mm;
            }
        }

        Dictionary<string, SystemMapper> mappedSystems = new Dictionary<string, SystemMapper>();

        public MappingManager()
        {
            Session.CurrentSession.CachedItems.Add("MappingManager", this);
        }
        
        public string GetExternalID(string system, string type, string id)
        {

            if (!mappedSystems.ContainsKey(system))
                mappedSystems.Add(system, new SystemMapper(system));

            return mappedSystems[system].GetExternalID(type, id);
        }

        public string GetInternalID(string system, string type, string id)
        {
            if (!mappedSystems.ContainsKey(system))
                mappedSystems.Add(system, new SystemMapper(system));

            return mappedSystems[system].GetInternalID(type, id);
        }

        #region ICacheable Members

        public void Clear()
        {
            mappedSystems.Clear();
        }

        #endregion

        #region Add Mappings

        public void AddMapping(string system, string type, string OMSValue, string toValue)
        {
            AddMapping(system, type, OMSValue, toValue, null, false);
        }

        public void AddMapping(string system, string type, string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            if(!mappedSystems.ContainsKey(system))
                mappedSystems.Add(system, new SystemMapper(system));

            mappedSystems[system].AddMapping(type, OMSValue, toValue, mappingType, isSetting);
        }

        public void EditMapping(string system, string type, string OMSValue, string toValue, string mappingType, bool isSetting)
        {
            if (!mappedSystems.ContainsKey(system))
                mappedSystems.Add(system, new SystemMapper(system));

            mappedSystems[system].EditMapping(type, OMSValue, toValue, mappingType, isSetting);
        }

        public void Update(string system, string type)
        {
            if (!mappedSystems.ContainsKey(system))
                return;

            mappedSystems[system].Update(type);
        }

        #endregion

    }
}
