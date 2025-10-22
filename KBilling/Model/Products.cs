using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class Product : ObservableObject {
      [ObservableProperty] private int? no;
      [ObservableProperty] private string? productName;
      [ObservableProperty] private int? productNumber;
      [ObservableProperty] private decimal? purchaseRate;
      [ObservableProperty] private decimal? sellingRate;
      [ObservableProperty] private decimal? quantity;
      [ObservableProperty] private string? status;
      [ObservableProperty] private string? createdby;
      [ObservableProperty] private string? createddate;
      [ObservableProperty] private string? modifiedby;
      [ObservableProperty] private string? modifieddate;

      Product? mOriginal;

      public void Clear () {
         ProductName = null;
         ProductNumber = null;
         PurchaseRate = null;
         SellingRate = null;
         Quantity = null;
      }

      public void SnapShot () => mOriginal = (Product)this.MemberwiseClone ();

      public bool IsModified () => mOriginal != null && (PurchaseRate != mOriginal.PurchaseRate || SellingRate != mOriginal.SellingRate);
   }

   public static class ProductExtensions {
      public static bool IsProductClass (this object obj) => obj is Product;
   }
}
