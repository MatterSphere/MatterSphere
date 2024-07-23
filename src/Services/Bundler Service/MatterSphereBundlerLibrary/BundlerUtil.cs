using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MatterSphereBundlerLibrary
{
    public class BundlerUtil
    {
        private const string SUPPORTED_FILE_EXTENSIONS = ".doc|.docx|.docm|.rtf|.xls|.xlsx|.xlsm|.msg|.png|.gif|.bmp|.jpg|.jpeg|.tiff|.tif|.ppt|.pptx|.pptm|.pdf";

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding { get { return Encoding.UTF8; } }
        }

        public static bool IsExtensionSupported(string extension)
        {
            return SUPPORTED_FILE_EXTENSIONS.Contains(extension.ToLowerInvariant());
        }

        /// <summary>
        /// Class To XML String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myClass"></param>
        /// <returns></returns>
        public static string ClassToXMLString<T>(T myClass)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringWriter writer = new Utf8StringWriter())
                {
                    serializer.Serialize(writer, myClass);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Class To XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// XML String To Class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XMLStringToClass<T>(string xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xml))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XML To Class Error: " + ex.Message);
            }
        }
    }
}
