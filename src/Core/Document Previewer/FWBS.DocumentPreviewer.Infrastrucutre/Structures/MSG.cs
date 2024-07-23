// Stephen Toub

using System;
using System.Runtime.InteropServices;

namespace Fwbs.Documents.Preview.Handlers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    }
}