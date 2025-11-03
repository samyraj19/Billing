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

namespace KBilling.ViewModel {
   public partial class BillVM : BillDetails {
      public BillVM () {
         BillItems = new ();
         BillItems.CollectionChanged += CollectionChanged;
      }

      void CollectionChanged (object? sender, NotifyCollectionChangedEventArgs e) {
         for (int i = 0; i < BillItems?.Count; i++) BillItems[i].No = i + 1;
         UpdateBill ();
      }

      public void AddItem (Product product) {
         BillItems.Add (new BillDetails {
            BillId = BillHeader.BillId,
            ProductId = product.ProductNumber,
            ProductName = product.ProductName,
            Price = product.SellingRate,
         });
      }

      public void UpdateBill (decimal? discount = null) {
         if(BillHeader == null || BillItems == null) return;
         BillHeader.Discount = discount.HasValue ? discount.Value : BillHeader.Discount;
         BillHeader.Itemcount = BillItems.Count;
         BillHeader.SubTotal = BillItems.Sum (item => item.Amount);
         BillHeader.Total = BillHeader.SubTotal - BillHeader.Discount;
         BillHeader.ReceivedAmount = BillHeader.Total;
      }

      public bool CanPay () {
         var errors = new List<string> ();
         if (BillItems.Count == 0) errors.Add ("Add at least one item to the bill.");
         if (BillItems.Any (item => item.Quantity <= 0)) errors.Add ("All items must have quantity greater than zero.");
         if (BillHeader.PaymentMethod == EPaymentMode.None.Get ()) errors.Add ("Select a payment method.");
         if (errors.Count > 0) {
            var message = "• " + string.Join ("\n• ", errors);
            var msgBox = MsgBox.ShowErrorAsync ("Error", message);
            return false;
         }
         BillHeader.CreatedDate = DateTime.Now.ToSql ();
         BillHeader.CreatedBy = AppSession.CurrentUser?.Username;
         return Repo.Bills.Insert (BillHeader,billItems);
      }

      public void ResetBill () {
         BillHeader = new ();
         BillItems.Clear ();
      }

      #region Fields
      [ObservableProperty] BillHeaderVM billHeader = new ();
      [ObservableProperty] BillCollection<BillDetails> billItems;
      #endregion

   }

   public class BillHeaderVM : BillHeader {
      public BillHeaderVM () { }
   }
}
