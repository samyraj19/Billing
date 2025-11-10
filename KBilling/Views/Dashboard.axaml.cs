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
      RegisterToggleEvents ();
      DefaultSelection ();
      VM ()?.Sales?.LoadReport (mType);
      VM ()?.Transaction?.LoadTransaction ();
      VM ()?.TopSelling?.LoadTopSellingItems (mType);
      VM ()?.StockReport?.LoadStockRpt ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnregisterToggleEvents ();

   void OnToggleClicked (object? sender, RoutedEventArgs e) {
      if (sender is not ToggleButton btn || btn.Tag is not EReportType type) return;

      mType = type;
      VM ()?.Sales?.LoadReport (mType);
      VM ()?.TopSelling?.LoadTopSellingItems (mType);

      TogglePanel.Children.OfType<ToggleButton> ().Where (t => t != btn).ToList ().ForEach (t => t.IsChecked = false);
   }

   void RegisterToggleEvents () {
      foreach (var toggle in TogglePanel.Children.OfType<ToggleButton> ())
         toggle.Click += OnToggleClicked;
   }

   void UnregisterToggleEvents () {
      foreach (var toggle in TogglePanel.Children.OfType<ToggleButton> ())
         toggle.Click -= OnToggleClicked;
   }

   void DefaultSelection () {
      if (TogglePanel.Children.Count > 0 && TogglePanel.Children[0] is ToggleButton btn)
         btn.IsChecked = true;
   }

   DashboardVM VM () => DataContext is DashboardVM vm ? vm : throw new InvalidOperationException ("DataContext is not of type SalesVM");

   #region Fields
   EReportType mType = EReportType.Today;
   #endregion
}