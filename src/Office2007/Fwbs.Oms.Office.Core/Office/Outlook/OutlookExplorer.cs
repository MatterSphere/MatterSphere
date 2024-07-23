using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using MSOffice = Microsoft.Office.Core;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed partial class OutlookExplorer :
        OutlookObject
        , MSOutlook.Explorer
        , IWin32Window
    {
        #region Fields

        private MSOutlook.Explorer explorer;
        private Fwbs.WinFinder.Window win;
        private OutlookExplorers parent;
        private Hooks.KeyboardHook kbhook;
        private OutlookFolder _folder = null;
        private OfficeCommandBars commandbars;
        private bool shown;
        private Timer timer;

        #endregion

        #region Events

        public event CancelEventHandler Closing;

        public event EventHandler Shown;


        #endregion

        #region Constructors

        public OutlookExplorer(OutlookExplorers parent, MSOutlook.Explorer explorer)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (explorer == null)
                throw new ArgumentNullException("explorer");

            this.parent = parent;
            this.explorer = explorer;

            this.Init(explorer);

            if (Application.CurrentExplorer == null)
                Application.CurrentExplorer = this;
        }

        #endregion

        #region Overrides

        public override OutlookApplication Application
        {
            get
            {
                return parent.Application;
            }
        }

        protected override void Init(object obj)
        {

            win = OutlookUtils.FindWindow(explorer, true);
            kbhook = new Hooks.KeyboardHook();
            timer = new Timer();

            InstallKeyHooks();

            base.Init(obj);

        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (win != null)
                    {
                        win.Dispose();
                        win = null;
                    }

                    if (kbhook != null)
                    {
                        kbhook.UnInstall();
                        kbhook.Dispose();
                        kbhook = null;
                    }

                    if (timer != null)
                    {
                        timer.Dispose();
                        timer = null;
                    }

                    if (commandbars != null)
                    {
                        commandbars.Dispose();
                        commandbars = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            kbhook.KeyDown -= new Hooks.KeyboardEventHandler(kbhook_KeyDown);

            var ev = explorer as MSOutlook.ExplorerEvents_Event;

            if (explorer != null)
            {
                try { explorer.SelectionChange -= explorer_SelectionChange; }
                catch { }
                try { explorer.Deactivate -= explorer_Deactivate; }
                catch { };
                try { explorer.BeforeFolderSwitch -= explorer_BeforeFolderSwitch; }
                catch { }
                try { explorer.BeforeItemPaste -= explorer_BeforeItemPaste; }
                catch { }
                try { explorer.BeforeItemCopy -= explorer_BeforeItemCopy; }
                catch { }
                try { explorer.BeforeItemCut -= explorer_BeforeItemCut; }
                catch { }
            }
            if (ev != null)
            {
                try { ev.Activate -= explorer_Activate; }
                catch { }
                try { ev.Close -= explorer_Close; }
                catch { };
            }

            UnInstallKnownButtons();

            Fwbs.WinFinder.LocalWindow localwin = win as Fwbs.WinFinder.LocalWindow;
            if (localwin != null)
            {
                localwin.Closing -= win_Closing;
                localwin.Message -= win_Message;
            }

        }

        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();


            var ev = explorer as MSOutlook.ExplorerEvents_Event;

            if (explorer != null)
            {
                explorer.SelectionChange += explorer_SelectionChange;
                explorer.BeforeFolderSwitch += explorer_BeforeFolderSwitch;
                explorer.BeforeItemPaste += explorer_BeforeItemPaste;
                explorer.BeforeItemCut += explorer_BeforeItemCut;
                explorer.BeforeItemCopy += explorer_BeforeItemCopy;
                explorer.Deactivate += explorer_Deactivate;
            }
            if (ev != null)
            {
                ev.Activate += explorer_Activate;
                ev.Close += explorer_Close;
            }


            Fwbs.WinFinder.LocalWindow localwin = win as Fwbs.WinFinder.LocalWindow;
            if (localwin != null)
            {
                localwin.Closing += win_Closing;
                localwin.Message += win_Message;
            }

            kbhook.KeyDown += new Hooks.KeyboardEventHandler(kbhook_KeyDown);

        }




        #endregion

        #region Captured Events

        private bool iscopying;

        private void explorer_BeforeItemCopy(ref bool Cancel)
        {
            iscopying = true;
        }

        private void explorer_BeforeItemCut(ref bool Cancel)
        {
            iscopying = false;
        }        

        private void explorer_BeforeItemPaste(ref object ClipboardContent, MSOutlook.MAPIFolder Target, ref bool Cancel)
        {
            //Added GM 31 Jan 2014 - if the folder being dragged into does not have defaultitemtype of olMailItem, then return and allow Outlook to behave as normal
            if (Target.DefaultItemType != MSOutlook.OlItemType.olMailItem)
                return;

            try
            {

                InstallKeyHooks();

                if (!Application.Settings.Events.Paste.Enabled)
                    return;

                if (!Application.Settings.IsConnected())
                    return;

                var target = Application.GetFolder(Target);

                bool isdeletedfolder = target.EntryID == Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderDeletedItems).EntryID;

                if (IgnoreFolder(target) && !isdeletedfolder)
                    return;


                OnBeforeItemPaste(ref ClipboardContent, target, ref Cancel);

                var sel = ClipboardContent as MSOutlook.Selection;

                if (sel == null)
                    return;


                using (var selection = new OutlookSelection(Application, sel))
                {

                    var source = GetSourceFolder(selection);

                    if (source != null && source.EntryID == target.EntryID)
                        return;

                    if (Application.Settings.Events.Paste.Delayed)
                    {

                        Cancel = true;

                        OnDelayedAction(new Action(() =>
                            {
                                using (Application.BeginProcess())
                                {
                                    BeforeManipulateItemsEventArgs args = null;

                                    if (isdeletedfolder)
                                    {
                                        var e = new BeforeDeleteItemsEventArgs(this, selection, true, EventSource.Event, false);
                                        Application.OnBeforeDeleteItems(e);
                                        args = e;
                                    }
                                    else
                                    {
                                        var e = new BeforeMoveItemsEventArgs(this, selection, true, EventSource.Event, source, target, iscopying);
                                        Application.OnBeforeMoveItems(e);
                                        args = e;
                                    }

                                    if (args.OnAction != null)
                                        args.OnAction();

                                    if (!args.Handled)
                                    {
                                        foreach (var item in new DetachableItems(Application, selection))
                                        {
                                            if (item.IsDeleted)
                                                continue;
                                            item.Move(Target);
                                        }
                                    }
                                }

                            }));
                    }
                    else
                    {
                        using (Application.BeginProcess())
                        {
                            var args = new BeforeMoveItemsEventArgs(this, selection, true, EventSource.Event, source, target, iscopying);
                            args.Cancel = Cancel;
                            Application.OnBeforeMoveItems(args);

                            if (args.OnAction != null)
                                args.OnAction();

                            if (args.Handled)
                            {
                                Cancel = args.Cancel;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cancel = true;
                Trace.WriteLine(ex, "Outlook.Explorer.BeforeItemPaste");
                throw;
            }
        }

        private OutlookFolder GetSourceFolder(MSOutlook.Selection selection)
        {
            if (selection.Count > 0)
            {
                var item = (OutlookItem)selection[1];
                return item.Folder;
            }
            return _folder;
        }

        private void explorer_BeforeFolderSwitch(object NewFolder, ref bool Cancel)
        {
            try
            {
                InstallKeyHooks();
                var of = Application.GetFolder(NewFolder as MSOutlook.MAPIFolder);
                DisableMatterViewOnFolder(of);
                _folder = of;
                OnBeforeFolderSwitch(_folder, ref Cancel);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer.BeforeFolderSwitch");
                throw;
            }
        }

        private static void DisableMatterViewOnFolder(OutlookFolder of)
        {
            of.WebViewOn = false;
            of.WebViewURL = null;
        }

        private void explorer_SelectionChange()
        {
            //Only managage the loaded items when the explorer is actually shown.
            //This is because when Word constructs and controls outlook and creates a message, before the items is shown the
            //explorer selection event kicks in and unloads the COM object causing an RCW error.
            if (!Visible)
                return;

            if (!Application.Settings.IsConnected())
                return;

            try
            {
                var selecteditems = Application.GetCurrentSelectedItemsFromExplorers();

                int count = selecteditems.Count();

                switch (count)
                {
                    case 0:
                        break;
                    default:
                        {
                            foreach (var item in selecteditems)
                            {
                                item.Attach();
                                item.AttachEvents();
                            }

                            parent.Application.LoadedItems.SetItems(selecteditems);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer.SelectionChange");
                throw;
            }
        }

        private void explorer_Close()
        {
            try
            {
                kbhook.UnInstall();

                if (Application.CurrentExplorer == this)
                    Application.CurrentExplorer = null;

                parent.RemoveExplorer(explorer);

                //This is the only way to dispose and release objects when executed within an no addin instance (ie.e Word automating Outlook).
                if (parent.Count == 0 && !Application.IsAddinInstance)
                {
                    Application.Dispose();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer.Close");
                throw;
            }
        }

        private void explorer_Activate()
        {
            if (explorer.WindowState == MSOutlook.OlWindowState.olMinimized)
                return;

            InstallKnownButtons();

            if (!shown)
            {
                shown = true;                
                var ev = Shown;
                if (ev != null)
                    ev(this, EventArgs.Empty);

            }

            Application.CurrentExplorer = this;

            try
            {
                InstallKeyHooks();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer.Activate");
                throw;
            }
        }

        private void explorer_Deactivate()
        {

            try
            {
                kbhook.UnInstall();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer.Deactivate");
                throw;
            }
        }


        private void win_Closing(object sender, Fwbs.WinFinder.MessageEventArgs e)
        {
            try
            {
                CancelEventArgs cancel = new CancelEventArgs();

                var ev = Closing;
                if (ev != null)
                {

                    ev(this, cancel);
                    if (cancel.Cancel)
                    {
                        e.Handled = true;
                        return;
                    }

                }

                if (parent.Count == 1)
                {
                    if (parent.Application.OnClosing(cancel))
                    {
                        if (cancel.Cancel)
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Explorer..Window.Closing");
                throw;
            }
        }


        private void win_Message(object sender, Fwbs.WinFinder.MessageEventArgs e)
        {
            if (e.Message.Msg == (int)WinFinder.WindowMessage.CopyData)
            {
                if (e.Message.WParam.ToInt32() == OutlookApplication.WM_REFRESH_PROPS)
                {
                    var cps = (COPYDATASTRUCT)Marshal.PtrToStructure(e.Message.LParam, typeof(COPYDATASTRUCT));
                    var ids = cps.lpData.Split('+');

                    var item = Application.LoadedItems.GetItemFromEntryId(ids[0], null);
                    if (item == null)
                        item = Application.GetItemFromId(ids[0], ids[1]);

                    if (item != null)
                        item.SynchroniseProperties();

                }
            }
        }

        private void kbhook_KeyDown(object sender, Hooks.KeyboardEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Key Hook - Key: {0} - Time: {1} - Flags: {2}", e.Key, e.Time, e.Flags));

            IOutlookItems items;

            var action = KBHookSave(e, out items);

            if (action != null)
            {
                KBHookExecute(e, action, items);
                return;
            }

            action = KBHookOpen(e, out items);

            if (action != null)
            {
                KBHookExecute(e, action, items);
                return;
            }

            action = KBHookPermanentDelete(e, out items);

            if (action != null)
            {
                KBHookExecute(e, action, items);
                return;
            }

            action = KBHookDelete(e, out items);

            if (action != null)
            {
                KBHookExecute(e, action, items);
                return;
            }

            action = KBHookPrint(e, out items);

            if (action != null)
            {
                KBHookExecute(e, action, items);
                return;
            }


        }

        private void KBHookExecute(Hooks.KeyboardEventArgs e, Action action, IOutlookItems items)
        {
            try
            {

                try
                {

                    if (Application.Settings.KeyHooks.Delay <= 0)
                    {
                        action();
                        return;
                    }

                    if (e != null)
                        e.Handled = true;


                    OnDelayedAction(action);

                }

                catch (Exception ex)
                {
                    if (e != null)
                        e.Handled = true;
                    MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

            }
            finally
            {
                if (items != null)
                    items.Dispose();
            }
        }

        private void OnDelayedAction(Action action)
        {
            if (timer.Tag != null)
            {
                return;
            }

            timer.Interval = Application.Settings.KeyHooks.Delay;
            EventHandler eh = null;

            eh = (s2, e2) =>
            {
                try
                {
                    timer.Tick -= eh;
                    timer.Enabled = false;

                    action();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    timer.Tag = null;
                }
            };

            timer.Tick += eh;

            timer.Start();
            timer.Tag = String.Empty;
        }

        private Action KBHookPrint(Hooks.KeyboardEventArgs e, out IOutlookItems items)
        {
            items = null;

            if (!Application.Settings.KeyHooks.PrintKey.Enabled)
                return null;

            if (!(e.Key == Keys.P && kbhook.IsPressed(Keys.ControlKey)))
                return null;

            if (!Application.Settings.IsConnected())
                return null;

            try
            {
                if (Application.Settings.KeyHooks.UseCommands)
                {
                    e.Handled = true;
                    return new Action(() =>
                    {
                        ExecuteButton(_printbtn);
                    });
                }
                else
                {
                    OutlookExplorer exp;

                    items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp);

                    if (exp != this)
                    {
                        e.Handled = true;
                        return null;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return null;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforePrintItemsEventArgs(this, items, true, EventSource.KeyHook);

                        Application.OnBeforePrintItems(args);

                        if (args.Handled)
                        {
                            e.Handled = args.Cancel;
                            return args.OnAction;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Handled = true;
                MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private Action KBHookDelete(Hooks.KeyboardEventArgs e, out IOutlookItems items)
        {
            items = null;

            if (!Application.Settings.KeyHooks.DeleteKey.Enabled)
                return null;

            if (!(e.Key == Keys.Delete))
                return null;

            if (!(kbhook.IsPressed(Keys.ControlKey) == false && kbhook.IsPressed(Keys.Menu) == false))
                return null;

            if (e.Flags != Hooks.ExtendedKeyFlag.LLKHF_EXTENDED)
                return null;

            if (!IsListFocused())
                return null;

            if (!Application.Settings.IsConnected())
                return null;

            try
            {
                if (Application.Settings.KeyHooks.UseCommands)
                {
                    e.Handled = true;
                    return new Action(() =>
                    {
                        ExecuteButton(_delbtn);
                    });
                }
                else
                {
                    OutlookExplorer exp;

                    items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp);

                    if (exp != this)
                    {
                        e.Handled = true;
                        return null;
                    }


                    if (items == null || IgnoreFolder(CurrentFolder))
                        return null;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeDeleteItemsEventArgs(this, items, true, EventSource.KeyHook, false);
                        Application.OnBeforeDeleteItems(args);

                        if (args.Handled)
                        {
                            e.Handled = args.Cancel;
                            return args.OnAction;
                        }

                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                e.Handled = true;
                MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        private Action KBHookPermanentDelete(Hooks.KeyboardEventArgs e, out IOutlookItems items)
        {
            items = null;

            if (!Application.Settings.KeyHooks.DeleteKey.Enabled)
                return null;

            if (!(e.Key == Keys.D && kbhook.IsPressed(Keys.ControlKey)))
                return null;

            if (!Application.Settings.IsConnected())
                return null;

            try
            {
                if (Application.Settings.KeyHooks.UseCommands)
                {
                    e.Handled = true;
                    return new Action(() =>
                    {
                        ExecuteButton(_permdelbtn);
                    });
                }
                else
                {
                    OutlookExplorer exp;

                    items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp);

                    if (exp != this)
                    {
                        e.Handled = true;
                        return null;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return null;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeDeleteItemsEventArgs(this, items, true, EventSource.KeyHook, true);
                        Application.OnBeforeDeleteItems(args);

                        if (args.Handled)
                        {
                            e.Handled = args.Cancel;
                            return args.OnAction;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Handled = true;
                MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private Action KBHookOpen(Hooks.KeyboardEventArgs e, out IOutlookItems items)
        {
            items = null;

            if (!Application.Settings.KeyHooks.OpenKey.Enabled)
                return null;

            if (!(e.Key == Keys.O && kbhook.IsPressed(Keys.ControlKey)))
                return null;

            if (!IsListFocused())
                return null;

            if (!Application.Settings.IsConnected())
                return null;

            try
            {
                if (Application.Settings.KeyHooks.UseCommands)
                {
                    e.Handled = true;
                    return new Action(() =>
                    {
                        ExecuteButton(_openbtn);
                    });
                }
                else
                {
                    OutlookExplorer exp;

                    items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp);

                    if (exp != this)
                    {
                        e.Handled = true;
                        return null;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return null;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeOpenItemsEventArgs(this, items, true, EventSource.KeyHook);
                        Application.OnBeforeOpenItems(args);

                        if (args.Handled)
                        {
                            e.Handled = args.Cancel;
                            return args.OnAction;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Handled = true;
                MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private Action KBHookSave(Hooks.KeyboardEventArgs e, out IOutlookItems items)
        {
            items = null;

            if (!Application.Settings.KeyHooks.SaveKey.Enabled)
                return null;

            if (!(e.Key == Keys.S && kbhook.IsPressed(Keys.ControlKey)))
                return null;

            if (!IsListFocused())
                return null;

            if (!Application.Settings.IsConnected())
                return null;

            try
            {
                if (Application.Settings.KeyHooks.UseCommands)
                {
                    e.Handled = true;
                    return new Action(() =>
                    {
                        ExecuteButton(_savebtn);
                    });
                }
                else
                {
                    OutlookExplorer exp;

                    items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp);

                    if (exp != this)
                    {
                        e.Handled = true;
                        return null;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return null;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeSaveItemsEventArgs(this, items, true, EventSource.KeyHook);

                        Application.OnBeforeSaveItems(args);

                        if (args.Handled)
                        {
                            e.Handled = args.Cancel;
                            return args.OnAction;
                        }

                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Handled = true;
                MessageBox.Show(Application.ActiveWindow, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }



        #endregion

        #region Buttons CommandBar Overrides



        private MSOffice.CommandBarButton _delbtn;
        private MSOffice.CommandBarButton _movetobtn;
        private MSOffice.CommandBarButton _copytobtn;
        private MSOffice.CommandBarButton _exitbtn;

        private MSOffice.CommandBarButton _printbtn;
        private MSOffice.CommandBarButton _savebtn;
        private MSOffice.CommandBarButton _openbtn;
        private MSOffice.CommandBarButton _permdelbtn;
        private MSOffice.CommandBar KeyHookBar = null;
        private List<MSOffice.CommandBarButton> executingbuttons = new List<MSOffice.CommandBarButton>();

        private void ExecuteButton(MSOffice.CommandBarButton button)
        {

            if (executingbuttons.Contains(button))
                return;

            try
            {
                executingbuttons.Add(button);

                button.Execute();
            }
            catch (NullReferenceException nullex)
            {
                if (button == null)
                    throw new ApplicationException(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("CMDBTNNTRGSTR", "ExecuteButton: CommandBarButton has not been registered. %1% has not been able to capture your key press.", "", FWBS.OMS.Branding.APPLICATION_NAME).Text, nullex);

            }
            catch (COMException comex)
            {
                if (comex.ErrorCode == HResults.E_FAIL)
                    return;

                throw;
            }
            finally
            {
                executingbuttons.Remove(button);
            }
        }

        private void InstallKnownButtons()
        {
            if (Application.IsAddinInstance && Application.MajorVersion < 14)
            {
                RegisterButton(478, ref _delbtn, ButtonClickDelete);
                RegisterButton(1679, ref _movetobtn, ButtonClickMove);
                RegisterButton(1676, ref _copytobtn, ButtonClickCopy);                

                if (Application.Settings.KeyHooks.UseCommands)
                {
                    RegisterKeyHookButton("Print", ref _printbtn, ButtonClickPrint);
                    RegisterKeyHookButton("Save", ref _savebtn, ButtonClickSave);
                    RegisterKeyHookButton("Open", ref _openbtn, ButtonClickOpen);
                    RegisterKeyHookButton("Delete", ref _permdelbtn, ButtonClickPermanentDelete);
                }
            }
        }

        private void UnInstallKnownButtons()
        {
            if (Application.IsAddinInstance && Application.MajorVersion < 14)
            {
                UnRegisterButton(478, ref _delbtn, ButtonClickDelete);
                UnRegisterButton(1679, ref _movetobtn, ButtonClickMove);
                UnRegisterButton(1676, ref _copytobtn, ButtonClickCopy);
                UnRegisterButton(1891, ref _exitbtn, ButtonClickExit);
            }
        }

        private void RegisterKeyHookButton(string name, ref MSOffice.CommandBarButton btn, MSOffice._CommandBarButtonEvents_ClickEventHandler del)
        {
            if (btn != null)
                return;

            Trace.WriteLine(string.Format("RegisterKeyHookButton: {0}", name));

            if (KeyHookBar == null)
            {
                KeyHookBar = CommandBars.Add("Outlook - KeyHooks", Type.Missing, Type.Missing, true);
                KeyHookBar.Visible = false;
            }

            btn = (MSOffice.CommandBarButton)KeyHookBar.Controls.Add(MSOffice.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, true);
            btn.Caption = name;
            btn.Click += del;

            Trace.WriteLine(string.Format("----->Registered Button : {0}", name));
        }

        private void RegisterButton(int id, ref MSOffice.CommandBarButton btn, MSOffice._CommandBarButtonEvents_ClickEventHandler del)
        {
            if (btn != null)
                return;

            Trace.WriteLine(string.Format("RegisterButton: {0}", id));

            var ctrls = CommandBars.FindControls(MSOffice.MsoControlType.msoControlButton, id, Type.Missing, Type.Missing);
            if (ctrls != null)
            {
                if (ctrls.Count > 0 && btn == null)
                {
                    btn = (MSOffice.CommandBarButton)ctrls[1];
                    btn.Click += del;
                    Trace.WriteLine(string.Format("----->Registered Button: {0}", id));
                }
            }
        }

        private void UnRegisterButton(int id, ref MSOffice.CommandBarButton btn, MSOffice._CommandBarButtonEvents_ClickEventHandler del)
        {
            var ctrls = CommandBars.FindControls(MSOffice.MsoControlType.msoControlButton, id, Type.Missing, Type.Missing);
            if (ctrls != null)
            {
                if (ctrls.Count > 0 && btn == null)
                {
                    btn = (MSOffice.CommandBarButton)ctrls[1];

                    try
                    {
                        btn.Click -= del;
                    }
                    catch { }
                }
            }
        }

        private void ButtonClickPermanentDelete(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }


                    if (items == null || IgnoreFolder(CurrentFolder))
                        return;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeDeleteItemsEventArgs(this, items, true, EventSource.Button, true);
                        Application.OnBeforeDeleteItems(args);

                        if (args.OnAction != null)
                            args.OnAction();

                        if (args.Handled)
                        {
                            CancelDefault = args.Cancel;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickOpen(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeOpenItemsEventArgs(this, items, true, EventSource.Button);
                        Application.OnBeforeOpenItems(args);

                        if (args.OnAction != null)
                            args.OnAction();

                        if (args.Handled)
                        {
                            CancelDefault = args.Cancel;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickPrint(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforePrintItemsEventArgs(this, items, true, EventSource.Button);

                        Application.OnBeforePrintItems(args);

                        if (args.OnAction != null)
                            args.OnAction();

                        if (args.Handled)
                        {
                            CancelDefault = args.Cancel;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickSave(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return;

                    using (Application.BeginProcess())
                    {
                        var args = new BeforeSaveItemsEventArgs(this, items, true, EventSource.Button);

                        Application.OnBeforeSaveItems(args);

                        if (args.OnAction != null)
                            args.OnAction();

                        if (args.Handled)
                        {
                            CancelDefault = args.Cancel;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickExit(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                var e = new CancelEventArgs(CancelDefault);
                if (Application.OnClosing(e))
                    CancelDefault = e.Cancel;
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickMove(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (!Application.Settings.IsConnected())
                    return;

                var target = (OutlookFolder)Session.PickFolder();
                if (target == null)
                {
                    CancelDefault = true;
                    return;
                }

                bool isdeletedfolder = target.EntryID == Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderDeletedItems).EntryID;


                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null)
                        return;

                    using (Application.BeginProcess())
                    {
                        if (IgnoreFolder(target) && !isdeletedfolder)
                        {
                            CancelDefault = true;
                            foreach (var item in items)
                            {
                                if (item.IsDeleted)
                                    continue;
                                item.Move(target);
                            }

                            return;
                        }

                        BeforeManipulateItemsEventArgs args = null;
                        if (isdeletedfolder)
                        {
                            var e = new BeforeDeleteItemsEventArgs(this, items, true, EventSource.Button, false);
                            e.Cancel = CancelDefault;
                            Application.OnBeforeDeleteItems(e);
                            args = e;
                        }
                        else
                        {
                            var e = new BeforeMoveItemsEventArgs(this, items, true, EventSource.Button, _folder, target, false);
                            e.Cancel = CancelDefault;
                            Application.OnBeforeMoveItems(e);
                            args = e;
                        }

                        CancelDefault = true;

                        if (args.OnAction != null)
                            args.OnAction();

                        //Cancel must be set to true otherwise the Pick folder screen pops up again.
                        if (args.Handled == false)
                        {
                            foreach (var item in new DetachableItems(Application, items))
                            {
                                if (item.IsDeleted)
                                    continue;
                                item.Move(target);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ButtonClickCopy(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (!Application.Settings.IsConnected())
                    return;

                var target = (OutlookFolder)Session.PickFolder();
                if (target == null)
                {
                    CancelDefault = true;
                    return;
                }

                bool isdeletedfolder = target.EntryID == Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderDeletedItems).EntryID;

                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null)
                        return;

                    using (Application.BeginProcess())
                    {
                        if (IgnoreFolder(target) & !isdeletedfolder)
                        {
                            CancelDefault = true;
                            foreach (var item in items)
                            {
                                if (item.IsDeleted)
                                    continue;
                                var copyitem = (OutlookItem)item.Copy();
                                copyitem.Move(target);
                            }

                            return;
                        }

                        BeforeManipulateItemsEventArgs args = null;

                        if (isdeletedfolder)
                        {
                            var e = new BeforeDeleteItemsEventArgs(this, items, true, EventSource.Button, true);
                            e.Cancel = CancelDefault;
                            Application.OnBeforeDeleteItems(e);
                            args = e;
                        }
                        else
                        {
                            var e = new BeforeMoveItemsEventArgs(this, items, true, EventSource.Button, _folder, target, true);
                            e.Cancel = CancelDefault;
                            Application.OnBeforeMoveItems(e);
                            args = e;
                        }

                        CancelDefault = true;

                        if (args.OnAction != null)
                            args.OnAction();

                        //Cancel must be set to true otherwise the Pick folder screen pops up again.
                        if (args.Handled == false)
                        {
                            foreach (var item in new DetachableItems(Application, items))
                            {
                                if (item.IsDeleted)
                                    continue;

                                var copyitem = (OutlookItem)item.Copy();

                                copyitem.Attach();

                                try
                                {
                                    copyitem.Move(target);
                                }
                                finally
                                {
                                    copyitem.Detach();
                                    item.Detach();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonClickDelete(MSOffice.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            try
            {
                if (!Application.Settings.IsConnected())
                    return;

                OutlookExplorer exp;

                using (var items = GatherCurrentItems(Application.ActiveExplorer(), true, out exp))
                {
                    if (exp != this)
                    {
                        return;
                    }

                    if (items == null || IgnoreFolder(CurrentFolder))
                        return;

                    var e = new BeforeDeleteItemsEventArgs(this, items, false, EventSource.Button, false);
                    e.Cancel = CancelDefault;
                    Application.OnBeforeDeleteItems(e);

                    if (e.OnAction != null)
                        e.OnAction();

                    if (e.Handled)
                    {
                        CancelDefault = e.Cancel;
                        return;
                    }

                    foreach (var i in items)
                    {
                        i.DeleteRequested = true;
                    }

                }
            }
            catch (Exception ex)
            {
                CancelDefault = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(this, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Methods

        private void CheckCountWarning(IOutlookItems items)
        {
            if (Application.Settings.Memory.MultipleItemWarningEnabled && Application.Settings.IsConnected())
            {
                if (items.Count > Application.Settings.Memory.MultipleItemWarningSize)
                    MessageBox.Show(Application.Settings.Memory.MultipleItemWarningMessage, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private IOutlookItems GatherCurrentItems(object obj, bool useActiveExplorer, out OutlookExplorer exp)
        {
            exp = null;

            if (!IsActiveWindowAllowed())
                return null;

            exp = GetSelectedExplorer(obj, useActiveExplorer);

            if (exp != null)
            {
                var items = (IOutlookItems)exp.Selection;

                CheckCountWarning(items);

                return items;
            }

            return null;

        }

        private OutlookExplorer GetSelectedExplorer(object obj, bool useActiveExplorer)
        {
            var exp = obj as OutlookExplorer;
            if (exp != null)
                return exp;

            while (obj != null)
            {
                try
                {
                    obj = OutlookItems.GetPropertyEx<object>(obj, "Parent");

                    if (obj is OutlookExplorer)
                        return (OutlookExplorer)obj;
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode == HResults.E_FAIL)
                        break;
                }

                if (obj is MSOutlook.Inspector)
                    break;
            }

            if (exp == null)
            {
                if (useActiveExplorer)
                    return (OutlookExplorer)Application.ActiveExplorer();
            }

            return exp;
        }

        private bool IgnoreFolder(MSOutlook.MAPIFolder folder)
        {
            if (folder == null)
                return false;

            if (folder.EntryID == Application.Session.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderDeletedItems).EntryID)
                return true;

            //Added to fix issue with Hummingbird folder causing issue with firing of additional events
            //MNW - 1 June 2005
            if (folder.FolderPath.StartsWith("\\HUMMINGBIRD DM", StringComparison.InvariantCultureIgnoreCase))
                return true;

            var ret = Array.Find(Application.Settings.IgnoredFolders, (s) => Helpers.IsMatch(folder.FolderPath.ToUpperInvariant(), s));

            return ret != null;
        }

        private bool IsListFocused()
        {
            return Fwbs.WinFinder.CommonWindows.FocusedWindow.Class.Equals(Application.Settings.ListWindowClass, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool IsActiveWindowAllowed()
        {
            //DM - This exception will occur when the parent window is not an explorer or inspector
            //window. Just like the delete rule error Mike found below.
            //Check to see if the Advanced Find window. May only work in english of course.
            Fwbs.WinFinder.Window win = Fwbs.WinFinder.CommonWindows.ActiveWindow;
            if (win == null)
                return false;

            return Array.Find(Application.Settings.DisallowedWindowTitles, (s) => Helpers.IsMatch(win.Text.ToUpperInvariant(), s)) == null;

        }


        internal void InstallKeyHooks()
        {
            kbhook.UnInstall();

            if (Application.IsAddinInstance && Application.Settings.KeyHooks.Enabled)
            {
                kbhook.Install();
            }
        }

        internal void UnInstallKeyHooks()
        {
            kbhook.UnInstall();
        }

        #endregion

        #region Properties

        internal MSOutlook.Explorer InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return explorer;
            }
        }

        internal WinFinder.Window Window
        {
            get
            {
                return win;
            }
        }

        public bool Visible
        {
            get
            {
                if (win != null)
                    return win.IsVisible;
                else
                    return false;
            }
            set
            {
                if (win != null)
                    win.IsVisible = value;
            }
        }

        public string Caption
        {
            get
            {
                return explorer.Caption;
            }
            set
            {
                if (win != null)
                    win.Text = value ?? String.Empty;
            }
        }

        #endregion

        #region IWin32Window Members

        public IntPtr Handle
        {
            get
            {
                if (win == null)
                    return IntPtr.Zero;
                else
                    return win.Handle;
            }
        }

        #endregion

        #region _Explorer Members

        public void Activate()
        {
            ((MSOutlook._Explorer)explorer).Activate();
            FWBS.Common.Functions.SetForegroundWindow(win.Handle);
        }

        public void Activate(bool force)
        {
            ((MSOutlook._Explorer)explorer).Activate();

            if (force)
                explorer_Activate();
        }



        MSOutlook.Application MSOutlook._Explorer.Application
        {
            get { return Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return explorer.Class; }
        }



        public void Close()
        {
            ((MSOutlook._Explorer)explorer).Close();
        }

        public MSOffice.CommandBars CommandBars
        {
            get
            {
                if (commandbars == null)
                    commandbars = new OfficeCommandBars(explorer.CommandBars);
                return commandbars;
            }
        }

        public Microsoft.Office.Interop.Outlook.MAPIFolder CurrentFolder
        {
            get
            {
                try
                {
                    return Application.GetFolder(explorer.CurrentFolder);
                }
                catch (COMException)
                {
                    //Explorer has been closed.
                    return null;
                }
            }
            set
            {
                var of = value as OutlookFolder;

                if (of == null)
                    explorer.CurrentFolder = value;
                else
                    explorer.CurrentFolder = of.InternalItem;
            }
        }

        public object CurrentView
        {
            get
            {
                return explorer.CurrentView;
            }
            set
            {
                explorer.CurrentView = value;
            }
        }

        public void DeselectFolder(MSOutlook.MAPIFolder Folder)
        {
            var of = Folder as OutlookFolder;
            if (of == null)
                explorer.DeselectFolder(Folder);
            else
                explorer.DeselectFolder(of.InternalItem);


        }

        public void Display()
        {
            explorer.Display();
        }

        public object HTMLDocument
        {
            get { return explorer.HTMLDocument; }
        }

        public int Height
        {
            get
            {
                return explorer.Height;
            }
            set
            {
                explorer.Height = value;
            }
        }

        public bool IsFolderSelected(MSOutlook.MAPIFolder Folder)
        {
            var of = Folder as OutlookFolder;
            if (of == null)
                return explorer.IsFolderSelected(Folder);
            else
                return explorer.IsFolderSelected(of.InternalItem);
        }

        public bool IsPaneVisible(MSOutlook.OlPane Pane)
        {
            return explorer.IsPaneVisible(Pane);
        }

        public int Left
        {
            get
            {
                return explorer.Left;
            }
            set
            {
                explorer.Left = value;
            }
        }


        public MSOutlook.Panes Panes
        {
            get { return explorer.Panes; }
        }

        //TODO: Check
        public object Parent
        {
            get { return explorer.Parent; }
        }

        public bool IsMailView
        {
            get
            {
                MSOutlook.OlNavigationModuleType? navModuleType = NavigationPane?.CurrentModule.NavigationModuleType;
                return (navModuleType == MSOutlook.OlNavigationModuleType.olModuleMail) ||
                    (navModuleType == MSOutlook.OlNavigationModuleType.olModuleFolderList && CurrentFolder?.DefaultItemType == MSOutlook.OlItemType.olMailItem);
            }
        }

        public void SelectFolder(MSOutlook.MAPIFolder Folder)
        {
            var of = Folder as OutlookFolder;
            if (of == null)
                explorer.SelectFolder(Folder);
            else
                explorer.SelectFolder(of.InternalItem);
        }

        public MSOutlook.Selection Selection
        {
            get
            {
                return new OutlookSelection(Application, explorer.Selection);
            }
        }


        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public void ShowPane(MSOutlook.OlPane Pane, bool Visible)
        {
            explorer.ShowPane(Pane, Visible);
        }

        public int Top
        {
            get
            {
                return explorer.Top;
            }
            set
            {
                explorer.Top = value;
            }
        }

        public object Views
        {
            get { return explorer.Views; }
        }

        public int Width
        {
            get
            {
                return explorer.Width;
            }
            set
            {
                explorer.Width = value;
            }
        }

        public MSOutlook.OlWindowState WindowState
        {
            get
            {
                return explorer.WindowState;
            }
            set
            {
                explorer.WindowState = value;
            }
        }

        #endregion

        #region ExplorerEvents_10_Event Members

        event MSOutlook.ExplorerEvents_10_ActivateEventHandler MSOutlook.ExplorerEvents_10_Event.Activate
        {
            add
            {
                ((MSOutlook.ExplorerEvents_10_Event)explorer).Activate += value;
            }
            remove
            {
                ((MSOutlook.ExplorerEvents_10_Event)explorer).Activate -= value;
            }
        }


        public event MSOutlook.ExplorerEvents_10_BeforeItemCopyEventHandler BeforeItemCopy
        {
            add
            {
                explorer.BeforeItemCopy += value;
            }
            remove
            {
                explorer.BeforeItemCopy -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_BeforeItemCutEventHandler BeforeItemCut
        {
            add
            {
                explorer.BeforeItemCut += value;
            }
            remove
            {
                explorer.BeforeItemCut -= value;
            }
        }



        public event MSOutlook.ExplorerEvents_10_BeforeMaximizeEventHandler BeforeMaximize
        {
            add
            {
                explorer.BeforeMaximize += value;
            }
            remove
            {
                explorer.BeforeMaximize -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_BeforeMinimizeEventHandler BeforeMinimize
        {
            add
            {
                explorer.BeforeMinimize += value;
            }
            remove
            {
                explorer.BeforeMinimize -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_BeforeMoveEventHandler BeforeMove
        {
            add
            {
                explorer.BeforeMove += value;
            }
            remove
            {
                explorer.BeforeMove -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_BeforeSizeEventHandler BeforeSize
        {
            add
            {
                explorer.BeforeSize += value;
            }
            remove
            {
                explorer.BeforeSize -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_BeforeViewSwitchEventHandler BeforeViewSwitch
        {
            add
            {
                explorer.BeforeViewSwitch += value;
            }
            remove
            {
                explorer.BeforeViewSwitch -= value;
            }
        }

        event MSOutlook.ExplorerEvents_10_CloseEventHandler MSOutlook.ExplorerEvents_10_Event.Close
        {
            add
            {
                ((MSOutlook.ExplorerEvents_10_Event)explorer).Close += value;
            }
            remove
            {
                ((MSOutlook.ExplorerEvents_10_Event)explorer).Close -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_DeactivateEventHandler Deactivate
        {
            add
            {
                explorer.Deactivate += value;
            }
            remove
            {
                explorer.Deactivate -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_FolderSwitchEventHandler FolderSwitch
        {
            add
            {
                explorer.FolderSwitch += value;
            }
            remove
            {
                explorer.FolderSwitch -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_SelectionChangeEventHandler SelectionChange
        {
            add
            {
                explorer.SelectionChange += value;
            }
            remove
            {
                explorer.SelectionChange -= value;
            }
        }

        public event MSOutlook.ExplorerEvents_10_ViewSwitchEventHandler ViewSwitch
        {
            add
            {
                explorer.ViewSwitch += value;
            }
            remove
            {
                explorer.ViewSwitch -= value;
            }
        }

        #region Overrides

        public event MSOutlook.ExplorerEvents_10_BeforeFolderSwitchEventHandler BeforeFolderSwitch;

        private void OnBeforeFolderSwitch(OutlookFolder folder, ref bool cancel)
        {
            Application.LoadedFolders.Refresh(folder);

            var ev = BeforeFolderSwitch;
            if (ev != null)
                ev(folder, ref cancel);
        }

        public event MSOutlook.ExplorerEvents_10_BeforeItemPasteEventHandler BeforeItemPaste;

        private void OnBeforeItemPaste(ref object clipboardConent, MSOutlook.MAPIFolder target, ref bool cancel)
        {
            var ev = BeforeItemPaste;
            if (ev != null)
                ev(ref clipboardConent, target, ref cancel);
        }

        #endregion

        #endregion



    }

    internal class TypeExtensions
    {
        public static string GetTypeName(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();

            if (IsComType(type))
            {
                var ti = GetComTypeInfo(obj, true);
                return GetComTypeName(ti);
            }

            return type.Name;
        }

        public static string[] GetTypeMembers(object obj)
        {
            var type = obj.GetType();

            if (IsComType(type))
            {
                var ti = GetComTypeInfo(obj, true);
                return GetComTypeMembers(ti);
            }

            return type.GetMembers().Select(m => m.Name).ToArray();

        }

        private static bool IsComType(Type type)
        {
            if (type.IsCOMObject && (string.CompareOrdinal(type.Name, "__ComObject") == 0))
                return true;

            return false;
        }

        [System.Security.SecuritySafeCritical]
        private static System.Runtime.InteropServices.ComTypes.ITypeInfo GetComTypeInfo(object obj, bool throwException)
        {
            try
            {
                new System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode).Demand();
            }
            catch (StackOverflowException exception)
            {
                throw exception;
            }
            catch (OutOfMemoryException exception2)
            {
                throw exception2;
            }
            catch (System.Threading.ThreadAbortException exception3)
            {
                throw exception3;
            }
            catch (Exception exception4)
            {
                if (throwException)
                {
                    throw exception4;
                }

                return null;
            }

            System.Runtime.InteropServices.ComTypes.ITypeInfo type = null;

            IDispatch dispatch = obj as IDispatch;
            if (((dispatch != null) && (dispatch.GetTypeInfo(0, 0x409, out type) >= 0)))
            {
                return type;
            }

            return null;
        }

        private static string GetComTypeName(System.Runtime.InteropServices.ComTypes.ITypeInfo type)
        {
            string doc = null;
            string helpfile = null;
            int num;
            string name = "__ComObject";

            type.GetDocumentation(-1, out name, out doc, out num, out helpfile);

            if (name[0] == '_')
            {
                name = name.Substring(1);
            }

            return name;
        }

        private static string[] GetComTypeMembers(System.Runtime.InteropServices.ComTypes.ITypeInfo type)
        {
            const int max = 1;



            int ctr = 0;

            var list = new List<string>();

            while (true)
            {
                IntPtr pfunc;

                try
                {
                    type.GetFuncDesc(ctr, out pfunc);
                }
                catch (COMException)
                {
                    break;
                }

                ctr++;

                if (pfunc == IntPtr.Zero)
                    break;

                var funcdesc = (System.Runtime.InteropServices.ComTypes.FUNCDESC)Marshal.PtrToStructure(pfunc, typeof(System.Runtime.InteropServices.ComTypes.FUNCDESC));

                string[] names = new string[max];
                int count;
                type.GetNames(funcdesc.memid, names, max, out count);

                type.ReleaseFuncDesc(pfunc);

                if (count == 0)
                    continue;

                list.Add(names[0]);

            }

            list.Sort();

            return list.Distinct().ToArray();
        }

        [ComImport, EditorBrowsable(EditorBrowsableState.Never), Guid("00020400-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IDispatch
        {
            [PreserveSig, System.Security.SecurityCritical, Obsolete("Bad signature. Fix and verify signature before use.", true)]
            int GetTypeInfoCount();
            [PreserveSig, System.Security.SecurityCritical]
            int GetTypeInfo([In] int index, [In] int lcid, [MarshalAs(UnmanagedType.Interface)] out System.Runtime.InteropServices.ComTypes.ITypeInfo pTypeInfo);
            [PreserveSig, System.Security.SecurityCritical]
            int GetIDsOfNames();
            [PreserveSig, System.Security.SecurityCritical]
            int Invoke();
        }




    }
}
