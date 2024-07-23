using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.Elasticsearch;
using FWBS.OMS.UI.UserControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Elasticsearch.GlobalSearch
{
    public partial class ucGlobalSearch : UserControl
    {
        private ucEmptyResult _emptyResult;
        private readonly int _maximumSuggestsAmount;
        
        private SortOrder _sortOrder;
        private Dictionary<string, string> _columnToFieldMapping;
        private ActionButtonsBuilder _actionButtonsBuilder;

        public ucGlobalSearch()
        {
            InitializeComponent();

            searchPagination.SetPageSizes(new int[] { 50, 100, 250 }, Session.CurrentSession.CurrentUser.PaginationPageSize);
            searchPagination.UpdatePageControls(0);
            PageSize = searchPagination.PageSize;
            CurrentPage = searchPagination.CurrentPage;
            searchPagination.onPageSettingsChanged += PageSettingsChanged;

            EntityTypes = new List<EntityTypeEnum>();
            FieldsFilter = new List<SearchFilter.FieldFilterItem>();
            this.columnName.HeaderText = Session.CurrentSession.Resources.GetResource("Name", "Name", string.Empty).Text;
            this.columnDateModified.HeaderText = Session.CurrentSession.Resources.GetResource("DateModified", "Date Modified", string.Empty).Text;
            this.columnDocumentType.HeaderText = Session.CurrentSession.Resources.GetResource("DocumentType", "Document Type", string.Empty).Text;
            this.columnDetails.HeaderText = Session.CurrentSession.Resources.GetResource("Details", "Details", string.Empty).Text;
            this.Tag = Session.CurrentSession.Resources.GetResource("GlobalSearch", "Global Search", string.Empty).Text;

            var factory = new SearchBuilderFactory();
            _searchBuilder = factory.CreateSearchBuilder(false);
            searchBox.FilterChanged += FilterChanged;
            searchBox.QueryChanged += ChangeQuery;
            searchBox.SearchStarted += SearchCalled;

            _maximumSuggestsAmount = Session.CurrentSession.MaximumSuggestsAmount;
            _actionButtonsBuilder = new ActionButtonsBuilder(
                columnOmsType.Index,
                columnFileId.Index,
                columnClientId.Index,
                columnContactId.Index,
                columnDocumentId.Index,
                columnAssociateId.Index,
                columnMatterSphereId.Index);

            InitializeFieldMapping();

            // disable sorting for NON-elastic search only
            if (!Session.CurrentSession.IsESSearchConfigured)
                DisableColumnsSorting();
            // enable sorting by summary if enabled
            else if (Session.CurrentSession.IsSearchSummaryFieldEnabled)
                columnDetails.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        private void InitializeFieldMapping()
        {
            // Warning! Field's name used in ElasticSearch request is case-sensitive
            _columnToFieldMapping = new Dictionary<string, string>
            {
                [columnIcon.Name]         = "objecttype",
                [columnName.Name]         = "title.raw",
                [columnDateModified.Name] = "modifieddate",
                [columnDocumentType.Name] = "documentType",
                [columnOmsType.Name]      = "objecttype",
                [columnDetails.Name]      = "summary.raw"
            };
        }

        private void DisableColumnsSorting()
        {
            foreach (var column in dataView.Columns.OfType<DataGridViewTextBoxColumn>())
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void ClearResults()
        {
            searchBox.ClearQuery();
            Query = string.Empty;
            dataView.Rows.Clear();
            ResetPages();
            ShowResults();
        }

        #region UI Events

        private void ucGlobalSearch_DpiChangedAfterParent(object sender, EventArgs e)
        {
            UpdateDocumentIcons();
        }

        private void dataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                InvokeOpenAction(dataView.Rows[e.RowIndex]);
            }
        }

        private void dataView_SortChanged(object sender, Windows.DataGridViewEx.SortDataEventArgs e)
        {
            _sortOrder = e.Order;
            ResetPages();
            SearchAsync();
        }

        private void dataView_MouseDown(object sender, MouseEventArgs e)
        {
            var hitInfo = dataView.HitTest(e.X, e.Y);
            if (hitInfo.Type == DataGridViewHitTestType.Cell)
            {
                dataView.Rows[hitInfo.RowIndex].Selected = true;
            }
        }

        private void dataView_MouseUp(object sender, MouseEventArgs e)
        {
            var hitInfo = dataView.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && hitInfo.Type == DataGridViewHitTestType.Cell && e.Clicks == 1)
            {
                var row = dataView.Rows[hitInfo.RowIndex];
                var buttons = _actionButtonsBuilder.BuildContextMenuItems(row);
                if (buttons != null && buttons.Count > 0)
                {
                    ContextMenu contextMenu = new ContextMenu();
                    foreach (var button in buttons)
                    {
                        contextMenu.MenuItems.Add(button);
                    }
                    contextMenu.Show(dataView, e.Location);
                }
            }
        }

        private void dataView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter && dataView.Focused && dataView.CurrentRowIndex > -1)
            {
                var row = dataView.SelectedRows[0];
                InvokeOpenAction(row);
            }
        }

        private void InvokeOpenAction(DataGridViewRow row)
        {
            var entityType = row.Cells[columnOmsType.Index].Value.ToString();
            ActionProvider provider = new ActionProvider();
            var id = ConvertDef.ToInt64(Convert.ToString(row.Cells[columnMatterSphereId.Index].Value), 0);
            if (id > 0)
            {
                provider.GetAction($"open_{entityType}")?.Invoke(id);
            }
        }

        #endregion UI Events

        #region Private methods

        private ImageList BuildImageList(List<string> extensions)
        {
            var imageList = FWBS.OMS.UI.Windows.Images.GetCommonIcons(DeviceDpi, true, 16);
            var imageCollection = new ImageList
            {
                ImageSize = imageList.ImageSize
            };

            imageCollection.Images.Add("CONTACT", imageList.Images["contact"]);
            imageCollection.Images.Add("CLIENT", imageList.Images["client"]);
            imageCollection.Images.Add("FILE", imageList.Images["file"]);
            imageCollection.Images.Add("NOTE", imageList.Images["note"]);
            imageCollection.Images.Add("APPOINTMENT", imageList.Images["appointment"]);
            imageCollection.Images.Add("ADDRESS", imageList.Images["address"]);
            imageCollection.Images.Add("TASK", imageList.Images["task"]);
            imageCollection.Images.Add("ASSOCIATE", imageList.Images["associate"]);

            foreach (var extension in extensions)
            {
                Image icon = FWBS.Common.IconReader.GetFileIcon($"12345.{extension}", FWBS.Common.IconReader.IconSize.Small, false).ToBitmap();
                imageCollection.Images.Add(extension, icon);
            }

            return imageCollection;
        }

        private string GetIconKey(string extension, string type)
        {
            type = type.ToUpper();
            if (type == "EMAIL" || type == "DOCUMENT" || type == "PRECEDENT")
            {
                return extension.Trim('.');
            }

            return type;
        }

        private void FilterChanged(object sender, FilterChangedEventArgs e)
        {
            EntityTypes.Clear();
            EntityTypes.AddRange(e.SelectedTypes);

            FieldsFilter.Clear();
            foreach (var docOrEmail in e.SelectedTypes.Where(x => x == EntityTypeEnum.Document || x == EntityTypeEnum.Email))
            {
                if (!string.IsNullOrEmpty(e.DocumentsDateRange.Item1))
                    FieldsFilter.Add(new SearchFilter.FieldFilterItem("documentStartDate", e.DocumentsDateRange.Item1) 
                        { EntityType = docOrEmail, Operator = ComparisonOperator.GreaterOrEqual, TargetField = "modifieddate" });

                if (!string.IsNullOrEmpty(e.DocumentsDateRange.Item2))
                    FieldsFilter.Add(new SearchFilter.FieldFilterItem("documentEndDate", e.DocumentsDateRange.Item2) 
                        { EntityType = docOrEmail, Operator = ComparisonOperator.LessOrEqual, TargetField = "modifieddate" });
            }
        }

        private void ResetPages()
        {
            searchPagination.ResetPages(0);
            PageSize = searchPagination.PageSize;
            CurrentPage = searchPagination.CurrentPage;
        }

        private void UpdateDocumentIcons()
        {
            var extensions = new List<string>();

            foreach (DataGridViewRow row in dataView.Rows)
            {
                extensions.Add(row.Cells[columnExtension.Index].Value.ToString());
            }

            extensions = extensions.Where(ext => !string.IsNullOrWhiteSpace(ext))
                .Distinct()
                .ToList();

            var icons = BuildImageList(extensions);

            foreach (DataGridViewRow row in dataView.Rows)
            {
                row.Cells[columnIcon.Index].Value = icons.Images[GetIconKey(row.Cells[columnExtension.Index].Value.ToString(), row.Cells[columnOmsType.Index].Value.ToString())];
            }
        }

        #endregion Private methods

        #region Paggination

        private bool _pageChanged;

        private void PageSettingsChanged(object sender, EventArgs e)
        {
            var control = sender as ucSearchPagination;
            if (control == null)
                return;

            PageSize = control.PageSize;
            CurrentPage = control.CurrentPage;
            _pageChanged = true;
            Session.CurrentSession.CurrentUser.PaginationPageSize = PageSize;
            SearchAsync();
        }

        #endregion Paggination

        #region Search

        private ISearchBuilder _searchBuilder;
        public string Query { get; private set; }
        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public List<EntityTypeEnum> EntityTypes { get; private set; }
        public List<SearchFilter.FieldFilterItem> FieldsFilter { get; private set; }

        public void SetResult(SearchResult result)
        {
            this.BeginInvoke((MethodInvoker)(() => { InsertResult(result); }));
        }

        private void InsertResult(Common.Elasticsearch.SearchResult result)
        {
            dataView.Rows.Clear();

            var icons = BuildImageList(result.Documents.Where(doc => !string.IsNullOrWhiteSpace(doc.Extension))
                .Select(doc => doc.Extension.Trim('.'))
                .Distinct().ToList());

            for (int i = 0; i < result.Documents.Count; i++)
            {
                dataView.Rows.Add();
                ResponseItem document = result.Documents[i];
                DataGridViewCellCollection cells = dataView.Rows[i].Cells;
                cells[columnIcon.Index].Value = icons.Images[GetIconKey(document.Extension, document.ObjectType)];
                cells[columnIcon.Index].ToolTipText = Session.CurrentSession.Resources.GetResource(document.ObjectType, document.ObjectType, string.Empty).Text;

                cells[columnName.Index].Value = document.Title;
                cells[columnName.Index].ToolTipText = RemoveHighlight(document.Title);
                
                var dateValue = document.ModifiedDate > DateTime.MinValue
                    ? $"{document.ModifiedDate.ToString(Session.CurrentSession.DefaultDateTimeFormat.ShortDatePattern)} {document.ModifiedDate.ToString(Session.CurrentSession.DefaultDateTimeFormat.ShortTimePattern)}" 
                    : null;
                cells[columnDateModified.Index].Value = dateValue;
                cells[columnDetails.Index].Value = document.Summary;
                cells[columnDetails.Index].ToolTipText = RemoveHighlight(document.Summary);

                cells[columnDocumentType.Index].Value = document.DocumentType;
                cells[columnOmsType.Index].Value = document.ObjectType;
                cells[columnFileId.Index].Value = document.FileId;
                cells[columnClientId.Index].Value = document.ClientId;
                cells[columnContactId.Index].Value = document.ContactId;
                cells[columnDocumentId.Index].Value = document.DocumentId;
                cells[columnAssociateId.Index].Value = document.AssociateId;
                cells[columnExtension.Index].Value = !string.IsNullOrWhiteSpace(document.Extension)
                    ? document.Extension.Trim('.')
                    : null;
                cells[columnMatterSphereId.Index].Value = document.MatterSphereId;
            }

            if (_pageChanged)
            {
                searchPagination.UpdatePageControls(result.TotalDocuments);
                _pageChanged = false;
            }
            else
            {
                searchPagination.ResetPages(result.TotalDocuments);
            }

            if (!result.Documents.Any() && dataView.Visible)
            {
                ShowEmptyPage();
            }

            if (result.Documents.Any() && !dataView.Visible)
            {
                ShowResults();
            }
        }

        private void ShowResults()
        {
            if (_emptyResult != null && _emptyResult.Visible)
            {
                _emptyResult.Visible = false;
                this.Controls.Remove(_emptyResult);
                this.Controls.Remove(TitleContainer);

                dataView.Visible = true;
                searchPagination.Visible = true;
                this.Controls.Add(dataView);
                this.Controls.Add(searchPagination);
                this.Controls.Add(TitleContainer);
            }
        }

        private void ShowEmptyPage()
        {
            if (dataView.Visible)
            {
                dataView.Visible = false;
                searchPagination.Visible = false;
                this.Controls.Remove(dataView);
                this.Controls.Remove(searchPagination);
                this.Controls.Remove(TitleContainer);

                _emptyResult = new ucEmptyResult()
                {
                    Dock = DockStyle.Fill,
                };

                this.Controls.Add(_emptyResult, true);
                this.Controls.Add(TitleContainer);
            }
        }

        public async void SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(Query))
                return;

            var request = new SearchFilter(Query)
            {
                PageInfo = new SearchFilter.PageData(
                    page: CurrentPage - 1,
                    size: PageSize),
                TypesFilter = EntityTypes.ToList(),
                FieldsFilter = FieldsFilter.ToList(),
                WithHighlights = true
            };
            if (dataView.SortedColumn != null)
            {
                request.SortInfo = new SearchFilter.SortData()
                {
                    Field = _columnToFieldMapping[dataView.SortedColumn.Name],
                    Order = _sortOrder.ToElasticsearchString()
                };
            }
            dataView.Enabled = false;
            searchPagination.Enabled = false;
            searchBox.ShowProgress(true);
            try
            {
                var result = await System.Threading.Tasks.Task.Run(() => _searchBuilder.Search(request));
                SetResult(result);
                searchBox.ShowProgress(false);
            }
            catch (Exception e)
            {
                searchBox.ShowProgress(false);
                ShowError(e);
            }
            finally
            {
                dataView.Enabled = true;
                searchPagination.Enabled = true;
            }
        }

        private void ShowError(Exception ex)
        {
            this.BeginInvoke((MethodInvoker)(()=> { ErrorBox.Show(ParentForm, ex); }));
        }

        private async void GetSuggestsAsync(CancellationToken token)
        {
            try
            {
                var result = await System.Threading.Tasks.Task.Run(
                    () => _searchBuilder.GetSuggests(Query, _maximumSuggestsAmount), token);
                if (!token.IsCancellationRequested)
                {
                    searchBox.SetSuggests(result.ToArray());
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private void ChangeQuery(object sender, QueryChangedEventArgs queryChangedEventArgs)
        {
            if (queryChangedEventArgs.Query.Length > 0)
            {
                Query = queryChangedEventArgs.Query;
                GetSuggestsAsync(queryChangedEventArgs.CancellationToken);
            }
            else
            {
                QueryRemoved(sender, queryChangedEventArgs);
            }
        }

        private void SearchCalled(object sender, EventArgs e)
        {
            Query = searchBox.Query;
            dataView.Rows.Clear();
            ResetPages();
            SearchAsync();
        }

        private void QueryRemoved(object sender, EventArgs e)
        {
            ClearResults();
        }

        private string RemoveHighlight(string value)
        {
            return value?.Replace("<highlight>", string.Empty)
                .Replace("</highlight>", string.Empty);
        }

        #endregion

    }
}
