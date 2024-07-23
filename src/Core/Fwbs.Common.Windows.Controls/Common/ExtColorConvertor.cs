using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Reflection;


namespace FWBS.Common.UI.Windows
{
    public class ExtColorConverter : ExpandableObjectConverter
    {
        public ExtColorConverter()
        {
        }

        public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type destinationType)
        {
            if (destinationType == typeof(System.String))
                return true;
            else
                return base.CanConvertFrom(ctx, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo culture, object value)
        {
            if (value != null && value.GetType() == typeof(System.String))
            {
                string color = value as string;
                int colorno = 0;
                if (String.IsNullOrEmpty(color) == false)
                {
                    Color newcolor = Color.FromName(color);
                    if (newcolor.IsKnownColor == false)
                    {
                        if (Int32.TryParse(color, out colorno))
                            newcolor = Color.FromArgb(colorno);
                        else
                        {
                            string[] settings = color.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            ExtColorPresets preset = (ExtColorPresets)ConvertDef.ToEnum(settings[0], ExtColorPresets.None);
                            ExtColorTheme theme = (ExtColorTheme)ConvertDef.ToEnum(settings[1], ExtColorTheme.Auto);
                            return new ExtColor(preset, theme);
                        }
                    }
                    return new ExtColor(newcolor,ExtColorPresets.None, ExtColorTheme.None);
                }
                else
                    return new ExtColor();
            }
            else
                return base.ConvertFrom(ctx, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext ctx, Type destinationType)
        {
            if (destinationType == typeof(System.String))
                return true;
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            else
                return base.CanConvertTo(ctx, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String))
            {
                ExtColor extc = value as ExtColor;
                if (extc != null)
                {
                    string output = "";
                    if (extc.Presets != ExtColorPresets.None)
                        output = extc.Presets.ToString() + ", " + extc.ThemeXP.ToString();
                    else if (extc.SetColor.IsKnownColor)
                        output = extc.SetColor.Name;
                    else
                        output = Convert.ToString(extc.SetColor.ToArgb());
                    return output;
                }
                else
                    return base.ConvertTo(ctx, culture, value, destinationType);
            }
            else if (destinationType == typeof(InstanceDescriptor))
            {
                ExtColor gf = value as ExtColor;
                if (gf != null)
                {
                    if (gf.Presets == ExtColorPresets.None)
                    {
                        Type[] parms = new Type[] { typeof(Color) };
                        ConstructorInfo ci = typeof(ExtColor).GetConstructor(parms);
                        return new InstanceDescriptor(ci, new object[] { gf.SetColor });
                    }
                    else
                    {
                        Type[] parms = new Type[] { typeof(ExtColorPresets), typeof(ExtColorTheme) };
                        ConstructorInfo ci = typeof(ExtColor).GetConstructor(parms);
                        return new InstanceDescriptor(ci, new object[] { gf.Presets, gf.ThemeXP });
                    }
                }
                else
                    return base.ConvertTo(ctx, culture, value, destinationType);
            }
            else
                return base.ConvertTo(ctx, culture, value, destinationType);
        }
    }
}
