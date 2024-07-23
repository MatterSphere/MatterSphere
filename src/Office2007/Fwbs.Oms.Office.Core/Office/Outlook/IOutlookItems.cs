using System;
using System.Collections.Generic;

namespace Fwbs.Office.Outlook
{

    public interface IOutlookItems : IEnumerable<OutlookItem>, IDisposable
    {
        int Count { get; }

        OutlookItem this[int index] { get; }

    }

   
}
