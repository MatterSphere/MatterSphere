using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class PrecedentTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new Precedent());
        }

        [TestCase(10, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(Precedent), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("precTitle")]
        [TestCase("precDesc")]
        [TestCase("precContents")]
        [TestCase("precCategory")]
        [TestCase("precSubCategory")]
        [TestCase("precDeleted")]
        [TestCase("precedentExtension")]
        [TestCase("precedentType")]
        [TestCase("authorType")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(Precedent), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
