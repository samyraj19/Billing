using System;
using System.Collections.Generic;
using System.Data;
using KBilling.DataBase;
using KBilling.Interfaces;
using KBilling.Model;
using Microsoft.Data.SqlClient;

namespace KBilling.Services {
   public class SalesRepo : ISalesRepo {
      public SalesSummary GetSalesReport (EReportType type, SalesSummary sales) {
         var parameters = new[]{
            new SqlParameter("@FilterType", SqlDbType.VarChar, 20) { Value = type.ToString() }
         };
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Sales.GetSalesSummary, parameters);

         if (dt.Rows.Count > 0) {
            sales.TotalItems = Convert.ToString (dt.Rows[0]["TotalProducts"]);
            sales.TotalBills = Convert.ToString (dt.Rows[0]["TotalInvoices"]);
            sales.SalesAmount = Convert.ToString (dt.Rows[0]["TotalSales"]);
            sales.TotalProfit = Convert.ToString (dt.Rows[0]["TotalProfit"]);
         }
         return sales;
      }

      public IEnumerable<StockRepot> GetStockReports () {
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Sales.GetStocksReport);
         var stocks = new List<StockRepot> ();
         if (dt.Rows.Count > 0) {
            foreach (DataRow row in dt.Rows) {
               var stock = new StockRepot ();
               stock.ItemCode = Convert.ToString (row["ProductCode"]);
               stock.ItemName = Convert.ToString (row["ProductName"]);
               stock.StockQuantity = Convert.ToString (row["QuantityInStock"]);
               stock.Status = Convert.ToString (row["StockStatus"]);
               stocks.Add (stock);
            }
         }
         return stocks;
      }

      public IEnumerable<TopSellingItems> GetTopSellings (EReportType type) {
         var parameters = new[]{
            new SqlParameter("@FilterType", SqlDbType.VarChar, 20) { Value = type.ToString() }
         };
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Sales.GetTopSellingItems, parameters);
         var items = new List<TopSellingItems> ();
         if (dt.Rows.Count > 0) {
            foreach (DataRow row in dt.Rows) {
               var item = new TopSellingItems ();
               item.ItemCode = Convert.ToString (row["ProductCode"]);
               item.ItemName = Convert.ToString (row["ProductName"]);
               item.Quantity = Convert.ToString (row["TotalQuantitySold"]);
               item.Amount = Convert.ToString (row["TotalSalesAmount"]);
               items.Add (item);
            }
         }
         return items;
      }

      public IEnumerable<LastestTransaction> GetTransaction () {
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Sales.GetLatestTransactions);
         var transactions = new List<LastestTransaction> ();
         if (dt.Rows.Count > 0) {
            foreach (DataRow row in dt.Rows) {
               var transaction = new LastestTransaction ();
               transaction.BillNumber = Convert.ToString (row["BillNumber"]);
               transaction.Date = Convert.ToString (row["CreatedOn"]);
               transaction.Amount = Convert.ToString (row["TotalAmount"]);
               transaction.ReceivedAmount = Convert.ToString (row["ReceivedAmount"]);
               transaction.BalanceAmount = Convert.ToString (row["BalanceAmount"]);
               transaction.Payment = Convert.ToString (row["PaymentMode"]);
               transactions.Add(transaction);
            }
         }
         return transactions;
      }
   }
}
