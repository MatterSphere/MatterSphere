using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows
{

    public class ResourceLookupItemEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Console.WriteLine(value);
            if (value == null)
            {
                string _name = "";
                string _text = "";
                try { _name = Convert.ToString(TypeDescriptor.GetProperties(context.Instance)["Name"].GetValue(context.Instance)); }
                catch
                { }
                try { _text = Convert.ToString(TypeDescriptor.GetProperties(context.Instance)["Text"].GetValue(context.Instance)); }
                catch
                { }
                return new FWBS.OMS.UI.Windows.ResourceLookupItem(_name, _text, "");
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("Do you wish to remove the Resource Lookup", "Resource Lookup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return null;
                else
                    return value;
            }
        }
    }

}
