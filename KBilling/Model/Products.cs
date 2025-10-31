﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Extension;
using KBilling.ViewModel;

namespace KBilling.Model {
   public partial class Product : ObservableObject {
      [ObservableProperty] int? no;
      [ObservableProperty] string? productName;
      [ObservableProperty] int? productNumber;
      [ObservableProperty] decimal? purchaseRate;
      [ObservableProperty] decimal? sellingRate;
      [ObservableProperty] int? quantity;
      [ObservableProperty] string? status;
      [ObservableProperty] string? stocklevel;
      [ObservableProperty] bool? isActive;
      [ObservableProperty] string? createdby;
      [ObservableProperty] string? createddate = DateTime.Now.ToSql();
      [ObservableProperty] string? modifiedby;
      [ObservableProperty] string? modifieddate = DateTime.Now.ToSql ();

      Product? mOriginal;

      public void Clear () {
         ProductName = null;
         ProductNumber = null;
         PurchaseRate = null;
         SellingRate = null;
         Quantity = null;
      }

      public void SnapShot () => mOriginal = (Product)this.MemberwiseClone ();

      public bool IsModified () => mOriginal != null && (PurchaseRate != mOriginal.PurchaseRate || SellingRate != mOriginal.SellingRate);
   }

   public static class ProductExtensions {
      public static bool IsProductClass (this object obj) => obj is Product;
   }
}
