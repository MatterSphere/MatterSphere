using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Common
{
    public partial class ucSearchPagination : UserControl
    {
        public event EventHandler onPageSettingsChanged;

        private const string _prevPageSymbol = "";
        private const string _nextPageSymbol = "";
        private int[] _defaultPages = new int[] { 50, 100, 250 };
        private readonly string _lblOf;
        private bool _checkPagesChanges;
        private bool _checkPageSizeChanges;

        [Browsable(false)]
        [DefaultValue(50)]
        internal int PageSize { get; private set; }

        [Browsable(false)]
        [DefaultValue(1)]
        internal int CurrentPage { get; set; }

        [Browsable(false)]
        [DefaultValue(0)]
        internal int TotalItems { get; private set; }

        public ucSearchPagination()
        {
            CurrentPage = 1;
            PageSize = 50;
            InitializeComponent();

            SetPageSizes(_defaultPages, PageSize);
            _lblOf = Session.CurrentSession.IsConnected ? Session.CurrentSession.Resources.GetResource("OF", "of", "").Text + " " : "of ";
            this.btnPrevPage.Text = _prevPageSymbol;
            this.btnNext.Text = _nextPageSymbol;
        }

        internal void SetPageSizes(int[] pages, int defaultSize)
        {
            cmbPageSize.Items.Clear();
            if (pages.Length > 0)
            {
                PageSize = pages[0];
                foreach (int page in pages)
                {
                    cmbPageSize.Items.Add(page);
                    if (page == defaultSize)
                        PageSize = page;
                }
                cmbPageSize.SelectedItem = PageSize;
            }
            _checkPageSizeChanges = true;
        }

        internal void UpdatePageControls(int total)
        {
            TotalItems = total;
            TotalCount.Text = total.ToString();
            lblOf.Text = _lblOf + GetLastPage().ToString();
            SetPages();
            CheckPageButtons();
        }

        internal void ResetPages(int total)
        {
            CurrentPage = 1;
            UpdatePageControls(total);
        }

        private int GetLastPage()
        {
            return Math.Max((int)Math.Ceiling(TotalItems / (double)PageSize), 1);
        }

        private void SetPages()
        {
            var pages = new List<int>() { 1 };

            int lastPage = GetLastPage();
            for (int i = Math.Max(CurrentPage - 5, 2), n = Math.Min(CurrentPage + 5, lastPage - 1); i <= n; i++)
            {
                pages.Add(i);
            }

            if (lastPage > 1)
                pages.Add(lastPage);

            cmbPages.Items.Clear();
            foreach (int page in pages)
            {
                cmbPages.Items.Add(page);
            }

            _checkPagesChanges = false;
            cmbPages.SelectedItem = CurrentPage;
            _checkPagesChanges = true;
        }

        private void CheckPageButtons()
        {
            btnPrevPage.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage * PageSize < TotalItems;
        }

        #region UI events

        private void cmbPageSize_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_checkPageSizeChanges)
            {
                CurrentPage = 1;
                PageSize = cmbPageSize.SelectedItem == null
                    ? (int)cmbPageSize.Items[0]
                    : (int)cmbPageSize.SelectedItem;

                onPageSettingsChanged?.Invoke(this, e);
            }
        }

        private void cmbPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_checkPagesChanges)
            {
                CurrentPage = (int)cmbPages.SelectedItem;
                onPageSettingsChanged?.Invoke(this, e);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            onPageSettingsChanged?.Invoke(this, e);
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            onPageSettingsChanged?.Invoke(this, e);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var pen = new Pen(Color.FromArgb(216, 216, 216), LogicalToDeviceUnits(1)))
            {
                e.Graphics.DrawLine(pen, 0, 0, Width, 0);
            }
        }

        #endregion UI events
    }
}
