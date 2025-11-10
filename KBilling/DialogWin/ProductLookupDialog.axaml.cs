using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.Model;
using KBilling.ViewModel;
using static KBilling.DataBase.SP;

namespace KBilling;
public partial class ProductLookupDialog : Window {
   public ProductLookupDialog () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      mIsShow = false;
      KeyDown -= OnKeyDown;
      ClosedEvent?.Invoke (mProduct);
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      mIsShow = true;
      KeyDown += OnKeyDown;
      DataGridProducts.DoubleTapped += OnDoubleTapped;
      VM ().LoadData ();
   }

   void OnDoubleTapped (object? sender, TappedEventArgs e) {
      if (DataGridProducts.SelectedItem is Product item) {
         if (item.Quantity == 0) MsgBox.ShowErrorAsync ("Item", "OUT OF STOCK.");
         else {
            mProduct = item;
            Close ();
         }
      }
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Escape) Close ();
   }

   public void UpdateRefresh (string? value) => VM ().UpdateFilter (value ?? string.Empty);

   ProductVM VM () => DataContext is ProductVM vm ? vm : throw new InvalidOperationException ("DataContext is not ProductVM");

   #region Fields
   public event Action<Product>? ClosedEvent;
   Product mProduct;
   public bool mIsShow = false;
   #endregion
}