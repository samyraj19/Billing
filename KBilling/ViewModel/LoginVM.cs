using KBilling.Model;
using KBilling.Services;

namespace KBilling.ViewModel {
   internal class LoginVM : LoginModel {

      public bool Authenticate (string username, string password) {
         // For demonstration purposes, we use hardcoded credentials.
         // In a real application, you would validate against a database or other secure storage.
         return (username == "Admin" || username == "Staff" || username == "Supervisor") && password == "password";
      }

      public EUserRoles? Role => AppSession.Role;
      public bool IsLoggedIn => AppSession.IsLoggedIn;
   }
}
