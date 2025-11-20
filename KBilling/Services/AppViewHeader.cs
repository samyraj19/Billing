using System.Collections.Generic;

namespace KBilling.Services {
   public static class AppViewHeader {
      static readonly Dictionary<string, string> headers = new () {
         { "DashBoard", "📊 Dashboard" },
         { "AddProduct", "➕ Add New Item" },
         { "PriceUpdateView", "💲 Update Price" },
         { "StocksView", "📦 Update Stocks" },
         { "BillingView", "💳 Bill Entry" },
         { "CategoryView","➕ Add New Category" },
         { "InvoiceView", "🧾 Invoices" }
      };

      public static string Get (string viewName) => headers.TryGetValue (viewName, out var header) ? header : viewName; // fallback if not found
   }
}
