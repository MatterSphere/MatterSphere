using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    /// <summary>
    /// Summary description for Toolbar.
    /// </summary>
    public class ToolBar : System.Windows.Forms.ToolBar
	{
		public ToolBar() : base()
		{
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ImageList != null)
                {
                    this.ImageList = null;
                }
            }
            base.Dispose(disposing);
        }

		protected override bool ProcessMnemonic(char charCode)
		{
			foreach (ToolBarButton tb in this.Buttons)
				if (IsMnemonic(charCode, tb.Text) && tb.Visible && tb.Enabled)
				{
                    if (tb.Style == ToolBarButtonStyle.ToggleButton)
                        tb.Pushed = !tb.Pushed;
                    this.OnButtonClick(new ToolBarButtonClickEventArgs(tb));
					return true;
				}
			return false;
		}

        #region Native Code

        private const ushort DDARROW_WIDTH = 15;
        private const uint TBIF_SIZE = 0x00000040;

        private const int TB_SETBUTTONINFOW = 0x0440;
        private const int TB_SETBUTTONINFOA = 0x0442;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct TBBUTTONINFO
        {
            public uint cbSize;
            public uint dwMask;
            public int idCommand;
            public int iImage;
            public byte fsState;
            public byte fsStyle;
            public ushort cx;
            public IntPtr lParam;
            public IntPtr pszText;
            public int cchTest;
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == TB_SETBUTTONINFOW || m.Msg == TB_SETBUTTONINFOA) && DropDownArrows && (DeviceDpi != 96))
            {
                TBBUTTONINFO tbButtonInfo = (TBBUTTONINFO)m.GetLParam(typeof(TBBUTTONINFO));
                if ((tbButtonInfo.dwMask & TBIF_SIZE) != 0)
                {
                    ToolBarButton button = Buttons[m.WParam.ToInt32()];
                    if (button.Style == ToolBarButtonStyle.DropDownButton)
                    {
                        tbButtonInfo.cx -= DDARROW_WIDTH - 1;
                        tbButtonInfo.cx += (ushort)LogicalToDeviceUnits(DDARROW_WIDTH + 1);
                        Marshal.StructureToPtr(tbButtonInfo, m.LParam, true);
                    }
                }
            }

            base.WndProc(ref m);
        }

        #endregion
	}
}
