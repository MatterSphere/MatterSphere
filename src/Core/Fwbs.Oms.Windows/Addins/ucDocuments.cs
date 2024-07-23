using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.UI.Windows.DocumentManagement;
    using FWBS.OMS.UI.Windows.DocumentManagement.Addins;

    public sealed class ucDocuments : UserControl, Interfaces.IOMSTypeAddin, IDocumentsAddin, IAdvancedPreviewHandler, IFindDocument
	{
        IDocumentsAddin addin;

        public ucDocuments()
        {
        }

        #region IOMSTypeAddin Members

        [DefaultValue("")]
        public string AddinText
        {
            get 
            {
                if (addin == null)
                    return String.Empty;
                else
                    return addin.AddinText; 
            }
        }

        public void Initialise(IOMSType obj)
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);
            addin.Initialise(obj);
        }

        public bool Connect(IOMSType obj)
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);


            return addin.Connect(obj);
        }

        public ucPanelNav[] Panels
        {
            get { return addin.Panels;}
        }

        public ucPanelNav[] GlobalPanels
        {
            get { return addin.GlobalPanels; }
        }

        public Control UIElement
        {
            get 
            {
                if (addin == null)
                    ConstructUIElement(DocumentAddinHost.TypeDisplay);
                   
                return addin.UIElement; 
            }
        }

        private void ConstructUIElement(DocumentAddinHost host)
        {

            if (Session.CurrentSession.IsLoggedIn)
            {
                string type = "";
                
                switch (host)
                {
                    case DocumentAddinHost.DocumentPicker:
                        type = Session.CurrentSession.DocumentPickerAddinOverride;
                        break;
                    case DocumentAddinHost.OpenDialog:
                        type = Session.CurrentSession.DocumentOpenAddinOverride;
                        break;
                    case DocumentAddinHost.TypeDisplay:
                        type = Session.CurrentSession.DocumentAddinOverride;
                        break;
                }

                Type t = Session.CurrentSession.TypeManager.TryLoad(type);
                if (t != null)
                {
                    try
                    {
                        addin = Session.CurrentSession.TypeManager.Create(t) as IDocumentsAddin;
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                    }
                }
            }

            if (addin == null)
                addin = new ucBuiltInDocuments();

            if (Dirty != null)addin.Dirty += new EventHandler(addin_Dirty);
            if (DocumentSelected != null) addin.DocumentSelected += new EventHandler(addin_DocumentSelected);
            if (DocumentSelecting != null) addin.DocumentSelecting += new EventHandler(addin_DocumentSelecting);
            if (DocumentsRefreshed != null) addin.DocumentsRefreshed += new EventHandler(addin_DocumentsRefreshed);
            if (NewOMSTypeWindow != null) addin.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(addin_NewOMSTypeWindow);

            this.Controls.Add(addin.UIElement);
            addin.UIElement.Dock = DockStyle.Fill;
        }


        #endregion

        #region IOMSItem Members

        public void UpdateItem()
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);


            addin.UpdateItem();
        }

        public void RefreshItem()
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);


            addin.RefreshItem();
        }

        public void CancelItem()
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);


            addin.CancelItem();
        }

        public void SelectItem()
        {
            if (addin == null)
                ConstructUIElement(DocumentAddinHost.TypeDisplay);


            addin.SelectItem();
        }

        [DefaultValue(false)]
        public bool IsDirty
        {
            get { return addin.IsDirty; }
        }

        [DefaultValue(false)]
        public bool ToBeRefreshed
        {
            get
            {
                if (addin == null)
                    return false;
                return addin.ToBeRefreshed;
            }
            set
            {
                if (this.addin == null)
                    return;
                addin.ToBeRefreshed = value;
            }
        }

        public event EventHandler Dirty;


        #endregion

        #region IOpenOMSType Members

        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow;

        #endregion

        #region IDocumentsAddin Members

        public void InitialiseHost(DocumentAddinHost host)
        {
            if (addin == null)
                ConstructUIElement(host);

            addin.InitialiseHost(host);
        }

        public OMSDocument[] SelectedDocuments
        {
            get { return addin.SelectedDocuments; }
        }

        public string[] SelectedDocumentIds
        {
            get { return addin.SelectedDocumentIds; }
        }

        public int DocumentCount
        {
            get { return addin.DocumentCount; }
        }

        public string GetCurrentDocumentDetailsAsRTF()
        {
            return addin.GetCurrentDocumentDetailsAsRTF();
        }

        public bool SupportsView(DocumentPickerType view)
        {
            return addin.SupportsView(view);
        }

        public void ShowView(DocumentPickerType view, IOMSType obj)
        {
            addin.ShowView(view, obj);
        }

        public DocumentPickerType DefaultView
        {
            get
            {
                return addin.DefaultView;
            }
        }

        public void LoadPreview()
        {
            IAdvancedPreviewHandler preview = addin as IAdvancedPreviewHandler;
            if (preview != null)
                preview.LoadPreview();
        }

        public void UnloadPreview()
        {
            IAdvancedPreviewHandler preview = addin as IAdvancedPreviewHandler;
            if (preview != null)
                preview.UnloadPreview();
        }

        public event EventHandler DocumentSelecting;
        public event EventHandler DocumentSelected;
        public event EventHandler DocumentsRefreshed;


        #endregion



        void addin_DocumentsRefreshed(object sender, EventArgs e)
        {
            if (DocumentsRefreshed != null)
                DocumentsRefreshed(sender, e);
        }

        void addin_DocumentSelecting(object sender, EventArgs e)
        {
            if (DocumentSelecting != null)
                DocumentSelecting(sender, e);
        }

        void addin_DocumentSelected(object sender, EventArgs e)
        {
            if (DocumentSelected != null)
                DocumentSelected(sender, e);
        }

        void addin_Dirty(object sender, EventArgs e)
        {
            if (Dirty != null)
                Dirty(sender, e);
        }


        void addin_NewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (NewOMSTypeWindow != null)
                NewOMSTypeWindow(sender, e);
        }

        #region IFindDocument Members

        public OMSDocument Find(string docId)
        {
            IFindDocument find = addin as IFindDocument;
            if (find == null)
                return OMSDocument.GetDocument(docId, Session.CurrentSession.DuplicateDocumentIDs);

            return find.Find(docId);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (addin != null)
                {
                    addin.Dirty -= new EventHandler(addin_Dirty);
                    addin.DocumentSelected -= new EventHandler(addin_DocumentSelected);
                    addin.DocumentSelecting -= new EventHandler(addin_DocumentSelecting);
                    addin.DocumentsRefreshed -= new EventHandler(addin_DocumentsRefreshed);
                    addin.NewOMSTypeWindow -= new NewOMSTypeWindowEventHandler(addin_NewOMSTypeWindow);

                    IDisposable dispose = addin as IDisposable;
                    if (dispose != null)
                        dispose.Dispose();
                    addin = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
