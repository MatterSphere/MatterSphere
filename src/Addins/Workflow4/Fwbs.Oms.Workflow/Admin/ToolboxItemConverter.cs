using System;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FWBS.OMS.Workflow.Admin
{
    internal class ToolboxItemConverter : IValueConverter
    {
        static ResourceDictionary iconsDict;
        public string DefaultResource { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Drawing retVal = null;
            ToolboxItemWrapper wrapper = value as ToolboxItemWrapper;

            if (null != wrapper)
            {
                //  Generate the resource name
                string resourceName = GenerateResourceName(wrapper.Type);
                DefaultResource = resourceName;

                iconsDict = iconsDict ?? new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/System.Activities.Presentation;component/themes/icons.xaml")
                };

                object resource = iconsDict.Contains(resourceName) ? iconsDict[resourceName] : null;
                DrawingBrush drawingBrush = resource as DrawingBrush;
                if (drawingBrush != null)
                {
                    retVal = drawingBrush.Drawing;
                }
                else
                {
                    //  And lookup in app resources
                    DrawingBrush icon = Application.Current.Resources[resourceName] as DrawingBrush;

                    //  No icon found, now try the designer - this is really the last resort
                    if (icon == null)
                    {
                        // Get the [Designer] for the passed activity
                        DesignerAttribute designer = Attribute.GetCustomAttribute(wrapper.Type, typeof(DesignerAttribute)) as DesignerAttribute;

                        if (designer != null)
                        {
                            ActivityDesigner ad = Activator.CreateInstance(Type.GetType(designer.DesignerTypeName)) as ActivityDesigner;
                            if (ad != null)
                            {
                                icon = ad.Icon;
                            }
                        }
                    }

                    //  If not found, provide a fallback
                    if (icon == null)
                    {
                        icon = iconsDict["GenericLeafActivityIcon"] as DrawingBrush;
                    }

                    if (icon != null)
                    {
                        retVal = icon.Drawing;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Convert Back
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate Resource Name
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private string GenerateResourceName(Type t)
        {
            string resource = this.DefaultResource;

            if (t.IsGenericType)
            {
                resource = string.Concat(t.Name.Substring(0, t.Name.IndexOf('`')), "Icon");

                if (resource.Equals("ForEachWithBodyFactoryIcon"))
                {
                    resource = "ForEachIcon";
                }
                else if (resource.Equals("ParallelForEachWithBodyFactoryIcon"))
                {
                    resource = "ParallelForEachIcon";
                }
            }
            else
            {
                switch (t.Name)
                {
                    case "Flowchart":
                        {
                            resource = "FlowChartIcon";
                            break;
                        }
                    case "TransactedReceiveScope":
                        {
                            resource = "TransactionReceiveScopeIcon";
                            break;
                        }
                    case "ReceiveAndSendReplyFactory":
                        {
                            resource = "ReceiveAndSendReplyIcon";
                            break;
                        }
                    case "SendAndReceiveReplyFactory":
                        {
                            resource = "SendAndReceiveReplyIcon";
                            break;
                        }
                    default:
                        {
                            resource = string.Concat(t.Name, "Icon");
                            break;
                        }
                }
            }

            return resource;
        }
    }
}
