using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.TabHeaders
{
    internal partial class ucTabs : UserControl
    {
        private List<tabHeader> _tabHeaders;
        private bool _isFullSizeMode;

        public ucTabs()
        {
            InitializeComponent();

            _tabHeaders = new List<tabHeader>();
            _isFullSizeMode = true;
        }

        public int HeadersFullSizeWidth
        {
            get
            {
                return _tabHeaders.Any()
                    ? _tabHeaders.Sum(header => header.FullSizeWidth)
                    : 0;
            }
        }

        public void AddHeader(tabHeader header)
        {
            _tabHeaders.Add(header);
            this.Controls.Add(header);
            CheckHeaders();
        }


        private void ucTabs_SizeChanged(object sender, System.EventArgs e)
        {
            CheckHeaders();
        }

        private void CheckHeaders()
        {
            if (_isFullSizeMode && HeadersFullSizeWidth > this.Width)
            {
                foreach (var tabHeader in _tabHeaders)
                {
                    tabHeader.SetCompactSizeMode();
                }

                _isFullSizeMode = false;

                return;
            }

            if (!_isFullSizeMode && HeadersFullSizeWidth <= this.Width)
            {
                foreach (var tabHeader in _tabHeaders)
                {
                    tabHeader.SetFullSizeMode();
                }

                _isFullSizeMode = true;
            }
        }
    }
}
