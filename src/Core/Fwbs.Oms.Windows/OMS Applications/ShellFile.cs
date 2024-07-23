using System;

namespace FWBS.OMS.UI.Windows
{
    public sealed class ShellFile : IDisposable
    {
        private System.IO.FileInfo _file = null;
        private System.IO.FileInfo[] _attachments = null;
        private System.Xml.XmlDocument _doc = null;
        private System.IO.OleFileInfo _olefile;

        private ShellFile()
        {
        }

        public ShellFile(System.IO.FileInfo file, params System.IO.FileInfo[] attachments)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            _file = file;
            _attachments = attachments;
            if (_attachments == null)
                _attachments = new System.IO.FileInfo[0];
            _olefile = new System.IO.OleFileInfo(_file.FullName);
        }

        public System.IO.FileInfo File
        {
            get
            {
                return _file;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(name))
                    return File.Name;
                else
                    return name;
            }
            set
            {
                name = value;
            }
        }

        public System.IO.FileInfo[] Attachments
        {
            get
            {
                if (_attachments.GetLength(0) == 0)
                    _attachments = new System.IO.FileInfo[0];
                return _attachments;
            }
        }

        public void SetProperty(string name, object value)
        {
            //Add the global prefix
            string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
            _olefile.SetProperty(globalname, value);
            SetXmlProperty(name, value);
        }

        private void SetXmlProperty(string name, object value)
        {
            try
            {
                RemoveXmlProperty(name);
                System.Xml.XmlElement vars = (System.Xml.XmlElement)Info.SelectSingleNode("/DOCUMENT/VARIABLES");
                System.Xml.XmlElement newvar = Info.CreateElement("VARIABLE");
                System.Xml.XmlAttribute newval = Info.CreateAttribute(name);
                newval.Value = Convert.ToString(value);
                newvar.Attributes.Append(newval);
                vars.AppendChild(newvar);
            }
            catch
            {
            }
        }

        public object GetProperty(string name)
        {
            //Add the global prefix
            string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
            if (_olefile.HasProperty(globalname))
                return _olefile.GetProperty(globalname);
            else
            {
                if (HasXmlProperty(name))
                    return GetXmlProperty(name);
            }


            return null;
        }

        private object GetXmlProperty(string name)
        {
            if (HasXmlProperty(name))
                return Info.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE/@" + System.Xml.XmlConvert.EncodeName(name)).Value;
            else
                return null;
        }

        public bool HasProperty(string name)
        {
            try
            {
                //Add the global prefix
                string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
                if (!_olefile.HasProperty(globalname))
                    return HasXmlProperty(name);
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private bool HasXmlProperty(string name)
        {
            try
            {
                return (Info.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE/@" + System.Xml.XmlConvert.EncodeName(name)) != null);
            }
            catch
            {
                return false;
            }
        }

        public void RemoveProperty(string name)
        {
            try
            {
                //Add the global prefix
                string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
                _olefile.RemoveProperty(globalname);
                RemoveXmlProperty(name);
            }
            catch { }

        }

        private void RemoveXmlProperty(string name)
        {
            try
            {
                if (HasXmlProperty(name))
                {
                    System.Xml.XmlElement vars = Info.SelectSingleNode("/DOCUMENT/VARIABLES") as System.Xml.XmlElement;
                    System.Xml.XmlElement var = Info.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE[@" + System.Xml.XmlConvert.EncodeName(name) + "]") as System.Xml.XmlElement;
                    if (vars != null && var != null) vars.RemoveChild(var);
                }
            }
            catch
            {
            }
        }

        public void Save()
        {
            _olefile.Save();
        }

        private System.Xml.XmlDocument Info
        {
            get
            {
                if (_doc == null)
                {
                    _doc = new System.Xml.XmlDocument();
                }
                System.Xml.XmlElement root = _doc.SelectSingleNode("/DOCUMENT") as System.Xml.XmlElement;
                System.Xml.XmlElement vars = _doc.SelectSingleNode("/DOCUMENT/VARIABLES") as System.Xml.XmlElement;

                if (root == null)
                {
                    root = _doc.CreateElement("DOCUMENT");
                    _doc.AppendChild(root);
                }
                if (vars == null)
                {
                    vars = _doc.CreateElement("VARIABLES");
                    root.AppendChild(vars);
                }

                return _doc;
            }
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            ((IDisposable)_olefile).Dispose();
        }

        #endregion
    }
}
