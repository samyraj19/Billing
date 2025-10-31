using System;

namespace KBilling.Extension {
   public static class DateTimeEx {
         /// <summary>Converts a DateTime to a SQL-compatible string format (yyyy-MM-dd HH:mm:ss.fffffff).</summary>
         public static string ToSql (this DateTime value)=> value.ToString ("yyyy-MM-dd HH:mm:ss.fffffff");
   }
}
