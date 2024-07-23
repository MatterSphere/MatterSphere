using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Fwbs.Documents
{

    public sealed class OfficeXmlDocument : IRawDocument, ICustomPropertiesDocument, IDisposable
    {
        private IOfficeXmlParser parser;

        #region Constructors

        public OfficeXmlDocument()
        {
            parser = CreateParser();
        }

        #endregion

        #region Methods

        private static IOfficeXmlParser CreateParser()
        {
            Type type = Type.GetType("Fwbs.Documents.PackageOfficeXmlParser, Fwbs.Documents.PackageProperties", false);

            if (type == null)
            {
                return CreateFallbackParser();
            }

            try
            {
                return (IOfficeXmlParser)Activator.CreateInstance(type);
            }
            catch (TypeLoadException)
            {
                return CreateFallbackParser();
            }
        }

        private static IOfficeXmlParser CreateFallbackParser()
        {
            Type type = Type.GetType("Fwbs.Documents.JZipOfficeXmlParser, Fwbs.Documents.JZipPackageProperties", true);
            return (IOfficeXmlParser)Activator.CreateInstance(type);
        }

        #endregion


        #region ICustomPropertiesDocument Members

        public void Open(FileInfo file)
        {
            if (!IsOpen)
            {
                if (file == null)
                    throw new ArgumentNullException("file");

                if (!System.IO.File.Exists(file.FullName))
                    throw new System.IO.FileNotFoundException("", file.FullName);

                try
                {
                    parser.Open(file);
                }
                catch(Exception ex)
                {
                    throw new IOException(ex.Message, ex);
                }
            }
        }

        public void Save()
        {
            if (!IsOpen)
                throw new FileClosedException();

            parser.Save();

        }

        public void Close()
        {
            parser.Close();
        }

        public bool IsOpen
        {
            get { return parser.IsOpen; }
        }

        public void ReadCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (!IsOpen)
                throw new FileClosedException();

            XmlNodeList nodelist = parser.CustomPropertiesXml.SelectNodes("d:Properties/d:property", parser.NamespaceManager);

            foreach (XmlElement node in nodelist)
            {
                //  You found the node. Now check its type.
                if (node.HasChildNodes)
                {
                    XmlNode valueNode = node.ChildNodes[0];
                    if (valueNode != null)
                    {
                        OfficeXmlPropertyConverter conv = new OfficeXmlPropertyConverter();
                        object val = conv.FromSource(valueNode.InnerText, valueNode.Name);

                        try
                        {
                            string name = node.Attributes["name"]?.Value;
                            CustomProperty prop = properties.Add(null, name);
                            prop.Value = val;
                        }
                        catch (ArgumentException)
                        {
                            //Not a valid property name.
                        }
                    }
                }
            }

            properties.Accept();
        }

        public void WriteCustomProperties(CustomPropertyCollection properties)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");


            if (!IsOpen)
                throw new FileClosedException();

            OfficeXmlPropertyConverter conv = new OfficeXmlPropertyConverter();

            foreach (CustomProperty prop in properties)
            {
                string search = string.Format(CultureInfo.InvariantCulture, "d:Properties/d:property[translate(@name, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')='{0}']", prop.Name.ToUpperInvariant());

                if (prop.IsDeleted)
                {
                    XmlNode node = parser.CustomPropertiesXml.SelectSingleNode(search, parser.NamespaceManager);
                    if (node != null && node.ParentNode != null)
                        node.ParentNode.RemoveChild(node);
                }
                else if(prop.HasChanged)
                {
                    Type actualtype = conv.ConvertType(prop.Value.GetType());
                    string val = conv.ToSource(prop.Value);
                    string type = conv.ToSourceType(actualtype);
                   
                    XmlNode node = parser.CustomPropertiesXml.SelectSingleNode(search, parser.NamespaceManager);

                    //If it already exists.

                    if (node != null)
                    {
                        if (node.HasChildNodes)
                        {
                            XmlNode valueNode = node.ChildNodes[0];
                            if (valueNode != null)
                            {
                                string typeName = valueNode.Name;
                                if (type == typeName)
                                {
                                    //  The types are the same. 
                                    //  Replace the value of the node.
                                    valueNode.InnerText = val;
                                    //  If the property existed, and its type
                                    //  has not changed, you are finished.

                                }
                                else
                                {
                                    //  Types are different. Delete the node
                                    //  and clear the node variable.
                                    if (node.ParentNode != null)
                                        node.ParentNode.RemoveChild(node);
                                    node = null;
                                }
                            }
                        }


                    }


                    if (!String.IsNullOrEmpty(type))
                    {
                        //If it does not exist.
                        if (node == null)
                        {
                            string pidValue = "2";

                            XmlNode propertiesNode = parser.CustomPropertiesXml.DocumentElement;
                            if (propertiesNode != null && propertiesNode.HasChildNodes)
                            {
                                XmlNode lastNode = propertiesNode.LastChild;
                                if (lastNode != null && lastNode.Attributes != null)
                                {
                                    XmlAttribute pidAttr = lastNode.Attributes["pid"];
                                    if (pidAttr != null)
                                    {
                                        pidValue = pidAttr.Value;
                                        //  Increment pidValue, so that the new property
                                        //  gets a pid value one higher. This value should be 
                                        //  numeric, but it never hurt so to confirm.
                                        int value = 0;
                                        if (int.TryParse(pidValue, out value))
                                        {
                                            pidValue = Convert.ToString(value + 1, CultureInfo.InvariantCulture);
                                        }
                                    }
                                }
                            }

                            node = parser.CustomPropertiesXml.CreateElement("property", OfficeXmlNamespaces.CUSTOM_PROPERTIES_SCHEMA);
                            node.Attributes.Append(parser.CustomPropertiesXml.CreateAttribute("name"));
                            node.Attributes["name"].Value = prop.Name;

                            node.Attributes.Append(parser.CustomPropertiesXml.CreateAttribute("fmtid"));
                            node.Attributes["fmtid"].Value = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";

                            node.Attributes.Append(parser.CustomPropertiesXml.CreateAttribute("pid"));
                            node.Attributes["pid"].Value = pidValue;

                            XmlNode valueNode = parser.CustomPropertiesXml.CreateElement(type, OfficeXmlNamespaces.CUSTOM_VTYPE_SCHEMA);
                            valueNode.InnerText = val;
                            node.AppendChild(valueNode);
                            parser.CustomPropertiesXml.DocumentElement.AppendChild(node);
                        }
                    }
                }


            }
        }


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }


}