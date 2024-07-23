using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public static class ImageListSelector
    {
        public static ImageList GetImageList()
        {
            return FWBS.OMS.UI.Windows.Images.Windows8();
        }
        
        public static omsImageLists GetOMSImageList()
        {
            return omsImageLists.Windows8;
        }
    }
}
