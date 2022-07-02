using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.providers
{
    public class DataProvider
    {
        private string? ConnectionString { get; set; }

        public DataProvider() { }

        public DataProvider(string? connectionString)
        {
            if (connectionString == null)
            {
                throw new NullReferenceException("Missing configuration file.");
            }
            ConnectionString = connectionString;
        }

        public void CloseConnection(SqlConnection connection) => connection.Close();

        public SqlParameter CreateParameter(string name, int size,
            object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Size = size,
                Direction = direction,
                Value = value
            };
        }

        public IDataReader GetDataReader(string cmdText, CommandType cmdType,
            out SqlConnection connection, params SqlParameter[] parameters)
        {
            IDataReader? reader = null;
            try
            {
                connection = new SqlConnection(ConnectionString);
                connection.Open();
                var command = new SqlCommand(cmdText, connection);
                command.CommandType = cmdType;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return reader;
        }

        public void ModifyData(string cmdText, CommandType cmdType, params SqlParameter[] parameters)
        {
            try
            {
                using var connection = new SqlConnection(ConnectionString);
                connection.Open();
                using var command = new SqlCommand(cmdText, connection);
                command.CommandType = cmdType;

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                }
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
