using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;

namespace Horizon.ViewModels.IFilters
{
    public class DocumentItemViewModel : INotifyPropertyChanged
    {
        public DocumentItemViewModel(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FullName
        {
            get { return System.IO.Path.GetFileName(Path); }
        }

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
