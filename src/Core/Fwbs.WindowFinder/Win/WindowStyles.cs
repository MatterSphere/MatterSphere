using System;

namespace Fwbs
{
    namespace WinFinder
    {
        [Flags]
        public enum WindowStyles : long
        {
            Overlapped = 0x00000000,
            Popup = 0x80000000,
            Child = 0x40000000,
            Minimise = 0x20000000,
            Visible = 0x10000000,
            Disabled = 0x08000000,
            ClipSiblings = 0x04000000,
            ClipChildren = 0x02000000,
            Maximise = 0x01000000,
            Caption = 0x00C00000,     /* WS_BORDER | WS_DLGFRAME  */
            Border = 0x00800000,
            DialogFrame = 0x00400000,
            VScroll = 0x00200000,
            HScroll = 0x00100000,
            SysMenu = 0x00080000,
            ThickFrame = 0x00040000,
            Group = 0x00020000,
            TabStop = 0x00010000,
            MinimiseBox = 0x00020000,
            MaximiseBox = 0x00010000,
            Tiled = Overlapped,
            Iconic = Minimise,
            SizeBox = ThickFrame,
            OverlappedWindow = (Overlapped | Caption | SysMenu | ThickFrame | Minimise | Maximise),
            TileWindow = OverlappedWindow,
            PopupWindow = (Popup | Border | SysMenu),
            ChildWindow = Child

        }
    }
}