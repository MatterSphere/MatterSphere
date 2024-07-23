using System.Drawing;
using Telerik.WinControls.UI;

// NOTE: classes below are not longer used in Core, however some packages are still reference this code.

namespace FWBS.OMS.UI.Windows.TreeViewNavigation
{
    #region INodeFormatter
    public interface INodeFormatter
    {
        Font Font { get; }
        Color ForeColor { get; }
    }
    #endregion INodeFormatter

    #region DefaultSettings
    public class DefaultSettings
    {
        public static int ItemHeight
        {
            get
            {
                return 20;
            }
        }


        public Color Colour
        {
            get
            {
                return Color.White;
            }
        }
    }
    #endregion DefaultSettings


    #region ParentNodeSettings
    public class ParentNodeSettings : INodeFormatter
    {
        public Font Font
        {
            get
            {
                return new Font(new FontFamily(CurrentUIVersion.Font), CurrentUIVersion.FontSize, FontStyle.Bold);
            }
        }


        public Color ForeColor
        {
            get
            {
                return Color.Black;
            }
        }


        public static int ItemHeight
        {
            get
            {
                return 25;
            }
        }
    }
    #endregion ParentNodeSettings


    #region ChildNodeSettings
    public class ChildNodeSettings : INodeFormatter
    {
        public Font Font
        {
            get
            {
                return new Font(new FontFamily(CurrentUIVersion.Font), CurrentUIVersion.FontSize, FontStyle.Regular);
            }
        }


        public Color ForeColor
        {
            get
            {
                return Color.Black;
            }
        }


        public static int ItemHeight
        {
            get
            {
                return 20;
            }
        }
    }
    #endregion ChildNodeSettings


    #region SelectedNodeSettings
    public class SelectedNodeSettings : INodeFormatter
    {
        public Font Font
        {
            get
            {
                return new Font(new FontFamily(CurrentUIVersion.Font), CurrentUIVersion.FontSize, FontStyle.Bold);
            }
        }


        public Color ForeColor
        {
            get
            {
                return Color.Black;
            }
        }


        public Color Color
        {
            get
            {
                return Color.LightGray;
            }
        }
    }
    #endregion SelectedNodeSettings


    #region HoveredNodeSettings
    public class HoveredNodeSettings
    {
        public Color Color
        {
            get
            {
                return Color.AliceBlue;
            }
        }
    }
    #endregion HoveredNodeSettings
}

namespace FWBS.OMS.UI.Windows
{
    using FWBS.OMS.UI.Windows.TreeViewNavigation;

    public static class TreeNodeColoration
    {
        #region Methods

        public static void SetNodeColouring(RadTreeView treeView, Color color)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BackColor = color;
                treeView.SelectedNode.BackColor2 = color;
                treeView.SelectedNode.BackColor3 = color;
                treeView.SelectedNode.BackColor4 = color;
                treeView.SelectedNode.BorderColor = Color.White;
            }
        }


        public static void SetSelectedNodeSettings(RadTreeView treeView)
        {
            var selectedNodeSettings = new TreeViewNavigation.SelectedNodeSettings();

            if (treeView.SelectedNode != null)
            {
                SetNodeFormat(treeView.SelectedNode, selectedNodeSettings);
                SetNodeColouring(treeView, selectedNodeSettings.Color);
            }
        }


        public static void SetNodesToDefaultSettings(RadTreeView treeView)
        {
            var parentNodeSettings = new TreeViewNavigation.ParentNodeSettings();
            var childNodeSettings = new TreeViewNavigation.ChildNodeSettings();

            foreach (RadTreeNode parentNode in treeView.Nodes)
            {
                SetNodeFormat(parentNode, parentNodeSettings);

                if (NodeHasChildren(parentNode))
                {
                    foreach (RadTreeNode childNode in parentNode.Nodes)
                    {
                        SetNodeFormat(childNode, childNodeSettings);
                        SetChildNodeDefaultSettings(childNode);
                    }
                }
            }
        }


        private static bool NodeHasChildren(RadTreeNode node)
        {
            return node.Nodes.Count > 0;
        }


       public static void SetNodeFormat(RadTreeNode node, INodeFormatter nodeSettings)
       {
           node.ForeColor = nodeSettings.ForeColor;
           node.Font = nodeSettings.Font;
       }


        public static void SetChildNodeDefaultSettings(RadTreeNode parentNode)
        {
            var childNodeSettings = new TreeViewNavigation.ChildNodeSettings();

            foreach (RadTreeNode node in parentNode.Nodes)
            {
                SetNodeFormat(node, childNodeSettings);
                
                if (NodeHasChildren(node))
                    SetChildNodeDefaultSettings(node);
            }
        }


        public static void SetMouseHoverColour(TreeNodeFormattingEventArgs e, Color colour)
        {
            if (e.NodeElement.IsMouseOver)
            {
                e.NodeElement.BackColor = colour;
                e.NodeElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
                e.NodeElement.DrawFill = true;
            }
        }

        #endregion Methods
    }
}
