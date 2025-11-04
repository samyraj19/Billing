using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KBilling.Core;

namespace KBilling.Extension {
   public static class ObservableCollectionExtensions {
      /// <summary>
      /// Adds a range of items to any ObservableCollection-based collection.
      /// </summary>
      public static void AddRange<T, TCollection> (this TCollection collection, IEnumerable<T> items)
         where TCollection : ObservableCollection<T> {
         if (collection == null || items == null) return;
         foreach (var item in items)
            collection.Add (item);
      }

      /// <summary>
      /// Clears and replaces all items in the collection.
      /// </summary>
      public static void SetCollection<T, TCollection> (this TCollection collection, IEnumerable<T> items)
         where TCollection : ObservableCollection<T> {
         if (collection == null) return;
         collection.Clear ();
         if (items == null) return;
         foreach (var item in items)
            collection.Add (item);
      }

      /// <summary>
      /// Filters a source collection and replaces the target collection with the filtered result.
      /// </summary>
      public static void Filter<T, TCollection> (
         this TCollection collection,
         IEnumerable<T> source,
         Func<T, bool> predicate)
         where TCollection : ObservableCollection<T> {
         if (collection == null || source == null) return;
         var filtered = predicate is null ? source : source.Where (predicate);
         collection.SetCollection (filtered);
      }
   }
}
