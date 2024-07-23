using System;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.UserControls.Breadcrumbs
{
    internal class BreadCrumbItem
    {
        private BreadCrumbItem(string title, ViewEnum viewType, bool isRoot)
        {
            Key = Guid.NewGuid();
            Title = title;
            ViewType = viewType;
            IsRootItem = isRoot;
        }

        private BreadCrumbItem(IDisplay display, TabPage page, ViewEnum viewType) : this(page?.Text, viewType, false)
        {
            Display = display;
            Page = page;
        }

        public Guid Key { get; }
        public string Title { get; set; }
        public TabPage Page { get; }
        public BreadCrumbItem NextItem { get; set; }
        public ViewEnum ViewType { get; }
        public bool IsRootItem { get; }
        public IDisplay Display { get; }

        public bool IsLastItem
        {
            get { return NextItem == null; }
        }

        public static BreadCrumbItem CreateRootItem(string title, ViewEnum viewType)
        {
            return new BreadCrumbItem(title, viewType, true);
        }

        public static BreadCrumbItem CreateSearchResultsItem(string title)
        {
            return new BreadCrumbItem(title, ViewEnum.ElasticSearch, false);
        }

        public static BreadCrumbItem CreateBreadCrumbItem(ucOMSTypeDisplayV2 display, TabPage page, ViewEnum viewType)
        {
            return new BreadCrumbItem(display, page, viewType);
        }
    }
}
