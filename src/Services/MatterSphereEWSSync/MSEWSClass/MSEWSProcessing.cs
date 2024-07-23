using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MSEWSClass
{
    public class MSEWSProcessing
    {
        #region Base Methods
        private MSEWSClass.MSEWS msewsClass;
        private MSEWSClass.MSEWSDatabase msewsDB;
        private MSEWSClass.MSEWSLogging msewsLog;

        private void GenerateClassReferences()
        {
            if (msewsClass == null) msewsClass = new MSEWS();
            if (msewsDB == null) msewsDB = new MSEWSDatabase();
            if (msewsLog == null) msewsLog = new MSEWSLogging("MSEWSLogFile");

        }
        private void DisposeOfClassReferences()
        {
            msewsClass.Dispose();
            if (msewsClass != null) msewsClass = null;

            if (msewsDB != null) msewsDB = null;
            if (msewsLog != null) msewsLog = null;
        }
        private void MarkItemWithErrorMessage(MSEWSItem item, StringBuilder sb)
        {
            try
            {
                if (MSEWSConfiguration.GetConfigurationItemBool("MarkErroringItemsasErrored"))
                {
                    item.iItemStatus = 5;
                    item.iLastProcessed = DateTime.UtcNow;
                    item.iErrorMessage = sb.ToString();
                    msewsDB.UpdateItemRow(item);
                }
            }
            catch (Exception ex2)
            {
                StringBuilder sb2 = new StringBuilder();
                sb2.AppendLine("Error Occurred Marking Item Errored : " + item.ItemID);
                sb2.AppendLine(ex2.Message);
                sb2.AppendLine("Inner Exception");
                if (ex2.InnerException != null) sb.AppendLine(ex2.InnerException.Message);
                msewsLog.CreateErrorEntry(sb2.ToString());
            }
        }
        #endregion

        public void RunProcess()
        {
            try
            {
                GenerateClassReferences();
                msewsLog.CreateLogEntry("MSEWS - Process Started");
                ProcessItemsToBeDeleted();
                ProcessItemsToUpdate();
                ProcessNewItems();
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Run Process Error");
                sb.AppendLine(ex.Message);
                sb.AppendLine("Inner Exception");
                if (ex.InnerException != null) sb.AppendLine(ex.InnerException.Message);
                msewsLog.CreateErrorEntry(sb.ToString());
            }
            finally
            {
                msewsLog.CreateLogEntry("MSEWS - Process Completed");
                DisposeOfClassReferences();
            }

        }

        #region UpdateItem
        private void ProcessItemsToUpdate()
        {
            List<MSEWSItem> itemList = GetItemsToUpdate();
            if (itemList == null) { msewsLog.CreateLogEntry("No Items to Update"); }
            else { UpdateItems(itemList); }
            itemList = null;
        }

        private List<MSEWSItem> GetItemsToUpdate()
        {
            try
            {
                DataTable itemTable = msewsDB.GetItemsToUpdate();
                if (itemTable == null)
                {
                    return null;
                }
                List<MSEWSItem> items = new List<MSEWSItem>();
                foreach (DataRow row in itemTable.Rows)
                {
                    items.Add(new MSEWSItem(row));
                }
                itemTable = null;
                return items;
            }
            catch (Exception ex)
            {
                msewsLog.CreateErrorEntry(ex.Message);
                return null;
            }
        }

        private void UpdateItems(List<MSEWSItem> itemsToUpdate)
        {
            foreach (MSEWSItem item in itemsToUpdate)
            {
                try
                {
                    msewsLog.CreateLogEntry("Start of Process to Update Item for ItemID: " + item.ItemID);
                    UpdateItem(item);
                    msewsLog.CreateLogEntry("End of Process to Update Item for ItemID: " + item.ItemID);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error Occurred Updating Item : " + item.ItemID);
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("Inner Exception");
                    if (ex.InnerException != null) sb.AppendLine(ex.InnerException.Message);
                    msewsLog.CreateErrorEntry(sb.ToString());
                    MarkItemWithErrorMessage(item, sb);
                }
            }
        }

        private void UpdateItem(MSEWSItem item)
        {
            switch (item.iType)
            {
                case "A":
                    msewsClass.UpdateAppointement(item);
                    break;
                case "T":
                    msewsClass.UpdateTask(item);
                    break;
                case "M":
                    msewsClass.UpdateMeeting(item);
                    break;
            }
            if (item.MSEWSItemRowUpdated)
            {
                msewsDB.UpdateItemRow(item);
            }
        }
        #endregion

        #region DeleteItems
        private void ProcessItemsToBeDeleted()
        {
            List<MSEWSItem> itemList = GetItemsToDelete();
            if (itemList == null) { msewsLog.CreateLogEntry("No Items to Delete"); }
            else { DeleteItems(itemList); }
            itemList = null;
        }


        private void DeleteItems(List<MSEWSItem> itemsToDelete)
        {
            foreach (MSEWSItem item in itemsToDelete)
            {
                try
                {
                    msewsLog.CreateLogEntry("Start of Process to Delete Item for ItemID: " + item.ItemID);
                    DeleteItem(item);
                    msewsLog.CreateLogEntry("End of Process to Delete Item for ItemID: " + item.ItemID);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error Occurred Deleting Item : " + item.ItemID);
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("Inner Exception");
                    if (ex.InnerException != null) sb.AppendLine(ex.InnerException.Message);
                    msewsLog.CreateErrorEntry(sb.ToString());
                    MarkItemWithErrorMessage(item, sb);
                }
            }
        }

        private void DeleteItem(MSEWSItem item)
        {
            msewsClass.DeleteItem(item);
            if (item.MSEWSItemRowUpdated)
            {
                msewsDB.UpdateItemRow(item);
            }
        }

        private List<MSEWSItem> GetItemsToDelete()
        {
            try
            {
                DataTable itemTable = msewsDB.GetItemsToDelete();
                if (itemTable == null)
                {
                    return null;
                }
                List<MSEWSItem> items = new List<MSEWSItem>();
                foreach (DataRow row in itemTable.Rows)
                {
                    items.Add(new MSEWSItem(row));
                }
                itemTable = null;
                return items;
            }
            catch (Exception ex)
            {
                msewsLog.CreateErrorEntry(ex.Message);
                return null;
            }
        }
        #endregion

        #region NewItems
        private void ProcessNewItems()
        {
            List<MSEWSItem> itemList = GetItemsToAdd();
            if (itemList == null) { msewsLog.CreateLogEntry("No Items to Add"); }
            else { AddNewItems(itemList); }
            itemList = null;
        }

        private void AddNewItems(List<MSEWSItem> ItemsToCreate)
        {
            foreach (MSEWSItem item in ItemsToCreate)
            {
                try
                {
                    msewsLog.CreateLogEntry("Start of Process to create new Item for ItemID: " + item.ItemID);
                    CreateNewItem(item);
                    msewsLog.CreateLogEntry("End of Process to create new Item for ItemID: " + item.ItemID);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error Occurred Creating New Item : " + item.ItemID);
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("Inner Exception");
                    if (ex.InnerException != null) sb.AppendLine(ex.InnerException.Message);
                    msewsLog.CreateErrorEntry(sb.ToString());
                    MarkItemWithErrorMessage(item, sb);
                }
            }
            
        }

        private void CreateNewItem(MSEWSItem item)
        {
            switch (item.iType)
            {
                case "A":
                    msewsClass.CreateNewAppointement(item);
                    break;
                case "T":
                    msewsClass.CreateNewTask(item);
                    break;
                case "M":
                    msewsClass.CreateNewMeeting(item);
                    break;
            }
            if (item.MSEWSItemRowUpdated)
            {
                msewsDB.UpdateItemRow(item);
            }
        }

        private List<MSEWSItem> GetItemsToAdd()
        {
            try
            {
                DataTable itemTable = msewsDB.GetItemsToAdd();
                if (itemTable == null)
                {
                    return null;
                }
                List<MSEWSItem> items = new List<MSEWSItem>();
                foreach (DataRow row in itemTable.Rows)
                {
                    items.Add(new MSEWSItem(row));
                }
                itemTable = null;
                return items;
            }
            catch (Exception ex)
            {
                msewsLog.CreateErrorEntry(ex.Message);
                return null;
            }
        }
        #endregion


    }
}
