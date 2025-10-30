using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class StockVM : ProductVM {

      public StockVM () { }

      public void UpdateStock () => FilterProducts?.ToList ().ForEach (item => item.Stocklevel = Get (item.Quantity).ToDisplay ());

      StockLevel Get (decimal? qty) => qty switch {
         <= 0 => StockLevel.OutOfStock,
         <= 5 => StockLevel.Low,
         <= 20 => StockLevel.Medium,
         _ => StockLevel.High
      };
   }
}
