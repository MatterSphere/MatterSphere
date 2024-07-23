using System;
using System.Runtime.InteropServices;


namespace Fwbs.Office.Outlook
{
    using System.Diagnostics;
    using System.Windows.Forms;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookInspector :
        OutlookObject
        , MSOutlook.Inspector
        , IWin32Window
    { 

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr GetFocus();

        #region Fields

        private Fwbs.WinFinder.Window win;
        private MSOutlook.Inspector inspector;
        private readonly OutlookInspectors parent;
        private Redemption.SafeInspector safeitem;
        private OfficeCommandBars bars;

        #endregion

        #region Constructors

        public OutlookInspector(OutlookInspectors parent, MSOutlook.Inspector inspector)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (inspector == null)
                throw new ArgumentNullException("inspector");

            this.parent = parent;
            this.inspector = inspector;

            this.Init(inspector);
        }

        #endregion

        #region Events


        public event MSOutlook.InspectorEvents_10_CloseEventHandler Closed;


        public event MSOutlook.InspectorEvents_10_ActivateEventHandler Activated;
 

        #endregion

        #region Overrides


        public override OutlookApplication Application
        {
            get { return parent.Application; }
        }

        protected override void Init(object obj)
        {          
            win = OutlookUtils.FindWindow(inspector);
  
            base.Init(obj);
        }

        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();

     
            var ev = inspector as MSOutlook.InspectorEvents_Event;

            if (ev != null)
            {
                ev.Close += inspector_Close;
                ev.Activate += inspector_Activate;
            }
        }


        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            var ev = inspector as MSOutlook.InspectorEvents_Event;

            if (ev != null)
            {
                try{ev.Close -= inspector_Close;}
                catch  { }
                try { ev.Activate -= inspector_Activate; }
                catch  { }
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (currentitem != null)
                    {
                        var ci = currentitem;
                        currentitem.Inspector = null;
                        currentitem = null;
                        Application.LoadedItems.Refresh(ci);
                    }

                    if (safeitem != null)
                    {
                        if (Marshal.IsComObject(safeitem))
                            Marshal.ReleaseComObject(safeitem);

                        safeitem = null;
                    }

                    if (bars != null)
                    {
                        bars.Dispose();
                        bars = null;
                    }
                }

            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Captured Events

        private void inspector_Close()
        {
            try
            {
                EnsureRaiseEvent(Closed);
            }
            finally
            {

                try
                {
                    parent.RemoveInspector(inspector);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, "Outlook.Inspector.Close");
                    throw;
                }
            }
        }

        private void inspector_Activate()
        {
            var item = (OutlookItem)CurrentItem;
            
            item.SynchroniseProperties();

            var args = new BeforeItemEventArgs(item, false);
            Application.OnBeforeActivateItem(args);

            EnsureRaiseEvent(Activated);
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

        public static void ApplyActiveControlData()
        {
            var handle = GetFocus();
            var win = Fwbs.WinFinder.WindowFactory.GetWindow(handle);
            if (win != null)
            {
                win.UnFocus();
                System.Windows.Forms.Application.DoEvents();
                win.Focus();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        #endregion

        #region _Inspector Members

        public void Activate()
        {
            ((MSOutlook._Inspector)inspector).Activate();
            FWBS.Common.Functions.SetForegroundWindow(win.Handle);
        }

        public bool IsDeleted
        {
            get
            {
                try
                {
                    var caption = inspector.Caption;
                    return String.IsNullOrEmpty(caption);
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode == HResults.E_OBJECT_DELETED_OR_MOVED)
                        return true;

                    return false;
                }
                catch (AccessViolationException)
                {
                    return true;
                }
                catch (InvalidComObjectException)
                {
                    return true;
                }
            }
        }
        public string Caption
        {
            get
            {
                try
                {
                    return inspector.Caption;
                }
                catch (AccessViolationException)
                {
                    if (win != null)
                        return win.Text;

                    return String.Empty;
                }
            }
            set
            {
                if (win != null)
                    win.Text = value;
            }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return inspector.Class; }
        }

        public void Close(MSOutlook.OlInspectorClose SaveMode)
        {
            ((MSOutlook._Inspector)inspector).Close(SaveMode);
        }

        public void Display()
        {
            Display(false);
        }
        public void Display(object Modal)
        {
            inspector.Display(Modal);
        }

        public MSOutlook.OlEditorType EditorType
        {
            get { return inspector.EditorType; }
        }

        public object HTMLEditor
        {
            get { return inspector.HTMLEditor; }
        }

        public int Height
        {
            get
            {
                return inspector.Height;
            }
            set
            {
                inspector.Height = value;
            }
        }

        public void HideFormPage(string PageName)
        {
            inspector.HideFormPage(PageName);
        }

        public bool IsWordMail()
        {
            return inspector.IsWordMail();
        }

        public int Left
        {
            get
            {
                return inspector.Left;
            }
            set
            {
                inspector.Left = value;
            }
        }

        public object ModifiedFormPages
        {
            get { return inspector.ModifiedFormPages; }
        }

   

        public object Parent
        {
            get { return inspector.Parent; }
        }

   

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public void SetControlItemProperty(object Control, string PropertyName)
        {
            inspector.SetControlItemProperty(Control, PropertyName);
        }

        public void SetCurrentFormPage(string PageName)
        {
            inspector.SetCurrentFormPage(PageName);
        }

        public void ShowFormPage(string PageName)
        {
            inspector.ShowFormPage(PageName);
        }

        public int Top
        {
            get
            {
                return inspector.Top;
            }
            set
            {
                inspector.Top = value;
            }
        }

        public int Width
        {
            get
            {
                return inspector.Width;
            }
            set
            {
                inspector.Width = value;
            }
        }

        public MSOutlook.OlWindowState WindowState
        {
            get
            {
                return inspector.WindowState;
            }
            set
            {
                inspector.WindowState = value;
            }
        }

        public bool Visible
        {
            get
            {
                return win.IsVisible;
            }
            set
            {
                if (win != null)
                    win.IsVisible = value;
            }
        }

        public object WordEditor
        {
            get { return inspector.WordEditor; }
        }


        MSOutlook.Application MSOutlook._Inspector.Application
        {
            get { return Application; }
        }

        
        public Microsoft.Office.Core.CommandBars CommandBars
        {
            get 
            {
                if (bars == null)
                    bars = new OfficeCommandBars(inspector.CommandBars);
                return bars; }
        }

        private OutlookItem currentitem;
        public object CurrentItem
        {
            get 
            {
                if (currentitem == null)
                    currentitem = GetItem(() => inspector.CurrentItem);
                return currentitem;
            }
        }

        #endregion

        #region InspectorEvents_10_Event Members

        event MSOutlook.InspectorEvents_10_ActivateEventHandler MSOutlook.InspectorEvents_10_Event.Activate
        {
            add
            {
                ((MSOutlook.InspectorEvents_10_Event)inspector).Activate += value;
            }
            remove
            {
                ((MSOutlook.InspectorEvents_10_Event)inspector).Activate -= value;
            }
        }

        public event MSOutlook.InspectorEvents_10_BeforeMaximizeEventHandler BeforeMaximize
        {
            add
            {
                inspector.BeforeMaximize += value;
            }
            remove
            {
                inspector.BeforeMaximize -= value;
            }
        }

        public event MSOutlook.InspectorEvents_10_BeforeMinimizeEventHandler BeforeMinimize
        {
            add
            {
                inspector.BeforeMinimize += value;
            }
            remove
            {
                inspector.BeforeMinimize -= value;
            }
        }

        public event MSOutlook.InspectorEvents_10_BeforeMoveEventHandler BeforeMove
        {
            add
            {
                inspector.BeforeMove += value;
            }
            remove
            {
                inspector.BeforeMove -= value;
            }
        }

        public event MSOutlook.InspectorEvents_10_BeforeSizeEventHandler BeforeSize
        {
            add
            {
                inspector.BeforeSize += value;
            }
            remove
            {
                inspector.BeforeSize -= value;
            }
        }

        event MSOutlook.InspectorEvents_10_CloseEventHandler MSOutlook.InspectorEvents_10_Event.Close
        {
            add
            {
                ((MSOutlook.InspectorEvents_10_Event)inspector).Close += value;
            }
            remove
            {
                ((MSOutlook.InspectorEvents_10_Event)inspector).Close -= value;
            }
        }

        public event MSOutlook.InspectorEvents_10_DeactivateEventHandler Deactivate
        {
            add
            {
                inspector.Deactivate += value;
            }
            remove
            {
                inspector.Deactivate -= value;
            }
        }


        #endregion

        #region Redemption

        internal protected Redemption.SafeInspector SafeItem
        {
            get
            {
                SetupSafeItem();

                return safeitem;
            }
        }
        private void SetupSafeItem()
        {
            if (safeitem == null)
            {
                safeitem = Redemption.RedemptionFactory.Default.CreateSafeInspector();
                SafeItem.Item = inspector;
            }
        }

        #endregion

        #region Properties

        internal MSOutlook.Inspector InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return inspector;
            }
        }

        #endregion
    }
}
