using System;
using System.Globalization;

namespace Fwbs.Oms.DialogInterceptor
{
    using Fwbs.WinFinder;
    using Microsoft.Win32;

    public static class DialogFactory
    {
        private static ProcessDialogDictionary dialogs = new ProcessDialogDictionary();
        private static DialogConfigDictionary globaldialogs = new DialogConfigDictionary();

        public static System.Collections.Generic.IEnumerable<string> ConfiguredProcesses
        {
            get { return dialogs.Keys; }
        }


        public static Dialog CreateDialog(Window win)
        {
            if (win == null)
                return null;

            string PROCNAME = String.Format("{0}.EXE", win.Process.ProcessName.ToUpperInvariant());
            string WINTITLE = win.Text.ToUpperInvariant();

            if (dialogs.ContainsKey(PROCNAME))
            {
                DialogConfigDictionary configs = dialogs[PROCNAME];
                
                foreach (DialogConfig config in configs.Values)
                {
                    if (config.WindowTitle.ToUpperInvariant() == WINTITLE)
                    {
                        switch (config.DialogType.ToUpperInvariant())
                        {
                            case "SAVEAS":
                                return new SaveAsDialog(win, config);
                            case "PRINT":
                                break;
                            case "OPEN":
                                break;
                            case "NEW":
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (win.IsValid)
            {
                //Centering the form to the parent does not help if it is minimised.  Especially if it is a dialog.
                if (win.Parent.IsMinimised || !win.Parent.IsVisible)
                    win.Centre(System.Windows.Forms.Screen.PrimaryScreen);
                else
                    win.Centre();
            }

            return null;

        }


        public static void ClearDialogConfiguration()
        {
            dialogs.Clear();
            globaldialogs.Clear();
            isDisabled = null;
            isDisabledSecondary = null;
        }

        public static void BuildDialogConfigurations()
        {
            using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"Software\Policies"))
            {
                if (!isDisabled.HasValue)
                    isDisabled = GetDisabled(reg);
                if (!isDisabledSecondary.HasValue)
                    isDisabledSecondary = GetDisabled(reg, true);
                BuildDialogConfigByKey(reg);
            }
            using (RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"Software\Policies"))
            {
                if (!isDisabled.HasValue)
                    isDisabled = GetDisabled(reg);
                if (!isDisabledSecondary.HasValue)
                    isDisabledSecondary = GetDisabled(reg, true);
                BuildDialogConfigByKey(reg);
            }
            using (RegistryKey reg = Registry.LocalMachine.OpenSubKey("Software"))
            {
                if (!isDisabled.HasValue)
                    isDisabled = GetDisabled(reg);
                if (!isDisabledSecondary.HasValue)
                    isDisabledSecondary = GetDisabled(reg, true);
                BuildDialogConfigByKey(reg);
            }
            using (RegistryKey reg = Registry.CurrentUser.OpenSubKey("Software"))
            {
                if (!isDisabled.HasValue)
                    isDisabled = GetDisabled(reg);
                if (!isDisabledSecondary.HasValue)
                    isDisabledSecondary = GetDisabled(reg, true);
                BuildDialogConfigByKey(reg);
            }
        }

        private static bool? isDisabled;
        public static bool IsDisabled
        {
            get
            {
                return isDisabled.GetValueOrDefault();
            }
        }

        private static bool? isDisabledSecondary;
        public static bool IsDisabledSecondary
        {
            get
            {
                return Environment.Is64BitOperatingSystem ? isDisabledSecondary.GetValueOrDefault() : true;
            }
        }

        private static bool? GetDisabled(RegistryKey hotkey, bool secondary = false)
        {
            using (RegistryKey root = hotkey.OpenSubKey(@"FWBS\OMS\DialogInterceptor", false))
            {
                try
                {
                    if (root == null)
                        return null;

                    object val = root.GetValue(secondary ? "DisableSecondary" : "Disable");
                    if (val == null)
                        return null;
                    else
                        return Convert.ToBoolean(val, CultureInfo.InvariantCulture);
                }
                catch (InvalidCastException)
                {
                    return null;
                }
                catch (FormatException)
                {
                    return null;
                }
            }
        }

        private static void BuildDialogConfigByKey(RegistryKey hotkey)
        {
            if (hotkey == null)
                return;

            using (RegistryKey dlgskey = hotkey.OpenSubKey(@"FWBS\OMS\DialogInterceptor\Dialogs", false))
            {
                if (dlgskey == null)
                    return;

                string[] dlgsnames = dlgskey.GetSubKeyNames();

                foreach (string name in dlgsnames)
                {
                    using (RegistryKey dlgkey = dlgskey.OpenSubKey(name))
                    {
                        if (dlgskey == null)
                            continue;

                        DialogConfig g_config = null;

                        if (globaldialogs.ContainsKey(name))
                            g_config = globaldialogs[name];
                        else
                        {
                            g_config = new DialogConfig();
                            globaldialogs.Add(name, g_config);
                        }

                        MergeDialogConfigByKey(dlgkey, name, g_config);
                    }
                }

            }


            using (RegistryKey procskey = hotkey.OpenSubKey(@"FWBS\OMS\DialogInterceptor\Processes", false))
            {
                if (procskey == null)
                    return;

                string[] keynames = procskey.GetSubKeyNames();
                foreach (string name in keynames)
                {
                    DialogConfigDictionary procdlgconfigs = null;
                    if (dialogs.ContainsKey(name))
                        procdlgconfigs = dialogs[name];
                    else
                    {
                        procdlgconfigs = new DialogConfigDictionary();
                        dialogs.Add(name, procdlgconfigs);
                    }

                    using (RegistryKey prockey = procskey.OpenSubKey(name))
                    {
                        if (prockey == null)
                            continue;

                        string[] keynames2 = prockey.GetSubKeyNames();
                        foreach (string name2 in keynames2)
                        {
                            using (RegistryKey procdlgkey = prockey.OpenSubKey(name2))
                            {
                                if (procdlgkey == null)
                                    continue;

                                DialogConfig config = null;
                                if (procdlgconfigs.ContainsKey(name2))
                                    config = procdlgconfigs[name2];
                                else
                                {
                                    config = new DialogConfig();
                                    procdlgconfigs.Add(name2, config);
                                }

                                MergeDialogConfigByKey(procdlgkey, name2, config);

                                if (globaldialogs.ContainsKey(name2))
                                    MergeDialogConfig(config, globaldialogs[name2]);
                                else
                                {
                                    DialogConfig g_config = new DialogConfig();
                                    g_config.DialogType = name2;
                                    globaldialogs.Add(name2, g_config);
                                }
                            }
                        }
                    }

                    

                }


            }
        }


        private static void MergeDialogConfigByKey(RegistryKey dlgkey, string name, DialogConfig config)
        {
            if (String.IsNullOrEmpty(config.DialogType))
                config.DialogType = name;
            if (String.IsNullOrEmpty(config.WindowClass))
                config.WindowClass = Convert.ToString(dlgkey.GetValue("WindowClass", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.WindowTitle))
                config.WindowTitle = Convert.ToString(dlgkey.GetValue("WindowTitle", String.Empty), CultureInfo.InvariantCulture);
            if (config.CancelDialogId < 0)
                config.CancelDialogId = Convert.ToInt32(dlgkey.GetValue("CancelDialogId", -1), CultureInfo.InvariantCulture);
            if (config.OkDialogId < 0)
                config.OkDialogId = Convert.ToInt32(dlgkey.GetValue("OkDialogId", -1), CultureInfo.InvariantCulture);
            if (config.FileNameDialogId < 0)
                config.FileNameDialogId = Convert.ToInt32(dlgkey.GetValue("FileNameDialogId", -1), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.CancelButtonClass))
                config.CancelButtonClass = Convert.ToString(dlgkey.GetValue("CancelButtonClass", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.CancelButtonText))
                config.CancelButtonText = Convert.ToString(dlgkey.GetValue("CancelButtonText", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.OkButtonClass))
                config.OkButtonClass = Convert.ToString(dlgkey.GetValue("OKButtonClass", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.OkButtonText))
                config.OkButtonText = Convert.ToString(dlgkey.GetValue("OKButtonText", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.FileNameClass))
                config.FileNameClass = Convert.ToString(dlgkey.GetValue("FileNameClass", String.Empty), CultureInfo.InvariantCulture);
            if (String.IsNullOrEmpty(config.FileExtension))
                config.FileExtension = Convert.ToString(dlgkey.GetValue("FileExtension", String.Empty), CultureInfo.InvariantCulture);

        }

        private static void MergeDialogConfig(DialogConfig config, DialogConfig master)
        {
            if (String.IsNullOrEmpty(config.DialogType))
                config.DialogType = master.DialogType;
            if (String.IsNullOrEmpty(config.WindowClass))
                config.WindowClass = master.WindowClass;
            if (String.IsNullOrEmpty(config.WindowTitle))
                config.WindowTitle = master.WindowTitle;
            if (config.CancelDialogId < 0)
                config.CancelDialogId = master.CancelDialogId;
            if (config.OkDialogId < 0)
                config.OkDialogId = master.OkDialogId;
            if (config.FileNameDialogId < 0)
                config.FileNameDialogId = master.FileNameDialogId;
            if (String.IsNullOrEmpty(config.CancelButtonClass))
                config.CancelButtonClass = master.CancelButtonClass;
            if (String.IsNullOrEmpty(config.CancelButtonText))
                config.CancelButtonText = master.CancelButtonText;
            if (String.IsNullOrEmpty(config.OkButtonClass))
                config.OkButtonClass = master.OkButtonClass;
            if (String.IsNullOrEmpty(config.OkButtonText))
                config.OkButtonText = master.OkButtonText;
            if (String.IsNullOrEmpty(config.FileNameClass))
                config.FileNameClass = master.FileNameClass;
            if (String.IsNullOrEmpty(config.FileExtension))
                config.FileExtension = master.FileExtension;
        }

    }
}
