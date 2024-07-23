using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;

namespace Horizon.ViewModels.IFilters
{
    public class IFilterListItemViewModel : INotifyPropertyChanged
    {
        public string Extension { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileVersion { get; set; }
        public string Company { get; set; }

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
