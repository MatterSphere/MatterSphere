using System;

namespace Fwbs.Documents
{
    using System.IO;
    using System.IO.Packaging;
    using System.Xml;

    internal class PackageOfficeXmlParser : IOfficeXmlParser
    {
        #region Fields

        private Package package;
        private XmlNamespaceManager nsManager;
        private PackagePart customPropsPart;
        private XmlDocument customPropsDoc;
        private Uri customPropsUri = new Uri("/docProps/custom.xml", UriKind.Relative);

        #endregion

        #region IOfficeXmlParser Members

        public System.Xml.XmlDocument CustomPropertiesXml
        {
            get { return customPropsDoc; }
        }

        public System.Xml.XmlNamespaceManager NamespaceManager
        {
            get { return nsManager; }
        }

        #endregion

        #region IRawDocument Members

        public void Open(System.IO.FileInfo file)
        {
            if (!IsOpen)
            {

                package = Package.Open(file.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                //  Get the custom part (custom.xml). It may not exist.
                foreach (PackageRelationship relationship in package.GetRelationshipsByType(OfficeXmlNamespaces.CUSTOM_PROPERTIES_RELATIONSHIP_TYPE))
                {
                    Uri documentUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri);
                    customPropsPart = package.GetPart(documentUri);
                    //  There is only one custom properties part, 
                    // if it exists at all.
                    break;
                }

                //  Manage namespaces to perform Xml XPath queries.
                NameTable nt = new NameTable();
                nsManager = new XmlNamespaceManager(nt);
                nsManager.AddNamespace("d", OfficeXmlNamespaces.CUSTOM_PROPERTIES_SCHEMA);
                nsManager.AddNamespace("vt", OfficeXmlNamespaces.CUSTOM_VTYPE_SCHEMA);

                XmlNode rootNode = null;

                // Next code block goes here.
                if (customPropsPart == null)
                {
                    customPropsDoc = new XmlDocument(nt);
                    customPropsPart = package.CreatePart(customPropsUri, "application/vnd.openxmlformats-officedocument.custom-properties+xml");    //as per Alex H's fix
                    //  Set up the rudimentary custom part.
                    rootNode = customPropsDoc.CreateElement("Properties", OfficeXmlNamespaces.CUSTOM_PROPERTIES_SCHEMA);
                    rootNode.Attributes.Append(customPropsDoc.CreateAttribute("xmlns:vt"));
                    rootNode.Attributes["xmlns:vt"].Value = OfficeXmlNamespaces.CUSTOM_VTYPE_SCHEMA;

                    customPropsDoc.AppendChild(rootNode);
                    package.CreateRelationship(customPropsUri, TargetMode.Internal, OfficeXmlNamespaces.CUSTOM_PROPERTIES_RELATIONSHIP_TYPE);   //as per Alex H's fix
                    customPropsDoc.Save(customPropsPart.GetStream(FileMode.Create, FileAccess.Write));  //as per Alex H's fix
                }
                else
                {
                    //  Load the contents of the custom properties part 
                    //  into an XML document.

                    customPropsDoc = new XmlDocument(nt);
                    customPropsDoc.Load(customPropsPart.GetStream());
                    rootNode = customPropsDoc.DocumentElement;   //as per Alex H's fix 
                }

            }

        }

        public void Save()
        {
            if (customPropsPart == null)
            {

                //  The part does not exist. Create it now.
                customPropsPart = package.CreatePart(customPropsUri, "application/xml");


                //  Create the document's relationship to the 
                //  new custom properties part.
                package.CreateRelationship(customPropsUri, TargetMode.Internal, OfficeXmlNamespaces.CUSTOM_PROPERTIES_RELATIONSHIP_TYPE);

 
            }

            customPropsDoc.Save(customPropsPart.GetStream(FileMode.Create, FileAccess.Write));
        }

        public void Close()
        {
            IDisposable disp = package as IDisposable;
            if (disp != null)
                disp.Dispose();

            if (package != null)
                package.Close();

            package = null;
            nsManager = null;
            customPropsPart = null;
            customPropsDoc = null;
        }

        public bool IsOpen
        {
            get { return (package != null); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
