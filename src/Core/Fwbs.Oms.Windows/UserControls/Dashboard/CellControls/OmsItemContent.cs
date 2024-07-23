using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public class OmsItemContent : IEditFormContent
    {
        private ucOmsItem _omsItem;

        public OmsItemContent(ucOmsItem omsItem)
        {
            _omsItem = omsItem;
        }

        #region IEditFormContent

        public bool AutoScroll
        {
            get { return _omsItem.AutoScroll; }
            set { _omsItem.AutoScroll = value; }
        }

        public Control Content
        {
            get { return _omsItem; }
        }

        public DockStyle Dock
        {
            get { return _omsItem.Dock; }
            set { _omsItem.Dock = value; }
        }

        public bool Enabled
        {
            get { return _omsItem.Enabled; }
            set { _omsItem.Enabled = value; }
        }

        public bool IsDirty
        {
            get
            {
                return _omsItem.IsDirty;
            }
        }

        public Point Location
        {
            get { return _omsItem.Location; }
            set { _omsItem.Location = value; }
        }

        public void UpdateContent()
        {
            _omsItem.UpdateItem();
        }

        #endregion
    }
}
