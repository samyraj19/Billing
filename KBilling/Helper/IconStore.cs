using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace KBilling.Helper;
public static class IconStore {

   static IconStore () => LoadImage ();

   public static void LoadImage () {
      Register ("DashBoard", "dashboard.png", "dashboard-white.png");
      Register ("CategoryView", "category.png", "category-white.png");
      Register ("AddProduct", "product.png", "product-white.png");
      Register ("PriceUpdateView", "price1.png", "price1-white.png");
      Register ("StocksView", "stocks.png", "stocks-white.png");
      Register ("BillingView", "bill.png", "bill-white.png");
      Register ("InvoiceView", "invoice.png", "invoice-white.png");
   }

   static void Register (string key, string inactiveImage, string activeImage) {
      Inactive[key] = ImageHelper.Load ($"{AssetPath}{inactiveImage}");
      Active[key] = ImageHelper.Load ($"{AssetPath}{activeImage}");
   }

   #region Fields
   const string AssetPath = "avares://KBilling/Assets/";
   public static readonly Dictionary<string, Bitmap> Active = [];
   public static readonly Dictionary<string, Bitmap> Inactive = [];
   #endregion
}
