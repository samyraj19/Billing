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
      BtnUpdatePrices.Click += BtnUpdatePrices_Click;
   }

   void BtnUpdatePrices_Click (object? sender, RoutedEventArgs e) {
      var items = VM?.FilterProducts?.Where (p => p.IsModified()).ToList(); // Get modified items
   }

   #region Fields
   UpdatePricingVM? VM => DataContext as UpdatePricingVM;
   #endregion
}