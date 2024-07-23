using System;
using System.Runtime.InteropServices;

namespace FWBS.OMS.UI
{
    public class TabControl : System.Windows.Forms.TabControl
    {
        public bool isBorderDisabled = false;

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.RightToLeftLayout = true;
            else
                this.RightToLeftLayout = false;
        }

        /// <summary>
        /// Scales a System.Windows.Forms.TabPage if necessary and adds it to the collection.
        /// </summary>
        /// <param name="tabPage">
        /// The System.Windows.Forms.TabPage to add.
        /// </param>
        public void AddTabPage(System.Windows.Forms.TabPage tabPage)
        {
            if (DeviceDpi != 96)
                tabPage.Scale(new System.Drawing.SizeF(DeviceDpi / 96F, DeviceDpi / 96F));

            this.TabPages.Add(tabPage);
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            int TCM_ADJUSTRECT = 0x1328;
            //Change borders in runtime
            if (m.Msg == TCM_ADJUSTRECT && isBorderDisabled)
            {
                RECT rect = (RECT)m.GetLParam(typeof(RECT));
                var tabControlBorderSize = 4;
                rect.Left = this.Left - tabControlBorderSize;
                rect.Right = this.Right + tabControlBorderSize;
                rect.Top = this.Top - 1;
                rect.Bottom = this.Bottom + tabControlBorderSize;
                Marshal.StructureToPtr(rect, m.LParam, true);
            }
            // call the base class implementation
            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }
    }

    public class TreeView : System.Windows.Forms.TreeView
    {
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.RightToLeftLayout = true;
            else
                this.RightToLeftLayout = false;
        }
    }

    public class TreeViewRad : Telerik.WinControls.UI.RadTreeView
    {
    }

    public class ListView : System.Windows.Forms.ListView
    {
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_MOUSEWHEEL = 0x020A;

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.RightToLeftLayout = true;
            else
                this.RightToLeftLayout = false;
        }

        protected override void ScaleControl(System.Drawing.SizeF factor, System.Windows.Forms.BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            if (factor.Width != 1 && (specified & System.Windows.Forms.BoundsSpecified.Width) != 0)
            {
                foreach (System.Windows.Forms.ColumnHeader column in Columns)
                {
                    column.Width = Convert.ToInt32(column.Width * factor.Width);
                }
            }
        }

        protected override void WndProc(ref System.Windows.Forms.Message msg)
        {
            if ((msg.Msg == WM_VSCROLL) || (msg.Msg == WM_HSCROLL) || (msg.Msg == WM_MOUSEWHEEL))
            {
                this.Focus();
            }

            base.WndProc(ref msg);
        }
    }

    public class StatusBar : System.Windows.Forms.StatusBar
    {
        protected override void ScaleControl(System.Drawing.SizeF factor, System.Windows.Forms.BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            if (factor.Width != 1 && (specified & System.Windows.Forms.BoundsSpecified.Width) != 0)
            {
                foreach (System.Windows.Forms.StatusBarPanel statusBarPanel in Panels)
                {
                    if (statusBarPanel.AutoSize == System.Windows.Forms.StatusBarPanelAutoSize.None)
                    {
                        statusBarPanel.Width = Convert.ToInt32(statusBarPanel.Width * factor.Width);
                    }
                }
            }
        }
    }

    public class RichTextBox : System.Windows.Forms.RichTextBox
    {
        private string _rtf;

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            _rtf = Rtf;
            base.OnDpiChangedBeforeParent(e);
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            Rtf = _rtf;
            base.OnDpiChangedAfterParent(e);
        }
    }
}
