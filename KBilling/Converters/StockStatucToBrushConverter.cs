using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace KBilling.Converters {
   public class StockStatucToBrushConverter : IValueConverter {
      public object? Convert (object? value, Type targetType, object? parameter, CultureInfo culture) {
         return value switch {
            "Low" => Brushes.Red,
            "Medium" => Brushes.Yellow,
            "High" => Brushes.Green,
            "InSufficient" => Brushes.DarkRed,
            _ => Avalonia.Media.Brushes.Gray
         };
      }

      public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture) {
         throw new NotImplementedException ();
      }
   }
}
