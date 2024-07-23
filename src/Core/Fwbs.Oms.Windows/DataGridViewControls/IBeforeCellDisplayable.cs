using System;
using System.Data;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// DataGrid columns that implement this interface can support custom row's highlighting.
    /// Highlighting can be set both through code and package scripts.
    /// </summary>
    public interface IBeforeCellDisplayable
    {
        event EventHandler<CellDisplayEventArgs> BeforeCellDisplayEvent;

        CellDisplayEventArgs OnBeforeCellDisplayEvent(int rowNum, CurrencyManager source, string text, string columnName);
    }
}
