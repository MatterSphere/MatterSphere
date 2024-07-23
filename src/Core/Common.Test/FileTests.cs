using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Test
{
    [TestClass]
    public class FileTests
    {
        [TestMethod]
        public void ExtractInvalidChars_Test_EmptyString()
        {
            string input = "";
            string expected = "";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_EnglishAlphabet_AllCharsAreValidOutputShouldEqualInput()
        {
            string input = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string expected = input;
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_NumericDigits_AllCharsAreValidOutputShouldEqualInput()
        {
            string input = "1234567890";
            string expected = input;
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_EnglishAlphabetAndNumericDigits_AllCharsAreValidOutputShouldEqualInput()
        {
            string input = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string expected = input;
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithSpaces_AllCharsAreValidOutputShouldEqualInput()
        {
            string input = "a b c d e 1";
            string expected = input;
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithTabs_TabsShouldBeRemoved()
        {
            string input = "a\tb\t1";
            string expected = "ab1";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithConsecutiveTabs_TabsShouldBeRemoved()
        {
            string input = "a\t\tb\t1";
            string expected = "ab1";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithNewLine_NewLinesShouldBeRemoved()
        {
            string input = string.Format("a{0}b{0}c{0}", Environment.NewLine);
            string expected = "abc";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithCarriageReturns_CarriageReturnsShouldBeRemoved()
        {
            string input = "a\rb\rc";
            string expected = "abc";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }        

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithLineFeeds_LineFeedsShouldBeRemoved()
        {
            string input = "a\nb\nc";
            string expected = "abc";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithCarriageReturnLineFeeds_CarriageReturnLineFeedsShouldBeRemoved()
        {
            string input = "a\r\nb\r\nc";
            string expected = "abc";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }        

        [TestMethod]
        public void ExtractInvalidChars_Test_StringWithOrignallyRemovedCharacters_AllShouldBeRemoved()
        {
            StringBuilder input = new StringBuilder();
            
            char[] chars = new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

            input.Append("ab");
            input.Append(chars);
            input.Append("cd");
            
            string expected = "abcd";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input.ToString());

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_TrimStartAndEndWhitespace()
        {
            string input = "   abcd   ";
            string expected = "abcd";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_ReplaceInvalidCharWithSpace()
        {
            string input = "a\rb";
            string expected = "a b";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input," ");

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_ReplaceConsecutiveInvalidCharsWithSpaces()
        {
            string input = "a\r\n\tb";
            string expected = "a   b";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input, " ");

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_RemoveBackSlashCharacter()
        {
            string input = "a\\b";
            string expected = "a b";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input, " ");

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void ExtractInvalidChars_Test_RemoveBaaaackSlashCharacter()
        {
            string input = "\ta\\b\r\n";
            string expected = "a b";
            string output = FWBS.Common.Directory.ExtractInvalidChars(input, " ");

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ExtractInvalidChars_Test_InvalidCharacterAsAReplacementShouldThrowArgumentException()
        {
            string input = "abc";
            
            string output = FWBS.Common.Directory.ExtractInvalidChars(input, "\t");     
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void ExtractInvalidChars_Test_NameIsNullShouldThrowArgumentNullException()
        {
            string input = null;
            
            string output = FWBS.Common.Directory.ExtractInvalidChars(input);
        }

        [TestMethod]       
        public void ExtractInvalidChars_Test_PassingInEmptyStringShouldReturnEmptyString()
        {
            string input = string.Empty;
            string expected = string.Empty;
            string output = FWBS.Common.Directory.ExtractInvalidChars(input,"A");
            
            Assert.AreEqual(expected, output);
        }   
    }
}
