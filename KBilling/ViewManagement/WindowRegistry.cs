using System;
using Avalonia.Controls;
using System.Collections.Generic;

namespace KBilling.ViewManagement {
   public sealed class WindowRegistry {
      public void Register (string key, Window window) {
         ArgumentNullException.ThrowIfNull (key);
         ArgumentNullException.ThrowIfNull (window);

         mWindows.TryAdd (key, window);
      }

      public Window Get (string key)
          => mWindows.TryGetValue (key, out var window)
              ? window
              : throw new KeyNotFoundException ($"Window '{key}' not registered.");

      #region Fields
      static readonly Lazy<WindowRegistry> mInstance = new (() => new ());
      public static WindowRegistry Instance => mInstance.Value;
      readonly Dictionary<string, Window> mWindows = new ();
      #endregion
   }
}
