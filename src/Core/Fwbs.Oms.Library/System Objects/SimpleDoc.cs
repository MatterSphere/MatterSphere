using System;

namespace FWBS.OMS
{
	/// <summary>
	/// A simple base document that may be used for SimpleOMSApp.
	/// The document is based off a xml document.
	/// </summary>
	public class SimpleDoc
	{
		private System.Xml.XmlDocument _doc = null;
        private System.IO.FileInfo _file = null;

		public  SimpleDoc()
		{
		}

		public SimpleDoc(System.Xml.XmlDocument doc, System.IO.FileInfo file)
		{
			_doc = doc;
            _file = file;
		}

		public System.Xml.XmlDocument Xml
		{
			get
			{
				BuildDocSchema(ref _doc);
				return _doc;
			}
		}

        public System.IO.FileInfo File
        {
            get
            {
                return _file;
            }
        }

		#region Properties

		public string Text
		{
			get
			{
				return Convert.ToString(GetExtraInfo("_text", ""));
			}
			set
			{
				if (value == null)
					SetExtraInfo("_text", "");
				else
					SetExtraInfo("_text", value);
			}
		}

		public int VariableCount
		{
			get
			{
				int ctr = 0;
				foreach (System.Xml.XmlNode nd in Xml.SelectSingleNode("/DOCUMENT/VARIABLES").ChildNodes)
				{
					if (nd.NodeType == System.Xml.XmlNodeType.Element)
						ctr++;
				}
				return ctr;
			}
		}

		#endregion

		#region Methods

		public bool SetVariable(string varName, object val)
		{
			try
			{
				System.Xml.XmlAttribute newval = null;
				try
				{
					newval = (System.Xml.XmlAttribute) Xml.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE/@" + System.Xml.XmlConvert.EncodeName(varName));
				}
				catch{}
				if (newval == null)
				{
					System.Xml.XmlElement vars = (System.Xml.XmlElement)Xml.SelectSingleNode("/DOCUMENT/VARIABLES");
					System.Xml.XmlElement newvar = Xml.CreateElement("VARIABLE");
					newval = Xml.CreateAttribute(varName);
					newvar.Attributes.Append(newval);
					vars.AppendChild(newvar);
				}
				newval.Value = Convert.ToString(val);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public object GetVariable(string varName)
		{
			return Xml.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE/@" + System.Xml.XmlConvert.EncodeName(varName)).Value;
		}

		public string GetVariableName(int index)
		{
			return Xml.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE[" + (index + 1).ToString() + "]").Attributes[0].Name;
		}

		public bool HasVariable(string varName)
		{
			try
			{
				return (Xml.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE/@" + System.Xml.XmlConvert.EncodeName(varName)) != null);
			}
			catch
			{
				return false;
			}
		}

		public void RemoveVariable(string varName)
		{
			try
			{
				if (HasVariable(varName))
				{
					System.Xml.XmlElement vars =  Xml.SelectSingleNode("/DOCUMENT/VARIABLES") as System.Xml.XmlElement;
					System.Xml.XmlElement var = Xml.SelectSingleNode("/DOCUMENT/VARIABLES/VARIABLE[@" + System.Xml.XmlConvert.EncodeName(varName) + "]") as System.Xml.XmlElement;
					if (vars != null && var != null) vars.RemoveChild(var);
				}
			}
			catch{}
		}

		public void SetExtraInfo(string name, object val)
		{
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

			BuildDocSchema(ref _doc);

			System.Xml.XmlElement vars = null;

			try
			{
				vars =  _doc.SelectSingleNode("/DOCUMENT/FIELDS") as System.Xml.XmlElement;
				System.Xml.XmlElement var = _doc.SelectSingleNode("/DOCUMENT/FIELDS/FIELD[@" + System.Xml.XmlConvert.EncodeName(name) + "]") as System.Xml.XmlElement;
				if (vars != null && var != null) vars.RemoveChild(var);
			}
			catch{}

			vars = (System.Xml.XmlElement)_doc.SelectSingleNode("/DOCUMENT/FIELDS");
			System.Xml.XmlElement newvar = _doc.CreateElement("FIELD");
			System.Xml.XmlAttribute newval = _doc.CreateAttribute(name);
			newval.Value = Convert.ToString(val);
			newvar.Attributes.Append(newval);
			vars.AppendChild(newvar);
		}

		public object GetExtraInfo(string name, object def)
		{
            object val;
			BuildDocSchema(ref _doc);
			try
			{
				val = (_doc.SelectSingleNode("/DOCUMENT/FIELDS/FIELD/@" + System.Xml.XmlConvert.EncodeName(name)).Value);

			}
			catch
			{
				val = def;
			}

            //UTCFIX: DM - 30/11/06 - return local time
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
		}

		public static void BuildDocSchema(ref System.Xml.XmlDocument doc)
		{
			if (doc== null)
			{
				doc = new System.Xml.XmlDocument();
			}
			System.Xml.XmlElement root = doc.SelectSingleNode("/DOCUMENT") as System.Xml.XmlElement;
			System.Xml.XmlElement vars = doc.SelectSingleNode("/DOCUMENT/VARIABLES") as System.Xml.XmlElement;
			System.Xml.XmlElement fields = doc.SelectSingleNode("/DOCUMENT/FIELDS") as System.Xml.XmlElement;
			if (root == null)
			{
				root = doc.CreateElement("DOCUMENT");
				doc.AppendChild(root);
			}
			if (vars == null)
			{
				vars = doc.CreateElement("VARIABLES");
				root.AppendChild(vars);
			}
			if (fields == null)
			{
				fields = doc.CreateElement("FIELDS");
				root.AppendChild(fields);
			}
		}


		public void Save(string fileName)
		{
			Xml.Save(fileName);
            _file = new System.IO.FileInfo(fileName);
		}

		#endregion;

	}
}
