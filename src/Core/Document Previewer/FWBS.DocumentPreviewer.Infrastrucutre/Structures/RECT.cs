// Stephen Toub

using System.Drawing;
using System.Runtime.InteropServices;

namespace Fwbs.Documents.Preview.Handlers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public RECT(Rectangle rect)
        {
            left = rect.Left;
            top = rect.Top;
            right = rect.Right;
            bottom = rect.Bottom;
        }

        public int left;
        public int top;
        public int right;
        public int bottom;

        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(left, top, right, bottom);
        }
    }
}