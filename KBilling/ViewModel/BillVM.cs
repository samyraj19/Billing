using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class BillVM : BillDetails {
      public BillVM () {
         BillItems = new ();
         BillItems.CollectionChanged += CollectionChanged;
      }

      void CollectionChanged (object? sender, NotifyCollectionChangedEventArgs e) {
         BillHeader.SubTotal = SubTotal ();
         BillHeader.Total = Total (BillHeader.Discount);
         BillHeader.Itemcount = BillItems.Count (x => x.BillId != null);
      }

      public void AddItem (Product product) {
         BillItems.Add (new BillDetails {
            BillId = BillHeader.BillId,
            ProductId = product.ProductNumber,
            ProductName = product.ProductName,
            Price = product.SellingRate,
         });
      }

      public void ClearAll () => BillItems.Clear ();

      public decimal SubTotal () => BillItems.Sum (item => item.Amount);

      public decimal Total (decimal discount) => SubTotal () - discount;

      public decimal UpdateTotal () => SubTotal () - BillHeader.Discount;

      public void ResetBill () {
         BillHeader = new ();
         BillItems.Clear ();
      }

      #region Fields
      [ObservableProperty] BillHeaderVM billHeader = new ();
      [ObservableProperty] ObservableCollection<BillDetails> billItems;
      #endregion

   }

   public class BillHeaderVM : BillHeader {
      public BillHeaderVM () { }
   }
}
