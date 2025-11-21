using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KBilling.Helper;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class StockVM : ProductVM {

      public StockVM () { }

      public bool UpdateQty (IEnumerable<Product> products) {
         return Repo.Products.UpdateQty (products);
      }

      public void LoadStockData () {
         LoadData ();
         FilterProducts?.ToList ().ForEach (item => item.Stocklevel = Get (item.Quantity).ToDisplay ());
      }

      [RelayCommand]
      public void Update () {
         var items = FilterProducts?.Where (p => p.IsModifiedQty ()).ToList (); // Get modified items
         if (items is not null && UpdateQty (items)) {
            MsgBox.ShowSuccessAsync ("Success", "Quantity updated successfully.");
            LoadStockData ();
         } else MsgBox.ShowErrorAsync ("Fail", "Failed to update Quantity.");
      }

      StockLevel Get (decimal? qty) => qty switch {
         <= 0 => StockLevel.InSufficient,
         <= 5 => StockLevel.Low,
         <= 20 => StockLevel.Medium,
         _ => StockLevel.High
      };

      #region Observable Properties & Events
      [ObservableProperty] string? searchText;

      partial void OnSearchTextChanging (string? value) => UpdateFilter (value ?? string.Empty);
      #endregion
   }
}
