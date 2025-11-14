using System;
using System.Diagnostics.Contracts;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace KBilling.Helper {
   public class ImageHelper {
      public static Bitmap Load (string img) => new (AssetLoader.Open (new Uri (img)));
   }
}
