using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Presentation.Services;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace FWBS.WF.OMS.ActivityLibrary
{
    public class ParametersPropertyValueEditor : DialogPropertyValueEditor
    {
        public ParametersPropertyValueEditor()
        {
            this.InlineEditorTemplate = new DataTemplate();

            //  Stack Panel
            FrameworkElementFactory stack = new FrameworkElementFactory(typeof(StackPanel));
            stack.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stack.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Right);

            //  Label
            FrameworkElementFactory label = new FrameworkElementFactory(typeof(Label));
            label.SetValue(Label.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            label.SetValue(Label.ContentProperty, "(Collection)");
            stack.AppendChild(label);

            //  Edit Button
            FrameworkElementFactory editModeSwitch = new FrameworkElementFactory(typeof(EditModeSwitchButton));
            editModeSwitch.SetValue(EditModeSwitchButton.TargetEditModeProperty, PropertyContainerEditMode.Dialog);
            editModeSwitch.SetValue(EditModeSwitchButton.HorizontalAlignmentProperty, HorizontalAlignment.Right);
            stack.AppendChild(editModeSwitch);

            this.InlineEditorTemplate.VisualTree = stack;
        }

        /// <summary>
        /// Show Dialog
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <param name="commandSource"></param>
        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            ModelPropertyEntryToOwnerActivityConverter propertyEntryConverter = new ModelPropertyEntryToOwnerActivityConverter();
            ModelItem modelItem = (ModelItem)propertyEntryConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null);

            var window = new ParametersEditor();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Topmost = true;
            window.Title = "Parameter Editor (" + propertyValue.ParentProperty.DisplayName + ")";

            //  Make a copy of values
            ObservableCollection<KeyParameters> restoreKeyParameters = new ObservableCollection<KeyParameters>();
            foreach (var value in (IEnumerable)propertyValue.Value)
            {
                KeyParameters kp = new KeyParameters();
                kp = (KeyParameters)value;
                restoreKeyParameters.Add(kp);
            }

            //  Bind and display parameters
            window.DataContext = modelItem;
            var result = window.ShowDialog();

            if (result.HasValue && result.Value)
            {
                if (window.lbParameters.Items.Count > 0)
                {
                    //  Update to new values
                    IList parameters = (IList)window.lbParameters.ItemsSource;

                    if (parameters != null)
                    {
                        propertyValue.Value = parameters;
                    }
                }
            }
            else
            {
                //  Restore the old values
                propertyValue.Value = restoreKeyParameters;
            }
        }

        private List<DynamicActivityProperty> GetProperties(PropertyValue propertyValue)
        {
            ModelPropertyEntryToOwnerActivityConverter propertyEntryConverter = new ModelPropertyEntryToOwnerActivityConverter();
            ModelItem modelItem = (ModelItem)propertyEntryConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null);
            EditingContext editingContext = modelItem.GetEditingContext();
            ModelService modelService = editingContext.Services.GetService<ModelService>();
            ModelItemCollection argumentsList = modelService.Root.Properties["Properties"].Collection;
            List<DynamicActivityProperty> list = new List<DynamicActivityProperty>();
            foreach (var current in argumentsList)
            {
                list.Add((DynamicActivityProperty)current.GetCurrentValue());
            }

            return list;
        }
    }
}
