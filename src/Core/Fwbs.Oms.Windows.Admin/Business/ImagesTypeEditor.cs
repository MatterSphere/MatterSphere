using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    internal class ImagesTypeEditor: UITypeEditor
    {
        //A class to hold our OpenFileDialog Settings
        public class ImagesTypeEditorAttribute : Attribute
        {
            public ImagesTypeEditorAttribute(string sFileFilter, string sDialogTitle)
            {
                m_Filter = sFileFilter;
                m_Title = sDialogTitle;
            }

            //The File Filter(s) of the open dialog
            private string m_Filter;
            public string Filter
            {
                get { return m_Filter; }
                set { m_Filter = value; }
            }

            //The Title of the open dialog
            private string m_Title;
            public string Title
            {
                get { return m_Title; }
                set { m_Title = value; }
            }
        }

        //The default settings for the file dialog
        private ImagesTypeEditorAttribute m_Settings = new ImagesTypeEditorAttribute("All Files (*.*)|*.*", "Open");
        public ImagesTypeEditorAttribute Settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        //Define a modal editor style and capture the settings from the property
        public override UITypeEditorEditStyle GetEditStyle(
                    ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
                return base.GetEditStyle(context);

            //Retrieve our settings attribute (if one is specified)
            ImagesTypeEditorAttribute sa = (ImagesTypeEditorAttribute)context.PropertyDescriptor.Attributes[typeof(ImagesTypeEditorAttribute)];
            if (sa != null)
                m_Settings = sa; //Store it in the editor
            return UITypeEditorEditStyle.Modal;
        }

        //Do the actual editing
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || context.Instance == null || provider == null)
                return value;

            //Initialize the file dialog with our settings
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = m_Settings.Filter;
            dlg.CheckFileExists = true;
            dlg.Title = m_Settings.Title;
            FWBS.OMS.Images img = value as FWBS.OMS.Images;
            string filename;
            if (img != null)
                dlg.FileName = img.Text;
            //Display the dialog and change the value if confirmed
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filename = dlg.FileName;
                img = new FWBS.OMS.Images();
                Image i = Image.FromFile(filename);
                img.Image = i;
                if (filename.Length < 50)
                    img.Text = filename;
                else
                    img.Text = new FileInfo(filename).Name;
                return img;
            }
            else
                return value;
        }
    }
}
