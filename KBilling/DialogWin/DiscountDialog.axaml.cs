using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewManagement;
using KBilling.ViewModel;

namespace KBilling;
public partial class DiscountDialog : Window {

   public DiscountDialog () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += Unload;
   }

   void Unload (object? sender, RoutedEventArgs e) {
      BtnAdd.Click -= (s, e) => Close ();
      BtnCancel.Click -= (s, e) => Close ();
   }

   void OnKeyDown (object? sender, KeyEventArgs e) {
      if (e.Key == Key.Enter) AddDiscount ();
      else if (e.Key == Key.Escape) Close ();
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnAdd.Click += (s, e) => AddDiscount ();
      BtnCancel.Click += (s, e) => Close ();
      KeyDown += OnKeyDown;
   }

   void AddDiscount () {
      Discount = decimal.TryParse (TextboxDiscount.Text, out var discount) ? discount : 0m;
      DiscountApplied.Invoke(discount);
      Close ();
   }

   #region Fields
   public decimal Discount { get; private set; } = 0m;
   public event Action<decimal> DiscountApplied;
   #endregion
}