namespace KBilling.Model {
   public enum PaymentMode {
      Cash,
      Online,
   }

   public static class PaymentModeExtensions {
      public static string Get (this PaymentMode mode) {
         return mode switch {
            PaymentMode.Cash => "Cash",
            PaymentMode.Online => "Online",
            _ => "Unknown"
         };
      }
   }
}
