using System;
using System.Globalization;
using Avalonia.Data.Converters;
using KBilling.Extension;

namespace KBilling.Converters {
   public class DsicountRupeesConverter : IValueConverter {
      public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture) {
         if (value is null) return string.Empty;
         if(decimal.TryParse (value.ToString (), out var val)) {
            // Always show with minus symbol
            string formatted = Math.Abs (val).ToRuppes ();
            return $"-{formatted}";
         }
         return value.ToString ();
      }

      public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture) {
         if (value is string s) {
            s = s.Replace ("₹", "")
                 .Replace (",", "")
                 .Replace ("-", "")
                 .Trim ();

            if (decimal.TryParse (s, out var result)) {
               // Always return negative value
               return -Math.Abs (result);
            }
         }
         return 0m;
      }
   }
}
