using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class Stock : ObservableObject {
      [ObservableProperty] int? no;
      [ObservableProperty] string? productName;
      [ObservableProperty] int? productNumber;
      [ObservableProperty] decimal? sellingrate;
      [ObservableProperty] decimal? quantity;
      [ObservableProperty] string? status;
      [ObservableProperty] string? stocklevel;
   }
}
