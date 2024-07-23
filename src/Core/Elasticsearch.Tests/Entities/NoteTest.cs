using System;
using Elasticsearch.Models.Entities;
using Newtonsoft.Json;
using NUnit.Framework;
using UnitTestBase;

namespace Elasticsearch.Tests.Entities
{
    [TestFixture]
    public class NoteTest
    {
        [Test]
        public void ShouldReleaseCorrectInterface()
        {
            Assert.IsInstanceOf(typeof(BaseItem), new Note());
        }

        [TestCase(6, typeof(JsonPropertyAttribute))]
        public void ShouldVerifyFieldsWithAttributes(int fieldCount, Type fieldType)
        {
            Assert.AreEqual(fieldCount, TestHelpersBase.GetFieldCountMarkedWithAttribute(typeof(Note), fieldType), string.Format("Field attribute type is {0}.", fieldType));
        }

        [TestCase("appoinmentId")]
        [TestCase("fileId")]
        [TestCase("clientId")]
        [TestCase("contactId")]
        [TestCase("note")]
        [TestCase("noteSource")]
        public void ShouldVerifyRequiredFieldsWithAttributes(string propertyName)
        {
            Assert.AreEqual(1, TestHelpersBase.GetRequiredFieldCountWithJsonAttribute(typeof(Note), propertyName), string.Format("Property not found {0}.", propertyName));
        }
    }
}
