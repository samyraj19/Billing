using System.Data;
using System;
using KBilling.Interfaces;
using Microsoft.Data.SqlClient;
using KBilling.Model;
using KBilling.DataBase;
using System.Collections.Generic;

namespace KBilling.Services {
   public class BillRepo : IBillRepo {
      public bool Insert (BillHeader? bill,IEnumerable<BillDetails> details) {
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
            new SqlParameter("@OutInvoiceNumber", SqlDbType.NVarChar, 30) { Direction = ParameterDirection.Output }
         };

         App.Repo.QueryExe.ExecuteSP ("sp_InsertBillHeader", parameters);

         // Retrieve output values
         bill.BillId = Convert.ToInt64 (parameters[10].Value);
         bill.BillNumber = Convert.ToString (parameters[11].Value);
         foreach(var detail in details) {
            detail.BillId = bill.BillId;
            detail.BillNo = bill.BillNumber;
            Insert (detail);
         }
         return true;
      }

      bool Insert (BillDetails? bill) {
         ArgumentNullException.ThrowIfNull (bill);

         var parameters = new[]
         {
             new SqlParameter("@BillId", SqlDbType.BigInt) { Value = bill.BillId },
             new SqlParameter("@BillNumber", SqlDbType.NVarChar) { Value = bill.BillNo },
             new SqlParameter("@ProductCode", SqlDbType.Int) { Value = bill.ProductId },
             new SqlParameter("@ProductName", SqlDbType.NVarChar, 200) { Value = bill.ProductName },
             new SqlParameter("@Price", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = bill.Price },
             new SqlParameter("@Quantity", SqlDbType.Int) { Value = bill.Quantity },
         };

         App.Repo.QueryExe.ExecuteSP ("sp_InsertBillDetails", parameters);
         return true;
      }
   }
}
