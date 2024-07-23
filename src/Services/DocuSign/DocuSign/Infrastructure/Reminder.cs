using System;

namespace FWBS.OMS.DocuSign
{
    public class Reminder
    {
        public Reminder(DateTime startDate, int frequency)
        {
            Delay = Math.Max((startDate - DateTime.Today).Days, 1);
            Frequency = Math.Max(frequency, 1);
        }

        internal Reminder(int delay, int frequency)
        {
            Delay = delay;
            Frequency = frequency;
        }

        public int Delay { get; internal set; }
        public int Frequency { get; internal set; }
    }
}