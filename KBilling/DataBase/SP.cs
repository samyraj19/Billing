namespace KBilling.DataBase {
   public static class SP {
      public static class Products {
         public const string GetAll = "sp_GetAllProducts";
         public const string Insert = "sp_InsertProduct";
         public const string Update = "sp_UpdateProduct";
         public const string Delete = "sp_DeleteProduct";
         public const string UpdateStock = "sp_UpdateItemQuantity";
      }
      public static class  Bills {
         public const string GetBillsHeader = "sp_GetAllBills";
         public const string GetBillDetails = "sp_GetBillDetails";
         public const string InsertHeader = "sp_InsertBillHeader";
         public const string InsertDetails = "sp_InsertBillDetails";

      }
      public static class Sales {
         public const string GetBillsHeader = "sp_GetSalesReport";
      }
   }
}
