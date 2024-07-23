using System;
using FWBS.OMS.Search;
using NUnit.Framework;

namespace Business.Test.Search
{
    [TestFixture]
    public class EntityConverterTest
    {
        [TestCase(1, "F1", "Name", "Person")]
        public void GetItemFromClient(long clientId, string clientNumber, string clientName, string clientType)
        {
            var date = DateTime.Now;
            var row = new CommonRow("Client", clientId.ToString())
            {
                ClientId = clientId,
                ClientInfo = new CommonRow.Client(clientNumber, clientName, clientType),
                ModifiedDate = date
            };
            var converter = new EntityConverter();

            var item = converter.Convert(row);

            Assert.AreEqual(clientId.ToString(), item.MatterSphereId);
            Assert.AreEqual($"{clientNumber}: {clientName}", item.Title);
            Assert.AreEqual(clientType, item.Summary);
            Assert.AreEqual(date, item.ModifiedDate);
            Assert.AreEqual(clientId, item.ClientId);
            Assert.AreEqual("client", item.ObjectType);
        }
    }
}
