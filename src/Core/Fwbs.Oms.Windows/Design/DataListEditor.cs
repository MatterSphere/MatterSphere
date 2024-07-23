using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FWBS.OMS.Design
{
    public class DataListEditor : UITypeEditor
    {
        private IWindowsFormsEditorService iWFES;


        public DataListEditor() { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext ctx)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            string displayMember;
            string valueMember;
            DataListAttribute dla;
            DataTable data;
            FWBS.OMS.EnquiryEngine.DataLists dl = DataListConverter.GetData(context, out data, out valueMember, out displayMember, out dla);

            if (dl == null)
            {
                return value;
            }
            else
            {
                iWFES = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                FWBS.OMS.UI.Windows.Design.frmListSelector frmListSelector1 = new FWBS.OMS.UI.Windows.Design.frmListSelector();
                frmListSelector1.Text = dl.Description;
                frmListSelector1.List.DisplayMember = displayMember;
                frmListSelector1.List.ValueMember = valueMember;
                frmListSelector1.List.DataSource = data;
                frmListSelector1.List.SelectedValue = value;

                iWFES.ShowDialog(frmListSelector1);

                if (frmListSelector1.DialogResult == DialogResult.OK)
                {
                    value = frmListSelector1.List.SelectedValue;
                }
            }
            return value;
        }


       
    }

  
}