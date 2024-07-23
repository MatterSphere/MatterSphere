using System;
using FWBS.OMS.Search;
using NUnit.Framework;

namespace Business.Test.Search
{
    [TestFixture]
    public class HighlighterTest
    {
        [TestCase("word1 word2", "\"word2\"", "word1 <highlight>word2</highlight>")]
        [TestCase("word1 word2 word3 word1 word22", "\"word2*\"", "word1 <highlight>word2</highlight> word3 word1 <highlight>word2</highlight>2")]
        [TestCase("word1 word2 word3 word1 word2 word1", "\"word2 word1\"", "<highlight>word1</highlight> <highlight>word2</highlight> word3 <highlight>word1</highlight> <highlight>word2</highlight> <highlight>word1</highlight>")]
        [TestCase("word1 word2 word3 word1 word2 word3", "\"word2\" AND \"word3\"", "word1 <highlight>word2</highlight> <highlight>word3</highlight> word1 <highlight>word2</highlight> <highlight>word3</highlight>")]
        public void TestMatch(string source, string query, string output)
        {
            var words = query.Split(new string[] { "\" AND \"", "\"", " ", "*" }, StringSplitOptions.RemoveEmptyEntries);
            var highlighter = new Highlighter(words);

            var result = highlighter.SetHighlights(source);

            Assert.AreEqual(output, result);
        }
    }
}
