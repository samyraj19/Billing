using System;

namespace KBilling.Model {
   public enum EAction {
      None,
      Edit,
      Delete,
   }

   public static class EActionExtensions {
      public static EAction GetEAction (this string action) => Enum.TryParse<EAction> (action, out var result) ? result : EAction.None;
      public static bool IsEdit (this string action) => Enum.TryParse<EAction> (action, out var result) && result == EAction.Edit;
      public static bool IsDelete (this string action) => Enum.TryParse<EAction> (action, out var result) && result == EAction.Delete;

      public static bool IsEdit (this EAction action) => action == EAction.Edit;
      public static bool IsDelete (this EAction action) => action == EAction.Delete;
   }
}
