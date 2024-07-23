using System;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    class ImageListConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var coolButtons = FWBS.OMS.UI.Windows.Images.CoolButtons16();

            int pos = (int)value;
            if (pos < 0)
                return null;

            var image = coolButtons.Images[pos];

            var bmp = new System.Windows.Media.Imaging.BitmapImage();
            bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bmp.BeginInit();

            var ms = new System.IO.MemoryStream();

                // Save to a memory stream...

                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                // Rewind the stream...

                ms.Seek(0, System.IO.SeekOrigin.Begin);

                // Tell the WPF image to use this stream...

                bmp.StreamSource = ms;

                bmp.EndInit();


                return bmp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
