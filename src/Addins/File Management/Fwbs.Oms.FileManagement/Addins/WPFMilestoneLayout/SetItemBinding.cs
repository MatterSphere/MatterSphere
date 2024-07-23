using System.Windows;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class SetItemBinding
    {

        public static readonly DependencyProperty StageTasksBindingProperty = DependencyProperty.RegisterAttached(
              "StageTasksBinding",
              typeof(Milestones.MilestoneStage),
              typeof(SetItemBinding),
              new PropertyMetadata(new PropertyChangedCallback(StageTasksBinding_Changed))
            );

        public static void SetStageTasksBinding(DependencyObject element, MilestoneStage value)
        {
            element.SetValue(StageTasksBindingProperty, value);
        }
        public static Milestones.MilestoneStage GetStageTasksBinding(DependencyObject element)
        {
            return (Milestones.MilestoneStage)element.GetValue(StageTasksBindingProperty);
        }

        private static void StageTasksBinding_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var lb = sender as System.Windows.Controls.ListBox;
            var stage = lb.DataContext as Milestones.MilestoneStage;
            var newStage = e.NewValue as Milestones.MilestoneStage;
            if (stage == newStage)
            {
                var binding = new System.Windows.Data.Binding(" DataContext.SelectedTask");
                binding.ElementName = "Root";

                lb.SetBinding(System.Windows.Controls.ListBox.SelectedItemProperty, binding);
            }
            else
            {
                lb.SelectedItem = null;
                lb.SelectedIndex = -1;
            }

        }
    }

}
