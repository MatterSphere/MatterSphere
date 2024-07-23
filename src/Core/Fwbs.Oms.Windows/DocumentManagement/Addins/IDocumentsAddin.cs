using System;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{
    public interface IDocumentsAddin : Interfaces.IOMSTypeAddin
    {
        void InitialiseHost(DocumentAddinHost host);

        OMSDocument[] SelectedDocuments { get;}
        string[] SelectedDocumentIds { get;}
        int DocumentCount{ get;}

        string GetCurrentDocumentDetailsAsRTF();

        bool SupportsView(DocumentPickerType view);
        DocumentPickerType DefaultView { get;}
        void ShowView(DocumentPickerType view, FWBS.OMS.Interfaces.IOMSType obj);


        event EventHandler DocumentSelecting;
        event EventHandler DocumentSelected;

        event EventHandler DocumentsRefreshed;
    }
}
