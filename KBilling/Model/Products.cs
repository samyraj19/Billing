using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.ViewModel;

namespace KBilling.Model {
   public partial class Product : ObservableObject {
      [ObservableProperty] int? no;
      [ObservableProperty] string? productName;
      [ObservableProperty] int? productNumber;
      [ObservableProperty] decimal? purchaseRate;
      [ObservableProperty] decimal? sellingRate;
      [ObservableProperty] decimal? quantity;
      [ObservableProperty] string? status;
      [ObservableProperty] string? stocklevel;
      [ObservableProperty] string? createdby;
      [ObservableProperty] string? createddate;
      [ObservableProperty] string? modifiedby;
      [ObservableProperty] string? modifieddate;

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
