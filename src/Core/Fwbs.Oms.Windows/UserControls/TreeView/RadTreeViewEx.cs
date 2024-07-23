using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.UserControls.TreeView
{
    /// <summary>
    /// Extended Treeview control which based on RadTreeView (Telerik control) and allows to automate internal encapsulated checkbox elements in node items.
    /// </summary>
    public class RadTreeViewEx : RadTreeView
    {
        public RadTreeViewEx() : base()
        { }

        /// <summary>
        /// Solving style issue
        /// https://www.telerik.com/forums/radtreeview-customcontrol-loosing-default-theme
        /// </summary>
        public override string ThemeClassName
        {
            get
            {
                return typeof(RadTreeView).FullName;
            }
        }

        protected override AccessibleObject CreateAccessibilityInstance()
        {
            if (!EnableRadAccessibilityObjects)
                return base.CreateAccessibilityInstance();

            return new RadTreeViewAccessibleObjectEx(this);
        }

        public class RadTreeViewAccessibleObjectEx : RadTreeViewAccessibleObject
        {
            private RadTreeViewEx _owner;
            private Dictionary<AccessibleObject, CheckBoxElementAccessibleObject> _cachedObjects;

            public RadTreeViewAccessibleObjectEx(RadTreeViewEx owner) : base(owner)
            {
                _owner = owner;
                _cachedObjects = new Dictionary<AccessibleObject, CheckBoxElementAccessibleObject>();
            }

            public override int GetChildCount()
            {
                int count = base.GetChildCount();

                var nodesWithCheckBox = _owner.TreeViewElement.GetNodes().Where(n => n.CheckType == CheckType.CheckBox);

                return count + nodesWithCheckBox.Count();
            }

            public override AccessibleObject GetChild(int index)
            {
                int count = base.GetChildCount();

                if (index < count)
                    return base.GetChild(index);

                RadTreeNode[] nodes = _owner.TreeViewElement.GetNodes().Where(n => n.CheckType == CheckType.CheckBox).ToArray();

                index = index - count;

                if (index < nodes.Length)
                    return GetAccessibleObject(base.GetNodeAccessibleObject(nodes[index]));

                return null;
            }

            public override AccessibleObject HitTest(int x, int y)
            {
                AccessibleObject baseAccObj = base.HitTest(x, y);

                if (_cachedObjects.ContainsKey(baseAccObj))
                {
                    CheckBoxElementAccessibleObject accObj = _cachedObjects[baseAccObj];

                    if (accObj.Bounds.Contains(x, y))
                        return accObj;
                }

                return baseAccObj;
            }

            private AccessibleObject GetAccessibleObject(AccessibleObject accObj)
            {
                if (!_cachedObjects.ContainsKey(accObj))
                {
                    RadTreeNode node = ((RadTreeNodeAccessibleObject)accObj).Owner as RadTreeNode;

                    _cachedObjects.Add(accObj, new CheckBoxElementAccessibleObject(node));
                }

                return _cachedObjects[accObj];
            }
        }
    }
}

