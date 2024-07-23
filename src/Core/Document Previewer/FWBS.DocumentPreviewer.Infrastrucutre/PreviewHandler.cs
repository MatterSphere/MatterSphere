using System;
using System.Drawing;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview.Handlers
{

    public partial class PreviewHandler : 
        UserControl, 
        IPreviewHandler,
        IPreviewHandlerVisuals,
        IOleWindow,
        IObjectWithSite
    {

        [System.Runtime.InteropServices.DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        public PreviewHandler()
        {
            InitializeComponent();
        }

        #region IPreviewHandler Members

        public void SetWindow(IntPtr hwnd, ref RECT rect)
        {
            SetParent(this.Handle, hwnd);
            SetRect(ref rect);
        }

        public void SetRect(ref RECT rect)
        {
            this.Bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
        }

        public virtual void DoPreview()
        {
            throw new NotImplementedException();
        }

        public virtual void Unload()
        {
            this.Dispose();
        }

        public void SetFocus()
        {
            this.Focus();
        }

        public void QueryFocus(out IntPtr phwnd)
        {
            phwnd = this.Handle;
        }

        public uint TranslateAccelerator(ref MSG pmsg)
        {
            if (frame != null) 
                return frame.TranslateAccelerator(ref pmsg);

            const uint S_FALSE = 1;
            return S_FALSE;
        }

        #endregion

        #region IPreviewHandlerVisuals Members

        public void SetBackgroundColor(COLORREF color)
        {
        }

        public void SetFont(ref LOGFONT plf)
        {
        }

        public void SetTextColor(COLORREF color)
        {
            this.ForeColor = color.Color;
        }

        #endregion

        #region IOleWindow Members

        public void GetWindow(out IntPtr phwnd)
        {
            phwnd = IntPtr.Zero;
            phwnd = this.Handle;
        }

        public void ContextSensitiveHelp(bool fEnterMode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IObjectWithSite Members

        private object unkSite;
        protected IPreviewHandlerFrame frame;

        public void SetSite(object pUnkSite)
        {
            unkSite = pUnkSite;
            frame = unkSite as IPreviewHandlerFrame;
        }

        public void GetSite(ref Guid riid, out object ppvSite)
        {
            ppvSite = unkSite;
        }

        #endregion
    }
}
