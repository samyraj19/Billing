namespace KBilling.Model {
   public class UserModel {
      public string? Username { get; set; } = string.Empty;
      public EUserRoles? Role { get; set; } = EUserRoles.Admin; // e.g. "Admin", "Staff", "Supervisor"
   }
}
