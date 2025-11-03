using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Interfaces {
   public interface IBillRepo {
      bool Insert (BillHeader? bill,IEnumerable<BillDetails> details);
   }
}
