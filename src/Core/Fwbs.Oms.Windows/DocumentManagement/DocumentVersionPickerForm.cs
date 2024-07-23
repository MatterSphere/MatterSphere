using System;
using System.Collections.Generic;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    internal partial class DocumentVersionPickerForm : BaseForm
    {
        internal DocumentVersionPickerForm()
        {
            InitializeComponent();
        }


        public DocumentVersionPickerForm(OMSDocument doc) : this()
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            this.Icon = doc.GetIcon();

            textBox1.Text = doc.ID.ToString() + " - " + doc.Description;
            this.versions.Initialise(doc);
            this.versions.Connect(doc);
            this.versions.DocumentVersionsControl.InfoPanel.Visible = true;
            this.versions.RefreshItem();
        }

        private void DocumentVersionPickerForm_Shown(object sender, EventArgs e)
        {
            this.versions.SelectItem();
        }


        private List<IStorageItemVersion> selected = new List<IStorageItemVersion>();
        public IStorageItemVersion[] SelectedVersions
        {
            get
            {
                return selected.ToArray();
            }
        }

        public bool MultiSelect
        {
            get
            {
                return versions.DocumentVersionsControl.MultiSelect;
            }
            set
            {
                versions.DocumentVersionsControl.MultiSelect = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            selected.AddRange(versions.DocumentVersionsControl.SelectedVersions);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            selected.Clear();
        }
    }
}
