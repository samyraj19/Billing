using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace KBilling.ViewManagement {
   public sealed class ViewRegistry {

      private ViewRegistry () { }
      public void Register (string key, UserControl view) {
         ArgumentException.ThrowIfNullOrEmpty (key);
         ArgumentNullException.ThrowIfNull (view);
         if (!mViews.ContainsKey (key)) mViews.Add (key, view);
      }

      public UserControl Get (string key) => mViews.TryGetValue (key, out var view) ? view : throw new KeyNotFoundException ($"View '{key}' is not registered.");

      #region Fields
      static readonly Lazy<ViewRegistry> mInstance = new (() => new ViewRegistry ());
      public static ViewRegistry Instance => mInstance.Value;
      readonly Dictionary<string, UserControl> mViews = new ();
      #endregion
   }
}
