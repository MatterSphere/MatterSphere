using System.Windows.Forms;

namespace FWBS.OMS.UI
{
    public interface IMenuMerge
    {
        void MergeMenus(MenuStrip source);
        void UnMergeMenus(MenuStrip source);
    }
}
