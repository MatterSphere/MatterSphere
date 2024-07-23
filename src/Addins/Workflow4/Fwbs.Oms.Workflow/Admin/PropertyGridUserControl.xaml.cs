using System.Windows.Controls;

namespace FWBS.OMS.Workflow.Admin
{
    /// <summary>
    /// Interaction logic for PropertyGridUserControl.xaml
    /// </summary>
    public partial class PropertyGridUserControl : UserControl
    {
        private System.Windows.Forms.PropertyGrid _PropertyGrid;

        public object Item
        {
            get { return _PropertyGrid.SelectedObject; }
            set { _PropertyGrid.SelectedObject = value; }
        }

        public PropertyGridUserControl()
        {
            InitializeComponent();

            _PropertyGrid = new System.Windows.Forms.PropertyGrid();
            windowsFormsHost1.Child = _PropertyGrid;
        }
    }
}
