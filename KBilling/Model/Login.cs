using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class LoginModel : BaseModel {
      [ObservableProperty] private string? user;
      [ObservableProperty] private string? password = "password";
      [ObservableProperty] private bool? rememberMe;
   }
}

