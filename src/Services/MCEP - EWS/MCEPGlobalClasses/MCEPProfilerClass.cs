using System;
using System.Collections.Generic;
using System.Data;

namespace MCEPGlobalClasses
{
    public class MCEPProfilerClass
    {
        private MCEPGlobalClasses.MCEPEWS mcepEWS;
        private MCEPGlobalClasses.MCEPDatabase mcepDB;
        private MCEPGlobalClasses.MCEPLogging mcepLog;

        private void GenerateClassReferences()
        {
            if (mcepLog == null) mcepLog = new MCEPLogging("MCEPProfilerEWS");
            if (mcepEWS == null) mcepEWS = new MCEPEWS();
            if (mcepDB == null) mcepDB = new MCEPDatabase();
        }

        private void DisposeOfClassReferences()
        {
            if (mcepEWS != null)
            {
                mcepEWS.Dispose();
                mcepEWS = null;
            }
            if (mcepDB != null)
            {
                mcepDB.Dispose();
                mcepDB = null;
            }
            if (mcepLog != null)
            {
                mcepLog = null;
            }
        }

        public bool IsCancellationRequested { get; set; }

        public void RunProcess()
        {
            try
            {
                GenerateClassReferences();
                //Get List of Users to sync
                mcepLog.CreateLogEntry("MCEP Profiler - Process Started");
                mcepLog.CreateLogEntry("Getting List of Users");
                List<MCEPUser> usersToSync = GetUsersToSync();
                if (usersToSync == null || usersToSync.Count == 0)
                {
                    mcepLog.CreateLogEntry("No Users Returned to Process");
                    return;
                }
                mcepLog.CreateLogEntry("No of Users Returned : " + Convert.ToString(usersToSync.Count));
                mcepLog.CreateLogEntry("Processing of User Begins");
                ProcessUserList(usersToSync);
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry("Run Process Error", ex);
            }
            finally
            {
                mcepLog.CreateLogEntry("MCEP Profiler - Process Ended");
                DisposeOfClassReferences();
            }
        }

        private void ProcessUserList(List<MCEPUser> usersToSync)
        {
            foreach (MCEPUser user in usersToSync)
            {
                if (IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    mcepLog.CreateLogEntry("Start of Process for Email Address : " + user.EmailAddress);
                    mcepEWS.Impersonate(user.EmailAddress);
                    if (CheckRootFolderIDSet(user))
                    {
                        ProcessRootFolder(user);
                    }
                    else
                    {
                        mcepLog.CreateLogEntry("No Root Folder Folder found for Email : " + user.EmailAddress);
                    }
                }
                catch (Exception ex)
                {
                    mcepLog.CreateErrorEntry("Error Occurred Processing Email : " + user.EmailAddress, ex);
                }
            }
        }

        private void ProcessRootFolder(MCEPUser user)
        {
            mcepEWS.FindMatterSphereMessages(user.EmailAddress,user.RootFolderID,user.UserID,mcepLog);
            user.UserUpdated = DateTime.UtcNow;
            user.UserLastRan = DateTime.UtcNow;
            mcepDB.UpdateUserRow(user);
        }

        private bool CheckRootFolderIDSet(MCEPUser user)
        {
            try
            {
                // Check for RootFolderID and set if not set.
                if (string.IsNullOrEmpty(user.RootFolderID))
                {
                    string FolderID = mcepEWS.GetRootFolderID(user.EmailAddress, user.RootFolderName);
                    if (FolderID == null)
                    {
                        return false;
                    }
                    else
                    {
                        user.RootFolderID = FolderID;
                        user.UserUpdated = DateTime.UtcNow;
                        mcepDB.UpdateUserRow(user);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry(ex.Message);
                return false;
            }
        }

        private List<MCEPUser> GetUsersToSync()
        {
            try
            {
                DataTable userTable = mcepDB.UserToSyncFromDatabase();
                if (userTable == null)
                {
                    return null;
                }
                List<MCEPUser> users = new List<MCEPUser>();
                foreach (DataRow row in userTable.Rows)
                {
                    users.Add(new MCEPUser(row));
                }
                userTable = null;
                return users;
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry(ex.Message);
                return null;
            }
        }
    }
}
