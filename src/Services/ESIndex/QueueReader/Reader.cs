using System;
using System.Data;
using System.Data.SqlClient;
using Models.Common;

namespace QueueReader
{
    public class Reader
    {
        private readonly string _connection;
        private readonly string _query;

        public Reader(string connection, string queue, string field = "message_body")
        {
            _connection = connection;
            _query = $"waitfor(RECEIVE top(@count) {field}, conversation_handle FROM {queue}), timeout @timeout";
        }

        public Message ReadMessage()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            using (SqlCommand cmd = new SqlCommand(_query, conn))
            {
                SqlParameter pCount = cmd.Parameters.Add("@count", SqlDbType.Int);
                pCount.Value = 1;
                SqlParameter pTimeout = cmd.Parameters.Add("@timeout", SqlDbType.Int);
                pTimeout.Value = 0;
                cmd.CommandTimeout = 0;
                conn.Open();
                var reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    return new Message
                    (
                        (byte[])reader.GetValue(0),
                        ((Guid)reader.GetValue(1)).ToString()
                    );
                }

                return null;
            }
        }
    }
}
