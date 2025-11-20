using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Interfaces {
   public interface IBillRepo {
      bool Insert (BillHeader? bill, IEnumerable<BillDetails>? details);
      IEnumerable<BillHeader> GetAllBills (string? sdata, string? edate);
      IEnumerable<BillDetails> GetAllBillDetailsById (long id);
   }
}
