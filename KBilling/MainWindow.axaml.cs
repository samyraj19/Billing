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

         if (AppSession.Role != EUserRoles.Staff) view.ShowView ("MainView");
         else view.ShowView ("BillingView");
      }

      public static MainWindow Instance { get; private set; } = null!;
   }

}