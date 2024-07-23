using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class AssociateTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new Associate());
        }

        [TestCase(11, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(Associate), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("associateType")]
        [TestCase("assocHeading")]
        [TestCase("assocSalut")]
        [TestCase("file-Id")]
        [TestCase("contact-Id")]
        [TestCase("client-Id")]
        [TestCase("fileStatus")]
        [TestCase("fileType")]
        [TestCase("fileDesc")]
        [TestCase("contName")]
        [TestCase("contactType")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(Associate), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
