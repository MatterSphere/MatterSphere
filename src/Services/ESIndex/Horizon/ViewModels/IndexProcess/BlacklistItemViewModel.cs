using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;
using Horizon.DAL;
using Horizon.Models.IndexProcess;

namespace Horizon.ViewModels.IndexProcess
{
    public class BlacklistItemViewModel : INotifyPropertyChanged
    {
        public delegate void EmptyItemsDelegate();
        public delegate void ErrorDelegate(string errorMessage);
        public event EmptyItemsDelegate onEmptyItems;
        public event ErrorDelegate onError;

        private Provider _provider;

        public BlacklistItemViewModel(Provider provider, string extension)
        {
            _provider = provider;
            Extension = extension;
            ExtensionItems = new ObservableCollection<ExtensionItem>();
        }

        public string Extension { get; set; }

        public ObservableCollection<ExtensionItem> ExtensionItems { get; set; }

        private ExtensionItem _selectedExtensionItem;
        public ExtensionItem SelectedExtensionItem
        {
            get { return _selectedExtensionItem; }
            set
            {
                if (_selectedExtensionItem != value)
                {
                    _selectedExtensionItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ItemsNumber
        {
            get
            {
                var number = ExtensionItems != null
                    ? ExtensionItems.Count
                    : 0;

                if (number == 0)
                {
                    onEmptyItems?.Invoke();
                }

                return number;
            }
        }

        #region Methods

        public void UpdateItemsNumber()
        {
            OnPropertyChanged(nameof(ItemsNumber));
        }

        #endregion

        #region Commands

        #region Remove item
        private ICommand _removeItemCommand;
        public ICommand RemoveItemCommand
        {
            get
            {
                if (_removeItemCommand == null)
                {
                    _removeItemCommand = new RelayCommand(
                        rc => this.CanRemoveItem(),
                        rc => this.RemoveItem());
                }
                return _removeItemCommand;
            }
        }

        private bool CanRemoveItem()
        {
            return SelectedExtensionItem != null;
        }

        private void RemoveItem()
        {
            var result = _provider.RemoveBlacklistItem(Extension, SelectedExtensionItem.Metadata, SelectedExtensionItem.Encoding);
            if (result.IsSuccess)
            {
                ExtensionItems.Remove(SelectedExtensionItem);
                OnPropertyChanged(nameof(ItemsNumber));
            }
            else
            {
                onError?.Invoke(result.ErrorMessage);
            }
        }
        #endregion

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
