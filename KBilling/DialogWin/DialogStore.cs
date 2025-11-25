using System.Collections.Generic;
using Avalonia.Controls;

namespace KBilling.DialogWin;
public static class DialogStore {

   public static Window? Get (string key) {
      if (Dialogs.TryGetValue (key, out var win))
         return win;

      return null; // or throw exception
   }

   public static Dictionary<string, Window> Dialogs { get; } =
       new Dictionary<string, Window>
       {
            { "discount", new DiscountDialog() },
            { "itemlist", new ItemListDialog() },
            { "productlookup", new ProductLookupDialog() }
       };
}
