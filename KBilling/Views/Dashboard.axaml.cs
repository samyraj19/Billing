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
      var visible = AppSession.Role == Model.EUserRoles.Admin ? true : false;

      BtnPricing.IsVisible = visible;
      BtnStock.IsVisible = visible;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) {
      BtnProduct.Click -= BtnProductClick;
   }

   void RegEvents () {
      BtnProduct.Click += BtnProductClick;
   }

   void BtnProductClick (object? sender, RoutedEventArgs e) {
      mViewManager.ShowView ("AddProduct");
   }

   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}