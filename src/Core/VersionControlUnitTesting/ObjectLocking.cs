using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FWBS.OMS.UI.Windows;
using System.Data;
using FWBS.OMS.Data;
using FWBS.OMS;

namespace VersionControlUnitTesting
{
    [TestClass]
    public class Objectlocking
    {
        [TestMethod]
        public void LockEnquiryForm()
        {
            Connect();
            LockState ls = new LockState();

            ls.LockEnquiryFormObject("scrFilMain");
            string strSQL = "select * from dbObjectLocking where ObjectCode = 'scrFilMain' and ObjectType = 'EnquiryForm'";
            DataTable dt = GetTestData(strSQL);
            Assert.AreEqual(1, Convert.ToInt32(dt.Rows.Count));
        }

        [TestMethod]
        public void UnLockEnquiryForm()
        {
            Connect();
            LockState ls = new LockState();

            ls.LockEnquiryFormObject("scrFilMain");
            string strSQL = "select * from dbObjectLocking where ObjectCode = 'scrFilMain' and ObjectType = 'EnquiryForm'";
            DataTable dt = GetTestData(strSQL);
            Assert.AreEqual(1, Convert.ToInt32(dt.Rows.Count));

            ls.UnlockEnquiryFormObject("scrFilMain");
            string strSQLDelete = "select * from dbObjectLocking where ObjectCode = 'scrFilMain' and ObjectType = 'EnquiryForm'";
            DataTable dtDelete = GetTestData(strSQLDelete);
            Assert.AreEqual(0, Convert.ToInt32(dtDelete.Rows.Count));
        }


        private DataTable GetTestData(string sql)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            System.Data.DataTable dt = connection.ExecuteSQL(sql, null);
            return dt;
        }


        private bool Connect()
        {
            FWBS.OMS.Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly();
            FWBS.OMS.Data.DatabaseConnections connections = new FWBS.OMS.Data.DatabaseConnections("MCEPEWS", "OMS", "2.0");
            FWBS.OMS.Data.DatabaseSettings settings = connections.CreateDatabaseSettings();

            settings.DatabaseName = "MSV70TEST3";
            settings.LoginType = "NT";
            settings.Provider = "SQL";
            settings.Server = "ukmm-0153631-8";

            Session.CurrentSession.LogOn(settings, Environment.UserName, "", true);
            return true;
        }
    }
}
