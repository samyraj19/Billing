using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;
using Avalonia;

namespace KBilling.Helper {
   public static class MsgBox {

      public static Task<ButtonResult> ShowInfoAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Info)
              .ShowWindowDialogAsync (MainWindow.Instance);

      public static Task<ButtonResult> ShowWarningAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Warning)
              .ShowWindowDialogAsync (MainWindow.Instance);

      public static Task<ButtonResult> ShowErrorAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Error)
              .ShowWindowDialogAsync (MainWindow.Instance);

      public static Task<ButtonResult> ShowConfirmAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.YesNo, Icon.Question)
              .ShowWindowDialogAsync (MainWindow.Instance);

      public static Task<ButtonResult> ShowSuccessAsync (string title, string message) =>
          MessageBoxManager
              .GetMessageBoxStandard (title, message, ButtonEnum.Ok, Icon.Success)
              .ShowWindowDialogAsync (MainWindow.Instance);
   }

   public static class AppMsg {
      public static Task<ButtonResult> AskItem () => MsgBox.ShowErrorAsync ("Select Item", "Please select an item.");

      public static Task<ButtonResult> AskDelItem () => MsgBox.ShowConfirmAsync ("Delete Item", "Are you sure you want to delete this item?");

      public static Task<ButtonResult> AlreadyExists () => MsgBox.ShowErrorAsync ("Duplicate", "This item already exists in the collection.");
   }
}
