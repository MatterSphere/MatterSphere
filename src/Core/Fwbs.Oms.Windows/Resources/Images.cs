using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class Images
    {
        #region enums
        public enum IconSize : int
        {
            SizeAll = 0,
            Size16 = 16,
            Size24 = 24,
            Size32 = 32,
            Size40 = 40,
            Size48 = 48,
            Size56 = 56,
            Size64 = 64,
            Size72 = 72,
            Size128 = 128,
            Size256 = 256
        }

		public enum imgCoolButtons{None = -1,New,Open,Save,Cut,Copy,Paste,Delete,OK,Undo,Redo,Find,Print,Find2,FindNext,Help,ZoomIn,ZoomOut,Back,Foreward,Favorites,AddFavorites,Stop,Refresh,Home,Edit,Tools,LargeIcons,ListIcons,Font,Details,FolderView,Media,Foreign,Reset,Mail,Parent,FolderOK,Cog,Up,Down,LogOff,Rename,NewFolder,Currency,Company,Couple,Deceased,Four,Individual,LargeGoverment,Minor,SmallGoverment,SoleTrader,Three}
		public enum imgEntities{None = -1,Default=0,Company=1,Couple=2,Deceased=3,Four=4,Individual=5,LargeGoverment=6,Minor=7,SmallGoverment=8,SoleTrader=9,Three=10}
        #endregion

        public static ImageList Arrows
        {
            get
            {
                return CloneList(Crownwood.Magic.Common.ResourceHelper.LoadBitmapStrip(typeof(FWBS.OMS.UI.Windows.ucPanelNav),
			    "FWBS.OMS.UI.Resources.Arrows.bmp",
			    new Size(16, 16),
			    new Point(0,0)),"Arrows");
            }
        }

        public static ImageList ArrowsSel
        {
            get
            {
                return CloneList(Crownwood.Magic.Common.ResourceHelper.LoadBitmapStrip(typeof(FWBS.OMS.UI.Windows.ucPanelNav),
                "FWBS.OMS.UI.Resources.Arrows.bmp",
                new Size(16, 16),
                new Point(0, 0)), "ArrowsSel");
            }
        }

        public static ImageList PlusMinus
        {
            get
            {
                return CloneList(Crownwood.Magic.Common.ResourceHelper.LoadBitmapStrip(typeof(FWBS.OMS.UI.Windows.ucPanelNav),
                "FWBS.OMS.UI.Resources.PlusMinus.bmp",
                new Size(16, 16),
                new Point(0, 0)), "PlusMinus");
            }
        }

        public static ImageList PlusMinusSel
        {
            get
            {
                return CloneList(Crownwood.Magic.Common.ResourceHelper.LoadBitmapStrip(typeof(FWBS.OMS.UI.Windows.ucPanelNav),
                "FWBS.OMS.UI.Resources.Arrows.bmp",
                new Size(16, 16),
                new Point(0, 0)), "PlusMinusSel");
            }
        }

		private static ImageList _whiteicons = null;
		public static ImageList WhiteIcons()
		{
			string icon = "";
			if (_whiteicons == null)
			{
				_whiteicons = new ImageList();
				_whiteicons.ImageSize = new Size(16,16);
				icon = "FWBS.OMS.UI.Resources.WhiteIcons.Close.ico";
                _whiteicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
				icon = "FWBS.OMS.UI.Resources.WhiteIcons.Expand.ico";
                _whiteicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
				icon = "FWBS.OMS.UI.Resources.WhiteIcons.Shrink.ico";
                _whiteicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
			}
            return CloneList(_whiteicons, "White Icons");
		}

        private static ImageList _blackicons = null;
        public static ImageList BlackIcons()
        {
            string icon = "";
            if (_blackicons == null)
            {
                _blackicons = new ImageList();
                _blackicons.ImageSize = new Size(16, 16);
                icon = "FWBS.OMS.UI.Resources.BlackIcons.Close.ico";
                _blackicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
                icon = "FWBS.OMS.UI.Resources.BlackIcons.Expand.ico";
                _blackicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
                icon = "FWBS.OMS.UI.Resources.BlackIcons.Shrink.ico";
                _blackicons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
            }
            return CloneList(_blackicons, "Black Icons");
        }

        private static ImageList _treeViewNavigationIcons = null;
        public static ImageList TreeViewNavigationIcons()
        {
            string icon = "";
            if (_treeViewNavigationIcons == null)
            {
                _treeViewNavigationIcons = new ImageList();
                _treeViewNavigationIcons.ImageSize = new Size(16, 16);
                icon = "FWBS.OMS.UI.Resources.TreeNavigationIcons.ico_burgermenu.ico";
                _treeViewNavigationIcons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
                icon = "FWBS.OMS.UI.Resources.TreeNavigationIcons.ico_information.ico";
                _treeViewNavigationIcons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
                icon = "FWBS.OMS.UI.Resources.TreeNavigationIcons.ico_information_right.ico";
                _treeViewNavigationIcons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, new Size(16, 16)));
            }
            return CloneList(_treeViewNavigationIcons, "Tree View Navigation Icons");
        }

        private static ImageList _windows8 = null;
        public static ImageList Windows8()
        {
            if (_windows8 == null)
            {
                _windows8 = new ImageList();
                _windows8.ImageSize = new Size(1, 1);
            }
            return CloneList(_windows8, "Windows 8 Icons");
        }

        public static Image GetResetIcon(int deviceDpi)
        {
            switch (FWBS.OMS.UI.Windows.Images.GetStandardDpi(deviceDpi))
            {
                case 96:
                    return Properties.Resources.titlebar_search_close_96x;
                case 120:
                    return Properties.Resources.titlebar_search_close_120x;
                case 144:
                    return Properties.Resources.titlebar_search_close_144x;
                case 168:
                    return Properties.Resources.titlebar_search_close_168x;
                default:
                    return Properties.Resources.titlebar_search_close_192x;
            }
        }

        public static Image GetSearchIcon(int deviceDpi)
        {
            switch (FWBS.OMS.UI.Windows.Images.GetStandardDpi(deviceDpi))
            {
                case 96:
                    return Properties.Resources.titlebar_search_96x;
                case 120:
                    return Properties.Resources.titlebar_search_120x;
                case 144:
                    return Properties.Resources.titlebar_search_144x;
                case 168:
                    return Properties.Resources.titlebar_search_168x;
                default:
                    return Properties.Resources.titlebar_search_192x;
            }
        }

        #region FolderForms
        [Obsolete("Please use GetFolderFormsIcons(IconSize.Size16)")]
        public static ImageList imgFolderForms16
        {
            get
            {
                return GetFolderFormsIcons(IconSize.Size16);
            }
        }

        [Obsolete("Please use GetFolderFormsIcons(IconSize.Size32)")]
        public static ImageList imgFolderForms32
        {
            get
            {
                return GetFolderFormsIcons(IconSize.Size32);
            }
        }

        private const int folderFormsLength = 2;
        private const string folderFormsPrefix = "FolderForms";
        private const string folderFormsResourceLocation = "FWBS.OMS.UI.Resources.FolderForms";
        public static ImageList GetFolderFormsIcons(IconSize size)
        {
            return GetList(size, folderFormsLength, folderFormsPrefix, folderFormsResourceLocation);
        }

        #endregion

        #region RichText Icons

        [Obsolete("Please Use GetRichTextIcons(IconSize.Size16)")]
        public static ImageList RichText
        {
            get
            {
                return GetRichTextIcons(IconSize.Size16);

            }
        }

        private const int richTextLength = 6;
        private const string richTextPrefix = "RichText";
        private const string richTextResourceLocation = "FWBS.OMS.UI.Resources.RichText";
        public static ImageList GetRichTextIcons(IconSize size)
        {
            return GetList(size, richTextLength, richTextPrefix, richTextResourceLocation);
        }

        #endregion

        #region Individual Images

        public static Image Fax
        {
            get
            {
                return GetCoolButton(85, IconSize.Size48).ToBitmap();
            }
        }

        public static Image Print
        {
            get
            {
                return GetCoolButton(11, IconSize.Size32).ToBitmap();
            }
        }

        public static Image Lock
        {
            get
            {
                return GetCoolButton(86, IconSize.Size48).ToBitmap();
            }
        }


        #endregion

        #region Cool Buttons

        [Obsolete("Please use the GetCoolButtons24() method")]
        public static ImageList CoolButtons24
        {
            get
            {
                return FWBS.OMS.UI.Windows.Images.CloneList(GetCoolButtons24(), "CoolButtons24");
            }
        }

        private const int coolButtonsCount = 117;
        private const string coolButtonsPrefix = "Cool Buttons";
        private const string CoolButtonResourceLocation = "FWBS.OMS.UI.Resources.CoolButtons.V2";
        public static ImageList GetCoolButtonsList(IconSize size)
        {
            return GetList(size, coolButtonsCount, coolButtonsPrefix, CoolButtonResourceLocation);
        }

        public static Icon GetCoolButton(int index, IconSize size)
        {
            var indexCode = Convert.ToString(index);
            indexCode = indexCode.PadLeft(2, '0');

            return GetIcon(indexCode, size, CoolButtonResourceLocation);
        }

        public static Icon GetCoolButton(int index, IconSize startSize, int dpi)
        {
            var size = GetIconSize(dpi, (int) startSize);
            return GetCoolButton(index, size);
        }

        public static ImageList GetCoolButtons24()
        {
            return GetCoolButtonsList(IconSize.Size24);
        }

		public static ImageList CoolButtons16()
		{
            return GetCoolButtonsList(IconSize.Size16);
		}

        #endregion


        #region Common Icons

        private const int commonIconsCount = 26;
        private const string commonIconsPrefix = "Common Icons";
        private const string commonIconsResourceLocation = "FWBS.OMS.UI.Resources.Common";

        private static readonly string[] CommonIconKeys = new string[] 
        {
            "refresh", "address", "appointment", "client", "contact", "filter", "file", "note", "preview", "task",
            "filter_white", "close_white", "associate", "columnsettings", "grey_plus", "full_screen",
            "grey_magnifier", "upload", "upload_disabled", "upload_selected", "calculator", "pop_over",
            "filter_black", "close", "calendar", "add", "edit", "delete", "delete_disabled", "delete_selected",
            "archive", "complaint", "document", "keydate", "precedent", "timeentry", "undertaking", "default"
        };

        public static ImageList GetCommonIcons(int dpi, bool withKeys, int startSize = 32)
        {
            var size = GetIconSize(dpi, startSize);

            if (withKeys)
            {
                return GetList(size, CommonIconKeys, commonIconsPrefix, commonIconsResourceLocation);
            }

            return GetList(size, commonIconsCount, commonIconsPrefix, commonIconsResourceLocation);
        }

        public static Image GetCommonIcon(int dpi, string key)
        {
            var size = GetIconSize(dpi);
            return GetIcon(size, key, commonIconsPrefix, commonIconsResourceLocation);
        }

        #endregion

        #region Dialog Icons

        private const int lastDialogIcon = 14;
        private const string dialogIconsPrefix = "Dialog Icons";
        private const string dialogIconsResourceLocation = "FWBS.OMS.UI.Resources.DialogIcons";

        public enum DialogIcons : int
        {
            Default = 0, Appointment, Archive, Associate, Client, Complaint, Contact, Document, File, KeyDate,
            Precedent, Task, TimeEntry, Undertaking, EliteApp
        }

        public static ImageList GetDialogIconsBySize(IconSize size)
        {
            return GetList(size, lastDialogIcon, dialogIconsPrefix, dialogIconsResourceLocation);
        }

        public static Icon GetDialogIcon(string key)
        {
            return GetIcon(key, IconSize.SizeAll, dialogIconsResourceLocation);
        }

        public static Icon GetDialogIcon(DialogIcons dialogIcon)
        {
            return GetDialogIcon((int)dialogIcon);
        }

        public static Icon GetDialogIcon(int index)
        {
            return GetIcon($"{index:00}", IconSize.SizeAll, dialogIconsResourceLocation);
        }

        #endregion

        #region New Entity Images

        private const string newEntityImagesResourceLocation = "FWBS.OMS.UI.Resources.NewEntities";

        public static Metafile GetNewEntityImage(string name)
        {
            Metafile metafile = null;
            string streamName = string.Format("{0}.{1}.emf", newEntityImagesResourceLocation, name);
            using (var stream = CurrentAssembly.GetManifestResourceStream(streamName))
            {
                if (stream != null)
                    metafile = new Metafile(stream);
            }
            return metafile;
        }

        #endregion

        #region Navigation Icons
        private const int navigationIconsCount = 39;
        private const string navigationIconsPrefix = "Navigation Panel Icons";
        private const string navigationIconsResourceLocation = "FWBS.OMS.UI.Resources.NavigationIcons";

        public static ImageList GetNavigationIcons(int dpi)
        {
            var size = GetIconSize(dpi);
            return GetList(size, navigationIconsCount, navigationIconsPrefix, navigationIconsResourceLocation);
        }

        public static ImageList GetNavigationIconsBySize(IconSize size)
        {
            return GetList(IconSize.Size16, navigationIconsCount, navigationIconsPrefix, navigationIconsResourceLocation);

        }

        public static Image GetNavigationIcon(int dpi)
        {
            return GetTreeViewNavigationIcons(dpi, 0);
        }

        public static Image GetNavigationGroupItemIcon(int dpi)
        {
            return GetTreeViewNavigationIcons(dpi, 1);
        }

        private static Image GetTreeViewNavigationIcons(int dpi, int index)
        {
            var size = GetIconSize(dpi);
            var imageList = GetTreeViewNavigationIcons(size);

            return imageList.Images[index];
        }

        private static ImageList GetTreeViewNavigationIcons(IconSize iconSize)
        {
            var size = new Size((int)iconSize, (int)iconSize);
            var navigationIcons = new ImageList
            {
                ImageSize = size
            };

            var icon = "FWBS.OMS.UI.Resources.TreeNavigationIcons.ico_navmenu.ico";
            navigationIcons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, size));
            icon = "FWBS.OMS.UI.Resources.TreeNavigationIcons.ico_groupitem.ico";
            navigationIcons.Images.Add(Crownwood.Magic.Common.ResourceHelper.LoadIcon(typeof(FWBS.OMS.UI.Windows.Images), icon, size));

            return navigationIcons;
        }

        private static IconSize GetIconSize(int dpi)
        {
            double k = (double)dpi / 96;
            if (k <= 1)
            {
                return IconSize.Size32;
            }

            if (k > 1 && k <= 1.25)
            {
                return IconSize.Size40;
            }

            if (k > 1.25 && k <= 1.5)
            {
                return IconSize.Size48;
            }

            if (k > 1.5 && k <= 1.75)
            {
                return IconSize.Size56;
            }

            return IconSize.Size64;
        }

        public static IconSize GetIconSize(int dpi, int startSize)
        {
            double k = (double)dpi / 96;
            if (k <= 1)
            {
                return (IconSize)startSize;
            }

            if (k > 1 && k <= 1.25)
            {
                return GetIconSize(startSize * 1.25);
            }

            if (k > 1.25 && k <= 1.5)
            {
                return GetIconSize(startSize * 1.5);
            }

            if (k > 1.5 && k <= 1.75)
            {
                return GetIconSize(startSize * 1.75);
            }

            return GetIconSize(startSize * 2);
        }

        private static IconSize GetIconSize(double size)
        {
            var values = new List<int>();
            foreach (int iconSize in Enum.GetValues(typeof(IconSize)))
            {
                values.Add(iconSize);
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] > size)
                {
                    return i == 0
                        ? (IconSize) values[i]
                        : (IconSize) values[i - 1];
                }
            }

            return (IconSize) values[values.Count - 1];
        }

        #endregion

        #region Development

        private const int developmentListLength = 15;
        private const string developmentName = "Development";
        private const string developmentResourcePath = "FWBS.OMS.UI.Resources.Developments.V2";
        public static ImageList GetDevelopmentList(IconSize size)
        {
            return GetList(size, developmentListLength, developmentName, developmentResourcePath);
        }

        public static Icon GetDevelopmentIcon(int index, IconSize size)
        {
            var indexCode = Convert.ToString(index);
            indexCode = indexCode.PadLeft(2, '0');

            return GetIcon(indexCode, size, developmentResourcePath);
        }

        public static ImageList Developments()
        {
            return GetDevelopmentList(IconSize.Size16);
        }

        public static ImageList Developments32()
        {
            return GetDevelopmentList(IconSize.Size32);
        }

        #endregion
		
        #region AdminMenu
        private const int adminMenuCount =53;
        private const string adminMenuPrefix = "AdminMenu";
        private const string adminMenuResourceLocation = "FWBS.OMS.UI.Resources.AdminMenu.V2";
        public static ImageList GetAdminMenuList(IconSize size)
        {
            return GetList(size, adminMenuCount, adminMenuPrefix, adminMenuResourceLocation);
        }

        public static Icon GetAdminMenuItem(int index, IconSize size)
        {
            var indexCode = Convert.ToString(index);
            indexCode = indexCode.PadLeft(2, '0');

            return GetIcon(indexCode, size, adminMenuResourceLocation);
        }

        public static ImageList AdminMenu48()
        {
            return GetAdminMenuList(IconSize.Size48);
        }

		public static ImageList AdminMenu32()
		{
            return GetAdminMenuList(IconSize.Size32);
		}

        public static ImageList AdminMenu24()
        {
            return GetAdminMenuList(IconSize.Size24);
        }

        public static ImageList AdminMenu16()
        {
            return GetAdminMenuList(IconSize.Size16);
        }
        #endregion

        #region Entities

		public static ImageList Entities()
		{
            return GetEntitiesList(IconSize.Size16);
		}

        public static ImageList Entities32()
        {
            return GetEntitiesList(IconSize.Size32);
        }

        private static Dictionary<IconSize, ImageList> entitiesList = new Dictionary<IconSize, ImageList>();
        public static ImageList GetEntitiesList(IconSize size)
        {
            if (!entitiesList.ContainsKey(size))
            {
                ImageList l = null;
                GetEntitiesList(size, ref l);

                entitiesList.Add(size, l);

            }

            return CloneList(entitiesList[size], "Entities "+ (int)size);
        }

        private const int lastEntityImage = 63;
        private static void GetEntitiesList(IconSize size ,ref ImageList imageList)
        {
            if (imageList == null)
            {
                imageList = new ImageList();
                imageList.ColorDepth = ColorDepth.Depth32Bit;
                imageList.ImageSize = new Size((int)size, (int)size);
            }

            for (int i = 0; i <=10; i++)
                imageList.Images.Add(GetEntityIconNoTransform(i, size));

            imageList.Images.Add(GetEntityIcon("Users", size));
            for (int i = 13; i <= lastEntityImage; i++)
                imageList.Images.Add(GetEntityIconNoTransform(i, size));

        }


        public static Icon GetEntityIcon(int index)
		{
			return GetEntityIcon(index, false);
		}

		public static Icon GetEntityIcon (int index, bool large)
		{
            if (large)
            {
                return GetEntityIcon(index, IconSize.Size32);
            }
            else
                return GetEntityIcon(index, IconSize.Size16);

		}

        public static Icon GetEntityIcon(int index, IconSize size)
        {
            var imageName = Convert.ToString(index);

            if (index == 11)
                imageName = "Users";
            else if (index > 11)
                imageName = Convert.ToString(++index);

            return GetEntityIcon(imageName, size);
        }

        

        private static Icon GetEntityIconNoTransform(int index, IconSize size)
        {
            return GetEntityIcon(Convert.ToString(index), size);
        }


        private const string EntityResourceLocation = "FWBS.OMS.UI.Resources.Entities.V2";
        public static Icon GetEntityIcon(string name, IconSize size)
        {
            return GetIcon(name, size, EntityResourceLocation, "ico");
        }
        #endregion

        #region Generic
        private static Dictionary<string, ImageList> systemLists = new Dictionary<string, ImageList>();
        private static ImageList GetList(IconSize size, int iconCount, string namePrefix, string resPath)
        {
            string name = string.Format("{0} {1}", namePrefix, (int)size);

            if (!systemLists.ContainsKey(name))
            {
                var images = new ImageList();
                images.ColorDepth = ColorDepth.Depth32Bit;
                images.ImageSize = new Size((int)size, (int)size);
                for (int i = 0; i <= iconCount; i++)
                {
                    var indexCode = Convert.ToString(i);
                    indexCode = indexCode.PadLeft(2, '0');

                    var icon = GetIcon(indexCode, size, resPath);
                    if (icon == null)
                    {
                        icon = CreateEmptyIcon(size);
                    }

                    images.Images.Add(icon);
                }

                systemLists.Add(name, images);
            }

            var cl =  CloneList(systemLists[name], name);
            cl.Tag = name;
            return cl;
        }

        private static ImageList GetList(IconSize size, string[] keys, string namePrefix, string resPath)
        {
            string name = $"{namePrefix} {(int) size}";

            if (!systemLists.ContainsKey(name))
            {
                var images = new ImageList();
                images.ColorDepth = ColorDepth.Depth32Bit;
                images.ImageSize = new Size((int)size, (int)size);
                foreach (var key in keys)
                {
                    var icon = GetIcon(key, size, resPath);
                    if (icon == null)
                    {
                        icon = CreateEmptyIcon(size);
                    }

                    images.Images.Add(key, icon);
                }

                systemLists.Add(name, images);
            }

            var cl = CloneListWithKeys(systemLists[name]);
            cl.Tag = name;

            return cl;
        }

        private static Image GetIcon(IconSize size, string key, string namePrefix, string resPath)
        {
            string name = $"{namePrefix} {(int)size}";

            if (!systemLists.ContainsKey(name))
            {
                var icon = GetIcon(key, size, resPath);
                if (icon == null)
                {
                    icon = CreateEmptyIcon(size);
                }

                return icon.ToBitmap();
            }
            
            return systemLists[name].Images[key];
        }

        public static ImageList CloneListWithKeys(ImageList images)
        {
            ImageList clonedList = new ImageList();
            clonedList.ColorDepth = images.ColorDepth;
            clonedList.ImageSize = images.ImageSize;

            foreach (var key in images.Images.Keys)
            {
                if (images.Images.ContainsKey(key))
                {
                    clonedList.Images.Add(key, images.Images[key]);
                }
            }

            return clonedList;
        }

        private static Icon CreateEmptyIcon(IconSize size)
        {
            var bitmap = new Bitmap((int)size, (int)size);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillEllipse(Brushes.White, 0, 0, (int)size, (int)size);
            }

            return Icon.FromHandle(bitmap.GetHicon());
        }

        private static Icon GetIcon(string iconId, IconSize size, string path, string extension = "ICO")
        {
            return GetIcon(string.Format("{0}.{1}.{2}", path, iconId, extension), size);
        }

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

        private static Icon GetIcon(string iconResourceLocation, IconSize size)
        {
            using (var stream = CurrentAssembly.GetManifestResourceStream(iconResourceLocation))
            {
                if (stream == null) return null;

                if (size == IconSize.SizeAll)
                    return new Icon(stream);

                return new Icon(stream, new Size((int) size, (int) size));
            }
        }

        // Not the closest grayscale representation in the RGB space, but pretty close.
        // Closest would be the cubic root of the product of the RGB colors,
        // but that cannot be represented in ColorMatrix.
        public static void DrawGrayedImage(Graphics g, Image image, int left, int top)
        {
            DrawGrayedImage(g, image, left, top, image.Width, image.Height);
        }

        public static void DrawGrayedImage(Graphics g, Image image, int left, int top, int width, int height)
        {
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cm = new ColorMatrix();

            // 1/3 on the top 3 rows and 3 columns
            cm.Matrix00 = 1 / 3f;
            cm.Matrix01 = 1 / 3f;
            cm.Matrix02 = 1 / 3f;
            cm.Matrix10 = 1 / 3f;
            cm.Matrix11 = 1 / 3f;
            cm.Matrix12 = 1 / 3f;
            cm.Matrix20 = 1 / 3f;
            cm.Matrix21 = 1 / 3f;
            cm.Matrix22 = 1 / 3f;

            ia.SetColorMatrix(cm);

            g.DrawImage(image, new Rectangle(left, top, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, ia);
        }

        public static ImageList CloneList(ImageList images, string Name)
        {
            ImageList n = new ImageList();
            n.ColorDepth = images.ColorDepth;
            n.ImageSize = images.ImageSize;

            foreach (var key in images.Images.Keys)
            {
                if (images.Images.ContainsKey(key))
                {
                    n.Images.Add(key, images.Images[key]);
                }
            }

            foreach (Image i in images.Images)
                n.Images.Add(i);
            return n;
        }

        public static ImageList ScaleList(ImageList imageList, Size imageSize)
        {
            ImageList newList = new ImageList()
            {
                ColorDepth = imageList.ColorDepth,
                ImageSize = imageSize
            };

            for (int i = 0; i < imageList.Images.Count; i++)
            {
                newList.Images.Add(imageList.Images[i]);
                newList.Images.SetKeyName(i, imageList.Images.Keys[i]);
            }

            return newList;
        }

        #endregion

        public static int GetStandardDpi(int deviceDpi)
        {
            if (deviceDpi <= 96)
            {
                return 96;
            }

            if (deviceDpi <= 120)
            {
                return 120;
            }

            if (deviceDpi <= 144)
            {
                return 144;
            }

            if (deviceDpi <= 168)
            {
                return 168;
            }

            return 192;
        }

    }

    public static class ExpandCollapseIconSelector
    {
        /// <summary>
        /// Returns the appropriate ExpandCollapse ImageList based on the current UI version
        /// </summary>
        public static ImageList GetExpandCollapseIcons()
        {
            return FWBS.OMS.UI.Windows.Images.PlusMinus;
        }

        /// <summary>
        /// Returns the appropriate ExpandCollapse Image based on the current UI version 
        /// based on a supplied index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Image GetExpandCollapseIconByIndex(int index)
        {
            return FWBS.OMS.UI.Windows.Images.PlusMinus.Images[index];
        }
    }
}
