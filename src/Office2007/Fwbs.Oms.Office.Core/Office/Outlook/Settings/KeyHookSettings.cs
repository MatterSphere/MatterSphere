namespace Fwbs.Office.Outlook.Settings
{

    public class KeyHookSettings
    {
        public KeyHookSettings()
        {
            PrintKey = new KeySettings();
            SaveKey = new KeySettings();
            OpenKey = new KeySettings();
            DeleteKey = new KeySettings();
        }

        public bool Enabled { get; internal set; }

        public int Delay { get; internal set; }

        public bool UseCommands { get; internal set; }

        public KeySettings PrintKey { get; private set; }

        public KeySettings DeleteKey { get; private set; }

        public KeySettings SaveKey { get; private set; }

        public KeySettings OpenKey { get; private set; }

        public KeyHookSettings Clone()
        {
            var settings = (KeyHookSettings)MemberwiseClone();
            settings.PrintKey = PrintKey.Clone();
            settings.DeleteKey = PrintKey.Clone();
            settings.SaveKey = PrintKey.Clone();
            settings.OpenKey = PrintKey.Clone();
            return settings;
        }
    }

    public class KeySettings
    {
        public bool Enabled { get; internal set; }

        public KeySettings Clone()
        {
            return (KeySettings)MemberwiseClone();
        }

    }

}
