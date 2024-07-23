using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MatterSphereEWS
{
    public class MatterSphereEWSFE
    {
        #region Base Methods
        private EWS ews;
        private Database DB;
        private Logging Log;
        private MatterSphereSettings msSettings;
        private DateTime lastrun;

        private void GenerateClassReferences()
        {
            if (Log == null) Log = new Logging("MSEWSLogFile");
            if (ews == null) ews = new EWS(Log);
            if (DB == null) DB = new Database();
            if (msSettings == null) msSettings = DB.GetRegInfoSetting();
        }

        private void DisposeOfClassReferences()
        {
            if (msSettings != null) msSettings = null;
            if (ews != null)
            {
                ews.Dispose();
                ews = null;
            }
            if (DB != null)
            {
                DB.Dispose();
                DB = null;
            }
            if (Log != null) Log = null;
        }

        #endregion

        public bool IsCancellationRequested { get; set; }

        public void RunProcess()
        {
            try
            {
                GenerateClassReferences();
                Log.CreateLogEntry("MSEWS - Process Started");
                lastrun = GetLastRun();
                if (!IsCancellationRequested)
                {
                    Log.CreateLogEntry("MSEWS - Process Fee Earners");
                    ProcessFeeEarners();
                }
                if (!IsCancellationRequested)
                {
                    Log.CreateLogEntry("MSEWS - Process New Appointments");
                    ProcessNewAppointments();
                }
                if (!IsCancellationRequested)
                {
                    Log.CreateLogEntry("MSEWS - Process Deleted Appointments");
                    ProcessDeletedAppointments();
                }
                if (!IsCancellationRequested)
                {
                    UpdateLastRun();
                }
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry("Run Process Error", ex);
            }
            finally
            {
                Log.CreateLogEntry("MSEWS - Process Completed");
                DisposeOfClassReferences();
            }
        }

        private DateTime GetLastRun()
        {
            int extratime = Convert.ToInt16(Config.GetConfigurationItem("AdditionalMinutes"));
            return DB.GetLastRun().AddMinutes(-extratime);
        }
        //keep
        private void UpdateLastRun()
        {
            DB.UpdateLastRun();
        }
        //keep
        private void ProcessNewAppointments()
        {
            List<AppointmentItem> newAppItems = GetNewAppointments();
            if (newAppItems == null || newAppItems.Count == 0)
            {
                Log.CreateLogEntry("No New Items to Process");
            }
            else
            {
                foreach (AppointmentItem appItem in newAppItems)
                {
                    if (IsCancellationRequested)
                    {
                        break;
                    }
                    try
                    {
                        UserSettings usrSettings = GetUserSettings(appItem);
                        ews.ProcessNewAppointment(appItem, usrSettings, msSettings);
                        DB.UpdateItemRow(appItem);
                    }
                    catch (Exception ex)
                    {
                        Log.CreateErrorEntry("Appointment Creation Failure. ID = " + appItem.ItemID.ToString(), ex);
                    }
                }
            }
        }
        
        private List<AppointmentItem> GetNewAppointments()
        {
            try
            {
                DataTable itemTable = DB.GetNewAppointments();
                if (itemTable == null)
                {
                    return null;
                }
                List<AppointmentItem> items = new List<AppointmentItem>();
                foreach (DataRow row in itemTable.Rows)
                {
                    items.Add(new AppointmentItem(row));
                }
                itemTable = null;
                return items;
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry(ex.Message);
                return null;
            }
        }
        private void ProcessDeletedAppointments()
        {
            List<AppointmentItem> deletedAppItems = GetDeletedAppointments();
            if (deletedAppItems == null || deletedAppItems.Count == 0)
            {
                Log.CreateLogEntry("No Deleted Item to Process");
            }
            else
            {
                foreach (AppointmentItem appItem in deletedAppItems)
                {
                    if (IsCancellationRequested)
                    {
                        break;
                    }
                    try
                    {
                        UserSettings usrSettings = GetUserSettings(appItem);
                        ews.ProcessDeletedAppointment(appItem, usrSettings, msSettings);
                        DB.UpdateItemRow(appItem);
                    }
                    catch (Exception ex)
                    {
                        Log.CreateErrorEntry("Appointment Deletion Failure. ID = " + appItem.ItemID.ToString(), ex);
                    }
                }
            }
        }

        private List<AppointmentItem> GetDeletedAppointments()
        {
            try
            {
                DataTable itemTable = DB.GetDeletedAppointments();
                if (itemTable == null)
                {
                    return null;
                }
                List<AppointmentItem> items = new List<AppointmentItem>();
                foreach (DataRow row in itemTable.Rows)
                {
                    items.Add(new AppointmentItem(row));
                }
                itemTable = null;
                return items;
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry(ex.Message);
                return null;
            }
        }
        //keep
        private void ProcessFeeEarners()
        {
            DataTable feeEarners = DB.GetFeeEarnerList();
            foreach (DataRow row in feeEarners.Rows)
            {
                if (IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    Int32 feeUsrID = Convert.ToInt32(row["feeusrid"]);
                    Log.CreateLogEntry("Starting Sync for Fee Earner with ID: " + feeUsrID.ToString());
                    UserSettings feeSettings = GetFeeEarnerInfo(feeUsrID);
                    List<AppItemShort> ewsAppointments = ews.GetUpdatedAppointments(feeSettings.MailBoxAddress, lastrun);
                    if (ewsAppointments != null)
                    {
                        List<AppItemShort> msAppointments = DB.GetUpdatedAppointments(feeUsrID, lastrun);
                        IEnumerable<AppItemShort> inBoth = msAppointments.Where(p => ewsAppointments.Any(p2 => p2.AppointmentID == p.AppointmentID));
                        IEnumerable<AppItemShort> updatedinMSNotInEWS = msAppointments.Where(p => !ewsAppointments.Any(p2 => p2.AppointmentID == p.AppointmentID));
                        IEnumerable<AppItemShort> updatedinEWSNotInMS = ewsAppointments.Where(p => !msAppointments.Any(p2 => p2.AppointmentID == p.AppointmentID));
                        Int64 countBoth = inBoth.Count();
                        Int64 countMS = updatedinMSNotInEWS.Count();
                        Int64 countEWS = updatedinEWSNotInMS.Count();
                        ProcessItemsInBoth(inBoth, feeSettings);
                        ProcessItemsInMSNotEWS(updatedinMSNotInEWS, feeSettings);
                        ProcessItemsInEWSNotMS(updatedinEWSNotInMS, feeSettings);
                    }
                    Log.CreateLogEntry("Finished Sync for Fee Earner with ID: " + feeUsrID.ToString());
                }
                catch (Exception ex)
                {
                    Log.CreateErrorEntry("Process Fee Earner Error", ex);
                }

            }
        }

        private UserSettings GetFeeEarnerInfo(Int32 feeusrID)
        {
            Log.CreateLogEntry("Getting User Settings for UserID: " + feeusrID.ToString());
            UserSettings usrSettings = new UserSettings();

            usrSettings.MailBoxAddress = GetMailBoxNameFromFeeEarner(feeusrID);
            usrSettings.UserTimeZone = GetUserTimeZone(feeusrID);
            usrSettings.usrID = feeusrID;
            Log.CreateLogEntry("User Settings retrieved for UserID: " + feeusrID.ToString());
            Log.CreateLogEntry("User MailBoxAddress: " + usrSettings.MailBoxAddress);
            Log.CreateLogEntry("User TimeZone: " + usrSettings.UserTimeZone.StandardName);
            return usrSettings;
        }

        private string GetMailBoxNameFromFeeEarner(int feeusrID)
        {
            return DB.GetFeeEarnerEmailAddress(Convert.ToInt64(feeusrID));
        }

        private void ProcessItemsInEWSNotMS(IEnumerable<AppItemShort> updatedinEWSNotInMS, UserSettings feeSettings)
        {
            foreach(AppItemShort appItemShort in updatedinEWSNotInMS)
            {
                try
                {
                    AppointmentItem appItem = DB.GetAppointment(appItemShort.AppointmentID);
                    if (appItem.iFeeEarnerID == feeSettings.usrID) // Skip Item if moved to new fee earner to avoid time not changing.
                    {
                        ews.UpdateAppointmentEWStoMS(appItem, feeSettings, msSettings);
                        DB.UpdateItemRow(appItem);
                    }
                    
                }
                catch (Exception ex)
                {
                    Log.CreateErrorEntry("Process Items in EWS Error", ex);
                }
            }
        }

        private void ProcessItemsInMSNotEWS(IEnumerable<AppItemShort> updatedinMSNotInEWS, UserSettings feeSettings)
        {
            foreach (AppItemShort appItemShort in updatedinMSNotInEWS)
            {
                try
                {
                    AppointmentItem appItem = DB.GetAppointment(appItemShort.AppointmentID);
                    ews.UpdateAppointmentMStoEWS(appItem, feeSettings, msSettings);
                    DB.UpdateItemRow(appItem);
                }
                catch (Exception ex)
                {
                    Log.CreateErrorEntry("Process Items Updated in MatterSphere Error", ex);
                }
            }
        }

        private void ProcessItemsInBoth(IEnumerable<AppItemShort> inBoth, UserSettings feeSettings)
        {
            foreach (AppItemShort appItemShort in inBoth)
            {
                try
                {
                    AppointmentItem appItem = DB.GetAppointment(appItemShort.AppointmentID);
                    ews.UpdateAppointmentBoth(appItem, feeSettings, msSettings);
                    DB.UpdateItemRow(appItem);
                }
                catch (Exception ex)
                {
                    Log.CreateErrorEntry("Process Items in Both Error", ex);
                }
            }
        }
       
        private UserSettings GetUserSettings(AppointmentItem appItem)
        {
            Log.CreateLogEntry("Getting User Settings for UserID: " + appItem.iFeeEarnerID);
            UserSettings usrSettings = new UserSettings();
            usrSettings.MailBoxAddress = GetMailBoxName(appItem);
            usrSettings.UserTimeZone = GetUserTimeZone(appItem.iFeeEarnerID);
            Log.CreateLogEntry("User Settings retrieved for UserID: " + appItem.iFeeEarnerID);
            Log.CreateLogEntry("User MailBoxAddress: " + usrSettings.MailBoxAddress);
            Log.CreateLogEntry("User TimeZone: " + usrSettings.UserTimeZone.StandardName);
            return usrSettings;
        }

        private TimeZoneInfo GetUserTimeZone(int feeEarnerID)
        {
            TimeZoneInfo tzinfo = TimeZoneInfo.Utc;
            try
            { 
                string tzString = DB.GetUserTimeZone(feeEarnerID);
                tzinfo = TimeZoneInfo.FindSystemTimeZoneById(tzString);
            }
            catch
            {
                tzinfo = TimeZoneInfo.FindSystemTimeZoneById(Config.GetConfigurationItem("DefaultTimeZone"));
            }
            return tzinfo;
        }

        private string GetMailBoxName(AppointmentItem appItem)
        {
            if (!String.IsNullOrEmpty(appItem.userEmail))
            {
                return appItem.userEmail;
            }
            return DB.GetFeeEarnerEmailAddress(Convert.ToInt64(appItem.iFeeEarnerID));
        }
    }
}
