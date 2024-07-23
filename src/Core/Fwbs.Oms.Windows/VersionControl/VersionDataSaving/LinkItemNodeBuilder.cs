using System.Xml;

namespace FWBS.OMS.UI.Windows
{
    public class LinkItemNodeBuilder
    {
        VersionDataXMLHeader versionDataXMLHeader;


        public LinkItemNodeBuilder(VersionDataXMLHeader versionDataXMLHeader)
        {
            this.versionDataXMLHeader = versionDataXMLHeader;
        }


        public XmlNode BuildLinkItemNode(LinkedItem linkItem)
        {
            var linkItemNode = versionDataXMLHeader.CreateElement("LinkItem");
            PopulateLinkedItemElementWithData(linkItemNode, "Code", linkItem.Code);
            PopulateLinkedItemElementWithData(linkItemNode, "Version", linkItem.Version.ToString());
            PopulateLinkedItemElementWithData(linkItemNode, "Table", linkItem.Destination);
            return linkItemNode;
        }


        private void PopulateLinkedItemElementWithData(XmlNode linkedItemNode, string elementName, string value)
        {
            var element = versionDataXMLHeader.CreateElement(elementName);
            element.InnerText = value;
            linkedItemNode.AppendChild(element);
        }


        public void AddLinkItemNodeToVersionHeaderXML(XmlNode linkItemNode)
        {
            var linkedItemsNode = versionDataXMLHeader.GetElementsByTagName("LinkedItems");
            linkedItemsNode[0].AppendChild(linkItemNode);
        }
    }
}
