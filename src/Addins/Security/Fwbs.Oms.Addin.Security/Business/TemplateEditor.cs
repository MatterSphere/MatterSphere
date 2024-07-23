using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using FWBS.OMS.Addin.Security.Windows;

namespace FWBS.OMS.Addin.Security
{
    internal class TemplateEditor : UITypeEditor
    {
        private IWindowsFormsEditorService iWFES;
        public TemplateEditor() { }


        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (FWBS.OMS.Session.CurrentSession.AdvancedSecurity)
            {

                iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                FWBS.OMS.Security.TemplateSecurity myvalue = value as FWBS.OMS.Security.TemplateSecurity;
                string code = myvalue.Code;
                string type = myvalue.Type;
                frmSecurityDialog frm = new frmSecurityDialog(type, code);
                iWFES.ShowDialog(frm);
                if (frm.DialogResult == DialogResult.OK)
                {
                    myvalue.HasSecurity = true;
                }
                return myvalue;
            }
            else
            {
                FWBS.OMS.UI.Windows.MessageBox.ShowInformation("ADVSECDIS", "This feature is not available on Basic Security.  Please contact Sales for Information on Advanced Security");
                return value;
            }
        }
    }

    /// <summary>
    /// Summary description for DataBuilderEditor.
    /// </summary>
    internal class TemplateConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type sourceType)
        {
            if (sourceType == typeof(System.String))
                return true;
            else
                return base.CanConvertFrom(ctx, sourceType);
        }

        internal static bool running = false;
        
        public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(System.String))
            {
                FWBS.OMS.OMSType thetype = ctx.Instance as FWBS.OMS.OMSType;
                if (thetype != null)
                {
                    string data = value as string;
                    if (data == "")
                    {
                        FWBS.OMS.Security.TemplateSecurity newvalue = new FWBS.OMS.Security.TemplateSecurity(thetype.GetType().Name, thetype.Code);
                        thetype.IsDirty = true;
                        thetype.OnDirty();

                        newvalue.ClearSecurity();
                        return newvalue;
                    }
                    else
                        return base.ConvertFrom(ctx, culture, value);
                }
                else
                    return base.ConvertFrom(ctx, culture, value);

            }
            else
                return base.ConvertFrom(ctx, culture, value);
        }
    }
}
