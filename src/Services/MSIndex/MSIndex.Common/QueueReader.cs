using System.Data;
using System.Data.SqlClient;
using MSIndex.Common.Interfaces;

namespace MSIndex.Common
{
    public class QueueReader : IQueueReader
    {
        private readonly string _connection;
        private readonly string _query;

        public QueueReader(string connection, string queue, string field = "message_body")
        {
            _connection = connection;
            _query = $"waitfor(RECEIVE top(@count) {field} FROM {queue}), timeout @timeout";
        }

        public byte[] Read()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand(_query, conn))
                {
                    SqlParameter pCount = cmd.Parameters.Add("@count", SqlDbType.Int);
                    pCount.Value = 1;
                    SqlParameter pTimeout = cmd.Parameters.Add("@timeout", SqlDbType.Int);
                    pTimeout.Value = 0;
                    cmd.CommandTimeout = 0;
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        return (byte[])reader.GetValue(0);
                    }

                    return null;
                }
            }
        }
    }
}
