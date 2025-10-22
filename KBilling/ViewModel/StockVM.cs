using System.Collections.ObjectModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class StockVM : Stock {
      public StockVM () { 
         TestData (); 
      }

      void TestData () {
         Stocks.Add (new Stock { ProductName = "Test Product 1", ProductNumber = 1001, Quantity = 50 });
         Stocks.Add (new Stock { ProductName = "Test Product 2", ProductNumber = 1002, Quantity = 30 });
         Stocks.Add (new Stock { ProductName = "Test Product 3", ProductNumber = 1003, Quantity = 20 });
      }

      #region Fields
      public ObservableCollection<Stock> Stocks { get; set; } = [];
      #endregion
   }
}
