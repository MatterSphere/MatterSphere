using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    class SetExpandedStateADP
    {
        public static readonly DependencyProperty SetExpandedProperty = DependencyProperty.RegisterAttached(
          "SetExpanded",
          typeof(bool),
          typeof(SetExpandedStateADP),
          new PropertyMetadata(new PropertyChangedCallback(SetExpanded_Changed))
        );

        public static void SetSetExpanded(DependencyObject element, bool value)
        {
            element.SetValue(SetExpandedProperty, value);
        }
        public static bool GetSetExpanded(DependencyObject element)
        {
            return (bool)element.GetValue(SetExpandedProperty);
        }

        private static void SetExpanded_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            bool? state = e.NewValue as bool?;
            if (state == null || !state.HasValue)
                return;

            var toggle = sender as System.Windows.Controls.Primitives.ToggleButton;

            toggle.SetCurrentValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, state.Value);
        }


        public static readonly DependencyProperty CurrentStageProperty = DependencyProperty.RegisterAttached(
          "CurrentStage",
          typeof(Milestones.MilestoneStage),
          typeof(SetExpandedStateADP),
          new PropertyMetadata(new PropertyChangedCallback(CurrentStage_Changed))
        );

        public static void SetCurrentStage(DependencyObject element, Milestones.MilestoneStage value)
        {
            element.SetValue(CurrentStageProperty, value);
        }
        public static Milestones.MilestoneStage GetCurrentStage(DependencyObject element)
        {
            return (Milestones.MilestoneStage)element.GetValue(CurrentStageProperty);
        }

        private static void CurrentStage_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
         
                


            bool isChecked = GetIsCheckedState(sender);

            sender.SetCurrentValue(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, isChecked);

        }

        private static bool GetIsCheckedState(DependencyObject sender)
        {
               var stage = GetCurrentStage(sender);
            

            if (stage == null)
                return false;

            if (stage.IsNextDue)
                return true;

            var stageStates = GetStageStates(sender);

            if (stageStates == null)
                return false;

            var stageState = GetViewStateManager(stage.Description, stageStates);

            return stageState.IsExpanded;
        }


        private static StageViewStateManager GetViewStateManager(string description, ObservableCollection<StageViewStateManager> stageStates)
        {
            var viewManager = stageStates.FirstOrDefault(s => s.Description == description);
            if (viewManager != null)
                return viewManager;

            viewManager = new StageViewStateManager();
            viewManager.Description = description;

            stageStates.Add(viewManager);

            return viewManager;
        }

        public static readonly DependencyProperty StageStatesProperty = DependencyProperty.RegisterAttached(
          "StageStates",
          typeof(ObservableCollection<StageViewStateManager>),
          typeof(SetExpandedStateADP),
          new PropertyMetadata(new PropertyChangedCallback(CurrentStage_Changed))
        );

        public static void SetStageStates(DependencyObject element, ObservableCollection<StageViewStateManager> value)
        {
            element.SetValue(StageStatesProperty, value);
        }
        public static ObservableCollection<StageViewStateManager> GetStageStates(DependencyObject element)
        {
            return (ObservableCollection<StageViewStateManager>)element.GetValue(StageStatesProperty);
        }

       
        
        

    }
}
