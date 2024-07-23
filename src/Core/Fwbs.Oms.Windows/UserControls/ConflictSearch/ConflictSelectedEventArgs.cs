using System;
using System.Data;

namespace FWBS.OMS.UI.UserControls.ConflictSearch
{
    /// <summary>
    /// Conflict selected changed event arguments of the search conflicts control.
    /// </summary>
    public class ConflictSelectedEventArgs : EventArgs
    {
        internal ConflictSelectedEventArgs(DataRow row)
        {
            this.SelectedRow = row;
        }

        public DataRow SelectedRow { get; } = null;
    }
}
