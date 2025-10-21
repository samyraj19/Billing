using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using KBilling.Model;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Collections.Specialized;
using KBilling.Services;
using System.Linq;
using System;

namespace KBilling.ViewModel {
   public class ProductVM : Products {

      public ProductVM () {
         AppSession.RoleChanged += (s, e) => OnPropertyChanged (nameof (Role));
         AllProducts = new ObservableCollection<Products> ();
         AllProducts.CollectionChanged += ProductsCollectionChanged;
         FilterProducts = new ObservableCollection<Products> ();
      }

      #region Methods
      public bool CanSubmit () {
         var errors = new List<string> ();
         if (string.IsNullOrWhiteSpace (ProductName)) errors.Add ("Product name is required.");
         if (ProductNumber is null || ProductNumber < 0) errors.Add ("ProductNumber must be > 0.");
         if (PurchaseRate is null || PurchaseRate <= 0) errors.Add ("Purchase rate must be > 0.");
         if (SellingRate is null || SellingRate <= 0) errors.Add ("Selling rate must be > 0.");
         if (Quantity is null || Quantity < 0) errors.Add ("Quantity cannot be negative.");
         if (!MinSellingRate ()) errors.Add ("Selling rate must be at least 40% higher than purchase rate.");

         if (errors.Count > 0) {
            MessageBoxManager.GetMessageBoxStandard ("Add product Error", "• " + string.Join ("\n• ", errors), ButtonEnum.Ok, Icon.Error)
                             .ShowAsync ();
            return false;
         }

         return true;
      }

      bool MinSellingRate () {
         var minsellrate = PurchaseRate + (PurchaseRate * 40 / 100);
         if (SellingRate < minsellrate) return false;
         return true;
      }

      public void AddProductToList () {
         AllProducts?.Add (new Products () {
            ProductName = this.ProductName,
            ProductNumber = this.ProductNumber,
            PurchaseRate = this.PurchaseRate,
            SellingRate = this.SellingRate,
            Quantity = this.Quantity
         });
         UpdateFilter (string.Empty);
      }

      public void UpdateFilter (string text) {
         IEnumerable<Products> filtered;
         if (string.IsNullOrEmpty (text)) filtered = AllProducts;
         else filtered = AllProducts.Where (p => p.ProductName.Contains (text, StringComparison.OrdinalIgnoreCase));

         FilterProducts.Clear ();
         foreach (var item in filtered) FilterProducts.Add (item);
      }

      void ProductsCollectionChanged (object? sender, NotifyCollectionChangedEventArgs e) {
         if (e.NewItems != null && AllProducts is not null) {
            int no = 1;
            foreach (Products item in AllProducts) item.No = no++;
         }
      }
      #endregion

      #region Fields
      readonly ObservableCollection<Products>? AllProducts;
      public ObservableCollection<Products>? FilterProducts { get; set; }
      public EUserRoles? Role => AppSession.Role;
      #endregion
   }
}
