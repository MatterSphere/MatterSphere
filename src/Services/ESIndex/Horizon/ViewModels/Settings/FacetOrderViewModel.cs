using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.DAL;
using Horizon.Providers;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.Settings
{
    public class FacetOrderViewModel : PageViewModel
    {
        private readonly Provider _provider;
        private List<IndexEntityViewModel> _indexEntities;

        public FacetOrderViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Facet Order";
            _provider = new Provider(MainViewModel.Settings.DbConnection);

            LoadIndexEntities();
            Task.Run(() => LoadFacets())
                .ContinueWith((task) => { FacetableFields = task.Result; }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private ObservableCollection<FacetableFieldViewModel> _facetableFields;
        public ObservableCollection<FacetableFieldViewModel> FacetableFields
        {
            get { return _facetableFields; }
            set
            {
                if (_facetableFields != value)
                {
                    _facetableFields = value;
                    OnPropertyChanged();
                }
            }
        }

        private FacetableFieldViewModel _selectedField;
        public FacetableFieldViewModel SelectedField
        {
            get { return _selectedField; }
            set
            {
                if (_selectedField != value)
                {
                    _selectedField = value;
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

        private void LoadIndexEntities()
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

            _indexEntities = response.Value.Select(entity => new IndexEntityViewModel(entity, false)).Where(entity => entity.VisibleEntity).ToList();
        }

        private ObservableCollection<FacetableFieldViewModel> LoadFacets()
        {
            var collection = new ObservableCollection<FacetableFieldViewModel>();
            
            var dataIndex = GetIndexInfo(IndexTypeEnum.Data);
            if (dataIndex == null)
            {
                ShowErrorMessage("The data index was not found");
                return collection;
            }

            var response = _provider.GetAllIndexFields(dataIndex.Id);
            if (!response.Key.IsSuccess)
            {
                ShowErrorMessage(response.Key.ErrorMessage);
                return collection;
            }

            var lookups = _provider.GetFacetableCodeLookups();
            Dictionary<string, string> facetableLookups = lookups.Value;
            if (!lookups.Key.IsSuccess)
            {
                ShowErrorMessage(lookups.Key.ErrorMessage);
                facetableLookups = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            }

            foreach(var item in (response.Value
                .Where(x => x.Facetable)
                .Select(indexField => new FacetableFieldViewModel(indexField))
                .OrderBy(x => x.FacetSort).ThenBy(x => x.Name)))
            {
                item.EntityName = _indexEntities.FirstOrDefault(e => e.Id == item.EntityId)?.Name;
                item.FieldDesc = facetableLookups.ContainsKey(item.FieldCode) ? facetableLookups[item.FieldCode] : string.Empty;
                collection.Add(item);
            }
            return collection;
        }

        private IndexInfo GetIndexInfo(IndexTypeEnum indexType)
        {
            return GetIndices().FirstOrDefault(index => index.IndexType == indexType);
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

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        rc => this.CanSave(),
                        rc => this.Save());
                }
                return _saveCommand;
            }
        }

        private bool CanSave()
        {
            return _facetableFields != null && FacetableFields.Any(x => x.IsDirty);
        }

        private void Save()
        {
            foreach (var item in FacetableFields.Where(x => x.IsDirty))
            {
                if (UpdateIndexField(item))
                    item.IsDirty = false;
            }
        }

        private bool UpdateIndexField(FacetableFieldViewModel field)
        {
            KeyValuePair<ResponseStatus, bool> result;
            var indexField = new IndexField(field.EntityId, null, field.Name, field.FieldType)
            {
                Searchable = field.Searchable,
                Facetable = field.Facetable,
                FacetOrder = field.FacetOrder,
                FieldCode = field.FieldCode,
                Suggestable = field.Suggestable,
                Analyzer = field.Analyzer,
                ExtendedData = field.ExtendedTable,
                FieldCodeLookupGroup = field.FieldCodeLookupGroup
            };
            if (field.EntityId == 0 && field.EntityName == null)
            {
                result = _provider.UpdateCommonIndexField(IndexTypeEnum.Data, indexField);
            }
            else
            {
                result = _provider.UpdateIndexField(indexField);
            }

            if (!result.Key.IsSuccess)
            {
                var provider = new ErrorMessageProvider();
                provider.ShowErrorMessage(MainWindow, result.Key.ErrorMessage);
            }

            return result.Key.IsSuccess;
        }
    }
}
