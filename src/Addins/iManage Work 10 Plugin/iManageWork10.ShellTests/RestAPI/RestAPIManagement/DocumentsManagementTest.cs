using System;
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
    class DocumentsManagementTest
    {

        private const string LIBRARY = "preflibrary";

        private IRestApiClient _restApiClient;

        private DocumentsManagement _documentsManagement;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _documentsManagement = new DocumentsManagement(_restApiClient);
        }

        #region GetDocumentOperations

        [Test]
        public void GetDocumentOperations_DocumentIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.GetDocumentOperations(null));
        }

        [Test]
        public void GetDocumentOperations_DocumentIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _documentsManagement.GetDocumentOperations(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(ex.ParamName, Is.EqualTo("documentId"));
        }

        [Test]
        [TestCase("doc", null)]
        [TestCase("doc1", "")]
        [TestCase("1", "not_default_library")]
        public void GetDocumentOperations_CorrectUrlBuilt(string documentId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentOperations>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}/operations"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<DocumentOperations>());

            _documentsManagement.GetDocumentOperations(documentId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region GetDocumentProfile

        [Test]
        public void GetDocumentProfile_DocumentIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.GetDocumentProfile(null));
        }

        [Test]
        public void GetDocumentProfile_DocumentIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _documentsManagement.GetDocumentProfile(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(ex.ParamName, Is.EqualTo("documentId"));
        }

        [Test]
        [TestCase("doc", null)]
        [TestCase("doc1", "")]
        [TestCase("1", "not_default_library")]
        public void GetDocumentProfile_CorrectUrlBuilt(string documentId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentProfile>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<DocumentProfile>());

            _documentsManagement.GetDocumentProfile(documentId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region ReplaceDocumentContent

        [Test]
        public void ReplaceDocumentContent_DocumentIdOrLocalFilePathNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.ReplaceDocumentContent(null, @"c:\tmp\file.docx"));
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.ReplaceDocumentContent("id", null));
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.ReplaceDocumentContent(null, null));
        }

        [Test]
        public void ReplaceDocumentContent_DocumentIdOrLocalFilePathEmpty_ArgumentExceptionThrown()
        {
            ArgumentException exDocId = Assert.Throws<ArgumentException>(() => _documentsManagement.ReplaceDocumentContent(string.Empty, @"c:\tmp\file.docx"));
            Assert.That(exDocId.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(exDocId.ParamName, Is.EqualTo("documentId"));

            ArgumentException exFilePath = Assert.Throws<ArgumentException>(() => _documentsManagement.ReplaceDocumentContent("id", string.Empty));
            Assert.That(exFilePath.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: localFilePath"));
            Assert.That(exFilePath.ParamName, Is.EqualTo("localFilePath"));
        }

        [Test]
        [Ignore("Local file should exist.")]
        [TestCase("doc", @"c:\tmp\cache.txt", null)]
        [TestCase("doc1", @"c:\tmp\cache.txt", "")]
        [TestCase("1", @"c:\tmp\cache.txt", "not_default_library")]
        public void ReplaceDocumentContent_CorrectUrlBuilt(string documentId, string localFilePath, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Repeat.Twice().Return(LIBRARY);
            }

            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentProfile>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get), 
                    Arg<string>.Is.Null, 
                    Arg<string>.Is.Null))
                .Return(new DataResponse<DocumentProfile> { Data = new DocumentProfile()});

            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentProfile>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}/file"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Put), 
                    Arg<PostDocumentProfileProperties>.Is.NotNull, 
                    Arg<string>.Is.Equal(localFilePath)))
                .Return(new DataResponse<DocumentProfile>());

            _documentsManagement.ReplaceDocumentContent(documentId, localFilePath, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region GetDocumentCheckOut

        [Test]
        public void GetDocumentCheckOut_DocumentIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.GetDocumentCheckOut(null));
        }

        [Test]
        public void GetDocumentCheckOut_DocumentIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _documentsManagement.GetDocumentCheckOut(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(ex.ParamName, Is.EqualTo("documentId"));
        }

        [Test]
        [TestCase("doc", null)]
        [TestCase("doc1", "")]
        [TestCase("1", "not_default_library")]
        public void GetDocumentCheckOut_CorrectUrlBuilt(string documentId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<DocumentCheckOut>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}/checkout"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<DocumentCheckOut>());

            _documentsManagement.GetDocumentCheckOut(documentId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region PostDocumentLock

        [Test]
        public void PostDocumentLock_DocumentIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.PostDocumentLock(null, new PostDocumentLockProperties()));
        }

        [Test]
        public void PostDocumentLock_DocumentIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _documentsManagement.PostDocumentLock(string.Empty, new PostDocumentLockProperties()));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(ex.ParamName, Is.EqualTo("documentId"));
        }

        [Test]
        [TestCase("doc", null)]
        [TestCase("doc1", "")]
        [TestCase("1", "not_default_library")]
        public void PostDocumentLock_CorrectUrlBuilt(string documentId, string library)
        {
            var postDocumentLock = new PostDocumentLockProperties();
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<object>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}/lock"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Post), 
                    Arg<object>.Is.Equal(postDocumentLock),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<object>());

            _documentsManagement.PostDocumentLock(documentId, postDocumentLock, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region DeleteDocumentLock

        [Test]
        public void DeleteDocumentLock_DocumentIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _documentsManagement.DeleteDocumentLock(null));
        }

        [Test]
        public void DeleteDocumentLock_DocumentIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _documentsManagement.DeleteDocumentLock(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: documentId"));
            Assert.That(ex.ParamName, Is.EqualTo("documentId"));
        }

        [Test]
        [TestCase("doc", null)]
        [TestCase("doc1", "")]
        [TestCase("1", "not_default_library")]
        public void DeleteDocumentLock_CorrectUrlBuilt(string documentId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<object>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{documentId}/lock"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Delete),
                    Arg<object>.Is.Null, 
                    Arg<string>.Is.Null))
                .Return(new DataResponse<object>());

            _documentsManagement.DeleteDocumentLock(documentId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY : library)}/documents";
        }
    }
}
