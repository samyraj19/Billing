using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Interfaces {
   public interface ICategoryRepo {
      bool Insert (Category category);
      IEnumerable<Category> GetAll ();
   }
}
