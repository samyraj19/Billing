using Avalonia.Styling;
using KBilling.DataBase;

namespace KBilling.Interfaces {
   public interface IGlobalRepo {
      IProductRepo Products { get; }
      IBillRepo Bills { get; }
      ISalesRepo Sales { get; }
      QueryExecutor QueryExe { get; } // shared 
   }
}
