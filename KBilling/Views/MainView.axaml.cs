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
      if (isAdmin) {
         mViewManager.ShowView ("DashBoard");
         ToDisplayHeader ("DashBoard");
      }
      BtnPricing.IsVisible = isAdmin;
      BtnStock.IsVisible = isAdmin;
      BtnDashboard.IsVisible = isAdmin;
      BtnPos.IsVisible = isAdmin;
      RegisterEvents ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnregisterEvents ();

   void RegisterEvents () {
      BtnProduct.Click += (s, e) => ShowView ("AddProduct");
      BtnDashboard.Click += (s, e) => ShowView ("DashBoard");
      BtnPricing.Click += (s, e) => ShowView ("PriceUpdateView");
      BtnStock.Click += (s, e) => ShowView ("StocksView");
      BtnPos.Click += (s, e) => ShowView ("BillingView");
   }

   void UnregisterEvents () {
      BtnProduct.Click -= (s, e) => ShowView ("AddProduct");
      BtnDashboard.Click -= (s, e) => ShowView ("DashBoard");
      BtnPricing.Click -= (s, e) => ShowView ("PriceUpdateView");
      BtnStock.Click -= (s, e) => ShowView ("StocksView");
      BtnPos.Click -= (s, e) => ShowView ("BillingView");
   }

   void ShowView (string key) {
      mViewManager.ShowView (key);
      ToDisplayHeader (key);
   }

   void ToDisplayHeader (string key) => lblHeader.Content = AppViewHeader.Get (key);

   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}