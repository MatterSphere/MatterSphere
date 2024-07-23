using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Horizon.Views.Settings
{
    /// <summary>
    /// Interaction logic for IndexFieldFormView.xaml
    /// </summary>
    public partial class IndexFieldFormEditView : Window
    {
        public IndexFieldFormEditView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void facetOrderTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
