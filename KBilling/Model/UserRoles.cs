using System;

namespace KBilling.Model {
   public enum EUserRoles {
      Admin = 0,
      Supervisor = 1,
      Staff = 2,
      None = 3
   }

   public static class EUserRolesEx {
      public static EUserRoles ToRole (this string user) {
         if (string.IsNullOrWhiteSpace (user)) throw new ArgumentException ("Username cannot be empty");

         return user switch {
            "Admin" => EUserRoles.Admin,
            "Supervisor" => EUserRoles.Supervisor,
            "Staff" => EUserRoles.Staff,
            _ => EUserRoles.None // default role
         };
      }
   }
}
