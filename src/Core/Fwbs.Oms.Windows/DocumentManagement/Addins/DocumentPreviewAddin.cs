using System;
using System.ComponentModel;
using System.Diagnostics;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    public partial class DocumentPreviewAddin : ucBaseAddin
    {

        private bool displaydocumentdescription = true;
        [DefaultValue(true)]
        public bool DislayDocumentDescription
        {
            get
            {
                return displaydocumentdescription;
            }
            set
            {
                displaydocumentdescription = value;
            }
        }

        public DocumentPreviewAddin()
        {
            Debug.WriteLine("DocumentPreviewAddin Constructor...");
            Debug.WriteLine("DocumentPreviewAddin Init Component...");
			
            InitializeComponent();
            Debug.WriteLine("DocumentPreviewAddin End Init Component...");
            Debug.WriteLine("DocumentPreviewAddin End Constructor...");


            tbDescription.Visible = displaydocumentdescription;
        }


        private FWBS.OMS.DocumentManagement.Storage.IStorageItem storageItem = null;
        private object obj;

        public bool Connect(FWBS.OMS.DocumentManagement.Storage.IStorageItem item)
        {
            return Connect((object)item);
        }

        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            return Connect((object)obj);
        }

        private bool Connect(object obj)
        {
            Debug.WriteLine("DocumentPreviewAddin Connect...");

            if (obj == this.obj)
                return false;

            this.obj = obj;
            this.storageItem = obj as FWBS.OMS.DocumentManagement.Storage.IStorageItem;

            ToBeRefreshed = true;
            Debug.WriteLine("DocumentPreviewAddin End Connect...");
            return storageItem != null;

        }

        public override void RefreshItem()
        {
            if (!ToBeRefreshed)
                return;

            Debug.WriteLine("DocumentPreviewAddin Refreshitem...");
            tbDescription.Visible = false;
            if (storageItem != null)
            {
                lblError.Visible = false;

                try
                {
                    previewer1.PreviewFile(storageItem);
                
                    previewer1.Visible = true;
                    txtPreview.Visible = false;
                    previewer1.BringToFront();
                }
                catch(NotSupportedException)
                {
                    previewer1.Visible = false;


                    txtPreview.Clear();
                    try
                    {

                        txtPreview.Rtf = storageItem.Preview;
                    }
                    catch
                    {
                        try
                        {
                            txtPreview.Text = storageItem.Preview;
                        }
                        catch
                        {
                            txtPreview.Text = "";
                        }
                    }
                    txtPreview.Visible = true;

                }
            }
            else
            {
                previewer1.Visible = false;
                txtPreview.Visible = false;
            }

            ToBeRefreshed = false;
            Debug.WriteLine("DocumentPreviewAddin End Refreshitem...");
           
        }

        public void SetError(string errorMessage)
        {
            ToBeRefreshed = true;
            storageItem = null;
            RefreshItem();
            lblError.Text = errorMessage;
            lblError.Visible = true;
        }

        public void UnloadPreview()
        {
            previewer1.Unload();
            ToBeRefreshed = true;
        }

        public void LoadPreview()
        {
            previewer1.Load();
        }

    }
}
