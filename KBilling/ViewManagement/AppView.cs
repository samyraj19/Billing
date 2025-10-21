namespace KBilling.ViewManagement {
   public static class AppView {
      public static void RegAll () {
         var reg = ViewRegistry.Instance;
         reg.Register ("MainView", new MainView ());
         reg.Register ("DashBoard", new Dashboard ());
         reg.Register ("AddProduct", new AddProducts ());

         var windowRegistry = WindowRegistry.Instance;
         windowRegistry.Register ("MainWindow", new MainWindow ());
      }
   }
}
