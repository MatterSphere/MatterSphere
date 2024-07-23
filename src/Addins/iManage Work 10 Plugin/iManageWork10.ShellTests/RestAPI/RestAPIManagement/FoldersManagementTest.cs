using System;
using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.RestAPI.RestAPIManagement
{
    [TestFixture]
    class FoldersManagementTest
    {

        private const string LIBRARY = "preflibrary";

        private IRestApiClient _restApiClient;

        private FoldersManagement _foldersManagement;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _foldersManagement = new FoldersManagement(_restApiClient);
        }

        #region GetFolderDocuments

        [Test]
        public void GetFolderDocuments_FolderIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _foldersManagement.GetFolderDocuments(null, new SearchDocumentsProperties()));
        }

        [Test]
        public void GetFolderDocuments_FolderIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _foldersManagement.GetFolderDocuments(string.Empty, new SearchDocumentsProperties()));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: folderId"));
            Assert.That(ex.ParamName, Is.EqualTo("folderId"));
        }

        [Test]
        [TestCase("folder", null)]
        [TestCase("folder", "")]
        [TestCase("1", "not_default_library")]
        public void GetFolderDocuments_CorrectUrlBuilt(string folderId, string library)
        {
            SearchDocumentsProperties searchDocumentsProperties = new SearchDocumentsProperties();

            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<DocumentProfile>>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{folderId}/documents"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get), 
                    Arg<object>.Is.Equal(searchDocumentsProperties),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<DocumentProfile>>());

            _foldersManagement.GetFolderDocuments(folderId, searchDocumentsProperties, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region PostFolderDocument

        [Test]
        public void PostFolderDocument_FolderIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _foldersManagement.PostFolderDocument(null, new DocumentProfile(), null));
        }

        [Test]
        public void PostFolderDocument_FolderIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _foldersManagement.PostFolderDocument(string.Empty, new DocumentProfile(), null));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: folderId"));
            Assert.That(ex.ParamName, Is.EqualTo("folderId"));
        }

        [Test]
        [TestCase("folder", "filepath", null)]
        [TestCase("folder", "filepath", "")]
        [TestCase("1", "filepath", "not_default_library")]
        public void PostFolderDocument_CorrectUrlBuilt(string folderId, string localFilePath, string library)
        {
            DocumentProfile documentProfile = new DocumentProfile();
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentProfile>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{folderId}/documents"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Post),
                    Arg<DocumentProfile>.Is.NotNull,
                    Arg<string>.Is.Equal(localFilePath)))
                .Return(new DataResponse<DocumentProfile>());

            _foldersManagement.PostFolderDocument(folderId, documentProfile, localFilePath, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY : library)}/folders";
        }

    }
}
