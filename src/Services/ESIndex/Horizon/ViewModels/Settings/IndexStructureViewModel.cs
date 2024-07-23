using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.DAL;
using Horizon.Providers;
using Horizon.ViewModels.Common;
using Horizon.Views.Settings;
using IndexField = Horizon.Models.Settings.IndexField;

namespace Horizon.ViewModels.Settings
{
    public class IndexStructureViewModel : PageViewModel
    {
        private readonly Provider _provider;
        private readonly IElasticsearchProvider _esProvider;
        private DateTime? _indexCheckDate;
        private bool _indexCreated;
        private string _indexName;
        private bool _indexCreatedWasInitialized;
        private Dictionary<string, string> _facetableLookups;

        public IndexStructureViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Index Structure";
            IsEntitiesLoading = true;
            _provider = new Provider(MainViewModel.Settings.DbConnection);
            _esProvider = new ElasticsearchProvider(MainViewModel.Settings.ElasticsearchServer, MainViewModel.Settings.ElasticsearchApiKey);
            DefaultFields = new ObservableCollection<IndexField>();
            CustomFields = new ObservableCollection<IndexField>();
            LoadEntitiesAsync();
        }

        private bool _isEntitiesLoading;
        public bool IsEntitiesLoading
        {
            get
            {
                return _isEntitiesLoading;
            }
            set
            {
                if (_isEntitiesLoading != value)
                {
                    _isEntitiesLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoadProcessCompleted));
                }
            }
        }

        public bool IsLoadProcessCompleted
        {
            get { return !_isEntitiesLoading; }
        }

        private bool _summaryFieldEnabled;
        public bool SummaryFieldEnabled
        {
            get { return _summaryFieldEnabled; }
            set
            {
                _summaryFieldEnabled = value;
            }
        }

        private ObservableCollection<IndexEntityViewModel> _indexEntities;
        public ObservableCollection<IndexEntityViewModel> IndexEntities
        {
            get { return _indexEntities; }
            set
            {
                if (_indexEntities != value)
                {
                    _indexEntities = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        private IndexEntityViewModel _selectedEntity;
        public IndexEntityViewModel SelectedEntity
        {
            get { return _selectedEntity; }
            set
            {
                if (_selectedEntity != value)
                {
                    if (_selectedEntity != null)
                    {
                        if (_selectedEntity.IsDirty)
                        {
                            UpdateSummaryTemplateCommand.Execute(_selectedEntity);
                        }
                        _selectedEntity.PropertyChanged -= SelectedEntity_PropertyChanged;
                    }

                    _selectedEntity = value;
                    _selectedEntity.IndexCreated = CheckIndex();
                    _selectedEntity.PropertyChanged += SelectedEntity_PropertyChanged;

                    OnPropertyChanged();
                    LoadFields();
                }
            }
        }

        public ObservableCollection<IndexField> DefaultFields { get; set; }
        public ObservableCollection<IndexField> CustomFields { get; set; }

        private IndexField _selectedDefaultField;
        public IndexField SelectedDefaultField
        {
            get { return _selectedDefaultField; }
            set
            {
                if (_selectedDefaultField != value)
                {
                    _selectedDefaultField = value;
                    OnPropertyChanged();
                }
            }
        }

        private IndexField _selectedCustomField;
        public IndexField SelectedCustomField
        {
            get { return _selectedCustomField; }
            set
            {
                if (_selectedCustomField != value)
                {
                    _selectedCustomField = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDirty { get; private set; }

        #region Commands

        #region IncludeEntity
        private ICommand _includeEntityCommand;
        public ICommand IncludeEntityCommand
        {
            get
            {
                if (_includeEntityCommand == null)
                {
                    _includeEntityCommand = new RelayCommand(
                        rc => this.CanIncludeEntity(),
                        rc => this.IncludeEntity());
                }
                return _includeEntityCommand;
            }
        }

        private bool CanIncludeEntity()
        {
            return SelectedEntity != null
                   && !SelectedEntity.IndexingEnabled
                   && !SelectedEntity.IsDefault;
        }

        private void IncludeEntity()
        {
            var result = _provider.ChangeIndexingEnabling(SelectedEntity.Id, true);

            if (result.IsSuccess)
            {
                SelectedEntity.IndexingEnabled = true;
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #region ExcludeEntity
        private ICommand _excludeEntityCommand;
        public ICommand ExcludeEntityCommand
        {
            get
            {
                if (_excludeEntityCommand == null)
                {
                    _excludeEntityCommand = new RelayCommand(
                        rc => this.CanExcludeEntity(),
                        rc => this.ExcludeEntity());
                }
                return _excludeEntityCommand;
            }
        }

        private bool CanExcludeEntity()
        {
            return SelectedEntity != null
                   && SelectedEntity.IndexingEnabled
                   && !SelectedEntity.IsDefault;
        }

        private void ExcludeEntity()
        {
            var result = _provider.ChangeIndexingEnabling(SelectedEntity.Id, false);

            if (result.IsSuccess)
            {
                SelectedEntity.IndexingEnabled = false;
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        private ICommand _updateSummaryTemplateCommand;
        public ICommand UpdateSummaryTemplateCommand
        {
            get
            {
                if (_updateSummaryTemplateCommand == null)
                {
                    _updateSummaryTemplateCommand = new RelayCommand(
                        rc => this.CanUpdateSummaryTemplate(),
                        rc => this.UpdateSummaryTemplate());
                }
                return _updateSummaryTemplateCommand;
            }
        }

        private bool CanUpdateSummaryTemplate()
        {
            return SelectedEntity != null;
        }

        private void UpdateSummaryTemplate()
        {
            var result = _provider.UpdateSummaryTemplate(new IndexEntity(SelectedEntity.Id, SelectedEntity.Name, SelectedEntity.TableName, SelectedEntity.Key)
            {
                SummaryTemplate = SelectedEntity.SummaryTemplate
            });

            if (result.IsSuccess)
            {
                SelectedEntity.IsDirty = false;
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }


        #region AddField
        private ICommand _addFieldCommand;
        public ICommand AddFieldCommand
        {
            get
            {
                if (_addFieldCommand == null)
                {
                    _addFieldCommand = new RelayCommand(
                        rc => this.CanAddField(),
                        rc => this.AddField());
                }
                return _addFieldCommand;
            }
        }

        private bool CanAddField()
        {
            return SelectedEntity != null
                   && SelectedEntity.CustomizableEntity
                   && SelectedEntity.IndexingEnabled
                   && !CheckIndex();
        }

        private void AddField()
        {
            var model = new IndexFieldFormViewModel(
                _provider,
                SelectedEntity.Id,
                SelectedEntity.TableName.Split('.')[1],
                SelectedEntity.Key,
                DefaultFields.Select(field => field.Name).ToList(),
                CustomFields.Select(field => field.Name).ToList(),
                _facetableLookups);
            var window = new IndexFieldFormView()
            {
                DataContext = model,
                Owner = MainWindow
            };
            model.Window = window;
            window.ShowDialog();

            if (model.Result)
            {
                UpdateCustomFields();
            }
        }
        #endregion

        #region DeleteField
        private ICommand _deleteFieldCommand;
        public ICommand DeleteFieldCommand
        {
            get
            {
                if (_deleteFieldCommand == null)
                {
                    _deleteFieldCommand = new RelayCommand(
                        rc => this.CanDeleteField(),
                        rc => this.DeleteField());
                }
                return _deleteFieldCommand;
            }
        }

        private bool CanDeleteField()
        {
            return SelectedEntity != null
                   && SelectedEntity.IndexingEnabled
                   && SelectedCustomField != null
                   && !SelectedCustomField.IsDefault
                   && !CheckIndex();
        }

        private void DeleteField()
        {
            var result = _provider.DeleteIndexField(SelectedCustomField.EntityId, SelectedCustomField.Name);
            if (result.IsSuccess)
            {
                CustomFields.Remove(SelectedCustomField);
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        private ICommand _editEntityCommand;
        public ICommand EditEntityCommand
        {
            get
            {
                if (_editEntityCommand == null)
                {
                    _editEntityCommand = new RelayCommand(
                        rc => this.CanEditEntity(_selectedEntity),
                        rc => this.EditEntity(_selectedEntity));
                }
                return _editEntityCommand;
            }
        }

        private bool CanEditEntity(IndexEntityViewModel selectedEntity)
        {
            return selectedEntity != null
                    && selectedEntity.IndexingEnabled
                    && !CheckIndex();
        }

        private void EditEntity(IndexEntityViewModel selectedEntity)
        {
            var model = new IndexEntityEditViewModel(_provider, selectedEntity);
            var window = new IndexEntityFormEditView()
            {
                DataContext = model,
                Owner = MainWindow
            };
            model.Window = window;
            window.ShowDialog();

            if (model.Result)
            {
                LoadEntities();
            }
        }

        #endregion

        #region EditField
        private ICommand _editDefaultFieldCommand;
        public ICommand EditDefaultFieldCommand
        {
            get
            {
                if (_editDefaultFieldCommand == null)
                {
                    _editDefaultFieldCommand = new RelayCommand(
                        rc => this.CanEditField(_selectedDefaultField),
                        rc => this.EditField(_selectedDefaultField));
                }
                return _editDefaultFieldCommand;
            }
        }
        private ICommand _editCustomFieldCommand;
        public ICommand EditCustomFieldCommand
        {
            get
            {
                if (_editCustomFieldCommand == null)
                {
                    _editCustomFieldCommand = new RelayCommand(
                        rc => this.CanEditField(_selectedCustomField),
                        rc => this.EditField(_selectedCustomField));
                }
                return _editCustomFieldCommand;
            }
        }

        private bool CanEditField(IndexField selectedField)
        {
            return  SelectedEntity != null
                    && SelectedEntity.CustomizableEntity
                    && (selectedField != null)
                    && SelectedEntity.IndexingEnabled
                    && !CheckIndex();
        }

        private void EditField(IndexField selectedField)
        {
            var model = new IndexFieldFormEditViewModel(
                _provider,
                SelectedEntity.Id,
                SelectedEntity.TableName.Split('.')[1],
                SelectedEntity.Key,
                DefaultFields.Select(field => field.Name).ToList(),
                CustomFields.Select(field => field.Name).ToList(),
                _facetableLookups,
                selectedField
                );
            var window = new IndexFieldFormEditView()
            {
                DataContext = model,
                Owner = MainWindow
            };
            model.Window = window;
            window.ShowDialog();

            if (model.Result)
            {
                LoadFields();
            }
        }
        #endregion

        #region Private methods

        private async void LoadEntitiesAsync()
        {
            await Task.Run(() => LoadEntities());
        }

        private void LoadEntities()
        {
            var dataIndex = GetIndexInfo(IndexTypeEnum.Data);
            if (dataIndex == null)
            {
                ShowErrorMessage("The data index was not found");
                return;
            }

            var response = _provider.GetIndexEntities(dataIndex.Id);
            if (!response.Key.IsSuccess)
            {
                ShowErrorMessage(response.Key.ErrorMessage);
                return;
            }

            bool indexCreated = CheckIndex();
            IndexEntities = new ObservableCollection<IndexEntityViewModel>(response.Value
                .Select(entity => new IndexEntityViewModel(entity, indexCreated))
                .Where(entity => entity.VisibleEntity));

            _facetableLookups = _provider.GetFacetableCodeLookups().Value ?? new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            SummaryFieldEnabled = _provider.GetIndexSettings().Value.SummaryFieldEnabled;

            IsEntitiesLoading = false;
        }

        private List<IndexInfo> GetIndices()
        {
            var response = _provider.GetIndices();
            if (!response.Key.IsSuccess)
            {
                ShowErrorMessage(response.Key.ErrorMessage);
                return new List<IndexInfo>();
            }

            return response.Value;
        }

        private void LoadFields()
        {
            if (SelectedEntity == null)
            {
                return;
            }

            var response = _provider.GetIndexFields(SelectedEntity.Id);
            if (!response.Key.IsSuccess)
            {
                ShowErrorMessage(response.Key.ErrorMessage);
                return;
            }

            DefaultFields.Clear();
            CustomFields.Clear();

            var fields = response.Value.Select(field => new IndexField(field)).ToList();
            foreach (var field in fields)
            {
                if (_facetableLookups.ContainsKey(field.FieldCode))
                {
                    field.FieldDesc = _facetableLookups[field.FieldCode];
                }

                if (field.IsDefault)
                {
                    DefaultFields.Add(field);
                }
                else
                {
                    CustomFields.Add(field);
                }
            }
        }

        private void UpdateCustomFields()
        {
            var response = _provider.GetIndexFields(SelectedEntity.Id);
            if (!response.Key.IsSuccess)
            {
                ShowErrorMessage(response.Key.ErrorMessage);
                return;
            }

            CustomFields.Clear();
            var fields = response.Value.Select(field => new IndexField(field)).Where(field => !field.IsDefault).ToList();
            foreach (var field in fields)
            {
                if (_facetableLookups.ContainsKey(field.FieldCode))
                {
                    field.FieldDesc = _facetableLookups[field.FieldCode];
                }
                CustomFields.Add(field);
            }
        }

        private string GetDataIndex()
        {
            if (!string.IsNullOrWhiteSpace(_indexName))
            {
                return _indexName;
            }

            var dataIndex = GetIndexInfo(IndexTypeEnum.Data);
            if (dataIndex != null)
            {
                _indexName = dataIndex.Name;
            }

            return _indexName;
        }

        private bool CheckIndex()
        {
            if (_indexCreated)
            {
                return true;
            }

            if (_indexCheckDate.HasValue && _indexCheckDate.Value.AddMinutes(5) > DateTime.Now)
            {
                return _indexCreated;
            }

            _indexCheckDate = DateTime.Now;
            var dataIndex = GetDataIndex();
            _indexCreated = !string.IsNullOrWhiteSpace(dataIndex) && _esProvider.CheckIndex(dataIndex);

            if (_indexCreatedWasInitialized && _indexCreated)
            {
                foreach (var item in IndexEntities)
                {
                    item.IndexCreated = _indexCreated;
                }
            }
            _indexCreatedWasInitialized = true;

            return _indexCreated;
        }

        private IndexInfo GetIndexInfo(IndexTypeEnum indexType)
        {
            return GetIndices().FirstOrDefault(index => index.IndexType == indexType);
        }

        private void SelectedEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IndexingEnabled")
            {
                IndexEntityViewModel entity = sender as IndexEntityViewModel;
                if (entity.IndexingEnabled)
                {
                    IncludeEntityCommand.Execute(entity);
                }
                else
                {
                    ExcludeEntityCommand.Execute(entity);
                }
            }
        }
        #endregion
    }
}
