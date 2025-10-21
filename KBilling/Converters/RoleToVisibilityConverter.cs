using System;
using System.Globalization;
using Avalonia.Data.Converters;
using KBilling.Model;

namespace KBilling.Converters {
   internal class RoleToVisibilityConverter : IValueConverter {
      public EUserRoles RequiredRole { get; set; }

      public object Convert (object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture) {
         if (value is EUserRoles role) return role == RequiredRole;
         return false;
      }

      public object? ConvertBack (object? value, Type targetType, object? parameter, CultureInfo culture) {
         throw new NotImplementedException ();
      }
   }
}
