using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    public class VersionControlSupport
    {
        VersionComparisonSelector vcs;
        public event EventHandler<RestorationCompletedEventArgs> RestorationCompleted;

        public void OnRestorationCompleted(RestorationCompletedEventArgs e)
        {
            if (RestorationCompleted != null)
                RestorationCompleted(this, e);
        }

        public bool CheckObjectInIfNecessary(string sql, string code, long version)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", code));
            parList.Add(connection.CreateParameter("version", version));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            if (dt != null && dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        public void OpenComparisonTool(string code, LockableObjects obj)
        {
            vcs = new VersionComparisonSelector(code, obj);
            vcs.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            var result = vcs.ShowDialog();
            vcs.StartPosition = FormStartPosition.CenterScreen;
            vcs.FormClosing += new FormClosingEventHandler(vcs_FormClosing);
        }

        void vcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            vcs.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);    
        }

        private void vcs_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            OnRestorationCompleted(e);
        }

        public long IncrementVersionNumber(string code, long currentversion, string versiontable)
        {
            long highestversion = GetHighestCheckedInVersion(code, versiontable);
            if (highestversion != 0 && highestversion > currentversion)
                return highestversion + 1;
            else
                return currentversion + 1;
        }

        private  long GetHighestCheckedInVersion(string code, string versiontable)
        {
            string sql = "select MAX(Version) as Version from " + versiontable + " where Code = '" + code + "'";
            List<IDataParameter> parList = new List<IDataParameter>();
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return ConvertDef.ToInt64(dt.Rows[0]["Version"], 0);
        }

    }
}
