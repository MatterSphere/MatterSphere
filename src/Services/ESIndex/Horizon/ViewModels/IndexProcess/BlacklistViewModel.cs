using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.DAL;
using Horizon.Models.IndexProcess;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexProcess
{
    public class BlacklistViewModel : PageViewModel
    {
        private Provider _provider;
        private string _search;

        public BlacklistViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Extensions Blacklist";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            BlacklistGroups = new List<BlacklistItemViewModel>();
            FilteredList = new ObservableCollection<BlacklistItemViewModel>();
            LoadBlacklist();
            NewBlacklistItem = new BlacklistItemFromViewModel(string.Empty);
            MainViewModel.onSearch += Search;
        }

        #region Properties

        public ObservableCollection<BlacklistItemViewModel> FilteredList { get; set; }
        public List<BlacklistItemViewModel> BlacklistGroups { get; set; }

        private BlacklistItemViewModel _selectedBlacklistGroupItem;
        public BlacklistItemViewModel SelectedBlacklistGroupItem
        {
            get { return _selectedBlacklistGroupItem; }
            set
            {
                if (_selectedBlacklistGroupItem != value)
                {
                    _selectedBlacklistGroupItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private BlacklistItemFromViewModel _newBlacklistItem;
        public BlacklistItemFromViewModel NewBlacklistItem
        {
            get { return _newBlacklistItem; }
            set
            {
                if (_newBlacklistItem != value)
                {
                    _newBlacklistItem = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _creationFormOpened;
        public bool CreationFormOpened
        {
            get { return _creationFormOpened; }
            set
            {
                if (_creationFormOpened != value)
                {
                    _creationFormOpened = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Extensions", BlacklistGroups.Count);
        }

        #region Commands

        #region RemoveGroup
        private ICommand _removeGroupCommand;
        public ICommand RemoveGroupCommand
        {
            get
            {
                if (_removeGroupCommand == null)
                {
                    _removeGroupCommand = new RelayCommand(
                        rc => this.CanRemoveGroup(),
                        rc => this.RemoveGroup());
                }
                return _removeGroupCommand;
            }
        }

        private bool CanRemoveGroup()
        {
            return SelectedBlacklistGroupItem != null;
        }

        private void RemoveGroup()
        {
            var result = _provider.RemoveBlacklistGroup(SelectedBlacklistGroupItem.Extension);
            if (result.IsSuccess)
            {
                BlacklistGroups.Remove(SelectedBlacklistGroupItem);
                FilteredList.Remove(SelectedBlacklistGroupItem);
                SetStatusBarContent();
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #region StartCreateGroup
        private ICommand _startCreateGroupCommand;
        public ICommand StartCreateGroupCommand
        {
            get
            {
                if (_startCreateGroupCommand == null)
                {
                    _startCreateGroupCommand = new RelayCommand(
                        rc => this.CanStartCreateGroup(),
                        rc => this.StartCreateGroup());
                }
                return _startCreateGroupCommand;
            }
        }

        private bool CanStartCreateGroup()
        {
            return true;
        }

        private void StartCreateGroup()
        {
            CreationFormOpened = true;
            NewBlacklistItem = new BlacklistItemFromViewModel(string.Empty);
        }
        #endregion

        #region StartCreateGroupItem
        private ICommand _startCreateGroupItemCommand;
        public ICommand StartCreateGroupItemCommand
        {
            get
            {
                if (_startCreateGroupItemCommand == null)
                {
                    _startCreateGroupItemCommand = new RelayCommand(
                        rc => this.CanStartCreateGroupItem(),
                        rc => this.StartCreateGroupItem());
                }
                return _startCreateGroupItemCommand;
            }
        }

        private bool CanStartCreateGroupItem()
        {
            return SelectedBlacklistGroupItem != null;
        }

        private void StartCreateGroupItem()
        {
            CreationFormOpened = true;
            NewBlacklistItem = new BlacklistItemFromViewModel(SelectedBlacklistGroupItem.Extension);
        }
        #endregion

        #region AddBlacklistItem
        private ICommand _addBlacklistItemCommand;
        public ICommand AddBlacklistItemCommand
        {
            get
            {
                if (_addBlacklistItemCommand == null)
                {
                    _addBlacklistItemCommand = new RelayCommand(
                        rc => this.CanAddBlacklistItem(),
                        rc => this.AddBlacklistItem());
                }
                return _addBlacklistItemCommand;
            }
        }

        private bool CanAddBlacklistItem()
        {
            return NewBlacklistItem != null
                   && NewBlacklistItem.CanSave
                   && (!BlacklistGroups.Any() || !BlacklistGroups.Any(
                       group =>
                           group.Extension == NewBlacklistItem.ExtensionValidValue &&
                           group.ExtensionItems.Any(
                               item =>
                                   item.Metadata == NewBlacklistItem.MetadataValidValue &&
                                   item.Encoding == NewBlacklistItem.EncodingValidValue &&
                                   item.MaxSize == NewBlacklistItem.MaxSizeValidValue)));
        }

        private void AddBlacklistItem()
        {
            var newItem = new BlacklistItem(
                NewBlacklistItem.ExtensionValidValue,
                NewBlacklistItem.MetadataValidValue,
                NewBlacklistItem.EncodingValidValue,
                NewBlacklistItem.MaxSizeValidValue);
            var result = _provider.AddBlacklistItem(newItem);
            if (result.IsSuccess)
            {
                var group = LoadExtensions().First(it => it.Extension == newItem.Extension);
                var updatedItem = BlacklistGroups.FirstOrDefault(it => it.Extension == group.Extension);
                if (updatedItem != null)
                {
                    updatedItem.ExtensionItems.Clear();
                    foreach (var extensionItem in group.ExtensionItems)
                    {
                        updatedItem.ExtensionItems.Add(extensionItem);
                    }

                    updatedItem.UpdateItemsNumber();
                }
                else
                {
                    BlacklistGroups.Add(group);
                    SetStatusBarContent();
                }

                Search(_search);
                NewBlacklistItem = new BlacklistItemFromViewModel(string.Empty);
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #endregion

        #region Private methods

        private async void LoadBlacklist()
        {
            ResponseWaiting = true;
            var blacklistItems = await Task.Run(() => LoadExtensions());
            BlacklistGroups.Clear();
            foreach (var blacklistItem in blacklistItems.OrderBy(it => it.Extension).ToList())
            {
                BlacklistGroups.Add(blacklistItem);
            }

            FilteredList.Clear();
            foreach (var blacklistItem in BlacklistGroups)
            {
                FilteredList.Add(blacklistItem);
            }
            
            MainViewModel.CanSearch = true;
            SetStatusBarContent();
            ResponseWaiting = false;
        }

        private List<BlacklistItemViewModel> LoadExtensions()
        {
            var result = _provider.GetBlacklist();
            if (result.Key.IsSuccess)
            {
                var blacklistItems = new List<BlacklistItemViewModel>();
                foreach (var group in result.Value.GroupBy(item => item.Extension).ToList())
                {
                    var blacklistItem = new BlacklistItemViewModel(_provider, group.Key);
                    var items = group
                        .Select(ext => new ExtensionItem(group.Key, ext.Metadata, ext.Encoding, ext.MaxSize)).ToList();
                    foreach (var item in items)
                    {
                        blacklistItem.ExtensionItems.Add(item);
                    }

                    blacklistItem.onError += ShowErrorMessage;
                    blacklistItem.onEmptyItems += ItemsEmpty;

                    blacklistItems.Add(blacklistItem);
                }

                return blacklistItems;
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<BlacklistItemViewModel>();
        }

        private void ItemsEmpty()
        {
            if (SelectedBlacklistGroupItem != null)
            {
                RemoveGroup();
            }
        }

        private void Search(string text)
        {
            _search = text;
            if (!string.IsNullOrEmpty(text))
            {
                FilteredList.Clear();
                var items = BlacklistGroups.Where(it => it.Extension.Contains(text.ToLower().Trim())).ToList();
                foreach (var item in items)
                {
                    FilteredList.Add(item);
                }
            }
            else
            {
                FilteredList.Clear();
                foreach (var blacklistGroup in BlacklistGroups)
                {
                    FilteredList.Add(blacklistGroup);
                }
            }
        }

        #endregion
    }
}
