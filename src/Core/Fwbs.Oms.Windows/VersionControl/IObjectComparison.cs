using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using FWBS.OMS.Data;


namespace FWBS.OMS.UI.Windows
{
    public interface IObjectComparison
    {
        string xmlFileLocation { get; }
        string code { get; set; }
        string type { get; }
        int image { get; }

        void RunComparisonProcess();

        DataTable ExecuteDataGather();
    }


    public abstract class BaseObjectComparer : IObjectComparison
    {
        protected string _code;
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

        protected string _type;
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        protected string _sql;
        public string sql
        {
            get { return _sql; }
            set { _sql = value; }
        }

        protected int _image;
        public int image
        {
            get { return _image; }
            set { _image = value; }
        }

        protected string _comparesql;
        public string compareSQL
        {
            get { return _comparesql; }
            set { _comparesql = value; }
        }

        protected DataTable _dtversions;
        public DataTable dtVersions
        {
            get { return _dtversions; }
            set { _dtversions = value; }
        }

        protected int _currentversion;
        protected int _selectedversion;

        internal List<IDataParameter> parList;
        internal IConnection connection;

        protected string _xmlFileLocation;
        public string xmlFileLocation
        {
            get { return _xmlFileLocation; }

        }
        protected string xmlFieldName;
        protected string currentXML;
        protected string currentFileName;
        protected string selectedXML;
        protected string selectedFileName;
        protected string formattedXML;

        string exepath = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("CompareTool"));
        string command = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("CompareCommand"));


        public BaseObjectComparer(string Code, int currentversion, int selectedversion)
        {
            _code = Code;
            _currentversion = currentversion;
            _selectedversion = selectedversion;
            parList = new List<IDataParameter>();
            connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            _xmlFileLocation = Convert.ToString(FWBS.OMS.Session.CurrentSession.GetSpecificData("CompareLocation"));
        }

        public abstract void RunComparisonProcess();

        protected bool CheckCurrentVersionIsCheckedIn(int currentversion)
        {
            PopulateDTVersions();
            if (_dtversions.Rows.Count < 2)
            {
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("NOVERSCOMP", "No versions to compare current version to.", "").Text);
                return false;
            }

            DataRow[] rows = _dtversions.Select("Version = " + currentversion);
            if(rows.Length == 0)
                return false;
            
            return true;
        }

        private void PopulateDTVersions()
        {
            parList.Clear();
            parList.Add(connection.CreateParameter("code", _code));
            _dtversions = ReturnDataBaseData(_comparesql, parList);
        }

        protected void ActivateComparisonSoftware(string current, string selected)
        {
            string comparetool = exepath;
            if (CheckCompareToolFilePath(ref comparetool)
                && Directory.Exists(_xmlFileLocation) 
                && !string.IsNullOrWhiteSpace(command))
                System.Diagnostics.Process.Start(comparetool, string.Format(command, current, selected));
            else
                CompareConfigurationErrorMessage();
        }

        protected void CompareConfigurationErrorMessage()
        {
            System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("IOCCONFIGCHECK"), ResourceLookup.GetLookupText("IOCMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool CheckCompareToolFilePath(ref string path, string RelativePath = "", string Extension = "")
        {
            if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            {
                try
                {
                    if (!Path.IsPathRooted(path))
                    {
                        if (string.IsNullOrEmpty(RelativePath))
                            path = Path.GetFullPath(path);
                        else
                            path = Path.Combine(RelativePath, path);
                    }
                    FileInfo fileInfo = new FileInfo(path);
                    bool throwEx = fileInfo.Length == -1;
                    throwEx = fileInfo.IsReadOnly;

                    if (!string.IsNullOrEmpty(Extension))
                    {
                        if (Path.GetExtension(path).Equals(Extension, StringComparison.InvariantCultureIgnoreCase))
                        {
                            path = path.Trim();
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return true;
                }
                catch (ArgumentNullException) { }
                catch (System.Security.SecurityException) { }
                catch (ArgumentException) { }
                catch (UnauthorizedAccessException) { }
                catch (PathTooLongException) { }
                catch (NotSupportedException) { }
                catch (FileNotFoundException) { }
                catch (IOException) { }
                catch (Exception) { }
            }
            else
            {
            }
            return false;
        }

        protected void CheckForInvalidCharactersInXML(string xml, string tagname, out string validatedxml)
        {
            string validatedsource = "";
            string opentag = "<" + tagname + ">";
            int from = xml.IndexOf(opentag);
            int to = xml.LastIndexOf("</" + tagname + ">");
            string source = xml.Substring((from + opentag.Length), (to - (from + opentag.Length)));
            if (source.IndexOf("<>") != -1)
                validatedsource = source.Replace("<>", " (is not equal to) ").Replace("< >", " (is not equal to) ");
            else
                validatedsource = source.Replace("<", " (is less than) ").Replace(">", " (is greater than) ");
            validatedxml = xml.Replace(source, validatedsource);
        }  

        protected void GatherComparisonData()
        {
            DataRow[] current = _dtversions.Select("Version = " + _currentversion);
            currentXML = Convert.ToString(current[0][xmlFieldName]);
            currentFileName = "MSComp-" + _code + "v" + Convert.ToString(_currentversion);

            DataRow[] selected = _dtversions.Select("Version = " + _selectedversion);
            selectedXML = Convert.ToString(selected[0][xmlFieldName]);
            selectedFileName = "MSComp-" + _code + "v" + Convert.ToString(_selectedversion);
        }

        public virtual DataTable ExecuteDataGather()
        {
            System.Data.DataTable dt = null;
            parList.Add(connection.CreateParameter("code", _code));
            dt = ReturnDataBaseData(sql, parList);
            return dt;
        }

        internal System.Data.DataTable ReturnDataBaseData(string sql, List<IDataParameter> parlist)
        {
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return dt;
        }
    }


    public class EnquiryFormComparer : BaseObjectComparer
    {
        string completeCurrentXML;
        string completeSelectedXML;

        public EnquiryFormComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _image = 7;
            _type = "EnquiryForm";
            _sql = " select enqVersion as Version, Created, CreatedBy from dbEnquiry where enqCode = @code";
            _comparesql = "select * from dbEnquiryVersionData where Code = @code order by Version desc";
        }

        public override void RunComparisonProcess()
        {
            if (base.CheckCurrentVersionIsCheckedIn(_currentversion))
            {
                xmlFieldName = "dbEnquiry";
                StripOutEnquiryConfig(xmlFieldName);

                xmlFieldName = "dbEnquiryQuestion";
                StripOutEnquiryConfig(xmlFieldName);

                xmlFieldName = "dbEnquiryPage";
                StripOutEnquiryConfig(xmlFieldName);

                base.ActivateComparisonSoftware(BuildXMLFile(completeCurrentXML, currentFileName), BuildXMLFile(completeSelectedXML, selectedFileName));
            }
        }

        private void StripOutEnquiryConfig(string xmlField)
        {
            base.GatherComparisonData();
            DecodeXML(xmlField, currentXML, "current");
            DecodeXML(xmlField, selectedXML, "selected");
        }

        private void DecodeXML(string xmlField, string XML, string completeXML)
        {
            GetAllXElements(XElement.Parse(RemoveSchema(StripXMLDeclarations(HttpUtility.HtmlDecode(XML)))), xmlField, completeXML);
        }

        private void AppendToCurrentCompleteXML(string element)
        {
            if (element != null)
            {
                if (string.IsNullOrWhiteSpace(completeCurrentXML))
                    completeCurrentXML += element;
                else
                    completeCurrentXML += "\n" + element;
            }
        }

        private void AppendToSelectedCompleteXML(string element)
        {
            if (element != null)
            {
                if (string.IsNullOrWhiteSpace(completeSelectedXML))
                    completeSelectedXML += element;
                else
                    completeSelectedXML += "\n" + element;
            }
        }

        private string BuildXMLFile(string completeXML, string filename)
        {
            File.WriteAllText(Path.Combine(xmlFileLocation, filename + ".xml"), completeXML);
            return Path.Combine(xmlFileLocation, filename + ".xml");
        }

        private string StripXMLDeclarations(string xml)
        {
            return Regex.Replace(xml,@"<\?xml.*\?>","");
        }

        private string RemoveSchema(string content)
        {
            int from = content.IndexOf("<xs:schema");
            int to = content.LastIndexOf("</xs:schema>") + "</xs:schema>".Length;
            return content.Remove(from, to - from);
        }

        private void GetAllXElements(XElement completeElement, string xmlField, string completeXML)
        {
            IEnumerable<XElement> elements =
                from el in completeElement.Descendants(xmlField)
                select el;
            foreach (XElement el in elements)
            {
                if (completeXML == "current")
                    AppendToCurrentCompleteXML(el.ToString(SaveOptions.None));
                else
                    AppendToSelectedCompleteXML(el.ToString(SaveOptions.None));
            }
        }

        public override DataTable ExecuteDataGather()
        {
            return base.ExecuteDataGather();
        }
    }


    public class SearchListComparer : BaseObjectComparer
    {
        public SearchListComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _image = 5; 
            _type = "SearchList";
            xmlFieldName = "dbSearchListConfig";
            _sql = "select schVersion as Version, Created, CreatedBy from dbSearchListConfig where schCode = @code";
            _comparesql = "select * from dbSearchListVersionData where Code = @code order by Version desc";
        }

        public override void RunComparisonProcess()
        {
            if (base.CheckCurrentVersionIsCheckedIn(_currentversion))
            {
                base.GatherComparisonData();
                string currentFile = BuildXMLFile(currentXML, currentFileName);
                string selectedFile = BuildXMLFile(selectedXML, selectedFileName);
                base.ActivateComparisonSoftware(currentFile, selectedFile);
            }
        }

        public string BuildXMLFile(string xml, string filename)
        {
            string decodedXML = DecodeXML(xml);
            var completeXML = XElement.Parse(decodedXML);
            var searchlistconfig = completeXML.Element(xmlFieldName);
            formattedXML = searchlistconfig.ToString(SaveOptions.None);
            File.WriteAllText(Path.Combine(xmlFileLocation, filename + ".xml"), formattedXML);
            return Path.Combine(xmlFileLocation, filename + ".xml");
        }

        private string DecodeXML(string xml)
        {
            string validatedxml;
            base.CheckForInvalidCharactersInXML(HttpUtility.HtmlDecode(xml), "schSourceCall", out validatedxml);
            return validatedxml;
        }

        public override DataTable ExecuteDataGather()
        {
            return base.ExecuteDataGather();
        }
    }


    public class ScriptComparer : BaseObjectComparer
    {
        public ScriptComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _image = 9; 
            _type = "Script";
            xmlFieldName = "dbScript";
            _sql = "select scrVersion as Version, Created, CreatedBy from dbScript where scrCode = @code";
            _comparesql = "select * from dbScriptVersionData where Code = @code order by Version desc";
        }

        public override void RunComparisonProcess()
        {
            if (base.CheckCurrentVersionIsCheckedIn(_currentversion))
            {
                GatherComparisonData();
                string currentFile = BuildXMLFile(currentXML, currentFileName);
                string selectedFile = BuildXMLFile(selectedXML, selectedFileName);
                base.ActivateComparisonSoftware(currentFile, selectedFile);
            }
        }

        public string BuildXMLFile(string xml, string filename)
        {
            string content = "";    
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            XmlNodeList nodes = xdoc.GetElementsByTagName("scrText");
            XmlDocument xcode = new XmlDocument();
            xcode.LoadXml(Convert.ToString(nodes[0].InnerText));

            XmlNode nd = xcode.SelectSingleNode("/config/script/units");
            if (nd != null)
            {
                content = Deserialise(nd.InnerText);
                using (StreamWriter sw = new StreamWriter(Path.Combine(xmlFileLocation, filename + ".xml")))
                {
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            return Path.Combine(xmlFileLocation, filename + ".xml");
        }

        private string Deserialise(string data)
        {
            byte[] buffer = Convert.FromBase64String(data);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding(false);
            data = enc.GetString(buffer);
            return data;
        }	
    }


    public class DataListComparer : BaseObjectComparer
    {
        public DataListComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _image = 18;
            _type = "DataList";
            xmlFieldName = "dbEnquiryDataList";
            _sql = "select enqDLVersion as Version, Created, CreatedBy from dbEnquiryDataList d where d.enqTable = @code order by enqDLVersion desc";
            _comparesql = "select * from dbDataListVersionData where Code = @code order by Version desc";
        }

        public override void RunComparisonProcess()
        {
            if (base.CheckCurrentVersionIsCheckedIn(_currentversion))
            {
                base.GatherComparisonData();
                string currentFile = BuildXMLFile(currentXML, currentFileName);
                string selectedFile = BuildXMLFile(selectedXML, selectedFileName);
                base.ActivateComparisonSoftware(currentFile, selectedFile);
            }
        }

        public string BuildXMLFile(string xml, string filename)
        {
            string decodeXML = DecodeXML(xml);
            var completeXML = XElement.Parse(decodeXML);
            var datalistconfig = completeXML.Element(xmlFieldName);
            formattedXML = datalistconfig.ToString(SaveOptions.None);
            File.WriteAllText(Path.Combine(xmlFileLocation, filename + ".xml"), formattedXML);
            return Path.Combine(xmlFileLocation, filename + ".xml");
        }

        private string DecodeXML(string xml)
        {
            string validatedxml;
            base.CheckForInvalidCharactersInXML(HttpUtility.HtmlDecode(xml), "enqCall", out validatedxml);
            return validatedxml;
        }

        public override DataTable ExecuteDataGather()
        {
            return base.ExecuteDataGather();
        }
    }


    public class PrecedentComparer : BaseObjectComparer
    {
        public PrecedentComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _type = "Precedent";
            _sql = "select Created, CreatedBy, (select verLabel from dbPrecedentVersion pv inner join dbPrecedents p on p.precPath = pv.verToken where p.PrecID = @code) as Version from dbPrecedents where precID = @code";
            _image = 34;
        }


        public override void RunComparisonProcess()
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("CANNOTCOMP", "Cannot compare", "").Text);
        }


        public override DataTable ExecuteDataGather()
        {
            return base.ExecuteDataGather();
        }
    }


    public class FileManagementComparer : BaseObjectComparer
    {
        public FileManagementComparer(string Code, int currentversion = 0, int selectedversion = 0) : base(Code, currentversion, selectedversion)
        {
            _image = 6;
            _type = "FileManagement";
            xmlFieldName = "dbFileManagementApplication";
            _sql = "select appVer as Version, Created, CreatedBy from dbFileManagementApplication where appCode = @code";
            _comparesql = "select * from dbFileManagementVersionData where Code = @code order by Version desc";
        }

        public override void RunComparisonProcess()
        {
            if (base.CheckCurrentVersionIsCheckedIn(_currentversion))
            {
                base.GatherComparisonData();
                string currentFile = BuildXMLFile(currentXML, currentFileName);
                string selectedFile = BuildXMLFile(selectedXML, selectedFileName);
                base.ActivateComparisonSoftware(currentFile, selectedFile);
            }
        }

        public string BuildXMLFile(string xml, string filename)
        {
            string decodedXML = DecodeXML(xml);
            var completeXML = XElement.Parse(decodedXML);
            var filemgmtconfig = completeXML.Element(xmlFieldName);
            formattedXML = filemgmtconfig.ToString(SaveOptions.None);
            File.WriteAllText(Path.Combine(xmlFileLocation, filename + ".xml"), formattedXML);
            return Path.Combine(xmlFileLocation, filename + ".xml");
        }

        private string DecodeXML(string xml)
        {
            return HttpUtility.HtmlDecode(xml);
        }
    }


    public static class ObjectComparisonFactory
    {
        public static IObjectComparison CreateIObjectComparison(string code, string type, int versionA = 0, int versionB = 0)
        {
            switch (type.ToString())
            {
                case "EnquiryForm":
                    return new EnquiryFormComparer(code, versionA, versionB);
                case "DataList":
                    return new DataListComparer(code, versionA, versionB);
                case "Script":
                    return new ScriptComparer(code, versionA, versionB);   
                case "SearchList":
                    return new SearchListComparer(code, versionA, versionB);    
                case "Precedent":
                    return new PrecedentComparer(code, versionA, versionB);
                case "FileManagement":
                    return new FileManagementComparer(code, versionA, versionB);
            }
            return null;
        }
    }
}
