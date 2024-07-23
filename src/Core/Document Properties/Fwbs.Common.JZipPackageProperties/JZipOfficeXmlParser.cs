using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Fwbs.Documents
{
    public class JZipOfficeXmlParser : IOfficeXmlParser
    {
        private class Info
        {
            public XmlDocument Xml;
            public XmlNamespaceManager Manager;
            public ZipArchiveEntry Entry;
        }

        private ZipArchive package;

        private Dictionary<string, Info> knownentries = new Dictionary<string, Info>();
        private Info customPropsInfo;
        private Info contentTypesInfo;
        private Info relationshipsInfo;

        private const string customPropsUri = "docProps/custom.xml";
        private const string contentTypesUri = "[Content_Types].xml";
        private const string relationshipsUri = "_rels/.rels";

        public JZipOfficeXmlParser()
        {
        }

        public bool IsOpen
        {
            get
            {
                return package != null;
            }
        }

        public void Open(System.IO.FileInfo file)
        {
            if (!IsOpen)
            {
                FileStream fs = null;
                try
                {   // ZipArchive will own file stream if initialization succeed and close it on Dispose
                    fs = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                    package = new ZipArchive(fs, ZipArchiveMode.Update, false);
                }
                catch
                {
                    if (fs != null)
                        fs.Dispose();

                    throw;
                }
                finally
                {
                    fs = null;
                }

                //  Manage namespaces to perform Xml XPath queries.
                customPropsInfo = new Info();

                NameTable nt = new NameTable();
                customPropsInfo.Manager = new XmlNamespaceManager(nt);
                customPropsInfo.Manager.AddNamespace("d", OfficeXmlNamespaces.CUSTOM_PROPERTIES_SCHEMA);
                customPropsInfo.Manager.AddNamespace("vt", OfficeXmlNamespaces.CUSTOM_VTYPE_SCHEMA);
                customPropsInfo.Entry = package.GetEntry(customPropsUri);

                // Next code block goes here.
                if (customPropsInfo.Entry == null)
                {
                    customPropsInfo.Xml = new XmlDocument(nt);
                    customPropsInfo.Xml.AppendChild(customPropsInfo.Xml.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

                    //  Set up the rudimentary custom part.
                    XmlNode rootNode = customPropsInfo.Xml.CreateElement("Properties", OfficeXmlNamespaces.CUSTOM_PROPERTIES_SCHEMA);
                    rootNode.Attributes.Append(customPropsInfo.Xml.CreateAttribute("xmlns:vt"));
                    rootNode.Attributes["xmlns:vt"].Value = OfficeXmlNamespaces.CUSTOM_VTYPE_SCHEMA;

                    customPropsInfo.Xml.AppendChild(rootNode);
                }
                else
                {
                    customPropsInfo.Xml = GetXml(customPropsInfo.Entry, nt);
                }

                knownentries.Add(customPropsUri, customPropsInfo);

                contentTypesInfo = new Info();
                contentTypesInfo.Entry = package.GetEntry(contentTypesUri);
                if (contentTypesInfo.Entry == null)
                    throw new FileLoadException(String.Format(CultureInfo.InvariantCulture, "Invalid format. {0} not found.", contentTypesUri));

                NameTable nt2 = new NameTable();
                contentTypesInfo.Manager = new XmlNamespaceManager(nt2);
                contentTypesInfo.Manager.AddNamespace("d", OfficeXmlNamespaces.CONTENT_TYPES_SCHEMA);
                contentTypesInfo.Xml = GetXml(contentTypesInfo.Entry, nt2);
                knownentries.Add(contentTypesUri, contentTypesInfo);

                relationshipsInfo = new Info();
                relationshipsInfo.Entry = package.GetEntry(relationshipsUri);
                if (relationshipsInfo.Entry == null)
                    throw new FileLoadException(String.Format(CultureInfo.InvariantCulture, "Invalid format. {0} not found.", relationshipsUri));

                NameTable nt3 = new NameTable();
                relationshipsInfo.Manager = new XmlNamespaceManager(nt3);
                relationshipsInfo.Manager.AddNamespace("d", OfficeXmlNamespaces.RELATIONSHIPS_SCHEMA);
                relationshipsInfo.Xml = GetXml(relationshipsInfo.Entry, null);
                knownentries.Add(relationshipsUri, relationshipsInfo);
            }
        }

        private void CreateRelationship()
        {
            XmlNode node = contentTypesInfo.Xml.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, @"/d:Types/d:Override[@PartName='/{0}']", customPropsUri), contentTypesInfo.Manager);
            if (node == null)
            {
                XmlElement el = contentTypesInfo.Xml.CreateElement("Override", OfficeXmlNamespaces.CONTENT_TYPES_SCHEMA);
                XmlAttribute attr = contentTypesInfo.Xml.CreateAttribute("PartName");
                attr.Value = "/" + customPropsUri;
                XmlAttribute attr2 = contentTypesInfo.Xml.CreateAttribute("ContentType");
                attr2.Value = @"application/vnd.openxmlformats-officedocument.custom-properties+xml";
                el.Attributes.Append(attr);
                el.Attributes.Append(attr2);
                contentTypesInfo.Xml.DocumentElement.AppendChild(el);
            }

            //<Relationship Id="rId4" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/custom-properties" Target="docProps/custom.xml" /> 
            node = relationshipsInfo.Xml.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, @"/d:Relationships/d:Relationship[@Target='{0}']", customPropsUri), relationshipsInfo.Manager);
            if (node == null)
            {
                int ctr = 1;
                while (relationshipsInfo.Xml.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, @"/d:Relationships/d:Relationship[@Id='rId{0}']", ctr), relationshipsInfo.Manager) != null)
                    ctr++;

                XmlElement el = relationshipsInfo.Xml.CreateElement("Relationship", OfficeXmlNamespaces.RELATIONSHIPS_SCHEMA);
                XmlAttribute attr = relationshipsInfo.Xml.CreateAttribute("Id");
                attr.Value = "rId" + ctr.ToString(CultureInfo.InvariantCulture);
                XmlAttribute attr2 = relationshipsInfo.Xml.CreateAttribute("Type");
                attr2.Value = OfficeXmlNamespaces.CUSTOM_PROPERTIES_RELATIONSHIP_TYPE;
                XmlAttribute attr3 = relationshipsInfo.Xml.CreateAttribute("Target");
                attr3.Value = customPropsUri;
                el.Attributes.Append(attr);
                el.Attributes.Append(attr2);
                el.Attributes.Append(attr3);
                relationshipsInfo.Xml.DocumentElement.AppendChild(el);
            }
        }


        private XmlDocument GetXml(ZipArchiveEntry ze, NameTable nt)
        {
            using (Stream stream = ze.Open())
            {
                //  Load the contents of the custom properties part into an XML document.
                XmlDocument xml = (nt == null) ? new XmlDocument() : new XmlDocument(nt);
                xml.Load(stream);
                return xml;
            }
        }

        public XmlNamespaceManager NamespaceManager
        {
            get
            {
                return customPropsInfo.Manager;
            }
        }

        public XmlDocument CustomPropertiesXml
        {
            get
            {
                return customPropsInfo.Xml;
            }
        }

        public void Close()
        {
            customPropsInfo = contentTypesInfo = relationshipsInfo = null;
            knownentries.Clear();

            if (package != null)
            {
                package.Dispose();
                package = null;
            }
        }

        public void Save()
        {
            //  Create the document's relationship to the 
            //  new custom properties part.
            CreateRelationship();

            foreach (var kvp in knownentries)
            {
                Info info = kvp.Value;
                if (info.Entry == null)
                {
                    info.Entry = package.CreateEntry(kvp.Key);
                }

                using (Stream stream = info.Entry.Open())
                {
                    info.Xml.Save(stream);
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
            knownentries = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}