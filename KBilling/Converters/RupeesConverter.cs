using System;
using System.Globalization;
using Avalonia.Data.Converters;
using KBilling.Extension;

namespace KBilling.Converters {
   public class RupeesConverter : IValueConverter {
      public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture) {
         if(value is null) return null;
         if (decimal.TryParse (value.ToString (), out var val)) return val.ToRuppes ();
         return value.ToString ();
      }

      public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture) {
         // Optional: remove ₹ and commas to parse back
         if (value is string s) {
            s = s.Replace ("₹", "").Replace (",", "").Trim ();
            if (decimal.TryParse (s, out var result))
               return result;
         }
         return 0m;
      }
   }
}
