using System;
using System.Collections.Generic;
using System.Data;
using FWBS.OMS.Data;

namespace FWBS.OMS.Addin.Security
{
    public class Services
    {
        private Services()
        {
        }

        public static void ShowDocumentSecurity(Int64 docID)
        {
            FWBS.OMS.Addin.Security.Windows.frmSecurityDialog dialog = new FWBS.OMS.Addin.Security.Windows.frmSecurityDialog(docID);
            dialog.ShowDialog();
            dialog.Dispose();
            dialog = null;
        }


        /// <summary>
        /// Advanced Security Diagnostic Tools
        /// </summary>
        public class DiagnosticTools
        {
            /// <summary>
            /// Provides a security report for the selected user and object
            /// </summary>
            /// <param name="userNTLogin">NTLogin of the user to be checked</param>
            /// <param name="objectCode">Type of object, for example 'FILE'</param>
            /// <param name="objectID">The object's underlying ID</param>
            /// <returns></returns>
            public static string UserAccessReport(string userNTLogin, string objectType, long objectID)
            {
                System.Diagnostics.Debug.WriteLine("UserAccessReport", "ADVSECURITY");

                string _msg = "";

                DataTable _dtChk = CheckAccess(userNTLogin, objectType, objectID);
                if (_dtChk != null)
                {
                    if (_dtChk.Rows.Count > 0)
                        _msg = _dtChk.Rows[0][0].ToString();
                }

                return _msg;
            }

            private static DataTable CheckAccess(string userNTLogin, string objectType, long objectID)
            {
                System.Diagnostics.Debug.WriteLine("Check Access", "ADVSECURITY");
                System.Diagnostics.Debug.WriteLine(string.Format("userNTLogin {0}", userNTLogin), "ADVSECURITY");
                System.Diagnostics.Debug.WriteLine(string.Format("objectCode {0}", objectType), "ADVSECURITY");
                System.Diagnostics.Debug.WriteLine(string.Format("objectID {0}", objectID), "ADVSECURITY");

                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("user", userNTLogin));
                parList.Add(connection.CreateParameter("type", objectType));
                parList.Add(connection.CreateParameter("ID", objectID));
                return connection.ExecuteProcedure("config.CheckAccess", parList);
            }
        }

    }
}