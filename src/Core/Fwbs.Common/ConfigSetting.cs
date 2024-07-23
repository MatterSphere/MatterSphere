using System;
using System.Data;
using System.Xml;

namespace FWBS.Common
{
    /// <summary>
    /// A class that accepts an xml snippet and xpath information to simplify the 
    /// writing and reading of xml attributes / values.
    /// </summary>
    /// <example>
    ///  <config>
    ///  </config>
    ///  _xmlParams = new FWBS.Common.ConfigSetting(_rw,"enqParameters");
    ///  _xmlParams.Current = "params";
    ///	 foreach(ConfigSettingItem dr in _xmlParams.CurrentChildItems)
    ///		Console.WriteLine(dr.GetString("name",""));
    ///		Console.WriteLine(dr.GetString("type",""));
    ///		Console.WriteLine(dr.GetString("test",""));
    ///		Console.WriteLine(dr.GetString(""));
    ///	 }
    ///	 <output>
    ///	 id
    ///	 System.String
    ///	 1234
    ///	 %1%
    ///	 max
    ///	 System.Int32
    ///	 50
    ///	 %2%
    ///	 </output>
    /// </example>
    public class ConfigSetting
	{
		#region Events

		/// <summary>
		/// An event that gets raised when a change has been made to the underlying
		/// xml document throught the config setting object.
		/// </summary>
		public event EventHandler Changed = null;

		#endregion

		#region Fields

		/// <summary>
		/// The xml document that is currently being manipulated.
		/// </summary>
		private System.Xml.XmlDocument _configdata = new System.Xml.XmlDocument();

		/// <summary>
		/// A reference to the root config node.
		/// </summary>
		private XmlElement _xmlConfig = null;

		/// <summary>
		/// The current xml element to read or write from.
		/// </summary>
		private XmlElement _current = null;

		/// <summary>
		/// The current element xpath string.
		/// </summary>
		private string _currentXPath = "";

		/// <summary>
		/// The source object to keep in sync.
		/// </summary>
		private DataRow _source = null;

		/// <summary>
		/// If source is a data row then this is the field name of the source.
		/// </summary>
		private string _source2 = "";


		#endregion

		#region Constructors

		/// <summary>
		/// A constructor that accepts an xml string which will be converted into
		/// a xml DOM.
		/// </summary>
		/// <param name="xml">XML document as a string.</param>
		public ConfigSetting(string xml)
		{
			try
			{
				if (xml == "")
				{
					// Check if null then do a default XML Document
					xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><config></config>";
				}
				_configdata.LoadXml(xml);
			}
			catch
			{
				_configdata.LoadXml(xml);
			}
			_configdata.PreserveWhitespace = false; 
			_xmlConfig = _configdata.DocumentElement;
			_current = (XmlElement)_xmlConfig;
			if (_current.Name != "config")
			{
				xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?><config>" + _configdata.DocumentElement.OuterXml + "</config>";
				_configdata.LoadXml(xml);
				_xmlConfig = _configdata.DocumentElement;
				_current = (XmlElement)_xmlConfig;
			}
		}

        /// <summary>
        /// A constructor that accepts an xml file which will be converted into
        /// a xml DOM.
        /// </summary>
        /// <param name="xmlfile">XML document as file.</param>
        public ConfigSetting(System.IO.FileInfo xmlfile)
		{
			_configdata.Load(xmlfile.FullName);
			_configdata.PreserveWhitespace = false; 
			_xmlConfig = _configdata.DocumentElement;
			_current = (XmlElement)_xmlConfig;
		}

		/// <summary>
		/// A constructor that accepts a data row alongside a field name so that the
		/// xml data within the row can be synchronised if changed within the object itself.
		/// </summary>
		/// <param name="Row">The data row to be kept in sync.</param>
		/// <param name="Fieldname">The field name within the data row.</param>
		public ConfigSetting(DataRow Row, string Fieldname) : this(Convert.ToString(Row[Fieldname]))
		{
			_configdata.PreserveWhitespace = false; 
			_source = Row;
			_source2 = Fieldname;
			OnChanged();
		}

		/// <summary>
		/// Sets up the configuration data with an already created xml document.
		/// </summary>
		/// <param name="xmlDoc">Existing xml document.</param>
		public ConfigSetting(XmlDocument xmlDoc) : this (xmlDoc.InnerXml)
		{
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Writes an attribute to the xml document at the specified node.
		/// </summary>
		/// <param name="element">The element to retrieve the value / attribute value from.</param>
		/// <param name="name">The name of the attribute, uses the value section if empty string.</param>
		/// <param name="val">The value to write.</param>
		private void WriteAttribute(XmlElement element, string name, string val)
		{
			if (element == null) return;

			if (name == String.Empty)
			{
				element.Value = val;
			}
			else
			{
				XmlAttribute attr = element.GetAttributeNode(name);
				if (attr == null)
					element.Attributes.Append(CreateAttribute(name, val));
				else
					attr.Value = val;
			}
		}

		/// <summary>
		/// Creates an attribute ready to be appended to an element.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="val">The value to use for the attribute.</param>
		/// <returns>The attribute to be appended.</returns>
		private System.Xml.XmlAttribute CreateAttribute(string name, string val)
		{
			System.Xml.XmlAttribute attr = _configdata.CreateAttribute(name);
			attr.Value = val;
			return attr;
		}

		/// <summary>
		/// Creates an element ready to be appended to another.
		/// </summary>
		/// <param name="xpath">The xpath of the parent of the element.</param>
        /// <param name="create"></param>
		/// <returns>The element that has been appended.</returns>
		private System.Xml.XmlElement CreateElement(string xpath, bool create)
		{
			string newparce = xpath;

			if (newparce.StartsWith("/config")) newparce = newparce.Substring(7,newparce.Length-7);
			string [] parents = newparce.Split('/');
			XmlNode nd=null;
			if (newparce == "") nd = _xmlConfig.SelectSingleNode(newparce); else nd = _current.SelectSingleNode(newparce);
			if (nd == null && create)
			{
				if (newparce.StartsWith("/")) 
					nd = _xmlConfig;
				else
					nd = _current;
				foreach (string par in parents)
				{
					if (par != "")
					{
						if (nd.SelectSingleNode(par) == null) 
						{
							System.Xml.XmlElement ed = _configdata.CreateElement(par);
							nd.AppendChild(ed);
							nd = ed;
						}
						else
							nd = nd.SelectSingleNode(par);
					}
				}
			}
			if (nd != null)
			{
				System.Xml.XmlElement el = (XmlElement)nd;
				return el;
			}
			else 
				return null;
		}
		
		/// <summary>
		/// Gets an attribute / value from the specified element.
		/// </summary>
		/// <param name="element">The element to read from.</param>
		/// <param name="name">The name of the attribute to access.  Use empty string to use the element value.</param>
		/// <param name="defVal">Thee default value to use if the value does not exist.</param>
		/// <returns>The string representation of the value.</returns>
		private string ReadAttribute(XmlElement element, string name, string defVal)
		{
			string ret = defVal;
			XmlAttribute attr = element.GetAttributeNode(name);
			if (attr == null)
				WriteAttribute(element,name,defVal);
			else
				defVal = attr.Value;
			
			return defVal;
		}

		/// <summary>
		/// Activates the change event.
		/// </summary>
		private void OnChanged()
		{
			if (_source != null)
			{
				_source[_source2] = _configdata.InnerXml;
			}

			if (Changed != null)
				Changed(this,EventArgs.Empty);
		}

		#endregion

		#region Public Methods


		/// <summary>
		/// Gets an attribute / element value from the underlying xml document.
		/// </summary>
		/// <param name="elementXPath">xpath to the element to manipulate.</param>
		/// <param name="name">The name of the attribute, empty string if the element value.</param>
		/// <param name="defVal">The default value to use if one does not exist.</param>
		/// <returns>The settings value.</returns>
		public string GetSetting (string elementXPath, string name, string defVal)
		{
			XmlElement el = CreateElement(elementXPath,false);
			if (el == null)
				return defVal;
			else
				return ReadAttribute(el, name, defVal);
		}

		/// <summary>
		/// Writes an attribute / element value to the underlying xml document.
		/// </summary>
		/// <param name="elementXPath">xpath to the element to manipulate.</param>
		/// <param name="name">The name of the attribute, empty string if the element value.</param>
		/// <param name="val">The default value to use if one does not exist.</param>
		public void SetSetting (string elementXPath, string name, string val)
		{
			XmlElement el = CreateElement(elementXPath,true);
			WriteAttribute(el, name, val);
			OnChanged();
		}


		/// <summary>
		/// Textual representation of the config settings object.
		/// </summary>
		/// <returns>Ditto</returns>
		public override string ToString()
		{
			return _configdata.InnerXml;
		}

		public void Synchronise()
		{
			OnChanged();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a reference to the underlying xml document.
		/// </summary>
		public XmlDocument DocObject
		{
			get
			{
				return _configdata;	
			}
		}

		/// <summary>
		/// Gets or Sets the current element to read or write from.
		/// </summary>
		public string Current
		{
			get
			{
				return _currentXPath;
			}
			set
			{
				
				_current = CreateElement(value,true);
				_currentXPath = value;
			}
		}

		public XmlElement DocCurrent
		{
			get
			{
				return _current;
			}
		}

		/// <summary>
		/// Returns a list of ConfigSettingItem of the Current Xpath
		/// </summary>
		public ConfigSettingItem[] CurrentChildItems
		{
			get
			{
				ConfigSettingItem [] _items;
				System.Collections.ArrayList arr = new System.Collections.ArrayList();
				int ctr = 0;
				foreach(XmlNode dr in _current.ChildNodes)
				{
					if (dr is XmlElement)
					{
						arr.Add(new ConfigSettingItem((XmlElement)dr));
						ctr++;
					}
				}
				_items = new ConfigSettingItem[ctr];
				arr.CopyTo(_items);
				return _items;
			}
		}

		/// <summary>
		/// Add a Child Item to the Xpath
		/// </summary>
		/// <returns></returns>
		public ConfigSettingItem AddChildItem(string Name)
		{
			XmlNode _newnode = _xmlConfig.OwnerDocument.CreateNode(XmlNodeType.Element,Name,"");
			return AddChildItem((XmlElement)_newnode);
		}

		/// <summary>
		/// Add a Child Item with Element to the Xpath
		/// </summary>
		/// <param name="Element"></param>
		/// <returns></returns>
		public ConfigSettingItem AddChildItem(XmlElement Element)
		{
			_current.AppendChild(Element);
			return new ConfigSettingItem(Element);
		}

		/// <summary>
		/// Clear Elements from the Current Expath
		/// </summary>
		public void ClearElements()
		{
			for(int ui = _current.ChildNodes.Count-1; ui > -1; ui--)
			{
				if (_current.ChildNodes[ui] is XmlElement)
				{
					XmlNode dr = _current.ChildNodes[ui];
					_current.RemoveChild(dr);
				}
			}
		}
		#endregion

		#region Get Current Strings
		/// <summary>
		/// Get Element Value
		/// </summary>
		/// <param name="DefVal"></param>
		/// <returns></returns>
		public string GetString(string DefVal)
		{
			return GetString("",DefVal);
		}

		/// <summary>
		/// Set Element Value
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public string SetString(string Value)
		{
			return SetString("",Value);
		}
			
		/// <summary>
		/// Returns a string from the Attribute of the Node
		/// </summary>
		/// <param name="Name">Name of the Attribute</param>
		/// <param name="DefVal">The Default Value</param>
		/// <returns>A String</returns>
		public string GetString(string Name, string DefVal)
		{
			try
			{
				if (Name == "")
					return _current.InnerText;
				else
					return ReadAttribute(_current, Name, DefVal);
			}
			catch
			{
				return DefVal;
			}
		}

		/// <summary>
		/// Sets a String to an Attribute of the Node
		/// </summary>
		/// <param name="Name">Attribute Name</param>
		/// <param name="Value">The Value</param>
		/// <returns>Returns The Value</returns>
		public string SetString(string Name, string Value)
		{
			try
			{
				if (Name == "")
					_current.InnerText = Value;
				else
					WriteAttribute(_current, Name, Value);
				return Value;
			}
			catch
			{
				return Value;
			}
		}
		#endregion

	}

	public class ConfigSettingItem
	{
		#region Fields, Construtors, Properties
		private XmlElement _element;
		
		internal ConfigSettingItem(XmlElement Element)
		{
			_element = Element;
		}

		public XmlElement Element
		{
			get
			{
				return _element;
			}
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Writes an attribute to the xml document at the specified node.
		/// </summary>
		/// <param name="element">The element to retrieve the value / attribute value from.</param>
		/// <param name="name">The name of the attribute, uses the value section if empty string.</param>
		/// <param name="val">The value to write.</param>
		private void WriteAttribute(XmlElement element, string name, string val)
		{
			if (element == null) return;

			if (name == String.Empty)
			{
				element.Value = val;
			}
			else
			{
				XmlAttribute attr = element.GetAttributeNode(name);
				if (attr == null)
					element.Attributes.Append(CreateAttribute(name, val));
				else
					attr.Value = val;
			}
		}

		/// <summary>
		/// Creates an attribute ready to be appended to an element.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="val">The value to use for the attribute.</param>
		/// <returns>The attribute to be appended.</returns>
		private System.Xml.XmlAttribute CreateAttribute(string name, string val)
		{
			System.Xml.XmlAttribute attr = _element.OwnerDocument.CreateAttribute(name);
			attr.Value = val;
			return attr;
		}
		
		/// <summary>
		/// Gets an attribute / value from the specified element.
		/// </summary>
		/// <param name="element">The element to read from.</param>
		/// <param name="name">The name of the attribute to access.  Use empty string to use the element value.</param>
		/// <param name="defVal">Thee default value to use if the value does not exist.</param>
		/// <returns>The string representation of the value.</returns>
		private string ReadAttribute(XmlElement element, string name, string defVal)
		{
			string ret = defVal;
			XmlAttribute attr = element.GetAttributeNode(name);
			if (attr == null)
				WriteAttribute(element,name,defVal);
			else
				defVal = attr.Value;
			
			return defVal;
		}

		#endregion

		#region Get Element Strings
		/// <summary>
		/// Get Element Value
		/// </summary>
		/// <param name="DefVal"></param>
		/// <returns></returns>
		public string GetString(string DefVal)
		{
			return GetString("",DefVal);
		}

		/// <summary>
		/// Set Element Value
		/// </summary>
		/// <param name="Value"></param>
		/// <returns></returns>
		public string SetString(string Value)
		{
			return SetString("",Value);
		}
			
		/// <summary>
		/// Returns a string from the Attribute of the Node
		/// </summary>
		/// <param name="Name">Name of the Attribute</param>
		/// <param name="DefVal">The Default Value</param>
		/// <returns>A String</returns>
		public string GetString(string Name, string DefVal)
		{
			try
			{
				if (Name == "")
					return _element.InnerText;
				else
					return ReadAttribute(_element, Name, DefVal);
			}
			catch
			{
				return DefVal;
			}
		}

		/// <summary>
		/// Sets a String to an Attribute of the Node
		/// </summary>
		/// <param name="Name">Attribute Name</param>
		/// <param name="Value">The Value</param>
		/// <returns>Returns The Value</returns>
		public string SetString(string Name, string Value)
		{
			try
			{
				if (Name == "")
					_element.InnerText = Value;
				else
					WriteAttribute(_element, Name, Value);
				return Value;
			}
			catch
			{
				return Value;
			}
		}
		#endregion
	}
}
