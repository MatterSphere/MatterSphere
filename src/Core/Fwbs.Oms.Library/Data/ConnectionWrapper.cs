using System;

namespace FWBS.OMS.Data
{
    using System.Data;
    using System.Diagnostics;
    using System.Reflection;
    using Fwbs.Framework.Licensing;

    internal sealed class ConnectionWrapper : IConnection
    {
        private readonly Connection connection;
        private readonly ILicensingManager manager;

        internal ConnectionWrapper(Connection connection, ILicensingManager manager)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;
            this.manager = manager;
        }

        public void Connect()
        {
            ValidateCaller(null, "Connect");

            connection.Connect();
        }

        public void Disconnect()
        {
            ValidateCaller(null, "Disconnect");

            connection.Disconnect();
        }

        public DataTable Execute(DataTableExecuteParameters parameters)
        {
            ValidateCaller(parameters, "Execute");

            return connection.Execute(parameters);
        }

        public DataSet Execute(DataSetExecuteParameters parameters)
        {
            ValidateCaller(parameters, "Execute");

            return connection.Execute(parameters);
        }

        public int Execute(ExecuteParameters parameters)
        {
            ValidateCaller(parameters, "Execute");

            return connection.Execute(parameters);
        }

        public object ExecuteScalar(ExecuteParameters parameters)
        {
            ValidateCaller(parameters, "ExecuteScalar");

            return connection.ExecuteScalar(parameters);
        }

        public IDataReader ExecuteReader(ExecuteParameters parameters)
        {
            ValidateCaller(parameters, "ExecuteReader");

            return connection.ExecuteReader(parameters);
        }

        public IDataParameter CreateParameter(string name, object value)
        {
            return connection.CreateParameter(name, value);
        }

        public void Update(DataTableUpdateParameters parameters)
        {
            ValidateCaller(parameters, "Update");

            connection.Update(parameters);
        }

        public void Update(DataRowUpdateParameters parameters)
        {
            ValidateCaller(parameters, "Update");

            connection.Update(parameters);
        }


        private void ValidateCaller(object val, string method)
        {
            var currentmeth = MethodInfo.GetCurrentMethod();

            var stack = new StackTrace(false);

            foreach (var fr in stack.GetFrames())
            {
                var meth = fr.GetMethod();

                if (meth == currentmeth)
                    continue;

                if (meth.DeclaringType != null)
                {
                    var ass = meth.DeclaringType.Assembly;

                    var consumer = manager.GetConsumer(ass);

                    if (Fwbs.Framework.ProductInfo.Tokens.PublicKeyToken.Equals(consumer.PublicKeyToken, StringComparison.OrdinalIgnoreCase))
                        continue;

                    manager.Validate(ass);

                    const string Category = "Connection Accessed";
                    const string Output = @"'{0}' Called By {1} ({2})";

                    Trace.WriteLine(String.Format(Output, method, meth, ass), Category);
                }
                else
                    Debug.WriteLine("DeclaringType is null - skipping");

                break;
            }

        }

        public bool IsConnected
        {
            get { return connection.IsConnected; }
        }

        public bool IsExecuting
        {
            get { return connection.IsExecuting; }
        }
    }
}
