using System.IO;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class DocumentsManagement : RestApiManagement
    {
        public DocumentsManagement(IRestApiClient restApiClient) : base(restApiClient)
        {}

        public DocumentOperations GetDocumentOperations(string documentId, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            return _restApiClient.ExecuteRequest<DataResponse<DocumentOperations>>($"{GetRootUrl(library)}/{documentId}/operations", HttpMethod.Get).Data;
        }

        public DocumentProfile GetDocumentProfile(string documentId, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            return _restApiClient.ExecuteRequest<DataResponse<DocumentProfile>>($"{GetRootUrl(library)}/{documentId}", HttpMethod.Get).Data;
        }

        public DocumentProfile ReplaceDocumentContent(string documentId, string localFilePath, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            ValidateUriParameter(nameof(localFilePath), localFilePath);
            var documentProfile = GetDocumentProfile(documentId, library);
            FileInfo fileInfo = new FileInfo(localFilePath);
            documentProfile.Size = (int)fileInfo.Length;
            return _restApiClient.ExecuteRequest<DataResponse<DocumentProfile>>($"{GetRootUrl(library)}/{documentId}/file", HttpMethod.Put, new PostDocumentProfileProperties
                    {Warnings = false, DocProfile = documentProfile}, localFilePath).Data;
        }

        public DocumentCheckOut GetDocumentCheckOut(string documentId, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            return _restApiClient.ExecuteRequest<DataResponse<DocumentCheckOut>>($"{GetRootUrl(library)}/{documentId}/checkout", HttpMethod.Get).Data;
        }

        public void PostDocumentLock(string documentId, PostDocumentLockProperties postDocumentLockProperties, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            _restApiClient.ExecuteRequest<DataResponse<object>>($"{GetRootUrl(library)}/{documentId}/lock", HttpMethod.Post, postDocumentLockProperties);
        }

        public void DeleteDocumentLock(string documentId, string library = null)
        {
            ValidateUriParameter(nameof(documentId), documentId);
            _restApiClient.ExecuteRequest<DataResponse<object>>($"{GetRootUrl(library)}/{documentId}/lock", HttpMethod.Delete);
        }

        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/documents";
        }
    }
    
}