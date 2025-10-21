using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace KBilling.Helper {
   public static class EnterFocusHelper {
      /// <summary>
      /// Attach Enter-to-next focus behavior for all input controls inside a root.
      /// </summary>
      public static void Attach (Control root) {
         // Get all TextBoxes inside the visual tree
         foreach (var input in root.GetVisualDescendants ().OfType<IInputElement> ()) {
            input.KeyDown += OnEnterKeyDown;
         }
      }

      private static void OnEnterKeyDown (object? sender, KeyEventArgs e) {
         if (e.Key != Key.Enter || sender is not TextBox textBox) return;
         e.Handled = true;
         // Find next focusable control
         var next = KeyboardNavigationHandler.GetNext (textBox, NavigationDirection.Next);
         if (next != null) {
            // Focus the next control directly
            next.Focus ();
         }
      }
   }
}
