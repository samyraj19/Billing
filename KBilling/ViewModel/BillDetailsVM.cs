using System.Collections.ObjectModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class BillDetailsVM : BillDetails {
      public BillDetailsVM () {
         TestData ();
      }

      void TestData () {
         BillDetails.Add (new Model.BillDetails () { Amount = 100, BillId = "B001", Price = 50, ProductId = "P001", ProductName = "Product 1", Quantity = 2 });
         BillDetails.Add (new Model.BillDetails () { Amount = 200, BillId = "B002", Price = 100, ProductId = "P002", ProductName = "Product 2", Quantity = 2 });
         BillDetails.Add (new Model.BillDetails () { Amount = 150, BillId = "B003", Price = 75, ProductId = "P003", ProductName = "Product 3", Quantity = 2 });
      }

      #region Fields
      public ObservableCollection<BillDetails> BillDetails { get; set; } = [];
      #endregion

   }
}
