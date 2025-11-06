using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.ViewModel;

namespace KBilling;
public partial class ItemListDialog : Window {
   public ItemListDialog () {
      InitializeComponent ();
      Loaded += OnLoaded;
      Unloaded += OnUnloaded;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      //throw new NotImplementedException ();
   }

   void OnLoaded (object? sender, RoutedEventArgs e) {
      SearchTextbox.Focus ();
      SearchTextbox.TextChanging += OnTextChanging;
      VM ().GetAll ();
   }

   void OnTextChanging (object? sender, TextChangingEventArgs e) {
      VM ().Filter ((sender as TextBox)?.Text?.Trim () ?? string.Empty);
   }

   CategoryVM VM () => DataContext is CategoryVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type CategoryVM");

}