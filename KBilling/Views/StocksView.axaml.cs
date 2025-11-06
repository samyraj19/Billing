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
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      BtnUpdatestock.Click -= BtnUpdateQtyClick;
      SearchTxt.TextChanging -= (s, ev) => VM ().UpdateFilter (SearchTxt?.Text ?? string.Empty);
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      LoadData ();
      BtnUpdatestock.Click += BtnUpdateQtyClick;
      SearchTxt.TextChanging += (s, ev) => VM ().UpdateFilter (SearchTxt?.Text ?? string.Empty);
   }

   void BtnUpdateQtyClick (object? sender, RoutedEventArgs e) {
      var items = VM ().FilterProducts?.Where (p => p.IsModifiedQty ()).ToList (); // Get modified items
      if (items is not null && VM ().UpdateQty (items)) {
         MsgBox.ShowSuccessAsync ("Success", "Quantity updated successfully.");
         LoadData ();
      } else MsgBox.ShowErrorAsync ("Fail", "Failed to update Quantity.");
   }

   void LoadData () {
      VM ().LoadData ();
      VM ().UpdateStock ();
   }

   StockVM VM () => DataContext is StockVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type StockVM");
}