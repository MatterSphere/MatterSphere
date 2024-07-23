using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Horizon.Annotations;
using Horizon.Providers;

namespace Horizon.ViewModels.Common
{
    public class PageViewModel : INotifyPropertyChanged
    {
        public PageViewModel(MainViewModel mainViewModel)
        {
            MainWindow = mainViewModel.Window;
            MainViewModel = mainViewModel;
            MainViewModel.CanSearch = false;
            ResetStatusBar();
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasTitle));
                }
            }
        }

        public bool HasTitle { get { return !string.IsNullOrEmpty(Title); } }

        public MainViewModel MainViewModel { get; set; }
        public PageViewModel PreviousPage { get; set; }
        public Window MainWindow { get; private set; }

        private void ResetStatusBar()
        {
            MainViewModel.StatusBar = new StatusBarViewModel();
        }

        private bool _responseWaiting;
        public bool ResponseWaiting
        {
            get { return _responseWaiting; }
            set
            {
                if (_responseWaiting != value)
                {
                    _responseWaiting = value;
                    OnPropertyChanged();
                }
            }
        }

        protected void SetParameterToStatusBar<T>(int number, string label, T value)
        {
            string svalue = Convert.ToString(value);
            switch (number)
            {
                case 1:
                    MainViewModel.StatusBar.FirstLabel = label;
                    MainViewModel.StatusBar.FirstValue = svalue;
                    break;
                case 2:
                    MainViewModel.StatusBar.SecondLabel = label;
                    MainViewModel.StatusBar.SecondValue = svalue;
                    break;
                case 3:
                    MainViewModel.StatusBar.ThirdLabel = label;
                    MainViewModel.StatusBar.ThirdValue = svalue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(number));
            }
        }

        public virtual void SetStatusBarContent()
        {
        }

        public void ShowErrorMessage(string message)
        {
            var provider = new ErrorMessageProvider();
            provider.ShowErrorMessage(MainWindow, message);
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
