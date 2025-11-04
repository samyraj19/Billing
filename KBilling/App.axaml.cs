using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KBilling.DataBase;
using KBilling.Interfaces;
using KBilling.ViewManagement;
using KBilling.Services;

namespace KBilling {
   public partial class App : Application {
      public override void Initialize () {
         AvaloniaXamlLoader.Load (this);
      }

      public override void OnFrameworkInitializationCompleted () {
         Db.Connect ();
         Repo = new GlobalRepo ();
         AppView.RegAll ();
         if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new Login ();
         }

         base.OnFrameworkInitializationCompleted ();
      }

      public static IGlobalRepo Repo { get; private set; }
   }
}