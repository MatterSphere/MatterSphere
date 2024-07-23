using System;
using System.Collections.Generic;

namespace FWBS.OMS
{
    using System.Xml;

    public sealed class XmlProperties
    {
        private readonly Interfaces.IExtraInfo _obj;
        private readonly XmlDocument _doc;
        private readonly string _xmlfield;
        private bool supported;
        private readonly IDictionary<string, string> _defaultForms = new Dictionary<string, string>();
        private readonly IDictionary<string, string> _defaultSearchLists = new Dictionary<string, string>();
        private readonly IDictionary<string, string> _defaultSearchListsGroups = new Dictionary<string, string>();

        #region XML Settings Methods

        public XmlDocument Xml
        {
            get
            {
                CheckSupported();

                return _doc;
            }
        }

        public bool IsSupported
        {
            get
            {
                return supported;
            }
        }


        private void CheckSupported()
        {
            if (!IsSupported)
                throw new NotSupportedException(String.Format("'{0}' does not support xml properties.", _obj.GetType().FullName));
        }

        public XmlProperties(Interfaces.IExtraInfo obj, string xmlfield)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (String.IsNullOrEmpty(xmlfield))
                throw new ArgumentNullException("xmlfield");

            this._obj = obj;
            this._xmlfield = xmlfield;
            this._doc = new System.Xml.XmlDocument();

            _doc.PreserveWhitespace = false;

            Refresh();

        }

        public void Refresh()
        {
            string xml = String.Empty;


            try
            {
                xml = Convert.ToString(_obj.GetExtraInfo(_xmlfield)).Trim();
                if (xml != String.Empty)
                    _doc.LoadXml(xml);

                supported = true;

                System.Xml.XmlNode root = _doc.SelectSingleNode("/config");
                if (root == null)
                {
                    root = _doc.CreateElement("config");
                    _doc.AppendChild(root);
                }


                System.Xml.XmlElement _settings = root.SelectSingleNode("settings") as System.Xml.XmlElement;
                if (_settings == null)
                {
                    _settings = _doc.CreateElement("settings");
                    root.AppendChild(_settings);
                }

                BuildOverrides();
            }
            catch (ArgumentException)
            {
                supported = false;
            }
        }

        public object GetProperty(string name, object defaultValue)
        {
            CheckSupported();

            System.Xml.XmlElement el = _doc.SelectSingleNode("/config/settings/property[@name='" + System.Xml.XmlConvert.EncodeName(name) + "']") as System.Xml.XmlElement;
            if (el == null)
                return defaultValue;
            else
                return el.Attributes["value"].Value;
        }

        public bool SetProperty(string name, object val)
        {
            CheckSupported();

            bool changed = false;

            System.Xml.XmlElement el = _doc.SelectSingleNode("/config/settings/property[@name='" + System.Xml.XmlConvert.EncodeName(name) + "']") as System.Xml.XmlElement;
            if (el == null)
            {
                el = _doc.CreateElement("property");
                System.Xml.XmlAttribute n = _doc.CreateAttribute("name");
                n.Value = name;
                System.Xml.XmlAttribute v = _doc.CreateAttribute("value");
                v.Value = Convert.ToString(val);
                el.Attributes.Append(n);
                el.Attributes.Append(v);

                System.Xml.XmlElement _settings = _doc.SelectSingleNode("/config/settings") as System.Xml.XmlElement;
                _settings.AppendChild(el);

                changed = true;

            }
            else
            {
                string original = el.Attributes["value"].Value;
                string newval = Convert.ToString(val);
                if (original != newval)
                {
                    el.Attributes["value"].Value = newval;
                    changed = true;
                }
            }


            return changed;

        }

        public void Update()
        {
            CheckSupported();

            SaveOverrides();

            string originalval = Convert.ToString(this._obj.GetExtraInfo(_xmlfield));
            if (originalval != _doc.OuterXml)
                this._obj.SetExtraInfo(_xmlfield, _doc.OuterXml);
        }



        #endregion

        #region Overrides

        public IDictionary<string, string> DefaultSystemForms
        {
            get
            {
                return _defaultForms;
            }
        }

        public IDictionary<string, string> DefaultSystemSearchLists
        {
            get
            {
                return _defaultSearchLists;
            }
        }

        public IDictionary<string, string> DefaultSystemSearchListGroups
        {
            get
            {
                return _defaultSearchListsGroups;
            }
        }

        private void BuildOverrides()
        {
            BuildOverridesGroup("defaultForms",_defaultForms);
            BuildOverridesGroup("defaultSearchLists",_defaultSearchLists);
            BuildOverridesGroup("defaultSearchListsGroups",_defaultSearchListsGroups);
        }

        private void BuildOverridesGroup(string group, IDictionary<string, string> overrides)
        {
            var elroot = this.Xml.SelectSingleNode("config");

            var elGroup = elroot.SelectSingleNode(group);

            overrides.Clear();

            if (elGroup != null)
            {
                foreach (var child in elGroup.ChildNodes)
                {
                    var elchild = child as XmlElement;
                    if (elchild == null)
                        continue;
                    if (!overrides.ContainsKey(elchild.Name))
                        overrides.Add(new KeyValuePair<string, string>(elchild.Name, elchild.InnerText));
                }
            }
        }

        private void SaveOverrides()
        {
            SaveOverridesGroup("defaultForms",_defaultForms);
            SaveOverridesGroup("defaultSearchLists",_defaultSearchLists);
            SaveOverridesGroup("defaultSearchListsGroups",_defaultSearchListsGroups);
        }

        private void SaveOverridesGroup(string group, IDictionary<string, string> overrides)
        {
            var elroot = this.Xml.SelectSingleNode("config");

            var elGroup = elroot.SelectSingleNode(group);

            if (elGroup == null)
            {
                elGroup = this.Xml.CreateElement(group);
                elroot.AppendChild(elGroup);
            }

            elGroup.RemoveAll();

            foreach (var child in overrides)
            {
                var elItem = elGroup.SelectSingleNode(child.Key);
                if (elItem == null)
                {
                    elItem = this.Xml.CreateElement(child.Key);
                    elGroup.AppendChild(elItem);
                }

                if (string.IsNullOrEmpty(child.Value.Trim()))
                {
                    elGroup.RemoveChild(elItem);
                    continue;
                }

                elItem.InnerText = child.Value;
            }

            if (elGroup.ChildNodes.Count == 0)
                elroot.RemoveChild(elGroup);

        }

        #endregion

    }
}
