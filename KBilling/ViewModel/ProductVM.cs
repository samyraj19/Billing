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
using KBilling.Helper;
using KBilling.Extension;

namespace KBilling.ViewModel {
   public class ProductVM : Product {

      public ProductVM () {
         AppSession.RoleChanged += (s, e) => OnPropertyChanged (nameof (Role));
         FilterProducts.CollectionChanged += RenumberProducts;
         TestData ();
      }

      #region Methods

      void TestData () {
         var products = new[]
         {
            new Product { ProductName = "Product A", ProductNumber = 101, PurchaseRate = 100, SellingRate = 150, Quantity = 50 },
            new Product { ProductName = "Product B", ProductNumber = 102, PurchaseRate = 200, SellingRate = 300, Quantity = 30 },
            new Product { ProductName = "Product C", ProductNumber = 103, PurchaseRate = 150, SellingRate = 225, Quantity = 20 },
            new Product { ProductName = "Product D", ProductNumber = 104, PurchaseRate = 250, SellingRate = 375, Quantity = 10 },
        };

         AllProducts?.AddRange (products);
         UpdateFilter (string.Empty);
      }

      public bool CanSubmit () {
         var errors = new List<string> ();
         if (!Is.NotEmpty (ProductName)) errors.Add ("Product name is required.");
         if (ProductNumber is null || ProductNumber < 0) errors.Add ("ProductNumber must be > 0.");
         if (AppSession.Role == EUserRoles.Admin) {
            if (PurchaseRate is null || PurchaseRate <= 0) errors.Add ("Purchase rate must be > 0.");
            if (SellingRate is null || SellingRate <= 0) errors.Add ("Selling rate must be > 0.");
            if (!IsSellingRateValid ()) errors.Add ("Selling rate must be at least 40% higher than purchase rate.");
         }
         if (Quantity is null || Quantity < 0) errors.Add ("Quantity cannot be negative.");

         if (errors.Count > 0) {
            MsgBox.ShowErrorAsync ("Add product Error", "• " + string.Join ("\n• ", errors));
            return false;
         }
         return true;
      }

      bool IsSellingRateValid () => SellingRate >= PurchaseRate + (PurchaseRate * 0.4m);

      public void Edit (Product product) {
         ProductName = product.ProductName;
         ProductNumber = product.ProductNumber;
         if (AppSession.Role == EUserRoles.Admin) {
            PurchaseRate = product.PurchaseRate;
            SellingRate = product.SellingRate;
         }
         Quantity = product.Quantity;
      }

      public void AddOrUpdate (Product product, int code) {
         var existing = AllProducts?.SingleOrDefault (p => p.ProductNumber == code);
         if (existing is not null) UpdateItem (product, existing);
         else AddItem (product);
         UpdateFilter (string.Empty);
      }

      void AddItem (Product product) {
         if (AllProducts is not null && AllProducts.Any (p => p.ProductNumber == product.ProductNumber)) {
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
         if (AllProducts is null) return;
         string filterValue = text?.Trim () ?? string.Empty;

         FilterProducts?.Filter (AllProducts, p => string.IsNullOrEmpty (filterValue) ||
                         p.ProductName.Contains (filterValue, StringComparison.OrdinalIgnoreCase) ||
                         p.ProductNumber.ToString ().Contains (filterValue, StringComparison.OrdinalIgnoreCase));
      }

      void RenumberProducts (object? sender, NotifyCollectionChangedEventArgs e) {
         int no = 1;
         if (AllProducts is null) return;
         foreach (Product item in AllProducts) item.No = no++;
      }

      public void Refersh () => UpdateFilter (string.Empty);
      #endregion

      #region Fields
      ObservableCollection<Product>? AllProducts { get; } = new ();
      public ObservableCollection<Product>? FilterProducts { get; } = new ();
      public EUserRoles? Role => AppSession.Role;
      #endregion
   }
}
