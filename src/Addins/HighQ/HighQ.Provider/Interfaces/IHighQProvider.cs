using System;
using System.Data;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;

namespace FWBS.OMS.HighQ.Interfaces
{
    internal interface IHighQProvider
    {
        event EventHandler<TokensEventArgs> TokensUpdated;

        string AccessToken { get; }
        string RefreshToken { get; }

        void Build(int clientId, string secret, string site, string redirectUri, TokenDetails tokenDetails);
        SheetItemsResponse GetISheetsCollection(int iSheetId);
        DataTable GetISheetsDataTable(int iSheetId);
        int GetFolderId(FolderDetails details);
        FolderInfoResponse GetFolderInfo(int folderId);
        int UploadDocument(DocumentDetails details);
        FolderItem[] GetFolders(int rootFolder);
    }
}
