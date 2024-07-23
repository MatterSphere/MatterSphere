using System;
using System.Collections.ObjectModel;

namespace Telerik.WinControls.UI
{
    class ImageList
    {
        internal readonly object[] Images = new object[1];
    }

    public class RadTreeView
    {
        internal RadTreeView()
        {
            Nodes = new RadTreeNodeCollection();
            ImageList = new ImageList();
        }
        internal bool EnableTheming { get; set; }
        internal string ThemeName { get; set; }
        internal ImageList ImageList { get; private set; }
        internal RadTreeNodeCollection Nodes { get; private set; }
        internal void BeginUpdate() { }
        internal void EndUpdate() { }
    }

    public class RadTreeNode
    {
        internal RadTreeNode(string text = null)
        {
            Nodes = new RadTreeNodeCollection() { Owner = this };
            Text = text;
        }
        internal string Name { get; set; }
        internal string Text { get; set; }
        internal object Tag { get; set; }
        internal object Image { get; set; }
        internal CheckType CheckType { get; set; }
        internal RadTreeNode Parent { get; set; }
        internal RadTreeNodeCollection Nodes { get; private set; }
    }

    public class RadTreeNodeCollection : Collection<RadTreeNode>
    {
        internal RadTreeNode Owner { get; set; }

        internal new void Add(RadTreeNode item)
        {
            item.Parent = Owner;
            base.Add(item);
        }

        internal RadTreeNode Add(string text)
        {
            RadTreeNode node = new RadTreeNode(text);
            Add(node);
            return node;
        }
    }

    enum CheckType
    {
        None = 0, CheckBox = 1
    }
}

namespace FWBS.OMS.UI.Windows
{
    namespace TreeViewNavigation
    {
        using Telerik.WinControls.UI;

        static class TreeViewFormatter
        {
            internal static RadTreeNode NewTreeNode()
            {
                return new RadTreeNode();
            }
        }
    }

    static class ErrorBox
    {
        internal static void Show(Exception ex)
        {
            OMSEXPORT.StaticLibrary.LogErrorMessage(ex.Message);
        }
    }
}