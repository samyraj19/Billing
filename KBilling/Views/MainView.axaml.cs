using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;

namespace KBilling;
public partial class MainView : UserControl {
   public MainView () {
      InitializeComponent ();
      Loaded += OnLoad;
      Unloaded += OnUnloaded;
   }

   void OnLoad (object? sender, RoutedEventArgs e) {
      bool isAdmin = AppSession.Role == EUserRoles.Admin;
      if (isAdmin) mViewManager.ShowView ("DashBoard");
      BtnPricing.IsVisible = isAdmin;
      BtnStock.IsVisible = isAdmin;
      BtnDashboard.IsVisible = isAdmin;
      RegisterEvents ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnregisterEvents ();

   void RegisterEvents () {
      BtnProduct.Click += (s, e) => ShowView ("AddProduct");
      BtnDashboard.Click += (s, e) => ShowView ("DashBoard");
      BtnPricing.Click += (s, e) => ShowView ("PriceUpdateView");
      BtnStock.Click += (s, e) => ShowView ("StocksView");
   }

   void UnregisterEvents () {
      BtnProduct.Click -= (s, e) => ShowView ("AddProduct");
      BtnDashboard.Click -= (s, e) => ShowView ("DashBoard");
      BtnPricing.Click -= (s, e) => ShowView ("PriceUpdateView");
      BtnStock.Click -= (s, e) => ShowView ("StocksView");
   }

   void ShowView (string key) => mViewManager.ShowView (key);

   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}