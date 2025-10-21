using System;

namespace KBilling.Model {
   public enum EAction {
      Edit,
      Delete,
   }

   public static class EActionExtensions {
      public static bool IsEdit (this string action) => Enum.TryParse<EAction> (action, out var result) && result == EAction.Edit;
      public static bool IsDelete (this string action) => Enum.TryParse<EAction> (action, out var result) && result == EAction.Delete;
   }
}
