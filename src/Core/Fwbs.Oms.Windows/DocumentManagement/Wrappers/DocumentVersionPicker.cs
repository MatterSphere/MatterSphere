using System;
using System.Windows.Forms;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public sealed class DocumentVersionPicker
    {

        public DocumentVersionPicker()
        {
        }

        public IStorageItemVersion[] Show(IWin32Window owner)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                if (Document == null)
                {
                    DocumentPicker docpicker = new DocumentPicker();

                    OMSDocument[] docs = docpicker.Show(owner);
                    if (docs == null)
                        return null;

                    Document = docs[0];
                }
                using (DocumentVersionPickerForm dlg = new DocumentVersionPickerForm(Document))
                {
                    if (!String.IsNullOrEmpty(Title))
                        dlg.Text = Title;

                    dlg.MultiSelect = MultiSelect;
                    
                    if (dlg.ShowDialog(owner) == DialogResult.OK)
                        return dlg.SelectedVersions;
                    else
                        return null;
                }
            }

            return null;

        }

        public string Title { get; set; }
        public OMSDocument Document{get;set;}
        public bool MultiSelect { get; set; }
        

    }
}
