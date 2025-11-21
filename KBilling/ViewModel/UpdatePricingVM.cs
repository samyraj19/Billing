using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KBilling.Helper;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class UpdatePricingVM : ProductVM {
      public UpdatePricingVM () { }

      public bool DBUpdate (IEnumerable<Product> products) {
         return Repo.Products.UpdatePrices (products);
      }

      [RelayCommand]
      public void Update () {
         var items = FilterProducts?.Where (p => p.IsModified ()).ToList (); // Get modified items
         if (items is null) return;
         if (DBUpdate (items)) {
            MsgBox.ShowSuccessAsync ("Success", "Prices updated successfully.");
            LoadData ();
         } else MsgBox.ShowErrorAsync ("Fail", "Failed to update prices.");
      }



      #region ----- Observable Properties & Events -----
      [ObservableProperty] string? searchText;

      partial void OnSearchTextChanging (string? value) => UpdateFilter (value ?? string.Empty);
      #endregion
   }
}
