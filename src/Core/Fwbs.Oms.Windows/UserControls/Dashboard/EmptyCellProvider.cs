using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal class EmptyCellProvider
    {
        private CellMenuBuilder _menuBuilder;
        private List<Point> _emptyCells;

        internal EmptyCellProvider(CellMenuBuilder menuBuilder)
        {
            _menuBuilder = menuBuilder;
            _emptyCells = new List<Point>();
        }

        internal void CreateCells(ucDashboard dashboard)
        {
            for (int row = 0; row < dashboard.RowsNumber; row++)
            {
                for (int column = 0; column < dashboard.ColumnsNumber; column++)
                {
                    var cellData = new Point(column, row);
                    _emptyCells.Add(cellData);

                    var cell = CreateCell(cellData);
                    cell.SetCellsContainer(dashboard);
                    dashboard.AttachInsertHandler(cell);
                    dashboard.AddEmptyCell(cell);
                }
            }
        }

        internal void AddEmptyCells(Point location, Size size, ucDashboard dashboard)
        {
            for (int i = 0; i < size.Width; i++)
            {
                for (int j = 0; j < size.Height; j++)
                {
                    var data = _emptyCells.First(ec => ec.X == location.X + i && ec.Y == location.Y + j);
                    var cell = CreateCell(data);
                    cell.SetCellsContainer(dashboard);
                    dashboard.AttachInsertHandler(cell);
                    dashboard.AddEmptyCell(cell);
                }
            }
        }

        private ucEmptyCell CreateCell(Point location)
        {
            var cell = new ucEmptyCell
            {
                BackColor = Color.White,
                ContextMenuStrip = new DashboardContextMenuStrip(),
                Dock = System.Windows.Forms.DockStyle.Fill,
                Padding = new System.Windows.Forms.Padding(4)
            };

            cell.SetMenuBuilder(_menuBuilder);
            cell.CellLocation = location;

            return cell;
        }
    }
}
