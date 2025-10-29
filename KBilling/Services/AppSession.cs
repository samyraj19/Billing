using System;
using KBilling.Model;

namespace KBilling.Services {
   public static class AppSession {
      private static UserModel? mCurrentUser;

      public static UserModel? CurrentUser {
         get => mCurrentUser;
         private set {
            mCurrentUser = value;
            RoleChanged?.Invoke (null, EventArgs.Empty); // notify everyone
         }
      }
      public static string? HeaderTitle;
      public static bool IsLoggedIn => CurrentUser != null;
      public static EUserRoles? Role => CurrentUser?.Role;

      public static void Login (UserModel user) => CurrentUser = user;
      public static void Logout () => CurrentUser = null;

      // Event triggered when role changes
      public static event EventHandler? RoleChanged;
   }
}
