using System;

namespace Fwbs.Office.Outlook
{
    partial class OutlookTask 
    {
        public DateTime ToDoTaskOrdinal
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ToDoTaskOrdinal);
            }
            set
            {
                InternalItem.ToDoTaskOrdinal = Helpers.LocalToLocal(value);
            }
        }
    }
}
