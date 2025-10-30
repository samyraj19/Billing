namespace KBilling.DataBase {
   public static class SP {
      public static class ProductsSP {
         public const string GetAll = "sp_GetAllProducts";
         public const string Insert = "sp_InsertProduct";
         public const string Update = "sp_UpdateProduct";
         public const string Delete = "sp_DeleteProduct";
      }
      public static class  BillSP {
         public const string GetBillsHeader = "sp_GetBillsHeader";
         public const string GetBillDetails = "sp_GetBillDetails";
         public const string InsertHeader = "sp_InsertBillHeader";
         public const string InsertDetails = "sp_InsertBillDetails";

      }
   }
}
