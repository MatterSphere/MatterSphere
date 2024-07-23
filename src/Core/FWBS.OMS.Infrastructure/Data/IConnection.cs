using System.Data;

namespace FWBS.OMS.Data
{
    public interface IConnection 
    {
      
        void Connect();

        void Disconnect();

        DataTable Execute(DataTableExecuteParameters parameters);

        DataSet Execute(DataSetExecuteParameters parameters);

        int Execute(ExecuteParameters parameters);

        object ExecuteScalar(ExecuteParameters parameters);

        IDataReader ExecuteReader(ExecuteParameters parameters);

        IDataParameter CreateParameter(string name, object value);

        void Update(DataTableUpdateParameters parameters);

        void Update(DataRowUpdateParameters parameters);

        bool IsConnected { get; }

        bool IsExecuting { get; }


    }
}
