using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;


namespace FWBS.OMS.UI.Windows
{
    public class ResourceLookupItemConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext ctx, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            else
                return base.CanConvertTo(ctx, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                ResourceLookupItem gf = value as ResourceLookupItem;
                if (gf != null)
                {
                    Type[] parms = new Type[] { typeof(String), typeof(String), typeof(String) };
                    ConstructorInfo ci = typeof(ResourceLookupItem).GetConstructor(parms);
                    return new InstanceDescriptor(ci, new object[] { gf.Code, gf.Description, gf.Help });
                }
                else
                    return base.ConvertTo(ctx, culture, value, destinationType);
            }
            else
                return base.ConvertTo(ctx, culture, value, destinationType);
        }
    }

}
