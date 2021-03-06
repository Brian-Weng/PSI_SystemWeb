using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PIS_System.Managers
{
    public class DBBase
    {
        public DataTable GetDataTable(string dbCommand, List<SqlParameter> parameters)
        {
            string connectionString = this.GetConnectionString();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(dbCommand, connection);
                command.Parameters.AddRange(parameters.ToArray());
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    
                    DataTable dt = null;
                    if(reader.HasRows)
                    {
                        dt = new DataTable();
                        dt.Load(reader);
                    }

                    reader.Close();
                    return dt;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public object GetScale(string dbCommand, List<SqlParameter> parameters)
        {
            string connectionString = this.GetConnectionString();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(dbCommand, connection);
                List<SqlParameter> parameters2 = new List<SqlParameter>();
                foreach (var item in parameters)
                {
                    parameters2.Add(new SqlParameter(item.ParameterName, item.Value));
                }
                command.Parameters.AddRange(parameters2.ToArray());
                try
                {
                    connection.Open();
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public int ExecuteNonQuery(string dbCommand, List<SqlParameter> parameters)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(dbCommand, connection);

                var newParameter = parameters.Select(c => ((ICloneable)c).Clone());
                command.Parameters.AddRange(newParameter.ToArray());

                connection.Open();
                SqlTransaction sqlTransaction = connection.BeginTransaction();
                command.Transaction = sqlTransaction;

                try
                {
                    int totalChange = command.ExecuteNonQuery();
                    sqlTransaction.Commit();
                    return totalChange;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private string GetConnectionString()
        {
            var manage = System.Configuration.ConfigurationManager.ConnectionStrings["PSI_System"];

            if (manage == null)
                return string.Empty;
            else
                return manage.ConnectionString;
        }
    }
}