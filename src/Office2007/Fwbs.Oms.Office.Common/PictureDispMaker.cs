using System.Windows.Forms;

namespace Fwbs.Oms.Office.Common
{

    internal sealed class PictureDispMaker : AxHost
    {
        public PictureDispMaker()
            : base("")
        {
        }

        public static stdole.IPictureDisp ConvertImage(System.Drawing.Image image)
        {
            return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
        }

        public static stdole.IPictureDisp ConvertImage(System.Drawing.Icon icon)
        {
            return ConvertImage(icon.ToBitmap());
        }

    }
}
