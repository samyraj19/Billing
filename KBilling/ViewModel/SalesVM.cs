using KBilling.Core;
using KBilling.Extension;
using KBilling.Model;

namespace KBilling.ViewModel {
   public class SalesVM : SalesSummary {
      public SalesVM () { }

      public void LoadReport (EReportType type) {
         Repo.Sales.GetSalesReport (type, this);
      }
   }

   public class TransactionVM : LastestTransaction {
      public TransactionVM () { }
      public void LoadTransaction () {
         var trans = Repo.Sales.GetTransaction ();
         Transactions.SetCollection (trans);
      }

      public AutoNumberedCollection<LastestTransaction> Transactions { get; set; } = [];
   }

   public class TopSellingItemsVM : TopSellingItems {
      public TopSellingItemsVM () { }
      public void LoadTopSellingItems (EReportType type) {
         var items = Repo.Sales.GetTopSellings (type);
         Items.SetCollection (items);
      }
      public AutoNumberedCollection<TopSellingItems> Items { get; set; } = [];
   }

   public class StockReportVM : StockRepot {
      public StockReportVM () { }

      public void LoadStockRpt () {
         var reports = Repo.Sales.GetStockReports ();
         StockReports.SetCollection (reports);
      }

      public AutoNumberedCollection<StockRepot> StockReports { get; set; } = [];
   }
}
