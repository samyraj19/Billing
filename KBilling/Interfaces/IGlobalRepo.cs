using Avalonia.Styling;
using KBilling.DataBase;

namespace KBilling.Interfaces {
   public interface IGlobalRepo {
      IProductRepo Products { get; }
      IBillRepo Bills { get; }
      QueryExecutor QueryExe { get; } // shared 
   }
}
