using System;

namespace KBilling.Services {
   public class ProductWatcher {

      public void StartWatching () {

      }

      #region Fields
      event EventHandler? ProductChanged;
      #endregion
   }
}
