using System.Globalization;

namespace KBilling.Extension {
   public static class CurrencyExtension {
      public static string ToRuppes (this decimal amount) {
         var culture = new CultureInfo ("en-IN");
         culture.NumberFormat.CurrencySymbol = "₹";
         return string.Format (culture, "{0:C}", amount);
      }
   }
}
