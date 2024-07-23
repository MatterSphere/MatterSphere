using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    public interface ISingleSelectedRowColors
    {
        Color SelectedRowBackColor { get; }
        Color SelectedRowForeColor { get; }
        Color UnselectedRowBackColor { get; }
        Color UnselectedRowForeColor { get; }
    }


    internal class Version2SingleSelectedRowColors : ISingleSelectedRowColors
    {
        public Color SelectedRowBackColor
        {
            get
            {
                return Color.FromArgb(26, 128, 187);
            }
        }

        public Color SelectedRowForeColor
        {
            get
            {
                return SystemColors.HighlightText;
            }
        }

        public Color UnselectedRowBackColor
        {
            get
            {
                return SystemColors.Window;
            }
        }

        public Color UnselectedRowForeColor
        {
            get
            {
                return SystemColors.WindowText;
            }
        }
    }
}
