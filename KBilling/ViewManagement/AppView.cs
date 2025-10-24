namespace KBilling.ViewManagement {
   public static class AppView {
      public static void RegAll () {
         var reg = ViewRegistry.Instance;
         reg.Register ("MainView", new MainView ());
         reg.Register ("DashBoard", new Dashboard ());
         reg.Register ("AddProduct", new AddProducts ());
         reg.Register ("PriceUpdateView", new PriceUpdateView ());
         reg.Register ("StocksView", new StocksView ());

         reg.Register ("BillingView", new BillingView ());

         var windowRegistry = WindowRegistry.Instance;
         windowRegistry.Register<MainWindow> ("MainWindow");
         windowRegistry.Register<DiscountDialog> ("DiscountDialog");
         windowRegistry.Register<ProductLookupDialog> ("ProductLookupDialog"); 
      }
   }
}
