
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using KBilling.Helper;

namespace KBilling.Controls {
   /// <summary> A button control that displays an icon.</summary>
   public class IconButton : Button {
      public IconButton () { Classes.Add ("Base"); }

      public Bitmap? Icon {
         get => GetValue (ActiveImageSourceProperty);
         set => SetValue (ActiveImageSourceProperty, value);
      }

      public bool IsSelected {
         get => GetValue (IsSelectedProperty);
         set => SetValue (IsSelectedProperty, value);
      }

      public bool UseHoverIcon {
         get => GetValue (IsHoverProperty);
         set => SetValue (IsHoverProperty, value);
      }

      public static readonly StyledProperty<Bitmap?> ActiveImageSourceProperty =
          AvaloniaProperty.Register<IconButton, Bitmap?> (nameof (Icon));

      public static readonly StyledProperty<bool> IsSelectedProperty =
          AvaloniaProperty.Register<IconButton, bool> (nameof (IsSelected));

      public static readonly StyledProperty<bool> IsHoverProperty =
          AvaloniaProperty.Register<IconButton, bool> (nameof (UseHoverIcon));

      protected override void OnPointerEntered (PointerEventArgs e) {
         base.OnPointerEntered (e);
         if (!UseHoverIcon) return;
         if (Icon != null) Icon = ImageHelper.Load ("avares://KBilling/Assets/close-hover.png");
      }

      protected override void OnPointerExited (PointerEventArgs e) {
         base.OnPointerExited (e);
         if (!UseHoverIcon) return;
         if (Icon != null) Icon = ImageHelper.Load ("avares://KBilling/Assets/close-black.png");
      }
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

   public class LeftMenubarButton : IconButtonText {
      public LeftMenubarButton () { Classes.Add ("Leftmenu"); }
      protected override void OnPointerEntered (PointerEventArgs e) {
         base.OnPointerEntered (e);
         UpdateIcon ();
      }

      protected override void OnPointerExited (PointerEventArgs e) {
         base.OnPointerExited (e);
         UpdateIcon ();
      }

      public void UpdateIcon () {
         if (Tag is not string key) return;
         IconStore.LoadImage ();
         Icon = IsSelected ? IconStore.Active[key] : IconStore.Inactive[key];
      }
   }
}
