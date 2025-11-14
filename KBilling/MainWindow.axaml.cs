using System;
using Avalonia.Controls;
using Avalonia.Input;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;

namespace KBilling {
   public partial class MainWindow : Window {
      public MainWindow () {
         InitializeComponent ();
         Loaded += OnLoad;
         Instance = this;
      }

      void OnLoad (object? sender, Avalonia.Interactivity.RoutedEventArgs e) {
         btnClose.Click += (s, e) => Close ();
         // Default view
         var view = new ViewManager (ContentPanel);
         var isAdmin = AppSession.Role == EUserRoles.Admin;
         var isStaff = AppSession.Role == EUserRoles.Staff;
         view.ShowView (isAdmin ? "MainView" : isStaff ? "BillingView" : "MainView");
      }

      protected override void OnSizeChanged (SizeChangedEventArgs e) {
         base.OnSizeChanged (e);
         Width = 1280;
         Height = 720;
      }

      protected override void OnOpened (EventArgs e) {
         base.OnOpened (e);
         // Remove the title bar and chrome (including system buttons)
         ExtendClientAreaToDecorationsHint = true;
         ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.NoChrome;
      }


      public static MainWindow Instance { get; private set; } = null!;
   }

}