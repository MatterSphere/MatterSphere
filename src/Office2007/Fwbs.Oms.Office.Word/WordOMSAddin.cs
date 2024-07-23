using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.UI.Elasticsearch.GlobalSearch;
using FWBS.OMS.UI.Windows;
using Microsoft.Office.Tools;
using MSOffice = Microsoft.Office.Core;

namespace Fwbs.Oms.Office.Word
{
    using FWBS.OMS;

    public partial class WordOMSAddin
    {
        private Common.OfficeOMSAddin addin;

        private void WordOMSAddin_Startup(object sender, System.EventArgs e)
        {
            Connect();
        }

        private void WordOMSAddin_Shutdown(object sender, System.EventArgs e)
        {
            FWBS.OMS.UI.Windows.Office.WordOMS2 omsapp = addin.OMSApplication as FWBS.OMS.UI.Windows.Office.WordOMS2;
            if (omsapp != null)
            {
                omsapp.AddinShutdown();
            }
            Disconnect();
        }

        protected override object RequestComAddInAutomationService()
        {
            CreateOMSApp();
            return new Common.ExternalOfficeOMSAddin(addin);
        }

        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(MSOffice.IRibbonExtensibility).GUID)
            {
                CreateOMSApp();
                return addin;
            }

            return base.RequestService(serviceGuid);
        }

        #region Methods

        private void CreateOMSApp()
        {
            if (addin == null)
            {
                FWBS.OMS.UI.Windows.Office.WordOMS2 omsapp = new FWBS.OMS.UI.Windows.Office.WordOMS2(Application, "WORD", false);
                addin = new Common.OfficeOMSAddin(omsapp, Application, Application.CommandBars, CustomTaskPanes, false);
            }
        }

        

        private void Connect()
        {
            CreateOMSApp();
            AttachEvents();
            addin.AutoConnect();
        }

        private void Disconnect()
        {
            DetachEvents();
            Session.CurrentSession.Disconnect();
        }

        private void AttachEvents()
        {
            this.Application.DocumentChange += new Microsoft.Office.Interop.Word.ApplicationEvents4_DocumentChangeEventHandler(Application_DocumentChange);
            addin.ControlVisibleRequest += new Fwbs.Oms.Office.Common.ControlVisibleCallbackDelegate(addin_ControlVisibleRequest);
        }
        private void DetachEvents()
        {
            try
            {
                this.Application.DocumentChange -= new Microsoft.Office.Interop.Word.ApplicationEvents4_DocumentChangeEventHandler(Application_DocumentChange);
            }
            catch { }

            addin.ControlVisibleRequest -= new Fwbs.Oms.Office.Common.ControlVisibleCallbackDelegate(addin_ControlVisibleRequest);
        }



        void addin_ControlVisibleRequest(object sender, Fwbs.Oms.Office.Common.ControlVisibleCallbackEventArgs e)
        {
            string command = e.Command.ToUpper();

            if (command == "OMS;TEMPLATERUN;MANUALOPEN")
            {

                e.ReturnValue = Session.CurrentSession.IsLoggedIn && Convert.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "LocalOpen").GetSetting(false));
                e.Handled = true;
            }
            else if (command == "OMS;TEMPLATERUN;MANUALSAVE")
            {

                e.ReturnValue = Session.CurrentSession.IsLoggedIn && Convert.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "LocalSaveAs").GetSetting(false));
                e.Handled = true;
            }
            


        }

       
        #endregion

        #region Captured Events


        private void Application_DocumentChange()
        {
            try
            {
                if (Application.Documents.Count == 0)
                    addin.RefreshUI(Application);
                else
                    addin.RefreshUI(false, Application.ActiveDocument);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        #endregion

        #region VSTO generated code

        public override void BeginInit()
        {
            try
            {
                Type DpiHelperType = typeof(System.Windows.Forms.Control).Assembly.GetType("System.Windows.Forms.DpiHelper");
                DpiHelperType.GetField("dpiAwarenessValue", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, "PerMonitorV2");
                DpiHelperType.GetField("enableDpiChangedHighDpiImprovements", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);

                AppDomainSetup appDomainSetup = (AppDomainSetup)typeof(AppDomain).GetProperty("FusionStore", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(AppDomain.CurrentDomain);
                appDomainSetup.TargetFrameworkName = Assembly.GetExecutingAssembly().GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>().FrameworkName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                base.BeginInit();
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(WordOMSAddin_Startup);
            this.Shutdown += new System.EventHandler(WordOMSAddin_Shutdown);
        }

        #endregion
    }
}
