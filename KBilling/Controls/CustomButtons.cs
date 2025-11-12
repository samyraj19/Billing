
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace KBilling.Controls {
   /// <summary> A button control that displays an icon.</summary>
   public class IconButton : Button {
      public IconButton () { }

      public Bitmap? Icon {
         get => GetValue (ActiveImageSourceProperty);
         set => SetValue (ActiveImageSourceProperty, value);
      }

      public bool IsSelected {
         get => GetValue (IsSelectedProperty);
         set => SetValue (IsSelectedProperty, value);
      }

      public static readonly StyledProperty<Bitmap?> ActiveImageSourceProperty =
          AvaloniaProperty.Register<IconButton, Bitmap?> (nameof (Icon));

      public static readonly StyledProperty<bool> IsSelectedProperty =
          AvaloniaProperty.Register<IconButton, bool> (nameof (IsSelected));
   }

   /// <summary>A button control that displays an icon and text.</summary>
   public class IconButtonText : IconButton {
      public IconButtonText () { Classes.Add ("WitheText"); }

      public string Text {
         get => GetValue (TextProperty);
         set => SetValue (TextProperty, value);
      }

      public static readonly StyledProperty<string> TextProperty =
          AvaloniaProperty.Register<IconButtonText, string> (nameof (Text));
   }
}
