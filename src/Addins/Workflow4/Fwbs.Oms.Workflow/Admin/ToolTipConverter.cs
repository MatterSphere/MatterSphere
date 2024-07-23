using System;
using System.Activities.Presentation.Toolbox;
using System.Collections.Generic;
using System.Windows.Data;

namespace FWBS.OMS.Workflow.Admin
{
    [ValueConversion(typeof(ToolboxItemWrapper), typeof(string))]
    public class ToolTipConverter : IValueConverter
    {
        public static Dictionary<ToolboxItemWrapper, string> ToolTipDic = new Dictionary<ToolboxItemWrapper, string>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ToolboxItemWrapper itemWrapper = (ToolboxItemWrapper)value;

            if (ToolTipDic.ContainsKey(itemWrapper))
            {
                return ToolTipDic[itemWrapper];
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
