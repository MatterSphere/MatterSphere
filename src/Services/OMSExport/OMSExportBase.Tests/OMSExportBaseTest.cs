using FWBS.OMS.OMSEXPORT;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Assert = NUnit.Framework.Assert;

namespace OMSExportBase.Tests
{
    [TestFixture]
    public class OmsExportBaseTest
    {
        private const int ReturnedRowsNumber = 55;
        private const string query = "test";
        private IDatabaseProvider provider;
        private IDbConnection connection;
        private SqlCommand command;
        private readonly IEnumerable<SqlParameter> parameters = new List<SqlParameter>();

        [SetUp]
        public void Setup()
        {
            command = MockRepository.GenerateMock<SqlCommand>();
            command
                .Stub(c => c.ExecuteNonQuery())               
                .Return(ReturnedRowsNumber);

            connection = MockRepository.GenerateMock<SqlConnection>();
            connection
                .Stub(c => c.Open());
            connection
                .Stub(c => c.Close());

            provider = MockRepository.GeneratePartialMock<FWBS.OMS.OMSEXPORT.OMSExportBase>(connection);
            provider
                .Stub(p => p.CreateSqlCommand(query, parameters)).Return(command);
        }

        [Test]
        public void ShouldReleaseIDatabaseProviderInterface()
        {
            Assert.IsInstanceOf<IDatabaseProvider>(provider);
        }

        [Test]      
        public void ShouldCallExecuteSqlWithParameters()
        {                
           var result = provider.ExecuteSQL(query, parameters);       
           Assert.AreEqual(ReturnedRowsNumber, result);      
        }
    }
}
