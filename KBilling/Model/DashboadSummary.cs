using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class SalesSummary : BaseModel {
      [ObservableProperty] string? totalItems;
      [ObservableProperty] string? totalBills;
      [ObservableProperty] string? salesAmount;
      [ObservableProperty] string? totalProfit;
   }

   public partial class LastestTransaction : BaseModel {
      [ObservableProperty] int no;
      [ObservableProperty] string? billNumber;
      [ObservableProperty] string? date;
      [ObservableProperty] string? amount;
      [ObservableProperty] string? receivedAmount;
      [ObservableProperty] string? balanceAmount;
      [ObservableProperty] string? payment;
   }

   public partial class TopSellingItems : BaseModel {
      [ObservableProperty] string? itemCode;
      [ObservableProperty] string? itemName;
      [ObservableProperty] string? quantity;
      [ObservableProperty] string? amount;
   }

   public partial class StockRepot : BaseModel {
      [ObservableProperty] string? itemCode;
      [ObservableProperty] string? itemName;
      [ObservableProperty] string? stockQuantity;
      [ObservableProperty] string? status;
   }
}
