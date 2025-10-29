using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
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
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      RegEvents ();
      DefaultSelection ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {

   }

   void OnToggleClicked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton btn) return;
      TogglePanel.Children.OfType<ToggleButton> ().Where (t => t != btn).ToList ().ForEach (t => t.IsChecked = false);
   }

   void RegEvents () {
      TogglePanel.Children.OfType<ToggleButton> ().ToList ().ForEach (toggle => toggle.Click += OnToggleClicked);
   }

   void DefaultSelection () {
      if(TogglePanel.Children[0] is ToggleButton btn) btn.IsChecked = true;
   }

   #region Fields
   #endregion
}