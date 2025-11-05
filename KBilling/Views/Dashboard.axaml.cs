using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;
using KBilling.ViewModel;

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
      VM ()?.Sales?.LoadReport (mType);
      VM ()?.Transaction?.LoadTransaction ();
      VM ()?.TopSelling?.LoadTopSellingItems (mType);
      VM ()?.StockReport?.LoadStockRpt ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      TogglePanel.Children.OfType<ToggleButton> ().ToList ().ForEach (toggle => toggle.Click -= OnToggleClicked);
   }

   void OnToggleClicked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton btn) return;
      TogglePanel.Children.OfType<ToggleButton> ().Where (t => t != btn).ToList ().ForEach (t => t.IsChecked = false);
      if (btn.Tag is EReportType type) {
         mType = type;
         VM ()?.Sales?.LoadReport (mType);
         VM ()?.TopSelling?.LoadTopSellingItems (mType);
      }
   }

   void RegEvents () {
      TogglePanel.Children.OfType<ToggleButton> ().ToList ().ForEach (toggle => toggle.Click += OnToggleClicked);
   }

   void DefaultSelection () {
      if (TogglePanel.Children[0] is ToggleButton btn) btn.IsChecked = true;
   }

   DashboardVM VM () => DataContext is DashboardVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type SalesVM");

   #region Fields
   EReportType mType = EReportType.Daily;
   #endregion
}