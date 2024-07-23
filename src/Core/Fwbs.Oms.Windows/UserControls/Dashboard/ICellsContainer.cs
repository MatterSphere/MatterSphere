using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal interface ICellsContainer
    {
        List<Size> GetAvailableSizes(Point coordinate, Control self = null);
    }
}
