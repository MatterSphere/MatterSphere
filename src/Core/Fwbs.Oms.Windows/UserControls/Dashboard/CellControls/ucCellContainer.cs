using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.TabHeaders;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Interfaces;
using Infragistics.Win;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public partial class ucCellContainer : UserControl, IDashboardItem, IOpenOMSType
    {
        private const int SUBMENU_ITEM_HEIGHT = 32;

        private string _maximizeToolTipLabel = CodeLookup.GetLookup("DASHBOARD", "MXMZ", "Maximize");
        private string _minimizeToolTipLabel = CodeLookup.GetLookup("DASHBOARD", "MNMZ", "Minimize");

        private readonly bool _isConfigurationMode;
        private bool _isMaximized;
        private UltraPeekPopup _popup;
        private ContextMenuPopup _gridSettingsPopup;
        private SecondLevelPopupContainer _gridSettingsContainer;
        private ContextMenuPopup _resizeSettingsPopup;
        private SecondLevelPopupContainer _resizeSettingsContainer;
        private tabHeader[] _tabHeaders;
        private BaseContainerPage _currentPage;
        private IPageProvider _pageProvider;
        private ucSearchControl _searchControl;
        private EnquiryForm _enquiryForm;
        private string _code;
        private BaseContainerPage _mainControl;

        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow;

        public void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            NewOMSTypeWindow?.Invoke(this, e);
        }

        #region Constructors
        public ucCellContainer(bool isConfigurationMode)
        {
            InitializeComponent();
            _isConfigurationMode = isConfigurationMode;
            btnActions.Text = "";
            PopOutVisible = true;
            SetIcons();
            this.toolTip.SetToolTip(this.btnFullScreen, _maximizeToolTipLabel);

            _popup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Content = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true
                },
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            _gridSettingsPopup = new ContextMenuPopup();
            _gridSettingsContainer = new SecondLevelPopupContainer(_gridSettingsPopup);
            _resizeSettingsPopup = new ContextMenuPopup()
            {
                DefaultMinWidth = 30,
                Padding = new Padding(0)
            };
            _resizeSettingsContainer = new SecondLevelPopupContainer(_resizeSettingsPopup);

            searchPanel.Closed += ResetSearch;
            searchPanel.QueryChanged += OnQueryChanged;

            if (isConfigurationMode)
            {
                HideFullScreenButton();
            }
        }

        #endregion

        #region Events

        public event EventHandler Closing;
        public event EventHandler<string> ActionCalled;
        public event EventHandler<string> QueryChanged;
        public event EventHandler<string> SearchStarted;
        public event EventHandler Maximizing;
        public event EventHandler Minimizing;
        public event EventHandler AddNewClicked;
        public event EventHandler DeleteClicked;
        public event EventHandler<ResizeOptionsEventArgs> ResizeClicked;

        protected virtual void OnResizeClicked(ResizeOptionsEventArgs e)
        {
            ResizeClicked?.Invoke(this, e);
        }

        public event EventHandler<ResizingEventArgs> Resizing;

        protected virtual void OnResizing(ResizingEventArgs e)
        {
            Resizing?.Invoke(this, e);
        }

        #endregion

        public Point CellLocation { get; set; }
        public Size CellSize { get; set; }
        public DashboardCell DashboardCell { get; set; }
        public FavoritesProvider FavoritesProvider { get; set; }
        public bool PopOutVisible { get; set; }

        #region Public methods

        public void SetDashboardInformation(Guid id, string dashboardCode)
        {
            DashboardCell.Id = id;
            DashboardCell.DashboardCode = dashboardCode;
            if (_currentPage != null)
            {
                _currentPage.DashboardCellGuid = id;
                _currentPage.DashboardCode = dashboardCode;
            }
        }

        public void SetContent(object parent = null)
        {
            if (_searchControl != null)
            {
                _searchControl.SetSearchList(_code, true, parent, new FWBS.Common.KeyValueCollection());
                _searchControl.RefreshItem();
            }
            else if (_enquiryForm != null)
            {
                _enquiryForm.Enquiry = Enquiry.GetEnquiry(_code, parent, EnquiryMode.None, true, null);
            }
            else if (_currentPage != null)
            {
                _currentPage.ParentObject = parent;
            }
        }

        public void InsertEnquiryForm(EnquiryForm enquiryForm, string code)
        {
            container.Controls.Remove(bottomLine);
            container.Controls.Remove(bottomPanel);
            enquiryForm.Dock = DockStyle.Fill;
            mainContainer.Controls.Add(enquiryForm);
            topPanelTable.Controls.Remove(btnSearch);

            _enquiryForm = enquiryForm;
            _code = code;
        }

        public void InsertSearchList(ucSearchControl searchControl, string code)
        {
            container.Controls.Remove(bottomLine);
            container.Controls.Remove(bottomPanel);
            searchControl.Dock = DockStyle.Fill;
            mainContainer.Controls.Add(searchControl);

            _searchControl = searchControl;
            _code = code;
        }

        public void InsertDefaultItem(IPageProvider pageProvider)
        {
            _pageProvider = pageProvider;
            var headers = new List<tabHeader>();
            var tabsControl = new ucTabs
            {
                Dock = DockStyle.Fill
            };

            foreach (var header in _pageProvider.Headers)
            {
                var details = _pageProvider.GetDetails(header);
                var tabHeader = new tabHeader
                {
                    Code = header,
                    Dock = DockStyle.Left,
                    OmsObjectCode = details.OmsObjectCode
                };

                tabHeader.SetTitle(details.Title);
                tabHeader.Clicked += ClickTabHeader;

                headers.Add(tabHeader);
                tabsControl.AddHeader(tabHeader);
            }

            titlePanel.Controls.Add(tabsControl);

            _tabHeaders = headers.ToArray();
            _tabHeaders.Last().SelectItem();

            OpenPage(_tabHeaders.Last(), false);
        }

        public void InsertTitle(string text)
        {
            titlePanel.Controls.Clear();
            titlePanel.Padding = new Padding(LogicalToDeviceUnits(17), 0, 0, 0);
            var title = new Label
            {
                AutoSize = false,
                AutoEllipsis = true,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 12F),
                Text = Session.CurrentSession.Terminology.Parse(text, true).Replace("&", "&&"),
                TextAlign = ContentAlignment.MiddleLeft
            };

            titlePanel.Controls.Add(title);
        }

        public void HideDeleteButton()
        {
            bottomPanel.Controls.Remove(btnDelete);
        }

        public void ShowBottomPanel()
        {
            bottomLine.Show();
            bottomPanel.Show();
        }

        public void HideBottomPanel()
        {
            bottomLine.Hide();
            bottomPanel.Hide();
        }

        public void HideAddNewButton()
        {
            bottomPanel.Controls.Remove(btnAdd);
        }

        public void HideFullScreenButton()
        {
            topPanelTable.Controls.Remove(btnFullScreen);
        }

        public void HideFilterButton()
        {
            topPanelTable.Controls.Remove(btnFilter);
        }

        #endregion

        #region Private methods

        private void ClickTabHeader(object sender, EventArgs e)
        {
            foreach (var header in _tabHeaders)
            {
                if (header == sender)
                {
                    header.SelectItem();
                }
                else
                {
                    header.UnselectItem();
                }
            }

            var tabHeader = sender as tabHeader;
            if (tabHeader != null && _currentPage.Code != tabHeader.Code)
            {
                OpenPage(tabHeader);
                tabHeader.Select();
            }
        }

        private void OpenPage(tabHeader header, bool withScale = true)
        {
            if (mainContainer.Controls.Any())
            {
                BaseContainerPage openedPage = mainContainer.Controls[0] as BaseContainerPage;
                if (openedPage != null)
                {
                    openedPage.QueryCompleted -= pageControl.SetTotalPages;
                    openedPage.PagesReset -= ResetPages;
                    openedPage.EnableChanged -= OnEnableChanged;
                    openedPage.ActionsEnabled -= OnActionsEnabled;
                    Unsubscribe(openedPage);
                    mainContainer.Controls.Remove(openedPage);
                    openedPage.Dispose();
                }
                
                searchPanel.SearchCalled -= StartSearch;
            }

            mainContainer.Controls.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            pageControl.Reset();
            btnDelete.Enabled = false;

            var page = _pageProvider.GetPage(header.Code);
            page.IsConfigurationMode = _isConfigurationMode;

            if (page.HideBottomPanel)
            {
                HideBottomPanel();
            }
            else
            {
                ShowBottomPanel();
            }

            if (page.HideSearchButton)
            {
                btnSearch.Hide();
            }
            else
            {
                btnSearch.Show();
            }

            pageControl.PageSize = page.PageSize;
            page.SetFavoritesProvider(FavoritesProvider);
            page.DashboardCellGuid = DashboardCell.Id;
            page.DashboardCode = DashboardCell.DashboardCode;
            page.QueryCompleted += pageControl.SetTotalPages;
            page.PagesReset += ResetPages;
            page.EnableChanged += OnEnableChanged;
            page.ActionsEnabled += OnActionsEnabled;
            page.SetColumnSettings();
            page.UpdateData();
            mainContainer.Controls.Add(page, withScale);
            Subscribe(page);
            searchPanel.SearchCalled += StartSearch;

            _currentPage = page;
            btnFilter.Visible = page.FilterOptions.Any();
            if (!string.IsNullOrWhiteSpace(page.DefaultFilter))
            {
                this.toolTip.SetToolTip(this.btnFilter, page.DefaultFilter);
            }
        }

        private void Unsubscribe(BaseContainerPage page)
        {
            SearchStarted -= page.StartSearch;
            AddNewClicked -= page.AddNew;
            DeleteClicked -= page.Delete;
            ActionCalled -= page.CallAction;
            pageControl.PageChanged -= page.ChangePage;
            page.DeleteEnableStatusChanged -= DeleteEnableStatusChangedHandler;
            page.EditFormOpening -= OnEditFormOpen;
        }
        
        private void Subscribe(BaseContainerPage page)
        {
            SearchStarted += page.StartSearch;
            AddNewClicked += page.AddNew;
            DeleteClicked += page.Delete;
            ActionCalled += page.CallAction;
            pageControl.PageChanged += page.ChangePage;
            page.DeleteEnableStatusChanged += DeleteEnableStatusChangedHandler;
            page.EditFormOpening += OnEditFormOpen;
        }

        private void OnEditFormOpen(object sender, EditFormParameters param)
        {
            var form = new EnquiryForm();


            if (param.EnquiryMode == EnquiryMode.Add || param.EnquiryMode == EnquiryMode.Edit)
            {
                if (param.Code == "DSHBTSKADD")
                {
                    if (param.EnquiryMode == EnquiryMode.Add)
                    {
                        if (!SelectFile())
                        {
                            return;
                        }
                    }

                    form.Enquiry = Enquiry.GetEnquiry(param.Code, container, param.EnquiryMode, false, param.Parameters);
                    OpenEditForm(
                        form,
                        param.EnquiryMode,
                        param.EnquiryMode == EnquiryMode.Add
                            ? CodeLookup.GetLookup("DASHBOARD", "ADDTSK", "Add New Task")
                            : CodeLookup.GetLookup("DASHBOARD", "EDTTSK", "Edit Task"));
                }
                else if (param.Code == "DSHBKDADD")
                {
                    if (!SelectFile())
                    {
                        return;
                    }

                    var item = new ucKeyDatesForm(param.Code, Session.CurrentSession.CurrentFile, EnquiryEngine.EnquiryMode.Add, false, null);
                    {
                        Dock = DockStyle.Fill;
                    }

                    OpenEditForm(item, param.EnquiryMode);
                }
            }
        }

        private bool SelectFile()
        {
            try
            {
                SelectFile dlg = new SelectFile(Session.CurrentSession.CurrentFile);

                return dlg.Show(null) != null;
            }
            catch (OMSException ex)
            {
                if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                {
                    throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                }
            }

            return false;
        }

        private void StartSearch(object sender, string e)
        {
            pageControl.Reset();
            SearchStarted?.Invoke(sender, e);
        }

        private void ResetPages(object sender, EventArgs e)
        {
            pageControl.Reset();
        }

        private void ResetSearch(object sender, EventArgs e)
        {
            ResetSearch();
        }

        private void ResetSearch()
        {
            HideSearchPanel();
            pageControl.Reset();
            SearchStarted?.Invoke(null, String.Empty);
        }

        private void HideSearchPanel()
        {
            topPanel.Visible = true;
            topLine.Visible = true;
            searchPanel.Visible = false;
        }

        private void SetIcons()
        {
            btnFullScreen.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "full_screen");
            btnSearch.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "grey_magnifier");
            btnFilter.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "filter_black");
        }

        private void Close(object sender, EventArgs e)
        {
            Closing?.Invoke(this, e);
        }

        private void ActionHandler(string code, bool resetResult)
        {
            if (resetResult)
            {
                pageControl.Reset();
            }

            ActionCalled?.Invoke(this, code);
        }

        private void FilterHandler(string code, string title)
        {
            pageControl.Reset();
            this.toolTip.SetToolTip(this.btnFilter, title);
            ActionCalled?.Invoke(this, code);
        }

        private void DeleteEnableStatusChangedHandler(object sender, bool e)
        {
            btnDelete.Enabled = e;
        }

        private ContentContainer CreateContentContainer()
        {
            var itemBuilder = new DashboardItemBuilder();
            var selectedHeader = _tabHeaders?.First(header => header.IsSelected);
            return itemBuilder.CreateWindowContent(
                this.DashboardCell,
                selectedHeader?.OmsObjectCode,
                selectedHeader?.Text);
        }

        private void PopOver(object sender, EventArgs e)
        {
            var form = new CellDialog();
            form.AddContentControl(CreateContentContainer());
            form.Show();
        }

        private void ShowGridSettings(object sender, EventArgs e)
        {
            _gridSettingsPopup.Controls.Clear();

            for (int i = _currentPage.Columns.Length -1; i >= 0; i--)
            {
                ColumnData columnData = _currentPage.Columns[i];
                if (!columnData.AllowManage)
                    continue;

                var item = new Elasticsearch.BlueCheckBox
                {
                    Name = columnData.Name,
                    Text = columnData.Title,
                    Checked = columnData.Visible,
                    Dock = DockStyle.Top
                };
                item.CheckedChanged += ColumnVisibilityChanged;

                _gridSettingsPopup.AddItem(item);
            }
            
            _gridSettingsContainer.Height = _gridSettingsPopup.Controls.Count * LogicalToDeviceUnits(SUBMENU_ITEM_HEIGHT);
            _gridSettingsContainer.Show((Control)sender);
        }

        private void ShowResizeOptions(object sender, EventArgs e)
        {
            _resizeSettingsPopup.Controls.Clear();
            var contextMenuItemBuilder = new ContextMenuItemBuilder();

            var resizeOptionsEventArgs = new ResizeOptionsEventArgs();
            OnResizeClicked(resizeOptionsEventArgs);
            if (!resizeOptionsEventArgs.Handled || !resizeOptionsEventArgs.ResizeOptions.Any())
            {
                var button = contextMenuItemBuilder.CreateTextButton(CodeLookup.GetLookup("DASHBOARD", "NOOPTSAVAILABLE", "No options available"), ShowResizeOptions, false);
                _resizeSettingsPopup.AddItem(button);
            }
            else
            {
                foreach (var item in resizeOptionsEventArgs.ResizeOptions)
                {
                    var button = contextMenuItemBuilder.CreateTextButton($"{item.Width}x{item.Height}", (object s, EventArgs ea) =>
                    {
                        _resizeSettingsContainer.Close();
                        _popup.Close();
                        OnResizing(new ResizingEventArgs()
                        {
                            NewSize = item
                        });
                    }, true);
                    _resizeSettingsPopup.AddItem(button);
                }
            }
            _resizeSettingsContainer.Height = _resizeSettingsPopup.Controls.Count * LogicalToDeviceUnits(SUBMENU_ITEM_HEIGHT);
            _resizeSettingsContainer.Show((Control)sender);
        }

        private void OpenEditForm(EnquiryForm form, EnquiryMode mode, string title = null)
        {
            HidePanels();

            _mainControl = mainContainer.Controls[0] as BaseContainerPage;
            mainContainer.Controls.Clear();

            var editForm = new ucEditForm
            {
                Dock = DockStyle.Fill,
                IsNew = mode == EnquiryMode.Add
            };

            editForm.InsertTitle(title ?? form.Description);
            
            editForm.Closed += OnCloseEditForm;
            editForm.Canceled += OnCloseEditForm;
            editForm.Added += OnEditFormApplied;
            
            mainContainer.Controls.Add(editForm, true);

            editForm.InsertContent(form);
        }

        private void OpenEditForm(ucOmsItem omsItem, EnquiryMode mode)
        {
            HidePanels();

            _mainControl = mainContainer.Controls[0] as BaseContainerPage;
            mainContainer.Controls.Clear();

            var editForm = new ucEditForm
            {
                Dock = DockStyle.Fill,
                IsNew = mode == EnquiryMode.Add
            };

            editForm.InsertTitle(omsItem.Description);

            editForm.Closed += OnCloseEditForm;
            editForm.Canceled += OnCloseEditForm;
            editForm.Added += OnEditFormApplied;

            mainContainer.Controls.Add(editForm, true);

            editForm.InsertContent(omsItem);
        }

        private void OnCloseEditForm(object sender, EventArgs e)
        {
            var editForm = mainContainer.Controls[0] as ucEditForm;

            editForm.Closed -= OnCloseEditForm;
            editForm.Canceled -= OnCloseEditForm;
            editForm.Added -= OnEditFormApplied;

            mainContainer.Controls.Clear();
            mainContainer.Controls.Add(_mainControl);

            ShowPanels();
        }

        private void OnEditFormApplied(object sender, EventArgs e)
        {
            var editForm = mainContainer.Controls[0] as ucEditForm;
            if (editForm.UpdateItem())
            {
                mainContainer.Controls.Clear();
                _mainControl.UpdateData(true);
                mainContainer.Controls.Add(_mainControl);

                ShowPanels();
            }
        }

        private void ShowPanels()
        {
            _mainControl.ViewMode = false;
            topPanel.Show();
            topLine.Show();
            bottomPanel.Show();
            bottomLine.Show();
        }

        private void HidePanels()
        {
            if (searchPanel.Visible)
            {
                ResetSearch();
            }

            topPanel.Hide();
            topLine.Hide();
            bottomPanel.Hide();
            bottomLine.Hide();
        }

        #endregion

        #region UI events

        private void ColumnVisibilityChanged(object sender, System.EventArgs e)
        {
            var checkbox = ((CheckBox)sender);
            _currentPage.ChangeColumnVisibility(checkbox.Name, checkbox.Checked);
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            topPanel.Visible = false;
            topLine.Visible = false;
            searchPanel.ShowSearch();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SetShadow(e);
        }
        
        private void btnActions_Click(object sender, EventArgs e)
        {
            var actionsBuilder = DashboardCell.CreateActionMenuBuilder(_popup);
            actionsBuilder.AddCloseButton(Close);
            if (!_isConfigurationMode)
            {
                if (_currentPage != null && _currentPage.AllowColumnChange)
                {
                    actionsBuilder.AddGridSettingsButton(ShowGridSettings);
                }

                if (PopOutVisible)
                {
                    actionsBuilder.AddPopOutButton(PopOver);
                }

                if (_currentPage != null)
                {
                    actionsBuilder.AddActions(_currentPage.ActionItems, ActionHandler);
                }
            }
            actionsBuilder.AddResizeButton(ShowResizeOptions);

            _popup.Show((Control)sender, Infragistics.Win.Peek.PeekLocation.BelowItem);
            MethodInfo methodInfo = typeof(DropDownManager).GetMethod("GetManager", System.Reflection.BindingFlags.NonPublic | BindingFlags.Static);
            DropDownManager manager = methodInfo.Invoke(null, new object[] { (Control)sender }) as DropDownManager;
            FieldInfo fieldInfo = typeof(DropDownManager).GetField("eatMouseMessageOnAutoCloseup", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(manager, false);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            var actionsBuilder = DashboardCell.CreateActionMenuBuilder(_popup);
            if (_currentPage != null && _currentPage.FilterOptions.Any())
            {
                actionsBuilder.AddFilters(_currentPage.FilterOptions, FilterHandler);
            }

            _popup.Show((Control)sender, Infragistics.Win.Peek.PeekLocation.BelowItem);
        }

        private void OnQueryChanged(object sender, string e)
        {
            QueryChanged?.Invoke(sender, e);
        }

        private void ucCellContainer_DpiChangedAfterParent(object sender, EventArgs e)
        {
            SetIcons();
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            if (_isMaximized)
            {
                _isMaximized = false;
                Minimizing?.Invoke(this, e);
                this.toolTip.SetToolTip(this.btnFullScreen, _maximizeToolTipLabel);
            }
            else
            {
                _isMaximized = true;
                Maximizing?.Invoke(this, e);
                this.toolTip.SetToolTip(this.btnFullScreen, _minimizeToolTipLabel);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNewClicked?.Invoke(this, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, e);
        }
        
        private void OnEnableChanged(object sender, bool e)
        {
            this.titlePanel.Enabled = e;
            this.btnFilter.Enabled = e;
            this.btnFullScreen.Enabled = e;
            this.btnSearch.Enabled = e;
            this.btnActions.Enabled = e;
            this.mainContainer.Enabled = e;
            this.bottomPanel.Enabled = e;
        }

        private void OnActionsEnabled(object sender, EventArgs e)
        {
            this.btnActions.Enabled = true;
        }

        #endregion

        #region Draw shadow
        private void DrawShadowLine(PaintEventArgs e, Color color, int thickness, Point[] points)
        {
            using (var pen = new Pen(color, thickness))
            {
                e.Graphics.DrawLine(pen, points[0].X, points[0].Y, points[1].X, points[1].Y);
            }
        }

        private Point[] GetBottomLine(int thickness, int number)
        {
            return new[]
            {
                new Point(container.Bounds.X + number * thickness,
                    container.Bounds.Y + container.Bounds.Height + number * thickness),
                new Point(container.Bounds.X + container.Bounds.Width - number * thickness,
                    container.Bounds.Y + container.Bounds.Height + number * thickness)
            };
        }

        private Point[] GetRightLine(int thickness, int number)
        {
            return new[]
            {
                new Point(container.Bounds.X + container.Bounds.Width + number * thickness,
                    container.Bounds.Y - number * thickness),
                new Point(container.Bounds.X + container.Bounds.Width + number * thickness,
                    container.Bounds.Y + container.Bounds.Height - number * thickness)
            };
        }

        private Point[] GetTopLine(int thickness, int number)
        {
            return new[]
            {
                new Point(container.Bounds.X + number * thickness,
                    container.Bounds.Y - number * thickness),
                new Point(container.Bounds.X + container.Bounds.Width - number * thickness,
                    container.Bounds.Y - number * thickness)
            };
        }

        private Point[] GetLeftLine(int thickness, int number)
        {
            return new[]
            {
                new Point(container.Bounds.X - number * thickness,
                    container.Bounds.Y + number * thickness),
                new Point(container.Bounds.X - number * thickness,
                    container.Bounds.Y + container.Bounds.Height - number * thickness)
            };
        }

        private void SetShadow(PaintEventArgs e)
        {
            var thickness = LogicalToDeviceUnits(1);

            DrawShadowLine(e, Color.FromArgb(226, 226, 226), thickness, GetBottomLine(thickness, 1));
            DrawShadowLine(e, Color.FromArgb(236, 236, 236), thickness, GetBottomLine(thickness, 2));
            DrawShadowLine(e, Color.FromArgb(245, 245, 245), thickness, GetBottomLine(thickness, 3));
            DrawShadowLine(e, Color.FromArgb(250, 250, 250), thickness, GetBottomLine(thickness, 4));
            DrawShadowLine(e, Color.FromArgb(253, 253, 253), thickness, GetBottomLine(thickness, 5));

            DrawShadowLine(e, Color.FromArgb(232, 232, 232), thickness, GetRightLine(thickness, 1));
            DrawShadowLine(e, Color.FromArgb(245, 245, 245), thickness, GetRightLine(thickness, 2));
            DrawShadowLine(e, Color.FromArgb(253, 253, 253), thickness, GetRightLine(thickness, 3));
            
            DrawShadowLine(e, Color.FromArgb(248, 248, 248), thickness, GetTopLine(thickness, 1));
            DrawShadowLine(e, Color.FromArgb(251, 251, 251), thickness, GetTopLine(thickness, 2));
            DrawShadowLine(e, Color.FromArgb(253, 253, 253), thickness, GetTopLine(thickness, 3));

            DrawShadowLine(e, Color.FromArgb(243, 243, 243), thickness, GetLeftLine(thickness, 1));
            DrawShadowLine(e, Color.FromArgb(248, 248, 248), thickness, GetLeftLine(thickness, 2));
            DrawShadowLine(e, Color.FromArgb(253, 253, 253), thickness, GetLeftLine(thickness, 3));
        }

        #endregion

        public class ResizeOptionsEventArgs : EventArgs
        {
            public List<Size> ResizeOptions { get; set; }
            public bool Handled { get; set; }
        }

        public class ResizingEventArgs : EventArgs
        {
            public Size NewSize { get; set; }
        }

    }
}
