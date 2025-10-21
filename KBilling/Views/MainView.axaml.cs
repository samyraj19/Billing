using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Services;
using KBilling.ViewManagement;

namespace KBilling;
public partial class MainView : UserControl {
   public MainView () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
      RegEvents ();
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      var visible = AppSession.Role == Model.EUserRoles.Admin ? true : false;
      mViewManager.ShowView ("DashBoard");
      BtnPricing.IsVisible = visible;
      BtnStock.IsVisible = visible;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      BtnDashboard.Click -= BtnProductClick;
      BtnProduct.Click -= BtnDashboardClick;
   }

   void RegEvents () {
      BtnProduct.Click += BtnProductClick;
      BtnDashboard.Click += BtnDashboardClick;
   }

   void BtnProductClick (object? sender, RoutedEventArgs e) {
      mViewManager.ShowView ("AddProduct");
   }

   void BtnDashboardClick (object? sender, RoutedEventArgs e) {
      mViewManager.ShowView ("DashBoard");
   }

   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}