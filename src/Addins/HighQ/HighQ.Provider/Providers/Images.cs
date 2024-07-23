using System.Drawing;

namespace FWBS.OMS.HighQ.Providers
{
    internal class Images
    {
        private static System.Reflection.Assembly _currentAssembly;
        private static System.Reflection.Assembly CurrentAssembly
        {
            get
            {
                if (_currentAssembly == null)
                    _currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                return _currentAssembly;
            }
        }

        public Image GetFolderImage(int dpi)
        {
            return GetIcon("folder", GetIconSize(dpi), "FWBS.OMS.HighQ.Resources").ToBitmap();
        }

        public Image GetOpenedFolderImage(int dpi)
        {
            return GetIcon("folder_opened", GetIconSize(dpi), "FWBS.OMS.HighQ.Resources").ToBitmap();
        }

        public Image GetClosedFolderImage(int dpi)
        {
            return GetIcon("folder_closed", GetIconSize(dpi), "FWBS.OMS.HighQ.Resources").ToBitmap();
        }

        private Icon GetIcon(string iconId, IconSize size, string path, string extension = "ICO")
        {
            using (var stream = CurrentAssembly.GetManifestResourceStream($"{path}.{iconId}.{extension}"))
            {
                return stream != null
                    ? new Icon(stream, new Size((int)size, (int)size))
                    : null;
            }
        }

        private IconSize GetIconSize(int dpi)
        {
            switch (dpi)
            {
                case 96:
                    return IconSize.Size16;
                case 120:
                    return IconSize.Size20;
                case 144:
                    return IconSize.Size24;
                case 168:
                    return IconSize.Size28;
                case 192:
                    return IconSize.Size32;
                default:
                    return IconSize.Size16;
            }
        }

        private enum IconSize : int
        {
            Size16 = 16,
            Size20 = 20,
            Size24 = 24,
            Size28 = 28,
            Size32 = 32
        }
    }
}
