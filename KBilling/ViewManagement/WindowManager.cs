using Avalonia.Controls;

namespace KBilling.ViewManagement {
   public sealed class WindowManager {
      public void ShowWindow (string key) {
         var window = WindowRegistry.Instance.Get (key);
         window.Show ();
      }

      public Window ShowDialog (Window owner, string key) {
         var window = WindowRegistry.Instance.Get (key);
         window.ShowDialog (owner);
         return window;
      }

      public Window GetWindow (string key) => WindowRegistry.Instance.Get (key);
   }
}
