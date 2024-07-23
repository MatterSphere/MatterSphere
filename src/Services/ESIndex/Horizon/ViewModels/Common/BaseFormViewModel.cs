using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;

namespace Horizon.ViewModels.Common
{
    public abstract class BaseFormViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public Window Window { get; set; }
        public bool Result { get; protected set; }

        #region Commands

        #region Save
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        rc => this.CanSave(),
                        rc => this.Save());
                }
                return _saveCommand;
            }
        }

        protected abstract bool CanSave();
        protected abstract void Save();
        #endregion

        #region Close
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(
                        rc => this.CanClose(),
                        rc => this.Close());
                }
                return _closeCommand;
            }
        }

        private bool CanClose()
        {
            return true;
        }

        public void Close()
        {
            Window.Close();
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
