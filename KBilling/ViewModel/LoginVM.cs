using System.Collections.Generic;
using KBilling.Model;
using KBilling.Services;
using KBilling.ViewManagement;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using KBilling.Helper;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;

namespace KBilling.ViewModel {
   public partial class LoginVM : LoginModel {

      public bool Authenticate (string? username, string? password) {
         // For demonstration purposes, we use hardcoded credentials.
         // In a real application, you would validate against a database or other secure storage.
         return (username == "Admin" || username == "Staff" || username == "Supervisor") && password == "password";
      }

      List<string> GetErrors () {
         var errors = new List<string> ();

         if (Is.IsEmpty (User)) errors.Add ("Pleaser enter user name");
         if (Is.IsEmpty (Password)) errors.Add ("Pleaser enter Password");
         if (!Authenticate (User, Password)) errors.Add ("Invalid Username or Password.");

         return errors;
      }

      bool CanLogin () {
         var errors = GetErrors ();
         if (errors.Count > 0) {
            MsgBox.ShowErrorAsync ("Error", "• " + string.Join ("\n• ", errors));
            return false;
         }
         return true;
      }

      [RelayCommand]
      public void Login (Window? win) {
         if (!CanLogin ()) return;
         AppSession.Login (new Model.UserModel { Username = User, Role = User.ToRole () });
         var mainwindow = WindowRegistry.Instance.Get ("MainWindow");
         mainwindow.Show ();
         win?.Close ();
      }
   }
}
