using System.Data;
using System.Data.SqlClient;

namespace PeachPayments.Data
{
    internal class SqlHelper
    {
        public static SqlCommand CreateCommand(string connectionString, string sqlCommand, CommandType commandType, SqlParameter[]? sqlParameters) {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new(sqlCommand, conn) {
                CommandType = commandType,
                CommandTimeout = 60
            };
            if(sqlParameters != null && sqlParameters.Length > 0) {
                cmd.Parameters.AddRange(sqlParameters);
            }

            return cmd;
        }

        public static DataTable ExecuteDataTable(string connectionString, string sqlCommand, CommandType commandType, SqlParameter[]? sqlParameters) {
            DataTable dtResult = new DataTable();
            SqlCommand? cmd = null;
            SqlDataAdapter dta;
            try {
                cmd = CreateCommand(connectionString, sqlCommand, commandType, sqlParameters);
                dta = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                dta.Fill(dtResult);
            } catch(Exception ex) {
                Logger.Error(ex.GetBaseException().ToString());
                throw;
            } finally {
                if(cmd != null && cmd.Connection != null) {
                    cmd.Connection.Close();
                }
            }

            return dtResult;
        }

        public static void ExecuteNonQuery(string connectionString, string sqlCommand, CommandType commandType, SqlParameter[] sqlParameters, bool createTransaction) {
            SqlCommand? cmd = null;
            try {
                cmd = CreateCommand(connectionString, sqlCommand, commandType, sqlParameters);

                cmd.Connection.Open();
                if(createTransaction) {
                    cmd.Transaction = cmd.Connection.BeginTransaction();
                }

                cmd.ExecuteNonQuery();

                if(createTransaction && cmd.Transaction != null) {
                    cmd.Transaction.Commit();
                }
            } catch(Exception ex) {
                Logger.Error(ex.GetBaseException().ToString());
                if(createTransaction && cmd.Transaction != null) {
                    cmd.Transaction.Rollback();
                }
                throw;
            } finally {
                if(cmd != null && cmd.Connection != null) {
                    cmd.Connection.Close();
                }               
            }
        }
    }
}
