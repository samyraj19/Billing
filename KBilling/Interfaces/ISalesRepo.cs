using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Interfaces {
   public interface ISalesRepo {
      SalesSummary GetSalesReport (EReportType type, SalesSummary sales);
      IEnumerable<LastestTransaction> GetTransaction ();
      IEnumerable<TopSellingItems> GetTopSellings (EReportType type);
      IEnumerable<StockRepot> GetStockReports ();
   }
}
