using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public interface IEditFormContent
    {
        bool AutoScroll { get; set; }
        Control Content { get; }
        DockStyle Dock { get; set; }
        bool Enabled { get; set; }
        bool IsDirty { get; }
        Point Location { get; set; }
        void UpdateContent();
    }
}
