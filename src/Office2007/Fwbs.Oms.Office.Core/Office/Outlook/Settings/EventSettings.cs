namespace Fwbs.Office.Outlook.Settings
{

    public class EventsSettings
    {
        public EventsSettings()
        {
            NewMail = new EventSettings();
            Paste = new EventSettings();
        }

        public EventSettings NewMail { get; private set; }

        public EventSettings Paste { get; private set; }

        public EventsSettings Clone()
        {
            var settings = (EventsSettings)MemberwiseClone();
            settings.NewMail = NewMail.Clone();
            settings.Paste = Paste.Clone();
            return settings;
        }
    }

    public class EventSettings
    {
        public bool Enabled { get; internal set; }

        public bool Delayed { get; internal set; }

        public EventSettings Clone()
        {
            return (EventSettings)MemberwiseClone();
        }

    }

}
