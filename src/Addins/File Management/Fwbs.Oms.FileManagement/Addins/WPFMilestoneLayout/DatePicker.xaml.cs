using System;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {
        private CultureInfo cultureInfo;

        public DatePicker()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            if (!new FWBS.OMS.StatusManagement.FileActivity(Session.CurrentSession.CurrentFile, FWBS.OMS.StatusManagement.Activities.FileStatusActivityType.TaskflowProcessing).IsAllowed())
            {
                this.date.IsEnabled = false;
                this.toggle.IsEnabled = false;
            }

            cultureInfo = Session.CurrentSession.DefaultCultureInfo;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (toggle.IsChecked.HasValue && toggle.IsChecked.Value)
                toggle.IsChecked = false;
        }

    }

    class ConvertBackForTextBox : IValueConverter
    {
        DateConverter dc;
        CultureInfo cultureInfo;

        public ConvertBackForTextBox()
        {
            dc = new DateConverter();
            cultureInfo = Session.CurrentSession.DefaultCultureInfo;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return dc.Convert(value, targetType, parameter, cultureInfo); //culture removed
        }
    }

    class DateConverter : IValueConverter
    {
        CultureInfo cultureInfo;

        public DateConverter()
        {
            cultureInfo = Session.CurrentSession.DefaultCultureInfo;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var d = value as DateTime?;
            if (d != null && d.HasValue)
            {
                return d.Value.ToLocalTime().ToString("d", cultureInfo.DateTimeFormat);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value as string;
            try
            {
                DateTime time= DateTime.Now;
               if(DateTime.TryParse(text, GetDateFormat(cultureInfo), DateTimeStyles.AssumeLocal, out time))
                    return DateTime.SpecifyKind(time, DateTimeKind.Local);

               return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
        {
            if (culture.Calendar is GregorianCalendar)
            {
                return culture.DateTimeFormat;
            }
            GregorianCalendar calendar = null;
            DateTimeFormatInfo dateTimeFormat = null;
            foreach (System.Globalization.Calendar calendar2 in culture.OptionalCalendars)
            {
                if (calendar2 is GregorianCalendar)
                {
                    if (calendar == null)
                    {
                        calendar = calendar2 as GregorianCalendar;
                    }
                    if (((GregorianCalendar)calendar2).CalendarType == GregorianCalendarTypes.Localized)
                    {
                        calendar = calendar2 as GregorianCalendar;
                        break;
                    }
                }
            }
            if (calendar == null)
            {
                dateTimeFormat = Session.CurrentSession.DefaultCultureInfo.DateTimeFormat;
                dateTimeFormat.Calendar = new GregorianCalendar();
                return dateTimeFormat;
            }
            dateTimeFormat = Session.CurrentSession.DefaultCultureInfo.DateTimeFormat;
            dateTimeFormat.Calendar = calendar;
            return dateTimeFormat;
        }
    }
}
