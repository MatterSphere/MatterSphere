using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace PackageUpgradeAnalyzer
{
    class XmlHelper
    {
        private readonly XmlDocument _xmlDoc;
        private readonly string _versionTag;

        public XmlHelper(string objectManifest, string versionTag)
        {
            _versionTag = versionTag;
            if (File.Exists(objectManifest))
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(objectManifest);
            }
        }

        public OmsObjectInfo GetOmsObjectInfo(string omsType, string code)
        {
            OmsObjectInfo info = null;
            if (_xmlDoc != null)
            {
                info = new OmsObjectInfo(omsType, code);

                string value = GetNodeValue(_versionTag);
                if (!string.IsNullOrEmpty(value))
                    info.Version = Convert.ToInt32(value, CultureInfo.InvariantCulture);

                value = GetNodeValue("Updated");
                if (!string.IsNullOrEmpty(value))
                    info.Updated = Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToUniversalTime();

                value = GetNodeValue("UpdatedBy");
                if (!string.IsNullOrEmpty(value))
                    info.UpdatedBy = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            }
            return info;
        }

        private string GetNodeValue(string tag)
        {
            return _xmlDoc.SelectSingleNode($"//{tag}")?.InnerText;
        }
    }
}
