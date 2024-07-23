using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class FoldersManagement : RestApiManagement
    {
        public FoldersManagement(IRestApiClient restApiClient) : base(restApiClient)
        {}

        public List<DocumentProfile> GetFolderDocuments(string folderId, SearchDocumentsProperties searchDocumentsProperties, string library = null)
        {
            ValidateUriParameter(nameof(folderId), folderId);
            // GET /work/api/v2/customers/{customerId}/libraries/{libraryId}/folders/{folderId}/children
            return _restApiClient.ExecuteRequest<DataResponse<List<DocumentProfile>>>($"{GetRootUrl(library)}/{folderId}/children", HttpMethod.Get, searchDocumentsProperties).Data;
        }

        public DocumentProfile PostFolderDocument(string folderId, DocumentProfile documentProfile, string localFilePath, string library = null)
        {
            ValidateUriParameter(nameof(folderId), folderId);
            return _restApiClient.ExecuteRequest<DataResponse<DocumentProfile>>($"{GetRootUrl(library)}/{folderId}/documents", HttpMethod.Post, new PostDocumentProfileProperties
                { Warnings = true, DocProfile = documentProfile }, localFilePath).Data;
        }

        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/folders";
        }
    }

}
