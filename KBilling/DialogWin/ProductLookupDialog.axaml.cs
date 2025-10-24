using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewModel;

namespace KBilling;
public partial class ProductLookupDialog : Window {
   public ProductLookupDialog () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      KeyDown -= OnKeyDown;
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      txtSearch.TextChanging += OnTextChanging;
      KeyDown += OnKeyDown;
   }

   void OnTextChanging (object? sender, TextChangingEventArgs e) {
      VM?.UpdateFilter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Escape) Close ();
   }

   #region Fields
   UpdatePricingVM? VM => DataContext as UpdatePricingVM;
   #endregion
}