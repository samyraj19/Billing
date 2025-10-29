using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewModel;

namespace KBilling;
public partial class StocksView : UserControl {
   public StocksView () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      //throw new NotImplementedException ();
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
     VM().LoadData ();
   }

   StockVM VM () {
      if (DataContext is StockVM vm) return vm;
      throw new InvalidOperationException ("DataContext is not of type UpdatePricingVM");
   }
}