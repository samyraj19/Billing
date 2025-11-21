using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Core;
using KBilling.Extension;
using KBilling.Helper;
using KBilling.Interfaces;
using KBilling.Model;
using KBilling.Services;
using MsBox.Avalonia.Enums;
using Tmds.DBus.Protocol;

namespace KBilling.ViewModel {
   public partial class BillVM : BillDetails {
      public BillVM () {
         BillItems = new ();
         BillItems.CollectionChanged += (_, __) => UpdateBill ();
      }

      public bool AddItem (Product product) {
         if (BillItems is null || BillHeader is null || product is null) return false; // Ensure BillHeader is not null  
         if (BillItems.Any (b => b.ProductCode == product.ProductNumber)) return false;
         BillItems.Add (new BillDetails {
            BillId = BillHeader.BillId, // Safe to access as BillHeader is checked for null  
            ProductCode = product.ProductNumber,
            ProductName = product.ProductName,
            Price = product.SellingRate,
            AvilableStock = product.Quantity
         });
         return true;
      }


      public void UpdateBill () {
         if (BillHeader == null || BillItems == null) return;

         decimal discountValue = Discount ?? BillHeader.Discount;
         BillHeader.Discount = discountValue;
         BillHeader.Itemcount = BillItems.Count;
         BillHeader.SubTotal = BillItems.Sum (item => item.Amount);
         BillHeader.Total = BillHeader.SubTotal - discountValue;
         BillHeader.ReceivedAmount = BillHeader.Total;
      }

      public bool CanPay () {
         if (BillItems is null || BillHeader is null) return false; // Ensure BillHeader is not null

         var errors = GetValidationErrors ();
         if (errors.Count > 0) {
            var message = "• " + string.Join ("\n• ", errors);
            MsgBox.ShowErrorAsync ("Error", message);
            return false;
         }

         BillHeader.CreatedDate = DateTime.Now.ToSql (); // Safe to access as BillHeader is checked for null
         BillHeader.CreatedBy = AppSession.CurrentUser?.Username;
         return Repo.Bills.Insert (BillHeader, BillItems);
      }

      /// <summary>Returns a list of validation error messages for the current bill.</summary>
      List<string> GetValidationErrors () {
         var errors = new List<string> ();

         if (BillItems is null || BillItems.Count == 0)
            errors.Add ("Add at least one item to the bill.");
         if (BillItems?.Any (item => item.Quantity <= 0) == true)
            errors.Add ("All items must have quantity greater than zero.");
         if (BillHeader?.Total <= 0)
            errors.Add ("Total amount must be greater than zero.");
         if (BillHeader?.PaymentMethod == EPaymentMode.None.Get ())
            errors.Add ("Select a payment method.");

         return errors;
      }

      public void Reset () {
         BillHeader = new ();
         BillItems?.Clear ();
      }

      #region Fields
      [ObservableProperty] BillHeaderVM? billHeader = new ();
      [ObservableProperty] AutoNumberedCollection<BillDetails>? billItems;

      [ObservableProperty] decimal? discount;
      #endregion

   }

   public class BillHeaderVM : BillHeader {
      public BillHeaderVM () { }
   }
}
