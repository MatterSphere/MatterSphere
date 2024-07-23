using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Fwbs.Documents.Preview.Zip
{
    internal class IconImageList : IDisposable
    {
        private ImageList icons;

        public IconImageList()
            : this(new ImageList())
        {
        }

        public IconImageList(ImageList iconslist)
        {
            icons = iconslist;
        }

        public ImageList GetIcons(Size imageSize)
        {
            return (icons.ImageSize == imageSize) ? icons : ScaleList(imageSize);
        }

        public int GetIconIndexForFile(string path)
        {
            int index = -1;

            if (!string.IsNullOrEmpty(path))
            {
                string extension = Path.GetExtension(path);

                if (!icons.Images.ContainsKey(extension))
                {
                    Icon icon = IconReader.GetFileIcon(path, IconReader.IconSize.Small, false);
                    icons.Images.Add(extension, icon.ToBitmap());
                }

                index = icons.Images.IndexOfKey(extension);
            }

            return index;
        }

        private ImageList ScaleList(Size imageSize)
        {
            ImageList newList = new ImageList()
            {
                ColorDepth = icons.ColorDepth,
                ImageSize = imageSize
            };

            for (int i = 0; i < icons.Images.Count; i++)
            {
                newList.Images.Add(icons.Images[i]);
                newList.Images.SetKeyName(i, icons.Images.Keys[i]);
            }

            return newList;
        }

        public void Dispose()
        {
            if (icons != null)
            {
                icons.Dispose();
                icons = null;
            }
        }

    }
}
