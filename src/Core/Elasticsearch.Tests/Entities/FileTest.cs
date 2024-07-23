using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class FileTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new File());
        }

        [TestCase(8, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(File), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("client-id")]
        [TestCase("fileNum")]
        [TestCase("fileStatus")]
        [TestCase("fileType")]
        [TestCase("fileDesc")]
        [TestCase("clientNum")]
        [TestCase("clientName")]
        [TestCase("clientType")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(File), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
