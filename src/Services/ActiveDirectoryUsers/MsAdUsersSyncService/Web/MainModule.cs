using System;
using System.Threading;
using MsAdUsersSyncService.MSADSync;
using Nancy;

namespace MsAdUsersSyncService.Web
{
    public class MainModule : NancyModule
    {
        private static ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(false);
        private static string _syncError = null;
        private static string _syncSuccess = null;

        public MainModule() : base("/")
        {
            Get["/"] = x =>
            {
                return View["index.html"];
            };

            Get["/state"] = s =>
            {
                if (manualResetEventSlim.IsSet)
                {
                    return "InProgress";
                }

                if (_syncError == "" && _syncSuccess != null)
                {
                    return $"Success: {_syncSuccess}";
                }

                if (_syncError != null && _syncError.StartsWith("Error:"))
                {
                    return _syncError;
                }               

                return "NotInProgress";
            };

            Post["/syncntusers"] = parameters =>
            {

                if (!manualResetEventSlim.IsSet)
                {
                    manualResetEventSlim.Set();

                    _syncError = null;
                    _syncSuccess = null;
                    try
                    {
                        MatterSphereDB msdb = new MatterSphereDB();

                        if (msdb.IsAAD)
                            msdb.SyncUsers(msdb.MakeUpnUserName, msdb.ControllerDomainName);
                        else
                            msdb.SyncUsers(msdb.MakeNtUserName, msdb.NtDomainName);

                        _syncSuccess = "Users have been synchronized.";

                        _syncError = "";
                    }
                    catch (Exception e)
                    {
                        _syncError = "Error: " + e;
                    }

                    manualResetEventSlim.Reset();
                }

                return HttpStatusCode.OK;
            };

            Post["/syncusernames"] = parameters =>
            {
            
                if (!manualResetEventSlim.IsSet)
                {
                    manualResetEventSlim.Set();

                    _syncError = null;
                    _syncSuccess = null;

                    try
                    {
                        MatterSphereDB msdb = new MatterSphereDB();

                        if (msdb.IsAAD)
                            _syncSuccess = msdb.ConvertUsersToUPN();
                        else
                            _syncSuccess = msdb.UpdateUserNames();

                        _syncError = "";
                    }
                    catch (Exception e)
                    {
                        _syncError = "Error: " + e.Message;
#if DEBUG
                        _syncError = "Error: " + e.ToString().Replace(Environment.NewLine, "<br>");
#endif
                    }

                    manualResetEventSlim.Reset();
                }

                return HttpStatusCode.OK;
            };
        }
    }
}