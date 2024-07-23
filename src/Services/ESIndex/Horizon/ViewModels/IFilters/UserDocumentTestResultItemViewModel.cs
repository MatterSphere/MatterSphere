using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Horizon.Annotations;
using Horizon.TestiFilter.Common;

namespace Horizon.ViewModels.IFilters
{
    public class UserDocumentTestResultItemViewModel : INotifyPropertyChanged
    {
        public UserDocumentTestResultItemViewModel(TestCallback callback)
        {
            TestResult = callback.TestPassed;
            ErrorDetails = callback.TestPassed ? "-" : callback.Error.ToString();
            Path = callback.File.FullName;
            Content = callback.ResultValue;
            FileName = callback.File.Name;
        }

        public bool TestResult { get; set; }
        public string ErrorDetails { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }

        public Visibility ErrorLogoVisibility
        {
            get { return TestResult ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Visibility SuccessLogoVisibility
        {
            get { return TestResult ? Visibility.Visible : Visibility.Collapsed; }
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
