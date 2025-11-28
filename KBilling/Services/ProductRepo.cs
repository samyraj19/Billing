using System.Collections.Generic;
using KBilling.Interfaces;
using KBilling.Model;
using KBilling.DataBase;
using System.Data;
using static KBilling.DataBase.SP;
using System.Linq;
using System;
using Microsoft.Data.SqlClient;

namespace KBilling.Services {
   public class ProductRepo : IProductRepo {
      public IEnumerable<Product> GetAll () {
         DataTable dt = App.Repo.QueryExe.QuerySP (SP.Products.GetAll);
         if (dt.Rows.Count == 0) return Enumerable.Empty<Product> (); // safe, no nulls
         var products = new List<Product> ();
         foreach (DataRow row in dt.Rows) {
            var product = new Product {
               ProductId = (int)row["ProductId"],
               ProductNumber = (string)row["ProductCode"],
               ProductName = (string)row["ProductName"],
               PurchaseRate = (decimal)row["PurchasePrice"],
               SellingRate = (decimal)row["SellingPrice"],
               Quantity = (int)row["QuantityInStock"],
               Createdby = (string)row["CreatedBy"],
               Createddate = ((DateTime)row["CreatedDate"]).ToString (),
            };
            products.Add (product);
         }
         return products;
      }

      public bool Insert (Product p) {
         ArgumentNullException.ThrowIfNull (p);
         var parameters = new[]
         {
              new SqlParameter("@ProductCode", SqlDbType.NVarChar,100) { Value = p.ProductNumber ?? string.Empty},
              new SqlParameter("@ProductName", SqlDbType.NVarChar, 150) { Value = p.ProductName ?? string.Empty },
              new SqlParameter("@PurchasePrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.PurchaseRate ?? 0m },
              new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.SellingRate ?? 0m },
              new SqlParameter("@QuantityInStock", SqlDbType.Int) { Value = p.Quantity ?? 0 },
              new SqlParameter("@IsActive", SqlDbType.Bit) { Value = p.IsActive ?? true},
              new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 150) { Value = p.Createdby ?? string.Empty },
              new SqlParameter("@CreatedDate", SqlDbType.DateTime, 150) { Value = p.Modifieddate ?? string.Empty },
              new SqlParameter("@ModifyBy", SqlDbType.NVarChar, 150) { Value = p.Createdby ?? string.Empty },
              new SqlParameter("@ModifyDate", SqlDbType.DateTime, 150) { Value = p.Createddate ?? string.Empty },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Products.Insert, parameters);
         return true;
      }
      public bool Update (Product p) {
         ArgumentNullException.ThrowIfNull (p);
         var parameters = new[]{
              new SqlParameter("@ProductCode", SqlDbType.NVarChar,100) { Value = p.ProductNumber },
              new SqlParameter("@ProductName", SqlDbType.NVarChar, 150) { Value = p.ProductName ?? string.Empty },
              new SqlParameter("@PurchasePrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.PurchaseRate ?? 0m },
              new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.SellingRate ?? 0m },
              new SqlParameter("@QuantityInStock", SqlDbType.Int) { Value = p.Quantity ?? 0 },
              new SqlParameter("@IsActive", SqlDbType.Bit) { Value = p.IsActive ?? true},
              new SqlParameter("@ModifyBy", SqlDbType.NVarChar, 150) { Value = p.Createdby ?? string.Empty },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Products.Update, parameters);
         return true;
      }

      public bool Delete (string? code) {
         var parameters = new[]{
              new SqlParameter("@ProductCode", SqlDbType.NVarChar,100) { Value = code  },
         };
         App.Repo.QueryExe.ExecuteSP (SP.Products.Delete, parameters);
         return true;
      }

      public bool UpdatePrices (IEnumerable<Product> products) {
         ArgumentNullException.ThrowIfNull (products);
         foreach (var p in products) {
            if (p.HasRateChanges ()) {
               Update (p);
            }
         }
         return true;

         void Update (Product p) {
            var parameters = new[]{
              new SqlParameter("@ProductId", SqlDbType.Int) { Value = p.ProductId },
              new SqlParameter("@PurchasePrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.PurchaseRate ?? 0m },
              new SqlParameter("@SellingPrice", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = p.SellingRate ?? 0m },
              new SqlParameter("@ModifyBy", SqlDbType.NVarChar, 150) { Value = p.Createdby ?? string.Empty },
            };
            App.Repo.QueryExe.ExecuteSP (SP.Products.UpdatePrice, parameters);
         }
      }

      public bool UpdateQty (IEnumerable<Product> products) {
         ArgumentNullException.ThrowIfNull (products);
         foreach (var p in products) {
            if (p.HasQtyChanges ()) {
               Update (p);
            }
         }
         return true;

         void Update (Product p) {
            var parameters = new[]{
              new SqlParameter("@ProductId", SqlDbType.Int) { Value = p.ProductId },
              new SqlParameter("@Qty", SqlDbType.Int){ Value = p.Quantity ?? 0 },
              new SqlParameter("@ModifyBy", SqlDbType.NVarChar, 150) { Value = p.Createdby ?? string.Empty },
            };
            App.Repo.QueryExe.ExecuteSP (SP.Products.UpdateStock, parameters);
         }
      }
   }
}
