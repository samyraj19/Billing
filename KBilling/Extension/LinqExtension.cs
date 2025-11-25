using System;
using System.Collections.Generic;
using System.Linq;

namespace KBilling.Extension;
public static class LinqExtension {
   public static bool Exist<T> (this IEnumerable<T>? source, Func<T, bool> predicate)
        => source != null && source.Any (predicate);

   public static bool NotExists<T> (this IEnumerable<T> source, Func<T, bool> predicate)
        => !source.Any (predicate);
}
