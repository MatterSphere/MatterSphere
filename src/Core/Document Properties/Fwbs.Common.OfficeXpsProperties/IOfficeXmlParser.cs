using System;
using System.Xml;

namespace Fwbs.Documents
{
    public interface IOfficeXmlParser : IRawDocument, IDisposable
    {
        XmlDocument CustomPropertiesXml { get; }
        XmlNamespaceManager NamespaceManager { get; }
    }
}
