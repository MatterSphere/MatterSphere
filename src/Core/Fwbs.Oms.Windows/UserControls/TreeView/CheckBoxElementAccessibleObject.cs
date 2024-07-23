using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.UserControls.TreeView
{
    public class CheckBoxElementAccessibleObject : AccessibleObject
    {
        RadTreeNode _owner;
        public CheckBoxElementAccessibleObject(RadTreeNode owner)
            : base()
        {
            _owner = owner;
        }

        public override string Help
        {
            get { return "Used to expose properties for testing from the Telerik RadTreeNode."; }
        }

        public override Rectangle Bounds
        {
            get
            {
                TreeNodeElement nodeElement = _owner.TreeViewElement.GetElement(_owner);

                TreeNodeCheckBoxElement checkBoxElement = null;

                if (nodeElement != null)
                    checkBoxElement = nodeElement.GetChildrenByType(typeof(TreeNodeCheckBoxElement)).FirstOrDefault() as TreeNodeCheckBoxElement;

                if (checkBoxElement != null)
                    return new Rectangle(_owner.TreeView.PointToScreen(checkBoxElement.ControlBoundingRectangle.Location), checkBoxElement.Size);

                return Rectangle.Empty;
            }
        }

        public override string Name
        {
            get
            {
                return _owner.Text;
            }
        }

        public override string Description
        {
            get
            {
                return "CheckBoxName;" + this.Name + ";Checked=" + _owner.Checked;
            }
        }

        public override AccessibleRole Role
        {
            get
            {
                return AccessibleRole.CheckButton;
            }
        }

        public override AccessibleStates State
        {
            get
            {
                if (!_owner.Enabled)
                    return base.State;

                AccessibleStates state = base.State;

                if (_owner.Checked)
                    state |= AccessibleStates.Checked;

                return state;
            }
        }

        public override string DefaultAction
        {
            get
            {
                return _owner.Checked ? "Uncheck" : "Check";
            }
        }

        public override void DoDefaultAction()
        {
            _owner.CheckState = _owner.Checked ? ToggleState.Off : ToggleState.On;
        }
    }
}
