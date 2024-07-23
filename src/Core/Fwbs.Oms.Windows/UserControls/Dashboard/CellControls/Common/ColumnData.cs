using System.Drawing;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class ColumnData
    {
        public ColumnData(string name, ColumnTypeEnum columnType, string title, bool allowManage = true) : this(name, columnType)
        {
            Title = title;
            Visible = true;
            AllowManage = allowManage;
        }

        public ColumnData(string name, ColumnTypeEnum columnType)
        {
            Name = name;
            ReadOnly = true;
            ColumnType = columnType;
        }

        public string Name { get; private set; }
        public string Title { get; private set; }
        public bool Visible { get; set; }
        public bool AllowManage { get; private set; }
        public bool ReadOnly { get; set; }
        public ColumnTypeEnum ColumnType { get; private set; }
        public int? MinimumWidth { get; set; }
        public int? Width { get; set; }
        public System.Windows.Forms.DataGridViewAutoSizeColumnMode? AutoSizeMode { get; set; }
        public ucDataView.TextAlignmentEnum? TextAlignment { get; set; }
        public bool Resizable { get; set; }
        public bool DefaultPadding { get; set; }
        public bool Sortable { get; set; }
    }

    public enum ColumnTypeEnum
    {
        DataGridViewFavoritesColumn,
        DataGridViewFlagColumn,
        DataGridViewTextBoxColumn,
        DataGridViewImageColumn,
        DataGridViewActionsColumn,
        DataGridViewTaskStatusColumn,
        DataGridViewBlueCheckBoxColumn,
        DataGridViewCustomTextColumn,
        DataGridViewRecentsColumn
    }
}
