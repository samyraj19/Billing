using System.Collections.ObjectModel;
using System.Linq;

namespace KBilling.Helper {
   public static class AlphaList {
      public static ObservableCollection<string> Get (bool includeAll = true) {
         if (!includeAll) return mAlphabets;
         return new ObservableCollection<string> (new[] { "All" }.Concat (mAlphabets));
      }

      static readonly ObservableCollection<string> mAlphabets = new (Enumerable.Range ('A', 26).Select (c => ((char)c).ToString ()));
   }
}
