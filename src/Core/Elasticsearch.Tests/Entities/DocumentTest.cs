using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class DocumentTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new Document());
        }

        [TestCase(14, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(Document), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("file-id")]
        [TestCase("docDesc")]
        [TestCase("docContents")]
        [TestCase("documentExtension")]
        [TestCase("documentType")]
        [TestCase("docDeleted")]
        [TestCase("authorType")]
        [TestCase("client-Id")]
        [TestCase("fileStatus")]
        [TestCase("fileType")]
        [TestCase("fileDesc")]
        [TestCase("clientNum")]
        [TestCase("clientName")]
        [TestCase("clientType")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(Document), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
