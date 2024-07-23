using System.Xml;

namespace FWBS.OMS.UI.Windows
{
    public class VersionDataXMLHeader : XmlDocument
    {
        public VersionDataXMLHeader()
        {
            var versionDataNode = this.CreateElement("VersionData");
            this.AppendChild(versionDataNode);

            var linkItemsNode = this.CreateElement("LinkedItems");
            versionDataNode.AppendChild(linkItemsNode);
        }
    }
}
