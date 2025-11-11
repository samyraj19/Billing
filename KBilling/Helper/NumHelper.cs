using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using KBilling.Model;

namespace KBilling.Helper {
   public class NumHelper : BaseModel {
      // Integer only  
      public static void OnIntOnly (object? sender, TextInputEventArgs e) => e.Handled = !string.IsNullOrEmpty (e.Text) && !e.Text.All (char.IsDigit);

      // Decimal (with one dot)  
      public static void OnDecimalOnly (object? sender, TextInputEventArgs e) {
         if (sender is not TextBox tb || string.IsNullOrEmpty (tb.Text) || string.IsNullOrEmpty (e.Text)) return;
         bool invalid = e.Text.Any (c => !char.IsDigit (c) && c != '.') || (e.Text.Contains ('.') && tb.Text.Contains ('.'));
         e.Handled = invalid;
      }
   }
}
