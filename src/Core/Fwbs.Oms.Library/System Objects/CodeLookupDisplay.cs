using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace FWBS.OMS
{
	public enum CodeLookupDisplaySettings{ExtendedData,DataList,Commands,omsObjects,PrecedentAssoc,Dependents}

	[TypeConverter("FWBS.OMS.UI.Windows.Design.CodeLookupDisplayMultiConverter,OMS.UI")]
	[Editor("FWBS.OMS.UI.Windows.Design.CodeLookupDisplayMultiEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
	public class CodeLookupDisplayMulti : LookupTypeDescriptor
	{
		#region Fields
		protected static SortedList _codelookupcache = new SortedList();
		protected string _codes = "";
		protected string _type = "";
		protected DataRow _cl = null;
		public event EventHandler CodeLookupDisplayChanged;
		protected string _descriptions = "";
		#endregion

		#region Static
		private static DataRow GetCodeLookupCache(string type, string code)
		{
			DataRow _cl = _codelookupcache[type + "|" + code] as DataRow;
			if (_cl == null)
			{
				DataTable cdt = CodeLookup.GetLookups(System.Threading.Thread.CurrentThread.CurrentUICulture,type,code);
				if (cdt.Rows.Count >0) 
				{
					_cl = cdt.Rows[0];
					_codelookupcache.Add(type + "|" + code,_cl);
				}
			}
			return _cl;
		}
		#endregion

		#region Constructor
		public CodeLookupDisplayMulti(string type)
		{
			_type = type;
		}

		public CodeLookupDisplayMulti(string codes, string type)
		{
			this.Codes = codes;
			_type = type;
		}

		public override string ToString()
		{
			if (_descriptions == "") BuildDescription();
			return _descriptions;
		}
		#endregion

		#region Properties
		[Browsable(false)]
		public string Codes
		{
			get
			{
				return _codes;
			}
			set
			{
				_codes = value;
				if (Session.CurrentSession.IsLoggedIn && _type != "" && _codes !="" )
				{
					BuildDescription();
					OnCodeLookupDisplayChanged(new EventArgs());
				}
			}
		}

		[Browsable(false)]
		public string Type
		{
			get
			{
				return _type;
			}
		}

		private void BuildDescription()
		{
			string[] codes = _codes.Split(",".ToCharArray());
			_descriptions = "";
			foreach (string code in codes)
			{
				_cl = GetCodeLookupCache(_type,code);
				if (_cl != null)
					_descriptions += Convert.ToString(_cl["cddesc"]) + ", ";
				else if (code != "")
					_descriptions = "~" + code + "~";
				else
					_descriptions = "";
			}
			_descriptions = _descriptions.Trim(", ".ToCharArray());
		}

		#endregion

		#region Deligates
		protected virtual void OnCodeLookupDisplayChanged(EventArgs e) 
		{
			if (CodeLookupDisplayChanged != null)
				CodeLookupDisplayChanged(this, e);
		}

		#endregion
	}
	
	[TypeConverter("FWBS.OMS.UI.Windows.Design.CodeLookupDisplayConverter,OMS.UI")]
	[Editor("FWBS.OMS.UI.Windows.Design.CodeLookupDisplayEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
	public class CodeLookupDisplay : LookupTypeDescriptor
	{
		#region Fields
		protected static SortedList _codelookupcache = new SortedList();
		protected string _code = "";
		protected string _description = "";
		protected string _ui = "{Default}";
		protected string _type = "";
		protected string _help = "";
		public static bool EditorOpen = false;
		protected CodeLookup _cl = null;
		protected CodeLookupLocalized _cdl = null;
		public event EventHandler CodeLookupDisplayChanged;
		#endregion

		#region Static
		private static CodeLookup GetCodeLookupCache(string type, string code)
		{
			CodeLookup _cl = null;
			_cl = _codelookupcache[type + "|" + code] as CodeLookup;
			if (_cl == null)
			{
				_cl = new CodeLookup(type,code);
				_codelookupcache.Add(type + "|" + code,_cl);
			}
			return _cl;
		}
		#endregion
		
		#region Constructor
		public CodeLookupDisplay(string type)
		{
			_type = type;
		}

		public CodeLookupDisplay(string code, string type, string description, string ui, string help)
		{
			_code = code;
			_type = type;
			_description = description;
			_ui = ui;
			_help = help;
		}

		public void CodeLookupDisplayInt(string code, string type, string description, string ui, string help)
		{
			_code = code;
			_type = type;
			_description = description;
			_ui = ui;
			_help = help;
		}

		public override string ToString()
		{
			return _description;
		}
		#endregion

		#region Properties
		[System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
		public string Code
		{
			get
			{
				return _code;
			}
			set
			{
				_code = value;
				if (Session.CurrentSession.IsLoggedIn && _type != "" && _code !="" )
				{
					_cl = GetCodeLookupCache(_type,_code);
					_cdl = _cl[_ui];
					if (_cdl != null)
					{
						CodeLookupDisplayInt(_cdl.Code,_cdl.CodeType,_cdl.Caption,_cdl.Culture,_cdl.HelpText);
					}
					else
					{
						CodeLookupDisplayInt(_code,_type,"",_ui,"");
					}
					OnCodeLookupDisplayChanged(new EventArgs());
				}
				else
				{
					_description = "";
					_help = "";
					_ui = CodeLookup.DefaultCulture;
				}
			}
		}

		[RefreshProperties(RefreshProperties.All)]
		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public virtual string Description
		{
			get
			{
				return (_description == null ? "" : _description);
			}
			set
			{
				if (_code == "") throw new OMSException2("ERRCANTEDITCLD", "You can't edit the Description when no code is selected please colapse the this Property and enter it in the Box.");
				_description = value;
				if (Session.CurrentSession.IsLoggedIn)
				{
					_cdl.Caption = value;
					_cdl.Update();
				}
				OnCodeLookupDisplayChanged(new EventArgs());
			}
		}

		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public virtual string Help
		{
			get
			{
				return _help;
			}
			set
			{
				if (_code == "") throw new OMSException2("ERCANTEDITCLHLP", "You can't edit the Help when no code is selected please colapse the this Property and enter it in the Box.");
				_help = value;
				if (Session.CurrentSession.IsLoggedIn)
				{
					_cdl.HelpText = value;
					_cdl.Update();
				}
				OnCodeLookupDisplayChanged(new EventArgs());
			}
		}

		[Browsable(false)]
		public string Type
		{
			get
			{
				return _type;
			}
		}

		[TypeConverter("FWBS.OMS.UI.Windows.Design.CultureUIEditor,OMS.UI")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public virtual string UICulture
		{
			get
			{
				return _ui;
			}
			set
			{
				if (_code == "") throw new OMSException2("ERCANTEDITCLCUL", "You can't edit the Culture when no code is selected please colapse the this Property and enter it in the Box.");
				_ui = value;
				if (Session.CurrentSession.IsLoggedIn)
					_cdl = _cl[_ui];
				Code = Code;
			}
		}
		#endregion

		#region Protected
		protected string Str(object text)
		{
			return Convert.ToString(text);
		}
		#endregion

		#region Deligates
		protected virtual void OnCodeLookupDisplayChanged(EventArgs e) 
		{
			if (CodeLookupDisplayChanged != null)
				CodeLookupDisplayChanged(this, e);
		}

		#endregion
	}

	[Editor("FWBS.OMS.UI.Windows.Design.CodeLookupDisplayEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
	[TypeConverter("FWBS.OMS.UI.Windows.Design.CodeLookupReadOnlyConverter,OMS.UI")]
	public class CodeLookupDisplayReadOnly : CodeLookupDisplay
	{
		#region Fields
		private bool _readonly = false;
		private string _readonlymsg = "";
		#endregion
		
		#region Constructor
		public CodeLookupDisplayReadOnly(string type) : base(type)
		{
		}

		public override string ToString()
		{
			return this.Description;
		}

		#endregion

		#region Properties
		[ReadOnly(true)]
		public override string Description
		{
			get
			{
				return (_description == "" && base.Code != "" ? "~" + base.Code + "~" : _description);
			}
			set
			{
			}
		}

		[ReadOnly(true)]
		[Editor("FWBS.OMS.UI.Windows.Design.CodeDescriptionEditor,OMS.UI",typeof(System.Drawing.Design.UITypeEditor))]
		public override string Help
		{
			get
			{
				return _help;
			}
			set
			{
			}
		}

		[Browsable(false)]
		public bool ReadOnly
		{
			get
			{
				return _readonly;
			}
			set
			{
				_readonly=value;
			}
		}
		
		[Browsable(false)]
		public string ReadOnlyMessage
		{
			get
			{
				return _readonlymsg;
			}
			set
			{
				_readonlymsg=value;
			}
		}
		#endregion
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
	public class ParameterAttribute : Attribute 
	{
		private object _value = "";
		private string _key = "";
		private bool _parse = false;
		
		public ParameterAttribute(string key, object value)
		{
			_key = key;
			_value = value;
		}

		public ParameterAttribute(object value)
		{
			_value = value;
		}

		public string Key
		{
			get
			{
				return _key;
			}
		}

		public object Value
		{
			get
			{
				return _value;
			}
		}

		public bool Parse
		{
			get
			{
				return _parse;
			}
			set
			{
				_parse = value;
			}
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class CodeLookupSelectorTitleAttribute : Attribute 
	{
		string _title = "";
		public CodeLookupSelectorTitleAttribute(string Code, string Description)
		{
			_title = Session.CurrentSession.Resources.GetResource(Code,Description,"").Text;
		}

		public string Title
		{
			get
			{
				return _title;
			}
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	public class TextOnlyAttribute : Attribute 
	{
		bool _textonly = true;
		public TextOnlyAttribute(bool TextOnly)
		{
			_textonly = TextOnly;
		}

		public object TextOnly
		{
			get
			{
				return _textonly;
			}
		}
	}

	public enum CodeLookupUIAttributes {Question,ChangeCode}
	[AttributeUsage(AttributeTargets.All)]
	public class CodeLookupUIAttribute : Attribute 
	{
		CodeLookupUIAttributes _key = CodeLookupUIAttributes.Question;
		public CodeLookupUIAttribute(CodeLookupUIAttributes Key)
		{
			_key = Key;
		}

		public CodeLookupUIAttributes Value
		{
			get
			{
				return _key;
			}
		}
	}
}
