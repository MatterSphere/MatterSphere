using FWBS.OMS.Data;
using NUnit.Framework;

namespace Data.Tests
{
    [TestFixture()]
    public class SqlCommandBuilderTests
    {
        [Test]
        public void ShouldVerifyBuilderCreation()
        {
            var conn = new SQLConnection(string.Empty, "user1");
            var cmdBuilder = new FWBS.OMS.Data.SQLCommandBuilder(conn, "MSDEV.dbo.dbVersion", false, null);

            Assert.IsTrue(cmdBuilder.Cnn.Equals(conn));
        }

        [Test]
        public void ShouldVerifyBuilderCreationWithoutFields()
        {
            var conn = new SQLConnection(string.Empty, "user1");
            var cmdBuilder = new FWBS.OMS.Data.SQLCommandBuilder(conn, "MSDEV.dbo.dbVersion", false, null);

            Assert.IsTrue(cmdBuilder.DataAdapter.SelectCommand.CommandText.Equals("SELECT * FROM MSDEV.dbo.dbVersion"));
        }

        [Test]
        public void ShouldVerifyBuilderCreationWithEmptyArrayOfFields()
        {
            var conn = new SQLConnection(string.Empty, "user1");
            var cmdBuilder = new FWBS.OMS.Data.SQLCommandBuilder(conn, "MSDEV.dbo.dbVersion", false, new string[] {});

            Assert.IsTrue(cmdBuilder.DataAdapter.SelectCommand.CommandText.Equals("SELECT * FROM MSDEV.dbo.dbVersion"));
        }

        [Test]
        public void ShouldVerifyBuilderCreationWithFields()
        {
            var conn = new SQLConnection(string.Empty, "user1");
            var cmdBuilder = new FWBS.OMS.Data.SQLCommandBuilder(conn, "MSDEV.dbo.dbVersion", false, new string[] { "verMajor", "verTag" });

            Assert.IsTrue(cmdBuilder.DataAdapter.SelectCommand.CommandText.Equals("SELECT verMajor, verTag FROM MSDEV.dbo.dbVersion"));   
        }
    }
}
