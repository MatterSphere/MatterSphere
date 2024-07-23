using System;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using HighQ.Test.Common;
using Moq;
using NUnit.Framework;

namespace HighQ.Test
{
    [TestFixture]
    public class HighQTest
    {
        [Test]
        public void TestUploadDocument()
        {
            long docId = 1;
            long clientId = 2;
            string clientNo = "F9";
            string fileNo = "8";
            string documentPath = "documentPath";
            string documentDescription = "Document Description";
            var documentInfo = new DocumentInfo(clientId, clientNo, fileNo, documentPath, documentDescription);
            string accessToken = "AccessToken";
            string refreshToken = "RefreshToken";
            DateTime accessTokenExpiresAt = DateTime.UtcNow.AddDays(1);
            var tokenDetails = new TokenDetails(accessToken, refreshToken, accessTokenExpiresAt);
            int iSheetId = 5;
            int folderId = 6;
            int hqDocumentId = 7;

            var hqProviderMock = new Mock<IHighQProvider>();
            hqProviderMock.Setup(provider =>
                provider.Build(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TokenDetails>())).Verifiable();
            hqProviderMock.Setup(provider => provider.GetFolderId(It.IsAny<FolderDetails>()))
                .Returns(() => folderId);
            hqProviderMock.Setup(provider => provider.UploadDocument(It.IsAny<DocumentDetails>()))
                .Returns(() => hqDocumentId);

            var dbProviderMock = new Mock<IDbProvider>();
            dbProviderMock.Setup(provider => provider.GetDocumentInfo(It.IsAny<long>()))
                .Returns(() => documentInfo);
            dbProviderMock.Setup(provider => provider.GetOMSFileEntityId(It.IsAny<long>()))
                .Returns(() => iSheetId);
            dbProviderMock.Setup(provider => provider.GetUserTokens(It.IsAny<int>()))
                .Returns(() => tokenDetails);

            var sessionMockProvider = new SessionMockProvider();
            var sessionProviderMock = sessionMockProvider.GetMock();
            var folderPickerMock = new Mock<ITargetFolderPicker>();

            var highQ = new FWBS.OMS.HighQ.HighQ(hqProviderMock.Object, dbProviderMock.Object, sessionProviderMock.Object, folderPickerMock.Object);

            var result = highQ.UploadDocuments(new []{docId});

            hqProviderMock.Verify(m => m.Build(It.Is<int>(arg => arg == sessionMockProvider.HQClient),
                It.Is<string>(arg => arg == sessionMockProvider.HQSecret),
                It.Is<string>(arg => arg == sessionMockProvider.HQSite),
                It.Is<string>(arg => arg == sessionMockProvider.HQRedirectUri),
                It.Is<TokenDetails>(arg => arg == tokenDetails)));

            hqProviderMock.Verify(m => m.GetFolderId(It.Is<FolderDetails>(arg => arg.iSheetId == iSheetId
                                                                                 && arg.ClientNo == clientNo
                                                                                 && arg.OmsFileNo == fileNo
                                                                                 && arg.ClientColumnTitle ==
                                                                                 sessionMockProvider.ClientColumnTitle
                                                                                 && arg.OmsFileColumnTitle ==
                                                                                 sessionMockProvider.FileColumnTitle
                                                                                 && arg.FolderColumnTitle ==
                                                                                 sessionMockProvider
                                                                                     .FolderColumnTitle)));
            hqProviderMock.Verify(m => m.UploadDocument(
                It.Is<DocumentDetails>(arg => arg.DocumentId == docId
                                              && arg.Path == documentPath
                                              && arg.Description == documentDescription
                                              && arg.HQFolderId == folderId)));
            dbProviderMock.Verify(m => m.GetDocumentInfo(It.Is<long>(arg => arg == docId)));
            dbProviderMock.Verify(m => m.GetOMSFileEntityId(It.Is<long>(arg => arg == clientId)));
            dbProviderMock.Verify(m => m.GetUserTokens(It.Is<int>(arg => arg == sessionMockProvider.MSUserId)));
            Assert.IsNull(result[docId]);
        }
    }
}
