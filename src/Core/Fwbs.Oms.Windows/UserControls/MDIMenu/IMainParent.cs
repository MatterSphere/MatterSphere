using System.Collections;
using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    public interface IMainParent
    {
        SortedList OpenWindows { get; }

        object Action(string ActionCmd, string ActionLabel);
        Control ConstructAdminElement(string filter, Control parent, string ecmd);
        Control MacroCommands(string ecmd, string filter, out bool result);
    }
}
