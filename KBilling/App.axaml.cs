using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using KBilling.DataBase;
using KBilling.ViewManagement;

namespace KBilling {
   public partial class App : Application {
      public override void Initialize () {
         AvaloniaXamlLoader.Load (this);
      }

      public override void OnFrameworkInitializationCompleted () {
         Db.Connect ();
         AppView.RegAll ();
         if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new Login ();
         }

         base.OnFrameworkInitializationCompleted ();
      }
   }
}