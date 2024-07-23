using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.Elasticsearch;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.DataGridViewControls;
using FWBS.OMS.UI.UserControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Elasticsearch
{
    public partial class ElasticsearchControl : UserControl, ISearchConsumer
    {
        private const int _columnHeaderRowIndex = -1;
        private const string _closeSymbol = "╳";

        private FacetsBuilder _facetsBuilder;
        private List<FacetLabel> _facetLabels;
        private ActionButtonsBuilder _actionButtonsBuilder;

        private SortOrder _sortOrder;
        private Dictionary<string, string> _columnToFieldMapping;

        public ElasticsearchControl()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;

            this.btnCloseFilter.Text = _closeSymbol;
            this.btnClosePreview.Text = _closeSymbol;
            SetIcons();

            ChangePreviewVisibility(false);

            searchPagination.SetPageSizes(new int[] {50, 100, 250}, Session.CurrentSession.CurrentUser.PaginationPageSize);
            searchPagination.UpdatePageControls(0);
            PageSize = searchPagination.PageSize;
            CurrentPage = searchPagination.CurrentPage;
            searchPagination.onPageSettingsChanged += PageSettingsChanged;

            EntityTypes = new List<EntityTypeEnum>();
            LinkedEntityFilter = new List<SearchFilter.EntityFilterData>();
            FieldsFilter = new List<SearchFilter.FieldFilterItem>();
            _facetLabels = new List<FacetLabel>();
            _facetsBuilder = new FacetsBuilder(FacetGroupsPanel);
            _actionButtonsBuilder = new ActionButtonsBuilder(
                columnOmsType.Index,
                columnFileId.Index,
                columnClientId.Index,
                columnContactId.Index,
                columnDocumentId.Index,
                columnAssociateId.Index,
                columnMatterSphereId.Index);

            columnActions.GetActions = PopulateActionPopup;

            this.btnClear.Text = Session.CurrentSession.Resources.GetResource("Clear", "Clear", string.Empty).Text;
            this.columnDescription.HeaderText = Session.CurrentSession.Resources.GetResource("Description", "Description", string.Empty).Text;
            this.columnAuthor.HeaderText = Session.CurrentSession.Resources.GetResource("Author", "Author", string.Empty).Text;
            this.columnModified.HeaderText = Session.CurrentSession.Resources.GetResource("Modified", "Modified", string.Empty).Text;
            this.columnDocumentType.HeaderText = Session.CurrentSession.Resources.GetResource("DocumentType", "Document Type", string.Empty).Text;
            this.columnSummary.HeaderText = Session.CurrentSession.Resources.GetResource("Summary", "Summary", string.Empty).Text;
            this.columnActions.HeaderText = Session.CurrentSession.Resources.GetResource("Actions", "Actions", string.Empty).Text;

            btnFilter.Checked = true;
            btnPreview.Checked = false;

            documentPreview.Hide();

            InitializeFieldMapping();

            // disable sorting for NON-elastic search only
            if (!Session.CurrentSession.IsESSearchConfigured)
                DisableColumnsSorting();
            // enable sorting by summary if enabled
            else if (Session.CurrentSession.IsSearchSummaryFieldEnabled)
                columnSummary.SortMode = DataGridViewColumnSortMode.Programmatic;
        }

        private void InitializeFieldMapping()
        {
            // Warning! Fields name used in ElasticSearch request is case-sensitive
            _columnToFieldMapping = new Dictionary<string, string>
            {
                [columnIcon.Name]         = "objecttype",
                [columnDescription.Name]  = "title.raw",
                [columnAuthor.Name]       = "authorType",
                [columnModified.Name]     = "modifieddate",
                [columnDocumentType.Name] = "documentType",
                [columnSummary.Name]      = "summary.raw",
                [columnOmsType.Name]      = "objecttype"
            };
        }

        private void DisableColumnsSorting()
        {
            foreach (var column in dataView.Columns.OfType<DataGridViewTextBoxColumn>())
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        internal void ResetResults()
        {
            SetResult(new SearchResult());
            ResetPages();
            RemoveEntityFilter();
            LinkedEntityFilter.Clear();
            RemoveFilterLabels();

            onSearchReseted?.Invoke(this, EventArgs.Empty);
        }

        private void ChangeFilterVisibility(bool visible)
        {
            filterPanel.Visible = visible;
            rightPanelContainer.Visible = splitter.Visible = previewPanel.Visible || visible;
        }

        private void ChangePreviewVisibility(bool visible)
        {
            previewPanel.Visible = visible;
            rightPanelContainer.Visible = splitter.Visible = filterPanel.Visible || visible;
            
            if (visible)
            {
                UpdatePreview();
            }
        }

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

        private void SetSortInfo()
        {
            if (dataView.SortedColumn != null)
            {
                SortField = _columnToFieldMapping[dataView.SortedColumn.Name];
                SortOrder = _sortOrder.ToElasticsearchString();
            }
        }

        #region UI events
        private void btnCloseFilter_Click(object sender, EventArgs e)
        {
            btnFilter.Checked = false;
            ChangeFilterVisibility(false);
        }

        private void btnClosePreview_Click(object sender, EventArgs e)
        {
            btnPreview.Checked = false;
            ChangePreviewVisibility(false);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (btnPreview.Checked)
            {
                btnFilter.Checked = false;
                ChangeFilterVisibility(false);
                ChangePreviewVisibility(true);
            }
            else
            {
                ChangePreviewVisibility(false);
            }
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (btnFilter.Checked)
            {
                btnPreview.Checked = false;
                ChangePreviewVisibility(false);
                ChangeFilterVisibility(true);
            }
            else
            {
                ChangeFilterVisibility(false);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CloseAllFilterLabels();
        }

        private void dataView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = (sender as DataGridView);
            if (dataGridView != null)
            {
                dataGridView.Cursor = e.ColumnIndex == columnActions.Index && e.RowIndex > _columnHeaderRowIndex
                    ? Cursors.Hand
                    : Cursors.Default;
            }
        }

        private void ElasticsearchControl_DpiChangedAfterParent(object sender, EventArgs e)
        {
            SetIcons();
            UpdateDocumentIcons();
        }

        private void SetIcons()
        {
            this.btnFilter.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "filter");
            this.btnPreview.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "preview");
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
                row.Cells[columnIcon.Index].Value =
                    icons.Images[GetIconKey(row.Cells[columnExtension.Index].Value.ToString(), row.Cells[columnOmsType.Index].Value.ToString())];
            }
        }

        private void dataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > _columnHeaderRowIndex)
            {
                InvokeOpenAction(dataView.Rows[e.RowIndex]);
            }
        }

        private void dataView_SortChanged(object sender, DataGridViewEx.SortDataEventArgs e)
        {
            _sortOrder = e.Order;
            ResetPages();
            SetSortInfo();
            StartSearch();
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
            if (e.Button == MouseButtons.Right && e.Clicks == 1 && hitInfo.Type == DataGridViewHitTestType.Cell)
            {
                var row = dataView.Rows[hitInfo.RowIndex];
                var buttons = _actionButtonsBuilder.BuildContextMenuItems(row);
                ContextMenu _popup = new ContextMenu();
                if (buttons != null && buttons.Any())
                {
                    foreach (var button in buttons)
                    {
                        _popup.MenuItems.Add(button);
                    }
                }

                _popup.Show(dataView, e.Location);
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
            var id = FWBS.Common.ConvertDef.ToInt64(Convert.ToString(row.Cells[columnMatterSphereId.Index].Value), 0);
            if (id > 0)
            {
                provider.GetAction($"open_{entityType}")?.Invoke(id);
            }
        }

        #endregion UI events

        #region ISearchConsumer

        public event EventHandler onSearchOptionChanged;
        public event EventHandler onSearchReseted;

        public int PageSize { get; private set; }
        public int CurrentPage { get; private set; }
        public SearchFilter.EntityFilterData EntityFilter { get; private set; }
        public List<SearchFilter.EntityFilterData> LinkedEntityFilter { get; private set; }
        public List<EntityTypeEnum> EntityTypes { get; private set; }
        public List<SearchFilter.FieldFilterItem> FieldsFilter { get; private set; }
        public string SortField { get; private set; }
        public string SortOrder { get; private set; }

        public void SetResult(Common.Elasticsearch.SearchResult result)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => { InsertResult(result); }));
            else
                InsertResult(result);
        }

        private void InsertResult(Common.Elasticsearch.SearchResult result)
        {
            dataView.Rows.Clear();

            var icons = BuildImageList(result.Documents.Where(doc => !string.IsNullOrWhiteSpace(doc.Extension))
                .Select(doc => doc.Extension.Trim('.'))
                .Distinct().ToList());

            for (int i = 0; i < result.Documents.Count; i++)
            {
                ResponseItem document = result.Documents[i];
                DataGridViewCellCollection cells = dataView.Rows[dataView.Rows.Add()].Cells;
                cells[columnIcon.Index].Value = icons.Images[GetIconKey(document.Extension, document.ObjectType)];
                cells[columnIcon.Index].ToolTipText = Session.CurrentSession.Resources.GetResource(document.ObjectType, document.ObjectType, string.Empty).Text;
                cells[columnDescription.Index].Value = document.Title;
                cells[columnDescription.Index].ToolTipText = RemoveHighlight(document.Title);
                cells[columnAuthor.Index].Value = document.Author;
                cells[columnAuthor.Index].ToolTipText = RemoveHighlight(document.Author);

                var date = document.ModifiedDate > DateTime.MinValue
                    ? $"{document.ModifiedDate.ToString(Session.CurrentSession.DefaultDateTimeFormat.ShortDatePattern)} {document.ModifiedDate.ToString(Session.CurrentSession.DefaultDateTimeFormat.ShortTimePattern)}"
                    : null;
                cells[columnModified.Index].Value = date;
                cells[columnModified.Index].ToolTipText = RemoveHighlight(date);
                cells[columnDocumentType.Index].Value = document.DocumentType;
                cells[columnSummary.Index].Value = document.Summary;
                cells[columnSummary.Index].ToolTipText = RemoveHighlight(document.Summary);

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

            _facetsBuilder.Build(result.Aggregations, FacetHandler, _facetLabels);
            btnClear.Enabled = flpFilterLabels.Controls.Any();

            UpdatePreview();
        }

        public void SetEntity(IOMSType entity)
        {
            if (entity != null)
            {
                RemoveEntityFilter();
                LinkedEntityFilter.Clear();
                var entityData = GetEntityType(entity);
                if (entityData != null)
                {
                    AddEntityFilter(entityData);
                    if (entityData.EntityType == EntityTypeEnum.Client)
                    {
                        foreach (Contact contact in ((Client)entity).Contacts)
                        {
                            AddLinkedEntityFilter(GetEntityType(contact));
                        }
                    }
                }
            }
            dataView.Rows.Clear();
            ResetPages();
        }

        public void StartSearch()
        {
            onSearchOptionChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ShowError(Exception exception)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)(() => { ErrorBox.Show(TopLevelControl, exception); }));
            else
                ErrorBox.Show(TopLevelControl, exception);
        }

        private string RemoveHighlight(string value)
        {
            return value?.Replace("<highlight>", string.Empty)
                .Replace("</highlight>", string.Empty);
        }

        #endregion ISearchConsumer

        #region Paggination

        private bool _pageChanged;

        private void PageSettingsChanged(object sender, EventArgs e)
        {
            var control = sender as ucSearchPagination;
            if (control == null)
            {
                return;
            }

            PageSize = control.PageSize;
            CurrentPage = control.CurrentPage;
            _pageChanged = true;
            Session.CurrentSession.CurrentUser.PaginationPageSize = PageSize;
            onSearchOptionChanged?.Invoke(this, e);
        }

        #endregion Paggination

        #region FacetLabels

        private void FacetHandler(object sender, EventArgs args)
        {
            var item = sender as ucFacetItem;
            if (item == null)
            {
                return;
            }

            if (item.Checked)
            {
                AddFieldFilter(item.Key, item.Field, item.Value);
            }
            else
            {
                var count = flpFilterLabels.Controls.Count;
                int? index = null;
                for (int i = 0; i < count; i++)
                {
                    var facetLabel = flpFilterLabels.Controls[i] as ucFilterLabel;
                    if (facetLabel != null && facetLabel.Key == item.Key)
                    {
                        index = i;
                        break;
                    }
                }

                if (index.HasValue)
                {
                    RemoveFilterLabelControl(flpFilterLabels.Controls[index.Value] as ucFilterLabel);
                }
            }

            SendNotification();
        }

        private void CloseFacetLabel(object sender, EventArgs args)
        {
            var filterLabel = sender as ucFilterLabel;
            if (filterLabel == null)
            {
                return;
            }

            RemoveFilterLabelControl(filterLabel);
            SendNotification();
        }

        private void AddFieldFilter(Guid key, string field, string value)
        {
            var facetLabel = new ucFilterLabel
            {
                Key = key,
                Text = value
            };

            facetLabel.onClosed += CloseFacetLabel;
            AddControlToFilterLabels(facetLabel);

            _facetLabels.Add(new FacetLabel(key, field, value));
            FieldsFilter.Add(new SearchFilter.FieldFilterItem(field, value));
        }

        private void RemoveFieldFilter(Guid key)
        {
            var facetLabel = _facetLabels.First(lbl => lbl.Key == key);
            _facetLabels.Remove(facetLabel);

            var field = FieldsFilter.First(fld => fld.Field == facetLabel.Field && fld.Value == facetLabel.Value);
            FieldsFilter.Remove(field);
        }

        private void RemoveFilterLabelControl(ucFilterLabel filterLabel)
        {
            filterLabel.onClosed -= CloseFacetLabel;
            RemoveControlFromFilterLabels(filterLabel);

            RemoveFieldFilter(filterLabel.Key);
        }

        private void RemoveFilterLabels()
        {
            if (!flpFilterLabels.Controls.Any())
            {
                return;
            }

            var count = flpFilterLabels.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                var facetLabel = flpFilterLabels.Controls[0] as ucFilterLabel;
                if (facetLabel == null)
                {
                    continue;
                }

                if (facetLabel.IsEntityFilter)
                {
                    RemoveEntityFilter();
                }
                else
                {
                    RemoveFilterLabelControl(facetLabel);
                }
            }
        }

        private void CloseAllFilterLabels()
        {
            RemoveFilterLabels();
            SendNotification();
        }

        private void ResetPages()
        {
            searchPagination.ResetPages(0);
            PageSize = searchPagination.PageSize;
            CurrentPage = searchPagination.CurrentPage;
        }

        private void SendNotification()
        {
            ResetPages();
            onSearchOptionChanged?.Invoke(this, null);
        }

        #endregion FacetLabels

        #region Entity Filter

        private EntityData GetEntityType(IOMSType omsType)
        {
            if (omsType is Associate)
            {
                var associate = omsType as Associate;
                return new EntityData(EntityTypeEnum.Associate, associate.AssocHeading, associate.ID);
            }

            if (omsType is OMSFile)
            {
                var file = omsType as OMSFile;
                return new EntityData(EntityTypeEnum.File, $"{file.Client.ClientNo}: {file.FileNo}", file.ID);
            }

            if (omsType is Client)
            {
                var client = omsType as Client;
                return new EntityData(EntityTypeEnum.Client, $"{client.ClientNo}: {client.ClientName}", client.ID);
            }

            if (omsType is Contact)
            {
                var contact = omsType as Contact;
                return new EntityData(EntityTypeEnum.Contact, contact.Name, contact.ID);
            }

            return null;
        }

        private void AddEntityFilter(EntityData data)
        {
            EntityFilter = new SearchFilter.EntityFilterData(data.EntityType, data.Id);
            var facetLabel = new ucFilterLabel
            {
                Key = Guid.NewGuid(),
                Text = data.Description,
                IsEntityFilter = true
            };

            facetLabel.onClosed += CloseEntityLabel;
            AddControlToFilterLabels(facetLabel);
        }

        private void AddLinkedEntityFilter(EntityData data)
        {
            LinkedEntityFilter.Add(new SearchFilter.EntityFilterData(data.EntityType, data.Id));
        }

        private void CloseEntityLabel(object sender, EventArgs args)
        {
            var filterLabel = sender as ucFilterLabel;
            if (filterLabel == null)
            {
                return;
            }

            RemoveEntityLabelControl(filterLabel);
            SendNotification();
        }

        private void RemoveEntityLabelControl(ucFilterLabel filterLabel)
        {
            filterLabel.onClosed -= CloseFacetLabel;
            RemoveControlFromFilterLabels(filterLabel);
            EntityFilter = null;
        }

        private void RemoveControlFromFilterLabels(Control control)
        {
            flpFilterLabels.Controls.Remove(control);

            if (!flpFilterLabels.Controls.Any())
            {
                lblNoFilter.Visible = true;
            }
        }

        private void AddControlToFilterLabels(Control control)
        {
            flpFilterLabels.Controls.Add(control);
            lblNoFilter.Visible = false;
        }

        private void RemoveEntityFilter()
        {
            for (int i = 0; i < flpFilterLabels.Controls.Count; i++)
            {
                var label = flpFilterLabels.Controls[i] as ucFilterLabel;
                if (label != null && label.IsEntityFilter)
                {
                    RemoveEntityLabelControl(label);
                    break;
                }
            }
        }

        private class EntityData
        {
            public EntityData(EntityTypeEnum entityType, string description, long id)
            {
                EntityType = entityType;
                Description = description;
                Id = id.ToString();
            }

            internal EntityTypeEnum EntityType { get; set; }
            internal string Description { get; set; }
            internal string Id { get; set; }
        }

        #endregion Entity Filter

        #region Actions

        private IEnumerable<UserControls.ContextMenu.ContextMenuButton> PopulateActionPopup(DataGridViewActionsCell cell)
        {
            var row = cell.OwningRow;
            return _actionButtonsBuilder.BuildActionMenuButtons(row);
        }

        #endregion Actions

        #region Previewer

        private void dataView_SelectionChanged(object sender, EventArgs e)
        {
            if (previewPanel.Visible)
            {
                UpdatePreview();
            }
        }

        private void UpdatePreview()
        {
            if (dataView.SelectedRows.Count != 1 || dataView.SelectedRows[0].Cells[columnOmsType.Index].Value == null)
            {
                return;
            }

            DataGridViewRow row = dataView.SelectedRows[0];
            var objectType = row.Cells[columnOmsType.Index].Value.ToString();
            if (objectType == "document" || objectType == "precedent" || objectType == "email")
            {
                try
                {
                    documentPreview.Show();

                    IStorageItem document;
                    if (objectType == "document" || objectType == "email")
                    {
                        document = OMSDocument.GetDocument(Convert.ToInt64(row.Cells[columnMatterSphereId.Index].Value));
                    }
                    else
                    {
                        document = Precedent.GetPrecedent(Convert.ToInt64(row.Cells[columnMatterSphereId.Index].Value));
                    }

                    documentPreview.Connect(document);
                    documentPreview.RefreshItem();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    documentPreview.SetError(ex.Message);
                }
            }
            else
            {
                documentPreview.Hide();
            }
        }

        #endregion Previewer
    }
}
