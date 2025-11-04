using System;
using System.Data;
using KBilling.Interfaces;
using KBilling.Model;
using Microsoft.Data.SqlClient;

namespace KBilling.Services {
   public class SalesRepo : ISalesRepo {
      public DashboardModel GetSalesReport (EReportType type, DashboardModel sales) {
         var parameters = new[]{
            new SqlParameter("@FilterType", SqlDbType.VarChar, 20) { Value = type.ToString() }
         };
         DataTable dt = App.Repo.QueryExe.QuerySP ("sp_GetSalesReport", parameters);

         if (dt.Rows.Count > 0) {
            sales.TotalItems = Convert.ToString (dt.Rows[0]["TotalProducts"]);
            sales.TotalBills = Convert.ToString (dt.Rows[0]["TotalInvoices"]);
            sales.SalesAmount = Convert.ToString (dt.Rows[0]["TotalSales"]);
            sales.TotalProfit = Convert.ToString (dt.Rows[0]["TotalProfit"]);
         }
         return sales;
      }
   }
}
