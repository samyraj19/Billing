using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class UpdatePricingVM {

      public UpdatePricingVM () {
         FilterProducts.CollectionChanged += ProductsCollectionChanged;
         AddTempData ();
      }

      void AddTempData () {
         AllProducts.Add (new Product { ProductName = "Sample Product 1", ProductNumber = 101, PurchaseRate = 50, SellingRate = 80, Quantity = 100 });
         AllProducts.Add (new Product { ProductName = "Sample Product 2", ProductNumber = 102, PurchaseRate = 30, SellingRate = 50, Quantity = 200 });
         AllProducts.Add (new Product { ProductName = "Sample Product 3", ProductNumber = 103, PurchaseRate = 20, SellingRate = 35, Quantity = 150 });

         // Take snapshot
         foreach (var item in AllProducts) item.SnapShot ();
         UpdateFilter (string.Empty);
      }

      public void UpdateFilter (string text) {
         IEnumerable<Product> filtered;
         if (string.IsNullOrEmpty (text)) filtered = AllProducts;
         else filtered = AllProducts.Where (p => p.ProductName.Contains (text, StringComparison.OrdinalIgnoreCase));

         FilterProducts.Clear ();
         foreach (var item in filtered) FilterProducts.Add (item);
      }

      void ProductsCollectionChanged (object? sender, NotifyCollectionChangedEventArgs e) {
         if (e.NewItems != null && AllProducts is not null) {
            int no = 1;
            foreach (Product item in AllProducts) item.No = no++;
         }
      }

      #region Fields
      readonly ObservableCollection<Product> AllProducts = [];
      public ObservableCollection<Product> FilterProducts { get; set; } = [];
      #endregion
   }
}
