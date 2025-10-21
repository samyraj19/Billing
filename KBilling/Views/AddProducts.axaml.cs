using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewModel;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;

namespace KBilling;
public partial class AddProducts : UserControl {
   public AddProducts () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnLoaded;
      KeyDown += OnKeyDown;
      SearchTxt.TextChanging += SearchTextChanging;
      EnterFocusHelper.Attach (MainPanel);
      ProductNumberTextbox.AddHandler (InputElement.TextInputEvent, TextInputForInteger, RoutingStrategies.Tunnel);
      txtQty.AddHandler (InputElement.TextInputEvent, TextInputForInteger, RoutingStrategies.Tunnel);
      txtPurRate.AddHandler (InputElement.TextInputEvent, TextInputForDecimal, RoutingStrategies.Tunnel);
      txtSellRate.AddHandler (InputElement.TextInputEvent, TextInputForDecimal, RoutingStrategies.Tunnel);
   }

   void SearchTextChanging (object? sender, TextChangingEventArgs e) => VM?.UpdateFilter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);

   void OnLoaded (object? sender, Avalonia.Interactivity.RoutedEventArgs e) => BtnSubmit.Click += OnBtnSubmit;

   private void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
         Submit ();
         e.Handled = true;
      }
   }

   void OnBtnSubmit (object? sender, RoutedEventArgs e) => Submit ();

   void OnUnLoaded (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
      throw new System.NotImplementedException ();
   }

   void TextInputForInteger (object? sender, TextInputEventArgs e) => e.Handled = !int.TryParse (e.Text, out _);

   void TextInputForDecimal (object? sender, TextInputEventArgs e) {  // Only digits or dot
      if (!string.IsNullOrEmpty (e.Text) && !e.Text.All (c => char.IsDigit (c) || c == '.')) e.Handled = true;
      // Prevent more than one dot
      if ((!string.IsNullOrEmpty (txtPurRate.Text) && !string.IsNullOrEmpty (e.Text)) && e.Text.Contains ('.') && txtPurRate.Text.Contains ('.')) e.Handled = true;
      if ((!string.IsNullOrEmpty (txtSellRate.Text) && !string.IsNullOrEmpty (e.Text)) && e.Text.Contains ('.') && txtSellRate.Text.Contains ('.')) e.Handled = true;
   }

   #region Methods
   void Submit () {
      if (VM.CanSubmit ()) {
         MessageBoxManager.GetMessageBoxStandard ("Add product", "Product added successfully.", ButtonEnum.Ok, Icon.Success)
                          .ShowAsync ();
         VM.AddProductToList ();
         VM?.Clear ();
      }
   }

   #endregion

   #region Fields
   ProductVM? VM => DataContext as ProductVM;
   #endregion
}