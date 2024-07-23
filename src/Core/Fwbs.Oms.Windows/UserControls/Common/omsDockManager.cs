using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.UI.Windows;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;

namespace FWBS.OMS.UI
{
    public class omsDockManager : UltraDockManager
    {
        private const int posRight = 2;
        private const int posLeft = 1;
        private const int posTop = 4;
        private const int posBottom = 3;

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private IContainer components;

        private bool disableEvent = false;
        private Dictionary<string, Size> PaneSizes = new Dictionary<string, Size>();
        private DockablePaneBase Pane;

        private omsDockManagerInitializeSettings initializesettings;

        public omsDockManager()
            : base()
        {   
            Debug.WriteLine("omsDockManager Init ...");
            initializesettings = new omsDockManagerInitializeSettings { InitializePinned = true, InitializeDockingLocation = true, InitializePaneSize = true };
            InitializeComponent();
            this.ShowDisabledButtons = false;
        }

        public omsDockManager(IContainer container)
            : base(container)
        {
            Debug.WriteLine("omsDockManager Init Container...");
            initializesettings = new omsDockManagerInitializeSettings { InitializePinned = true, InitializeDockingLocation = true, InitializePaneSize = true };
            InitializeComponent();
            this.ShowDisabledButtons = false;
            Debug.WriteLine("omsDockManager Init Container End...");
        }

        public omsDockManager(IContainer container, omsDockManagerInitializeSettings InitializeSettings)
            : base(container)
        {
            Debug.WriteLine("omsDockManager Init Container...");

            initializesettings = InitializeSettings;
            InitializeComponent();
            this.ShowDisabledButtons = false;
            Debug.WriteLine("omsDockManager Init Container End...");
        }


        private DockAreaPane GetPain(DockedLocation pos)
        {
            foreach (var p in this.DockAreas)
                if (p.DockedLocation == pos)
                    return p;
            var n = new DockAreaPane(pos);
            this.DockAreas.Add(n);
            return n;
        }

        protected override void OnAfterDockChange(PaneEventArgs ea)
        {
            base.OnAfterDockChange(ea);

            foreach (var p in this.ControlPanes)
            {
                if (String.IsNullOrEmpty(p.Key) == false)
                {
                    EventHandler ev = null;

                    ev = (s, e) =>
                    {
                        p.Control.SizeChanged -= ev;
                        PaneSizes[p.Key] = p.Control.Size;
                        Save();
                    };

                    p.Control.SizeChanged += ev;
                }
            }
        }

        protected override void OnMouseEnterElement(Infragistics.Win.UIElementEventArgs e)
        {
            if (this.UseDefaultContextMenus == false)
                return;

            ImageAndTextUIElement pane =  e.Element as ImageAndTextUIElement;
            if (pane != null)
            {
                Pane = pane.GetContext() as DockablePaneBase;
                if (pane.Control != null && pane.Control.ContextMenu != null && pane.Control.ContextMenu.GetType().Name == "IGContextMenu")
                {
                    ContextMenu con = new ContextMenu();
                    MenuItem mnu = new MenuItem();
                    mnu.Tag = posTop;
                    mnu.Text = Session.CurrentSession.Resources.GetResource("DOCK2TOP", "Dock To Top", "").Text;
                    if (Pane.DockAreaPane.DockedLocation == DockedLocation.DockedTop)
                        mnu.Checked = true;
                    mnu.RadioCheck = true;
                    mnu.Click += new EventHandler(omsDockManager_Click);
                    con.MenuItems.Add(mnu);

                    mnu = new MenuItem();
                    mnu.Tag = posLeft;
                    if (Pane.DockAreaPane.DockedLocation == DockedLocation.DockedLeft)
                        mnu.Checked = true;
                    mnu.Text = Session.CurrentSession.Resources.GetResource("DOCK2LFT", "Dock To Left", "").Text;
                    mnu.RadioCheck = true;
                    mnu.Click += new EventHandler(omsDockManager_Click);
                    con.MenuItems.Add(mnu);

                    mnu = new MenuItem();
                    mnu.Tag = posBottom;
                    if (Pane.DockAreaPane.DockedLocation == DockedLocation.DockedBottom)
                        mnu.Checked = true;
                    mnu.Text = Session.CurrentSession.Resources.GetResource("DOCK2BTM", "Dock To Bottom", "").Text;
                    mnu.Click += new EventHandler(omsDockManager_Click);
                    mnu.RadioCheck = true;
                    con.MenuItems.Add(mnu);

                    mnu = new MenuItem();
                    mnu.Tag = posRight;
                    if (Pane.DockAreaPane.DockedLocation == DockedLocation.DockedRight)
                        mnu.Checked = true;
                    mnu.Text = Session.CurrentSession.Resources.GetResource("DOCK2RGT", "Dock To Right", "").Text;
                    mnu.RadioCheck = true;
                    mnu.Click += new EventHandler(omsDockManager_Click);
                    con.MenuItems.Add(mnu);
                    pane.Control.ContextMenu = con;
                }
            }
        }

        protected override void OnInitializePane(InitializePaneEventArgs e)
        {
            if (disableEvent) return;
            if (Session.CurrentSession.IsLoggedIn)
            {
                try
                {
                    disableEvent = true;
                    var docLeft = GetPain(DockedLocation.DockedLeft);
                    var docRight = GetPain(DockedLocation.DockedRight);
                    var docBottom = GetPain(DockedLocation.DockedBottom);
                    var docTop = GetPain(DockedLocation.DockedTop);
                    DockableControlPane p = e.Pane as DockableControlPane;
                    if (String.IsNullOrEmpty(e.Pane.Key) == false)
                    {
                        if (string.IsNullOrEmpty(e.Pane.TextTab) == false)
                            e.Pane.TextTab = Session.CurrentSession.Resources.GetResource(this.GenerateCodeLookupCode(e.Pane.TextTab), e.Pane.TextTab, "", true).Text;

                        if (string.IsNullOrEmpty(e.Pane.Text) == false)
                            e.Pane.Text = Session.CurrentSession.Resources.GetResource(this.GenerateCodeLookupCode(e.Pane.Text), e.Pane.Text, "", true).Text;

                        Favourites fav = new Favourites(e.Pane.Key);
                        
                        if (fav.Count > 0)
                        {
                            if(initializesettings.InitializeDockingLocation)
                                InitializePaneDocking(e, docLeft, docRight, docBottom, docTop, p, fav);

                            if(initializesettings.InitializePaneSize)
                                InitializePaneSize(e, p, fav);

                            if(initializesettings.InitializePinned)
                                InitializePanePinning(e, fav);
                        }
                        else
                        {
                            if (p.Pinned)
                                PaneSizes.Add(e.Pane.Key, p.DockAreaPane.Size);
                            else
                                PaneSizes.Add(e.Pane.Key, p.FlyoutSize);
                        }

                    }
                    else if (String.IsNullOrEmpty(e.Pane.Text) == false)
                        throw new Exception(String.Format("{0} is missing its Key", e.Pane.Text));
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                }
                finally
                {
                    disableEvent = false;
                }

            }
            base.OnInitializePane(e);
        }

        private static void InitializePanePinning(InitializePaneEventArgs e, Favourites fav)
        {
            if (Convert.ToBoolean(fav.Glyph(0)))
                e.Pane.Pin();
            else
            {
                e.Pane.Unpin();
                e.Pane.Manager.FlyIn(false);
            }
        }

        private void InitializePaneSize(InitializePaneEventArgs e, DockableControlPane p, Favourites fav)
        {
            try
            {
                string[] sizespit = fav.Param1(0).Split(",".ToCharArray());
                if (sizespit.Length > 1)
                {
                    Size psize = new Size(Convert.ToInt32(sizespit[0]), Convert.ToInt32(sizespit[1]));
                    p.DockAreaPane.Size = psize;
                    p.FlyoutSize = psize;
                    PaneSizes.Add(e.Pane.Key, psize);

                }
                else
                {
                    if (p.Pinned)
                        PaneSizes.Add(e.Pane.Key, p.DockAreaPane.Size);
                    else
                        PaneSizes.Add(e.Pane.Key, p.FlyoutSize);
                }
            }
            catch (Exception)
            {
                Trace.TraceError("ERR:DocManger-Size Error" + fav.Param1(0));
            }
        }

        private static void InitializePaneDocking(InitializePaneEventArgs e, DockAreaPane docLeft, DockAreaPane docRight, DockAreaPane docBottom, DockAreaPane docTop, DockableControlPane p, Favourites fav)
        {
            int ppos = -1;
            if (String.IsNullOrEmpty(fav.Param2(0)) == false)
                ppos = ConvertDef.ToInt32(fav.Param2(0), -1);
            p.Settings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            p.Settings.AllowMinimize = Infragistics.Win.DefaultableBoolean.True;
            p.Settings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            e.Pane.Pin();

            switch (ppos)
            {
                case posLeft:
                    p.RepositionToGroup(docLeft);
                    break;
                case posBottom:
                    p.RepositionToGroup(docBottom);
                    break;
                case posTop:
                    p.RepositionToGroup(docTop);
                    break;
                case posRight:
                    p.RepositionToGroup(docRight);
                    break;
            }
        }

        protected override void OnAfterPaneButtonClick(PaneButtonEventArgs e)
        {
            e.Pane.Manager.FlyIn();
            Save();
            base.OnAfterPaneButtonClick(e);
        }

        private void omsDockManager_Click(object sender, EventArgs e)
        {
            MenuItem mnu = sender as MenuItem;
            int ppos = Convert.ToInt32(mnu.Tag);
            int OrginalPos = Convert.ToInt32(Pane.DockAreaPane.DockedLocation);
            if (OrginalPos == ppos) return; // If No Change Return

            try
            {
                disableEvent = true; // Disable the OnInitializePane
                var docLeft = GetPain(DockedLocation.DockedLeft);
                var docRight = GetPain(DockedLocation.DockedRight);
                var docBottom = GetPain(DockedLocation.DockedBottom);
                var docTop = GetPain(DockedLocation.DockedTop);

                DockableControlPane pp = Pane as DockableControlPane;
                Size psize = PaneSizes[pp.Key];
                bool pinned = pp.Pinned;
                if (pinned == false)
                    Pane.Pin();
                foreach (MenuItem m in mnu.Parent.MenuItems)
                    m.Checked = false;
                mnu.Checked = true;

                switch (ppos)
                {
                    case posLeft:
                        pp.RepositionToGroup(docLeft);
                        break;
                    case posBottom:
                        pp.RepositionToGroup(docBottom);
                        break;
                    case posTop:
                        pp.RepositionToGroup(docTop);
                        break;
                    default:
                        pp.RepositionToGroup(docRight);
                        break;
                }

                switch (OrginalPos)
                {
                    case posLeft:
                        if (ppos == posRight)
                        {
                            pp.DockAreaPane.Size = psize;
                        }
                        else
                        {
                            pp.DockAreaPane.Size = SwitchSize(psize);
                        }
                        break;
                    case posBottom:
                        if (ppos == posTop)
                        {
                            pp.DockAreaPane.Size = psize;
                        }
                        else
                        {
                            pp.DockAreaPane.Size = SwitchSize(psize);
                        }
                        break;
                    case posTop:
                        if (ppos == posBottom)
                        {
                            pp.DockAreaPane.Size = psize;
                        }
                        else
                        {
                            pp.DockAreaPane.Size = SwitchSize(psize);
                        }
                        break;
                    default:
                        if (ppos == posLeft)
                        {
                            pp.DockAreaPane.Size = psize;
                        }
                        else
                        {
                            pp.DockAreaPane.Size = SwitchSize(psize);
                        }
                        break;
                }
                pp.FlyoutSize = pp.DockAreaPane.Size;
                PaneSizes[pp.Key] = pp.DockAreaPane.Size;

                if (pinned == false)
                    pp.Unpin();
            }
            finally
            {
                disableEvent = false;
                Save();
            }
        }

        protected override void OnBeforePaneButtonClick(CancelablePaneButtonEventArgs e)
        {
            base.OnBeforePaneButtonClick(e);
            DockableControlPane pp = e.Pane as DockableControlPane;

            if (pp.Pinned)
                pp.FlyoutSize = PaneSizes[e.Pane.Key];
            else
                pp.DockAreaPane.Size = PaneSizes[e.Pane.Key];
        }

        protected override void OnAfterSplitterDrag(PanesEventArgs e)
        {
            base.OnAfterSplitterDrag(e);
            foreach (var p in this.ControlPanes)
            {
                if (String.IsNullOrEmpty(p.Key) == false)
                    PaneSizes[p.Key] = p.Control.Size;
            }
            Save();
        }

        private string GenerateCodeLookupCode(string text)
        {
            int t = text.GetHashCode();
            return t.ToString("x");
        }

        private Size SwitchSize(Size size)
        {
            return new Size(size.Height, size.Width);
        }

        private void Save()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                Trace.WriteLine(this.GetType().Name + "Saving Panes");
                foreach (var p in this.ControlPanes)
                {
                    
                    Favourites fav = new Favourites(p.Key);
                    DockableControlPane pp = p as DockableControlPane;
                    if (fav.Count == 0)
                    {
                        fav.AddFavourite("PINNED", Convert.ToString(p.Pinned), String.Format("{0},{1}", PaneSizes[p.Key].Width, PaneSizes[p.Key].Height), Convert.ToInt32(pp.DockAreaPane.DockedLocation).ToString());
                    }
                    else
                    {
                        fav.Glyph(0, Convert.ToString(p.Pinned));
                        fav.Param1(0, String.Format("{0},{1}", PaneSizes[p.Key].Width, PaneSizes[p.Key].Height));
                        fav.Param2(0, Convert.ToInt32(pp.DockAreaPane.DockedLocation).ToString());
                    }
                    fav.Update();
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Text = "Dock To Top";
            // 
            // omsDockManager
            // 
            this.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            this.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            this.DefaultPaneSettings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.True;
            this.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            this.DefaultPaneSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.DefaultPaneSettings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            this.DefaultPaneSettings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
            this.ShowCloseButton = false;
            this.ShowDisabledButtons = false;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
