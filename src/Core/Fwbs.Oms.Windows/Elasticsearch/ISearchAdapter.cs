using System;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    public interface ISearchAdapter : IDisposable
    {
        void SetPageSource(IOMSType entity);
        void SearchAsync();
    }
}
