using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class TaskTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new Task());
        }

        [TestCase(7, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(Task), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("file-id")]
        [TestCase("taskType")]
        [TestCase("tskDesc")]
        [TestCase("client-id")]
        [TestCase("fileStatus")]
        [TestCase("fileType")]
        [TestCase("fileDesc")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(Task), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
