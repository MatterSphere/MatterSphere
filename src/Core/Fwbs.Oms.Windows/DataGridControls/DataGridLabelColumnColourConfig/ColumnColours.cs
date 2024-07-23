namespace FWBS.Common.UI.Windows
{
    internal class ColumnColours
    {
        internal ISingleSelectedRowColors SingleSelectedRow;
        internal IMultiSelectedRowColors MultiSelectedRows;

        public ColumnColours(ISingleSelectedRowColors singleSelectedRow, IMultiSelectedRowColors multiSelectedRows)
        {
            this.SingleSelectedRow = singleSelectedRow;
            this.MultiSelectedRows = multiSelectedRows;
        }
    }
}
