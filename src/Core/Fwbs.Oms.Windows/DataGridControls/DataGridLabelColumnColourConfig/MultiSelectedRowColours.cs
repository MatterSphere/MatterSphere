using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    public interface IMultiSelectedRowColors
    {
        Color LastSelectedRowBackColor { get; }
        Color LastSelectedRowForeColor { get; }
        Color SelectedRowsBackColor { get; }
        Color SelectedRowsForeColor { get; }
        Color UnselectedRowsBackColor { get; }
        Color UnselectedRowsForeColor { get; }
    }


    internal class Version2MultiSelectedRowColors : IMultiSelectedRowColors
    {
        public Color LastSelectedRowBackColor
        {
            get
            {
                return Color.FromArgb(26, 128, 187);
            }
        }

        public Color LastSelectedRowForeColor
        {
            get
            {
                return SystemColors.Window;
            }
        }

        public Color SelectedRowsBackColor
        {
            get
            {
                return Color.FromArgb(48, 140, 194);
            }
        }

        public Color SelectedRowsForeColor
        {
            get
            {
                return SystemColors.Window;
            }
        }

        public Color UnselectedRowsBackColor
        {
            get
            {
                return SystemColors.Window;
            }
        }

        public Color UnselectedRowsForeColor
        {
            get
            {
                return SystemColors.WindowText;
            }
        }
    }
}
