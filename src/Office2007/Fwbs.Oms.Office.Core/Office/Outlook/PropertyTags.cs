using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Fwbs.Office.Outlook
{
    using Redemption;

    internal static class PropertyTags
    {
        public const string PROPTAG_CUSTOM_PROPERTY = "{00020329-0000-0000-C000-000000000046}";
        public const uint CUSTOM_PROPERTY_BASELINE = 0x80000000;
        public const string MAPPINGS_CACHEABLE_KEY = "PROPTAGMAPPINGS";
    }

    // Cache PropTag to Name mappings to reduce the number of MAPIUtils.GetNamesFromIDs calls which are slow in Exchange Online mode
    internal class PropertyTagMappings : FWBS.OMS.Caching.ICacheable
    {
        private readonly string _cacheFilePath;
        private readonly Dictionary<string, Dictionary<int, INamedProperty>> _propTagMappings;

        public PropertyTagMappings()
        {
            _propTagMappings = new Dictionary<string, Dictionary<int, INamedProperty>>();

            DirectoryInfo cacheDir = new DirectoryInfo(Path.Combine(FWBS.OMS.Global.GetCachePath(), "Outlook"));
            if (!cacheDir.Exists)
                cacheDir.Create();

            _cacheFilePath = Path.Combine(cacheDir.FullName, "PropTagMappings.xml");
            if (File.Exists(_cacheFilePath))
                Load();
        }

        void FWBS.OMS.Caching.ICacheable.Clear()
        {
            _propTagMappings.Clear();
        }

        private void Load()
        {
            int proptag;
            INamedProperty namedProp;
            Dictionary<int, INamedProperty> propTagMapping = null;
            try
            {
                using (XmlTextReader xmlReader = new XmlTextReader(_cacheFilePath))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.IsStartElement())
                        {
                            switch (xmlReader.Name)
                            {
                                case "Mapping":
                                    propTagMapping = new Dictionary<int, INamedProperty>();
                                    _propTagMappings.Add(xmlReader.GetAttribute("signature"), propTagMapping);
                                    break;

                                case "Property":
                                    proptag = int.Parse(xmlReader.GetAttribute("tag"), System.Globalization.NumberStyles.HexNumber);
                                    namedProp = new CachedNamedProperty(xmlReader.GetAttribute("guid"), xmlReader.GetAttribute("name"));
                                    propTagMapping.Add(proptag, namedProp);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private void Save()
        {
            try
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(_cacheFilePath, System.Text.Encoding.UTF8) { Formatting = Formatting.Indented })
                {
                    xmlWriter.WriteStartDocument(true);
                    xmlWriter.WriteStartElement("PropTagMappings");

                    foreach (var stores in _propTagMappings)
                    {
                        xmlWriter.WriteStartElement("Mapping");
                        xmlWriter.WriteAttributeString("signature", stores.Key);

                        foreach (var mappings in stores.Value)
                        {
                            xmlWriter.WriteStartElement("Property");
                            xmlWriter.WriteAttributeString("tag", mappings.Key.ToString("x8"));
                            xmlWriter.WriteAttributeString("guid", mappings.Value.GUID);
                            xmlWriter.WriteAttributeString("name", Convert.ToString(mappings.Value.ID));
                            xmlWriter.WriteEndElement();
                        }

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        public Mapper GetMapper(MAPIUtils utils, object mapiobj)
        {
            return new Mapper(this, utils, mapiobj);
        }


        public class Mapper : IDisposable
        {
            private MAPIUtils _utils;
            private object _mapiobj;
            private bool _modified;
            private readonly Dictionary<int, INamedProperty> _propTagMapping;
            private readonly PropertyTagMappings _owner;

            internal Mapper(PropertyTagMappings owner, MAPIUtils utils, object mapiobj)
            {
                _owner = owner;
                _utils = utils;
                _mapiobj = mapiobj;
                // empty mapping signature means opened external msg file, so do not cache its properties
                string signature = _utils.HrArrayToString(_utils.HrGetOneProp(mapiobj, PropertyIds.PR_MAPPING_SIGNATURE) ?? new byte[0]);
                if (!string.IsNullOrEmpty(signature) && !owner._propTagMappings.TryGetValue(signature, out _propTagMapping))
                {
                    _propTagMapping = new Dictionary<int, INamedProperty>();
                    _owner._propTagMappings.Add(signature, _propTagMapping);
                }
            }

            public INamedProperty GetNamesFromIDs(int proptag)
            {
                INamedProperty namedProp;
                if (_propTagMapping == null)
                {
                    namedProp = _utils.GetNamesFromIDs(_mapiobj, proptag);
                }
                else if (!_propTagMapping.TryGetValue(proptag, out namedProp))
                {
                    NamedProperty np = _utils.GetNamesFromIDs(_mapiobj, proptag);
                    if (np != null)
                    {
                        namedProp = new CachedNamedProperty(np.GUID, np.ID);
                        _propTagMapping.Add(proptag, namedProp);
                        _modified = true;
                    }
                }
                return namedProp;
            }

            void IDisposable.Dispose()
            {
                if (_modified)
                {
                    _owner.Save();
                    _modified = false;
                }
                _mapiobj = null;
                _utils = null;
            }
        }


        private class CachedNamedProperty : INamedProperty
        {
            internal CachedNamedProperty(string guid, object id)
            {
                GUID = guid;
                ID = id;
            }

            public string GUID { get; private set; }
            public object ID { get; private set; }
        }
    }
}
