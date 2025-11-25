namespace KBilling.Model;
public enum StockLevel {
   All, Low, Medium, High, InSufficient
}

public static class StockLevelEx {
   public static string ToText (this StockLevel level) => level switch {
      StockLevel.All => "All",
      StockLevel.Low => "Low",
      StockLevel.Medium => "Medium",
      StockLevel.High => "High",
      StockLevel.InSufficient => "Out of Stock",
      _ => "Unknown"
   };
}
