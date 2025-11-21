using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Helper;
using KBilling.ViewModel;

namespace KBilling;
public partial class PriceUpdateView : UserControl {
   public PriceUpdateView () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) { }

   void OnLoad (object? sender, RoutedEventArgs e) => VM?.LoadData ();

   UpdatePricingVM? VM => DataContext as UpdatePricingVM;

}