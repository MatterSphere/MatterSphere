using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.TestiFilter.Common;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IFilters
{
    public class IFilterListViewModel : PageViewModel
    {
        private IFilterListViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _iFilterList = new List<IFilterListItemViewModel>();
            Title = "My iFilters";
            MainViewModel.onSearch += Search;
        }

        public IFilterListViewModel(MainViewModel mainViewModel, List<IFilterListItemViewModel> iFilters)
            : this(mainViewModel)
        {
            if (iFilters == null)
            {
                IsFiltersLoading = true;
                LoadFilters(iFilters);
            }
            else
            {
                SetCollection(iFilters);
                MainViewModel.CanSearch = IFilterList.Any();
            }
        }

        #region Properties

        private List<IFilterListItemViewModel> _iFilterList;
        public List<IFilterListItemViewModel> IFilterList
        {
            get { return _iFilterList; }
            set
            {
                if (_iFilterList != value)
                {
                    _iFilterList = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<IFilterListItemViewModel> _filteredList;
        public ObservableCollection<IFilterListItemViewModel> FilteredList
        {
            get { return _filteredList; }
            set
            {
                if (_filteredList != value)
                {
                    _filteredList = value;
                    OnPropertyChanged();
                }
            }
        }

        private IFilterListItemViewModel _selectedIFilterListItem;
        public IFilterListItemViewModel SelectedIFilterListItem
        {
            get { return _selectedIFilterListItem; }
            set
            {
                if (_selectedIFilterListItem != value)
                {
                    _selectedIFilterListItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isFiltersLoading;
        public bool IsFiltersLoading
        {
            get { return _isFiltersLoading; }
            set
            {
                if (_isFiltersLoading != value)
                {
                    _isFiltersLoading = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsLoadProcessCompleted));
                }
            }
        }

        public bool IsLoadProcessCompleted
        {
            get { return !_isFiltersLoading; }
        }

        #endregion

        #region Private methods

        private void Search(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                FilteredList = new ObservableCollection<IFilterListItemViewModel>(IFilterList.Where(
                        it => it.Extension.ToLower().Contains(text.ToLower().Trim())));
            }
            else
            {
                FilteredList = new ObservableCollection<IFilterListItemViewModel>(IFilterList);
            }
        }

        private List<IFilterListItemViewModel> GetiFilters()
        {
            var provider = new IFilterProvider();
            return provider.GetFilters()
                .Select(s => new IFilterListItemViewModel
                {
                    Extension = s.Extension != null && s.Extension.Length > 1 && s.Extension.Substring(0, 1) == "." ? s.Extension.Substring(1) : null,
                    Details = s.Details,
                    Description = s.Description,
                    Path = s.Path,
                    FileName = s.FileName,
                    FileDescription = s.FileDescription,
                    FileVersion = s.FileVersion,
                    Company = s.Company
                }).ToList();
        }

        private void SetCollection(List<IFilterListItemViewModel> filters)
        {
            IFilterList = filters;
            FilteredList = new ObservableCollection<IFilterListItemViewModel>(filters);
            SetStatusBarContent();
        }

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Extensions", IFilterList.Count);
            SetParameterToStatusBar(2, "iFilters", IFilterList.GroupBy(it => it.Path).Count());
        }

        private async void LoadFilters(List<IFilterListItemViewModel> iFilters)
        {
            ResponseWaiting = true;
            iFilters = await Task.Run(() => GetiFilters());
            iFilters = iFilters.OrderBy(it => it.Extension).ToList();
            SetCollection(iFilters);
            IsFiltersLoading = false;

            MainViewModel.IFilters = iFilters;
            MainViewModel.CanSearch = iFilters.Any();
            ResponseWaiting = false;
        }

        #endregion

        #region OpenFolder
        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        rc => this.CanOpenFolder(),
                        rc => this.OpenFolder());
                }
                return _openFolderCommand;
            }
        }

        private bool CanOpenFolder()
        {
            return SelectedIFilterListItem != null && !string.IsNullOrEmpty(SelectedIFilterListItem.Path);
        }

        private void OpenFolder()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo(SelectedIFilterListItem.Path);
            Process.Start("explorer.exe", "/select, \"" + startInfo.FileName + "\"");
        }
        #endregion
    }
}
