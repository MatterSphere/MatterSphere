using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public sealed class DocumentPicker
    {
        #region Fields

        private DocumentPickerType type;
        private Client client;
        private OMSFile file;
        private string title;

        #endregion

        public DocumentPicker ()
		{

		}

        public OMSDocument[] Show(IWin32Window owner)
        {
            return Show(owner, null);
        }

        public OMSDocument[] Show(IWin32Window owner, Common.KeyValueCollection parameters)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                if (client == null)
                    client = Session.CurrentSession.CurrentClient;
                if (file == null)
                    file = Session.CurrentSession.CurrentFile;

                using (DocumentPickerForm dlg = new DocumentPickerForm(type, client, file, parameters))
                {
                    if (!String.IsNullOrEmpty(title))
                        dlg.Text = title;

                    if (dlg.ShowDialog(owner) == DialogResult.OK)
                        return dlg.SelectedDocuments;
                    else
                        return null;
                }
            }

            return null;

        }

        public System.IO.FileInfo[] ShowLocal(IWin32Window owner)
        {
            return ShowLocal(owner, false);
        }

        public System.IO.FileInfo[] ShowLocal(IWin32Window owner, bool changes)
        {
            using (OfflineDocumentPicker dlg = new OfflineDocumentPicker(changes))
            {
                if (!String.IsNullOrEmpty(title))
                    dlg.Text = title;

                if (dlg.ShowDialog(owner) == DialogResult.OK)
                    return dlg.SelectedDocuments;
                else
                    return null;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public DocumentPickerType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public Client Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }

        public OMSFile File
        {
            get
            {
                return file;
            }
            set
            {
                file = value;
            }
        }
    }
}
