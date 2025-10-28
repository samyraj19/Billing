using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KBilling.DataBase {
   public static class DBConn {
      public static void Connect () {
         conn ??= new (mConnStr);
         if (!IsOpen ()) conn.Open ();
      }

      public static SqlConnection Get () {
         Connect ();
         return conn!;
      }

      public static bool IsOpen () => conn?.State == ConnectionState.Open;

      public static void Disconnect () => conn?.Close ();

      #region Fields
      static readonly string mConnStr = ConfigurationManager.ConnectionStrings["kbilling"].ConnectionString;
      static SqlConnection? conn;
      #endregion
   }
}
