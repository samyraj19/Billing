using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class DashboardModel : BaseModel {
      [ObservableProperty] string? totalItems;
      [ObservableProperty] string? totalBills;
      [ObservableProperty] string? salesAmount;
      [ObservableProperty] string? totalProfit;
   }

   public partial class LastestTransaction : BaseModel {
      [ObservableProperty] int no;
      [ObservableProperty] string? billNumber;
      [ObservableProperty] string? date;
      [ObservableProperty] decimal amount;
      [ObservableProperty] decimal receivedAmount;
      [ObservableProperty] decimal balanceAmount;
      [ObservableProperty] string? payment;
   }

   public partial class TopSellingItems : BaseModel{
      [ObservableProperty] string? itemCode;
      [ObservableProperty] string? itemName;
      [ObservableProperty] int quanityt;
      [ObservableProperty] decimal amount;
   }
}
