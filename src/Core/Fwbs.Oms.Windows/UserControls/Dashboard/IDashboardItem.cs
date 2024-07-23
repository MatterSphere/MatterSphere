using System.Drawing;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal interface IDashboardItem
    {
        Point CellLocation { get; set; }
        Size CellSize { get; set; }
    }
}
