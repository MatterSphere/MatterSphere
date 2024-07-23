using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DocumentArchivingClass
{
    public class DocumentArchiving
    {
        #region "Global Variables"

        private MSDatabase MSDB;
        private DocumentArchivingLogging Log;
                
        private  List <DocumentVersionRecord> lstDocversions =null;

        private bool stopArchiveProcessOnError = true;
        private bool stopDeleteProcessOnError = true;
        private bool stopProcessNow = false;


        #region "Run History Variables"
        private DateTime runEndTime;
        private int runTotalDocs = 0;
        private int runProcessedDocs = 0;
        private int runProcessedFiles = 0;
        private bool runCompleted = false;
        private StringBuilder runMessage = new StringBuilder();
        private string runProcessedArchiveInfoIDList = "";
        private Guid runGuid;
        private Int64 runID = 0;
        #endregion

        #endregion

        #region Base Methods
        private void GenerateClassReferences()
        {
            if (Log == null) Log = new DocumentArchivingLogging("DocumentArchiveLogFile");
            if (MSDB == null) MSDB = new MSDatabase();
        }

        private void DisposeOfClassReferences()
        {
            if (MSDB != null)
            {
                MSDB.Dispose();
                MSDB = null;
            }
            if (Log != null) Log = null;
        }

        private void MarkItemWithErrorMessage(DocumentRecord dr, StringBuilder sb)
        {
            try
            {
                if (DocumentArchivingConfiguration.GetConfigurationItemBool("MarkErroringItemsAsErrored"))
                {
                    dr.ArchiveStatus = 3;
                    dr.ArchiveMessage = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry("Error Occurred Marking Item Errored : ", ex);
            }
        }
        #endregion

        #region "Process methods"

        public bool IsCancellationRequested { get; set; }

        public void RunProcess()
        {
            stopProcessNow = false;

            try
            {
                GenerateClassReferences();
                Log.CreateLogEntry("Document Archive - Process Started");                
                ProcessDocuments();
            }
            catch (Exception ex)
            {
                if (Log != null)
                    Log.CreateErrorEntry("Run Process Error", ex);
            }
            finally
            {
                if (runID != 0)
                {
                    //End of archiving process - update the run table
                    runCompleted = (runProcessedDocs == runTotalDocs);
                    if (runCompleted)
                        runMessage.Clear().Append("Success");

                    runEndTime = DateTime.UtcNow;
                    MSDB.UpdateArchiveRunInDB(runGuid, runEndTime, runTotalDocs, runProcessedDocs, runProcessedFiles, runCompleted, runMessage.ToString().Trim('\r', '\n'), runProcessedArchiveInfoIDList);
                }
                Log.CreateLogEntry("Document Archive - Process Completed");
                DisposeOfClassReferences();
            }
        }

        private void ProcessDocuments()
        {
            List<DocumentRecord> lstDocuments = GetDocuments();       
           
            if (lstDocuments.Count == 0) 
            {
                Log.CreateLogEntry("No Documents to Process");
            }
            else
            {
                runGuid = Guid.NewGuid();
                runID = MSDB.CreateNewArchiveRunInDB(runGuid);
                if (runID != 0)
                {
                    Log.CreateLogEntry("No of Items to Process = " + lstDocuments.Count.ToString());
                    ProcessDocumentList(lstDocuments);
                    runTotalDocs = lstDocuments.Count;
                }
            }
        }

        private void ProcessDocumentList(List<DocumentRecord> lstDocuments)
        {
            foreach (DocumentRecord docRecord in lstDocuments)
            {
                if (IsCancellationRequested)
                {
                    break;
                }
                try
                {
                    Log.CreateLogEntry(string.Format("Start of Processing for ArchID:{0}. [DocumentID:{1} and RunID:{2}] ", docRecord.ArchiveID, docRecord.DocumentID, runID));

                    if (docRecord.SourceDirectory == docRecord.DestinationDirectory && docRecord.ArchiveAction == "A") 
                    {
                        docRecord.ArchiveMessage = "Source and Destination folders are the same on DocumentID: "+ docRecord.DocumentID;
                        docRecord.ArchiveStatus = 2;
                        docRecord.RunID = runID;
                        MSDB.UpdateItemRow(docRecord);                 
                    }
                    else
                    {
                        ProcessDocumentRecord(docRecord);
                    }

                    Log.CreateLogEntry(string.Format("End of Processing for ArchID:{0}. [DocumentID:{1} and RunID:{2}] ", docRecord.ArchiveID, docRecord.DocumentID, runID));
                  
                    //Stop process here if we are stopping on error
                    if (docRecord.ArchiveAction == "A" && stopArchiveProcessOnError && stopProcessNow)
                        return;
                    if (docRecord.ArchiveAction == "D" && stopDeleteProcessOnError && stopProcessNow)
                        return;
                }
                catch (Exception ex)
                {
                    Log.CreateErrorEntry("Error Occurred Creating New Item : " + docRecord.ArchiveID.ToString(), ex);
                }
            }
        }
        
        private void ProcessDocumentRecord(DocumentRecord docRecord)
        {
            if (docRecord.ArchiveAction == "A")
                ArchiveDocument(docRecord);
            else if (docRecord.ArchiveAction == "D")
                DeleteDocument(docRecord);
        }
        #endregion

        #region "Get Document methods"
        private List<DocumentRecord> GetDocuments()
        {
            List<DocumentRecord> items = new List<DocumentRecord>();
            try
            {
                DataTable itemTable = MSDB.GetDocumentList();
                if (itemTable != null)
                {
                    foreach (DataRow row in itemTable.Rows)
                    {
                        items.Add(new DocumentRecord(row));
                    }
                    itemTable = null;
                }
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry(ex.Message);
            }
            return items;
        }
        
        private List<DocumentVersionRecord> GetDocumentVersions(Int64 docID)
        {
            List<DocumentVersionRecord> items = new List<DocumentVersionRecord>();
            try
            {
                DataTable itemTable = MSDB.GetDocumentVersionList(docID);
                if (itemTable != null)
                {
                    foreach (DataRow row in itemTable.Rows)
                    {
                        items.Add(new DocumentVersionRecord(row));
                    }
                    itemTable = null;
                }
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry(ex.Message);
            }
            return items;
        }
        #endregion

        #region "Action : Delete"
        private void DeleteDocument(DocumentRecord docRecord)
        {
            lstDocversions = GetDocumentVersions(docRecord.DocumentID);
            bool deletefailed = false;
            int docVersionsDeleted = 0;

            //Delete all versions of the document
            foreach (DocumentVersionRecord docversionRecord in lstDocversions)
            {
                if (RemoveThisFile(Path.Combine(docRecord.SourceDirectory, docversionRecord.DocumentVerToken), docRecord, docversionRecord))
                {
                    docversionRecord.VersionDeletedSucessfully = true;
                    docVersionsDeleted++;
                    runProcessedFiles++;
                }
                else
                {
                    docversionRecord.VersionDeletedSucessfully = false;
                    deletefailed = true;
                    docRecord.ArchiveStatus = 3;
                    
                    if (stopDeleteProcessOnError)
                    {
                        stopProcessNow = true;
                        break;
                    }
                }
            }

            if (!deletefailed)
            {
                //Complete success in deleting the document
                MSDB.DeleteDocumentDataInDatabase(docRecord.DocumentID, "All versions deleted successfully", -1);
                docRecord.RunID = runID;
                docRecord.ArchiveStatus = 2;
                docRecord.ArchiveMessage = "File deleted successfully";
                runProcessedDocs++;
                runProcessedArchiveInfoIDList += docRecord.ArchiveID + ",";
            }
            else if (docVersionsDeleted >= 1)
            {
                MSDB.DeleteDocumentDataInDatabase(docRecord.DocumentID, string.Format("{0}/{1} versions deleted successfully", docVersionsDeleted, lstDocversions.Count), -1);
                runMessage.Insert(0, string.Format("\nDocID:{0}. {1}/{2} versions deleted successfully\n", docRecord.DocumentID, docVersionsDeleted, lstDocversions.Count));
            }

            if (docRecord.ArchiveStatus == 2)
                docRecord.RunID = runID;

            MSDB.UpdateItemRow(docRecord);
        }
        #endregion

        #region "Action : Archive"
        private void ArchiveDocument(DocumentRecord docRecord)
        {
            lstDocversions = GetDocumentVersions(docRecord.DocumentID);
            bool copyfailed = false;
            bool removefailed = false;
            docRecord.Archived = false;
            int docVersionsArchived = 0;

            //Copy each version to the archive location
            foreach (DocumentVersionRecord docversionRecord in lstDocversions)
            {
                if (CopyThisFile(Path.Combine(docRecord.SourceDirectory, docversionRecord.DocumentVerToken), Path.Combine(docRecord.DestinationDirectory, docversionRecord.DocumentVerToken), docRecord, docversionRecord))
                {
                    docversionRecord.VersionCopiedSucessfully = true;
                    runProcessedFiles++;
                }
                else
                {
                    docversionRecord.VersionCopiedSucessfully = false;
                    copyfailed = true;
                    docRecord.ArchiveStatus = 3;
                    break;
                }
            }

            //If there was an error copying a version, roll back and delete from archive location
            if (copyfailed)
            {
                foreach (DocumentVersionRecord docversionRecord in lstDocversions)
                {
                    if (docversionRecord.VersionCopiedSucessfully)
                    {
                        //*** Assumption made that we can remove the files from the location that we have just copied to
                        if (!RemoveThisFile(Path.Combine(docRecord.DestinationDirectory, docversionRecord.DocumentVerToken), docRecord, docversionRecord))
                        {
                            //Should not really come into here
                            if (stopDeleteProcessOnError)
                            {
                                docRecord.ArchiveStatus = 3;
                                stopProcessNow = true;
                                MSDB.UpdateItemRow(docRecord);
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                //Success in copying to archive location - update DB
                bool updateddocrow = MSDB.UpdateDocumentDirectoryID(docRecord.DocumentID, docRecord.ArchiveDirectoryID);
                docRecord.ArchiveMessage = "Files moved successfully\n";
                runProcessedDocs++;
                runProcessedArchiveInfoIDList += docRecord.ArchiveID + ",";

                //Remove files from source destination as all versions copied to archive location
                foreach (DocumentVersionRecord docversionRecord in lstDocversions)
                {
                    if (RemoveThisFile(Path.Combine(docRecord.SourceDirectory, docversionRecord.DocumentVerToken), docRecord, docversionRecord))
                    {
                        docversionRecord.VersionDeletedSucessfully = true;
                    }
                    else
                    {
                        docversionRecord.VersionDeletedSucessfully = false;
                        removefailed = true;
                        docRecord.ArchiveStatus = 3;
                        break;
                    }
                }

                //If we failed removing all version files from the source, update accordingly
                if (removefailed)
                {
                    MSDB.ArchiveDocumentDataInDatabase(docRecord.DocumentID, string.Format("{0}/{1} versions Archived successfully", docVersionsArchived, lstDocversions.Count), -1);
                }
                else
                {
                    //Complete success
                    docRecord.Archived = true;
                    docRecord.ArchiveStatus = 2;
                    docVersionsArchived++;
                 
                    MSDB.ArchiveDocumentDataInDatabase(docRecord.DocumentID, string.Format("All versions archived successfully. [Directory ID {0} to {1}]", docRecord.DocumentSourceDirectoryID, docRecord.ArchiveDirectoryID), -1);

                    docRecord.ArchiveMessage = "Document archived successfully";
                }
            }

            if (docRecord.ArchiveStatus == 2)
                docRecord.RunID = runID;

            MSDB.UpdateItemRow(docRecord);

            //Flag to stop the archive process (if setting on)
            if (stopArchiveProcessOnError)
                stopProcessNow = (copyfailed || removefailed);
        }
        #endregion

        #region "File system methods"

        #region "Copy"
        private bool CopyThisFile(string source, string destination, DocumentRecord docRecord, DocumentVersionRecord docVersionRecord) //rename?
        {
            bool result = false;
            try
            {
                FileInfo srcFileInfo = new FileInfo(source);
                if (!srcFileInfo.Exists)
                {
                    string _msg = string.Format("Cannot find file {0}.{1} [{2}]\n", docVersionRecord.DocumentID, docVersionRecord.DocumentVerLabel, source);
                    docRecord.ArchiveMessage += _msg;
                    runMessage.AppendFormat("Error copying file: {0}\n", docRecord.ArchiveMessage);
                    Log.CreateErrorEntry(_msg);
                }
                else
                {
                    FileInfo dstFileInfo = new FileInfo(destination);
                    dstFileInfo.Directory.Create();
                    srcFileInfo.CopyTo(destination);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Now do the exception
                Log.CreateErrorEntry("Error Occurred Copying Document : " + docRecord.DocumentID.ToString(), ex);

                docRecord.ArchiveMessage += string.Format("Error copying file {0}.{1} [{2}] to [{3}]\n", docVersionRecord.DocumentID, docVersionRecord.DocumentVerLabel, source, destination);
                runMessage.AppendFormat("Error: {0}\n{1}\n", docRecord.ArchiveMessage, ex.Message);
                if (ex.InnerException != null) runMessage.AppendLine(ex.InnerException.Message);
            }
            return result;
        }
        #endregion

        #region "Delete"
        private bool RemoveThisFile(string filepath, DocumentRecord docRecord, DocumentVersionRecord docVersionRecord) // new method 
        {
            bool result = false;
            string action = (docRecord.ArchiveAction == "A" ? "archiving" : "deletion");
            try
            {
                FileInfo fileInfo = new FileInfo(filepath);
                if (!fileInfo.Exists)
                {
                    docRecord.ArchiveMessage += string.Format("Cannot find file {0}.{1} [{2}]\n", docVersionRecord.DocumentID, docVersionRecord.DocumentVerLabel, filepath);
                    if (docRecord.ArchiveAction == "A")
                        runMessage.AppendFormat("Error during {0}: {1}\n", action, docRecord.ArchiveMessage);
                    else
                        result = true; // Do not fail on non-existing document in Deletion mode.
                }
                else
                {
                    fileInfo.Delete();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Now do the exception
                Log.CreateErrorEntry("Error Occurred removing Document: " + docRecord.DocumentID.ToString(), ex);

                docRecord.ArchiveMessage += string.Format("Error deleting file {0}.{1} [{2}]\n", docVersionRecord.DocumentID, docVersionRecord.DocumentVerLabel, filepath);
                runMessage.AppendFormat("Error during {0}: {1}\n{2}\n", action, docRecord.ArchiveMessage, ex.Message);
                if (ex.InnerException != null) runMessage.AppendLine(ex.InnerException.Message);
            }
            return result;
        }
        #endregion

        #endregion
      
    }
}
