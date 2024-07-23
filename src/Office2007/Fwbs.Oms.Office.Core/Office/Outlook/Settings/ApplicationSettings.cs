using System;
using System.Globalization;

namespace Fwbs.Office.Outlook.Settings
{
    public sealed class ApplicationSettings
    {
        private const string RegKey = @"software\fwbs\office\outlook";
        private const string PolicyKey = @"software\policies\fwbs\office\outlook";
        private OutlookApplication app;

        public ApplicationSettings(OutlookApplication app)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            this.app = app;

            this.KeyHooks = new KeyHookSettings();
            this.Memory = new MemorySettings();
            this.Events = new EventsSettings();
            this.Activation = new ActivationSettings();

            Load(this.Activation);
            Load(this.KeyHooks);

            var useCommands = this.KeyHooks.UseCommands;
            int appMajorVersion = this.app.MajorVersion;
            if (appMajorVersion >= 14)
            {
                this.KeyHooks.UseCommands = false;
            }
            Load(this.Memory);
            Load(this.Events);            
            
            ListWindowClass = GetValue<string>("ListWindowClass", appMajorVersion > 14 ? "OUTLOOKGRID" : "SUPERGRID");
            IgnoredFolders = GetValue<string[]>("IgnoredFolders", new string[0]);
            DisallowedWindowTitles = GetValue<string[]>("DisallowedWindowTitles", new string[] { ".*?ADVANCED FIND.*?" });
            

            if (useCommands)
            {
                if (KeyHooks.Delay < 0)
                    KeyHooks.Delay = 100;
            }
        }

        public ActivationSettings Activation { get; private set; }

        public EventsSettings Events {get;private set;}
        
        public KeyHookSettings KeyHooks { get; private set; }

        public MemorySettings Memory{get;private set;}

        public string ListWindowClass { get; private set; }

        public string [] IgnoredFolders {get;private set;}

        public string [] DisallowedWindowTitles{get;private set;}



        public bool CheckSpellingOnSend
        {
            get
            {
                using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(String.Format(CultureInfo.InvariantCulture, @"software\microsoft\office\{0}\outlook\options\spelling", app.GetVersion()), true))
                {
                    if (reg == null)
                        return false;

                    return Convert.ToBoolean(reg.GetValue("Check", false));
                }
            }

        }

        public bool IsWordEmailEditorEnabled
        {
            get
            {
                using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(String.Format(CultureInfo.InvariantCulture, @"Software\Microsoft\Office\{0}\Outlook\Options\Mail", app.GetVersion()), true))
                {
                    if (reg == null)
                        return true;

                    var editor = Convert.ToInt32(reg.GetValue("EditorPreference", -1));
                    if ((editor == -1) || ((editor | 1) == editor))
                        return true;
                }

                return false;
            }
        }



        public void TurnOffWordEmailEditing()
        {
            using (var reg = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(String.Format(CultureInfo.InvariantCulture, @"Software\Microsoft\Office\{0}\Outlook\Options\Mail", app.GetVersion())))
            {
                reg.SetValue("EditorPreference", 131072);
            }
        }

        public static void Load(ActivationSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.ShellExecuteEnabled = GetValue<bool>("Activation", "ShellExecuteEnabled", false);
            settings.ForceRelease = GetValue<bool>("Activation", "ForceRelease", false);
            settings.ProcessName = GetValue<string>("Activation", "ProcessName", "OUTLOOK");
            settings.ExePath = GetValue<string>("Activation", "ExePath", "OUTLOOK.EXE");
            settings.MaxTries = GetValue<int>("Activation", "MaxTries", 6);
            settings.WaitTimeout = GetValue<int>("Activation", "WaitTimeout", 5000);
            settings.MutexName = GetValue<string>("Activation", "MutexName", "fwbs.office.outlook");
        }

        public static void Load(KeyHookSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.Enabled = GetValue<bool>("KeyHooks", "Enabled", GetValue<bool>("EnableKeyHook", true));
            settings.Delay = GetValue<int>("KeyHooks", "Delay", GetValue<int>("KeyHookTimer", -1));
            settings.UseCommands = GetValue<bool>("KeyHooks", "UseCommands", true);

            settings.SaveKey.Enabled = GetValue<bool>(@"KeyHooks\Save", "Enabled", true);
            settings.PrintKey.Enabled = GetValue<bool>(@"KeyHooks\Print", "Enabled", false);
            settings.OpenKey.Enabled = GetValue<bool>(@"KeyHooks\Open", "Enabled", true);
            settings.DeleteKey.Enabled = GetValue<bool>(@"KeyHooks\Delete", "Enabled", true);
        }

        public static void Load(MemorySettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.AutoGarbageCollection = GetValue<bool>("Memory", "AutoGarbageCollection", GetValue<bool>("AutoGarbageCollection", true));
            settings.MultipleItemChunkSize = GetValue<int>("Memory", "MultipleItemChunkSize", 10);
            settings.MultipleItemWarningSize = GetValue<int>("Memory", "MultipleItemWarningSize", 100);
            settings.MultipleItemWarningEnabled = GetValue<bool>("Memory", "MultipleItemWarningEnabled", false);
            settings.MultipleItemWarningMessage = GetValue<string>("Memory", "MultipleItemWarningMessage", "The number of items that have been selected is more than the recommended amount when using online mode.");

        }

        public static void Load(EventsSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            settings.NewMail.Enabled = GetValue<bool>(@"Events\NewMail", "Enabled", GetValue<bool>("NewMailEventEnabled", true));
            settings.NewMail.Delayed = false;
            settings.Paste.Enabled = GetValue<bool>(@"Events\Paste", "Enabled", true);
            settings.Paste.Delayed = GetValue<bool>(@"Events\Paste", "Delayed", true);
        }

        private static T GetValue<T>(string name, T def)
        {
            return GetValue<T>(null, name, def); 
        }

        private static T GetValue<T>(string subkey, string name, T def)
        {
            if (!String.IsNullOrEmpty(subkey) && !subkey.StartsWith(@"\"))
                subkey = @"\" + subkey;

            bool exists = false;
            var value = GetRegistryValue<T>(Microsoft.Win32.Registry.LocalMachine.OpenSubKey(PolicyKey + subkey), name, ref exists);

            if (!exists)
                value = GetRegistryValue<T>(Microsoft.Win32.Registry.CurrentUser.OpenSubKey(PolicyKey + subkey), name, ref exists);

            if (!exists)
                value = GetRegistryValue<T>(Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKey + subkey), name, ref exists);

            if (!exists)
                value = GetRegistryValue<T>(Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegKey + subkey), name, ref exists);

            if (exists)
            {
                return value;
            }

            return def;
        }

        private static T GetRegistryValue<T>(Microsoft.Win32.RegistryKey subkey, string name, ref bool exists)
        {
            T ret = default(T);
            if (subkey != null)
            {
                var val = subkey.GetValue(name, null);
                if (val != null)
                {
                    ret = (T)Convert.ChangeType(val, typeof(T));
                    exists = true;
                }
                subkey.Close();
            }
            return (T)ret;
        }


        private Func<bool> isConnected = ()=> false;

        public Func<bool> IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value ?? (()=> false);
            }
        }

        public ApplicationSettings Clone()
        {
            var settings = (ApplicationSettings)MemberwiseClone();
            settings.KeyHooks = KeyHooks.Clone();
            settings.Memory = Memory.Clone();
            settings.Activation = Activation.Clone();
            return settings;
        }

   
    }
}
