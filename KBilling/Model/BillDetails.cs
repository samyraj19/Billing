using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class BillDetails : ObservableObject{
      [ObservableProperty] int no;
      [ObservableProperty] string? billId;
      [ObservableProperty] string? productId;
      [ObservableProperty] string? productName;
      [ObservableProperty] decimal price;
      [ObservableProperty] int quantity;

      public decimal Amount => Price * Quantity;

      partial void OnPriceChanged (decimal value) => OnPropertyChanged (nameof (Amount));
      partial void OnQuantityChanged (int value) => OnPropertyChanged (nameof (Amount));
   }

   public partial class BillHeader : ObservableObject {
      [ObservableProperty] string? billId;
      [ObservableProperty] string? customerName;
      [ObservableProperty] string? customerPhone;
      [ObservableProperty] DateTime billDate;
      [ObservableProperty] decimal itemcount;
      [ObservableProperty] decimal subTotal;
      [ObservableProperty] decimal discount;
      [ObservableProperty] decimal total;
   }
}
