using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;

namespace Horizon.ViewModels.StartPage
{
    public abstract class StartPageItemViewModel : INotifyPropertyChanged
    {
        private string _subNumberLabel;

        protected StartPageItemViewModel(MainViewModel main)
        {
            Main = main;
        }

        protected StartPageItemViewModel(long total, long? subNumber, string subNumberLabel, MainViewModel main) : this(main)
        {
            _total = total;
            _subNumber = subNumber;
            _subNumberLabel = subNumberLabel;
        }

        private long _total;
        public string Total { get { return _total.ToLabel(); } }

        public long? _subNumber;
        public string SubNumber
        {
            get
            {
                return _subNumber.HasValue
                    ? $"({_subNumber.Value.ToLabel()} {_subNumberLabel})"
                    : _subNumberLabel;
            }
        }

        public string Title { get; set; }
        public string Icon { get; set; }
        protected MainViewModel Main { get; set; }

        #region OpenDocumentErrorList
        private ICommand _openDetailsCommand;
        public ICommand OpenDetailsCommand
        {
            get
            {
                if (_openDetailsCommand == null)
                {
                    _openDetailsCommand = new RelayCommand(
                        rc => this.CanOpenDetails(),
                        rc => this.OpenDetails());
                }
                return _openDetailsCommand;
            }
        }

        private bool CanOpenDetails()
        {
            return true;
        }

        protected abstract void OpenDetails();
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
