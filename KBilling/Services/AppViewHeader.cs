using System.Collections.Generic;

namespace KBilling.Services {
   public static class AppViewHeader {
      static readonly Dictionary<string, string> _headers = new () {
         { "DashBoard", "📊 Dashboard" },
         { "AddProduct", "➕ Add New Item" },
         { "PriceUpdateView", "💲 Update Price" },
         { "StocksView", "📦 Stocks" },
      };

      public static string Get (string viewName) {
         return _headers.TryGetValue (viewName, out var header)
             ? header
             : viewName; // fallback if not found
      }
   }
}
