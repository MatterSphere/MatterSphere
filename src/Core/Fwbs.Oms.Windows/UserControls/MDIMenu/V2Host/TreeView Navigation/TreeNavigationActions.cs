using System;
using System.Collections;
using System.Windows.Forms;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.UI.UserControls.MDIMenu.V2Host.TreeView_Navigation;
using FWBS.OMS.UI.Windows.Admin;
using Infragistics.Win.UltraWinTabControl;

namespace FWBS.OMS.UI.Windows
{
    public abstract class TreeNavigationActions : IMainParent
    {
        #region fields & variables

        UltraTabControl tabcontrol;

        public string ParentKey { get; set; }
        public string Title { get; set; }
        public SortedList OpenWindows { get; private set; }
                    
        #endregion

        #region Constructors

        public TreeNavigationActions()
        {
            OpenWindows = new SortedList();
        }

        #endregion

        #region UltraTabControl Events

        private void SetupTabControlEvents()
        {
            tabcontrol.MouseDown -= new MouseEventHandler(tabcontrol_MouseDown);
            tabcontrol.MouseDown += new MouseEventHandler(tabcontrol_MouseDown);
            tabcontrol.TabClosing -= new TabClosingEventHandler(tabcontrol_TabClosing);
            tabcontrol.TabClosing += new TabClosingEventHandler(tabcontrol_TabClosing);
            tabcontrol.TabClosed -= new TabClosedEventHandler(tabcontrol_TabClosed);
            tabcontrol.TabClosed += new TabClosedEventHandler(tabcontrol_TabClosed);
        }

        private void tabcontrol_MouseDown(object sender, MouseEventArgs e)
        {
        
        }

        private void tabcontrol_TabClosing(object sender, TabClosingEventArgs e)
        {
            var tabControl = e.Tab.TabPage.Controls[0].Controls[0] as ITabControl;
            if (tabControl != null)
            {
                if (tabControl.CheckDialogResult() == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (e.Tab.TabPage.Controls.Count > 0)
            {
                if (e.Tab.TabPage.Controls[0].Controls.Count > 0)
                    e.Tab.TabPage.Controls[0].Controls[0].Dispose();
            }
            else
            {
                e.Tab.Dispose();
            }
        }

        private void tabcontrol_TabClosed(object sender, TabClosedEventArgs e)
        {
            e.Tab.Dispose();
        }

        public event EventHandler TabAdded;

        private void OnTabAdded(EventArgs e)
        {
            if (TabAdded != null)
                TabAdded(this, EventArgs.Empty);
        }

        #endregion

        #region Function Display Methods

        public void DisplaySelectedFunction(TreeNodeTagData tag)
        {
            Panel panel = new Panel();

            string obj = "";
            string cmd = tag.objCode;
            if (cmd.IndexOf("|") > -1)
            {
                obj = cmd.Substring(cmd.IndexOf("|") + 1);
                cmd = cmd.Substring(0, cmd.IndexOf("|"));
            }

            var _type = this.ConstructAdminElement(obj, panel, cmd);

            if (_type == null)
            {
                var result = false;
                this.MacroCommands(cmd, obj, out result);
                if (result)
                {
                    if(cmd == "MENUSCRIPT")
                        OnTabAdded(EventArgs.Empty);
                    return;
                }

                Type edit = Session.CurrentSession.TypeManager.TryLoad(cmd);
                if (edit != null)
                {
                    ucEditBase2 ctrl = (ucEditBase2)edit.InvokeMember(String.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                    DetermineDisplayMethod(ctrl, panel, tag.objText, tag.objCode);
                    return;
                }

                DisplaySearchList(cmd, ReplaceSystemVariables(tag.objText));
                return;
            }

            object ObjectTest = _type as NoDisplayControl;
            if (ObjectTest != null)
            {
                OnTabAdded(EventArgs.Empty);
                return;
            }
            
            DetermineDisplayMethod(_type, panel, ReplaceSystemVariables(tag.objText), tag.objCode);
        }

        public void DetermineDisplayMethod(Control _type, Panel panel, string tabtitle, string objectcode)
        {
            var ucEditBase2 = _type as ucEditBase2;
            if (ucEditBase2 != null)
                DisplayTypeAsUcEditBase2(ucEditBase2, panel, tabtitle);

            var ucEditBase = _type as ucEditBase;
            if (ucEditBase != null)
                DisplayTypeAsUcEditBase(ucEditBase, panel, tabtitle, objectcode);

            if (ucEditBase == null && ucEditBase2 == null)
            {
                panel.Controls.Add(_type, true);
                AddNewTabToTabControl(panel, tabtitle);
            }
        }

        private void DisplayTypeAsUcEditBase(ucEditBase _type, Panel panel, string tabtitle, string objectcode)
        {
            panel.Controls.Add(_type, true);
            _type.Dock = DockStyle.Fill;
            _type.tpList.BringToFront();
            _type.HostingTab = AddNewTabToTabControl(panel, tabtitle);
            _type.OriginalTabText = tabtitle;
            if(objectcode.Contains("CL|"))
                _type.DirectCodeLookupAccess = true;
        }

        private void DisplayTypeAsUcEditBase2(ucEditBase2 _type, Panel panel, string tabtitle)
        {
            ucScreen screen = _type as ucScreen;
            if (screen != null)
            {
                DisplayScreen(screen, panel, tabtitle);
            }
            else
            {
                panel.Controls.Add(_type, true);
                _type.Dock = DockStyle.Fill;
                _type.tpList.BringToFront();
                _type.Initialise(this, panel, new FWBS.Common.KeyValueCollection());
                _type.HostingTab = AddNewTabToTabControl(panel, tabtitle);
                _type.OriginalTabText = _type.HostingTab.Text;
            }
        }

        private void DisplayScreen(ucScreen screen, Panel panel, string tabtitle)
        {
            screen.HostingTab = AddNewTabToTabControl(panel, tabtitle);
            screen.OriginalTabText = tabtitle; 
            panel.Controls.Add(screen, true);
            screen.Dock = DockStyle.Fill;
        }

        private void DisplaySearchList(string objCode, string objText)
        {
            if (objCode != "AKC")
            {
                Panel panel = new Panel();
                ucSearchControl search = new ucSearchControl();
                panel.Controls.Add(search, true);
                search.SetSearchList(objCode, null, null);
                search.ShowPanelButtons();
                search.Dock = DockStyle.Fill;
                search.BringToFront();
                if (search.SearchList.Style == SearchListStyle.List || search.SearchList.Style == SearchListStyle.Filter)
                    search.Search();
                AddNewTabToTabControl(panel, objText);
            }
        }

        private UltraTab AddNewTabToTabControl(Panel panel, string tabtitle)
        {
            UltraTab tab = new UltraTab();
            UltraTabsCollection tabs = tabcontrol.Tabs;
            RemoveWelcomeTab();
            tab = tabs.Add(SetNewTabKey(tabtitle), tabtitle);
            tab.TabPage.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
            tabcontrol.SelectedTab = tab;
            OnTabAdded(EventArgs.Empty);
            return tab;
        }

        private void RemoveWelcomeTab()
        {
            foreach (UltraTab tab in tabcontrol.Tabs)
            {
                if (tab.Key.StartsWith("Welcome"))
                    tabcontrol.Tabs.Remove(tab);
            }
        }

        private string SetNewTabKey(string tabtitle)
        {
            if (tabcontrol.Tabs.Count == 0)
                return "1";
            else
                return tabtitle + " - " + System.DateTime.Now.Ticks.ToString();
        }

        private string ReplaceSystemVariables(string originalTabTitle)
        {
            originalTabTitle = Session.CurrentSession.Terminology.Parse(originalTabTitle, true);
            originalTabTitle = originalTabTitle.Replace("&", "&&");
            return originalTabTitle;
        }

        #endregion

        #region Dirty Tab Checks

        private bool CheckDisplayObjectIsDirty(UltraTab tab)
        {
            if (tab.TabPage.Controls[0].Controls.Count > 0)
            {
                IOBjectDirty displayobject = tab.TabPage.Controls[0].Controls[0] as IOBjectDirty;
                if (displayobject == null)
                    return false;
                else
                    return displayobject.IsObjectDirty();
            }
            return false;
        }

        #endregion

        public virtual object Action(string code, string command)
        {
            return null;
        }

        public virtual Control ConstructAdminElement(string filter, Control parent, string ecmd)
        {
            return null;
        }

        public virtual Control MacroCommands(string ecmd, string filter, out bool result)
        {
            result = false;
            return null;
        }

        internal void Setup(UltraTabControl ultraTabControl)
        {
            tabcontrol = ultraTabControl;
            SetupTabControlEvents();
        }

        public virtual void ApplicationLinkClicked(ucNavCmdButtons sender)
        {
            throw new NotImplementedException();
        }

        public virtual void SystemUpdateClick(ucDockMainView ucHome2, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void BuildApplicationPanels(ucNavCommands ucNavApplications)
        {
            
        }

        protected void OnPackageInstalled()
        {
            if (PackageInstalled != null)
                PackageInstalled(this, EventArgs.Empty);
        }

        public event EventHandler PackageInstalled;

        public virtual bool IsApplicationsPanelVisible
        {
            get
            {
                return true;
            }
        }

        public virtual bool IsSystemUpdatePanelVisible 
        {
            get
            {
                return true;
            }
        }
    }
}