using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Services;
using KBilling.ViewManagement;

namespace KBilling;
public partial class Dashboard : UserControl {
   public Dashboard () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
      RegEvents ();
   }

   void OnLoad (object? sender, RoutedEventArgs e) {

   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {

   }

   void RegEvents () {
     
   }

   #region Fields
   #endregion
}