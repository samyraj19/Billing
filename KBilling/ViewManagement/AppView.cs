namespace KBilling.ViewManagement {
   public static class AppView {
      public static void RegAll () {
         var reg = ViewRegistry.Instance;
         reg.Register ("Dashboard", new Dashboard ());
         reg.Register ("AddProduct", new AddProducts ());

         var windowRegistry = WindowRegistry.Instance;
         windowRegistry.Register ("MainWindow", new MainWindow ());
      }
   }
}
