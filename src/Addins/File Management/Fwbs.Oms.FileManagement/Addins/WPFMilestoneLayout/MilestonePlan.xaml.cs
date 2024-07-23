using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{

    /// <summary>
    /// Interaction logic for MilestonePlan.xaml
    /// </summary>
    public partial class MilestonePlan : UserControl
    {
        public MilestonePlan()
        {
            InitializeComponent();
            
        }

        public void SetupData(MilestonePlanVM vm)
        {
            this.DataContext = vm;
            this.Resources.Add("VM", vm);

            var proxy = this.Resources["ModelProxy"] as ModelProxy;
            proxy.Model = vm;
                        
        }

        public void Shutdown()
        {
            var proxy = this.Resources["ModelProxy"] as ModelProxy;
            proxy.Model = null;
            this.DataContext = null;            
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = e.OriginalSource as Grid;
            if (grid == null)
            {
                var element = e.OriginalSource as DependencyObject;

                if (element != null)
                    grid = VisualTreeHelper.GetParent(element) as Grid;
            }
                
            if (grid == null)
                return;
            
            var name = grid.GetValue(Grid.NameProperty);

            if (name == null || name.ToString() != "StageHeader")
                return;
            
            var ctrl = VisualTreeHelper.GetChild(grid, 4) as System.Windows.Controls.Primitives.ToggleButton;
            if (ctrl == null)
                return;

            var isChecked = (bool)ctrl.GetValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty);

            ctrl.SetValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, !isChecked);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var ctrl = sender as Control;

            var stage = ctrl.DataContext as Milestones.MilestoneStage;

            ((MilestonePlanVM)this.DataContext).SelectedStage = stage;
        }       

        private void ListBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var ctrl = sender as Control;

            var stage = ctrl.DataContext as Milestones.MilestoneStage;

            ((MilestonePlanVM)this.DataContext).SelectedStage = stage;
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var lb = sender as ListBox;
            if (lb == null)
                return;
            lb.SelectedIndex = -1;
        }

        private void TasksExpand_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Primitives.ToggleButton;

            var stage = btn.DataContext as Milestones.MilestoneStage;

            var vm = this.DataContext as MilestonePlanVM;

            vm.UpdateStageState(stage, btn.IsChecked.Value);
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            var menu = (System.Windows.Controls.ContextMenu)sender;

            if (menu.Items == null)
                return;

            foreach (var item in menu.Items)
            {
                var disposable = item as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }

    public class SeparatorStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is WPFSplitterViewAction)
            {
                return (Style)((FrameworkElement)container).FindResource("separatorStyle");
            }

            if (item is WPFDisabledViewAction)
            {
                return (Style)((FrameworkElement)container).FindResource("msd");
            }

            return (Style)((FrameworkElement)container).FindResource("ms"); 
        }
    }

}
