using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public partial class ucPageControl : UserControl
    {
        public event EventHandler<int> PageChanged;

        private const string _prevPage = "";
        private const string _nextPage = "";
        private const string _upPage = "";
        private const string _downPage = "";
        private const int _defaultPageSize = 50;

        private int _totalPages = 1;
        private bool _checkChanges = false;

        public ucPageControl()
        {
            InitializeComponent();

            btnPrevPage.Text = _prevPage;
            btnNextPage.Text = _nextPage;
            btnUp.Text = _upPage;
            btnDown.Text = _downPage;

            PageSize = _defaultPageSize;
            CurrentPage = 1;
            tbCurrentPage.Text = "1";
        }

        private int _currentPage;
        private int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                CheckNavigationButtons();
            }
        }

        public double PageSize { get; set; }

        #region Methods

        public void SetTotalPages(object sender, int totalItems)
        {
            _totalPages = Math.Max((int)Math.Ceiling(totalItems / PageSize), 1);
            lblTotal.Text = _totalPages.ToString();
            CheckNavigationButtons();
        }

        public void Reset()
        {
            CurrentPage = 1;
            tbCurrentPage.Text = "1";
            ChangeSelection(false);
        }

        #endregion

        #region Private methods

        private void CheckNavigationButtons()
        {
            btnPrevPage.Enabled = CurrentPage > 1;
            btnNextPage.Enabled = CurrentPage < _totalPages;
            btnDown.Enabled = CurrentPage > 1;
            btnUp.Enabled = CurrentPage < _totalPages;
        }

        private void DisableNavigationButtons(int page)
        {
            btnDown.Enabled = page > 1;
            btnUp.Enabled = page < _totalPages;
            btnNextPage.Enabled = false;
            btnPrevPage.Enabled = false;
        }

        private void IncreasePage()
        {
            _checkChanges = true;
            tbCurrentPage.Text = $"{CurrentPage + 1}";
        }

        private void DecreasePage()
        {
            _checkChanges = true;
            tbCurrentPage.Text = $"{CurrentPage - 1}";
        }

        private void PreIncreasePage()
        {
            int page;
            Int32.TryParse(tbCurrentPage.Text, out page);
            page++;

            DisableNavigationButtons(page);
            tbCurrentPage.Text = page.ToString();
            ChangeSelection(true);

            tbCurrentPage.Focus();
        }

        private void PreDecreasePage()
        {
            int page;
            Int32.TryParse(tbCurrentPage.Text, out page);
            page--;

            DisableNavigationButtons(page);
            tbCurrentPage.Text = page.ToString();
            ChangeSelection(true);

            tbCurrentPage.Focus();
        }

        private bool ValidateQuery(int page)
        {
            if (page < 1)
            {
                _checkChanges = true;
                tbCurrentPage.Text = "1";
                return false;
            }

            if (page > _totalPages)
            {
                _checkChanges = true;
                tbCurrentPage.Text = _totalPages.ToString();
                return false;
            }

            return true;
        }

        private void ChangeSelection(bool isSelected)
        {
            pnlCurrentPage.BackColor = isSelected
                ? Color.FromArgb(237, 243, 250)
                : Color.White;
            tbCurrentPage.BackColor = isSelected
                ? Color.FromArgb(237, 243, 250)
                : Color.White;
        }

        #endregion

        #region UI events

        private void btnNextPage_Click(object sender, System.EventArgs e)
        {
            IncreasePage();
        }

        private void btnPrevPage_Click(object sender, System.EventArgs e)
        {
            DecreasePage();
        }

        private void btnUp_Click(object sender, System.EventArgs e)
        {
            PreIncreasePage();
        }

        private void btnDown_Click(object sender, System.EventArgs e)
        {
            PreDecreasePage();
        }

        private void tbCurrentPage_TextChanged(object sender, System.EventArgs e)
        {
            if (_checkChanges)
            {
                int currentPage;
                Int32.TryParse(tbCurrentPage.Text, out currentPage);

                if (!ValidateQuery(currentPage))
                {
                    return;
                }

                CurrentPage = currentPage;
                _checkChanges = false;
                PageChanged?.Invoke(this, CurrentPage);
            }
        }

        private void tbCurrentPage_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                int currentPage;
                Int32.TryParse(tbCurrentPage.Text, out currentPage);

                if (!ValidateQuery(currentPage))
                {
                    return;
                }

                ChangeSelection(false);
                CurrentPage = currentPage;
                CheckNavigationButtons();
                PageChanged?.Invoke(this, CurrentPage);
            }
        }

        #endregion
    }
}
