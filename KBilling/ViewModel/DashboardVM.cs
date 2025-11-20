using CommunityToolkit.Mvvm.ComponentModel;
using KBilling.Model;

namespace KBilling.ViewModel {
   public partial class DashboardVM : BaseModel {
      public DashboardVM () {
         Init ();
      }

      void Init () {
         Sales = new ();
         Transaction = new ();
         TopSelling = new ();
         StockReport = new ();
      }

      public void LoadData () {
         Sales?.LoadReport (RType);
         Transaction?.LoadTransaction ();
         TopSelling?.LoadTopSellingItems (RType);
         StockReport?.LoadStockRpt ();
      }

      public EReportType RType { get; set; } = EReportType.Today;

      [ObservableProperty] SalesVM? sales;
      [ObservableProperty] TransactionVM? transaction;
      [ObservableProperty] TopSellingItemsVM? topSelling;
      [ObservableProperty] StockReportVM? stockReport;
   }
}
