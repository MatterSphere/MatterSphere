using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class CultureConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string code = values[0].ToString();
            string text = values[1].ToString();

            

            var vals = values.ToList();
            vals.Remove(code);
            vals.Remove(text);

            var repVals = new List<string>();

            foreach (var val in vals)
            {
                if (val == null)
                    repVals.Add(string.Empty);
                else if (val is DateTime)
                    repVals.Add(((DateTime)val).ToString("d"));
                else
                    repVals.Add(val.ToString());
            }


            var ret = Session.CurrentSession.Resources.GetResource(code, text, "",true, repVals.ToArray()).Text;


            return ret;
            

        }

        

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
