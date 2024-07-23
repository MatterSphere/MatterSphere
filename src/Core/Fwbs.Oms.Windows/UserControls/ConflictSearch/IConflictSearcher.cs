using System;
using System.Windows.Forms;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.ConflictSearch
{
    internal interface IConflictSearcher
    {
        event EventHandler ItemSelected;

        event SearchButtonEventHandler SearchButtonCommands;

        event SearchCompletedEventHandler SearchCompleted;

        event SearchStateChangedEventHandler StateChanged;

        bool AutoScroll { get; set; }

        Button cmdSearch { get; }

        Button cmdSelect { get; }

        EnquiryForm EnquiryForm { get; }

        bool Visible { get; set; }

        FWBS.Common.KeyValueCollection ReturnValues { get; }

        SearchList SearchList { get; }

        bool Focus();

        bool SelectRowItem();

        void SetSearchListType(string type, object parent, FWBS.Common.KeyValueCollection parameters);

    }
}
