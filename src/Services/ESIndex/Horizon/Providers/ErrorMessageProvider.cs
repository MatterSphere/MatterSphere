using System;
using System.Windows;
using Horizon.ViewModels.Common;
using Horizon.Views.Common;

namespace Horizon.Providers
{
    public class ErrorMessageProvider
    {
        public void ShowErrorMessage(Window parent, string message)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var window = new ErrorView();

                try
                {
                    window.Owner = parent;
                }
                finally
                {
                    var model = new ErrorViewModel(window, message);
                    window.DataContext = model;
                    window.ShowDialog();
                }
            });
        }
    }
}
