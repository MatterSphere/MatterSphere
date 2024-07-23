using FWBS.OMS.HighQ.Interfaces;
using Moq;

namespace HighQ.Test.Common
{
    internal class SessionMockProvider
    {
        public SessionMockProvider()
        {
            MSUserId = 1;
            HQClient = 2;
            HQSecret = "secret";
            HQSite = "site";
            ClientColumnTitle = "ClientNo";
            FileColumnTitle = "FileNo";
            FolderColumnTitle = "ExportFolder";
        }

        public int MSUserId { get; set; }
        public int HQClient { get; set; }
        public string HQSecret { get; set; }
        public string HQSite { get; set; }
        public string HQRedirectUri { get; set; }
        public string ClientColumnTitle { get; set; }
        public string FileColumnTitle { get; set; }
        public string FolderColumnTitle { get; set; }

        internal Mock<ISessionProvider> GetMock()
        {
            var sessionProviderMock = new Mock<ISessionProvider>();
            sessionProviderMock.Setup(provider => provider.GetUserId())
                .Returns(() => MSUserId);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_CLNTID"))
                .Returns(() => HQClient.ToString());
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_SCRT"))
                .Returns(() => HQSecret);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_SITE"))
                .Returns(() => HQSite);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_RDRURI"))
                .Returns(() => HQRedirectUri);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_CLNCLM"))
                .Returns(() => ClientColumnTitle);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_FILECLM"))
                .Returns(() => FileColumnTitle);
            sessionProviderMock.Setup(provider => provider.GetSpecificData("HQ_FLDRCLM"))
                .Returns(() => FolderColumnTitle);

            return sessionProviderMock;
        }
    }
}
