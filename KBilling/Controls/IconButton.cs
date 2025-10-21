using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace KBilling.Controls {
   public class IconButton : Button {
      public IconButton () {
         Classes.Add ("IconButtonStyle");
      }
      public Bitmap? Icon {
         get => GetValue (ActiveImageSourceProperty);
         set => SetValue (ActiveImageSourceProperty, value);
      }

      public static readonly StyledProperty<Bitmap?> ActiveImageSourceProperty = AvaloniaProperty.Register<IconButton, Bitmap?> (nameof (Icon));
   }
}
