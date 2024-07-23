using System.Linq;
using NUnit.Framework;

namespace ElasticsearchProvider.Tests
{
    [TestFixture]
    public class SuggestionsFactoryTest
    {
        private string _input = "Open the  first! open - the second.";
        private SuggestionsFactory _factory = new SuggestionsFactory();

        [Test]
        public void CheckUpperAndLowerCase()
        {
            var result = _factory.CreateSuggestions(_input);

            Assert.IsTrue(result.Any(word => word == "Open"));
            Assert.IsTrue(result.Any(word => word == "open"));
        }

        [Test]
        public void ExcludedDoubleSpace()
        {
            var result = _factory.CreateSuggestions(_input);

            Assert.IsFalse(result.Any(word => word == "the  first"));
        }

        [Test]
        public void ExcludedSameWords()
        {
            var result = _factory.CreateSuggestions(_input);

            Assert.AreEqual(1, result.Count(word => word == "the"));
        }

        [Test]
        public void IncludedWholePhrase()
        {
            var result = _factory.CreateSuggestions(_input);

            Assert.IsTrue(result.Any(word => word == "Open the first open the second"));
        }

        [Test]
        public void NullValueTest()
        {
            var result = _factory.CreateSuggestions(null);

            Assert.AreEqual(0, result.Length);
        }
    }
}
