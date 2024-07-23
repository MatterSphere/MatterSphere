using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.DocumentManagement.Storage;



namespace FWBS.OMS.UI.Windows
{
    public class PrecedentVersionRestorer
    {
        private bool showmessage;
        private IConnection connection;
        private IStorageItemVersion flagtoversion;
        IStorageItemVersion flagfromversion;
        VersionDataRestorationProcessor processor;

        long? currentproductionscriptversion;
        long flagToPrecVersionScriptVersion;
        FWBS.OMS.DocumentManagement.PrecedentVersion flagToPrecVersion;

        public bool RestoreScript { get; private set; }
        
        public PrecedentVersionRestorer(IStorageItemVersion FlagToVersion, IStorageItemVersion FlagFromVersion, bool ShowMessage)
        {
            flagtoversion = FlagToVersion;
            showmessage = ShowMessage;
            flagfromversion = FlagFromVersion;
        }

        public void CheckForScriptRestoration()
        {
            if (Session.CurrentSession.CurrentUser.IsInRoles("PRECDEVELOPER"))
            {
                flagToPrecVersion = (FWBS.OMS.DocumentManagement.PrecedentVersion)flagtoversion;

                flagToPrecVersion.ParentDocument.Script.Refresh();
                if (flagToPrecVersion.ParentDocument.HasScript)
                    currentproductionscriptversion = flagToPrecVersion.ParentDocument.Script.Version;

                flagToPrecVersionScriptVersion = GetScriptVersionNumber(flagToPrecVersion.ScriptVersionID);

                if (!currentproductionscriptversion.HasValue && flagToPrecVersionScriptVersion == 0)
                {
                    return;
                }
                else if (currentproductionscriptversion.HasValue && flagToPrecVersionScriptVersion != 0)
                {
                    AskUserToRestoreScript();
                }
                else if (!currentproductionscriptversion.HasValue && flagToPrecVersionScriptVersion != 0)
                {
                    AskUserToRestoreScript();
                }
                else if (currentproductionscriptversion.HasValue && flagToPrecVersionScriptVersion == 0)
                {
                    return;
                }
            }
        }

        private void AskUserToRestoreScript()
        {
            if ((flagToPrecVersionScriptVersion != currentproductionscriptversion) && showmessage)
            {
                DialogResult r = System.Windows.Forms.MessageBox.Show(ResourceLookup.GetLookupText("PRECSCRRESTORE1"), ResourceLookup.GetLookupText("PRECSCRRESTORE2"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                    RestoreScript = true;
                else
                    RestoreScript = false;
            }
        }

        //**********************************************
        // ******   Precedent Script Restoration  ******
        //**********************************************

        public void RestorePrecedentScriptVersion()
        {
            if (RestoreScript)
            {
                DataTable dtCurrentVersions = CreateCurrentVersionTable();
                DataTable dtRestoreVersions = CreateRestoreTable();

                PopulateScriptCurrentVersionTable(flagToPrecVersion, dtCurrentVersions);
                PopulateScriptRestoreVersionTable(flagToPrecVersion, flagToPrecVersionScriptVersion, dtRestoreVersions);

                processor = new VersionDataRestorationProcessor(dtCurrentVersions, dtRestoreVersions);
                processor.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(processor_RestorationCompleted);
                processor.ProcessVersionData();
            }
        }

        private static void PopulateScriptCurrentVersionTable(FWBS.OMS.DocumentManagement.PrecedentVersion precVersion, DataTable dtCurrentVersion)
        {
            dtCurrentVersion.Rows.Add(
                Convert.ToString(precVersion.ParentDocument.Script.Code),
                Convert.ToString(precVersion.ParentDocument.Script.Version),
                "Script"
            );
        }

        private static void PopulateScriptRestoreVersionTable(FWBS.OMS.DocumentManagement.PrecedentVersion precVersion, long precVersionScriptVersion, DataTable dtRestoreVersion)
        {
            dtRestoreVersion.Rows.Add(
                Convert.ToString("Script"),
                Convert.ToString(precVersion.ParentDocument.Script.Code),
                Convert.ToString(Convert.ToString(precVersionScriptVersion)),
                "dbScriptVersionData"
            );
        }

        private long GetScriptVersionNumber(long? VersionID)
        {
            if (VersionID.HasValue)
            {
                List<IDataParameter> parList = new List<IDataParameter>();
                connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                parList.Add(connection.CreateParameter("VersionID", VersionID));
                System.Data.DataTable dt = connection.ExecuteSQL("select Version from dbScriptVersionData where VersionID = @VersionID", parList);
                connection.Disconnect();
                return ConvertDef.ToInt64(dt.Rows[0]["Version"], 0);
            }
            else
                return 0;
        }

        private DataTable CreateCurrentVersionTable()
        {
            DataTable currentversions = new DataTable();
            currentversions.Columns.Add("Code", typeof(string));
            currentversions.Columns.Add("CurrentVersionNumber", typeof(string));
            currentversions.Columns.Add("Type", typeof(string));
            return currentversions;
        }

        private DataTable CreateRestoreTable()
        {
            DataTable restoreversion = new DataTable();
            restoreversion.Columns.Add("ObjectType", typeof(string));
            restoreversion.Columns.Add("Code", typeof(string));
            restoreversion.Columns.Add("VersionNumber", typeof(string));
            restoreversion.Columns.Add("TableName", typeof(string));
            return restoreversion;
        }

        private void processor_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            processor.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(processor_RestorationCompleted);
        }

        //**********************************************
        // ****** Precedent Audit Record Creation ******
        //**********************************************

        public void CreatePrecedentRestorationAuditRecord()
        {
            DataTable dtCurrentVersion = CreateCurrentVersionTable();
            DataTable dtRestoreVersion = CreateRestoreTable();

            PopulatePrecedentCurrentVersionTable(flagfromversion, dtCurrentVersion);
            PopulatePrecedentRestoreVersionTable(flagtoversion, dtRestoreVersion);

            RestorationAuditCreator audit = new RestorationAuditCreator(dtRestoreVersion);
            audit.CreateRestorationAuditRecords(dtCurrentVersion);
        }

        private static void PopulatePrecedentCurrentVersionTable(IStorageItemVersion originalver, DataTable dtCurrentVersion)
        {
            dtCurrentVersion.Rows.Add(
                ((PrecedentVersion)originalver).ParentDocument.Title + " (" + ((PrecedentVersion)originalver).ParentDocument.ID + ")",
                Convert.ToString(originalver.Label),
                "Precedent"
            );
        }

        private static void PopulatePrecedentRestoreVersionTable(IStorageItemVersion ver, DataTable dtRestoreVersion)
        {
            dtRestoreVersion.Rows.Add(
                Convert.ToString("Precedent"),
                ((PrecedentVersion)ver).ParentDocument.Title + " (" + ((PrecedentVersion)ver).ParentDocument.ID + ")",
                Convert.ToString(ver.Label),
                "dbPrecedentVersion"
            );
        }
    }
}
