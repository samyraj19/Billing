using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
    public partial class Products : ObservableObject{
      [ObservableProperty] private int? no;
      [ObservableProperty] private string? productName;
      [ObservableProperty] private int? productNumber;
      [ObservableProperty] private decimal? purchaseRate;
      [ObservableProperty] private decimal? sellingRate;
      [ObservableProperty] private decimal? quantity;
      [ObservableProperty] private string? status;
      [ObservableProperty] private string? createdby;
      [ObservableProperty] private string? date;

      public void Clear () {
         ProductName = null;
         ProductNumber = null;
         PurchaseRate = null;
         SellingRate = null;
         Quantity = null;
      }
   }
}
