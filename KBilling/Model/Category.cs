using CommunityToolkit.Mvvm.ComponentModel;

namespace KBilling.Model {
   public partial class Category : BaseModel {
      [ObservableProperty] int no;
      [ObservableProperty] int categoryId;
      [ObservableProperty] string? name;
      [ObservableProperty] string? prefix;
      [ObservableProperty] string? code;
      [ObservableProperty] string? description;
   }
}
