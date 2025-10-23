using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class BillVM : BillDetails {
      public BillVM () {
         BillItems = new ();
         BillItems.CollectionChanged += Collectionchanged;
         TestData ();
      }

      void Collectionchanged (object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
         BillHeader.SubTotal = SubTotal ();  
         BillHeader.Total = Total (BillHeader.Discount);
         BillHeader.Itemcount = BillItems.Count (x => x.BillId != null);
      }

      void TestData () {
         BillItems.Add (new Model.BillDetails () { BillId = "B001", Price = 50, ProductId = "P001", ProductName = "Product 1", Quantity = 5 });
         BillItems.Add (new Model.BillDetails () { BillId = "B002", Price = 100, ProductId = "P002", ProductName = "Product 2", Quantity = 2 });
         BillItems.Add (new Model.BillDetails () { BillId = "B003", Price = 75, ProductId = "P003", ProductName = "Product 3", Quantity = 2 });
      }

      decimal SubTotal () {
         decimal total = 0;
         foreach (var item in BillItems) total += item.Amount;
         return total;
      }

      decimal Total (decimal discount) => SubTotal () - discount;

      #region Fields
      [ObservableProperty] BillHeaderVM billHeader = new ();
      [ObservableProperty] ObservableCollection<BillDetails> billItems;
      #endregion

   }

   public class BillHeaderVM : BillHeader {
      public BillHeaderVM () { }
   }
}
