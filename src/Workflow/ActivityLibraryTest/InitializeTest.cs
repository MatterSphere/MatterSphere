using System;
using System.Reflection;
using FWBS.Common;
using FWBS.OMS;
using FWBS.OMS.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class InitializeTest
    {
        private static readonly string[] Lan1Machines = new[] {"LAB1", "LAB3"};
        private static readonly string[] Lan2Machines = new[] {"LAB2"};

        /// <summary>
        /// OMS Login
        /// </summary>
        public static void OMSLogin()
        {
            Init(null, Environment.UserName, null);
        }

        /// <summary>
        /// Init
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void Init(string domain, string userName, string password)
        {
            Session.CurrentSession.APIConsumer = Assembly.GetExecutingAssembly();

            var connections = new DatabaseConnections("OMSTest", "DBSETT", "2.0");
            DatabaseSettings settings = connections.CreateDatabaseSettings();
            settings.DatabaseName = "OMS";
            settings.LoginType = "NT";
            settings.Provider = "SQL";
            settings.Server = Environment.MachineName;

            Session.CurrentSession.LogOn(settings, userName, "", true);
            SpecialFolders.SetFolderPath(Environment.SpecialFolder.LocalApplicationData, "C:\\OMSCache\\" + Guid.NewGuid());
        }

        /// <summary>
        /// Get Server Name
        /// </summary>
        /// <returns></returns>
        private static string GetServerName()
        {
            if (Array.Exists(Lan1Machines, m => String.Equals(m, Environment.MachineName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return "LABSERVER";
            }
            if (Array.Exists(Lan2Machines, m => String.Equals(m, Environment.MachineName, StringComparison.InvariantCultureIgnoreCase)))
            {
                return "LABSERVER";
            }

            throw new InvalidOperationException("Machine not found.");
        }

    }
}
