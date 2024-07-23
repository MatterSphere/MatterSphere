using System;
using System.Text;

namespace Fwbs
{
    namespace WinFinder
    {
        using System.ComponentModel;
        using System.Drawing;
        using System.Runtime.InteropServices;
        using System.Windows.Forms;

        public class Window : System.Windows.Forms.IWin32Window, IDisposable
        {

            #region Fields

            private Window parent;
            private WindowList children;
            private readonly IntPtr handle;
            private Point originallocation;
            private Size originalsize;
            private readonly static string[] TextExClasses = new string[] { "COMBOBOX", "COMBOBOXEX32", "EDIT" };

            private IntPtr thumbnail;

            #endregion

            #region Constructors

            protected internal Window(IntPtr handle)
            {
                this.handle = handle;
                this.originallocation = Location;
                this.originalsize = Size;
            }

            #endregion

            #region Properties

            IntPtr IWin32Window.Handle
            {
                get
                {
                    if (IsValid == false)
                        return IntPtr.Zero;
                    else
                        return handle;
                }
            }

            public IntPtr Handle
            {
                get
                {
                    return handle;
                }
            }

            public HandleRef HandleRef
            {
                get
                {
                    return new HandleRef(this, handle);
                }
            }

            public bool IsLocal
            {
                get
                {
                    return Process.Id == System.Diagnostics.Process.GetCurrentProcess().Id;
                }
            }

            private Icon GetIcon(NativeMethods.IconSize size)
            {
                try
                {
                    IntPtr ret;

                    NativeMethods.SendMessageTimeout(
                        HandleRef,
                        WindowMessage.GetIcon,
                        (IntPtr)size,
                        IntPtr.Zero,
                        NativeMethods.SendMessageTimeoutFlags.AbortIfHung,
                        1000,
                        out ret);

                    if (ret == IntPtr.Zero)
                    {
                        switch (size)
                        {
                            case NativeMethods.IconSize.Big:
                                ret = NativeMethods.GetClassLongPtr(HandleRef, (int)NativeMethods.GetClassLongFlags.HIcon);
                                break;
                            case NativeMethods.IconSize.Small:
                                ret = NativeMethods.GetClassLongPtr(HandleRef, (int)NativeMethods.GetClassLongFlags.HIconSmall);
                                break;
                        }
                    }

                    if (ret == IntPtr.Zero)
                    {
                        NativeMethods.SendMessageTimeout(
                            HandleRef,
                            WindowMessage.QueryDragIcon,
                            IntPtr.Zero,
                            IntPtr.Zero,
                        NativeMethods.SendMessageTimeoutFlags.AbortIfHung,
                            1000, out ret);
                    }

                    if (ret == IntPtr.Zero)
                        return null;
                    else
                    {
                        Icon ico = Icon.FromHandle(ret);
                        if (ico.Size == Size.Empty)
                        {
                            ico.Dispose();
                            return null;
                        }
                        else
                            return ico;
                    }
                }
                catch (Win32Exception)
                {
                    return null;
                }
            }

            private void SetIcon(Icon icon, NativeMethods.IconSize size)
            {
                if (icon == null)
                    NativeMethods.SendMessage(HandleRef, WindowMessage.SetIcon, (IntPtr)size, IntPtr.Zero);
                else
                {
                    IntPtr ico = icon.Handle;
                    NativeMethods.SendMessage(HandleRef, WindowMessage.SetIcon, (IntPtr)size, ico);
                }
            }

            public Icon Icon
            {
                get
                {
                    return GetIcon(NativeMethods.IconSize.Small);
                }
                set
                {
                    SetIcon(value, NativeMethods.IconSize.Small);
                }
            }

            public Icon LargeIcon
            {
                get
                {
                    return GetIcon(NativeMethods.IconSize.Big);
                }
                set
                {
                    SetIcon(value, NativeMethods.IconSize.Big);
                }
            }

  
            public string Text
            {
                get
                {
                    string cls = Class.ToUpperInvariant();

                    
                    if (Array.IndexOf<string>(TextExClasses, cls) > -1)
                        return NativeMethods.GetWindowTextEx(HandleRef);
                    else
                        return NativeMethods.GetWindowText(HandleRef);
                }
                set
                {
                    string cls = Class.ToUpperInvariant();

                    //Wake up calls
                    if (string.Compare(cls, "COMBOBOX", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append((char)32);
                        NativeMethods.SendMessageText(HandleRef, WindowMessage.Char, new IntPtr(1), sb);
                        NativeMethods.SetWindowText(HandleRef, value);
                    }
                    else if (string.Compare(cls, "EDIT", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Focus();
                        NativeMethods.SetWindowText(HandleRef, value);
                        UnFocus();
                    }
                    else
                        NativeMethods.SetWindowText(HandleRef, value);
                }
            }

            public string FileName
            {
                get
                {
                    try
                    {
                        System.Diagnostics.Process proc = Process;
                        return proc.MainModule.FileName;
                    }
                    catch (Win32Exception)
                    {
                        return String.Empty;
                    }

                }
            }

            public string Class
            {
                get
                {
                    return NativeMethods.GetClassName(HandleRef);
                }
            }


            public WindowList Children
            {
                get
                {
                    if (children == null)
                    {
                        children = new WindowList(this);
                        children.Refresh();
                    }

                    return children;
                }
            }

            public Window RootParent
            {
                get
                {
                    return WindowFactory.GetWindow(NativeMethods.GetAncestor(HandleRef, NativeMethods.GetAncestorFlags.Root));
                }
            }

            public Window Parent
            {
                get
                {
                    if (parent == null)
                    {
                        IntPtr parenthandle = NativeMethods.GetParent(HandleRef);
                        if (parenthandle != IntPtr.Zero)
                            parent = WindowFactory.GetWindow(parenthandle);
                    }
                    return parent;
                }
                set
                {
                    IntPtr ret;

                    if (value == null)
                        ret = NativeMethods.SetParent(HandleRef, IntPtr.Zero);
                    else
                        ret = NativeMethods.SetParent(HandleRef, value.HandleRef);

                    if (ret == IntPtr.Zero)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
            }

            public System.Diagnostics.Process Process
            {
                get
                {
                    int procid = 0;
                    NativeMethods.GetWindowThreadProcessId(HandleRef, out procid);
                    return System.Diagnostics.Process.GetProcessById(procid);
                }
            }


            public System.Windows.Forms.Screen Screen
            {
                get
                {
                    return System.Windows.Forms.Screen.FromHandle(Handle);
                }
            }


            public int DialogControlId
            {
                get
                {
                    return NativeMethods.GetDlgCtrlID(HandleRef);
                }
            }


            #region Titlebar

            public Rectangle TitleBarBounds
            {
                get
                {
                    NativeMethods.TitleBarInfo info = new NativeMethods.TitleBarInfo();
                    info.cbSize = Marshal.SizeOf(info);
                    bool ret = NativeMethods.GetTitleBarInfo(HandleRef, ref info);
                    if (!ret)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                    return new Rectangle(info.rcTitleBar.X, info.rcTitleBar.Y, info.rcTitleBar.Width - info.rcTitleBar.Left, info.rcTitleBar.Height - info.rcTitleBar.Top);

                }
            }

            public Point TitleBarLocation
            {
                get
                {
                    return TitleBarBounds.Location;
                }
            }

            public Size TitleBarSize
            {
                get
                {
                    return TitleBarBounds.Size;
                }
            }


            #endregion

            #region Dimensions

            public Size ClientSize
            {
                get
                {
                    Rectangle rect = new Rectangle();
                    bool ret = NativeMethods.GetClientRect(HandleRef, ref rect);

                    if (!ret)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                    return rect.Size;
                }
            }

            public Rectangle ClientRectangle
            {
                get
                {
                    return new Rectangle(new Point(0, 0), ClientSize);
                }
            }

            public Size OriginalSize
            {
                get
                {
                    return originalsize;
                }
                set
                {
                    originalsize = value;
                }
            }

            public Size Size
            {
                get
                {
                    return Bounds.Size;
                }
                set
                {
                    Point point = Location;
                    bool ret = NativeMethods.MoveWindow(HandleRef, point.X, point.Y, value.Width, value.Height, true);

                    if (!ret)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
            }

            public Point OriginalLocation
            {
                get
                {
                    return originallocation;
                }
                set
                {
                    originallocation = value;
                }
            }

            public Point Location
            {
                get
                {
                    return Bounds.Location;
                }
                set
                {
                    Size size = Size;

                    bool ret = NativeMethods.MoveWindow(HandleRef, value.X, value.Y, size.Width, size.Height, true);
                    if (!ret)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }

            }

            public Rectangle Bounds
            {
                get
                {
                    Rectangle rect = new Rectangle();
                    NativeMethods.GetWindowRect(HandleRef, ref rect);
                    return new Rectangle(rect.Left, rect.Top, rect.Width - rect.Left, rect.Height - rect.Top);
                }
                set
                {
                    bool ret = NativeMethods.MoveWindow(HandleRef, value.X, value.Y, value.Width, value.Height, true);

                    if (!ret)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
            }


            #endregion

            #region State

            public WindowStyles Styles
            {
                get
                {
                    return (WindowStyles)NativeMethods.GetWindowLong(HandleRef, NativeMethods.GetWindowLongFlags.Style);
                }
                private set
                {
                    int ret = NativeMethods.SetWindowLong(HandleRef, NativeMethods.GetWindowLongFlags.Style, (uint)value);
                    if (ret == 0)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
            }

            public WindowExtendedStyles ExtendedStyles
            {
                get
                {
                    return (WindowExtendedStyles)NativeMethods.GetWindowLong(HandleRef, NativeMethods.GetWindowLongFlags.ExStyle);
                }
                private set
                {
                    int ret = NativeMethods.SetWindowLong(HandleRef, NativeMethods.GetWindowLongFlags.ExStyle, (uint)value);

                    if (ret == 0)
                        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                }
            }

            public bool IsVisible
            {
                get
                {
                    return NativeMethods.IsWindowVisible(HandleRef);
                }
                set
                {
                    if (value)
                    {
                        if (!IsVisible)
                            Show();
                    }
                    else
                    {
                        if (IsVisible)
                            Hide();
                    }
                }
            }

            public bool IsMaximised
            {
                get
                {
                    return NativeMethods.IsZoomed(HandleRef);
                }
            }

            public bool IsMinimised
            {
                get
                {
                    return NativeMethods.IsIconic(HandleRef);
                }
            }

            public bool HasMinimizeBox
            {
                get
                {
                    return (Styles | WindowStyles.MinimiseBox) == Styles;
                }
                set
                {
                    if (value)
                        Styles |= WindowStyles.MinimiseBox;
                    else
                        Styles &= ~WindowStyles.MinimiseBox;
                }
            }

            public bool HasMaximizeBox
            {
                get
                {
                    return (Styles | WindowStyles.MaximiseBox) == Styles;
                }
                set
                {
                    if (value)
                        Styles |= WindowStyles.MaximiseBox;
                    else
                        Styles &= ~WindowStyles.MaximiseBox;
                }
            }

            public bool HasCaption
            {
                get
                {
                    return (Styles | WindowStyles.Caption) == Styles;
                }
                set
                {
                    ModifyStyle(WindowStyles.Caption, value);
                }
            }

            public bool HasControlBox
            {
                get
                {
                    return (Styles | WindowStyles.SysMenu) == Styles;
                }
                set
                {
                    ModifyStyle(WindowStyles.SysMenu, value);
                }
            }


            public bool IsEnabled
            {
                get
                {
                    return (Styles | WindowStyles.Disabled) != Styles;
                }
                set
                {
                    ModifyStyle(WindowStyles.Disabled, !value);
                }
            }

            public bool HasHorizontalScroll
            {
                get
                {
                    return (Styles | WindowStyles.HScroll) == Styles;
                }
                set
                {
                    ModifyStyle(WindowStyles.HScroll, value);
                }
            }

            public bool HasVerticalScroll
            {
                get
                {
                    return (Styles | WindowStyles.VScroll) == Styles;
                }
                set
                {
                    ModifyStyle(WindowStyles.VScroll, value);
                }
            }

            public bool IsHung
            {
                get
                {
                    return NativeMethods.IsHungAppWindow(HandleRef);
                }
            }

            public bool IsValid
            {
                get
                {
                    return NativeMethods.IsWindow(HandleRef);
                }
            }

            public bool IsChild
            {
                get
                {
                    Window p = Parent;
                    if (p == null)
                        return false;
                    else
                        return NativeMethods.IsChild(p.HandleRef, HandleRef);
                }
            }

            private bool topmost;
            public bool TopMost
            {
                get
                {
                    return topmost;
                }
                set
                {
                    if (value)
                    {
                        NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.TopMost, 0, 0, 0, 0,
                            NativeMethods.SetWindowPosFlags.NoActivate |
                            NativeMethods.SetWindowPosFlags.NoMove |
                            NativeMethods.SetWindowPosFlags.NoSize |
                            NativeMethods.SetWindowPosFlags.ShowWindow);
                        topmost = true;
                    }
                    else
                    {
                        bool ret = NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.NoTopMost, 0, 0, 0, 0,
          NativeMethods.SetWindowPosFlags.NoActivate |
          NativeMethods.SetWindowPosFlags.NoMove |
          NativeMethods.SetWindowPosFlags.NoSize |
          NativeMethods.SetWindowPosFlags.ShowWindow);


                        if (!ret)
                            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

                        topmost = false;
                    }
                }
            }

            private bool highlighted;
            public bool IsHighlighted
            {
                get
                {
                    return highlighted;
                }
            }


            public bool IsUnicode
            {
                get
                {
                    return NativeMethods.IsWindowUnicode(HandleRef);
                }
            }

            #endregion


            #endregion

            #region Find Methods

            public WindowList Find(string className)
            {
                return WindowList.Find(this, new WindowFilter(className));
            }

            public WindowList Find(string className, string title)
            {
                return WindowList.Find(this, new WindowFilter(className, title));
            }

            public WindowList Find(WindowFilter filterCriteria)
            {
                return WindowList.Find(this, filterCriteria);
            }

            public WindowList Find(WindowFilter filterCriteria, bool recursive)
            {
                return WindowList.Find(this, filterCriteria, recursive);
            }

            public WindowList Find(WindowFilter filterCriteria, WindowFilterCallback callback)
            {
                return WindowList.Find(this, filterCriteria, callback);
            }

            public WindowList Find(WindowFilter filterCriteria, WindowFilterCallback callback, bool recursive)
            {
                return WindowList.Find(this, filterCriteria, callback, recursive);
            }

            #endregion

            #region Methods

            public int Tile()
            {
                return NativeMethods.TileWindows(HandleRef, NativeMethods.MdiTitle.Vertical, IntPtr.Zero, 0, null);
            }

            public int Cascade()
            {
                return NativeMethods.CascadeWindows(HandleRef, NativeMethods.MdiTitle.Default, IntPtr.Zero, 0, null);
            }

            public Window Next()
            {
                return WindowFactory.GetWindow(NativeMethods.GetWindow(HandleRef, NativeMethods.GetWindowFlags.Next));
            }

            public Window Previous()
            {
                return WindowFactory.GetWindow(NativeMethods.GetWindow(HandleRef, NativeMethods.GetWindowFlags.Previous));
            }

            public Window GetDialogItem(int id)
            {
                return WindowFactory.GetWindow(NativeMethods.GetDlgItem(HandleRef, id));
            }

            public void Focus()
            {
                SendMessage(WindowMessage.SetFocus);
            }

            public void UnFocus()
            {
                SendMessage(WindowMessage.KillFocus);
            }

            public int SendMessage(WindowMessage message)
            {
                return NativeMethods.SendMessage(HandleRef, message, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            public int SendMessage(WindowMessage message, IntPtr param1, IntPtr param2)
            {
                return NativeMethods.SendMessage(HandleRef, message, param1, param2).ToInt32();
            }
            public int SendMessage(System.Windows.Forms.Message message)
            {
                return NativeMethods.SendMessage(HandleRef, message.Msg, message.LParam, message.WParam).ToInt32();
            }
            public int SendMessage(int message)
            {
                return NativeMethods.SendMessage(HandleRef, message, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            public int SendMessage(int message, IntPtr param1, IntPtr param2)
            {
                return NativeMethods.SendMessage(HandleRef, message, param1, param2).ToInt32();
            }

            public bool PostMessage(WindowMessage message)
            {
                return NativeMethods.PostMessage(HandleRef, message, IntPtr.Zero, IntPtr.Zero);
            }
            public bool PostMessage(WindowMessage message, IntPtr param1, IntPtr param2)
            {
                return NativeMethods.PostMessage(HandleRef, message, param1, param2);
            }

            public bool PostMessage(System.Windows.Forms.Message message)
            {
                return NativeMethods.PostMessage(HandleRef, message.Msg, message.LParam, message.WParam);
            }

            public bool PostMessage(int message)
            {
                return NativeMethods.PostMessage(HandleRef, message, IntPtr.Zero, IntPtr.Zero);
            }
            public bool PostMessage(int message, IntPtr param1, IntPtr param2)
            {
                return NativeMethods.PostMessage(HandleRef, message, param1, param2);
            }

            private void ModifyStyle(WindowStyles style, bool value)
            {
                if (value)
                    Styles |= style;
                else
                    Styles &= ~style;

                bool ret = NativeMethods.SetWindowPos(HandleRef, 0, 0, 0, 0, 0,
                     NativeMethods.SetWindowPosFlags.NoZOrder
                    | NativeMethods.SetWindowPosFlags.NoMove
                    | NativeMethods.SetWindowPosFlags.NoSize
                    | NativeMethods.SetWindowPosFlags.NoActivate
                    | NativeMethods.SetWindowPosFlags.FrameChanged);


                if (!ret)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }


            public void BringToFront()
            {
                bool ret = NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.TopMost, 0, 0, 0, 0,
    NativeMethods.SetWindowPosFlags.NoActivate |
    NativeMethods.SetWindowPosFlags.NoMove |
    NativeMethods.SetWindowPosFlags.NoSize |
    NativeMethods.SetWindowPosFlags.ShowWindow);


                if (!ret)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());


                ret = NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.NoTopMost, 0, 0, 0, 0,
    NativeMethods.SetWindowPosFlags.NoActivate |
    NativeMethods.SetWindowPosFlags.NoMove |
    NativeMethods.SetWindowPosFlags.NoSize |
    NativeMethods.SetWindowPosFlags.ShowWindow);


                if (!ret)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }

            public void SendToBack()
            {
                bool ret = NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.Bottom, 0, 0, 0, 0,
          NativeMethods.SetWindowPosFlags.NoActivate |
          NativeMethods.SetWindowPosFlags.NoMove |
          NativeMethods.SetWindowPosFlags.NoSize |
          NativeMethods.SetWindowPosFlags.ShowWindow);

                if (!ret)
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            }


            public bool Show()
            {
                return Show(true);
            }
            public bool Show(bool async)
            {
                bool ret;

                if (async)
                    ret = NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Show);
                else
                    ret = NativeMethods.ShowWindow(HandleRef, NativeMethods.ShowWindowStyles.Show);

                NativeMethods.UpdateWindow(HandleRef);

                return ret;
            }

            public bool Hide()
            {
                return Hide(true);
            }

            public bool Hide(bool async)
            {
                bool ret;
                if (async)
                    ret = NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Hide);
                else
                    ret = NativeMethods.ShowWindow(HandleRef, NativeMethods.ShowWindowStyles.Hide);

                return ret;
            }

            public bool Close()
            {
                return Close(false);
            }

            public bool Close(bool force)
            {
                if (force)
                    NativeMethods.EndTask(HandleRef, false, true);
                else
                {
                    if (!NativeMethods.DestroyWindow(HandleRef))
                        SendMessage(WindowMessage.Close);
                }

                return !IsValid;
            }

            public bool Activate()
            {
                if (NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Restore))
                {
                    if (NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.TopMost, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoSize))
                    {
                        if (NativeMethods.SetWindowPos(HandleRef, NativeMethods.SetWindowPosZOrder.NoTopMost, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.NoMove | NativeMethods.SetWindowPosFlags.NoSize))
                            return true;
                    }
                }

                return false;
            }

            public bool Minimise()
            {
                return NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Minimise);

            }

            public bool Maximise()
            {
                return NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Maximise);
            }

            public bool Restore()
            {
                return NativeMethods.ShowWindowAsync(HandleRef, NativeMethods.ShowWindowStyles.Restore);
            }


            public void Highlight()
            {
                if (!IsHighlighted)
                {
                    ToggleHighlight();
                    highlighted = true;
                }
            }

            public void UnHighlight()
            {
                if (IsHighlighted)
                {
                    ToggleHighlight();
                    highlighted = false;
                }
            }

            private void ToggleHighlight()
            {
                const int frameWidth = 3;

                Rectangle rect = new Rectangle();

                rect = this.ClientRectangle;

                using (Graphics gr = Graphics.FromHwnd(Handle))
                {
                    IntPtr hdc = gr.GetHdc();

                    NativeMethods.PatBlt(hdc, rect.Left, rect.Top, rect.Right - rect.Left, frameWidth, NativeMethods.BitBltFlags.DestInvert);
                    NativeMethods.PatBlt(hdc, rect.Left, rect.Bottom - frameWidth, frameWidth, -(rect.Bottom - rect.Top - 2 * frameWidth), NativeMethods.BitBltFlags.DestInvert);
                    NativeMethods.PatBlt(hdc, rect.Right - frameWidth, rect.Top + frameWidth, frameWidth, rect.Bottom - rect.Top - 2 * frameWidth, NativeMethods.BitBltFlags.DestInvert);
                    NativeMethods.PatBlt(hdc, rect.Right, rect.Bottom - frameWidth, -(rect.Right - rect.Left), frameWidth, NativeMethods.BitBltFlags.DestInvert);

                    gr.ReleaseHdc(hdc);
                }

            }

            public Image Capture()
            {
                int hr = 0;
                Size size = Size;

                Image img = new Bitmap(size.Width, size.Height);
                using (Graphics destgr = Graphics.FromImage(img), sourcegr = Graphics.FromHwnd(Handle))
                {
                    IntPtr desthdc = destgr.GetHdc();
                    IntPtr sourcehdc = sourcegr.GetHdc();
                    if (!NativeMethods.BitBlt(desthdc, 0, 0, img.Width, img.Height, sourcehdc, 0, 0, NativeMethods.BitBltFlags.SourceCopy))
                    {
                        hr = Marshal.GetHRForLastWin32Error();
                    }
                    destgr.ReleaseHdc(desthdc);
                    sourcegr.ReleaseHdc(sourcehdc);
                }

                if (hr != 0)
                {
                    img.Dispose();
                    Marshal.ThrowExceptionForHR(hr);
                }

                return img;
            }

            public Image GetThumbnail(int width, int height)
            {
                int hr = 0;
                Size size = Size;

                if (size.Width > size.Height)
                    height = (int)(((double)size.Height / (double)size.Width) * (double)width);
                else
                    width = (int)(((double)size.Width / (double)size.Height) * (double)height);


                Image img = new Bitmap(width, height);
                using (Graphics destgr = Graphics.FromImage(img), sourcegr = Graphics.FromHwnd(Handle))
                {
                    IntPtr desthdc = destgr.GetHdc();
                    IntPtr sourcehdc = sourcegr.GetHdc();
                    if (!NativeMethods.StretchBlt(desthdc, 0, 0, img.Width, img.Height, sourcehdc, 0, 0, size.Width, size.Height, NativeMethods.BitBltFlags.SourceCopy))
                    {
                        hr = Marshal.GetHRForLastWin32Error();
                    }
                    destgr.ReleaseHdc(desthdc);
                    sourcegr.ReleaseHdc(sourcehdc);
                }

                if (hr != 0)
                {
                    img.Dispose();
                    Marshal.ThrowExceptionForHR(hr);
                }

                return img;
            }

            public void SetDefaults()
            {
                Location = OriginalLocation;
                Size = OriginalSize;
            }

            public void MoveOffScreen()
            {
                Point offscreen = new Point();
                foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens)
                {
                    if (scr.Bounds.Bottom > offscreen.Y)
                        offscreen.Y = scr.Bounds.Bottom;
                    if (scr.Bounds.Right > offscreen.X)
                        offscreen.X = scr.Bounds.Right;
                }

                this.Location = offscreen;
            }

            public void Centre()
            {
                Centre(this.Parent);
            }

            public void Centre(Window window)
            {
                if (window == null)
                    window = this.Parent;

                if (window == null)
                    Centre(this.Screen);
                else
                {
                    Centre(window.Bounds);
                }
            }

            public void Centre(Screen screen)
            {
                if (screen == null)
                    screen = Screen.PrimaryScreen;

                if (screen == null)
                    throw new ArgumentNullException("screen");

                Centre(screen.Bounds);
            }

            private void Centre(Rectangle bounds)
            {
                this.Location = new Point(bounds.Left + (bounds.Width / 2) - (this.Size.Width / 2), bounds.Top + (bounds.Height / 2) - (this.Size.Height / 2));
            }

            public void StopFlash()
            {
                NativeMethods.FlashWindowInfo info = new NativeMethods.FlashWindowInfo();
                info.dwFlags = NativeMethods.FlashVals.Stop;
                Flash(info);
            }

            public void Flash()
            {
                Flash(FlashWhat.All, FlashUntil.Continuous, 0, int.MaxValue);
            }

            public void Flash(FlashWhat what, FlashUntil until, int speed, int count)
            {
                NativeMethods.FlashWindowInfo info = new NativeMethods.FlashWindowInfo();
                info.dwFlags = unchecked((NativeMethods.FlashVals)((uint)what | (uint)until));
                info.dwTimeout = unchecked((uint)speed);
                info.uCount = unchecked((uint)count);
                Flash(info);
            }

            private void Flash(NativeMethods.FlashWindowInfo info)
            {
                info.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.FlashWindowInfo));
                info.hwnd = Handle;
                NativeMethods.FlashWindowEx(ref info);
            }

            public override string ToString()
            {
                return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} ({1}) - {2}", Handle, Class, Text);
            }

            #endregion

            #region Vista Thumbnails

            private void InternalUpdateLiveThumbnail(Rectangle destination)
            {

                Size size = new Size();


                NativeMethods.DwmQueryThumbnailSourceSize(thumbnail, out size);

                NativeMethods.DwmThumbailProperties props = new NativeMethods.DwmThumbailProperties();

                props.fVisible = true;
                props.dwFlags = NativeMethods.DwmThumbnailPropertyFlags.Visible | NativeMethods.DwmThumbnailPropertyFlags.RectDestination | NativeMethods.DwmThumbnailPropertyFlags.Opacity;
                props.opacity = (byte)255;
                props.rcDestination = destination;

                if (size.Width < 100)
                {
                    props.rcDestination.Width = size.Width;
                }

                if (size.Height < 100)
                {
                    props.rcDestination.Height = size.Height;
                }

                NativeMethods.DwmUpdateThumbnailProperties(thumbnail, ref props);
            }

            public bool AttachLiveThumbnail(System.Windows.Forms.IWin32Window window)
            {
                if (window == null)
                    throw new ArgumentNullException("window");

                using (Window win = new Window(window.Handle))
                {

                    bool update = false;
                    if (thumbnail == IntPtr.Zero)
                    {
                        int i = NativeMethods.DwmRegisterThumbnail(new HandleRef(window, window.Handle), HandleRef, out thumbnail);

                        if (i == 0)
                            update = true;
                    }
                    else
                        update = true;

                    if (update)
                    {
                        InternalUpdateLiveThumbnail(new Rectangle(new Point(0, 0), win.ClientSize));
                        return true;
                    }
                    else
                        return false;
                }
            }

            public void DetachLiveThumbnail()
            {
                if (thumbnail != IntPtr.Zero)
                    NativeMethods.DwmUnregisterThumbnail(thumbnail);
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (parent != null)
                    {
                        parent.Dispose();
                        parent = null;
                    }
                }

                DetachLiveThumbnail();
            }

            ~Window()
            {
                Dispose(false);
            }

            #endregion

        }
    }
}