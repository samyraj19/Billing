namespace KBilling.Model {
   public enum StockLevel {
      Low, Medium, High, OutOfStock
   }

   public static class StockLevelEx {
      public static string ToDisplay (this StockLevel level) => level switch {
         StockLevel.Low => "Low",
         StockLevel.Medium => "Medium",
         StockLevel.High => "High",
         StockLevel.OutOfStock => "Out of Stock",
         _ => "Unknown"
      };
   }
}
