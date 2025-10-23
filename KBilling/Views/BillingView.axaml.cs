using System.Linq;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using KBilling.ViewManagement;

namespace KBilling;
public partial class BillingView : UserControl {
   public BillingView () {
      InitializeComponent ();
      Loaded += OnLoaded;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      BtnDiscount.Click += OnDiscountClick;
   }

   void OnDiscountClick (object? sender, RoutedEventArgs e) {
      mManager.ShowDialog (MainWindow.Instance, "DiscountDialog");
   }

   #region Fields
   readonly WindowManager mManager = new ();
   #endregion

}