using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Converters;
using System.Windows.Data;
using Microsoft.VisualBasic.Activities;

namespace FWBS.MatterCentre.Controls
{
    class TypeToArgumentTypeConverter : IValueConverter
    {
        ArgumentToExpressionConverter argC;
        public TypeToArgumentTypeConverter()
        {
            argC = new ArgumentToExpressionConverter();   
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            
            var convertValue = argC.Convert(value, targetType, parameter, culture);

            if (convertValue == null)
            {
                InArgument<object> inArg = value as InArgument<object>;

                if (inArg != null)
                {
                    Activity<object> expression = inArg.Expression;
                    VisualBasicValue<object> vbexpression = expression as VisualBasicValue<object>;
                    Literal<object> literal = expression as Literal<object>;
                    if (literal != null)
                    {
                        return literal.Value;
                    }
                    else if (vbexpression != null)
                    {
                        return new InArgument<object>((object)vbexpression.ExpressionText);
                    }
                }

                return value;
            }

            return convertValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = argC.ConvertBack(value, targetType, parameter, culture);

            if (val == null)
            {
                return value;
            }

            return val;
        }
    }
}
