using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Displays a connection builder popup form to create or edit.
    /// </summary>
    public sealed class ConnectionStringBuilder
    {
        private ConnectionStringBuilder() { }

        /// <summary>
        /// Displays the ConnectionBuilder for the creation of a new string.
        /// </summary>
        /// <returns>The created connection string.</returns>
        public static string Show()
        {

            return Show(null as IWin32Window);
        }


        /// <summary>
        /// Displays the ConnectionBuilder for the creation of a new string.
        /// </summary>
        /// <param name="owner">Owner of the form.</param>
        /// <returns>The created connection string.</returns>
        public static string Show(IWin32Window owner)
        {
            object cb = null;
            object cnn = null;

            Type t = Type.GetTypeFromProgID("DataLinks");

            if (t == null)
                t = Type.GetTypeFromCLSID(new Guid("{2206CDB2-19C1-11D1-89E0-00C04FD7A829}"));

            if (t == null)
                return "";
            else
                cb = Activator.CreateInstance(t);

            if (cb == null)
                return "";

            if (owner != null)
                cb.GetType().InvokeMember("hWnd", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty, null, cb, new object[1] { owner.Handle });

            cnn = cb.GetType().InvokeMember("PromptNew", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod, null, cb, null);

            if (cnn == null)
                return "";

            return Convert.ToString(cnn.GetType().InvokeMember("ConnectionString", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty, null, cnn, null));

        }

        /// <summary>
        /// Displays the ConnectionBuilder for the creation of an existing string.
        /// </summary>
        /// <param name="connectionString">Connection string to edit.</param>
        /// <returns>The created / altered connection string.</returns>
        public static string Show(string connectionString)
        {
            return Show(connectionString, null);
        }

        /// <summary>
        /// Displays the ConnectionBuilder for the creation of an existing string.
        /// </summary>
        /// <param name="connectionString">Connection string to edit.</param>
        /// <param name="owner">Owner of the form.</param>
        /// <returns>The created / altered connection string.</returns>
        public static string Show(string connectionString, IWin32Window owner)
        {
            object cb = null;
            object cnn = null;

            Type t = Type.GetTypeFromProgID("DataLinks");

            if (t == null)
                t = Type.GetTypeFromCLSID(new Guid("{2206CDB2-19C1-11D1-89E0-00C04FD7A829}"));

            if (t == null)
                return connectionString;
            else
                cb = Activator.CreateInstance(t);

            if (cb == null)
                return connectionString;

            if (owner != null)
                cb.GetType().InvokeMember("hWnd", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty, null, cb, new object[1] { owner.Handle });

            Type t2 = Type.GetTypeFromProgID("ADODB.Connection");
            if (t2 == null)
                return connectionString;

            cnn = Activator.CreateInstance(t2);
            cnn.GetType().InvokeMember("ConnectionString", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.SetProperty, null, cnn, new object[1] { connectionString });

            bool ret = Convert.ToBoolean(cb.GetType().InvokeMember("PromptEdit", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.InvokeMethod, null, cb, new object[1] { cnn }));
            if (ret == false)
                return connectionString;
            else
                return Convert.ToString(cnn.GetType().InvokeMember("ConnectionString", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.GetProperty, null, cnn, null));

        }

    }

}
