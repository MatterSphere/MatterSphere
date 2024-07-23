using System;
using System.Collections.Generic;
using System.Data;

namespace MatterSphereEWS
{
    public class MatterSphereDelete
    {
        #region Base Methods
        private EWS ews;
        private Database DB;
        private Logging Log;
        private MatterSphereSettings msSettings;

        private void GenerateClassReferences()
        {
            if (Log == null) Log = new Logging("MSEWSDeleteLogFile");
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
        #region DeleteService

        public bool IsCancellationRequested { get; set; }

        public void RunProcess()
        {
            try
            {
                GenerateClassReferences();
                Log.CreateLogEntry("MSEWS - Delete Process Started");
                ProcessActiveAppointments();
                Log.CreateLogEntry("MSEWS - Delete Process Ended");
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry("Run Process Error", ex);
            }
            finally
            {
                Log.CreateLogEntry("MSEWS - Delete Process Completed");
                DisposeOfClassReferences();
            }

        }

        private void ProcessActiveAppointments()
        {
            List<AppointmentItem> newAppItems = GetAllAppointments();
            if (newAppItems == null || newAppItems.Count == 0)
            {
                Log.CreateLogEntry("No Items to Process");
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
                        if (appItem.iEWSID == null && appItem.iEntryID == null)
                            continue;

                        UserSettings usrSettings = GetUserSettings(appItem);
                        ews.CheckItemExists(appItem, usrSettings, msSettings);
                        DB.UpdateItemRow(appItem);
                    }
                    catch (Exception ex)
                    {
                        Log.CreateErrorEntry("Appointment Checking Failure. ID = " + appItem.ItemID.ToString(), ex);
                    }
                }
            }
        }

        private List<AppointmentItem> GetAllAppointments()
        {
            try
            {
                DataTable itemTable = DB.GetAllAppointments();
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
        private string GetMailBoxName(AppointmentItem appItem)
        {
            if (!String.IsNullOrEmpty(appItem.userEmail))
            {
                return appItem.userEmail;
            }
            return DB.GetFeeEarnerEmailAddress(Convert.ToInt64(appItem.iFeeEarnerID));
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
        #endregion DeleteService
    }
}
