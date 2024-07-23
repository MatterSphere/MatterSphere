using System;
using System.Data;

namespace FWBS.OMS.Apps
{
    public sealed class RegisteredApplication
    {
        #region Fields

        private DataRow _data = null;
        private System.Collections.Generic.List<string> exts;
        private System.Xml.XmlDocument xml;

        #endregion

        #region Constructors

        internal RegisteredApplication(DataRow data)
        {
            _data = data;

            AddDefaultExtensions();
            BuildXml();
        }


        #endregion

        #region Methods

        private void SetExtraInfo(string fieldName, object val)
        {
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }
            _data[fieldName] = val;
        }
        private object GetExtraInfo(string fieldName)
        {
            object val = _data[fieldName];
            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
        }

        public Interfaces.IOMSApp CreateOMSApp()
        {
            return this.Type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null) as Interfaces.IOMSApp;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Handles(string extension)
        {
            if (String.IsNullOrEmpty(extension))
                return false;

            extension = extension.Trim('.').ToUpper();

            return HandledExtensions.Contains(extension);
        }

        
        private void BuildXml()
        {
            //Create the document if it does not already exist.
            if (xml == null)
                xml = new System.Xml.XmlDocument();

            xml.PreserveWhitespace = false;
            string xmldata = Convert.ToString(GetExtraInfo("appXML")).Trim();
            if (xmldata != String.Empty)
                xml.LoadXml(xmldata);

            System.Xml.XmlNode el_root = xml.SelectSingleNode("/Config");
            if (el_root == null)
            {
                xml.RemoveAll();
                el_root = xml.CreateElement("Config");
                xml.AppendChild(el_root);
            }

            System.Xml.XmlElement el_application = el_root.SelectSingleNode("Application") as System.Xml.XmlElement;
            if (el_application == null)
            {
                el_application = xml.CreateElement("Application");
                el_root.AppendChild(el_application);
            }

            System.Xml.XmlElement el_extensions = el_application.SelectSingleNode("Extensions") as System.Xml.XmlElement;
            if (el_extensions == null)
            {
                el_extensions = xml.CreateElement("Extensions");
                el_application.AppendChild(el_extensions);
            }


            ModifyExtensions(el_extensions);
        }


        private void AddDefaultExtensions()
        {
            switch (Code)
            {
                case "WORD":
                    AddExtension("doc");
                    AddExtension("dot");
                    AddExtension("rtf");
                    AddExtension("docx");    
                    break;
                case "OUTLOOK":
                    AddExtension("msg");
                    break;
                case "EXCEL":
                    AddExtension("xls");
                    AddExtension("xlsx");
                    break;

            }
        }

        private void AddExtension(string extension)
        {
            extension = extension.ToUpper();
            if (!HandledExtensions.Contains(extension))
                HandledExtensions.Add(extension);
        }

        private void RemoveExtension(string extension)
        {
            extension = extension.ToUpper();
            if (HandledExtensions.Contains(extension))
                HandledExtensions.Remove(extension);
        }


        private void ModifyExtensions(System.Xml.XmlElement element)
        {
            if (exts == null)
                exts = new System.Collections.Generic.List<string>();

            foreach (System.Xml.XmlElement el_ext in element.ChildNodes)
            {
                string ext = el_ext.InnerText.Trim();
                bool result;
                bool.TryParse(el_ext.GetAttribute("exclude"), out result);
                if (result)
                    RemoveExtension(ext);
                else
                    AddExtension(ext);

            }
        }


        #endregion

        #region Properties


        public short ID
        {
            get
            {
                return Convert.ToInt16(GetExtraInfo("appID"));
            }
        }

        public bool AutoPrint
        {
            get
            {
                bool? setting = Session.CurrentSession.CurrentUser.AutoPrintByApplication(this.GUID);
                if (setting == null)
                    setting = Session.CurrentSession.AutoPrintByApplication(this.GUID);

                if (setting != null)
                    return setting.Value;
                else
                {
                    FWBS.Common.ConfigSetting _config = new FWBS.Common.ConfigSetting(Convert.ToString(GetExtraInfo("appXML")).Trim());
                    return Convert.ToBoolean(_config.GetSetting("Config", "autoPrint", "True"));
                }
                
            }
        }


        public string Code
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appCode"));
            }
        }


        public string Name
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appName"));
            }
        }

        private Guid GUID
        {
            get
            {
                return new Guid(Convert.ToString(GetExtraInfo("appGuid")));
            }
        }

        public Type Type
        {
            get
            {
                if (Convert.ToString(GetExtraInfo("AppType")) == "")
                    return null;
                // Try and create and instantiate the object through the type name

                Type objtype;
                objtype = Session.CurrentSession.TypeManager.Load(Convert.ToString(GetExtraInfo("appType")));
                return objtype;

            }
        }

        public string Path
        {
            get
            {
                return Convert.ToString(GetExtraInfo("appPath"));
            }
        }

        /// <summary>
        /// Gets the default blank precedent for the application type.
        /// </summary>
        public Precedent BlankPrecedent
        {
            get
            {
                object id = GetExtraInfo("appBlankPrecedent");
                if (id == DBNull.Value)
                    return Precedent.GetPrecedent("DEFAULT", Convert.ToString(GetExtraInfo("appBlankPrecedentType")), "", "", "", Session.CurrentSession.DefaultCulture, "");
                else
                    return Precedent.GetPrecedent(Convert.ToInt64(id));
            }
        }

        public System.Collections.Generic.List<string> HandledExtensions
        {
            get
            {
                if (exts == null)
                    exts = new System.Collections.Generic.List<string>();
                return exts;
            }
        }

        #endregion

    }



}
