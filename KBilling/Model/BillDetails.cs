using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KBilling.Model {
   public partial class BillDetails : BaseModel {
      [ObservableProperty] int no;
      [ObservableProperty] long billId;
      [ObservableProperty] string? billNo;
      [ObservableProperty] string? productCode;
      [ObservableProperty] string? productName;
      [ObservableProperty] decimal? price;
      [ObservableProperty] int quantity;
      [ObservableProperty] int? avilableStock;
      [ObservableProperty] decimal? dbAmount;
      [ObservableProperty] bool isAdHoc;

      public decimal Amount => Price.Value * Quantity;

      partial void OnPriceChanged (decimal? value) => OnPropertyChanged (nameof (Amount));
      partial void OnQuantityChanged (int value) => OnPropertyChanged (nameof (Amount));

      public static BillDetails CreateEmpty () {
         return new BillDetails {
            ProductName = $"item",
            Quantity = 1,
            Price = 0,
            IsAdHoc = false
         };
      }
   }

   public partial class BillHeader : BaseModel {
      [ObservableProperty] long billId;
      [ObservableProperty] string? billNumber;
      [ObservableProperty] string? customerName;
      [ObservableProperty] string? customerPhone;
      [ObservableProperty] DateTime billDate;
      [ObservableProperty] decimal itemcount;
      [ObservableProperty] decimal subTotal;
      [ObservableProperty] decimal discount;
      [ObservableProperty] decimal total;
      [ObservableProperty] decimal receivedAmount;
      [ObservableProperty] decimal balanceAmount;
      [ObservableProperty] string? paymentMethod = EPaymentMode.None.Get ();
      [ObservableProperty] string? remarks;
      [ObservableProperty] string? createdBy;
      [ObservableProperty] string? createdDate;
   }
}
