using System.Windows;

namespace Horizon
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            new Providers.ErrorMessageProvider().ShowErrorMessage(MainWindow, e.Exception.Message);
            e.Handled = true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            TestiFilter.Common.TestManager.Destroy();
        }
    }
}
