using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewManagement;
using KBilling.ViewModel;

namespace KBilling;
public partial class DiscountDialog : Window {

   public DiscountDialog () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += Unload;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnAdd.Click += OnAddClick;
      BtnCancel.Click += (s, e) => Close ();
      KeyDown += OnKeyDown;
      TextboxDiscount.AddHandler (InputElement.TextInputEvent, NumHelper.OnIntOnly, RoutingStrategies.Tunnel);
   }

   void Unload (object? sender, RoutedEventArgs e) {
      BtnAdd.Click -= OnAddClick;
      BtnCancel.Click -= (s, e) => Close ();
      KeyDown -= OnKeyDown;
   }

   void OnAddClick (object? sender, RoutedEventArgs e) => ApplyDiscount ();

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) {
         ApplyDiscount ();
         e.Handled = true;
      } else if (e.Key == Key.Escape) Close ();
   }

   void ApplyDiscount () {
      if (decimal.TryParse (TextboxDiscount.Text, out var value)) {
         DiscountApplied?.Invoke (value);
         Close ();
      }
   }

   #region Fields
   public event Action<decimal>? DiscountApplied;
   #endregion
}