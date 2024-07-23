using System;
using System.Linq;
using FWBS.OMS.UI.Windows;

namespace FWBS.MatterCentre.Controls
{
    public class MCImageListConverter : System.Windows.Data.IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int pos = System.Convert.ToInt32(value);
			if (pos < 0)
				return null;


			System.Drawing.Image image = null;
			var libraryName = System.Convert.ToString(parameter);

			if (!string.IsNullOrWhiteSpace(libraryName) && (libraryName.Contains(' ') || libraryName.Contains(';')))
			{
				image = GetImage(libraryName, pos);
			}
			else
			{
				System.Windows.Forms.ImageList buttons = GetImageList(parameter);

				image = buttons.Images[pos];
			}

			if (image == null)
				return null;

			System.Windows.Media.Imaging.BitmapImage bmp = null;
			System.IO.MemoryStream ms = null;
			try
			{
				bmp = new System.Windows.Media.Imaging.BitmapImage();
				bmp.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
				bmp.BeginInit();

				ms = new System.IO.MemoryStream();

				// Save to a memory stream...

				image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

				// Rewind the stream...

				ms.Seek(0, System.IO.SeekOrigin.Begin);

				// Tell the WPF image to use this stream...

				bmp.StreamSource = ms;

				bmp.EndInit();
			}
			finally
			{
				if (ms != null)
				{
					ms = null;
				}
			}

			return bmp;
		}

		private static System.Drawing.Image GetImage(string libraryName, int imageIndex)
		{
			System.Drawing.Image image = null;
			var parts = libraryName.Split(' ', ';');

			var size = (Images.IconSize)Enum.Parse(typeof(Images.IconSize), "Size" + parts[1]);

			switch (parts[0].ToUpperInvariant())
			{
				case "ADMIN":
				case "ADMINMENU":
					{
						image = FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(imageIndex, size).ToBitmap();
						break;
					}
				case "COOL":
				case "BUTTONS":
				case "COOLBUTTON":
				case "COOLBUTTONS":
					{
						image = FWBS.OMS.UI.Windows.Images.GetCoolButton(imageIndex, size).ToBitmap();
						break;
					}
				case "DEVELOPMENT":
				case "DEVELOPMENTS":
					{
						image = FWBS.OMS.UI.Windows.Images.GetDevelopmentIcon(imageIndex, size).ToBitmap();
						break;
					}
				case "ENTITIY":
				case "ENTITIES":
					{
						image = FWBS.OMS.UI.Windows.Images.GetEntityIcon(imageIndex, size).ToBitmap();
						break;
					}

			   
			}

			return image;
		}

		private static System.Windows.Forms.ImageList GetImageList(object parameter)
		{
			string libraryName = parameter as string;

			System.Windows.Forms.ImageList buttons = null;

			if (string.IsNullOrWhiteSpace(libraryName))
			{
				buttons = FWBS.OMS.UI.Windows.Images.CoolButtons16();
			}
			else
			{
				

				libraryName = libraryName.ToUpperInvariant();



				switch (libraryName)
				{
					case "ADMINMENU16":
						buttons = FWBS.OMS.UI.Windows.Images.AdminMenu16();
						break;
					case "ADMINMENU32":
						buttons = FWBS.OMS.UI.Windows.Images.AdminMenu32();
						break;
					case "ADMINMENU48":
						buttons = FWBS.OMS.UI.Windows.Images.AdminMenu48();
						break;
					case "COOLBUTTONS16":
						buttons = FWBS.OMS.UI.Windows.Images.CoolButtons16();
						break;
					case "COOLBUTTONS24":
						buttons = FWBS.OMS.UI.Windows.Images.GetCoolButtons24();
						break;
					case "DEVELOPMENTS":
					case "DEVELOPMENTS16":
						buttons = FWBS.OMS.UI.Windows.Images.Developments();
						break;
					case "DEVELOPMENTS32":
						buttons = FWBS.OMS.UI.Windows.Images.Developments32();
						break;
					case "ENTITIES":
					case "ENTITIES16":
						buttons = FWBS.OMS.UI.Windows.Images.Entities();
						break;
					case "ENTITIES32":
						buttons = FWBS.OMS.UI.Windows.Images.Entities32();
						break;
					case "IMGFOLDERFORMS16":
						buttons = FWBS.OMS.UI.Windows.Images.GetFolderFormsIcons(Images.IconSize.Size16);
						break;
					case "IMGFOLDERFORMS32":
						buttons = FWBS.OMS.UI.Windows.Images.GetFolderFormsIcons(Images.IconSize.Size32);
						break;
					case "RICHTEXT":
						buttons = FWBS.OMS.UI.Windows.Images.GetRichTextIcons(Images.IconSize.Size24);
						break;
					case "WHITEICONS":
						buttons = FWBS.OMS.UI.Windows.Images.WhiteIcons();
						break;
					default:
						buttons = FWBS.OMS.UI.Windows.Images.CoolButtons16();
						break;
				}
			}
			return buttons;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
	
}
