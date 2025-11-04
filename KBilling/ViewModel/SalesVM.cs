using KBilling.Model;

namespace KBilling.ViewModel {
   public class SalesVM : DashboardModel {
      public SalesVM () { }

      public void LoadReport (EReportType type) {
         Repo.Sales.GetSalesReport (type, this);
      }
   }
}
