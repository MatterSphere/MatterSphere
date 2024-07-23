using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.UI.Elasticsearch;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.Windows
{
    public class ucSearchTextControl : UserControl, ISearchProducer
    {
        private class FilterCheckBox : CheckBox
        {
            protected override bool ShowFocusCues { get { return false; } }
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private const int DefaultDpi = 96;
        private Telerik.WinControls.UI.RadTextBoxControl searchTextBoxControl;
        private System.Windows.Forms.PictureBox titleBarSearchIcon;
        private System.Windows.Forms.PictureBox titleBarClearIcon;
        private FilterCheckBox titleBarFilter;
        private CancellationTokenSource _suggestsCancellationTokenSource;
        private FilterTypesPopup _filterPopup;
        private NavigationPopupContainer _popupContainer;
        private readonly int _minimalSymbolsCountForSuggests;

        public ucSearchTextControl()
        {
            InitializeComponent();
            ChangeTitleBarPanelIcons();

            this.TabStop = false;
            if (Session.CurrentSession.IsConnected)
            {
                searchTextBoxControl.NullText = Session.CurrentSession.Resources.GetResource("cmdSearch", "Search", "").Text.Replace("&", "");
                searchTextBoxControl.MaxDropDownItemCount = Session.CurrentSession.MaximumSuggestsAmount;
                this.Visible = Session.CurrentSession.IsSearchConfigured;
                _minimalSymbolsCountForSuggests = Session.CurrentSession.MinimalSymbolsCountForSuggests;
            }

            CustomizeSearchTextBoxControl();
        }

        private bool IsSearchAllowed
        {
            get { return !searchTextBoxControl.IsReadOnly && searchTextBoxControl.TextLength > 0; }
        }

        private void InitializeFilterPopup()
        {
            _filterPopup = new FilterTypesPopup();
            _filterPopup.LoadSettings();

            _popupContainer = new NavigationPopupContainer(_filterPopup);
            _popupContainer.Closed += PopupContainerClosed;

            FireFilterChanged();
        }

        private void CustomizeSearchTextBoxControl()
        {
            this.searchTextBoxControl.ListElement.Font = this.Font;
            this.searchTextBoxControl.ListElement.ItemHeight = (int) (this.Font.Size * 2);
            this.searchTextBoxControl.ListElement.VisualItemFormatting += ListElement_VisualItemFormatting;
            this.searchTextBoxControl.TextBoxElement.DrawBorder = false;
            this.searchTextBoxControl.TextBoxElement.AutoCompleteDropDown.PopupClosed += AutoCompleteDropDown_PopupClosed;
            this.searchTextBoxControl.TextBoxElement.AutoCompleteDropDown.PopupOpening += AutoCompleteDropDown_PopupOpening;
            this.searchTextBoxControl.TextBoxElement.AutoCompleteDropDown.PopupOpened += AutoCompleteDropDown_PopupOpened;

            var dropDown = (RadTextBoxAutoCompleteDropDown)this.searchTextBoxControl.TextBoxElement.AutoCompleteDropDown;
            dropDown.AutoSize = true;
            dropDown.SizingMode = SizingMode.None;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                    components = null;
                }
                if (_filterPopup != null)
                {
                    _filterPopup.Dispose();
                    _filterPopup = null;
                }
                _popupContainer = null;
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchTextBoxControl = new Telerik.WinControls.UI.RadTextBoxControl();
            this.titleBarSearchIcon = new System.Windows.Forms.PictureBox();
            this.titleBarClearIcon = new System.Windows.Forms.PictureBox();
            this.titleBarFilter = new FWBS.OMS.UI.Windows.ucSearchTextControl.FilterCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.searchTextBoxControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarSearchIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarClearIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // searchTextBoxControl
            // 
            this.searchTextBoxControl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.searchTextBoxControl.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.searchTextBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTextBoxControl.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.searchTextBoxControl.Location = new System.Drawing.Point(32, 0);
            this.searchTextBoxControl.Margin = new System.Windows.Forms.Padding(0);
            this.searchTextBoxControl.Name = "searchTextBoxControl";
            this.searchTextBoxControl.Padding = new System.Windows.Forms.Padding(3);
            // 
            // 
            // 
            this.searchTextBoxControl.RootElement.ControlBounds = new System.Drawing.Rectangle(32, 0, 125, 20);
            this.searchTextBoxControl.ShowNullText = true;
            this.searchTextBoxControl.Size = new System.Drawing.Size(224, 32);
            this.searchTextBoxControl.TabIndex = 1;
            this.searchTextBoxControl.WordWrap = false;
            this.searchTextBoxControl.TextChanged += new System.EventHandler(this.searchTextBoxControl_TextChanged);
            this.searchTextBoxControl.Enter += new System.EventHandler(this.searchTextBoxControl_Enter);
            this.searchTextBoxControl.Leave += new System.EventHandler(this.searchTextBoxControl_Leave);
            this.searchTextBoxControl.MouseEnter += new System.EventHandler(this.searchTextBoxControl_MouseEnter);
            this.searchTextBoxControl.MouseLeave += new System.EventHandler(this.searchTextBoxControl_MouseLeave);
            this.searchTextBoxControl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.searchTextBoxControl_PreviewKeyDown);
            // 
            // titleBarSearchIcon
            // 
            this.titleBarSearchIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.titleBarSearchIcon.Enabled = false;
            this.titleBarSearchIcon.Image = global::FWBS.OMS.UI.Properties.Resources.titlebar_search_96x;
            this.titleBarSearchIcon.Location = new System.Drawing.Point(0, 0);
            this.titleBarSearchIcon.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarSearchIcon.Name = "titleBarSearchIcon";
            this.titleBarSearchIcon.Size = new System.Drawing.Size(32, 32);
            this.titleBarSearchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.titleBarSearchIcon.TabIndex = 0;
            this.titleBarSearchIcon.TabStop = false;
            this.titleBarSearchIcon.Click += new System.EventHandler(this.titleBarSearchIcon_Click);
            // 
            // titleBarClearIcon
            // 
            this.titleBarClearIcon.Dock = System.Windows.Forms.DockStyle.Right;
            this.titleBarClearIcon.Enabled = false;
            this.titleBarClearIcon.Image = global::FWBS.OMS.UI.Properties.Resources.titlebar_search_close_96x;
            this.titleBarClearIcon.Location = new System.Drawing.Point(256, 0);
            this.titleBarClearIcon.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarClearIcon.Name = "titleBarClearIcon";
            this.titleBarClearIcon.Size = new System.Drawing.Size(32, 32);
            this.titleBarClearIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.titleBarClearIcon.TabIndex = 0;
            this.titleBarClearIcon.TabStop = false;
            this.titleBarClearIcon.Click += new System.EventHandler(this.titleBarClear_Click);
            // 
            // titleBarFilter
            // 
            this.titleBarFilter.Appearance = System.Windows.Forms.Appearance.Button;
            this.titleBarFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(17)))), ((int)(((byte)(76)))));
            this.titleBarFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.titleBarFilter.Dock = System.Windows.Forms.DockStyle.Right;
            this.titleBarFilter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.titleBarFilter.FlatAppearance.BorderSize = 0;
            this.titleBarFilter.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(1)))), ((int)(((byte)(198)))));
            this.titleBarFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(120)))), ((int)(((byte)(193)))));
            this.titleBarFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(1)))), ((int)(((byte)(198)))));
            this.titleBarFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.titleBarFilter.ForeColor = System.Drawing.Color.White;
            this.titleBarFilter.Location = new System.Drawing.Point(288, 0);
            this.titleBarFilter.Margin = new System.Windows.Forms.Padding(0);
            this.titleBarFilter.Name = "titleBarFilter";
            this.titleBarFilter.Size = new System.Drawing.Size(32, 32);
            this.titleBarFilter.TabIndex = 0;
            this.titleBarFilter.TabStop = false;
            this.titleBarFilter.UseVisualStyleBackColor = false;
            this.titleBarFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // ucSearchTextControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.Controls.Add(this.searchTextBoxControl);
            this.Controls.Add(this.titleBarSearchIcon);
            this.Controls.Add(this.titleBarClearIcon);
            this.Controls.Add(this.titleBarFilter);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Name = "ucSearchTextControl";
            this.Size = new System.Drawing.Size(320, 32);
            ((System.ComponentModel.ISupportInitialize)(this.searchTextBoxControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarSearchIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.titleBarClearIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                searchTextBoxControl.BackColor = value;
                titleBarClearIcon.BackColor = value;
                titleBarSearchIcon.BackColor = value;
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            ChangeTitleBarPanelIcons();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!this.searchTextBoxControl.Focused)
                SetControlsBackColor(Color.White);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.searchTextBoxControl.Focused)
                SetControlsBackColor(this.BackColor);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var autoCompleteDropDown = (RadTextBoxAutoCompleteDropDown)this.searchTextBoxControl.TextBoxElement.AutoCompleteDropDown;
            ResizeAutoCompleteDropDown(autoCompleteDropDown);
        }

        #region Events

        private void ListElement_VisualItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            if (args.VisualItem.Active)
            {
                args.VisualItem.BackColor = Color.FromArgb(208, 224, 242);
                args.VisualItem.DrawBorder = false;
                args.VisualItem.GradientStyle = GradientStyles.Solid;
            }
            else
            {
                args.VisualItem.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                args.VisualItem.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            }
        }

        private void AutoCompleteDropDown_PopupClosed(object sender, RadPopupClosedEventArgs args)
        {
            ClearAutoSuggests();
        }

        private void AutoCompleteDropDown_PopupOpening(object sender, System.ComponentModel.CancelEventArgs args)
        {
            var autoCompleteDropDown = (RadTextBoxAutoCompleteDropDown)sender;
            ResizeAutoCompleteDropDown(autoCompleteDropDown);
        }

        private void AutoCompleteDropDown_PopupOpened(object sender, EventArgs args)
        {
            var autoCompleteDropDown = (RadTextBoxAutoCompleteDropDown)sender;
            ResizeAutoCompleteDropDown(autoCompleteDropDown);
        }

        private void titleBarClear_Click(object sender, EventArgs e)
        {
            ClearAutoSuggests();
            if (!searchTextBoxControl.IsReadOnly)
                this.searchTextBoxControl.Clear();
        }

        private void titleBarSearchIcon_Click(object sender, EventArgs e)
        {
            if (IsSearchAllowed)
                Search(searchTextBoxControl.Text);
        }

        private void searchTextBoxControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return && IsSearchAllowed)
                Search(searchTextBoxControl.Text);
        }

        private void searchTextBoxControl_TextChanged(object sender, EventArgs e)
        {
            titleBarSearchIcon.Enabled = titleBarClearIcon.Enabled = (searchTextBoxControl.TextLength > 0);

            if (_suggestsCancellationTokenSource != null)
            {
                _suggestsCancellationTokenSource.Cancel();
                _suggestsCancellationTokenSource.Dispose();
            }
            _suggestsCancellationTokenSource = new CancellationTokenSource();

            if (searchTextBoxControl.TextLength >= _minimalSymbolsCountForSuggests &&
                !searchTextBoxControl.Text.EndsWith(" "))
            {
                OnQueryChanged(new QueryChangedEventArgs(searchTextBoxControl.Text,
                    _suggestsCancellationTokenSource.Token));
            }
        }

        private void searchTextBoxControl_Enter(object sender, EventArgs e)
        {
            SetControlsBackColor(Color.White);
        }

        private void searchTextBoxControl_Leave(object sender, EventArgs e)
        {
            SetControlsBackColor(this.BackColor);
        }

        private void searchTextBoxControl_MouseEnter(object sender, EventArgs e)
        {
            if (!this.searchTextBoxControl.Focused)
                SetControlsBackColor(Color.White);
        }

        private void searchTextBoxControl_MouseLeave(object sender, EventArgs e)
        {
            if (!this.searchTextBoxControl.Focused)
                SetControlsBackColor(this.BackColor);
        }

        #endregion

        #region Private methods

        private void SetControlsBackColor(Color backColor)
        {
            foreach (Control control in Controls)
            {
                if (control != titleBarFilter)
                    control.BackColor = backColor;
            }
        }

        private void Search(string request)
        {
            ClearAutoSuggests();
            Query = request;

            if (_filterPopup == null)
               InitializeFilterPopup();

            if (!_filterPopup.GetSelectedTypes().Any())
            {
                titleBarFilter.Checked = true;
                btnFilter_Click(titleBarFilter, EventArgs.Empty);
                return;
            }

            OnSearchStarted(EventArgs.Empty);
        }

        private void ClearAutoSuggests()
        {
            this.searchTextBoxControl.AutoCompleteItems.Clear();
        }

        private void ChangeSearchIcon(bool isSearchInProgress)
        {
            var imageName = isSearchInProgress ? "loading" : "titlebar_search";
            ChangeIconByDpi(titleBarSearchIcon, imageName);
        }

        /// <summary>
        /// Change set of icons from titlebar panel 
        /// </summary>
        private void ChangeTitleBarPanelIcons()
        {
            ChangeIconByDpi(titleBarSearchIcon, "titlebar_search");
            ChangeIconByDpi(titleBarClearIcon, "titlebar_search_close");
            this.titleBarFilter.Image = Images.GetCommonIcon(DeviceDpi, "filter_white");
        }

        /// <summary>
        /// Change image to another image in control depending on dpi
        /// </summary>
        /// <param name="pictureBox">control where you need to change the image</param>
        /// <param name="imageName">Image name without dpi in name</param>
        private void ChangeIconByDpi(PictureBox pictureBox, string imageName)
        {
            var imageNameWithDpi = $"{imageName}_{DeviceDpi}x";

            var imageWithDpi = (Bitmap) Properties.Resources.ResourceManager.GetObject(imageNameWithDpi);
            if (imageWithDpi != null)
            {
                pictureBox.Image = imageWithDpi;
            }
            else
            {
                var imageNameWithDefaultDpi = $"{imageName}_{DefaultDpi}x";

                var imageWithDefaultDpi =
                    (Bitmap) Properties.Resources.ResourceManager.GetObject(imageNameWithDefaultDpi);

                if (imageWithDefaultDpi != null)
                {
                    pictureBox.Image = imageWithDefaultDpi;
                }
                else
                {
                    var newImage = (Bitmap) Properties.Resources.ResourceManager.GetObject(imageName);
                    if (newImage != null)
                    {
                        ScaleBitmapLogicalToDevice(ref newImage);
                        pictureBox.Image = newImage;
                    }
                }
            }
        }

        private void ResizeAutoCompleteDropDown(RadTextBoxAutoCompleteDropDown autoCompleteDropDown)
        {
            float wScale = searchTextBoxControl.TextBoxElement.DpiScaleFactor.Width;
            float hScale = searchTextBoxControl.TextBoxElement.DpiScaleFactor.Height;
            autoCompleteDropDown.MinimumSize = new Size((int)(searchTextBoxControl.Size.Width / wScale), 0);
            autoCompleteDropDown.MaximumSize = new Size((int)(searchTextBoxControl.Size.Width / wScale), (int)(autoCompleteDropDown.MaximumSize.Height / hScale));
        }

        #endregion

        #region ISearchProducer
        public event EventHandler SearchStarted;
        public event EventHandler<QueryChangedEventArgs> QueryChanged;
        public string Query { get; private set; }

        public void ClearQuery()
        {
            searchTextBoxControl.Text = null;
            Query = null;
            ShowProgress(false);
            _filterPopup?.Dispose();
            _filterPopup = null;
        }

        public void SetSuggests(string[] suggests)
        {
            BeginInvoke(new Action(() =>
            {
                foreach (var suggest in suggests)
                {
                    if (!searchTextBoxControl.AutoCompleteItems.Contains(suggest))
                        searchTextBoxControl.AutoCompleteItems.Add(suggest);
                }

                var textBoxElement = searchTextBoxControl.TextBoxElement;
                if (!textBoxElement.IsAutoCompleteDropDownOpen && suggests.Length > 0)
                {
                    Point location = textBoxElement.ElementTree.Control.PointToScreen(textBoxElement.ControlBoundingRectangle.Location);
                    location.Y += textBoxElement.ControlBoundingRectangle.Height;
                    textBoxElement.ShowDropDown(location);
                }
            }));
        }

        public void ShowProgress(bool show)
        {
            ChangeSearchIcon(show);
            searchTextBoxControl.IsReadOnly = show;
            titleBarFilter.Enabled = !show;

            if (show)
            {
                if (searchTextBoxControl.Text != Query)
                {
                    searchTextBoxControl.TextChanged -= searchTextBoxControl_TextChanged;
                    searchTextBoxControl.Text = Query;
                    searchTextBoxControl.TextChanged += searchTextBoxControl_TextChanged;
                }
                _filterPopup?.SaveSettings();
            }
        }

        #endregion ISearchProducer

        protected virtual void OnSearchStarted(EventArgs e)
        {
            SearchStarted?.Invoke(this, e);
        }

        protected virtual void OnQueryChanged(QueryChangedEventArgs e)
        {
            QueryChanged?.Invoke(this, e);
        }

        #region Type Filter

        public event EventHandler<FilterChangedEventArgs> FilterChanged;

        private void FireFilterChanged()
        {
            FilterChangedEventArgs args = new FilterChangedEventArgs();
            args.SelectedTypes = _filterPopup.GetSelectedTypes();
            args.DocumentsDateRange = _filterPopup.GetDocumentsDateRange();
            FilterChanged?.Invoke(this, args);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (titleBarFilter.Checked)
            {
                if (_filterPopup == null)
                    InitializeFilterPopup();

                _filterPopup.SearchAllowed = IsSearchAllowed;
                _popupContainer.Show(this.titleBarFilter);
            }
        }

        private void PopupContainerClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            if (e.CloseReason != ToolStripDropDownCloseReason.AppClicked || !titleBarFilter.ClientRectangle.Contains(titleBarFilter.PointToClient(MousePosition)))
            {
                titleBarFilter.Checked = false;
            }

            FireFilterChanged();

            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked && !searchTextBoxControl.IsReadOnly && searchTextBoxControl.TextLength > 0)
            {
                Search(searchTextBoxControl.Text);
            }
        }

        #endregion Type filter
    }
}
