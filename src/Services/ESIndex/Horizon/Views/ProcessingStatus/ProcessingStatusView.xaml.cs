using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Horizon.Views.ProcessingStatus
{
    /// <summary>
    /// Interaction logic for ProcessHistoryView.xaml
    /// </summary>
    public partial class ProcessingStatusView : UserControl
    {
        private ScrollViewer _scrollViewer;

        public ProcessingStatusView()
        {
            InitializeComponent();
        }

        private void StatusDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = FindVisualChild<ScrollViewer>(StatusDataGrid);
        }

        private void StatusDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer = null;
        }

        private void DetailDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            e = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta) { RoutedEvent = MouseWheelEvent, Source = sender };
            _scrollViewer?.RaiseEvent(e);
        }

        private static ChildItem FindVisualChild<ChildItem>(DependencyObject obj) where ChildItem : DependencyObject
        {
            for (int i = 0, n = VisualTreeHelper.GetChildrenCount(obj); i < n; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is ChildItem)
                {
                    return (ChildItem)child;
                }
                else
                {
                    ChildItem childOfChild = FindVisualChild<ChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
