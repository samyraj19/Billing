using Avalonia.Controls;

namespace KBilling.ViewManagement {
   public sealed class WindowManager {
      public void ShowWindow (string key) {
         var window = WindowRegistry.Instance.Get (key);
         window.Show ();
      }

      public void ShowDialog (Window owner, string key) {
         var window = WindowRegistry.Instance.Get (key);
         window.ShowDialog (owner);
      }
   }
}
