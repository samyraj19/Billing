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

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      BtnUpdatePrices.Click -= BtnUpdatePrices_Click;
      SearchTxt.TextChanging -= (s, ev) => VM ().UpdateFilter (SearchTxt?.Text);
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      BtnUpdatePrices.Click += BtnUpdatePrices_Click;
      Load ();
      SearchTxt.TextChanging += (s, ev) => VM ().UpdateFilter (SearchTxt?.Text);
   }

   void BtnUpdatePrices_Click (object? sender, RoutedEventArgs e) {
      var items = VM ().FilterProducts?.Where (p => p.IsModified ()).ToList (); // Get modified items
      if (VM ().UpdatePricing (items)) {
         MsgBox.ShowSuccessAsync ("Success", "Prices updated successfully.");
         Load ();
      } else MsgBox.ShowErrorAsync ("Fail", "Failed to update prices.");
   }

   void Load () => VM ().LoadData ();

   UpdatePricingVM VM () => DataContext is UpdatePricingVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type UpdatePricingVM");

}