using KBilling.Model;

namespace KBilling.ViewModel {
   public class DashboardVM {
      public DashboardVM () {
         Init ();
      }

      void Init () {
         Sales = new ();
         Transaction = new ();
         TopSelling = new ();
         StockReport = new ();
      }

      public SalesVM? Sales { get; set; }
      public TransactionVM? Transaction { get; set; }
      public TopSellingItemsVM? TopSelling { get; set; }
      public StockReportVM? StockReport { get; set; }
   }
}
