using System;
using System.Collections.Generic;
using System.ComponentModel;
using FWBS.Common;

namespace FWBS.OMS
{
	/// <summary>
	/// A property descriptor for localized code lookups.
	/// </summary>
	public class LookupPropertyDescriptor : PropertyDescriptor 
	{ 
		private PropertyDescriptor baseProp; 
		private ResourceItem _res;
		private object _extender = null;
 
		public LookupPropertyDescriptor(PropertyDescriptor baseProp, Attribute[] filter) : this(baseProp, filter, null) 
		{ 
		} 
 
		public LookupPropertyDescriptor(PropertyDescriptor baseProp, Attribute[] filter, object extender) : base(baseProp) 
		{ 
			_extender = extender;
			this.baseProp = baseProp;
			if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
			{
				string resid = baseProp.Name;
				foreach(Attribute attr in this.baseProp.Attributes)
				{
					if (attr is LookupAttribute)
					{
						resid = ((LookupAttribute)attr).Code;
						break;
					}
				}

				_res = FWBS.OMS.Session.CurrentSession.Resources.GetResource(resid,"","");
			}
			else
				_res = new ResourceItem(this.baseProp.Name, this.baseProp.Description);
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
				return _res.Help;
			}
		}

		public override bool IsReadOnly 
		{ 
			get {return baseProp.IsReadOnly;} 
		}
 
		public override bool CanResetValue(object component) 
		{ 
			return this.baseProp.CanResetValue(_extender ?? component);
		}
 
		public override Type ComponentType 
		{ 
			get{return baseProp.ComponentType;} 
		} 
 
		public override object GetValue(object component) 
		{ 
			return this.baseProp.GetValue(_extender ?? component);
		} 
 
		public override Type PropertyType 
		{ 
			get{return this.baseProp.PropertyType;} 
		} 
 
		public override void ResetValue(object component) 
		{ 
			baseProp.ResetValue(_extender ?? component);
		} 
 
		public override void SetValue(object component, object Value) 
		{
            if (Value == null || Value.GetType() == this.baseProp.PropertyType || this.baseProp.PropertyType.IsAssignableFrom(Value.GetType()))
            {
                if (_extender == null)
                    this.baseProp.SetValue(component, Value);
                else
                    this.baseProp.SetValue(_extender, Value);
            }
            else
            {
                if (_extender == null)
                {
                    if (this.baseProp.PropertyType == typeof(DateTimeNULL))
                        this.baseProp.SetValue(component, ConvertDef.ToDateTimeNULL(Value,DBNull.Value));
                    else
                        this.baseProp.SetValue(component, Convert.ChangeType(Value, this.baseProp.PropertyType));
                }
                else
                {
                    if (this.baseProp.PropertyType == typeof(DateTimeNULL))
                        this.baseProp.SetValue(_extender, ConvertDef.ToDateTimeNULL(Value, DBNull.Value));
                    else
                        this.baseProp.SetValue(_extender, Convert.ChangeType(Value, this.baseProp.PropertyType));
                }
            }

		} 
 
		public override bool ShouldSerializeValue(object component) 
		{ 
			return this.baseProp.ShouldSerializeValue(_extender ?? component);
		} 

		#endregion

		public object Extender
		{
			get
			{
				return _extender;
			}
		}

	}  


	/// <summary>
	/// The type decsriptor that is used in conjuction with the lookup property descriptor.
	/// </summary>
	public class LookupTypeDescriptor : ICustomTypeDescriptor 
	{
        private object instance;
        private Type type;

        protected LookupTypeDescriptor()
        {
            this.instance = this;
            this.type = instance.GetType();
        }

        public LookupTypeDescriptor(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException("instance");

            this.instance = instance;
            this.type = instance.GetType();
        }

        public LookupTypeDescriptor(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.type = type;
            this.instance = null;
        }
 
        protected virtual PropertyDescriptorCollection FilterProperties(List<PropertyDescriptor> props)
        {
            return new PropertyDescriptorCollection(props.ToArray());
        }

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] filter) 
		{ 
			PropertyDescriptorCollection baseProps = TypeDescriptor.GetProperties(type, filter); 
			
			List<PropertyDescriptor> props = new List<PropertyDescriptor>();

			//Add the base properties.
			for (int i = 0; i < baseProps.Count; i++) 
			{ 
				props.Add(new LookupPropertyDescriptor(baseProps[i], filter)); 
			} 

			try
			{
				//Add the extender properties.
				if (Session.CurrentSession.EnableAddins)
				{
					object[] extenders = Session.CurrentSession.Addins.GetObjectExtenders(instance ?? type);
					foreach (object ext in extenders)
					{
						try
						{
							PropertyDescriptorCollection extprops = TypeDescriptor.GetProperties(ext);
							for (int i = 0; i < extprops.Count; i++) 
							{ 
								props.Add(new LookupPropertyDescriptor(extprops[i], filter, ext)); 
							}
						}
						catch(Exception ex)
						{
							Console.WriteLine(ex);
						}
					}
				}
			}
			catch(Exception ex)
			{
				Console.Write(ex);
			}
			
			// probably wanna cache this... 
			return FilterProperties(props);
		} 
 
		AttributeCollection ICustomTypeDescriptor.GetAttributes() 
		{ 
			return TypeDescriptor.GetAttributes(type); 
		} 
 

		string ICustomTypeDescriptor.GetClassName() 
		{ 
			return TypeDescriptor.GetClassName(type); 
		} 
       
		string ICustomTypeDescriptor.GetComponentName() 
		{ 
			return TypeDescriptor.GetComponentName(type); 
		} 
 
		TypeConverter ICustomTypeDescriptor.GetConverter() 
		{ 
			return TypeDescriptor.GetConverter(type); 
		} 
 

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() 
		{ 
			return TypeDescriptor.GetDefaultEvent(type); 
		} 

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(System.Attribute[] attributes) 
		{ 
			return TypeDescriptor.GetEvents(type, attributes); 
		} 
 
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents() 
		{ 
			return TypeDescriptor.GetEvents(type); 
		} 
 
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() 
		{ 
			return TypeDescriptor.GetDefaultProperty(type); 
		} 
 
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() 
		{ 
			return ((ICustomTypeDescriptor)this).GetProperties(null); 
		} 
 
		object ICustomTypeDescriptor.GetEditor(System.Type editorBaseType) 
		{ 
			return TypeDescriptor.GetEditor(type, editorBaseType); 
		} 
 
		object ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd) 
		{ 
			return instance; 
		} 
	}

   
}
