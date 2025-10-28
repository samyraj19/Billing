using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace KBilling.Helper {
   public class MsgBox {
      public static Task<ButtonResult> ShowInfoAsync (string title, string message) =>
            MessageBoxManager
                .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Info)
                .ShowAsync ();

      public static Task<ButtonResult> ShowWarningAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Warning)
              .ShowAsync ();

      public static Task<ButtonResult> ShowErrorAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Error)
              .ShowAsync ();

      public static Task<ButtonResult> ShowConfirmAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.YesNo, Icon.Question)
              .ShowAsync ();
   }

   public static class AppMsg {
      public static Task<ButtonResult> AskItem () => MsgBox.ShowErrorAsync ("Select Item", "Please select an item.");

      public static Task<ButtonResult> AskDelItem () => MsgBox.ShowConfirmAsync ("Delete Item", "Are you sure you want to delete this item?");
   }
}
