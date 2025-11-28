using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KBilling.Extension;
using KBilling.Helper;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class StockVM : ProductVM {

      public StockVM () { }

      public bool UpdateQty (IEnumerable<Product> products) => Repo.Products.UpdateQty (products);

      public void LoadStockData () {
         LoadData ();
         FilterProducts?.ToList ().ForEach (item => item.Stocklevel = Get (item.Quantity).ToText ());
      }

      [RelayCommand]
      public void Update () {
         var items = FilterProducts?.Where (p => p.HasQtyChanges ()).ToList (); // Get modified items
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

      public Func<StockLevel, string> Display => x => x.ToText ();

      public Array StockLevels { get; } = Enum.GetValues (typeof (StockLevel));

      #region Observable Properties & Events
      [ObservableProperty] string? searchText;
      [ObservableProperty] StockLevel selectedLevel;

      partial void OnSearchTextChanging (string? value) => UpdateFilter (value ?? string.Empty);

      partial void OnSelectedLevelChanged (StockLevel value) {
         var text = value.ToText () == StockLevel.All.ToText() ? string.Empty : value.ToText ();
         StockFilter (text);
      }
      #endregion
   }
}
