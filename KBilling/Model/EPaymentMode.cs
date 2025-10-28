namespace KBilling.Model {
   public enum EPaymentMode {
      None,
      Cash,
      Online,
   }

   public static class PaymentModeExtensions {
      public static string Get (this EPaymentMode mode) {
         return mode switch {
            EPaymentMode.Cash => "Cash",
            EPaymentMode.Online => "Online",
            _ => "Unknown"
         };
      }
   }
}
