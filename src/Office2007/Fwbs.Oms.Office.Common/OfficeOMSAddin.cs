using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using MSOffice = Microsoft.Office.Core;

namespace Fwbs.Oms.Office.Common
{
    using FWBS.Common;
    using FWBS.OMS;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.UI.Windows;
    using FWBS.OMS.UI.Windows.Office;

    [ComVisible(true)]
    public sealed class OfficeOMSAddin : MSOffice.IRibbonExtensibility, IDisposable
    {
        private class RunCommands
        {
            public const string Connect = "ADDIN;CONNECT";
        }

        #region Fields

        private MSOffice.IRibbonUI ribbon;

        private System.Collections.ObjectModel.ReadOnlyCollection<FWBS.OMS.Script.MenuScriptType> menuscripts;
        private System.Data.DataTable resources;

        private OfficeOMSApp omsapp;
        private MSOffice.CommandBars bars;
        private Microsoft.Office.Tools.CustomTaskPaneCollection panes;

        private List<string> AlwaysVisible = new List<string>();
        private object app;
        private bool useCommandBars = false;
        private bool useTaskPaneWizards;
        private string applicationName = "?";

        private Dictionary<string, OMSRibbonCache> ribbons = new Dictionary<string, OMSRibbonCache>();

        private const string OMSUIXmlNamespace = "http://schemas.fwbs.net/oms/2006/11/omsui";

        #endregion

        #region Events

        public event ControlResourceRequestDelegate ControlResourceRequest;
        public event ControlStringCallbackDelegate ControlLabelRequest;
        public event ControlStringCallbackDelegate ControlDescriptionRequest;
        public event ControlStringCallbackDelegate ControlScreenTipRequest;
        public event ControlStringCallbackDelegate ControlSuperTipRequest;
        public event ControlStringCallbackDelegate ControlKeyTipRequest;
        public event ControlStringCallbackDelegate ControlSizeRequest;

        public event ControlDynamicMenuCallbackDelegate ControlDynamicMenuRequest;

        public event ControlVisibleCallbackDelegate ControlEnabledRequest;
        public event ControlVisibleCallbackDelegate ControlVisibleRequest;
        public event ControlFlagCallbackDelegate ControlShowImageRequest;
        public event ControlFlagCallbackDelegate ControlShowLabelRequest;
        public event ControlFlagCallbackDelegate ControlPressedRequest;

        public event ControlActionCallbackDelegate ControlAction;
        public event ControlToggleCallbackDelegate ControlToggle;


        #endregion

        #region Event Raising Methods

        private void OnControlResourceRequest(ControlResourceRequestEventArgs e)
        {
            ControlResourceRequestDelegate ev = ControlResourceRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlLabelRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlLabelRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlDescriptionRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlDescriptionRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlScreenTipRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlScreenTipRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlSuperTipRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlSuperTipRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlKeyTipRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlKeyTipRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlDynamicMenuRequest(ControlDynamicMenuCallbackEventArgs e)
        {
            ControlDynamicMenuCallbackDelegate ev = ControlDynamicMenuRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlSizeRequest(ControlStringCallbackEventArgs e)
        {
            ControlStringCallbackDelegate ev = ControlSizeRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlAction(ControlActionCallbackEventArgs e)
        {
            ControlActionCallbackDelegate ev = ControlAction;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlToggle(ControlToggleCallbackEventArgs e)
        {
            ControlToggleCallbackDelegate ev = ControlToggle;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlEnabledRequest(ControlVisibleCallbackEventArgs e)
        {
            ControlVisibleCallbackDelegate ev = ControlEnabledRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlVisibleRequest(ControlVisibleCallbackEventArgs e)
        {
            ControlVisibleCallbackDelegate ev = ControlVisibleRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlPressedRequest(ControlFlagCallbackEventArgs e)
        {
            ControlFlagCallbackDelegate ev = ControlPressedRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlShowImageRequest(ControlFlagCallbackEventArgs e)
        {
            ControlFlagCallbackDelegate ev = ControlShowImageRequest;
            if (ev != null)
                ev(this, e);
        }

        private void OnControlShowLabelRequest(ControlFlagCallbackEventArgs e)
        {
            ControlFlagCallbackDelegate ev = ControlShowLabelRequest;
            if (ev != null)
                ev(this, e);
        }

        #endregion

        #region Constructors

        public OfficeOMSAddin(OfficeOMSApp omsApp, object application, MSOffice.CommandBars commandBars, Microsoft.Office.Tools.CustomTaskPaneCollection panes, bool useCommandBars)
        {
            AlwaysVisible.Add(RunCommands.Connect);
            AlwaysVisible.Add("TabHome_Connect");

            try
            {
                //Subscribe to thread (unhandled) exception events
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException -= new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
                System.Windows.Forms.Application.ThreadException -= new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(currentDomain_UnhandledException);
                System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            }
            catch { }

            if (omsApp == null)
                throw new ArgumentNullException("omsApp");

            if (application == null)
                throw new ArgumentNullException("application");

            if (commandBars == null)
                throw new ArgumentNullException("commandBars");

            this.applicationName = omsApp.ApplicationName;
            this.app = application;
            this.bars = commandBars;
            this.panes = panes;
            this.omsapp = omsApp;
            this.useCommandBars = useCommandBars;
            this.useTaskPaneWizards = ConvertDef.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "TaskPaneWizards").GetSetting(false), false);
            current = this;

            Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly();

            Session.CurrentSession.Connected -= new EventHandler(OMS_Connected);
            Session.CurrentSession.Disconnected -= new EventHandler(OMS_Disconnected);
            Session.CurrentSession.Connected += new EventHandler(OMS_Connected);
            Session.CurrentSession.Disconnected += new EventHandler(OMS_Disconnected);

            if (useCommandBars)
            {
                //Build the default bars and command bars if needed.
                BuildDefaultCommandBar();

                BuildCommandBars(true);
            }

            if (useTaskPaneWizards)
            {
                ControlAction += new ControlActionCallbackDelegate(addin_ControlAction);
            }
        }

        #endregion

        #region Properties

        public string ApplicationName
        {
            get
            {
                return applicationName;
            }
        }

        [ComVisible(false)]
        public OfficeOMSApp OMSApplication
        {
            get
            {
                return omsapp;
            }
        }

        [ComVisible(false)]
        public object Application
        {
            get
            {
                return app;
            }
        }

        [ComVisible(false)]
        public MSOffice.CommandBars CommandBars
        {
            get
            {
                return bars;
            }
        }

        [ComVisible(false)]
        public Microsoft.Office.Tools.CustomTaskPaneCollection Panes
        {
            get
            {
                return panes;
            }
        }

        public bool UseCommandBars
        {
            get
            {
                return useCommandBars;
            }
        }

        #endregion

        #region Methods

        public void AutoConnect()
        {
            if (Session.CurrentSession.IsLoggedIn == false)
            {
                if (Session.CurrentSession.GetConnectedClients().Length > 0)
                {
                    FWBS.Common.ApplicationSetting autologon = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "AutoLogon", true);

                    if (autologon.ToBoolean())
                        OMSApplication.RunCommand(this, "SYSTEM;CONNECT");
                }
                else
                {
                    if (OMSApplication != null)
                    {
                        FWBS.Common.ApplicationSetting autologon = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "AutoLogon", false);
                        FWBS.Common.ApplicationSetting forcelogon = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "ForceLogon", -1);

                        if (autologon.ToBoolean())
                        {
                            if (Convert.ToInt32(forcelogon.GetSetting()) >= 0)
                                OMSApplication.RunCommand(this, String.Format("SYSTEM;CONNECT;FORCE;{0}", forcelogon.ToString()));
                            else
                                OMSApplication.RunCommand(this, "SYSTEM;CONNECT");

                        }
                    }
                }
            }
        }

        private FWBS.OMS.Script.MenuScriptAggregator menuScriptsAgg;

        #endregion

        #region Ribbon Bar

        public void RefreshUI(object activeDoc)
        {
            RefreshUI(true, activeDoc);
        }

        public void RefreshUI(bool includePanels, object activeDoc)
        {
            lock (ribbons)
            {
                foreach (var rc in ribbons.Values)
                {
                    lock (rc.CachedConfigs)
                    {
                        rc.CachedConfigs.Clear();
                    }
                }
            }

            if (ribbon != null)
                ribbon.Invalidate();

            if (includePanels)
            {
                if (panes != null)
                {

                    for (int ctr = panes.Count - 1; ctr >= 0; ctr--)
                    {

                        Microsoft.Office.Tools.CustomTaskPane pane = panes[ctr];
                        if (!pane.Visible)
                            continue;
                        Common.Panes.BasePane basepane = pane.Control as Common.Panes.BasePane;
                        if (basepane != null)
                        {
                            OMSDocument windowDoc = null;
                            OMSDocument docDoc = null;

                            try
                            {
                                windowDoc = omsapp.GetCurrentDocument(pane.Window);
                                docDoc = omsapp.GetCurrentDocument(activeDoc);
                            }
                            catch (Exception)
                            { }

                            basepane.Refresh(activeDoc);
                        }
                    }
                }
            }
        }

        public void RefreshUIControl(string id)
        {
            lock (ribbons)
            {
                foreach (var rc in ribbons)
                {
                    lock (rc.Value.CachedConfigs)
                    {
                        if (rc.Value.CachedConfigs.ContainsKey(id))
                            rc.Value.CachedConfigs.Remove(id);
                    }
                }
            }

            if (ribbon != null)
                ribbon.InvalidateControl(id);
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

        private void SetDefaultRibbonControls(string ribbonid, XmlElement el)
        {
            CheckForCustomNode(ribbonid, el);

            while (el != null)
            {
                for (int ctr = el.ChildNodes.Count - 1; ctr >= 0; ctr--)
                {
                    var node = el.ChildNodes[ctr];

                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement ctrl = (XmlElement)node;
                        SetDefaultRibbonControl(ribbonid, ctrl);
                        SetDefaultRibbonControls(ribbonid, ctrl);
                        
                    }
                }

                el = null;
            }
        }

        private void CheckForCustomNode(string ribbonID, XmlElement el)
        {
            if (el.NamespaceURI == OMSUIXmlNamespace)
            {
                el.ParentNode.RemoveChild(el);
                return;
            }

            for (int ctr = el.Attributes.Count - 1; ctr >= 0; ctr--)
            {
                var attr = el.Attributes[ctr];

                if (attr.NamespaceURI == OMSUIXmlNamespace)
                {
                    if (attr.LocalName == "RibbonIDRegEx")
                    {
                        if (HasRibbonID(attr.Value, ribbonID))
                            el.Attributes.Remove(attr);
                        else
                            el.ParentNode.RemoveChild(el);
                    }
                    else
                    {
                        el.Attributes.Remove(attr);
                    }
                }
            }
        }

        private static bool HasRibbonID(string expression, string input)
        {
            if (string.Equals(expression, input))
                return true;

            var match = System.Text.RegularExpressions.Regex.Match(input, expression);

            return match.Success;
        }
  
        private static string GetRibbonIdFromControl(XmlElement ctrl, out string id)
        {
            id = null;

            var aid = ctrl.Attributes["id"];
            if (aid == null)
                return null;

            if (String.IsNullOrEmpty(aid.Value))
                return null;

            return GetRibbonIdFromControlId(aid.Value, out id);
        }

        private static string GetRibbonIdFromControlId(string name, out string id)
        {
            id = null;

            if (String.IsNullOrEmpty(name))
                return null;

            var s = name.Split(new string[]{"-_-"}, StringSplitOptions.RemoveEmptyEntries);
            switch (s.Length)
            {
                case 0:
                    id = String.Empty;
                    return String.Empty;
                case 1:
                    id = s[0];
                    return String.Empty;
                default:
                    id = s[1];
                    return s[0];
            }

        }

        private void SetRibbonControlId(string ribbonid, XmlElement ctrl)
        {
            var id = ctrl.Attributes["id"];

            if (id == null)
                return;
            
            var val = id.Value;

            if (String.IsNullOrEmpty(val))
                return;

            if (String.IsNullOrEmpty(ribbonid))
                return;

            if (val.StartsWith(ribbonid + "-_-"))
                return;

            id.Value = String.Format("{0}-_-{1}", ribbonid, id.Value);
        }

        private void SetDefaultRibbonControl(string ribbonid, XmlElement ctrl)
        {
            SetRibbonControlId(ribbonid, ctrl);

            if (RibbonControlSupports(ctrl, "getLabel"))
                SetRibbonControlAttr(ctrl, "getLabel", "GetLabel");
            if (RibbonControlSupports(ctrl, "getDescription"))
                SetRibbonControlAttr(ctrl, "getDescription", "GetDescription");
            if (RibbonControlSupports(ctrl, "getScreentip"))
                SetRibbonControlAttr(ctrl, "getScreentip", "GetScreenTip");
            if (RibbonControlSupports(ctrl, "getSupertip"))
                SetRibbonControlAttr(ctrl, "getSupertip", "GetSuperTip");
            if (RibbonControlSupports(ctrl, "getSize"))
                SetRibbonControlAttr(ctrl, "getSize", "GetSize");
            if (RibbonControlSupports(ctrl, "onAction"))
                SetRibbonControlAttr(ctrl, "onAction", "RunCommand");
            if (RibbonControlSupports(ctrl, "getKeytip"))
                SetRibbonControlAttr(ctrl, "getKeytip", "GetKeyTip");
            if (RibbonControlSupports(ctrl, "getEnabled"))
                SetRibbonControlAttr(ctrl, "getEnabled", "GetEnabled");
            if (RibbonControlSupports(ctrl, "getVisible"))
                SetRibbonControlAttr(ctrl, "getVisible", "GetVisible");
            if (RibbonControlSupports(ctrl, "getShowImage"))
                SetRibbonControlAttr(ctrl, "getShowImage", "GetShowImage");
            if (RibbonControlSupports(ctrl, "getShowLabel"))
                SetRibbonControlAttr(ctrl, "getShowLabel", "GetShowLabel");
        }

        private void SetRibbonControlAttr(XmlElement ctrl, string name, string value)
        {
            string geteqiv;
            string nonget;
            if (name.StartsWith("get"))
            {
                geteqiv = name;
                nonget = String.Format("{0}{1}", char.ToLower(name[3]), name.Substring(4));
                if (ctrl.HasAttribute(nonget) == false)
                {
                    string orig = ctrl.GetAttribute(name);
                    if (String.IsNullOrEmpty(orig) && orig != value)
                        ctrl.SetAttribute(name, value);
                }
            }
            else
            {
                nonget = name;
                geteqiv = String.Format("get{0}", Microsoft.VisualBasic.Strings.StrConv(name, Microsoft.VisualBasic.VbStrConv.ProperCase, 0));

                if (ctrl.HasAttribute(geteqiv))
                    ctrl.RemoveAttribute(geteqiv);

                string orig = ctrl.GetAttribute(name);
                if (String.IsNullOrEmpty(orig) && orig != value)
                    ctrl.SetAttribute(name, value);
            }


        }

        private bool RibbonControlSupports(XmlElement ctrl, string attribute)
        {


            switch (ctrl.Name.ToLower())
            {
                case "customui":
                    return false;
                case "commands":
                    return false;
                case "command":
                    return false;
                case "ribbon":
                    return false;
                case "tabs":
                    return false;
                case "tab":
                    {
                        switch (attribute.ToLower())
                        {
                            case "getkeytip":
                                return true;
                            case "getlabel":
                                return true;
                            case "getvisible":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "group":
                    {
                        switch (attribute.ToLower())
                        {
                            case "getkeytip":
                                return true;
                            case "getlabel":
                                return true;
                            case "getscreentip":
                                return true;
                            case "getsupertip":
                                return true;
                            case "getvisible":
                                return true;
                            default:
                                return false;
                        }

                    }
                case "checkbox":
                    {
                        switch (attribute.ToLower())
                        {
                            case "getlabel":
                                return true;
                            case "getdescription":
                                return true;
                            case "getscreentip":
                                return true;
                            case "getsupertip":
                                return true;
                            case "getkeytip":
                                return true;
                            case "getvisible":
                                return true;
                            case "getenabled":
                                return true;
                            case "getpressed":
                                return true;
                            case "onaction":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "menuseparator":
                    {
                        switch (attribute.ToLower())
                        {
                            case "id":
                                return true;
                            case "text":
                                return true;
                            case "gettext":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "dynamicmenu":
                    {
                        switch (attribute.ToLower())
                        {
                            case "getcontent":
                                return true;
                            case "getsize":
                                return true;
                            case "getlabel":
                                return true;
                            case "getdescription":
                                return true;
                            case "getscreentip":
                                return true;
                            case "getsupertip":
                                return true;
                            case "getkeytip":
                                return true;
                            case "getenabled":
                                return true;
                            case "getvisible":
                                return true;
                            case "getshowimage":
                                return true;
                            case "getshowlabel":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "separator":
                    {
                        switch (attribute.ToLower())
                        {
                            case "id":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "splitbutton":
                    {
                        switch (attribute.ToLower())
                        {
                            case "getenabled":
                                return true;
                            case "getkeytip":
                                return true;
                            case "getsize":
                                return true;
                            case "getshowlabel":
                                return true;
                            case "getvisible":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "menu":
                    {
                        if (ctrl.ParentNode.Name.ToLower() == "group")
                        {
                            switch (attribute.ToLower())
                            {
                                case "getsize":
                                    return true;
                            }
                        }

                        switch (attribute.ToLower())
                        {

                            case "getlabel":
                                return true;
                            case "getdescription":
                                return true;
                            case "getscreentip":
                                return true;
                            case "getsupertip":
                                return true;
                            case "getkeytip":
                                return true;
                            case "getenabled":
                                return true;
                            case "getvisible":
                                return true;
                            case "getshowimage":
                                return true;
                            case "getshowlabel":
                                return true;
                            default:
                                return false;
                        }
                    }
                case "button":
                    {
                        if (ctrl.ParentNode.Name.ToLower() == "contextmenu")
                        {
                            switch (attribute.ToLower())
                            {
                                case "size":
                                    return true;
                                case "getsize":
                                    return false;
                                case "keytip":
                                    return false;
                                case "getkeytip":
                                    return false;
                            }

                        }
                        if (ctrl.ParentNode.Name.ToLower() == "menu")
                        {
                            switch (attribute.ToLower())
                            {
                                case "size":
                                    return false;
                                case "getsize":
                                    return false;
                                case "keytip":
                                    return false;
                                case "getkeytip":
                                    return false;
                            }

                        }
                        if (ctrl.ParentNode.Name.ToLower() == "officemenu")
                        {
                            switch (attribute.ToLower())
                            {
                                case "size":
                                case "getsize":
                                    return false;
                            }
                        }

                        if (ctrl.ParentNode.Name.ToLower() == "dialogboxlauncher")
                        {
                            switch (attribute.ToLower())
                            {
                                case "size":
                                    return false;
                                case "getsize":
                                    return false;
                            }

                        }

                        return true;
                    }
                case "labelcontrol":
                    {
                        switch (attribute.ToLower())
                        {

                            case "getlabel":
                                return true;
                            case "getsupertip":
                                return true;
                            case "getenabled":
                                return true;
                            case "getvisible":
                                return true;
                            case "getshowlabel":
                                return true;
                            default:
                                return false;
                        }
                    }

            }

            return false;

        }



        private RibbonControlConfig GetRibbonControlConfig(string ribbonid, string id, string command, object context)
        {
            bool isSelection;
            var item = GetItemFromSelectionContext(context, out isSelection);
            return GetRibbonControlConfig(ribbonid, id, command, item, isSelection);
        }

        private RibbonControlConfig GetRibbonControlConfig(string ribbonid, string id, string command, object context, bool isDynamic)
        {
            OMSRibbonCache rc = ribbons[ribbonid];

            lock (rc.CachedConfigs)
            {
                if (rc.CachedConfigs.ContainsKey(id) && !isDynamic)
                    return rc.CachedConfigs[id];
            }

            RibbonControlConfig config = new RibbonControlConfig();
            config.Id = id;

            config.Command = ParseCommand(command);
            config.Visible = true;
            config.Enabled = true;

            if (Session.CurrentSession.IsLoggedIn)
            {
                XmlNodeList nodes = rc.Config.SelectNodes(String.Format("/oms/rules/controls/control[@id='{0}']", id));
                if (nodes.Count > 0)
                {
                    XmlAttribute attr_resid = nodes[0].Attributes["resource"];
                    XmlAttribute attr_pack = nodes[0].Attributes["package"];
                    XmlAttribute attr_lics = nodes[0].Attributes["license"];
                    XmlAttribute attr_cond = nodes[0].Attributes["condition"];
                    XmlAttribute attr_role = nodes[0].Attributes["role"];
                    XmlAttribute attr_hide = nodes[0].Attributes["hide"];
                    XmlAttribute attr_size = nodes[0].Attributes["size"];
                    XmlAttribute attr_filt = nodes[0].Attributes["filter"];
                    XmlAttribute attr_efilt = nodes[0].Attributes["enabledFilter"];

                    if (attr_resid != null && attr_resid.Value.Length > 0)
                    {
                        config.ResourceId = attr_resid.Value;
                    }

                    //Look at the old command bar button configuration for default values for text and tooltip etc...
                    if (resources != null)
                    {
                        using (System.Data.DataView vw = new System.Data.DataView(resources))
                        {
                            vw.RowFilter = String.Format("[cdcode] = '{0}'", config.ResourceId.Replace("'", "''"));
                            if (vw.Count > 0)
                            {
                                config.Label = Convert.ToString(vw[0]["cddesc"]);
                                config.Tooltip = Convert.ToString(vw[0]["cdhelp"]);
                            }
                            else
                            {
                                //if they dont exist auto add them
                                if (!string.IsNullOrEmpty(config.ResourceId))
                                {
                                    config.Label = Convert.ToString(Properties.Resources.ResourceManager.GetString(id));
                                    config.Tooltip = Convert.ToString(Properties.Resources.ResourceManager.GetString(string.Format("SuperTip_{0}", id)));
                                    CodeLookup.Create("CBCCAPTIONS", config.ResourceId, config.Label, config.Tooltip, "{default}", true, false, true);
                                }
                            }
                        }
                    }

                    ApplyRibbonControlConfig(context, config, attr_pack, attr_lics, attr_cond, attr_role, attr_hide, attr_filt, attr_efilt);
                    if (config.Visible == false)
                        return config;

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("'{0}' does not exist in rules file.", id));
                }

                nodes = rc.Config.SelectNodes(String.Format("/oms/rules/commands/command[@id='{0}']", command));
                if (nodes.Count > 0)
                {
                    XmlAttribute attr_pack = nodes[0].Attributes["package"];
                    XmlAttribute attr_lics = nodes[0].Attributes["license"];
                    XmlAttribute attr_cond = nodes[0].Attributes["condition"];
                    XmlAttribute attr_role = nodes[0].Attributes["role"];
                    XmlAttribute attr_hide = nodes[0].Attributes["hide"];
                    XmlAttribute attr_filt = nodes[0].Attributes["filter"];
                    XmlAttribute attr_efilt = nodes[0].Attributes["enabledFilter"];

                    ApplyRibbonControlConfig(context, config, attr_pack, attr_lics, attr_cond, attr_role, attr_hide, attr_filt, attr_efilt);
                    if (config.Visible == false)
                        return config;


                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("'{0}' does not exist in rules file.", command));
                }


            }
            lock (rc.CachedConfigs)
            {
                if (rc.CachedConfigs.ContainsKey(id) == false && !isDynamic)
                    rc.CachedConfigs.Add(id, config);
            }
            return config;
        }

        private static string ParseCommand(string command)
        {
            var pars = command.Split(';');
            int start = 0;

            switch (pars[0].ToUpperInvariant())
            {
                case "ADDIN":
                case "SCRIPT":
                    start = 0;
                    break;
                case "OMS":
                case "SYSTEM":
                default:
                    start = 1;
                    break;
            }

            return String.Join(";", pars, start, pars.Length - start);
        }

        private void ApplyRibbonControlConfig(object context, RibbonControlConfig config, XmlAttribute attr_pack, XmlAttribute attr_lics, XmlAttribute attr_cond, XmlAttribute attr_role, XmlAttribute attr_hide, XmlAttribute attr_filt, XmlAttribute attr_efilt)
        {
            if (attr_efilt != null && attr_efilt.Value.Length > 0)
            {
                config.EnabledFilter = attr_efilt.Value;
                config.Enabled = ApplyRibbonFilter(config.EnabledFilter, context);
            }

            if (attr_filt != null && attr_filt.Value.Length > 0)
            {
                config.Filter = attr_filt.Value;
            }

            if (attr_pack != null && attr_pack.Value.Length > 0)
            {
                config.Visible = CheckCriteria(attr_pack.Value, new CriteriaDelegate(Session.CurrentSession.IsPackageInstalled));
                if (config.Visible == false)
                    return;
            }
            if (attr_lics != null && attr_lics.Value.Length > 0)
            {
                config.Visible = CheckCriteria(attr_lics.Value, new CriteriaDelegate(Session.CurrentSession.IsLicensedFor));
                if (config.Visible == false)
                    return;
            }
            if (attr_cond != null && attr_cond.Value.Length > 0)
            {
                config.Visible = Session.CurrentSession.ValidateConditional(null, Convert.ToString(attr_cond.Value).Split(Environment.NewLine.ToCharArray()));
                if (config.Visible == false)
                    return;
            }
            if (attr_role != null && attr_role.Value.Length > 0)
            {
                config.Visible = CheckCriteria(attr_role.Value, new CriteriaDelegate(Session.CurrentSession.CurrentUser.IsInRoles));
                if (config.Visible == false)
                    return;
            }

            if (menuScriptsAgg != null)
            {
                if (menuScriptsAgg.Validate(config, context))
                    return;
            }

            if (attr_hide != null && attr_hide.Value.Length > 0)
            {
                config.Visible = !FWBS.Common.ConvertDef.ToBoolean(attr_hide.Value, false);
                if (config.Visible == false)
                    return;
            }

            if (attr_filt != null && attr_filt.Value.Length > 0)
            {
                config.Visible = ApplyRibbonFilter(config.Filter, context);
                if (config.Visible == false)
                    return;
            }
        }

        private static object GetItemFromSelectionContext(object context, out bool isSelection)
        {
            var sel = context as Microsoft.Office.Interop.Outlook.Selection;
            if (sel == null)
            {
                isSelection = false;
                return context;
            }
            else
            {
                var app = Fwbs.Office.Outlook.OutlookApplication.GetApplication(sel.Application);
                isSelection = true;
                return app.GetItem(sel[1]);
            }
        }



        private delegate bool CriteriaDelegate(string val);

        private bool CheckCriteria(string criteria, CriteriaDelegate callback)
        {
            if (String.IsNullOrEmpty(criteria) || callback == null)
                return true;

            char op = criteria[0];

            if (op == '&' || op == '|')
            {
                string[] vals = criteria.Substring(1).Split(';');

                switch (op)
                {
                    case '&':
                        {
                            foreach (string val in vals)
                            {
                                if (callback(val) == false)
                                    return false;
                            }

                            return true;
                        }
                    case '|':
                        {
                            foreach (string val in vals)
                            {
                                if (callback(val))
                                    return true;
                            }

                            return false;
                        }
                    default:
                        goto case '&';

                }

            }
            else
            {
                return callback(criteria);
            }
        }


        private bool ApplyRibbonFilter(string filter, object context)
        {
            if (IsEmptyInspector(context))
                return false;

            bool tested = false;

            bool isprec = false;
            long precid = 0;
            bool isdoc = false;
            long docid = 0;

            int doccount = OMSApplication.GetDocumentCount();

            context = context ?? OMSApplication;

            if (doccount > 0)
            {
                isprec = OMSApplication.IsPrecedent(context);
                precid = OMSApplication.GetDocVariable(context, FWBS.OMS.Interfaces.IOMSApp.PRECEDENT, 0);
                isdoc = OMSApplication.IsCompanyDocument(context) && !isprec;
                docid = OMSApplication.GetDocVariable(context, FWBS.OMS.Interfaces.IOMSApp.DOCUMENT, 0);
            }

            if (String.IsNullOrEmpty(filter))
                return true;

            string[] filters = filter.Split(';');

            foreach (string cfilter in filters)
            {
                string filt = cfilter;

                if (filt.StartsWith("*!"))
                {

                    if (filt.ToUpper().StartsWith(String.Format("*!{0}", ApplicationName.ToUpper())))
                    {
                        return false;
                    }
                }
                else if (filt.StartsWith("*"))
                {
                    if (filt.ToUpper() == String.Format("*{0}", ApplicationName.ToUpper()))
                    {
                        return true;
                    }
                    else if (filt.ToUpper().Contains(String.Format("*{0}", ApplicationName.ToUpper())))
                    {
                        //need to strip out the application inc +
                        filt = filt.Remove(0, 2 + ApplicationName.Length);
                    }
                    else
                    {
                        tested = true;
                        continue;
                    }

                }


                switch (filt.ToUpper())
                {
                    case "PREC":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isprec == true)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "PREC+NEW":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isprec)
                                {
                                    if (precid == 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "PREC+SAVED":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isprec)
                                {
                                    if (precid != 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "DOC":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isdoc == true)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "DOC+NEW":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isdoc)
                                {
                                    if (docid == 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "DOC+SAVED":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isdoc)
                                {
                                    if (docid != 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "DOC+IN":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isdoc == true)
                                {
                                    DocumentDirection dir = OMSApplication.GetDocDirection(context, null);
                                    if (dir == DocumentDirection.In)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "DOC+OUT":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (isdoc)
                                {
                                    DocumentDirection dir = OMSApplication.GetDocDirection(context, null);
                                    if (dir == DocumentDirection.Out)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "NEW":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                DocumentDirection dir = OMSApplication.GetDocDirection(context, null);
                                if (dir == DocumentDirection.Out)
                                {
                                    if (docid == 0 && precid == 0)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                        break;
                    case "SAVED":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                if (docid != 0 || precid != 0)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "IN":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                DocumentDirection dir = OMSApplication.GetDocDirection(context, null);
                                if (dir == DocumentDirection.In)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "OUT":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                DocumentDirection dir = OMSApplication.GetDocDirection(context, null);
                                if (dir == DocumentDirection.Out)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "ASSOCIATE":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                long associd = OMSApplication.GetDocVariable(context, FWBS.OMS.Interfaces.IOMSApp.ASSOCIATE, 0);

                                if (associd != 0)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "FILE":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                long fileid = OMSApplication.GetDocVariable(context, FWBS.OMS.Interfaces.IOMSApp.FILE, 0);

                                if (fileid != 0)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "CLIENT":
                        {
                            tested = true;
                            if (doccount > 0)
                            {
                                long clid = OMSApplication.GetDocVariable(context, FWBS.OMS.Interfaces.IOMSApp.CLIENT, 0);

                                if (clid != 0)
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case "HIDEWHENSAVED":
                        tested = true;
                        return isdoc == false;

                    case "CONNECTED":
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                                return true;
                        }
                        break;
                }

            }


            if (tested)
                return false;
            else
                return true;
        }

        #endregion

        #region Captured Events


        private void OMS_Connected(object sender, EventArgs e)
        {
            try
            {
                menuScriptsAgg = new FWBS.OMS.Script.MenuScriptAggregator(app,omsapp);
                menuscripts = menuScriptsAgg.MenuScripts;

                resources = CodeLookup.GetLookups("CBCCAPTIONS");

                //Only call this for backward compatibility in command bars.
                if (UseCommandBars)
                {
                    BuildDefaultCommandBar();
                    BuildCommandBars(false);
                }

                RefreshUI(app);

            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private void OMS_Disconnected(object sender, EventArgs e)
        {
            try
            {
                DisconnectDispose();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private void DisconnectDispose()
        {
            if (UseCommandBars)
            {
                UnBuildCommandBars();
            }

            try
            {
                RefreshUI(false, app);
            }
            catch
            {
            }

            try
            {
                menuScriptsAgg.Dispose();
            }
            catch
            {
            }

            if (resources != null)
            {
                resources.Dispose();
                resources = null;
            }

            try
            {
                if (panes != null)
                {
                    for (int ctr = panes.Count - 1; ctr >= 0; ctr--)
                    {
                        Microsoft.Office.Tools.CustomTaskPane pane = panes[ctr];

                        Common.Panes.BasePane basepane = pane.Control as Common.Panes.BasePane;
                        if (basepane != null)
                        {
                            basepane.Dispose();
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                //Already disposed by VSTO
            }
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception as Exception;
            if (ex != null)
            {
                if (ex.Message.StartsWith("Handle is not initialized"))
                    return;
                if (ex.StackTrace == "   at System.Windows.Forms.TabControl.get_SelectedTabInternal()\r\n   at System.Windows.Forms.TabControl.OnEnter(EventArgs e)\r\n   at System.Windows.Forms.ContainerControl.UpdateFocusedControl()" ||
                    ex.StackTrace == "   at System.Windows.Forms.TabControl.get_SelectedTabInternal()\r\n   at System.Windows.Forms.TabControl.OnEnter(EventArgs e)\r\n   at System.Windows.Forms.Control.NotifyEnter()\r\n   at System.Windows.Forms.ContainerControl.UpdateFocusedControl()")
                    return;

                Console.WriteLine("Thread Handler caught : " + ex.Message);
                using (DPIContextBlock contextBlock = omsapp?._dpiAwareness > 0 ? new DPIContextBlock(omsapp._dpiAwareness) : null)
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void currentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                if (ex.Message.StartsWith("Handle is not initialized"))
                    return;

                Console.WriteLine("Thread Handler caught : " + ex.Message);
                using (DPIContextBlock contextBlock = omsapp?._dpiAwareness > 0 ? new DPIContextBlock(omsapp._dpiAwareness) : null)
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        #endregion

        #region Static

        private static OfficeOMSAddin current;

        public static OfficeOMSAddin Current
        {
            get
            {
                return current;
            }

        }

        #endregion

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            OMSRibbonCache rc;
            lock (ribbons)
            {
                if (!ribbons.TryGetValue(ribbonID, out rc))
                {
                    rc = new OMSRibbonCache();
                    ribbons.Add(ribbonID, rc);
                }
            }

            rc.RibbonId = ribbonID;

            try
            {

                rc.RibbonXml = new XmlDocument();
                rc.Config = new XmlDocument();

                ControlResourceRequestEventArgs e = new ControlResourceRequestEventArgs(ribbonID);
                OnControlResourceRequest(e);

                ApplicationSetting ribbonfilelocation = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "RibbonLocation", "Path", "");
                string resourcepath = ribbonfilelocation.ToString() + "\\MCRibbon-" + ribbonID.Trim() + ".xml";
                string configpath = ribbonfilelocation.ToString() + "\\MCRibbonConfig-" + ribbonID.Trim() + ".xml";
                string commandspath = ribbonfilelocation.ToString() + "\\MCRibbonCommands-" + ribbonID.Trim() + ".xml";


                // Original Code as if no XML File available
                if (File.Exists(resourcepath))
                    rc.RibbonXml.Load(resourcepath);
                else if (String.IsNullOrEmpty(e.Resource))
                    rc.RibbonXml.LoadXml(GetResourceText("Fwbs.Oms.Office.Common.DefaultMainRibbon.xml"));
                else
                    rc.RibbonXml.LoadXml(e.Resource);

                rc.RibbonNSM = new XmlNamespaceManager(rc.RibbonXml.NameTable);
                rc.RibbonNSM.AddNamespace("rib", rc.RibbonXml.DocumentElement.NamespaceURI);

                if (File.Exists(configpath))
                    rc.Config.Load(configpath);
                else if (String.IsNullOrEmpty(e.Config))
                    rc.Config.LoadXml(GetResourceText("Fwbs.Oms.Office.Common.DefaultMainRibbonConfig.xml"));
                else
                    rc.Config.LoadXml(e.Config);

                System.Xml.XmlElement root = rc.RibbonXml.DocumentElement;
                SetDefaultRibbonControls(ribbonID, root);

                if (File.Exists(commandspath) || String.IsNullOrEmpty(e.Commands) == false)
                {
                    XmlDocument commands = new XmlDocument();

                    if (File.Exists(commandspath))
                        commands.Load(commandspath);
                    else
                        commands.LoadXml(e.Commands);

                    XmlElement el_commands = rc.RibbonXml.DocumentElement.SelectSingleNode("rib:commands", rc.RibbonNSM) as XmlElement;
                    XmlElement el_ribbon = rc.RibbonXml.DocumentElement.SelectSingleNode("rib:ribbon", rc.RibbonNSM) as XmlElement;
                    if (el_commands == null)
                    {
                        el_commands = rc.RibbonXml.CreateElement("commands", rc.RibbonXml.DocumentElement.NamespaceURI);
                        if (el_ribbon == null)
                            rc.RibbonXml.DocumentElement.AppendChild(el_commands);
                        else
                            rc.RibbonXml.DocumentElement.InsertBefore(el_commands, el_ribbon);

                        //Make sure a onAction attribute is added if one does not exist.  The defaqult is a cancellable
                        //method named RunButtonCommand.
                        el_commands.InnerXml = commands.DocumentElement.InnerXml;
                        foreach (XmlElement el_cmd in el_commands.ChildNodes)
                        {
                            XmlAttribute attr = el_commands.OwnerDocument.CreateAttribute("onAction");
                            attr.Value = "RunButtonCommand";
                            if (!el_cmd.HasAttribute("onAction"))
                                el_cmd.Attributes.Append(attr);
                        }
                    }
                }

                ApplicationSetting debugset = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "DebugRibbonBar", "Enabled", "false");
                bool debug = debugset.ToBoolean();

                if (debug)
                {
                    System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FWBSDEBUG");
                    rc.RibbonXml.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FWBSDEBUG\\MCRibbon-" + ribbonID.Trim() + ".xml");
                    rc.Config.Save(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FWBSDEBUG\\MCRibbonConfig-" + ribbonID.Trim() + ".xml");
                }


                string ret = rc.RibbonXml.OuterXml;

                


                return ret;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                rc.Config.LoadXml(GetResourceText("Fwbs.Oms.Office.Common.DefaultMainRibbonConfig.xml"));
                rc.RibbonXml.LoadXml(GetResourceText("Fwbs.Oms.Office.Common.DefaultMainRibbon.xml"));
                return rc.RibbonXml.OuterXml;
            }

        }





        #endregion

        #region Ribbon Callbacks

        #region Actions / Run Commands       

        public void RunCommand(MSOffice.IRibbonControl control)
        {
            RunCommand(control.Id, control.Tag, control.Context);
        }

        public bool? RunCommand(string id, string command, object context)
        {
            if (String.IsNullOrEmpty(command))
                return null;

            IDisposable contextBlock = null;
            try
            {
                if (omsapp._dpiAwareness > 0)
                {
                    contextBlock = new DPIContextBlock(omsapp._dpiAwareness);
                }
                Cursor.Current = Cursors.WaitCursor;

                GetRibbonIdFromControlId(id, out id);

                string[] commands = command.Split(';');

                bool? handled;

                ControlActionCallbackEventArgs ea = new ControlActionCallbackEventArgs(id, command, context);
                OnControlAction(ea);
                if (ea.Handled)
                {
                    return ea.CancelDefault;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool captured = false;
                            ms.OnExecuteAction(id, command, ref captured);
                            if (captured)
                                return true;
                        }
                    }
                }


                switch (commands[0].ToUpper())
                {
                    case "ADDIN":
                        {
                            handled = RunAddinCommand(id, commands, context);
                        }
                        break;
                    case "SCRIPT":
                        {
                            handled = RunScriptCommand(id, commands);
                        }
                        break;
                    case "OMS":
                        {
                            handled = RunOMSCommand(id, commands);
                        }
                        break;
                    case "SYSTEM":
                        {
                            handled = RunOMSCommand(id, commands);
                        }
                        break;
                    default:
                        {
                            handled = RunOMSCommand(id, commands);
                        }
                        break;
                }

                if (handled == null)
                    return null;

                if (!handled.Value)
                {
                    System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("UNHNDLDCMD", "Unhandled command.  Please make sure the Run Command exists and that you are logged in.", "").Text);
                }

                return handled;

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                if (contextBlock != null)
                    contextBlock.Dispose();
            }
        }

        private bool? RunAddinCommand(string id, string[] commands, object context)
        {
            string command = commands[1].ToUpper();
            switch (command)
            {
                case "CONNECT":
                    {

                        if (Session.CurrentSession.IsLoggedIn)
                            Session.CurrentSession.Disconnect();
                        else
                        {
                            Services.CheckLogin(omsapp.ActiveWindow);
                            omsapp.ActivateWindow(omsapp.ActiveWindow);
                        }

                        return true;
                    }
                case "INSERTPRECEDENT":
                    {
                        if (commands.Length > 2)
                        {
                            Associate assoc = OMSApplication.GetCurrentAssociate(Application);
                            Precedent prec = Precedent.GetPrecedent(Convert.ToInt64(commands[2]));
                            PrecedentJob job = new PrecedentJob(prec);
                            job.Associate = assoc;
                            job.Validate();
                            job.AsNewTemplate = false;
                            Services.ProcessJob(OMSApplication, job);
                            return true;
                        }
                    }
                    break;
                case "DOCPROPSPANE":
                    {
                        Common.Panes.BasePane.Create<Common.Panes.DocumentPropertyPane>(this, command, context, false).Visible = true;
                    }
                    return true;
                case "ASSOCDOCSPANE":
                    {
                        Common.Panes.BasePane.Create<Common.Panes.AssociatedDocumentsPane>(this, command, context, false).Visible = true;
                    }
                    return true;
                case "SEARCH":
                    Common.Panes.BasePane.Create<Common.Panes.GlobalSearchPane>(this, command, context, false).Visible = true;
                    return true;
            }

            return false;
        }

        private bool? RunScriptCommand(string id, string[] commands)
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            return menuScriptsAgg.Execute(OfficeOMSAddin.Current.Application, OfficeOMSAddin.Current.OMSApplication, commands[1]);
        }

        internal string RunScriptCommand(string commandName,  FWBS.Common.KeyValueCollection properties)
        {
            
            object returnValue;
            if (menuScriptsAgg.Invoke(commandName, out returnValue, properties))
                return Convert.ToString(returnValue);
            else
                return null;
            
        }

        private bool? RunOMSCommand(string id, string[] commands)
        {
            System.Windows.Forms.Application.DoEvents();
            string cmd = String.Join(";", commands);
            bool? ret = OfficeOMSAddin.Current.OMSApplication.WillHandleRunCommand(OfficeOMSAddin.Current.OMSApplication, cmd);
            if (ret.HasValue && ret.Value)
            {
                OfficeOMSAddin.Current.OMSApplication.RunCommand(OfficeOMSAddin.Current.OMSApplication, cmd);
                return true;
            }
            else
                return ret;
        }

        public void RunToggleCommand(MSOffice.IRibbonControl control, bool pressed)
        {
            RunToggleCommand(control.Id, control.Tag, control.Context, pressed);
        }

        public bool? RunToggleCommand(string id, string command, object context, bool pressed)
        {



            try
            {

                Cursor.Current = Cursors.WaitCursor;

                if (String.IsNullOrEmpty(command))
                    return null;

                string[] commands = command.Split(';');

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                bool? handled = false;

                ControlToggleCallbackEventArgs ea = new ControlToggleCallbackEventArgs(id, command, context, pressed);
                OnControlToggle(ea);
                if (ea.Handled)
                {
                    return true;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool captured = false;
                            ms.OnExecuteToggle(id, command, pressed, ref captured);
                            if (captured)
                                return true;
                        }
                    }
                }


                switch (commands[0].ToUpper())
                {
                    case "ADDIN":
                        {
                            handled = RunAddinToggleCommand(id, commands, pressed);
                        }
                        break;
                    case "SCRIPT":
                        {
                            handled = RunScriptToggleCommand(id, commands, pressed);
                        }
                        break;
                    case "OMS":
                        {
                            handled = RunOMSToggleCommand(id, commands, pressed);
                        }
                        break;
                    case "SYSTEM":
                        {
                            handled = RunOMSToggleCommand(id, commands, pressed);
                        }
                        break;
                    default:
                        {
                            handled = RunOMSToggleCommand(id, commands, pressed);
                        }
                        break;
                }


                if (handled == null)
                    return null;

                if (!handled.Value)
                {
                    System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("UNHNDLDCMD", "Unhandled command.  Please make sure the Run Command exists and that you are logged in.", "").Text);
                }

                return handled;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool? RunAddinToggleCommand(string id, string[] commands, bool pressed)
        {
            return false;
        }

        private bool? RunScriptToggleCommand(string id, string[] commands, bool pressed)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                try
                {
                    if (menuscripts != null)
                    {
                        if (menuscripts.Count > 0)
                        {
                            foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                            {
                                if (ms != null)
                                {

                                    ms.SetAppObject(OfficeOMSAddin.Current.Application, OfficeOMSAddin.Current.OMSApplication);
                                    System.Reflection.MethodInfo m = ms.GetType().GetMethod(commands[1], BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                                    if (m != null)
                                    {
                                        m.Invoke(ms, null);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    return false;

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                        ex = ex.InnerException;
                    
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("PRBLLDSCRFRMM", "Problem Loading Script From Main Menu ''%1%''", "", commands[1]).Text, ex);
                }
            }

            return false;
        }

        private bool? RunOMSToggleCommand(string id, string[] commands, bool pressed)
        {
            return RunOMSCommand(id, commands);
        }


        public void RunButtonCommand(MSOffice.IRibbonControl control, ref bool cancel)
        {
            cancel = false;
            if (Session.CurrentSession.IsLoggedIn)
            {
                string id, cmd = null;

                GetRibbonIdFromControlId(control.Id, out id);

                switch (id.ToUpperInvariant())
                {
                    case "FILESAVE":
                        if ((this.omsapp.DocumentManagementMode & DocumentManagementMode.Save) == DocumentManagementMode.None)
                            break;
                        cmd = "OMS;SAVE";
                        break;
                    case "FILESAVEAS":
                        if ((this.omsapp.DocumentManagementMode & DocumentManagementMode.Save) == DocumentManagementMode.None)
                            break;   
                        cmd = "OMS;SAVEAS";
                        break;
                    case "FILEOPEN":
                        if ((this.omsapp.DocumentManagementMode & DocumentManagementMode.Open) == DocumentManagementMode.None)
                            break;
                        cmd = "OMS;OPEN";
                        break;
                    case "FILEPRINT":
                    case "FILEPRINTPREVIEW":
                        ApplicationSetting disablePrint = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "", "DisableOMSExcelPrint", "False");
                        if (((this.omsapp.DocumentManagementMode & DocumentManagementMode.Print) == DocumentManagementMode.None) || disablePrint.ToBoolean())
                            break;
                        cmd = id.Equals("FILEPRINT", StringComparison.InvariantCultureIgnoreCase) ? "OMS;PRINT" : "OMS;PRINTPREVIEW";
                        break;
                    case "DELETE":
                        cmd = "ADDIN;DELETEITEM";
                        break;
                    case "EDITMESSAGE":
                        cmd = "ADDIN;EDITMESSAGE";
                        break;
                }

                if (!String.IsNullOrEmpty(cmd))
                {
                    cancel = (RunCommand(id, cmd, control.Context) == true);
                }
            }
        }

        private void addin_ControlAction(object sender, ControlActionCallbackEventArgs e)
        {
            if (useTaskPaneWizards)
            {
                string[] commands = e.Command.Split(';');
                if (commands.Length > 2 && commands[commands.Length - 1].Equals("TASKPANE", StringComparison.OrdinalIgnoreCase))
                {
                    Common.Panes.BasePane.Create<Common.Panes.WizardPane>(this, commands[1].ToUpper(), e.Context, true).Visible = true;
                    e.Handled = true;
                }
            }
        }

        #endregion

        public void OnLoad(MSOffice.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public string GetContent(MSOffice.IRibbonControl control)
        {
            var content = GetContent(control.Id, control.Tag, control.Context);

            string id;

            var ribbonid = GetRibbonIdFromControlId(control.Id, out id);

            if (content != null)
            {
                SetDefaultRibbonControls(ribbonid, content);
                return content.OuterXml;
            }

            return null;
        }

        public XmlElement GetContent(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                XmlDocument doc = new XmlDocument();
                XmlElement menu = doc.CreateElement("menu");
                menu.SetAttribute("xmlns", "http://schemas.microsoft.com/office/2009/07/customui");
                
                doc.AppendChild(menu);

                ControlDynamicMenuCallbackEventArgs ea = new ControlDynamicMenuCallbackEventArgs(id, command, context, menu);
                OnControlDynamicMenuRequest(ea);
                if (ea.Handled)
                {
                    return doc.DocumentElement;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            ms.OnControlDynamicMenuRequest(id, command, menu, ref handled);
                            if (handled)
                            {
                                return doc.DocumentElement;
                            }
                        }
                    }
                }

                switch (command.ToUpper())
                {
                    case "ADDIN;INSERTPRECEDENT":
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                            {
                                BuildInsertPrecedentMenu(menu);
                            }
                        }
                        break;
                }

                return doc.DocumentElement;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public bool GetEnabled(MSOffice.IRibbonControl control)
        {
            return GetEnabled(control.Id, control.Tag, control.Context);
        }
        public bool GetEnabled(string id, string command, object context)
        {
            try
            {
                if (IsEmptyInspector(context))
                    return false;

                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                if (AlwaysVisible.Contains(command) || AlwaysVisible.Contains(id))
                    return true;

                RibbonControlConfig config = GetRibbonControlConfig(ribbonid, id, command, context);

                ControlVisibleCallbackEventArgs ea = new ControlVisibleCallbackEventArgs(id, command, context, config.EnabledFilter);
                OnControlEnabledRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            bool ret = ms.OnControlEnabledRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }


                if (config.Enabled == false)
                    return false;


                return Session.CurrentSession.IsLoggedIn;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }


        public bool GetVisible(MSOffice.IRibbonControl control)
        {
            return GetVisible(control.Id, control.Tag, control.Context);
        }
        public bool GetVisible(string id, string command, object context)
        {
            try
            {
                if (IsEmptyInspector(context))
                    return false;

                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                if (AlwaysVisible.Contains(command) || AlwaysVisible.Contains(id))
                    return true;

                RibbonControlConfig config = GetRibbonControlConfig(ribbonid, id, command, context);

                ControlVisibleCallbackEventArgs ea = new ControlVisibleCallbackEventArgs(id, command, context, config.Filter);
                OnControlVisibleRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;

                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            bool ret = ms.OnControlVisibleRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                if (config.Visible == false)
                    return false;

                return Session.CurrentSession.IsLoggedIn;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }

        }

        private static bool IsEmptyInspector(object context)
        {
            Microsoft.Office.Interop.Outlook.Inspector inspector = context as Microsoft.Office.Interop.Outlook.Inspector;

            if (inspector == null)
                return false;

            if (inspector.CurrentItem == null)
                return true;

            return false;
        }

        public bool GetPressed(MSOffice.IRibbonControl control)
        {
            return GetPressed(control.Id, control.Tag, control.Context);
        }
        public bool GetPressed(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlFlagCallbackEventArgs ea = new ControlFlagCallbackEventArgs(id, command, context);
                OnControlPressedRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }


                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            bool ret = ms.OnControlPressedRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public bool GetShowImage(MSOffice.IRibbonControl control)
        {
            return GetShowImage(control.Id, control.Tag, control.Context);
        }
        public bool GetShowImage(string id, string command, object context)
        {

            try
            {
                if (IsEmptyInspector(context))
                    return false;

                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlFlagCallbackEventArgs ea = new ControlFlagCallbackEventArgs(id, command, context);
                OnControlShowImageRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            bool ret = ms.OnControlShowImageRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public bool GetShowLabel(MSOffice.IRibbonControl control)
        {
            return GetShowLabel(control.Id, control.Tag, control.Context);
        }
        public bool GetShowLabel(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                GetRibbonIdFromControlId(id, out id);

                ControlFlagCallbackEventArgs ea = new ControlFlagCallbackEventArgs(id, command, context);
                OnControlShowLabelRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }


                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            bool ret = ms.OnControlShowLabelRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }

        }

        public MSOffice.RibbonControlSize GetSize(MSOffice.IRibbonControl control)
        {
            return GetSize(control.Id, control.Tag, control.Context);
        }
        public MSOffice.RibbonControlSize GetSize(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                switch (command.ToUpper())
                {
                    case RunCommands.Connect:
                        {
                            return MSOffice.RibbonControlSize.RibbonControlSizeLarge;
                        }
                }

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlSizeRequest(ea);
                if (ea.Handled)
                {
                    switch (ea.ReturnValue.ToLower())
                    {
                        case "normal":
                            return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeRegular;
                        case "large":
                            return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
                        default:
                            goto case "normal";
                    }
                }


                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlSizeRequest(id, command, ref handled);
                            if (handled)
                            {
                                switch (ret.ToLower())
                                {
                                    case "normal":
                                        return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeRegular;
                                    case "large":
                                        return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
                                    default:
                                        goto case "normal";
                                }
                            }
                        }
                    }
                }

                RibbonControlConfig config = GetRibbonControlConfig(ribbonid, id, command, context);
                switch (config.Size.ToLower())
                {
                    case "normal":
                        return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeRegular;
                    case "large":
                        return Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
                    default:
                        goto case "large";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }

        }

        public string GetKeyTip(MSOffice.IRibbonControl control)
        {
            return GetKeyTip(control.Id, control.Tag, control.Context);
        }
        public string GetKeyTip(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlKeyTipRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlKeyTipRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                return String.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public string GetLabel(MSOffice.IRibbonControl control)
        {
            string text = GetLabel(control.Id, control.Tag, control.Context);

            if (text == null)
                return String.Empty;

            if (Session.CurrentSession.IsLoggedIn)
                text = Session.CurrentSession.Terminology.Parse(text, true);

            return text;
        }

        public string GetLabel(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlLabelRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }


                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlLabelRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                RibbonControlConfig config = GetRibbonControlConfig(ribbonid, id, command, context);
                if (String.IsNullOrEmpty(config.Label) == false)
                    return config.Label;

                switch (command.ToUpper())
                {
                    case RunCommands.Connect:
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                                return Session.CurrentSession.RegistryRes("Disconnect", Properties.Resources.Disconnect);
                            else
                                return Session.CurrentSession.RegistryRes("Connect", Properties.Resources.Connect);
                        }
                }

                return Session.CurrentSession.RegistryRes(String.Format("Ribbon_{0}_Label", id), Properties.Resources.ResourceManager.GetString(id));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }


        }

        public string GetLogonLabel(MSOffice.IRibbonControl control)
        {
            string Message;
            if (Session.CurrentSession.IsLoggedIn)
                Message = Session.CurrentSession.CurrentDatabase.ToString();
            else
                Message = GetLabel(control.Id, control.Tag, control.Context);

            if (Message == null)
                return String.Empty;

            if (Session.CurrentSession.IsLoggedIn)
                Message = Session.CurrentSession.Terminology.Parse(Message, true);

            return Message;
        }

        public string GetDescription(MSOffice.IRibbonControl control)
        {
            string text = GetDescription(control.Id, control.Tag, control.Context);

            if (text == null)
                text = GetLabel(control);
            else
            {
                if (Session.CurrentSession.IsLoggedIn)
                    text = Session.CurrentSession.Terminology.Parse(text, true);
            }

            return text;
        }

        public string GetDescription(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlDescriptionRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlDescriptionRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                switch (command.ToUpper())
                {
                    case RunCommands.Connect:
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                                return Session.CurrentSession.RegistryRes("Disconnect", Properties.Resources.Disconnect);
                            else
                                return Session.CurrentSession.RegistryRes("Connect", Properties.Resources.Connect);
                        }
                }

                return Session.CurrentSession.RegistryRes(String.Format("Ribbon_{0}_Desc", id), Properties.Resources.ResourceManager.GetString(String.Format("Description_{0}", id)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }

        }

        public string GetScreenTip(MSOffice.IRibbonControl control)
        {
            string text = GetScreenTip(control.Id, control.Tag, control.Context);

            if (text == null)
                text = GetLabel(control);
            else
            {
                if (Session.CurrentSession.IsLoggedIn)
                    text = Session.CurrentSession.Terminology.Parse(text, true);
            }

            return text;
        }

        public string GetScreenTip(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlScreenTipRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlScreenTipRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }

                switch (command.ToUpper())
                {
                    case RunCommands.Connect:
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                                return Session.CurrentSession.RegistryRes("Disconnect_Tip", Properties.Resources.Tip_Disconnect);
                            else
                                return Session.CurrentSession.RegistryRes("Connect_Tip", Properties.Resources.Tip_Connect);
                        }
                }

                return Session.CurrentSession.RegistryRes(String.Format("Ribbon_{0}_Tip", id), Properties.Resources.ResourceManager.GetString(String.Format("Tip_{0}", id)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }


        public string GetSuperTip(MSOffice.IRibbonControl control)
        {
            string text = GetSuperTip(control.Id, control.Tag, control.Context);

            if (text != null)
            {
                if (Session.CurrentSession.IsLoggedIn)
                    text = Session.CurrentSession.Terminology.Parse(text, true);
            }

            return text;
        }

        public string GetSuperTip(string id, string command, object context)
        {
            try
            {
                if (command == null)
                    command = String.Empty;

                var ribbonid = GetRibbonIdFromControlId(id, out id);

                ControlStringCallbackEventArgs ea = new ControlStringCallbackEventArgs(id, command, context);
                OnControlSuperTipRequest(ea);
                if (ea.Handled)
                {
                    return ea.ReturnValue;
                }

                if (menuscripts != null)
                {
                    foreach (FWBS.OMS.Script.MenuScriptType ms in menuscripts)
                    {
                        if (ms != null)
                        {
                            bool handled = false;
                            string ret = ms.OnControlSuperTipRequest(id, command, ref handled);
                            if (handled)
                                return ret;
                        }
                    }
                }


                RibbonControlConfig config = GetRibbonControlConfig(ribbonid, id, command, context);
                if (String.IsNullOrEmpty(config.Label) == false)
                {
                    if (config.Tooltip.Length > 1)
                        return config.Tooltip;
                }

                switch (command.ToUpper())
                {
                    case RunCommands.Connect:
                        {
                            if (Session.CurrentSession.IsLoggedIn)
                                return Session.CurrentSession.RegistryRes("Ribbon_Disconnect_SuperTip", Properties.Resources.SuperTip_Disconnect);
                            else
                                return Session.CurrentSession.RegistryRes("Ribbon_Connect_SuperTip", Properties.Resources.SuperTip_Connect);
                        }
                }

                return Session.CurrentSession.RegistryRes(String.Format("Ribbon_{0}_SuperTip", id), Properties.Resources.ResourceManager.GetString(String.Format("SuperTip_{0}", id)));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }
        }

        public stdole.IPictureDisp GetImage(string name)
        {
            try
            {
                stdole.IPictureDisp pic = null;

                try
                {
                    using (Stream strm = CurrentUIVersion.UseOfficeImages ? null : Assembly.GetExecutingAssembly().GetManifestResourceStream($"Fwbs.Oms.Office.Common.Images.{name}.png"))
                    {
                        if (strm == null)
                        {
                            pic = OfficeOMSAddin.Current.CommandBars.GetImageMso(name.TrimEnd('+', '#', '-'), 64, 64);
                        }
                        else
                        {
                            using (System.Drawing.Image img = System.Drawing.Bitmap.FromStream(strm))
                            {
                                pic = PictureDispMaker.ConvertImage(img);
                            }
                        }
                    }
                }
                catch
                {
                    pic = null;
                }

                if (pic == null)
                    pic = OfficeOMSAddin.Current.CommandBars.GetImageMso("HappyFace", 64, 64);

                return pic;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw;
            }


        }


        public stdole.IPictureDisp GetConnectImage(MSOffice.IRibbonControl control)
        {
            if (Session.CurrentSession.IsLoggedIn)
                return GetImage("DeclineInvitation");
            else
                return GetImage("AcceptInvitation");
        }


        #endregion

        #region Dynamic Menus

        private System.Xml.XmlElement CreateDynamicMenuControl(string ribbonid, XmlElement menu, string type)
        {
            XmlElement ctrl = menu.OwnerDocument.CreateElement(type);
            menu.AppendChild(ctrl);

            SetDefaultRibbonControl(ribbonid, ctrl);

            return ctrl;
        }

        private void BuildInsertPrecedentMenu(XmlElement menu)
        {
            Associate assoc = OMSApplication.GetCurrentAssociate(Application);
            string type = OMSApplication.GetActiveDocType(Application);

            System.Data.DataTable dt = (assoc == null) ? null : Precedent.GetAssocPrecedents(assoc, type);

            string id;
            var ribbonid = GetRibbonIdFromControl(menu, out id);

            if (dt == null || dt.Rows.Count == 0)
            {
                XmlElement ctrl = CreateDynamicMenuControl(ribbonid, menu, "button");
                SetRibbonControlAttr(ctrl, "id", String.Format("DynamicOMS_Insert_Precedent_Zero"));

            }
            else
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    XmlElement ctrl = CreateDynamicMenuControl(ribbonid, menu, "button");

                    SetRibbonControlAttr(ctrl, "id", String.Format("DynamicMenu_Insert_Precedent_{0}", row["precid"]));
                    SetRibbonControlAttr(ctrl, "label", String.Format("{0} - {1}", Convert.ToString(row["prectitle"]), Convert.ToString(row["precdesc"])));
                    SetRibbonControlAttr(ctrl, "description", String.Format("{0} - {1}", Convert.ToString(row["prectitle"]), Convert.ToString(row["precdesc"])));
                    SetRibbonControlAttr(ctrl, "tag", String.Format("ADDIN;INSERTPRECEDENT;{0}", row["precid"]));
                    SetRibbonControlAttr(ctrl, "image", "MailMergeAddressBlockInsert");

                }



            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ribbons != null)
                {
                    lock (ribbons)
                    {
                        foreach (var rc in ribbons.Values)
                        {
                            lock (rc.CachedConfigs)
                            {
                                rc.CachedConfigs.Clear();
                            }
                        }
                    }
                }

                DisconnectDispose();

                app = null;

                this._omsbar = null;
                this._omsbarctrls.Clear();
                this._precbar = null;
                this._precbarctrls.Clear();
                this._timebar = null;
                this._timebarctrls.Clear();

            }

        }

        ~OfficeOMSAddin()
        {
            Dispose(false);
        }


        #endregion

        #region Command Bar Building

        private object _omsbar;
        private List<MSOffice.CommandBarControl> _omsbarctrls = new List<MSOffice.CommandBarControl>();
        private object _precbar;
        private List<MSOffice.CommandBarControl> _precbarctrls = new List<MSOffice.CommandBarControl>();
        private object _timebar;
        private List<MSOffice.CommandBarControl> _timebarctrls = new List<MSOffice.CommandBarControl>();

        private bool CommandBarMergeMenus
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "MergeMenus", "False");
                return reg.ToBoolean();
            }
        }


        private bool CommandBarPrecedentMenuVisible
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\CommandBars", CBM.PRECEDENT_COMMAND_BAR_CODE, "False");
                return reg.ToBoolean();
            }
        }

        private bool CommandBarTimeMenuVisible
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\CommandBars", CBM.TIME_COMMAND_BAR_CODE, "True");
                return reg.ToBoolean();
            }
        }

        private bool CommandBarMainMenuVisible
        {
            get
            {
                FWBS.Common.ApplicationSetting reg = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\CommandBars", CBM.MAIN_COMMAND_BAR_CODE, "True");
                return reg.ToBoolean();
            }
        }


        private void BuildDefaultCommandBar()
        {
            BuildDefaultCommandBar(CommandBars);
        }



        private void BuildDefaultCommandBar(MSOffice.CommandBars bars)
        {
            try
            {
                bool merge = CommandBarMergeMenus;

                MSOffice.CommandBarButton ctrlConnect = null;
                MSOffice.CommandBarButton ctrlDisconnect = null;


                if (merge)
                {
                    MSOffice.CommandBar menu = null;

                    foreach (MSOffice.CommandBar cb in bars)
                    {
                        if (cb.Type == MSOffice.MsoBarType.msoBarTypeMenuBar)
                        {
                            menu = cb;
                            break;
                        }
                    }

                    if (menu != null)
                    {
                        try
                        {
                            _omsbar = menu.Controls[CBM.GetMainName()];
                        }
                        catch
                        { }

                        if (_omsbar == null)
                        {
                            MSOffice.CommandBarPopup omsbar = (MSOffice.CommandBarPopup)menu.Controls.Add(MSOffice.MsoControlType.msoControlPopup, Missing.Value, Missing.Value, Missing.Value, true);
                            omsbar.Caption = CBM.MAIN_COMMAND_BAR_NAME;
                            omsbar.Visible = true;
                            _omsbar = omsbar;

                            ctrlConnect = (MSOffice.CommandBarButton)omsbar.Controls.Add(MSOffice.MsoControlType.msoControlButton, Missing.Value, Missing.Value, Missing.Value, true);
                            ctrlDisconnect = (MSOffice.CommandBarButton)omsbar.Controls.Add(MSOffice.MsoControlType.msoControlButton, Missing.Value, Missing.Value, Missing.Value, true);
                            
                        }

                        CBM.Add(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME);

                        try
                        {
                            _timebar = bars[CBM.GetTimeName()];
                        }
                        catch { }

                        if (_timebar == null)
                        {
                            MSOffice.CommandBarPopup timebar = (MSOffice.CommandBarPopup)menu.Controls.Add(MSOffice.MsoControlType.msoControlPopup, Missing.Value, Missing.Value, Missing.Value, true);
                            timebar.Caption = CBM.TIME_COMMAND_BAR_NAME;
                            timebar.Visible = false;
                            _timebar = timebar;
                        }
                        CBM.Add(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME);

                        try
                        {
                            _precbar = bars[CBM.GetPrecedentName()];
                        }
                        catch { }

                        if (_precbar == null)
                        {
                            MSOffice.CommandBarPopup precbar = (MSOffice.CommandBarPopup)menu.Controls.Add(MSOffice.MsoControlType.msoControlPopup, Missing.Value, Missing.Value, Missing.Value, true);
                            precbar.Caption = CBM.PRECEDENT_COMMAND_BAR_NAME;
                            precbar.Visible = false;
                            _precbar = precbar;
                        }

                        CBM.Add(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME);


                    }
                }
                else
                {
                    try
                    {
                        _omsbar = bars[CBM.GetMainName()];
                    }
                    catch { }

                    if (_omsbar == null)
                    {
                        MSOffice.CommandBar omsbar = bars.Add(CBM.MAIN_COMMAND_BAR_NAME, Missing.Value, Missing.Value, true);
                        omsbar.Position = MSOffice.MsoBarPosition.msoBarTop;
                        omsbar.Visible = true;
                        _omsbar = omsbar;

                        ctrlConnect = (MSOffice.CommandBarButton)omsbar.Controls.Add(MSOffice.MsoControlType.msoControlButton, Missing.Value, Missing.Value, Missing.Value, true);
                        ctrlDisconnect = (MSOffice.CommandBarButton)omsbar.Controls.Add(MSOffice.MsoControlType.msoControlButton, Missing.Value, Missing.Value, Missing.Value, true);

                    }

                    CBM.Add(CBM.MAIN_COMMAND_BAR_CODE, CBM.MAIN_COMMAND_BAR_NAME);

                    try
                    {
                        _timebar = bars[CBM.GetTimeName()];
                    }
                    catch { }

                    if (_timebar == null)
                    {
                        MSOffice.CommandBar timebar = bars.Add(CBM.TIME_COMMAND_BAR_NAME, Missing.Value, Missing.Value, true);
                        timebar.Position = MSOffice.MsoBarPosition.msoBarTop;
                        timebar.Visible = false;
                        _timebar = timebar;
                    }

                    CBM.Add(CBM.TIME_COMMAND_BAR_CODE, CBM.TIME_COMMAND_BAR_NAME);

                    try
                    {
                        _precbar = bars[CBM.GetPrecedentName()];
                    }
                    catch { }

                    if (_precbar == null)
                    {
                        MSOffice.CommandBar precbar = bars.Add(CBM.PRECEDENT_COMMAND_BAR_NAME, Missing.Value, Missing.Value, true);
                        precbar.Position = MSOffice.MsoBarPosition.msoBarTop;
                        precbar.Visible = false;
                        _precbar = precbar;
                    }

                    CBM.Add(CBM.PRECEDENT_COMMAND_BAR_CODE, CBM.PRECEDENT_COMMAND_BAR_NAME);

                }


                if (ctrlConnect != null)
                {
                    ctrlConnect.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(CommandBarButtonClick);
                    ctrlConnect.Tag = "SYSTEM;CONNECT";
                    ctrlConnect.Style = MSOffice.MsoButtonStyle.msoButtonIconAndCaption;
                    ctrlConnect.Caption = Properties.Resources.Connect;
                    ctrlConnect.TooltipText = Properties.Resources.SuperTip_Connect;
                    ctrlConnect.FaceId = 279;
                    ctrlConnect.Visible = true;
                    _omsbarctrls.Add(ctrlConnect);
                }

                if (ctrlDisconnect != null)
                {
                    ctrlDisconnect.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(CommandBarButtonClick);
                    ctrlDisconnect.Tag = "SYSTEM;DISCONNECT";
                    ctrlDisconnect.Style = MSOffice.MsoButtonStyle.msoButtonIconAndCaption;
                    ctrlDisconnect.Caption = Properties.Resources.Disconnect;
                    ctrlDisconnect.TooltipText = Properties.Resources.SuperTip_Disconnect;
                    ctrlDisconnect.FaceId = 2151;
                    ctrlDisconnect.Visible = true;
                    _omsbarctrls.Add(ctrlDisconnect);

                }
            }

            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }


        private void BuildCommandBars(bool local)
        {

            BuildCommandBar(this, local, _omsbar, _omsbarctrls, CBM.MAIN_COMMAND_BAR_CODE, "", CommandBarMainMenuVisible);
            BuildCommandBar(this, local, _timebar, _timebarctrls, CBM.TIME_COMMAND_BAR_CODE, "", CommandBarTimeMenuVisible);

            if (Session.CurrentSession.IsLoggedIn)
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles("PRECEDIT"))
                {
                    BuildCommandBar(this, local, _precbar, _precbarctrls, CBM.PRECEDENT_COMMAND_BAR_CODE, "", CommandBarPrecedentMenuVisible);
                }
            }

            MSOffice.CommandBarPopup popup = _omsbar as MSOffice.CommandBarPopup;
            MSOffice.CommandBar bar = _omsbar as MSOffice.CommandBar;
            if (popup != null)
                popup.Visible = true;
            else if (bar != null)
                bar.Visible = true;

            RunCommand("", "SYSTEM;TOOLBARSCHANGED", null);
        }

        private void BuildCommandBar(object doc, bool local, object bar, List<MSOffice.CommandBarControl> ctrls, string commandBar, string filter, bool visible)
        {

            try
            {


                //Get the data set that holds the rendering values.
                System.Data.DataSet cb = null;

                //Parent command bar for using FindControl
                MSOffice.CommandBar parent = null;

                if (local)
                {

                    try
                    {
                        cb = FWBS.OMS.Session.GetLocalCommandBar(commandBar);
                    }
                    catch
                    {
                        return;
                    }
                }
                else
                    cb = FWBS.OMS.Session.CurrentSession.GetCommandBar(commandBar);

                MSOffice.CommandBarPopup popup = bar as MSOffice.CommandBarPopup;
                MSOffice.CommandBar cmdbar = bar as MSOffice.CommandBar;

                //Place the bar that to the position specified in the database.
                //Create it if it has not been already.
                if (cmdbar != null)
                {
                    parent = cmdbar;
                    cmdbar.Name = Convert.ToString(cb.Tables["COMMANDBAR"].Rows[0]["cbdesc"]);
                    cmdbar.Position = (MSOffice.MsoBarPosition)System.Enum.Parse(typeof(MSOffice.MsoBarPosition), Convert.ToString(cb.Tables["COMMANDBAR"].Rows[0]["cbposition"]), true);
                }
                else if (popup != null)
                {
                    parent = popup.Parent;
                }
                else
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("CMDBRCRPTNPRNT", "Command bar is corrupt.  It has no parent.", "").Text);

                //Reinitialise the controls array list with the number of controls in the database.
                //But first, reduce the existing controls click delegate counter.

                foreach (MSOffice.CommandBarControl cbc in ctrls)
                {
                    if (cbc is MSOffice.CommandBarButton)
                    {
                        try
                        {
                            MSOffice.CommandBarButton btn = (MSOffice.CommandBarButton)cbc;
                            btn.Click -= new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(CommandBarButtonClick);
                        }
                        catch { }
                    }
                }

                int vis_count = 0;


                //Loop through all the data rows and start building each control that
                //is to exist on the 

                foreach (System.Data.DataRowView row in cb.Tables["CONTROLS"].DefaultView)
                {
                    //Current control to be added.
                    MSOffice.CommandBarControl ctrl = null;

                    //Control type parsed into the office command bar control type from a string in the database.
                    MSOffice.MsoControlType tpe = (MSOffice.MsoControlType)System.Enum.Parse(typeof(MSOffice.MsoControlType), Convert.ToString(row["ctrltype"]), true);

                    //***************************
                    //Control Specific Properties
                    //***************************
                    string tag = String.Empty;
                    switch (Convert.ToString(row["ctrlcode"]))
                    {
                        case "CONNECT":
                            {
                                tag = "SYSTEM;CONNECT";
                            }
                            break;
                        case "DISCONNECT":
                            {
                                tag = "SYSTEM;DISCONNECT";
                            }
                            break;
                        default:
                            {
                                if (Convert.ToString(row["ctrlrunCommand"]).StartsWith("SCRIPT"))
                                    tag = String.Format("{0};{1}", Convert.ToString(row["ctrlid"]), Convert.ToString(row["ctrlrunCommand"]));
                                else
                                    tag = String.Format("{0};OMS;{1}", Convert.ToString(row["ctrlid"]), Convert.ToString(row["ctrlrunCommand"]));
                            }
                            break;
                    }

                    var mi = new FWBS.OMS.Script.MenuItem();
                    mi.Id = Convert.ToString(row["ctrlid"]);
                    mi.Command = Convert.ToString(row["ctrlrunCommand"]);
                    mi.Label = Convert.ToString(row["ctrldesc"]);
                    mi.Tooltip = Convert.ToString(row["ctrlhelp"]);
                    mi.Filter = Convert.ToString(row["ctrlfilter"]);

                    //If the control has no parent value then add it to the parent bar.
                    //Otherwise, add it to the control if it is a popup style control
                    //that is.
                    if (tpe == MSOffice.MsoControlType.msoControlPopup)
                    {
                        tag = Convert.ToString(row["ctrlcode"]);
                        ctrl = parent.FindControl(Missing.Value, Missing.Value, tag, Missing.Value, true);
                    }
                    else
                        ctrl = parent.FindControl(Missing.Value, Missing.Value, tag, Missing.Value, true);


                    if (ctrl == null)
                    {
                        if (row["ctrlparent"] == DBNull.Value)
                        {
                            if (popup != null)
                                ctrl = popup.Controls.Add(tpe, Missing.Value, Missing.Value, Missing.Value, true);
                            else if (cmdbar != null)
                                ctrl = cmdbar.Controls.Add(tpe, Missing.Value, Missing.Value, Missing.Value, true);

                        }
                        else
                        {
                            ctrl = parent.FindControl(Missing.Value, Missing.Value, Convert.ToString(row["ctrlparent"]), Missing.Value, true);
                            if (ctrl is MSOffice.CommandBarPopup)
                            {
                                ctrl = ((MSOffice.CommandBarPopup)ctrl).Controls.Add(tpe, Missing.Value, Missing.Value, Missing.Value, true);
                            }
                        }
                    }


                    //If the control has been successfully created then set the control properties.
                    if (ctrl != null)
                    {

                        ctrl.Tag = tag;
                        ctrl.Caption = mi.Label;
                        ctrl.TooltipText = mi.Tooltip;

                        ctrl.BeginGroup = Convert.ToBoolean(row["ctrlbegingroup"]);

                        //**********************
                        //Button Type Properties
                        //**********************
                        if (ctrl is MSOffice.CommandBarButton)
                        {

                            MSOffice.CommandBarButton btn = (MSOffice.CommandBarButton)ctrl;
                            if (row["ctrlicon"] == DBNull.Value)
                                btn.Style = MSOffice.MsoButtonStyle.msoButtonCaption;
                            else
                            {
                                btn.Style = MSOffice.MsoButtonStyle.msoButtonIconAndCaption;
                                btn.FaceId = Convert.ToInt32(row["ctrlicon"]);
                            }

                            btn.Click += new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(CommandBarButtonClick);
                        }


                        if (IsCommandBarControlVisible(row.Row, filter))
                        {
                            mi.Visible = true;
                            mi.Enabled = true;
                        }
                        else
                        {
                            mi.Enabled = true;
                            mi.Visible = false;
                        }

                        if (menuScriptsAgg != null)
                        {
                            menuScriptsAgg.Validate(mi, doc);

                            ctrl.Caption = mi.Label;
                            ctrl.TooltipText = mi.Tooltip;
                            ctrl.Visible = mi.Visible;
                            ctrl.Enabled = mi.Enabled;
                        }
                        else
                        {
                            ctrl.Visible = true;
                        }

                        if (ctrl.Visible)
                            vis_count = vis_count + 1;

                        //Store the control reference.
                        if (ctrls.Contains(ctrl) == false)
                            ctrls.Add(ctrl);

                        //Up the counter.
                    }
                }

                if (vis_count == 0)
                {
                    if (popup != null)
                        popup.Visible = false;
                    else if (cmdbar != null)
                        cmdbar.Visible = false;
                }
                else
                {
                    if (popup != null)
                        popup.Visible = visible;
                    else if (cmdbar != null)
                        cmdbar.Visible = visible;
                }

            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }

        }

        private bool IsCommandBarControlVisible(System.Data.DataRow ctrl, string filter)
        {
            User usr = Session.CurrentSession.CurrentUser;

            if (usr.IsInRoles(Convert.ToString(ctrl["ctrlrole"])) == false)
            {
                return false;
            }

            if (Session.CurrentSession.ValidateConditional(null, Convert.ToString(ctrl["ctrlcondition"]).Split(Environment.NewLine.ToCharArray())) == false)
            {
                return false;
            }



            System.Data.DataView vw = new System.Data.DataView(ctrl.Table);
            if (filter == String.Empty)
            {
                string f = "(ctrlfilter = '*' or ctrlfilter = '{0}') and ctrlid = '{1}'";
                f = String.Format(f, ApplicationName, Convert.ToString(ctrl["ctrlid"]));
                vw.RowFilter = f;
                if (vw.Count == 0)
                {
                    f = "((ctrlfilter like '%[*]%' or ctrlfilter like '%{0}%') and (ctrlfilter like '%[*]%' and ctrlfilter not like '%!{0}%')) and ctrlid = '{1}'";
                    f = String.Format(f, ApplicationName, Convert.ToString(ctrl["ctrlid"]));
                    vw.RowFilter = f;
                }
            }
            else
            {
                vw.RowFilter = "(" + filter + ") and ctrlid = '" + Convert.ToString(ctrl["ctrlid"]) + "'";
            }

            return vw.Count > 0;
        }

        private void UnBuildCommandBar(object cmdbar, bool visible)
        {
            try
            {
                if (cmdbar != null)
                {
                    MSOffice.CommandBarPopup popup = cmdbar as MSOffice.CommandBarPopup;
                    MSOffice.CommandBar bar = cmdbar as MSOffice.CommandBar;

                    if (popup != null)
                    {
                        foreach (MSOffice.CommandBarControl t in popup.Controls)
                        {
                            if ((t.Tag == "SYSTEM;CONNECT") || (t.Tag == "SYSTEM;DISCONNECT"))
                            { }
                            else
                                t.Delete(true);
                        }

                        if (popup.Visible != visible)
                            popup.Visible = visible;

                    }
                    else if (bar != null)
                    {
                        foreach (MSOffice.CommandBarControl t in bar.Controls)
                        {
                            if ((t.Tag == "SYSTEM;CONNECT") || (t.Tag == "SYSTEM;DISCONNECT"))
                            { }
                            else
                                t.Delete(true);
                        }

                        if (bar.Visible != visible)
                            bar.Visible = visible;
                    }


                }

            }
            catch { }
        }

        private void UnBuildCommandBars()
        {
            UnBuildCommandBar(_omsbar, true);
            UnBuildCommandBar(_timebar, false);
            UnBuildCommandBar(_precbar, false);


            RunCommand("", "SYSTEM;TOOLBARSCHANGED", null);
        }

        private void CommandBarButtonClick(MSOffice.CommandBarButton button, ref bool cancelDefault)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if ((button.Tag == "SYSTEM;CONNECT") || (button.Tag == "SYSTEM;DISCONNECT"))
                {
                    RunCommand(button.Id.ToString(), button.Tag, null);
                }
                else
                {
                    List<string> commands = new List<string>(button.Tag.Split(';'));
                    commands.RemoveAt(0);
                    RunCommand(button.Id.ToString(), String.Join(";", commands.ToArray()), null);
                }
                cancelDefault = true;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                System.Windows.Forms.MessageBox.Show(button.Tag.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }

        #endregion

        private class OMSRibbonCache
        {
            public string RibbonId { get; set; }
            public XmlDocument Config { get; set; }
            public XmlDocument RibbonXml { get; set; }
            public XmlNamespaceManager RibbonNSM { get; set; }

            private Dictionary<string, RibbonControlConfig> cachedconfigs = new Dictionary<string, RibbonControlConfig>();

            public Dictionary<string, RibbonControlConfig> CachedConfigs
            {
                get
                {
                    return cachedconfigs;
                }
            }
        }
    }
}
