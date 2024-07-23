using System.Threading;
using Elasticsearch.Tests.IntegrationTests.Common;
using NUnit.Framework;
using RestSharp;

namespace Elasticsearch.Tests.IntegrationTests
{
    [SetUpFixture]
    public class InitialSetup
    {
        [OneTimeSetUp]
        public void PrepareIndex()
        {
            var restClient = new RestClient(ElasticConfiguration.Url);
            if (!string.IsNullOrEmpty(ElasticConfiguration.IndexApiKey))
                restClient.AddDefaultHeader("Authorization", $"ApiKey {ElasticConfiguration.IndexApiKey}");

            var builder = new IndexBuilder(restClient, ElasticConfiguration.DataIndex, ElasticConfiguration.UserIndex);
            builder.CreateDataIndex();

            var clientTomas = new IndexBuilder.ClientData(
                "37D2AAA7-D2B2-4555-8378-E155549217A0",
                1,
                "Tomas",
                "F1",
                "Person",
                "11111111-1111-1111-1111-111111111111(ALLOW)");
            var clientDenis = new IndexBuilder.ClientData(
                "0EC1E28F-B8CF-462C-B940-A39C978F21BB",
                2,
                "Denis",
                "F2",
                "Person",
                "11111111-1111-1111-1111-111111111111(ALLOW) 22222222-2222-2222-2222-222222222222(DENY)");
            var clientArthur = new IndexBuilder.ClientData(
                "722F9D66-8C1C-4C30-A415-F0392990F68B",
                3,
                "Arthur",
                "F3",
                "Pre client",
                "33333333-3333-3333-3333-333333333333(DENY) 44444444-4444-4444-4444-444444444444(ALLOW)");
            var clientJack = new IndexBuilder.ClientData(
                "1A87650F-ACEB-4CD2-9A92-56B6A446EDDE",
                4,
                "Jack",
                "F4",
                "Company",
                "11111111-1111-1111-1111-111111111111(DENY) 33333333-3333-3333-3333-333333333333(DENY) 44444444-4444-4444-4444-444444444444(ALLOW)");
            var clientAlbert = new IndexBuilder.ClientData(
                "E5A53247-0E15-4ACF-B64E-241649188352",
                5,
                "Albert",
                "F5",
                "Company",
                "33333333-3333-3333-3333-333333333333(ALLOW)");

            builder.AddClient(clientTomas);
            builder.AddClient(clientDenis);
            builder.AddClient(clientArthur);
            builder.AddClient(clientJack);
            builder.AddClient(clientAlbert);

            builder.AddFile(
                new IndexBuilder.FileData(
                    "C0143517-B5AC-4D81-99D3-275F4BC5925B",
                    1,
                    "First Matter",
                    "live",
                    "Custom Matter Type",
                    "44444444-4444-4444-4444-444444444444(DENY)"),
                clientTomas);
            builder.AddFile(
                new IndexBuilder.FileData(
                    "00F8EA97-C089-425B-8ACD-5C8A3458C1EF",
                    2,
                    "Second Matter",
                    "closed",
                    "Default",
                    "44444444-4444-4444-4444-444444444444(ALLOW)"),
                clientTomas);
            builder.AddFile(
                new IndexBuilder.FileData(
                    "3547630C-12F5-444B-98CB-98F77CC7A5B4",
                    3,
                    "Denis Matter",
                    "live",
                    "Default",
                    "22222222-2222-2222-2222-222222222222(ALLOW) 44444444-4444-4444-4444-444444444444(ALLOW)"),
                clientDenis);
            builder.AddFile(
                new IndexBuilder.FileData(
                    "338842C5-1A0E-47E7-A8E4-61DF47C0EE0B",
                    10,
                    "Matter 100/1",
                    "live",
                    "Default",
                    "44444444-4444-4444-4444-444444444444(ALLOW)"),
                clientTomas);

            builder.CreateUserIndex();

            builder.AddUser(new IndexBuilder.UserData(
                "8B05846E-CBE5-421A-90A2-58109D9B4B54",
                1,
                "User0",
                "User0",
                "869A69F6-7B98-4ACC-8E59-7DE87F354559"));
            builder.AddUser(new IndexBuilder.UserData(
                "D52E3DE3-7127-488D-9814-60A362305A03",
                2,
                "User1",
                "User1",
                "11111111-1111-1111-1111-111111111111"));
            builder.AddUser(new IndexBuilder.UserData(
                "8BBFAB15-5794-4E1B-9FFA-93B32B3C51E5",
                3,
                "User2",
                "User2",
                "22222222-2222-2222-2222-222222222222"));
            builder.AddUser(new IndexBuilder.UserData(
                "F7992D11-267D-4A30-8B49-EC077D90BCB6",
                4,
                "User34",
                "User34",
                "33333333-3333-3333-3333-333333333333 44444444-4444-4444-4444-444444444444"));

            Thread.Sleep(2000);
        }
    }
}