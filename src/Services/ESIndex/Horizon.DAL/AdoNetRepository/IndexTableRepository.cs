using System;
using System.Data.SqlClient;
using Common.AdoNet;
using Horizon.Common.Interfaces;

namespace Horizon.DAL.AdoNetRepository
{
    public class IndexTableRepository : IIndexTableRepository
    {
        private readonly string _connection;

        public IndexTableRepository(string connection)
        {
            _connection = connection;
        }

        public bool GetObjectTypeFullCopyRequired(string objectTypeName)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT TOP(1) FullCopyRequired FROM [search].[ESIndexTable] WHERE ObjectType = @ObjectType", conn);
                var objectTypeParameter = new StringParameter("ObjectType", objectTypeName);
                objectTypeParameter.AddSqlParameter(command.Parameters);

                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader.GetBoolean(0);
                    }
                }
            }
            throw new ArgumentException($"ObjectType {objectTypeName} not found.");
        }

        public void SetObjectTypeFullCopyRequired(string objectTypeName, bool fullCopyRequired)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("UPDATE [search].[ESIndexTable] SET [FullCopyRequired] = @FullCopyRequired WHERE ObjectType = @ObjectType", conn);
                var fullCopyRequiredParameter = new BoolParameter("@FullCopyRequired", fullCopyRequired);
                fullCopyRequiredParameter.AddSqlParameter(command.Parameters);
                var objectTypeParameter = new StringParameter("ObjectType", objectTypeName);
                objectTypeParameter.AddSqlParameter(command.Parameters);

                command.ExecuteNonQuery();
            }
        }
    }
}
