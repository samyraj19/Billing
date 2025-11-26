using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KBilling.Controls;
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
      ShowView (isAdmin ? "DashBoard" : "CategoryView");
      DefaultSelection (isAdmin);
      BtnPricing.IsVisible = isAdmin;
      BtnStock.IsVisible = isAdmin;
      BtnDashboard.IsVisible = isAdmin;
      BtnPos.IsVisible = isAdmin;
      btnInvoice.IsVisible = isAdmin;
      RegEvents ();
      string? user = AppSession.CurrentUser?.Username ?? "Guest";
      lblUser.Content = user;
      UserShortText.Text = user[..1].ToUpper ();
      BtnDashboard.IsSelected = isAdmin;
      BtnCategory.IsSelected = !isAdmin;
   }

   void OnUnloaded (object? sender, RoutedEventArgs e) => UnRegEvents ();

   void NavButtonClick (object? sender, RoutedEventArgs e) {
      if (sender is Button btn && btn.Tag is string key)
         ShowView (key);

      foreach (LeftMenubarButton b in NavPanel.Children.OfType<LeftMenubarButton> ()) {
         b.IsSelected = b == sender;
         b.UpdateIcon ();
      }
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
      ToggleHeader (key != "DashBoard");
   }

   void ToDisplay (string key) => lblHeader.Content = AppViewHeader.Get (key);

   void ToggleHeader (bool visible) {
      GirdContentPanel.RowDefinitions[0].Height = visible
       ? new GridLength (5, GridUnitType.Star)
       : new GridLength (0);
   }

   void DefaultSelection (bool admin) {
      var btn = admin ? BtnDashboard : BtnCategory;
      btn.IsSelected = true;
      btn.UpdateIcon ();
   }


   #region Fields
   ViewManager mViewManager => new (ContentPanel);
   #endregion
}