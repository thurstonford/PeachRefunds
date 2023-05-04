using EcomRefunds.Helpers;
using PeachPayments.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PeachPayments.Data
{
    internal static class DataProvider
    {           
        public static void UpdateRefundStatus(
            Int64 refundId, 
            Int32 statusId, 
            string code, 
            string? description,
            string? refundTransactionId) {

            SqlHelper.ExecuteNonQuery(
                SettingsHelper.REFUND_CONN_STR!,
                "peachpayments.uspUpdateRefundStatus",
                CommandType.StoredProcedure,
                new SqlParameter[] {
                    new SqlParameter("@RefundId", refundId),
                    new SqlParameter("@RefundStatusId", statusId),
                    new SqlParameter("@ResultCode", code),
                    new SqlParameter("@ResultDescription", description), // the error, if any
                    new SqlParameter("@RefundTransactionId", refundTransactionId),
                }, 
                true);
        }

        public static Refund? GetRefundToProcess() {
            Refund? peachPaymentsRefund = null;

            DataTable dt = SqlHelper.ExecuteDataTable(
                SettingsHelper.REFUND_CONN_STR!,
                "peachpayments.uspGetRefundToProcess",
                CommandType.StoredProcedure,
                null);

            foreach(DataRow dr in dt.Rows) {
                peachPaymentsRefund = new();
                SetEntityValues(peachPaymentsRefund, dr);
            }

            return peachPaymentsRefund;

        }

        public static void SetEntityValues(object targetObject, DataRow dr) {
            foreach (DataColumn dc in dr.Table.Columns) {
                string columnName = dc.ColumnName;
                object value = dr[columnName];

                PropertyInfo pi = targetObject.GetType().GetProperty(columnName);

                if (pi != null) {
                    Type type = pi.PropertyType;

                    Type uType = Nullable.GetUnderlyingType(type);
                    if (uType != null) {
                        type = uType;
                    }

                    switch (type.FullName) {
                        case "System.String":
                            pi.SetValue(targetObject, GetString(dr, dc.ColumnName), null);
                            break;
                        case "System.Byte":
                            pi.SetValue(targetObject, GetByte(dr, dc.ColumnName), null);
                            break;
                        case "System.Single":
                            pi.SetValue(targetObject, GetSingle(dr, dc.ColumnName), null);
                            break;
                        case "System.Int16":
                            pi.SetValue(targetObject, GetInt16(dr, dc.ColumnName), null);
                            break;
                        case "System.Int32":
                            pi.SetValue(targetObject, GetInt32(dr, dc.ColumnName), null);
                            break;
                        case "System.Int64":
                            pi.SetValue(targetObject, GetInt64(dr, dc.ColumnName), null);
                            break;
                        case "System.Double":
                            pi.SetValue(targetObject, GetDouble(dr, dc.ColumnName), null);
                            break;
                        case "System.Decimal":
                            pi.SetValue(targetObject, GetDecimal(dr, dc.ColumnName), null);
                            break;
                        case "System.DateTime":
                            pi.SetValue(targetObject, GetDateTime(dr, dc.ColumnName), null);
                            break;
                        case "System.Boolean":
                        case "System.Nullable<bool>":
                            pi.SetValue(targetObject, GetBoolean(dr, dc.ColumnName), null);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static T? GetValue<T>(this DataRow row, string columnName) where T : struct {
            if (row.Table.Columns[columnName] == null || row.IsNull(columnName)) {
                return null;
            }
            return row[columnName] as T?;
        }

        public static String GetString(DataRow row, string columnName) {
            return (row.Table.Columns[columnName] == null || row.IsNull(columnName)) ? null : ((String)row[columnName]).Trim();
        }

        public static Single? GetSingle(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Single.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Int16? GetInt16(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Int16.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Int32? GetInt32(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Int32.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Int64? GetInt64(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Int64.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Byte? GetByte(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Byte.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Boolean? GetBoolean(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Boolean.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Decimal? GetDecimal(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Decimal.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static Double? GetDouble(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return Double.Parse(row[columnName].ToString());
            }
            return null;
        }

        public static DateTime? GetDateTime(DataRow row, string columnName) {
            if (ColumnExists(row, columnName) && !ColumnValueIsNull(row, columnName)) {
                return (DateTime)(row[columnName]);
            }
            return null;
        }

        public static bool ColumnExists(DataRow dr, string columnName) {
            return dr.Table.Columns[columnName] != null;
        }

        public static bool ColumnValueIsNull(DataRow dr, string columnName) {
            return dr.IsNull(columnName);
        }
    }
}

