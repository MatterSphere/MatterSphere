using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MSOffice = Microsoft.Office.Core;

namespace Fwbs.Oms.Office.Excel
{
    using FWBS.OMS;

    public partial class ExcelOMSAddin
    {
        private Common.OfficeOMSAddin addin;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Connect();
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            FWBS.OMS.UI.Windows.Office.ExcelOMS omsapp = addin.OMSApplication as FWBS.OMS.UI.Windows.Office.ExcelOMS;
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


        private void addin_ControlResourceRequest(object sender, Fwbs.Oms.Office.Common.ControlResourceRequestEventArgs e)
        {
            e.Commands = GetResourceText("Fwbs.Oms.Office.Excel.RibbonCommands.xml");
        }

        #region Methods

        private void CreateOMSApp()
        {
            if (addin == null)
            {
                FWBS.OMS.UI.Windows.Office.ExcelOMS omsapp = new FWBS.OMS.UI.Windows.Office.ExcelOMS(Application, "EXCEL", false);
                addin = new Common.OfficeOMSAddin(omsapp, Application, Application.CommandBars, CustomTaskPanes, false);
                addin.ControlResourceRequest += new Fwbs.Oms.Office.Common.ControlResourceRequestDelegate(addin_ControlResourceRequest);
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
            this.Application.WorkbookActivate += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
            this.Application.WorkbookBeforeClose += new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler(Application_WorkbookBeforeClose);
            addin.ControlVisibleRequest += new Fwbs.Oms.Office.Common.ControlVisibleCallbackDelegate(addin_ControlVisibleRequest);
        }

        private void DetachEvents()
        {
            this.Application.WorkbookActivate -= new Microsoft.Office.Interop.Excel.AppEvents_WorkbookActivateEventHandler(Application_WorkbookActivate);
            this.Application.WorkbookBeforeClose -= new Microsoft.Office.Interop.Excel.AppEvents_WorkbookBeforeCloseEventHandler(Application_WorkbookBeforeClose);
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

      

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Captured Events

        private void Application_WorkbookBeforeClose(Microsoft.Office.Interop.Excel.Workbook Wb, ref bool Cancel)
        {
            try
            {
                addin.RefreshUI(false, Wb);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void Application_WorkbookActivate(Microsoft.Office.Interop.Excel.Workbook Wb)
        {
            try
            {
                addin.RefreshUI(false, Wb);
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
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
