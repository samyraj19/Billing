using System;
using Avalonia;
using Avalonia.Controls;
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

   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnAdd.Click += (s, e) => AddDiscount ();
      BtnCancel.Click += (s, e) => Close ();
   }

   void AddDiscount () {
      Discount = decimal.TryParse (TextboxDiscount.Text, out var discount) ? discount : 0m;
      Close ();
   }

   #region Fields
   public decimal Discount { get; private set; } = 0m;
   #endregion
}