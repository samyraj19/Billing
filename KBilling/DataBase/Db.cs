using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KBilling.DataBase {
   public static class Db {
      public static void Connect () {
         string connstr = ConfigurationManager.ConnectionStrings["kbilling"].ConnectionString;

         if (conn is { State: ConnectionState.Open }) return;

         conn = new SqlConnection (connstr);
         conn.Open ();
      }

      public static void Disconnect () => conn?.Close ();

      #region Fields
      public static SqlConnection? conn;
      #endregion
   }
}
