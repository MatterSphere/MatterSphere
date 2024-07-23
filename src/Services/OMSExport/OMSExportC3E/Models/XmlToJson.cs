using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    static class XmlToJson
    {
        public static string ClientRequest(string xmlDataInput, FindContactAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "Client.xslt");
            if (replace != null)
            {
                json = json.Replace("%ENTITY%", replace.EntIndex);
                json = json.Replace("%INVOICESITE%", replace.SiteIndex);
            }
            return json;
        }

        public static string ClientRequest(string xmlDataInput, FindClientAttribute replace)
        {
            string json = Transform(xmlDataInput, "Client.xslt");
            if (replace != null)
            {
                json = json.Replace("%CLIENTID%", replace.ClientID);
                json = json.Replace("%CLIDATEID%", replace.CliDateID);
            }
            return json;
        }

        public static string MatterRequest(string xmlDataInput, FindClientAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "Matter.xslt");
            if (replace != null)
            {
                json = json.Replace("%CLIENT%", replace.ClientIndex);
            }
            return json;
        }

        public static string MatterRequest(string xmlDataInput, FindMatterAttribute replace)
        {
            string json = Transform(xmlDataInput, "Matter.xslt");
            if (replace != null)
            {
                json = json.Replace("%MATTERID%", replace.MatterID);
                json = json.Replace("%MATTDATEID%", replace.MattDateID);
            }
            return json;
        }

        public static string OrganizationContactRequest(string xmlDataInput, FindContactAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "OrgContact.xslt");
            if (replace != null)
            {
                json = json.Replace("%ENTITYID%", replace.EntityID);
                json = json.Replace("%RELATEID%", replace.RelateID);
                json = json.Replace("%SITEID%", replace.SiteID);
            }
            return json;
        }

        public static string PersonalContactRequest(string xmlDataInput, FindContactAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "PersContact.xslt");
            if (replace != null)
            {
                json = json.Replace("%ENTITYID%", replace.EntityID);
                json = json.Replace("%RELATEID%", replace.RelateID);
                json = json.Replace("%SITEID%", replace.SiteID);
            }
            return json;
        }

        public static string TimeRequest(string xmlDataInput, FindMatterAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "Time.xslt");
            if (replace != null)
            {
                json = json.Replace("%MATTER%", replace.MattIndex);
            }
            return json;
        }

        public static string CostCardRequest(string xmlDataInput, FindMatterAttribute replace = null)
        {
            string json = Transform(xmlDataInput, "CostCard.xslt");
            if (replace != null)
            {
                json = json.Replace("%MATTER%", replace.MattIndex);
            }
            return json;
        }

        public static string NBISearchRequest(string xmlDataInput)
        {
            string json = Transform(xmlDataInput, "NBISearch.xslt");
            return json;
        }

        private static string Transform(string xmlInput, string xsltResourceName)
        {
            var xDoc = new XDocument();

            using (var xsltStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("FWBS.OMS.OMSEXPORT.XSLT." + xsltResourceName))
            {
                using (var xsltReader = XmlReader.Create(xsltStream))
                {
                    var transformer = new XslCompiledTransform();
                    transformer.Load(xsltReader);

                    using (var strReader = new StringReader(xmlInput))
                    {
                        using (var xmlReader = XmlReader.Create(strReader))
                        {
                            using (var xdocWriter = xDoc.CreateWriter())
                            {
                                transformer.Transform(xmlReader, xdocWriter);
                            }
                        }
                    }
                }
            }

            string json = JsonConvert.SerializeXNode(xDoc);
            return json;
        }
    }
}
