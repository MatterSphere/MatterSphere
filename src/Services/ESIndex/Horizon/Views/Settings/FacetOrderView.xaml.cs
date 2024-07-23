using System.Text.RegularExpressions;
using System.Windows.Controls;
using Horizon.ViewModels.Settings;

namespace Horizon.Views.Settings
{
    /// <summary>
    /// Interaction logic for FacetOrderView.xaml
    /// </summary>
    public partial class FacetOrderView : UserControl
    {
        public FacetOrderView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
