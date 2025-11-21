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
      VM?.GetAll ();
   }

   InvoiceVM? VM => DataContext as InvoiceVM;
}