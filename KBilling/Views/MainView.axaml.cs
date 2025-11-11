using System;
using System.Linq;
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
      mViewManager.ShowView (isAdmin ? "DashBoard" : "CategoryView");
      BtnPricing.IsVisible = isAdmin;
      BtnStock.IsVisible = isAdmin;
      BtnDashboard.IsVisible = isAdmin;
      BtnPos.IsVisible = isAdmin;
      RegEvents ();
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnRegEvents ();

   void NavButtonClick (object? sender, RoutedEventArgs e) {
      if (sender is Button btn && btn.Tag is string key)
         ShowView (key);
   }

   void RegEvents () {
      foreach (Button btn in NavPanel.Children.OfType<Button> ())
         btn.Click += NavButtonClick;
   }

   void UnRegEvents () {
      foreach (Button btn in NavPanel.Children.OfType<Button> ())
         btn.Click -= NavButtonClick;
   }

   void ShowView (string key) {
      mViewManager.ShowView (key);
      ToDisplay (key);
   }

   void ToDisplay (string key) => lblHeader.Content = AppViewHeader.Get (key);

   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}