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
using static System.Collections.Specialized.BitVector32;

namespace KBilling.ViewModel {
   public class ProductVM : Product {

      public ProductVM () {
         AppSession.RoleChanged += (s, e) => OnPropertyChanged (nameof (Role));
         AllProducts = new ObservableCollection<Product> ();
         FilterProducts = new ObservableCollection<Product> ();
         FilterProducts.CollectionChanged += ProductsCollectionChanged;
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

      public void Edit (Product product) {
         ProductName = product.ProductName;
         ProductNumber = product.ProductNumber;
         PurchaseRate = product.PurchaseRate;
         SellingRate = product.SellingRate;
         Quantity = product.Quantity;
      }

      public void AddOrUpdateProductInList (Product product,EAction action) {
         var existingProduct = AllProducts.FirstOrDefault (p => p.ProductNumber == product.ProductNumber);
         if (existingProduct is not null && action.IsEdit()) UpdateItem (product, existingProduct);
         else AddItem (product);
         UpdateFilter (string.Empty);
      }

      void AddItem (Product product) {
         if (AllProducts.Any (p => p.ProductNumber == product.ProductNumber)) {
            MessageBoxManager.GetMessageBoxStandard ("Add product Error", "Product number already there. Please choose a different number", ButtonEnum.Ok, Icon.Error)
                             .ShowAsync ();
            return;
         }
         AllProducts?.Add (new () {
            ProductName = product.ProductName,
            ProductNumber = product.ProductNumber,
            PurchaseRate = product.PurchaseRate,
            SellingRate = product.SellingRate,
            Quantity = product.Quantity
         });
         MessageBoxManager.GetMessageBoxStandard ("Add product", "Product added successfully.", ButtonEnum.Ok, Icon.Success).ShowAsync ();
      }

      void UpdateItem (Product current, Product existing) {
         existing.ProductName = current.ProductName;
         existing.ProductNumber = current.ProductNumber;
         existing.PurchaseRate = current.PurchaseRate;
         existing.SellingRate = current.SellingRate;
         existing.Quantity = current.Quantity;
         MessageBoxManager.GetMessageBoxStandard ("Add product", "Product updated successfully.", ButtonEnum.Ok, Icon.Success).ShowAsync ();
      }

      public void DeleteItem (Product item) {
         AllProducts?.Remove (item);
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
      #endregion

      #region Fields
      readonly ObservableCollection<Product>? AllProducts;
      public ObservableCollection<Product>? FilterProducts { get; set; }
      public EUserRoles? Role => AppSession.Role;
      #endregion
   }
}
