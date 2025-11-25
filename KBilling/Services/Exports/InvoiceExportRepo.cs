using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KBilling.Interfaces;
using KBilling.Model;
using KBilling.ViewModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace KBilling.Services;
public class InvoiceExportRepo : IExport {
   public InvoiceExportRepo () => QuestPDF.Settings.License = LicenseType.Community;
   public byte[] PDF (InvoiceVM invoice) {
      ArgumentNullException.ThrowIfNull (invoice);

      var document = CreateDoc (invoice);
      using var ms = new MemoryStream ();
      document.GeneratePdf (ms);

      return ms.ToArray ();
   }

   IDocument CreateDoc (InvoiceVM invoice) {
      ArgumentNullException.ThrowIfNull (invoice.SelectedBill);

      var doc = Document.Create (container => {
         container.Page (page => {
            page.Size (PageSizes.A4);
            page.Margin (30);
            page.PageColor (Colors.White);
            page.DefaultTextStyle (x => x.FontSize (10));

            page.Content ().Column (col => {
               AddHeader (col.Item (), invoice.SelectedBill);
               col.Item ().PaddingVertical (8).LineHorizontal (1);
               AddCustomer (col.Item (), invoice.SelectedBill);
               col.Item ().PaddingVertical (8);
               AddItemsTable (col.Item (), invoice.BillDetails);
               AddTotals (col.Item (), invoice.SelectedBill);
            });
         });
      });

      return doc;
   }

   void AddHeader (IContainer container, BillHeader bill) {
      ArgumentNullException.ThrowIfNull (bill);

      container.Row (row => {
         row.RelativeItem ().Column (c => {
            c.Item ().Text ("INVOICE").FontSize (20).SemiBold ();
            c.Item ().Text ($"No: {bill.BillNumber}").FontSize (11);
            c.Item ().Text ($"Date: {bill.CreatedDate?.ToString ()}");
         });
      });
   }

   void AddCustomer (IContainer container, BillHeader bill) {
      container.Row (r => {
         r.RelativeItem ().Column (c => {
            c.Item ().Text ("Bill To:").SemiBold ();
            c.Item ().Text (bill.CustomerName);
            if (!string.IsNullOrWhiteSpace (bill.CustomerPhone))
               c.Item ().Text (bill.CustomerPhone);
         });

         r.ConstantItem (180).Column (c => {
            c.Item ().Text ($"Payment: {bill.PaymentMethod}");
         });
      });
   }

   void AddItemsTable (IContainer container, IEnumerable<BillDetails> bills) {
      container.Table (table => {
         table.ColumnsDefinition (columns => {
            columns.ConstantColumn (70);
            columns.RelativeColumn (3);
            columns.ConstantColumn (40);
            columns.ConstantColumn (80);
            columns.ConstantColumn (80);
         });

         // Header
         table.Header (header => {
            header.Cell ().Element (HeadStyle).Text ("Code");
            header.Cell ().Element (HeadStyle).Text ("Description");
            header.Cell ().Element (HeadStyle).Text ("Qty").AlignCenter ();
            header.Cell ().Element (HeadStyle).Text ("Unit").AlignRight ();
            header.Cell ().Element (HeadStyle).Text ("Total").AlignRight ();
         });

         // Rows
         foreach (var it in bills) {
            table.Cell ().Element (RowStyle).Text (it.ProductCode);
            table.Cell ().Element (RowStyle).Text (it.ProductName);
            table.Cell ().Element (RowStyle).AlignCenter ().Text (it.Quantity.ToString ());
            table.Cell ().Element (RowStyle).AlignRight ().Text ($"{it.Price:0.00}");
            table.Cell ().Element (RowStyle).AlignRight ().Text ($"{it.Amount:0.00}");
         }

         static IContainer HeadStyle (IContainer c) => c.Background ("#F1F1F1").Padding (6).BorderBottom (1).BorderColor ("#DDDDDD");
         static IContainer RowStyle (IContainer c) => c.Padding (6).BorderBottom (1).BorderColor ("#EEEEEE");
      });
   }

   void AddTotals (IContainer container, BillHeader bill) {
      container.PaddingTop (10).AlignRight ().Column (col => {
         col.Item ().Text ($"Subtotal: {bill.SubTotal.ToString ()}");
         col.Item ().Text ($"Discount: {bill.Discount.ToString ()}");

         col.Item ().PaddingTop (6).Text ($"Grand Total: {bill.Total.ToString ()}")
             .FontSize (12).SemiBold ();
         col.Item ().PaddingTop (6).Text ($"Received Amount: {bill.ReceivedAmount.ToString ()}")
            .FontSize (12).SemiBold ();

      });
   }
}
