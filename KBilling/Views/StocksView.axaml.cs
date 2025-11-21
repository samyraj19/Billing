using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewModel;

namespace KBilling;
public partial class StocksView : UserControl {
   public StocksView () {
      InitializeComponent ();
      Loaded += OnLoaded;
   }

   void OnLoaded (object? sender, RoutedEventArgs e) => VM?.LoadStockData ();
 
   StockVM? VM => DataContext as StockVM;
}