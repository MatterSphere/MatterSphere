using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class ImageListProvider
    {
        public ImageList BuildImageList(List<string> extensions, int deviceDpi)
        {
            var imageList = FWBS.OMS.UI.Windows.Images.GetCommonIcons(deviceDpi, true, 16);
            var imageCollection = new ImageList
            {
                ImageSize = imageList.ImageSize
            };

            imageCollection.Images.Add("CLIENT", imageList.Images["client"]);
            imageCollection.Images.Add("FILE", imageList.Images["file"]);

            foreach (var extension in extensions)
            {
                Image icon = FWBS.Common.IconReader.GetFileIcon($"12345.{extension}", FWBS.Common.IconReader.IconSize.Small, false).ToBitmap();
                imageCollection.Images.Add(extension, icon);
            }

            return imageCollection;
        }
    }
}
