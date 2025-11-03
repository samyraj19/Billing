using KBilling.DataBase;
using KBilling.Interfaces;

namespace KBilling.Services {
   public class GlobalRepo : IGlobalRepo {

      public GlobalRepo () {}
 
      public IProductRepo Products => new ProductRepo ();

      public IBillRepo Bills => new BillRepo ();

      public QueryExecutor QueryExe => new QueryExecutor ();
   }
}
