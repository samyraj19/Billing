using System;
using System.Collections.Generic;
using KBilling.Model;

namespace KBilling.Helper {
   public static class CollectionHelper {
      public static void ReArrange (IEnumerable<object> items) {
         if (items is null) return;

         int index = 1;
         foreach (var item in items) {
            switch (item) {
               case Product p: p.No = index++; break;
               case BillDetails b: b.No = index++; break;
               case LastestTransaction t: t.No = index++; break;
               case Category c: c.No = index++; break;
            }
         }
      }
   }
}
