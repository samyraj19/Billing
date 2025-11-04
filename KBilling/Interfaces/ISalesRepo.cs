using KBilling.Model;

namespace KBilling.Interfaces {
   public interface ISalesRepo {
      DashboardModel GetSalesReport (EReportType type,DashboardModel sales);
   }
}
