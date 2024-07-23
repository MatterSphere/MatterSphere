using System;
using System.Globalization;

namespace FWBS.OMS.Dashboard
{
    public class KeyDateRow
    {
        public KeyDateRow(int id, DateTime date, string description, long fileId)
        {
            Id = id;
            Date = date;
            Description = description;
            FileId = fileId;
        }

        public int Id { get; }
        public DateTime Date { get; }
        public string Description { get; }
        public long FileId { get; }

        public string KeyDateType { get; set; }
        public string ClientNo { get; set; }
        public string FileNo { get; set; }

        public int Day
        {
            get { return Date.Day; }
        }

        public string DateLabel
        {
            get { return $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Date.Month)} {Date.Day}"; }
        }
    }
}
