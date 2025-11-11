using Avalonia.Controls;
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
         // Default view
         var view = new ViewManager (ContentPanel);
         var isAdmin = AppSession.Role == EUserRoles.Admin;
         view.ShowView (isAdmin ? "MainView" : "BillingView");
      }

      public static MainWindow Instance { get; private set; } = null!;
   }

}