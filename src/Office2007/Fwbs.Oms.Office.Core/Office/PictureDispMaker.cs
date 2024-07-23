using System.Windows.Forms;

namespace Fwbs.Office
{
    internal sealed class PictureDispMaker : AxHost
    {
        public PictureDispMaker()
            : base("")
        {
        }

        public static System.Drawing.Image ConvertToImage(stdole.IPictureDisp pic)
        {
            return (System.Drawing.Image)GetPictureFromIPicture(pic);
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
