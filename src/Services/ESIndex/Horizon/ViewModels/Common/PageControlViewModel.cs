using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;

namespace Horizon.ViewModels.Common
{
    public class PageControlViewModel : INotifyPropertyChanged
    {
        public delegate void MethodContainer();
        public event MethodContainer onPageChange;

        private readonly long _generalNumber;

        public PageControlViewModel(long generalNumber, int pageSize)
        {
            _generalNumber = generalNumber;
            PageSize = pageSize;
            CurrentPage = 1;
        }

        private int _currentPage;
        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PageLabel));
                }
            }
        }

        public int PageSize { get; set; }

        public string PageLabel
        {
            get { return $"{CurrentPage} / {Math.Ceiling((double)_generalNumber / PageSize)}"; }
        }

        public int GetStartNumber()
        {
            return (CurrentPage - 1) * PageSize + 1;
        }


        #region StepBackward
        private ICommand _stepBackwardCommand;
        public ICommand StepBackwardCommand
        {
            get
            {
                if (_stepBackwardCommand == null)
                {
                    _stepBackwardCommand = new RelayCommand(
                        rc => this.CanStepBackward(),
                        rc => this.StepBackward());
                }
                return _stepBackwardCommand;
            }
        }

        private bool CanStepBackward()
        {
            return CurrentPage > 1;
        }

        private void StepBackward()
        {
            CurrentPage--;
            onPageChange?.Invoke();
        }
        #endregion

        #region StepForward
        private ICommand _stepForwardCommand;
        public ICommand StepForwardCommand
        {
            get
            {
                if (_stepForwardCommand == null)
                {
                    _stepForwardCommand = new RelayCommand(
                        rc => this.CanStepForward(),
                        rc => this.StepForward());
                }
                return _stepForwardCommand;
            }
        }

        private bool CanStepForward()
        {
            return CurrentPage * PageSize < _generalNumber;
        }

        private void StepForward()
        {
            CurrentPage++;
            onPageChange?.Invoke();
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
