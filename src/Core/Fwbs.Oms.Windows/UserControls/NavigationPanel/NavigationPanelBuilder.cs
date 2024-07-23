using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS;

namespace FWBS.Common.UI.Windows
{
    internal class NavigationPanelBuilder : NavigationBuilder
    {
        private readonly ucNavigationPanel _panel;
        private const string _codeLookupGroupType = "DLGGROUPCAPTION";

        public NavigationPanelBuilder(ucNavigationPanel panel, Dictionary<string, GroupData> groups, List<TabData> tabs) : base(groups, tabs)
        {
            _panel = panel;
        }

        public override void Build(string currentItem)
        {
            _panel.ClearNavigationItems();
            _panel.LoadIcons();
            _panel.SetNavigationIcon();

            FillGroups();

            var pageItemFactory = new PageItemFactory(_panel.Images);
            var groupItemFactory = new GroupItemFactory(_panel.GetGroupItemIcon());

            var groups = Groups.ToList();
            for (int i = groups.Count-1; i >= 0; i--)
            {
                var pageItems = new List<ucNavigationItem>();
                foreach (var item in groups[i].Value.Items)
                {
                    pageItems.Add(pageItemFactory.Create(item.Code, item.Description, item.ImageIndex));
                }

                _panel.AddGroup(groupItemFactory.Create(null, GetGroupHeaderText(groups[i].Key)), pageItems);
            }
            _panel.ScalePanel();

            if (currentItem != null)
            {
                _panel.SelectItem(currentItem);
            }
        }

        private string GetGroupHeaderText(string groupHeaderDescription)
        {
            return Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup(_codeLookupGroupType, groupHeaderDescription), true);
        }

        #region Classes

        private class GroupItemFactory : ILabelFactory
        {
            private Image _image;

            public GroupItemFactory(Image image)
            {
                _image = image;
            }

            public ucNavigationItem Create(string code, string description, int imageIndex = 0)
            {
                return new ucNavigationItem(description, _image);
            }
        }

        private class PageItemFactory : ILabelFactory
        {
            private ImageList _images;

            public PageItemFactory(ImageList images)
            {
                _images = images;
            }

            public ucNavigationItem Create(string code, string description, int imageIndex = 0)
            {
                Image image;
                try
                {
                    image = _images.Images[imageIndex];
                }
                catch
                {
                    image = _images.Images[0];
                }

                return new ucNavigationItem(description, code, image, imageIndex);
            }
        }

        #endregion
    }
}
