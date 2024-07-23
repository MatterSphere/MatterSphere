using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using COM = System.Runtime.InteropServices.ComTypes;

namespace FWBS.OMS.UI.Windows.Admin
{
    internal class InteropDefs
    {


        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int RegisterApplicationRestart([MarshalAs(UnmanagedType.LPWStr)] string CommandLine, uint dwFlags);

        // Declared in RestartManager.h
        // DWORD RmStartSession(DWORD *pSessionHandle,DWORD dwSessionFlags, __out_ecount(CCH_RM_SESSION_KEY+1) WCHAR strSessionKey[]);
        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Auto, PreserveSig = true)]
        internal static extern int RmStartSession(ref uint pSessionHandle, uint reserved, StringBuilder strSessionKey);

        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Auto, PreserveSig = true)]
        internal static extern int RmEndSession(uint sessionHandle);

        //DWORD RmRegisterResources(DWORD dwSessionHandle,UINT nFiles,LPCWSTR rgsFileNames[],UINT nApplications,RM_UNIQUE_PROCESS rgApplications[],
        //UINT nServices,LPCWSTR rgsServiceNames[]);
        [DllImport("Rstrtmgr.dll", CharSet = CharSet.Auto, PreserveSig = true)]
        internal static extern int RmRegisterResources(uint sessionHandle,
            int nFiles, string[] rgsFileNames,
            int nApps, RM_UNIQUE_PROCESS[] rgApps,
            int nServices, string[] rgsServiceNames);

        // DWORD RmShutdown(DWORD dwSessionHandle, ULONG lActionFlags, RM_WRITE_STATUS_CALLBACK fnStatus);
        [DllImport("Rstrtmgr.dll", PreserveSig = true)]
        internal static extern int RmShutdown(uint sessionHandle, uint actionFlags, IntPtr ParamNotUsed);

        [DllImport("Rstrtmgr.dll", PreserveSig = true)]
        internal static extern int RmRestart(uint sessionHandle, uint reserved, IntPtr ParamNotUsed);

        // DWORD RmGetList(DWORD dwSessionHandle,UINT* pnProcInfoNeeded,UINT* pnProcInfo, RM_PROCESS_INFO rgAffectedApps[ ], LPDWORD lpdwRebootReasons);
        [DllImport("Rstrtmgr.dll", PreserveSig = true)]
        internal static extern int RmGetList(uint sessionHandle, out int nProcInfoNeeded,
            ref uint pProcInfo, ref RM_PROCESS_INFO[] rgAffectedApps, out uint RebootReason);

        //DWORD WINAPI RmCancelCurrentTask(DWORD dwSessionHandle);
        [DllImport("Rstrtmgr.dll", PreserveSig = true)]
        internal static extern int RmCancelCurrentTask(uint dwSessionHandle);

        [DllImport("Kernel32.dll", PreserveSig = true)]
        internal static extern uint GetApplicationRestartSettings(IntPtr hProcess, IntPtr ParamNotUsed1,
            ref uint pcchSize, IntPtr ParamNotUsed2);

        // BOOL GetProcessTimes(HANDLE hProcess, LPFILETIME lpCreationTime,LPFILETIME lpExitTime,LPFILETIME lpKernelTime,LPFILETIME lpUserTime);
        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern bool GetProcessTimes(IntPtr Process,
            out COM.FILETIME lpCreationTime, out COM.FILETIME lpExitTime,
            out COM.FILETIME lpKernelTime, out COM.FILETIME lpUserTime);
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RM_UNIQUE_PROCESS
    {
        uint dwProcessId;              // PID
        COM.FILETIME ProcessStartTime; // Process creation time

        internal RM_UNIQUE_PROCESS(int PID)
        {
            dwProcessId = (uint)PID;

            System.Diagnostics.Process s = System.Diagnostics.Process.GetProcessById(PID);

            COM.FILETIME exitTime;
            COM.FILETIME kernelTime;
            COM.FILETIME userTime;
            InteropDefs.GetProcessTimes(s.Handle, out ProcessStartTime, out exitTime, out kernelTime, out userTime);
        }
    }

    internal enum RM_APP_TYPE
    {
        RmUnknownApp = 0,
        RmMainWindow = 1,
        RmOtherWindow = 2,
        RmService = 3,
        RmExplorer = 4,
        RmConsole = 5,
        RmCritical = 6,
    }

    [Flags]
    internal enum RM_APP_STATUS
    {
        RmStatusUnknown = 0x0,
        RmStatusRunning = 0x1,
        RmStatusStopped = 0x2,
        RmStatusStoppedOther = 0x4,
        RmStatusRestarted = 0x8,
        RmStatusErrorOnStop = 0x10,
        RmStatusErrorOnRestart = 0x20,
        RmStatusShutdownMasked = 0x40,
        RmStatusRestartMasked = 0x80,
    }

    internal enum RM_REBOOT_REASON
    {
        RmRebootReason = 0x0,
        RmRebootReasonPermissionDenied = 0x1,
        RmRebootReasonSessionMismatch = 0x2,
        RmRebootReasonCriticalProcess = 0x4,
        RmRebootReasonCriticalService = 0x8,
        RmRebootReasonDetectedSelf = 0x10,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct RM_PROCESS_INFO
    {
        RM_UNIQUE_PROCESS Process;
        string strAppName;
        string strServiceShortName;
        RM_APP_TYPE ApplicationType;
        uint AppStatus;
        uint TSSessionId;
        bool bRestartable;
    }

    public class RestartManagerSession
    {
        private uint sessionHandle = 0;
        private List<string> files = new List<string>();
        private List<string> services = new List<string>();
        private List<RM_UNIQUE_PROCESS> apps = new List<RM_UNIQUE_PROCESS>();

        public RestartManagerSession()
        {
            StartSession();
        }

        ~RestartManagerSession()
        {
            EndSession();
        }

        private void StartSession()
        {
            StringBuilder sb = new StringBuilder(Marshal.SizeOf(typeof(Guid)) * 2);
            int retVal = InteropDefs.RmStartSession(ref this.sessionHandle, 0, sb);
            if (retVal != 0)
            {
                throw new ApplicationException(Session.CurrentSession.Resources.GetMessage("STARTRETURN", "RmStartSession returned: 0x%1%", "", retVal.ToString()).Text);
            }
        }

        public void EndSession()
        {
            ClearSession(false);

            if (sessionHandle != 0)
            {
                InteropDefs.RmEndSession(this.sessionHandle);
                this.sessionHandle = 0;
            }
        }

        public void ClearSession(bool CancelTask)
        {
            if (CancelTask)
            {
                InteropDefs.RmCancelCurrentTask(sessionHandle);
                sessionHandle = 0;
            }

            files.Clear();
            services.Clear();
            apps.Clear();
        }

        public void AddFileResource(string FilePath)
        {
            files.Add(FilePath);
        }

        public void AddAppResource(int ProcessId)
        {
            RM_UNIQUE_PROCESS p = new RM_UNIQUE_PROCESS(ProcessId);
            apps.Add(p);
        }

        public void AddService(string ServiceShortName)
        {
            services.Add(ServiceShortName);
        }

        public int GetResourceUseInfo()
        {
            int nProcNeeded = 0;
            uint numProc = 0;
            RM_PROCESS_INFO[] procInfo = new RM_PROCESS_INFO[5];
            uint rebootReason = 0;

            int retVal = InteropDefs.RmGetList(this.sessionHandle, out nProcNeeded, ref numProc, ref procInfo, out rebootReason);

            return (retVal);
        }
        public int RegisterResources()
        {
            int retVal = InteropDefs.RmRegisterResources(this.sessionHandle,
                files.Count, ((files.Count == 0) ? null : files.ToArray()),
                apps.Count, ((apps.Count == 0) ? null : apps.ToArray()),
                services.Count, ((services.Count == 0) ? null : services.ToArray()));

            return (retVal);
        }

        public int Shutdown(int Flags)
        {
            uint flags = (uint)Flags;

            int retVal = InteropDefs.RmShutdown(this.sessionHandle, flags, IntPtr.Zero);

            return (retVal);
        }

        public int Restart()
        {
            int retVal = InteropDefs.RmRestart(this.sessionHandle, 0, IntPtr.Zero);
            return (retVal);
        }

    }
}
