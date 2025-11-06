using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Interfaces {
   public interface IProductRepo {
      IEnumerable<Product> GetAll ();
      bool Insert (Product product);
      bool Update (Product product);
      bool Delete (int? productNumber);
      bool UpdatePrices (IEnumerable<Product> products);
      bool UpdateQty (IEnumerable<Product> products);
   }
}
