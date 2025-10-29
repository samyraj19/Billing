using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewModel;

namespace KBilling;

public partial class PriceUpdateView : UserControl {
   public PriceUpdateView () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      BtnUpdatePrices.Click -= BtnUpdatePrices_Click;
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      BtnUpdatePrices.Click += BtnUpdatePrices_Click;
      VM ().LoadData ();
   }

   void BtnUpdatePrices_Click (object? sender, RoutedEventArgs e) {
      var items = VM ().FilterProducts?.Where (p => p.IsModified ()).ToList (); // Get modified items
   }

   UpdatePricingVM VM () {
      if (DataContext is UpdatePricingVM vm) return vm;
      throw new InvalidOperationException ("DataContext is not of type UpdatePricingVM");
   }
}