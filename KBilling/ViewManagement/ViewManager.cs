using System;
using System.Reflection.Metadata;
using Avalonia.Controls;

namespace KBilling.ViewManagement {
   public class ViewManager {

      public ViewManager (ContentControl content) {
         ArgumentNullException.ThrowIfNull (content);
         mControl = content;
      }

      public void ShowView (string key) {
         ArgumentNullException.ThrowIfNull (mControl);
         mControl.Content = ViewRegistry.Instance.Get (key);
      }

      #region Fields
      readonly ContentControl? mControl;
      #endregion
   }
}
