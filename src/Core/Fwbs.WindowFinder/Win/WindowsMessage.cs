namespace Fwbs
{
    namespace WinFinder
    {
        public enum WindowMessage
        {
            Null = 0x0,
            Print = 0x317,
            GetIcon = 0x7F,
            SetIcon = 0x80,
            Paint = 0xF,
            Redraw = 0x000B,
            Close = 0x10,
            Quit = 0x12,
            EndSession = 0x16,
            QueryEndSession = 0x11,
            CopyData = 0x004A,
            QueryDragIcon = 55,
            GetText = 0xD,
            SetText = 0xC,
            Click = 0x00F5,
            SetFocus = 0x7,
            KillFocus = 0x8,
            Command = 0x111,
            Char = 0x102
        }
    }
}