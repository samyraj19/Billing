using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class BillDetails : ObservableObject{
      [ObservableProperty] string? billId;
      [ObservableProperty] string? productId;
      [ObservableProperty] string? productName;
      [ObservableProperty] decimal price;
      [ObservableProperty] int quantity;
      [ObservableProperty] decimal amount;
   }
}
