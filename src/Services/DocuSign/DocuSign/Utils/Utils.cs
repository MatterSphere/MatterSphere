using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FWBS.OMS.DocuSign
{
    public static class Utils
    {
        public static string DefaultRedirectUrl
        {
            get { return "http://localhost/DocuSign"; }
        }

        private static ImageList _recipientsImageList;

        public static ImageList RecipientsImageList
        {
            get
            {
                if (_recipientsImageList == null)
                {
                    _recipientsImageList = new ImageList();
                    _recipientsImageList.ColorDepth = ColorDepth.Depth8Bit;
                    _recipientsImageList.ImageSize = new Size(16, 16);
                    _recipientsImageList.TransparentColor = Color.White;

                    using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FWBS.OMS.Properties.RecipientTypes.bmp"))
                    {
                        _recipientsImageList.Images.AddStrip(Image.FromStream(resourceStream));
                    }

                    foreach (RecipientType recipientType in (RecipientType[])Enum.GetValues(typeof(RecipientType)))
                    {
                        _recipientsImageList.Images.SetKeyName((int)recipientType, recipientType.ToString());
                    }
                }
                return _recipientsImageList;
            }
        }

        public static string FormatByteSize(long fileSize)
        {
            StringBuilder sb = new StringBuilder(12);
            StrFormatByteSize(fileSize, sb, sb.Capacity);
            return sb.ToString();
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern UIntPtr StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);
    }
}
