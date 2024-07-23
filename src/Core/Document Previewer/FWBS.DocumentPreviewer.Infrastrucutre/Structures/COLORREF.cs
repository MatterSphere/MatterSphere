using System.Drawing;
using System.Runtime.InteropServices;

namespace Fwbs.Documents.Preview.Handlers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COLORREF
    {
        private uint dword;

        public COLORREF(Color col)
        {
            dword = (uint)col.ToArgb() & 0x00ffffff;
        }

        public Color Color
        {
            get
            {
                return
                    Color.FromArgb((int)(0x000000FFU & dword), (int)(0x0000FF00U & dword) >> 8,
                                   (int)(0x00FF0000U & dword) >> 16);
            }
        }

        public uint Dword
        {
            get { return dword; }
            set { dword = value; }
        }
    }
}