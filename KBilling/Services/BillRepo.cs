using System.Data;
using System;
using KBilling.Interfaces;
using Microsoft.Data.SqlClient;
using KBilling.Model;
using KBilling.DataBase;
using System.Collections.Generic;
using System.Linq;
using KBilling.Helper;
using static KBilling.DataBase.SP;

namespace KBilling.Services {
   public class BillRepo : IBillRepo {
      public bool Insert (BillHeader? bill, IEnumerable<BillDetails> details) {
         ArgumentNullException.ThrowIfNull (bill);

         var parameters = new[]
         {
            new SqlParameter("@CustomerName", SqlDbType.NVarChar, 200) { Value = (object?)bill.CustomerName ?? DBNull.Value },
            new SqlParameter("@CustomerPhone", SqlDbType.NVarChar, 20) { Value = (object?)bill.CustomerPhone ?? DBNull.Value },
            new SqlParameter("@SubTotal", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.SubTotal },
            new SqlParameter("@Discount", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.Discount },
            new SqlParameter("@Total", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.Total },
            new SqlParameter("@ReceivedAmount", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.ReceivedAmount },
            new SqlParameter("@PaymentMode", SqlDbType.NVarChar, 30) { Value = bill.PaymentMethod ?? string.Empty },
            new SqlParameter("@Remarks", SqlDbType.NVarChar, 200) { Value = (object?)bill.Remarks ?? DBNull.Value },
            new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 100) { Value = bill.CreatedBy ?? string.Empty },
            new SqlParameter("@CreatedDate", SqlDbType.DateTime, 100) { Value = bill.CreatedDate ?? string.Empty },

            // OUTPUT parameters
            new SqlParameter("@OutInvoiceID", SqlDbType.BigInt) { Direction = ParameterDirection.Output },
            new SqlParameter("@OutInvoiceNumber", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output }
         };

         App.Repo.QueryExe.ExecuteSP (SP.Bills.InsertHeader, parameters);

         // Retrieve output values
         bill.BillId = Convert.ToInt64 (parameters[10].Value);
         bill.BillNumber = Convert.ToString (parameters[11].Value);
         foreach (var detail in details) {
            detail.BillId = bill.BillId;
            detail.BillNo = bill.BillNumber;
            Insert (detail);
            DetectQuantity (detail);
         }
         return true;
      }

      void Insert (BillDetails? bill) {
         ArgumentNullException.ThrowIfNull (bill);

         var parameters = new[]
         {
             new SqlParameter("@BillId", SqlDbType.BigInt) { Value = bill.BillId },
             new SqlParameter("@BillNumber", SqlDbType.NVarChar,100) { Value = bill.BillNo },
             new SqlParameter("@ProductCode", SqlDbType.NVarChar,100) { Value = bill.ProductCode },
             new SqlParameter("@ProductName", SqlDbType.NVarChar, 200) { Value = bill.ProductName },
             new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.Price },
             new SqlParameter("@Quantity", SqlDbType.Int) { Value = bill.Quantity },
         };

         App.Repo.QueryExe.ExecuteSP (SP.Bills.InsertDetails, parameters);
      }

      void DetectQuantity (BillDetails? bill) {
         ArgumentNullException.ThrowIfNull (bill);

         var parameters = new[]
         {
             new SqlParameter("@Code",SqlDbType.NVarChar,100) { Value = bill.ProductCode },
             new SqlParameter("@SoldQty", SqlDbType.Int) { Value = bill.Quantity },
         };

         App.Repo.QueryExe.ExecuteSP (SP.Bills.DetectQuantity, parameters);
      }


      public IEnumerable<BillHeader> GetAllBills (string sdata, string edate) {
         var parameters = SqlParamHelper.PList (("@FromDate", sdata), ("@ToDate", edate));

         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Bills.GetBillsHeader, parameters);
         if (dt.Rows.Count == 0) return Enumerable.Empty<BillHeader> (); // safe, no nulls
         var bills = new List<BillHeader> ();
         foreach (DataRow row in dt.Rows) {
            var bill = new BillHeader {
               BillNumber = Convert.ToString (row["InvoiceNumber"]),
               BillId = Convert.ToInt64 (row["InvoiceID"]),
               CustomerName = row["CustomerName"] as string ?? "Unknown",
               Total = Convert.ToDecimal (row["Total"]),
               ReceivedAmount = Convert.ToDecimal (row["ReceivedAmount"]),
               BalanceAmount = Convert.ToDecimal (row["BalanceAmount"]),
               PaymentMethod = row["PaymentMode"] as string ?? "N/A",
            };
            bills.Add (bill);
         }
         return bills;
      }

      public IEnumerable<BillDetails> GetAllBillDetailsById (long id) {
         var parameters = SqlParamHelper.PList (("@BillId", id));

         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Bills.GetBillDetailsByid, parameters);
         if (dt.Rows.Count == 0) return Enumerable.Empty<BillDetails> (); // safe, no nulls
         var details = new List<BillDetails> ();
         foreach (DataRow row in dt.Rows) {
            var detail = new BillDetails {
               ProductCode = row["ProductCode"] as string ?? "N/A",
               ProductName = row["ProductName"] as string ?? "N/A",
               Price = Convert.ToDecimal (row["Price"]),
               Quantity = Convert.ToInt32 (row["Quantity"]),
               DbAmount = Convert.ToDecimal (row["Amount"]),
            };
            details.Add (detail);
         }
         return details;
      }
   }
}
