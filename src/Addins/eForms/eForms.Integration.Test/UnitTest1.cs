using System;
using System.Diagnostics;
using FWBS.OMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FPETest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        private const string webservice = "http://test/FormsPortalService.asmx";

        public UnitTest1()
        {
            FWBS.OMS.Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly();
            FWBS.OMS.UI.Windows.Services.CheckLogin();
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

        #region Additional test attributes
        [ClassCleanup()]
        public static void MyClassCleanup() 
        {
            FWBS.OMS.FormDefinition def = new FWBS.OMS.FormDefinition(19, webservice, "Danny", "Thompson");
            def.Delete();

            Trace.WriteLine("Delete Form Data");
            FWBS.OMS.FormData form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "Danny", "Thompson");
            form.Delete();
            Trace.WriteLine("Success");
        }
        #endregion

        [TestMethod]
        public void FormDataTest()
        {
            Trace.WriteLine("Create Form Data");
            FWBS.OMS.FormData form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "Danny", "Thompson");
            form.SetExtraInfo("Seller1", "Daniel Thompson");
            form.CampaignID = 1;
            form.Update();
            Trace.WriteLine("Success");

            Trace.WriteLine("Fetch Form Data");
            form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "Danny", "Thompson");
            Assert.AreEqual(1, form.CampaignID);
            Assert.AreEqual("Daniel Thompson", Convert.ToString(form.GetExtraInfo("Seller1")));
            Trace.WriteLine("Success");

            Trace.WriteLine("Update Form Data");
            form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "Danny", "Thompson");
            form.SetExtraInfo("Seller1", "Daniel Charles Thompson");
            form.Update();
            Trace.WriteLine("Success");

            Trace.WriteLine("Fetch Form Data");
            form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "Danny", "Thompson");
            Assert.AreEqual("Daniel Charles Thompson", Convert.ToString(form.GetExtraInfo("Seller1")));
            Trace.WriteLine(form.ObjectID);
            Trace.WriteLine(form.ObjectType);
            Trace.WriteLine(form.FormGuid);
            Trace.WriteLine(form.FormID);
            Trace.WriteLine("Success");


        }

        [TestMethod]
        [ExpectedException(typeof(FormDataException))]
        public void BadFormDataRequestInvalidForm()
        {
            Trace.WriteLine("Create Form Data");
            FWBS.OMS.FormData form = new FWBS.OMS.FormData(-1, "FILE", 2072549536, webservice, "Danny", "Thompson");
            form.SetExtraInfo("Seller1", "Daniel Thompson");
            form.Update();
            Trace.WriteLine("Success");
        }

        [TestMethod]
        [ExpectedException(typeof(FormDataException))]
        public void BadFormDataRequestInvalidPassword()
        {
            MyClassCleanup();
            Trace.WriteLine("Create Form Data");
            FWBS.OMS.FormData form = new FWBS.OMS.FormData(19, "FILE", 2072549536, webservice, "anny", "Thompson");
            form.SetExtraInfo("Seller1", "Daniel Thompson");
            form.Update();
            Trace.WriteLine("Success");
        }

        [TestMethod]
        [ExpectedException(typeof(FormDataException))]
        public void BadPasswordFormDefinationTest()
        {
            FWBS.OMS.FormDefinition def = new FWBS.OMS.FormDefinition(31, webservice, "Danny", "fjklklads");
        }

        [TestMethod]
        [ExpectedException(typeof(FormDataException))]
        public void BadFormDefinition()
        {
            FWBS.OMS.FormDefinition def = new FWBS.OMS.FormDefinition(-1, webservice, "Danny", "Thompson");
        }

        [TestMethod]
        public void FormDefinationTest()
        {
            FWBS.OMS.FormDefinition def = new FWBS.OMS.FormDefinition(19, webservice,"Danny","Thompson");
        }

        [TestMethod]
        public void FormDataEngineTest()
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(2072549536);
            FWBS.OMS.FormData formdata =  file.ExtendedData["FORM19"].Object as FWBS.OMS.FormData;
            formdata.SetExtraInfo("fpeEmail", "danny@test.net");
            formdata.SetExtraInfo("fpeDescription", file.FileDescription);
            formdata.CampaignID = 0;

            Trace.WriteLine("Request Form Test");
            FWBS.OMS.FormDataEngine engine = new FWBS.OMS.FormDataEngine(formdata, file, false);

            Assert.AreEqual(engine.RequestForm(), FWBS.OMS.FormServiceEventArgs.EmptyRequest);

            Trace.WriteLine("Refresh Form Test");
            engine = new FWBS.OMS.FormDataEngine(formdata, file, false);
            Assert.AreEqual(engine.RefreshForm(), FWBS.OMS.FormServiceEventArgs.EmptyRefresh);

            Trace.WriteLine("Completed Date Test");
            DateTime d = DateTime.Now;
            formdata.SetExtraInfo("fpeCompleted", d);
            Assert.AreEqual(formdata.Completed, (DateTime)d);
            
            Trace.WriteLine("Cancel Form Request");
            engine = new FWBS.OMS.FormDataEngine(formdata, file, false);
            Assert.AreEqual(engine.CancelRequest("Because I made a mistake"), FWBS.OMS.FormServiceEventArgs.EmptyCancel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FormDataEngineRefreshBeforeRequestTest()
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(2072549585);
            FWBS.OMS.FormData formdata = file.ExtendedData["FORM19"].Object as FWBS.OMS.FormData;

            Trace.WriteLine("Refresh Form Test");
            FWBS.OMS.FormDataEngine engine = new FWBS.OMS.FormDataEngine(formdata, file, false);
            engine.RefreshForm();

            formdata.Delete();
        }
    }
}
