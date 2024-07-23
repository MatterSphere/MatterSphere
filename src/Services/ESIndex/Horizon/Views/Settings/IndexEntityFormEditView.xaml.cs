using System.Windows;
using System.Windows.Input;

namespace Horizon.Views.Settings
{
    /// <summary>
    /// Interaction logic for IndexEntityFormEditView.xaml
    /// </summary>
    public partial class IndexEntityFormEditView : Window
    {
        public IndexEntityFormEditView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
