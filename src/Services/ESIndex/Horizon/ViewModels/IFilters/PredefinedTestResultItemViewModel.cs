using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;
using System.Windows;
using Horizon.TestiFilter.Common;

namespace Horizon.ViewModels.IFilters
{
    public class PredefinedTestResultItemViewModel : INotifyPropertyChanged
    {
        public PredefinedTestResultItemViewModel(string extension)
        {
            DocumentType = extension.Substring(1);
        }

        public PredefinedTestResultItemViewModel(TestCallback callback)
        {
            DocumentType = callback.File.Extension.Substring(1);
            TestResult = callback.TestPassed;
            ErrorDetails = callback.TestPassed ? null : callback.Error.ToString();
        }

        #region Properties

        public string DocumentType { get; set; }

        private bool? _testResult;
        public bool? TestResult
        {
            get { return _testResult; }
            set
            {
                if (_testResult != value)
                {
                    _testResult = value;
                    OnPropertyChanged();

                    OnPropertyChanged(nameof(ErrorLogoVisibility));
                    OnPropertyChanged(nameof(SuccessLogoVisibility));
                }
            }
        }

        private string _errorDetails;
        public string ErrorDetails
        {
            get { return _errorDetails; }
            set
            {
                if (_errorDetails != value)
                {
                    _errorDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility ErrorLogoVisibility
        {
            get
            {
                if (TestResult.HasValue)
                {
                    return TestResult.Value
                        ? Visibility.Collapsed
                        : Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
        }

        public Visibility SuccessLogoVisibility
        {
            get
            {
                if (TestResult.HasValue)
                {
                    return TestResult.Value
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }

                return Visibility.Collapsed;
            }
        }

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
