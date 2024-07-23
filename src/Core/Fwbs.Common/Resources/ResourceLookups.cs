using System;
using System.ComponentModel;

namespace FWBS.Common.Resources
{

	/// <summary>
	/// Fetches localized resources and messages for the application.
	/// </summary>
	public class ResourceLookup
	{
		#region Fields

		private System.Data.DataTable _res;
	
		public static ResourceLookup ResourceSet = null;

		public static ResourceLookup MessageSet = null;

		#endregion

		#region Constructors

		private ResourceLookup(){}

		public ResourceLookup(System.Data.DataTable res, string fieldCode, string fieldType, string fieldDesc, string fieldHelp, string defaultType)
		{
			_res = res;
			
			if (_res.Columns.Contains(fieldCode))
			{
				_res.Columns[fieldCode].Caption = fieldCode;
				_res.Columns[fieldCode].ColumnName = "RESCODE";
			}
			else
				throw new FWBSException("MUSTHAVERESCODE", "Must have a resource code mapping field.");

			if (_res.Columns.Contains(fieldType))
			{
				_res.Columns[fieldType].Caption = fieldType;
				_res.Columns[fieldType].DefaultValue = defaultType;
				_res.Columns[fieldType].ColumnName = "RESTYPE";
			}

			if (_res.Columns.Contains(fieldDesc))
			{
				_res.Columns[fieldDesc].Caption = fieldDesc;
				_res.Columns[fieldDesc].ColumnName = "RESDESC";
			}
			else
				throw new FWBSException("MUSTHAVERESDESC", "Must have a resource text mapping field.");

			if (_res.Columns.Contains(fieldHelp))
			{
				_res.Columns[fieldHelp].Caption = fieldHelp;
				_res.Columns[fieldHelp].ColumnName = "RESHELP";
			}
			else
				throw new FWBSException("MUSTHAVERESHELP", "Must have a resource help mapping field.");

		}

		#endregion

		#region Static Methods

		public static ResourceItemAttribute GetResource(ResourceItemAttribute res, params string [] param)
		{
			if (ResourceSet == null)
			{
				res.Text = String.Format(res.Text, param);
				res.Help = String.Format(res.Help, param);
				return res;
			}
			else
				return ResourceSet.GetRes(res, param);
		}

		public static ResourceItemAttribute GetMessage(ResourceItemAttribute res, params string [] param)
		{
			if (MessageSet == null)
			{
				res.Text = String.Format(res.Text, param);
				res.Help = String.Format(res.Help, param);
				return res;
			}
			else
				return MessageSet.GetRes(res, param);
		}

		#endregion

		#region Methods

		public ResourceItemAttribute GetRes(ResourceItemAttribute res, params string [] param)
		{
			return GetRes(_res, res, param);
		}

		private ResourceItemAttribute GetRes(System.Data.DataTable dt, ResourceItemAttribute res, params string [] param)
		{
			if (param == null) param = new string[0];
			ResourceItemAttribute ret = res;

			try
			{
				if (dt != null)
				{
					System.Data.DataView vw = dt.DefaultView;
					vw.RowFilter = "RESCODE = '" + res.Code.Replace("'", "''") + "'";
					if (vw.Count > 0)
					{
						ret.Text = Convert.ToString(vw[0]["RESDESC"]);
						ret.Help = Convert.ToString(vw[0]["RESHELP"]);
					}
					else
					{
						System.Data.DataRow r = dt.NewRow();
						r["RESCODE"] = res.Code;
						r["RESDESC"] = res.Text;
						r["RESHELP"] = res.Help;
						dt.Rows.Add(r);
					}
				}
			}
			catch{}

			ret.Text = String.Format(ret.Text,param);
			ret.Help = String.Format(ret.Help, param);
			return ret;
		}

		

		#endregion

	}



	/// <summary>
	/// A property descriptor for localized code lookups.
	/// </summary>
	public class ResourcePropertyDescriptor : PropertyDescriptor 
	{ 
		private PropertyDescriptor baseProp; 
		private Resources.ResourceItemAttribute _res;
		private Resources.ResourceItemAttribute _help;

 
		public ResourcePropertyDescriptor(PropertyDescriptor baseProp, Attribute[] filter) : base(baseProp) 
		{ 
			this.baseProp = baseProp;
			
			ResourceItemAttribute res = null;
			ResourceHelpItemAttribute help = null;
			foreach(Attribute attr in this.baseProp.Attributes)
			{
				if (attr is ResourceHelpItemAttribute)
					help = (ResourceHelpItemAttribute)attr;
				else if (attr is ResourceItemAttribute)
					res = (ResourceItemAttribute)attr;
				
			}
			if (res == null)
			{
				res = new ResourceItemAttribute(this.baseProp.Name, this.baseProp.Name);
				res.Help = baseProp.Description;
			}

			_res = ResourceLookup.GetResource(res);
			
			if (help != null)
				_help = ResourceLookup.GetResource(help);
		} 
 
		#region PropertyDescriptor

		public override string Name 
		{ 
			get{return this.baseProp.Name;} 
		} 
 
		public override string DisplayName 
		{ 
			get 
			{ 
				return _res.Text;
			} 
		} 

		public override string Description
		{
			get
			{
				if (_help == null)
					return _res.Help;
				else
					return _help.Text;
			}
		}

		public override bool IsReadOnly 
		{ 
			get {return baseProp.IsReadOnly;} 
		}        
 
		public override bool CanResetValue(object component) 
		{ 
			return this.baseProp.CanResetValue(component); 
		}     
 
		public override Type ComponentType 
		{ 
			get{return baseProp.ComponentType;} 
		} 
 
		public override object GetValue(object component) 
		{ 
			return this.baseProp.GetValue(component); 
		} 
 
		public override Type PropertyType 
		{ 
			get{return this.baseProp.PropertyType;} 
		} 
 
		public override void ResetValue(object component) 
		{ 
			baseProp.ResetValue(component); 
		} 
 
		public override void SetValue(object component, object Value) 
		{ 
			this.baseProp.SetValue(component, Value); 
		} 
 
		public override bool ShouldSerializeValue(object component) 
		{ 
			return this.baseProp.ShouldSerializeValue(component); 
		} 

		#endregion

	}  


	/// <summary>
	/// The type decsriptor that is used in conjuction with the lookup property descriptor.
	/// </summary>
	public class ResourceTypeDescriptor : ICustomTypeDescriptor 
	{ 
 
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
		{ 
			PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(GetType(), filter); 
			// notice we use the type here so we don't recurse 
			PropertyDescriptor[] newProps = new PropertyDescriptor[baseProps.Count]; 
 
			for (int i = 0; i < baseProps.Count; i++) 
			{ 
				newProps[i] = new ResourcePropertyDescriptor(baseProps[i], filter); 
				string oldname = ((PropertyDescriptor)baseProps[i]).DisplayName ; 
				string newname = ((ResourcePropertyDescriptor)newProps[i]).DisplayName; 
			} 
 
			// probably wanna cache this... 
			return new PropertyDescriptorCollection(newProps); 
		} 
 
		AttributeCollection ICustomTypeDescriptor.GetAttributes() 
		{ 
			return TypeDescriptor.GetAttributes(this, true); 
		} 
 

		string ICustomTypeDescriptor.GetClassName() 
		{ 
			return TypeDescriptor.GetClassName(this, true); 
		} 
       
		string ICustomTypeDescriptor.GetComponentName() 
		{ 
			return TypeDescriptor.GetComponentName(this, true); 
		} 
 
		TypeConverter ICustomTypeDescriptor.GetConverter() 
		{ 
			return TypeDescriptor.GetConverter(this, true); 
		} 
 

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
		{ 
			return TypeDescriptor.GetDefaultEvent(this, true); 
		} 

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(System.Attribute[] attributes) 
		{ 
			return TypeDescriptor.GetEvents(this, attributes, true); 
		} 
 
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
		{ 
			return TypeDescriptor.GetEvents(this, true); 
		} 
 
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
		{ 
			return TypeDescriptor.GetDefaultProperty(this, true); 
		} 
 
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() 
		{ 
			return TypeDescriptor.GetProperties(this, true); 
		} 
 
		object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType) 
		{ 
			return TypeDescriptor.GetEditor(this, editorBaseType, true); 
		} 
 
		object ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) 
		{ 
			return this; 
		} 
	}

	
	/// <summary>
	/// Overrided Localized Category Attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class ResourceItemCategoryAttribute : System.ComponentModel.CategoryAttribute 
	{
		private string _text;
		
		public ResourceItemCategoryAttribute(string code, string text) : base(code)
		{
			_text = text;
		}

		protected override string GetLocalizedString(string code) 
		{
			ResourceItemAttribute res = new ResourceItemAttribute(code, _text);
			return ResourceLookup.GetResource(res).Text;
		}
	}

	/// <summary>
	/// A code lookup attribute that holds a code to a resourced code lookup.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	[TypeConverter(typeof(ResourceItemConverter))]
	[Serializable()]
	public class ResourceItemAttribute : Attribute
	{
		private string _code;
		private string _text;
		private string _help;

		public ResourceItemAttribute (string code, string text)
		{
			_code = code;
			_text = text;
			if (_text == String.Empty)
				_text = "~" + _code + "~";
			_help = "";
		}

		public string Code
		{
			get
			{
				if (_code.Length > 15) _code = _code.Substring(0, 15);
				return _code;
			}
			set
			{
				_code = value;
			}
		}

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		public string Help
		{
			get
			{
				return _help;
			}
			set
			{
				_help = value;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public override object TypeId
		{
			get
			{
				return base.TypeId;
			}
		}

		public override string ToString()
		{
			return _text;
		}

	}

	[AttributeUsage(AttributeTargets.All)]
	public class ResourceHelpItemAttribute : ResourceItemAttribute
	{
		public ResourceHelpItemAttribute  (string code, string text) : base(code, text)
		{
		}
	}


	public class ResourceItemConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo (ITypeDescriptorContext ctx , Type destinationType )
		{
			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor))
				return true;
			else
				return base.CanConvertTo (ctx, destinationType);
		}

		public override object ConvertTo ( ITypeDescriptorContext ctx , System.Globalization.CultureInfo culture , object value , Type destinationType )
		{
			if (destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor))
			{
				ResourceItemAttribute gf = value as ResourceItemAttribute;
				if (gf != null)
				{
					Type[] parms = new Type[] {typeof(String),typeof(String),typeof(String)};
					System.Reflection.ConstructorInfo	ci = typeof(ResourceItemAttribute).GetConstructor(parms);
					return new System.ComponentModel.Design.Serialization.InstanceDescriptor (ci,new object[]{gf.Code, gf.Text, gf.Help});
				}
				else
					return base.ConvertTo(ctx, culture, value, destinationType); 
			}
			else
				return base.ConvertTo(ctx, culture, value, destinationType ) ;
		}
	}




}
