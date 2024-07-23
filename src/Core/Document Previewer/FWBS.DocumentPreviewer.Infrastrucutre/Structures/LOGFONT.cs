// Stephen Toub

using System.Runtime.InteropServices;

namespace Fwbs.Documents.Preview.Handlers
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LOGFONT
    {
        public byte lfCharSet;
        public byte lfClipPrecision;
        public int lfEscapement;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string lfFaceName = string.Empty;

        public int lfHeight;

        public byte lfItalic;
        public int lfOrientation;
        public byte lfOutPrecision;
        public byte lfPitchAndFamily;
        public byte lfQuality;
        public byte lfStrikeOut;
        public byte lfUnderline;
        public int lfWeight;
        public int lfWidth;
    }
}