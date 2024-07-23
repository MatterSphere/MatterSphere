using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class BaseItemTest
    {
        [Test]
        public void ShouldBeAbstract()
        {
            Assert.IsTrue(typeof(BaseItem).IsAbstract);
        }

        [TestCase(6, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(BaseItem), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("id")]
        [TestCase("title")]
        [TestCase("mattersphereid")]
        [TestCase("modifieddate")]
        [TestCase("highlights")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(BaseItem), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
