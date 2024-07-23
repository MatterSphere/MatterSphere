using System.ComponentModel;
using System.Runtime.CompilerServices;
using Horizon.Annotations;

namespace Horizon.ViewModels.Common
{
    public class StatusBarViewModel : INotifyPropertyChanged
    {
        private string _firstLabel;
        public string FirstLabel
        {
            get
            {
                return string.IsNullOrEmpty(_firstLabel)
                    ? null
                    : $"{_firstLabel}:";
            }
            set
            {
                if (_firstLabel != value)
                {
                    _firstLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _firstValue;
        public string FirstValue
        {
            get { return _firstValue; }
            set
            {
                if (_firstValue != value)
                {
                    _firstValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _secondLabel;
        public string SecondLabel
        {
            get
            {
                return string.IsNullOrEmpty(_secondLabel)
                    ? null
                    : $"{_secondLabel}:";
            }
            set
            {
                if (_secondLabel != value)
                {
                    _secondLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _secondValue;
        public string SecondValue
        {
            get { return _secondValue; }
            set
            {
                if (_secondValue != value)
                {
                    _secondValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _thirdLabel;
        public string ThirdLabel
        {
            get
            {
                return string.IsNullOrEmpty(_thirdLabel)
                    ? null
                    : $"{_thirdLabel}:";
            }
            set
            {
                if (_thirdLabel != value)
                {
                    _thirdLabel = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _thirdValue;
        public string ThirdValue
        {
            get { return _thirdValue; }
            set
            {
                if (_thirdValue != value)
                {
                    _thirdValue = value;
                    OnPropertyChanged();
                }
            }
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
