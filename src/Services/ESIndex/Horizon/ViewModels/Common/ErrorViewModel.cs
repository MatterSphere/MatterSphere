using System.Windows;
using System.Windows.Input;
using Horizon.Common;

namespace Horizon.ViewModels.Common
{
    public class ErrorViewModel
    {
        public Window _window;

        public ErrorViewModel(Window window, string message)
        {
            _window = window;
            ErrorMessage = message;
        }

        public string ErrorMessage { get; set; }

        #region Commands
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
            _window.Close();
        }
        #endregion
        #endregion
    }
}
