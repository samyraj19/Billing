using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.Model;
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
      DataGridProducts.DoubleTapped += OnDoubleTapped;
      VM ().LoadData ();
   }

   void OnDoubleTapped (object? sender, TappedEventArgs e) {
      if (DataGridProducts.SelectedItem is Product item) {
         if (item.Quantity == 0) MsgBox.ShowErrorAsync ("Item", "OUT OF STOCK.");
         else {
            ProductSelected?.Invoke (item); Close ();
         }
      }
   }

   void OnTextChanging (object? sender, TextChangingEventArgs e) {
      VM ().UpdateFilter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Escape) Close ();
   }

   ProductVM VM () {
      if (DataContext is ProductVM vm) return vm;
      else throw new InvalidOperationException ("DataContext is not of type ProductVM");
   }

   #region Fields
   public event Action<Product>? ProductSelected;
   #endregion
}