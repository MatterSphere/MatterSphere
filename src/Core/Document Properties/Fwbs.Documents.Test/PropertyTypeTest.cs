using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    using System.IO;

    /// <summary>
    /// Summary description for PropertyTypeTest
    /// </summary>
    [TestClass]
    public class PropertyTypeTest
    {
        public PropertyTypeTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void SetStringTest()
        {
            SetAndCheckTest<string, string>("Hello", "Hello");
        }
        [TestMethod]
        public void SetStringBuilderTest()
        {
            SetAndCheckTest<StringBuilder, string>(new StringBuilder("Hello"), "Hello");
        }
        [TestMethod]
        public void SetCharTest()
        {
            SetAndCheckTest<char, string>('h', "h");
        }


        [TestMethod]
        public void SetInt64Test()
        {
            SetAndCheckTest<Int64, Double>(Int64.MaxValue, (Double)Int64.MaxValue);
        }

        [TestMethod]
        public void SetUInt64Test()
        {
            SetAndCheckTest<UInt64, Double>(UInt64.MaxValue, (Double)UInt64.MaxValue);
        }

        [TestMethod]
        public void SetInt32Test()
        {
            SetAndCheckTest<Int32, Int32>(Int32.MaxValue, Int32.MaxValue);
        }
        [TestMethod]
        public void SetUInt32Test()
        {
            SetAndCheckTest<UInt32, Double>(UInt32.MaxValue, (Double)UInt32.MaxValue);
        }

        [TestMethod]
        public void SetInt16Test()
        {
            SetAndCheckTest<Int16, Int32>(Int16.MaxValue, (Int32)Int16.MaxValue);
        }
        [TestMethod]
        public void SetUInt16Test()
        {
            SetAndCheckTest<UInt16, Int32>(UInt16.MaxValue, (Int32)UInt16.MaxValue);
        }

        [TestMethod]
        public void SetByteTest()
        {
            SetAndCheckTest<Byte, Int32>(Byte.MaxValue, (Int32)Byte.MaxValue);
        }
        [TestMethod]
        public void SetSByteTest()
        {
            SetAndCheckTest<SByte, Int32>(SByte.MaxValue, (Int32)SByte.MaxValue);
        }

        [TestMethod]
        public void SetDoubleTest()
        {
            Double dbl = Double.MaxValue;
            if (dbl > UInt64.MaxValue)
                dbl = (Double)UInt64.MaxValue;
            SetAndCheckTest<Double, Double>(dbl, dbl);
        }
        [TestMethod]
        public void SetSingleTest()
        {
            SetAndCheckTest<Single, Double>(Single.MaxValue, (Double)Single.MaxValue);
        }

        [TestMethod]
        public void SetDateTimeTest()
        {
            DateTime dte = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Local);
            SetAndCheckTest<DateTime, DateTime>(dte, dte);
        }

        [TestMethod]
        public void SetBooleanTest()
        {
            SetAndCheckTest<Boolean, Boolean>(true, true);
        }

        //This fixes Bug Item 1472 on TFS2
        [TestMethod]
        public void SetGuidAsStringTest()
        {
            SetAndCheckTest<Guid, string>(Guid.Empty, Guid.Empty.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetNoNamePropertyTest()
        {
            SetAndCheckTest<String, String>(String.Empty, "test", "test");
        }


        private void SetAndCheckTest<TVal, TExpected>(TVal value, TExpected expected)
        {
            SetAndCheckTest<TVal, TExpected>("T1", value, expected);
        }
        private void SetAndCheckTest<TVal, TExpected>(string name, TVal value, TExpected expected)
        {
            foreach (FileInfo file in DocumentInfoTest.TestDocuments.Values)
            {
                using (DocumentInfo doc = new DocumentInfo(file))
                {
                    doc.CustomProperties[name].Value = value;
                    doc.Save();
                }

                using (DocumentInfo doc = new DocumentInfo(file))
                {
                    Assert.IsTrue(doc.CustomProperties.Contains(name), "Property '{0}' should exist for file '{1}'.", name, file.Name);
                    Assert.AreEqual<Type>(expected.GetType(), doc.CustomProperties[name].Value.GetType(), "Property value type not expected value type for file '{0}'.", file.Name);
                    Assert.AreEqual(expected, doc.CustomProperties[name].Value, "Property value not expected value for file '{0}'.", file.Name);
                }

            }
        }
    }
}
