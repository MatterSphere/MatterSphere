using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FWBS.OMS.Drive
{
    using Fwbs.Oms.DialogInterceptor;

    static class Registry
    {
        private const string DriveKey = "VirtualDrive";
        private static readonly HashSet<string> WriteAllowedProcesses;
        private static readonly HashSet<string> ManagedOfficeProcesses;

        static Registry()
        {
            DialogFactory.BuildDialogConfigurations();
            WriteAllowedProcesses = new HashSet<string>(DialogFactory.ConfiguredProcesses, StringComparer.OrdinalIgnoreCase);
            DialogFactory.ClearDialogConfiguration();
            ManagedOfficeProcesses = new HashSet<string>(new string[] { "WINWORD", "EXCEL", "OUTLOOK" }, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsProcessWriteAllowed(int processId)
        {
            try
            {
                Process process = Process.GetProcessById(processId);
                return WriteAllowedProcesses.Contains(process.ProcessName + ".EXE");
            }
            catch
            {
                return false;
            }
        }

        public static bool IsManagedOfficeProcess(int processId)
        {
            try
            {
                Process process = Process.GetProcessById(processId);
                return ManagedOfficeProcesses.Contains(process.ProcessName);
            }
            catch
            {
                return false;
            }
        }

        public static bool ShowAllFolders
        {
            get
            {
                var appSetting = new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, DriveKey, "ShowAllFolders");
                return Common.ConvertDef.ToBoolean(appSetting.GetSetting(true), true);
            }
        }

        public static int MetadataCacheTimeout
        {
            get
            {
                var appSetting = new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, DriveKey, "MetadataCacheTimeout");
                return Math.Max(Common.ConvertDef.ToInt32(appSetting.GetSetting(15), 15), 5);
            }
        }

        public static int BinaryCacheTimeout
        {
            get
            {
                var appSetting = new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, DriveKey, "BinaryCacheTimeout");
                return Math.Max(Common.ConvertDef.ToInt32(appSetting.GetSetting(15 * 60), 15 * 60), 5 * 60);
            }
        }

        public static int OperationTimeout
        {
            get
            {
                var appSetting = new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, DriveKey, "OperationTimeout");
                return Math.Max(Common.ConvertDef.ToInt32(appSetting.GetSetting(60), 60), 30);
            }
        }

        public static int MaxDatabaseConnections
        {
            get
            {
                var appSetting = new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, DriveKey, "MaxDatabaseConnections");
                return Math.Max(Common.ConvertDef.ToInt32(appSetting.GetSetting(3), 3), 1);
            }
        }
    }
}
