using System;
using System.Data;
using System.Drawing;

namespace FWBS.Common.UI.Windows
{
    public class CellDisplayEventArgs : EventArgs
    {
        public CellDisplayEventArgs(int rownum, DataRowView row, string text, string columnname)
        {
            this.BackColor = Color.Empty;
            this.ForeColor = Color.Empty;
            this.RowNum = rownum;
            this.Row = row;
            this.Text = text;
            this.ColumnName = columnname;
        }

        public string ColumnName { get; private set; }

        public string Text { get; set; }

        public Color BackColor { get; set; }

        public Color ForeColor { get; set; }

        public int RowNum { get; private set; }

        public DataRowView Row { get; private set; }
    }
}
