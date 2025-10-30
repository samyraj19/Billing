using Microsoft.Data.SqlClient;
using System.Data;

namespace KBilling.DataBase {
   public class QueryExecutor {
      private SqlCommand CreateCommand (string spName, params SqlParameter[]? parameters) {
         var cmd = new SqlCommand (spName, Db.conn) { CommandType = CommandType.StoredProcedure };
         if (parameters?.Length > 0) cmd.Parameters.AddRange (parameters);
         return cmd;
      }

      // 🔹 SELECT — returns a DataTable
      public DataTable QuerySP (string spName, params SqlParameter[]? parameters) {
         using var cmd = CreateCommand (spName, parameters);
         using var adapter = new SqlDataAdapter (cmd);
         var table = new DataTable ();
         adapter.Fill (table);
         return table;
      }

      // 🔹 INSERT / UPDATE / DELETE — returns affected rows
      public int ExecuteSP (string spName, params SqlParameter[]? parameters) {
         using var cmd = CreateCommand (spName, parameters);
         return cmd.ExecuteNonQuery ();
      }

      // 🔹 Single value (e.g., COUNT(*))
      public object? ScalarSP (string spName, params SqlParameter[]? parameters) {
         using var cmd = CreateCommand (spName, parameters);
         return cmd.ExecuteScalar ();
      }
   }
}
