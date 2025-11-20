using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewModel;

namespace KBilling;
public partial class InvoiceView : UserControl {
   public InvoiceView () {
      InitializeComponent ();
   }

   protected override void OnLoaded (RoutedEventArgs e) {
      base.OnLoaded (e);

      if (DataContext is InvoiceVM vm) vm.GetAll ();
   }
}