using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scanning.UnitTest
{
    /// <summary>
    /// CM - 01.04.2014
    /// Tip: For the image tests place source image(s) (e.g. "test.jpg") in '.\TestFiles' folder
    /// </summary>
    [TestClass]
    public class ImageTests
    {
        private const string FILENAME = @".\TestFiles\test";
        private const string CONVERT_FOLDER = @".\TestFiles\converted";
        private const string TEMP_FILENAME = @".\TestFiles\converted\~test";

        [TestMethod]
        [DeploymentItem(@".\TestFiles\test.jpeg", "TestFiles")]
        [DataRow(".jpeg")]
        [DeploymentItem(@".\TestFiles\test.jpg", "TestFiles")]
        [DataRow(".jpg")]
        [DeploymentItem(@".\TestFiles\test.png", "TestFiles")]
        [DataRow(".png")]
        [DeploymentItem(@".\TestFiles\test.bmp", "TestFiles")]
        [DataRow(".bmp")]
        [DeploymentItem(@".\TestFiles\test.gif", "TestFiles")]
        [DataRow(".gif")]
        [DeploymentItem(@".\TestFiles\test.pdf", "TestFiles")]
        [DataRow(".pdf")]
        public void SaveAsTiff(string extension)
        {
            string filePath = $"{FILENAME}{extension}";
            string newFilePath = $"{FILENAME}{extension}.tif";

            Assert.IsTrue(File.Exists(filePath), $"File '{filePath}' not found");

            var frm = new FWBS.Scanning.frmConvertProgress();
            frm.SaveAsTif(filePath, newFilePath);
            frm.Dispose();

            Assert.IsTrue(File.Exists(newFilePath), $"After save File ({newFilePath}) not found");
        }

        [TestMethod]
        [DeploymentItem(@".\TestFiles\test.jpeg", "TestFiles")]
        [DataRow(".jpeg")]
        [DeploymentItem(@".\TestFiles\test.jpg", "TestFiles")]
        [DataRow(".jpg")]
        [DeploymentItem(@".\TestFiles\test.png", "TestFiles")]
        [DataRow(".png")]
        [DeploymentItem(@".\TestFiles\test.bmp", "TestFiles")]
        [DataRow(".bmp")]
        [DeploymentItem(@".\TestFiles\test.gif", "TestFiles")]
        [DataRow(".gif")]
        [DeploymentItem(@".\TestFiles\test.pdf", "TestFiles")]
        [DataRow(".pdf")]
        public void SaveAsTemporaryTiff(string extension)
        {
            string filePath = $"{FILENAME}{extension}";

            Assert.IsTrue(File.Exists(filePath), $"File '{filePath}' not found");

            Directory.CreateDirectory(CONVERT_FOLDER);

            string tempFilePath = $"{TEMP_FILENAME}{extension}.tif";

            var frm = new FWBS.Scanning.frmConvertProgress();
            frm.SaveAsTif(filePath, tempFilePath);
            frm.Dispose();

            Assert.IsTrue(File.Exists(tempFilePath), $"After save Temporary File ({tempFilePath}) not found");
        }
    }
}