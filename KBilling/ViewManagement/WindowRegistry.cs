using System;
using Avalonia.Controls;
using System.Collections.Generic;

namespace KBilling.ViewManagement {
   public sealed class WindowRegistry {
      public void Register<T> (string key) where T : Window, new() {
         ArgumentNullException.ThrowIfNull (key);
         mFactories[key] = () => new T ();
      }

      public Window Get (string key) {
         ArgumentNullException.ThrowIfNull (key);

         if (mFactories.TryGetValue (key, out var factory))
            return factory ();

         throw new KeyNotFoundException ($"Window '{key}' not registered.");
      }

      #region Fields
      static readonly Lazy<WindowRegistry> mInstance = new (() => new ());
      public static WindowRegistry Instance => mInstance.Value;
      readonly Dictionary<string, Func<Window>> mFactories = new ();
      #endregion
   }
}
