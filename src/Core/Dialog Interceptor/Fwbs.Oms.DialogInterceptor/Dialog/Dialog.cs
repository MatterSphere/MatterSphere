using System;

namespace Fwbs.Oms.DialogInterceptor
{
    using Fwbs.WinFinder;

    public abstract class Dialog : System.Windows.Forms.IWin32Window
    {
        #region Fields

        private readonly Window win;
        private readonly DialogConfig config;

        #endregion

        #region Constructors

        protected Dialog(Window win, DialogConfig config)
        {
            if (win == null)
                throw new ArgumentNullException("win");
            if (config == null)
                throw new ArgumentNullException("config");

            this.win = win;
            this.config = config;
            Hide();
        }

        #endregion

        #region Properties

        protected Window InternalWindow
        {
            get
            {
                return win;
            }
        }

        protected DialogConfig Configuration
        {
            get
            {
                return config;
            }
        }

        public bool IsValid
        {
            get
            {
                return win.IsValid;
            }
        }

        public IntPtr Handle
        {
            get
            {
                return win.Handle;
            }
        }

        public Window Parent
        {
            get
            {
                return win.Parent;
            }
        }

        protected virtual Window CancelButton
        {
            get
            {
                Window w = null;
                if (Configuration.CancelDialogId > -1)
                {
                    w = InternalWindow.GetDialogItem(Configuration.CancelDialogId);
                }

                if (w == null)
                {
                    WindowList wl = WindowList.Find(win, new WindowFilter(Configuration.CancelButtonClass, Configuration.CancelButtonText), true);
                    if (wl.Count > 0)
                        w = wl[0];
                    else
                        w = null;
                }

                return w;
            }
        }

        protected virtual Window OkButton
        {
            get
            {
                Window w = null;
                if (Configuration.OkDialogId > -1)
                {
                    w = InternalWindow.GetDialogItem(Configuration.OkDialogId);
                }

                if (w == null)
                {
                    WindowList wl = WindowList.Find(InternalWindow, new WindowFilter(Configuration.OkButtonClass, Configuration.OkButtonText), true);
                    if (wl.Count > 0)
                        w = wl[0];
                    else
                        w = null;
                }

                return w;
            }
        }

        #endregion

        #region Methods

        public void Hide()
        {
            this.win.MoveOffScreen();
            if (win.IsVisible)
                win.Hide(false);
        }

        public void Show()
        {
            SetDefaultLocation();
            win.Show();
        }

        protected void SetDefaultLocation()
        {
            //Centering the form to the parent does not help if it is minimised.  Especially if it is a dialog.
            if (InternalWindow.Parent.IsMinimised || !InternalWindow.Parent.IsVisible)
                InternalWindow.Centre(System.Windows.Forms.Screen.PrimaryScreen);
            else
                InternalWindow.Centre();
        }

        public void Cancel()
        {
            if (IsValid)
            {
                SetDefaultLocation();
                if (Configuration.CancelDialogId > -1)
                    NativeMethods.SendDlgItemMessage(win.Handle, Configuration.CancelDialogId, (int)WindowMessage.Click, IntPtr.Zero, IntPtr.Zero);
                else
                {
                    Window btn = CancelButton;
                    if (btn != null)
                        btn.SendMessage(WindowMessage.Click);
                }
            }

            win.Close();
        }

        public virtual void Ok()
        {
            SetDefaultLocation();

            if (Configuration.OkDialogId > -1)
                NativeMethods.SendDlgItemMessage(win.Handle, Configuration.OkDialogId, (int)WindowMessage.Click, IntPtr.Zero, IntPtr.Zero);
            else
            {
                Window btn = OkButton;
                if (btn != null)
                    btn.SendMessage(WindowMessage.Click);
            }
            win.Close();
        }



        protected Window FindByClassHierarchy(string className)
        {
            if (String.IsNullOrEmpty(className))
                return null;

            string[] names = className.ToUpperInvariant().Split(';');

            WindowList wl = WindowList.Find(win, new WindowFilter(names[0]), true);
            if (wl.Count > 0)
            {
                int ctr = 0;

                foreach (Window w in wl)
                {
                    Window parent = w;
                    while (parent.Handle != win.Handle && ctr < names.Length)
                    {
                        if (parent.Class.ToUpperInvariant() == names[ctr])
                        {
                            if (ctr == names.Length - 1)
                                return parent;
                        }

                        parent = parent.Parent;
                        ctr++;
                    }
                }
            }

            return null;
        }

        #endregion

    }
}
