using System;
using Avalonia.Controls;

namespace KBilling.ViewManagement {
   public class ViewManager {

      public ViewManager (ContentControl content) {
         mContentControl = content ?? throw new ArgumentNullException (nameof (content));
      }

      public void ShowView (string key) {
         mContentControl.Content = ViewRegistry.Instance.Get (key);
      }

      #region Fields
      readonly ContentControl? mContentControl;
      #endregion
   }
}
