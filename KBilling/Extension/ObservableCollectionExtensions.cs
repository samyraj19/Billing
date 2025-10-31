using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KBilling.Core;

namespace KBilling.Extension {
   public static class ObservableCollectionExtensions {
      public static void AddRange<T> (this BillCollection<T> collection, IEnumerable<T> items) {
         foreach (var item in items)
            collection.Add (item);
      }

      public static void SetCollection<T> (this BillCollection<T> collection, IEnumerable<T> items) {
         collection.Clear ();
         foreach (var item in items)
            collection.Add (item);
      }
      /// <summary>
      /// Filters the collection based on a predicate and replaces its contents with the filtered results.
      /// </summary>
      public static void Filter<T> (
         this BillCollection<T> collection,
         IEnumerable<T> source,
         Func<T, bool> predicate) {
         if (collection is null || source is null) return;

         var filtered = predicate is null ? source : source.Where (predicate);
         collection.SetCollection (filtered);
      }
   }
}
